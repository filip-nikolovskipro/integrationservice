using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker.Model
{
    public class SyncState
    {
        [BsonId]
        public int TenantId { get; set; }

        [BsonElement("LastSyncTimeUtc")]
        public DateTime LastSyncTimeUtc { get; set; }

        [BsonElement("StartSyncTimeUtc")]
        public DateTime StartSyncTimeUtc { get; set; }

        [BsonElement("SyncFinished")]
        public bool SyncFinished { get; set; }

    }
}
