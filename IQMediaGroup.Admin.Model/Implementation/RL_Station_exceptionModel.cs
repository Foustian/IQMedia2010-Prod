using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Core.Enumeration;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IRL_Station_exceptionModel
    /// </summary>
    internal class RL_Station_exceptionModel : IQMediaGroupDataLayer, IRL_Station_exceptionModel
    {

        /// <summary>
        /// This Method adds RL_Station_exception Information.
        /// </summary>
        /// <param name="p_RL_Station_exception">RL_Station_exception Object of core</param>
        /// <returns>RL_Station_exceptionKey of added record</returns>
        public string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception,DateTime p_RequestedDate,int p_RequestedTime)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_Station_ID", DbType.String, p_RL_Station_exception.RL_Station_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Date", DbType.DateTime, p_RL_Station_exception.RL_Station_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Time", DbType.Int32, p_RL_Station_exception.RL_Station_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Time_zone", DbType.String, p_RL_Station_exception.Time_zone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GMT_Adj", DbType.String, p_RL_Station_exception.GMT_Adj, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DST_Adj", DbType.String, p_RL_Station_exception.DST_Adj, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Process", DbType.String, p_RL_Station_exception.IQ_Process, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Pass_count", DbType.String, p_RL_Station_exception.Pass_count, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestedDate", DbType.Date, p_RequestedDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RequestedTime", DbType.Int32, p_RequestedTime, ParameterDirection.Input));
                


                _ListOfDataType.Add(new DataType("@RL_Station_exceptionKey", DbType.Int64, p_RL_Station_exception.RL_Station_exceptionKey, ParameterDirection.Output));


                _Result = this.ExecuteNonQuery("usp_RL_Station_exception_InsertWithRequestedDateTime", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public string AddRL_Station_exception(RL_Station_exception p_RL_Station_exception,bool p_IsMisMatch)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_Station_ID", DbType.String, p_RL_Station_exception.RL_Station_ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Date", DbType.DateTime, p_RL_Station_exception.RL_Station_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_Station_Time", DbType.Int32, p_RL_Station_exception.RL_Station_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Time_zone", DbType.String, p_RL_Station_exception.Time_zone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@GMT_Adj", DbType.String, p_RL_Station_exception.GMT_Adj, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DST_Adj", DbType.String, p_RL_Station_exception.DST_Adj, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Process", DbType.String, p_RL_Station_exception.IQ_Process, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Pass_count", DbType.String, p_RL_Station_exception.Pass_count, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RL_CC_TEXT_FileName", DbType.String, p_RL_Station_exception.RL_CC_TEXT_FileName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RQ_Converted_Date", DbType.Date, p_RL_Station_exception.RQ_Converted_Date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@RQ_Converted_Time", DbType.Int32, p_RL_Station_exception.RQ_Converted_Time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsMisMatch", DbType.Boolean, p_IsMisMatch, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@RL_Station_exceptionKey", DbType.Int64, p_RL_Station_exception.RL_Station_exceptionKey, ParameterDirection.Output));


                _Result = this.ExecuteNonQuery("usp_RL_Station_exception_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method deletes RL_Station_exception
        /// </summary>
        /// <param name="p_RL_Station_exceptionKey">RL_Station_exceptionKey</param>
        /// <returns>1/0-Deleted or not</returns>
        public string DeleteRL_Station_exception(string p_RL_Station_exceptionKey)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@RL_Station_exceptionKey", DbType.String, p_RL_Station_exceptionKey, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_RL_Station_exception_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetNoPassCountRL_Station_exception(DateTime p_StartDate, DateTime p_EndDate, IQ_Process p_IQ_Process)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@StartDate", DbType.Date, p_StartDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@EndDate", DbType.Date, p_EndDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Process", DbType.String, p_IQ_Process.ToString(), ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_RL_Station_Exception_SelectNoPasscount", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}