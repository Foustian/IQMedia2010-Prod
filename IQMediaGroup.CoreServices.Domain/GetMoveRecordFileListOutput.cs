using System;
using System.Collections.Generic;

namespace IQMediaGroup.CoreServices.Domain
{
    public class GetMoveRecordFileListOutput
    {
        public int Status { get; set; }

        public string Message { get; set; }

        public List<MoveRecordFile> MoveRecordFileList { get; set; }
    }

    public class MoveRecordFile
    {
        public Guid RecordFileGUID { get; set; }

        public string Location{ get; set; }

        public string Status { get; set; }
    }
}