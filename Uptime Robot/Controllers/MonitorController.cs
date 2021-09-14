using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
			//TODO:Userid to monitorservice
			//TOOD: make monitorservice async
			var user = await _userManager.GetUserAsync(User);
			var monitors = await _monitorService.GetAllMonitors(user);
			return View(monitors);
		}

		// GET: Monitor/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			//var monitor = await _context.Monitors
			//    .FirstOrDefaultAsync(m => m.Id == id);
			var monitor = await _monitorService.GetMonitor((int)id);
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
		public async Task<IActionResult> Create([Bind("Id,Header,Url,Interval")] Monitor monitor)
		{
			//add validation
			//TODO:monitor to monitordto
			if (ModelState.IsValid)
			{
				var user = await _userManager.GetUserAsync(User);
				await _monitorService.AddMonitor(monitor, user);
				return RedirectToAction(nameof(Index));
			}
			return View(monitor);
		}

		// GET: Monitor/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var monitor = await _monitorService.GetMonitor((int)id);
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
		public async Task<IActionResult> Edit(int id, [Bind("Id,Header,Url,Interval")] Monitor monitor)
		{
			if (id != monitor.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				//try
				//{
				await _monitorService.UpdateMonitor(monitor, id);

				//_context.Update(monitor);
				//await _context.SaveChangesAsync();
				//}
				//catch (DbUpdateConcurrencyException)
				//{
				//    if (!MonitorExists(monitor.Id))
				//    {
				//        return NotFound();
				//    }
				//    else
				//    {
				//        throw;
				//    }
				//}
				return RedirectToAction(nameof(Index));
			}
			return View(monitor);
		}

		//GET: Monitor/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var monitor = await _monitorService.GetMonitor((int) id);
			if (monitor == null)
			{
				return NotFound();
			}

			return View(monitor);
		}

		// POST: Monitor/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			await _monitorService.DeleteWebsiteById(id);
			return RedirectToAction(nameof(Index));
		}


	}
}
