using System;
using IQMediaGroup.Common.Util;
using IQMedia.Services.SMS.Domain;
using System.Linq;

namespace IQMedia.Services.SMS.Logic
{
    public class ClickatellLogic : BaseLogic, ILogic
    {
        public bool InsertClickatell(ClickatelInput p_ClickatelInput)
        {
            try
            {
                Log4NetLogger.Info("going to insert for Clickatell Service");

                long? ID = Context.InsertIQCTSMSResults(p_ClickatelInput.CustomerPhoneNo, p_ClickatelInput.ReceivedDateTime, p_ClickatelInput.MsgText, p_ClickatelInput.MesssagId).FirstOrDefault();
                if (ID != null && ID > 0)
                {
                    return true;
                }
                else
                {
                    Log4NetLogger.Warning("Clickatell request not inserted");
                }
                return false;
                

            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
