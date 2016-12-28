using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public class StationLogic : BaseLogic, ILogic
    {

        //public Boolean SelectStationSharingByClipIDNClientGUID(Guid? clipID, Guid? customerGUID)
        public Boolean SelectStationSharingByClipIDNClientGUID(Guid? clipID,Guid clientGuid,Guid customerGuid)
        {
            try
            {
                //var result = Context.GetStationSharingByClipIDNClientGUID(clipID, customerGUID).FirstOrDefault().Value;
                var result = Context.GetStationSharingByClipIDNClientGUID(clipID,clientGuid,customerGuid).FirstOrDefault();

                Log4NetLogger.Info("First Result: " + result);

                return (Boolean)result.Value;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
