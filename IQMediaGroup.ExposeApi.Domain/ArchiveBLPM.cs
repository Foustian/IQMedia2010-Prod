using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    public partial class ArchiveBLPM : ArchiveMedia
    {
        [DataMember]
        public Nullable<System.Guid> CategoryGuid
        {
            get;
            set;
        }

        [DataMember]
        public string CategoryName
        {
            get;
            set;
        }

        [DataMember(Name = "Audience")]
        public int Circulation
        {
            get;
            set;
        }      

        [DataMember(Name="Url")]
        public string FileLocation
        {
            get;
            set;
        }        

        [DataMember(Name="Publisher")]
        public string Pub_Name
        {
            get;
            set;
        }

        [DataMember]
        public string Title
        {
            get;
            set;
        }       
    }
}