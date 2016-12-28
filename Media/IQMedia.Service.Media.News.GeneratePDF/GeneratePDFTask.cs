using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IQMedia.Service.Media.News.GeneratePDF
{
    internal class GeneratePDFTask : IEquatable<GeneratePDFTask>
    {
        private Int64 _ArticleID;
        
        public Int64 ArticleID { get{return _ArticleID;} }

        private string _Url;
        public string Url { get { return _Url; } }

        private DateTime _Harvest_Time;
        public DateTime Harvest_Time { get { return _Harvest_Time; } }

        private int _RootPathID;
        public int RootPathID { get { return _RootPathID; } }

        public  TskStatus Status { get; set; }

        internal GeneratePDFTask(Int64 p_ArticleID, string p_Url, DateTime p_Harvest_Time, int p_RootPathID)
        {
            _ArticleID = p_ArticleID;
            _Url = p_Url;
            _Harvest_Time = p_Harvest_Time;
            _RootPathID = p_RootPathID;
        }

        public bool Equals(GeneratePDFTask other)
        {
            return ArticleID.Equals(other.ArticleID);
        }

        public enum TskStatus
        {
            IN_PROCESS,
            Invalid_Client,
            File_Generated,
            File_Not_Generated,
            Unable_To_Copy,
            Generated,
            Exception,
            Not_Returned_From_,
            TRACKING_FAILED
        }
    }
}
