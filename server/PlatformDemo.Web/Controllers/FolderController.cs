using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;

namespace PlatformDemo.Web.Controllers
{
    [Route("api/[controller]")]
    public class FolderController : Controller
    {
        private AppDbContext db;

        public FolderController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]")]
        public async Task<List<Folder>> GetRootFolders() => await db.GetRootFolders();

        [HttpGet("[action]/{parentId}")]
        public async Task<List<Folder>> GetFolderFolders([FromRoute]int parentId) => await db.GetFolderFolders(parentId);

        [HttpGet("[action]/{folderId}")]
        public async Task<Folder> GetFolder([FromRoute]int folderId) => await db.GetFolder(folderId);

        [HttpPost("[action]")]
        public async Task<bool> ValidateFolderName([FromBody]Folder folder) => await db.ValidateFolderName(folder);

        [HttpPost("[action]")]
        public async Task AddFolder([FromBody]Folder folder) => await db.AddFolder(folder);

        [HttpPost("[action]")]
        public async Task UpdateFolder([FromBody]Folder folder) => await db.UpdateFolder(folder);

        [HttpPost("[action]")]
        public async Task RemoveFolder([FromBody]Folder folder) => await db.RemoveFolder(folder);
    }
}