using System;
using System.ComponentModel;

namespace Uptime_Robot.Models
{
	public class MonitorLogViewModel
	{
		public int Id { get; set; }
		public Guid MonitorId { get; set; }
		[DisplayName("Status")]
		public bool IsUp { get; set; }
		public DateTime TimeStamp { get; set; }
	}
}
