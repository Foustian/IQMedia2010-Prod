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

namespace IQMediaGroup.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRL_STATION
    /// </summary>
    internal class IQ_STATIONController : IIQ_STATIONController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIQ_STATIONModel _IIQ_STATIONModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public IQ_STATIONController()
        {
            _IIQ_STATIONModel = _ModelFactory.CreateObject<IIQ_STATIONModel>();
        }

        /// <summary>
        /// This method gets all RadioStations
        /// </summary>
        /// <returns>List of objects of RL_Station</returns>
        public List<IQ_STATION> SelectAllRadioStations()
        {
            try
            {
                DataSet _DataSet = null;

                List<IQ_STATION> _ListOfRL_STATION = new List<IQ_STATION>();

                _DataSet = _IIQ_STATIONModel.SelectRadioStations();
                _ListOfRL_STATION = FillRL_STATION(_DataSet);

                return _ListOfRL_STATION;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary> 
        /// This method will feill RL_STATION summary 
        /// </summary> 
        /// <returns> List Of RL_STATION </returns> 
        public List<IQ_STATION> FillRL_STATION(DataSet p_DataSet)
        {
            List<IQ_STATION> _ListofRL_STATION = new List<IQ_STATION>();

            if (p_DataSet != null && p_DataSet.Tables.Count > 0)
            {
                DataTable _DataTable = p_DataSet.Tables[0];

                foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                {
                    IQ_STATION _RL_STATION = new IQ_STATION();

                    
                    if (_DataTable.Columns.Contains("IQ_Station_ID"))
                    {
                        _RL_STATION.IQ_Station_ID = Convert.ToString(_DataRow["IQ_Station_ID"]); 
                    }

                    if (_DataTable.Columns.Contains("Format"))
                    {
                        _RL_STATION.Format = Convert.ToString(_DataRow["Format"]); 
                    }

                    if (_DataTable.Columns.Contains("Station_Call_Sign"))
                    {
                        _RL_STATION.Station_Call_Sign = Convert.ToString(_DataRow["Station_Call_Sign"]); 
                    }

                    if (_DataTable.Columns.Contains("IsActive"))
                    {
                        _RL_STATION.IsActive = Convert.ToBoolean(_DataRow["IsActive"]); 
                    }

                    if (_DataTable.Columns.Contains("dma_name"))
                    {
                        _RL_STATION.dma_name = Convert.ToString(_DataRow["dma_name"]); 
                    }

                    if (_DataTable.Columns.Contains("dma_num"))
                    {
                        _RL_STATION.dma_num = Convert.ToString(_DataRow["dma_num"]); 
                    }

                    if (_DataTable.Columns.Contains("gmt_adj"))
                    {
                        _RL_STATION.gmt_adj = Convert.ToString(_DataRow["gmt_adj"]); 
                    }

                    if (_DataTable.Columns.Contains("dst_adj"))
                    {
                        _RL_STATION.dst_adj = Convert.ToString(_DataRow["dst_adj"]); 
                    }

                    if (_DataTable.Columns.Contains("_CategoryID"))
                    {
                        _RL_STATION.CategoryID = Convert.ToInt32(_DataRow["_CategoryID"]);
                    }

                    if (_DataTable.Columns.Contains("_RegionID"))
                    {
                        _RL_STATION.RegionID = Convert.ToInt32(_DataRow["_RegionID"]);
                    }

                    _ListofRL_STATION.Add(_RL_STATION);
                }
            }

            return _ListofRL_STATION;
        }

        public MasterIQ_Station GetAllDetailWithRegion(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed)
        {
            try
            {
                DataSet _DataSet = null;
                MasterIQ_Station _MasterIQ_Station = new MasterIQ_Station();

                _DataSet = _IIQ_STATIONModel.GetAllDetailWithRegion(p_ClientGUID, out IsAllDmaAllowed, out IsAllStationAllowed, out IsAllClassAllowed);
                _MasterIQ_Station._ListofMarket = FillMarketInformation(_DataSet);
                _MasterIQ_Station._ListofType = FillProgramTypeInformation(_DataSet);
                _MasterIQ_Station._ListofAffil = FillAffilInformation(_DataSet);

                return _MasterIQ_Station;

            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

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
        private List<IQ_STATION> FillAffilInformation(DataSet _DataSet)
        {
            List<IQ_STATION> _ListOfAffilTypeInformation = new List<IQ_STATION>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[2].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[2].Rows)
                    {
                        IQ_STATION _IQ_STATION = new IQ_STATION();

                        if (_DataSet.Tables[2].Columns.Contains("IQ_Station_ID"))
                        {
                            _IQ_STATION.IQ_Station_ID = Convert.ToString(_DataRow["IQ_Station_ID"]);
                        }
                        if (_DataSet.Tables[2].Columns.Contains("Station_Affil_Cat_Name"))
                        {
                            _IQ_STATION.Station_Affil_Cat_Name = Convert.ToString(_DataRow["Station_Affil_Cat_Name"]);
                        }
                        if (_DataSet.Tables[2].Columns.Contains("Station_Affil_Cat_Num"))
                        {
                            _IQ_STATION.Station_Affil_Cat_Num = Convert.ToInt32(_DataRow["Station_Affil_Cat_Num"]);
                        }
                        if (_DataSet.Tables[2].Columns.Contains("Station_Call_Sign"))
                        {
                            _IQ_STATION.Station_Call_Sign = Convert.ToString(_DataRow["Station_Call_Sign"]);
                        }
                        _ListOfAffilTypeInformation.Add(_IQ_STATION);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfAffilTypeInformation;
        }


    }
}