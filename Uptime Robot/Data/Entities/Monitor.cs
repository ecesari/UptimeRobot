using System.Collections.Generic;

namespace Uptime_Robot.Data.Entities
{
    public class Monitor :BaseModel
    {
        //public int Id { get; set; }
        public string Header { get; set; }
        public string Url { get; set; }
        public int Interval { get; set; }
        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }
        public virtual IEnumerable<MonitorLog> Logs { get; set; }
    }
}
