using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQNotificationTrackingController
    {
        /// <summary>
        /// Description:This method will get IQNotificationTracking
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List of object of IQNotificationTracking</returns>
        List<IQNotificationTracking> SelectForNotification();

        int SendNotifications(string p_FilePath);
    }
}
