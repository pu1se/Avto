﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Avto.BL._Core;
using Avto.BL.Services;
using Avto.DAL;

namespace Avto.BL
{
    public static class DependencyManager
    {
        public static void Configure(IServiceCollection services, AppSettings settings)
        {
            services.AddSingleton(settings);
            services.AddScoped<Storage>();
            services.AddScoped<LogService>();

            services.AddTransientHandlers();
            services.AddTransientServices();
            services.AddTransientExchangeProviderApis();

            services.AddDbContext<Storage>(
                options =>
                    options.UseSqlServer(settings.DatabaseConnection)
            );                       
        }

        private static IEnumerable<Type> _allTypes;
        private static IEnumerable<Type> AllTypes
        {
            get { return _allTypes ?? (_allTypes = Assembly.GetExecutingAssembly().GetTypes()); }
        }

        private static IEnumerable<Type> _handlerTypes;
        private static void AddTransientHandlers(this IServiceCollection services)
        {
            if (_handlerTypes == null)
            {
                _handlerTypes = AllTypes
                    .Where(
                        type => 
                        type.GetInterfaces().Contains(typeof(IHandler)) &&
                        type.BaseType != typeof(CallResultShortcuts)
                    );
            }
            
            foreach (var type in _handlerTypes)
            {
                services.AddTransient(type);
            }
        }

        private static IEnumerable<Type> _serviceTypes;
        private static void AddTransientServices(this IServiceCollection services)
        {
            if (_serviceTypes == null)
            {
                _serviceTypes = AllTypes.Where(type => type.BaseType == typeof(BaseService));
            }
            
            foreach (var type in _serviceTypes)
            {
                services.AddTransient(type);
            }
        }

        private static IEnumerable<Type> _providerTypes;
        private static void AddTransientExchangeProviderApis(this IServiceCollection services)
        {
            if (_providerTypes == null)
            {
                _providerTypes = AllTypes.Where(type => type.BaseType == typeof(BaseExternalApiProvider));
            }
            
            foreach (var type in _providerTypes)
            {
                services.AddTransient(type);
            }
        }
    }
}
