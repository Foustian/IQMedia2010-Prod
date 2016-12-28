using System;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Collections.Generic;
using IQMediaGroup.Model.Base;
using System.Data;

namespace IQMediaGroup.Model.Implementaion
{
    internal class UGC_Upload_LogMdoel : IQMediaGroupDataLayer, IUGC_Upload_LogModel
    {
        public string Insert(UGC_Upload_Log p_UGC_Upload_Log)
        {
            try
            {
                string _Result = string.Empty;
                
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CustomerGUID", DbType.Guid, p_UGC_Upload_Log.CustomerGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UGCContentXml", DbType.Xml, p_UGC_Upload_Log.UGCContentXml, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FileName", DbType.String, p_UGC_Upload_Log.FileName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@UploadedDateTime", DbType.DateTime, p_UGC_Upload_Log.UploadedDateTime, ParameterDirection.Input));

                _Result = this.ExecuteNonQuery("usp_UGC_Upload_Log_Insert", _ListOfDataType);                

                return _Result;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }
    }
}