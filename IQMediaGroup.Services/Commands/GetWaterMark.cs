using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using IQMediaGroup.Logic;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetWaterMark : ICommand
    {
        public Guid? _ClipID { get; private set; }

        public GetWaterMark(object ClipID)
        {
            _ClipID = (ClipID is NullParameter) ? null : (Guid?)ClipID;
        }
        public void Execute(HttpRequest request, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            CLogger.Debug("Get WaterMark Request Started");
            try
            {
                if (_ClipID != null)
                {
                    CLogger.Debug("{\"ClipID\":\"" + _ClipID + "\"");
                    GetWaterMarkLogic _GetWaterMarkLogic = (GetWaterMarkLogic)LogicFactory.GetLogic(LogicType.WaterMark);
                    _JSONResult = _GetWaterMarkLogic.GetClientWaterMark(_ClipID);
                    
                }
                else
                {
                    _JSONResult = Serializer.Searialize("{\"Status\":\"1\",\"Message\":\"ClipID is not valid!!\",\"Path\":\"" + string.Empty + "\"}");
                    CLogger.Debug(_JSONResult);
                }

            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
                _JSONResult = Serializer.Searialize("{\"Status\":\"1\",\"Message\":\"An error occurred, please try again!!\",\"Path\":\"" + string.Empty + "\"}");
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("Get WaterMark Request Ended");
            HttpResponse.Output.Write(_JSONResult);
        }
    }
}