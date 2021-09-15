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

		private int _executionCount = 0;
		private readonly ILogger<UptimeService> _logger;
		private readonly IServiceProvider _serviceProvider;

		private Timer _timer;

		public UptimeService(ILogger<UptimeService> logger, IServiceProvider serviceProvider)
		{
			_logger = logger;
			_serviceProvider = serviceProvider;
		}

		
		//Start service
		public Task StartAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Uptime service has started.");
			_timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1));
			return Task.CompletedTask;
		}

		//Check all monitors according to their interval, send notifications if necessary
		private async void DoWork(object state)
		{
            Interlocked.Increment(ref _executionCount);

			using (var scope = _serviceProvider.CreateScope())
			{
				var dataService = scope.ServiceProvider.GetService<IMonitorService>();
				var emailService = scope.ServiceProvider.GetService<IEmailSender>();
				var monitors = await dataService.GetAllMonitors();
				//iterate over monitors in db
				foreach (var monitor in monitors)
				{
					//check if it is time to monitor the url
					if (_executionCount % monitor.Interval == 0)
					{
						var monitorIsUp = false;

						try
						{
							_logger.LogInformation("Monitoring started", monitor.Url);

							//send a request to the url to check if it is up
							var client = new HttpClient();
							var checkingResponse = await client.GetAsync(monitor.Url);
							monitorIsUp = checkingResponse.IsSuccessStatusCode;

						}
						catch (Exception ex)
						{
							monitorIsUp = false;

							_logger.LogError(
								$"There was an error trying to connect to the monitor. The exception message is: {ex.Message}",
								monitor.Url);
						}
						finally
						{
							//add a history log
							await dataService.AddLog(monitor.Id, monitorIsUp);
							if (!monitorIsUp)
							{
								//the monitor is down, send a notification to the owner
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
		}


		//Stop service
		public Task StopAsync(CancellationToken cancellationToken)
		{
			_logger.LogInformation("Uptime Service is stopping.");
			_timer?.Change(Timeout.Infinite, 0);
			return Task.CompletedTask;
		}
	}
}
