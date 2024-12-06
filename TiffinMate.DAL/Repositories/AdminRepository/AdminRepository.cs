using Microsoft.EntityFrameworkCore;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.AdminInterface;

namespace TiffinMate.DAL.Repositories.AdminRepository
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
    }
}
