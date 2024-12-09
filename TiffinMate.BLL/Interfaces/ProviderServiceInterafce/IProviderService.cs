using Microsoft.AspNetCore.Http;
using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IProviderService
    {
        Task<bool> AddProvider(ProviderDTO product, IFormFile certificateFile);
        Task<bool> AddProviderDetails(ProviderDetailsDTO providerDetailsdto, IFormFile logo, IFormFile image);

        Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto);

    }
}
