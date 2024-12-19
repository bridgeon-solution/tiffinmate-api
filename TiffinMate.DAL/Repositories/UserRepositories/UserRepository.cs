using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserInterfaces;

namespace TiffinMate.DAL.Repositories.UserRepositories
{
    public class UserRepository : IUserRepository

    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> BlockUnblockUser(Guid id)

        {
            var user = await _context.users.SingleOrDefaultAsync(u => u.id == id);
            return user;
        }
        public async Task<List<User>> GetUsers()
        {
            return await _context.users.ToListAsync();

        }
        public async Task<User> GetUserById(Guid id)
        {
            return await _context.users.FirstOrDefaultAsync(x => x.id == id);
        }
        public async Task UpdateUser(User user)
        {
            _context.users.Update(user);
            await _context.SaveChangesAsync();
        }

        public async Task<List<User>> UserPagination()
        {
            return await _context.users.ToListAsync();

        }


    }
}
