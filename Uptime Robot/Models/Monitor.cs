using System.Collections.Generic;

namespace Uptime_Robot.Models
{
    public class Monitor
    {
        public int Id { get; set; }
        public string Header { get; set; }
        public string Url { get; set; }
        public int Interval { get; set; }
        public ApplicationUser Owner { get; set; }
        public IEnumerable<MonitorLog> Logs { get; set; }
        public bool IsActive { get; set; }
    }
}
