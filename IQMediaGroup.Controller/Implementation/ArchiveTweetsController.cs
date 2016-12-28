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
    public class ArchiveTweetsController : IArchiveTweetsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveTweetsModel _IArchiveTweetsModel;

        public ArchiveTweetsController()
        {
            _IArchiveTweetsModel = _ModelFactory.CreateObject<IArchiveTweetsModel>();
        }


        public List<ArchiveTweets> GetArchiveTweetsBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveTweets> _ListOfArchiveTweets = null;

                _DataSet = _IArchiveTweetsModel.GetArchiveTweetsBySearch(p_ClientGUID, p_SearchTermTitle, p_SearchTermDesc, p_SearchTermKeyword, p_SearchTermCC, p_FromDate, p_ToDate, p_Category1GUID, p_Category2GUID, p_Category3GUID, p_Category4GUID, p_CategoryOperator1, p_CategoryOperator2, p_CategoryOperator3, p_ListCustomerGUID, p_PageNumber, p_PageSize, p_SortField, p_IsAscending, out p_TotalRecordsCount);
                _ListOfArchiveTweets = FillArchiveTweetsInformation(_DataSet);
                return _ListOfArchiveTweets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<ArchiveTweets> FillArchiveTweetsInformation(DataSet _DataSet)
        {
            try
            {
                List<ArchiveTweets> _ListOfArchiveTweets = new List<ArchiveTweets>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        ArchiveTweets _ArchiveTweets = new ArchiveTweets();

                        if (_DataTable.Columns.Contains("ArchiveTweets_Key") && !_DataRow["ArchiveTweets_Key"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.ArchiveTweets_Key = Convert.ToInt32(_DataRow["ArchiveTweets_Key"]);
                        }

                        if (_DataTable.Columns.Contains("Actor_FollowersCount") && !_DataRow["Actor_FollowersCount"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_FollowersCount = Convert.ToInt64(_DataRow["Actor_FollowersCount"]);
                        }

                        if (_DataTable.Columns.Contains("Actor_FriendsCount") && !_DataRow["Actor_FriendsCount"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_FriendsCount = Convert.ToInt64(_DataRow["Actor_FriendsCount"]);
                        }

                        if (_DataTable.Columns.Contains("Tweet_Body") && !_DataRow["Tweet_Body"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Tweet_Body = Convert.ToString(_DataRow["Tweet_Body"]);
                        }

                        if (_DataTable.Columns.Contains("Actor_Image") && !_DataRow["Actor_Image"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_Image = Convert.ToString(_DataRow["Actor_Image"]);
                        }

                        if (_DataTable.Columns.Contains("Tweet_PostedDateTime") && !_DataRow["Tweet_PostedDateTime"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Tweet_PostedDateTime = Convert.ToDateTime(_DataRow["Tweet_PostedDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("gnip_Klout_Score") && !_DataRow["gnip_Klout_Score"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.gnip_Klout_Score = Convert.ToInt64(_DataRow["gnip_Klout_Score"]);
                        }

                        if (_DataTable.Columns.Contains("Actor_DisplayName") && !_DataRow["Actor_DisplayName"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_DisplayName = Convert.ToString(_DataRow["Actor_DisplayName"]);
                        }

                        if (_DataTable.Columns.Contains("Title") && !_DataRow["Title"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataTable.Columns.Contains("Actor_link") && !_DataRow["Actor_link"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_link = Convert.ToString(_DataRow["Actor_link"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryGuid") && !_DataRow["CategoryGuid"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.CategoryGuid = new Guid(Convert.ToString(_DataRow["CategoryGuid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory1Guid") && !_DataRow["SubCategory1Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.SubCategory1Guid = new Guid(Convert.ToString(_DataRow["SubCategory1Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory2Guid") && !_DataRow["SubCategory2Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.SubCategory2Guid = new Guid(Convert.ToString(_DataRow["SubCategory2Guid"]));
                        }

                        if (_DataTable.Columns.Contains("SubCategory3Guid") && !_DataRow["SubCategory3Guid"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.SubCategory3Guid = new Guid(Convert.ToString(_DataRow["SubCategory3Guid"]));
                        }

                        if (_DataTable.Columns.Contains("Description") && !_DataRow["Description"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataTable.Columns.Contains("Keywords") && !_DataRow["Keywords"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Keywords = Convert.ToString(_DataRow["Keywords"]);
                        }

                        if (_DataTable.Columns.Contains("Rating") && !_DataRow["Rating"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Rating = Convert.ToInt16(_DataRow["Rating"]);
                        }

                        if (_DataTable.Columns.Contains("Actor_PreferredUserName") && !_DataRow["Actor_PreferredUserName"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_PreferredUserName = Convert.ToString(_DataRow["Actor_PreferredUserName"]);
                        }

                        _ListOfArchiveTweets.Add(_ArchiveTweets);
                    }
                }

                return _ListOfArchiveTweets;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public string DeleteArchiveTweets(string p_DeleteArchiveTweets)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveTweetsModel.DeleteArchiveTweets(p_DeleteArchiveTweets);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<ArchiveTweets> GetArchiVeTweetsByArchiveTweets_Key(Int64 p_ArchiveTweetsKey)
        {
            try
            {
                DataSet _Result;
                List<ArchiveTweets> _ListOfArchiveTweets = null;
                _Result = _IArchiveTweetsModel.GetArchiVeTweetsByArchiveTweets_Key(p_ArchiveTweetsKey);
                _ListOfArchiveTweets = FillArchiveTweetsInformation(_Result);
                return _ListOfArchiveTweets;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string UpdateArchiveTweets(ArchiveTweets archiveTweets)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveTweetsModel.UpdateArchiveTweets(archiveTweets);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string InsertArchiveTweet(ArchiveTweets _ArchiveTweets)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IArchiveTweetsModel.InsertArchiveTweet(_ArchiveTweets);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetEmailContent(List<ArchiveTweets> lstArchiveTweets)
        {
            try
            {
                StringBuilder emailContent = new StringBuilder();
                emailContent.Append("<table border=\"0\" cellpadding=\"3\" cellspacing=\"3\" style=\"font-family:Verdana;font-size:11px;\">");
                emailContent.Append("<tr>");
                emailContent.Append("<th style=\"width:150px;\" align=\"center\">Title</th>");
                emailContent.Append("<th>Tweet Body</th>");
                emailContent.Append("<th>Display Name</th>");
                emailContent.Append("<th>Tweet Posted Date</th>");
                emailContent.Append("</tr>");
                foreach (ArchiveTweets archiveTweets in lstArchiveTweets)
                {
                    emailContent.AppendFormat("<tr><td style=\"width:150px;\" align=\"center\">{0}</td>", HttpContext.Current.Server.HtmlEncode(archiveTweets.Title));
                    emailContent.AppendFormat("<td>{0}</td>", HttpContext.Current.Server.HtmlEncode(archiveTweets.Tweet_Body));
                    emailContent.AppendFormat("<td>{0}</td>", HttpContext.Current.Server.HtmlEncode(archiveTweets.Actor_DisplayName));
                    emailContent.AppendFormat("<td>{0}</td></tr>", HttpContext.Current.Server.HtmlEncode(Convert.ToString(archiveTweets.Tweet_PostedDateTime)));
                }
                emailContent.Append("</table>");
                return emailContent.ToString();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

    }

}