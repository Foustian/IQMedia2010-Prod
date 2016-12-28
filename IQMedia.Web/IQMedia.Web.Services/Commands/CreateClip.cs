using System;
using System.Configuration;
using System.IO;
using System.Security.Authentication;
using System.Web;
using System.Xml;
using IQMedia.Common.Util;
using IQMedia.Logic;
using IQMedia.Web.Common;
using IQMedia.Web.Services.Serializers;

namespace IQMedia.Web.Services.Commands
{
    public class CreateClip : AbstractXmlCommand, ICommand
    {
        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {
            base.Serializer = new CreateClipSerializer();
            var xtr = new XmlTextReader(request.InputStream);
            try
            {
                //First deserialize the incoming data
                base.Serializer.ReadXml(xtr);
                
                //Then check to make sure we're logged
                if (!Authentication.IsAuthenticated)
                    throw new AuthenticationException("Error Creating Clip. User is not authenticated.");

                var clip = ((CreateClipSerializer) base.Serializer).Clip;
                var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);
                clipLgc.Insert(clip);

                //Log the initial impression
                bool logImpressions;
                Boolean.TryParse(ConfigurationManager.AppSettings["LogClipImpressions"], out logImpressions);
                if (logImpressions && clip != null)
                {
                    var trackLgc = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
                    //NOTE: UserGuid and IP Address are useless here as its only incrementing the viewsummary
                    trackLgc.InsertImpression(clip.Guid, Authentication.CurrentUser.Guid, HttpContext.Current.Request.UserHostAddress);
                }

                //TODO: Notify ClipSearch of the new clip to be added to the index
                //clipLgc.NotifyClipSearch(clip);

                //TODO: Send request to Thumbnail Generator Service
                //clipLgc.GenerateThumbnail(width, height, offset, clip.Guid);

                //TODO: Notify CDNService of new clip
                //clipLgc.PublishToCdn(clip.Guid);
            }
            catch (Exception ex)
            {
                Logger.Error("CreateClip()", ex);
                //If its not an authentication exception, mask the exception so we don't exposed anything dangerous...
                if(!(ex is AuthenticationException))
                    ex = new Exception("Error Creating Clip. Please try again later.");
                ((CreateClipSerializer)base.Serializer).Exception = ex;
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "text/xml";
            var sw = new StringWriter();
            var xtw = new XmlTextWriter(sw);
            base.Serializer.WriteXml(xtw);
            response.Output.Write(sw.ToString());
        }

        #endregion
    }
}