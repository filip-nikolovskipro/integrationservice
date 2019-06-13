using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker.Model
{
    public class CallData
    {
        [BsonId]
        public long call_id { get; set; }

        [BsonElement("TenantId")]
        public int TenantId { get; set; }

        [BsonElement("sid")]
        public int sid { get; set; }

        [BsonElement("site_id")]
        public int site_id { get; set; }

        [BsonElement("ani")]
        public string ani { get; set; }

        [BsonElement("audio_module_num")]
        public string audio_module_num { get; set; }

        [BsonElement("audio_ch_num")]
        public int audio_ch_num { get; set; }

        [BsonElement("audio_start_time1")]
        public DateTime audio_start_time1 { get; set; }

        [BsonElement("audio_start_time_gmt")]
        public DateTime audio_start_time_gmt { get; set; }

        [BsonElement("AUDIO_START_TIME")]
        public DateTime AUDIO_START_TIME { get; set; }

        [BsonElement("personal_id")]
        public int personal_id { get; set; }

        [BsonElement("direction")]
        public int direction { get; set; }

        [BsonElement("duration_seconds")]
        public int duration_seconds { get; set; }

        [BsonElement("contact_duration_seconds")]
        public int contact_duration_seconds { get; set; }

    }

    public class CallsWrapper
    {
        public IEnumerable<CallData> Sessions { get; set; }
    }
}