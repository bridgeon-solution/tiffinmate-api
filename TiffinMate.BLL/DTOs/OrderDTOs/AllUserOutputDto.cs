using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class AllUserOutputDto
    {
        public int TotalCount { get; set; }
        public List<AllUsersDto> AllUsers { get; set; }
    }
}
