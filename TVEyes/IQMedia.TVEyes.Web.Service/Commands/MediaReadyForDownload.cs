using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMedia.TVEyes.Common.Util;
using IQMedia.TVEyes.Logic;

namespace IQMedia.TVEyes.Web.Service.Commands
{
    public class MediaReadyForDownload : ICommand
    {
        public string _result { get; private set; }
        public string _media { get; private set; }
        public string _package { get; private set; }
        public int? _id { get; private set; }

        public MediaReadyForDownload(object ID,object result, object media, object package)
        {
            _id = (ID is NullParameter) ? null : (int?)ID;
            _result = (result is NullParameter) ? null : (String)result;
            _media = (media is NullParameter) ? null : (String)media;
            _package = (package is NullParameter) ? null : (String)package;
        }

        public void Execute(HttpRequest request, HttpResponse response)
        {
            Logger.Debug("MediaReadyForDownload Service Started");
            string OutputResult = string.Empty;
            try
            {
                Logger.Info("id : " + _id);
                Logger.Info("media : " + _media);
                Logger.Info("package : " + _package);
                Logger.Info("result : " + _result);

                if (!_id.HasValue)
                {
                    OutputResult = "ID is missing";
                    throw new ArgumentException(OutputResult);
                }

                MediaReadyForDownloadLogic mediaReadyForDownloadLogic = (MediaReadyForDownloadLogic)LogicFactory.GetLogic(LogicType.MediaReadyForDownload);
                OutputResult = mediaReadyForDownloadLogic.ArchiveTVEyesUpdateDownloadStatus(_id.Value, _result, _media, _package);

            }
            catch (ArgumentException exception)
            {
                OutputResult = exception.Message;
                Logger.Error(exception.Message);
            }
            catch (Exception exception)
            {
                OutputResult = "An error occured, please try again.";
                Logger.Error("Error : " + exception.Message + "\n Inner Exception" + exception.InnerException + "\n stack : " + exception.StackTrace);
            }

            response.ContentEncoding = System.Text.Encoding.UTF8;

            response.ContentType = "application/json";


            Logger.Debug("MediaReadyForDownload Service Ended");
            response.Output.Write(OutputResult);
        }
    }
}