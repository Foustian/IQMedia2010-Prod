using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace IQMediaGroup.Logic.IOS
{
    public static class LogicFactory
    {
        private static readonly Hashtable LogicMap = new Hashtable();

        /// <summary>
        /// Gets the logic from the singleton map. If it doesn't exist; creates it and adds it to the map.
        /// </summary>
        /// <param name="logicType">Type of the logic.</param>
        /// <returns></returns>
        public static ILogic GetLogic(LogicType logicType)
        {
            if (LogicMap[logicType] == null)
                LogicMap[logicType] = CreateLogic(logicType);

            return (ILogic)LogicMap[logicType];
        }

        /// <summary>
        /// Creates the logic for the specified logic type.
        /// </summary>
        /// <param name="logicType">Type of the logic.</param>
        /// <returns></returns>
        private static ILogic CreateLogic(LogicType logicType)
        {
            switch (logicType)
            {
                case LogicType.Clip:
                    return new ClipLogic();
                case LogicType.Customer:
                    return new CustomerLogic();
                case LogicType.UGCUpload:
                    return new UGCUploadLogic();
                case LogicType.Video:
                    return new VideoLogic();
                case LogicType.Tracking:
                    return new TrackingLogic();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
            }
        }

    }

    public enum LogicType
    {
        Clip,
        Export,
        Service,
        Customer,
        UGCUpload,
        Video,
        Tracking
    }
}
