using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class IQ_ReportController : IIQ_ReportController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQ_ReportModel _IMentionReportModel;

        public IQ_ReportController()
        {
            _IMentionReportModel = _ModelFactory.CreateObject<IIQ_ReportModel>();
        }

        public IQ_Report GetReportXmlByReportGUID(Guid guid)
        {
            try
            {
                DataSet _DataSet = _IMentionReportModel.GetReportXmlByReportGUID(guid);

                List<IQ_Report> _ListOfIQ_Report = null;
                _ListOfIQ_Report = FillIQ_Report(_DataSet);
                return _ListOfIQ_Report.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<IQ_Report> GetReportByReportTypeAndClientGuid(int p_ReportType, Guid p_ClientGuid, DateTime p_ReportDate)
        {
            try
            {
                DataSet _DataSet = _IMentionReportModel.GetReportByReportTypeAndClientGuid(p_ReportType, p_ClientGuid, p_ReportDate);

                List<IQ_Report> _ListOfIQ_Report = null;
                _ListOfIQ_Report = FillIQ_Report(_DataSet);
                return _ListOfIQ_Report;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public List<IQ_Report> FillIQ_Report(DataSet _DataSet)
        {
            try
            {
                List<IQ_Report> _ListOfIQ_Report = new List<IQ_Report>();

                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQ_Report _IQ_Report = new IQ_Report();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && !_DataRow["ID"].Equals(DBNull.Value))
                        {
                            _IQ_Report.ID = Convert.ToInt32(_DataRow["ID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ReportRule") && !_DataRow["ReportRule"].Equals(DBNull.Value))
                        {
                            _IQ_Report.ReportRule = Convert.ToString(_DataRow["ReportRule"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Name") && !_DataRow["Name"].Equals(DBNull.Value))
                        {
                            _IQ_Report.ReportType = Convert.ToString(_DataRow["Name"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Identity") && !_DataRow["Identity"].Equals(DBNull.Value))
                        {
                            _IQ_Report.Identity = Convert.ToString(_DataRow["Identity"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ClientGuid") && !_DataRow["ClientGuid"].Equals(DBNull.Value))
                        {
                            _IQ_Report.ClientGuid = new Guid(_DataRow["ClientGuid"].ToString());
                        }

                        if (_DataSet.Tables[0].Columns.Contains("DateCreated") && !_DataRow["DateCreated"].Equals(DBNull.Value))
                        {
                            _IQ_Report.DateCreated = Convert.ToDateTime(_DataRow["DateCreated"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _IQ_Report.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("_ReportTypeID"))
                        {
                            _IQ_Report._ReportTypeID = Convert.ToInt32(_DataRow["_ReportTypeID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Title"))
                        {
                            _IQ_Report.Title = Convert.ToString(_DataRow["Title"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("ReportGUID"))
                        {
                            _IQ_Report.ReportGUID = new Guid(Convert.ToString(_DataRow["ReportGUID"]));
                        }



                        _ListOfIQ_Report.Add(_IQ_Report);
                    }
                }

                return _ListOfIQ_Report;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
