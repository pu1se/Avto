using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using PaymentMS.BL._Core;
using PaymentMS.BL.Services.Balance.Models;
using PaymentMS.BL.Services.Stripe.Models;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.DAL.Repositories;
using Stripe;

namespace PaymentMS.BL.Services
{
    public abstract class BaseService : CallResultShortcuts
    {
        protected Storage Storage { get; }
        private IServiceProvider Services { get; }

        protected BaseService(Storage storage, IServiceProvider services)
        {
            Storage = storage;
            Services = services;
        }

        protected THandler GetHandler<THandler>() where THandler : IHandler
        {
            return Services.GetRequiredService<THandler>();
        }
    }
}
