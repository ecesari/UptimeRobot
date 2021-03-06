using System;
using System.ComponentModel.DataAnnotations;

namespace Uptime_Robot.Data.Entities
{
	public abstract class BaseModel : IBaseModel
	{

		[Key]
		public Guid Id { get; set; }

		public DateTime CreateDate { get; set; }

		public DateTime? UpdateDate { get; set; }
		public bool IsActive { get; set; }
	}
}
