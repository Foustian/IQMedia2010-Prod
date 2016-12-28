using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Domain;
using System.Net;
using System.IO;
using System.Configuration;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.Logic
{
    public class RawMediaLogic : BaseLogic, ILogic
    {

        public IQAgentTVResults GetIQagentTVResultsByID(Int64? ID)
        {
            IQAgentTVResults iQAgentTVResults = Context.GetIQagent_TVResults_ByID(ID).FirstOrDefault();
            return iQAgentTVResults;
        }

        public IQCoreRecordFile GetIQCoreRecordFileLocationByGUID(Guid? rlVideoGuid)
        {
            IQCoreRecordFile iQCoreRecordFile = Context.GetIQCoreRecordFileLocationByGUID(rlVideoGuid).FirstOrDefault();
            return iQCoreRecordFile;

        }

        public Guid? GetRawMediaGuidByIQAgentIFrameID(Guid iQAgentIframeGuid)
        {
            return (Guid?)Context.GetVideoGuidByiQAgentiFrameID(iQAgentIframeGuid).FirstOrDefault();
        }

        public Guid? GetRecordfileByGuid(Guid guid)
        {
            return Context.GetRecordfileByGuid(guid).FirstOrDefault();
        }

        public String GetOffsetByGetHighlightedCC(Int64? ID)
        {
            try
            {
                Uri _Uri = new Uri(ConfigurationManager.AppSettings["HighLightedCCURL"] + ID);
                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);

                _objWebRequest.Timeout = 100000000;
                _objWebRequest.Method = "GET";

                StreamReader _StreamReader = null;

                string responseHighlightedCC = string.Empty;

                if ((_objWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_objWebRequest.GetResponse().GetResponseStream());
                    responseHighlightedCC = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                return responseHighlightedCC;
            }
            catch (Exception exception)
            {

                throw exception;
            }

        }

        public IQCoreRootPath SelectIQCoreRootPathByID(Int64 iQRootPathID)
        {
            try
            {
                var iQCoreRootPath = (IQCoreRootPath)Context.getIQCoreRootPathByID(iQRootPathID).FirstOrDefault();
                return iQCoreRootPath;
            }
            catch (Exception exception)
            {

                throw exception;
            }


        }

        public string GetServiceURLByRootPathID(Int64 rootPathID)
        {
            try
            {
                string remoteSvcUrl = Context.GetRemoteSvcUrlByRootPathID(rootPathID).FirstOrDefault();
                return remoteSvcUrl;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public string RemoteThumbGenService(string ServiceURL, Int64? Offset, string Location, Guid? RawMediaGuid, string FinalLocation)
        {
            string _Response = string.Empty;
            try
            {
                ServiceURL = ServiceURL + ConfigurationManager.AppSettings["RemoteGenerateThumbGenServiceURL"] + "?Offset=" + Offset + "&Location=" + Location + "&RawMediaGuid=" + RawMediaGuid + "&FinalLocation=" + FinalLocation;
                Log4NetLogger.Debug("Remote Service Final URL :" + ServiceURL);

                Uri _Uri = new Uri(ServiceURL);


                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(ServiceURL);

                /* _objWebRequest.Timeout = int.Parse(ConfigurationManager.AppSettings["RemoteCallTimeOut"]);*/
                _objWebRequest.ContentType = "application/xml";
                _objWebRequest.Method = "GET";
                System.Text.ASCIIEncoding _objEncodedData = new System.Text.ASCIIEncoding();
                //byte[] byteArray = _objEncodedData.GetBytes(ClipXML.ToString());

                /*_objWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _objWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();*/

                StreamReader _StreamReader = null;

                if ((_objWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_objWebRequest.GetResponse().GetResponseStream());
                    _Response = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }
                return _Response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
