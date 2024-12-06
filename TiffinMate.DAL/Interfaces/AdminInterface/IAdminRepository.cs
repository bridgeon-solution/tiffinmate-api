using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities;

namespace TiffinMate.DAL.Interfaces.AdminInterface
{
    public interface IAdminRepository
    {
        Task<Admin> AdminLogin(string email);
    }
}
