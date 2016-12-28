using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQRegionModel : IQMediaGroupDataLayer, IIQRegionModel
    {

        public DataSet GetAllRegion()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();               

                _DataSet = this.GetDataSet("usp_IQRegion_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
