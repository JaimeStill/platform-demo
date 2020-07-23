using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Core;
using PlatformDemo.Core.Extensions;
using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data.Extensions
{
    public static class NotificationExtensions
    {
        #region Notifications

        public static async Task<int> GetNotificationCount(this AppDbContext db)
        {
            var count = await db.Notifications
                .Where(x =>
                    !x.IsDeleted &&
                    !x.IsRead
                )
                .Select(x => x.Id)
                .CountAsync();

            return count;
        }

        public static async Task<List<Notification>> GetUnreadNotifications(this AppDbContext db)
        {
            var notifications = await db.Notifications
                .Where(x =>
                    !x.IsDeleted &&
                    !x.IsRead
                )
                .OrderByDescending(x => x.PushDate)
                .ToListAsync();

            return notifications;
        }

        public static async Task<List<Notification>> GetReadNotifications(this AppDbContext db)
        {
            var notifications = await db.Notifications
                .Where(x =>
                    !x.IsDeleted &&
                    x.IsRead
                )
                .OrderByDescending(x => x.PushDate)
                .ToListAsync();

            return notifications;
        }

        public static async Task<List<Notification>> GetDeletedNotifications(this AppDbContext db)
        {
            var notifications = await db.Notifications
                .Where(x => x.IsDeleted)
                .OrderByDescending(x => x.PushDate)
                .ToListAsync();

            return notifications;
        }

        public static async Task AddNotification(this AppDbContext db, Notification notification)
        {
            if (notification.Validate())
            {
                notification.PushDate = DateTime.UtcNow.ToGMTString();
                await db.Notifications.AddAsync(notification);
                await db.SaveChangesAsync();
            }
        }

        public static async Task ToggleNotificationRead(this AppDbContext db, Notification notification)
        {
            db.Notifications.Attach(notification);
            notification.IsRead = !notification.IsRead;
            await db.SaveChangesAsync();
        }

        public static async Task ToggleNotificationDeleted(this AppDbContext db, Notification notification)
        {
            db.Notifications.Attach(notification);
            notification.IsDeleted = !notification.IsDeleted;
            await db.SaveChangesAsync();
        }

        public static async Task RemoveNotification(this AppDbContext db, Notification notification)
        {
            db.Notifications.Remove(notification);
            await db.SaveChangesAsync();
        }

        public static bool Validate(this Notification notification)
        {
            if (string.IsNullOrEmpty(notification.Message))
            {
                throw new AppException("The provided notification does not have a message", ExceptionType.Validation);
            }

            return true;
        }

        #endregion

        #region Worker Service Methods

        public static async Task GenerateAlertNotification(this AppDbContext db, Alert alert)
        {
            var notification = new Notification
            {
                Message = alert.Message,
                Url = alert.Url,
                PushDate = DateTime.UtcNow.ToGMTString(),
                IsAlert = true
            };

            await db.Notifications.AddAsync(notification);
            await db.SaveChangesAsync();
        }

        #endregion
    }
}