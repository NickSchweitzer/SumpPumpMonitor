using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Configuration
{
    public class IoTHubConfiguration
    {
        public string HostName { get; set; }
        public string SharedAccessKeyName { get; set; }
        public string SharedAccessKey { get; set; }
    }
}
