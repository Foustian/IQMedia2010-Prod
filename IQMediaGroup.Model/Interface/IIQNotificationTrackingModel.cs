using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Interface
{
    public interface IIQNotificationTrackingModel
    {
        /// <summary>
        /// Description:This method will Select IQNotificationTracking.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>DataSet of IQNotificationTracking</returns>
        DataSet SelectForNotification();

        /// <summary>
        /// Description:This method will Update IQNotificationTracking.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_IQNotificationTracking">object of IQNotificationTracking</param>
        /// <returns>Count</returns>
        string Update(IQNotificationTracking _IQNotificationTracking);
    }
}
