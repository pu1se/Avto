using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Avto.BL.Services.Balance.Handlers;
using Avto.BL.Services.Balance.Models;
using Avto.BL.Services.Stripe;
using Avto.BL.Services.Stripe.Models;
using Avto.DAL;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Balance
{
    public class BalanceService : BaseService
    {
        public BalanceService(
            Storage storage, 
            IServiceProvider services) : base(storage, services)
        {
        }


        public async Task<CallDataResult<IEnumerable<BalanceProviderOrganizationModel>>> GetPossibleProviderOrganzations()
        {
            var entities = await Storage.Organizations
                                .Where(e => e.SendingWays.Count == 0 &&
                                            e.BalanceClients.Count == 0)
                                .Include(e => e.ReceivingWays)
                                .ToListAsync();

            return SuccessListResult(entities.Select(e => e.ToModel()));
        }

        public async Task<CallDataResult<BalanceProviderModel>> GetProvider(Guid organizationId)
        {
            var entity = await Storage.BalanceProviders
                                .Include(e => e.Organization)
                                .Include(e => e.ReceivingWays)
                                .GetAsync(
                                    e => e.OrganizationId == organizationId
                                );

            if (entity == null)
            {
                return NotFoundResult<BalanceProviderModel>("Balance provider was not found.");
            }

            return SuccessResult(entity.ToProviderModel());
        }

        public async Task<CallDataResult<IEnumerable<BalanceProviderModel>>> GetProviders()
        {
            var entities = await Storage.BalanceProviders
                                .Include(e => e.Organization)
                                .Include(e => e.ReceivingWays)
                                .ToListAsync();

            return SuccessListResult(entities.Select(e=>e.ToProviderModel()));
        }

        public async Task<CallDataResult<IEnumerable<BalanceClientModel>>> GetClients(Guid providerOrganizationId)
        {
            var list = await Storage.BalanceClients
                                .Where(e => e.BalanceProvider.OrganizationId == providerOrganizationId)
                                .Select(BalanceClientModel.Map())
                                .ToListAsync();

            return SuccessListResult(list);
        }

        public async Task<CallDataResult<IEnumerable<BalancePaymentModel>>> GetPayments(Guid providerOrganizationId, Guid clientOrganizationId)
        {
            var list = await Storage.Payments
                                .Where(e => e.BalanceClient.OrganizationId == clientOrganizationId &&
                                            e.BalanceClient.BalanceProvider.OrganizationId == providerOrganizationId)
                                .OrderByDescending(e => e.CreatedDateUtc)
                                .Select(BalancePaymentModel.Map())
                                .ToListAsync();
            
            return SuccessListResult(list);

        }

        public async Task<CallDataResult<BalanceClientModel>> GetClient(Guid providerOrganizationId, Guid clientOrganizationId)
        {
            var model = await Storage.BalanceClients
                                .Where(e => e.BalanceProvider.OrganizationId == providerOrganizationId &&
                                            e.OrganizationId == clientOrganizationId)
                                .Select(BalanceClientModel.Map())
                                .GetAsync();
            if (model == null)
            {
                return NotFoundResult<BalanceClientModel>($"Balance client with organization id {clientOrganizationId} " +
                                                          $"and provider organization id {providerOrganizationId} was not found.");
            }

            return SuccessResult(model);
        }

        public Task<CallResult> AddEditProvider(AddEditBalanceProviderCommand command)
        {
            return GetHandler<AddEditBalanceProviderCommandHandler>().HandleAsync(command);
        }

        public Task<CallResult> AddClient(AddBalanceClientCommand model)
        {
            return GetHandler<AddBalanceClientCommandHandler>().HandleAsync(model);
        }

        public Task<CallResult> IncreaseByWireTransfer(IncreaseBalaceByWireTransferCommand command)
        {
            return GetHandler<IncreaseBalanceByWireTransferCommandHandler>().HandleAsync(command);
        }

        public Task<CallResult> SendPayment(SendBalancePaymentCommand command)
        {
            return GetHandler<SendBalancePaymentCommandHandler>().HandleAsync(command);
        }

        public Task<CallResult> IncreaseByStripeCard(IncreaseBalanceByStripeCardCommand command)
        {
            return GetHandler<IncreaseBalanceByStripeCardCommandHandler>().HandleAsync(command);
        }
    }
}
