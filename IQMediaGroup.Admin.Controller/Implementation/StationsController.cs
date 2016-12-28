using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class StationsController: IStationsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IStationsModel _IStationsModel;

        public StationsController()
        {
            _IStationsModel = _ModelFactory.CreateObject<IStationsModel>();            
        }

        /// <summary>
        /// Description: This Methods Insert Stations.
        /// Added By: Maulik Gandhi   
        /// </summary>
        public string InsertStations(Stations p_Stations)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IStationsModel.InsertStations(p_Stations);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This get all Station Information.
        /// Added By: Maulik Gandhi   
        /// </summary>
        public List<Stations> GetStationDetail(Stations p_Stations)
        {
            DataSet _DataSet = null;
            List<Stations> _ListOfStations = null;

            try
            {
                _DataSet = _IStationsModel.GetStationDetail(p_Stations);
                _ListOfStations = FillStationInfo(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStations;
        }
        /// <summary>
        /// Description: This Methods Fills Station Information from DataSet.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="_DataSet">Dataset for Station Infromarmation</param>
        private List<Stations> FillStationInformation(DataSet _DataSet)
        {
            List<Stations> _ListOfStationInformation = new List<Stations>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Stations _Stations = new Stations();
                        _Stations.StationKey = Convert.ToInt32(_DataRow["StationKey"]);
                        _Stations.Station_MediaService_ID = Convert.ToString(_DataRow["Station_MediaService_ID"]);
                        _Stations.station_name = Convert.ToString(_DataRow["station_name"]);
                        _Stations.CityID = Convert.ToInt32(_DataRow["StationCityID"]);
                        _Stations.station_call_sign = _DataRow["station_call_sign"].ToString();
                        _ListOfStationInformation.Add(_Stations);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfStationInformation;
        }

        
        /// <summary>
        /// Description: This get all Station Information by Station Name.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_StationName">list of object of Stations</param>
        /// <returns>list of object of Stations</returns>
        public List<Stations> GetStations(string p_StationName)
        {
            DataSet _DataSet = null;
            List<Stations> _ListOfStations = null;

            try
            {
                _DataSet = _IStationsModel.GetStations(p_StationName);
                _ListOfStations = FillStationInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfStations;
        }

        /// <summary>
        /// Description: This fil all Station Information by Station Name.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_StationName">list of object of Stations</param>
        /// <returns>list of object of Stations</returns>
        private List<Stations> FillStationInfo(DataSet _DataSet)
        {
            List<Stations> _ListOfStationInformation = new List<Stations>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        Stations _Stations = new Stations();
                        _Stations.StationKey = Convert.ToInt32(_DataRow["StationKey"]);
                        _ListOfStationInformation.Add(_Stations);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfStationInformation;
        }

    }

}
