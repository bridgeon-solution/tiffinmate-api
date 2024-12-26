using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class PlanRequest
    {
        public string date { get; set; }
        public List<Guid> categories { get; set; }
    }
}
