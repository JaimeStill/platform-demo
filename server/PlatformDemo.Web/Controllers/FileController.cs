using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;

using PlatformDemo.Data;
using PlatformDemo.Data.Entities;
using PlatformDemo.Data.Extensions;

namespace PlatformDemo.Web.Controllers
{
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private AppDbContext db;

        public FileController(AppDbContext db)
        {
            this.db = db;
        }

        [HttpGet("[action]/{folderId}")]
        public async Task<List<File>> GetFolderFiles([FromRoute]int folderId) => await db.GetFolderFiles(folderId);

        [HttpGet("[action]/{fileId}")]
        public async Task<File> GetFile([FromRoute]int fileId) => await db.GetFile(fileId);

        [HttpPost("[action]")]
        public async Task<bool> ValidateFileName([FromBody]File file) => await db.ValidateFileName(file);

        [HttpPost("[action]")]
        public async Task AddFile([FromBody]File file) => await db.AddFile(file);

        [HttpPost("[action]")]
        public async Task UpdateFile([FromBody]File file) => await db.UpdateFile(file);

        [HttpPost("[action]")]
        public async Task RemoveFile([FromBody]File file) => await db.RemoveFile(file);
    }
}