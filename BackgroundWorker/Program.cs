using BackgroundWorker.Authentication;
using BackgroundWorker.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Extensions.Http;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace BackgroundWorker
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Calls Collector Start!");

            var configuration = GetConfiguration();

            var host = new HostBuilder()
				 //.ConfigureLogging((hostContext, config) =>
				 //{
					// config.AddConsole();
					// config.AddDebug();
				 //})
				.ConfigureHostConfiguration(config =>
				{
					config.AddEnvironmentVariables();
				})
				.ConfigureAppConfiguration((hostContext, config) =>
				{
					config.AddJsonFile("appsettings.json", optional: true);
					config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json", optional: true);
					config.AddCommandLine(args);
				})
				.ConfigureServices((hostContext, services) =>
				{
                    services.Configure<IntegrationServiceSettings>(configuration);
                    //services.con


                    services.AddLogging();

					#region snippet1
					services.AddHostedService<CallsCollectorService>();
                    services.AddScoped<IAuthentication, Authentication.Authentication>();
                    services.AddScoped<IRetriveCallsService, RetriveCallsService>();
                    services.AddScoped<IStoreCallsService, StoreCallsService>();


                    services.AddHttpClient("recorder", client =>
                    {
                        client.BaseAddress = new Uri(configuration.GetValue<string>("PremiseUrl")); // ["PremiseUrl"]); //("http://ptev152mt.telab.local/");
                    })
                    .AddPolicyHandler(GetRetryPolicy())
                    .AddPolicyHandler(GetCircuitBreakerPolicy());
                    //.AddHttpMessageHandler;


                    #endregion


                })
				.UseConsoleLifetime()
				.Build();


			using (host)
			{
				// Start the host
				 host.Start();

				//// Monitor for new background queue work items
				//var monitorLoop = host.Services.GetRequiredService<MonitorLoop>();
				//monitorLoop.StartMonitorLoop();

				// Wait for the host to shutdown
				host.WaitForShutdown();
			}

			Console.ReadLine();
		}


        private static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddInMemoryCollection()
                .AddEnvironmentVariables();

            var config = builder.Build();

            return config;
        }

        private static IntegrationServiceSettings GetConfigurationFormDB(IConfiguration config)
        {
            throw new NotImplementedException();
        }

        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
              .HandleTransientHttpError()
              .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
              .WaitAndRetryAsync(6, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        }

        static IAsyncPolicy<HttpResponseMessage> GetCircuitBreakerPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .CircuitBreakerAsync(5, TimeSpan.FromSeconds(30));
        }
    }

}

