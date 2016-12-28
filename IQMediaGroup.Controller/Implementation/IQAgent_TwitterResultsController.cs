using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQAgent_TwitterResultsController : IIQAgent_TwitterResultsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQAgent_TwitterResultsModel _IIQAgent_TwitterResultsModel;

        public IQAgent_TwitterResultsController()
        {
            _IIQAgent_TwitterResultsModel = _ModelFactory.CreateObject<IIQAgent_TwitterResultsModel>();
        }

        public List<IQAgent_TwitterResult> GetIQAgentTwitterResultsBySearchRequestID(int p_SearchRequestID, int p_PageSize, int p_PageNumber, string p_SortField, bool p_IsAcending, out int p_TotalRecordsCount)
        {
            try
            {
                List<IQAgent_TwitterResult> _ListOfIQAgent_TwitterResult;
                DataSet _DataSet = _IIQAgent_TwitterResultsModel.GetIQAgentTwitterResultsBySearchRequestID(p_SearchRequestID, p_PageSize, p_PageNumber, p_SortField, p_IsAcending, out p_TotalRecordsCount);
                _ListOfIQAgent_TwitterResult = FillIQAgent_TwitterResultInformation(_DataSet);
                return _ListOfIQAgent_TwitterResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQAgent_TwitterResult> FillIQAgent_TwitterResultInformation(DataSet _DataSet)
        {
            try
            {
                List<IQAgent_TwitterResult> _ListOfIQAgent_TwitterResult = new List<IQAgent_TwitterResult>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        IQAgent_TwitterResult _IQAgent_TwitterResult = new IQAgent_TwitterResult();

                        if (_DataTable.Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataTable.Columns.Contains("actor_link") && !_DataRow["actor_link"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.actor_link = Convert.ToString(_DataRow["actor_link"]);
                        }

                        if (_DataTable.Columns.Contains("actor_displayName") && !_DataRow["actor_displayName"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.actor_displayName = Convert.ToString(_DataRow["actor_displayName"]);
                        }

                        if (_DataTable.Columns.Contains("actor_preferredName") && !_DataRow["actor_preferredName"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.actor_preferredName = Convert.ToString(_DataRow["actor_preferredName"]);
                        }

                        if (_DataTable.Columns.Contains("Summary") && !_DataRow["Summary"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.Summary = Convert.ToString(_DataRow["Summary"]);
                        }

                        if (_DataTable.Columns.Contains("gnip_Klout_score") && !_DataRow["gnip_Klout_score"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.gnip_Klout_score = Convert.ToInt16(_DataRow["gnip_Klout_score"]);
                        }

                        if (_DataTable.Columns.Contains("actor_followerscount") && !_DataRow["actor_followerscount"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.actor_followerscount = Convert.ToInt32(_DataRow["actor_followerscount"]);
                        }

                        if (_DataTable.Columns.Contains("actor_friendscount") && !_DataRow["actor_friendscount"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.actor_friendscount = Convert.ToInt32(_DataRow["actor_friendscount"]);
                        }


                        if (_DataTable.Columns.Contains("tweet_postedDateTime") && !_DataRow["tweet_postedDateTime"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.tweet_postedDateTime = Convert.ToDateTime(_DataRow["tweet_postedDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("actor_image") && !_DataRow["actor_image"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.actor_image = Convert.ToString(_DataRow["actor_image"]);
                        }

                        if (_DataTable.Columns.Contains("tweetid") && !_DataRow["tweetid"].Equals(DBNull.Value))
                        {
                            _IQAgent_TwitterResult.tweetid = Convert.ToString(_DataRow["tweetid"]);
                        }


                        _ListOfIQAgent_TwitterResult.Add(_IQAgent_TwitterResult);
                    }
                }

                return _ListOfIQAgent_TwitterResult;

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
