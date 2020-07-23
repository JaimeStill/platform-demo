using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;

using PlatformDemo.Core.Extensions;
using PlatformDemo.Data.Entities;

namespace PlatformDemo.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Alert> Alerts { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<File> Files { get; set; }
        public DbSet<Folder> Folders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Folder>()
                .HasOne(x => x.Parent)
                .WithMany(x => x.Folders)
                .HasForeignKey(x => x.ParentId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Model
                .GetEntityTypes()
                .ToList()
                .ForEach(x =>
                {
                    modelBuilder
                        .Entity(x.Name)
                        .ToTable(x.Name.Split('.').Last());
                });
        }

        public override int SaveChanges()
        {
            var changeState = GetChangeState();

            var result = base.SaveChanges();

            CreateAudit(changeState);

            result = base.SaveChanges();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken))
        {
            var changeState = GetChangeState();

            var result = await base.SaveChangesAsync(token);

            await CreateAuditAsync(changeState);

            result = await base.SaveChangesAsync(token);

            return result;
        }

        ChangeState GetChangeState () => new ChangeState
        {
            Added = GetEntityEntries(EntityState.Added),
            Modified = GetEntityEntries(EntityState.Modified),
            Deleted = GetEntityEntries(EntityState.Deleted)
        };

        List<EntityEntry> GetEntityEntries(EntityState state) => ChangeTracker
            .Entries()
            .Where(x =>
                !x.Metadata.Name.Contains("Audit") &&
                x.State == state
            )
            .ToList();

        void CreateAudit(ChangeState changeState)
        {
            foreach (var entry in changeState.Added)
                GenerateAudit(entry, audit => this.Audits.Add(audit), "Added");

            foreach (var entry in changeState.Modified)
                GenerateAudit(entry, audit => this.Audits.Add(audit), "Modified");

            foreach (var entry in changeState.Deleted)
                GenerateAudit(entry, audit => this.Audits.Add(audit), "Deleted");
        }

        Task CreateAuditAsync(ChangeState changeState) => Task.Run(() => CreateAudit(changeState));

        void GenerateAudit(EntityEntry entry, Action<Audit> generator, string state)
        {
            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };

            var entity = JsonConvert.SerializeObject(entry.Entity, settings);

            var audit = new Audit
            {
                Key = (int)entry.Property("Id").OriginalValue,
                State = state,
                EntityType = entry.Metadata.Name,
                Entity = entity,
                AuditDate = DateTime.UtcNow.ToGMTString()
            };

            generator(audit);
        }
    }
}