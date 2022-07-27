using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.System.CheckLogsForTodayErrorsAndNotifyAboutThem;
using Avto.BL.Services.System.CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNot;
using Avto.BL.Services.System.SendSystemWarningEmail;
using Avto.DAL;

namespace Avto.BL.Services.System
{
    public class SystemService : BaseService
    {
        public SystemService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallResult> SendSystemWarningEmail(string message)
        {
            return this.GetHandler<SendSystemWarningEmailCommandHandler>().HandleAsync(new SendSystemWarningEmailCommand
            {
                Message = message
            });
        }

        public Task<CallResult> CheckLogsForTodayErrorsAndNotifyAboutThem()
        {
            return this.GetHandler<CheckLogsForTodayErrorsAndNotifyAboutThemQueryHandler>().HandleAsync(EmptyQuery.Value);
        }

        public Task<CallResult> CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNot()
        {
            return this.GetHandler<CheckThatCalculatedCurrencyExchangeRatesTableWasUpdatedAndNotifyIfNotQueryHandler>().HandleAsync(EmptyQuery.Value);
        }
    }
}
