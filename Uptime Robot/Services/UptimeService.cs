using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Uptime_Robot.Services
{
	public class UptimeService : IHostedService
	{

		private int executionCount = 0;
		private readonly ILogger<UptimeService> _logger;
		private readonly IServiceProvider _serviceProvider;

		private Timer _timer;

		public UptimeService(ILogger<UptimeService> logger, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Hosted Service running.");


			_timer = new Timer(DoWork, null, TimeSpan.Zero,
				TimeSpan.FromMinutes(1));

			return Task.CompletedTask;
		}

		private async void DoWork(object state)
		{
			//cachele => cacheteki monitörlerin son updatedatei ve cachein updatdateini al karşılaştır

			var count = Interlocked.Increment(ref executionCount);
			using (var scope = _serviceProvider.CreateScope())
			{
				var dataService = scope.ServiceProvider.GetService<IMonitorService>();
				var emailService = scope.ServiceProvider.GetService<IEmailSender>();
				var monitors = await dataService.GetAllMonitors();
				//TODO:monitorler içerisinde aynı anda olan aynı urller iki defa kontrol edilmeyebilir
				foreach (var monitor in monitors)
				{
					if (executionCount % monitor.Interval == 0)
					{
						var client = new HttpClient();
						try
						{
							var checkingResponse = await client.GetAsync(monitor.Url);

							var monitorIsUp = checkingResponse.IsSuccessStatusCode;

							if (!monitorIsUp)
							{
								await emailService.SendEmailAsync(monitor.OwnerEmail, $"{monitor.Header} is down!", $"Uh oh! It seems like your monitor is down. Your monitor named {monitor.Header} at address is down. Current time is {DateTime.Now}");
								//send e-mail

							}
							await dataService.AddLog(monitor.Id, monitorIsUp);
						}
						catch (Exception ex)
						{
							_logger.LogError($"There was an error trying to connect to the monitor. The exception message is: {ex.Message}", monitor.Url);
						}
					}
				}
			}


			_logger.LogInformation(
				"Timed Hosted Service is working. Count: {Count}", count);
		}


		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Timed Hosted Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}

		public void Dispose()
		{
			_timer?.Dispose();
		}
	}
}
