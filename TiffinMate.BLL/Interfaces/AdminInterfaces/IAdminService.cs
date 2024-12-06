using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.AdmiDTO;

namespace TiffinMate.BLL.Interfaces.AdminInterface
{
    public interface IAdminService
    {
        Task<string> AdminLogin(AdminLoginDTO adminLoginDTO);
    }
}
