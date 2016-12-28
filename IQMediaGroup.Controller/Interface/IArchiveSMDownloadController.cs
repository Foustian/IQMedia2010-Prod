using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    public interface IArchiveSMDownloadController
    {
        List<ArchiveSMDownload> GetByCustomerGuid(Guid p_CustomerGuid);

        string InsertList(Guid p_CustomerGuid, SqlXml p_XmlData);

        string DeactivateArticle(Guid p_ID);

        string UpdateDownloadStatus(Int64 p_ID, Int16 p_DownloadStatus, string p_FileLocation);

        List<ArchiveSMDownload> GetArticleFileLocationAndStatus(string p_XML);
    }
}
