using System;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public class DataPointRepository : TableRepository<DataPointEntity>
    {
        public DataPointRepository(string connectionString) : base(connectionString, "SumpPumpMonitorData") { }
    }
}
