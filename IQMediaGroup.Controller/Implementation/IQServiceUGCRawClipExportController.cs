using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQServiceUGCRawClipExportController : IIQServiceUGCRawClipExportController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private readonly IIQServiceUGCRawClipExportModel _IIQServiceUGCRawClipExportModel;
        //List<Clip> _ListOfTempClip = new List<Clip>();

        public IQServiceUGCRawClipExportController()
        {
            _IIQServiceUGCRawClipExportModel = _ModelFactory.CreateObject<IIQServiceUGCRawClipExportModel>();
        }

        /// <summary>
        /// This method get the OutPutPAth of Clip By ClipGUID
        /// </summary>
        /// <param name="ClipGUID"></param>
        /// <returns></returns>
        public string GetClipPathByClipGUID(Guid ClipGUID)
        {
            try
            {
                string _OutPutPath = string.Empty;
                DataSet _DataSet;   
                _DataSet = _IIQServiceUGCRawClipExportModel.GetClipPathByClipGUID(ClipGUID);
                List<IQServiceUGCRawClipExport> _ListOfIQServiceUGCRawClipExport = FillIQServiceUGCRawClipExport(_DataSet);
                if (_ListOfIQServiceUGCRawClipExport.Count > 0)
                {
                    //_OutPutPath = _ListOfIQServiceUGCRawClipExport[0].OutputPath;
                }
                return _OutPutPath;
                
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        private List<IQServiceUGCRawClipExport> FillIQServiceUGCRawClipExport(DataSet _DataSet)
        {
            List<IQServiceUGCRawClipExport> _ListOfIQServiceUGCRawClipExport = new List<IQServiceUGCRawClipExport>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];

                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        IQServiceUGCRawClipExport _IQServiceUGCRawClipExport = new IQServiceUGCRawClipExport();

                        if (_DataTable.Columns.Contains("ClipGUID") && !_DataRow["ClipGUID"].Equals(DBNull.Value))
                        {
                            _IQServiceUGCRawClipExport.ClipGUID = new Guid(_DataRow["ClipGUID"].ToString());
                        }

                        //if (_DataTable.Columns.Contains("OutputPath") && !_DataRow["OutputPath"].Equals(DBNull.Value))
                        //{
                        //    _IQServiceUGCRawClipExport.OutputPath = Convert.ToString(_DataRow["OutputPath"]);
                        //}

                        if (_DataTable.Columns.Contains("Status") && !_DataRow["Status"].Equals(DBNull.Value))
                        {
                            _IQServiceUGCRawClipExport.Status = Convert.ToString(_DataRow["Status"]);
                        }

                        //if (_DataTable.Columns.Contains("NoOftimesDownloaded") && !_DataRow["NoOftimesDownloaded"].Equals(DBNull.Value))
                        //{
                        //    _IQServiceUGCRawClipExport.NoOftimesDownloaded = Convert.ToInt32(_DataRow["NoOftimesDownloaded"]);
                        //}

                        _ListOfIQServiceUGCRawClipExport.Add(_IQServiceUGCRawClipExport);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfIQServiceUGCRawClipExport;
        }
    }
}
