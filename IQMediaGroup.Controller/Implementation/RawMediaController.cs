using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using IQMediaGroup.Controller.App_Code;
using IQMediaGroup.Core.Enumeration;
using System.Data;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Factory;
using System.Threading;
using System.Diagnostics;
using System.Web;
using System.Xml.Serialization;
using System.Runtime.Remoting.Contexts;
using System.Web.Hosting;
using System.Windows.Forms;

namespace IQMediaGroup.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRawMedia
    /// </summary>
    internal class RawMediaController : IRawMediaController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly ISearchRequestModel _IISearchRequestModel;


        public RawMediaController()
        {
            _IISearchRequestModel = _ModelFactory.CreateObject<ISearchRequestModel>();
        }

        public string GetChartData(String responseXML, String exporterName, String yAxisName, List<String> allStationCategoryNum, Dictionary<String, String> dictNumName)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(responseXML);
                FCZoomChart zoomChartNews = new FCZoomChart();

                zoomChartNews.Dataset = new List<Dataset>();



                zoomChartNews.Categories = new List<Category>();
                Category category = new Category();
                category.Category1 = new List<Category2>();
                zoomChartNews.Categories.Add(category);


                foreach (string AffilcategoryID in allStationCategoryNum)
                {
                    string totalcount = Convert.ToString(jsonData["facet_counts"]["facet_ranges"][AffilcategoryID]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);

                    string[] facetData = totalcount.Split(',');
                    category.Category1 = new List<Category2>(10);

                    /*string catName = ((Dictionary<String, String>)from finalDict in
                                                                      (from p in request.AffilForFacet
                                                                       select p.Key)
                                                                  select finalDict)[AffilcategoryID];*/
                    string catName = Convert.ToString(dictNumName.Where(a => a.Key.Equals(AffilcategoryID)).First().Value);// Select(s => s.Value));
                    //Convert.ToString(((Dictionary<String, String>)request.AffilForFacet.Select(s => s.Key)).Where(x => x.Key.Equals(AffilcategoryID)).First().Value;// Select(v => v.Value));

                    Dataset datasetValue = new Dataset();

                    datasetValue.Seriesname = catName;
                    datasetValue.Data = new List<Datum>();

                    zoomChartNews.Dataset.Add(datasetValue);

                    for (int i = 0; i < facetData.Length; i = i + 2)
                    {
                        datasetValue.Data.Add(new Datum() { Value = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty)) });
                        category.Category1.Add(new Category2() { Label = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt") });
                    }
                }


                string chartTitle = string.Empty;
                zoomChartNews.Chart = new Chart
                {
                    numVisibleLabels = ConfigurationManager.AppSettings[CommonConstants.ConfigZoomChartNoOfLables],
                    caption = string.Empty,
                    subcaption = "",
                    exportEnabled = "1",
                    exportAtClient = "1",
                    exportHandler = exporterName,// "fcBatchExporter",
                    allowpinmode = "0",
                    drawAnchors = "0",
                    bgColor = "FFFFFF",
                    showBorder = "0",
                    yaxisname = yAxisName,//"Program",
                    chartRightMargin = ConfigurationManager.AppSettings[CommonConstants.ConfigChartRightMargin]
                };

                /* ChartData chartData = new ChartData();
                 chartData.data = listOfData;
                 chartData.Chart = new Chart { caption = "test Caption", numberprefix = "$", showvalues = "0", xAxisName = "date", yAxisName = "hits" };*/

                string jsonForChart = CommonFunctions.SearializeJson(zoomChartNews);
                return jsonForChart;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public List<IQAgentIFrame> GetIqagentIframeData_ByiQAgentiFrameID(Guid iQAgentiFrameID)
        {
            try
            {
                DataSet _Result;

                _Result = _IISearchRequestModel.GetIqagentIframeData_ByiQAgentiFrameID(iQAgentiFrameID);
                List<IQAgentIFrame> lstIQAgentIFrame = FIllIQAgentIFramelist(_Result);
                return lstIQAgentIFrame;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<IQAgentIFrame> FIllIQAgentIFramelist(DataSet _Result)
        {
            try
            {
                List<IQAgentIFrame> lstIQAgentIFrame = new List<IQAgentIFrame>();


                if (_Result != null && _Result.Tables.Count > 0 && _Result.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in _Result.Tables[0].Rows)
                    {
                        IQAgentIFrame iQAgentIFrame = new IQAgentIFrame();
                        if (_Result.Tables[0].Columns.Contains("RawMediaGuid") && !dr["RawMediaGuid"].Equals(DBNull.Value))
                        {
                            iQAgentIFrame.RawMediaGuid = (Guid)(dr["RawMediaGuid"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("Expiry_Date") && !dr["Expiry_Date"].Equals(DBNull.Value))
                        {
                            iQAgentIFrame.Expiry_Date = Convert.ToDateTime(dr["Expiry_Date"]);
                        }

                        if (_Result.Tables[0].Columns.Contains("SearchTerm") && !dr["SearchTerm"].Equals(DBNull.Value))
                        {
                            iQAgentIFrame.SearchTerm = Convert.ToString(dr["SearchTerm"]);
                        }
                        lstIQAgentIFrame.Add(iQAgentIFrame);
                    }
                }
                return lstIQAgentIFrame;
            }
            catch (Exception)
            {

                throw;
            }
        }

        ///// <summary>
        ///// This method generates DateTime string
        ///// </summary>
        ///// <param name="p_DateMin">Min Date</param>
        ///// <param name="p_DateMax">Max Date</param>
        ///// <param name="p_TimeMin">Min Time</param>
        ///// <param name="p_TimeMax">Max Time</param>
        ///// <param name="IsDateTime">bool value</param>
        ///// <returns>DateTime string</returns>
        //public string GenerateTimeFromDate(string p_DateMin, string p_DateMax, int? p_TimeMin, int? p_TimeMax, bool IsDateTime)
        //{
        //    try
        //    {
        //        string TimeValue = string.Empty;

        //        int? _MaxTime = p_TimeMax;
        //        bool _DayDiff = false;

        //        for (DateTime _Index = Convert.ToDateTime(Convert.ToDateTime(p_DateMin).ToShortDateString()); _Index <= Convert.ToDateTime(Convert.ToDateTime(p_DateMax).ToShortDateString()); )
        //        {
        //            if (IsDateTime == false)
        //            {
        //                if (_Index.ToShortDateString() != Convert.ToDateTime(p_DateMax).ToShortDateString())
        //                {
        //                    p_TimeMax = 23;
        //                    _DayDiff = true;
        //                }
        //                else
        //                {
        //                    p_TimeMax = _MaxTime;

        //                    if (_DayDiff == true)
        //                    {
        //                        p_TimeMin = 0;
        //                    }
        //                }
        //            }

        //            for (int? Time = p_TimeMin; Time <= p_TimeMax; Time++)
        //            {
        //                if (string.IsNullOrEmpty(TimeValue.ToString()))
        //                {
        //                    TimeValue = CommonConstants.DblQuote + TimeValue + _Index.ToShortDateString() + CommonConstants.Space + Time.ToString() + CommonConstants.Colon + CommonConstants.RMZDZT + CommonConstants.DblQuote;
        //                }
        //                else
        //                {
        //                    TimeValue = TimeValue + CommonConstants.Comma + CommonConstants.DblQuote + _Index.ToShortDateString() + CommonConstants.Space + Time.ToString() + CommonConstants.Colon + CommonConstants.RMZDZT + CommonConstants.DblQuote;
        //                }

        //            }

        //            _Index = _Index.AddDays(1);
        //        }

        //        return TimeValue.ToString();
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}

        ///// <summary>
        ///// This method Gets RawMedia Caption 
        ///// </summary>
        ///// <param name="p_CacheKey">CacheKey of RawMedia</param>
        ///// <returns>List of object of Class Caption</returns>
        //public List<Caption> GetRawMediaCaption(string p_CacheKey)
        //{
        //    try
        //    {

        //        List<Caption> _ListOfCaption = new List<Caption>();

        //        KeyValue _KeyComplete = new KeyValue(CommonConstants.RMSearchComplete, false);

        //        for (int _StartHit = 0; _StartHit < Convert.ToInt32(ConfigurationManager.AppSettings[CommonConstants.ConfigNoOfCacheKeyRequest].ToString()); _StartHit++)
        //        {
        //            string _CacheKeyURL = ConfigurationManager.AppSettings[CommonConstants.ConfigRawSearch].ToString();
        //            _CacheKeyURL = _CacheKeyURL + CommonFunctions.AddSBQQueryString(_CacheKeyURL, CommonConstants.RMSearchCacheKeyKey, p_CacheKey, true, true, true);

        //            string _ResponseCachekey = CommonFunctions.GetHttpResponse(_CacheKeyURL);

        //            CommonFunctions.FindKey(_KeyComplete, _ResponseCachekey);

        //            if (CommonFunctions.GetBoolValue(_KeyComplete._KeyValue) == true)
        //            {
        //                string Caption = _ResponseCachekey;

        //                Regex _Regex = new Regex(CommonConstants.RMCaptionCaptionFormat);

        //                MatchCollection _MatchCollection = _Regex.Matches(Caption);

        //                if (_MatchCollection.Count > 0)
        //                {
        //                    int _IndexOfCaption = _ResponseCachekey.IndexOf(CommonConstants.RMCaptionCaptionKey);
        //                    int _IndexOfOffset = _ResponseCachekey.IndexOf(CommonConstants.RMCaptionOffsetKey);

        //                    string _SubString = _ResponseCachekey;
        //                    string _SubStringText = _ResponseCachekey;

        //                    while (_SubStringText.Length > 0)
        //                    {
        //                        if (_IndexOfCaption >= 0 && _IndexOfOffset >= 0 && _IndexOfOffset > _IndexOfCaption)
        //                        {
        //                            Caption _Caption = new Caption();

        //                            _SubString = _SubStringText.Substring(_IndexOfCaption + CommonConstants.RMCaptionCaptionKey.Length, (_IndexOfOffset - 2) - (_IndexOfCaption + CommonConstants.RMCaptionOffsetKey.Length));
        //                            _SubStringText = _SubStringText.Substring(_IndexOfOffset);

        //                            _Caption.CaptionString = _SubString;

        //                            int _IndexOfEndOffset = _SubStringText.IndexOf(CommonConstants.CurlyBracketClose);
        //                            _IndexOfOffset = _SubStringText.IndexOf(CommonConstants.RMCaptionOffsetKey);

        //                            _SubString = _SubStringText.Substring(_IndexOfOffset + CommonConstants.RMCaptionOffsetKey.Length, _IndexOfEndOffset - (_IndexOfOffset + CommonConstants.RMCaptionOffsetKey.Length));
        //                            _Caption.StartTime = Convert.ToInt32(_SubString);

        //                            _SubStringText = _SubStringText.Substring(_IndexOfEndOffset + 1);

        //                            _ListOfCaption.Add(_Caption);

        //                            _IndexOfCaption = _SubStringText.IndexOf(CommonConstants.RMCaptionCaptionKey);
        //                            _IndexOfOffset = _SubStringText.IndexOf(CommonConstants.RMCaptionOffsetKey);
        //                        }
        //                        else
        //                        {
        //                            _SubStringText = string.Empty;
        //                        }
        //                    }
        //                }

        //                break;
        //            }
        //        }

        //        return _ListOfCaption;
        //    }
        //    catch (Exception _Exception)
        //    {
        //        throw _Exception;
        //    }
        //}       

    }
}
