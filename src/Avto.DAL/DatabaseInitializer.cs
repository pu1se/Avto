using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore.Internal;
using Avto.DAL.Entities;
using Avto.DAL.Enums;
using Avto.DAL.Repositories;

namespace Avto.DAL
{
    public static class TestData
    {
        public static readonly Guid PaymentSenderOrganizationId = new Guid("1118aa58-c48e-4696-98cf-1b6e22c63076");
        public static readonly Guid PaymentReceiverOrganizationId = new Guid("2228aa58-c48e-4696-98cf-1b6e22c63076");
        public static readonly Guid BalanceProviderOrganizationId = new Guid("3333aa58-c48e-4696-98cf-1b6e22c63076");
        public static readonly Guid BalanceClientOrganizationId = new Guid("4444aa58-c48e-4696-98cf-1b6e22c63076");
        public static readonly string Stripe_PublicKey_1 = "pk_test_g6aW40paKZmytcLb8gyn3xxi00oca1e78s";
        public static readonly string Stripe_SecretKey_1 = "sk_test_Y9qislG9iCykJcQ0zBUTkjiO00HT1yzzuS";
        public static readonly string Stripe_PublicKey_2 = "pk_test_5McBOo8Ta3fM6YsycFxrlgnA00BbA90UCs";
        public static readonly string Stripe_SecretKey_2 = "sk_test_H4m1OIWE5D9IvfrkY6ZCvJ7p00HAYow5ER";
        public static readonly string Stripe_PublicKey_3 = "pk_test_BKgusmMo55W6ky6nRMRMJUsO00xSIhyIhb";
        public static readonly string Stripe_SecretKey_3 = "sk_test_0RAKPGEjOPqcSHDMJHnn5dUR005SqcmWeY";
        public static readonly string Stripe_PublicKey_4 = "pk_test_fnwOEbGVH3rHdVvyFdpOLkjM00yjb3HxiX";
        public static readonly string Stripe_SecretKey_4 = "sk_test_JqBUKkJz5eruQbSg17Z1Hx4s00QaSNzu8X";

        public static readonly Guid CurrencyOwnerOrganizationId = new Guid("5558aa58-c48e-4696-98cf-1b6e22c63076");
        public static readonly Guid FrontSellerOrganizationId = new Guid("7778aa58-c48e-4696-98cf-1b6e22c63076");
    }

    public static class DatabaseInitializer
    {        
        public static void SeedWithTestData(Storage storage)
        {
            if (!storage.Organizations.ExistsAsync(organization =>
                organization.Id == TestData.PaymentReceiverOrganizationId).GetAwaiter().GetResult())
            {
                TestDataForPaymentIteration(storage);
            }

            if (!storage.Organizations.ExistsAsync(organization =>
                organization.Id == TestData.BalanceClientOrganizationId).GetAwaiter().GetResult())
            {
                TestDataForBalanceIteration(storage);
            }

            if (!storage.Organizations.ExistsAsync(organization =>
                organization.Id == TestData.CurrencyOwnerOrganizationId).GetAwaiter().GetResult())
            {
                TestDataForExchangeConfigurationIteration(storage);
            }

            storage.SaveChangesAsync().GetAwaiter().GetResult();
        }

        private static void TestDataForExchangeConfigurationIteration(Storage storage)
        {
            foreach (var currencyItem in EnumHelper.ToList<CurrencyType>())
            {
                var isExists = storage.Currencies
                    .ExistsAsync(e => e.Code == currencyItem.ToString()).GetAwaiter().GetResult();

                if (!isExists)
                {
                    storage.Currencies.Add(new CurrencyEntity
                    {
                        Code = currencyItem.ToString(),
                        Name = EnumHelper.GetDescription(currencyItem),
                    });
                }
            }

            storage.Organizations.Add(new OrganizationEntity
            {
                Id = TestData.CurrencyOwnerOrganizationId,
                Name = "Currencies Owner"
            });

            storage.Organizations.Add(new OrganizationEntity
            {
                Id = TestData.FrontSellerOrganizationId,
                Name = "Seller with Currency Feature"
            });

            storage.CurrencyExchangeConfis.Add(new CurrencyExchangeConfigEntity
            {
                OrganizationId = TestData.CurrencyOwnerOrganizationId,
                ExchangeProvider = ExchangeProviderType.ECB,
                FromCurrencyCode = CurrencyType.EUR.ToString(),
                ToCurrencyCode = CurrencyType.USD.ToString(),
                Surcharge = new decimal(2.5),
            });

            storage.CurrencyExchangeConfis.Add(new CurrencyExchangeConfigEntity
            {
                OrganizationId = TestData.CurrencyOwnerOrganizationId,
                ExchangeProvider = ExchangeProviderType.CurrencyLayer,
                FromCurrencyCode = CurrencyType.EUR.ToString(),
                ToCurrencyCode = CurrencyType.DKK.ToString(),
                Surcharge = new decimal(3),
            });

            storage.CurrencyExchangeConfis.Add(new CurrencyExchangeConfigEntity
            {
                OrganizationId = TestData.CurrencyOwnerOrganizationId,
                ExchangeProvider = ExchangeProviderType.Custom,
                FromCurrencyCode = CurrencyType.EUR.ToString(),
                ToCurrencyCode = CurrencyType.CHF.ToString(),
                Surcharge = new decimal(4),
                CustomRate = new decimal(1.123)
            });
        }

        private static void TestDataForBalanceIteration(Storage storage)
        {
            storage.Organizations.Add(new OrganizationEntity
            {
                Id = TestData.BalanceProviderOrganizationId,
                Name = "Some Balance Provider"
            });

            storage.Organizations.Add(new OrganizationEntity
            {
                Id = TestData.BalanceClientOrganizationId,
                Name = "Some Balance Client"
            });

            var balanceProviderId = Guid.NewGuid();
            storage.BalanceProviders.Add(new BalanceProviderEntity
            {
                Id = balanceProviderId,
                CreditLimit = 1000,
                Currency = CurrencyType.EUR.ToString(),
                IsWireTransferIncomeEnabled = true,
                OrganizationId = TestData.BalanceProviderOrganizationId
            });

            storage.BalanceClients.Add(new BalanceClientEntity
            {
                OrganizationId = TestData.BalanceClientOrganizationId,
                BalanceProviderId = balanceProviderId,
            });
        }

        private static void TestDataForPaymentIteration(Storage storage)
        {
            storage.Organizations.Add(new OrganizationEntity
            {
                Id = TestData.PaymentReceiverOrganizationId,
                Name = "Some Reseller"
            });

            storage.Organizations.Add(new OrganizationEntity
            {
                Id = TestData.PaymentSenderOrganizationId,
                Name = "Some Customer"
            });

            storage.StripeReceivingWays.AddAsync(new ReceivingWayEntity
            {
                Id = Guid.NewGuid(),
                StripePrivateConfig = new StripeConfigForKeyVault
                {
                    PublicKey = TestData.Stripe_PublicKey_1,
                    SecretKey = TestData.Stripe_SecretKey_1
                },
                StripePublicConfig = new StripeConfigForDB
                {
                    PublicKey = TestData.Stripe_PublicKey_1
                },
                PaymentMethod = PaymentMethodType.StripeCard,
                OrganizationId = TestData.PaymentReceiverOrganizationId,
            }).GetAwaiter().GetResult();
        }
    }
}
