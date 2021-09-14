using System;

namespace Uptime_Robot.Models
{
    public class MonitorLog
    {
        public int Id { get; set; }
        public int MonitorId { get; set; }
        public bool IsUp { get; set; }
        public Monitor Monitor { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
