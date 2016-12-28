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
//using IQMediaGroup.Core.Enumeration;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Diagnostics;
using System.IO.Compression;
using System.Threading;
using System.ComponentModel;
using System.Reflection;

namespace IQMediaGroup.Common.Util
{
    /// <summary>
    /// This Class contains common functions
    /// </summary>
    public static class CommonFunctions
    {
        private static string AesKeyLicense = "6D372F5167584155694672674D486B67";
        private static string AesIVLicense = "516341644D4A3373";
        private static Random rd = new Random();
        
        //public static System.Threading.ThreadLocal<int> _TLSeqID = new ThreadLocal<int>(() => rd.Next());

        public static int GetRandomInt()
        { 
            return rd.Next();
        }

        public static List<string> osList = new List<string>
        {
            "windows nt\\s?(\\d+(\\.\\d+)*)?",
            "windows xp",
            "windows me",
            "win98",
            "win95",
            "win16",
            "android\\s?(\\d+(\\.\\d+)*)?",
            "iphone os\\s?(\\d+(_\\d+)+)",
            "iphone\\s*(\\d+(_\\d+)+)",
            "iphone",
            "ipod",
            "ipad",
            "windows phone os\\s?(\\d+(\\.\\d+)*)?",
            "macintosh|mac os x",
            "mac_powerpc",
            "linux",
            "ubuntu",
            "blackberry",
            "webos",
            "symbian(/\\d+(\\.\\d+)*)?",
            "symbianos(/\\d+(\\.\\d+)*)?",
            "symbianos(\\s\\d+(\\.\\d+)*)?",
            "SymbOS",
            "Windows CE"
        };

        public static Dictionary<string, string> deviceList = new Dictionary<string, string>{
            {"Xoom","Motorola Xoom"},
            {"Transformer TF101","Asus Transforrmer TF101"},
            {"Transformer Prime TF201","Asus Transformer Prime TF201"},
            {"SPH-M900","SPH-M900"},
            {"SPH-L710","Galaxy S 3"},
            {"SonyEricssonZ800","SonyEricsson Z800i"},
            {"SonyEricssonX1i","Xperia X1"},
            {"SonyEricssonX10i","SonyEricsson X10i"},
            {"SonyEricssonW995","SonyEricsson W995"},
            {"SonyEricssonW950i","SonyEricsson W950i"},
            {"SonyEricssonW850i","SonyEricsson W850i"},
            {"SonyEricssonW810i","SonyEricsson W810i"},
            {"SonyEricssonW660i","SonyEricsson W660i"},
            {"SonyEricssonW580i","SonyEricsson W580i"},
            {"SonyEricssonT68","SonyEricsson T68"},
            {"SonyEricssonT650i","SonyEricsson T650i"},
            {"SonyEricssonT610","SonyEricsson T610"},
            {"SonyEricssonT100","SonyEricsson T100"},
            {"SonyEricssonS500i","SonyEricsson S500i"},
            {"SonyEricssonP100","SonyEricsson P100"},
            {"SonyEricssonK810i","SonyEricsson K810i"},
            {"SonyEricssonK800i","SonyEricsson K800i"},
            {"SonyEricssonK750i","SonyEricsson K750i"},
            {"SonyEricssonK610i","SonyEricsson K610i"},
            {"SonyEricssonK550i","SonyEricsson K550i"},
            {"SonyEricssonK310iv","SonyEricsson K310iv"},
            {"SonyEricsson P900","SonyEricsson P900"},
            {"SGHX820","X820"},
            {"SGHX210","SGH X210 (WML)"},
            {"SGH-A867","SGH-A867 - Netfront"},
            {"SCH-I800","SCH-I800"},
            {"NokiaX7","Nokia X7"},
            {"NokiaN97","Nokia N97"},
            {"NokiaN950","Nokia N950"},
            {"NokiaN95","Nokia N95"},
            {"NokiaN8-00","NokiaN8-00"},
            {"NokiaN8","Nokia N8"},
            {"NokiaN73","Nokia N73"},
            {"NokiaN70","Nokia N70"},
            {"NokiaE63","Nokia E63"},
            {"NokiaE6-00","Nokia E6-00"},
            {"NokiaC7","Nokia C7"},
            {"NokiaC6-01","Nokia C6-01"},
            {"Nokia9500","Nokia 9500"},
            {"Nokia7250","Nokia 7250"},
            {"Nokia6630","Nokia 6630"},
            {"Nokia6230","Nokia 6230"},
            {"Nokia6120c","Nokia 6120 Classic"},
            {"Nokia5700","Nokia 5700"},
            {"Nokia3230","Nokia 3230"},
            {"Nokia 6600","Nokia 6600"},
            {"Nexus 7","Nexus 7"},
            {"Nexus 6","Nexus 6"},
            {"Nexus 5","Nexus 5"},
            {"Nexus 4","Nexus 4"},
            {"Nexus","Nexus"},
            {"MZ604","Motorola Xoom (32gb}, WiFi)"},
            {"MZ601","Motorola Xoom (3G)"},
            {"LX265","Rumor2 LX265 - Polaris"},
            {"Lumia 920","Nokia Lumia 920"},
            {"Lumia 620","Nokia Lumia 620"},
            {"LIFETAB_P9514","Medion LifeTab"},
            {"LG-LX550","Fusic LX550"},
            {"LG-GC900","LG GC900 Viewty"},
            {"LG V909","LG Optimus Pad 8.9"},
            {"K1","Lenovo IdeaPad K1"},
            {"HTC_One_X","HTC One X"},
            {"HTC One X","HTC One X"},
            {"GT-P7100","GT-P7100 tablet"},
            {"GT-P1000M","GT-P1000M"},
            {"GT-I5700","GT-I5700"},
            {"GT P7510","Samsung Galaxy Tab 10.1 (WiFi)"},
            {"GT P7500","Samsung Galaxy Tab 10.1 (WiFi+3G)"},
            {"GT P7310","Samsung Galaxy Tab 8.9 (WiFi)"},
            {"GT P6210","Samsung Galaxy Tab 7 (16gb}, WiFi)"},
            {"Galaxy S II","Galaxy S 2"},
            {"Galaxy Nexus","Galaxy Nexus"},
            {"Galaxy","Galaxy"},
            {"BNTV250","Nook Tablet (16gb)"},
            {"AT100","Toshiba AT100"},
            {"A501","Acer Iconia Tab A501"},
            {"A500","Acer Iconia Tab A500"},
            {"A100","Acer Iconia Tab A100"}
        };

