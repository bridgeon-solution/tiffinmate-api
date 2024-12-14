using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.DAL.Interfaces.ProviderInterface
{
    public interface IProviderRepository
    {
        Task<Provider> AddProviderAsync(Provider provider);
        Task<string> AddProviderDetailsAsync(ProviderDetails proDetails);
        Task<Provider> Login(string email, string password);
        Task<Provider> GetProviderById(Guid id);
        void Update(Provider provider);
        Task SaveChangesAsync();
        Task<List<Provider>> GetProviders();
        Task<bool> ExistsAsync(Guid providerId);
        Task<bool> Remove(Guid id);
    }
}
