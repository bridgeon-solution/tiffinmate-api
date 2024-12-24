using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.UserDTOs
{
    public class UserResultDTO
    {
        public int TotalCount { get; set; }
        public List<UserResponseDTO> Users { get; set; }
    }
}
