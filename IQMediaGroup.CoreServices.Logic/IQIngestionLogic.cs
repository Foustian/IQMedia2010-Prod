using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.CoreServices.Domain;
using IQMediaGroup.Common.Util;
using System.Xml;
using System.IO;
using System.Xml.Serialization;

namespace IQMediaGroup.CoreServices.Logic
{
    public class IQIngestionLogic : BaseLogic, ILogic
    {

        public OneHourIngestParamOutput GetOneHourIngestParam(string stationid)
        {
            try
            {
                var oneHourIngestParamOutput = new OneHourIngestParamOutput();
                var oneHourIngestParam = Context.GetParamsForOneHourDuration(stationid).FirstOrDefault();
                if (oneHourIngestParam != null)
                {
                    oneHourIngestParamOutput.status = 0;
                    oneHourIngestParamOutput.message = "Record fetched successfully";
                    oneHourIngestParamOutput.oneHourIngestParam = oneHourIngestParam;
                }
                else
                {
                    oneHourIngestParamOutput.status = 2;
                    oneHourIngestParamOutput.message = "No record found";
                }
                return oneHourIngestParamOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching OneHourIngestParam stationid:" + stationid, ex);
                throw ex;
            }
        }

        public FiveMinIngestParamOutput GetFiveMinIngestParam(string stationid)
        {
            try
            {
                var fiveMinIngestParamOutput = new FiveMinIngestParamOutput();
                var oneHourIngestParam = Context.GetParamsForFiveMinDuration(stationid).FirstOrDefault();
                if (oneHourIngestParam != null)
                {
                    fiveMinIngestParamOutput.status = 0;
                    fiveMinIngestParamOutput.message = "Record fetched successfully";
                    fiveMinIngestParamOutput.fiveMinIngestParam = oneHourIngestParam;
                }
                else
                {
                    fiveMinIngestParamOutput.status = 2;
                    fiveMinIngestParamOutput.message = "No record found";
                }
                return fiveMinIngestParamOutput;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while fetching FiveMinIngestParam stationid:" + stationid, ex);
                throw ex;
            }
        }

        public Int64 InsertIQLog_Ingestion(IQLog_IngestionInput iQLog_Ingestion)
        {
            try
            {
                var outParam = Context.InsertIQLog_Ingestion(iQLog_Ingestion.StationID.Trim(), iQLog_Ingestion.IQ_CC_Key.Trim(), iQLog_Ingestion.MediaType.Trim().ToUpper(), DateTime.Now, iQLog_Ingestion.Level.Trim(), iQLog_Ingestion.Logger.Trim(), iQLog_Ingestion.LogMessage.Trim()).FirstOrDefault();
                return (Int64)outParam;
            }
            catch (Exception ex)
            {
                Log4NetLogger.Fatal("An error occured while Inseting Ingestion LOG:" + ex);
                throw ex;
            }

        }


    }
}
