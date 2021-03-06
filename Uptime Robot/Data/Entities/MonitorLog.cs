using System;
using System.ComponentModel.DataAnnotations;

namespace Uptime_Robot.Data.Entities
{
    public class MonitorLog
    {
		[Key]
        public int Id { get; set; }
        public Guid MonitorId { get; set; }
        public bool IsUp { get; set; }
        public virtual Monitor Monitor { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
