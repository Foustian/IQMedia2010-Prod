using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class RawMedia
    {
        #region Primitive Properties

        [DataMember]
        public System.Guid RawMediaID
        {
            get;
            set;
        }

        [DataMember]
        public string StationLogo
        {
            get;
            set;
        }

        [DataMember]
        public string ProgramTitle
        {
            get;
            set;
        }

        [DataMember]
        public string DmaName
        {
            get;
            set;
        }

        [DataMember]
        public System.DateTime DateTime
        {
            get;
            set;
        }

        [DataMember]
        public Nullable<int> Hits
        {
            get { return _hits; }
            set { _hits = value; }
        }
        private Nullable<int> _hits = 0;

        [DataMember]
        public string URL
        {
            get;
            set;
        }

        [DataMember]
        public string Station
        {
            get;
            set;
        }

        [DataMember]
        public string Highlights
        {
            get;
            set;
        }

        public bool ShouldSerializeHighlights()
        {
            return !string.IsNullOrEmpty(Highlights);
        }

        #endregion
    }
}
