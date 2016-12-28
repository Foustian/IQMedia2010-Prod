﻿using System;
using System.Collections.Generic;
using System.Data;
using IQMedia.Data.Base;

namespace IQMedia.Data
{
    public class GoogleDA : IDataAccess
    {
        public void UpdateAuthCode(Guid clientGuid, string authCode)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@AuthCode", DbType.String, authCode, ParameterDirection.Input));
                DataAccess.ExecuteNonQuery("usp_v4_IQ_Google_UpdateAuthCode", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool CheckClientAccess(Guid clientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> outParameter = null;
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@HasAccess", DbType.Boolean, false, ParameterDirection.Output));
                DataSet result = DataAccess.GetDataSetWithOutParam("usp_v4_IQ_Google_CheckClientAccess", dataTypeList, out outParameter);

                if (outParameter != null && outParameter.Count > 0)
                {
                    return !string.IsNullOrWhiteSpace(outParameter["@HasAccess"]) ? Convert.ToBoolean(outParameter["@HasAccess"]) : false;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetClientID()
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                DataSet result = DataAccess.GetDataSet("usp_v4_IQ_Google_GetClientID", dataTypeList);

                if (result.Tables.Count == 1)
                {
                    return Convert.ToString(result.Tables[0].Rows[0][0]);
                }
                return String.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
