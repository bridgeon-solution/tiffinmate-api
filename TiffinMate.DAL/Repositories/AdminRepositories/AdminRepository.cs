using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.AdminInterfaces;
using NotificationEntity = TiffinMate.DAL.Entities.ProviderEntity.Notification;
namespace TiffinMate.DAL.Repositories.AdminRepositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbContext _appDbContext;
        public AdminRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<Admin> AdminLogin(string email)
        {
            var admin = await _appDbContext.Admins.FirstOrDefaultAsync(a => a.email == email);

            return admin;

        }
        public async Task AddAsync(NotificationEntity notification)
        {
            await _appDbContext.notifications.AddAsync(notification);
            await _appDbContext.SaveChangesAsync();
        }
        public async Task<List<Admin>> GetAdminAsync()
        {
            return await _appDbContext.Admins
                .Where(u => u.role == "admin")
                .ToListAsync();
        }


    }
}
