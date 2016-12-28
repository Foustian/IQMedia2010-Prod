using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Interface
{
    public interface IClipDownloadModel
    {
        /// <summary>
        /// This method gets ClipDownload details by Customer
        /// </summary>
        /// <param name="p_CustomerGUID"></param>
        /// <returns></returns>
        DataSet SelectByCustomer(Guid p_CustomerGUID);

        /// <summary>
        /// This method inserted ClipDownload details
        /// </summary>
        /// <param name="p_CustomerGUID">CustomerGUID</param>
        /// <param name="p_XmlData">Xml contains ClipID</param>
        /// <returns>Result</returns>
        string Insert(Guid p_CustomerGUID, SqlXml p_XmlData);

        string DeactivateClip(Guid p_ClipID);

        string Update(SqlXml p_SqlXml);

        string UpdateClipDownloadStatus(Int64 p_IQ_ClipDownload_Key, Int16 p_ClipDownloadStatus, string p_Location);
        DataSet GetFileLocationFromClipMeta(string p_XML);
        bool CheckForExistingStatusOfService(Guid p_ClipGUID, string Ext);
    }
}
