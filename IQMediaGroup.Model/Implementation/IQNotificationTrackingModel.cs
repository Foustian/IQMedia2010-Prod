using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQNotificationTrackingModel : IQMediaGroupDataLayer, IIQNotificationTrackingModel
    {
        #region IIQNotificationTrackingModel Members

        /// <summary>
        /// Description:This method will Select IQNotificationTracking.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>DataSet of IQNotificationTracking</returns>
        public DataSet SelectForNotification()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_IQNotificationTracking_SelectByCommunicationFlag", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will Update IQNotificationTracking.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationTracking">object of IQNotificationTracking</param>
        /// <returns>Count</returns>
        public string Update(IQNotificationTracking p_IQNotificationTracking)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQNotificationTrackingKey", DbType.Int64, p_IQNotificationTracking.IQNotificationTrackingKey, ParameterDirection.Input));
                _Result = ExecuteNonQuery("usp_IQNotificationTracking_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        #endregion
    }
}
