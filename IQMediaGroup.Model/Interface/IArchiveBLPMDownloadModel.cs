using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using System.Data.SqlTypes;

namespace IQMediaGroup.Model.Interface
{
    public interface IArchiveBLPMDownloadModel
    {
        DataSet GetByCustomerGuid(Guid p_CustomerGuid);

        string InsertListArchivePMDownload(Guid p_CustomerGuid, SqlXml p_XmlData);        

        DataSet GetArchivePMDownload(Guid customerGuid);

        string UpdateArchivePMDownload(Int64 id, Int16 downloadStatus);

        string DeleteArchivePMDownload(Int64 id);

    }
}
