using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Services.SMS.Domain
{
    public class SubscriptionOutput
    {
        public Int16 Status { get; set; }

        public string Message { get; set; }
        
        public string SubscriptionURL { get; set; }
    }
}
