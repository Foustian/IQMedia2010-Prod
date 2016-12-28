using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of interface IClusterModel
    /// </summary>
    internal class ClusterModel : IQMediaGroupDataLayer,IClusterModel
    {
        /// <summary>
        /// This method gets Cluster information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <returns>Dataset containig Cluster information.</returns>
        public DataSet GetAllCluster()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = this.GetDataSetByProcedure("usp_Cluster_SelectAll");

                return _DataSet;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets cluster information with all status(True\Flase)
        /// Added By: Bhavik Barot
        /// </summary>
        /// <returns>Dataset containig cluster information.</returns>
        public DataSet GetClusterWithAllStatus()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = this.GetDataSetByProcedure("usp_Cluster_Select");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts cluster information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Cluster">Object of cluster class</param>
        /// <returns>ClusterKey</returns>
        public string InsertCluster(Cluster p_Cluster)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClusterName", DbType.String, p_Cluster.ClusterName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClusterKey", DbType.Int32, p_Cluster.ClusterKey, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_Cluster_Insert", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates cluster information
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_Cluster">Object of Cluster class</param>
        /// <returns>ClusterKey</returns>
        public string UpdateCluster(Cluster p_Cluster)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClusterName", DbType.String, p_Cluster.ClusterName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClusterKey", DbType.Int32, p_Cluster.ClusterKey, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Active", DbType.Boolean, p_Cluster.IsActive, ParameterDirection.Input));

                _Result = ExecuteNonQuery("usp_Cluster_Update", _ListOfDataType);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
