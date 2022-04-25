using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avto.BL.Services.Exchange.Api.CurrencyLayer;
using Avto.BL.Services.Exchange.ExchangeProviders.API;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Exchange.Handlers.Queries.GetExchangeProviders
{
    public class GetProvidersQueryHandler 
        : QueryHandler<GetProvidersQuery, CallListDataResult<ExchangeProvidersResponse>>
    {
        private ProviderFactory ProviderFactory { get; }
        public GetProvidersQueryHandler(
            ProviderFactory providerFactory,
            Storage storage, 
            LogService logger) : base(storage, logger)
        {
            ProviderFactory = providerFactory;
        }

        protected override Task<CallListDataResult<ExchangeProvidersResponse>> HandleCommandAsync(GetProvidersQuery query)
        {
            var list = new List<ExchangeProviderType>();

            foreach (var providerType in EnumHelper.ToList<ExchangeProviderType>())
            {
                if (query.FilterByCurrency == null)
                {
                    list.Add(providerType);
                    continue;
                }

                var provider = ProviderFactory.GetProvider(providerType);
                if (provider.GetSupportedCurrencies().Contains(query.FilterByCurrency.Value))
                {
                    list.Add(providerType);
                }
            }
            
            var result = list
                .Select(item => new ExchangeProvidersResponse()
                {
                    Id = item.ToString(),
                    Name = EnumHelper.GetDescription(item)
                });
            return Task.FromResult(SuccessListResult(result));
        }
    }
}
