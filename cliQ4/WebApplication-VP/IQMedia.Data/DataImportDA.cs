using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using IQMedia.Model;
using System.Data;
using IQMedia.Shared.Utility;

namespace IQMedia.Data
{
    public class DataImportDA : IDataAccess
    {
        public DataImportClientModel GetDataImportClient(Guid clientGuid)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQDataImport_Clients_SelectByClient", dataTypeList);

                DataImportClientModel clientModel = null;

                if (dsSSP != null && dsSSP.Tables.Count == 1)
                {
                    if (dsSSP.Tables[0].Rows.Count == 1)
                    {
                        DataRow datarow = dsSSP.Tables[0].Rows[0];
                        clientModel = new DataImportClientModel();

                        clientModel.ID = Convert.ToInt32(datarow["ID"]);
                        clientModel.ViewPath = Convert.ToString(datarow["ViewPath"]);
                        clientModel.GetResultsMethod = Convert.ToString(datarow["GetResultsMethod"]);
                    }
                    else if (dsSSP.Tables[0].Rows.Count > 1)
                    {
                        // There should never be more than 1 record per client.
                        Log4NetLogger.Error(String.Format("Unexpected number of records ({0}) retrieved from IQDataImport_Clients for client {1}.", dsSSP.Tables[0].Rows.Count, clientGuid));
                    }
                }

                return clientModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Sony

        public List<SonySummaryModel> GetSonySummaryData(Guid clientGuid, DateTime fromDate, DateTime toDate, int dateIntervalType, string searchRequestIDXml, string filterXml, bool hasTM, bool hasTV, bool hasNM, bool hasTW, bool hasPM, bool hasPQ, bool hasSM)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, toDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DateIntervalType", DbType.Int16, dateIntervalType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FilterXml", DbType.Xml, filterXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsTVAccess", DbType.Boolean, hasTV, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsNMAccess", DbType.Boolean, hasNM, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsTWAccess", DbType.Boolean, hasTW, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsRadioAccess", DbType.Boolean, hasTM, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsSMAccess", DbType.Boolean, hasSM, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsBLPMAccess", DbType.Boolean, hasPM, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@IsPQAccess", DbType.Boolean, hasPQ, ParameterDirection.Input));

                DataSet dsSSP = DataAccess.GetDataSet("usp_v5_IQDataImport_Sony_SelectSummary", dataTypeList);

                return FillSonySummaryData(dsSSP);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SonySummaryModel> FillSonySummaryData(DataSet dataSet)
        {
            List<SonySummaryModel> lstSummaryData = new List<SonySummaryModel>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                // Chart Summary Data
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    SonySummaryModel dataSummaryModel = new SonySummaryModel();

                    if (!datarow["GMTDateTime"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.GMTDateTime = Convert.ToDateTime(datarow["GMTDateTime"]);
                    }
                    if (!datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                    }
                    if (!datarow["Artist"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.Artist = Convert.ToString(datarow["Artist"]);
                    }
                    if (!datarow["Album"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.Album = Convert.ToString(datarow["Album"]);
                    }
                    if (!datarow["Track"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.Track = Convert.ToString(datarow["Track"]);
                    }
                    if (!datarow["SubMediaType"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    }
                    if (!datarow["SearchRequestID"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.SearchRequestID = Convert.ToInt64(datarow["SearchRequestID"]);
                    }
                    if (!datarow["SeriesType"].Equals(DBNull.Value))
                    {
                        dataSummaryModel.SeriesType = Convert.ToString(datarow["SeriesType"]);
                    }

                    lstSummaryData.Add(dataSummaryModel);
                }
            }

            return lstSummaryData;
        }

        public List<SonyTableModel> GetSonyTableData(Guid clientGuid, DateTime fromDate, DateTime toDate, string searchRequestIDXml, int pageSize, int startIndex, string tableType, out int numTotalRecords)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                Dictionary<string, string> outParameter;
                dataTypeList.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.DateTime, fromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.DateTime, toDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, searchRequestIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@PageSize", DbType.Int32, pageSize, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@StartIndex", DbType.Int32, startIndex, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@TableType", DbType.String, tableType, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@NumTotalRecords", DbType.Int32, 0, ParameterDirection.Output));

                DataSet dsSSP = DataAccess.GetDataSetWithOutParam("usp_v5_IQDataImport_Sony_SelectTableData", dataTypeList, out outParameter);

                numTotalRecords = 0;
                if (outParameter != null && outParameter.Count > 0)
                {
                    numTotalRecords = !string.IsNullOrWhiteSpace(outParameter["@NumTotalRecords"]) ? Convert.ToInt32(outParameter["@NumTotalRecords"]) : 0;
                }

                return FillSonyTableData(dsSSP);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<SonyTableModel> FillSonyTableData(DataSet dataSet)
        {
            List<SonyTableModel> lstTableData = new List<SonyTableModel>();

            if (dataSet != null && dataSet.Tables.Count > 0)
            {
                foreach (DataRow datarow in dataSet.Tables[0].Rows)
                {
                    SonyTableModel trackModel = new SonyTableModel();

                    if (!datarow["RowNum"].Equals(DBNull.Value))
                    {
                        trackModel.RowID = Convert.ToInt32(datarow["RowNum"]);
                    }
                    if (!datarow["Artist"].Equals(DBNull.Value))
                    {
                        trackModel.Artist = Convert.ToString(datarow["Artist"]);
                    }
                    if (!datarow["Album"].Equals(DBNull.Value))
                    {
                        trackModel.Album = Convert.ToString(datarow["Album"]);
                    }
                    if (!datarow["Track"].Equals(DBNull.Value))
                    {
                        trackModel.Track = Convert.ToString(datarow["Track"]);
                    }
                    if (!datarow["TotalCount"].Equals(DBNull.Value))
                    {
                        trackModel.TotalCount = Convert.ToInt64(datarow["TotalCount"]);
                    }
                    if (!datarow["SpotifyCount"].Equals(DBNull.Value))
                    {
                        trackModel.SpotifyCount = Convert.ToInt64(datarow["SpotifyCount"]);
                    }
                    if (!datarow["ITunesDownloadCount"].Equals(DBNull.Value))
                    {
                        trackModel.ITunesDownloadCount = Convert.ToInt64(datarow["ITunesDownloadCount"]);
                    }
                    if (!datarow["ITunesStreamingCount"].Equals(DBNull.Value))
                    {
                        trackModel.ITunesStreamingCount = Convert.ToInt64(datarow["ITunesStreamingCount"]);
                    }

                    lstTableData.Add(trackModel);
                }
            }

            return lstTableData;
        }

        #endregion
    }
}
