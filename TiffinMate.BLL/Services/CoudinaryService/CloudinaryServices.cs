using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using TiffinMate.BLL.Interfaces.CloudinaryInterface;

namespace TiffinMate.BLL.Services.CoudinaryService
{
    public class CloudinaryServices:ICloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryServices()
        {
            var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
            var apiKey = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
            var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");

            if (string.IsNullOrEmpty(cloudName) || string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(apiSecret))
            {
                throw new Exception("Cloudinary configuration is missing or incomplete");
            }

            var account = new Account(cloudName, apiKey, apiSecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<string> UploadDocumentAsync(IFormFile file)
        {
            if (file == null || file.Length == 0) return null;

            using (var stream = file.OpenReadStream())
            {
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription(file.FileName, stream),
                  
                };

                var uploadResult = await _cloudinary.UploadAsync(uploadParams);

                if (uploadResult.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return uploadResult.SecureUrl.ToString();
                }
                else
                {
                    throw new Exception("Failed to upload certificate to Cloudinary.");
                }
            }
        }
    }
}
