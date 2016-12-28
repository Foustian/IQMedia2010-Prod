using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using IQMediaGroup.Core.Enumeration;
using System.Xml;
using IQMediaGroup.Controller.Factory;
using System.Threading;
using System.Data;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using PMGSearch;
using System.Xml.Linq;
using System.Data.SqlTypes;


namespace IQMediaGroup.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IClipController
    /// </summary>
    public class SocialMediaController : ISocialMediaController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly ISocialMediaModel _ISocialMediaModel;

        public SocialMediaController()
        {
            _ISocialMediaModel = _ModelFactory.CreateObject<ISocialMediaModel>();
        }

        public SocialMedia GetSocialMediaFilterData()
        {
            try
            {
                DataSet _DataSet;
                _DataSet = _ISocialMediaModel.GetSocialMediaFilterData();
                SocialMedia _SocialMedia = new SocialMedia();
                if (_DataSet.Tables.Count > 0)
                {
                    _SocialMedia.ListOFSourceCategory = FillSourceCategory(_DataSet.Tables[0]);

                    if (_DataSet.Tables.Count > 1)
                    {
                        _SocialMedia.ListOFSourceType = FillSourceType(_DataSet.Tables[1]);
                    }
                }

                return _SocialMedia;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        public string InsertArchiveSM(ArchiveSM p_ArchiveSM)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISocialMediaModel.InsertArchiveSM(p_ArchiveSM);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMBySearch(p_ClientGUID, p_SearchTermTitle, p_SearchTermDesc, p_SearchTermKeyword, p_SearchTermCC, p_FromDate, p_ToDate, p_Category1GUID, p_Category2GUID, p_Category3GUID, p_Category4GUID, p_CategoryOperator1, p_CategoryOperator2, p_CategoryOperator3, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, out p_TotalRecordsCount);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateArchiveSM(ArchiveSM p_ArchiveSM)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ISocialMediaModel.UpdateArchiveSM(p_ArchiveSM);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteArchiveSM(string p_DeleteArchiveSM)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ISocialMediaModel.DeleteArchiveSM(p_DeleteArchiveSM);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateIQCoreSMStatus(string p_ArticleID)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ISocialMediaModel.UpdateIQCoreSMStatus(p_ArticleID);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetArticlePathByArticleID(string ArticleID)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ISocialMediaModel.GetArticlePathByArticleID(ArticleID);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMByArchiveSMKey(int p_ArchiveSMKey)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMByArchiveSMKey(p_ArchiveSMKey);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SocialMedia.SM_SourceCategory> FillSourceCategory(DataTable _DataTable)
        {
            try
            {
                List<SocialMedia.SM_SourceCategory> _ListOFSM_SourceCategory = new List<SocialMedia.SM_SourceCategory>();

                if (_DataTable != null && _DataTable.Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        SocialMedia.SM_SourceCategory _SM_SourceCategory = new SocialMedia.SM_SourceCategory();
                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _SM_SourceCategory.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("Lable") && !_DataRow["Lable"].Equals(DBNull.Value))
                        {
                            _SM_SourceCategory.Lable = Convert.ToString(_DataRow["Lable"]);
                        }

                        if (_DataTable.Columns.Contains("Value") && !_DataRow["Value"].Equals(DBNull.Value))
                        {
                            _SM_SourceCategory.Value = Convert.ToString(_DataRow["Value"]);
                        }

                        if (_DataTable.Columns.Contains("Order_Number") && !_DataRow["Order_Number"].Equals(DBNull.Value))
                        {
                            _SM_SourceCategory.Order = Convert.ToInt32(_DataRow["Order_Number"]);
                        }

                        _ListOFSM_SourceCategory.Add(_SM_SourceCategory);
                    }
                }

                return _ListOFSM_SourceCategory;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SocialMedia.SM_SourceType> FillSourceType(DataTable _DataTable)
        {
            try
            {
                List<SocialMedia.SM_SourceType> _ListOFSM_SourceType = new List<SocialMedia.SM_SourceType>();

                if (_DataTable != null && _DataTable.Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        SocialMedia.SM_SourceType _SM_SourceType = new SocialMedia.SM_SourceType();
                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _SM_SourceType.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("Lable") && !_DataRow["Lable"].Equals(DBNull.Value))
                        {
                            _SM_SourceType.Lable = Convert.ToString(_DataRow["Lable"]);
                        }

                        if (_DataTable.Columns.Contains("Value") && !_DataRow["Value"].Equals(DBNull.Value))
                        {
                            _SM_SourceType.Value = Convert.ToString(_DataRow["Value"]);
                        }

                        if (_DataTable.Columns.Contains("Order_Number") && !_DataRow["Order_Number"].Equals(DBNull.Value))
                        {
                            _SM_SourceType.Order = Convert.ToInt32(_DataRow["Order_Number"]);
                        }

                        _ListOFSM_SourceType.Add(_SM_SourceType);
                    }
                }

                return _ListOFSM_SourceType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ArchiveSM> FillArchiveSMInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveSM> _ListOfArchiveSM = new List<ArchiveSM>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveSM _ArchiveSM = new ArchiveSM();

                        if (_DataTable.Columns.Contains("ArchiveSMKey") && !_DataRow["ArchiveSMKey"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.ArchiveSMKey = Convert.ToInt32(_DataRow["ArchiveSMKey"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("LastName") && !_DataRow["LastName"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.LastName = Convert.ToString(_DataRow["LastName"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerGuid") && !_DataRow["CustomerGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CustomerGuid = new Guid(Convert.ToString(_DataRow["CustomerGuid"]));
                        }

                        if (_DataTable.Columns.Contains("ClientGuid") && !_DataRow["ClientGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.ClientGuid = new Guid(Convert.ToString(_DataRow["ClientGuid"]));
                        }

                        if (_DataTable.Columns.Contains("CategoryGuid") && !_DataRow["CategoryGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CategoryGuid = new Guid(Convert.ToString(_DataRow["CategoryGuid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1Guid") && !_DataRow["SubCategory1Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.SubCategory1Guid = new Guid(Convert.ToString(_DataRow["SubCategory1Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2Guid") && !_DataRow["SubCategory2Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.SubCategory2Guid = new Guid(Convert.ToString(_DataRow["SubCategory2Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3Guid") && !_DataRow["SubCategory3Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.SubCategory3Guid = new Guid(Convert.ToString(_DataRow["SubCategory3Guid"]));
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.ArticleID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleContent") && !_DataRow["ArticleContent"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Content = Convert.ToString(_DataRow["ArticleContent"]);
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Url = Convert.ToString(_DataRow["Url"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.CategoryNames = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("Total") && !_DataRow["Total"].Equals(DBNull.Value))
                        {
                            _ArchiveSM.Total = Convert.ToInt32(_DataRow["Total"]);
                        }





                        _ListOfArchiveSM.Add(_ArchiveSM);
                    }
                }

                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public SearchSMResult GetSocialMediaGridData(SearchSMRequest searchSMRequest, SearchEngine searchEngine)
        {
            SearchSMResult searchSMResult = null;
            try
            {

                Logger.Info("Bind Grid for Social Media Request Starts");

                Logger.Info("PMG Request Start");

                searchSMResult = searchEngine.SearchSocialMedia(searchSMRequest);

                Logger.Info("PMG Request End");
                Logger.Info("Bind Grid for Social Media Request End");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return searchSMResult;

        }

        public string GetSocialMediaChartData(SearchSMRequest searchSMRequest, SearchEngine searchEngine, HttpContext currentContext, out Boolean isError)
        {
            string chartString = null;
            try
            {

                Logger.Info("Bind Grid for Social Media Request Starts");
                Logger.Info("PMG Request Start");
                isError = false;
                string smResult = searchEngine.SearchSocialMediaChart(searchSMRequest, out isError);
                String searchResponse = string.Empty;

                if (isError)
                {

                    XmlDocument _XmlDocument = new XmlDocument();


                    _XmlDocument.LoadXml(smResult);

                    XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName("response");

                    if (_XmlNodeList.Count > 0)
                    {
                        XmlAttributeCollection _XmlAttributeCollection = _XmlNodeList[0].Attributes;
                        foreach (XmlAttribute item in _XmlAttributeCollection)
                        {
                            if (item.Name == "status")
                            {
                                searchResponse = _XmlDocument.InnerXml;
                                searchResponse = searchResponse.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");
                            }
                        }
                    }
                    else
                    {
                        searchResponse = null;
                    }
                }
                else
                {
                    searchResponse = null;
                }

                ISearchLogController _ISearchLogController = _ControllerFactory.CreateObject<ISearchLogController>();
                _ISearchLogController.InsertSearchLog(currentContext, SearchType.IQPremium_SM.ToString(), searchSMRequest.SearchTerm, searchSMRequest.PageNumber, searchSMRequest.PageSize, 0, searchSMRequest.StartDate, searchSMRequest.EndDate, searchResponse, searchEngine.Url.ToString());

                chartString = GetJsonForChart(smResult, searchSMRequest, isError);

                Logger.Info("PMG Request End");
                Logger.Info("Bind Grid for Social Media Request End");

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return chartString;

        }

        private string GetJsonForChart(string chartString, SearchSMRequest searchSMRequest, Boolean isError)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject jsonData = new Newtonsoft.Json.Linq.JObject();
                if (!isError)
                {
                    jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(chartString);
                }


                FCZoomChart zoomChartNews = new FCZoomChart();

                zoomChartNews.Dataset = new List<Dataset>();



                zoomChartNews.Categories = new List<Category>();
                Category category = new Category();
                category.Category1 = new List<Category2>();
                zoomChartNews.Categories.Add(category);



                if (!isError && searchSMRequest.SourceType != null && (searchSMRequest.SourceType.Count > 0))
                {
                    foreach (String srcType in searchSMRequest.SourceType)
                    {
                        string totalcount = string.Empty;
                        try
                        {
                            totalcount = Convert.ToString(jsonData["facet_counts"]["facet_ranges"][srcType.Replace("\"", "")]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);
                        }
                        catch (Exception)
                        {

                        }

                        if (!string.IsNullOrWhiteSpace(totalcount))
                        {
                            string[] facetData = totalcount.Split(',');
                            category.Category1 = new List<Category2>(10);
                            string catName = srcType;// _viewstateInformation.listPublicationCategory.Where(s => s.ID.Equals(pCategory.ID)).First().Name;

                            Dataset datasetValue = new Dataset();

                            datasetValue.Seriesname = catName.Replace("\"", string.Empty);
                            datasetValue.Data = new List<Datum>();

                            zoomChartNews.Dataset.Add(datasetValue);

                            for (int i = 0; i < facetData.Length; i = i + 2)
                            {
                                datasetValue.Data.Add(new Datum() { Value = Convert.ToString(facetData[i + 1].Trim().Replace("\"", string.Empty)) });
                                category.Category1.Add(new Category2() { Label = Convert.ToDateTime(facetData[i].Trim().Replace("\"", string.Empty)).ToUniversalTime().ToString("MM/dd/yyyy hh:mm tt") });
                            }
                        }
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
                    exportHandler = "fcBatchExporterSM",
                    allowpinmode = "0",
                    drawAnchors = "0",
                    bgColor = "FFFFFF",
                    showBorder = "0",
                    yaxisname = "Articles",
                    chartRightMargin = ConfigurationManager.AppSettings[CommonConstants.ConfigChartRightMargin]

                };

                string jsonForChart = CommonFunctions.SearializeJson(zoomChartNews);
                return jsonForChart;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string GetEmailContent(List<ArchiveSM> lstArchiveSM)
        {
            try
            {
                StringBuilder emailContent = new StringBuilder();
                emailContent.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                emailContent.Append("<tr>");
                emailContent.Append("<th style=\"width:250px;\" align=\"center\">Title</th>");
                emailContent.Append("<th>Article URL</th>");
                emailContent.Append("</tr>");

                foreach (ArchiveSM archiveSM in lstArchiveSM)
                {

                    emailContent.AppendFormat("<tr><td style=\"width:250px;\" align=\"center\">{0}</td>", HttpContext.Current.Server.HtmlEncode(archiveSM.Title));
                    //string[] fragmenturl = archiveSM.Url.Split('?');
                    //if (fragmenturl.Length > 1)
                    //{
                    emailContent.AppendFormat("<td ><a href=\"{0}\">{0}</a></td></tr>", HttpContext.Current.Server.HtmlEncode(archiveSM.Url));
                    //}
                    //else
                    //{
                    //    emailContent.AppendFormat("<td ><a href=\"{0}\">{0}</a></td></tr>", fragmenturl[0]);
                    //}
                }
                emailContent.Append("</table>");
                return emailContent.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMReportGroupByCategory(p_ClientGUID, p_Date);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveSM> GetArchiveSMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveSM> _ListOfArchiveSM = null;

                _DataSet = _ISocialMediaModel.GetArchiveSMByCategoryGuidAndDate(p_ClientGUID, p_SortField, p_IsAscending, p_Date, p_CategoryGuid);
                _ListOfArchiveSM = FillArchiveSMInformation(_DataSet);
                return _ListOfArchiveSM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public XDocument GenerateListToXML(List<ArchiveSM> _ListOfArchiveSM)
        {
            XDocument xmlDocument = new XDocument(new XElement("list",
                       from _ArchiveClip in _ListOfArchiveSM
                       select new XElement("Element",
                           string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Title)) ? null :
                       new XAttribute("Title", _ArchiveClip.Title),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CustomerGuid)) ? null :
                       new XAttribute("CustomerGuid", _ArchiveClip.CustomerGuid),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ClientGuid)) ? null :
                       new XAttribute("ClientGuid", _ArchiveClip.ClientGuid),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CategoryGuid)) ? null :
                       new XAttribute("CategoryGuid", _ArchiveClip.CategoryGuid),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.SubCategory1Guid)) ? null :
                       new XAttribute("SubCategory1Guid", _ArchiveClip.SubCategory1Guid),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.SubCategory2Guid)) ? null :
                       new XAttribute("SubCategory2Guid", _ArchiveClip.SubCategory2Guid),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.SubCategory3Guid)) ? null :
                       new XAttribute("SubCategory3Guid", _ArchiveClip.SubCategory3Guid),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.ArticleID)) ? null :
                       new XAttribute("ArticleID", _ArchiveClip.ArticleID),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Content)) ? null :
                       new XAttribute("Content", _ArchiveClip.Content),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Url)) ? null :
                       new XAttribute("ArticleUrl", _ArchiveClip.Url),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Harvest_Time)) ? null :
                       new XAttribute("Harvest_Time", _ArchiveClip.Harvest_Time),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.FeedClass)) ? null :
                       new XAttribute("FeedClass", _ArchiveClip.FeedClass),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Publication)) ? null :
                       new XAttribute("CompeteURL", _ArchiveClip.Publication),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.homeLink)) ? null :
                       new XAttribute("homeLink", _ArchiveClip.homeLink)
                           )));
            return xmlDocument;
        }

        public string InsertArchiveSMByList(SqlXml p_SqlXml, out int p_Status, out int p_RecordsInserted)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISocialMediaModel.InsertArchiveSMByList(p_SqlXml, out p_Status, out p_RecordsInserted);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}