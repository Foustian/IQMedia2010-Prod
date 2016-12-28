using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Base;
using System.Data;
using System;
using IQMediaGroup.Core.HelperClasses;
using System.Collections.Generic;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQClient_UGCMapModel : IQMediaGroupDataLayer, IIQClient_UGCMapModel
    {
        public DataSet GetIQClient_UGCMapByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_IQClient_UGCMap_SelectByClientGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception) 
            {                
                throw _Exception;
            }
        }
    }
}