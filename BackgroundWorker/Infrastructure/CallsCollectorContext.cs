using BackgroundWorker.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace BackgroundWorker.Infrastructure
{
    public class CallsCollectorContext
    {
        private readonly IMongoDatabase _database = null;

        public CallsCollectorContext(IOptions<CallsCollectorSettings> settings)
        {
            var client = new MongoClient(settings.Value.MongoConnectionString);

            if (client != null)
            {
                _database = client.GetDatabase(settings.Value.MongoDatabase);
                
            }
        }

        public IMongoCollection<CallData> CallData
        {
            get
            {
                return _database.GetCollection<CallData>("CallsCollectorDataModel");
            }
        }

        public IMongoCollection<SyncState> SyncState
        {
            get
            {
                return _database.GetCollection<SyncState>("SyncState");
            }
        }
    }
}
