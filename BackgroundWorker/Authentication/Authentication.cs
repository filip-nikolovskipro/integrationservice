using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Formatting;

namespace BackgroundWorker.Authentication
{
	public class Authentication : IAuthentication
	{
		private readonly IntegrationServiceSettings _settings;
        private readonly IHttpClientFactory _httpClientFactory;

		public Authentication(IOptions<IntegrationServiceSettings> settings, IHttpClientFactory httpClientFactory)
		{
			_settings = settings.Value ?? throw new ArgumentNullException(nameof(settings));
            _httpClientFactory = httpClientFactory;
		}

		public string Token { get; private set; }
		public async Task<AuthToken> Authenticate()
		{
            //try
            //{
			    Login loginCredentials = new Login
			    {
				    user = "TenantAAdmin",
				    password = "verint1!",
			    };

                var _apiClient = _httpClientFactory.CreateClient("recorder");

                var loginContent = new StringContent(JsonConvert.SerializeObject(loginCredentials), System.Text.Encoding.UTF8, "application/json");
			    var response = await _apiClient.PostAsync(_settings.PremiseUrl + "wfo/rest/core-api/auth/token", loginContent);

                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsAsync<ObjectWrapper<AuthToken>>();
                Token = result.AuthToken.token;

                return result.AuthToken;
            //}
            //catch (Exception ex)
            //{
            //    var zz = ex.Message;

            //    throw;
            //}

            
        }
	}
}
