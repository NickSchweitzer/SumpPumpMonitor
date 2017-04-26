using System;
using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;


namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public class SumpPumpMetaRepository : TableRepository<SumpPumpMetaEntity>
    {
        public SumpPumpMetaRepository(string connectionString) : base(connectionString, "SumpPumpMonitorMeta") { }
    }
}
