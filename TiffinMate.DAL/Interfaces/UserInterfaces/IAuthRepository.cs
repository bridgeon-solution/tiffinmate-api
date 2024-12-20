using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.DAL.Interfaces.UserRepositoryInterface
{
    public interface IAuthRepository
    {
        Task<bool> UserExists(string email);
        Task CreateUser(User user);
        Task<User> GetUserByEmail(string email);
        Task<bool> UpdatePassword(User user, string password);
    }
}
