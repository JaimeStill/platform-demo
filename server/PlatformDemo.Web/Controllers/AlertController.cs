using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PlatformDemo.Core.ApiQuery;
using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;

namespace PlatformDemo.Web.Controllers
{
    [Route("api/[controller]")]
    public class AlertController : Controller
    {
        private AppDbContext db;

        public AlertController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(QueryResult<Alert>), 200)]
        public async Task<IActionResult> QueryAlerts(
            [FromRoute]string role,
            [FromQuery]string page,
            [FromQuery]string pageSize,
            [FromQuery]string search,
            [FromQuery]string sort
        ) => Ok(await db.QueryAlerts(page, pageSize, search, sort));

        [HttpGet("[action]")]
        public async Task<List<Alert>> GetAlerts() => await db.GetAlerts();

        [HttpGet("[action]/{search}")]
        public async Task<List<Alert>> SearchAlerts([FromRoute]string search) => await db.SearchAlerts(search);

        [HttpGet("[action]/{id}")]
        public async Task<Alert> GetAlert([FromRoute]int id) => await db.GetAlert(id);

        [HttpPost("[action]")]
        public async Task AddAlert([FromBody]Alert alert) => await db.AddAlert(alert);

        [HttpPost("[action]")]
        public async Task UpdateAlert([FromBody]Alert alert) => await db.UpdateAlert(alert);

        [HttpPost("[action]")]
        public async Task RemoveAlert([FromBody]Alert alert) => await db.RemoveAlert(alert);
    }
}