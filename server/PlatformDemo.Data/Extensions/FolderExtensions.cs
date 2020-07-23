using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Core;
using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data.Extensions
{
    public static class FolderExtensions
    {
        public static async Task<List<Folder>> GetRootFolders(this AppDbContext db)
        {
            var folders = await db.Folders
                .Where(x => !x.ParentId.HasValue)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return folders;
        }

        public static async Task<List<Folder>> GetFolderFolders(this AppDbContext db, int parentId)
        {
            var folders = await db.Folders
                .Where(x => x.ParentId == parentId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return folders;
        }

        public static async Task<Folder> GetFolder(this AppDbContext db, int folderId)
        {
            var folder = await db.Folders
                .FirstOrDefaultAsync(x => x.Id == folderId);

            return folder;
        }

        public static async Task<bool> ValidateFolderName(this AppDbContext db, Folder folder)
        {
            var check = await db.Folders
                .FirstOrDefaultAsync(x =>
                    x.Id != folder.Id &&
                    x.ParentId == folder.ParentId &&
                    x.Name.ToLower() == folder.Name.ToLower()
                );

            return check == null;
        }

        public static async Task AddFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                folder.Color = folder.SetColor();
                await db.Folders.AddAsync(folder);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateFolder(this AppDbContext db, Folder folder)
        {
            if (await folder.Validate(db))
            {
                folder.Color = folder.SetColor();
                db.Folders.Update(folder);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveFolder(this AppDbContext db, Folder folder)
        {
            await folder.RemoveFolderTree(db);
            await db.SaveChangesAsync();
        }

        static string SetColor(this Folder folder) => string.IsNullOrEmpty(folder.Color)
            ? "#333333"
            : folder.Color;

        static async Task RemoveFolderTree(this Folder folder, AppDbContext db)
        {
            db.Folders.Remove(folder);

            var folders = await db.Folders
                .Where(x => x.ParentId == folder.Id)
                .ToListAsync();

            foreach (var f in folders)
            {
                await f.RemoveFolderTree(db);
            }
        }

        static async Task<bool> Validate(this Folder folder, AppDbContext db)
        {
            if (string.IsNullOrEmpty(folder.Name))
            {
                throw new AppException("A folder must have a name", ExceptionType.Validation);
            }

            var check = await db.Folders
                .FirstOrDefaultAsync(x =>
                    x.Id != folder.Id &&
                    x.ParentId == folder.ParentId &&
                    x.Name.ToLower() == folder.Name.ToLower()
                );

            if (check != null)
            {
                throw new AppException("The folder name is already used in this folder", ExceptionType.Validation);
            }

            return true;
        }
    }
}