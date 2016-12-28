using System;
using System.Collections;

namespace IQMedia.Logic
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

            return (ILogic) LogicMap[logicType];
        }

        /// <summary>
        /// Creates the logic for the specified logic type.
        /// </summary>
        /// <param name="logicType">Type of the logic.</param>
        /// <returns></returns>
        private static ILogic CreateLogic(LogicType logicType)
        {
            switch(logicType)
            {
                case LogicType.Asset:
                    return new AssetLogic();
                case LogicType.Clip:
                    return new ClipLogic();
                case LogicType.ClipMeta:
                    return new ClipMetaLogic();
                case LogicType.Recordfile:
                    return new RecordfileLogic();
                case LogicType.Recording:
                    return new RecordingLogic();
                case LogicType.RootPathType:
                    return new RootPathTypeLogic();
                case LogicType.Source:
                    return new SourceLogic();
                case LogicType.Tracking:
                    return new TrackingLogic();
                case LogicType.User:
                    return new UserLogic();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
            }
        }
    }

    public enum LogicType
    {
        Asset,
        Clip,
        ClipMeta,
        Recordfile,
        Recording,
        RootPathType,
        Source,
        Tracking,
        User
    }
}
