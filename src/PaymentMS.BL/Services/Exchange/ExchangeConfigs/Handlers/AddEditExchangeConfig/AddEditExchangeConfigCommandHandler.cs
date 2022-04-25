using System.Threading.Tasks;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.Commands.RefreshAllProvidersTodayRates;
using PaymentMS.BL.Services.Exchange.ExchangeProviders.Handlers.RefreshAllProvidersTodayRates;
using PaymentMS.DAL.Entities;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.AddEditExchangeConfig
{
    public class AddEditExchangeConfigCommandHandler 
        : CommandHandler<AddEditExchangeConfigCommand, CallResult>
    {
        private RefreshTodayRatesForProviderCommandHandler RefreshRatesHandler { get; }

        public AddEditExchangeConfigCommandHandler(
            Storage storage, 
            RefreshTodayRatesForProviderCommandHandler refreshRatesHandler,
            LogService logger) : base(storage, logger)
        {
            RefreshRatesHandler = refreshRatesHandler;
        }

        protected override async Task<CallResult> HandleCommandAsync(AddEditExchangeConfigCommand command)
        {
            var validationResult = GetValidationResult(command);
            if (validationResult.IsSuccess == false)
            {
                return validationResult;
            }

            if (command.CustomRate.HasValue)
            {
                command.CustomRate = command.CustomRate.Value.ToRoundedRate();
            }

            var entity = await Storage.CurrencyExchangeConfis.GetAsync(
                e => 
                    e.FromCurrencyCode == command.FromCurrency.ToString() &&
                    e.ToCurrencyCode == command.ToCurrency.ToString() &&
                    e.OrganizationId == command.OrganizationId
            );

            if (entity == null)
            {
                Storage.CurrencyExchangeConfis.Add(new CurrencyExchangeConfigEntity
                {
                    OrganizationId = command.OrganizationId,
                    FromCurrencyCode = command.FromCurrency.ToString(),
                    ToCurrencyCode = command.ToCurrency.ToString(),
                    Surcharge = command.SurchargeAsPercent,
                    ExchangeProvider = command.RateSourceProvider.Value,
                    CustomRate = command.CustomRate,
                });
            }
            else
            {
                entity.Surcharge = command.SurchargeAsPercent;
                entity.ExchangeProvider = command.RateSourceProvider.Value;
                entity.CustomRate = command.CustomRate;
                Storage.CurrencyExchangeConfis.Update(entity);
            }

            await Storage.SaveChangesAsync();

            if (command.RateSourceProvider.IsCustom())
            {
                var refreshResult = await RefreshRatesHandler.HandleAsync(new RefreshTodayRatesForProviderCommand
                {
                    RefreshForProvider = command.RateSourceProvider
                });
                if (!refreshResult.IsSuccess)
                {
                    return refreshResult;
                }
            }

            return SuccessResult();
        }

        private CallResult GetValidationResult(AddEditExchangeConfigCommand command)
        {
            if (command.RateSourceProvider.IsCustom() == false)
            {
                return SuccessResult();
            }

            if (command.CustomRate == null)
            {
                return ValidationFailResult(
                    nameof(command.CustomRate),
                    $"{nameof(command.CustomRate)} is required if {nameof(command.RateSourceProvider)} is Custom.");
            }

            if (command.CustomRate.Value <= 0)
            {
                return ValidationFailResult(
                    nameof(command.CustomRate),
                    $"{nameof(command.CustomRate)} must has positive value.");
            }

            return SuccessResult();
        }
    }
}