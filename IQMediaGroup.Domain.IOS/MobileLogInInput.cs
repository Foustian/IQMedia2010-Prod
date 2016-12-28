using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain.IOS
{
    public class MobileLogInInput
    {
        public string UID { get; set; }

        public string UserID { get; set; }

        public string Password { get; set; }

        public string Application { get; set; }

        public string Version { get; set; }
    }
}
