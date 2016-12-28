﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Linq;


namespace IQMedia.Model
{
    public class IQAgent_SearchRequestModel : IEquatable<IQAgent_SearchRequestModel>
    {
        public string QueryName { get; set; }
        public long ID { get; set; }
        public Guid ClientGUID { get; set; }
        public Int32? Query_Version { get; set; }
        public string SearchTerm { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsRestrictedMedia { get; set; }
        public Int16 IsActive { get; set; }

        public bool Equals(IQAgent_SearchRequestModel other)
        {
            if (this.ID != other.ID)
                return false;

            return true;
        }
    }

    //public class IQAgentDropDown_TV_DMA
    //{
    //    public string DMANumber { get; set; }

    //    public string DMAName { get; set; }
    //}
    //public class IQAgentDropDown_TV_Class
    //{
    //    public string ClassID { get; set; }
    //    public string ClassName { get; set; }
    //}
    //public class IQAgentDropDown_TV_Station
    //{
    //    public string IQ_Station_ID { get; set; }

    //    public string Station_Call_Sign { get; set; }
    //}

    //public class IQAgentDropDown_TV_Affiliate
    //{
    //    public string AffiliateName { get; set; }

    //    public string AffiliateNum { get; set; }
    //}

    //public class IQAgentDropDown_TV_Region
    //{
    //    public string RegionNum { get; set; }

    //    public string RegionName { get; set; }
    //}

    //public class IQAgentDropDown_TV_Country
    //{
    //    public string CountryNum { get; set; }

    //    public string CountryName { get; set; }
    //}

    public class IQAgentDropDown_NM_Genere
    {
        public int ID { get; set; }

        public string Label { get; set; }
    }
    public class IQAgentDropDown_NM_Category
    {
        public int ID { get; set; }

        public string Label { get; set; }
    }
    public class IQAgentDropDown_NM_PublicationCategory
    {
        public int ID { get; set; }

        public string Label { get; set; }
    }
    public class IQAgentDropDown_NM_Region
    {
        public int ID { get; set; }

        public string Label { get; set; }
    }
    public class IQAgentDropDown_SM_SourceCategory
    {
        public int ID { get; set; }

        public string Label { get; set; }

        public string Value { get; set; }
    }
    public class IQAgentDropDown_SM_SourceType
    {
        public int ID { get; set; }

        public string Label { get; set; }
    }

    public class IQAgentSearchRequest_DropDown
    {
        public List<IQ_Dma> TV_DMAList { get; set; }

        public List<IQ_Class> TV_ClassList { get; set; }

        public List<IQ_Station> TV_StationList { get; set; }

        public List<Station_Affil> TV_AffiliateList { get; set; }

        public List<IQ_Region> TV_RegionList { get; set; }

        public List<IQ_Country> TV_CountryList { get; set; }

        public List<IQAgentDropDown_NM_Genere> NM_GenereList { get; set; }

        public List<IQAgentDropDown_NM_Category> NM_CategoryList { get; set; }

        public List<IQAgentDropDown_NM_PublicationCategory> NM_PublicationCategoryList { get; set; }

        public List<IQAgentDropDown_NM_Region> NM_RegionList { get; set; }

        public List<IQAgentDropDown_SM_SourceCategory> SM_SourceCategoryList { get; set; }

        public List<IQAgentDropDown_SM_SourceType> SM_SourceTypeList { get; set; }

        public Dictionary<string,string> CountryList { get; set; }

        public List<string> LanguageList { get; set; }
    }

    public partial class IQAgentSearchRequestPost
    {
        // Do not change these properties name. This names are mapped with "~/Views/Shared/_SetupIQAgetnSetupAddEdit.cshtml"

        public long hdnIQAgentSetupAddEditKey { get; set; }
        public string txtIQAgentSetupTitle { get; set; }
        public string txtIQAgentSetupSearchTerm { get; set; }
        public string chkIQAgentSetup_TV { get; set; }
        public string chkIQAgentSetup_NM { get; set; }
        public string chkIQAgentSetup_SM { get; set; }
        public string chkIQAgentSetup_FB { get; set; }
        public string chkIQAgentSetup_TW { get; set; }
        public string chkIQAgentSetup_TM { get; set; }
        public string chkIQAgentSetup_PM { get; set; }
        public string chkIQAgentSetup_PQ { get; set; }

