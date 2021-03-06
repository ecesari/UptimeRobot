using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IO;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using Uptime_Robot.Data;
using Uptime_Robot.Data.Entities;
using Uptime_Robot.Infrastructure;
using Uptime_Robot.Services;

namespace Uptime_Robot
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			#region Database and Identity Configurations
			services.AddDbContext<ApplicationDbContext>(options =>
					options.UseSqlServer(
						Configuration.GetConnectionString("DefaultConnection")), ServiceLifetime.Singleton);

			services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
				.AddEntityFrameworkStores<ApplicationDbContext>(); 
			#endregion

			services.AddControllersWithViews();
			services.AddRazorPages();

			
			#region AutoMapper Configuration
			var mapperConfig = new MapperConfiguration(mc =>
			{
				mc.AddProfile(new MapperConfig());

			});

			var mapper = mapperConfig.CreateMapper();
			services.AddSingleton(mapper);
			#endregion

			#region Identity Configurations

			services.Configure<IdentityOptions>(options =>
			  {
				  // Password settings.
				  options.Password.RequireDigit = true;
				  options.Password.RequireLowercase = true;
				  options.Password.RequireNonAlphanumeric = false;
				  options.Password.RequireUppercase = true;
				  options.Password.RequiredLength = 6;
				  options.Password.RequiredUniqueChars = 1;

				  // Lockout settings.
				  options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(20);
				  options.Lockout.MaxFailedAccessAttempts = 5;
				  options.Lockout.AllowedForNewUsers = true;

				  // User settings.
				  options.User.AllowedUserNameCharacters =
						"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				  options.User.RequireUniqueEmail = false;
			  });

			services.ConfigureApplicationCookie(options =>
			{
				// Cookie settings
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

				options.LoginPath = "/Identity/Account/Login";
				options.AccessDeniedPath = "/Identity/Account/AccessDenied";
				options.SlidingExpiration = true;
			});
			#endregion

			#region Dependency Injection
			services.AddHostedService<UptimeService>();
			services.AddTransient<IMonitorService, MonitorService>();
			services.AddTransient<IEmailSender, EmailSender>();
			services.AddTransient<UserManager<ApplicationUser>>();
			services.Configure<AuthMessageSenderOptions>(Configuration);

			#endregion
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
		{
			var path = Directory.GetCurrentDirectory();
			loggerFactory.AddFile($"{path}\\Logs\\Log.txt");

			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseDatabaseErrorPage();
			}
			else
			{
				app.UseExceptionHandler("/Home/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}
			app.UseHttpsRedirection();
			app.UseStaticFiles();

			app.UseRouting();

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}");
				endpoints.MapRazorPages();
			});
		}
	}
}
