using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;

namespace AnalyticsSearch
{
    class RESTClient
    {
        public static string getReponse(string URL, List<KeyValuePair<string, string>> parameters)
        {
            try
            {
                CommonFunctions.LogDebug("GetResponse");
                Uri address = new Uri(URL);
                string ret = string.Empty;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(address);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.Timeout = 210000;

                StringBuilder data = new StringBuilder();
                int count = 0;
                foreach (KeyValuePair<string, string> kvp in parameters)
                {
                    if (count > 0)
                    {
                        data.Append("&");
                    }

                    data.Append(kvp.Key + "=" + HttpUtility.UrlEncode(kvp.Value));
                    count++;
                }

                byte[] byteData = UTF8Encoding.UTF8.GetBytes(data.ToString());
                request.ContentLength = byteData.Length;

                string _URL = URL + data.ToString();

                CommonFunctions.LogDebug(string.Format("Analytics Search URL: {0}", _URL));

                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(byteData, 0, byteData.Length);
                }

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());

                    ret = reader.ReadToEnd();

                    return ret;
                }

            }
            catch (Exception exc)
            {
                return string.Empty;
            }
        }
    }
}
