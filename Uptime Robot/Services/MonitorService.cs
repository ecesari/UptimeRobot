using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Uptime_Robot.Data;
using Uptime_Robot.Models;

namespace Uptime_Robot.Services
{
    public interface IMonitorService
    {
	    Task<IEnumerable<Monitor>> GetAllMonitors(IdentityUser userId);
	    Task<IEnumerable<Monitor>> GetAllMonitors();
	    IEnumerable<Monitor> GetEnabledWebsites();
        Task AddLog(int monitorId, bool isUp);
        Task<Monitor> GetMonitor(int id);
        //Task<IEnumerable<UptimeRecord>> GetUptimeRecordsByUrl(string url);
        //Task<IEnumerable<UptimeRecord>> GetAllUptimeRecords();
        Task DeleteWebsiteById(int id);
        Task AddMonitor(Monitor monitor, ApplicationUser identityUser);
        Task UpdateMonitor(Monitor id, int id1);
    }

    public class MonitorService : IMonitorService
    {
        private readonly ApplicationDbContext _context;

        public MonitorService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Monitor>> GetAllMonitors(IdentityUser user)
        {
	        var monitors = await _context.Monitors.Where(x => x.Owner == user).ToListAsync();
            return monitors;
        }

        public async Task<IEnumerable<Monitor>> GetAllMonitors()
        {
	        var monitors = await _context.Monitors.AsNoTracking().ToListAsync();
	        return monitors;
        }

        public IEnumerable<Monitor> GetEnabledWebsites()
        {
            throw new NotImplementedException();
        }

        public async Task AddLog(int monitorId, bool isUp)
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
        

        public async Task<Monitor> GetMonitor(int id)
        {
	        var monitor = await  _context.Monitors.FirstOrDefaultAsync(m => m.Id == id);
	        return monitor;
        }

        public async Task DeleteWebsiteById(int id)
        {
	        var monitor = await _context.Monitors.FindAsync(id);
	        _context.Monitors.Remove(monitor);
	        await _context.SaveChangesAsync();
        }

        public async Task AddMonitor(Monitor monitor, ApplicationUser user)
        {
            //TODO:ownerId başka şekilde eklenir mi?
	        //monitor.OwnerId = userId;
	        monitor.Owner = user;
	        _context.Add(monitor);
	        await _context.SaveChangesAsync();
        }

        public async Task UpdateMonitor(Monitor monitor, int id)
        {
	        try
	        {
		        _context.Update(monitor);
		        await _context.SaveChangesAsync();
            }
	        catch (DbUpdateConcurrencyException)
	        {
		        var monitorExists= _context.Monitors.Any(e => e.Id == id);
		        
                if (!monitorExists)
		        {
			        //log
		        }
		        else
		        {
			        throw;
		        }
	        }
     
        }
    }
}
