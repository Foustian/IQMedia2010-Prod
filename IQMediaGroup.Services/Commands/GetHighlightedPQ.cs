using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetHighlightedPQ : ICommand
    {
        public Int64? _ID { get; private set; }
        public String _Format { get; private set; }
        public Int64? _ProQuestID { get; private set; }
        public String _SearchTerm { get; private set; }
        public int? _FragSize { get; private set; }

        public GetHighlightedPQ(object ID, object Format, object ProQuestID, object SearchTerm, object FragSize)
        {
            _ID = (ID is NullParameter) ? null : (Int64?)ID;
            _Format = (Format is NullParameter) ? null : (String)Format;
            _ProQuestID = (ProQuestID is NullParameter) ? null : (Int64?)ProQuestID;
            _SearchTerm = (SearchTerm is NullParameter) ? null : (String)SearchTerm;
            _FragSize = (FragSize is NullParameter) ? null : (int?)FragSize;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string jsonResult = string.Empty;
            CLogger.Debug("Get Highlighted ProQuest Service Started");
            var highlightedPQOutput = new HighlightedPQOutput();

            try
            {
                if (_ID == null && (_ProQuestID == null || string.IsNullOrWhiteSpace(_SearchTerm)))
                {
                    throw new ArgumentException("Invalid or missing input");
                }

                CLogger.Debug("{\"IQAgent_PQResultsID\":\"" + _ID + "}");
                CLogger.Debug("{\"ProQuestID\":\"" + _ProQuestID + "\"SearchTerm\":\"" + _SearchTerm + "}");

                var highlightedPQLogic = (PQLogic)LogicFactory.GetLogic(LogicType.PQ);
                var hasResult = false;

                if (_ID != null)
                {
                    var iQAgentSearchTerm = (IQAgentSearchTerm)highlightedPQLogic.GetSearchTermByIQAgent_PQResultsID(_ID);

                    if (iQAgentSearchTerm == null)
                    {
                        throw new CustomException("Invalid ID, does not exist or inactive");
                    }
                    else if (string.IsNullOrWhiteSpace(iQAgentSearchTerm.SearchTerm))
                    {
                        throw new CustomException("Search term is empty for IQAgent ProQuest Result ID : " + _ID);
                    }
                    else
                    {
                        List<string> listofHighlights = highlightedPQLogic.GetPQHighlight(iQAgentSearchTerm.SearchTerm, iQAgentSearchTerm.ArticleID, _FragSize, out hasResult, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.PQ.ToString(), null, null));

                        highlightedPQOutput.Highlights = listofHighlights;
                    }
                }
                else
                {
                    List<string> listofHighlights = highlightedPQLogic.GetPQHighlight(_SearchTerm, _ProQuestID.Value.ToString(), _FragSize, out hasResult, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.PQ.ToString(), null, null));

                    highlightedPQOutput.Highlights = listofHighlights;
                }

                if (!hasResult)
                {
                    highlightedPQOutput.Status = 1;
                    highlightedPQOutput.Message = "NoResult";
                }
                else
                {
                    if (highlightedPQOutput.Highlights.Count > 0)
                    {
                        highlightedPQOutput.Status = 0;
                        highlightedPQOutput.Message = "Success";
                    }
                    else
                    {
                        highlightedPQOutput.Status = 2;
                        highlightedPQOutput.Message = "NoHighlights";
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                highlightedPQOutput.Status = -2;
                highlightedPQOutput.Message = _CustomException.Message;
            }
            catch (ArgumentException ex)
            {
                highlightedPQOutput.Status = -2;
                highlightedPQOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                highlightedPQOutput.Status = -1;
                highlightedPQOutput.Message = ex.Message;
                CLogger.Error(ex.ToString());
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                jsonResult = Serializer.SerializeToXml(highlightedPQOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                jsonResult = Serializer.Searialize(highlightedPQOutput);
            }


            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + jsonResult);
            }
            CLogger.Debug("Get Highlighted ProQuest Service Ended");
            HttpResponse.Output.Write(jsonResult);
        }
    }
}