using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.DAL.Interfaces.UserInterfaces
{
    public interface IUserRepository
    {
        Task<User> BlockUnblockUser(Guid id);
        Task<List<User>> GetUsers();
        Task<User> GetUserById(Guid id);
        Task UpdateUser(User user);



    }
}
