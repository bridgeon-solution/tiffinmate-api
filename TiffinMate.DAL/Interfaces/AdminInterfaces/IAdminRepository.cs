using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;
using NotificationEntity = TiffinMate.DAL.Entities.ProviderEntity.Notification;
namespace TiffinMate.DAL.Interfaces.AdminInterfaces
{
    public interface IAdminRepository
    {
        Task<Admin> AdminLogin(string email);
        Task AddAsync(NotificationEntity notification);
        Task<List<Admin>> GetAdminAsync();
        Task<Admin> GetUserByRefreshTokenAsync(string refreshToken);
        void Update(Admin admin);
        Task SaveChangesAsync();
    }
}
