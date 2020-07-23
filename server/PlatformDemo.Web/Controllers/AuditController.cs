using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;

namespace PlatformDemo.Web.Controllers
{
    [Route("api/[controller]")]
    public class AuditController : Controller
    {
        private AppDbContext db;

        public AuditController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        public async Task<List<Audit>> GetAudits() => await db.GetAudits();

        [HttpGet("[action]/{entityType}")]
        public async Task<List<Audit>> GetAuditsByType([FromRoute]string entityType) => await db.GetAuditsByType(entityType);

        [HttpGet("[action]/{entityType}/{key}")]
        public async Task<List<Audit>> GetEntityAudit([FromRoute]string entityType, [FromRoute]int key) =>
            await db.GetEntityAudit(entityType, key);
    }
}