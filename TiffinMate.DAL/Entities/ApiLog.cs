using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities
{
    public class ApiLog
    {
        public int Id { get; set; }
        public string HttpMethod { get; set; }
        public string RequestPath { get; set; }
        public string QueryString { get; set; }
        public string RequestBody { get; set; }
        public int ResponseStatusCode { get; set; }
        public string ResponseBody { get; set; }
        public DateTime LoggedAt { get; set; }
        public long TimeTaken { get; set; }

    }
}
