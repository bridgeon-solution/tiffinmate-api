using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces.ProviderVerification
{
    public interface IProviderVerificationService
    {
        Task<bool> SendPassword(Guid providerId);

        Task<bool> RemoveData(Guid providerId);

    }
}
