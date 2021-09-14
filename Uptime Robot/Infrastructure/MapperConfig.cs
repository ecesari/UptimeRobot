using AutoMapper;
using Uptime_Robot.Data.Entities;
using Uptime_Robot.Models;

namespace Uptime_Robot.Infrastructure
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<Monitor, MonitorViewModel>()
				.ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.Owner.Email));
			CreateMap<MonitorViewModel, Monitor>();

			CreateMap<MonitorLog, MonitorLogViewModel>();
			CreateMap<MonitorLogViewModel, MonitorLog>();

		}
	}
}
