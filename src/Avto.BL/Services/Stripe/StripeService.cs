using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Avto.BL.Services.Stripe.Models;
using Avto.BL.Services.Stripe.Api;
using Avto.BL.Services.Stripe.Handlers;
using Avto.BL.Services.Stripe.Handlers.AddPaymentCard;
using Avto.BL.Services.Stripe.Handlers.AddReceivingWay;
using Avto.BL.Services.Stripe.Handlers.Commands.AddEditSendingWay;
using Avto.BL.Services.Stripe.Handlers.PaymentRefund;
using Avto.BL.Services.Stripe.Handlers.Queries.AllReceivingWays;
using Avto.BL.Services.Stripe.Handlers.Queries.DefaultSendingWay;
using Avto.BL.Services.Stripe.Handlers.Queries.Payments;
using Avto.BL.Services.Stripe.ResponseModels;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.BL.Services.Stripe
{
    public class StripeService : BaseService
    {        
        private SafeCallStripeApi StripeApi { get; }

        public StripeService(
            SafeCallStripeApi stripeApi, 
            Storage storage,
            IServiceProvider services) : base(storage, services)
        {
            StripeApi = stripeApi;
        }

       
        public Task<CallListDataResult<ReceivingWayQueryResponse>> GetAllReceivingWays()
        {
            return GetHandler<GetAllReceivingWaysQueryHandler>().HandleAsync(EmptyQuery.Value);
        }  

        public Task<CallListDataResult<PaymentQueryResponse>> GetPayments(GetPaymentsQuery query)
        {
            return GetHandler<GetPaymentsQueryHandler>().HandleAsync(query);
        }

        public Task<CallDataResult<DefaultSendingWayQueryResponse>> GetDefaultSendingWay(GetDefaultSendingWayQuery query)
        {
            return GetHandler<GetDefaultSendingWayQueryHandler>().HandleAsync(query);
        }

        public Task<CallDataResult<AddReceivingWayCommandResponse>> AddReceivingWay(AddReceivingWayCommand command)
        {
            return GetHandler<AddReceivingWayCommandHandler>().HandleAsync(command);
        }

        public Task<CallDataResult<AddEditSendingWayCommandResponse>> AddEditSendingWay(AddEditSendingWayCommand command)
        {
            return GetHandler<AddEditSendingWayCommandHandler>().HandleAsync(command);
        }

        public Task<CallDataResult<AddPaymentCardCommandResponse>> AddPaymentCardToReceiver(AddPaymentCardCommand command)
        {
            return GetHandler<AddPaymentCardCommandHandler>().HandleAsync(command);
        }

        public Task<CallDataResult<SendPaymentCommandResponse>> SendPayment(SendPaymentCommand command)
        {
            return GetHandler<SendPaymentCommandHandler>().HandleAsync(command);
        }

        public Task<CallDataResult<PaymentRefundCommandResponse>> PaymentRefund(PaymentRefundCommand command)
        {
            return GetHandler<PaymentRefundCommandHandler>().HandleAsync(command);
        }
    }
}
