using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.TVEyes.Domain;
using IQMedia.TVEyes.Common.Util;

namespace IQMedia.TVEyes.Logic
{
    public class TVEyesLogic : BaseLogic, ILogic
    {
        public string GetRootPathByRootPathID(Int64 p_RootPathID)
        {
            try
            {
                RootPath _rootpath = Context.GetRootPathByID(p_RootPathID).FirstOrDefault();
                if (_rootpath != null)
                {
                    return _rootpath.StoragePath;
                }
                else
                {
                    Logger.Warning("root path does not exist for root path id :" + p_RootPathID);
                    return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Logger.Error("error occred while fetching root path location", ex);
                throw;
            }
        }
    }
}
