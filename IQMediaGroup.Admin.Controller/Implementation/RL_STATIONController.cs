using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Controller.Interface;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IRL_STATION
    /// </summary>
    internal class RL_STATIONController : IRL_STATIONController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IRL_STATIONModel _IRL_STATIONModel;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public RL_STATIONController()
        {
            _IRL_STATIONModel = _ModelFactory.CreateObject<IRL_STATIONModel>();
        }


        /// <summary> 
        /// This method gets all RL_STATION details 
        /// </summary> 
        /// <returns> List Of RL_STATION </returns> 

        public List<RL_STATION> GetAllRL_STATION()
        {
            try
            {
                DataSet _DataSet = null;

                List<RL_STATION> _ListOfRL_STATION = new List<RL_STATION>();

                _DataSet = _IRL_STATIONModel.GetAllRL_STATION();
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

        public List<RL_STATION> FillRL_STATION(DataSet p_DataSet)
        {
            List<RL_STATION> _ListofRL_STATION = new List<RL_STATION>();

            if (p_DataSet != null && p_DataSet.Tables.Count > 0)
            {
                foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                {


                    RL_STATION _RL_STATION = new RL_STATION();
                    _RL_STATION.RL_Stationkey = Convert.ToInt64(_DataRow["RL_Stationkey"]);
                    _RL_STATION.RL_Station_ID = Convert.ToString(_DataRow["RL_Station_ID"]);
                    _RL_STATION.rl_format = Convert.ToString(_DataRow["rl_format"]);
                    _RL_STATION.station_call_sign = Convert.ToString(_DataRow["station_call_sign"]);
                    _RL_STATION.rl_station_active = Convert.ToString(_DataRow["rl_station_active"]);
                    _RL_STATION.time_zone = Convert.ToString(_DataRow["time_zone"]);
                    _RL_STATION.dma_name = Convert.ToString(_DataRow["dma_name"]);
                    _RL_STATION.dma_num = Convert.ToString(_DataRow["dma_num"]);
                    _RL_STATION.gmt_adj = Convert.ToString(_DataRow["gmt_adj"]);
                    _RL_STATION.dst_adj = Convert.ToString(_DataRow["dst_adj"]);

                    _ListofRL_STATION.Add(_RL_STATION);
                }
            }

            return _ListofRL_STATION;
        }

        /// <summary> 
        /// This method deletes RL_STATION 
        /// </summary> 
        /// <param name="p_RL_Stationkey"> RL_Stationkey</param> 
        /// <returns>1/0-Deleted or not</returns> 

        public string DeleteRL_STATION(string p_RL_Stationkey)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IRL_STATIONModel.DeleteRL_STATION(p_RL_Stationkey);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary> 
        /// This method gets all RL_STATION hich is active and format is 'TV'
        /// </summary> 
        /// <returns> List Of Objects RL_Stationkey </returns> 
        public List<RL_STATION> SelectAllTVStation()
        {
            try
            {
                DataSet _DataSet = null;

                List<RL_STATION> _ListOfRL_STATION = new List<RL_STATION>();

                _DataSet = _IRL_STATIONModel.SelectAllTVStation();
                _ListOfRL_STATION = FillRL_STATION(_DataSet);

                return _ListOfRL_STATION;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}