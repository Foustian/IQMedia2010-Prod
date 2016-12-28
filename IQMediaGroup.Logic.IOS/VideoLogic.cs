using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Logic.IOS
{
    public class VideoLogic : BaseLogic, ILogic
    {
        public UploadVideoOutput InsertVideoUpload(UploadVideoInput P_UploadVideoInput)
        {
            try
            {
                UploadVideoOutput uploadVideoOutput = new UploadVideoOutput();

                var uploadVideoID = Context.InsertUploadTracking(P_UploadVideoInput.UID, P_UploadVideoInput.FileName, P_UploadVideoInput.CatID, P_UploadVideoInput.Tags, P_UploadVideoInput.DT, P_UploadVideoInput.TimeZone, P_UploadVideoInput.VideoDT).FirstOrDefault();
                if (uploadVideoID.HasValue && uploadVideoID.Value > 0)
                {
                    uploadVideoOutput.ID = uploadVideoID.Value;
                    uploadVideoOutput.Status = 0;
                    uploadVideoOutput.Message = "Success";
                }
                else if (uploadVideoID == -1)
                {
                    uploadVideoOutput.Status = 1;
                    uploadVideoOutput.Message = "Invalid UniqueID for application";
                }
                else
                {
                    uploadVideoOutput.Status = 2;
                    uploadVideoOutput.Message = "Insert for upload video failed";
                }
                return uploadVideoOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw ex;
            }
        }

        public UpdateUploadStatusOutput UpdateUploadTrackingStatus(UpdateUploadStatusInput P_UpdateUploadStatusInput)
        {
            try
            {
                UpdateUploadStatusOutput updateUploadStatusOutput = new UpdateUploadStatusOutput();

                var uploadVideoID = Context.UpdateUploadTrackingStatus(P_UpdateUploadStatusInput.ID, P_UpdateUploadStatusInput.Status, P_UpdateUploadStatusInput.Comments).FirstOrDefault();
                if (uploadVideoID.HasValue && uploadVideoID.Value > 0)
                {
                    updateUploadStatusOutput.Status = 0;
                    updateUploadStatusOutput.Message = "Success";
                }
                else 
                {
                    updateUploadStatusOutput.Status = 1;
                    updateUploadStatusOutput.Message = "Update status for upload tracking failed";
                }
                
                return updateUploadStatusOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(ex);
                throw ex;
            }
        }
    }
}
