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
    internal class RedlassoStationMarketController : IRedlassoStationMarketController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IRedlassoStationMarketModel _IRedlassoStationMarketModel;

        public RedlassoStationMarketController()
        {
            _IRedlassoStationMarketModel = _ModelFactory.CreateObject<IRedlassoStationMarketModel>();            
        }

        /// <summary>
        /// Description:This method gets all RedlassoStationMarket
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List Of Object of helperclass Station</returns>
        public List<RedlassoStationMarket> GetAllRedlassoStationMarket()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<RedlassoStationMarket> _ListofRedlassoStationMarket = null;

                _DataSet = _IRedlassoStationMarketModel.GetRedlassoStationMarket();

                _ListofRedlassoStationMarket = FillListOfRedlassoStationMarket(_DataSet);

                return _ListofRedlassoStationMarket;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method fills all RedlassoStationMarket
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List Of Object of helperclass Station</returns>
        private List<RedlassoStationMarket> FillListOfRedlassoStationMarket(DataSet p_DataSet)
        {
            try
            {
                List<RedlassoStationMarket> _ListOfRedlassoStationMarket = new List<RedlassoStationMarket>();

                if (p_DataSet!=null && p_DataSet.Tables.Count>0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        RedlassoStationMarket _RedlassoStationMarket = new RedlassoStationMarket();
                       
                        _RedlassoStationMarket.StationMarketName = _DataRow["StationMarketName"].ToString();
                        _RedlassoStationMarket.RedlassoStationMarketKey = CommonFunctions.GetInt64Value(_DataRow["RedlassoStationMarketKey"].ToString());
                        _RedlassoStationMarket.IsActive = Convert.ToBoolean(_DataRow["IsActive"].ToString());

                        _ListOfRedlassoStationMarket.Add(_RedlassoStationMarket);
                    }
                }

                return _ListOfRedlassoStationMarket;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods Update RedlassoStationMarket Information.
        /// Added By: Maulik Gandhi   
        /// </summary>
        public string UpdateRedlassoStationMarket(RedlassoStationMarket _RedlassoStationMarket)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IRedlassoStationMarketModel.UpdateRedlassoStationMarket(_RedlassoStationMarket);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This Methods Insert RedlassoStationMarket Information.
        /// Added By: Maulik Gandhi   
        /// </summary>
        public string InsertRedlassoStationMarket(RedlassoStationMarket p_RedlassoStationMarket)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IRedlassoStationMarketModel.InsertRedlassoStationMarket(p_RedlassoStationMarket);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method gets all Active RedlassoStationMarket
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List Of Object of helperclass Station</returns>
        public List<RedlassoStationMarket> GetAllRedlassoAcitveStationMarket()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<RedlassoStationMarket> _ListofRedlassoStationMarket = null;

                _DataSet = _IRedlassoStationMarketModel.GetRedlassoActiveStationMarket();

                _ListofRedlassoStationMarket = FillListOfRedlassoActiveStationMarket(_DataSet);

                return _ListofRedlassoStationMarket;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method fills all Active RedlassoStationMarket
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>List Of Object of helperclass Station</returns>
        private List<RedlassoStationMarket> FillListOfRedlassoActiveStationMarket(DataSet p_DataSet)
        {
            try
            {
                List<RedlassoStationMarket> _ListOfRedlassoStationMarket = new List<RedlassoStationMarket>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        RedlassoStationMarket _RedlassoStationMarket = new RedlassoStationMarket();

                        
                        _RedlassoStationMarket.StationMarketName = _DataRow["StationMarketName"].ToString();
                        _RedlassoStationMarket.RedlassoStationMarketKey = CommonFunctions.GetInt64Value(_DataRow["RedlassoStationMarketKey"].ToString());
                        
                        _ListOfRedlassoStationMarket.Add(_RedlassoStationMarket);
                    }
                }

                return _ListOfRedlassoStationMarket;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
