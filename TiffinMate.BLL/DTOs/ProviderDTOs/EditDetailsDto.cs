using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.ProviderEntity;

namespace TiffinMate.BLL.DTOs.ProviderDTOs
{
    public class EditDetailsDto
    {
        public Guid provider_id { get; set; }
        public string username { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public string phone_no { get; set; }


    }
}
