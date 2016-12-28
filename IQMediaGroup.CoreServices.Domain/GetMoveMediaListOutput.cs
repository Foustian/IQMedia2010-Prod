using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMediaGroup.CoreServices.Domain
{
    public class GetMoveMediaListOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<MoveMedia> MoveMediaList { get; set; }
    }
}
