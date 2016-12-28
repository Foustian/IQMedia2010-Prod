using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Domain.IOS;
using IQMediaGroup.Common.IOS;
using System.IO;
using IQMediaGroup.Services.IOS.Web.Serializers;
using IQMediaGroup.Common.IOS.Util;
using System.Configuration;
using IQMediaGroup.Logic.IOS;
using System.Text;


namespace IQMediaGroup.Services.IOS.Web.Commands
{
    public class GetVars : ICommand
    {
        #region ICommand Members


        public string _ClipGUID { get; private set; }
        public string _IOSAppVer { get; private set; }
        public bool? _IsAndroid { get; private set; }
        public string _Callback { get; private set; }



        public GetVars(object ClipID, object IOSAppVersion, object IsAndroid, object callback)
        {
            _ClipGUID = (ClipID is NullParameter) ? null : (string)ClipID;
            _IOSAppVer = (IOSAppVersion is NullParameter) ? null : Convert.ToString(IOSAppVersion);
            _IsAndroid = (IsAndroid is NullParameter) ? null : (bool?)IsAndroid;
            _Callback = (callback is NullParameter) ? null : Convert.ToString(callback);
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _XMLResult = string.Empty;
            IOSGetLocationOutput iOSGetLocationOutput = new IOSGetLocationOutput();


            try
            {
                HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
                HttpResponse.ContentType = "application/xml";

                Log4NetLogger.Info("GetVars Request Started");

                if (string.IsNullOrWhiteSpace(_ClipGUID) || (string.IsNullOrWhiteSpace(_IOSAppVer) && _IsAndroid == null))
                {
                    throw new Exception("Invalid Input");
                }

                Log4NetLogger.Info("ClipGUID: " + _ClipGUID);
                Log4NetLogger.Info("Version: " + _IOSAppVer);
                Log4NetLogger.Info("IsAndroid: " + _IsAndroid);


                /* code to check older Version of IOSPlayer starts */

                if (_IsAndroid!=true)
                {
                    try
                    {
                        Version versionIOSAppVer = new Version(_IOSAppVer);
                        Version versionConfigIOSAppVer = new Version(ConfigurationManager.AppSettings["IOSAppVersion"]);

                        if (versionIOSAppVer < versionConfigIOSAppVer)
                        {

                            Log4NetLogger.Info("Passed Version is old Version");


                            iOSGetLocationOutput.IsOldVersion = true;
                            iOSGetLocationOutput.IOSAppUpdateUrl = ConfigurationManager.AppSettings["IOSAppUpdateUrl"];
                        }
                        else if (versionIOSAppVer > versionConfigIOSAppVer)
                        {
                            Log4NetLogger.Info("Error: Parameter IOSAppVer has new version then Config");
                            throw new Exception();
                        }
                    }
                    catch (ArgumentException argueEx)
                    {
                        Log4NetLogger.Error(argueEx.Message + argueEx.StackTrace);
                        throw new Exception(argueEx.Message);
                    }
                    catch (Exception ex)
                    {
                        Log4NetLogger.Error(ex.Message + ex.StackTrace);
                        throw new Exception(ex.Message);
                    }
                }

                /* code to check older Version of IOSPlayer ends */

                ClipLogic clipLogic = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

                Guid clipGuid;
                if (Guid.TryParse(_ClipGUID, out clipGuid))
                {
                    _XMLResult = clipLogic.GetClipIOSLocation(clipGuid);
                }
                else
                {
                    UTF8Encoding encoding = new UTF8Encoding();
                    byte[] key = encoding.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
                    byte[] iv = encoding.GetBytes(ConfigurationManager.AppSettings["AesIV"]);
                    string strDecryptGuid = CommonFunctions.DecryptStringFromBytes_Aes(_ClipGUID, key, iv);

                    Log4NetLogger.Info("Encrypted GUID : " + _ClipGUID);

                    if (Guid.TryParse(strDecryptGuid, out clipGuid))
                    {
                        Log4NetLogger.Info("RecordFile Guid : " + strDecryptGuid);
                        _XMLResult = clipLogic.GetRecordFileLocationByGuid(clipGuid);
                    }
                    else
                    {
                        throw new Exception("Invalid Encrypted ClipID");
                    }

                }



                Log4NetLogger.Info("Location: " + _XMLResult);

                /*
                 new Uri(_rf.RootPath.StreamSuffixPath).Host+appname+_rf.RootPath.StreamSuffixPath + _rf.Location).LocalPath;                   
                 */

                iOSGetLocationOutput.Media = _XMLResult;
                iOSGetLocationOutput.IsValidMedia = string.IsNullOrWhiteSpace(_XMLResult) ? false : true;

            }
            catch (Exception ex)
            {
                Log4NetLogger.Error("An Error Occurred" + ex);
                iOSGetLocationOutput.HasException = true;
                /* iOSGetLocationOutput.ExceptionMessage = ex.Message;*/
                iOSGetLocationOutput.IsValidMedia = false;
            }

            if (_IsAndroid!=true)
            {
                _XMLResult = Serializer.SerializeToXml(iOSGetLocationOutput); 
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                _XMLResult = Serializer.Searialize(iOSGetLocationOutput);
                _XMLResult = _Callback + "([" + _XMLResult + "])";
            }

            Log4NetLogger.Info("Response :" + _XMLResult);

            HttpResponse.Output.Write(_XMLResult);

            Log4NetLogger.Info("GetVars Request Ended");

        }

        #endregion
    }
}