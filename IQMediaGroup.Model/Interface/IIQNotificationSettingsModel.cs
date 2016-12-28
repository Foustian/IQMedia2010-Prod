using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using IQMediaGroup.Model.Implementation;
namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface IIQNotificationSettingsModel
    {
        /// <summary>
        /// Description:This method will Insert NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="_IQNotificationSettings">Objcet of IQNotificationSettings</param>
        /// <returns>Primary Key of IQNotificationSettings</returns>
        string InsertNotificationSettings(IQNotificationSettings _IQNotificationSettings);

        /// <summary>
        /// Description:This method will Update NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="p_IQNotificationSettings">Primary Key of IQNotificationSettings</param>
        /// <returns>Count</returns>
        string UpdateNotificationSettings(IQNotificationSettings p_IQNotificationSettings);

        /// <summary>
        /// Description:This method will get NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="_IQNotificationSettings">Objcet of IQNotificationSettings</param>
        /// <returns>DataSet of IQNotificationSettings</returns>
        DataSet GetNotificationSettings(IQNotificationSettings _IQNotificationSettings);

        /// <summary>
        /// Description:This method will Delete NotificationSettings
        /// Added By:Maulik GAndhi
        /// </summary>
        /// <param name="p_NotificationSettings">Primary Key of NotificationSettings</param>
        /// <returns>Count</returns>
        string DeleteNotificationSettings(string p_NotificationSettings);
    }
}
