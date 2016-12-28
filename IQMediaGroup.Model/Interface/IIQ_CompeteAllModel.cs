using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;


namespace IQMediaGroup.Model.Interface
{
    public interface IIQ_CompeteAllModel
    {
        /// <summary>
        /// This method gets AD Share Value For Article By ClientGuid and Xml Of Artiles WebSiteURL
        /// </summary>
        /// <returns>Dataset containing AD Share Values of Artiles.</returns>
        DataSet GetArtileAdShareValueByClientGuidAndXml(Guid p_ClientGuid,string p_WebSiteURLXml,string p_MediaType);
    }
}
