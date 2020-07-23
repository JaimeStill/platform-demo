using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data.Extensions
{
    public static class AuditExtensions
    {
        public static async Task<List<Audit>> GetAudits(this AppDbContext db) => await db.Audits
            .OrderByDescending(x => x.AuditDate)
            .ToListAsync();

        public static async Task<List<Audit>> GetAuditsByType(this AppDbContext db, string entityType) => await db.Audits
            .Where(x => x.EntityType.ToLower() == entityType.ToLower())
            .OrderByDescending(x => x.AuditDate)
            .ToListAsync();

        public static async Task<List<Audit>> GetEntityAudit(this AppDbContext db, string entityType, int key) =>
            await db.Audits
                .Where(x =>
                    x.EntityType.ToLower() == entityType.ToLower() &&
                    x.Key == key
                )
                .OrderByDescending(x => x.AuditDate)
                .ToListAsync();
    }
}