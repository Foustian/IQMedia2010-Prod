using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;
using System.Data.SqlTypes;


namespace IQMediaGroup.Model.Implementation
{
  
    internal class IQNotificationSettingsModel : IQMediaGroupDataLayer, IIQNotificationSettingsModel
    {
        /// <summary>
        /// Description:This method will Insert NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="_IQNotificationSettings">Objcet of IQNotificationSettings</param>
        /// <returns>Primary Key of IQNotificationSettings</returns>
        public string InsertNotificationSettings(IQNotificationSettings _IQNotificationSettings)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int32, _IQNotificationSettings.SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@TypeofEntry", DbType.String, _IQNotificationSettings.TypeofEntry, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Notification_Address", DbType.String, _IQNotificationSettings.Notification_Address, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Frequency", DbType.String, _IQNotificationSettings.Frequency, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@IQNotificationKey", DbType.Int32, _IQNotificationSettings.IQNotificationKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_IQNotificationSettings_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception; 
            }
        }

        /// <summary>
        /// Description:This method will Update NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">Primary Key of IQNotificationSettings</param>
        /// <returns>Count</returns>
        public string UpdateNotificationSettings(IQNotificationSettings p_IQNotificationSettings)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int32, p_IQNotificationSettings.SearchRequestID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQNotificationKey", DbType.Int32, p_IQNotificationSettings.IQNotificationKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Notification_Address", DbType.String, p_IQNotificationSettings.Notification_Address, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Frequency", DbType.String, p_IQNotificationSettings.Frequency, ParameterDirection.Input));
                _Result = ExecuteNonQuery("usp_IQNotificationSettings_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will get NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="_IQNotificationSettings">Objcet of IQNotificationSettings</param>
        /// <returns>DataSet of IQNotificationSettings</returns>
        public DataSet GetNotificationSettings(IQNotificationSettings _IQNotificationSettings)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@SearchRequestID", DbType.Int32, _IQNotificationSettings.SearchRequestID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQNotificationSettings_SelectBySearchRequestID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will Delete NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="p_NotificationSettings">Primary Key of NotificationSettings</param>
        /// <returns>Count</returns>
        public string DeleteNotificationSettings(string p_NotificationSettings)
        {
            try
            {

                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@IQNotificationKeys", DbType.String, p_NotificationSettings, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_IQNotificationSettings_Delete", _ListOfDataType);

                return _Result;

            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

    }
}
