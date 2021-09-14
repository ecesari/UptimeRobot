using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace Uptime_Robot.Models
{
    public class ApplicationUser : IdentityUser
    {
        public IList<Monitor> Monitors { get; set; }
    }
}
