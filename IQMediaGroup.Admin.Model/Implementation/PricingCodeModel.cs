using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Model.Interface;
using IQMediaGroup.Admin.Model.Base;
using System.Data;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Model.Implementation
{
    internal class PricingCodeModel : IQMediaGroupDataLayer, IPricingCodeModel
    {
        /// <summary>
        /// This method gets Pricing Code information.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <returns>Dataset containig Pricing Code information.</returns>
        public DataSet GetPricingCodeInfo()
        {
            try
            {
                DataSet _DataSet = null;

                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_PricingCode_SelectAll", _ListOfDataType);                

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
