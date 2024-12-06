using Amazon.S3;
using Amazon.S3.Model;

namespace TiffinMate.BLL.Services.AWS
{
    public interface IAwsS3Service
    {
        Task<string> UploadFileAsync(Stream fileStream, string fileName);
    }

    public class AWSProviderService : IAwsS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public AWSProviderService(IAmazonS3 s3Client, string bucketName)
        {
            _s3Client = s3Client;
            _bucketName = bucketName;
        }
        public async Task<string> UploadFileAsync(Stream fileStream, string fileName)
        {
            try
            {
                var request = new Amazon.S3.Model.PutObjectRequest
                {
                    BucketName = _bucketName,
                    Key = fileName, 
                    InputStream = fileStream,
                    ContentType = "application/octet-stream",
                    CannedACL = S3CannedACL.PublicRead
                };

                PutObjectResponse response = await _s3Client.PutObjectAsync(request);

                if (response.HttpStatusCode == System.Net.HttpStatusCode.OK)
                {
                    return $"https://{_bucketName}.s3.amazonaws.com/{fileName}";
                }
                else
                {
                    throw new Exception($"Error uploading file: {response.HttpStatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error uploading file to S3: {ex.Message}");
            }
        }
    }
}
