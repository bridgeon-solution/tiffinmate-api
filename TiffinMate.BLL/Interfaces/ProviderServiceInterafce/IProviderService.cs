using Microsoft.AspNetCore.Http;
using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IProviderService
    {
        Task<ProviderDTO> AddProvider(ProviderDTO product, IFormFile certificateFile);
        Task<string> AddProviderDetails(ProviderDetailsDTO providerDetailsdto, IFormFile logo, IFormFile image);

        Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto);

    }
}
