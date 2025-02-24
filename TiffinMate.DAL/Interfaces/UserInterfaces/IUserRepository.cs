using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Repositories.UserRepositories;

namespace TiffinMate.DAL.Interfaces.UserInterfaces
{
    public interface IUserRepository
    {
        Task<User> BlockUnblockUser(Guid id);
        Task<User> GetUserById(Guid id);
        Task UpdateUser(User user);
        Task<List<User>> GetUsers();
        Task<List<ProviderUserDto>> GetOrdersByProvider(Guid providerId);
        Task<List<Subscription>> GetSubscribedUsers();

    }
}
