using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Interface
{
    public interface IArchiveSMDownloadModel
    {
        DataSet GetByCustomerGuid(Guid p_CustomerGuid);

        string InsertList(Guid p_CustomerGuid, SqlXml p_XmlData);

        string DeactivateArticle(Guid p_ID);

        string UpdateDownloadStatus(Int64 p_ID, Int16 p_DownloadStatus, string p_FileLocation);

        DataSet GetArticleFileLocationAndStatus(string p_XML);
    }
}
