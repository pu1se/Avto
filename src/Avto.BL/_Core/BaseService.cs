using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Avto.BL._Core;
using Avto.BL.Services.Balance.Models;
using Avto.BL.Services.Stripe.Models;
using Avto.BL.Services.Stripe;
using Avto.DAL.Repositories;
using Stripe;

namespace Avto.BL.Services
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
