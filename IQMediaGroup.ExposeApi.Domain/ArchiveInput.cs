using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class ArchiveInput
    {
        public Guid? Category { get; set; }

        public bool FilterOnCreatedDateTime { get; set; }

        public DateTime? FromDateTime { get; set; }        

        public Int64? SeqID { get; set; }

        public string SessionID { get; set; }
        
        public DateTime? ToDateTime { get; set; }

        public int? Rows { get; set; }

        public string MediaType { get; set; }

        public string SubMediaTypeEnum { get{return !string.IsNullOrWhiteSpace(MediaType)?CommonFunctions.GetValueFromDescription<IQMediaGroup.Common.Util.CommonConstants.SubMediaType>(MediaType):null; } }
    }  
}
