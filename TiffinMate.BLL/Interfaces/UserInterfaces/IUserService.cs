using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.DAL.Entities;

namespace TiffinMate.BLL.Interfaces.UserInterfaces
{
    public interface IUserService
    {
        Task<UserProfileDto> GetUserById(Guid id);
        Task<BlockUnblockResponse> BlockUnblock(Guid id);
        Task<string> UploadImage(IFormFile image);
        Task<string> UpdateUser(Guid id, UserProfileDto reqDto);
        Task<UserResultDTO> GetUsers(int page, int pageSize, string search=null, string filter=null);
        Task<List<AllUserOutputDto>> UsersLists(Guid ProviderId, int page, int pageSize, string search = null);
    }
}
