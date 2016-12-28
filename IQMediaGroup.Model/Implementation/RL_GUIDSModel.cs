using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IRL_GUIDSModel
    /// </summary>
    internal class RL_GUIDSModel : IQMediaGroupDataLayer, IRL_GUIDSModel
    {

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

        /// <summary>
        /// This method gets all RL_GUIDS Records
        /// </summary>
        /// <param name="p_StatSkedProg">Object of Statskedprog</param>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        public DataSet GetAllRL_GUIDSByStatskedprogParams(StatSkedProg p_StatSkedProg)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, p_StatSkedProg.MinDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.DateTime, p_StatSkedProg.MaxDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Dma_Num", DbType.String, p_StatSkedProg.IQ_Dma_Num, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Cat", DbType.String, p_StatSkedProg.IQ_Cat, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_class", DbType.String, p_StatSkedProg.IQ_class, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title120", DbType.String, p_StatSkedProg.Title120, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Desc100", DbType.String, p_StatSkedProg.Desc100, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Station_Affil", DbType.String, p_StatSkedProg.Station_Affil, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_RL_GUIDS_SelectBySTATSKEDPROGParams", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets RL_GUIDS by IQCCKey of RadioStations
        /// </summary>
        /// <param name="p_IQCCKey">IQCCKey</param>
        /// <param name="p_PageNumber">PageNumber</param>
        /// <param name="p_PageSize">PageSize</param>
        /// <returns>Dataset contains RL_GUIDS information</returns>
        public DataSet GetAllRL_GUIDSByRadioStations(string p_IQCCKey, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQCCKey", DbType.String, p_IQCCKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsSortDirectionAsc", DbType.Boolean, p_IsSortDirectionAsc, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_RL_GUIDS_SelectByRadioStationsIQCCKey", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        public DataSet GetAllRL_GUIDSByRadioStationsByXML(SqlXml p_IQStationIDXML, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc, DateTime p_FromDate, DateTime p_ToDate)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQ_Station_ID", DbType.Xml, p_IQStationIDXML, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, p_PageNumber, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, p_PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, p_SortField, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsSortDirectionAsc", DbType.Boolean, p_IsSortDirectionAsc, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, p_FromDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Todate", DbType.DateTime, p_ToDate, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_RL_GUIDS_SelectByRadioStationsIQCCKeyXML", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}