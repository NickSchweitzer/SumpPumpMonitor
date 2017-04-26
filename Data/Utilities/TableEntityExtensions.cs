using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CodingMonkeyNet.SumpPumpMonitor.Data.Utilities
{
    public static class TableEntityExtensions
    {
        public static string ToRowKey(this DateTime dt)
        {
            return string.Format("{0:D19}", DateTime.MaxValue.Ticks - dt.Ticks);
        }

        public static DateTime ToDateTime(this string rowKey)
        {
            return new DateTime(DateTime.MaxValue.Ticks - Int64.Parse(rowKey));
        }
    }
}
