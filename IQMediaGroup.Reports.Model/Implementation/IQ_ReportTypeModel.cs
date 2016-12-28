using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Reports.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Model.Implementation
{
    internal class IQ_ReportTypeModel : IQMediaGroupDataLayer, IIQ_ReportTypeModel
    {
        public DataSet GetReportTypeByClientSettings(Guid p_ClientGuid,string p_MasterReportType)
        {
            try
            {
                DataSet _DataSet = null;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MasterReportType", DbType.String, p_MasterReportType, ParameterDirection.Input));

                _DataSet = GetDataSet("usp_IQ_ReportType_SelectByClientSettings", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
