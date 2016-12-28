using System;
using System.Web;
using IQMediaGroup.CoreServices.Config;
using IQMediaGroup.CoreServices.Commands;
using IQMediaGroup.Common.Util;

namespace IQMediaGroup.CoreServices
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

                var _ValidationLogic =  (IQMediaGroup.CoreServices.Logic.ValidationLogic)IQMediaGroup.CoreServices.Logic.LogicFactory.GetLogic(Logic.LogicType.Validation);
                bool IsValidate = _ValidationLogic.ValidateRequestIP(context.Request.Headers["X-ClientIP"]);

                if (IsValidate)
                {

                    var urlPath = HttpUtility.UrlDecode(context.Request.Url.AbsolutePath);
                    var urlMap = ConfigSettings.Mappings.UrlMappings.Find(map => urlPath.Equals(map.Url));
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
                try
                {
                   IQMediaGroup.Common.Util.Log4NetLogger.Error(ex.Message);

                    IQMediaGroup.CoreServices.Domain.RecordFileOutput _RecordFileOutput = new IQMediaGroup.CoreServices.Domain.RecordFileOutput();

                    _RecordFileOutput.Status = 1;
                    _RecordFileOutput.Message = ex.Message;

                    context.Response.Output.Write(IQMediaGroup.CoreServices.Serializers.Serializer.SerializeToXml(_RecordFileOutput));
                }
                catch (Exception)
                {                    
                    
                }
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