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
using IQMediaGroup.Core.Enumeration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Diagnostics;
using System.IO.Compression;
using System.Runtime.Serialization.Json;

namespace IQMediaGroup.Core.HelperClasses
{
    /// <summary>
    /// This Class contains common functions
    /// </summary>
    public static class CommonFunctions
    {
        /// <summary>
        /// Fills Combo From DataSet.
        /// Added By: vishal parekh
        /// </summary>
        /// <param name="p_DropDownList">The Dropdown list that we need to fill</param>
        /// <param name="p_ListofDataItems">Object from which we need to bind the dropdown</param>
        /// <param name="p_DataValueField">Value Iteam for Dropdown. For example ID that will be bind in background for record</param>
        /// <param name="p_DataTextField">Text value that will appear in List of dropdown</param>
        /// <param name="p_DisplayText">Text that we need to display for selected iteam</param>
        /// <param name="p_SelectedItem">Iteam that we need to make by default selected in state</param>
        public static void FillComboFromDataSet(ref DropDownList p_DropDownList, object p_ListofDataItems, string p_DataValueField, string p_DataTextField, string p_DisplayText, string p_SelectedItem)
        {
            try
            {
                p_DropDownList.DataSource = p_ListofDataItems;
                p_DropDownList.DataTextField = p_DataTextField;
                p_DropDownList.DataValueField = p_DataValueField;
                p_DropDownList.DataBind();
                if (p_DisplayText != "")
                {
                    ListItem _LiDefault = new ListItem();
                    _LiDefault.Text = p_DisplayText;
                    _LiDefault.Value = "";
                    p_DropDownList.Items.Insert(0, _LiDefault);
                }
                if (p_SelectedItem != "")
                {
                    p_DropDownList.SelectedValue = p_SelectedItem;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        ///================================================================================
        /// Function Name : MakeSerialization
        /// 
        /// <summary>
        /// This fucntion performs Serialization of object and returns the Response XML string
        /// Added By : vishal parekh.
        /// </summary>
        /// <param name="p_oSerializationObject">object to be serialized</param>
        /// <returns>Serialized XML Response String</returns>
        ///================================================================================
        public static string MakeSerialization(object p_SerializationObject)
        {
            try
            {
                string _XMLString = string.Empty;
                System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
                MemoryStream _MemoryStream = new MemoryStream();
                XmlTextWriter _XmlTextWriter = new XmlTextWriter(_MemoryStream, _Encoding);
                XmlSerializer _XmlSerializer = new XmlSerializer(p_SerializationObject.GetType());

                try
                {
                    _XmlSerializer.Serialize(_XmlTextWriter, p_SerializationObject);
                    _MemoryStream = (MemoryStream)_XmlTextWriter.BaseStream;
                    _XMLString = _Encoding.GetString(_MemoryStream.ToArray());
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }
                return _XMLString;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string MakeSerializationWithoutNameSpace(object p_SerializationObject)
        {
            try
            {
                string _XMLString = string.Empty;

                System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
                XmlWriterSettings _XmlWriterSettings = new XmlWriterSettings();
                // _XmlWriterSettings.Encoding=_Encoding;
                _XmlWriterSettings.OmitXmlDeclaration = true;

                XmlSerializer _XmlSerializer = new XmlSerializer(p_SerializationObject.GetType(), "");

                try
                {
                    StringWriter _StringWriter = new StringWriter();
                    using (XmlWriter _XmlWriter = XmlWriter.Create(_StringWriter,
                    _XmlWriterSettings))
                    {
                        XmlSerializerNamespaces _XmlSerializerNamespaces = new XmlSerializerNamespaces();
                        _XmlSerializerNamespaces.Add("", "");

                        _XmlSerializer.Serialize(_XmlWriter, p_SerializationObject, _XmlSerializerNamespaces);
                    }

                    _XMLString = _StringWriter.ToString();
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }
                return _XMLString;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

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
        /// This method try to convert string to double
        /// </summary>
        /// <param name="p_Double">string possibly conatins double value</param>
        /// <returns>double value or null</returns>
        public static double? GetDoubleValue(string p_Double)
        {
            double? _ReturnDbl = null;
            double _TempDbl;

            if (double.TryParse(p_Double, out _TempDbl))
            {
                _ReturnDbl = _TempDbl;
            }

            return _ReturnDbl;
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
        /// This method try to convert string to float
        /// </summary>
        /// <param name="p_float">string possibly conatins float value</param>
        /// <returns>float value or null</returns>
        public static float? GetFloatValue(string p_float)
        {
            float? _ReturnFloat = null;
            float _TempFloat;

            if (float.TryParse(p_float, out _TempFloat))
            {
                _ReturnFloat = _TempFloat;
            }

            return _ReturnFloat;
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

        public static Guid? GetGUIDValue(String p_GUID)
        {
            Guid? _ReturnGUID = null;

            try
            {
                _ReturnGUID = new Guid(p_GUID);
                return _ReturnGUID;

            }
            catch (FormatException)
            {
                return null;
            }
        }


        /// <summary>
        /// Function that sends email out
        /// </summary>
        /// <param name="sSMTPServer">SMTPServer</param>
        /// <param name="sSMTPPort">sSMTPPort</param>
        /// <param name="sSmtpSSL">sSmtpSSL</param>
        /// <param name="sSMTPUser">SMTPUser</param>
        /// <param name="sSMTPPassword">SMTPPassword</param>
        /// <param name="sTo">To</param>
        /// <param name="sCC">CC</param>
        /// <param name="sBCC">BCC</param>
        /// <param name="sFrom">From</param>
        /// <param name="sSubject">Subject</param>
        /// <param name="sMsgBody">Message Body</param>
        /// <param name="bHtml">Is Html</param>
        /// <param name="sAttachments">Attachment files</param>
        /// <param name="appSession">Current Session</param>
        /// <returns></returns>
        public static string SendMail(string sSMTPServer,
                                    string sSMTPPort,
                                    bool sSmtpSSL,
                                    string sSMTPUser,
                                    string sSMTPPassword,
                                    string sTo,
                                    string sCC,
                                    string sBCC,
                                    string sFrom,
                                    string sSubject,
                                    string sMsgBody,
                                    bool bHtml,
                                    string[] sAttachments)
        {
            string strMessage = "Please Set Email Configuration Setting.";

            if (sFrom.Length == 0 || sTo.Length == 0 || sSMTPServer.Length == 0)
                return strMessage;

            try
            {
                System.Net.Mail.MailMessage objMail = new System.Net.Mail.MailMessage();

                objMail.From = new MailAddress(sFrom);
                objMail.Subject = sSubject;
                objMail.Body = sMsgBody;

                if (sTo.Length > 0)
                    objMail.To.Add(sTo);
                if (sCC.Length > 0)
                    objMail.CC.Add(sCC);
                if (sBCC.Length > 0)
                    objMail.Bcc.Add(sBCC);

                if (bHtml)
                    objMail.IsBodyHtml = true;
                else
                    objMail.IsBodyHtml = false;


                if (null != sAttachments)
                {
                    for (int iCount = 0; iCount < sAttachments.Length; iCount++)
                    {
                        objMail.Attachments.Add(new Attachment(sAttachments[iCount]));
                    }
                }

                SmtpClient smtp = new SmtpClient();
                smtp.Host = sSMTPServer;
                System.Net.NetworkCredential objCredential = new System.Net.NetworkCredential(sSMTPUser, sSMTPPassword);
                smtp.Credentials = objCredential;
                smtp.Send(objMail);

                // Email was sent succesfully; return true;
                strMessage = "Email has been sent Successfully";
                return strMessage;
            }
            catch (Exception ex)
            {
                strMessage = "Error Sending Email";
                return strMessage;
                throw ex;
            }
        }



        public static string EmailSend(string From, string To, string Subject, string Body, string EmailContent)
        {
            try
            {

                /*SmtpClient smtp = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"]);
                smtp.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["UserName"], ConfigurationManager.AppSettings["Password"]);
                smtp.Port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
                smtp.EnableSsl = false;*/

                SmtpClient _SmtpClient = new SmtpClient();
                string MailUserName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerUser"));
                string MailPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerPassword"));

                NetworkCredential _NetworkCredential = new NetworkCredential(MailUserName, MailPassword);
                _SmtpClient.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("sSMTPPort"));
                _SmtpClient.Credentials = _NetworkCredential;
                _SmtpClient.Host = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServer"));

                MailMessage msg = new MailMessage();
                msg.IsBodyHtml = true;
                msg.From = new MailAddress(From);
                string[] ToEmailAddresses = To.Split(',');

                for (int i = 0; i < ToEmailAddresses.Length; i++)
                {
                    msg.To.Add(ToEmailAddresses[i]); //from argument
                }

                msg.Subject = Subject; //from argument

                string strEmailBody = string.Empty;

                string strEmailBodyfinal = string.Empty;
                // ViewstateInformation _ViewstateInformation = GetViewstateInformation();


                /*strEmailBodyfinal += "<html>";
                strEmailBodyfinal += "<body style=\"font-family:Verdana;font-size:11px;\">";
                if (!string.IsNullOrEmpty(Body))
                {

                    strEmailBodyfinal += Body.Replace("\r\n","<br />") + "<br />";
                    strEmailBodyfinal += EmailContent + "<br />";
                    strEmailBodyfinal += "Thanks,<br /> IQMedia Corp <br /> www.iqmediacorp.com";
                    strEmailBodyfinal += "</body>";
                    strEmailBodyfinal += "</html>";
                }
                else
                {
                    strEmailBodyfinal += "Hi," + "<br />";
                    strEmailBodyfinal += "Please Check out following IQMedia Clip(s) <br />";
                    strEmailBodyfinal += EmailContent + "<br />";
                    strEmailBodyfinal += "Thanks,<br /> IQMedia Corp <br /> www.iqmediacorp.com";
                    strEmailBodyfinal += "</body>";
                    strEmailBodyfinal += "</html>";
                }*/


                strEmailBodyfinal += "<div style=\"font-family:Verdana;font-size:11px;\">";
                if (!string.IsNullOrEmpty(Body))
                {

                    strEmailBodyfinal += Body.Replace("\r\n", "<br />") + "<br />";
                    strEmailBodyfinal += EmailContent + "<br />";
                    strEmailBodyfinal += "Thanks,<br /> iQMedia Corp <br /> www.iqmediacorp.com";
                }
                else
                {
                    //strEmailBodyfinal += "Hi," + "<br />";
                    //strEmailBodyfinal += "Please Check out following IQMedia Clip(s) <br />";
                    strEmailBodyfinal += EmailContent + "<br />";
                    strEmailBodyfinal += "Thanks,<br /> iQMedia Corp <br /> www.iqmediacorp.com";
                }

                strEmailBodyfinal = strEmailBodyfinal + "<br /><br />" + File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailPolicy.txt"), Encoding.UTF8);

                strEmailBodyfinal += "</div>";

                msg.Body = strEmailBodyfinal;
                msg.IsBodyHtml = true;

                _SmtpClient.Send(msg); //send email out
                //MailLog(strEmailBodyfinal, From, To);
                // msg.Dispose(); //get rid of the object

                return strEmailBodyfinal;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This Function  Validates the User Email Address
        /// </summary>
        /// <param name="_UserEmail">Contains User's Email Address</param>
        /// <returns>True if validate else false</returns>
        public static bool validateEmails(string _UserEmail)
        {
            try
            {

                string _EmailPatern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

                Regex _Regex = new Regex(_EmailPatern);

                return _Regex.IsMatch(_UserEmail);

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

        public static void SetSessionInformationIframe(SessionInformationIframe p_SessionInformationIframe)
        {
            try
            {
                HttpContext.Current.Session["SessionInformationIframe"] = p_SessionInformationIframe;
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

        public static SessionInformationIframe GetSessionInformationIframe()
        {
            SessionInformationIframe _SessionInformationIframe = null;

            try
            {
                _SessionInformationIframe = (SessionInformationIframe)HttpContext.Current.Session["SessionInformationIframe"];

                if (_SessionInformationIframe == null) _SessionInformationIframe = new SessionInformationIframe();
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

            return _SessionInformationIframe;
        }

        /// <summary>
        /// Description:This method add BQQueryString
        /// </summary>
        /// <param name="p_URL">Url</param>
        /// <param name="p_Key">QueryString Key</param>
        /// <param name="p_Value">QueryString Value</param>
        /// <param name="p_First">First Querysting or not</param>
        /// <param name="p_Quote">Value requires Dobule quote or not</param>
        /// <param name="p_SqBrackte">Value requires Bracket or not</param>
        /// <returns>Url</returns>
        public static string AddSBQQueryString(string p_URL, string p_Key, string p_Value, bool p_First, bool p_Quote, bool p_SqBracket)
        {
            try
            {


                string _Url = string.Empty;

                if (p_First == true)
                {
                    if (p_SqBracket == true)
                    {
                        if (p_Quote == true)
                        {
                            _Url = CommonConstants.QuestionMark + p_Key + CommonConstants.Equal + CommonConstants.SqBracketOpen + CommonConstants.DblQuote + p_Value + CommonConstants.DblQuote + CommonConstants.SqBracketClose;
                        }
                        else
                        {
                            _Url = CommonConstants.QuestionMark + p_Key + CommonConstants.Equal + CommonConstants.SqBracketOpen + p_Value + CommonConstants.SqBracketClose;
                        }
                    }
                    else if (p_SqBracket == false)
                    {
                        if (p_Quote == true)
                        {
                            _Url = CommonConstants.QuestionMark + p_Key + CommonConstants.Equal + CommonConstants.DblQuote + p_Value + CommonConstants.DblQuote;
                        }
                        else
                        {
                            _Url = CommonConstants.QuestionMark + p_Key + CommonConstants.Equal + p_Value;
                        }
                    }

                }
                else if (p_First == false)
                {
                    if (p_SqBracket == true)
                    {
                        if (p_Quote == true)
                        {
                            _Url = CommonConstants.Ampersand + p_Key + CommonConstants.Equal + CommonConstants.SqBracketOpen + CommonConstants.DblQuote + p_Value + CommonConstants.DblQuote + CommonConstants.SqBracketClose;
                        }
                        else
                        {
                            _Url = CommonConstants.Ampersand + p_Key + CommonConstants.Equal + CommonConstants.SqBracketOpen + p_Value + CommonConstants.SqBracketClose;
                        }
                    }
                    else if (p_SqBracket == false)
                    {
                        if (p_Quote == true)
                        {
                            _Url = CommonConstants.Ampersand + p_Key + CommonConstants.Equal + CommonConstants.DblQuote + p_Value + CommonConstants.DblQuote;
                        }
                        else
                        {
                            _Url = CommonConstants.Ampersand + p_Key + CommonConstants.Equal + p_Value;
                        }
                    }

                }
                return _Url;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string GetHttpResponse(string p_URL, CookieContainer p_CookieContainer = null, string p_Input = null, string p_ContentType = "application/json")
        {
            try
            {
                Uri _Uri = new Uri(p_URL);

                //HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(p_URL);
                HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_Uri);

                IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                _httpWebRequest.KeepAlive = false;
                //_httpWebRequest.Timeout = CommonConstants.RequestTimeOut;

                /* for testing */

                if (p_CookieContainer != null)
                {
                    _httpWebRequest.CookieContainer = p_CookieContainer;
                }

                if (!string.IsNullOrEmpty(p_Input))
                {
                    ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                    byte[] byteArray = _objEncodedData.GetBytes(p_Input);

                    _httpWebRequest.Method = "POST";
                    _httpWebRequest.ContentType = p_ContentType;
                    _httpWebRequest.ContentLength = byteArray.Length;

                    Stream _objStream = _httpWebRequest.GetRequestStream();
                    _objStream.Write(byteArray, 0, byteArray.Length);
                    _objStream.Close();
                }

                /* over for testing */

                StreamReader _StreamReader = null;
                string _ResponseRawMedia = string.Empty;
                if ((_httpWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_httpWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                return _ResponseRawMedia;
            }
            catch (TimeoutException _TimeoutException)
            {
                throw _TimeoutException;
            }
            catch (WebException)
            {
                throw new TimeoutException();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string SearializeJson(object _object)
        {
            try
            {
                DataContractJsonSerializer Serializer = new DataContractJsonSerializer(_object.GetType());

                MemoryStream Stream = new MemoryStream();

                Serializer.WriteObject(Stream, _object);

                Stream.Position = 0;

                StreamReader StreamReader = new StreamReader(Stream);

                return StreamReader.ReadToEnd();

                /*JavaScriptSerializer _JavaScriptSerializer = new JavaScriptSerializer();

                return _JavaScriptSerializer.Serialize(_object);*/
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {

            byte[] key = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

            byte[] inputByteArray = new byte[stringToDecrypt.Length + 1];
            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                return encoding.GetString(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;

            }

            /*RijndaelManaged RijndaelCipher = new RijndaelManaged();

             string Password = sEncryptionKey;
             string DecryptedData;

             try
             {
                 byte[] EncryptedData = Convert.FromBase64String(stringToDecrypt);

                 byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
                 //Making of the key for decryption
                 PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
                 //Creates a symmetric Rijndael decryptor object.
                 ICryptoTransform Decryptor = RijndaelCipher.CreateDecryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));

                 MemoryStream memoryStream = new MemoryStream(EncryptedData);
                 //Defines the cryptographics stream for decryption.THe stream contains decrpted data
                 CryptoStream cryptoStream = new CryptoStream(memoryStream, Decryptor, CryptoStreamMode.Read);

                 byte[] PlainText = new byte[EncryptedData.Length];
                 int DecryptedCount = cryptoStream.Read(PlainText, 0, PlainText.Length);
                 memoryStream.Close();
                 cryptoStream.Close();

                 //Converting to string
                 DecryptedData = Encoding.Unicode.GetString(PlainText, 0, DecryptedCount);
             }
             catch
             {
                 DecryptedData = stringToDecrypt;
             }

             return DecryptedData;*/



        }

        //base64 in encryption
        public static string Encrypt(string stringToEncrypt, string SEncryptionKey)
        {

            byte[] key = { };
            byte[] IV = { 0x12, 0x34, 0x56, 0x78, 0x90, 0xab, 0xcd, 0xef };

            try
            {
                key = System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                return Convert.ToBase64String(ms.ToArray());
            }
            catch (Exception e)
            {
                return e.Message;

            }

            /*RijndaelManaged RijndaelCipher = new RijndaelManaged();
             string Password = SEncryptionKey;
             byte[] PlainText = System.Text.Encoding.Unicode.GetBytes(stringToEncrypt);
             byte[] Salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
             PasswordDeriveBytes SecretKey = new PasswordDeriveBytes(Password, Salt);
             //Creates a symmetric encryptor object.
             ICryptoTransform Encryptor = RijndaelCipher.CreateEncryptor(SecretKey.GetBytes(32), SecretKey.GetBytes(16));
             MemoryStream memoryStream = new MemoryStream();
             //Defines a stream that links data streams to cryptographic transformations
             CryptoStream cryptoStream = new CryptoStream(memoryStream, Encryptor, CryptoStreamMode.Write);
             cryptoStream.Write(PlainText, 0, PlainText.Length);
             //Writes the final state and clears the buffer
             cryptoStream.FlushFinalBlock();
             byte[] CipherBytes = memoryStream.ToArray();
             memoryStream.Close();
             cryptoStream.Close();
             string EncryptedData = Convert.ToBase64String(CipherBytes);

             return EncryptedData;*/

            /*string rsaPrivate = SEncryptionKey;
            CspParameters csp = new CspParameters();
            csp.Flags = CspProviderFlags.UseMachineKeyStore;

            RSACryptoServiceProvider provider = new RSACryptoServiceProvider(csp);

            //provider.FromXmlString(rsaPrivate);

            ASCIIEncoding enc = new ASCIIEncoding();
            int numOfChars = enc.GetByteCount(stringToEncrypt);
            byte[] tempArray = enc.GetBytes(rsaPrivate);
            byte[] result = provider.Encrypt(tempArray, true);
            string resultString = Convert.ToBase64String(result);
            //Console.WriteLine("Encrypted : " + resultString);
            return resultString;*/

        }

        public static byte[] AesEncryptStringToBytes(string plainText, string AesKey, string AesIV)
        {
            byte[] encrypted;


            UTF8Encoding encoding = new UTF8Encoding();
            byte[] Key = encoding.GetBytes(AesKey);
            byte[] IV = encoding.GetBytes(AesIV);

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                aesAlg.Key = Key;
                aesAlg.IV = IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }


        /// <summary>
        /// Description:This method find key.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_FindKeyValue">FindKeyValue</param>
        /// <param name="p_String">String</param>
        /// <returns>true or false</returns>
        public static bool FindKey(KeyValue p_FindKeyValue, string p_String)
        {
            try
            {
                if (p_String.Contains(p_FindKeyValue._FindKey))
                {
                    string _SubString = p_String;
                    _SubString = p_String.Substring(p_String.IndexOf(p_FindKeyValue._FindKey));

                    if (p_FindKeyValue._FindKey == "\"Complete\":")
                    {
                        _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf(",\"") - p_FindKeyValue._FindKey.Length));
                    }
                    else if (p_FindKeyValue._FindKey == "\"Hits\":")
                    {
                        _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf("}") - p_FindKeyValue._FindKey.Length));
                    }
                    else if (p_FindKeyValue._FindKey == CommonConstants.RMSearchHitList)
                    {
                        _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf(CommonConstants.SqBracketClose) - p_FindKeyValue._FindKey.Length));
                    }
                    else
                    {
                        _SubString = _SubString.Substring(p_FindKeyValue._FindKey.Length, (_SubString.IndexOf("\",\"") - p_FindKeyValue._FindKey.Length));
                    }
                    p_FindKeyValue._KeyValue = _SubString;
                    p_FindKeyValue._SetKey = true;

                    return true;
                }

                return false;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method is to move file
        /// </summary>
        /// <param name="p_FileInfo">current file info</param>
        /// <param name="p_DesPath">Destination Path</param>
        public static void MoveFile(FileInfo p_FileInfo, string p_DesPath)
        {
            try
            {
                p_FileInfo.CopyTo(p_DesPath);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        /// <summary>
        /// This methods remove cookies from browser
        /// </summary>
        public static void RemoveCookies(Page _Page)
        {
            try
            {
                /*  List<HttpCookie> _ListOfCookie = new List<HttpCookie>();

                  for (int _Index = 0; _Index < HttpContext.Current.Request.Cookies.Count; _Index++)
                  {
                      HttpCookie _HttpCookie = HttpContext.Current.Request.Cookies[_Index];

                      _ListOfCookie.Add(_HttpCookie);
                  }

                  foreach (HttpCookie _HttpCookie in _ListOfCookie)
                  {
                      _HttpCookie.Expires = DateTime.Now.AddDays(-1);

                      HttpContext.Current.Response.Cookies.Add(_HttpCookie);

                      string _DeleteHttpCookie = CommonConstants.CookieDelete + CommonConstants.BracketOpen + CommonConstants.DblQuote + _HttpCookie.Name + CommonConstants.DblQuote + CommonConstants.BracketClose + CommonConstants.SemiColon;

                      _Page.ClientScript.RegisterStartupScript(_Page.GetType(), _HttpCookie.Name, _DeleteHttpCookie, true);
                  }

                  string _DeleteCookie = CommonConstants.CookieDelete + CommonConstants.BracketOpen + CommonConstants.DblQuote + CommonConstants.CookieASPROLES + CommonConstants.DblQuote + CommonConstants.BracketClose + CommonConstants.SemiColon;

                  _Page.ClientScript.RegisterStartupScript(_Page.GetType(), _DeleteCookie, _DeleteCookie, true);

                  _DeleteCookie = CommonConstants.CookieDelete + CommonConstants.BracketOpen + CommonConstants.DblQuote + CommonConstants.CookieBBQ + CommonConstants.DblQuote + CommonConstants.BracketClose + CommonConstants.SemiColon;

                  _Page.ClientScript.RegisterStartupScript(_Page.GetType(), _DeleteCookie, _DeleteCookie, true);

                  _DeleteCookie = CommonConstants.CookieDelete + CommonConstants.BracketOpen + CommonConstants.DblQuote + CommonConstants.CookieRLAUTH + CommonConstants.DblQuote + CommonConstants.BracketClose + CommonConstants.SemiColon;

                  _Page.ClientScript.RegisterStartupScript(_Page.GetType(), _DeleteCookie, _DeleteCookie, true);

                  _DeleteCookie = CommonConstants.CookieDelete + CommonConstants.BracketOpen + CommonConstants.DblQuote + CommonConstants.CookieRLUID + CommonConstants.DblQuote + CommonConstants.BracketClose + CommonConstants.SemiColon;

                  _Page.ClientScript.RegisterStartupScript(_Page.GetType(), _DeleteCookie, _DeleteCookie, true);*/
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        public static string ReadXml(string p_FilePath)
        {
            try
            {
                XmlDocument _XmlDocument = new XmlDocument();

                _XmlDocument.Load(p_FilePath);

                string XmlString = _XmlDocument.InnerXml;

                return XmlString;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string GenerateDateTimeString(string p_DateMin, string p_DateMax, int p_TimeMin, int p_TimeMax)
        {
            try
            {
                string TimeValue = string.Empty;

                for (DateTime _Index = Convert.ToDateTime(Convert.ToDateTime(p_DateMin).ToShortDateString()); _Index <= Convert.ToDateTime(Convert.ToDateTime(p_DateMax).ToShortDateString()); )
                {
                    for (int Time = p_TimeMin; Time <= p_TimeMax; Time++)
                    {
                        if (string.IsNullOrEmpty(TimeValue.ToString()))
                        {
                            TimeValue = CommonConstants.DblQuote + TimeValue + _Index.ToShortDateString() + CommonConstants.Space + Time.ToString() + CommonConstants.Colon + CommonConstants.RMZDZT + CommonConstants.DblQuote;
                        }
                        else
                        {
                            TimeValue = TimeValue + CommonConstants.Comma + CommonConstants.DblQuote + _Index.ToShortDateString() + CommonConstants.Space + Time.ToString() + CommonConstants.Colon + CommonConstants.RMZDZT + CommonConstants.DblQuote;
                        }
                    }

                    _Index = _Index.AddDays(1);
                }

                return TimeValue.ToString();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string formatOffset(int offs)
        {
            int h = 0;
            int m = 0;
            offs -= ((h = offs / 3600) * 3600);
            offs -= ((m = offs / 60) * 60);
            return ("" + h).PadLeft(2, '0') + ":" + ("" + m).PadLeft(2, '0') + ":" + ("" + offs).PadLeft(2, '0');
        }

        public static void BuildConnectionStringFromUserConfig(string p_ConnectionString)
        {
            try
            {
                string _ApplicationName = Environment.GetCommandLineArgs()[0];

                string _ExePath = System.IO.Path.Combine(Environment.CurrentDirectory, _ApplicationName);

                // Get the configuration file. The file name has
                // this format appname.exe.config.
                System.Configuration.Configuration _Config = ConfigurationManager.OpenExeConfiguration(_ExePath);

                ((System.Configuration.ConnectionStringsSection)(_Config.Sections["connectionStrings"])).ConnectionStrings[ConnectionStringKeys.IQMediaGroupConnectionString.ToString()].ConnectionString = p_ConnectionString;

                _Config.Save(ConfigurationSaveMode.Modified);

                // Force a reload of the changed section.
                // This makes the new values available for reading.
                ConfigurationManager.RefreshSection("connectionStrings");


            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
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

        public static object DeserializeJson(string _JSONString, Type _TargetType)
        {
            try
            {
                DataContractJsonSerializer Deserializer = new DataContractJsonSerializer(_TargetType);
                MemoryStream MemoryStream = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(_JSONString));

                return (Deserializer.ReadObject(MemoryStream));

                /*JavaScriptSerializer _JavaScriptSerializer = new JavaScriptSerializer();

                return _JavaScriptSerializer.Deserialize(_JSONString, _TargetType);*/
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string DecryptStringFromBytes_Aes(string encrypteString, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (string.IsNullOrWhiteSpace(encrypteString))
                throw new ArgumentNullException("encrypted string is null");

            //byte[] cipherText = Convert.FromBase64String(encrypteString.Replace(" ", "+"));
            byte[] cipherText = Convert.FromBase64String(encrypteString);

            //byte[] cipherText = StringToUTF8ByteArray(encrypteString);            
            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesAlg = new AesManaged())
            {
                UTF8Encoding encoding = new UTF8Encoding();


                aesAlg.Key = Key;// aesAlg.Key;
                aesAlg.IV = IV;// aesAlg.IV;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;

        }

        public static bool GeneratePdf(string inputFile, string outputFile)
        {
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            //NOTE: there seems to be an issue with the redirect
            startInfo.RedirectStandardError = false;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.FileName = ConfigurationManager.AppSettings["wkhtmltopdfPath"];
            startInfo.Arguments = inputFile + " " + outputFile;

            try
            {
                Logger.Debug(startInfo.FileName + " " + startInfo.Arguments);
                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    //NOTE: Can't do this; see error above about redirect
                    //var res = exeProcess.StandardError.ReadToEnd();
                    //Logger.Debug(res);
                }

                return File.Exists(outputFile);
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
                return false;
            }
        }


        public static bool SetSessionInformationByFormAuthentication(Customer customer, List<Role> listOfRole)
        {

            try
            {
                SessionInformation sessionInformation = new SessionInformation();
                sessionInformation.ClientGUID = customer.ClientGUID;
                sessionInformation.ClientID = customer.ClientID;
                sessionInformation.ClientName = customer.ClientName;
                sessionInformation.ClientPlayerLogoImage = customer.ClientPlayerLogoImage;
                sessionInformation.CustomeHeaderImage = customer.CustomHeaderImage;
                sessionInformation.CustomerKey = customer.CustomerKey;
                sessionInformation.Email = customer.Email;
                sessionInformation.FirstName = customer.FirstName;
                sessionInformation.LastName = customer.LastName;
                sessionInformation.IsClientPlayerLogoActive = customer.IsClientPlayerLogoActive;
                sessionInformation.ISCustomeHeader = customer.IsCustomHeader;
                sessionInformation.IsLogIn = true;
                sessionInformation.IsUgcAutoClip = false;
                sessionInformation.MultiLogin = customer.MultiLogin;
                sessionInformation.CustomerGUID = customer.CustomerGUID;
                sessionInformation.AuthorizedVersion = customer.AuthorizedVersion;


                if (listOfRole.Count > 0)
                {
                    sessionInformation._ListOfRoleName = listOfRole;

                    //_ListOfRole
                    foreach (Role _RoleName in listOfRole)
                    {
                        if (_RoleName.RoleName == RolesName.myIQAccess.ToString())
                        {
                            sessionInformation.IsmyIQ = true;
                        }
                        else if (_RoleName.RoleName == RolesName.IQBasic.ToString())
                        {
                            sessionInformation.IsiQBasic = true;
                        }
                        else if (_RoleName.RoleName == RolesName.AdvancedSearchAccess.ToString())
                        {
                            sessionInformation.IsiQAdvance = true;
                        }
                        else if (_RoleName.RoleName == RolesName.IQAgentWebsiteAccess.ToString())
                        {
                            sessionInformation.IsiQAgent = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.DownloadClips.ToString().ToLower())
                        {
                            sessionInformation.IsDownloadClips = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.IQCustomAccess.ToString().ToLower())
                        {
                            sessionInformation.IsiQCustom = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.UGCDownload.ToString().ToLower())
                        {
                            sessionInformation.IsUGCDownload = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.UGCUploadEdit.ToString().ToLower())
                        {
                            sessionInformation.IsUGCUploadEdit = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.UGCAutoClip.ToString().ToLower())
                        {
                            sessionInformation.IsUgcAutoClip = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremium.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremium = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.MyIQnew.ToString().ToLower())
                        {
                            sessionInformation.IsMyIQnew = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumSM.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremiumSM = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumNM.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremiumNM = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQSM.ToString().ToLower())
                        {
                            sessionInformation.IsmyiQSM = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQNM.ToString().ToLower())
                        {
                            sessionInformation.IsmyiQNM = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQPM.ToString().ToLower())
                        {
                            sessionInformation.IsmyiQPM = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQAgentReport.ToString().ToLower())
                        {
                            sessionInformation.IsiQAgentReport = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.myiQReport.ToString().ToLower())
                        {
                            sessionInformation.IsMyiQReport = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.MyIQTwitter.ToString().ToLower())
                        {
                            sessionInformation.IsMyIQTwitter = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumTwitter.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremiumTwitter = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumAgent.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremiumAgent = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumRadio.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremiumRadio = true;
                        }
                        else if (_RoleName.RoleName.Trim().ToLower() == RolesName.iQPremiumSentiment.ToString().ToLower())
                        {
                            sessionInformation.IsiQPremiumSentiment = true;
                        }
                    }
                    HttpContext.Current.Session["SessionInformation"] = sessionInformation;
                    return true;

                }
                else
                {
                    HttpContext.Current.Session["SessionInformation"] = sessionInformation;
                    return false;
                    //Response.Redirect("~/NoRole/", false);
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }




        }

    }

    /// <summary>
    /// Description:This method find Key Value.
    /// Added By:maulik Gandhi
    /// </summary>
    public class KeyValue
    {
        public string _FindKey { get; set; }
        public bool _SetKey { get; set; }
        public string _KeyValue { get; set; }

        public KeyValue(string p_FindKey, bool p_SetKey)
        {
            _FindKey = p_FindKey;
            _SetKey = p_SetKey;
        }
    }

    public static class Compressor
    {
        public static byte[] CompressViewState(byte[] uncompData)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                CompressionMode mode = CompressionMode.Compress;
                // Use the newly created memory stream for the compressed data.
                using (GZipStream gzip = new GZipStream(mem, mode, true))
                {
                    //Writes compressed byte to the underlying
                    //stream from the specified byte array.
                    gzip.Write(uncompData, 0, uncompData.Length);
                }
                return mem.ToArray();
            }
        }

        public static byte[] DecompressViewState(byte[] compData)
        {
            GZipStream gzip;
            using (MemoryStream inputMem = new MemoryStream())
            {
                inputMem.Write(compData, 0, compData.Length);
                // Reset the memory stream position to begin decompression.
                inputMem.Position = 0;
                CompressionMode mode = CompressionMode.Decompress;
                gzip = new GZipStream(inputMem, mode, true);


                using (MemoryStream outputMem = new MemoryStream())
                {
                    // Read 1024 bytes at a time
                    byte[] buf = new byte[1024];
                    int byteRead = -1;
                    byteRead = gzip.Read(buf, 0, buf.Length);
                    while (byteRead > 0)
                    {
                        //write to memory stream
                        outputMem.Write(buf, 0, byteRead);
                        byteRead = gzip.Read(buf, 0, buf.Length);
                    }
                    gzip.Close();
                    return outputMem.ToArray();
                }
            }
        }



        //public void ActiveTab(MasterPage obj)
        //{

        //}
    }




}
