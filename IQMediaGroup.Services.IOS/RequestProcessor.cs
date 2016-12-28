using System;
using System.Web;
using IQMediaGroup.Services.IOS.Web.Config;
using IQMediaGroup.Services.IOS.Web.Commands;
using System.Collections.Generic;
using IQMediaGroup.Common.IOS.Util;

namespace IQMediaGroup.Services.IOS.Web
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

                //Process the Mapping and Execute the Command Class
                var urlPath = context.Request.Url.PathAndQuery;
                var urlAbsolutePath = context.Request.Url.AbsolutePath;

                var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => string.Compare(urlAbsolutePath, map.Url, true) == 0);

                var command = CommandFactory.Create(urlMap, context.Request.Params);

                List<string> corsSvcList = new List<string> { 
                    "/iossvc/ExportMediaClip"                  
                };

                if (command != null && corsSvcList.Exists(svc => string.Compare(svc, urlMap.Url, true) == 0))
                {
                    switch (context.Request.HttpMethod.ToUpper())
                    {
                        //Cross-Origin preflight request
                        case "OPTIONS":

                            SetAllowCrossSiteRequestHeaders(context);

                            SetAllowCrossSiteRequestOrigin(context);

                            SetAllowCrossSiteRequestCredentials(context);

                            break;
                        case "GET":

                            SetAllowCrossSiteRequestHeaders(context);

                            SetAllowCrossSiteRequestCredentials(context);

                            if (SetAllowCrossSiteRequestOrigin(context))
                            {
                                command.Execute(context.Request, context.Response);
                            }

                            break;
                        default:
                            context.Response.Headers.Add("Allow", "OPTIONS, GET");
                            context.Response.StatusCode = 405;
                            return;
                    }
                }
                else if (command != null)
                {
                    command.Execute(context.Request, context.Response);
                }
                else
                {
                    Log4NetLogger.Info("Null command");
                }

            }
            catch (Exception ex)
            {
                context.Response.Output.Write("Error.");
                //context.Response.Output.Write(ex.Message);
                /* Exception Handling here */
            }

            //Process Benchmarking Information
            _end = DateTime.Now;
            var excTime = _end.Subtract(_start);

            /* Logging here */
        }

        #endregion

        private void SetAllowCrossSiteRequestCredentials(HttpContext context)
        {
            context.Response.AppendHeader("Access-Control-Allow-Credentials", "true");
        }

        private bool SetAllowCrossSiteRequestOrigin(HttpContext context)
        {
            string origin = context.Request.Headers["Origin"];

            if (!String.IsNullOrEmpty(origin))
            {
                Uri url = new Uri(origin);

                List<string> host = new List<string> { "www.iqmediacorp.com", "v4.iqmediacorp.com", "qav4.iqmediacorp.com", "qa.iqmediacorp.com", "iqmediacorp.com" };

                if (host.Exists(h => string.Compare(url.Host, h, true) == 0))
                {
                    context.Response.AppendHeader("Access-Control-Allow-Origin", origin);

                    return true;
                }
            }

            return false;
        }

        private void SetAllowCrossSiteRequestHeaders(HttpContext context)
        {
            //Allow GET and POST Methods
            string requestMethod = context.Request.Headers["Access-Control-Request-Method"];

            if (string.Compare(requestMethod, "GET", true) == 0)
            {
                context.Response.AppendHeader("Access-Control-Allow-Methods", requestMethod.ToUpper());
            }

            //Allow any Custom Header
            string requestHeaders = context.Request.Headers["Access-Control-Request-Headers"];

            if (!String.IsNullOrEmpty(requestHeaders))
            {
                context.Response.AppendHeader("Access-Control-Allow-Headers", requestHeaders);
            }
        }
    }
}