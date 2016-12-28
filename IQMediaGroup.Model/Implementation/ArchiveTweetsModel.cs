using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class ArchiveTweetsModel : IQMediaGroupDataLayer, IArchiveTweetsModel
    {

        public DataSet GetArchiveTweetsBySearch(Guid p_ClientGUID, string p_SearchTermTitle, string p_SearchTermDesc, string p_SearchTermKeyword, string p_SearchTermCC, DateTime? p_FromDate, DateTime? p_ToDate, Guid? p_Category1GUID, Guid? p_Category2GUID, Guid? p_Category3GUID, Guid? p_Category4GUID, string p_CategoryOperator1, string p_CategoryOperator2, string p_CategoryOperator3, string p_ListCustomerGUID, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsAscending, out int p_TotalRecordsCount)
        {
            try
            {
                p_TotalRecordsCount = 0;
                string _output;

                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAscending", DbType.Boolean, p_IsAscending, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermTitle", DbType.String, p_SearchTermTitle, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermDesc", DbType.String, p_SearchTermDesc, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermKeyword", DbType.String, p_SearchTermKeyword, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SearchTermCC", DbType.String, p_SearchTermCC, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DateFrom", DbType.Date, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DateTo", DbType.Date, p_ToDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID1", DbType.Guid, p_Category1GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID2", DbType.Guid, p_Category2GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID3", DbType.Guid, p_Category3GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGUID4", DbType.Guid, p_Category4GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator1", DbType.String, p_CategoryOperator1, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator2", DbType.String, p_CategoryOperator2, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryOperator3", DbType.String, p_CategoryOperator3, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.String, p_ListCustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TotalRecordsCount", DbType.Int32, p_TotalRecordsCount, ParameterDirection.Output));


                _DataSet = GetDataSetWithOutParam("usp_ArchiveTweets_Search", _ListOfDataType, out _output);

                if (!string.IsNullOrEmpty(_output))
                {
                    p_TotalRecordsCount = Convert.ToInt32(_output);
                }

                return _DataSet;
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
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArchiveTweetsKeys", DbType.String, p_DeleteArchiveTweets, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_ArchiveTweets_Delete", _ListOfDataType);

                return _Result;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        public DataSet GetArchiVeTweetsByArchiveTweets_Key(Int64 p_ArchiveTweetsKey)
        {
            try
            {

                DataSet _Result;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ArchiveTweets_Key", DbType.Int64, p_ArchiveTweetsKey, ParameterDirection.Input));
                _Result = GetDataSet("usp_ArchiveTweets_GetByArchiveTweets_Key", _ListOfDataType);
                return _Result;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        public string UpdateArchiveTweets(ArchiveTweets archiveTweets)
        {
            try
            {
                string _Result;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ArchiveTweets_Key", DbType.Int64, archiveTweets.ArchiveTweets_Key, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, archiveTweets.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryGuid", DbType.Guid, archiveTweets.CategoryGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory1Guid", DbType.Guid, archiveTweets.SubCategory1Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory2Guid", DbType.Guid, archiveTweets.SubCategory2Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SubCategory3Guid", DbType.Guid, archiveTweets.SubCategory3Guid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Description", DbType.String, archiveTweets.Description, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Keywords", DbType.String, archiveTweets.Keywords, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Rating", DbType.Int16, archiveTweets.Rating, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_ArchiveTweets_Update", _ListOfDataType);
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