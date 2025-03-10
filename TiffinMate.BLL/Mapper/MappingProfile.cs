﻿using AutoMapper;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.NotificationDTOs;
using TiffinMate.BLL.DTOs.OrderDTOs;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.DAL.Entities.OrderEntity;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.BLL.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Provider, ProviderDTO>().ReverseMap();
            CreateMap<Provider, LoginResponse>().ReverseMap();
            CreateMap<Provider, ProviderLoginDTO>().ReverseMap();
            CreateMap<User, RegisterUserDto>().ReverseMap();
            CreateMap<User, ProviderLoginResponse>().ReverseMap();
            CreateMap<ProviderDetails, ProviderDetailsDTO>().ReverseMap();
            CreateMap<UserResponseDTO, User>().ReverseMap();
            CreateMap<ProviderDetailResponse, ProviderDetails>().ReverseMap();
            CreateMap<ProviderDetailedDTO, ProviderDetails>().ReverseMap();
            CreateMap<MenuDto, Menu>().ReverseMap();
            CreateMap<FoodItem, FoodItemResponceDto>().ReverseMap();
            CreateMap<FoodItem,FoodItemDto>().ReverseMap();
            CreateMap<Categories,CategoryDto>().ReverseMap();
            CreateMap<ProviderResponseDTO,Provider>().ReverseMap();
            CreateMap<MenuRequestDto, Menu>().ReverseMap();
            CreateMap<Order,OrderRequestDTO>().ReverseMap();
            CreateMap<Subscription, OrderRequestDTO>();
            CreateMap<Notification,NotificationAdminResponceDto>().ReverseMap();
            
        }
    }
}

