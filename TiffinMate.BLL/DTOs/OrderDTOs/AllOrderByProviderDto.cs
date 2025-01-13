using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.ProviderDTOs;

namespace TiffinMate.BLL.DTOs.OrderDTOs
{
    public class AllOrderByProviderDto
    {
        public int TotalCount { get; set; }
        public List<GetOrderDetailsDto> Allorders { get; set; }
    }
}
