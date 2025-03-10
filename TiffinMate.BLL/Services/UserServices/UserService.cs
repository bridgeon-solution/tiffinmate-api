﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.BLL.Interfaces.CloudinaryInterface;
using TiffinMate.BLL.Interfaces.UserInterfaces;
using TiffinMate.BLL.Services.CoudinaryService;
using TiffinMate.DAL.DbContexts;
using TiffinMate.DAL.Entities;
using TiffinMate.DAL.Entities.OrderEntity;
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
        
        public async Task<BlockUnblockResponse> BlockUnblock(Guid id)
        {
            var user = await _userRepository.BlockUnblockUser(id);
            if (user != null)
            {   

                user.is_blocked = !user.is_blocked;
                user.updated_at = DateTime.UtcNow;
                _appDbContext.SaveChanges();
                return new BlockUnblockResponse
                {   
                    is_blocked = user.is_blocked == true ? true : false,
                    message = user.is_blocked == true ? "user is blocked" : "user is unblocked",
                  
                    
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
                email = user.email,
                address=user.address,
                city=user.city,
                image= user.image,
                is_blocked=user.is_blocked,
                subscription_status=user.subscription_status,
                created_at=user.created_at
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
        public async Task<UserResultDTO> GetUsers(int page, int pageSize, string search = null, string filter = null)
        {
            var allUsers = (await _userRepository.GetUsers()).AsQueryable();
           

           
            if (!string.IsNullOrEmpty(search))
            {
                allUsers = allUsers.Where(u =>
                    u.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                    u.email.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(filter))
            {
                if (filter == "true")
                {
                    allUsers = allUsers.Where(u => u.is_blocked);
                }
                else if (filter == "false")
                {
                    allUsers = allUsers.Where(u => !u.is_blocked);
                }
            }
            var totalCount = allUsers.Count();


            var pagedUsers = allUsers
                .OrderByDescending(u => u.created_at)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var userResponseList = pagedUsers.Select(u => new UserResponseDTO
            {
                id = u.id,
                name = u.name,
                email = u.email,
                is_blocked = u.is_blocked,
                updated_at = u.updated_at,
                created_at = u.created_at,
                subscription_status = u.subscription_status,
            }).ToList();

            return new UserResultDTO
            {
                TotalCount = totalCount,
                Users = userResponseList
            };
        }

        public async Task<List<AllUserOutputDto>> UsersLists(Guid ProviderId, int page, int pageSize, string search = null)
        {
            // Fetch orders and subscriptions
            var orders = (await _userRepository.GetOrdersByProvider(ProviderId)).ToList();

            if (!string.IsNullOrEmpty(search))
            {
                orders = orders.Where(u =>
                    (u.Order != null && (u.Order.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                         u.Order.user.email.Contains(search, StringComparison.OrdinalIgnoreCase))) ||
                    (u.Subscription != null && (u.Subscription.user.name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                                u.Subscription.user.email.Contains(search, StringComparison.OrdinalIgnoreCase)))
                ).ToList();
            }

            
            var allUsers = orders
                .GroupBy(o => o.Order?.user_id ?? o.Subscription?.user_id)
                .Select(g => g.First()) 
                .Select(o => new AllUsersDto
                {
                    user_name = o.Order?.user.name ?? o.Subscription?.user.name,
                    address = o.Order?.user.address ?? o.Subscription?.user.address,
                    city = o.Order?.user.city ?? o.Subscription?.user.city,
                    ph_no = o.Order?.user.phone ?? o.Subscription?.user.phone,
                    image = o.Order?.user.image ?? o.Subscription?.user.image,
                    email = o.Order?.user.email ?? o.Subscription?.user.email,
                    user_id = (o.Order?.user_id ?? o.Subscription?.user_id).GetValueOrDefault(),

                  
                    payment_history = o.Order?.user.payment_history?
    .Select(u => new PaymentHistoryDto
    {
        amount = u.amount,
        user_name = u.user.name,
        payment_date = u.payment_date,
        is_paid = u.is_paid
    }).ToList() ?? new List<PaymentHistoryDto> () ,

                   
                    order = o.Order?.user.order?
                        .Select(u => new orderDataDto
                        {
                           order_id=u.id,
                           menu_id=u.menu_id,
                            start_date=u.start_date,
                         total_price=u.total_price,
                           transaction_id=u.transaction_id,
                          order_string=u.order_string
                         }).ToList() ?? new List<orderDataDto>(),
                    subscription=o.Order?.user.subscription?
                    .Select(u=>new subscriptionDataDto
                    {
                        sub_id = u.id,
                        menu_id = u.menu_id,
                        start_date = u.start_date,
                        total_price = u.total_price,
                        transaction_id = u.transaction_id,
                        order_string = u.order_string
                    }).ToList()?? new List<subscriptionDataDto>()
                })
                
                .ToList();

            
            var totalCount = allUsers.Count;
            var pagedOrders = allUsers.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new AllUserOutputDto
            {
                TotalCount = totalCount,
                AllUsers = pagedOrders
            };

            return new List<AllUserOutputDto> { result };
        }





    }

}

