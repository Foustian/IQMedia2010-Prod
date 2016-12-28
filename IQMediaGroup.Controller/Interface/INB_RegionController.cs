using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface INB_RegionController
    {
        /// <summary>
        /// Select All Region from the Database
        /// </summary>
        /// <returns>returns list of Region</returns>
        List<NB_Region> GetAllRegion();
    }
}
