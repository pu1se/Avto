using System.Threading.Tasks;

namespace PaymentMS.DAL.CloudServices
{
    public interface IKeyVaultStorage
    {
        Task AddOrUpdateSecretAsync(string secretKey, string secretValue);
        Task DeleteSecretAsync(string secretName);
        Task<string> GetSecretByNameAsync(string secretKey);
    }
}