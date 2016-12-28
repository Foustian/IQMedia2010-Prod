using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQClient_CustomSettingsModel : IQMediaGroupDataLayer, IIQClient_CustomSettingsModel
    {

        public string GetSettingByClientGUID(Guid p_ClientGuid)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
               IDataReader _darareader = this.GetDataReader("usp_IQClient_CustomSettings_SelectByClientGUID", _ListOfDataType);

               if (_darareader.Read())
                   _Result = _darareader.GetString(0);


                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public void GetSentimentSettingsByClientGuid(Guid p_ClientGuid, out float? p_TVLowThreshold, out float? p_TVHighThreshold, out float? p_NMLowThreshold, out float? p_NMHighThreshold, out float? p_SMLowThreshold, out float? p_SMHighThreshold, out float? p_TwitterLowThreshold, out float? p_TwitterHighThreshold)
        {
            try
            {

                p_TVLowThreshold = null; p_NMLowThreshold = null; p_SMLowThreshold = null; p_TwitterLowThreshold = null;
                p_TVHighThreshold = null; p_NMHighThreshold = null; p_SMHighThreshold = null; p_TwitterHighThreshold = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                DataSet _DataSet = this.GetDataSet("usp_IQClient_CustomSettings_SelectSentimentSettingsByClientGuid", _ListOfDataType);

                if (_DataSet.Tables[0].Rows.Count > 0)
                {
                    p_TVLowThreshold = _DataSet.Tables[0].Rows[0]["TVLowThreshold"].Equals(DBNull.Value) ? null : (float?) Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["TVLowThreshold"]),5);
                    p_TVHighThreshold = _DataSet.Tables[0].Rows[0]["TVHighThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["TVHighThreshold"]),5);
                    p_NMLowThreshold = _DataSet.Tables[0].Rows[0]["NMLowThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["NMLowThreshold"]), 5);
                    p_NMHighThreshold = _DataSet.Tables[0].Rows[0]["NMHighThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["NMHighThreshold"]), 5);
                    p_SMLowThreshold = _DataSet.Tables[0].Rows[0]["SMLowThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["SMLowThreshold"]), 5);
                    p_SMHighThreshold = _DataSet.Tables[0].Rows[0]["SMHighThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["SMHighThreshold"]), 5);
                    p_TwitterLowThreshold = _DataSet.Tables[0].Rows[0]["TwitterLowThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["TwitterLowThreshold"]), 5);
                    p_TwitterHighThreshold = _DataSet.Tables[0].Rows[0]["TwitterHighThreshold"].Equals(DBNull.Value) ? null : (float?)Math.Round(Convert.ToDouble(_DataSet.Tables[0].Rows[0]["TwitterHighThreshold"]), 5); 
                     
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
