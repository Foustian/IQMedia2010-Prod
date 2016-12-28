using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Admin.Controller.Interface
{
    public interface IClusterController
    {
        /// <summary>
        /// This method gets all Cluster Information
        /// </summary>
        /// <returns>List of object of Class Cluster</returns>
        List<Cluster> GetAllCluster();

        /// <summary>
        /// This method gets Cluster information with both status(True\False)
        /// </summary>
        /// <returns>List of object of Class Cluster</returns>
        List<Cluster> GetClusterWithAllStatus();

        /// <summary>
        /// This method inserts Cluster information.
        /// </summary>
        /// <param name="p_Cluster">Dataset containing Cluster information.</param>
        /// <returns>ClusterKey</returns>
        string InsertCluster(Cluster p_Cluster);

        /// <summary>
        /// This method updates Cluster information.
        /// </summary>
        /// <param name="p_Cluster">Dataset containing Cluster information.</param>
        /// <returns>ClusterKey</returns>
        string UpdateCluster(Cluster p_Cluster);

    }
}
