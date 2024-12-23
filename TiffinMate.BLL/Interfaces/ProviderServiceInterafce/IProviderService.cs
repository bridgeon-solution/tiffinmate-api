using Microsoft.AspNetCore.Http;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IProviderService
    {
        Task<bool> AddProvider(ProviderDTO product, IFormFile certificateFile);
        Task<ProviderLoginResponse> AddLogin(ProviderLoginDTO providerdto);
        Task<bool> AddProviderDetails(ProviderDetailsDTO providerDetailsdto, IFormFile logo, IFormFile image);
       
        Task<ProviderLoginResponse> GetRefreshToken(string refreshToken);

        Task<BlockUnblockResponse> BlockUnblock(Guid id);
        //Task<List<ProviderDetailResponse>> GetProvidersWithDetail();
        //Task<ProviderDetailedDTO> GetProviderDetailsById(Guid id);
        Task<ProviderByIdDto> ProviderById(Guid providerId);
        Task<List<ProviderResponseDTO>> GetProviders(int page, int pageSize, string search = null, string filter = null, string verifystatus = null);
        Task<ProviderDetailedDTO> GetProviderDetailsById(Guid id);
        Task<List<ProviderDetailResponse>> GetProvidersWithDetail();

    }
}
