using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using System.IO;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlTypes;

namespace IQMediaGroup.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRL_GUIDS
    /// </summary>
    internal class RL_GUIDSController : IRL_GUIDSController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IRL_GUIDSModel _IRL_GUIDSModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public RL_GUIDSController()
        {
            _IRL_GUIDSModel = _ModelFactory.CreateObject<IRL_GUIDSModel>();
        }


        /// <summary> 
        /// This method gets all RL_GUIDS details 
        /// </summary> 
        /// <returns> List Of RL_GUIDS </returns> 

        public List<RL_GUIDS> GetAllRL_GUIDS()
        {
            try
            {
                DataSet _DataSet = null;

                List<RL_GUIDS> _ListOfRL_GUIDS = new List<RL_GUIDS>();

                _DataSet = _IRL_GUIDSModel.GetAllRL_GUIDS();
                _ListOfRL_GUIDS = FillRL_GUIDS(_DataSet);

                return _ListOfRL_GUIDS;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary> 
        /// This method will feill RL_GUIDS summary 
        /// </summary> 
        /// <returns> List Of RL_GUIDS </returns> 

        public List<RL_GUIDS> FillRL_GUIDS(DataSet p_DataSet)
        {
            List<RL_GUIDS> _ListofRL_GUIDS = new List<RL_GUIDS>();

            if (p_DataSet != null && p_DataSet.Tables.Count > 0)
            {
                DataTable _DataTable = p_DataSet.Tables[0];

                foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                {
                    RL_GUIDS _RL_GUIDS = new RL_GUIDS();

                    if (_DataTable.Columns.Contains("RL_GUIDSKey"))
                    {
                        _RL_GUIDS.RL_GUIDSKey = Convert.ToInt64(_DataRow["RL_GUIDSKey"]);
                    }

                    if (_DataTable.Columns.Contains("RL_Station_ID"))
                    {
                        _RL_GUIDS.RL_Station_ID = Convert.ToString(_DataRow["RL_Station_ID"]);
                    }

                    if (_DataTable.Columns.Contains("RL_Station_Date"))
                    {
                        _RL_GUIDS.RL_Station_Date = Convert.ToDateTime(_DataRow["RL_Station_Date"]);
                    }

                    if (_DataTable.Columns.Contains("RL_Station_Time"))
                    {
                        _RL_GUIDS.RL_Station_Time = Convert.ToInt32(_DataRow["RL_Station_Time"]);
                    }

                    if (_DataTable.Columns.Contains("RL_Time_zone"))
                    {
                        _RL_GUIDS.RL_Time_zone = Convert.ToString(_DataRow["RL_Time_zone"]);
                    }

                    if (_DataTable.Columns.Contains("RL_GUID"))
                    {
                        _RL_GUIDS.RL_GUID = Convert.ToString(_DataRow["RL_GUID"]);
                    }

                    if (_DataTable.Columns.Contains("GMT_Date"))
                    {
                        _RL_GUIDS.GMT_Date = Convert.ToDateTime(_DataRow["GMT_Date"]);
                    }

                    if (_DataTable.Columns.Contains("GMT_Time"))
                    {
                        _RL_GUIDS.GMT_Time = Convert.ToInt32(_DataRow["GMT_Time"]);
                    }

                    if (_DataTable.Columns.Contains("IQ_CC_Key"))
                    {
                        _RL_GUIDS.IQ_CC_Key = Convert.ToString(_DataRow["IQ_CC_Key"]);
                    }

                    if (_DataTable.Columns.Contains("GUID_Status"))
                    {
                        _RL_GUIDS.GUID_Status = Convert.ToBoolean(_DataRow["GUID_Status"]);
                    }

                    _ListofRL_GUIDS.Add(_RL_GUIDS);
                }
            }

            return _ListofRL_GUIDS;
        }


        /// <summary> 
        /// This method updates RL_GUIDS details 
        /// </summary> 
        /// <param name="p_RL_GUIDS">Object of RL_GUIDS</param> 
        /// <returns>1/0-Updated or not</returns> 

        public string UpdateRL_GUIDS(RL_GUIDS p_RL_GUIDS)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_GUIDSModel.UpdateRL_GUIDS(p_RL_GUIDS);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


        /// <summary> 
        /// This method deletes RL_GUIDS 
        /// </summary> 
        /// <param name="p_RL_GUIDSKey"> RL_GUIDSKey</param> 
        /// <returns>1/0-Deleted or not</returns> 

        public string DeleteRL_GUIDS(string p_RL_GUIDSKey)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_GUIDSModel.DeleteRL_GUIDS(p_RL_GUIDSKey);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets RL_GUIDS Records by IQ_CC_Key
        /// </summary>
        /// <returns>DataSet contains RL_GUIDS detail</returns>
        public List<RL_GUIDS> GetRL_GUIDSByIQCCKey(string p_IQ_CC_Key)
        {
            try
            {
                List<RL_GUIDS> _ListOfRL_GUIDS = null;
                DataSet _DataSet = null;

                _DataSet = _IRL_GUIDSModel.GetRL_GUIDSByIQCCKey(p_IQ_CC_Key);

                _ListOfRL_GUIDS = FillRL_GUIDS(_DataSet);

                return _ListOfRL_GUIDS;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets all RL_GUIDs by StatSkedProgParams
        /// </summary>
        /// <param name="p_StatSkedProg">Object of StatSkedProg</param>
        /// <returns>List of objects of RL_GUIDs</returns>
        public List<RL_GUIDS> GetAllRL_GUIDSByStatskedprogParams(StatSkedProg p_StatSkedProg)
        {
            try
            {
                DataSet _DataSet = null;


                _DataSet = _IRL_GUIDSModel.GetAllRL_GUIDSByStatskedprogParams(p_StatSkedProg);

                List<RL_GUIDS> _ListOfRL_GUIDS = FillRL_GUIDS(_DataSet);

                return _ListOfRL_GUIDS;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<RadioStation> GetAllRL_GUIDSByRadioStations(string p_IQCCKey, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc, out Int64 p_TotalRecordsCount)
        {
            try
            {
                List<RadioStation> _ListOfRadioStation = null;
                DataSet _DataSet = null;
                p_TotalRecordsCount = 0;

                _DataSet = _IRL_GUIDSModel.GetAllRL_GUIDSByRadioStations(p_IQCCKey, p_PageNumber, p_PageSize, p_SortField, p_IsSortDirectionAsc);

                _ListOfRadioStation = FillRadioStations(_DataSet);

                if (_DataSet != null && _DataSet.Tables.Count > 1)
                {
                    p_TotalRecordsCount = Convert.ToInt64(_DataSet.Tables[1].Rows[0][0].ToString());
                }

                return _ListOfRadioStation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public List<RadioStation> GetAllRL_GUIDSByRadioStationsByXML(SqlXml p_IQStationIDXML, int p_PageNumber, int p_PageSize, string p_SortField, bool p_IsSortDirectionAsc, DateTime p_FromDate, DateTime p_ToDate, out Int64 p_TotalRecordsCount)
        {
            try
            {
                List<RadioStation> _ListOfRadioStation = null;
                DataSet _DataSet = null;
                p_TotalRecordsCount = 0;

                _DataSet = _IRL_GUIDSModel.GetAllRL_GUIDSByRadioStationsByXML(p_IQStationIDXML, p_PageNumber, p_PageSize, p_SortField, p_IsSortDirectionAsc, p_FromDate, p_ToDate);

                _ListOfRadioStation = FillRadioStations(_DataSet);

                if (_DataSet != null && _DataSet.Tables.Count > 1)
                {
                    p_TotalRecordsCount = Convert.ToInt64(_DataSet.Tables[1].Rows[0][0].ToString());
                }

                return _ListOfRadioStation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<RadioStation> FillRadioStations(DataSet p_DataSet)
        {
            try
            {
                List<RadioStation> _ListOfRadioStation = new List<RadioStation>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    DataTable _DataTable = p_DataSet.Tables[0];

                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        RadioStation _RadioStation = new RadioStation();

                        if (_DataTable.Columns.Contains("RL_GUID"))
                        {
                            _RadioStation.RawMediaID = new Guid(_DataRow["RL_GUID"].ToString());
                        }

                        if (_DataTable.Columns.Contains("RL_Station_ID"))
                        {
                            _RadioStation.RL_Station_ID = Convert.ToString(_DataRow["RL_Station_ID"]);

                            if (File.Exists(HttpContext.Current.Server.MapPath("~/StationLogoImages/" + _RadioStation.RL_Station_ID + ".gif")))
                            {
                                _RadioStation.StationLogo = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _RadioStation.RL_Station_ID + ".gif";
                            }
                            else if (File.Exists(HttpContext.Current.Server.MapPath("~/StationLogoImages/" + _RadioStation.RL_Station_ID + ".jpg")))
                            {
                                _RadioStation.StationLogo = "http://" + HttpContext.Current.Request.ServerVariables["HTTP_HOST"] + "/StationLogoImages/" + _RadioStation.RL_Station_ID + ".jpg";
                            }
                        }

                        if (_DataTable.Columns.Contains("RL_Station_Date") && _DataTable.Columns.Contains("RL_Station_Time"))
                        {
                            DateTime _TempDateTime = Convert.ToDateTime(_DataRow["RL_Station_Date"].ToString());

                            _RadioStation.RawMediaDateTime = new DateTime(_TempDateTime.Year, _TempDateTime.Month, _TempDateTime.Day, Convert.ToInt32(_DataRow["RL_Station_Time"].ToString()) / 100, 0, 0);
                        }

                        if (_DataTable.Columns.Contains("dma_name"))
                        {
                            _RadioStation.dma_name = _DataRow["dma_name"].ToString();
                        }

                        _ListOfRadioStation.Add(_RadioStation);
                    }
                }

                return _ListOfRadioStation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}