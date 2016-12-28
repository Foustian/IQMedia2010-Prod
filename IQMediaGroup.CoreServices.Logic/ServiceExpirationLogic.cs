using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Logic
{
    public class ServiceExpirationLogic : BaseLogic, ILogic
    {
        public List<ServiceExpiration> GetServiceExpirationList(string rpSiteID, int? numRecord, bool isRemoteLocation)
        {
            try
            {
                var serviceExpiration = Context.GetServiceExpirationList(numRecord, rpSiteID, isRemoteLocation).ToList();
                return (List<ServiceExpiration>)serviceExpiration;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching service expiration List By rpsite ID:" + rpSiteID + " and numrecord: " + numRecord, ex);
                throw;
            }
        }

        public bool UpdateIQServiceExpirationStatus(string status, Guid? recordFileGuid)
        {
            try
            {
                Int32? returnValue = Context.UpdateIQServiceExpirationStatus(status, recordFileGuid).FirstOrDefault().Value;
                if (returnValue.HasValue && returnValue.Value > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while Updating Recodfile status By RecordFile GUID :" + recordFileGuid + " with status:" + status, ex);
                throw;
            }

        }
    }
}
