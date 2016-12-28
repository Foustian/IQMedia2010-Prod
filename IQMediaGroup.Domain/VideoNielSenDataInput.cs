using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain
{
    public class VideoNielSenDataInput
    {
        public Guid? Guid { get; set; }
        public Guid? ClientGuid { get; set; }
        public Boolean? IsRawMedia { get; set; }
        public int? IQ_Start_Point { get; set; }
        public string IQ_Dma_Num { get; set; }
    }
}