        /// <summary>
        /// Email Send
        /// </summary>
        /// <param name="From"></param>
        /// <param name="To"></param>
        /// <param name="Subject"></param>
        /// <param name="Body"></param>
        /// <param name="EmailContent"></param>
        /// <returns></returns>
        public static string EmailSend(string From, string To, string Subject, string Body, string EmailContent)
        {
            try
            {

                SmtpClient _SmtpClient = new SmtpClient();
                string MailUserName = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerUser"));
                string MailPassword = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings.Get("SMTPServerPassword"));

                NetworkCredential _NetworkCredential = new NetworkCredential(MailUserName, MailPassword);


                _SmtpClient.Port = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings.Get("sSMTPPort"));
                _SmtpClient.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["sSmtpSSL"]);
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

                strEmailBodyfinal += "<div style=\"font-family:Verdana;font-size:11px;\">";
                if (!string.IsNullOrEmpty(Body))
                {

                    strEmailBodyfinal += Body.Replace("\r\n", "<br />") + "<br />";
                    strEmailBodyfinal += EmailContent + "<br />";
                    strEmailBodyfinal += "Thanks,<br /> iQMedia Corp <br /> www.iqmediacorp.com";
                }
                else
                {
                    strEmailBodyfinal += "Hi," + "<br />";
                    strEmailBodyfinal += "Please Check out following IQMedia Clip(s) <br />";
                    strEmailBodyfinal += EmailContent + "<br />";
                    strEmailBodyfinal += "Thanks,<br /> iQMedia Corp <br /> www.iqmediacorp.com";
                }

                strEmailBodyfinal = strEmailBodyfinal + "<br /><br />" + File.ReadAllText(HttpContext.Current.Server.MapPath("~/EmailPolicy.txt"), Encoding.UTF8);

                strEmailBodyfinal += "</div>";

                msg.Body = strEmailBodyfinal;
                msg.IsBodyHtml = true;

                _SmtpClient.Send(msg); //send email out


                return strEmailBodyfinal;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// base64 encryption
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <param name="SEncryptionKey"></param>
        /// <returns></returns>
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

