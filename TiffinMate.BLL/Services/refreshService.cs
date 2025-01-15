using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.Interfaces;
using TiffinMate.BLL.Services.AdminServices;
using TiffinMate.BLL.Services.ProviderServices;
using TiffinMate.BLL.Services.UserServices;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.AdminInterfaces;
using TiffinMate.DAL.Interfaces.ProviderInterface;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.BLL.Services
{
    public class refreshService: RefreshInterface
    {
        private readonly IProviderRepository _providerRepository;
        private readonly string _jwtKey;
        private readonly IAdminRepository _adminRepository;
        private readonly IAuthRepository _authRepository;
        public refreshService(IProviderRepository providerRepository, IAdminRepository adminRepository, IAuthRepository authRepository)
        {
            _providerRepository = providerRepository;
            _jwtKey = Environment.GetEnvironmentVariable("JWT_KEY");
            _adminRepository = adminRepository;
            _authRepository = authRepository;
        }
        public async Task<ProviderLoginResponse> GetRefreshToken(string refreshToken)
        {
            try
            {
                var provider = await _providerRepository.GetUserByRefreshTokenAsync(refreshToken);

                var admin = await _adminRepository.GetUserByRefreshTokenAsync(refreshToken);
                var user = await _authRepository.GetUserByRefreshTokenAsync(refreshToken);
                if (provider != null)
                {
                    var tokens = new ProviderToken();
                    var newAccessToken = tokens.CreateTokenProvider(provider);

                    var tokenHelper = new TokenHelper();
                    var newRefreshToken = tokenHelper.GenerateRefreshToken(provider);

                    //update
                    provider.refresh_token = newRefreshToken;
                    provider.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
                    provider.updated_at = DateTime.UtcNow;

                    return new ProviderLoginResponse
                    {
                        id = provider.id,
                        email = provider.email,
                        token = newAccessToken,
                        refresh_token = newRefreshToken,
                    };
                }

                //admin:
                else if (admin != null)
                {

                    var tokens = new AdminToken();
                    var newAccessToken = tokens.CreateAdminToken(admin);

                    var tokenHelper = new TokenHelper();
                    var newRefreshToken = tokenHelper.GenerateRefreshTokenAdmin(admin);

                    //update
                    admin.refresh_token = newRefreshToken;
                    admin.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
                    admin.updated_at = DateTime.UtcNow;

                    return new ProviderLoginResponse
                    {
                        id = admin.id,
                        email = admin.email,
                        token = newAccessToken,
                        refresh_token = newRefreshToken,
                    };

                }
                else if (user != null)
                {

                    var tokens = new UserToken();
                    var newAccessToken = tokens.GenerateJwtToken(user);

                    var tokenHelper = new TokenHelper();
                    var newRefreshToken = tokenHelper.GenerateRefreshTokenUser(user);

                    //update
                    user.refresh_token = newRefreshToken;
                    user.refreshtoken_expiryDate = DateTime.UtcNow.AddDays(7);
                    user.updated_at = DateTime.UtcNow;

                    return new ProviderLoginResponse
                    {
                        id = user.id,
                        email = user.email,
                        token = newAccessToken,
                        refresh_token = newRefreshToken,
                    };

                }

                else
                {
                    throw new Exception("Invalid or expired refresh token.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}");
            }
        }
       
    }
}
