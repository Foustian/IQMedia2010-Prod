using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;

namespace IQMediaGroup.CoreServices.Domain
{
    [Serializable]
    [XmlRoot(ElementName = "RecordFile")]
    public class RecordFileInput
    {
        public string FileExtension
        {
            get;
            set;
        }

        public Nullable<int> EndOffset
        {
            get;
            set;
        }

        public Nullable<System.Guid> SourceGuid
        {
            get;
            set;
        }

        public Nullable<int> RecordFileTypeID
        {
            get;
            set;
        }

        public Nullable<int> RootPathID
        {
            get;
            set;
        }

        public string Status
        {
            get;
            set;
        }

        public Nullable<bool> IsUGC
        {
            get;
            set;
        }

        public UGCMetaData UGCMetaData
        {
            get;
            set;
        }

        public string DestinationFile
        {
            get;
            set;
        }

        public string EndDate
        {
            get;
            set;
        }

        public string StartDate
        {
            get;
            set;
        }

        public Nullable<System.Guid> RecordfileID
        {
            get;
            set;
        }

       
    }
}
