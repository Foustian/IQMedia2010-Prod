using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class CustomCategoryModel : IQMediaGroupDataLayer, ICustomCategoryModel
    {
        /// <summary>
        /// Add New Record into table
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string InsertCustomCategory(CustomCategory p_CustomCategory)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_CustomCategory.ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryName", DbType.String, p_CustomCategory.CategoryName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryDescription", DbType.String, p_CustomCategory.CategoryDescription, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@CategoryKey", DbType.Int32, p_CustomCategory.CategoryKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_CustomCategory_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Update existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string UpdateCustomCategory(CustomCategory p_CustomCategory)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CategoryKey", DbType.Int32, p_CustomCategory.CategoryKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryName", DbType.String, p_CustomCategory.CategoryName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CategoryDescription", DbType.String, p_CustomCategory.CategoryDescription, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_CustomCategory.ClientGUID, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Status", DbType.Int32, 0, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_CustomCategory_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Update existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string DeleteCustomCategory(Int64 p_CategoryKey)
        {
            try
            {
                string _Result = string.Empty;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@CategoryKey", DbType.Int32, p_CategoryKey, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_CustomCategory_Delete", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Returns all record of partucular Client
        /// </summary>
        /// <param name="p_ClientGUID"></param>
        /// <returns></returns>
        public DataSet SelectByClientID(Int64 p_ClientID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientID", DbType.Int64, p_ClientID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_CustomCategory_SelectByClientID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Returns all record of partucular Client
        /// </summary>
        /// <param name="p_ClientGUID"></param>
        /// <returns></returns>
        public DataSet SelectByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_CustomCategory_SelectByClientGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
