using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices.Logic
{
    public class RootPathLogic : BaseLogic, ILogic
    {

        public ActiveRootPathOutput GetActiveRootPath(string ipAddress)
        {
            try
            {
                var activeRootPathOutput = new ActiveRootPathOutput();
                var iqCore_RootPath = (List<RootPath>)Context.GetActiveRootPathByIP(ipAddress).ToList();
                if (iqCore_RootPath != null && iqCore_RootPath.Count > 0)
                {
                    activeRootPathOutput.message = iqCore_RootPath.Count.ToString() + " record(s) found.";
                    activeRootPathOutput.status = 0;
                    activeRootPathOutput.rootpaths = iqCore_RootPath;
                }
                else
                {
                    activeRootPathOutput.message = "No record found.";
                    activeRootPathOutput.status = 0;
                }
                return activeRootPathOutput;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching active roothpaths ipAddress:" + ipAddress, ex);
                throw ex;

            }
            
        }

        public UpdateRootPathStatusOutput UpdateRootPathStatus(Int64 rootpathID, Boolean status)
        {
            try
            {
                var updateRootPathStatusOutPut = new UpdateRootPathStatusOutput();
                var result = Context.UpdateRootPathStatus(rootpathID, status).SingleOrDefault();
                if (result != null && result > 0)
                {
                    updateRootPathStatusOutPut.message = "Record updated successfully.";
                    updateRootPathStatusOutPut.status = 0;

                }
                else
                {
                    updateRootPathStatusOutPut.message = "Record couldn't updated.";
                    updateRootPathStatusOutPut.status = 1;
                }
                return updateRootPathStatusOutPut;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while updating RootPath rootpathID:" + rootpathID, ex);
                throw ex;

            }
            
        }
    }
}
