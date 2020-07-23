using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PlatformDemo.Core.ApiQuery;
using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;

namespace PlatformDemo.Web.Controllers
{
    [Route("api/[controller]")]
    public class ItemController : Controller
    {
        private AppDbContext db;

        public ItemController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        [ProducesResponseType(typeof(QueryResult<Item>), 200)]
        public async Task<IActionResult> QueryItems(
            [FromQuery]string page,
            [FromQuery]string pageSize,
            [FromQuery]string search,
            [FromQuery]string sort
        ) => Ok(await db.QueryItems(page, pageSize, search, sort));
    }
}