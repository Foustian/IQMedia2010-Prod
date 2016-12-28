using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.TVEyes.Commands
{
    public class MediaReadyForDownload : ICommand
    {
        public string _result { get; private set; }
        public string _media { get; private set; }
        public string _package { get; private set; }

        public MediaReadyForDownload(object result, object media, object package)
        {
            _result = (result is NullParameter) ? null : (String)result;
            _media = (media is NullParameter) ? null : (String)media;
            _package = (package is NullParameter) ? null : (String)package;
        }

        public void Execute(HttpRequest request, HttpResponse response)
        {
            Log4NetLogger.Debug("MediaReadyForDownload Service Started");
            string OutputResult = "{\"Msg\":\"Success\"}";            

            response.ContentEncoding = System.Text.Encoding.UTF8;

            response.ContentType = "application/json";


            Log4NetLogger.Debug("MediaReadyForDownload Service Ended");
            response.Output.Write(OutputResult);
        }
    }
}