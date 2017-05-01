using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMonkeyNet.SumpPumpMonitor.Portal.Services
{
    public interface IIoTHubSender<T>
    {
        void SendMessage(string deviceId, T message);
    }
}
