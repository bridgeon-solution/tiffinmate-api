﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TiffinMate.DAL.Entities.ProviderEntity
{
    public class Notification:AuditableEntity
    {
        public int id { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string recipient_type { get; set; }
        public string? recipient_id { get; set; }
        public string notification_type { get; set; }
        public bool isread { get; set; } = false;
    }
}
