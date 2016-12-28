using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Interface
{
    public interface IIQ_ReportTypeController
    {
        List<IQ_ReportType> GetReportTypeByClientSettings(Guid p_ClientGuid, string p_MasterReportType);
    }
}
