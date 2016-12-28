using System;
using System.Configuration;
using System.Web;
using System.Xml.Serialization;
using IQMedia.Domain;
using IQMedia.Logic;

namespace IQMedia.Web.Services.Serializers
{
    public class GetVarsClipSerializer : IXmlSerializable
    {
        private readonly Clip _clip;
        private readonly User _author;
        private readonly User _currentUser;
        private readonly Guid _pid;

        public GetVarsClipSerializer(Clip clip, User author, User currentUser, Guid pid)
        {
            _clip = clip;
            _author = author;
            _currentUser = currentUser;
            _pid = pid;
        }
        
        #region IXmlSerializable Members

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);
            var hasVideo = _clip.Recordfile.RecordfileType.IsVideo();
            
            //TODO: Remove this as it is a hack for the change from fms2 to fms3
            var streamUrl = new Uri(_clip.Recordfile.RootPath.StreamSuffixPath).Host;
            streamUrl = streamUrl.Replace("media2.", "fms3.");

            //TODO: Query for actual view count...
            var trackLgc = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
            var plays = trackLgc.GetPlayCount(_clip.Guid);
            
            writer.WriteStartElement("root");
            writer.WriteAttributeString("status", "1"); //If we're here, it worked
            writer.WriteAttributeString("msg", ""); //No message for good status
            writer.WriteAttributeString("rid", _clip.Guid.ToString());
                //Begin MediaInfo Vars
                writer.WriteStartElement("mediaInfo");
                    writer.WriteAttributeString("title", _clip.ClipInfo.Title);
                    writer.WriteAttributeString("categoryKey", _clip.ClipInfo.CategoryKey);
                    writer.WriteAttributeString("categoryName", clipLgc.GetCategoryDescription(_clip.ClipInfo.CategoryKey));
                    writer.WriteAttributeString("keywords", _clip.ClipInfo.Keywords);
                    writer.WriteAttributeString("author", (!_clip.ClipInfo.CategoryKey.Equals("PR")) ? _author.UserName : "Anonymous");
                    writer.WriteAttributeString("startTime", _clip.StartOffset.ToString());
                    writer.WriteAttributeString("endTime", _clip.EndOffset.ToString());
                    writer.WriteAttributeString("sourceName", _clip.Recordfile.Recording.Source.Title);
                    writer.WriteAttributeString("sourceLogo", ConfigurationManager.AppSettings["SourceSmallLogoUrlPrefix"] + _clip.Recordfile.Recording.Source.Logo);
                    writer.WriteAttributeString("sourceUrl", _clip.Recordfile.Recording.Source.Url);
                    writer.WriteAttributeString("airdate", _clip.Recordfile.Recording.StartDate.ToShortDateString());
                    writer.WriteAttributeString("dateCreated", _clip.DateCreated.ToShortDateString());
                    writer.WriteAttributeString("plays", "" + plays);
                    writer.WriteCData(HttpContext.Current.Server.UrlDecode(_clip.ClipInfo.Description));
                writer.WriteEndElement();

                //Begin Recordfile Vars
                writer.WriteStartElement("vars");
                    String filename = new Uri(_clip.Recordfile.RootPath.StreamSuffixPath + _clip.Recordfile.Location).LocalPath;
                    writer.WriteAttributeString("fileName", filename.Substring(1, filename.Length - 1));
                    writer.WriteAttributeString("streamUrl", streamUrl);
                    writer.WriteAttributeString("serviceUrl", ConfigurationManager.AppSettings["RESTServicesPath"]);
                    writer.WriteAttributeString("appNameFMS", _clip.Recordfile.RootPath.AppName);
                    writer.WriteAttributeString("hasVideo", hasVideo.ToString().ToUpper());
                    writer.WriteAttributeString("hasCaption", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCC"])
                                        ? hasVideo.ToString().ToUpper() : "FALSE");
                    writer.WriteAttributeString("logPlayURL", ConfigurationManager.AppSettings["RESTServicesPath"] + "/svc/logs/logClipPlay");

                    //These values are hard-coded to "-1" until they prove useful
                    writer.WriteAttributeString("bitRate", "-1");
                    writer.WriteAttributeString("videoWidth", "-1");
                    writer.WriteAttributeString("videoHeight", "-1");
                    writer.WriteAttributeString("aspectRatio", "-1");
                    
                    //If it's video, then it has a thumbUrl, if its audio, then it has an audioGraphic
                    var assLgc = (AssetLogic) LogicFactory.GetLogic(LogicType.Asset);
                    if (hasVideo) writer.WriteAttributeString("thumbUrl", assLgc.GetThumbnailUrl(_clip, ThumbnailType.Small));
                    else writer.WriteAttributeString("audioGraphic", assLgc.GetThumbnailUrl(_clip, ThumbnailType.Player));

                    //Player Vars for Menus
                    writer.WriteAttributeString("userId", _author.Guid.ToString());
                    if (!_currentUser.Guid.Equals(Guid.Empty))
                        writer.WriteAttributeString("emailFrom", _currentUser.Email);
                    writer.WriteAttributeString("embedUrl", ConfigurationManager.AppSettings["DefaultEmbedUrl"]);
                    //see if there is a special link for this partner, if no then null and use PlayerUrl in web.config
                    var linkUrl = clipLgc.GetPartnerLinkUrl(_pid);
                    linkUrl = (String.IsNullOrEmpty(linkUrl)) ? ConfigurationManager.AppSettings["DefaultLinkUrl"] : linkUrl;
                    writer.WriteAttributeString("linkUrl", linkUrl);
                writer.WriteEndElement();
            
            writer.WriteEndElement();
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}