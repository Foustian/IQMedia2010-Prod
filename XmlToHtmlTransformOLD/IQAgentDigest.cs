using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.IO;

namespace XmlToHtmlTransformOLD
{
    public class IQAgentDigest
    {
        public string ClientName { get; set; }

        public string ClientHeader { get; set; }

        public string ClientHeaderImagePath { get; set; }

        public string ImageBaseUrl { get; set; }

        public string SubDomainBaseUrl { get; set; }

        public string IQAgentDigestUrl { get; set; }

        [XmlArrayItem(ElementName = "Agent")]
        public List<string> Summary { get; set; }

        [XmlElement(ElementName = "SearchRequest")]
        public List<SearchRequest> SearchRequest { get; set; }

        public bool UseRollup { get; set; }
    }

    public class SearchRequest
    {

        public Int64 ID { get; set; }

        public string QueryName { get; set; }

        public string SearchRequestUrl { get; set; }

        public TV TV { get; set; }

        public News News { get; set; }

        public Forum Forum { get; set; }

        public Blog Blog { get; set; }

        public SocialMedia SocialMedia { get; set; }

        public Twitter Twitter { get; set; }

        public PrintMedia PrintMedia { get; set; }

        public TM TM { get; set; }

        [XmlElement(ElementName = "ProQuest")]
        public PQ PQ { get; set; }
    }

    public class TV
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName = "TVNode")]
        public List<TVNode> TVNode { get; set; }
    }

    public class TVNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public Int64? Audience { get; set; }

        public float? MediaValue { get; set; }

        public string Nielsen_Result { get; set; }

        public string National_Nielsen_Result { get; set; }

        public Int64? National_Audience { get; set; }

        public float? National_MediaValue { get; set; }

        public string Title { get; set; }

        public string HighlightingText { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string Url { get; set; }

        public string Market { get; set; }

        public string StationLogo { get; set; }

        public string ThumbnailImage { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName="ParentID", IsNullable=true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }

    }

    public class News
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName="NMNode")]
        public List<NMNode> NMNode { get; set; }
    }

    public class NMNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string Compete_Result { get; set; }

        public string TimeZone { get; set; }

        public Int64? Audience { get; set; }

        public float? MediaValue { get; set; }

        public string Title { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string Url { get; set; }

        public string HighlightingText { get; set; }

        public string CompeteUrl { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }
    }

    public class Blog
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName = "BlogNode")]
        public List<BlogNode> BlogNode { get; set; }
    }

    public class BlogNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string Compete_Result { get; set; }

        public string TimeZone { get; set; }

        public Int64? Audience { get; set; }

        public float? MediaValue { get; set; }

        public string Title { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string Url { get; set; }

        public string HighlightingText { get; set; }

        public string CompeteUrl { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }
    }

    public class Forum
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName="ForumNode")]
        public List<ForumNode> ForumNode { get; set; }
    }

    public class ForumNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public string Title { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string Url { get; set; }

        public string HighlightingText { get; set; }

        public string CompeteUrl { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }
    }

    public class SocialMedia
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName = "SMNode")]
        public List<SMNode> SMNode { get; set; }
    }

    public class SMNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public string Title { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string Url { get; set; }

        public string HighlightingText { get; set; }

        public string CompeteUrl { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }
    }

    public class Twitter
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName="TWNode")]
        public List<TWNode> TWNode { get; set; }
    }

    public class TWNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public string Url { get; set; }

        public string HighlightingText { get; set; }

        public string ActorDisplayname { get; set; }

        public string PreferredUserName { get; set; }

        public string ActorImage { get; set; }

        public int KloutScore { get; set; }

        public int FollowersCount { get; set; }

        public int FreiendsCount { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }

    }

    public class PrintMedia
    {

        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName = "PMNode")]
        public List<PMNode> PMNode { get; set; }

        
    }

    public class PMNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public string Title { get; set; }

        public string Pub_Name { get; set; }

        public string HighlightingText { get; set; }

        public int Circulation { get; set; }

        public string FileLocation { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }
    }

    public class TM
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName = "TMNode")]
        public List<TMNode> TMNode { get; set; }
    }

    public class TMNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public string Title { get; set; }

        public string DMARank { get; set; }

        public string HighlightingText { get; set; }

        public string Market { get; set; }

        public string TranscriptUrl { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        public Int64 MediaID { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }

    }
    
    public class PQ
    {
        public int NoOfTotalItems { get; set; }

        public int NoOfDisplayItems { get; set; }

        [XmlElement(ElementName = "PQNode")]
        public List<PQNode> PQNode { get; set; }
    }

    public class PQNode
    {
        public DateTime MediaDate { get; set; }

        public DateTime GmtDate { get; set; }

        public string TimeZone { get; set; }

        public string Title { get; set; }

        public string Pub_Name { get; set; }

        public string HighlightingText { get; set; }

        public Int64 MediaID { get; set; }

        public int PositiveSentiment { get; set; }

        public int NegativeSentiment { get; set; }

        [XmlElement(ElementName = "ParentID", IsNullable = true)]
        public string _ParentID { get; set; }

        public Int64? ParentID
        {
            get
            {
                Int64 val;

                return !string.IsNullOrWhiteSpace(_ParentID) && Int64.TryParse(_ParentID, out val) ? (Int64?)val : null;
            }
        }

        /*[XmlElement(ElementName = "Url", IsNullable = true)]
        public string _Url { get; set; }*/

        public string Url { get { 
                    string key = "0B358AB55C5D059DFFDD7028AD9985EB"; 
                    string autoGenIV="";
                    string encID = GenerateEncryptedUrl(key, Convert.ToString(MediaID), out autoGenIV, true);
                    string tempIV="";
                    string encIV = GenerateEncryptedUrl("6A26F02B6D9EB6DD68F85A012BD8322B", autoGenIV, out tempIV, false, "C6DBC2575C2652B01B3F80D27225058D");
                    string finalString = encID.Substring(0, 16) + encIV.Substring(16, 16) + encID.Length + encID.Substring(16) + encIV.Substring(0, 16) + encIV.Substring(32);
                    return finalString;
                } }

        private string GenerateEncryptedUrl(string p_Key, string p_Data, out string p_AutoGenIV, bool p_IsAutoGenIV = true, string p_IV="")
        {
            byte[] encrypted;
            p_AutoGenIV = p_IV;

            UTF8Encoding encoding = new UTF8Encoding();

            // Create an AesManaged object
            // with the specified key and IV.
            using (AesManaged aesManaged = new AesManaged())
            {
                aesManaged.Mode = CipherMode.CBC;
                aesManaged.Padding = PaddingMode.PKCS7;
                aesManaged.Key = encoding.GetBytes(p_Key);

                if(p_IsAutoGenIV)
                {
                    aesManaged.GenerateIV();
                }
                else
                {
                    aesManaged.IV = StringToByteArray(p_IV);
                }

                p_AutoGenIV = ByteArrayToString(aesManaged.IV);
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
                            swEncrypt.Write(p_Data);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return (ByteArrayToString(encrypted));
        }

        private string ByteArrayToString(byte[] ba)
        {
            string hex = BitConverter.ToString(ba);
            return hex.Replace("-", "");
        }

        private byte[] StringToByteArray(String hex)
        {
            int NumberChars = hex.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            return bytes;
        }
    }

}
