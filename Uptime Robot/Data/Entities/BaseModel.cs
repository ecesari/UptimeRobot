using System;
using System.ComponentModel.DataAnnotations;

namespace Uptime_Robot.Data.Entities
{
	public class BaseModel
	{
		[Key]
		public Guid Id { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }
	}
}
