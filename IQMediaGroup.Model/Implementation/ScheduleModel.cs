using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;

using System.Configuration;

namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of interface IStationModel
    /// </summary>
    internal class ScheduleModel : IQMediaGroupDataLayer,IScheduleModel
    {
        /// <summary>
        /// This method inserts Schedule information.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="p_Schedule">Object of Schedule class</param>
        /// <returns>ScheduleKey</returns>
        public string InsertSchedule(Schedule p_Schedule)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@StationID", DbType.String, p_Schedule.StationID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ProgramID", DbType.String, p_Schedule.ProgramID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Air_Date", DbType.DateTime, p_Schedule.air_date, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Air_Time", DbType.String, p_Schedule.air_time, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Duration", DbType.String, p_Schedule.duration, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TV_Rating", DbType.String, p_Schedule.tv_rating, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ScheduleKey", DbType.Int64, p_Schedule.ScheduleKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_Schedule_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
