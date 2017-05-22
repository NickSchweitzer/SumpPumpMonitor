using CodingMonkeyNet.SumpPumpMonitor.Data.Configuration;
using System;

using CodingMonkeyNet.SumpPumpMonitor.Data.Entities;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Repositories
{
    public class SettingsRepository : TwinRepository<SumpPumpSettingEntity>
    {
        public SettingsRepository(IoTHubConfiguration config)
        : base(config)
        { }
    }
}