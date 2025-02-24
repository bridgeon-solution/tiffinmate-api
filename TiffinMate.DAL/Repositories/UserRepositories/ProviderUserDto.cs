using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.DAL.Repositories.UserRepositories
{
    public class ProviderUserDto
    {
        public Order Order { get; set; }
        public Subscription Subscription { get; set; }
    }
}