        // TV Params
        public string txtIQAgentSetupProgramTitle { get; set; }
        public string txtIQAgentSetupAppearing { get; set; }
        public List<string> ddlIQAgentSetupCategory_TV { get; set; }
        public List<string> ddlIQAgentSetupDMA_TV { get; set; }
        public List<string> ddlIQAgentSetupStation_TV { get; set; }
        public List<string> ddlIQAgentSetupAffiliate_TV { get; set; }
        public List<string> ddlIQAgentSetupRegion_TV { get; set; }
        public List<string> ddlIQAgentSetupCountry_TV { get; set; }
        public string txtIQAgentSetupSearchTerm_TV { get; set; }
        public bool? chkIQAgentSetupUserMasterSearchTerm_TV { get; set; }
        public string txtIQAgentSetupZipCodes { get; set; }

        // Online News Params
        public string txtIQAgentSetupPublication_NM { get; set; }
        public List<string> ddlIQAgentSetupCategory_NM { get; set; }
        public List<string> ddlIQAgentSetupPublicationCategory_NM { get; set; }
        public List<string> ddlIQAgentSetupGenere_NM { get; set; }
        public List<string> ddlIQAgentSetupRegion_NM { get; set; }
        public List<string> ddlIQAgentSetupLanguage_NM { get; set; }
        public List<string> ddlIQAgentSetupCountry_NM { get; set; }
        public string txtIQAgentSetupSearchTerm_NM { get; set; }
        public bool? chkIQAgentSetupUserMasterSearchTerm_NM { get; set; }


        // Social Media Params
        public string txtIQAgentSetupSource_SM { get; set; }
        public string txtIQAgentSetupAuthor_SM { get; set; }
        public string txtIQAgentSetupTitle_SM { get; set; }
        public List<string> ddlIQAgentSetupSourceType_SM { get; set; }
        public string txtIQAgentSetupSearchTerm_SM { get; set; }
        public bool? chkIQAgentSetupUserMasterSearchTerm_SM { get; set; }

        // Facebook Params
        public string txtIQAgentSetupFBPageID { get; set; }
        public string txtIQAgentSetupFBPage { get; set; }
        public string txtIQAgentSetupExcludeFBPageID { get; set; }
        public string txtIQAgentSetupExcludeFBPage { get; set; }
        public bool? chkIQAgentSetupIncludeDefault { get; set; }
        public string txtIQAgentSetupSearchTerm_FB { get; set; }
        public bool? chkIQAgentSetupUserMasterSearchTerm_FB { get; set; }

        // Twitter Params
        public string txtIQAgentSetupActor_TW { get; set; }
        public string txtIQAgentSetupFollowersCount_From_TW { get; set; }
        public string txtIQAgentSetupFollowersCount_To_TW { get; set; }
        public string txtIQAgentSetupFriendsCount_From_TW { get; set; }
        public string txtIQAgentSetupFriendsCount_To_TW { get; set; }
        public string txtIQAgentSetupKloutScore_From_TW { get; set; }
        public string txtIQAgentSetupKloutScore_To_TW { get; set; }
        public string txtIQAgentSetupGnipTag_TW { get; set; }
        public string txtIQAgentSetupSearchTerm_TW { get; set; }
        public bool? chkIQAgentSetupUserMasterSearchTerm_TW { get; set; }

        // ProQuest Params
        public string txtIQAgentSetupPublication_PQ { get; set; }
        public string txtIQAgentSetupAuthor_PQ { get; set; }
        public List<string> ddlIQAgentSetupLanguage_PQ { get; set; }
        public string txtIQAgentSetupSearchTerm_PQ { get; set; }
        public bool? chkIQAgentSetupUserMasterSearchTerm_PQ { get; set; }

