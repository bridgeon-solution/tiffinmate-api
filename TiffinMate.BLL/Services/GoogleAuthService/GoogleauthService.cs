using Google.Apis.Auth;
using System;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.BLL.Services.GoogleAuthService
{
    public class GoogleauthService: IGoogleAuth
    {
        private readonly IAuthRepository _authrepo;

        public GoogleauthService(IAuthRepository authrepo)
        {
            _authrepo = authrepo;
        }

        public async Task<TiffinMate.DAL.Entities.User> Authenticate(GoogleJsonWebSignature.Payload payload)
        {
            await Task.Delay(1);
            return await this.FindUserOrAdd(payload);
        }

        private async Task<TiffinMate.DAL.Entities.User> FindUserOrAdd(GoogleJsonWebSignature.Payload payload)
        {
            var u = await _authrepo.HaveAUser(payload.Email);

            if (u == null)
            {
                u = new TiffinMate.DAL.Entities.User()
                {
                    id = Guid.NewGuid(),
                    name = payload.Name,
                    email = payload.Email,
                   
                };
                await _authrepo.CreateUser(u);
            }
            
            //this.PrintUsers();
            return u;
        }

       
    }
}
