using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.DTOs.UserDTOs;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ProviderResultDTO
    {
        public int TotalCount { get; set; }
        public List<ProviderResponseDTO> Providers { get; set; }
    }
}
