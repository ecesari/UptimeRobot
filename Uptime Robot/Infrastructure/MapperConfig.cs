using System;
using System.Linq;
using AutoMapper;
using Uptime_Robot.Data.Entities;
using Uptime_Robot.Models;

namespace Uptime_Robot.Infrastructure
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			//Monitor mapper
			CreateMap<Monitor, MonitorViewModel>()
				.ForMember(dest => dest.OwnerEmail, opt => opt.MapFrom(src => src.Owner.Email))
				.ForMember(dest => dest.TotalUptimePercentageValue, opt => opt.MapFrom(src => src.Logs.Count() > 0 ? (int)Math.Round((double)(100 * src.Logs.Count(x => x.IsUp)) / src.Logs.Count()) : 0))
				.ForMember(dest => dest.Last24HrsUptimePercentageValue, opt => opt.MapFrom(src => src.Logs.Count() > 0 ? (int)Math.Round((double)(100 * src.Logs.Count(x => x.TimeStamp > DateTime.Now.AddDays(-1) && x.TimeStamp <= DateTime.Now && x.IsUp)) / src.Logs.Count(x => x.TimeStamp > DateTime.Now.AddDays(-1) && x.TimeStamp <= DateTime.Now)) : 0));
			CreateMap<MonitorViewModel, Monitor>()
				.ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true)); 

			//Monitor Log Mapper
			CreateMap<MonitorLog, MonitorLogViewModel>();
			CreateMap<MonitorLogViewModel, MonitorLog>();

		}
	}
}
