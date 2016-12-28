using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IIQ_ReportController
    {
        List<IQ_Report> GetReportByReportTypeAndClientGuid(int p_ReportType, Guid p_ClientGuid, DateTime p_ReportDate);

        IQ_Report GetReportXmlByReportGUID(Guid guid);
    }
}
