using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;
using PMGSearch;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    public class ArchiveNMController : IArchiveNMController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveNMModel _IArchiveNMModel;

        public ArchiveNMController()
        {
            _IArchiveNMModel = _ModelFactory.CreateObject<IArchiveNMModel>();
        }

        public string InsertArchiveNM(ArchiveNM p_ArchiveClips)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveNMModel.InsertArchiveNM(p_ArchiveClips);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateArchiveNM(ArchiveNM p_ArchiveNM)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IArchiveNMModel.UpdateArchiveNM(p_ArchiveNM);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateIQCoreNMStatus(string p_ArticleID)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IArchiveNMModel.UpdateIQCoreNMStatus(p_ArticleID);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteArchiveNM(string p_DeleteArchiveNM)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveNMModel.DeleteArchiveNM(p_DeleteArchiveNM);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNM> GetArchiveNMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMBySearch(p_ClientGUID, p_SearchTermTitle, p_SearchTermDesc, p_SearchTermKeyword, p_SearchTermCC, p_FromDate, p_ToDate, p_Category1GUID, p_Category2GUID, p_Category3GUID, p_Category4GUID, p_CategoryOperator1, p_CategoryOperator2, p_CategoryOperator3, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, out p_TotalRecordsCount);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ArchiveNM> FillArchiveNMInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveNM> _ListOfArchiveNM = new List<ArchiveNM>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveNM _ArchiveNM = new ArchiveNM();

                        if (_DataTable.Columns.Contains("ArchiveNMKey") && !_DataRow["ArchiveNMKey"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.ArchiveNMKey = Convert.ToInt32(_DataRow["ArchiveNMKey"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("FirstName") && !_DataRow["FirstName"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.FirstName = Convert.ToString(_DataRow["FirstName"]);
                        }

                        if (_DataTable.Columns.Contains("LastName") && !_DataRow["LastName"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.LastName = Convert.ToString(_DataRow["LastName"]);
                        }

                        if (_DataTable.Columns.Contains("CustomerGuid") && !_DataRow["CustomerGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CustomerGuid = new Guid(Convert.ToString(_DataRow["CustomerGuid"]));
                        }

                        if (_DataTable.Columns.Contains("ClientGuid") && !_DataRow["ClientGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.ClientGuid = new Guid(Convert.ToString(_DataRow["ClientGuid"]));
                        }

                        if (_DataTable.Columns.Contains("CategoryGuid") && !_DataRow["CategoryGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CategoryGuid = new Guid(Convert.ToString(_DataRow["CategoryGuid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1Guid") && !_DataRow["SubCategory1Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.SubCategory1Guid = new Guid(Convert.ToString(_DataRow["SubCategory1Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2Guid") && !_DataRow["SubCategory2Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.SubCategory2Guid = new Guid(Convert.ToString(_DataRow["SubCategory2Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3Guid") && !_DataRow["SubCategory3Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.SubCategory3Guid = new Guid(Convert.ToString(_DataRow["SubCategory3Guid"]));
                        }

                        if (_DataTable.Columns.Contains("ArticleID") && !_DataRow["ArticleID"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.ArticleID = Convert.ToString(_DataRow["ArticleID"]);
                        }

                        if (_DataTable.Columns.Contains("ArticleContent") && !_DataRow["ArticleContent"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Content = Convert.ToString(_DataRow["ArticleContent"]);
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Url = Convert.ToString(_DataRow["Url"]);
                        }


                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.CategoryNames = Convert.ToString(_DataRow["CategoryName"]);
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("Total") && !_DataRow["Total"].Equals(DBNull.Value))
                        {
                            _ArchiveNM.Total = Convert.ToInt32(_DataRow["Total"]);
                        }



                        _ListOfArchiveNM.Add(_ArchiveNM);
                    }
                }

                return _ListOfArchiveNM;

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

                _Result = _IArchiveNMModel.GetArticlePathByArticleID(ArticleID);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNM> GetArchiveNMByArchiveNMKey(int p_ArchiveNMKey)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMByArchiveNMKey(p_ArchiveNMKey);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetChartData(string newsChartData, String exporterName, string yAxisName, List<NB_PublicationCategory> lstNBPublicationCategory, List<int> publicationCategory)
        {
            try
            {
                Newtonsoft.Json.Linq.JObject jsonData = (Newtonsoft.Json.Linq.JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(newsChartData);
                FCZoomChart zoomChartNews = new FCZoomChart();

                zoomChartNews.Dataset = new List<Dataset>();

                zoomChartNews.Categories = new List<Category>();
                Category category = new Category();
                category.Category1 = new List<Category2>();
                zoomChartNews.Categories.Add(category);

                if (lstNBPublicationCategory != null && lstNBPublicationCategory != null)
                {

                    foreach (NB_PublicationCategory pCategory in lstNBPublicationCategory)
                    {
                        if (publicationCategory != null && (publicationCategory.Count <= 0 || publicationCategory.Contains(Convert.ToInt32(pCategory.ID))))
                        {
                            string totalcount = Convert.ToString(jsonData["facet_counts"]["facet_ranges"][pCategory.ID.ToString()]["counts"]).Replace("\r\n", string.Empty).Replace("[", string.Empty).Replace("]", string.Empty);

                            string[] facetData = totalcount.Split(',');
                            category.Category1 = new List<Category2>(10);

                            string catName = lstNBPublicationCategory.Where(s => s.ID.Equals(pCategory.ID)).First().Name;

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
                    exportHandler = exporterName,//"fcBatchExporterNews",
                    allowpinmode = "0",
                    drawAnchors = "0",
                    bgColor = "FFFFFF",
                    showBorder = "0",
                    yaxisname = yAxisName,//"Article",
                    chartRightMargin = ConfigurationManager.AppSettings[CommonConstants.ConfigChartRightMargin]

                };

                string jsonForChart = CommonFunctions.SearializeJson(zoomChartNews);

                return jsonForChart;
            }
            catch (Exception exception)
            {

                throw exception;
            }
        }

        public string GetEmailContent(List<ArchiveNM> lstArchiveNM)
        {
            try
            {
                StringBuilder emailContent = new StringBuilder();
                emailContent.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                emailContent.Append("<tr>");
                emailContent.Append("<th style=\"width:150px;\" align=\"center\">Title</th>");
                emailContent.Append("<th>URL</th>");
                emailContent.Append("</tr>");
                foreach (ArchiveNM archiveNM in lstArchiveNM)
                {
                    emailContent.AppendFormat("<tr><td style=\"width:150px;\" align=\"center\">{0}</td>", HttpContext.Current.Server.HtmlEncode(archiveNM.Title));

                    //string[] fragmenturl = archiveNM.Url.Split('?');

                    //Uri uri = new Uri(archiveNM.Url);
                    //if (fragmenturl.Length > 1)
                    //{
                    emailContent.AppendFormat("<td ><a href=\"{0}\">{0}</a></td></tr>", HttpContext.Current.Server.HtmlEncode(archiveNM.Url));
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

        public List<ArchiveNM> GetArchiveNMReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMReportGroupByCategory(p_ClientGUID, p_Date);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveNM> GetArchiveNMByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveNM> _ListOfArchiveNM = null;

                _DataSet = _IArchiveNMModel.GetArchiveNMByCategoryGuidAndDate(p_ClientGUID, p_SortField, p_IsAscending, p_Date, p_CategoryGuid);
                _ListOfArchiveNM = FillArchiveNMInformation(_DataSet);
                return _ListOfArchiveNM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public XDocument GenerateListToXML(List<ArchiveNM> _ListOfArchiveNM)
        {
            XDocument xmlDocument = new XDocument(new XElement("list",
                       from _ArchiveClip in _ListOfArchiveNM
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
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.CompeteUrl)) ? null :
                       new XAttribute("CompeteURL", _ArchiveClip.CompeteUrl),
                       string.IsNullOrEmpty(Convert.ToString(_ArchiveClip.Publication)) ? null :
                       new XAttribute("Publication", _ArchiveClip.Publication)
                           )));
            return xmlDocument;
        }

        public string InsertArchiveNMByList(SqlXml p_SqlXml, out int p_Status, out int p_RecordsInserted)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IArchiveNMModel.InsertArchiveNMByList(p_SqlXml, out p_Status, out p_RecordsInserted);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
