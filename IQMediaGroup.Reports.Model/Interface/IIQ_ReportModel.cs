using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace IQMediaGroup.Reports.Model.Interface
{
    public interface IIQ_ReportModel
    {
        DataSet GetReportByReportTypeAndClientGuid(int p_ReportType, Guid p_ClientGuid, DateTime p_ReportDate);

        DataSet GetReportXmlByReportGUID(Guid guid);
    }
}
