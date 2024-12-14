using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<UserResponseDto> GetUserById(Guid id);
        Task<List<User>> GetAllUsers();
        Task<BlockUnblockResponse> BlockUnblock(Guid id);
    }
}