        public string txtIQAgentSetupExcludeDomains_NM { get; set; }
        public string txtIQAgentSetupExcludeDomains_SM { get; set; }
        public string txtIQAgentSetupExcludeHandles_TW { get; set; }
    }    

    public class IQNotifationSettingsModel
    {
        public long IQNotificationKey { get; set; }

        [XmlElement("SearchRequestList")]
        [XmlArrayItem("SearchRequestID")]
        public List<Int64> SearchRequestList { get; set; }

        public string Notification_Time { get; set; }

        public Int64 ReportImageID { get; set; }

        public short? Notification_Day { get; set; }

        [XmlElement("EmailAddressList")]
        [XmlArrayItem("EmailAddress")]
        public List<string> Notification_Address { get; set; }

        public string SearchRequestNames { get; set; }

        public IQMedia.Shared.Utility.CommonFunctions.IQNotificationFrequency Frequency { get; set; }

        [XmlElement("MediaTypeList")]
        [XmlArrayItem("MediaType")]
        public List<string> MediaTypeList { get; set; }

        public bool UseRollup { get; set; }
    }

    public class EmailAddressList
    {
        public EmailAddressList()
        {
            EmailAddress = new List<string>();
        }

        [XmlElement("EmailAddress")]
        public List<string> EmailAddress { get; set; }
    }

    public class SearchRequestIDList
    {
        public SearchRequestIDList()
        {
            SearchRequestID = new List<Int64>();
        }

        [XmlElement("SearchRequestID")]
        public List<Int64> SearchRequestID { get; set; }
    }

    public class IQNotifationSettingsPostModel
    {
        public IQNotifationSettingsModel IQNotifationSettings { get; set; }
        public IQNotifationSettings_DropDown IQNotifationSettings_DropDown { get; set; }
    }

    public class IQNotifationSettings_DropDown
    {
        public List<IQClient_CustomImageModel> ReportImageList { get; set; }
        public List<IQAgent_SearchRequestModel> SearchRequestList { get; set; }
    }

    public class MediaTypeList 
    {
        public MediaTypeList()
        {
            MediaType = new List<string>();
        }

        [XmlElement("MediaType")]
        public List<string> MediaType { get; set; }
    }
}
namespace IQMedia.Model.IQAgentXML
{
    public class SearchRequest 
    {
        public SearchRequest()
        {
            
        }

        public string SearchTerm { get; set; }

        public TV TV { get; set; }
        public News News { get; set; }
        public SocialMedia SocialMedia { get; set; }
        public Facebook Facebook { get; set; }
        public Twitter Twitter { get; set; }
        public TM TM { get; set; }        
        public PM PM { get; set; }
        public PQ PQ { get; set; }

        // These properties to determined whether include node while serialization or not. Don't rename or change these properties name
        // Pattern = PeropertyName + "Specified"

        [System.Xml.Serialization.XmlIgnore]
        public bool TVSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool NewsSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool SocialMediaSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool FacebookSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool TwitterSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool TMSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool PMSpecified { get; set; }

        [System.Xml.Serialization.XmlIgnore]
        public bool PQSpecified { get; set; }
    }

    public class TV
    {
        public TV()
        {
            IQ_Dma_Set = new IQ_Dma_Set();
            IQ_Station_Set = new IQ_Station_Set();
            Station_Affiliate_Set = new Station_Affiliate_Set();
            IQ_Class_Set = new IQ_Class_Set();
            IQ_Region_Set = new IQ_Region_Set();
            IQ_Country_Set = new IQ_Country_Set();
        }
        public string ProgramTitle { get; set; }
        
        public string Appearing { get; set; }

        public MediumSearchTerm SearchTerm { get; set; }
        
        [XmlElement("IQ_Dma_Set")]
        public IQ_Dma_Set IQ_Dma_Set { get; set; }

        [XmlElement("IQ_Station_Set")]
        public IQ_Station_Set IQ_Station_Set { get; set; }

        [XmlElement("Station_Affiliate_Set")]
        public Station_Affiliate_Set Station_Affiliate_Set { get; set; }
        
