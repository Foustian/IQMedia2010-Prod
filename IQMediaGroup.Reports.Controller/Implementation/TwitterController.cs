using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using PMGSearch;
using System.Data;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    public class TwitterController : ITwitterController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ITwitterModel _ITwitterModel;

        public TwitterController()
        {
            _ITwitterModel = _ModelFactory.CreateObject<ITwitterModel>();
        }

        public List<TwitterResult> GetIQAgent_TwitterResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, out string Query_Name)
        {
            try
            {
                DataSet _DataSet = null;
                List<TwitterResult> _ListOfTwitterResult = null;

                _DataSet = _ITwitterModel.GetIQAgent_TwitterResultBySearchDate(p_ClientGuid, p_IQAgentSearchRequestID, p_FromDate, p_ToDate, p_NoOfRecordsToDisplay, out Query_Name);
                _ListOfTwitterResult = FillNewsResultInformation(_DataSet);
                return _ListOfTwitterResult;

            }
            catch (Exception)
            {
                throw;
            }
        }


        private List<TwitterResult> FillNewsResultInformation(DataSet _DataSet)
        {
            try
            {
                List<TwitterResult> _ListOfTwitterResult = new List<TwitterResult>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        TwitterResult _TwitterResult = new TwitterResult();

                        

                        if (_DataTable.Columns.Contains("actor_link") && !_DataRow["actor_link"].Equals(DBNull.Value))
                        {
                            _TwitterResult.actor_link = Convert.ToString(_DataRow["actor_link"]);
                        }

                        if (_DataTable.Columns.Contains("actor_displayName") && !_DataRow["actor_displayName"].Equals(DBNull.Value))
                        {
                            _TwitterResult.actor_displayName = Convert.ToString(_DataRow["actor_displayName"]);
                        }

                        if (_DataTable.Columns.Contains("actor_preferredName") && !_DataRow["actor_preferredName"].Equals(DBNull.Value))
                        {
                            _TwitterResult.actor_prefferedUserName = Convert.ToString(_DataRow["actor_preferredName"]);
                        }

                        if (_DataTable.Columns.Contains("Summary") && !_DataRow["Summary"].Equals(DBNull.Value))
                        {
                            _TwitterResult.tweet_body = Convert.ToString(_DataRow["Summary"]);
                        }

                        if (_DataTable.Columns.Contains("gnip_Klout_score") && !_DataRow["gnip_Klout_score"].Equals(DBNull.Value))
                        {
                            _TwitterResult.Klout_score = Convert.ToInt64(_DataRow["gnip_Klout_score"]);
                        }

                        if (_DataTable.Columns.Contains("actor_followerscount") && !_DataRow["actor_followerscount"].Equals(DBNull.Value))
                        {
                            _TwitterResult.followers_count = Convert.ToInt64(_DataRow["actor_followerscount"]);
                        }

                        if (_DataTable.Columns.Contains("actor_friendscount") && !_DataRow["actor_friendscount"].Equals(DBNull.Value))
                        {
                            _TwitterResult.friends_count = Convert.ToInt64(_DataRow["actor_friendscount"]);
                        }

                        if (_DataTable.Columns.Contains("tweet_postedDateTime") && !_DataRow["tweet_postedDateTime"].Equals(DBNull.Value))
                        {
                            _TwitterResult.tweet_postedDateTime = Convert.ToString(_DataRow["tweet_postedDateTime"]);
                        }

                        if (_DataTable.Columns.Contains("actor_image") && !_DataRow["actor_image"].Equals(DBNull.Value))
                        {
                            _TwitterResult.actor_image = Convert.ToString(_DataRow["actor_image"]);
                        }
                        if (_DataTable.Columns.Contains("tweetid") && !_DataRow["tweetid"].Equals(DBNull.Value))
                        {
                            _TwitterResult.tweet_id = Convert.ToInt64(_DataRow["tweetid"]);
                        }

                        _ListOfTwitterResult.Add(_TwitterResult);
                    }
                }

                return _ListOfTwitterResult;

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
                _Result = _ITwitterModel.InsertArchiveTweet(_ArchiveTweets);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
