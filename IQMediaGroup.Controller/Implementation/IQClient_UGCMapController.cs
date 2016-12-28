using System;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;

namespace IQMediaGroup.Controller
{
    internal class IQClient_UGCMapController : IIQClient_UGCMapController
    {
        ModelFactory _ModelFactory = new ModelFactory();
        IIQClient_UGCMapModel _IIQClient_UGCMapModel = null;

        public IQClient_UGCMapController()
        {
            _IIQClient_UGCMapModel = _ModelFactory.CreateObject<IIQClient_UGCMapModel>();
        }

        public IQClient_UGCMap GetIQClient_UGCMapByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                IQClient_UGCMap _IQClient_UGCMap = null;

                DataSet _DataSet = _IIQClient_UGCMapModel.GetIQClient_UGCMapByClientGUID(p_ClientGUID);

                _IQClient_UGCMap = FillIQClient_UGCMap(_DataSet);

                return _IQClient_UGCMap;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        private IQClient_UGCMap FillIQClient_UGCMap(DataSet _DataSet)
        {
            try
            {
                IQClient_UGCMap _IQClient_UGCMap = null;

                if (_DataSet!=null && _DataSet.Tables.Count>0 && _DataSet.Tables[0].Rows.Count>0)
                {
                    DataTable _DataTable = _DataSet.Tables[0];
                    _IQClient_UGCMap = new IQClient_UGCMap();

                    foreach (DataRow _DataRow in _DataTable.Rows)
                    {
                        if (_DataTable.Columns.Contains("IQClient_UGCMapKey") && !_DataRow["IQClient_UGCMapKey"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.IQClient_UGCMapKey = Convert.ToInt64(_DataRow["IQClient_UGCMapKey"]);
                        }

                        if (_DataTable.Columns.Contains("SourceGUID") && !_DataRow["SourceGUID"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.SourceGUID = new Guid(Convert.ToString(_DataRow["SourceGUID"]));
                        }

                        if (_DataTable.Columns.Contains("SourceID") && !_DataRow["SourceID"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.SourceID = Convert.ToString(_DataRow["SourceID"]);
                        }

                        if (_DataTable.Columns.Contains("ClientGUID") && !_DataRow["ClientGUID"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.ClientGUID = new Guid(Convert.ToString(_DataRow["ClientGUID"]));
                        }

                        if (_DataTable.Columns.Contains("AutoClip_Status") && !_DataRow["AutoClip_Status"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.AutoClip_Status = Convert.ToBoolean(_DataRow["AutoClip_Status"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }

                        if (_DataTable.Columns.Contains("ModifiedDate") && !_DataRow["ModifiedDate"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.ModifiedDate = Convert.ToDateTime(_DataRow["ModifiedDate"]);
                        }

                        if (_DataTable.Columns.Contains("CreatedBy") && !_DataRow["CreatedBy"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.CreatedBy = Convert.ToString(_DataRow["CreatedBy"]);
                        }

                        if (_DataTable.Columns.Contains("ModifiedBy") && !_DataRow["ModifiedBy"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.ModifiedBy = Convert.ToString(_DataRow["ModifiedBy"]);
                        }

                        if (_DataTable.Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _IQClient_UGCMap.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }
                    }
                }

                return _IQClient_UGCMap;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }
    }
}