using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IClusterController
    /// </summary>
    internal class ClusterController:IClusterController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IClusterModel _IClusterModel;

        public ClusterController()
        {
            _IClusterModel = _ModelFactory.CreateObject<IClusterModel>();
        }

        /// <summary>
        /// This method gets all Cluster Information
        /// </summary>
        /// <returns>List of object of Class Cluster</returns>
        public List<Cluster> GetAllCluster()
        {
            try
            {
                List<Cluster> _ListOfCustomer = null;

                DataSet _DataSet = _IClusterModel.GetAllCluster();

                _ListOfCustomer = FillListOfCluster(_DataSet);

                return _ListOfCustomer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
        
        /// <summary>
        /// This method gets Cluster information with both status(True\False)
        /// </summary>
        /// <returns>List of object of Class Cluster</returns>
        public List<Cluster> GetClusterWithAllStatus()
        {
            try
            {
                List<Cluster> _ListOfCustomer = null;

                DataSet _DataSet = _IClusterModel.GetClusterWithAllStatus();

                _ListOfCustomer = FillListOfCluster(_DataSet);

                return _ListOfCustomer;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object Cluster from DataSet
        /// </summary>
        /// <param name="p_DataSet">DataSet contains ClusterInformation</param>
        /// <returns>List of Object of class Cluster</returns>
        private List<Cluster> FillListOfCluster(DataSet p_DataSet)
        {
            try
            {
                List<Cluster> _ListOfCluster = new List<Cluster>();

                if (p_DataSet!=null && p_DataSet.Tables.Count>0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Cluster _Cluster = new Cluster();

                        _Cluster.ClusterKey = CommonFunctions.GetInt64Value(_DataRow["ClusterKey"].ToString());
                        _Cluster.ClusterName = _DataRow["ClusterName"].ToString();
                        _Cluster.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfCluster.Add(_Cluster);
                    }
                }

                return _ListOfCluster;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
        }

        /// <summary>
        /// This method inserts Cluster information.
        /// </summary>
        /// <param name="p_Cluster">Dataset containing Cluster information.</param>
        /// <returns>ClusterKey</returns>
        public string InsertCluster(Cluster p_Cluster)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClusterModel.InsertCluster(p_Cluster);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates Cluster information.
        /// </summary>
        /// <param name="p_Cluster">Dataset containing Cluster information.</param>
        /// <returns>ClusterKey</returns>
        public string UpdateCluster(Cluster p_Cluster)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _IClusterModel.UpdateCluster(p_Cluster);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
