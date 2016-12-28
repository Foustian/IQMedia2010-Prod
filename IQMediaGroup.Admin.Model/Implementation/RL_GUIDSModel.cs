using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IRL_GUIDSModel
    /// </summary>
    internal class RL_GUIDSModel : IQMediaGroupDataLayer, IRL_GUIDSModel
    {

        /// <summary>
        /// This Method adds RL_GUIDS Information.
        /// </summary>
        /// <param name="p_RL_GUIDS">RL_GUIDS Object of core</param>
        /// <returns>RL_GUIDSKey of added record</returns>
        public string AddRL_GUIDS(RL_GUIDS p_RL_GUIDS,DateTime p_RequestDate,int p_RequestTime)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_Station_ID", DbType.String, p_RL_GUIDS.RL_Station_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Date", DbType.DateTime, p_RL_GUIDS.RL_Station_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Time", DbType.Int32, p_RL_GUIDS.RL_Station_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Time_zone", DbType.String, p_RL_GUIDS.RL_Time_zone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_GUID", DbType.String, p_RL_GUIDS.RL_GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GMT_Date", DbType.DateTime, p_RL_GUIDS.GMT_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GMT_Time", DbType.Int32, p_RL_GUIDS.GMT_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_CC_Key", DbType.String, p_RL_GUIDS.IQ_CC_Key, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GUID_Status", DbType.Boolean, p_RL_GUIDS.GUID_Status, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestDate", DbType.Date, p_RequestDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestTime", DbType.Int32, p_RequestTime, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@RL_GUIDSKey", DbType.Int64, p_RL_GUIDS.RL_GUIDSKey, ParameterDirection.Output));


                _Result = this.ExecuteNonQuery("usp_RL_GUIDS_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates RL_GUIDS detail
        /// </summary>
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param>
        /// <returns>1/0-Updated or not</returns>
        public string UpdateRL_GUIDS(RL_GUIDS p_RL_GUIDS)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_GUIDSKey", DbType.Int64, p_RL_GUIDS.RL_GUIDSKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_ID", DbType.String, p_RL_GUIDS.RL_Station_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Date", DbType.DateTime, p_RL_GUIDS.RL_Station_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Time", DbType.Int32, p_RL_GUIDS.RL_Station_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Time_zone", DbType.String, p_RL_GUIDS.RL_Time_zone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_GUID", DbType.String, p_RL_GUIDS.RL_GUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GMT_Date", DbType.DateTime, p_RL_GUIDS.GMT_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GMT_Time", DbType.Int32, p_RL_GUIDS.GMT_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_CC_Key", DbType.String, p_RL_GUIDS.IQ_CC_Key, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GUID_Status", DbType.Boolean, p_RL_GUIDS.GUID_Status, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedBy", DbType.String, p_RL_GUIDS.CreatedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedBy", DbType.String, p_RL_GUIDS.ModifiedBy, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedDate", DbType.DateTime, p_RL_GUIDS.CreatedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ModifiedDate", DbType.DateTime, p_RL_GUIDS.ModifiedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsActive", DbType.Boolean, p_RL_GUIDS.IsActive, ParameterDirection.Input));


                _Result = this.ExecuteNonQuery("usp_RL_GUIDS_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets all RL_GUIDS Records
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        public DataSet GetAllRL_GUIDS()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_RL_GUIDS_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method deletes RL_GUIDS
        /// </summary>
        /// <param name="p_RL_GUIDSKey">RL_GUIDSKey</param>
        /// <returns>1/0-Deleted or not</returns>
        public string DeleteRL_GUIDS(string p_RL_GUIDSKey)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_GUIDSKey", DbType.String, p_RL_GUIDSKey, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_RL_GUIDS_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets RL_GUIDS Records by IQ_CC_Key
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        public DataSet GetRL_GUIDSByIQCCKey(string p_IQ_CC_Key)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQ_CC_Key", DbType.String, p_IQ_CC_Key, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_RL_GUIDS_SelectByIQCCKey", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}