using System;
using System.Threading.Tasks;
using PaymentMS.DAL.Enums;
using PaymentMS.DAL.Repositories;

namespace PaymentMS.BL.Services.Exchange.ExchangeConfigs.Handlers.Commands.DeleteExchangeConfig
{
    public class DeleteExchangeConfigCommandHandler 
        : CommandHandler<DeleteExchangeConfigCommand, CallResult>
    {
        public DeleteExchangeConfigCommandHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override async Task<CallResult> HandleCommandAsync(DeleteExchangeConfigCommand command)
        {
            var entity = await Storage.CurrencyExchangeConfis.GetAsync(
                e => 
                    e.FromCurrencyCode == command.FromCurrency.ToString() &&
                    e.ToCurrencyCode == command.ToCurrency.ToString() &&
                    e.OrganizationId == command.OrganizationId
            );

            if (entity == null)
            {
                return SuccessResult();
            }

            await Storage.CurrencyExchangeConfis.DeleteAsync(entity);
            
            return SuccessResult();
        }
    }
}