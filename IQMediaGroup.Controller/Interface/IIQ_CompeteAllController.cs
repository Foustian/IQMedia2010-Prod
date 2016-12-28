using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface IIQ_CompeteAllController
    {
        /// <summary>
        /// This method gets AD Share Value For Article By ClientGuid and Xml Of Artiles WebSiteURL
        /// </summary>
        /// <returns>List containing AD Share Values of Artiles.</returns>
        List<IQ_CompeteAll> GetArtileAdShareValueByClientGuidAndXml(Guid p_ClientGuid, string p_WebSiteURLXml,string p_MediaType);
    }
}
