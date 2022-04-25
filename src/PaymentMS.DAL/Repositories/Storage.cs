using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using PaymentMS.DAL.CloudServices;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Repositories.RepositoryWithNoId;
using PaymentMS.DAL.Transactions;

namespace PaymentMS.DAL.Repositories
{
    public class Storage : IDisposable
    {
        private DataContext DataСontext { get; }
        private IKeyVaultStorage KeyVaultStorage { get; }
        public bool UseNoTracking { get; set; }

        public Storage(DataContext context, IKeyVaultStorage keyVaultStorage)
        {
            DataСontext = context ?? throw new ArgumentNullException(nameof(context));
            KeyVaultStorage = keyVaultStorage ?? throw new ArgumentNullException(nameof(keyVaultStorage));
            UseNoTracking = true;
        }


        public IGuidIdCommandRepository<OrganizationEntity> Organizations => new GuidIdCommandRepository<OrganizationEntity>(DataСontext, UseNoTracking);

        public IGuidIdCommandRepository<PaymentEntity> Payments => new GuidIdCommandRepository<PaymentEntity>(DataСontext, UseNoTracking);

        public IGuidIdCommandRepository<SendingWayEntity> PaymentSendingWays => new GuidIdCommandRepository<SendingWayEntity>(DataСontext, UseNoTracking);

        public IGuidIdCommandRepository<ReceivingWayEntity> StripeReceivingWays => new StripeReceivingWayCommandRepository(DataСontext, KeyVaultStorage, UseNoTracking);

        public IGuidIdCommandRepository<BalanceProviderEntity> BalanceProviders => new GuidIdCommandRepository<BalanceProviderEntity>(DataСontext, UseNoTracking);

        public IGuidIdCommandRepository<BalanceClientEntity> BalanceClients => new GuidIdCommandRepository<BalanceClientEntity>(DataСontext, UseNoTracking);

        public IGuidIdCommandRepository<ApiLogEntity> ApiLogs => new GuidIdCommandRepository<ApiLogEntity>(DataСontext, false);

        public ISimpleCommandRepository<CurrencyEntity> Currencies => new SimpleCommandRepository<CurrencyEntity>(DataСontext, false);

        public ISimpleCommandRepository<CurrencyExchangeRateEntity> CurrencyExchangeRates => new SimpleCommandRepository<CurrencyExchangeRateEntity>(DataСontext, false);

        public ISimpleCommandRepository<CurrencyExchangeConfigEntity> CurrencyExchangeConfis => new SimpleCommandRepository<CurrencyExchangeConfigEntity>(DataСontext, false);

        public ISimpleCommandRepository<CalculatedCurrencyExchangeRateEntity> CalculatedCurrencyExchangeRates => new SimpleCommandRepository<CalculatedCurrencyExchangeRateEntity>(DataСontext, false);

        public Task SaveChangesAsync()
        {
            return DataСontext.SaveChangesAsync();
        }

        public async Task<ITransaction> BeginTransactionAsync()
        {
            var dbTrasaction = await DataСontext.Database.BeginTransactionAsync();
            return new Transaction(dbTrasaction);
        }


        public void Dispose()
        {
            ReleaseResources();
            GC.SuppressFinalize(this);
        }

        ~Storage()
        {
            ReleaseResources();
        }

        private bool _resourcesWasRelease = false;
        private void ReleaseResources()
        {
            if (_resourcesWasRelease)
                return;

            DataСontext.Dispose();
            
            _resourcesWasRelease = true;
        }
    }
}
