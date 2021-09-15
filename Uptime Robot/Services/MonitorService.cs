using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Uptime_Robot.Data;
using Uptime_Robot.Data.Entities;
using Uptime_Robot.Models;
namespace Uptime_Robot.Services
{
	public interface IMonitorService
	{
		Task<List<MonitorViewModel>> GetAllMonitors(string userId);
		Task<IEnumerable<MonitorViewModel>> GetAllMonitors();
		Task AddLog(Guid monitorId, bool isUp);
		Task<MonitorViewModel> GetMonitor(Guid id);
		Task DeleteWebsiteById(Guid id);
		Task AddMonitor(MonitorViewModel monitor, string userId);
		Task UpdateMonitor(MonitorViewModel id, Guid id1);
	}

	public class MonitorService : IMonitorService
	{
		private readonly ApplicationDbContext _context;
		private readonly IMapper _mapper;
		private readonly ILogger<MonitorService> _logger;


		public MonitorService(ApplicationDbContext context, IMapper mapper, ILogger<MonitorService> logger)
		{
			_context = context;
			_mapper = mapper;
			_logger = logger;
		}

		public async Task<List<MonitorViewModel>> GetAllMonitors(string userId)
		{
			var monitors = await _context.Monitors.Include(x => x.Logs).Where(x => x.OwnerId == userId && x.IsActive).ToListAsync();
			var list = monitors.Select(monitor => _mapper.Map<MonitorViewModel>(monitor)).ToList();

			return list;
		}

		public async Task<IEnumerable<MonitorViewModel>> GetAllMonitors()
		{
			var monitors = await _context.Monitors.Where(x => x.IsActive).Include(x => x.Owner).AsNoTracking().ToListAsync();
			var list = monitors.Select(monitor => _mapper.Map<MonitorViewModel>(monitor)).ToList();
			return list;
		}

		public async Task AddLog(Guid monitorId, bool isUp)
		{
			var monitorHistory = new MonitorLog
			{
				MonitorId = monitorId,
				TimeStamp = DateTime.Now,
				IsUp = isUp
			};
			_context.MonitorLogs.Add(monitorHistory);
			await _context.SaveChangesAsync();
		}


		public async Task<MonitorViewModel> GetMonitor(Guid id)
		{
			var monitor = await _context.Monitors.Include(x => x.Logs).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
			monitor.Logs = monitor.Logs.OrderByDescending(x => x.TimeStamp).Where(x =>
				x.TimeStamp > DateTime.Now.AddDays(-1) && x.TimeStamp <= DateTime.Now);
			var vm = _mapper.Map<MonitorViewModel>(monitor);
			return vm;
		}

		public async Task DeleteWebsiteById(Guid id)
		{
			var monitor = await _context.Monitors.FindAsync(id);
			monitor.IsActive = false;
			_context.Update(monitor);
			await _context.SaveChangesAsync();
		}

		public async Task AddMonitor(MonitorViewModel monitorViewModel, string userId)
		{
			var monitor = _mapper.Map<Monitor>(monitorViewModel);
			monitor.OwnerId = userId;
			monitor.IsActive = true;
			_context.Add(monitor);
			await _context.SaveChangesAsync();
		}

		public async Task UpdateMonitor(MonitorViewModel monitorViewModel, Guid id)
		{

			var monitor = await _context.Monitors.FindAsync(monitorViewModel.Id);
			monitor.Header = monitorViewModel.Header;
			monitor.Interval = monitorViewModel.Interval;
			monitor.Url = monitorViewModel.Url;
			_context.Update(monitor);
			await _context.SaveChangesAsync();

		}
	}
}
