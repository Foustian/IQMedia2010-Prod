using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Model.Implementation
{
    internal class NB_PublicationCategoryModel  : IQMediaGroupDataLayer, INB_PublicationCategoryModel
    {
        public DataSet GetAllPublicationCategory()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_NB_PublicationCategory_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
