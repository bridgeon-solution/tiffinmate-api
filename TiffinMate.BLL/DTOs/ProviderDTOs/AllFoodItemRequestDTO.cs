using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class AllFoodItemRequestDTO
    {
        public List<Guid> categories { get; set; }
        public Guid menu_id { get; set; }

    }
}
