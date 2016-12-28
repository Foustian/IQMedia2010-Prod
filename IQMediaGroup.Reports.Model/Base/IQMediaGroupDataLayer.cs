using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using System.Data.Common;
using IQMediaGroup.Core.HelperClasses;
using System.Data.SqlClient;

namespace IQMediaGroup.Reports.Model.Base
{
    internal class IQMediaGroupDataLayer
    {

        private  string _CONNECTION_STRING_KEY = ConnectionStringKeys.IQMediaGroupConnectionString.ToString();
        

        public  string CONNECTION_STRING_KEY
        {
            get
            {
                //_CONNECTION_STRING_KEY = HttpContext.Current.Request.ServerVariables["SERVER_NAME"].ToString();
                return _CONNECTION_STRING_KEY; 
            }
            set { _CONNECTION_STRING_KEY = value; }
        }

        public IDataReader GetDataReaderByStatement(string sqlCommand)
        {
            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetSqlStringCommand(sqlCommand);


            // The ExecuteReader call will request the connection to be closed upon
            // the closing of the DataReader. The DataReader will be closed 
            // automatically when it is disposed.
            IDataReader dataReader = db.ExecuteReader(dbCommand);

            return dataReader;
        }


        public string ExecuteNonQuery(string ProcedureName, List<DataType> oListOfDataType)
        {

            Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

            Int32 returnValue = 0;

            string sOutPramName = string.Empty;
            string _ReturnValue = string.Empty;

            foreach (DataType oDataType in oListOfDataType)
            {
                if (oDataType != null)
                {
                    if (oDataType.Direction == ParameterDirection.Input)
                    {
                        oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                    }

                    if (oDataType.Direction == ParameterDirection.Output || oDataType.Direction == ParameterDirection.InputOutput)
                    {
                        oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, returnValue);
                        sOutPramName = oDataType.ParameterName;
                    }
                }

            }

            _ReturnValue = Convert.ToString(oDatabase.ExecuteNonQuery(oDbCommand));

            if (!string.IsNullOrEmpty(sOutPramName))
            {
                _ReturnValue = Convert.ToString(oDatabase.GetParameterValue(oDbCommand, sOutPramName));

            }

            return _ReturnValue;
        }

        //public string ExecuteNonQueryWithSQLDatType(string ProcedureName, List<SQLDataType> oListOfDataType)
        //{

        //    Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
        //    DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

        //    Int32 returnValue = 0;

        //    string sOutPramName = string.Empty;
        //    string _ReturnValue = string.Empty;

        //    foreach (SQLDataType oDataType in oListOfDataType)
        //    {
        //        if (oDataType != null)
        //        {
        //            if (oDataType.Direction == ParameterDirection.Input)
        //            {
        //                oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
        //            }

        //            if (oDataType.Direction == ParameterDirection.Output || oDataType.Direction == ParameterDirection.InputOutput)
        //            {
        //                oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, returnValue);
        //                sOutPramName = oDataType.ParameterName;
        //            }
        //        }

        //    }

        //    _ReturnValue = Convert.ToString(oDatabase.ExecuteNonQuery(oDbCommand));

        //    if (!string.IsNullOrEmpty(sOutPramName))
        //    {
        //        _ReturnValue = Convert.ToString(oDatabase.GetParameterValue(oDbCommand, sOutPramName));

        //    }

        //    return _ReturnValue;
        //}

        public List<string> ExecuteNonQuery(string ProcedureName, List<DataType> oListOfDataType, bool Status)
        {

            Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

            Int32 returnValue = 0;

            string sOutPramName = string.Empty;
            string _ReturnValue = string.Empty;

            List<string> _ListOfReturn = new List<string>();

            foreach (DataType oDataType in oListOfDataType)
            {
                if (oDataType != null)
                {
                    if (oDataType.Direction == ParameterDirection.Input)
                    {
                        oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                    }

                    if (oDataType.Direction == ParameterDirection.Output)
                    {
                        oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, returnValue);
                        sOutPramName = oDataType.ParameterName;
                    }
                }

            }

            _ReturnValue = Convert.ToString(oDatabase.ExecuteNonQuery(oDbCommand));

