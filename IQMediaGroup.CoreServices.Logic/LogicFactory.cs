using System;
using System.Collections;

namespace IQMediaGroup.CoreServices.Logic
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
                case LogicType.RecordFile:
                    return new RecordFileLogic();
                //case LogicType.PlayerData:
                //    return new PlayerDataLogic();
                //case LogicType.UGCRawClipExport:
                //    return new ExportUGCRawClipLogic();
                //case LogicType.WaterMark:
                //    return new GetWaterMarkLogic();
                //case LogicType.Client:
                //    return new ClientLogic();
                case LogicType.Source:
                    return new SourceLogic();
                case LogicType.RootPath:
                    return new RootPathLogic();
                case LogicType.FiveMinStaging:
                    return new FiveMinStagingLogic();
                case LogicType.Validation:
                    return new ValidationLogic();
                case LogicType.IQIngestion:
                    return new IQIngestionLogic();
                case LogicType.ServiceExpiration:
                    return new ServiceExpirationLogic();
                case LogicType.MoveMedia:
                    return new MoveMediaLogic();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
            }
        }

    }

    public enum LogicType
    {
        RecordFile,
        FiveMinStaging,
        Validation,
        Source,
        RootPath,
        IQIngestion,
        ServiceExpiration,
        MoveMedia
    }
}
