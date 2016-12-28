using System;
using System.Configuration;
using System.Xml.Serialization;
using IQMedia.Domain;
using IQMedia.Logic;

namespace IQMedia.Web.Services.Serializers
{
    public class GetVarsRecordfileSerializer : IXmlSerializable
    {
        private readonly Recordfile _rFile;

        public GetVarsRecordfileSerializer(Recordfile rFile)
        {
            _rFile = rFile;
        }

        #region IXmlSerializable Members

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var hasVideo = _rFile.RecordfileType.IsVideo();

            //TODO: Remove this as it is a hack for the change from fms2 to fms3
            var streamUrl = new Uri(_rFile.RootPath.StreamSuffixPath).Host;
            streamUrl = streamUrl.Replace("media2.", "fms3.");

            writer.WriteStartElement("root");
            writer.WriteAttributeString("status", "1"); //If we're here, it worked
            writer.WriteAttributeString("msg", ""); //No message for good status
            writer.WriteAttributeString("rid", _rFile.Guid.ToString());

            //Begin MediaInfo Vars
            writer.WriteStartElement("mediaInfo");
                writer.WriteAttributeString("title", "Raw Media");
                writer.WriteAttributeString("categoryKey", "NA");
                writer.WriteAttributeString("categoryName", "N/A");
                writer.WriteAttributeString("keywords", "Raw Media");
                writer.WriteAttributeString("author", _rFile.Recording.Source.Title);
                writer.WriteAttributeString("startTime", _rFile.StartOffset.ToString());
                writer.WriteAttributeString("endTime", _rFile.EndOffset.ToString());
                writer.WriteAttributeString("sourceTitle", _rFile.Recording.Source.Title);
                writer.WriteAttributeString("sourceLogo", ConfigurationManager.AppSettings["SourceSmallLogoUrlPrefix"] + _rFile.Recording.Source.Logo);
                writer.WriteAttributeString("sourceUrl", _rFile.Recording.Source.Url);
                writer.WriteAttributeString("airdate", _rFile.Recording.StartDate.ToShortDateString());
                writer.WriteAttributeString("dateCreated", _rFile.DateCreated.ToShortDateString());
                writer.WriteAttributeString("plays", "0");
                writer.WriteCData("");
            writer.WriteEndElement();

            //Begin Recordfile Vars
            writer.WriteStartElement("vars");
                String filename = new Uri(_rFile.RootPath.StreamSuffixPath + _rFile.Location).LocalPath;
                writer.WriteAttributeString("fileName", filename.Substring(1, filename.Length - 1));
                writer.WriteAttributeString("streamUrl", streamUrl);
                writer.WriteAttributeString("serviceUrl", ConfigurationManager.AppSettings["RESTServicesPath"]);
                writer.WriteAttributeString("appNameFMS", _rFile.RootPath.AppName);
                writer.WriteAttributeString("hasVideo", hasVideo.ToString().ToUpper());
                writer.WriteAttributeString("hasCaption", Convert.ToBoolean(ConfigurationManager.AppSettings["EnableCC"])
                                                        ? hasVideo.ToString().ToUpper() : "FALSE");
                writer.WriteAttributeString("logPlayURL", ConfigurationManager.AppSettings["RESTServicesPath"] + "/svc/logs/logRawPlay");

                //These values are hard-coded to "-1" until they prove useful
                writer.WriteAttributeString("bitRate", "-1");
                writer.WriteAttributeString("videoWidth", "-1");
                writer.WriteAttributeString("videoHeight", "-1");
                writer.WriteAttributeString("aspectRatio", "-1");

                //If it's video, then it has a thumbUrl, if its audio, then it has an audioGraphic
                var assLgc = (AssetLogic)LogicFactory.GetLogic(LogicType.Asset);
                if (hasVideo) writer.WriteAttributeString("thumbUrl", assLgc.GetThumbnailUrl(_rFile, ThumbnailType.Small));
                else writer.WriteAttributeString("audioGraphic", assLgc.GetThumbnailUrl(_rFile, ThumbnailType.Player));
                
                //NOTE: There are no menu vars for raw media... (but we hacked these just in case...)
                writer.WriteAttributeString("userId", Guid.Empty.ToString());
                /*if (!_currentUser.Guid.Equals(Guid.Empty))
                    writer.WriteAttributeString("emailFrom", _currentUser.Email);*/
                writer.WriteAttributeString("embedUrl", ConfigurationManager.AppSettings["DefaultEmbedUrl"]);
                writer.WriteAttributeString("linkUrl", ConfigurationManager.AppSettings["DefaultLinkUrl"]);
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