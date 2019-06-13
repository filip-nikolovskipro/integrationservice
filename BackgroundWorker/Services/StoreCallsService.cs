using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BackgroundWorker.Infrastructure;
using BackgroundWorker.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace BackgroundWorker.Services
{
    public class StoreCallsService : IStoreCallsService
    {
        private readonly IntegrationServiceContext _context;
        private readonly IntegrationServiceSettings _settings;


        public StoreCallsService(IOptions<IntegrationServiceSettings> settings)
        {
            _context =  new IntegrationServiceContext(settings);
            _settings = settings.Value;
        }

        public async Task StoreCalls(IEnumerable<CallData> calls)
        {
            if (calls.Count() > 0)
            {
                var result = await _context.CallData.BulkWriteAsync(
                    calls.Select(d => new ReplaceOneModel<CallData>(Builders<CallData>.Filter.Eq(c => c.call_id, d.call_id), d) { IsUpsert = true })
                    );
            }

            await UpdateSyncInfo();
        }

        public async Task UpdateSyncInfo()
        {
            var filter = Builders<SyncState>.Filter.Eq(c => c.TenantId, _settings.TenantId);

            var updateDefinition = Builders<SyncState>.Update
                    .Set(x => x.SyncFinished, true);

            await _context.SyncState.UpdateOneAsync(filter, updateDefinition);
        }
    }
}
