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
        public async Task <string>AddProviderDetailsAsync(ProviderDetails proDetails)
        {
            _context.ProvidersDetails.Add(proDetails);
            await _context.SaveChangesAsync();
            return "added";
        }
        public async Task<Provider> Login(string email, string password)
        {
            return await _context.Set<Provider>().FirstOrDefaultAsync(p => p.email == email && p.password == password);
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

        public async Task<List<Provider>> GetProviders()
        {
            return await _context.Providers.ToListAsync();
        }




    }
}
