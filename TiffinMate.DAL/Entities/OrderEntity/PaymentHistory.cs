﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.OrderEntity
{
    public class PaymentHistory
    {
        public Guid id { get; set; }
        public Guid user_id { get; set; }
        public decimal amount { get; set; }
        public DateTime payment_date { get; set; }
        public bool is_paid { get; set; } = false;
        public Guid subscription_id { get; set; }
        public Subscription subscription { get; set; }

    }
}
