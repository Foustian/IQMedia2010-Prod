using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace IQMediaGroup.ExposeApi.Domain
{
    [DataContract]
    [Serializable]
    public class CreateTVAgentInput
    {
        [DataMember]
        public string SessionID { get; set; }

        [DataMember]
        public Guid ClientGUID { get; set; }

        [DataMember]
        public SearchRequestAgent SearchRequest { get; set; }
    }

    [XmlRoot(ElementName = "SearchRequest")]
    public class SearchRequestAgent
    {
        public string AgentName { get; set; }

        public TV TV { get; set; }
    }

    public class TV
    {
        public string ProgramTitle { get; set; }

        public string Appearing { get; set; }

        public string SearchTerm { get; set; }

        public List<TVDma> DmaList { get; set; }

        public List<Station> StationList { get; set; }

        public List<Station_Affil> AffiliateList { get; set; }

        public List<ProgramCategory> ProgramCategoryList { get; set; }

        public List<Region> RegionList { get; set; }

        public List<Country> CountryList { get; set; }
    }

    [XmlType(TypeName = "Dma")]
    public class TVDma
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class Station
    {
        public string name { get; set; }

        public string StationCallSign { get; set; }

        public string DmaName { get; set; }

        public string Affiliate { get; set; }

        public string Country { get; set; }

        public string Region { get; set; }
    }

    [XmlType(TypeName = "Affiliate")]
    public class Station_Affil
    {
        public string name { get; set; }
    }

    public class Region
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class ProgramCategory
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class Country
    {
        public string num { get; set; }
        public string name { get; set; }
    }
}
