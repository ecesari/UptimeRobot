using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Uptime_Robot.Models;

namespace Uptime_Robot.Controllers
{
    public class HomeController : Controller
    {
	    public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
