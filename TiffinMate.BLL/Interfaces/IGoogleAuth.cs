using Google.Apis.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.Interfaces
{
    public interface IGoogleAuth
    {
        Task<TiffinMate.DAL.Entities.User> Authenticate(GoogleJsonWebSignature.Payload payload);
        //Task<TiffinMate.DAL.Entities.User> FindUserOrAdd(GoogleJsonWebSignature.Payload payload);
    }
}
