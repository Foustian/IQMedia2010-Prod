using System.Web;
using System;
using IQMedia.Services.SMS.Config;
using IQMedia.Services.SMS.Commands;

namespace IQMedia.Services.SMS
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
                //Process the Mapping and Execute the Command Class
                var urlPath = context.Request.Url.PathAndQuery;
                var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => urlPath.Contains(map.Url));
                var command = CommandFactory.Create(urlMap, context.Request.Params);


                if (command != null)
                    command.Execute(context.Request, context.Response);
               
            }
            catch (Exception ex)
            {
                context.Response.Output.Write(ex.Message + "<br>" + ex.InnerException + "<br>" + ex.StackTrace);
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
