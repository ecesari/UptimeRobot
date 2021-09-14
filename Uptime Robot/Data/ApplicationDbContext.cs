using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Uptime_Robot.Data.Entities;

namespace Uptime_Robot.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Monitor> Monitors { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<MonitorLog> MonitorLogs { get; set; }

        //protected override void OnModelCreating(ModelBuilder builder)
        //{
	       // modelBuilder.Entity<YourEntity>().Property(x => x.Id).HasDefaultValueSql("NEWID()");

	       // modelBuilder.Entity<User>().HasData(
		      //  new User() { Id = Guid.NewGuid(), Email = "Mubeen@gmail.com", Name = "Mubeen", Password = "123123" },
		      //  new User() { Id = Guid.NewGuid(), Email = "Tahir@gmail.com", Name = "Tahir", Password = "321321" },
		      //  new User() { Id = Guid.NewGuid(), Email = "Cheema@gmail.com", Name = "Cheema", Password = "123321" }
	       // );
        //}
    }
}
