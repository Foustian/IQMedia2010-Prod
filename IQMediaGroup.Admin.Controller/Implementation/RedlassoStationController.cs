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
    internal class RedlassoStationController : IRedlassoStationController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IRedlassoStationModel _IRedlassoStationModel;

        public RedlassoStationController()
        {
            _IRedlassoStationModel = _ModelFactory.CreateObject<IRedlassoStationModel>();            
        }

       
        /// <summary>
        /// Description: This Methods Gets StationMarket.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <returns>List of object of Station Market</returns>
        public List<RedlassoStation> GetAllStationMarket()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<RedlassoStation> _ListRedlassoStation = null;

                _DataSet = _IRedlassoStationModel.GetStationMarket();

                _ListRedlassoStation = FillListOfStationMarket(_DataSet);

                return _ListRedlassoStation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        
        /// <summary>
        /// Description: This Methods Gets Redlasso Station Info.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <returns>List of object of RedlassoStation</returns>
        public List<RedlassoStation> GetRedlassoStationInfo()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<RedlassoStation> _ListRedlassoStation = null;

                _DataSet = _IRedlassoStationModel.GetRedlassoStationInfo();

                _ListRedlassoStation = FillListOfRedlassoStation(_DataSet);

                return _ListRedlassoStation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods fills StationMarket.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <returns>List of object of Station Market Information</returns>
        private List<RedlassoStation> FillListOfStationMarket(DataSet p_DataSet)
        {
            try
            {
                List<RedlassoStation> _ListOfStationMarket = new List<RedlassoStation>();

                if (p_DataSet!=null && p_DataSet.Tables.Count>0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        RedlassoStation _RedlassoStation = new RedlassoStation();
                        _RedlassoStation.StationMarket = _DataRow["StationMarket"].ToString();

                        _ListOfStationMarket.Add(_RedlassoStation);
                    }
                }

                return _ListOfStationMarket;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods fills RedlassoStation.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_DataSet">Dataset of RedlassoStation</param>
        /// <returns>List of object of Redlasso Station</returns>
        private List<RedlassoStation> FillListOfRedlassoStation(DataSet p_DataSet)
        {
            try
            {
                List<RedlassoStation> _ListOfStationMarket = new List<RedlassoStation>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        RedlassoStation _RedlassoStation = new RedlassoStation();
                        _RedlassoStation.RedlassoStationCode = _DataRow["RedlassoStationCode"].ToString();
                        _RedlassoStation.RedlassoStationKey = Convert.ToInt64(_DataRow["RedlassoStationKey"].ToString());
                        _RedlassoStation.RedlassoStationMarketID = Convert.ToInt64(_DataRow["RedlassoStationMarketID"].ToString());
                        _RedlassoStation.RedlassoStationTypeID = Convert.ToInt64(_DataRow["RedlassoStationTypeID"].ToString());
                        _RedlassoStation.StationID = Convert.ToInt64(_DataRow["StationID"].ToString());
                        _RedlassoStation.ClusterID = Convert.ToInt64(_DataRow["ClusterID"].ToString());
                        _RedlassoStation.station_name = _DataRow["station_name"].ToString();
                        _RedlassoStation.StationMarketName = _DataRow["StationMarketName"].ToString();
                        _RedlassoStation.ClusterName = _DataRow["ClusterName"].ToString();
                        _RedlassoStation.StationTypeName = _DataRow["StationTypeName"].ToString();
                        _ListOfStationMarket.Add(_RedlassoStation);
                    }
                }

                return _ListOfStationMarket;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods insert RedlassoStation.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_RedlassoStation">objet of RedlassoStation</param>
        /// <returns>Output RedlassoStationKey </returns>
        public string InsertRedlassoStation(RedlassoStation p_RedlassoStation)
        {
            try
            {
                string _Result = null;

                _Result = _IRedlassoStationModel.InsertRedlassoStation(p_RedlassoStation);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods update RedlassoStation.
        /// Added By: Maulik Gandhi   
        /// </summary>
        /// <param name="p_RedlassoStation">objet of RedlassoStation</param>
        public string UpdateRedlassoStation(RedlassoStation p_RedlassoStation)
        {
            try
            {
                string _Result = null;

                _Result = _IRedlassoStationModel.UpdateRedlassoStation(p_RedlassoStation);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
