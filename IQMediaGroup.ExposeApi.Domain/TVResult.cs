using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class TVResult
    {
        #region Primitive Properties

        public long SeqID
        {
            get;
            set;
        }

        public string ProgramTitle
        {
            get;
            set;
        }

        public long SRID
        {
            get;
            set;
        }

        public Nullable<System.DateTime> GMTDateTime
        {
            get;
            set;
        }

        public string Station
        {
            get;
            set;
        }

        public Nullable<System.DateTime> MediaDateTime
        {
            get;
            set;
        }

        public string DmaName
        {
            get;
            set;
        }

        public Nullable<int> HitCount
        {
            get;
            set;
        }

        public Nullable<int> Audience
        {
            get;
            set;
        }

        public Nullable<decimal> MediaValue
        {
            get;
            set;
        }

        public string HighLights
        {
            get;
            set;
        }

        public Nullable<int> PositiveSentiment
        {
            get;
            set;
        }

        public Nullable<int> NegativeSentiment
        {
            get;
            set;
        }

        public System.Guid VideoGuid
        {
            get;
            set;
        }

        public string CC
        {
            get;
            set;
        }

        public string StationLogo
        {
            get;
            set;
        }

        public string URL
        {
            get;
            set;
        }

        public Nullable<long> ParentID
        {
            get;
            set;
        }

        public string ThumbUrl
        {
            get;
            set;
        }

        [XmlIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public System.DateTime RL_Date
        {
            get;
            set;
        }

        [XmlIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public int RL_Time
        {
            get;
            set;
        }

        [XmlIgnore]
        [System.Runtime.Serialization.IgnoreDataMember]
        public string StationID
        {
            get;
            set;
        }

        #endregion
    }
}
