using BackgroundWorker.Authentication;
using BackgroundWorker.Infrastructure;
using BackgroundWorker.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Services
{
    public class RetriveCallsService : IRetriveCallsService
    {
		private readonly IAuthentication _authentication;
        private readonly IntegrationServiceSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IntegrationServiceContext _context;


        public RetriveCallsService(IOptions<IntegrationServiceSettings> settings,  IAuthentication authentication, IHttpClientFactory httpClientFactory)
        {
            _settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
            _httpClientFactory = httpClientFactory;
            _context = new IntegrationServiceContext(settings);
        }

        public async Task<string> GetCalls(string token)
        {
            Console.WriteLine(".. GetCallsFromPremise .. " + DateTime.Now);


            string result = string.Empty;
            var beginPeriod = await GetSyncStartTime();
            var endPeriod = DateTime.UtcNow;

            DynamicQueryRequest requestBody = new DynamicQueryRequest
            {
                Period = new Period
                {
                    BeginPeriod = beginPeriod,
                    EndPeriod = endPeriod,
                }
            };

            if (string.IsNullOrEmpty(_authentication.Token))
                await _authentication.Authenticate();

            var _apiClient = _httpClientFactory.CreateClient("recorder");
            _apiClient.DefaultRequestHeaders.Add("Impact360AuthToken", _authentication.Token);

            var response = await _apiClient.PostAsJsonAsync<DynamicQueryRequest>("/DASWebAPI/Query/ExecuteDynamicQuery", requestBody);

            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                AuthToken authToken = await _authentication.Authenticate();
                token = authToken.token;
                return await GetCalls(token);
            }

            response.EnsureSuccessStatusCode();

            result = await response.Content.ReadAsStringAsync();

            Console.WriteLine(".. RetrivedCallsFromPremise .. " + DateTime.Now);

            await UpdateState(requestBody);

            return result;
        }

        public async Task<DateTime> GetSyncStartTime()
        {

            var states = await _context.SyncState.FindAsync(Builders<SyncState>.Filter.Eq(x => x.TenantId, _settings.TenantId));
            SyncState state = states.FirstOrDefault();

            if (state == null)
                return _settings.StartSyncTimeUtc;

            //while (!state.SyncFinished)
            //    await GetSyncStartTime();

            return state.StartSyncTimeUtc;
        }

        public async Task UpdateState(DynamicQueryRequest requestBody)
        {
            SyncState state = new SyncState
            {
                TenantId = _settings.TenantId,
                LastSyncTimeUtc = (DateTime)requestBody.Period.BeginPeriod,
                StartSyncTimeUtc = (DateTime)requestBody.Period.EndPeriod,
                SyncFinished = false
            };


            var filter = Builders<SyncState>.Filter.Eq(c => c.TenantId, _settings.TenantId);
            var updateOptions = new UpdateOptions() { IsUpsert = true };

           await _context.SyncState.ReplaceOneAsync(filter, state, updateOptions);

        }
    }
}