        public static byte[] AesEncryptStringToBytes(string plainText)
        {
            byte[] encrypted;


            UTF8Encoding encoding = new UTF8Encoding();
            byte[] Key = encoding.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
            byte[] IV = encoding.GetBytes(ConfigurationManager.AppSettings["AesIV"]);

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

        public static string EncryptLicenseStringAES(string rawString)
        {
            byte[] encrypted;


            UTF8Encoding encoding = new UTF8Encoding();

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Key = encoding.GetBytes(AesKeyLicense);
                aesManaged.IV = encoding.GetBytes(AesIVLicense);

                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aesManaged.CreateEncryptor(aesManaged.Key, aesManaged.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(rawString);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }
            // Return the encrypted bytes from the memory stream.
            return Convert.ToBase64String(encrypted);
        }

        public static string AesDecryptStringFromBytes(string encrypteString)// byte[] cipherText, byte[] Key, byte[] IV)
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
                byte[] Key = encoding.GetBytes(ConfigurationManager.AppSettings["AesKey"]);
                byte[] IV = encoding.GetBytes(ConfigurationManager.AppSettings["AesIV"]);


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

        public static bool GenerateThumbnail(string location, int offset, string outputFile,string logUID="")
        {
            var startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.RedirectStandardError = true;
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            //TODO: Fix the path issue
            startInfo.FileName = ConfigurationManager.AppSettings["ffmpegPath"];

            startInfo.Arguments = String.Format("-y -an -vframes 1 -ss {0} -i \"{1}\" \"{2}\"",
                offset, location, outputFile);

            try
            {
                Log4NetLogger.Debug(logUID + startInfo.FileName + " " + startInfo.Arguments);

                using (var exeProcess = Process.Start(startInfo))
                {
                    exeProcess.WaitForExit();
                    var res = exeProcess.StandardError.ReadToEnd();
                    Log4NetLogger.Debug(logUID + res);
                }

                return File.Exists(outputFile);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(logUID,ex);
                return false;
            }
        }

        public static bool CopyFile(string sourceFile, string destinationFile, int bufferSize = (128*1024), string logUID = "")
        {
            if (String.IsNullOrWhiteSpace(sourceFile) || String.IsNullOrWhiteSpace(destinationFile))
            {
                Log4NetLogger.Warning(String.Format((logUID + "Source file '{0}' or Destination file '{1}' was not valid."), sourceFile, destinationFile));
                return false;
            }

            var success = false;
            var buffer = new byte[bufferSize];
            BinaryReader reader = null;
            BinaryWriter writer = null;

            try
            {
                var destinationPath = Path.GetDirectoryName(destinationFile);
                try
                {
                    if (!Directory.Exists(destinationPath))
                        Directory.CreateDirectory(destinationPath);
                }
                catch (Exception ex)
                {
                    Log4NetLogger.Warning(logUID + "Error creating directory: " + destinationPath);
                    throw new Exception("The directory could not be created.", ex);
                }

                //Set fileshare to NONE to attempt to minimize duplicates...
                reader = new BinaryReader(new FileStream(sourceFile, FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, FileOptions.SequentialScan));
                writer = new BinaryWriter(new FileStream(destinationFile, FileMode.Create, FileAccess.Write));

                //Log Progess
                Log4NetLogger.Debug(String.Format((logUID + "Copying file from '{0}' destination '{1}'"), sourceFile, destinationFile));

                int bytesRead;
                while ((bytesRead = reader.Read(buffer, 0, bufferSize)) > 0)
                    writer.Write(buffer, 0, bytesRead);

                if (File.Exists(destinationFile))
                {
                    //File copied successfully...Log Success
                    Log4NetLogger.Info(String.Format((logUID +  "File '{0}' successfully copied to destination '{1}'"), sourceFile, destinationFile));
                    success = true;
                }
                else
                {
                    //File didn't make it
                    throw new Exception("File didn't copy to destination successfully.");
                }
            }
            catch (FileNotFoundException ex)
            {
                Log4NetLogger.Warning(String.Format((logUID + "The source file '{0}' does not exist on the file system."), sourceFile), ex);
            }
            catch (IOException ex)
            {
                Log4NetLogger.Warning(String.Format((logUID + "The source file '{0}' is in use by another thread and will be skipped."), sourceFile), ex);
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(String.Format((logUID + "Error copying '{0}' to '{1}'."), sourceFile, destinationFile), ex);
            }
            finally
            {
                if (null != reader) reader.Dispose();
                if (null != writer) writer.Dispose();
                //Nullify the buffer to force it to the garbage collector incase this is part of the memory leak...
                buffer = null;
            }
            return success;
        }

        public static bool DeleteFile(string filePath, string logUID = "")
        {
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    Log4NetLogger.Info(String.Format((logUID + "File '{0}' successfully deleted."), filePath));
                    return true;
                }

                Log4NetLogger.Info(String.Format((logUID + "File '{0}' does not exist and doesn't need to be deleted."), filePath));
                return true;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error((logUID + "Error deleting file: " + filePath), ex);
                return false;
            }
        }

        public static object DeserialiazeXml(string p_XMLString, object p_Deserialization)
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

        public static string GetValueFromDescription<T>(string description)
        {

            string returnValue = string.Empty;
            var type = typeof(T);
            if (!type.IsEnum) throw new InvalidOperationException();
            foreach (var field in type.GetFields())
            {
                var attribute = Attribute.GetCustomAttribute(field,
                typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attribute != null)
                {
                    if (attribute.Description == description)
                    {
                        //returnValue = true;
                        returnValue = Convert.ToString((T)field.GetValue(null));
                    }
                    //return (T)field.GetValue(null);
                }

            }
            return returnValue;
            //throw new ArgumentException("Not found.", "description");
            // or return default(T); 
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public static T StringToEnum<T>(string name)
        {
            return (T)Enum.Parse(typeof(T), name);
        }

        /// <summary>
        /// Check if value of p_Value param is DBNull.value then return NULL else cast p_Value to p_type and return.
        /// </summary>
        /// <param name="p_Value">Value</param>
        /// <param name="p_type">Destination type</param>
        /// <returns></returns>
        public static dynamic CheckDBNullNReturnValue<T>(object p_Value)
        {
            var t = typeof(T);

            if (p_Value != DBNull.Value)
            {
                if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    t = Nullable.GetUnderlyingType(t);
                }

                return (T)Convert.ChangeType(p_Value, t);               
            }
            else if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                return null;
            }
            else
            {
                return default(T);
            }
        }
    }
}
