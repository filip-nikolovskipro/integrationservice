using BackgroundWorker.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Services
{
    public interface IStoreCallsService
    {
        Task StoreCalls(IEnumerable<CallData> calls);
    }
}
