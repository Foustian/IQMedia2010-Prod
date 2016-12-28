using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Logic.IOS
{
    public class UGCUploadLogic : BaseLogic,ILogic
    {
        public Int64 InsertUGCUpload(UGCUploadInput p_UGCUploadInput)
        {
            try
            {
                decimal? output = Context.InsertIOSUGCUpload(p_UGCUploadInput.FileName,
                                           p_UGCUploadInput.Title,
                                           p_UGCUploadInput.Description,
                                           p_UGCUploadInput.Keywords,
                                           p_UGCUploadInput.StartTime,
                                           p_UGCUploadInput.EndTime,
                                           p_UGCUploadInput.UUID).FirstOrDefault();

                return output.HasValue ? Convert.ToInt64(output.Value) : 0;
            }
            catch(Exception ex)
            {
                Log4NetLogger.Error("Error Occured while inserting ugc upload into db: " + ex.ToString() + " stack : " + ex.StackTrace);
                throw;
            }


        }
    }
}
