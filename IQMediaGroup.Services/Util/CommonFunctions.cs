using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Domain;

namespace IQMediaGroup.Services.Util
{
    public class CommonFunctions
    {
        public static T InitializeRequest<T>(HttpRequest p_httpRequest, string p_Format)
        {
            try
            {
                string request = string.Empty;

                using (StreamReader StreamReader = new StreamReader(p_httpRequest.InputStream))
                {
                    request = StreamReader.ReadToEnd();
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogRequestResponse"]) == true)
                {
                    Log4NetLogger.Debug(request);
                }

                var inputObject = Activator.CreateInstance<T>();

                try
                {
                    if (!string.IsNullOrWhiteSpace(p_Format) && string.Compare(p_Format, CommonConstants.formatType.xml.ToString(), true) == 0)
                    {
                        inputObject = (T)Serializer.DeserialiazeXml(request, inputObject);
                    }
                    else
                    {
                        inputObject = (T)Serializer.Deserialize(request, inputObject.GetType());
                    }
                }
                catch (Exception)
                {
                    throw new CustomException("Error during parsing input");
                }

                return inputObject;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void ReturnResponse(HttpResponse HttpResponse, dynamic p_outputObject, string p_Format, bool p_IsCustomizedSerializer = false, string p_USeqID = "")
        {
            string outputResult = string.Empty;

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            try
            {
                if (!string.IsNullOrWhiteSpace(p_Format) && string.Compare(p_Format, CommonConstants.formatType.xml.ToString(), true) == 0)
                {
                    if (p_IsCustomizedSerializer)
                    {
                        outputResult = Serializer.SerializeXmlDC(p_outputObject);
                    }
                    else
                    {
                        outputResult = Serializer.SerializeToXmlWithoutNameSpace(p_outputObject);
                    }
                    HttpResponse.ContentType = "application/xml";
                }
                else
                {
                    outputResult = Serializer.Searialize(p_outputObject);

                    HttpResponse.ContentType = "application/json";
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogRequestResponse"]) == true)
                {
                    Log4NetLogger.Debug(p_USeqID + " - " + outputResult);
                }
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(p_USeqID, ex);
            }

            HttpResponse.Output.Write(outputResult);
        }

        public static string GetUniqueSeqIDForLog()
        {
            return (Convert.ToString(System.Web.HttpContext.Current.Items["USeqID"]) + " - ");
        }   
    }

    public static class CLogger
    {
        public static void Info(string message)
        {
            Log4NetLogger.Info((CommonFunctions.GetUniqueSeqIDForLog() + message));
        }

        public static void Debug(string message)
        {
            Log4NetLogger.Debug((CommonFunctions.GetUniqueSeqIDForLog() + message));
        }

        public static void Warning(Exception ex)
        {
            Warning("", ex);
        }

        public static void Warning(string message, Exception ex = null)
        {
            Log4NetLogger.Warning((CommonFunctions.GetUniqueSeqIDForLog() + message), ex);
        }

        public static void Error(Exception ex)
        {
            Error("", ex);
        }

        public static void Error(string message, Exception ex = null)
        {
            Log4NetLogger.Error((CommonFunctions.GetUniqueSeqIDForLog() + message), ex);
        }

        public static void Fatal(Exception ex)
        {
            Fatal("", ex);
        }

        public static void Fatal(string message, Exception ex = null)
        {
            Log4NetLogger.Fatal((CommonFunctions.GetUniqueSeqIDForLog() + message), ex);
        }
    }
}