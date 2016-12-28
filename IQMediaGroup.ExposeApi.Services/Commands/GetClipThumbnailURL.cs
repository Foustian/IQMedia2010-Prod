using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.ExposeApi.Logic;

namespace IQMediaGroup.ExposeApi.Services.Commands
{
    public class GetClipThumbnailURL : ICommand
    {
        public readonly Guid? _ClipGUID;

        public GetClipThumbnailURL(object ClipID)
        {
            _ClipGUID = (ClipID is NullParameter) ? null : (Guid?)ClipID;
        }

        public void Execute(HttpRequest p_HttpRequest, HttpResponse p_HttpResponse)
        {
            var clipThumbURL = ConfigurationManager.AppSettings["ClipThumbnailUnavailableURL"];

            try
            {
                Log4NetLogger.Info(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetClipThumbnailURL Request Started");

                if (!_ClipGUID.HasValue)
                {
                    throw new ArgumentException("Invalid or missing ClipID.");
                }

                var archiveLogic = (ArchiveLogic)LogicFactory.GetLogic(LogicType.Archive);
                clipThumbURL = archiveLogic.GetClipThumbnailURL(_ClipGUID.Value);

            }            
            catch (Exception ex)
            {
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog(), ex);
            }

            p_HttpResponse.StatusCode = 301;
            p_HttpResponse.AddHeader("Location", clipThumbURL);

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - GetClipThumbnailURL Request Ended");
        }
    }
}