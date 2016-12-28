using System;
using IQMediaGroup.Domain.CaptureLog;


namespace IQMediaGroup.Logic.CaptureLog
{
    public class CapturesvcLogic : BaseLogic, ILogic
    {
        public string InsertCaptureLog(string CaptureData, string Status, string Source)
        {
            try
            {
                string response = "CaptureLog Added Successfully";

                CaptureFTPLogging _CaptureFTPLogging = new CaptureFTPLogging();
                _CaptureFTPLogging.CaptureData = CaptureData;
                _CaptureFTPLogging.CreatedDate = DateTime.Now;
                _CaptureFTPLogging.Source = Source;
                _CaptureFTPLogging.Status = Status;

                Context.CaptureFTPLoggings.AddObject(_CaptureFTPLogging);

                Context.SaveChanges();
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
