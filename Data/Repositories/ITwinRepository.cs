using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public interface ITwinRepository<T>
        where T : new()
    {
        Task<IEnumerable<DeviceTwinEntity<T>>> All();
        Task<DeviceTwinEntity<T>> ById(string deviceId);
        Task Update(DeviceTwinEntity<T> entity);
    }
}