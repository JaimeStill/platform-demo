using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Core;
using PlatformDemo.Core.ApiQuery;
using PlatformDemo.Core.Extensions;
using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data.Extensions
{
    public static class AlertExtensions
    {
        #region Alerts

        static IQueryable<Alert> Search(this IQueryable<Alert> alerts, string search)
        {
            search = search.ToLower();

            return alerts
                .Where(x =>
                    x.Message.ToLower().Contains(search) ||
                    x.Url.ToLower().Contains(search)
                );
        }

        public static async Task<QueryResult<Alert>> QueryAlerts(
            this AppDbContext db,
            string page,
            string pageSize,
            string search,
            string sort
        ) {
            var container = new QueryContainer<Alert>(
                db.Alerts,
                page, pageSize, search, sort
            );

            return await container.Query((alerts, s) => alerts.Search(s));
        }

        public static async Task<List<Alert>> GetAlerts(this AppDbContext db)
        {
            var alerts = await db.Alerts
                .OrderBy(x => x.Trigger)
                .ToListAsync();

            return alerts;
        }

        public static async Task<List<Alert>> SearchAlerts(this AppDbContext db, string search)
        {
            var alerts = await db.Alerts
                .Search(search)
                .OrderBy(x => x.Trigger)
                .ToListAsync();

            return alerts;
        }

        public static async Task<Alert> GetAlert(this AppDbContext db, int id)
        {
            var alert = await db.Alerts
                .FirstOrDefaultAsync(x => x.Id == id);

            return alert;
        }

        public static async Task AddAlert(this AppDbContext db, Alert alert)
        {
            if (alert.Validate())
            {
                await db.Alerts.AddAsync(alert);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateAlert(this AppDbContext db, Alert alert)
        {
            if (alert.Validate())
            {
                db.Alerts.Update(alert);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveAlert(this AppDbContext db, Alert alert)
        {
            db.Alerts.Remove(alert);
            await db.SaveChangesAsync();
        }

        public static bool Validate(this Alert alert)
        {
            if (alert.Trigger == null)
            {
                throw new AppException("An alert must have a trigger DateTime", ExceptionType.Validation);
            }

            if (string.IsNullOrEmpty(alert.Message))
            {
                throw new AppException("An alert must have a message", ExceptionType.Validation);
            }

            if (alert.Recurring)
            {
                if (alert.Minutes < 1 && alert.Hours < 1 && alert.Days < 1 && alert.Months < 1 && alert.Years < 1)
                {
                    throw new AppException("A recurring alert must contain an interval", ExceptionType.Validation);
                }
            }

            return true;
        }

        #endregion

        #region Worker Service Methods

        static async Task<List<Alert>> GetAlertsToSend(this AppDbContext db)
        {
            var alerts = await db.Alerts
                .OrderBy(x => x.Trigger)
                .ToListAsync();

            return alerts
                .Where(alert => DateTime.Parse(alert.Trigger) <= DateTime.Now)
                .ToList();
        }

        static async Task SendAlerts(this AppDbContext db, List<Alert> alerts)
        {
            var recurring = alerts.Where(x => x.Recurring)
                .ToList();

            var remove = alerts.Where(x => !x.Recurring);

            db.Alerts.AttachRange(recurring);

            recurring.ForEach(alert =>
                alert.Trigger = alert.Trigger.UpdateDateTime(
                    alert.Years,
                    alert.Months,
                    alert.Days,
                    alert.Hours,
                    alert.Minutes
                )
            );

            db.Alerts.RemoveRange(remove);
            await db.SaveChangesAsync();
        }

        public static async Task TriggerAlerts(this AppDbContext db, Func<List<Alert>, Task> trigger)
        {
            var alerts = await db.GetAlertsToSend();
            await trigger(alerts);
            await db.SendAlerts(alerts);
        }

        #endregion
    }
}