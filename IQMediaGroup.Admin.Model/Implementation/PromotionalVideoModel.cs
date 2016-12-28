using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface IPromotionalVideoModel
    /// </summary>
    internal class PromotionalVideoModel : IQMediaGroupDataLayer,IPromotionalVideoModel
    {
        /// <summary>
        /// Description: This method Gets the Promotional Video Information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_VideoID">VideoID</param>
        /// <returns>Video Information for the VideoID</returns>
        public DataSet GetPromotionalVideoByVideoID()
        {
            try
            {
                DataSet _DataSet = new DataSet();

                _DataSet = this.GetDataSetByProcedure("usp_PromotionalVideo_SelectByVideoID");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This method Gets the Promotional Video Information By Page Name.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_PageID"></param>
        /// <returns></returns>
        public DataSet GetPromotionalVideoByPageName(string p_PageName)
        {
            try
            {
                DataSet _DataSet = new DataSet();

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@PageName", DbType.String, p_PageName, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_PromotionalVideo_SelectByPageName", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
