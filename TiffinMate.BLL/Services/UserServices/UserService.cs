using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Interfaces.UserInterfaces;
using TiffinMate.DAL.Interfaces.UserRepositoryInterface;

namespace TiffinMate.BLL.Services.UserServices
{
    public class UserService:IUserService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IUserRepository _userRepository;
        private readonly AppDbContext _appDbContext;
        public UserService(IAuthRepository authRepository, IUserRepository userRepository, AppDbContext appDbContext)

        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _appDbContext = appDbContext;
        }
        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetUsers();
        }
        public async Task<BlockUnblockResponse> BlockUnblock(Guid id)
        {
            var user = await _userRepository.BlockUnblockUser(id);
            if (user != null)
            {
                user.is_blocked = !user.is_blocked;
                _appDbContext.SaveChanges();
                return new BlockUnblockResponse
                {
                    is_blocked = user.is_blocked == true ? true : false,
                    message = user.is_blocked == true ? "user is blocked" : "user is unblocked"
                };
            }

            return new BlockUnblockResponse
            {
                message = "invalid user"
            };
        }

        public async Task<UserResponseDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            return new UserResponseDto
            {
                name = user.name,
                phone = user.phone,
                email = user.email
            };
        }


    }

    }

