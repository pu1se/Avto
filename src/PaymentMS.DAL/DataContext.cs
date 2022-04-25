using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PaymentMS.DAL.Entities;

namespace PaymentMS.DAL
{
    public sealed class DataContext : DbContext
    {
        public DbSet<OrganizationEntity> Organizations { get; set; }        
        public DbSet<SendingWayEntity> PaymentSendingWays { get; set; }
        public DbSet<ReceivingWayEntity> PaymentReceivingWays { get; set; }
        public DbSet<PaymentEntity> Payments { get; set; }
        public DbSet<BalanceProviderEntity> BalanceProviders { get; set; }
        public DbSet<BalanceClientEntity> BalanceClients { get; set; }
        public DbSet<ApiLogEntity> ApiLogs { get; set; }
        public DbSet<CurrencyEntity> Currencis { get; set; }
        public DbSet<CurrencyExchangeRateEntity> ExchangeRates { get; set; }
        public DbSet<CurrencyExchangeConfigEntity> CurrencyConfigs { get; set; }
        public DbSet<CalculatedCurrencyExchangeRateEntity> CalculatedCurrencyExchangeRates { get; set; }
        private static bool MigrationWasChecked { get; set; }
        
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
            if (!MigrationWasChecked)
            {
                Database.Migrate();
                MigrationWasChecked = true;
            }
        }

        //hack: for add-migration command
        public DataContext() : base(GetOptions(@"Server=.\; Database=PaymentMS; Initial Catalog=PaymentMS;Integrated Security=False;Trusted_Connection=True;"))
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
            modelBuilder.Entity<CurrencyExchangeRateEntity>()
                .HasIndex(e => e.ExchangeDate);
            modelBuilder.Entity<CurrencyExchangeRateEntity>()
                .HasIndex(e => e.ExchangeProvider);
            modelBuilder.Entity<CurrencyExchangeRateEntity>()
                .HasIndex(e => e.FromCurrencyCode);
            modelBuilder.Entity<CurrencyExchangeRateEntity>()
                .HasIndex(e => e.ToCurrencyCode);


            modelBuilder.Entity<CurrencyExchangeConfigEntity>()
                .HasKey(
                e => new
                {
                    e.FromCurrencyCode,
                    e.ToCurrencyCode,
                    e.OrganizationId
                });
            modelBuilder.Entity<CurrencyExchangeConfigEntity>()
                .HasIndex(e => e.OrganizationId);


            modelBuilder.Entity<CalculatedCurrencyExchangeRateEntity>()
                .HasIndex(
                    e => new
                    {
                        e.FromCurrencyCode,
                        e.ToCurrencyCode,
                        e.ExchangeDate,
                        e.OrganizationId
                    });
            modelBuilder.Entity<CalculatedCurrencyExchangeRateEntity>()
                .HasIndex(e => e.FromCurrencyCode);
            modelBuilder.Entity<CalculatedCurrencyExchangeRateEntity>()
                .HasIndex(e => e.ToCurrencyCode);
            modelBuilder.Entity<CalculatedCurrencyExchangeRateEntity>()
                .HasIndex(e => e.OrganizationId);
            modelBuilder.Entity<CalculatedCurrencyExchangeRateEntity>()
                .HasIndex(e => e.ExchangeDate).IsClustered(true);
            modelBuilder.Entity<CalculatedCurrencyExchangeRateEntity>()
                .HasKey(e => e.Id).IsClustered(false);
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

            foreach (var changedEntity in ChangeTracker.Entries())
            {
                if (changedEntity.Entity is IEntityWithTrackedDates entity)
                {
                    switch (changedEntity.State)
                    {
                        case EntityState.Added:
                            entity.CreatedDateUtc = now;
                            entity.LastUpdatedDateUtc = now;
                            break;
                        case EntityState.Modified:
                            entity.LastUpdatedDateUtc = now;
                            break;
                    }
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
