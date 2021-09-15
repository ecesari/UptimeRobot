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
						bool monitorIsUp;
						bool monitoringSuccessful;

						try
						{
							_logger.LogInformation("Monitoring started", monitor.Url);

							var checkingResponse = await client.GetAsync(monitor.Url);

							monitorIsUp = checkingResponse.IsSuccessStatusCode;
							monitoringSuccessful = true;
							await dataService.AddLog(monitor.Id, monitorIsUp);

						}
						catch (Exception ex)
						{
							monitorIsUp = false;
							monitoringSuccessful = false;

							_logger.LogError($"There was an error trying to connect to the monitor. The exception message is: {ex.Message}", monitor.Url);
						}

						if (!monitorIsUp && monitoringSuccessful)
						{
							_logger.LogInformation($"{monitor.Url} owned by {monitor.OwnerEmail} is down, sending an e-mail", DateTime.Now);

							try
							{
								await emailService.SendEmailAsync(monitor.OwnerEmail, $"{monitor.Header} is down!", $"Uh oh! It seems like your monitor is down. Your monitor named {monitor.Header} at address is down. Current time is {DateTime.Now}");
							}
							catch (Exception ex)
							{
								_logger.LogError($"There was an error trying to send an e-mail. The error message is: {ex.Message}", monitor.Url);

							}
						}


					}
				}
			}
		}


		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Uptime Service is stopping.");

			_timer?.Change(Timeout.Infinite, 0);

			return Task.CompletedTask;
		}
	}
}