        [XmlElement("IQ_Class_Set")]
        public IQ_Class_Set IQ_Class_Set { get; set; }

        [XmlElement("IQ_Region_Set")]
        public IQ_Region_Set IQ_Region_Set { get; set; }

        [XmlElement("IQ_Country_Set")]
        public IQ_Country_Set IQ_Country_Set { get; set; }

        [XmlArrayItem("ZipCode")]
        public List<string> ZipCodes { get; set; }
    }
    public class IQ_Dma_Set
    {
        public IQ_Dma_Set()
        {
            IQ_Dma = new List<IQ_Dma>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }
        
        [XmlAttribute]
        public string SelectionMethod { get; set; }
        
        [XmlElement("IQ_Dma")]
        public List<IQ_Dma> IQ_Dma { get; set; }
    }
    public class IQ_Dma
    {
        public string num { get; set; }
        public string name { get; set; }
    }
    public class IQ_Station_Set
    {
        public IQ_Station_Set()
        {
            IQ_Station_ID = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("IQ_Station_ID")]
        public List<string> IQ_Station_ID { get; set; }
    }

    public class Station_Affiliate_Set
    {
        public Station_Affiliate_Set()
        {
            Station_Affil = new List<Station_Affil>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("Station_Affil")]
        public List<Station_Affil> Station_Affil { get; set; }
    }

