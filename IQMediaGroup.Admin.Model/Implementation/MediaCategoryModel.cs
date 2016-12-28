using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Admin.Model.Interface;
using System.Data;


namespace IQMediaGroup.Admin.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerModel
    /// </summary>
    internal class MediaCategoryModel : IQMediaGroupDataLayer, IMediaCategoryModel
    {
        
        /// <summary>
        /// Description: This method Gets the All Product Information.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>List of Media Category Detail</returns>
        public DataSet GetMediaCategoryDetail()
        {
            try
            {
                DataSet _DataSet = new DataSet(); 

                _DataSet = this.GetDataSetByProcedure("usp_MediaCategory_SelectAll");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
