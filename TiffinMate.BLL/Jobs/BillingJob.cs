using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiffinMate.BLL.Interfaces;

namespace TiffinMate.BLL.Jobs
{
    public class BillingJob: IJob
    {
        private readonly IBillingService _billingService;

        public BillingJob(IBillingService billingService)
        {
            _billingService = billingService;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            await _billingService.SendMonthlyBills();
        }
    }
  
}
