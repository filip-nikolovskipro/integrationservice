using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker
{
	public class IntegrationServiceSettings
	{
        [BsonId]
        public int TenantId { get; set; }
        public string MongoConnectionString { get; set; } 
        public string MongoDatabase { get; set; } 

        [BsonElement("PremiseUrl")]
        public string PremiseUrl { get; set; }

        [BsonElement("StartSyncTimeUtc")]
        public DateTime StartSyncTimeUtc { get; set; }

        [BsonElement("CheckUpdateTime")]
        public int CheckUpdateTime { get; set; }
    }
}
