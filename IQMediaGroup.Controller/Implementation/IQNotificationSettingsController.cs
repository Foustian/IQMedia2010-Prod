using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using Microsoft.CSharp;
using System.Text.RegularExpressions;
using System.Data.SqlTypes;
using System.Collections;
using System.Xml.Linq;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQNotificationSettingsController : IIQNotificationSettingsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IIQNotificationSettingsModel _IIQNotificationSettingsModel;

        public IQNotificationSettingsController()
        {
            _IIQNotificationSettingsModel = _ModelFactory.CreateObject<IIQNotificationSettingsModel>();
        }

        /// <summary>
        /// Description:This method will insert NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">object of IQNotificationSettings</param>
        /// <returns>Primary key of IQNotificationSettings</returns>
        public string InsertNotificationSettings(IQNotificationSettings p_IQNotificationSettings)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IIQNotificationSettingsModel.InsertNotificationSettings(p_IQNotificationSettings);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will update NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">object of IQNotificationSettings</param>
        /// <returns>Count</returns>
        public string UpdateNotificationSettings(IQNotificationSettings p_IQNotificationSettings)
        {
            try
            {
                string _Result = _IIQNotificationSettingsModel.UpdateNotificationSettings(p_IQNotificationSettings);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will get NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">object of IQNotificationSettings</param>
        /// <returns>List of object of IQNotificationSettings</returns>
        public List<IQNotificationSettings> GetIQNotificationSettings(IQNotificationSettings p_IQNotificationSettings)
        {
            DataSet _DataSet = null;
            List<IQNotificationSettings> _ListOfIQNotificationSettings = null;
            try
            {
                _DataSet = _IIQNotificationSettingsModel.GetNotificationSettings(p_IQNotificationSettings);
                _ListOfIQNotificationSettings = FillNotificationSettings(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQNotificationSettings;
        }

        /// <summary>
        /// Description:This method will fill NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">object of IQNotificationSettings</param>
        /// <returns>List of object of IQNotificationSettings</returns>
        private List<IQNotificationSettings> FillNotificationSettings(DataSet _DataSet)
        {
            List<IQNotificationSettings> _ListOfIQNotificationSettings = new List<IQNotificationSettings>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQNotificationSettings _IQNotificationSettings = new IQNotificationSettings();
                        
                        _IQNotificationSettings.TypeofEntry = Convert.ToString(_DataRow["TypeofEntry"]);
                        _IQNotificationSettings.Notification_Address = Convert.ToString(_DataRow["Notification_Address"]);
                        _IQNotificationSettings.Frequency = Convert.ToString(_DataRow["Frequency"]);
                        _IQNotificationSettings.IQNotificationKey = Convert.ToInt32(_DataRow["IQNotificationKey"]);

                        _ListOfIQNotificationSettings.Add(_IQNotificationSettings);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQNotificationSettings;
        }

        /// <summary>
        /// Description:This method will delete NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_NotificationSettings">object of IQNotificationSettings</param>
        /// <returns>count</returns>
        public string DeleteNotificationSettings(string p_NotificationSettings)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IIQNotificationSettingsModel.DeleteNotificationSettings(p_NotificationSettings);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
