using System;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public class AlertRepository : TableRepository<AlertEntity>
    {
        public AlertRepository(string connectionString)
            : base(connectionString, "Alerts")
        { }
    }
}