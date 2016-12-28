using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Interface
{
    public interface IArchiveBLPMDownloadController
    {
        List<ArchiveBLPMDownload> GetByCustomerGuid(Guid p_CustomerGuid);

        string InsertListArchivePMDownload(Guid p_CustomerGuid, SqlXml p_XmlData);

        List<ArchiveBLPMDownload> GetArchivePMDownload(Guid customerGuid);

        string UpdateArchivePMDownload(Int64 id, Int16 downloadStatus);

        string DeleteArchivePMDownload(Int64 id);


    }
}
