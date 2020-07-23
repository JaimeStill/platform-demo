using System.Linq;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;

using PlatformDemo.Core.ApiQuery;
using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data.Extensions
{
    public static class ItemExtensions
    {
        static IQueryable<Item> SetIncludes(this DbSet<Item> items) =>
            items.Include(x => x.Category);

        public static IQueryable<Item> Search(this IQueryable<Item> items, string search) =>
            items.Where(item =>
                item.Name.ToLower().Contains(search) ||
                item.Category.Name.ToLower().Contains(search) ||
                item.Description.ToLower().Contains(search)
            );

        public static async Task<QueryResult<Item>> QueryItems(
            this AppDbContext db,
            string page,
            string pageSize,
            string search,
            string sort
        )
        {
            var container = new QueryContainer<Item>(
                db.Items.SetIncludes(),
                page,
                pageSize,
                search,
                sort
            );

            return await container.Query((items, s) => items.Search(s));
        }
    }
}