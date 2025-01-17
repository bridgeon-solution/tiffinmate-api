using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.DAL.Entities.OrderEntity;

namespace TiffinMate.BLL.Interfaces
{
    public interface IBillingService
    {
        Task SendMonthlyBills();
        Task<bool> SendBillEmail(string to, PaymentHistory invoice);
    }
}
