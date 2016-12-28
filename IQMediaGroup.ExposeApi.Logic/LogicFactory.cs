using System;
using System.Collections;

namespace IQMediaGroup.ExposeApi.Logic
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
                 case LogicType.StatskedprogData:
                     return new StatskedprogLogic();    
                 case LogicType.RawMedia:
                     return new RawMediaLogic();
                 case LogicType.RadioStation:
                     return new RadioStationLogic();
                 case LogicType.PMGSearchLog:
                     return new PMGSearchLogLogic();
                 case LogicType.Validation:
                     return new ValidationLogic();
                 case LogicType.Authentication:
                     return new AuthenticationLogic();
                 case LogicType.Archive:
                     return new ArchiveLogic();
                 case LogicType.IQAgent:
                     return new IQAgentLogic();
                 case LogicType.Category:
                     return new CategoryLogic();
                 default:
                     //If we get to this point, no logic has bee defined and the code 'SHOULD' fail...
                     throw new ArgumentException("No Logic defined for requested type: '" + logicType + "'");
             }
         }

    }

    public enum LogicType
    {
        StatskedprogData,
        RawMedia,
        RadioStation,
        PMGSearchLog,
        Validation,
        Authentication,
        Archive,
        IQAgent,
        Category
    }
}
