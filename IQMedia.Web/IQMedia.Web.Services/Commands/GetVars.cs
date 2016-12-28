using System;
using System.Configuration;
using System.Web;
using IQMedia.Common.Util;
using IQMedia.Logic;
using IQMedia.Web.Common;
using IQMedia.Web.Services.Serializers;

namespace IQMedia.Web.Services.Commands
{
    public class GetVars : AbstractXmlCommand, ICommand
    {
        private readonly Guid? _mediaGuid;
        private readonly Guid? _pid;
        private readonly bool _local;

        public GetVars(object mediaGuid, object pid, object local)
        {
            _mediaGuid = (mediaGuid is NullParameter) ? null : (Guid?)mediaGuid;
            _pid = (pid is NullParameter) ? null : (Guid?)pid;
            _local = (local == null || local is NullParameter) ? false : (bool)local;
        }

        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {
            try
            {
                //Throw an exception if we don't have a valid Guid
                if (!_mediaGuid.HasValue)
                    throw new ArgumentException("Invalid or missing Media Guid.");

                var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

                //First see if the media is a clip...
                var clip = clipLgc.GetClip(_mediaGuid.Value);
                if (clip != null)
                {
                    //If we're only looking for local files, make the changes...
                    if (ConfigurationManager.AppSettings["IgnoreCdn"].ToLower() == "true" || _local == true)
                    {
                        var rfLgc = (RecordfileLogic)LogicFactory.GetLogic(LogicType.Recordfile);
                        clip.Recordfile = rfLgc.GetLocalRecordfile(clip.Recordfile);
                    }

                    var userLgc = (UserLogic)LogicFactory.GetLogic(LogicType.User);
                    var author = userLgc.GetUser(clip.UserGuid);
                    base.Serializer = new GetVarsClipSerializer(clip, author, Authentication.CurrentUser, _pid.GetValueOrDefault(Guid.Empty));
                    base.WriteXmlOutput(response);
                }
                //If not, try to see if its a recordfile
                else
                {
                    var rfLgc = (RecordfileLogic)LogicFactory.GetLogic(LogicType.Recordfile);
                    var rFile = rfLgc.GetRecordfile(_mediaGuid.Value);
                    if(rFile != null)
                    {
                        base.Serializer = new GetVarsRecordfileSerializer(rFile);
                        base.WriteXmlOutput(response);
                    }
                    //Its not a clip OR recordfile... ERROR!
                    else throw new Exception("Invalid Media Guid");
                }
            }
            catch (Exception ex)
            {
                Logger.Error("GetVars()", ex);
                base.WriteXmlError(response, ex, _mediaGuid.GetValueOrDefault(Guid.Empty));
            }
        }

        #endregion
    }
}