using System;
using Microsoft.Extensions.DependencyInjection;
using Avto.BL._Core;
using Avto.DAL;

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
