using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.KeyVault.Models;
using Microsoft.Azure.Services.AppAuthentication;

namespace Avto.DAL.CloudServices
{
    public class KeyVaultStorage : IKeyVaultStorage
    {
        public string BaseUrl { get; protected set; }

        public KeyVaultStorage(string keyVaultBaseUrl)
        {
            BaseUrl = keyVaultBaseUrl;
        }

        private void ValidateSecretKey(string secretKey,string secretValue)
        {
            if (secretKey.IsNullOrEmpty())
            {
                throw new ArgumentException("Secret name must not be null", nameof(secretKey));
            }

            if (secretValue.IsNullOrEmpty())
            {
                throw new ArgumentException("Secret value must not be null", nameof(secretValue));
            }

            if (secretKey.Length > (int) sbyte.MaxValue)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(secretKey),
                    secretKey.Length,
                    "Secret names must be 1..127 characters in length.");
            }
        }

        public async Task AddOrUpdateSecretAsync(string secretKey,string secretValue)
        {
            ValidateSecretKey(secretKey, secretValue);

            using (var client = CreateKeyVaultClient())
            {
                await client.SetSecretAsync(BaseUrl, secretKey, secretValue);
            }
        }
        
        public async Task<string> GetSecretByNameAsync(string secretKey)
        {
            using (var client = CreateKeyVaultClient())
            {
                var secretBundle = await client.GetSecretAsync(BaseUrl, secretKey);
                return secretBundle.Value;
            }
        }

        public async Task DeleteSecretAsync(string secretName)
        {
            using (var client = CreateKeyVaultClient())
            {
                await client.DeleteSecretAsync(BaseUrl, secretName);
            }
        }
        

        private KeyVaultClient CreateKeyVaultClient()
        {
            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var keyVaultClient = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback));
            return keyVaultClient;
        }
        
    }
}