            if (!string.IsNullOrEmpty(sOutPramName))
            {
                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType.Direction == ParameterDirection.Output)
                    {
                        _ReturnValue = Convert.ToString(oDatabase.GetParameterValue(oDbCommand, oDataType.ParameterName));
                        _ListOfReturn.Add(_ReturnValue);
                    }
                }
            }

            return _ListOfReturn;
        }

        public string ExecuteNonQuery(string ProcedureName, List<DataType> oListOfDataType, out Dictionary<string, string> p_Outputparams)
        {

            Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);

            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

            Int32 returnValue = 65535;

            string sOutPramName = string.Empty;
            string _ReturnValue = string.Empty;


            p_Outputparams = new Dictionary<string, string>();

            foreach (DataType oDataType in oListOfDataType)
            {
                if (oDataType != null)
                {
                    if (oDataType.Direction == ParameterDirection.Input)
                    {
                        oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                    }

                    if (oDataType.Direction == ParameterDirection.Output)
                    {
                        oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, returnValue);
                        sOutPramName = oDataType.ParameterName;
                    }
                }

            }

            _ReturnValue = Convert.ToString(oDatabase.ExecuteNonQuery(oDbCommand));

            if (!string.IsNullOrEmpty(sOutPramName))
            {
                string _OutputParamValue = string.Empty;

                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType.Direction == ParameterDirection.Output)
                    {
                        _OutputParamValue = Convert.ToString(oDatabase.GetParameterValue(oDbCommand, oDataType.ParameterName));
                        p_Outputparams.Add(oDataType.ParameterName, _OutputParamValue);
                    }
                }
            }

            return _ReturnValue;
        }

        public DataSet GetDataSet(string sProcedureName, List<DataType> oListOfDataType)
        {
            DataSet oDataSet = null;
            try
            {

                Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
                DbCommand oDbCommand = oDatabase.GetStoredProcCommand(sProcedureName);
                oDbCommand.CommandTimeout = 0;

                foreach (DataType oDataType in oListOfDataType)
                {
                    oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                }

                oDataSet = oDatabase.ExecuteDataSet(oDbCommand);

            }
            catch (Exception oException)
            {
                throw oException;
            }
            return oDataSet;

        }

        public DataSet GetDataSetWithOutParam(string sProcedureName, List<DataType> oListOfDataType,out string returnValue )
        {
            DataSet oDataSet = null;
            try
            {
                returnValue = string.Empty;

                Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
                DbCommand oDbCommand = oDatabase.GetStoredProcCommand(sProcedureName);
                oDbCommand.CommandTimeout = 0;

                int returnSize=100;                            

                string sOutPramName = string.Empty;

                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType.Direction == ParameterDirection.Input)
                    {
                        oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                    }

                    if (oDataType.Direction == ParameterDirection.Output)
                    {
                        oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, returnSize);
                        sOutPramName = oDataType.ParameterName;
                    }
                }

                oDataSet = oDatabase.ExecuteDataSet(oDbCommand);

                if (!string.IsNullOrEmpty(sOutPramName))
                {
                    returnValue = Convert.ToString(oDatabase.GetParameterValue(oDbCommand, sOutPramName));
                }

            }
            catch (Exception oException)
            {
                throw oException;
            }
            return oDataSet;

        }

        public DataSet GetDataSetWithOutParam(string sProcedureName, List<DataType> oListOfDataType, out Dictionary<string, string> p_Outputparams)
        {
            DataSet oDataSet = null;
            try
            {
                p_Outputparams = new Dictionary<string, string>();

                Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
                DbCommand oDbCommand = oDatabase.GetStoredProcCommand(sProcedureName);
                oDbCommand.CommandTimeout = 0;

                int returnSize = 0;

                string sOutPramName = string.Empty;

                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType.Direction == ParameterDirection.Input)
                    {
                        oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                    }

                    if (oDataType.Direction == ParameterDirection.Output)
                    {
                        if (oDataType.dbType == DbType.String)
                        {
                            int retrunStringSize = 1000;
                            oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, retrunStringSize);
                        }
                        else
                        {
                            oDatabase.AddOutParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, returnSize);
                        }
                        sOutPramName = oDataType.ParameterName;
                    }
                }

                oDataSet = oDatabase.ExecuteDataSet(oDbCommand);


                if (!string.IsNullOrEmpty(sOutPramName))
                {
                    string _OutputParamValue = string.Empty;

                    foreach (DataType oDataType in oListOfDataType)
                    {
                        if (oDataType.Direction == ParameterDirection.Output)
                        {
                            _OutputParamValue = Convert.ToString(oDatabase.GetParameterValue(oDbCommand, oDataType.ParameterName));
                            p_Outputparams.Add(oDataType.ParameterName, _OutputParamValue);
                        }
                    }
                }
               

            }
            catch (Exception oException)
            {
                throw oException;
            }
            return oDataSet;

        }

       

        /// <summary>
        /// This function is used to retrieve last affected text from database.
        /// </summary>
        /// <param name="ProcedureName">Stored procedure name.</param>
        /// <param name="oListOfDataType">List Of DataTypes.</param>
        /// <param name="returnVal">Return last affected text.</param>
        public void ExecuteScalar(string ProcedureName, List<DataType> oListOfDataType, ref string returnVal)
        {
            try
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
                DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType != null)
                    {
                        if (oDataType.Direction == ParameterDirection.Input)
                        {
                            oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                        }
                    }
                }
                returnVal = Convert.ToString(oDatabase.ExecuteScalar(oDbCommand));
            }
            catch (Exception oException)
            {
                throw oException;
            }
           
        }

        /// <summary>
        /// This function is used to retrieve last affected text from database.
        /// </summary>
        /// <param name="ProcedureName">Stored procedure name.</param>
        /// <param name="oListOfDataType">List Of DataTypes.</param>
        /// <param name="returnVal">Return last affected text.</param>
        public object ExecuteScalar(string ProcedureName, List<DataType> oListOfDataType)
        {
            try
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
                DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType != null)
                    {
                        if (oDataType.Direction == ParameterDirection.Input)
                        {
                            oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                        }
                    }
                }
                return (oDatabase.ExecuteScalar(oDbCommand));
            }
            catch (Exception oException)
            {
                throw oException;
            }

        }

        /// <summary>
        /// This method is used for to GetDataSetByProcedure(Generic Function)
        /// </summary>
        /// <param name="sParameterName">Name of the s parameter.</param>
        /// <param name="sParameterValue">The s parameter value.</param>
        /// <param name="sProcedureName">Name of the s procedure.</param>
        /// <returns></returns>
        /// ================================================================================================
        /// MethodName:GetDataSetByProcedure
        /// ================================================================================================
        public DataSet GetDataSetByProcedure(string sParameterName, string sParameterValue, string sProcedureName)
        {
            DataSet oDataSet = null;
            Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand oDbCommand = oDatabase.GetStoredProcCommand(sProcedureName);
            try
            {
                oDatabase.AddInParameter(oDbCommand, sParameterName, DbType.String, sParameterValue);
            }
            catch (Exception oException)
            {
                throw oException;
            }
            oDataSet = oDatabase.ExecuteDataSet(oDbCommand);
            return oDataSet;
        }

        ///================================================================================================
        /// MethodName: GetDataReaderByProcedure, It is Generic Function.
        ///================================================================================================

        public IDataReader GetDataReaderByProcedure(string procedureName)
        {

            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetStoredProcCommand(procedureName);

            // The ExecuteReader call will request the connection to be closed upon
            // the closing of the DataReader. The DataReader will be closed 
            // automatically when it is disposed.
            IDataReader dataReader = db.ExecuteReader(dbCommand);

            return dataReader;
        }

        public IDataReader GetDataReader(string ProcedureName, List<DataType> oListOfDataType)
        {
            try
            {
                Database oDatabase = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
                DbCommand oDbCommand = oDatabase.GetStoredProcCommand(ProcedureName);

                foreach (DataType oDataType in oListOfDataType)
                {
                    if (oDataType != null)
                    {
                        if (oDataType.Direction == ParameterDirection.Input)
                        {
                            oDatabase.AddInParameter(oDbCommand, oDataType.ParameterName, oDataType.dbType, oDataType.Value);
                        }
                    }
                }

                IDataReader dataReader = oDatabase.ExecuteReader(oDbCommand);
                return dataReader;

            }
            catch (Exception oException)
            {
                throw oException;
            }
        }

        /// <summary>
        /// Gets the data set by statement.
        /// </summary>
        /// <param name="sqlCommand">The SQL command.</param>
        /// <returns></returns>
        /// ================================================================================================
        /// MethodName: GetDataSetByStatement, It is a Generic Function.
        /// ================================================================================================
        public DataSet GetDataSetByStatement(string sqlCommand)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetStoredProcCommand(sqlCommand);


            // DataSet that will hold the returned results		
            DataSet objectsDataSet = null;
            objectsDataSet = db.ExecuteDataSet(dbCommand);
            // Note: connection was closed by ExecuteDataSet method call 

            return objectsDataSet;
        }

        public DataSet GetDataSetBySQLStatement(string Query)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetSqlStringCommand(Query);


            // DataSet that will hold the returned results		
            DataSet objectsDataSet = null;
            objectsDataSet = db.ExecuteDataSet(dbCommand);
            // Note: connection was closed by ExecuteDataSet method call 

            return objectsDataSet;
        }

        /// <summary>
        /// Gets the data set by procedure.
        /// </summary>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns></returns>
        /// ================================================================================================
        /// MethodName: GetDataSetByProcedure, It is a Generic Function.
        /// ================================================================================================

        public DataSet GetDataSetByProcedure(string procedureName)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetStoredProcCommand(procedureName);

            // DataSet that will hold the returned results		
            DataSet objectsDataSet = null;
            dbCommand.CommandTimeout = 300;
            objectsDataSet = db.ExecuteDataSet(dbCommand);
            // Note: connection was closed by ExecuteDataSet method call 

            return objectsDataSet;
        }       

        /// <summary>
        /// Gets the data set by procedure.
        /// </summary>
        /// <param name="Category">The category.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns></returns>
        /// ================================================================================================
        /// MethodName: GetDataSetByProcedure, It is a Generic Function.
        /// ================================================================================================

        public DataSet GetDataSetByProcedure(int Category, string procedureName)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetStoredProcCommand(procedureName);

            // Retrieve products from the specified category.
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, Category);

            // DataSet that will hold the returned results		
            DataSet objectsDataSet = null;
            objectsDataSet = db.ExecuteDataSet(dbCommand);
            // Note: connection was closed by ExecuteDataSet method call 

            return objectsDataSet;
        }
        ///================================================================================================
        /// MethodName: GetDataSetByProcedure, It is a Generic Function.
        ///================================================================================================

        public DataSet GetDataSetByProcedure(Database db, DbCommand dbCommand)
        {

            // DataSet that will hold the returned results		
            DataSet objectsDataSet = null;
            objectsDataSet = db.ExecuteDataSet(dbCommand);
            // Note: connection was closed by ExecuteDataSet method call 

            return objectsDataSet;
        }

        public Database CreateDataBase()
        {
            return DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);            
        }
        /// <summary>
        /// Saves the details by procedure.
        /// </summary>
        /// <param name="productID">The product ID.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns></returns>
        /// ================================================================================================
        /// MethodName: SaveDetailsByProcedure, It is a Generic Function.
        /// ================================================================================================

        public int SaveDetailsByProcedure(int productID, string procedureName)
        {
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration.
            Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);
            DbCommand dbCommand = db.GetStoredProcCommand(procedureName);

            // Add paramters
            // Input parameters can specify the input value
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productID);

            // Output parameters specify the size of the return data
            db.AddOutParameter(dbCommand, "generatedID", DbType.Int32, 9);

            db.ExecuteNonQuery(dbCommand);

            // Row of data is captured via output parameters
            int results = (int)db.GetParameterValue(dbCommand, "generatedID");

            return results;
        }
        /// <summary>
        /// Gets the scalar value by procedure.
        /// </summary>
        /// <param name="valueID">The value ID.</param>
        /// <param name="procedureName">Name of the procedure.</param>
        /// <returns></returns>
        /// ================================================================================================
        /// MethodName: GetScalarValueByProcedure, It is a Generic Function.
        /// ================================================================================================

        public string GetScalarValueByProcedure(int valueID, string procedureName)
        {
            string returnValue = string.Empty;

            try
            {
                // Create the Database object, using the default database service. The
                // default database service is determined through configuration.
                Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);

                // Passing the productID value to the GetStoredProcCommand
                // results in parameter discovery being used to correctly establish the parameter
                // information for the productID. Subsequent calls to this method will
                // cause the block to retrieve the parameter information from the 
                // cache, and not require rediscovery.
                DbCommand dbCommand = db.GetStoredProcCommand(procedureName, valueID);

                // Retrieve ProdcutName. ExecuteScalar returns an object, so
                // we cast to the correct type (string).
                returnValue = (string)db.ExecuteScalar(dbCommand);

            }

            catch (Exception oException)
            {
                throw oException;
            }

            return returnValue;
        }

        public Int32 GetScalarValueByProcedure(string valueID, string procedureName)
        {
            Int32 returnValue = 0;
            // Create the Database object, using the default database service. The
            // default database service is determined through configuration. 
            try
            {

                Database db = DatabaseFactory.CreateDatabase(CONNECTION_STRING_KEY);

                // Passing the productID value to the GetStoredProcCommand
                // results in parameter discovery being used to correctly establish the parameter
                // information for the productID. Subsequent calls to this method will
                // cause the block to retrieve the parameter information from the 
                // cache, and not require rediscovery.
                DbCommand dbCommand = db.GetStoredProcCommand(procedureName, valueID);

                // Retrieve ProdcutName. ExecuteScalar returns an object, so
                // we cast to the correct type (string).
                if (!Convert.IsDBNull(db.ExecuteScalar(dbCommand)))
                {
                    returnValue = Convert.ToInt32(db.ExecuteScalar(dbCommand));
                }

            }

            catch (Exception oException)
            {
                throw oException;
            }

            return returnValue;

        }


    }
}
