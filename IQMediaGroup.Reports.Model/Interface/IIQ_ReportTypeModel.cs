using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface IIQ_ReportTypeModel
    {
        DataSet GetReportTypeByClientSettings(Guid p_ClientGuid, string p_MasterReportType);
    }
}
