using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.ExtractDataFromCsvFileCollection;
using Avto.BL.Services.Exchange.GetAllExchangeRatesForToday;
using Avto.BL.Services.Exchange.GetAvailableCurrencies;
using Avto.BL.Services.Exchange.GetExchangeRates;
using Avto.BL.Services.Exchange.ImportDataFromExchangeRatesCsvFolder;
using Avto.BL.Services.Exchange.RefreshExchangeRates;
using Avto.DAL;
using Avto.DAL.Enums;

namespace Avto.BL.Services.Exchange
{
    public class ExchangeService : BaseService
    {
        public ExchangeService(Storage storage, IServiceProvider services) : base(storage, services)
        {
        }

        public Task<CallResult> ExtractDataFromCsvFileCollection()
        {
            return GetHandler<ExtractDataFromCsvFileCollectionCommandHandler>().HandleAsync(EmptyCommand.Value);
        }

        public Task<CallListResult<AvailableCurrencyResponse>> GetAvailableCurrencies()
        {
            return GetHandler<GetAvailableCurrenciesQueryHandler>().HandleAsync(EmptyQuery.Value);
        }

        public Task<CallResult> RefreshExchangeRates()
        {
            return GetHandler<RefreshExchangeRatesCommandHandler>().HandleAsync(EmptyCommand.Value);
        }

        public Task<CallListResult<GetSpecificExchangeRatesInfoQueryResponse>> GetSpecificExchangeRatesInfo(
            GetSpecificExchangeRatesInfoQuery infoQuery)
        {
            return GetHandler<GetSpecificExchangeRatesInfoQueryHandler>().HandleAsync(infoQuery);
        }

        public Task<CallListResult<GetAllExchangeRatesForTodayQueryResponse>> GetAllExchangeRatesForToday()
        {
            return GetHandler<GetAllExchangeRatesForTodayQueryHandler>().HandleAsync(EmptyQuery.Value);
        }

        public Task<CallResult> ImportDataFromExchangeRatesCsvFolder()
        {
            return GetHandler<ImportDataFromExchangeRatesCsvFolderCommandHandler>().HandleAsync(EmptyCommand.Value);
        }

        public CurrencyType GetBaseCurrency()
        {
            return CurrencyType.EUR;
        }
    }
}
