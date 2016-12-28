using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using System.Configuration;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class GetHighlightedSM : ICommand
    {
        public Int64? _ID { get; private set; }
        public String _Format { get; private set; }
        public String _ArticleID { get; private set; }
        public String _SearchTerm { get; private set; }
        public int? _FragSize { get; private set; }


        public GetHighlightedSM(object ID, object Format, object ArticleID, object SearchTerm, object FragSize)
        {
            _ID = (ID is NullParameter) ? null : (Int64?)ID;
            _Format = (Format is NullParameter) ? null : (String)Format;
            _ArticleID = (ArticleID is NullParameter) ? null : (String)ArticleID;
            _SearchTerm = (SearchTerm is NullParameter) ? null : (String)SearchTerm;
            _FragSize = (FragSize is NullParameter) ? null : (int?)FragSize;
        }


        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string jsonResult = string.Empty;
            CLogger.Debug("Get Highlighted SM Service Started");
            var highlightedSMOutput = new HighlightedSMOutput();

            try
            {

                if (_ID == null && (string.IsNullOrWhiteSpace(_ArticleID) || string.IsNullOrWhiteSpace(_SearchTerm)))
                {
                    //throw new ArgumentException("Invalid or missing ID");
                    throw new ArgumentException("Invalid or missing input");
                }

                CLogger.Debug("{\"IQAgent_SMResultsID\":\"" + _ID + "}");
                CLogger.Debug("{\"ArticleID\":\"" + _ArticleID + "\"SearchTerm\":\"" + _SearchTerm + "}");

                var highlightedSMLogic = (SMLogic)LogicFactory.GetLogic(LogicType.SM);
                var hasResult = false;

                if (_ID != null)
                {
                    var iQAgentSearchTerm = (IQAgentSearchTerm)highlightedSMLogic.GetSearchTermByIQAgent_SMResultsID(_ID);

                    if (iQAgentSearchTerm == null || string.IsNullOrWhiteSpace(iQAgentSearchTerm.SearchTerm))
                    {
                        throw new CustomException("Invalid Request");
                    }
                    else
                    {
                        List<string> listofHighlights = highlightedSMLogic.GetSMHighlight(iQAgentSearchTerm.SearchTerm, iQAgentSearchTerm.ArticleID, _FragSize, out hasResult, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.MO.ToString(), null, null));
                        highlightedSMOutput.Highlights = listofHighlights;
                    }
                }
                else
                {
                    List<string> listofHighlights = highlightedSMLogic.GetSMHighlight(_SearchTerm, _ArticleID, _FragSize, out hasResult, BaseCommand.GeneratePMGUrl(BaseCommand.PMGUrlType.MO.ToString(), null, null));
                    highlightedSMOutput.Highlights = listofHighlights;
                }

                if (!hasResult)
                {
                    highlightedSMOutput.Message = "NoResult";
                    highlightedSMOutput.Status = 1;         
                }
                else
                {
                    if (highlightedSMOutput.Highlights.Count>0)
                    {
                        highlightedSMOutput.Message = "Success";
                        highlightedSMOutput.Status = 0;                
                    }
                    else
                    {
                        highlightedSMOutput.Message = "NoHighlights";
                        highlightedSMOutput.Status = 2;         
                    }
                }
            }
            catch (CustomException _CustomException)
            {
                highlightedSMOutput.Status = -2;
                highlightedSMOutput.Message = _CustomException.Message;
            }
            catch (ArgumentException ex)
            {
                highlightedSMOutput.Status = -2;
                highlightedSMOutput.Message = ex.Message;
            }
            catch (Exception ex)
            {

                highlightedSMOutput.Status = -1;
                highlightedSMOutput.Message = ex.Message;
                CLogger.Error(ex.ToString());
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;

            if (!string.IsNullOrWhiteSpace(_Format) && _Format.ToLower() == CommonConstants.formatType.xml.ToString().ToLower())
            {
                HttpResponse.ContentType = "application/xml";
                jsonResult = Serializer.SerializeToXml(highlightedSMOutput);
            }
            else
            {
                HttpResponse.ContentType = "application/json";
                jsonResult = Serializer.Searialize(highlightedSMOutput);
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