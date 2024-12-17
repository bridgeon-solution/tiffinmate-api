using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.CloudinaryInterface;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.BLL.Services.CoudinaryService;
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
        private readonly ICloudinaryService _cloudinary;
        private readonly IMapper _mapper;
        public UserService(IAuthRepository authRepository, IUserRepository userRepository, AppDbContext appDbContext, ICloudinaryService cloudinary,IMapper mapper)

        {
            _authRepository = authRepository;
            _userRepository = userRepository;
            _appDbContext = appDbContext;
            _cloudinary = cloudinary;
            _mapper = mapper;
        }
        public async Task<List<User>> GetAllUsers()
        {
            var user= await _userRepository.GetUsers();
            return user;
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

        public async Task<UserProfileDto> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return null;
            }

            return new UserProfileDto
            {
                name = user.name,
                phone = user.phone,
                email = user.email
            };
        }

        public async Task<string> UploadImage(IFormFile image)
        {
           
            var imageUrl = await _cloudinary.UploadDocumentAsync(image);
            return imageUrl;
        }
        public async Task<string> UpdateUser(Guid id, UserProfileDto reqDto)
        {
            var user = await _userRepository.GetUserById(id);
            if (user == null)
            {
                return null;
            }

            user.name = reqDto.name;
            user.phone = reqDto.phone;
            user.email = reqDto.email;
            user.address = reqDto.address;
            user.city = reqDto.city;
            user.updated_at = DateTime.UtcNow;

           
            if (!string.IsNullOrEmpty(reqDto.image))
            {
                
                user.image = reqDto.image;
            }

            await _userRepository.UpdateUser(user);
            return "Updated successfully";
        }



    }

}

