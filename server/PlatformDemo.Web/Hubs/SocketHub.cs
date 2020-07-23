using System.Threading.Tasks;

using Microsoft.AspNetCore.SignalR;

using PlatformDemo.Data;

namespace PlatformDemo.Web.Hubs
{
    public class SocketHub : Hub
    {
        private AppDbContext db;

        public SocketHub(AppDbContext db)
        {
            this.db = db;
        }

        public async Task triggerNotification(string message)
        {
            await Clients.All
                .SendAsync("receiveNotification", $"notification: {message}");
        }
    }
}