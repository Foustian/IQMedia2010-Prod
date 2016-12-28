using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.Logic;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetHighlightedCC : ICommand
    {
        public Int64? _ID { get; private set; }
        public String _Format { get; private set; }
        public String _IQ_CC_Key { get; private set; }
        public String _SearchTerm { get; private set; }
        public int? _FragOffset { get; private set; }


        public GetHighlightedCC(object ID, object Format, object IQ_CC_Key, object SearchTerm, object FragOffset)
        {
            _ID = (ID is NullParameter) ? null : (Int64?)ID;
            _Format = (Format is NullParameter) ? null : (String)Format;
            _IQ_CC_Key = (IQ_CC_Key is NullParameter) ? null : (String)IQ_CC_Key;
            _SearchTerm = (SearchTerm is NullParameter) ? null : (String)SearchTerm;
            _FragOffset = (FragOffset is NullParameter) ? null : (int?)FragOffset;
        }


        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string jsonResult = string.Empty;
            CLogger.Debug("Get Highlighted CC Started");
            var highlightedCCOutput = new HighlightedCCOutput();

            try
            {

                if (_ID == null && (string.IsNullOrWhiteSpace(_IQ_CC_Key) || string.IsNullOrWhiteSpace(_SearchTerm)))
                {
                    //throw new ArgumentException("Invalid or missing ID");
                    throw new ArgumentException("Invalid or missing input");
                }


                CLogger.Debug("{\"IQAgent_TVResultsID\":\"" + _ID + "}");
                CLogger.Debug("{\"IQ_CC_Key\":\"" + _IQ_CC_Key + "\"SearchTerm\":\"" + _SearchTerm + "}");

                var highlightedCCLogic = (CCLogic)LogicFactory.GetLogic(LogicType.GetHighlightedCC);
                var hasResult=false;

                if (_ID != null)
                {
                    var iQAgentRawMedia = (IQAgentRawMedia)highlightedCCLogic.GetSearchTermByIQAgent_TVResultsID(_ID);

                    if (iQAgentRawMedia == null || string.IsNullOrWhiteSpace(iQAgentRawMedia.SearchTerm))
                    {
                        throw new CustomException("Invalid Request");
                    }
                    else
                    {
                        List<ClosedCaption> listofCC = highlightedCCLogic.GetClosedCaption(iQAgentRawMedia.SearchTerm, iQAgentRawMedia.RL_VideoGUID, string.Empty, _FragOffset, out hasResult, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.TV.ToString(), null, null));

                        highlightedCCOutput.CC = listofCC;
                    }
                }
                else
                {
                    List<ClosedCaption> listofCC = highlightedCCLogic.GetClosedCaption(_SearchTerm, Guid.Empty, _IQ_CC_Key, _FragOffset, out hasResult, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.TV.ToString(), null, null));

                    highlightedCCOutput.CC = listofCC;
                }

                if (!hasResult)
                {
                    highlightedCCOutput.Status = 1;
                    highlightedCCOutput.Message = "NoResult";
                }
                else
                {
                    if (highlightedCCOutput.CC.Count>0)
                    {
                        highlightedCCOutput.Status = 0;
                        highlightedCCOutput.Message = "Success";
                        highlightedCCOutput.CC = highlightedCCOutput.CC.OrderBy(cc => cc.Offset).ToList();
                    }
                    else
                    {
                        highlightedCCOutput.Status = 2;
                        highlightedCCOutput.Message = "NoHighlights";
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                highlightedCCOutput.Status = -2;
                highlightedCCOutput.Message = _CustomException.Message;
            }
            catch (ArgumentException ex)
            {
                highlightedCCOutput.Status = -2;
                highlightedCCOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                highlightedCCOutput.Status = -1;
                highlightedCCOutput.Message = ex.Message;
                CLogger.Error(ex.ToString());
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                jsonResult = Serializer.SerializeToXml(highlightedCCOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                jsonResult = Serializer.Searialize(highlightedCCOutput);
            }


            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + jsonResult);
            }
            CLogger.Debug("Get Highlighted CC Ended");
            HttpResponse.Output.Write(jsonResult);

        }

    }
}