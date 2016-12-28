using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedsSearch
{
    public class Hit
    {
        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        } Int64 _ID;

        public Int64 MediaID
        {
            get { return _MediaID; }
            set { _MediaID = value; }
        } Int64 _MediaID;

        public string ArticleID
        {
            get { return _ArticleID; }
            set { _ArticleID = value; }
        } string _ArticleID;

        public int ParentID
        {
            get { return _ParentID; }
            set { _ParentID = value; }
        } int _ParentID;

        public DateTime MediaDate
        {
            get { return _MediaDate; }
            set { _MediaDate = value; }
        } DateTime _MediaDate;

        public DateTime LocalDate
        {
            get { return _LocalDate; }
            set { _LocalDate = value; }
        } DateTime _LocalDate;

        public string MediaType
        {
            get { return _MediaType; }
            set { _MediaType = value; }
        } string _MediaType;

        public string MediaCategory
        {
            get { return _MediaCategory; }
            set { _MediaCategory = value; }
        } string _MediaCategory;

        public string v5MediaType
        {
            get { return _v5MediaType; }
            set { _v5MediaType = value; }
        } string _v5MediaType;

        public string v5MediaCategory
        {
            get { return _v5MediaCategory; }
            set { _v5MediaCategory = value; }
        } string _v5MediaCategory;

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        } string _Title;

        public Int16 PositiveSentiment
        {
            get { return _PositiveSentiment; }
            set { _PositiveSentiment = value; }
        } Int16 _PositiveSentiment = 0;

        public Int16 NegativeSentiment
        {
            get { return _NegativeSentiment; }
            set { _NegativeSentiment = value; }
        } Int16 _NegativeSentiment = 0;

        public int NumberOfHits
        {
            get { return _NumberOfHits; }
            set { _NumberOfHits = value; }
        } int _NumberOfHits;

        public decimal MediaValue
        {
            get { return _MediaValue; }
            set { _MediaValue = value; }
        } decimal _MediaValue;

        public decimal IQProminence
        {
            get { return _IQProminence; }
            set { _IQProminence = value; }
        } decimal _IQProminence;

        public decimal IQProminenceMultiplier
        {
            get { return _IQProminenceMultiplier; }
            set { _IQProminenceMultiplier = value; }
        } decimal _IQProminenceMultiplier;

        public string HighlightingText
        {
            get { return _HighlightingText; }
            set { _HighlightingText = value; }
        } string _HighlightingText;

        public Int64 SearchRequestID
        {
            get { return _SearchRequestID; }
            set { _SearchRequestID = value; }
        } Int64 _SearchRequestID;

        public string SearchRequest
        {
            get { return _SearchRequest; }
            set { _SearchRequest = value; }
        } string _SearchRequest;

        public string SearchAgentName
        {
            get { return _SearchAgentName; }
            set { _SearchAgentName = value; }
        } string _SearchAgentName;

        public int Audience
        {
            get { return _Audience; }
            set { _Audience = value; }
        } int _Audience;

        public string AudienceType
        {
            get { return _AudienceType; }
            set { _AudienceType = value; }
        } string _AudienceType;

        public string ThumbnailUrl
        {
            get { return _ThumbnailUrl; }
            set { _ThumbnailUrl = value; }
        } string _ThumbnailUrl;

        public string StationID
        {
            get { return _StationID; }
            set { _StationID = value; }
        } string _StationID;

        public string Outlet
        {
            get { return _Outlet; }
            set { _Outlet = value; }
        } string _Outlet;

        public string Market
        {
            get { return _Market; }
            set { _Market = value; }
        } string _Market;

        public Guid VideoGUID
        {
            get { return _VideoGUID; }
            set { _VideoGUID = value; }
        } Guid _VideoGUID;

        public string TimeZone
        {
            get { return _TimeZone; }
            set { _TimeZone = value; }
        } string _TimeZone;

        public Int16 IQLicense
        {
            get { return _IQLicense; }
            set { _IQLicense = value; }
        } Int16 _IQLicense;

        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        } string _Url;

        public string PlayerUrl
        {
            get { return _PlayerUrl; }
            set { _PlayerUrl = value; }
        } string _PlayerUrl;

        public string FileLocation
        {
            get { return _FileLocation; }
            set { _FileLocation = value; }
        } string _FileLocation;

        public string Publication
        {
            get { return _Publication; }
            set { _Publication = value; }
        } string _Publication;

        public string ActorPreferredName
        {
            get { return _ActorPreferredName; }
            set { _ActorPreferredName = value; }
        } string _ActorPreferredName;

        public int ActorFriendsCount
        {
            get { return _ActorFriendsCount; }
            set { _ActorFriendsCount = value; }
        } int _ActorFriendsCount;

        public Int64 NationalAudience
        {
            get { return _NationalAudience; }
            set { _NationalAudience = value; }
        } Int64 _NationalAudience;

        public string NationalAudienceType
        {
            get { return _NationalAudienceType; }
            set { _NationalAudienceType = value; }
        } string _NationalAudienceType;

        public decimal NationalMediaValue
        {
            get { return _NationalMediaValue; }
            set { _NationalMediaValue = value; }
        } decimal _NationalMediaValue;

        public string SearchTerm { get; set; }

        public string IQ_CC_Key { get; set; }

        public string Content { get; set; }

        public string DmaID { get; set; }

        public int StationIDNum { get; set; }

        public int Duration { get; set; }

        public List<string> Authors { get; set; }

        public string Copyright { get; set; }

        public DateTime AvailableDate { get; set; }

        public Int16 LanguageNum { get; set; }

        public string ArticleStats { get; set; }
    }
}
