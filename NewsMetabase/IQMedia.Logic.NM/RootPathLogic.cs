using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Domain.NM;

namespace IQMedia.Logic.NM
{
    public class RootPathLogic:BaseLogic,ILogic
    {
        /// <summary>
        /// Gets the next root path in a random pattern.
        /// </summary>
        /// <param name="pathType">Name of the RootPathType.</param>
        /// <param name="includeInactive">When set to 'true', will additionall include inactive rootpaths.</param>
        /// <returns>A random RootPath.</returns>
        public RootPath GetNextRootPath(PathType pathType, bool includeInactive = false)
        {
            var rptLgc = (RootPathTypeLogic)LogicFactory.GetLogic(LogicType.RootPathType);
            var rpt = rptLgc.GetRootPathTypeByName(pathType.ToString());

            if (rpt == null)
                throw new Exception("No valid rootpathtype found for specified pathType: " + pathType);
            //Cast to a list
            var paths = new List<RootPath>(includeInactive ? rpt.RootPaths : rpt.RootPaths.Where(rp => rp.IsActive));
            if (paths.Count <= 0)
                throw new Exception("No active rootpaths found for specified pathType: " + pathType);

            //Grab a random RootPath
            var idx = new Random().Next(paths.Count);
            return paths[idx];
        }

        public RootPath GetRootPathByID(int id)
        {
            return Context.RootPaths.FirstOrDefault(rp => rp.ID.Equals(id));          
        }
    }

    /// <summary>
    /// Helper Enumerator to strongly-type the RootPathTypes
    /// </summary>
    public enum PathType
    {
        NM
    }
}
