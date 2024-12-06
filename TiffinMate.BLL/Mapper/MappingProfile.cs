using AutoMapper;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.BLL.DTOs.UserDTOs;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.BLL.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Provider, ProviderDTO>().ReverseMap();
            CreateMap<Provider, LoginResponse>().ReverseMap();
            CreateMap<Provider, ProviderLoginDTO>().ReverseMap();
            CreateMap<User,RegisterUserDto>().ReverseMap();
        }
    }
}
