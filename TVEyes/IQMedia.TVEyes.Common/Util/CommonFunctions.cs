using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using System.IO;
using System.ComponentModel;

namespace IQMedia.TVEyes.Common.Util
{
    public static class CommonFunctions
    {

        public static string DecryptStringFromBytes_Aes(string encrypteString)// byte[] cipherText, byte[] Key, byte[] IV)
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
                byte[] Key = encoding.GetBytes("43DD9199E882F08814E1864B45E4F117");
                byte[] IV = encoding.GetBytes("40275DC0B57CD8D6");


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

        public enum CategoryType
        {
            [Description("Social Media")]
            SocialMedia,
            [Description("Online News")]
            NM,
            [Description("Print Media")]
            PM,
            [Description("TV")]
            TV,
            [Description("Twitter")]
            TW,
            [Description("Forum")]
            Forum,
            [Description("Blog")]
            Blog
        }
    }

    public class Article
    {
        public string ArticleID { get; set; }
        public string SearchTerm { get; set; }

    }

    
}
