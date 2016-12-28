using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Reports.Model.Implementation
{
    internal class IQAgent_SearchRequestModel : IQMediaGroupDataLayer, IIQAgent_SearchRequestModel
    {
        public void GetAllowedMediaTypesByID(int ID, out bool IsAllowTV, out bool IsAllowNM, out bool IsAllowSM)
        {
            try
            {

                DataSet _DataSet = null;

                IsAllowTV = false; IsAllowNM = false; IsAllowSM = false;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ID", DbType.Int32, ID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAllowTV", DbType.Boolean, IsAllowTV, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsAllowNM", DbType.Boolean, IsAllowNM, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsAllowSM", DbType.Boolean, IsAllowSM, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;

                _DataSet = this.GetDataSetWithOutParam("usp_IQAgent_SearchRequest_SelectAllowedMediaTypesByID", _ListOfDataType, out _OutputParams);
                if (_OutputParams != null && _OutputParams.Count > 2)
                {
                    IsAllowTV = Convert.ToBoolean(_OutputParams["@IsAllowTV"]);
                    IsAllowNM = Convert.ToBoolean(_OutputParams["@IsAllowNM"]);
                    IsAllowSM = Convert.ToBoolean(_OutputParams["@IsAllowSM"]);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
