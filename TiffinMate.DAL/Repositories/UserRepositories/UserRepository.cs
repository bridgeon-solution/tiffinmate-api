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
  public  class UserRepository:IUserRepository
    {
        private readonly AppDbContext _appDbContext;

        public UserRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
      public async  Task<User> BlockUnblockUser(Guid id)
        {
            var user = await _appDbContext.users.SingleOrDefaultAsync(u => u.id == id);
            return user;
        }

    }
}
