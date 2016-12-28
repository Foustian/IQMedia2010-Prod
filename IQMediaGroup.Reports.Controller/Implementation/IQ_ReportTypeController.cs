using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class IQ_ReportTypeController : IIQ_ReportTypeController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQ_ReportTypeModel _IIQ_ReportTypeModel;

        public IQ_ReportTypeController()
        {
            _IIQ_ReportTypeModel = _ModelFactory.CreateObject<IIQ_ReportTypeModel>();
        }

        public List<IQ_ReportType> GetReportTypeByClientSettings(Guid p_ClientGuid, string p_MasterReportType)
        {
            try
            {
                DataSet _DataSet = _IIQ_ReportTypeModel.GetReportTypeByClientSettings(p_ClientGuid, p_MasterReportType);

                List<IQ_ReportType> _ListOfIQ_ReportType = null;
                _ListOfIQ_ReportType = FillIQ_ReportType(_DataSet);
                return _ListOfIQ_ReportType;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<IQ_ReportType> FillIQ_ReportType(DataSet _DataSet)
        {
            try
            {
                List<IQ_ReportType> _ListOfIQ_ReportType = new List<IQ_ReportType>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQ_ReportType _IQ_ReportType = new IQ_ReportType();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _IQ_ReportType.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Name"))
                        {
                            _IQ_ReportType.Name = Convert.ToString(_DataRow["Name"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Identity"))
                        {
                            _IQ_ReportType.Identity = Convert.ToString(_DataRow["Identity"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Description"))
                        {
                            _IQ_ReportType.Description = Convert.ToString(_DataRow["Description"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("DateCreated") && !_DataRow["DateCreated"].Equals(DBNull.Value))
                        {
                            _IQ_ReportType.DateCreated = Convert.ToDateTime(_DataRow["DateCreated"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _IQ_ReportType.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ReportIdentity") && !_DataRow["ReportIdentity"].Equals(DBNull.Value))
                        {
                            _IQ_ReportType.Identity = Convert.ToString(_DataRow["ReportIdentity"]);
                        }
                        _ListOfIQ_ReportType.Add(_IQ_ReportType);
                    }
                }

                return _ListOfIQ_ReportType;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
