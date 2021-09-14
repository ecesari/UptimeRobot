using System;
using System.Collections.Generic;

namespace Uptime_Robot.Models
{
	public class MonitorViewModel
	{
		public Guid Id { get; set; }
		public string Header { get; set; }
		public string Url { get; set; }
		public int Interval { get; set; }
		public string OwnerEmail { get; set; }
		public IList<MonitorLogViewModel> Logs { get; set; }
    }
}
