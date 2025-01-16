using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.OrderEntity
{
    public class Invoice
    {
        public Guid UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime InvoiceDate { get; set; }
        public string InvoiceDetails { get; set; }
    }
}
