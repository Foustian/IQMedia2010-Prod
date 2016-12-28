using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Core.HelperClasses
{
    public class IQClient_CustomSettings
    {
        public int IQCustom_ClientID { get; set; }

        public Guid ClientGUID { get; set; }

        public string IQAdvanceSettings { get; set; }

        public bool IsActive { get; set; }
    }
}