    public class Station_Affil
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class IQ_Class_Set
    {
        public IQ_Class_Set()
        {
            IQ_Class = new List<IQ_Class>();
        }

        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("IQ_Class")]
        public List<IQ_Class> IQ_Class { get; set; }
    }
    public class IQ_Class
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class IQ_Country_Set
    {
        public IQ_Country_Set()
        {
            IQ_Country = new List<IQ_Country>();
        }

        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("IQ_Country")]
        public List<IQ_Country> IQ_Country { get; set; }
    }
    public class IQ_Country
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class IQ_Region_Set
    {
        public IQ_Region_Set()
        {
            IQ_Region = new List<IQ_Region>();
        }

        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("IQ_Region")]
        public List<IQ_Region> IQ_Region { get; set; }
    }

    public class IQ_Region
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    public class News
    {
        public News()
        {
            NewsCategory_Set = new NewsCategory_Set();
            PublicationCategory_Set = new PublicationCategory_Set();
            Genre_Set = new Genre_Set();
            Region_Set = new Region_Set();
            Language_Set = new Language_Set();
            Country_Set = new Country_Set();
        }
        [XmlArrayItem("Publication")]
        public List<string> Publications { get; set; }

        public MediumSearchTerm SearchTerm { get; set; }

        [XmlElement("NewsCategory_Set")]
        public NewsCategory_Set NewsCategory_Set { get; set; }

        [XmlElement("PublicationCategory_Set")]
        public PublicationCategory_Set PublicationCategory_Set { get; set; }

        [XmlElement("Genre_Set")]
        public Genre_Set Genre_Set { get; set; }

        [XmlElement("Region_Set")]
        public Region_Set Region_Set { get; set; }

        [XmlElement("Country_Set")]
        public Country_Set Country_Set { get; set; }

        [XmlElement("Language_Set")]
        public Language_Set Language_Set { get; set; }

        [XmlArrayItem("domain")]
        public List<string> ExlcudeDomains { get; set; }
    }
    public class NewsCategory_Set
    {
        public NewsCategory_Set()
        {
            NewsCategory = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("NewsCategory")]
        public List<string> NewsCategory { get; set; }
    }
    public class PublicationCategory_Set
    {
        public PublicationCategory_Set()
        {
            PublicationCategory = new List<string>();
        }

        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("PublicationCategory")]
        public List<string> PublicationCategory { get; set; }
    }
    public class Genre_Set
    {
        public Genre_Set()
        {
            Genre = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("Genre")]
        public List<string> Genre { get; set; }
    }
    public class Region_Set
    {
        public Region_Set()
        {
            Region = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("Region")]
        public List<string> Region { get; set; }
    }

    public class Country_Set
    {
        public Country_Set()
        {
            Country = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("Country")]
        public List<string> Country { get; set; }
    }

    public class Language_Set
    {
        public Language_Set()
        {
            Language = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("Language")]
        public List<string> Language { get; set; }
    }

    public class SocialMedia
    {
        public SocialMedia()
        {
            SourceType_Set = new SourceType_Set();
        }
        [XmlArrayItem("Source")]
        public List<string> Sources { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        public MediumSearchTerm SearchTerm { get; set; }

        [XmlElement("SourceType_Set")]
        public SourceType_Set SourceType_Set { get; set; }

        [XmlArrayItem("domain")]
        public List<string> ExlcudeDomains { get; set; }
    }

    public class SourceType_Set
    {
        public SourceType_Set()
        {
            SourceType = new List<string>();
        }
        [XmlAttribute]
        public bool IsAllowAll { get; set; }

        [XmlElement("SourceType")]
        public List<string> SourceType { get; set; }
    }

    public class Facebook
    {
        public Facebook()
        { }
        [XmlArrayItem("FBPage")]
        public List<FBPage> FBPages { get; set; }

        [XmlArrayItem("ExcludeFBPage")]
        public List<FBPage> ExcludeFBPages { get; set; }

        public bool IncludeDefaultPages { get; set; }

        public MediumSearchTerm SearchTerm { get; set; }
    }

    public class FBPage
    {
        public string ID { get; set; }
        public string Page { get; set; }
    }

    public class Twitter
    {
        public Twitter()
        {
            ActorFriendsRange = new IQAgentXML.ActorFriendsRange();
            ActorFollowersRange = new IQAgentXML.ActorFollowersRange();
            KloutScoreRange = new KloutScoreRange();
        }
        public string Actor { get; set; }

        public MediumSearchTerm SearchTerm { get; set; }

        [XmlArrayItem("GnipTag")]
        public List<Guid> GnipTagList { get; set; }

        [XmlElement("ActorFriendsRange")]
        public ActorFriendsRange ActorFriendsRange { get; set; }

        [XmlElement("ActorFollowersRange")]
        public ActorFollowersRange ActorFollowersRange { get; set; }

        [XmlElement("KloutScoreRange")]
        public KloutScoreRange KloutScoreRange { get; set; }

        [XmlArrayItem("handle")]
        public List<string> ExclusionHandles { get; set; }
    }
    public class ActorFriendsRange
    {
        public string From { get; set; }
        public string To { get; set; }
    }
    public class ActorFollowersRange
    {
        public string From { get; set; }
        public string To { get; set; }
    }
    public class KloutScoreRange
    {
        public string From { get; set; }
        public string To { get; set; }
    }

    public class TM
    {
        public string TVEyesSearchGUID { get; set; }

        public string SearchTerm { get; set; }
    }

    public class PM
    {
        public MediumSearchTerm SearchTerm { get; set; }
        
        private string blpmXml;

        [XmlAnyElement]
        public XmlElement[] DocumentNodes { get; set; }

        [XmlIgnore]
        public string BLPMXml
        {
            get
            {
                if (this.blpmXml == null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<PM>");
                    foreach (var node in this.DocumentNodes)
                    {
                        sb.Append(node.OuterXml);
                    }
                    sb.Append("</PM>");
                    this.blpmXml = XElement.Parse(sb.ToString()).ToString();
                }

                return this.blpmXml;
            }            
        }
    }

    public class PQ
    {
        public PQ()
        {
            Language_Set = new Language_Set();
        }

        public MediumSearchTerm SearchTerm { get; set; }

        [XmlArrayItem("Publication")]
        public List<string> Publications { get; set; }

        [XmlArrayItem("Author")]
        public List<string> Authors { get; set; }

        [XmlElement("Language_Set")]
        public Language_Set Language_Set { get; set; }
    }

    public class MediumSearchTerm
    {
        [XmlAttribute]
        public bool IsUserMaster { get; set; }

        [XmlText(typeof(String))]
        public string SearchTerm { get; set; }
    }
}
