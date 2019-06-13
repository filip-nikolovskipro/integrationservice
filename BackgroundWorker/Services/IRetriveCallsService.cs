using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BackgroundWorker.Services
{
    public interface IRetriveCallsService
    {
        Task<string> GetCalls(string token);
    }
}
