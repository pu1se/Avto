using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PaymentMS.BL;
using PaymentMS.BL.Services;
using PaymentMS.BL.Services.Currency;
using PaymentMS.BL.Services.Stripe;
using PaymentMS.BL.Services.Stripe.Api;
using PaymentMS.DAL;
using PaymentMS.DAL.CloudServices;
using PaymentMS.DAL.Repositories;
using PaymentMS.Tests.BL.Mocks;

namespace PaymentMS.Tests.BL.Base
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
        public void BaseInitialize()
        {
            if (_isFirstCall)
            {
                _serviceProvider = GetDiContainer();

                using (var scope = _serviceProvider.CreateScope())
                {
                    DatabaseInitializer.SeedWithTestData(scope.ServiceProvider.GetRequiredService<Storage>());    
                }
                
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

            configurationBuilder.AddAzureKeyVault(
                appSettings.AzureKeyVaultUrl
            );

            var services = new ServiceCollection();
            AddDependencies(services, appSettings);

            return services.BuildServiceProvider();
        }

        private void AddDependencies(ServiceCollection services, AppSettings appSettings)
        {            
            var connectionStringForUnitTests = appSettings.DatabaseConnection;

#if DEBUG
            connectionStringForUnitTests =
                @"Server=.\; Database=PaymentMS; Initial Catalog=PaymentMS;Integrated Security=False;Trusted_Connection=True;";
#endif
            
            DependencyManager.Configure(services, appSettings);

            TestIsRunningOnLocalPC = true;
            //hack for CI machine
#if RELEASE
            services.AddTransient<IKeyVaultStorage, KeyVaultMock>();
            services.AddTransient<IStripeApi, StripeApiMock>();
            TestIsRunningOnLocalPC = false;
#endif

            var dbOption = new DbContextOptionsBuilder<DataContext>()
                .UseSqlServer(connectionStringForUnitTests)
                .Options;
            services.AddTransient(serviceProvider => new DataContext(dbOption));
            services.AddTransient<Storage>();
        }        
    }
}
