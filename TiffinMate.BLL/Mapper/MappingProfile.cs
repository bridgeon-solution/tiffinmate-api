using AutoMapper;
using Supabase.Gotrue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;

namespace TiffinMate.BLL.Mapper
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,RegisterUserDto>().ReverseMap();
        }
    }
}
