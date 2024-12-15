using Microsoft.AspNetCore.Http;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IProviderService
    {
        Task<bool> AddProvider(ProviderDTO product, IFormFile certificateFile);
        Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto);
        Task<bool> AddProviderDetails(ProviderDetailsDTO providerDetailsdto, IFormFile logo, IFormFile image);
        Task<List<Provider>> GetProviders();
        Task<ProviderLoginResponse> GetRefreshToken(string refreshToken);

    }
}
