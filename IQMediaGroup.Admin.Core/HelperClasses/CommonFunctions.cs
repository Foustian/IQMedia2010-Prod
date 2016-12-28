using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Net;
using System.Net.Mail;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web;
using System.Data;
using System.Configuration;
using IQMediaGroup.Admin.Core.Enumeration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Diagnostics;
using System.IO.Compression;

namespace IQMediaGroup.Admin.Core.HelperClasses
{
    /// <summary>
    /// This Class contains common functions
    /// </summary>
    public static class CommonFunctions
    {
        

        ///================================================================================
        /// Function Name : MakeDeserialiazation
        /// 
        /// <summary>
        /// This fucntion performs DeSerialization of XML string and returns the object 
        /// Added By : vishal parekh.
        /// </summary>
        /// <param name="p_sXMLString">XML String to be Deserialized</param>
        /// <param name="p_Deserialization">Deserialization object type</param>
        /// <returns>Deserialized object </returns>
        ///================================================================================
        public static object MakeDeserialiazation(string p_XMLString, object p_Deserialization)
        {
            StringReader _StringReader;
            XmlTextReader _XmlTextReader;

            try
            {
                XmlSerializer _XmlSerializer = new XmlSerializer(p_Deserialization.GetType());
                _StringReader = new StringReader(p_XMLString);
                _XmlTextReader = new XmlTextReader(_StringReader);
                p_Deserialization = (object)_XmlSerializer.Deserialize(_XmlTextReader);
                _StringReader.Close();
                _XmlTextReader.Close();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return p_Deserialization;
        }

        /// <summary>
        /// Reconstruct an object from an XML string
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string xml)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T));
            MemoryStream memoryStream = new MemoryStream(StringToUTF8ByteArray(xml));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
            return (T)xs.Deserialize(memoryStream);
        }


        /// <summary>
        /// Converts the String to UTF8 Byte array and is used in De serialization
        /// </summary>
        /// <param name="pXmlString"></param>
        /// <returns></returns>
        private static Byte[] StringToUTF8ByteArray(string pXmlString)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            byte[] byteArray = encoding.GetBytes(pXmlString);
            return byteArray;
        }
    

        /// <summary>
        /// This method try to convert string to int
        /// </summary>
        /// <param name="p_Double">string possibly conatins int value</param>
        /// <returns>int value or null</returns>
        public static int? GetIntValue(string p_Int)
        {
            int? _ReturnInt = null;
            int _TempInt;

            if (int.TryParse(p_Int, out _TempInt))
            {
                _ReturnInt = _TempInt;
            }

            return _ReturnInt;
        }

        /// <summary>
        /// This method try to convert string to DateTime
        /// </summary>
        /// <param name="p_DT">string possibly conatins DateTime value</param>
        /// <returns>DateTime value or null</returns>
        public static DateTime? GetDateTimeValue(string p_DT)
        {
            DateTime? _ReturnDT = null;
            DateTime _TempDT;

            if (DateTime.TryParse(p_DT, out _TempDT))
            {
                _ReturnDT = _TempDT;
            }

            return _ReturnDT;
        }

        /// <summary>
        /// This method try to convert string to bool
        /// </summary>
        /// <param name="p_Bl">string possibly conatins bool value</param>
        /// <returns>bool value or null</returns>
        public static bool? GetBoolValue(string p_Bl)
        {
            bool? _ReturnBl = null;
            bool _TempBl;

            if (bool.TryParse(p_Bl, out _TempBl))
            {
                _ReturnBl = _TempBl;
            }

            return _ReturnBl;
        }

       

        /// <summary>
        /// This method try to parse string into Int64
        /// </summary>
        /// <param name="p_Int64">string possible value Int64</param>
        /// <returns>Null or Int64</returns>
        public static Int64? GetInt64Value(string p_Int64)
        {
            try
            {
                Int64? _ReturnInt64 = null;
                Int64 _TempInt64;

                if (Int64.TryParse(p_Int64, out _TempInt64))
                {
                    _ReturnInt64 = _TempInt64;
                }

                return _ReturnInt64;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

      
        /// <summary>
        /// Description: This method will set the Session Informaiton.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_SessionInformation">Contains the SessionInformation that needs to be set in session.</param>
        public static void SetSessionInformation(SessionInformation p_SessionInformation)
        {
            try
            {
                HttpContext.Current.Session["SessionInformation"] = p_SessionInformation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This method Gets the Session information from session.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>SessionInformation Object</returns>
        public static SessionInformation GetSessionInformation()
        {
            SessionInformation _SessionInformation = null;

            try
            {
                _SessionInformation = (SessionInformation)HttpContext.Current.Session["SessionInformation"];

                if (_SessionInformation == null) _SessionInformation = new SessionInformation();
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

            return _SessionInformation;
        }

        public static void LogInfo(string LogMessage)
        {
            try
            {
                if (Convert.ToBoolean(ConfigurationManager.AppSettings["IsLOGWrite"]) == true)
                {
                    string path = ConfigurationManager.AppSettings["LOGAdvancedSearchServicesFileLocation"] + "LOG_" + DateTime.Today.ToString("MMddyyyy") + ".csv";
                    if (!File.Exists(path))
                    {
                        File.Create(path).Close();
                    }
                    using (StreamWriter w = File.AppendText(path))
                    {
                        w.WriteLine(DateTime.Now.ToString() + " , [INFO] ,\"" + LogMessage + "\"");
                    }
                }
            }
            catch (Exception)
            {
                
            }

        }

    }
}
