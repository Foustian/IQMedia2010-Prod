using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class ArchiveTweetsController : IArchiveTweetsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IArchiveTweetsModel _IArchiveTweetsModel;

        public ArchiveTweetsController()
        {
            _IArchiveTweetsModel = _ModelFactory.CreateObject<IArchiveTweetsModel>();
        }

        public List<ArchiveTweets> GetArchiveTweetsReportGroupByCategory(Guid p_ClientGUID, DateTime p_Date)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveTweets> _ListOfArchiveTweets = null;

                _DataSet = _IArchiveTweetsModel.GetArchiveTweetsReportGroupByCategory(p_ClientGUID, p_Date);
                _ListOfArchiveTweets = FillArchiveTweetsInformation(_DataSet);
                return _ListOfArchiveTweets;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<ArchiveTweets> GetArchiveTweetsByCategoryGuidAndDate(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_Date, Guid p_CategoryGuid)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveTweets> _ListOfArchiveTweets = null;

                _DataSet = _IArchiveTweetsModel.GetArchiveTweetsByCategoryGuidAndDate(p_ClientGUID, p_SortField, p_IsAscending, p_Date, p_CategoryGuid);
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

                        if (_DataTable.Columns.Contains("Actor_PreferredUserName") && !_DataRow["Actor_PreferredUserName"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.Actor_PreferredUserName = Convert.ToString(_DataRow["Actor_PreferredUserName"]);
                        }

                        if (_DataTable.Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _ArchiveTweets.CategoryNames = Convert.ToString(_DataRow["CategoryName"]);
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

        public List<ArchiveTweets> GetArchiveTweetsByDurationNCategoryGuid(Guid p_ClientGUID, string p_SortField, bool p_IsAscending, DateTime p_FromDate, DateTime p_ToDate, Guid p_CategoryGuid)
        {
            try
            {
                DataSet _DataSet = null;
                List<ArchiveTweets> _ListOfArchiveTweets = null;

                _DataSet = _IArchiveTweetsModel.GetArchiveTweetsByDurationNCategoryGuid(p_ClientGUID, p_SortField, p_IsAscending, p_FromDate, p_ToDate, p_CategoryGuid);
                _ListOfArchiveTweets = FillArchiveTweetsInformation(_DataSet);
                return _ListOfArchiveTweets;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
