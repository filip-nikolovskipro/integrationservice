using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker.Config
{
    public class ApplicationSettingsFactory
    {
        private readonly IntegrationServiceSettings _settings;

        public ApplicationSettingsFactory(IOptions<IntegrationServiceSettings> settings)
        {
            _settings = settings.Value;
        }

        //public static GetSettings()
        //{

        //}
    }
}
