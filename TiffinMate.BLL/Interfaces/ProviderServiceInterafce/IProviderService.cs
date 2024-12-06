using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.Interfaces.ProviderServiceInterafce
{
    public interface IProviderService
    {
        Task<string> UploadCertificateToS3(Stream certificateStream, string uniqueFileName, string contentType);
        Task<LoginResponse> LoginProvider(ProviderLoginDTO loginData);
        Task<string> GenereateAndSendPassword(Guid ProviderId);

    }
}
