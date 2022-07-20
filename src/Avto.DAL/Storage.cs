using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Avto.DAL.Entities;

namespace Avto.DAL
{
    public sealed class Storage : DbContext
    {
        public DbSet<LogEntity> Logs { get; set; }
        public DbSet<CurrencyEntity> Currencies { get; set; }
        public DbSet<ExchangeRateEntity> ExchangeRates { get; set; }
        private static bool MigrationWasChecked { get; set; }
        public bool UseNoTracking { get; set; }

        public Storage(DbContextOptions<Storage> options)
            : base(options)
        {
            if (!MigrationWasChecked)
            {
                Database.Migrate();
                DatabaseInitializer.SeedBaseData(this);
                MigrationWasChecked = true;
            }
        }

        //hack: for add-migration command
        public Storage() : base(GetOptions(@"Server=.\; Database=Avto; Initial Catalog=Avto;Integrated Security=False;Trusted_Connection=True;"))
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return SqlServerDbContextOptionsExtensions.UseSqlServer(new DbContextOptionsBuilder(), connectionString).Options;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            SetDefaultDecimalSize(modelBuilder);
            DeletionPolicy(modelBuilder);
            SetIndexes(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }

        private static void SetIndexes(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ExchangeRateEntity>()
                .HasIndex(e => e.ExchangeDate);
            modelBuilder.Entity<ExchangeRateEntity>()
                .HasIndex(e => e.FromCurrencyCode);
            modelBuilder.Entity<ExchangeRateEntity>()
                .HasIndex(e => e.ToCurrencyCode);
        }

        private static void SetDefaultDecimalSize(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                        .SelectMany(t => t.GetProperties())
                        .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(26, 10)");
            }
        }

        private static void DeletionPolicy(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var now = DateTime.UtcNow;

            foreach (var entity in ChangeTracker.Entries())
            {
                if (entity.Entity is IEntityWithTrackedDates entityWithTrackedDates)
                {
                    switch (entity.State)
                    {
                        case EntityState.Added:
                            entityWithTrackedDates.CreatedDateUtc = now;
                            entityWithTrackedDates.LastUpdatedDateUtc = now;
                            break;
                        case EntityState.Modified:
                            entityWithTrackedDates.LastUpdatedDateUtc = now;
                            break;
                    }
                }

                if (entity is IEntityWithGuidId entityWithGuidId)
                {
                    if (entity.State == EntityState.Added && entityWithGuidId.Id == Guid.Empty)
                    {
                        entityWithGuidId.Id = Guid.NewGuid();
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
