using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class NB_NewsCategoryModel : IQMediaGroupDataLayer, INB_NewsCategoryModel
    {
        public DataSet GetAllNewsCategory()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_NB_NewsCategory_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
