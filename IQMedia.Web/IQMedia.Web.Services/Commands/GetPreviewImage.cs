using System;
using System.Configuration;
using System.Web;
using IQMedia.Common.Util;
using IQMedia.Logic;
using IQMedia.Web.Common;

namespace IQMedia.Web.Services.Commands
{
	public class GetPreviewImage : BaseCommand, ICommand
	{
		private readonly Guid? _clipGuid;
		private readonly bool _logOnly;

		public GetPreviewImage(object clipGuid, object logOnly)
		{
            _clipGuid = (clipGuid is NullParameter) ? null : (Guid?)clipGuid;
			_logOnly = (logOnly == null || logOnly is NullParameter) ? false : (bool)logOnly;
		}
		
		#region ICommand Members

		public void Execute(HttpRequest request, HttpResponse response)
		{
			try
			{
                //Throw an exception if we don't have a valid Guid
                if (!_clipGuid.HasValue)
                    throw new ArgumentException("Invalid or missing Clip Guid.");

                var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);
				var clip = clipLgc.GetClip(_clipGuid.Value);
                var assLgc = (AssetLogic)LogicFactory.GetLogic(LogicType.Asset);
                var url = assLgc.GetThumbnailUrl(clip, ThumbnailType.Player);

				//NOTE: This logic is a bit trivial. Sucess if valid clip, otherwise fail.
				if (_logOnly)
				{
					var success = (clip == null) ? "0" : "1";
					response.Write("success=" + success);
					return;
				}
				//Redirect to the specified image if we're not only logging...
				response.Redirect(url, false);

				//NOTE: this code is commented out because IQMedia doesn't need it... (but we may want it again some day...)
                //We're already delivered the response to the user but lets do some back-end work in the thread
				//At this point, this is essentially Async...
				/*bool logImpressions;
				Boolean.TryParse(ConfigurationManager.AppSettings["LogClipImpressions"], out logImpressions);
				if (logImpressions && clip != null)
				{
					var trackLgc = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
					//NOTE: UserGuid and IP Address are useless here as its only incrementing the viewsummary
                    trackLgc.InsertImpression(clip.Guid, Authentication.CurrentUser.Guid, HttpContext.Current.Request.UserHostAddress);
				}*/
			}
			catch (Exception ex)
			{
				Logger.Error("GetPreviewImage()", ex);
			}
		}

		#endregion
	}
}