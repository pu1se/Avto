using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault.Models;
using PaymentMS.DAL;
using PaymentMS.DAL.CloudServices;

namespace PaymentMS.Tests.BL.Mocks
{
    public class KeyVaultMock : IKeyVaultStorage
    {
        private static Dictionary<string, string> Collection { get; }

        static KeyVaultMock()
        {
            Collection = new Dictionary<string, string>
            {
                {"tmp", "hello world"},
            };
        }


        public Task AddOrUpdateSecretAsync(string secretKey, string secretValue)
        {
            Collection[secretKey] = secretValue;
            return Task.FromResult(secretKey);
        }

        public Task DeleteSecretAsync(string secretName)
        {
            if (Collection.ContainsKey(secretName))
            {
                Collection.Remove(secretName);
            }

            return Task.FromResult("");
        }

        public Task<string> GetSecretByNameAsync(string secretKey)
        {
            if (!Collection.ContainsKey(secretKey))
                return Task.FromResult(string.Empty);

            return Task.FromResult(Collection[secretKey]);
        }
    }
}
