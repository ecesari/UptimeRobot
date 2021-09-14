using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Uptime_Robot.Models
{
	public class MonitorViewModel
	{
		public Guid Id { get; set; }
		[Required]
		[DisplayName("Name")]
		public string Header { get; set; }
		[Required]
		[DataType(DataType.Url)]
		public string Url { get; set; }
		[Required]
		public int Interval { get; set; }
		public string OwnerEmail { get; set; }
		[DisplayName("All Time Uptime (%)")]
		public decimal TotalUptimePercentageValue { get; set; }
		[DisplayName("Last 24 Hours Uptime (%)")] public decimal Last24HrsUptimePercentageValue { get; set; }
		public IList<MonitorLogViewModel> Logs { get; set; }
    }
}
