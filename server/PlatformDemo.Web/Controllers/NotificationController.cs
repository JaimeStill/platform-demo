using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;

namespace PlatformDemo.Web.Controllers
{
    [Route("api/[controller]")]
    public class NotificationController : Controller
    {
        private AppDbContext db;

        public NotificationController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        public async Task<int> GetNotificationCount() =>
            await db.GetNotificationCount();

        [HttpGet("[action]")]
        public async Task<List<Notification>> GetUnreadNotifications() =>
            await db.GetUnreadNotifications();

        [HttpGet("[action]")]
        public async Task<List<Notification>> GetReadNotifications() =>
            await db.GetReadNotifications();

        public async Task<List<Notification>> GetDeletedNotifications() =>
            await db.GetDeletedNotifications();

        [HttpPost("[action]")]
        public async Task AddNotification([FromBody]Notification notification) =>
            await db.AddNotification(notification);

        [HttpPost("[action]")]
        public async Task ToggleNotificationRead([FromBody]Notification notification) =>
            await db.ToggleNotificationRead(notification);

        [HttpPost("[action]")]
        public async Task ToggleNotificationDeleted([FromBody]Notification notification) =>
            await db.ToggleNotificationDeleted(notification);

        [HttpPost("[action]")]
        public async Task RemoveNotification([FromBody]Notification notification) =>
            await db.RemoveNotification(notification);
    }
}