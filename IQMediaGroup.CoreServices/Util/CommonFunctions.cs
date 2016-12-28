using System.Web;
using IQMediaGroup.CoreServices.Serializers;
using System;
using IQMediaGroup.Common.Util;
using System.Configuration;
using System.IO;

namespace IQMediaGroup.CoreServices.Util
{
    public class CommonFunctions
    {
        public static T InitializeRequest<T>(HttpRequest p_httpRequest, string p_SVCName, string p_Format = "xml")
        {
            Logger.LogInfo(p_SVCName + " Request Started");

            string request = string.Empty;

            using (StreamReader StreamReader = new StreamReader(p_httpRequest.InputStream))
            {
                request = StreamReader.ReadToEnd();
            }

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogRequestResponse"]) == true)
            {
                Logger.LogInfo(p_SVCName + " - " + request);
            }

            var inputObject = Activator.CreateInstance<T>();


            if (!string.IsNullOrWhiteSpace(p_Format) && string.Compare(p_Format, CommonConstants.formatType.xml.ToString(), true) == 0)
            {
                inputObject = (T)Serializer.DeserialiazeXml(request, inputObject);
            }
            else
            {
                inputObject = (T)Serializer.Deserialize(request, inputObject.GetType());
            }

            return inputObject;
        }

        public static void ReturnResponse(HttpResponse HttpResponse, dynamic p_outputObject, string p_SVCName, string p_Format = "xml")
        {
            string outputResult = string.Empty;

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            try
            {
                if (!string.IsNullOrWhiteSpace(p_Format) && string.Compare(p_Format, "xml", true) == 0)
                {
                    outputResult = Serializer.SerializeToXml(p_outputObject);

                    HttpResponse.ContentType = "application/xml";
                }
                else
                {
                    outputResult = Serializer.Searialize(p_outputObject);

                    HttpResponse.ContentType = "application/json";
                }

                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLogRequestResponse"]) == true)
                {
                    Logger.LogInfo(p_SVCName + " output: " + outputResult);
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Error: "+p_SVCName+" - "+ex.ToString());
            }

            Logger.LogInfo(p_SVCName + " Request Ended");
            HttpResponse.Output.Write(outputResult);
        }
    }
}