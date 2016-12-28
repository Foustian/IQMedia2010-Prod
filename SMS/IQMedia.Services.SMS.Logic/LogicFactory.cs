﻿using System;
using System.Collections;

namespace IQMedia.Services.SMS.Logic
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
                case LogicType.Subscription:
                    return new SubscriptionLogic();
                case LogicType.Validation:
                    return new ValidationLogic();
                case LogicType.CTSMSResult:
                    return new CTSMSResultLogic();
                case LogicType.Clickatell :
                    return new ClickatellLogic();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
            }
        }

    }

    public enum LogicType
    {
        Subscription,
        Validation,
        CTSMSResult,
        Clickatell
    }
}