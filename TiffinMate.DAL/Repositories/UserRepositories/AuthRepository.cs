using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.DAL.Repositories.UserRepositories
{
    public class AuthRepository:IAuthRepository
    {
        private readonly AppDbContext _context;
        public AuthRepository(AppDbContext context)
        {
            _context = context;
            
        }

        public async Task<bool>UserExists(string email)
        {
           return await _context.users.AnyAsync(x => x.email == email);
        }
        public async Task CreateUser(User user)
        {
            await _context.users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<User>GetUserByEmail(string email)
        {
            return await _context.users.FirstOrDefaultAsync(e => e.email == email);
            
        }
        public async Task<bool> UpdatePassword(User user, string password)
        {
            user.password = password;
            user.updated_at = DateTime.UtcNow;

            
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
