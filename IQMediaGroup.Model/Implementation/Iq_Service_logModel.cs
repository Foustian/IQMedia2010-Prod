using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Core.Enumeration;

namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IRL_GUIDSModel
    /// </summary>
    internal class Iq_Service_logModel : IQMediaGroupDataLayer, IIq_Service_logModel
    {

        /// <summary>
        /// Description:This method will insert Iq_Service_log.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <returns>primary key of Iq_Service_log</returns>
        public string AddIq_Service_log(Iq_Service_log p_Iq_Service_log)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ModuleName", DbType.String, p_Iq_Service_log.ModuleName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedDatetime", DbType.DateTime, p_Iq_Service_log.CreatedDatetime, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ServiceCode", DbType.String, p_Iq_Service_log.ServiceCode, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ConfigRequest", DbType.String, p_Iq_Service_log.ConfigRequest, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@Iq_Service_logKey", DbType.Int64, p_Iq_Service_log.Iq_Service_logKey, ParameterDirection.Output));

                _Result = this.ExecuteNonQuery("usp_Iq_Service_log_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will insert Iq_Service_log.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_Iq_Service_log">object of Iq_Service_log</param>
        /// <param name="p_ConnectionStringKeys">Name of ConnectionString</param>
        /// <returns>primary key of Iq_Service_log</returns>
        public string AddIq_Service_log(Iq_Service_log p_Iq_Service_log, ConnectionStringKeys p_ConnectionStringKeys)
        {
            try
            {
                this.CONNECTION_STRING_KEY = p_ConnectionStringKeys.ToString();

                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ModuleName", DbType.String, p_Iq_Service_log.ModuleName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CreatedDatetime", DbType.DateTime, p_Iq_Service_log.CreatedDatetime, ParameterDirection.Input));
                //_ListOfDataType.Add(new DataType("@EndedDateTime", DbType.DateTime, p_Iq_Service_log.EndedDateTime, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ServiceCode", DbType.String, p_Iq_Service_log.ServiceCode, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ConfigRequest", DbType.String, p_Iq_Service_log.ConfigRequest, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@Iq_Service_logKey", DbType.Int64, p_Iq_Service_log.Iq_Service_logKey, ParameterDirection.Output));


                _Result=this.ExecuteNonQuery("usp_Iq_Service_log_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}