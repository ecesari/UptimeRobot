using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Uptime_Robot.Data.Entities;
using Monitor = Uptime_Robot.Data.Entities.Monitor;

namespace Uptime_Robot.Data
{
	public class ApplicationDbContext : IdentityDbContext
	{
		private ILogger<ApplicationDbContext> _logger;
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, ILogger<ApplicationDbContext> logger)
			: base(options)
		{
			_logger = logger;
		}
		public DbSet<Monitor> Monitors { get; set; }
		public DbSet<ApplicationUser> ApplicationUsers { get; set; }
		public DbSet<MonitorLog> MonitorLogs { get; set; }

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
		{
			this.ChangeTracker.DetectChanges();
			var added = this.ChangeTracker.Entries()
				.Where(t => t.State == EntityState.Added)
				.Select(t => t.Entity)
				.ToArray();

			foreach (var entity in added)
			{
				if (entity is IBaseModel)
				{
					var track = entity as IBaseModel;
					track.CreateDate = DateTime.Now;
				}
			}

			var modified = this.ChangeTracker.Entries()
				.Where(t => t.State == EntityState.Modified)
				.Select(t => t.Entity)
				.ToArray();

			foreach (var entity in modified)
			{
				if (entity is IBaseModel)
				{
					var track = entity as IBaseModel;
					track.UpdateDate = DateTime.Now;
				}
			}
			try
			{
				return base.SaveChangesAsync(cancellationToken);

			}
			catch (Exception exception)
			{
				_logger.LogCritical("There was an error while updating the database", exception); 
				return null;
			}
		}
	}
}
