using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class AllReview
    {
        public Guid id { get; set; } = Guid.NewGuid();
        public Guid ProviderId { get; set; }
        public Guid UserId { get; set; }
        public string review { get; set; }
        public string username { get; set; }
        public string providername { get; set; }
        public string image { get; set; }
    }
}
