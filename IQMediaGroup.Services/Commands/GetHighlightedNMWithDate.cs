using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.Logic;
using IQMediaGroup.Domain;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetHighlightedNMWithDate : ICommand
    {
        public Int64? _ID { get; private set; }
        public String _Format { get; private set; }
        public Int64? _ArticleID { get; private set; }
        public String _SearchTerm { get; private set; }
        public int? _FragSize { get; private set; }
        public DateTime? _Date { get; private set; }


        public GetHighlightedNMWithDate(object ID, object Format, object ArticleID, object SearchTerm, object FragSize,object Date)
        {
            _ID = (ID is NullParameter) ? null : (Int64?)ID;
            _Format = (Format is NullParameter) ? null : (String)Format;
            _ArticleID = (ArticleID is NullParameter) ? null : (Int64?)ArticleID;
            _SearchTerm = (SearchTerm is NullParameter) ? null : (String)SearchTerm;
            _FragSize = (FragSize is NullParameter) ? null : (int?)FragSize;
            _Date = (Date is NullParameter) ? null : (DateTime?)Date;         
        }


        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string jsonResult = string.Empty;
            CLogger.Debug("Get Highlighted News Service Started");
            var highlightedNewsOutput = new HighlightedNewsOutput();

            try
            {

                if (_ID == null && (_ArticleID == null || string.IsNullOrWhiteSpace(_SearchTerm)))
                {
                    //throw new ArgumentException("Invalid or missing ID");
                    throw new ArgumentException("Invalid or missing input");
                }


                CLogger.Debug("{\"IQAgent_NMResultsID\":\"" + _ID + "}");
                CLogger.Debug("{\"ArticleID\":\"" + _ArticleID + "\"SearchTerm\":\"" + _SearchTerm + "}");

                var highlightedNewsLogic = (NewsLogic)LogicFactory.GetLogic(LogicType.NM);
                var hasResult = false;

                if (_ID != null)
                {
                    var iQAgentSearchTerm = (IQAgentSearchTerm)highlightedNewsLogic.GetSearchTermByIQAgent_NMResultsID(_ID);

                    if (iQAgentSearchTerm == null)
                    {
                        throw new CustomException("Invalid ID, does not exist or inactive");
                    }
                    else if (string.IsNullOrWhiteSpace(iQAgentSearchTerm.SearchTerm))
                    {
                        throw new CustomException("Search term is empty for IQAgent News Result ID : " + _ID);
                    }
                    else
                    {
                        string _PmgUrl = BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.MO.ToString(), _Date);
                        List<string> listofHighlights = highlightedNewsLogic.GetNewsHighlight(iQAgentSearchTerm.SearchTerm, iQAgentSearchTerm.ArticleID, _FragSize, out hasResult,_PmgUrl);

                        highlightedNewsOutput.Highlights = listofHighlights;
                    }
                }
                else
                {
                    string _PmgUrl = BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.MO.ToString(), _Date);
                    List<string> listofHighlights = highlightedNewsLogic.GetNewsHighlight(_SearchTerm, _ArticleID.Value.ToString(), _FragSize, out hasResult, _PmgUrl);

                    highlightedNewsOutput.Highlights = listofHighlights;
                }

                if (!hasResult)
                {
                    highlightedNewsOutput.Status = 1;
                    highlightedNewsOutput.Message = "NoResult";
                }
                else
                {
                    if (highlightedNewsOutput.Highlights.Count > 0)
                    {
                        highlightedNewsOutput.Status = 0;
                        highlightedNewsOutput.Message = "Success";
                    }
                    else
                    {
                        highlightedNewsOutput.Status = 2;
                        highlightedNewsOutput.Message = "NoHighlights";
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                highlightedNewsOutput.Status = -2;
                highlightedNewsOutput.Message = _CustomException.Message;
            }
            catch (ArgumentException ex)
            {
                highlightedNewsOutput.Status = -2;
                highlightedNewsOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {
                highlightedNewsOutput.Status = -1;
                highlightedNewsOutput.Message = ex.Message;
                CLogger.Error(ex.ToString());
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                jsonResult = Serializer.SerializeToXml(highlightedNewsOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                jsonResult = Serializer.Searialize(highlightedNewsOutput);
            }


            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + jsonResult);
            }
            CLogger.Debug("Get Highlighted News Service Ended");
            HttpResponse.Output.Write(jsonResult);

        }
    }
}