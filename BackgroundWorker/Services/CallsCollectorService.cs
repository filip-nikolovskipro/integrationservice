using BackgroundWorker.Authentication;
using BackgroundWorker.Model;
using BackgroundWorker.Services;
using Dapper;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BackgroundWorker
{
	public class CallsCollectorService : BackgroundService
	{
		private readonly ILogger<CallsCollectorService> _logger;
		public IServiceProvider Services { get; }
		private readonly IntegrationServiceSettings _settings;
		private readonly IAuthentication _authentication;

        private readonly IRetriveCallsService _retriveCallsService;
        private readonly IStoreCallsService _storeCallsService;

		//private readonly HttpClient _apiClient;



		public CallsCollectorService(ILogger<CallsCollectorService> logger, 
            IOptions<IntegrationServiceSettings> settings, 
            IAuthentication authentication, 
            IRetriveCallsService retriveCallsService,
            IStoreCallsService storeCallsService)
		{
			_logger = logger ?? throw new ArgumentNullException(nameof(logger));
			_settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
			_authentication = authentication ?? throw new ArgumentNullException(nameof(authentication));
            _retriveCallsService = retriveCallsService ?? throw new ArgumentNullException(nameof(retriveCallsService));
            _storeCallsService = storeCallsService ?? throw new ArgumentNullException(nameof(storeCallsService));
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			_logger.LogDebug("CallsCollectorService is starting.");

			stoppingToken.Register(() => _logger.LogDebug("#1 CallsCollectorService background task is stopping."));

			//var token = await _authentication.Authenticate();


			while (!stoppingToken.IsCancellationRequested)
			{
				_logger.LogDebug("CallsCollectorService background task is doing background work.");

				await SyncCallsWithPremise();

				await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
			}

			_logger.LogDebug("GracePeriodManagerService background task is stopping.");

			await Task.CompletedTask;
        }

        private async Task SyncCallsWithPremise()
		{

            try
            {
                var callsJson = await _retriveCallsService.GetCalls("HsNwBOi43y");

                var calls = JsonConvert.DeserializeObject<CallsWrapper>(callsJson);
                calls.Sessions.Select(c => c).ToList().ForEach(c => c.TenantId = _settings.TenantId);

                await _storeCallsService.StoreCalls(calls.Sessions);
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
				//throw;
				Console.WriteLine("ERROR: " + msg);
			}

		}
	}
}
