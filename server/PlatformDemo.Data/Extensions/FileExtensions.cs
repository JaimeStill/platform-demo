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
    public static class FileExtensions
    {
        public static async Task<List<File>> GetFolderFiles(this AppDbContext db, int folderId)
        {
            var files = await db.Files
                .Where(x => x.FolderId == folderId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            return files;
        }

        public static async Task<File> GetFile(this AppDbContext db, int fileId)
        {
            var file = await db.Files
                .FirstOrDefaultAsync(x => x.Id == fileId);

            return file;
        }

        public static async Task<bool> ValidateFileName(this AppDbContext db, File file)
        {
            var check = await db.Files
                .FirstOrDefaultAsync(x =>
                    x.Id != file.Id &&
                    x.FolderId == file.FolderId &&
                    x.Name.ToLower() == file.Name.ToLower()
                );

            return check == null;
        }

        public static async Task AddFile(this AppDbContext db, File file)
        {
            if (await file.Validate(db))
            {
                file.CreationTime = DateTime.Now;
                file.Length = 1024.RandomLong();
                await db.Files.AddAsync(file);
                await db.SaveChangesAsync();
            }
        }

        public static async Task UpdateFile(this AppDbContext db, File file)
        {
            if (await file.Validate(db))
            {
                db.Files.Update(file);
                await db.SaveChangesAsync();
            }
        }

        public static async Task RemoveFile(this AppDbContext db, File file)
        {
            db.Files.Remove(file);
            await db.SaveChangesAsync();
        }

        static async Task<bool> Validate(this File file, AppDbContext db)
        {
            if (string.IsNullOrEmpty(file.Name))
            {
                throw new AppException("A file must have a name", ExceptionType.Validation);
            }

            if (file.FolderId < 1)
            {
                throw new AppException("A file must be associated with a folder", ExceptionType.Validation);
            }

            var check = await db.Files
                .FirstOrDefaultAsync(x =>
                    x.Id != file.Id &&
                    x.FolderId == file.FolderId &&
                    x.Name.ToLower() == file.Name.ToLower()
                );

            if (check != null)
            {
                throw new AppException("The file name is already used in this folder", ExceptionType.Validation);
            }

            return true;
        }
    }
}