using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class PaginationReview
    {
        public int TotalCount { get; set; }
        public List<AllReview> reviews { get; set; }
    }
}
