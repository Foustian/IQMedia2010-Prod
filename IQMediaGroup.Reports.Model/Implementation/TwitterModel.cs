using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Reports.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Model.Implementation
{
    internal class TwitterModel : IQMediaGroupDataLayer, ITwitterModel
    {
        public DataSet GetIQAgent_TwitterResultBySearchDate(Guid p_ClientGuid, int p_IQAgentSearchRequestID, DateTime p_FromDate, DateTime p_ToDate, int p_NoOfRecordsToDisplay, out string Query_Name)
        {
            try
            {
                Query_Name = string.Empty;

                string _output;

                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQAgentSearchRequestID", DbType.Int32, p_IQAgentSearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.DateTime, p_ToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@NoOfRecordsToDisplay", DbType.Int32, p_NoOfRecordsToDisplay, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Query_Name", DbType.String, Query_Name, ParameterDirection.Output));

                _DataSet = this.GetDataSetWithOutParam("usp_IQAgent_TwitterResults_SelectReportByDate", _ListOfDataType, out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    Query_Name = Convert.ToString(_output);
                }

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string InsertArchiveTweet(ArchiveTweets _ArchiveTweets)
        {
            try
            {
                string _Result;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@Title", DbType.String, _ArchiveTweets.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, _ArchiveTweets.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, _ArchiveTweets.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, _ArchiveTweets.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGuid", DbType.Guid, _ArchiveTweets.CustomerGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, _ArchiveTweets.ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1Guid", DbType.Guid, _ArchiveTweets.SubCategory1Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2Guid", DbType.Guid, _ArchiveTweets.SubCategory2Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3Guid", DbType.Guid, _ArchiveTweets.SubCategory3Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Tweet_ID", DbType.Int64, _ArchiveTweets.Tweet_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Actor_DisplayName", DbType.String, _ArchiveTweets.Actor_DisplayName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Actor_PreferredUserName", DbType.String, _ArchiveTweets.Actor_PreferredUserName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Tweet_Body", DbType.String, _ArchiveTweets.Tweet_Body, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Actor_FollowersCount", DbType.Int64, _ArchiveTweets.Actor_FollowersCount, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Actor_FriendsCount", DbType.Int64, _ArchiveTweets.Actor_FriendsCount, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Actor_Image", DbType.String, _ArchiveTweets.Actor_Image, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Actor_link", DbType.String, _ArchiveTweets.Actor_link, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@gnip_Klout_Score", DbType.Int64, _ArchiveTweets.gnip_Klout_Score, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Tweet_PostedDateTime", DbType.DateTime, _ArchiveTweets.Tweet_PostedDateTime, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, _ArchiveTweets.Rating, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ArchiveTweetKey", DbType.Int64, _ArchiveTweets.ArchiveTweets_Key, ParameterDirection.Output));


                _Result = ExecuteNonQuery("usp_ArchiveTweets_Insert", _ListOfDataType);
                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
