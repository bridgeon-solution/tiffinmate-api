using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.CloudinaryInterface
{
    public interface ICloudinaryService
    {
        Task<string> UploadDocumentAsync(IFormFile file);
    }
}
