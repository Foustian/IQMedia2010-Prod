using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Common.Util;
using System.Configuration;

namespace IQMediaGroup.Logic
{
    public class GetWaterMarkLogic : BaseLogic, ILogic
    {
        public string GetClientWaterMark(System.Guid? ClipID)
        {
            try
            {
                string StrClientWaterMarkImage = string.Empty;
                string _Result = string.Empty;
                
                 StrClientWaterMarkImage  =  Convert.ToString(Context.GetClientPlayerLogoByClipGUID(ClipID.Value).Single());

                 if (!string.IsNullOrEmpty(StrClientWaterMarkImage))
                 {

                     _Result = "{\"Status\":\"0\",\"Message\":\"WaterMark Fetched Successfully!!\",\"Path\":\"" + ConfigurationManager.AppSettings["URLWaterMark"] + StrClientWaterMarkImage + "\"}";
                 }
                 else
                 {
                     _Result = "{\"Status\":\"1\",\"Message\":\"WaterMark Does Not Exists!!\",\"Path\":\"" + string.Empty + "\"}";
                 }
                 Log4NetLogger.Debug(_Result);
                 return _Result;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
