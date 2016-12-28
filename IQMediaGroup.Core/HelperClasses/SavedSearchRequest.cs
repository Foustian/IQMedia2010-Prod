using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using System.Runtime.Serialization;

namespace IQMediaGroup.Core.HelperClasses
{
    [XmlRoot(ElementName = "SearchRequest")]
    [DataContract]
    public class SavedSearchRequest : IEquatable<SavedSearchRequest>
    {
        public string SearchTerm { get; set; }

        [XmlElement("TV")]
        public TV tv { get; set; }

        [XmlElement("Radio")]
        public Radio radio { get; set; }

        [XmlElement("News")]
        public News news { get; set; }


        [XmlElement(ElementName = "SocialMedia")]
        public SocialMediaElement socialMedia { get; set; }

        [XmlElement("Twitter")]
        public Twitter twitter { get; set; }


        public bool Equals(SavedSearchRequest other)
        {
            if (this.SearchTerm != other.SearchTerm)
                return false;

            #region TV Filter Equality

            if (this.tv != null && other.tv != null)
            {
                if (this.tv.tvDuration != null && other.tv.tvDuration != null)
                {
                    if (this.tv.tvDuration.DayDuration != other.tv.tvDuration.DayDuration)
                        return false;

                    if (this.tv.tvDuration.FromDate != other.tv.tvDuration.FromDate)
                        return false;

                    if (this.tv.tvDuration.ToDate != other.tv.tvDuration.ToDate)
                        return false;
                }
                else if ((this.tv.tvDuration != null && other.tv.tvDuration == null)
                    || (this.tv.tvDuration == null && other.tv.tvDuration != null))
                {
                    return false;
                }


                if (this.tv.ProgramTitle != other.tv.ProgramTitle)
                    return false;

                if (this.tv.Appearing != other.tv.Appearing)
                    return false;




                if (this.tv.iQDmaSet != null && other.tv.iQDmaSet != null)
                {
                    if (this.tv.iQDmaSet.SelectionMethod != other.tv.iQDmaSet.SelectionMethod)
                        return false;

                    if (this.tv.iQDmaSet.listofIQDma != null && other.tv.iQDmaSet.listofIQDma != null)
                    {
                        if (this.tv.iQDmaSet.listofIQDma.Count() != other.tv.iQDmaSet.listofIQDma.Count())
                            return false;


                        IEnumerable<string> obj1 = this.tv.iQDmaSet.listofIQDma.Select(a => a.name).Except(other.tv.iQDmaSet.listofIQDma.Select(a => a.name));
                        IEnumerable<string> obj2 = other.tv.iQDmaSet.listofIQDma.Select(a => a.name).Except(this.tv.iQDmaSet.listofIQDma.Select(a => a.name));
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }
                    }
                    else if ((this.tv.iQDmaSet.listofIQDma != null && other.tv.iQDmaSet.listofIQDma == null)
                        || (this.tv.iQDmaSet.listofIQDma == null && other.tv.iQDmaSet.listofIQDma != null))
                    {
                        return false;
                    }
                }
                else if ((this.tv.iQDmaSet != null && other.tv.iQDmaSet == null) || (this.tv.iQDmaSet == null && other.tv.iQDmaSet != null))
                {
                    return false;
                }

                if (this.tv.iQClassSet != null && other.tv.iQClassSet != null)
                {
                    if (this.tv.iQClassSet.listofIQCLass != null && other.tv.iQClassSet.listofIQCLass != null)
                    {
                        if (this.tv.iQClassSet.listofIQCLass.Count() != other.tv.iQClassSet.listofIQCLass.Count())
                            return false;

                        IEnumerable<string> obj1 = this.tv.iQClassSet.listofIQCLass.Select(a => a.name).Except(other.tv.iQClassSet.listofIQCLass.Select(a => a.name));
                        IEnumerable<string> obj2 = other.tv.iQClassSet.listofIQCLass.Select(a => a.name).Except(this.tv.iQClassSet.listofIQCLass.Select(a => a.name));
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }
                    }
                    else if ((this.tv.iQClassSet.listofIQCLass != null && other.tv.iQClassSet.listofIQCLass == null)
                        || (this.tv.iQClassSet.listofIQCLass == null && other.tv.iQClassSet.listofIQCLass != null))
                    {
                        return false;
                    }
                }
                else if ((this.tv.iQClassSet != null && other.tv.iQClassSet == null) || (this.tv.iQClassSet == null && other.tv.iQClassSet != null))
                {
                    return false;
                }

