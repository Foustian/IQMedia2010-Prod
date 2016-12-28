using System;
using System.Collections.Generic;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class UGCDownloadTrackingModel : IQMediaGroup.Model.Base.IQMediaGroupDataLayer, IQMediaGroup.Model.Interface.IUGCDownloadTrackingModel
    {
        public string Insert(UGCDownloadTracking _UGCDownloadTracking)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, _UGCDownloadTracking.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UGCGUID", DbType.Guid, _UGCDownloadTracking.UGCGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DownloadedDateTime", DbType.DateTime, _UGCDownloadTracking.DownloadedDateTime, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsDownloadSuccess", DbType.Boolean, _UGCDownloadTracking.IsDownloadSuccess, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@DownloadDescription", DbType.String, _UGCDownloadTracking.DownloadDescription, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@UGCDownloadTrackingKey", DbType.Int64, _UGCDownloadTracking.UGCDownloadTrackingKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_UGCDownloadTracking_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }
    }
}