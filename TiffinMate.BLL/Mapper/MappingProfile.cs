using AutoMapper;
<<<<<<< HEAD
=======
using Supabase.Gotrue;
>>>>>>> c712c6ee4897cb2d2db8161b99f35c3817284a5d
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
<<<<<<< HEAD
using TiffinMate.BLL.DTOs.ProviderDTOs;
using TiffinMate.DAL.Entities.ProviderEntity;
=======
using TiffinMate.BLL.DTOs.UserDTOs;
>>>>>>> c712c6ee4897cb2d2db8161b99f35c3817284a5d

namespace TiffinMate.BLL.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
<<<<<<< HEAD
            CreateMap<Provider, ProviderDTO>().ReverseMap();
            CreateMap<Provider, LoginResponse>().ReverseMap();
            CreateMap<Provider, ProviderLoginDTO>().ReverseMap();


=======
            CreateMap<User,RegisterUserDto>().ReverseMap();
>>>>>>> c712c6ee4897cb2d2db8161b99f35c3817284a5d
        }
    }
}
