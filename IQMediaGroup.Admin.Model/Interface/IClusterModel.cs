using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Interface
{
    /// <summary>
    /// Interface for Cluster
    /// </summary>
    public interface IClusterModel
    {
        /// <summary>
        /// This method gets all active Clusers
        /// </summary>
        /// <returns>DataSet contains necessary information of Cluster</returns>
        DataSet GetAllCluster();

        /// <summary>
        /// This method gets all Clusters
        /// </summary>
        /// <returns>DataSet contains necessary information of Cluster</returns>
        DataSet GetClusterWithAllStatus();

        /// <summary>
        /// This method inserts cluster information.
        /// </summary>
        /// <param name="_Cluster"></param>
        /// <returns></returns>
        string InsertCluster(Cluster p_Cluster);

        /// <summary>
        /// This method updates Cluster Information.
        /// </summary>
        /// <param name="_Cluster"></param>
        /// <returns></returns>
        string UpdateCluster(Cluster p_Cluster);
    }
}
