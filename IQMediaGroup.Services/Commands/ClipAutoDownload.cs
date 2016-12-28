using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.Web.Common;
using System.Security.Authentication;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using System.Configuration;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    

    public class ClipAutoDownload : ICommand
    {
        #region ICommand Members

        public Guid? _ClipID { get; private set; }
        public Guid? _ClientGUID { get; private set; }
        public Guid? _CustomerGUID { get; private set; }
        public Guid? _CategoryGUID { get; private set; }

        public ClipAutoDownload(object ClipID, object CategoryGUID, object ClientGUID, object CustomerGUID)
        {
            _ClipID = (ClipID is NullParameter) ? null : (Guid?)ClipID;
            _ClientGUID = (ClientGUID is NullParameter) ? null : (Guid?)ClientGUID;
            _CustomerGUID = (CustomerGUID is NullParameter) ? null : (Guid?)CustomerGUID;
            _CategoryGUID = (CategoryGUID is NullParameter) ? null : (Guid?)CategoryGUID;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            
            try
            {
                CLogger.Debug("Clip Auto Download Request Started");
                if (!Authentication.IsAuthenticated)
                    throw new AuthenticationException();

                if (_ClipID==null)
                {
                    throw new ArgumentException("Invalid or missing ClipID");
                }

                if (_ClientGUID==null)
                {
                    throw new ArgumentException("Invalid or missing ClientGUID");
                }

                if (_CustomerGUID==null)
                {
                    throw new ArgumentException("Invalid or missing CustomerGUID");
                }

                if (_CategoryGUID==null)
                {
                    throw new ArgumentException("Invalid or missing CategoryGUID");
                }

                if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                {
                    CLogger.Debug("{\"ClipID\":\"" + _ClipID + "\",\"ClientGUID\":\"" + _ClientGUID + "\",\"CustomerGUID\":\"" + _CustomerGUID + "\",\"CategoryGUID\":\"" + _CategoryGUID + "\"}");
                }

                ClipDownloadLogic ClipDownloadLogic = (ClipDownloadLogic)LogicFactory.GetLogic(LogicType.ClipDownload);
                string  Result=ClipDownloadLogic.DownloadClip(_ClipID.Value, _ClientGUID.Value, _CustomerGUID.Value, _CategoryGUID.Value);

                _JSONResult = Serializer.Searialize(Result);
            }
            catch (ArgumentException ArgumentException)
            {
                _JSONResult = Serializer.Searialize(ArgumentException.Message);
            }
            catch (AuthenticationException)
            {
                _JSONResult = Serializer.Searialize("User is not authenticated.");
            }
            catch (CustomException _CustomException)
            {
                _JSONResult = Serializer.Searialize(_CustomException.Message);
            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace + "inner exception :" + ex.InnerException);
                _JSONResult = Serializer.Searialize("An error occurred, please try again");
            }
            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_JSONResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";

            CLogger.Debug("Clip Auto Download Request Ended");
            HttpResponse.Output.Write(_JSONResult);
            
        }

        #endregion
    }
}