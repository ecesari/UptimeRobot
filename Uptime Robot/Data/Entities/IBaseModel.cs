using System;

namespace Uptime_Robot.Data.Entities
{
	public interface IBaseModel
	{
		Guid Id { get; set; }
		DateTime CreateDate { get; set; }
		DateTime? UpdateDate { get; set; }
	}
}