                if (this.tv.stationAffiliateSet != null && other.tv.stationAffiliateSet != null)
                {
                    if (this.tv.stationAffiliateSet.listOfIQStationID != null && other.tv.stationAffiliateSet.listOfIQStationID != null)
                    {
                        if (this.tv.stationAffiliateSet.listOfIQStationID.Count() != other.tv.stationAffiliateSet.listOfIQStationID.Count())
                            return false;

                        IEnumerable<string> obj1 = this.tv.stationAffiliateSet.listOfIQStationID.Except(other.tv.stationAffiliateSet.listOfIQStationID);
                        IEnumerable<string> obj2 = other.tv.stationAffiliateSet.listOfIQStationID.Except(this.tv.stationAffiliateSet.listOfIQStationID);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }
                    }
                    else if ((this.tv.stationAffiliateSet.listOfIQStationID != null && other.tv.stationAffiliateSet.listOfIQStationID == null)
                        || (this.tv.stationAffiliateSet.listOfIQStationID == null && other.tv.stationAffiliateSet.listOfIQStationID != null))
                    {
                        return false;
                    }
                }
                else if ((this.tv.stationAffiliateSet != null && other.tv.stationAffiliateSet == null)
                    || (this.tv.stationAffiliateSet == null && other.tv.stationAffiliateSet != null))
                {
                    return false;
                }
            }
            else if ((this.tv != null && other.tv == null)
                    || (this.tv == null && other.tv != null))
            {
                return false;
            }


            #endregion

            #region Radio Equality

            if (this.radio != null && other.radio != null)
            {
                if (this.radio.radioDuration != null && other.radio.radioDuration != null)
                {
                    if (this.radio.radioDuration.DayDuration != other.radio.radioDuration.DayDuration)
                        return false;

                    if (this.radio.radioDuration.FromDate != other.radio.radioDuration.FromDate)
                        return false;

                    if (this.radio.radioDuration.ToDate != other.radio.radioDuration.ToDate)
                        return false;
                }
                else if ((this.radio.radioDuration != null && other.radio.radioDuration == null)
                    || (this.radio.radioDuration == null && other.radio.radioDuration != null))
                {
                    return false;
                }

                if (this.radio.radioIQDmaSet != null && other.radio.radioIQDmaSet != null)
                {
                    if (this.radio.radioIQDmaSet.listofIQDma != null && other.radio.radioIQDmaSet.listofIQDma != null)
                    {
                        if (this.radio.radioIQDmaSet.listofIQDma.Count() != other.radio.radioIQDmaSet.listofIQDma.Count())
                            return false;

                        IEnumerable<string> obj1 = this.radio.radioIQDmaSet.listofIQDma.Select(a => a.name).Except(other.radio.radioIQDmaSet.listofIQDma.Select(a => a.name));
                        IEnumerable<string> obj2 = other.radio.radioIQDmaSet.listofIQDma.Select(a => a.name).Except(this.radio.radioIQDmaSet.listofIQDma.Select(a => a.name));
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }

                    }
                    else if ((this.radio.radioIQDmaSet.listofIQDma != null && other.radio.radioIQDmaSet.listofIQDma == null)
                   || (this.radio.radioIQDmaSet.listofIQDma == null && other.radio.radioIQDmaSet.listofIQDma != null))
                    {
                        return false;
                    }


                }
                else if ((this.radio.radioDuration != null && other.radio.radioDuration == null)
                   || (this.radio.radioDuration == null && other.radio.radioDuration != null))
                {
                    return false;
                }
            }
            else if ((this.radio != null && other.radio == null)
                   || (this.radio == null && other.radio != null))
            {
                return false;
            }

            #endregion

            #region News Filter

            if (this.news != null && other.news != null)
            {
                if (this.news.NewsDuration != null && other.news.NewsDuration != null)
                {
                    if (this.news.NewsDuration.DayDuration != other.news.NewsDuration.DayDuration)
                        return false;

                    if (this.news.NewsDuration.FromDate != other.news.NewsDuration.FromDate)
                        return false;

                    if (this.news.NewsDuration.ToDate != other.news.NewsDuration.ToDate)
                        return false;
                }
                else if ((this.news.NewsDuration != null && other.news.NewsDuration == null)
                    || (this.news.NewsDuration == null && other.news.NewsDuration != null))
                {
                    return false;
                }

                if (this.news.Publication != other.news.Publication)
                    return false;

                if (this.news.newsCategory_Set != null && other.news.newsCategory_Set != null)
                {

                    if (this.news.newsCategory_Set.listOfNewsCategory != null && other.news.newsCategory_Set.listOfNewsCategory != null)
                    {
                        if (this.news.newsCategory_Set.listOfNewsCategory.Count() != other.news.newsCategory_Set.listOfNewsCategory.Count())
                            return false;

                        IEnumerable<string> obj1 = this.news.newsCategory_Set.listOfNewsCategory.Except(other.news.newsCategory_Set.listOfNewsCategory);
                        IEnumerable<string> obj2 = other.news.newsCategory_Set.listOfNewsCategory.Except(this.news.newsCategory_Set.listOfNewsCategory);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }

                    }
                    else if ((this.news.newsCategory_Set.listOfNewsCategory != null && other.news.newsCategory_Set.listOfNewsCategory == null)
                   || (this.news.newsCategory_Set.listOfNewsCategory == null && other.news.newsCategory_Set.listOfNewsCategory != null))
                    {
                        return false;
                    }
                }
                else if ((this.news.newsCategory_Set != null && other.news.newsCategory_Set == null)
                   || (this.news.newsCategory_Set == null && other.news.newsCategory_Set != null))
                {
                    return false;
                }

                if (this.news.publicationCategory_Set != null && other.news.publicationCategory_Set != null)
                {

                    if (this.news.publicationCategory_Set.listofPublicationCategory != null && other.news.publicationCategory_Set.listofPublicationCategory != null)
                    {
                        if (this.news.publicationCategory_Set.listofPublicationCategory.Count() != other.news.publicationCategory_Set.listofPublicationCategory.Count())
                            return false;

                        IEnumerable<string> obj1 = this.news.publicationCategory_Set.listofPublicationCategory.Except(other.news.publicationCategory_Set.listofPublicationCategory);
                        IEnumerable<string> obj2 = other.news.publicationCategory_Set.listofPublicationCategory.Except(this.news.publicationCategory_Set.listofPublicationCategory);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }

                    }
                    else if ((this.news.publicationCategory_Set.listofPublicationCategory != null && other.news.publicationCategory_Set.listofPublicationCategory == null)
                   || (this.news.publicationCategory_Set.listofPublicationCategory == null && other.news.publicationCategory_Set.listofPublicationCategory != null))
                    {
                        return false;
                    }
                }
                else if ((this.news.publicationCategory_Set != null && other.news.publicationCategory_Set == null)
                   || (this.news.publicationCategory_Set == null && other.news.publicationCategory_Set != null))
                {
                    return false;
                }

                if (this.news.genre_Set != null && other.news.genre_Set != null)
                {

                    if (this.news.genre_Set.listOfGenre != null && other.news.genre_Set.listOfGenre != null)
                    {
                        if (this.news.genre_Set.listOfGenre.Count() != other.news.genre_Set.listOfGenre.Count())
                            return false;

                        IEnumerable<object> obj1 = this.news.genre_Set.listOfGenre.Except(other.news.genre_Set.listOfGenre);
                        IEnumerable<object> obj2 = other.news.genre_Set.listOfGenre.Except(this.news.genre_Set.listOfGenre);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }

                    }
                    else if ((this.news.genre_Set.listOfGenre != null && other.news.genre_Set.listOfGenre == null)
                   || (this.news.genre_Set.listOfGenre == null && other.news.genre_Set.listOfGenre != null))
                    {
                        return false;
                    }
                }
                else if ((this.news.genre_Set != null && other.news.genre_Set == null)
                   || (this.news.genre_Set == null && other.news.genre_Set != null))
                {
                    return false;
                }

                if (this.news.region_Set != null && other.news.region_Set != null)
                {

                    if (this.news.region_Set.listOfRegion != null && other.news.region_Set.listOfRegion != null)
                    {
                        if (this.news.region_Set.listOfRegion.Count() != other.news.region_Set.listOfRegion.Count())
                            return false;

                        IEnumerable<object> obj1 = this.news.region_Set.listOfRegion.Except(other.news.region_Set.listOfRegion);
                        IEnumerable<object> obj2 = other.news.region_Set.listOfRegion.Except(this.news.region_Set.listOfRegion);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }

                    }
                    else if ((this.news.region_Set.listOfRegion != null && other.news.region_Set.listOfRegion == null)
                   || (this.news.region_Set.listOfRegion == null && other.news.region_Set.listOfRegion != null))
                    {
                        return false;
                    }
                }
                else if ((this.news.region_Set != null && other.news.region_Set == null)
                   || (this.news.region_Set == null && other.news.region_Set != null))
                {
                    return false;
                }




            }
            else if ((this.news != null && other.news == null)
                    || (this.news == null && other.news != null))
            {
                return false;
            }




            #endregion

            #region Social Media

            if (this.socialMedia != null && other.socialMedia != null)
            {
                if (this.socialMedia.smDuration != null && other.socialMedia.smDuration != null)
                {
                    if (this.socialMedia.smDuration.DayDuration != other.socialMedia.smDuration.DayDuration)
                        return false;

                    if (this.socialMedia.smDuration.FromDate != other.socialMedia.smDuration.FromDate)
                        return false;

                    if (this.socialMedia.smDuration.ToDate != other.socialMedia.smDuration.ToDate)
                        return false;
                }
                else if ((this.socialMedia.smDuration != null && other.socialMedia.smDuration == null)
                    || (this.socialMedia.smDuration == null && other.socialMedia.smDuration != null))
                {
                    return false;
                }

                if (this.socialMedia.Source != other.socialMedia.Source)
                    return false;

                if (this.socialMedia.Author != other.socialMedia.Author)
                    return false;

                if (this.socialMedia.Title != other.socialMedia.Title)
                    return false;

                if (this.socialMedia.sourceCategory_Set != null && other.socialMedia.sourceCategory_Set != null)
                {
                    if (this.socialMedia.sourceCategory_Set.listOfSourceCategory != null && other.socialMedia.sourceCategory_Set.listOfSourceCategory != null)
                    {
                        if (this.socialMedia.sourceCategory_Set.listOfSourceCategory.Count() != other.socialMedia.sourceCategory_Set.listOfSourceCategory.Count())
                            return false;

                        IEnumerable<object> obj1 = this.socialMedia.sourceCategory_Set.listOfSourceCategory.Except(other.socialMedia.sourceCategory_Set.listOfSourceCategory);
                        IEnumerable<object> obj2 = other.socialMedia.sourceCategory_Set.listOfSourceCategory.Except(this.socialMedia.sourceCategory_Set.listOfSourceCategory);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }
                    }
                    else if ((this.socialMedia.sourceCategory_Set.listOfSourceCategory != null && other.socialMedia.sourceCategory_Set.listOfSourceCategory == null)
                   || (this.socialMedia.sourceCategory_Set.listOfSourceCategory == null && other.socialMedia.sourceCategory_Set.listOfSourceCategory != null))
                    {
                        return false;
                    }
                }
                else if ((this.socialMedia.sourceCategory_Set != null && other.socialMedia.sourceCategory_Set == null)
                   || (this.socialMedia.sourceCategory_Set == null && other.socialMedia.sourceCategory_Set != null))
                {
                    return false;
                }

                if (this.socialMedia.sourceType_Set != null && other.socialMedia.sourceType_Set != null)
                {
                    if (this.socialMedia.sourceType_Set.listOfSourceType != null && other.socialMedia.sourceType_Set.listOfSourceType != null)
                    {
                        if (this.socialMedia.sourceType_Set.listOfSourceType.Count() != other.socialMedia.sourceType_Set.listOfSourceType.Count())
                            return false;

                        IEnumerable<object> obj1 = this.socialMedia.sourceType_Set.listOfSourceType.Except(other.socialMedia.sourceType_Set.listOfSourceType);
                        IEnumerable<object> obj2 = other.socialMedia.sourceType_Set.listOfSourceType.Except(this.socialMedia.sourceType_Set.listOfSourceType);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }
                    }
                    else if ((this.socialMedia.sourceType_Set.listOfSourceType != null && other.socialMedia.sourceType_Set.listOfSourceType == null)
                   || (this.socialMedia.sourceType_Set.listOfSourceType == null && other.socialMedia.sourceType_Set.listOfSourceType != null))
                    {
                        return false;
                    }
                }
                else if ((this.socialMedia.sourceType_Set != null && other.socialMedia.sourceType_Set == null)
                   || (this.socialMedia.sourceType_Set == null && other.socialMedia.sourceType_Set != null))
                {
                    return false;
                }

                if (this.socialMedia.sourceRank_Set != null && other.socialMedia.sourceRank_Set != null)
                {
                    if (this.socialMedia.sourceRank_Set.listofsourceRank != null && other.socialMedia.sourceRank_Set.listofsourceRank != null)
                    {
                        if (this.socialMedia.sourceRank_Set.listofsourceRank.Count() != other.socialMedia.sourceRank_Set.listofsourceRank.Count())
                            return false;

                        IEnumerable<object> obj1 = this.socialMedia.sourceRank_Set.listofsourceRank.Except(other.socialMedia.sourceRank_Set.listofsourceRank);
                        IEnumerable<object> obj2 = other.socialMedia.sourceRank_Set.listofsourceRank.Except(this.socialMedia.sourceRank_Set.listofsourceRank);
                        if (obj1.Count() > 0 || obj2.Count() > 0)
                        {
                            return false;
                        }
                    }
                    else if ((this.socialMedia.sourceRank_Set.listofsourceRank != null && other.socialMedia.sourceRank_Set.listofsourceRank == null)
                   || (this.socialMedia.sourceRank_Set.listofsourceRank == null && other.socialMedia.sourceRank_Set.listofsourceRank != null))
                    {
                        return false;
                    }
                }
                else if ((this.socialMedia.sourceRank_Set != null && other.socialMedia.sourceRank_Set == null)
                   || (this.socialMedia.sourceRank_Set == null && other.socialMedia.sourceRank_Set != null))
                {
                    return false;
                }


            }
            else if ((this.socialMedia != null && other.socialMedia == null)
                    || (this.socialMedia == null && other.socialMedia != null))
            {
                return false;
            }


            #endregion

            #region Twitter

            if (this.twitter != null && other.twitter != null)
            {
                if (this.twitter.twitterDuration != null && other.twitter.twitterDuration != null)
                {
                    if (this.twitter.twitterDuration.DayDuration != other.twitter.twitterDuration.DayDuration)
                        return false;

                    if (this.twitter.twitterDuration.FromDate != other.twitter.twitterDuration.FromDate)
                        return false;

                    if (this.twitter.twitterDuration.ToDate != other.twitter.twitterDuration.ToDate)
                        return false;
                }
                else if ((this.twitter.twitterDuration != null && other.twitter.twitterDuration == null)
                    || (this.twitter.twitterDuration == null && other.twitter.twitterDuration != null))
                {
                    return false;
                }

                if (this.twitter.Actor != other.twitter.Actor)
                    return false;

                if (this.twitter.actorFollowersRange != null && other.twitter.actorFollowersRange != null)
                {
                    if (this.twitter.actorFollowersRange.From != other.twitter.actorFollowersRange.From)
                        return false;

                    if (this.twitter.actorFollowersRange.To != other.twitter.actorFollowersRange.To)
                        return false;
                }
                else if ((this.twitter.actorFollowersRange != null && other.twitter.actorFollowersRange == null)
                   || (this.twitter.actorFollowersRange == null && other.twitter.actorFollowersRange != null))
                {
                    return false;
                }

                if (this.twitter.actorFriendsRange != null && other.twitter.actorFriendsRange != null)
                {
                    if (this.twitter.actorFriendsRange.From != other.twitter.actorFriendsRange.From)
                        return false;

                    if (this.twitter.actorFriendsRange.To != other.twitter.actorFriendsRange.To)
                        return false;
                }
                else if ((this.twitter.actorFriendsRange != null && other.twitter.actorFriendsRange == null)
                   || (this.twitter.actorFriendsRange == null && other.twitter.actorFriendsRange != null))
                {
                    return false;
                }

                if (this.twitter.kloutScoreRange != null && other.twitter.kloutScoreRange != null)
                {
                    if (this.twitter.kloutScoreRange.From != other.twitter.kloutScoreRange.From)
                        return false;

                    if (this.twitter.kloutScoreRange.To != other.twitter.kloutScoreRange.To)
                        return false;
                }
                else if ((this.twitter.kloutScoreRange != null && other.twitter.kloutScoreRange == null)
                   || (this.twitter.kloutScoreRange == null && other.twitter.kloutScoreRange != null))
                {
                    return false;
                }

            }
            else if ((this.twitter != null && other.twitter == null)
                    || (this.twitter == null && other.twitter != null))
            {
                return false;
            }

            #endregion

            return true;
        }
    }

    #region TV  Prop

    public class TV
    {

        public string ProgramTitle { get; set; }
        public string Appearing { get; set; }

        [XmlElement("Duration")]
        public Duration tvDuration { get; set; }

        [XmlElement(ElementName = "IQ_Dma_Set")]
        public IQ_Dma_Set iQDmaSet { get; set; }

        [XmlElement(ElementName = "Station_Affiliate_Set")]
        public Station_Affiliate_Set stationAffiliateSet { get; set; }

        [XmlElement(ElementName = "IQ_Class_Set")]
        public IQ_Class_Set iQClassSet { get; set; }

    }


    public class Duration
    {
        public string DayDuration { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
    }


    public class IQ_Dma_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }

        [XmlAttribute("SelectionMethod")]
        public string SelectionMethod { get; set; }

        [XmlElement("IQ_Dma")]
        public List<IQ_Dma> listofIQDma { get; set; }



    }


    public class IQ_Dma
    {
        public string num { get; set; }
        public string name { get; set; }


    }


    public class Station_Affiliate_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }

        [XmlElement("IQ_Station_ID")]
        public List<string> listOfIQStationID { get; set; }
    }


    public class IQ_Station_ID
    {
        public string IQ_Station_IDElement { get; set; }
    }

    public class IQ_Class_Set
    {
        public bool IsAllowAll { get; set; }
        [XmlElement("IQ_Class")]
        public List<IQ_Class> listofIQCLass { get; set; }
    }

    public class IQ_Class
    {
        public string num { get; set; }
        public string name { get; set; }
    }

    #endregion

    #region News  Prop


    public class News
    {
        public string Publication { get; set; }
        [XmlElement("Duration")]
        public Duration NewsDuration { get; set; }

        [XmlElement("NewsCategory_Set")]
        public NewsCategory_Set newsCategory_Set { get; set; }

        [XmlElement("PublicationCategory_Set")]
        public PublicationCategory_Set publicationCategory_Set { get; set; }

        [XmlElement("Genre_Set")]
        public Genre_Set genre_Set { get; set; }

        [XmlElement("Region_Set")]
        public Region_Set region_Set { get; set; }

    }

    public class NewsCategory_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }
        public List<string> listOfNewsCategory { get; set; }
    }

    /*public class NewsCategory
    {
        [XmlElement("NewsCategory")]
        public string NewsCategoryElement { get; set; }
    }*/

    public class PublicationCategory_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }

        [XmlElement("PublicationCategory")]
        public List<string> listofPublicationCategory { get; set; }
    }

    /*public class PublicationCategory
    {
        [XmlElement("PublicationCategory")]
        public string PublicationCategoryElement { get; set; }
    }*/

    public class Genre_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }
        public List<String> listOfGenre { get; set; }
    }

    /*public class Genre
    {
        [XmlElement("Genre")]
        public string GenreElement { get; set; }
    }*/

    public class Region_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }
        public List<String> listOfRegion { get; set; }
    }

    /*public class Region
    {
        [XmlElement("Region")]
        public string RegionElement { get; set; }
    }*/

    #endregion

    #region Social Media Prop

    public class SocialMediaElement
    {
        [XmlElement("Duration")]
        public Duration smDuration { get; set; }

        public string Source { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }

        [XmlElement("SourceCategory_Set")]
        public SourceCategory_Set sourceCategory_Set { get; set; }

        [XmlElement("SourceType_Set")]
        public SourceType_Set sourceType_Set { get; set; }

        [XmlElement("SourceRank_Set")]
        public SourceRank_Set sourceRank_Set { get; set; }


    }

    public class SourceRank_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }
        public List<string> listofsourceRank { get; set; }

    }

    /*public class SourceRank
    {
        [XmlElement("SourceRank")]
        public string SourceRankEle { get; set; }
    }*/

    public class SourceCategory_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }
        public List<String> listOfSourceCategory { get; set; }

    }

    /*public class SourceCategory
    {
        [XmlElement("SourceCategory")]
        public string SourceCategoryElement { get; set; }
    }*/

    public class SourceType_Set
    {
        [XmlAttribute("IsAllowAll")]
        public bool IsAllowAll { get; set; }
        public List<string> listOfSourceType { get; set; }

    }

    /*public class SourceType
    {
        [XmlElement("SourceType")]
        public string SourceTypeElement { get; set; }
    }*/


    #endregion

    #region Twitter Prop
    public class Twitter
    {
        public string Actor { get; set; }

        [XmlElement("Duration")]
        public Duration twitterDuration { get; set; }
        [XmlElement("ActorFollowersRange")]
        public ActorFollowersRange actorFollowersRange { get; set; }

        [XmlElement("ActorFriendsRange")]
        public ActorFriendsRange actorFriendsRange { get; set; }

        [XmlElement("KloutScoreRange")]
        public KloutScoreRange kloutScoreRange { get; set; }

    }

    public class ActorFollowersRange
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    public class ActorFriendsRange
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    public class KloutScoreRange
    {
        public int From { get; set; }
        public int To { get; set; }
    }

    #endregion

    #region Radio Prop

    public class Radio
    {
        [XmlElement("Duration")]
        public Duration radioDuration { get; set; }

        [XmlElement("IQ_Dma_Set")]
        public IQ_Dma_Set radioIQDmaSet { get; set; }
    }





    #endregion

    /* public class SearchRequest
     {
         public string SearchTerm { get; set; }
         public TV tv { get; set; }
         public Radio radio { get; set; }
         public News news { get; set; }
         public SocialMedia socialMedia { get; set; }
         public Twitter twitter { get; set; }


     }*/


}
