using System;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using IQMedia.Common.Util;
using IQMedia.Domain;
using IQMedia.Domain.Sparse;
using IQMedia.Logic;
using IQMedia.Web.Common;

namespace IQMedia.Web.Services.Commands
{
    public class UpdateClip : BaseCommand, ICommand
    {
        private static readonly JavaScriptSerializer _serializer = new JavaScriptSerializer();
        private readonly SparseClip _clip;

        public UpdateClip(string clip)
        {
            try { _clip = _serializer.Deserialize<SparseClip>(clip); }
            catch (Exception ex) {
                Logger.Error("An error occurred while deserializing the input clip.", ex);
            }
        }
        
        #region ICommand Members

        public void Execute(HttpRequest request, HttpResponse response)
        {
            string[] result;
            var user = Authentication.CurrentUser;

            if(_clip != null) {
                var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);
                var c = clipLgc.GetClip(_clip.Guid);

                if(c != null) {
                    if(c.UserGuid.Equals(user.Guid)) {
                        try {
                            //OK, we're good to go...
                            c.ClipInfo.Title = (!String.IsNullOrWhiteSpace(_clip.Title))
                                ? _clip.Title : c.ClipInfo.Title;
                            c.ClipInfo.CategoryKey = (!String.IsNullOrWhiteSpace(_clip.CategoryKey))
                                ? _clip.CategoryKey : c.ClipInfo.CategoryKey;
                            c.ClipInfo.Description = (!String.IsNullOrWhiteSpace(_clip.Description))
                                ? _clip.Description : c.ClipInfo.Description;

                            //Time to do ClipMeta
                            if (_clip.ClipMeta != null && _clip.ClipMeta.Count() > 0)
                            {
                                var keys = c.ClipMeta.Select(m => m.Field);
                                foreach (var meta in _clip.ClipMeta)
                                {
                                    if (keys.Contains(meta.Key))
                                        c.ClipMeta.Single(m => m.Field == meta.Key).Value = meta.Value;
                                    else c.ClipMeta.Add(new ClipMeta { ClipGuid = c.Guid, Field = meta.Key, Value = meta.Value });
                                }
                            }

                            //Persist changes
                            clipLgc.SaveChanges();
                            result = new[] { "0", "Clip Updated Successfully." };
                        } 
                        catch(Exception ex) {
                            Logger.Error("An error occurred while updating the clip: " + _clip.Guid, ex);
                            result = new[] { "4", "An error occurred while updating the clip." };
                        }
                    }
                    else result = new[] { "3", "You are either not logged in, or you do not own this clip." };
                }
                else result = new[] { "2", "Invalid Clip Guid." };
            }
            else result = new[] { "1", "Invalid Clip Data." };

            //Serialize and return the response...
            var json = _serializer.Serialize(result);
            response.Write(json);
        }

        #endregion
    }
}