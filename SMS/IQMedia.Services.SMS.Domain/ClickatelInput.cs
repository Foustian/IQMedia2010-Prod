using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Services.SMS.Domain
{
    public class ClickatelInput
    {
        public string CustomerPhoneNo { get; set; }
        public DateTime? ReceivedDateTime { get; set; }
        public string MsgText { get; set; }
        public string MesssagId { get; set; }
    }
}
