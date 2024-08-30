using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Avto.BL;
using Avto.BL.Services.Exchange;
using Avto.BL.Services.Exchange.RefreshExchangeRates;
using Avto.DAL;

namespace Avto.Tests.BL.Base
{
    public abstract class BaseServiceTests<TService>
    {
        private static bool _isFirstCall = true;
        private static ServiceProvider _serviceProvider;
        private IServiceScope _scope;
        protected TService Service { get; private set; }
        protected Storage Storage { get; private set; }
        protected bool TestIsRunningOnLocalPC { get; private set; }



        [TestInitialize]
        public async Task BaseInitialize()
        {
            if (_isFirstCall)
            {
                _serviceProvider = GetDiContainer();

                using (var scope = _serviceProvider.CreateScope())
                {
                    DatabaseInitializer.SeedTestData(scope.ServiceProvider.GetRequiredService<Storage>());    
                }
                
                _scope = _serviceProvider.CreateScope(); 
                var calculatedExchangeRatesService = Resolve<ExchangeService>();
                await calculatedExchangeRatesService.RefreshExchangeRates();

                _isFirstCall = false;
            }

            _scope = _serviceProvider.CreateScope();   
            Service = _scope.ServiceProvider.GetRequiredService<TService>();
            Storage = _scope.ServiceProvider.GetRequiredService<Storage>();
        }

        public void CleanStorageCache()
        {
            Storage.Dispose();
            Storage = Resolve<Storage>();
        }

        [TestCleanup]
        public void BaseCleanup()
        {
            _scope.Dispose();
        }

        protected T Resolve<T>()
        {
            return _scope.ServiceProvider.GetRequiredService<T>();
        }

        private ServiceProvider GetDiContainer()
        {
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            var configuration = configurationBuilder.Build();
            var appSettings = new AppSettings(configuration);

            var services = new ServiceCollection();
            DependencyManager.Configure(services, appSettings);
            TestIsRunningOnLocalPC = true;
            //hack for CI machine
#if RELEASE
            TestIsRunningOnLocalPC = false;
#endif

            return services.BuildServiceProvider();
        }
    }
}
