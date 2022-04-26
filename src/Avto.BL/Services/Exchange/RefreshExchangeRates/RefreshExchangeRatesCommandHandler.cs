using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.DAL;

namespace Avto.BL.Services.Exchange.RefreshExchangeRates
{
    public class RefreshExchangeRatesCommandHandler : CommandHandler<EmptyCommand, CallResult>
    {
        public RefreshExchangeRatesCommandHandler(Storage storage, LogService logger) : base(storage, logger)
        {
        }

        protected override Task<CallResult> HandleCommandAsync(EmptyCommand command)
        {
            return Task.FromResult(SuccessResult());
        }
    }
}
