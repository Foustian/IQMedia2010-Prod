using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Logic;
using IQMediaGroup.Domain;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMedia.Web.Common;
using System.Text.RegularExpressions;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class LogRawMediaPlay : ICommand
    {
        private Guid? _rawMediaGuid;
        private readonly string _referrer;
        private String _EncryptedID;
        private readonly string _host;
        public LogRawMediaPlay(object rawMediaGuid, object refferer,object host)
        {
            if (rawMediaGuid is NullParameter)
            {
                _rawMediaGuid = null;
            }
            else
            {
                Guid originalGuid = Guid.Empty;
                if (Guid.TryParse(Convert.ToString(rawMediaGuid), out(originalGuid)))
                {
                    _rawMediaGuid = originalGuid;
                }
                else
                {
                    _EncryptedID = Convert.ToString(rawMediaGuid);
                }
            }


            _referrer = (refferer is NullParameter) ? String.Empty : (string)refferer;
            _host = host is NullParameter ? string.Empty : (string)host;
        }

        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {

            CLogger.Info("LogClipPlay Service Started");
            CLogger.Debug("Client's IP :" + _host);

            var rawMediaLogic = (RawMediaLogic)LogicFactory.GetLogic(LogicType.RawMedia);

            var output = new LogRawMediaPlayOutput();

            try
            {
                //Throw an exception if we don't have a valid Guid
                if (_rawMediaGuid == null && string.IsNullOrWhiteSpace(_EncryptedID))
                {
                    throw new ArgumentException("Invalid or missing Raw Media Guid.");
                }

                if (!string.IsNullOrWhiteSpace(_EncryptedID))
                {
                    _rawMediaGuid = rawMediaLogic.GetRawMediaGuidByIQAgentIFrameID(new Guid(IQMediaGroup.Common.Util.CommonFunctions.AesDecryptStringFromBytes(_EncryptedID)));
                }
                    

                var rawmedia = rawMediaLogic.GetRecordfileByGuid(_rawMediaGuid.Value);
                //If we don't have a raw media, we can't do anything else so we fail and return.
                if (rawmedia == null)
                    throw new ArgumentNullException(String.Format("The rawmedia '{0}' does not exist.", _rawMediaGuid.Value));

                bool logPlays;
                Boolean.TryParse(ConfigurationManager.AppSettings["LogRawMediaPlays"], out logPlays);
                if (logPlays)
                {
                    string VisitorsIPAddr = string.IsNullOrEmpty(_host) ? !string.IsNullOrEmpty(HttpContext.Current.Request.Headers["X-ClientIP"]) ? HttpContext.Current.Request.Headers["X-ClientIP"] : HttpContext.Current.Request.UserHostAddress : _host;

                    string devicename = string.Empty;
                    string os = string.Empty;
                    if (!string.IsNullOrEmpty(HttpContext.Current.Request.UserAgent))
                    {

                        CLogger.Info("user agent : " + HttpContext.Current.Request.UserAgent.ToLower());

                        foreach (string strregex in IQMediaGroup.Common.Util.CommonFunctions.osList)
                        {
                            Regex regex = new Regex(strregex);
                            if (regex.IsMatch(HttpContext.Current.Request.UserAgent.ToLower()))
                            {
                                Match m = regex.Match(HttpContext.Current.Request.UserAgent.ToLower());
                                os = m.Value;
                                break;
                            }
                        }

                        foreach (var s in IQMediaGroup.Common.Util.CommonFunctions.deviceList)
                        {
                            Regex regex = new Regex(s.Key.ToLower());
                            if (regex.IsMatch(HttpContext.Current.Request.UserAgent.ToLower()))
                            {
                                devicename = s.Value;
                                break;
                            }
                        }
                    }

                    var trackLgc = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
                    output.PlayLogID = trackLgc.InsertPlay(rawmedia.Value, Authentication.CurrentUser.Guid, VisitorsIPAddr, _referrer, devicename, os);

                    //NOTE: We added this here since it used to be in previewImage and that call is (temporarily?) depricated
                    //Even tho Lakshmi doesn't care about impressions, it wouldn't make sense that a clip have more plays than
                    //impressions so we log the impression when we log a play so that way it maintains a 1 to 1 relationship at least
                    trackLgc.InsertImpression(rawmedia.Value, Authentication.CurrentUser.Guid, VisitorsIPAddr);

                    output.Status = 1;
                }
                else
                {
                    output.Status = 0;
                    output.Message = "Logging disabled.";
                }
            }
            catch (Exception ex)
            {
                CLogger.Error("LogClipPlay()", ex);
                output.Status = -1;
                output.Message = "Error";
            }

            response.Write(Serializer.Searialize(output));
            CLogger.Info("LogClipPlay Service Ended");
        }

        #endregion
    }
}