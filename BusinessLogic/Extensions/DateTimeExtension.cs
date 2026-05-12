using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BusinessLogic
{
    public static class DateTimeExtension
    {
        static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static DateTime DateFromTimesTick(long timestamp)
        {
            return _unixEpoch.AddTicks(timestamp);
        }
    }
}
