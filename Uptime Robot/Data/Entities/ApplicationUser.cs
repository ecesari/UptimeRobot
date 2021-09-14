using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Uptime_Robot.Models;

namespace Uptime_Robot.Data.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public virtual IList<Monitor> Monitors { get; set; }
    }
}
