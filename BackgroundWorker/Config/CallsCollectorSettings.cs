using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker
{
	public class CallsCollectorSettings
	{
        public string MongoConnectionString { get; set; } 
        public string MongoDatabase { get; set; } 
        public int CheckUpdateTime { get; set; }
        public string PremiseUrl { get; set; }
        public int TenantId { get; set; }
        public DateTime StartSyncTimeUtc { get; set; }
    }
}
