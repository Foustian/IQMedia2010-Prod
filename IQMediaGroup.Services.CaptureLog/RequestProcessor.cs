using System;
using System.Web;
using IQMediaGroup.Services.CaptureLog.Config;
using IQMediaGroup.Services.CaptureLog.Commands;
using IQMediaGroup.Logic.CaptureLog;

namespace IQMediaGroup.Services.CaptureLog
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

                ValidationLogic _ValidationLogic = new ValidationLogic();
                bool IsValidate = _ValidationLogic.ValidateCapturesvcInput(context.Request.UserHostAddress);

                if (IsValidate)
                {

                    var urlPath = HttpUtility.UrlDecode(context.Request.Url.PathAndQuery);
                    var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => urlPath.Contains(map.Url));
                    var command = CommandFactory.Create(urlMap, context.Request.Params);

                    if (command != null)
                        command.Execute(context.Request, context.Response);
                }
                else
                {
                    throw new Exception("User is not authenticated.");
                }
            }
            catch (Exception ex)
            {
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