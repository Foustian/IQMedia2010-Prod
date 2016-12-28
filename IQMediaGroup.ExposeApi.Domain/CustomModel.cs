using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using Microsoft.Data.Extensions;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Configuration;
using System.Collections;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Common.Config;
using System.Web;

namespace IQMediaGroup.ExposeApi.Domain
{
    public partial class IQMediaGroupExposeEntities : ObjectContext
    {
        #region Materializer<T>
        // Note use of static instances. Materializer<T> is thread-safe to prevent
        // multiple compilations of optimized delegates.

        private static readonly Materializer<Dma> s_IQ_DMAMaterializer = new Materializer<Dma>(r =>
            new Dma
            {
                Name = r.Field<string>("Dma_Name"),
                Num = r.Field<string>("Dma_Num")
            });

        private static readonly Materializer<Affiliate> s_Station_AffilMaterializer = new Materializer<Affiliate>(r =>
            new Affiliate
            {
                Name = r.Field<string>("Station_Affil")
            });

        private static readonly Materializer<Class> s_IQ_Class = new Materializer<Class>(r =>
            new Class
            {
                Name = r.Field<string>("IQ_Class"),
                Num = r.Field<string>("IQ_Class_Num")
            });

        private static readonly Materializer<Region> s_IQ_RegionMaterializer = new Materializer<Region>(r =>
            new Region
            {
                name = r.Field<string>("Region"),
                num = r.Field<string>("Region_Num")
            });

        private static readonly Materializer<Country> s_IQ_CountryMaterializer = new Materializer<Country>(r =>
            new Country
            {
                name = r.Field<string>("Country"),
                num = r.Field<string>("Country_Num")
            });

        private static readonly Materializer<Station> s_Station_Materializer = new Materializer<Station>(r =>
            new Station
            {
                name = r.Field<string>("IQ_Station_ID"),
                StationCallSign = r.Field<string>("Station_Call_Sign"),
                DmaName = r.Field<string>("Dma_Name"),
                Affiliate = r.Field<string>("Station_Affil"),
                Country = r.Field<string>("Country"),
                Region = r.Field<string>("Region")
            });

