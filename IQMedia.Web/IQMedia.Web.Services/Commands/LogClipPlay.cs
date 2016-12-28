using System;
using System.Configuration;
using System.Web;
using IQMedia.Common.Util;
using IQMedia.Logic;
using IQMedia.Web.Common;

namespace IQMedia.Web.Services.Commands
{
	public class LogClipPlay : BaseCommand, ICommand
	{
		private readonly Guid? _clipGuid;
		private readonly string _referrer;

		public LogClipPlay(object clipGuid, object refferer)
		{
            _clipGuid = (clipGuid is NullParameter) ? null : (Guid?)clipGuid;
			_referrer = (refferer is NullParameter) ? String.Empty : (string)refferer;
		}

		#region ICommand Members

		public void Execute(HttpRequest request, HttpResponse response)
		{
			var clipLgc = (ClipLogic)LogicFactory.GetLogic(LogicType.Clip);

			try
			{
                //Throw an exception if we don't have a valid Guid
                if (!_clipGuid.HasValue)
                    throw new ArgumentException("Invalid or missing Clip Guid.");

				var clip = clipLgc.GetClip(_clipGuid.Value);
				//If we don't have a clip, we can't do anything else so we fail and return.
                if (clip == null)
                    throw new ArgumentNullException(String.Format("The clip '{0}' does not exist.", _clipGuid.Value));

				bool logPlays;
				Boolean.TryParse(ConfigurationManager.AppSettings["LogClipPlays"], out logPlays);
				if (logPlays)
				{
					var trackLgc = (TrackingLogic)LogicFactory.GetLogic(LogicType.Tracking);
                    trackLgc.InsertPlay(clip.Guid, Authentication.CurrentUser.Guid, HttpContext.Current.Request.UserHostAddress, _referrer);
                    //NOTE: We added this here since it used to be in previewImage and that call is (temporarily?) depricated
                    //Even tho Lakshmi doesn't care about impressions, it wouldn't make sense that a clip have more plays than
                    //impressions so we log the impression when we log a play so that way it maintains a 1 to 1 relationship at least
                    trackLgc.InsertImpression(clip.Guid, Authentication.CurrentUser.Guid, HttpContext.Current.Request.UserHostAddress);
				}
                response.Write("success=1");
			}
			catch (Exception ex)
			{
				Logger.Error("LogClipPlay()", ex);
				response.Write("success=0");
			}
		}

		#endregion
	}
}