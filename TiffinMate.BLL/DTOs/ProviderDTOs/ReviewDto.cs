using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class ReviewDto
    {
        public Guid ProviderId { get; set; }
        public Guid UserId { get; set; }
        public string review { get; set; }
    }
}
