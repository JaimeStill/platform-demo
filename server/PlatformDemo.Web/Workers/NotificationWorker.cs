using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;
using PlatformDemo.Web.Hubs;

namespace PlatformDemo.Web.Workers
{
    public class NotificationWorker : IHostedService, IDisposable
    {
        private Timer timer;
        private IHubContext<SocketHub> socket;
        private ILogger<NotificationWorker> logger;
        private IServiceProvider services;

        public NotificationWorker(
            IHubContext<SocketHub> socket,
            ILogger<NotificationWorker> logger,
            IServiceProvider services
        )
        {
            this.logger = logger;
            this.socket = socket;
            this.services = services;
        }

        public Task StartAsync(CancellationToken cancel)
        {
            logger.LogInformation("Notification Worker: Started");
            timer = new Timer(TriggerAlerts, null, TimeSpan.Zero, TimeSpan.FromSeconds(15));
            return Task.CompletedTask;
        }

        private async Task NotifyClients(Alert alert) =>
            await socket.Clients.All
                .SendAsync("receiveAlertNotification", $"alert: {alert.Message}");

        private async void TriggerAlerts(object state)
        {
            logger.LogInformation("Notification Worker: Triggering Alerts");

            using var scope = services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            await db.TriggerAlerts(async alerts =>
            {
                foreach (var alert in alerts)
                {
                    await db.GenerateAlertNotification(alert);
                    await NotifyClients(alert);
                }
            });
        }

        public Task StopAsync(CancellationToken cancel)
        {
            logger.LogInformation("Notification Worker: Stopped");
            timer.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            timer?.Dispose();
        }
    }
}