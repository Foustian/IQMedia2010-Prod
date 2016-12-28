using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Admin.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;



namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of interface IStationModel
    /// </summary>
    internal class StatSkedProgModel : IQMediaGroupDataLayer, IStatSkedProgModel
    {
        
        /// <summary>
        /// Description: This method Gets the All Program Information.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        public DataSet GetAllDetail()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_STATSKEDPROG_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This method Gets the All Program Information by string.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        public DataSet GetDetailsByString(StatSkedProg _StatSkedProg)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQ_Time_Zone", DbType.String, _StatSkedProg.IQ_Time_Zone, ParameterDirection.Input));                
                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, _StatSkedProg.MinDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.DateTime, _StatSkedProg.MaxDate, ParameterDirection.Input));               
                _ListOfDataType.Add(new DataType("@IQ_Dma_Num", DbType.String, _StatSkedProg.IQ_Dma_Num, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Cat", DbType.String, _StatSkedProg.IQ_Cat, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_class", DbType.String, _StatSkedProg.IQ_class, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title120", DbType.String, _StatSkedProg.Title120, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Desc100", DbType.String, _StatSkedProg.Desc100, ParameterDirection.Input));                
                _ListOfDataType.Add(new DataType("@Station_Affil", DbType.String, _StatSkedProg.Station_Affil, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageNumber", DbType.Int32, _StatSkedProg.PageNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PageSize", DbType.Int32, _StatSkedProg.PageSize, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@SortField", DbType.String, _StatSkedProg.SortField, ParameterDirection.Input));                

                _DataSet = this.GetDataSet("usp_STATSKEDPROG_SelectByStringWithDateTime", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This method Gets the All Program Information by string.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>Dataset containing Program details.</returns>
        public DataSet GetDetailsByStringWithTime(StatSkedProg _StatSkedProg)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQ_Time_Zone", DbType.String, _StatSkedProg.IQ_Time_Zone, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@FromDate", DbType.DateTime, _StatSkedProg.MinDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ToDate", DbType.DateTime, _StatSkedProg.MaxDate, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Dma_Num", DbType.String, _StatSkedProg.IQ_Dma_Num, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_Cat", DbType.String, _StatSkedProg.IQ_Cat, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IQ_class", DbType.String, _StatSkedProg.IQ_class, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title120", DbType.String, _StatSkedProg.Title120, ParameterDirection.Input));


                _DataSet = this.GetDataSet("usp_STATSKEDPROG_SelectByStringWithTime", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will Get Details of StatSkedProg
        /// </summary>
        /// <param name="p_GUIDs">GUID</param>
        /// <returns>Dataset of StatSkedProg</returns>
        public DataSet GetDetailByGUIDs(string p_GUIDs)
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@GUIDs", DbType.String, p_GUIDs, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_STATSKEDPROG_SelectByGUIDs", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This Method will return all the SSP Data , As per Client Settings
        /// </summary>
        /// <param name="p_GUIDs">ClientGUID</param>
        /// <returns>Dataset of StatSkedProg</returns>
        public DataSet GetAllDetailByClientSettings(Guid p_ClientGUID, out Boolean IsAllDmaAllowed, out Boolean IsAllStationAllowed, out Boolean IsAllClassAllowed)
        {
            try
            {
                DataSet _DataSet = null;
                IsAllDmaAllowed = true; IsAllStationAllowed = true; IsAllClassAllowed = true;
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@IsAllDmaAllowed", DbType.Boolean, IsAllDmaAllowed, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsAllStationAllowed", DbType.Boolean, IsAllStationAllowed, ParameterDirection.Output));
                _ListOfDataType.Add(new DataType("@IsAllClassAllowed", DbType.Boolean, IsAllClassAllowed, ParameterDirection.Output));

                Dictionary<string, string> _OutputParams = null;


                _DataSet = this.GetDataSetWithOutParam("usp_STATSKEDPROG_SelectByClientSettings", _ListOfDataType, out _OutputParams);

                if (_OutputParams != null && _OutputParams.Count > 0)
                {
                    IsAllDmaAllowed = string.IsNullOrEmpty(_OutputParams["@IsAllDmaAllowed"]) ? true : Convert.ToBoolean(_OutputParams["@IsAllDmaAllowed"]);
                    IsAllStationAllowed = string.IsNullOrEmpty(_OutputParams["@IsAllStationAllowed"]) ? true : Convert.ToBoolean(_OutputParams["@IsAllStationAllowed"]);
                    IsAllClassAllowed = string.IsNullOrEmpty(_OutputParams["@IsAllClassAllowed"]) ? true : Convert.ToBoolean(_OutputParams["@IsAllClassAllowed"]);
                }
                else
                {
                    IsAllDmaAllowed = true; IsAllStationAllowed = true; IsAllClassAllowed = true;
                }

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
