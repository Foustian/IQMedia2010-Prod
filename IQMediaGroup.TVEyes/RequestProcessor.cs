using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using IQMediaGroup.TVEyes.Config;
using IQMediaGroup.TVEyes.Commands;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.TVEyes
{
    public class RequestProcessor : IHttpHandler
    {
        //Benchmarking Values
        private DateTime _start;
        private DateTime _end;

        #region IHttpHandler Members

        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            //Start Benchmark
            _start = DateTime.Now;

            try
            {
                //Some debug processing code to allow the client to test bad responses...
                //var debug = context.Request.QueryString.Get("debug");
                var debug = context.Request.Params.Get("debug");
                if (!String.IsNullOrEmpty(debug))
                {
                    if (debug.ToLower().Equals("failrespond")) return;
                    if (debug.ToLower().Equals("failgarbage"))
                    {
                        context.Response.Write("qwertyuiopasdfghjklzxcvbnm0123456789");
                        return;
                    }
                }

                Log4NetLogger.Debug("Requested Url :" + context.Request.Url);
                Log4NetLogger.Debug("Requested ip:" + context.Request.UserHostAddress);

                //Process the Mapping and Execute the Command Class
                var urlPath = context.Request.Url.PathAndQuery;
                var urlAbsolutePath = context.Request.Url.AbsolutePath;

                var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => urlAbsolutePath.ToLower().Equals(map.Url.ToLower()) || urlAbsolutePath.ToLower().Equals(map.Url.ToLower() + "/"));
                var command = CommandFactory.Create(urlMap, context.Request.Params);

                if (command != null)
                    command.Execute(context.Request, context.Response);
            }
            catch (Exception ex)
            {
                context.Response.Output.Write(ex.Message);
                /* Exception Handling here */
            }

            //Process Benchmarking Information
            _end = DateTime.Now;
            var excTime = _end.Subtract(_start);

            /* Logging here */
        }

        #endregion
    }
}
