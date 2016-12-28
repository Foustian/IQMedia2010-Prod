using System;
using System.Web;
using IQMedia.Common.Util;
using IQMedia.Web.Services.Commands;
using IQMedia.Web.Services.Config;

namespace IQMedia.Web.Services
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
                if(!String.IsNullOrEmpty(debug))
                {
                    if (debug.ToLower().Equals("failrespond")) return;
                    if (debug.ToLower().Equals("failgarbage"))
                    {
                        context.Response.Write("qwertyuiopasdfghjklzxcvbnm0123456789");
                        return;
                    }
                }
                
                //Process the Mapping and Execute the Command Class
                var urlPath = context.Request.Url.PathAndQuery;
                var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => urlPath.Contains(map.Url));
                var command = CommandFactory.Create(urlMap, context.Request.Params);

                if (command != null)
                    command.Execute(context.Request, context.Response);
            }
            catch(Exception ex)
            {
                Logger.Error(context.Request.Url.PathAndQuery, ex);
            }

            //Process Benchmarking Information
            _end = DateTime.Now;
            var excTime = _end.Subtract(_start);
            Logger.Debug(String.Format("Execution Time: {0}ms for {1}", excTime.Milliseconds, context.Request.Url.PathAndQuery));
        }

        #endregion
    }
}