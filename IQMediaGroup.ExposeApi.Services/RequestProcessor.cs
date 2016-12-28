using System; 
using System.Web;
using IQMediaGroup.ExposeApi.Services.Config;
using IQMediaGroup.ExposeApi.Services.Commands;
using IQMediaGroup.Common.Util;
using System.Collections.Generic;
using System.Linq;

namespace IQMediaGroup.ExposeApi.Services
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
                System.Web.HttpContext.Current.Items["USeqID"] = CommonFunctions.GetRandomInt() + " - " + context.Request.Headers["X-ClientIP"];

                Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - Started: " + context.Request.Url.PathAndQuery);

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

                var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => urlAbsolutePath.ToLower().Equals(map.Url.ToLower()) || urlAbsolutePath.ToLower().Equals(map.Url.ToLower() + "/"));
                var command = CommandFactory.Create(urlMap, context.Request.Params);

                List<string> corsSvcList = new List<string> { "getarchiveclip" };

                if (corsSvcList.Any(urlPath.ToLower().Contains))
                {
                    Log4NetLogger.Debug("..method..: " + context.Request.HttpMethod);

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
                                if (command != null)
                                    command.Execute(context.Request, context.Response);
                            }

                            break;
                        case "POST":

                            SetAllowCrossSiteRequestHeaders(context);

                            SetAllowCrossSiteRequestCredentials(context);

                            if (SetAllowCrossSiteRequestOrigin(context))
                            {
                                if (command != null)
                                    command.Execute(context.Request, context.Response);
                            }

                            break;
                        default:
                            context.Response.Headers.Add("Allow", "OPTIONS, GET, POST");
                            context.Response.StatusCode = 405;
                            return;
                    }
                }
                else
                {
                    if (command != null)
                        command.Execute(context.Request, context.Response);
                }
            }
            catch (Exception ex)
            {
                /* Exception Handling here */
                Log4NetLogger.Error(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - error :" + ex.ToString());
            }

            //Process Benchmarking Information
            _end = DateTime.Now;
            var excTime = _end.Subtract(_start);

            Log4NetLogger.Debug(IQMediaGroup.ExposeApi.Services.Util.CommonFunctions.GetUniqueSeqIDForLog() + " - " + String.Format("Execution Time: {0}ms for {1}", excTime.Milliseconds, context.Request.Url.PathAndQuery));

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

                List<string> host = new List<string> { "www.iqmediacorp.com", "v4.iqmediacorp.com", "qav4.iqmediacorp.com", "qa.iqmediacorp.com", "iqmediacorp.com", "v5.iqmediacorp.com", "v5svc.iqmediacorp.com", "qav5.iqmediacorp.com", "qav5svc.iqmediacorp.com" };

                Log4NetLogger.Debug("Request url: " + context.Request.Url.OriginalString + " origin: " + origin);

                if (host.Exists(h => string.Compare(url.Host, h, true) == 0))
                {
                    context.Response.AppendHeader("Access-Control-Allow-Origin", origin);

                    return true;
                }
                else
                {
                    Log4NetLogger.Debug("AllowCrossSite fails: " + context.Request.Url.OriginalString + " origin: " + origin);
                }
            }

            return false;
        }

        private void SetAllowCrossSiteRequestHeaders(HttpContext context)
        {
            //We allow only GET method
            string requestMethod = context.Request.Headers["Access-Control-Request-Method"];
            if (!String.IsNullOrEmpty(requestMethod) && requestMethod.ToUpper() == "GET")
                context.Response.AppendHeader("Access-Control-Allow-Methods", "GET");

            //We allow any custom headers
            string requestHeaders = context.Request.Headers["Access-Control-Request-Headers"];
            if (!String.IsNullOrEmpty(requestHeaders))
                context.Response.AppendHeader("Access-Control-Allow-Headers", requestHeaders);

        }
    }
}