using System;
using IQMediaGroup.Common.Util;
using IQMedia.Services.SMS.Domain;
using System.Reflection;

namespace IQMedia.Services.SMS.Logic
{
    public class CTSMSResultLogic : BaseLogic, ILogic
    {
        public int InsertCTSMSResult()
        {
            //Assembly.Load("IQMedia.Services.SMS.Domain.iqmediagroupEntities");
            var i = Context.InsertIQCTSMSResults("testPhoneNo", DateTime.Now, "STOP", "12345");

            return 1;
        }
    }
}