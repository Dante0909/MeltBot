﻿using PassionLib.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassionLib.DAL
{
    public class RunsContext : DbContext
    {
        public RunsContext(DbContextOptions<RunsContext> options) : base(options)
        {
        }

        public DbSet<Run> Runs => Set<Run>();
        public DbSet<User> Users => Set<User>();

        public DbSet<CraftEssence> CraftEssences => Set<CraftEssence>();
        public DbSet<MysticCode> MysticCodes => Set<MysticCode>();
        public DbSet<Quest> Quests => Set<Quest>();
        public DbSet<Servant> Servants => Set<Servant>();

        public DbSet<CraftEssenceAlias> CraftEssenceAliases => Set<CraftEssenceAlias>();
        public DbSet<MysticCodeAlias> MysticCodeAliases => Set<MysticCodeAlias>();
        public DbSet<QuestAlias> QuestAliases => Set<QuestAlias>();
        public DbSet<ServantAlias> ServantAliases => Set<ServantAlias>();

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            updateTimestamps();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override async Task<int> SaveChangesAsync(
            bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            updateTimestamps();
            return (await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken));
        }
        private void updateTimestamps()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is Run && (e.State == EntityState.Modified));
            var utcNow = DateTime.Now;

            foreach (var entityEntry in entries)
            {
                ((Run)entityEntry.Entity).UpdatedDate = utcNow;

                /*if (entityEntry.State == EntityState.Added)
                {
                    ((Run)entityEntry.Entity).CreatedDate = DateTime.Now;
                }*/ // should be handled in ctor now
            }
        }
    }
}