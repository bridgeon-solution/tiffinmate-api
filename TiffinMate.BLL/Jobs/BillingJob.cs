using Microsoft.Extensions.Logging;
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
        private readonly ILogger<BillingJob> _logger;
        public BillingJob(IBillingService billingService, ILogger<BillingJob> logger)
        {
            _billingService = billingService;
            _logger = logger;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("BillingJob starting at: {time}", DateTime.Now);
            try
            {
                await _billingService.SendMonthlyBills();
                _logger.LogInformation("BillingJob completed successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in BillingJob");
                throw;
            }
        }

    }
  
}
