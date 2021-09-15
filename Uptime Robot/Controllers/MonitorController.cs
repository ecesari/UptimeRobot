using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Uptime_Robot.Data.Entities;
using Uptime_Robot.Models;
using Uptime_Robot.Services;

namespace Uptime_Robot.Controllers
{
	public class MonitorController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly IMonitorService _monitorService;


		public MonitorController(UserManager<ApplicationUser> userManager, IMonitorService monitorService)
		{
			_userManager = userManager;
			_monitorService = monitorService;
		}

		// GET: Monitor
		public async Task<IActionResult> Index()
		{
			var userId = (await _userManager.GetUserAsync(User)).Id;
			var monitors = await _monitorService.GetAllMonitors(userId);
			return View(monitors);
		}

		// GET: Monitor/Details/5
		public async Task<IActionResult> Details(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var monitor = await _monitorService.GetMonitor((Guid)id);
			if (monitor == null)
			{
				return NotFound();
			}

			return View(monitor);
		}

		// GET: Monitor/Create
		public IActionResult Create()
		{
			return View();
		}

		// POST: Monitor/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Header,Url,Interval")] MonitorViewModel monitor)
		{
			//TODO:add validation
			if (ModelState.IsValid)
			{
				var userId = (await _userManager.GetUserAsync(User)).Id;
				await _monitorService.AddMonitor(monitor, userId);
				return RedirectToAction(nameof(Index));
			}
			return View(monitor);
		}

		// GET: Monitor/Edit/5
		public async Task<IActionResult> Edit(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var monitor = await _monitorService.GetMonitor((Guid) id);
			if (monitor == null)
			{
				return NotFound();
			}
			return View(monitor);
		}

		// POST: Monitor/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to, for 
		// more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(Guid id, [Bind("Id,Header,Url,Interval")] MonitorViewModel monitor)
		{
			if (id != monitor.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				await _monitorService.UpdateMonitor(monitor, id);
				return RedirectToAction(nameof(Index));
			}
			return View(monitor);
		}

		//GET: Monitor/Delete/5
		public async Task<IActionResult> Delete(Guid? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var monitor = await _monitorService.GetMonitor((Guid) id);
			if (monitor == null)
			{
				return NotFound();
			}

			return View(monitor);
		}

		// POST: Monitor/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(Guid id)
		{

			await _monitorService.DeleteMonitor(id);
			return RedirectToAction(nameof(Index));
		}


	}
}
