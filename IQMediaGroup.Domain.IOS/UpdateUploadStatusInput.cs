using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.Domain.IOS
{
    public class UpdateUploadStatusInput
    {
        public Int64? ID { get; set; }

        public string Status { get; set; }

        public string Comments { get; set; }
    }
}
