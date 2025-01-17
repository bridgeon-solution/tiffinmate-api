using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities.ProviderEntity;
using TiffinMate.DAL.Interfaces.NotificationInterfaces;

namespace TiffinMate.DAL.Repositories.NotificationRepository
{
    public class NotificationRepository:INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Notification notification)
        {
            await _context.notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        //getadminnotification
        public async Task<List<Notification>> GetAdminNotification(string recipienttype)
        {
            if (recipienttype != null)
            {
                return await _context.notifications.Where(e=>e.recipient_type==recipienttype & e.is_delete==false).ToListAsync();
            }
            else
            {
                return await _context.notifications.ToListAsync();
            }
        }
    }
}
