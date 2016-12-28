using System;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using System.Configuration;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetHighlightedTWWithDate : ICommand
    {
        public Int64? _ID { get; private set; }
        public String _Format { get; private set; }
        public String _TweetID { get; private set; }
        public String _SearchTerm { get; private set; }
        public DateTime? _Date { get; private set; }


        public GetHighlightedTWWithDate(object ID, object Format, object TweetID, object SearchTerm,object Date)
        {
            _ID = (ID is NullParameter) ? null : (Int64?)ID;
            _Format = (Format is NullParameter) ? null : (String)Format;
            _TweetID = (TweetID is NullParameter) ? null : (String)TweetID;
            _SearchTerm = (SearchTerm is NullParameter) ? null : (String)SearchTerm;
            _Date = (Date is NullParameter) ? null : (DateTime?)Date;         
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string jsonResult = string.Empty;
            CLogger.Debug("Get Highlighted SM Service Started");

            var highlightedTWOutput = new HighlightedTWOutput();

            try
            {

                if (_ID == null && (_TweetID==null || string.IsNullOrWhiteSpace(_SearchTerm)))
                {
                    //throw new ArgumentException("Invalid or missing ID");
                    throw new ArgumentException("Invalid or missing input");
                }

                CLogger.Debug("{\"TWResultsID\":\"" + _ID + "}");
                CLogger.Debug("{\"TweetID\":\"" + _TweetID + "\"SearchTerm\":\"" + _SearchTerm + "}");

                var highlightedTWLogic = (TWLogic)LogicFactory.GetLogic(LogicType.TW);
                var hasResult = false;

                if (_ID != null)
                {

                    var iQAgentSearchTerm = (IQAgentSearchTerm)highlightedTWLogic.GetSearchTermByIQAgent_TwitterResultsID(_ID.Value);
                    if (iQAgentSearchTerm == null || string.IsNullOrWhiteSpace(iQAgentSearchTerm.SearchTerm))
                    {
                        throw new CustomException("Invalid Request");
                    }
                    else
                    {
                        string _PmgUrl = BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.TW.ToString(), _Date);
                        string highlight = highlightedTWLogic.GetTWHighlight(iQAgentSearchTerm.SearchTerm, iQAgentSearchTerm.ArticleID, -1, out hasResult,_PmgUrl);
                        highlightedTWOutput.Text = highlight;
                    }
                }
                else
                {
                    string _PmgUrl = BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.TW.ToString(), _Date);
                    string highlight = highlightedTWLogic.GetTWHighlight(_SearchTerm, _TweetID, -1, out hasResult, _PmgUrl);
                   highlightedTWOutput.Text = highlight;
                }

                if (!hasResult)
                {
                    highlightedTWOutput.Message = "NoResult";
                    highlightedTWOutput.Status = 1;
                }
                else
                {
                    if (!string.IsNullOrWhiteSpace(highlightedTWOutput.Text))
                    {
                        highlightedTWOutput.Message = "Success";
                        highlightedTWOutput.Status = 0;
                    }
                    else
                    {
                        highlightedTWOutput.Message = "NoHighlights";
                        highlightedTWOutput.Status = 2;
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                highlightedTWOutput.Status = -2;
                highlightedTWOutput.Message = _CustomException.Message;
            }
            catch (ArgumentException ex)
            {
                highlightedTWOutput.Status = -2;
                highlightedTWOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {

                highlightedTWOutput.Status = -1;
                highlightedTWOutput.Message = ex.Message;
                CLogger.Error(ex.ToString());
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                jsonResult = Serializer.SerializeToXml(highlightedTWOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                jsonResult = Serializer.Searialize(highlightedTWOutput);
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug("Final result: " + jsonResult);
            }

            CLogger.Debug("Get Highlighted SM Service Ended");
            HttpResponse.Output.Write(jsonResult);
        }
    }
}