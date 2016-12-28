using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQNotificationSettingsController
    {
        /// <summary>
        /// Description:This method will insert NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">object of IQNotificationSettings</param>
        /// <returns>Primary key of IQNotificationSettings</returns>
        string InsertNotificationSettings(IQNotificationSettings p_IQNotificationSettings);

        /// <summary>
        /// Description:This method will update NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">object of IQNotificationSettings</param>
        /// <returns>Count</returns>
        string UpdateNotificationSettings(IQNotificationSettings p_IQNotificationSettings);

        /// <summary>
        /// Description:This method will get NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">object of IQNotificationSettings</param>
        /// <returns>List of object of IQNotificationSettings</returns>
        List<IQNotificationSettings> GetIQNotificationSettings(IQNotificationSettings p_IQNotificationSettings);

        /// <summary>
        /// Description:This method will delete NotificationSettings.
        /// Added By:maulik Gandhi
        /// </summary>
        /// <param name="p_NotificationSettings">object of IQNotificationSettings</param>
        /// <returns>count</returns>
        string DeleteNotificationSettings(string p_NotificationSettings);
    }

}
