using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;
using System.IO;
using System.Web;

namespace IQMediaGroup.Controller.Implementation
{
    internal class StatSkedProgController : IStatSkedProgController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IStatSkedProgModel _IStatSkedProgModel;

        public StatSkedProgController()
        {
            _IStatSkedProgModel = _ModelFactory.CreateObject<IStatSkedProgModel>();
        }

        /// <summary>
        /// Description: This Methods gets Program Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_Program">Dataset for Program</param>
        /// <returns>List of object of Program</returns>
        public MasterStatSkedProg GetAllDetail()
        {
            DataSet _DataSet = null;


            MasterStatSkedProg _MasterStatSkedProg = new MasterStatSkedProg();
            try
            {
                _DataSet = _IStatSkedProgModel.GetAllDetail();
                _MasterStatSkedProg._ListofMarket = FillMarketInformation(_DataSet);
                _MasterStatSkedProg._ListofType = FillProgramTypeInformation(_DataSet);
                _MasterStatSkedProg._ListofAffil = FillAffilInformation(_DataSet);

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _MasterStatSkedProg;
        }

        /// <summary>
        /// Description: This Methods Fills Program Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="_DataSet">Dataset for Program Infromarmation</param>
        /// <returns>List of object of Program Information</returns>
        private List<SSP_IQ_Dma_Name> FillMarketInformation(DataSet _DataSet)
        {
            List<SSP_IQ_Dma_Name> _ListOfMarketInformation = new List<SSP_IQ_Dma_Name>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        SSP_IQ_Dma_Name _SSP_IQ_Dma_Name = new SSP_IQ_Dma_Name();
                        _SSP_IQ_Dma_Name.IQ_Dma_Name = Convert.ToString(_DataRow["Dma_Name"]);
                        _SSP_IQ_Dma_Name.IQ_Dma_Num = Convert.ToString(_DataRow["Dma_Num"]);
                        if (_DataSet.Tables[0].Columns.Contains("_RegionID"))
                        {
                            _SSP_IQ_Dma_Name._RegionID = Convert.ToInt32(!string.IsNullOrEmpty(Convert.ToString(_DataRow["_RegionID"])) ? _DataRow["_RegionID"] : 0); 
                        }
                        _ListOfMarketInformation.Add(_SSP_IQ_Dma_Name);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfMarketInformation;
        }

        /// <summary>
        /// Description:This Methods Fills Program Type Information from DataSet.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Program Type Infromarmation</param>
        /// <returns>List of object of Program Type Information</returns>
        private List<SSP_IQ_Class> FillProgramTypeInformation(DataSet _DataSet)
        {
            List<SSP_IQ_Class> _ListOfProgramTypeInformation = new List<SSP_IQ_Class>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[1].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[1].Rows)
                    {
                        SSP_IQ_Class _SSP_IQ_Class = new SSP_IQ_Class();

                        _SSP_IQ_Class.IQ_Class = Convert.ToString(_DataRow["IQ_Class"]);
                        _SSP_IQ_Class.IQ_Class_Num = Convert.ToString(_DataRow["IQ_Class_Num"]);

                        _ListOfProgramTypeInformation.Add(_SSP_IQ_Class);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfProgramTypeInformation;
        }

        /// <summary>
        /// Description:This Methods Fills Affil Information from DataSet.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for Affil Infromarmation</param>
        /// <returns>List of object of Affil Information</returns>
        private List<SSP_Station_Affil> FillAffilInformation(DataSet _DataSet)
        {
            List<SSP_Station_Affil> _ListOfAffilTypeInformation = new List<SSP_Station_Affil>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[2].Rows)
                    {
                        SSP_Station_Affil _SSP_Station_Affil = new SSP_Station_Affil();

                        if (_DataSet.Tables[2].Columns.Contains("IQ_Station_ID"))
                        {
                            _SSP_Station_Affil.IQ_Station_ID = Convert.ToString(_DataRow["IQ_Station_ID"]);
                        }
                        if (_DataSet.Tables[2].Columns.Contains("Station_Affil"))
                        {
                            _SSP_Station_Affil.Station_Affil = Convert.ToString(_DataRow["Station_Affil"]);
                        }
                        if (_DataSet.Tables[2].Columns.Contains("Station_Affil_Num"))
                        {
                            _SSP_Station_Affil.Station_Affil_Num = Convert.ToString(_DataRow["Station_Affil_Num"]);
                        }

                        _ListOfAffilTypeInformation.Add(_SSP_Station_Affil);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfAffilTypeInformation;
        }

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <param name="p_TotalRowCount">TotalRowCount</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailsByString(StatSkedProg _StatSkedProg, out int p_TotalRowCount)
        {
            DataSet _DataSet = null;

            List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();
            try
            {
                _DataSet = _IStatSkedProgModel.GetDetailsByString(_StatSkedProg);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                p_TotalRowCount = 0;

                if (_DataSet != null && _DataSet.Tables.Count > 1 && _DataSet.Tables[1] != null && _DataSet.Tables[1].Rows.Count > 0)
                {
                    if (_DataSet.Tables[1].Columns.Contains("TotalRowCount"))
                    {
                        if (_DataSet.Tables[1].Rows[0]["TotalRowCount"] != null)
                        {
                            p_TotalRowCount = Convert.ToInt32(_DataSet.Tables[1].Rows[0]["TotalRowCount"].ToString());
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStatSkedProg;
        }

        public List<StatSkedProg> GetStatskedprogByParams(StatSkedProg _StatSkedProg, out int p_TotalRowCount)
        {
            try
            {
                DataSet _DataSet = null;

                List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();

                _DataSet = _IStatSkedProgModel.GetStatskedprogByParams(_StatSkedProg);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                p_TotalRowCount = 0;

                if (_DataSet != null && _DataSet.Tables.Count > 1 && _DataSet.Tables[1] != null && _DataSet.Tables[1].Rows.Count > 0)
                {
                    if (_DataSet.Tables[1].Columns.Contains("TotalRowCount"))
                    {
                        if (_DataSet.Tables[1].Rows[0]["TotalRowCount"] != null)
                        {
                            p_TotalRowCount = Convert.ToInt32(_DataSet.Tables[1].Rows[0]["TotalRowCount"].ToString());
                        }
                    }
                }

                return _ListOfStatSkedProg;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <param name="p_TotalRowCount">TotalRowCount</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailsForDefaultSearch(StatSkedProg _StatSkedProg, out int p_TotalRowCount)
        {
            DataSet _DataSet = null;

            List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();
            try
            {
                _DataSet = _IStatSkedProgModel.GetDetailsForDefaultSearch(_StatSkedProg);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                p_TotalRowCount = 0;

                if (_DataSet != null && _DataSet.Tables.Count > 1 && _DataSet.Tables[1] != null && _DataSet.Tables[1].Rows.Count > 0)
                {
                    if (_DataSet.Tables[1].Columns.Contains("TotalRowCount"))
                    {
                        if (_DataSet.Tables[1].Rows[0]["TotalRowCount"] != null)
                        {
                            p_TotalRowCount = Convert.ToInt32(_DataSet.Tables[1].Rows[0]["TotalRowCount"].ToString());
                        }
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStatSkedProg;
        }

        /// <summary>
        /// Description:This Methods gets StatSkedProg details By String With Time.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_StatSkedProg">object of StatSkedProg</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailsByStringWithTime(StatSkedProg _StatSkedProg)
        {
            DataSet _DataSet = null;

            List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();
            try
            {
                _DataSet = _IStatSkedProgModel.GetDetailsByString(_StatSkedProg);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStatSkedProg;
        }

        /// <summary>
        /// Description:This Methods Fill StatSkedProg details.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for StatSkedProg Infromarmation</param>
        /// <returns>List of object of StatSkedProg</returns>
        private List<StatSkedProg> FillAllInformation(DataSet _DataSet)
        {
            List<StatSkedProg> _ListOfInformation = new List<StatSkedProg>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        StatSkedProg _StatSkedProg = new StatSkedProg();

                        if (_DataSet.Tables[0].Columns.Contains("Station_ID") && !_DataRow["Station_ID"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.Station_ID = Convert.ToString(_DataRow["Station_ID"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Local_Air_Date") && !_DataRow["IQ_Local_Air_Date"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.IQ_Local_Air_Date = Convert.ToDateTime(_DataRow["IQ_Local_Air_Date"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Local_Air_Time") && !_DataRow["IQ_Local_Air_Time"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.IQ_Local_Air_Time = _DataRow["IQ_Local_Air_Time"].ToString();
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Dma_Num") && !_DataRow["IQ_Dma_Num"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.IQ_Dma_Num = Convert.ToString(_DataRow["IQ_Dma_Num"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_Dma_Name") && !_DataRow["IQ_Dma_Name"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.IQ_Dma_Name = Convert.ToString(_DataRow["IQ_Dma_Name"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("Title120") && !_DataRow["Title120"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.Title120 = Convert.ToString(_DataRow["Title120"]);
                        }

                        if (_DataSet.Tables[0].Columns.Contains("RL_GUID") && !_DataRow["RL_GUID"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.RL_GUID = _DataRow["RL_GUID"].ToString();
                        }

                        if (_DataSet.Tables[0].Columns.Contains("IQ_CC_Key") && !_DataRow["IQ_CC_Key"].Equals(DBNull.Value))
                        {
                            _StatSkedProg.IQ_CC_Key = _DataRow["IQ_CC_Key"].ToString();
                        }

                        _ListOfInformation.Add(_StatSkedProg);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfInformation;


        }

        public List<StatSkedProg> GetAllIQCCKeyByStatskedprogParams(StatSkedProg p_StatSkedProg)
        {
            try
            {
                DataSet _DataSet = null;


                _DataSet = _IStatSkedProgModel.GetAllIQCCKeyByStatskedprogParams(p_StatSkedProg);

                List<StatSkedProg> _ListOfStatSkedProg = FillAllInformation(_DataSet);

                return _ListOfStatSkedProg;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Methods get StatSkedProg details by GUIDs.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_GUIDs">p_GUID</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailByGUIDs(string p_GUIDs)
        {
            try
            {
                DataSet _DataSet = null;

                List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();

                _DataSet = _IStatSkedProgModel.GetDetailByGUIDs(p_GUIDs);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                return _ListOfStatSkedProg;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Methods get StatSkedProg details by GUIDs.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_GUIDs">p_GUID</param>
        /// <returns>List of object of StatSkedProg</returns>
        public List<StatSkedProg> GetDetailByIQCCKeys(string p_IQCCKeys)
        {
            try
            {
                DataSet _DataSet = null;

                List<StatSkedProg> _ListOfStatSkedProg = new List<StatSkedProg>();

                _DataSet = _IStatSkedProgModel.GetDetailByIQCCKeys(p_IQCCKeys);

                _ListOfStatSkedProg = FillAllInformation(_DataSet);

                return _ListOfStatSkedProg;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// Description:This Methods get Raw Media details by GUIDs.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ListOfRawMedia"></param>
        /// <returns></returns>
        public List<RawMedia> GetRawMediaByGUIDs(List<RawMedia> p_ListOfRawMedia)
        {
            try
            {
                string _GUIDs = string.Empty;

                foreach (RawMedia _RawMedia in p_ListOfRawMedia)
                {
                    if (string.IsNullOrEmpty(_GUIDs))
                    {
                        _GUIDs = CommonConstants.SingleQuote + _RawMedia.RawMediaID.ToString() + CommonConstants.SingleQuote;
                    }
                    else
                    {
                        _GUIDs = _GUIDs + CommonConstants.Comma + CommonConstants.SingleQuote + _RawMedia.RawMediaID.ToString() + CommonConstants.SingleQuote;
                    }
                }

                List<StatSkedProg> _ListOfStatSkedProg = GetDetailByGUIDs(_GUIDs);

                foreach (RawMedia _RawMedia in p_ListOfRawMedia)
                {
                    StatSkedProg _StatSkedProg = _ListOfStatSkedProg.Find(delegate(StatSkedProg _StatSkedProgTemp) { return _StatSkedProgTemp.RL_GUID.ToLower() == _RawMedia.RawMediaID.ToString().ToLower(); });

                    if (_StatSkedProg != null)
                    {
                        _RawMedia.StationID = _StatSkedProg.Station_ID;

                        _RawMedia.Title120 = _StatSkedProg.Title120;

                        if (File.Exists(HttpContext.Current.Server.MapPath("~/StationLogoImages/" + _StatSkedProg.Station_ID + ".gif")))
                        {
                            _RawMedia.StationLogo = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _StatSkedProg.Station_ID + ".gif";
                        }
                        else if (File.Exists(HttpContext.Current.Server.MapPath("~/StationLogoImages/" + _StatSkedProg.Station_ID + ".jpg")))
                        {
                            _RawMedia.StationLogo = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _StatSkedProg.Station_ID + ".jpg";
                        }
                    }
                }

                return p_ListOfRawMedia;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Methods get Raw Media details by GUIDs.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ListOfRawMedia"></param>
        /// <returns></returns>
        public List<RawMedia> GetRawMediaByIQCCKeys(List<RawMedia> p_ListOfRawMedia)
        {
            try
            {
                string _IQCCKeys = string.Join(",", p_ListOfRawMedia.Select(RawMediaObj => "'" + RawMediaObj.IQ_CC_Key + "'").ToArray());

                List<StatSkedProg> _ListOfStatSkedProg = GetDetailByIQCCKeys(_IQCCKeys);

                foreach (RawMedia _RawMedia in p_ListOfRawMedia)
                {
                    StatSkedProg _StatSkedProg = _ListOfStatSkedProg.Find(delegate(StatSkedProg _StatSkedProgTemp) { return _StatSkedProgTemp.IQ_CC_Key.ToLower() == _RawMedia.IQ_CC_Key.ToString().ToLower(); });

                    if (_StatSkedProg != null)
                    {
                        _RawMedia.Title120 = _StatSkedProg.Title120;
                    }
                }

                return p_ListOfRawMedia;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public MasterStatSkedProg GetAllDetailByClientSettings(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed)
        {
            try
            {
                DataSet _DataSet = null;
                MasterStatSkedProg _MasterStatSkedProg = new MasterStatSkedProg();

                _DataSet = _IStatSkedProgModel.GetAllDetailByClientSettings(p_ClientGUID, out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);
                _MasterStatSkedProg._ListofMarket = FillMarketInformation(_DataSet);
                _MasterStatSkedProg._ListofType = FillProgramTypeInformation(_DataSet);
                _MasterStatSkedProg._ListofAffil = FillAffilInformation(_DataSet);

                return _MasterStatSkedProg;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

        public MasterStatSkedProg GetAllDetailWithRegion(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed)
        {
            try
            {
                DataSet _DataSet = null;
                MasterStatSkedProg _MasterStatSkedProg = new MasterStatSkedProg();

                _DataSet = _IStatSkedProgModel.GetAllDetailWithRegion(p_ClientGUID, out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);
                _MasterStatSkedProg._ListofMarket = FillMarketInformation(_DataSet);
                _MasterStatSkedProg._ListofType = FillProgramTypeInformation(_DataSet);
                _MasterStatSkedProg._ListofAffil = FillAffilInformation(_DataSet);

                return _MasterStatSkedProg;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

    }
}
