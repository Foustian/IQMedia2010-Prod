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
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Globalization;
using System.Diagnostics;
using System.IO.Compression;
using System.Configuration;

namespace IQMediaGroup.Common.IOS.Util
{
    /// <summary>
    /// This Class contains common functions
    /// </summary>
    public static class CommonFunctions
    {
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

        public static string EncryptStringAES(string rawString, string key, string iv)
        {
            byte[] encrypted;


            UTF8Encoding encoding = new UTF8Encoding();

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Key = encoding.GetBytes(key);
                aesManaged.IV = encoding.GetBytes(iv);

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

    }
}
