using System;
using System.Collections;

namespace IQMediaGroup.Logic
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
                case LogicType.ClipDownload:
                    return new ClipDownloadLogic();
                case LogicType.PlayerData:
                    return new PlayerDataLogic();
                case LogicType.UGCRawClipExport:
                    return new ExportUGCRawClipLogic();
                case LogicType.WaterMark:
                    return new GetWaterMarkLogic();
                case LogicType.Client:
                    return new ClientLogic();
                case LogicType.BookMarkService:
                    return new BookmarkServiceLogic();
                case LogicType.EmailService:
                    return new EmailServiceLogic();
                case LogicType.CategoryService:
                    return new CategoriesServiceLogic();
                case LogicType.Validation:
                    return new ValidationLogic();
                case LogicType.NielSen:
                    return new NielSenDataLogic();
                case LogicType.IQAgentFrame:
                    return new IQAgentIframeLogic();
                case LogicType.GetHighlightedCC:
                    return new CCLogic();
                case LogicType.RawMedia:
                    return new RawMediaLogic();
                case LogicType.NM:
                    return new NewsLogic();
                case LogicType.SM:
                    return new SMLogic();
                case LogicType.Compete:
                    return new CompeteDataLogic();
                case LogicType.Station:
                    return new StationLogic();
                case LogicType.TW:
                    return new TWLogic();
                case LogicType.Tracking:
                    return new TrackingLogic();
                case LogicType.License:
                    return new LicenseLogic();
                case LogicType.TimeSync:
                    return new TimeSyncLogic();
                case LogicType.PQ:
                    return new PQLogic();
                default:
                    //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                    throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
            }
        }

    }

    public enum LogicType
    {
        ClipDownload,
        PlayerData,
        UGCRawClipExport,
        WaterMark,
        Client,
        BookMarkService,
        EmailService,
        CategoryService,
        Validation,
        NielSen,
        IQAgentFrame,
        GetHighlightedCC,
        RawMedia,
        NM,
        SM,
        Compete,
        Station,
        TW,
        Tracking,
        License,
        TimeSync,
        PQ
    }
}