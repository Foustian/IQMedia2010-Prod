using System;
using System.Configuration;
using System.IO;
using System.Web;
using System.Xml;
using IQMedia.Common.Util;
using IQMedia.Logic;

namespace IQMedia.Web.Services.Commands
{
    public class GetClosedCaptioning : ICommand
    {
        private readonly Guid? _mediaGuid;
        private int _startTime;
        private int _endTime;

        public GetClosedCaptioning(object mediaGuid, object startTime, object endTime)
        {
            _mediaGuid = (mediaGuid is NullParameter) ? null : (Guid?)mediaGuid;
            _startTime = (startTime is NullParameter) ? -1 : Convert.ToInt32(startTime);
            _endTime = (endTime is NullParameter) ? -1 : Convert.ToInt32(endTime);
        }

        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {
            //Throw an exception if we don't have a valid Guid
            if (!_mediaGuid.HasValue)
                throw new ArgumentException("Invalid or missing Media Guid.");

            var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);
            var rfLgc = (RecordfileLogic)LogicFactory.GetLogic(LogicType.Recordfile);

            var result = String.Empty;
            try
            {
                //First we check to see if the _fileId is a clip, if not, it must be a recordfile
                var clip = clipLgc.GetClip(_mediaGuid.Value);
                var rfile = (clip != null) ? clip.Recordfile : rfLgc.GetRecordfile(_mediaGuid.Value);

                //If the start & end offsets weren't defined AND we have a clip; set them manually
                if (clip != null)
                {
                    if (_startTime == -1) _startTime = clip.StartOffset;
                    if (_endTime == -1) _endTime = clip.EndOffset;
                }

                //Only do the work if we have a recordfile
                if (rfile != null)
                {
                    var txtRf = rfLgc.GetTextRecordfile(rfile);
                    var filePath = txtRf.RootPath.StoragePath + txtRf.Location;

                    //NOTE: Hack for legacy; REMOVE when functional!
                    if (ConfigurationManager.AppSettings["UseLegacyCCPath"].ToLower() == "true")
                    {
                        var tmp = txtRf.Location.Substring(txtRf.Location.LastIndexOf("\\") + 1);
                        filePath = "\\\\pmgfs01\\RL_Files\\" + txtRf.Recording.StartDate.ToString("yyyy-MM") + "\\" +
                            //Gotta insert these annoying double-zeros for the legacy case...       
                            tmp.Insert(tmp.LastIndexOf('.'), "00");
                    }

                    Logger.Debug("Attempting to fetch cc data from: " + filePath);
                    var fileStream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    var stream = new StreamReader(fileStream);
                    var content = stream.ReadToEnd();
                    stream.Close();

                    result = GetCCRange(content, _startTime, _endTime);
                }
                //Must not have a clip or a recordfile; throw exception
                else throw new Exception("No Recordfile found for requested GUID.");
            }
            //Log the error and pass an empty CC data file
            catch (Exception ex)
            {
                Logger.Error("GetClosedCaptioning()", ex);
                result = "<tt xmlns='http://www.w3.org/2006/10/ttaf1' lang='EN'><head><metadata><title xmlns='http://www.w3.org/2006/10/ttaf1#metadata'>IQMEDIA</title></metadata></head> <body region='subtitleArea'><div><p begin='0s' end='0s'></p></div></body></tt>";
            }
            //We want to write a response whether we had an error or not...
            finally
            {
                response.ContentType = "text/xml";
                response.Write(result);                
            }
        }

        #endregion

        private static string GetCCRange(string xml, int startOffset, int endOffset)
        {
            var xDoc = new XmlDocument();
            xDoc.LoadXml(xml);

            var ccData = xDoc.GetElementsByTagName("p");
            for(var i=0; i<ccData.Count; i++)
            {
                var ccPart = ccData[i];
                //Skip this node if its missing anything (Possible bad data)
                if (ccPart.Attributes == null || ccPart.ParentNode == null) continue;

                var begin = int.Parse(ccPart.Attributes["begin"].InnerText.Substring(
                    0, ccPart.Attributes["begin"].InnerText.Length - 1));
                var end = int.Parse(ccPart.Attributes["end"].InnerText.Substring(
                    0, ccPart.Attributes["end"].InnerText.Length - 1));

                //If the ccPart is not within range, remove it from the document
                if (end <= startOffset || (endOffset > 0 && begin > endOffset))
                {
                    ccPart.ParentNode.RemoveChild(ccPart);
                    i--;
                }
            }

            // Now create StringWriter object to get data from xml document.
            var sw = new StringWriter();
            var xtw = new XmlTextWriter(sw);
            xDoc.WriteTo(xtw);
            return sw.ToString();
        }
    }
}