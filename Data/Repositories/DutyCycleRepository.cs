using System;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public class DutyCycleRepository : TableRepository<DutyCycleEntity>
    {
        public DutyCycleRepository(string connectionString) : base(connectionString, "SumpPumpMonitorDuty") { }
    }
}
