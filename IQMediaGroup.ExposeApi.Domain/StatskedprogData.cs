using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;


namespace IQMediaGroup.ExposeApi.Domain
{
    [XmlRoot(ElementName = "TVMetaDataOutput")]
    [DataContract(Name = "TVMetaDataOutput")]
    public partial class StatskedprogData
    {
        [DataMember]
        public string Message { get; set; }

        [DataMember]
        public int Status { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]        
        public bool IsAllAffiliate { get; set; }

        [XmlIgnore]
        [IgnoreDataMember]        
        public bool IsAllDma { get; set; }

        [DataMember]
        public List<Affiliate> AffiliateList { get; set; }

        [DataMember]
        public List<Country> CountryList { get; set; }

        [DataMember]
        public List<Dma> DmaList { get; set; }

        [DataMember]
        public List<Class> ProgramCategoryList { get; set; }

        [DataMember]
        public List<Region> RegionList { get; set; }

        [DataMember]
        public List<Station> StationList { get; set; }
    }
}
