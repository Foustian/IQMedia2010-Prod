using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.ExposeApi.Domain;
using IQMediaGroup.Common.Util;
using System.Reflection;
using System.ComponentModel;
using System.Configuration;
using System.Xml.Linq;
using System.Dynamic;

namespace IQMediaGroup.ExposeApi.Logic
{
    public class ArchiveLogic : BaseLogic, ILogic
    {

        public ArchiveOutput GetArchiveData(ArchiveInput p_ArchiveInput, Guid p_ClientGuid, Guid p_CustomerGuid, Int64 p_CustomerID, Int64 p_ClientID, string p_USeqID = null)
        {
            try
            {
                ArchiveOutput objArchiveOutput = new ArchiveOutput();
                Int64 TotalResults = 0;
                Int64 SinceID = 0;
                var ArchiveData = Context.GetArchiveMedia(p_ArchiveInput, p_ClientGuid, p_CustomerGuid, p_CustomerID, p_ClientID, out TotalResults, out SinceID);
                objArchiveOutput.ArchiveMediaList = ArchiveData.OrderBy(am => am.ID);
                objArchiveOutput.TotalResults = TotalResults;
                if (objArchiveOutput.ArchiveMediaList != null && objArchiveOutput.ArchiveMediaList.Count() > 0 && objArchiveOutput.ArchiveMediaList.Max(a => a.ID) < SinceID)
                {
                    objArchiveOutput.HasNextPage = true;
                }
                else
                {
                    objArchiveOutput.HasNextPage = false;
                }
                return objArchiveOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Error(p_USeqID, ex);
                throw;
            }
        }
        
        /*
        private List<ArchiveMedia> FillIQArchieveResults(IEnumerable<ArchiveClip> p_ArchiveClipEnumeration)
        {
            List<ArchiveMedia> lstArchiveMedia = new List<ArchiveMedia>();

            if (p_ArchiveClipEnumeration != null && p_ArchiveClipEnumeration.Count() > 0)
            {

                ClipMeta clipMeta = new ClipMeta();
                List<ArchiveMedia> lstArchiveMediaClip = p_ArchiveClipEnumeration.Select(a => new ArchiveMedia
                {
                    SeqID = a.ID,
                    Title = a.Title,
                    CreatedDateTime = a.CreatedDate,
                    MediaDateTime = a.MediaDate,
                    Text = a.HighlightingText,
                    StationID = a.StationID,
                    Market = a.Market,
                    TimeZone = a.TimeZone,
                    PositiveSentiment = a.PositiveSentiment,
                    NegativeSentiment = a.NegativeSentiment,
                    Audience = a.Audience,
                    MediaValue = a.IQAdShareValue,
                    ClipID = a.ClipID,
                    Rank = a.Dma_Num,
                    Affiliate = a.Station_Affil,
                    ClipMeta = !string.IsNullOrEmpty(a.ClipMeta) ? GetClipMeta(a.ClipMeta) : null,
                    ParentID = a.ParentID,
                    CategoryGuid = a.CategoryGuid,
                    CategoryName = a.CategoryName,
                    ClipURL = ConfigurationManager.AppSettings["ClipPlayerURL"] + a.ClipID,
                    Keywords = a.Keywords,
                    Description = a.Description
                }).ToList();

                lstArchiveMedia.AddRange(lstArchiveMediaClip);

                lstArchiveMedia = lstArchiveMedia.OrderBy(a => a.SeqID).ToList();
            }

            return lstArchiveMedia;
        }
        */

        public ArchiveClipOutput GetArchiveClipByClipGUID(Guid p_ClipGUID, Guid p_ClientGUID)
        {
            ArchiveClipOutput clipOutput = new ArchiveClipOutput();

            var clip = Context.GetArchiveClipByClipGUID(p_ClipGUID, p_ClientGUID);
            clipOutput.Clip=clip;

            return clipOutput;        
        }

        public static string GetEnumDescription(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

            if (attributes != null &&
                attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        public string GetClipThumbnailURL(Guid p_ClipGUID)
        {
            var clipThumbURL = Context.GetClipThumbnailURL(p_ClipGUID);
            clipThumbURL = clipThumbURL.Replace("\\","/");

            return clipThumbURL;
        }       
    }
}