        private IEnumerable<ArchiveMedia> CustMaterialize<T>(DataTable p_DT, Int64 p_ClientID = 0, Int64 p_CustomerID = 0)
        {
            if (typeof(T) == typeof(ArchiveClip))
            {
                List<ArchiveClip> clipList = new List<ArchiveClip>();

                foreach (DataRow dataRow in p_DT.Rows)
                {
                    ArchiveClip clip = new ArchiveClip();

                    var station = (from ele in (XDocument.Parse(Convert.ToString(dataRow["StationDetail"])).Descendants("StationDetail"))
                                   select new
                                   {
                                       market = ele.Element("Dma_Name") != null ? ele.Element("Dma_Name").Value : "",
                                       stationAffil = ele.Element("Station_Affil") != null ? ele.Element("Station_Affil").Value : "",
                                       StationID = ele.Element("IQ_Station_ID") != null ? ele.Element("IQ_Station_ID").Value : "",
                                       StationCallSign = ele.Element("Station_Call_Sign") != null ? ele.Element("Station_Call_Sign").Value : "",
                                       TimeZone = ele.Element("TimeZone") != null ? ele.Element("TimeZone").Value : ""
                                   }).FirstOrDefault();

                    clip.Audience = dataRow["Audience"] != null && dataRow["Audience"] != DBNull.Value ? Convert.ToInt32(dataRow["Audience"]) : (int?)null;
                    clip.AudienceResult = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["AudienceResult"]);
                    clip.CategoryGuid = dataRow["CategoryGUID"] != null && dataRow["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(dataRow["CategoryGUID"])) : Guid.Empty;
                    clip.CategoryName = dataRow["CategoryName"] != null && dataRow["CategoryName"] != DBNull.Value ? Convert.ToString(dataRow["CategoryName"]) : null;
                    clip.ClipID = new Guid(Convert.ToString(dataRow["ClipID"]));
                    clip.ClipMeta = dataRow["ClipMeta"] != null && dataRow["ClipMeta"] != DBNull.Value ? Convert.ToString(GetClipMeta(Convert.ToString(dataRow["ClipMeta"]))) : null;
                    clip.ClipURL = ConfigurationManager.AppSettings["ClipPlayerURL"] + dataRow["ClipID"];
                    clip.Content = !string.IsNullOrEmpty(Convert.ToString(dataRow["Content"])) ? XDocument.Parse(Convert.ToString(dataRow["Content"])).ToString() : string.Empty;
                    clip.CreatedDate = Convert.ToDateTime(dataRow["CreatedDate"]);
                    clip.Description = dataRow["Description"] != null && dataRow["Description"] != DBNull.Value ? Convert.ToString(dataRow["Description"]) : null;
                    clip.ID = Convert.ToInt64(dataRow["ID"]);
                    clip.IQAdShareValue = dataRow["IQAdShareValue"] != null && dataRow["IQAdShareValue"] != DBNull.Value ? Convert.ToDecimal(dataRow["IQAdShareValue"]) : (decimal?)null;
                    clip.Keywords = dataRow["Keywords"] != null && dataRow["Keywords"] != DBNull.Value ? Convert.ToString(dataRow["Keywords"]) : null;
                    clip.Market = station.market;
                    clip.MediaDate = Convert.ToDateTime(dataRow["MediaDate"]);
                    clip.MediaType = Convert.ToString(dataRow["MediaType"]);
                    clip.NegativeSentiment = dataRow["NegativeSentiment"] != null && dataRow["NegativeSentiment"] != DBNull.Value ? Convert.ToInt16(dataRow["NegativeSentiment"]) : (short?)null;
                    clip.ParentID = dataRow["ParentID"] != null && dataRow["ParentID"] != DBNull.Value ? (Int64?)dataRow["ParentID"] : null;
                    clip.PositiveSentiment = dataRow["PositiveSentiment"] != null && dataRow["PositiveSentiment"] != DBNull.Value ? Convert.ToInt16(dataRow["PositiveSentiment"]) : (short?)null;
                    clip.Station = station.StationCallSign;
                    clip.Station_Affil = station.stationAffil;
                    clip.StationID = station.StationID;
                    clip.SubMediaType = Convert.ToString(dataRow["SubMediaType"]);
                    clip.TimeZone = station.TimeZone;
                    clip.Title = Convert.ToString(dataRow["Title"]);
                    clip.EmbedLink = string.Format(ConfigurationManager.AppSettings["ClipEmbedLink"], ConfigurationManager.AppSettings["ClipPlayerURL"] + dataRow["ClipID"]);
                    clip.ThumbnailURL = ConfigurationManager.AppSettings["ClipThumbnailURL"] + dataRow["ClipID"];

                    clipList.Add(clip);
                }

                return (IEnumerable<ArchiveMedia>)clipList.AsEnumerable();
            }
            else if (typeof(T) == typeof(ArchiveNM))
            {
                List<ArchiveNM> nmList = new List<ArchiveNM>();
                Int16 iqLicense = 0;

                foreach (DataRow dataRow in p_DT.Rows)
                {
                    Int16.TryParse(CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["IQLicense"]), out iqLicense);

                    ArchiveNM nm = new ArchiveNM
                    {
                        Audience = CommonFunctions.CheckDBNullNReturnValue<Int32>(dataRow["Audience"]),
                        AudienceResult = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["AudienceResult"]),
                        CategoryGuid = dataRow["CategoryGUID"] != null && dataRow["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(dataRow["CategoryGUID"])) : Guid.Empty,
                        CategoryName = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["CategoryName"]),
                        Content = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["Content"]),
                        CreatedDate = Convert.ToDateTime(dataRow["CreatedDate"]),
                        Description = dataRow["Description"] != null && dataRow["Description"] != DBNull.Value ? Convert.ToString(dataRow["Description"]) : null,
                        ID = Convert.ToInt64(dataRow["ID"]),
                        IQAdShareValue = CommonFunctions.CheckDBNullNReturnValue<Decimal>(dataRow["IQAdShareValue"]),
                        Keywords = dataRow["Keywords"] != null && dataRow["Keywords"] != DBNull.Value ? Convert.ToString(dataRow["Keywords"]) : null,
                        MediaDate = Convert.ToDateTime(dataRow["MediaDate"]),
                        MediaType = Convert.ToString(dataRow["MediaType"]),
                        NegativeSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["NegativeSentiment"]),
                        ParentID = dataRow["ParentID"] != null && dataRow["ParentID"] != DBNull.Value ? (Int64?)dataRow["ParentID"] : null,
                        PositiveSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["PositiveSentiment"]),
                        Publication = Convert.ToString(dataRow["Publication"]),
                        SubMediaType = Convert.ToString(dataRow["SubMediaType"]),
                        Title = Convert.ToString(dataRow["Title"]),
                        Url = !(iqLicense > 0) ? Convert.ToString(dataRow["Url"]) : ConfigurationManager.AppSettings["LicenseArticleUrl"] + HttpUtility.UrlEncode(CommonFunctions.EncryptLicenseStringAES(p_CustomerID + "¶API¶" + Convert.ToString(dataRow["Url"]) + "&u1=cliq40&u2=" + p_ClientID + "¶" + iqLicense))
                    };

                    nmList.Add(nm);
                }

                return (IEnumerable<ArchiveMedia>)nmList.AsEnumerable();
            }
            else if (typeof(T) == typeof(ArchiveSM))
            {
                List<ArchiveSM> smList = new List<ArchiveSM>();

                foreach (DataRow dataRow in p_DT.Rows)
                {
                    var articleStatus = new ArticleStatus();
                    var mt = Convert.ToString(dataRow["SubMediaType"]);

                    if (!string.IsNullOrEmpty(CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["ArticleStats"])))
                    {
                        articleStatus = (ArticleStatus)CommonFunctions.DeserialiazeXml(Convert.ToString(dataRow["ArticleStats"]), articleStatus); 
                    }

                    ArchiveSM sm = new ArchiveSM
                    {
                        Audience = (mt.ToUpper() == "BLOG" ? CommonFunctions.CheckDBNullNReturnValue<Int32?>(dataRow["Audience"]) : null),
                        AudienceResult = (mt.ToUpper() == "BLOG" ? CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["AudienceResult"]) : null),
                        CategoryGuid = dataRow["CategoryGUID"] != null && dataRow["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(dataRow["CategoryGUID"])) : Guid.Empty,
                        CategoryName = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["CategoryName"]),
                        Comments = articleStatus.Comments,
                        Content = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["Content"]),
                        CreatedDate = Convert.ToDateTime(dataRow["CreatedDate"]),
                        Description = dataRow["Description"] != null && dataRow["Description"] != DBNull.Value ? Convert.ToString(dataRow["Description"]) : null,
                        Homelink = Convert.ToString(dataRow["homelink"]),
                        ID = Convert.ToInt64(dataRow["ID"]),
                        IQAdShareValue = (mt.ToUpper() == "BLOG" ? CommonFunctions.CheckDBNullNReturnValue<Decimal?>(dataRow["IQAdShareValue"]) : null),
                        Keywords = dataRow["Keywords"] != null && dataRow["Keywords"] != DBNull.Value ? Convert.ToString(dataRow["Keywords"]) : null,
                        Likes = articleStatus.Likes,
                        MediaDate = Convert.ToDateTime(dataRow["MediaDate"]),
                        MediaType = Convert.ToString(dataRow["MediaType"]),
                        NegativeSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["NegativeSentiment"]),
                        ParentID = dataRow["ParentID"] != null && dataRow["ParentID"] != DBNull.Value ? (Int64?)dataRow["ParentID"] : null,
                        PositiveSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["PositiveSentiment"]),
                        Shares = articleStatus.Shares,
                        SubMediaType = Convert.ToString(dataRow["SubMediaType"]),
                        ThumbUrl = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["ThumbUrl"]),
                        Title = Convert.ToString(dataRow["Title"]),
                        Url = Convert.ToString(dataRow["Url"])
                    };

                    smList.Add(sm);
                }

                return (IEnumerable<ArchiveMedia>)smList.AsEnumerable();
            }
            else if (typeof(T) == typeof(ArchiveTweets))
            {
                List<ArchiveTweets> twList = new List<ArchiveTweets>();

                foreach (DataRow dataRow in p_DT.Rows)
                {
                    ArchiveTweets tw = new ArchiveTweets
                    {
                        Actor_DisplayName = Convert.ToString(dataRow["Actor_DisplayName"]),
                        Actor_Link = Convert.ToString(dataRow["Actor_link"]),
                        Actor_PreferredUserName = Convert.ToString(dataRow["Actor_PreferredUserName"]),
                        CategoryGuid = dataRow["CategoryGUID"] != null && dataRow["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(dataRow["CategoryGUID"])) : Guid.Empty,
                        CategoryName = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["CategoryName"]),
                        Content = Convert.ToString(dataRow["Content"]),
                        CreatedDate = Convert.ToDateTime(dataRow["CreatedDate"]),
                        Description = dataRow["Description"] != null && dataRow["Description"] != DBNull.Value ? Convert.ToString(dataRow["Description"]) : null,
                        ID = Convert.ToInt64(dataRow["ID"]),
                        Keywords = dataRow["Keywords"] != null && dataRow["Keywords"] != DBNull.Value ? Convert.ToString(dataRow["Keywords"]) : null,
                        MediaDate = Convert.ToDateTime(dataRow["MediaDate"]),
                        MediaType = Convert.ToString(dataRow["MediaType"]),
                        NegativeSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["NegativeSentiment"]),
                        ParentID = dataRow["ParentID"] != null && dataRow["ParentID"] != DBNull.Value ? (Int64?)dataRow["ParentID"] : null,
                        PositiveSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["PositiveSentiment"]),
                        SubMediaType = Convert.ToString(dataRow["SubMediaType"]),
                        Title = Convert.ToString(dataRow["Title"])
                    };

                    twList.Add(tw);
                }

                return (IEnumerable<ArchiveMedia>)twList.AsEnumerable();
            }
            else if (typeof(T) == typeof(ArchiveTVEyes))
            {
                List<ArchiveTVEyes> tmList = new List<ArchiveTVEyes>();

                foreach (DataRow dataRow in p_DT.Rows)
                {
                    ArchiveTVEyes tm = new ArchiveTVEyes
                    {
                        CategoryGuid = dataRow["CategoryGUID"] != null && dataRow["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(dataRow["CategoryGUID"])) : Guid.Empty,
                        CategoryName = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["CategoryName"]),
                        Content = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["Content"]),
                        CreatedDate = Convert.ToDateTime(dataRow["CreatedDate"]),
                        Description = dataRow["Description"] != null && dataRow["Description"] != DBNull.Value ? Convert.ToString(dataRow["Description"]) : null,
                        ID = Convert.ToInt64(dataRow["ID"]),
                        Keywords = dataRow["Keywords"] != null && dataRow["Keywords"] != DBNull.Value ? Convert.ToString(dataRow["Keywords"]) : null,
                        Market = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["Market"]),
                        MediaDate = Convert.ToDateTime(dataRow["MediaDate"]),
                        MediaType = Convert.ToString(dataRow["MediaType"]),
                        NegativeSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["NegativeSentiment"]),
                        ParentID = dataRow["ParentID"] != null && dataRow["ParentID"] != DBNull.Value ? (Int64?)dataRow["ParentID"] : null,
                        PositiveSentiment = CommonFunctions.CheckDBNullNReturnValue<Int16>(dataRow["PositiveSentiment"]),
                        SubMediaType = Convert.ToString(dataRow["SubMediaType"]),
                        Title = Convert.ToString(dataRow["Title"])
                    };

                    tmList.Add(tm);
                }

                return (IEnumerable<ArchiveMedia>)tmList.AsEnumerable();
            }
            else if (typeof(T) == typeof(ArchiveBLPM))
            {
                List<ArchiveBLPM> pmList = new List<ArchiveBLPM>();

                foreach (DataRow dataRow in p_DT.Rows)
                {
                    ArchiveBLPM pm = new ArchiveBLPM
                    {
                        CategoryGuid = dataRow["CategoryGUID"] != null && dataRow["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(dataRow["CategoryGUID"])) : Guid.Empty,
                        CategoryName = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["CategoryName"]),
                        Circulation = CommonFunctions.CheckDBNullNReturnValue<Int32>(dataRow["Circulation"]),
                        Content = CommonFunctions.CheckDBNullNReturnValue<String>(dataRow["Content"]),
                        CreatedDate = Convert.ToDateTime(dataRow["CreatedDate"]),
                        Description = dataRow["Description"] != null && dataRow["Description"] != DBNull.Value ? Convert.ToString(dataRow["Description"]) : null,
                        FileLocation = ConfigurationManager.AppSettings["IQArchieve_PMBaseUrl"] + Convert.ToString(dataRow["FileLocation"]),
                        ID = Convert.ToInt64(dataRow["ID"]),
                        Keywords = dataRow["Keywords"] != null && dataRow["Keywords"] != DBNull.Value ? Convert.ToString(dataRow["Keywords"]) : null,
                        MediaDate = Convert.ToDateTime(dataRow["MediaDate"]),
                        MediaType = Convert.ToString(dataRow["MediaType"]),
                        ParentID = dataRow["ParentID"] != null && dataRow["ParentID"] != DBNull.Value ? (Int64?)dataRow["ParentID"] : null,
                        Pub_Name = CommonFunctions.CheckDBNullNReturnValue<string>(dataRow["Pub_Name"]),
                        SubMediaType = Convert.ToString(dataRow["SubMediaType"]),
                        Title = Convert.ToString(dataRow["Title"])
                    };

                    pmList.Add(pm);
                }

                return (IEnumerable<ArchiveMedia>)pmList.AsEnumerable();
            }



            return null;

        }

        private static readonly Materializer<ArchiveClip> s_ArchiveClipMaterializer = new Materializer<ArchiveClip>(r =>
            new ArchiveClip
            {
                ID = r.Field<Int64>("ID"),
                ParentID = r["ParentID"] != null && r["ParentID"] != DBNull.Value ? (Int64?)r["ParentID"] : null,
                MediaType = r.Field<string>("MediaType"),
                SubMediaType = r.Field<string>("SubMediaType"),
                Title = r.Field<string>("Title"),
                Content = !string.IsNullOrEmpty(r.Field<string>("Content")) ? XDocument.Parse(r.Field<string>("Content")).ToString() : string.Empty,
                MediaDate = r.Field<DateTime>("MediaDate"),
                CreatedDate = r.Field<DateTime?>("CreatedDate"),
                ClipID = r.Field<Guid>("ClipID"),
                //Station_Affil = r.Field<string>("Station_Affil"),
                //Dma_Num = r.Field<string>("Dma_Rank"),
                Audience = r["Audience"] != null && r["Audience"] != DBNull.Value ? Convert.ToInt32(r["Audience"]) : (int?)null,
                IQAdShareValue = r["IQAdShareValue"] != null && r["IQAdShareValue"] != DBNull.Value ? Convert.ToDecimal(r["IQAdShareValue"]) : (decimal?)null,
                //Market = r.Field<string>("Market"),
                //StationID = r.Field<string>("StationLogo"),
                //TimeZone = r.Field<string>("TimeZone"),
                PositiveSentiment = r["PositiveSentiment"] != null && r["PositiveSentiment"] != DBNull.Value ? Convert.ToInt16(r["PositiveSentiment"]) : (short?)null,
                NegativeSentiment = r["NegativeSentiment"] != null && r["NegativeSentiment"] != DBNull.Value ? Convert.ToInt16(r["NegativeSentiment"]) : (short?)null,
                //ClipMeta = r["ClipMeta"] != null && r["ClipMeta"] != DBNull.Value ? Convert.ToString(GetClipMeta(Convert.ToString(r["ClipMeta"]))) : null,
                CategoryGuid = r["CategoryGUID"] != null && r["CategoryGUID"] != DBNull.Value ? r.Field<Guid?>("CategoryGUID") : null,
                CategoryName = r["CategoryName"] != null && r["CategoryName"] != DBNull.Value ? Convert.ToString(r["CategoryName"]) : null,
                Keywords = r["Keywords"] != null && r["Keywords"] != DBNull.Value ? Convert.ToString(r["Keywords"]) : null,
                Description = r["Description"] != null && r["Description"] != DBNull.Value ? Convert.ToString(r["Description"]) : null,
                ClipURL = ConfigurationManager.AppSettings["ClipPlayerURL"] + r.Field<Guid>("ClipID")
            });


        private static readonly Materializer<ArchiveNM> s_ArchiveNMMaterializer = new Materializer<ArchiveNM>(r =>
           new ArchiveNM
           {
               ID = r.Field<Int64>("ID"),
               Title = r.Field<string>("Title"),
               Content = r.Field<string>("Content"),
               MediaDate = r.Field<DateTime>("MediaDate"),
               SubMediaType = r.Field<string>("SubMediaType"),
               CreatedDate = r.Field<DateTime?>("CreatedDate"),
               Audience = r.Field<int?>("Audience"),
               IQAdShareValue = r.Field<decimal?>("IQAdShareValue"),
               AudienceResult = r.Field<string>("AudienceResult"),
               PositiveSentiment = r.Field<short?>("PositiveSentiment"),
               NegativeSentiment = r.Field<short?>("NegativeSentiment"),
               Publication = r.Field<string>("Publication"),
               Url = r.Field<string>("Url")
           });


        private static readonly Materializer<ArchiveSM> s_ArchiveSMMaterializer = new Materializer<ArchiveSM>(r =>
                 new ArchiveSM
                 {
                     ID = r.Field<Int64>("ID"),
                     Title = r.Field<string>("Title"),
                     Content = r.Field<string>("Content"),
                     MediaDate = r.Field<DateTime>("MediaDate"),
                     SubMediaType = r.Field<string>("SubMediaType"),
                     CreatedDate = r.Field<DateTime?>("CreatedDate"),
                     Audience = r.Field<int?>("Audience"),
                     IQAdShareValue = r.Field<decimal?>("IQAdShareValue"),
                     AudienceResult = r.Field<string>("AudienceResult"),
                     PositiveSentiment = r.Field<short?>("PositiveSentiment"),
                     NegativeSentiment = r.Field<short?>("NegativeSentiment"),
                     Homelink = r.Field<string>("homelink"),
                     Url = r.Field<string>("Url")
                 });

        private static readonly Materializer<ArchiveTweets> s_ArchiveTweetsMaterializer = new Materializer<ArchiveTweets>(r =>
                new ArchiveTweets
                {
                    ID = r.Field<Int64>("ID"),
                    Title = r.Field<string>("Title"),
                    Content = r.Field<string>("Content"),
                    MediaDate = r.Field<DateTime>("MediaDate"),
                    SubMediaType = r.Field<string>("SubMediaType"),
                    CreatedDate = r.Field<DateTime?>("CreatedDate"),
                    PositiveSentiment = r.Field<short?>("PositiveSentiment"),
                    NegativeSentiment = r.Field<short?>("NegativeSentiment"),
                    Actor_DisplayName = r.Field<string>("Actor_DisplayName"),
                    Actor_PreferredUserName = r.Field<string>("Actor_PreferredUserName"),
                    Actor_Link = r.Field<string>("Actor_link")
                });

        private static readonly Materializer<ArchiveTVEyes> s_ArchiveTVEyesMaterializer = new Materializer<ArchiveTVEyes>(r =>
               new ArchiveTVEyes
               {
                   ID = r.Field<Int64>("ID"),
                   Title = r.Field<string>("Title"),
                   HighlightingText = r.Field<string>("Content"),
                   MediaDate = r.Field<DateTime>("MediaDate"),
                   SubMediaType = r.Field<string>("SubMediaType"),
                   CreatedDate = r.Field<DateTime?>("CreatedDate"),
                   PositiveSentiment = r.Field<short?>("PositiveSentiment"),
                   NegativeSentiment = r.Field<short?>("NegativeSentiment")
               });

        private static readonly Materializer<ArchiveBLPM> s_ArchiveBLPMMaterializer = new Materializer<ArchiveBLPM>(r =>
              new ArchiveBLPM
              {
                  ID = r.Field<Int64>("ID"),
                  Title = r.Field<string>("Title"),
                  Content = r.Field<string>("Content"),
                  MediaDate = r.Field<DateTime>("MediaDate"),
                  SubMediaType = r.Field<string>("SubMediaType"),
                  CreatedDate = r.Field<DateTime?>("CreatedDate"),
                  FileLocation = r.Field<string>("FileLocation")
              });

        public Dictionary<string, object> GetStatskedprogDataByClientGUID(Guid p_ClientGUID, out bool p_IsAllDma, out bool p_IsAllStationAffil, out bool p_IsallClass)
        {
            p_IsallClass = false;
            p_IsAllDma = false;
            p_IsAllStationAffil = false;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQ_Station_SelectSSPDataByClientGUID", CommandType.StoredProcedure, new SqlParameter("ClientGUID", p_ClientGUID));
            IEnumerable<Dma> IQ_DmaSet;
            IEnumerable<Affiliate> Station_AffilSet = null;
            IEnumerable<Class> IQ_ClassSet = null;
            IEnumerable<Region> IQ_RegionSet = null;
            IEnumerable<Country> IQ_CountrySet = null;
            IEnumerable<Station> IQ_Station_Set = null;

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {

                // First result set includes the IQ_Dma
                IQ_DmaSet = s_IQ_DMAMaterializer.Materialize(reader).ToList();

                // Second result set includes the IQ_Class
                if (reader.NextResult())
                {
                    IQ_ClassSet = s_IQ_Class.Materialize(reader).ToList();
                }

                // Third result set includes the Station_Affil
                if (reader.NextResult())
                {
                    Station_AffilSet = s_Station_AffilMaterializer.Materialize(reader).ToList();
                }

                // Fourth result set includes the Region
                if (reader.NextResult())
                {
                    IQ_RegionSet = s_IQ_RegionMaterializer.Materialize(reader).ToList();
                }

                // Fifth result set includes the Country
                if (reader.NextResult())
                {
                    IQ_CountrySet = s_IQ_CountryMaterializer.Materialize(reader).ToList();
                }

                // Sixth result set includes the Country
                if (reader.NextResult())
                {
                    IQ_Station_Set = s_Station_Materializer.Materialize(reader).ToList();
                }

                // Seventh result set inclueds isAllowedAll or not, for dma, class and station
                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        p_IsallClass = Convert.ToBoolean(reader["IsAllClassAllowed"]);
                        p_IsAllDma = Convert.ToBoolean(reader["IsAllDmaAllowed"]);
                        p_IsAllStationAffil = Convert.ToBoolean(reader["IsAllStationAllowed"]);
                    }
                }
            }

            Dictionary<string, object> dicDmaAffil = new Dictionary<string, object>();

            dicDmaAffil.Add("IQ_Dma", IQ_DmaSet);
            dicDmaAffil.Add("Station_Affil", Station_AffilSet);
            dicDmaAffil.Add("IQ_Class", IQ_ClassSet);
            dicDmaAffil.Add("IQ_Region", IQ_RegionSet);
            dicDmaAffil.Add("IQ_Country", IQ_CountrySet);
            dicDmaAffil.Add("IQ_Station", IQ_Station_Set);

            return dicDmaAffil;
        }

        public IEnumerable<ArchiveMedia> GetArchiveMedia(ArchiveInput p_ArchiveInput, Guid p_ClientGuid, Guid p_CustomerGuid, Int64 p_CustomerID, Int64 p_ClientID, out Int64 p_TotalResults, out Int64 p_SinceID)
        {
            IEnumerable<ArchiveMedia> archiveClipEnumeration = new List<ArchiveMedia>();

            p_TotalResults = 0;
            p_SinceID = 0;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQArchive_Media_SelectByParams", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("PageSize", p_ArchiveInput.Rows));
            command.Parameters.Add(new SqlParameter("FromDate", p_ArchiveInput.FromDateTime.HasValue ? p_ArchiveInput.FromDateTime.Value : (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("ToDate", p_ArchiveInput.ToDateTime.HasValue ? p_ArchiveInput.ToDateTime.Value : (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("CategoryGUID", p_ArchiveInput.Category.HasValue ? p_ArchiveInput.Category.Value : (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("ClientGUID", p_ClientGuid));
            command.Parameters.Add(new SqlParameter("CustomerGUID", p_CustomerGuid));
            command.Parameters.Add(new SqlParameter("SubMediaType", !string.IsNullOrWhiteSpace(p_ArchiveInput.SubMediaTypeEnum) ? p_ArchiveInput.SubMediaTypeEnum : (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("SeqID", p_ArchiveInput.SeqID.HasValue ? p_ArchiveInput.SeqID : 0));
            command.Parameters.Add(new SqlParameter("IsFilterOnCreatedDate", p_ArchiveInput.FilterOnCreatedDateTime));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                // Clip
                var dtTmp = new DataTable();
                dtTmp.Load(reader);

                archiveClipEnumeration = archiveClipEnumeration.Concat(CustMaterialize<ArchiveClip>(dtTmp));

                // NM
                /* if (reader.NextResult())
                 {*/
                dtTmp = new DataTable();
                dtTmp.Load(reader);
                archiveClipEnumeration = archiveClipEnumeration.Concat(CustMaterialize<ArchiveNM>(dtTmp, p_ClientID, p_CustomerID));
                // archiveClipEnumeration.Concat(s_ArchiveNMMaterializer.Materialize(dtTmp.CreateDataReader()));
                //}

                // SM
                /*if (reader.NextResult())
                {*/
                dtTmp = new DataTable();
                dtTmp.Load(reader);
                archiveClipEnumeration = archiveClipEnumeration.Concat(CustMaterialize<ArchiveSM>(dtTmp));
                // archiveClipEnumeration.Concat(s_ArchiveSMMaterializer.Materialize(dtTmp.CreateDataReader()));
                //}

                //Tweets
                /*if (reader.NextResult())
                {*/
                dtTmp = new DataTable();
                dtTmp.Load(reader);
                archiveClipEnumeration = archiveClipEnumeration.Concat(CustMaterialize<ArchiveTweets>(dtTmp));
                //  archiveClipEnumeration.Concat(s_ArchiveTweetsMaterializer.Materialize(dtTmp.CreateDataReader()));
                //}

                //TVEyes
                /*if (reader.NextResult())
                {*/
                dtTmp = new DataTable();
                dtTmp.Load(reader);
                archiveClipEnumeration = archiveClipEnumeration.Concat(CustMaterialize<ArchiveTVEyes>(dtTmp));
                //  archiveClipEnumeration.Concat(s_ArchiveTVEyesMaterializer.Materialize(dtTmp.CreateDataReader()));
                //}

                //BLPM
                /*if (reader.NextResult())
                {*/
                dtTmp = new DataTable();
                dtTmp.Load(reader);
                archiveClipEnumeration = archiveClipEnumeration.Concat(CustMaterialize<ArchiveBLPM>(dtTmp));
                //archiveClipEnumeration.Concat(s_ArchiveBLPMMaterializer.Materialize(dtTmp.CreateDataReader()));
                //}

                // seventh result set inclueds TotalResults 
                /*if (reader.NextResult())
                {*/
                while (reader.Read())
                {
                    p_TotalResults = Convert.ToInt64(reader["TotalResults"]);
                    p_SinceID = Convert.ToInt64(reader["SinceID"]);
                }
                //}
            }

            return archiveClipEnumeration;
        }

        public Dictionary<string, object> CreateTVAgent(CreateTVAgentInput p_CreateTVAgentInput, string p_SearchTerm, Guid p_ClientGuid)
        {
            Dictionary<string, object> dicTVAgent = new Dictionary<string, object>();

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_SearchRequest_Insert", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGuid));
            command.Parameters.Add(new SqlParameter("@Query_Name", p_CreateTVAgentInput.SearchRequest.AgentName.Trim()));
            command.Parameters.Add(new SqlParameter("@SearchTerm", p_SearchTerm));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    dicTVAgent.Add("IQAgentSearchRequestID", reader["IQAgentSearchRequestID"] != DBNull.Value ? Convert.ToInt32(reader["IQAgentSearchRequestID"]) : (int?)null);
                    dicTVAgent.Add("AllowedIQAgent", reader["AllowedIQAgent"] != DBNull.Value ? Convert.ToInt32(reader["AllowedIQAgent"]) : (int?)null);
                    dicTVAgent.Add("SearchRequestCount", reader["SearchRequestCount"] != DBNull.Value ? Convert.ToInt32(reader["SearchRequestCount"]) : (int?)null);
                }
            }

            return dicTVAgent;
        }

        public int UpdateTVAgent(UpdateTVAgentInput p_UpdateTVAgentInput, string p_SearchTerm, Guid p_ClientGuid)
        {
            int Output = 0;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_SearchRequest_Update", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@IQAgent_SearchRequestID", p_UpdateTVAgentInput.SRID));
            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGuid));
            command.Parameters.Add(new SqlParameter("@Query_Name", p_UpdateTVAgentInput.SearchRequest.AgentName.Trim()));
            command.Parameters.Add(new SqlParameter("@SearchTerm", p_SearchTerm));

            using (command.Connection.CreateConnectionScope())
                Output = Convert.ToInt32(command.ExecuteScalar());

            return Output;
        }

        public int DeleteTVAgent(DeleteTVAgentInput p_DeleteTVAgentInput, Guid p_ClientGuid, Guid p_CustomerGuid)
        {
            int Output = 0;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_SearchRequest_DeleteRequest", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@SearchRequestKey", p_DeleteTVAgentInput.SRID));
            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGuid));
            command.Parameters.Add(new SqlParameter("@CustomerGuid", p_CustomerGuid));

            using (command.Connection.CreateConnectionScope())
                Output = Convert.ToInt32(command.ExecuteScalar());

            return Output;
        }

        public int SuspendTVAgent(SuspendTVAgentInput p_SuspendTVAgentInput, Guid p_ClientGUID, Guid p_CustomerGUID, out short? o_PreviousIsActive)
        {
            int output = 0;
            o_PreviousIsActive = null;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_SearchRequest_SuspendRequest", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ID", p_SuspendTVAgentInput.SRID));
            command.Parameters.Add(new SqlParameter("@ClientGUID", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("@CustomerGUID", p_CustomerGUID));

            using (command.Connection.CreateConnectionScope())
            {
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    output = Convert.ToInt32(dr["AffectedRows"]);
                    o_PreviousIsActive = dr["PreviousIsActive"] != DBNull.Value ? Convert.ToInt16(dr["PreviousIsActive"]) : (short?)null;
                }
            }

            return output;
        }

        /// <summary>
        /// To resume suspended Agent.
        /// </summary>
        /// <param name="p_SRID">ID of Agent SearchRequest</param>
        /// <param name="p_ClientGUID">ClientGUID of user calling method</param>
        /// <param name="p_CustomerGUID">CustomerGUID of user calling method</param>
        /// <param name="o_PreviousIsActive">Output variable, returns IsActive value of SearchRequest before update</param>
        /// <returns>Total no. of affected rows</returns>
        public int UnSuspendTVAgent(Int64 p_SRID, Guid p_ClientGUID, Guid p_CustomerGUID, out short? o_PreviousIsActive)
        {
            int output = 0;
            o_PreviousIsActive = null;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_SearchRequest_ResumeSuspendedRequest", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ID", p_SRID));
            command.Parameters.Add(new SqlParameter("@ClientGUID", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("@CustomerGUID", p_CustomerGUID));

            using (command.Connection.CreateConnectionScope())
            {
                var dr = command.ExecuteReader();

                while (dr.Read())
                {
                    output = Convert.ToInt32(dr["AffectedRows"]);
                    o_PreviousIsActive = dr["PreviousIsActive"] != DBNull.Value ? Convert.ToInt16(dr["PreviousIsActive"]) : (short?)null;
                }
            }

            return output;
        }

        public List<TVAgent> GetTVRequestsByClientGuid(Guid p_ClientGUID, string p_SRIDList)
        {

            List<TVAgent> tvAgentList = new List<TVAgent>();

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_SearchRequest_SelectTVRequests", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("@SRIDList", p_SRIDList != null ? p_SRIDList : (object)DBNull.Value));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TVAgent tvAgent = new TVAgent();
                    tvAgent.SRID = Convert.ToInt64(reader["SRID"]);
                    tvAgent.AgentName = Convert.ToString(reader["AgentName"]);
                    tvAgent.SearchTerm = Convert.ToString(reader["SearchTerm"]);
                    tvAgent.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    tvAgent.ModifiedDate = Convert.ToDateTime(reader["ModifiedDate"]);
                    tvAgentList.Add(tvAgent);
                }
            }

            return tvAgentList;
        }

        public List<TVResult> GetIQAgentTVResultsBySearchRequestID(Guid p_ClientGUID, Guid p_CustomerGuid, Int64? p_SRID, Int64? p_SeqID, Int32? p_PageSize, out Int64 p_TotalResults, out Int64 p_SinceID)
        {

            List<TVResult> tvResultList = new List<TVResult>();

            p_TotalResults = 0;
            p_SinceID = 0;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_TVResults_SelectBySearchRequestID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("@CustomerGuid", p_CustomerGuid));

            var sqlParamSRID = new SqlParameter("@SearchRequestID", SqlDbType.BigInt);
            sqlParamSRID.Value = (object)p_SRID ?? DBNull.Value;
            command.Parameters.Add(sqlParamSRID);

            var sqlParamSeqID = new SqlParameter("@SeqID", SqlDbType.BigInt);
            sqlParamSeqID.Value = (object)p_SeqID ?? DBNull.Value;
            command.Parameters.Add(sqlParamSeqID);

            command.Parameters.Add(new SqlParameter("@PageSize", p_PageSize));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    TVResult tvResult = new TVResult();
                    tvResult.SeqID = Convert.ToInt64(reader["SeqID"]);
                    tvResult.ProgramTitle = Convert.ToString(reader["ProgramTitle"]);
                    tvResult.SRID = Convert.ToInt64(reader["SRID"]);
                    tvResult.GMTDateTime = reader["GMTDateTime"] != DBNull.Value ? Convert.ToDateTime(reader["GMTDateTime"]) : (DateTime?)null;
                    tvResult.RL_Date = Convert.ToDateTime(reader["RL_Date"]);
                    tvResult.RL_Time = Convert.ToInt32(reader["RL_Time"]);
                    tvResult.StationID = Convert.ToString(reader["StationID"]);
                    tvResult.Station = Convert.ToString(reader["Station"]);
                    tvResult.DmaName = Convert.ToString(reader["DmaName"]);
                    tvResult.HitCount = reader["HitCount"] != DBNull.Value ? Convert.ToInt32(reader["HitCount"]) : (int?)null;
                    tvResult.Audience = reader["Audience"] != DBNull.Value ? Convert.ToInt32(reader["Audience"]) : (int?)null;
                    tvResult.MediaValue = reader["MediaValue"] != DBNull.Value ? Convert.ToDecimal(reader["MediaValue"]) : (decimal?)null;
                    tvResult.HighLights = Convert.ToString(reader["HighLights"]);
                    tvResult.PositiveSentiment = reader["PositiveSentiment"] != DBNull.Value ? Convert.ToInt32(reader["PositiveSentiment"]) : (int?)null;
                    tvResult.NegativeSentiment = reader["NegativeSentiment"] != DBNull.Value ? Convert.ToInt32(reader["NegativeSentiment"]) : (int?)null;
                    tvResult.VideoGuid = new Guid(Convert.ToString(reader["VideoGuid"]));
                    tvResult.ParentID = reader["ParentID"] != DBNull.Value ? Convert.ToInt32(reader["ParentID"]) : (Int64?)null;
                    tvResult.ThumbUrl = Convert.ToString(reader["ThumbUrl"]);
                    tvResultList.Add(tvResult);
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        p_TotalResults = Convert.ToInt64(reader["TotalResults"]);
                        p_SinceID = reader["SinceID"] != DBNull.Value ? Convert.ToInt32(reader["SinceID"]) : 0;
                    }
                }
            }
            return tvResultList;
        }

        public Dictionary<string, List<int>> GetPendingDeleteAgentRequestsNResults(Guid p_ClientGUID)
        {
            DbCommand command = this.CreateStoreCommand("usp_v4_IQAgent_MediaResults_SelectInactive", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGuid", p_ClientGUID));

            Dictionary<string, List<int>> dictResult = new Dictionary<string, List<int>>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                List<int> mediaResultIDList = new List<int>();

                while (reader.Read())
                {
                    mediaResultIDList.Add(Convert.ToInt32(reader["ID"]));
                }

                dictResult.Add("MediaResult", mediaResultIDList);

                if (reader.NextResult())
                {
                    List<int> searchRequestIDList = new List<int>();

                    while (reader.Read())
                    {
                        searchRequestIDList.Add(Convert.ToInt32(reader["SearchRequestID"]));
                    }

                    dictResult.Add("SearchRequest", searchRequestIDList);
                }
            }

            return dictResult;
        }

        public Dictionary<Int64, Int64> GetIQAgentTVResultsUpdate(Guid p_ClientGUID, Guid p_CustomerGuid, Int64? p_SeqID, Int32? p_PageSize, out Int64 p_TotalResults, out Int64 p_SinceID)
        {
            p_TotalResults = 0;
            p_SinceID = 0;

            DbCommand command = this.CreateStoreCommand("usp_isvc_IQAgent_MediaResults_UpdatedRecords_SelectByClientGUID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("@ClientGUID", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("@SeqID", p_SeqID ?? (object)DBNull.Value));
            command.Parameters.Add(new SqlParameter("@PageSize", p_PageSize));

            var updatedIDList = new Dictionary<Int64, Int64>();

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    updatedIDList.Add(Convert.ToInt64(reader["ID"]), Convert.ToInt64(reader["_MediaResultID"]));
                }

                if (reader.NextResult())
                {
                    while (reader.Read())
                    {
                        p_TotalResults = Convert.ToInt64(reader["TotalResults"]);
                        p_SinceID = reader["SinceID"] != DBNull.Value ? Convert.ToInt32(reader["SinceID"]) : 0;
                    }
                }
            }

            return updatedIDList;
        }

        private static dynamic GetClipMeta(string p_ClipMeta)
        {
            XDocument xDoc = XDocument.Parse(p_ClipMeta);
            string KeyValue = string.Empty;
            int keyInt = 0;
            dynamic metaData = new MetaData();

            foreach (XElement element in xDoc.Descendants().Where(p => p.HasElements == false))
            {
                keyInt++;
                if (keyInt % 2 != 0)
                {
                    KeyValue = element.Value;
                }
                else
                {
                    metaData.SetMember(KeyValue, element.Value);
                }
            }
            return metaData;
        }

        public ArchiveClip GetArchiveClipByClipGUID(Guid p_ClipGUID, Guid p_ClientGUID)
        {
            DbCommand command = this.CreateStoreCommand("usp_isvc_IQArchive_Media_SelectClipByClipGUID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("ClientGUID", p_ClientGUID));
            command.Parameters.Add(new SqlParameter("ClipGUID", p_ClipGUID));

            ArchiveClip clip = null;


            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    clip = new ArchiveClip();

                    var station = (from ele in (XDocument.Parse(Convert.ToString(reader["StationDetail"])).Descendants("StationDetail"))
                                   select new
                                   {
                                       market = ele.Element("Dma_Name") != null ? ele.Element("Dma_Name").Value : "",
                                       stationAffil = ele.Element("Station_Affil") != null ? ele.Element("Station_Affil").Value : "",
                                       StationID = ele.Element("IQ_Station_ID") != null ? ele.Element("IQ_Station_ID").Value : "",
                                       StationCallSign = ele.Element("Station_Call_Sign") != null ? ele.Element("Station_Call_Sign").Value : "",
                                       TimeZone = ele.Element("TimeZone") != null ? ele.Element("TimeZone").Value : ""
                                   }).FirstOrDefault();

                    clip.Audience = reader["Audience"] != null && reader["Audience"] != DBNull.Value ? Convert.ToInt32(reader["Audience"]) : (int?)null;
                    clip.AudienceResult = CommonFunctions.CheckDBNullNReturnValue<String>(reader["AudienceResult"]);
                    clip.CategoryGuid = reader["CategoryGUID"] != null && reader["CategoryGUID"] != DBNull.Value ? Guid.Parse(Convert.ToString(reader["CategoryGUID"])) : Guid.Empty;
                    clip.CategoryName = reader["CategoryName"] != null && reader["CategoryName"] != DBNull.Value ? Convert.ToString(reader["CategoryName"]) : null;
                    clip.ClipID = new Guid(Convert.ToString(reader["ClipID"]));
                    clip.ClipMeta = reader["ClipMeta"] != null && reader["ClipMeta"] != DBNull.Value ? Convert.ToString(GetClipMeta(Convert.ToString(reader["ClipMeta"]))) : null;
                    clip.ClipURL = ConfigurationManager.AppSettings["ClipPlayerURL"] + reader["ClipID"];
                    clip.Content = !string.IsNullOrEmpty(Convert.ToString(reader["Content"])) ? XDocument.Parse(Convert.ToString(reader["Content"])).ToString() : string.Empty;
                    clip.CreatedDate = Convert.ToDateTime(reader["CreatedDate"]);
                    clip.Description = reader["Description"] != null && reader["Description"] != DBNull.Value ? Convert.ToString(reader["Description"]) : null;
                    clip.ID = Convert.ToInt64(reader["ID"]);
                    clip.IQAdShareValue = reader["IQAdShareValue"] != null && reader["IQAdShareValue"] != DBNull.Value ? Convert.ToDecimal(reader["IQAdShareValue"]) : (decimal?)null;
                    clip.Keywords = reader["Keywords"] != null && reader["Keywords"] != DBNull.Value ? Convert.ToString(reader["Keywords"]) : null;
                    clip.Market = station.market;
                    clip.MediaDate = Convert.ToDateTime(reader["MediaDate"]);
                    clip.MediaType = Convert.ToString(reader["MediaType"]);
                    clip.NegativeSentiment = reader["NegativeSentiment"] != null && reader["NegativeSentiment"] != DBNull.Value ? Convert.ToInt16(reader["NegativeSentiment"]) : (short?)null;
                    clip.ParentID = reader["ParentID"] != null && reader["ParentID"] != DBNull.Value ? (Int64?)reader["ParentID"] : null;
                    clip.PositiveSentiment = reader["PositiveSentiment"] != null && reader["PositiveSentiment"] != DBNull.Value ? Convert.ToInt16(reader["PositiveSentiment"]) : (short?)null;
                    clip.Station = station.StationCallSign;
                    clip.Station_Affil = station.stationAffil;
                    clip.StationID = station.StationID;
                    clip.SubMediaType = Convert.ToString(reader["SubMediaType"]);
                    clip.TimeZone = station.TimeZone;
                    clip.Title = Convert.ToString(reader["Title"]);
                    clip.StartOffset = Convert.ToInt32(reader["StartOffset"]);
                    clip.EndOffset = Convert.ToInt32(reader["EndOffset"]);
                }
            }

            return clip;
        }

        public Dictionary<string,string> GetCustomerDetailsForAuthentication(string p_LoginID)
        {
            Dictionary<string, string> result = null;

            DbCommand command = this.CreateStoreCommand("usp_isvc_Customer_SelectForAuthentication", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("LoginID", p_LoginID));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    result = new Dictionary<string, string>();

                    result.Add("Password", Convert.ToString(reader["CustomerPassword"]));
                    result.Add("PasswordAttempts", Convert.ToString(reader["PasswordAttempts"]));
                }
            }

            return result;
        }

        public void UpdatePasswordAttempts(string p_LoginID, bool p_ResetPasswordAttempts)
        {
            DbCommand command = this.CreateStoreCommand("usp_isvc_Customer_UpdatedPasswordAttempts", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("LoginID", p_LoginID));
            command.Parameters.Add(new SqlParameter("ResetPasswordAttempts", p_ResetPasswordAttempts));

            using (command.Connection.CreateConnectionScope())
            {
                command.ExecuteNonQuery();
            }
        }

        public string GetClipThumbnailURL(Guid p_ClipGUID)
        {
            var clipThumbURL = "";

            DbCommand command = this.CreateStoreCommand("usp_isvc_AssetLocation_SelectByClipGUID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("ClipGUID", p_ClipGUID));

            using (command.Connection.CreateConnectionScope())
            {
                clipThumbURL = Convert.ToString(command.ExecuteScalar());
            }

            return clipThumbURL;
        }

        /// <summary>
        /// Get Customer Roles of Feeds and All media types
        /// </summary>
        /// <param name="p_CustomerGUID">CustomerGUID</param>
        /// <returns>Dictionary<string,bool> contains role name and Customer has access or not.</returns>
        public Dictionary<string, bool> GetCustomerFeedsRoles(Guid p_CustomerGUID)
        {
            var roles = new Dictionary<string, bool>();

            DbCommand command = this.CreateStoreCommand("usp_isvc_Role_SelectRoleByCustomerGUID", CommandType.StoredProcedure);

            command.Parameters.Add(new SqlParameter("CustomerGUID", p_CustomerGUID));

            using (command.Connection.CreateConnectionScope())
            using (DbDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    roles.Add(Convert.ToString(reader["RoleName"]), Convert.ToBoolean(reader["HasAccess"]));
                }
            }

            return roles;
        }

        #endregion
    }
}
