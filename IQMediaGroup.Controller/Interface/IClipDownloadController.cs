using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;
using System.Data;

namespace IQMediaGroup.Controller.Interface
{
    public interface IClipDownloadController
    {
        List<ClipDownload> SelectByCustomer(Guid p_CustomerGUID);

        string Insert(Guid p_CustomerGUID, SqlXml p_XmlData);

        string DeactivateClip(Guid p_ClipID);

        string Update(SqlXml p_SqlXml);

        string UpdateClipDownloadStatus(Int64 p_IQ_ClipDownload_Key, Int16 p_ClipDownloadStatus, string p_Location);

        List<ClipMeta> GetFileLocationFromClipMeta(string p_XML);

        bool CheckForExistingStatusOfService(Guid p_ClipGUID, string Ext);
    }
}
