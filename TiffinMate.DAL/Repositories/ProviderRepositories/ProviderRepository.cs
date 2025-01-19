using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.ProviderInterface;

namespace TiffinMate.DAL.Repositories.ProviderRepositories
{
    public class ProviderRepository: IProviderRepository
    {
        private readonly AppDbContext _context;

        public ProviderRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Provider> AddProviderAsync(Provider provider)
        {
            _context.Providers.Add(provider);
            await _context.SaveChangesAsync();
            return provider;
        }
        public async Task<string> AddProviderDetailsAsync(ProviderDetails proDetails)
        {
            _context.ProvidersDetails.Add(proDetails);
            await _context.SaveChangesAsync();
            return "ok";
        }
        public async Task<bool>EmailExistOrNot(string email)
        {
            return await _context.Providers.AnyAsync(o => o.email == email);
        }
        public async Task<Provider> Login(string email)
        {
            return await _context.Set<Provider>().FirstOrDefaultAsync(p => p.email == email);
        }
        public async Task<List<Provider>> GetProviderByCategory(string? verificationStatus)
        {
            return await _context.Providers.Where(x => x.verification_status == verificationStatus).ToListAsync();
        }
        public async Task<Provider> GetProviderById(Guid id)
        {
            return await _context.Set<Provider>().FirstOrDefaultAsync(p => p.id == id);
        }
        public async void Update(Provider provider)
        {
            _context.Providers.Update(provider);

        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<bool> ExistsAsync(Guid providerId)
        {
            return await _context.ProvidersDetails.AnyAsync(p => p.provider_id == providerId);
        }
        public async Task<bool> Remove(Guid id)
        {
            var remove = await _context.Set<Provider>().FirstOrDefaultAsync(p => p.id == id);
            _context.Providers.Remove(remove);
            return true;
        }
       
        public async Task<Provider> GetUserByRefreshTokenAsync(string refreshToken)
        {
            return await _context.Providers.FirstOrDefaultAsync(u => u.refresh_token == refreshToken);
        }
        public async Task<Provider> BlockUnblockUser(Guid id)
        {
            var provide = await _context.Providers.SingleOrDefaultAsync(u => u.id == id);
            return provide;
        }
        public async Task<List<Provider>> GetAProviderById(Guid id)
        {
            return await _context.Set<Provider>().Where(r => r.id == id)
         .Include(r => r.provider_details)
         .Include(r => r.review).Include(r=>r.rating)
         .ToListAsync();
        }

        public async Task<Provider> GetUserByEmail(string email)
        {
            return await _context.Providers.FirstOrDefaultAsync(e => e.email == email);

        }
        public async Task<bool> UpdatePassword(Provider provider, string password)
        {
            provider.password = password;
            provider.updated_at = DateTime.UtcNow;
            _context.Providers.Update(provider);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<List<Provider>> GetProviders()
        {
            return await _context.Providers.ToListAsync();

        }
        public async Task<List<ProviderDetails>> GetProvidersWithDetail()
        {
            return await _context.ProvidersDetails.ToListAsync();
        }
        public async Task<ProviderDetails> GetProviderDetailsById(Guid id)
        {
            return await _context.ProvidersDetails.FirstOrDefaultAsync(p => p.provider_id == id);
        }
        public async Task<ProviderDetails> GetProviderDetailsByProviderIdAsync(Guid providerId)
        {
            return await _context.ProvidersDetails.FirstOrDefaultAsync(pd => pd.provider_id == providerId);
        }
        public async void UpdateDetails(ProviderDetails provider)
        {
            _context.ProvidersDetails.Update(provider);

        }
    }
}


