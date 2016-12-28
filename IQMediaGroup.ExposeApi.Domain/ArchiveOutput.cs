using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Dynamic;

namespace IQMediaGroup.ExposeApi.Domain
{
    public class ArchiveOutput
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public Int64 TotalResults { get; set; }
        public bool HasNextPage { get; set; }
        public IEnumerable<ArchiveMedia> ArchiveMediaList { get; set; }
    }
    /*
    [KnownType(typeof(MetaData))]
    public class ArchiveMedia
    {
        [DataMember]
        public string Affiliate { get; set; }

        [DataMember]
        public int? Audience { get; set; }

        [DataMember]
        public Guid ClipID { get; set; }

        [DataMember]
        public DateTime? CreatedDateTime { get; set; }

        [DataMember]
        public string Market { get; set; }

        [DataMember]
        public DateTime MediaDateTime { get; set; }

        [DataMember]
        public decimal? MediaValue { get; set; }

        [DataMember]
        public short? NegativeSentiment { get; set; }

        [DataMember]
        public short? PositiveSentiment { get; set; }

        [DataMember]
        public string Rank { get; set; }

        [DataMember]
        public Int64 SeqID { get; set; }

        [DataMember]
        public string StationID { get; set; }

        [DataMember]
        public string Text { get; set; }

        [DataMember]
        public string TimeZone { get; set; }

        [DataMember]
        public string Title { get; set; }

        [DataMember]
        public Int64? ParentID { get; set; }

        [DataMember]
        public Guid? CategoryGuid { get; set; }

        [DataMember]
        public string CategoryName { get; set; }

        [DataMember]
        public string ClipURL { get; set; }

        [DataMember]
        public string Keywords { get; set; }

        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public dynamic ClipMeta { get; set; }
    }
    */
    public class Meta
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }

    [Serializable]
    public class MetaData : DynamicObject, ISerializable
    {
        [XmlElement(Namespace = "")]
        Dictionary<string, object> dictionary = new Dictionary<string, object>();

        public override bool TryGetMember(
            GetMemberBinder binder, out object result)
        {
            string name = binder.Name.ToLower();

            return dictionary.TryGetValue(name, out result);
        }

        public override bool TrySetMember(
            SetMemberBinder binder, object value)
        {
            dictionary[binder.Name] = value;

            return true;
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            foreach (String key in dictionary.Keys)
            {
                info.AddValue(key.ToString(), dictionary[key]);
            }
        }

        public void SetMember(string p_Name, string p_Value)
        {
            dictionary[p_Name] = p_Value;
        }
    }

    public class ClipMeta
    {
        [XmlElement(Namespace = "")]
        public dynamic MetaData { get; set; }
    }
}
