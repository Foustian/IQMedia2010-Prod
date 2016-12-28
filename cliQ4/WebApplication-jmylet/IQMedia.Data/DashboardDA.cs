using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMedia.Data.Base;
using System.Data;
using IQMedia.Model;

namespace IQMedia.Data
{
    public class DashboardDA
    {
        public List<SummaryReportModel> GetSummaryReportData(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate)
        {

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_SummaryReport_Select", dataTypeList);

            List<SummaryReportModel> listOfSummaryReportData = new List<SummaryReportModel>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    SummaryReportModel dashboardSumRep = new SummaryReportModel();
                    dashboardSumRep.GMT_DateTime = Convert.ToDateTime(datarow["DayDate"]);
                    dashboardSumRep.Number_Docs = Convert.ToInt64(datarow["NoOfDocs"]);
                    dashboardSumRep.MediaType = Convert.ToString(datarow["MediaType"]);
                    dashboardSumRep.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    dashboardSumRep.Number_Of_Hits = Convert.ToInt64(datarow["NoOfHits"]);
                    dashboardSumRep.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                    dashboardSumRep.Audience = Convert.ToInt64(datarow["Audience"]);

                    listOfSummaryReportData.Add(dashboardSumRep);

                }

                //dicSummaryReport.Add("SummaryReport", listOfSummaryReportData);
            }
            return listOfSummaryReportData;

        }

        public List<IQAgent_DaySummaryModel> GetHourSummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectByMedium", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public List<IQAgent_DaySummaryModel> GetDaySummaryMediumWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectByMedium", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        private List<IQAgent_DaySummaryModel> FillIQAgent_DaySummaryModel(DataSet ds)
        {
            try
            {
                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = new List<IQAgent_DaySummaryModel>();
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataRow datarow in ds.Tables[0].Rows)
                    {
                        IQAgent_DaySummaryModel iQAgent_DaySummaryModel = new IQAgent_DaySummaryModel();

                        if (ds.Tables[0].Columns.Contains("ClientGuid"))
                        {
                            iQAgent_DaySummaryModel.ClientGuid = new Guid(Convert.ToString(datarow["ClientGuid"]));
                        }
                        
                        if (ds.Tables[0].Columns.Contains("DayDate"))
                        {
                            iQAgent_DaySummaryModel.DayDate = Convert.ToDateTime(datarow["DayDate"]);
                        }
                        else if (ds.Tables[0].Columns.Contains("HourDateTime"))
                        {
                            iQAgent_DaySummaryModel.DayDate = Convert.ToDateTime(datarow["HourDateTime"]);
                        }

                        if (ds.Tables[0].Columns.Contains("Query_Name"))
                        {
                            iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["Query_Name"]);
                        }
                        else if (ds.Tables[0].Columns.Contains("DMA_Name"))
                        {
                            iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["DMA_Name"]);
                        }
                        else if (ds.Tables[0].Columns.Contains("Province"))
                        {
                            iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["Province"]);
                        }

                        if (ds.Tables[0].Columns.Contains("MediaType"))
                        {
                            iQAgent_DaySummaryModel.MediaType = Convert.ToString(datarow["MediaType"]);
                        }

                        if (ds.Tables[0].Columns.Contains("SubMediaType"))
                        {
                            iQAgent_DaySummaryModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                        }

                        if (ds.Tables[0].Columns.Contains("NoOfDocs"))
                        {
                            iQAgent_DaySummaryModel.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                        }

                        if (ds.Tables[0].Columns.Contains("NoOfHits"))
                        {
                            iQAgent_DaySummaryModel.NoOfHits = Convert.ToInt64(datarow["NoOfHits"]);
                        }

                        if (ds.Tables[0].Columns.Contains("Audience"))
                        {
                            iQAgent_DaySummaryModel.Audience = Convert.ToInt64(datarow["Audience"]);
                        }

                        if (ds.Tables[0].Columns.Contains("IQMediaValue"))
                        {
                            iQAgent_DaySummaryModel.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                        }

                        if (ds.Tables[0].Columns.Contains("PositiveSentiment"))
                        {
                            iQAgent_DaySummaryModel.PositiveSentiment = Convert.ToInt64(datarow["PositiveSentiment"]);
                        }

                        if (ds.Tables[0].Columns.Contains("NegativeSentiment"))
                        {
                            iQAgent_DaySummaryModel.NegativeSentiment = Convert.ToInt64(datarow["NegativeSentiment"]);
                        }

                        lstIQAgent_DaySummaryModel.Add(iQAgent_DaySummaryModel);

                    }


                }
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }


        public IQAgent_DashBoardModel GetDaySummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, bool p_IsRadioAccess, string p_SearchRequestXml, bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_Isv4PQAccess)
        {

            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();
            
            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsRadioAccess", DbType.Boolean, p_IsRadioAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsTVAccess", DbType.Boolean, p_Isv4TVAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsNMAccess", DbType.Boolean, p_Isv4NMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsSMAccess", DbType.Boolean, p_Isv4SMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsTWAccess", DbType.Boolean, p_Isv4TWAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsBLPMAccess", DbType.Boolean, p_Isv4BLPMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsPQAccess", DbType.Boolean, p_Isv4PQAccess, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectByDay", dataTypeList);

            objIQAgent_DashBoardModel = FillIQAgentSummary(dsSSP, p_Medium);

            return objIQAgent_DashBoardModel;

        }

        public IQAgent_DashBoardModel GetDaySummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, bool p_IsRadioAccess, string p_SearchRequestXml, bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_Isv4PQAccess)
        {
            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsRadioAccess", DbType.Boolean, p_IsRadioAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsTVAccess", DbType.Boolean, p_Isv4TVAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsNMAccess", DbType.Boolean, p_Isv4NMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsSMAccess", DbType.Boolean, p_Isv4SMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsTWAccess", DbType.Boolean, p_Isv4TWAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsBLPMAccess", DbType.Boolean, p_Isv4BLPMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsPQAccess", DbType.Boolean, p_Isv4PQAccess, ParameterDirection.Input));
            

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectByMonth", dataTypeList);

            objIQAgent_DashBoardModel = FillIQAgentSummary(dsSSP, p_Medium);

            return objIQAgent_DashBoardModel;

        }

        public IQAgent_DashBoardModel GetHourSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, bool p_IsRadioAccess, string p_SearchRequestXml, bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_Isv4PQAccess)
        {
            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();

            List<DataType> dataTypeList = new List<DataType>();
            dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsRadioAccess", DbType.Boolean, p_IsRadioAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsTVAccess", DbType.Boolean, p_Isv4TVAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsNMAccess", DbType.Boolean, p_Isv4NMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsSMAccess", DbType.Boolean, p_Isv4SMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsTWAccess", DbType.Boolean, p_Isv4TWAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsBLPMAccess", DbType.Boolean, p_Isv4BLPMAccess, ParameterDirection.Input));
            dataTypeList.Add(new DataType("@IsPQAccess", DbType.Boolean, p_Isv4PQAccess, ParameterDirection.Input));

            Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
            DataSet dsSSP = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectByHour", dataTypeList);

            objIQAgent_DashBoardModel = FillIQAgentSummary(dsSSP, p_Medium);

            return objIQAgent_DashBoardModel;

        }

        public IQAgent_DashBoardModel FillIQAgentSummary(DataSet dsSSP, string p_Medium)
        {

            IQAgent_DashBoardModel objIQAgent_DashBoardModel = new IQAgent_DashBoardModel();

            objIQAgent_DashBoardModel.ListOfIQAgentSummary = new List<IQAgent_DaySummaryModel>();

            if (dsSSP != null && dsSSP.Tables.Count > 0)
            {
                foreach (DataRow datarow in dsSSP.Tables[0].Rows)
                {
                    IQAgent_DaySummaryModel iQAgent_DaySummaryModel = new IQAgent_DaySummaryModel();

                    if (dsSSP.Tables[0].Columns.Contains("DayDate") && !datarow["DayDate"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.DayDate = Convert.ToDateTime(datarow["DayDate"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("MediaType") && !datarow["MediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.MediaType = Convert.ToString(datarow["MediaType"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("NoOfHits") && !datarow["NoOfHits"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.NoOfHits = Convert.ToInt64(datarow["NoOfHits"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("IQMediaValue") && !datarow["IQMediaValue"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.Audience = Convert.ToInt64(datarow["Audience"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.PositiveSentiment = Convert.ToInt64(datarow["PositiveSentiment"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_DaySummaryModel.NegativeSentiment = Convert.ToInt64(datarow["NegativeSentiment"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("ID"))
                    {
                        iQAgent_DaySummaryModel.SearchRequestID = Convert.ToInt64(datarow["ID"]);
                    }

                    if (dsSSP.Tables[0].Columns.Contains("Query_Name"))
                    {
                        iQAgent_DaySummaryModel.Query_Name = Convert.ToString(datarow["Query_Name"]);
                    }

                    objIQAgent_DashBoardModel.ListOfIQAgentSummary.Add(iQAgent_DaySummaryModel);
                }
            }

            if (!string.IsNullOrEmpty(p_Medium) && dsSSP != null && dsSSP.Tables.Count > 1)
            {
                objIQAgent_DashBoardModel.ListOfTopStationBroadCast = new List<DashboardTopResultsModel>();
                foreach (DataRow datarow in dsSSP.Tables[1].Rows)
                {
                    DashboardTopResultsModel iQAgent_TVResultsBroadCastModel = new DashboardTopResultsModel();
                    if (dsSSP.Tables[1].Columns.Contains("IQ_Station_ID") && !datarow["IQ_Station_ID"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["IQ_Station_ID"]);
                    }
                    else if (dsSSP.Tables[1].Columns.Contains("CompeteURL") && !datarow["CompeteURL"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["CompeteURL"]);
                    }
                    else if (dsSSP.Tables[1].Columns.Contains("actor_preferredname") && !datarow["actor_preferredname"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["actor_preferredname"]);
                    }
                    else if (dsSSP.Tables[1].Columns.Contains("Publication") && !datarow["Publication"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["Publication"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Name = Convert.ToString(datarow["DMA_Name"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("DMA_Num") && !datarow["DMA_Num"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Num = Convert.ToString(datarow["DMA_Num"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("_IQDmaID") && !datarow["_IQDmaID"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel._IQDmaIDs = Convert.ToInt32(datarow["_IQDmaID"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NoOfDocs = Convert.ToInt32(datarow["NoOfDocs"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Mentions = Convert.ToInt64(datarow["Mentions"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("MediaValue") && !datarow["MediaValue"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.MediaValue = Convert.ToDecimal(datarow["MediaValue"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Audience = Convert.ToInt64(datarow["Audience"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("FriendsCount") && !datarow["FriendsCount"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.FriendsCount = Convert.ToInt64(datarow["FriendsCount"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.PositiveSentiment = Convert.ToInt32(datarow["PositiveSentiment"]);
                    }

                    if (dsSSP.Tables[1].Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NegativeSentiment = Convert.ToInt32(datarow["NegativeSentiment"]);
                    }

                    objIQAgent_DashBoardModel.ListOfTopStationBroadCast.Add(iQAgent_TVResultsBroadCastModel);
                }
            }

            if (!string.IsNullOrEmpty(p_Medium) && dsSSP != null && dsSSP.Tables.Count > 2 && 
                    (p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString() ||
                     p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.NM.ToString() ||
                     p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.PM.ToString())
                )
            {
                objIQAgent_DashBoardModel.ListOfTopDMABroadCast = new List<DashboardTopResultsModel>();
                foreach (DataRow datarow in dsSSP.Tables[2].Rows)
                {
                    DashboardTopResultsModel iQAgent_TVResultsBroadCastModel = new DashboardTopResultsModel();
                    if (dsSSP.Tables[2].Columns.Contains("Author") && !datarow["Author"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Outlet_Name = Convert.ToString(datarow["Author"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Name = Convert.ToString(datarow["DMA_Name"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("DMA_Num") && !datarow["DMA_Num"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.DMA_Num = Convert.ToString(datarow["DMA_Num"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("_IQDmaID") && !datarow["_IQDmaID"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel._IQDmaIDs = Convert.ToInt32(datarow["_IQDmaID"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NoOfDocs = Convert.ToInt32(datarow["NoOfDocs"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Mentions = Convert.ToInt64(datarow["Mentions"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("MediaValue") && !datarow["MediaValue"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.MediaValue = Convert.ToDecimal(datarow["MediaValue"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Audience = Convert.ToInt64(datarow["Audience"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.PositiveSentiment = Convert.ToInt32(datarow["PositiveSentiment"]);
                    }

                    if (dsSSP.Tables[2].Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NegativeSentiment = Convert.ToInt32(datarow["NegativeSentiment"]);
                    }

                    objIQAgent_DashBoardModel.ListOfTopDMABroadCast.Add(iQAgent_TVResultsBroadCastModel);
                }
            }

            if (!string.IsNullOrEmpty(p_Medium) && dsSSP != null && dsSSP.Tables.Count > 3 && (p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString() || p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.NM.ToString()))
            {
                objIQAgent_DashBoardModel.DmaMentionMapList = new Dictionary<string, long>();
                foreach (DataRow datarow in dsSSP.Tables[3].Rows)
                {

                    string key = string.Empty;
                    string dma = string.Empty;
                    long mention = 0;

                    if (dsSSP.Tables[3].Columns.Contains("DMA_Name") && !datarow["DMA_Name"].Equals(DBNull.Value))
                    {
                        dma = Convert.ToString(datarow["DMA_Name"]);
                    }

                    if (dsSSP.Tables[3].Columns.Contains("DMA_Num") && !datarow["DMA_Num"].Equals(DBNull.Value))
                    {
                        key = Convert.ToString(datarow["DMA_Num"]);
                    }

                    if (dsSSP.Tables[3].Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        mention = Convert.ToInt64(datarow["Mentions"]);
                    }

                    objIQAgent_DashBoardModel.DmaMentionMapList.Add(dma, mention);
                }
            }

            if (p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString() && dsSSP != null && dsSSP.Tables.Count > 4)
            {
                objIQAgent_DashBoardModel.ListOfTopCountryBroadCast = new List<DashboardTopResultsModel>();
                foreach (DataRow datarow in dsSSP.Tables[4].Rows)
                {
                    DataTable dt = dsSSP.Tables[4];
                    DashboardTopResultsModel iQAgent_TVResultsBroadCastModel = new DashboardTopResultsModel();

                    if (dt.Columns.Contains("Country") && !datarow["Country"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Country = Convert.ToString(datarow["Country"]);
                    }

                    if (dt.Columns.Contains("Country_Num") && !datarow["Country_Num"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Country_Num = Convert.ToString(datarow["Country_Num"]);
                    }

                    if (dt.Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NoOfDocs = Convert.ToInt32(datarow["NoOfDocs"]);
                    }

                    if (dt.Columns.Contains("Mentions") && !datarow["Mentions"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.Mentions = Convert.ToInt64(datarow["Mentions"]);
                    }

                    if (dt.Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.PositiveSentiment = Convert.ToInt32(datarow["PositiveSentiment"]);
                    }

                    if (dt.Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                    {
                        iQAgent_TVResultsBroadCastModel.NegativeSentiment = Convert.ToInt32(datarow["NegativeSentiment"]);
                    }

                    objIQAgent_DashBoardModel.ListOfTopCountryBroadCast.Add(iQAgent_TVResultsBroadCastModel);
                }
            }

            if (!string.IsNullOrEmpty(p_Medium) && dsSSP != null && 
                (
                    (dsSSP.Tables.Count > 5 && p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString()) ||
                    (dsSSP.Tables.Count > 4 && p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.NM.ToString())
                )
               )
            {
                DataTable dt;
                if (p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString())
                {
                    dt = dsSSP.Tables[5];
                }
                else
                {
                    dt = dsSSP.Tables[4];
                }

                objIQAgent_DashBoardModel.CanadaMentionMapList = new Dictionary<string, long>();
                foreach (DataRow dr in dt.Rows)
                {
                    string province = String.Empty;
                    long mentions = 0;

                    if (dt.Columns.Contains("Province") && !dr["Province"].Equals(DBNull.Value))
                    {
                        province = Convert.ToString(dr["Province"]);
                    }
                    if (dt.Columns.Contains("Mentions") && !dr["Mentions"].Equals(DBNull.Value))
                    {
                        mentions = Convert.ToInt64(dr["Mentions"]);
                    }

                    objIQAgent_DashBoardModel.CanadaMentionMapList.Add(province, mentions);
                }
            }

            if (dsSSP != null)
            {
                objIQAgent_DashBoardModel.PrevIQAgentSummary = new IQAgent_DashBoardPrevSummaryModel();
                DataTable dtPrevDashboardSummary = new DataTable();
                if (string.IsNullOrEmpty(p_Medium) && dsSSP.Tables.Count > 1 && dsSSP.Tables[1].Rows.Count > 0)
                {
                    dtPrevDashboardSummary = dsSSP.Tables[1];
                    objIQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData = true;
                }
                else if (p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString() || p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.NM.ToString() || p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.PM.ToString())
                {
                    if (dsSSP.Tables.Count > 6 && dsSSP.Tables[6].Rows.Count > 0)
                    {
                        // TV
                        dtPrevDashboardSummary = dsSSP.Tables[6];
                        objIQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData = true;
                    }
                    else if (dsSSP.Tables.Count > 5 && dsSSP.Tables[5].Rows.Count > 0)
                    {
                        // NM
                        dtPrevDashboardSummary = dsSSP.Tables[5];
                        objIQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData = true;
                    }
                    else if (dsSSP.Tables.Count > 3 && dsSSP.Tables[3].Rows.Count > 0)
                    {
                        // PM
                        dtPrevDashboardSummary = dsSSP.Tables[3];
                        objIQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData = true;
                    }
                }
                else if ((p_Medium != Shared.Utility.CommonFunctions.DashBoardMediumType.TV.ToString() && p_Medium != Shared.Utility.CommonFunctions.DashBoardMediumType.NM.ToString() && p_Medium == Shared.Utility.CommonFunctions.DashBoardMediumType.PM.ToString()) && dsSSP.Tables.Count > 2)
                {
                    dtPrevDashboardSummary = dsSSP.Tables[2];
                    objIQAgent_DashBoardModel.PrevIQAgentSummary.IsEnoughData = true;
                }
                
                
                if (dtPrevDashboardSummary.Rows.Count > 0)
                {
                    List<IQAgent_ComparisionValues> ListOfIQAgent_ComparisionValues = new List<IQAgent_ComparisionValues>();
                    foreach (DataRow datarow in dtPrevDashboardSummary.Rows)
                    {
                        IQAgent_ComparisionValues objIQAgent_ComparisionValues = new IQAgent_ComparisionValues();
                        if (dtPrevDashboardSummary.Columns.Contains("NoOfDocs") && !datarow["NoOfDocs"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.NoOfDocs = Convert.ToInt64(datarow["NoOfDocs"]);
                            objIQAgent_ComparisionValues.TotalAirSeconds = Convert.ToInt64(datarow["NoOfDocs"]) * 8;
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("NoOfHits") && !datarow["NoOfHits"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.NoOfHits = Convert.ToInt64(datarow["NoOfHits"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("IQMediaValue") && !datarow["IQMediaValue"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.IQMediaValue = Convert.ToDecimal(datarow["IQMediaValue"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("Audience") && !datarow["Audience"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.Audience = Convert.ToInt64(datarow["Audience"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("PositiveSentiment") && !datarow["PositiveSentiment"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.PositiveSentiment = Convert.ToInt64(datarow["PositiveSentiment"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("NegativeSentiment") && !datarow["NegativeSentiment"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.NegativeSentiment = Convert.ToInt64(datarow["NegativeSentiment"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("MediaType") && !datarow["MediaType"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.MediaType = Convert.ToString(datarow["MediaType"]);
                        }

                        if (dtPrevDashboardSummary.Columns.Contains("SubMediaType") && !datarow["SubMediaType"].Equals(DBNull.Value))
                        {
                            objIQAgent_ComparisionValues.SubMediaType = Convert.ToString(datarow["SubMediaType"]);
                        }

                        ListOfIQAgent_ComparisionValues.Add(objIQAgent_ComparisionValues);
                    }

                    objIQAgent_DashBoardModel.PrevIQAgentSummary.ListOfIQAgentPrevSummary = ListOfIQAgent_ComparisionValues;
                }
            }

            return objIQAgent_DashBoardModel;
        }


        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaXml", DbType.Xml, p_DmaXml, ParameterDirection.Input));


                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectDmaSummaryByDay", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataDayWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProvinceXml", DbType.Xml, p_ProvinceXml, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectProvinceSummaryByDay", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaXml", DbType.Xml, p_DmaXml, ParameterDirection.Input));


                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectDmaSummaryByMonth", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataMonthWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProvinceXml", DbType.Xml, p_ProvinceXml, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_DaySummary_SelectProvinceSummaryByMonth", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetDmaSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_DmaXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@DmaXml", DbType.Xml, p_DmaXml, ParameterDirection.Input));


                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectDmaSummaryByHour", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IQAgent_DaySummaryModel> GetProvinceSummaryDataHourWise(Guid p_ClientGUID, DateTime p_FromDate, DateTime p_ToDate, string p_Medium, string p_SearchRequestXml, string p_ProvinceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@FromDate", DbType.Date, p_FromDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ToDate", DbType.Date, p_ToDate, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@SearchRequestIDXml", DbType.Xml, p_SearchRequestXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@ProvinceXml", DbType.Xml, p_ProvinceXml, ParameterDirection.Input));

                Dictionary<string, object> dicSummaryReport = new Dictionary<string, object>();
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQAgent_HourSummary_SelectProvinceSummaryByHour", dataTypeList);

                List<IQAgent_DaySummaryModel> lstIQAgent_DaySummaryModel = FillIQAgent_DaySummaryModel(ds);
                return lstIQAgent_DaySummaryModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQAgent_DashBoardModel GetAdhocSummaryData(string p_MediaIDXml, string p_Source, string p_Medium)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@MediaIDXml", DbType.Xml, p_MediaIDXml, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Source", DbType.String, p_Source, ParameterDirection.Input));
                dataTypeList.Add(new DataType("@Medium", DbType.String, p_Medium, ParameterDirection.Input));
                DataSet ds = DataAccess.GetDataSet("usp_v4_IQDashboard_AdhocSummary_SelectByID", dataTypeList);

                return FillIQAgentSummary(ds, p_Medium);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
