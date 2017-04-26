using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public interface ITableRepository<T>
        where T : TableEntity, new()
    {
        Task<IEnumerable<T>> All(string partitionKey = null);
        Task<IEnumerable<T>> Range(string partitionKey, DateTime startTime, DateTime? endTime);
        Task<IEnumerable<T>> Top(string partitionKey, int top);
        Task Insert(T item);
        Task Upsert(T item);
    }
}
