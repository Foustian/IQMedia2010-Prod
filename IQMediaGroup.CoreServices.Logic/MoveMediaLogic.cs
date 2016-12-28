using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.CoreServices.Domain;

namespace IQMediaGroup.CoreServices.Logic
{
    public class MoveMediaLogic : BaseLogic, ILogic
    {
        public List<MoveRecordFile> GetMoveMediaByRPID(int p_RPID, DateTime p_FromDate, DateTime p_ToDate, int p_NumRecords)
        {
            return Context.GetMoveRecordFileByRPID(p_RPID, p_FromDate, p_ToDate, p_NumRecords);
        }

        public Int64 InsertMoveMedia(MoveMedia p_mm, out int p_OutStatus)
        {
            return Context.InsertMoveMedia(p_mm, out p_OutStatus);
        }

        public int UpdateMoveMedia(MoveMedia p_mm, bool p_UpdateOrigin)
        {
            return Context.UpdateMoveMedia(p_mm, p_UpdateOrigin);
        }

        public List<MoveMedia> GetMoveMedia(GetMoveMediaListInput p_mm)
        {
            return Context.GetMoveMedia(p_mm);
        }
    }
}
