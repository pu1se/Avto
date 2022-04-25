using System.Threading.Tasks;

namespace Avto.DAL.CloudServices
{
    public interface IKeyVaultStorage
    {
        Task AddOrUpdateSecretAsync(string secretKey, string secretValue);
        Task DeleteSecretAsync(string secretName);
        Task<string> GetSecretByNameAsync(string secretKey);
    }
}