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
		Task DeleteMonitor(Guid id);
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

		//Get all monitors by userId
		//Returns List<MonitorViewModel>
		public async Task<List<MonitorViewModel>> GetAllMonitors(string userId)
		{
			
			var monitors = await _context.Monitors.Include(x => x.Logs).Where(x => x.OwnerId == userId && x.IsActive).ToListAsync();
			var list = monitors.Select(monitor => _mapper.Map<MonitorViewModel>(monitor)).ToList();

			return list;
		}

		//Get all monitors without any filter
		//Returns IEnumerable<MonitorViewModel>
		public async Task<IEnumerable<MonitorViewModel>> GetAllMonitors()
		{
			var monitors = await _context.Monitors.Where(x => x.IsActive).Include(x => x.Owner).AsNoTracking().ToListAsync();
			var list = monitors.Select(monitor => _mapper.Map<MonitorViewModel>(monitor)).ToList();
			return list;
		}


		//Add history log of a monitor to db
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


		//Get a single monitor by Id
		//Returns MonitorViewModel
		public async Task<MonitorViewModel> GetMonitor(Guid id)
		{
			//Filter logs to get the logs of the last 24 hours
			var monitor = await _context.Monitors.Include(x => x.Logs.Where(y =>
				y.TimeStamp > DateTime.Now.AddDays(-1) && y.TimeStamp <= DateTime.Now)).AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
			if (!MonitorExists(monitor)) return null;
			var vm = _mapper.Map<MonitorViewModel>(monitor);
			return vm;
		}



		
		//Delete a single monitor by Id
		public async Task DeleteMonitor(Guid id)
		{
			var monitor = await _context.Monitors.FindAsync(id);
			if (!MonitorExists(monitor)) return;
			monitor.IsActive = false;
			_context.Update(monitor);
			await _context.SaveChangesAsync();
		}

		//Delete a single monitor by ViewModel and userId
		public async Task AddMonitor(MonitorViewModel monitorViewModel, string userId)
		{
			var monitor = _mapper.Map<Monitor>(monitorViewModel);
			monitor.OwnerId = userId;
			monitor.IsActive = true;
			_context.Add(monitor);
			await _context.SaveChangesAsync();
		}

		//Update a single monitor by ViewModel
		public async Task UpdateMonitor(MonitorViewModel monitorViewModel, Guid id)
		{

			var monitor = await _context.Monitors.FindAsync(monitorViewModel.Id);
			if(!MonitorExists(monitor)) return;
			monitor.Header = monitorViewModel.Header;
			monitor.Interval = monitorViewModel.Interval;
			monitor.Url = monitorViewModel.Url;
			_context.Update(monitor);
			await _context.SaveChangesAsync();

		}

		//Check if monitor exists
		//If false, log an error
		private bool MonitorExists(Monitor monitor)
		{
			if (monitor != null) return true;
			_logger.LogError("No monitor has been found");
			return false;

		}
	}
}
