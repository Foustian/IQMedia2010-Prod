using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Reports.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Model.Implementation
{
    internal class IQ_ReportModel : IQMediaGroupDataLayer, IIQ_ReportModel
    {
        
        public DataSet GetReportXmlByReportGUID(Guid guid)
        {
            try
            {
                DataSet _DataSet = null;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ReportGUID", DbType.Guid, guid, ParameterDirection.Input));

                _DataSet = GetDataSet("usp_IQ_Report_SelectReportByReportGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetReportByReportTypeAndClientGuid(int p_ReportType, Guid p_ClientGuid,DateTime p_ReportDate)
        {
            try
            {
                DataSet _DataSet = null;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ReportType", DbType.Int32, p_ReportType, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ReportDate", DbType.DateTime, p_ReportDate, ParameterDirection.Input));

                _DataSet = GetDataSet("usp_IQ_Report_SelectReportByReportTypeAndClientGuid", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
