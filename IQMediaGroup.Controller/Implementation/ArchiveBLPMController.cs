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

namespace IQMediaGroup.Controller.Implementation
{
    public class ArchiveBLPMController : IArchiveBLPMController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveBLPMModel _IArchiveBLPMModel;

        public ArchiveBLPMController()
        {
            _IArchiveBLPMModel = _ModelFactory.CreateObject<IArchiveBLPMModel>();
        }


        public List<ArchiveBLPM> GetArchiveBLPMBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                DataSet _Result;
                _Result = _IArchiveBLPMModel.GetArchiveBLPMBySearch(p_ClientGUID, p_SearchTermTitle, p_SearchTermDesc, p_SearchTermKeyword, p_SearchTermCC, p_FromDate, p_ToDate, p_Category1GUID, p_Category2GUID, p_Category3GUID, p_Category4GUID, p_CategoryOperator1, p_CategoryOperator2, p_CategoryOperator3, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, out p_TotalRecordsCount);
                List<ArchiveBLPM> _ListOfArchiveBLPM = FillArchiveBLPMInformation(_Result);

                return _ListOfArchiveBLPM;
            }
            catch (Exception)
            {
                throw;
            }
        }


        private List<ArchiveBLPM> FillArchiveBLPMInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveBLPM> _ListOfArchiveBLPM = new List<ArchiveBLPM>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveBLPM _ArchiveBLPM = new ArchiveBLPM();

                        if (_DataTable.Columns.Contains("ArchiveBLPMKey") && !_DataRow["ArchiveBLPMKey"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.ArchiveBLPMKey = Convert.ToInt32(_DataRow["ArchiveBLPMKey"]);
                        }

                        if (_DataTable.Columns.Contains("Pub_Name") && !_DataRow["Pub_Name"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Pub_Name = Convert.ToString(_DataRow["Pub_Name"]);
                        }

                        if (_DataTable.Columns.Contains("BLID") && !_DataRow["BLID"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.BLID = Convert.ToString(_DataRow["BLID"]);
                        }

                        if (_DataTable.Columns.Contains("Headline") && !_DataRow["Headline"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Headline = Convert.ToString(_DataRow["Headline"]);
                        }


                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("PubDate") && !_DataRow["PubDate"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.PubDate = (DateTime?)(_DataRow["PubDate"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("FileLocation") && !_DataRow["FileLocation"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.FileLocation = Convert.ToString(_DataRow["FileLocation"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryGUID") && !_DataRow["CategoryGUID"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.CategoryGuid = new Guid(Convert.ToString(_DataRow["CategoryGUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1GUID") && !_DataRow["SubCategory1GUID"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.SubCategory1Guid = new Guid(Convert.ToString(_DataRow["SubCategory1GUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2GUID") && !_DataRow["SubCategory2GUID"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.SubCategory2Guid = new Guid(Convert.ToString(_DataRow["SubCategory2GUID"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3GUID") && !_DataRow["SubCategory3GUID"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.SubCategory3Guid = new Guid(Convert.ToString(_DataRow["SubCategory3GUID"]));
                        }

                        if (_DataTable.Columns.Contains("Url") && !_DataRow["Url"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Url = Convert.ToString(_DataRow["Url"]);
                        }

                        if (_DataTable.Columns.Contains("Circulation") && !_DataRow["Circulation"].Equals(DBNull.Value))
                        {
                            _ArchiveBLPM.Circulation = Convert.ToInt32(_DataRow["Circulation"]);
                        }

                        _ListOfArchiveBLPM.Add(_ArchiveBLPM);

                    }
                }

                return _ListOfArchiveBLPM;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmailContent(List<ArchiveBLPM> lstArchiveBLPM)
        {
            try
            {
                StringBuilder emailContent = new StringBuilder();
                emailContent.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                emailContent.Append("<tr>");
                emailContent.Append("<th style=\"width:150px;\" align=\"center\">Title</th>");
                emailContent.Append("<th>URL</th>");
                emailContent.Append("</tr>");
                foreach (ArchiveBLPM archiveBLPM in lstArchiveBLPM)
                {
                    emailContent.AppendFormat("<tr><td style=\"width:150px;\" align=\"center\">{0}</td>", HttpContext.Current.Server.HtmlEncode(archiveBLPM.Headline));
                    emailContent.AppendFormat("<td ><a href=\"{0}\">{0}</a></td></tr>", HttpContext.Current.Server.HtmlEncode(archiveBLPM.Url));


                }
                emailContent.Append("</table>");
                return emailContent.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public string UpdateArchivePM(ArchiveBLPM archiveBLPM)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveBLPMModel.UpdateArchivePM(archiveBLPM);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveBLPM> GetArchivePMByArchiveBLPMKey(int p_ArchiveBLPMKey)
        {
            try
            {
                DataSet _Result;
                _Result = _IArchiveBLPMModel.GetArchivePMByArchiveBLPMKey(p_ArchiveBLPMKey);
                List<ArchiveBLPM> lstArchiveBLPM = FillArchiveBLPMInformation(_Result);
                return lstArchiveBLPM;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteArchivePM(string p_DeleteArchivePM)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveBLPMModel.DeleteArchivePM(p_DeleteArchivePM);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }

}