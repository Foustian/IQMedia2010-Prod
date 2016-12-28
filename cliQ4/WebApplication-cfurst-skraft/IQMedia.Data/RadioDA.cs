using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;

namespace IQMedia.Data
{
    public class RadioDA : IDataAccess
    {
        public List<RadioStation> SelectRadioStations()
        {
            List<DataType> dataTypeList = new List<DataType>();
            DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectRadioStations", dataTypeList);

            List<RadioStation> radioStationList = new List<RadioStation>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    RadioStation rs = new RadioStation();

                    rs.DMA = Convert.ToString(dr["Market"]);
                    rs.StationID = Convert.ToString(dr["StationID"]);

                    radioStationList.Add(rs);
                }
            }

            return radioStationList;
        }

        public Dictionary<string,object> SelectRadioStationFilters()
        {
            List<DataType> dataTypeList = new List<DataType>();
            DataSet dataSet = DataAccess.GetDataSet("usp_v4_IQ_Station_SelectRadioFilters", dataTypeList);

            Dictionary<string,object> radioStationFilters = new Dictionary<string,object>();

            if (dataSet != null && dataSet.Tables.Count > 1)
            {
                List<TadsDma> marketList = new List<TadsDma>();
                List<TadsStation> stationList = new List<TadsStation>();
                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {
                    TadsDma dma = new TadsDma();

                    dma.Name = Convert.ToString(dr["MarketName"]);
                    dma.ID = Convert.ToString(dr["MarketId"]);
                    marketList.Add(dma);
                }
                radioStationFilters.Add("Market", marketList);
            
                foreach (DataRow dr in dataSet.Tables[1].Rows)
                {
                     TadsStation stat = new TadsStation();

                    stat.ID = Convert.ToString(dr["StationId"]);
                    stat.Name = Convert.ToString(dr["StationName"]);
                    stationList.Add(stat);
                }
                radioStationFilters.Add("Station", stationList);
            }
            return radioStationFilters;
        }

        public List<RadioModel> SelectRadioResults(DateTime? FromDate, DateTime? ToDate, string Market, bool IsAsc, int PageNo, int PageSize, ref long SinceID, out long TotalResults)
        {
            try
            {
                TotalResults = 0;

                Market = !string.IsNullOrWhiteSpace(Market) ? Market : null;

                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> p_outParameter;

                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Market", DbType.String, Market, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsAsc", DbType.Boolean, IsAsc, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageNo", DbType.Int32, PageNo, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int32, PageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SinceID", DbType.Int64, SinceID, ParameterDirection.Output));
                dataTypeList.Add(new DataType("@TotalResults", DbType.Int64, TotalResults, ParameterDirection.Output));

                DataSet dataSet = DataAccess.GetDataSetWithOutParam("usp_v4_Radio_SelectRadioResults", dataTypeList, out p_outParameter);

                if (p_outParameter != null && p_outParameter.Count > 0)
                {
                    SinceID = !string.IsNullOrWhiteSpace(p_outParameter["@SinceID"]) ? Convert.ToInt64(p_outParameter["@SinceID"]) : 0;
                    TotalResults = !string.IsNullOrWhiteSpace(p_outParameter["@TotalResults"]) ? Convert.ToInt32(p_outParameter["@TotalResults"]) : 0;
                }

                List<RadioModel> lstRadioModels = new List<RadioModel>();

                if (dataSet != null && dataSet.Tables.Count > 0)
                {
                    foreach (DataRow dr in dataSet.Tables[0].Rows)
                    {
                        RadioModel objRadioModel = new RadioModel();

                        if (!dr["RL_GUIDSKey"].Equals(DBNull.Value))
                        {
                            objRadioModel.RL_GUIDSKey = Convert.ToInt64(dr["RL_GUIDSKey"]);
                        }
                        if (!dr["RL_Station_ID"].Equals(DBNull.Value))
                        {
                            objRadioModel.IQ_Station_ID = Convert.ToString(dr["RL_Station_ID"]);
                        }
                        if (!dr["Dma_Name"].Equals(DBNull.Value))
                        {
                            objRadioModel.Market = Convert.ToString(dr["Dma_Name"]);
                        }
                        if (!dr["RL_StationDateTime"].Equals(DBNull.Value))
                        {
                            objRadioModel.RL_StationDateTime = Convert.ToDateTime(dr["RL_StationDateTime"]);
                        }
                        if (!dr["RL_GUID"].Equals(DBNull.Value))
                        {
                            objRadioModel.RL_GUID = Convert.ToString(dr["RL_GUID"]);
                        }

                        lstRadioModels.Add(objRadioModel);
                    }
                }

                return lstRadioModels;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
