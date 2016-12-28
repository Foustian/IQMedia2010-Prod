using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;


namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// Implementation of Interface ICustomerRoleModel
    /// </summary>
    internal class IQ_NewsModel : IQMediaGroupDataLayer, IIQ_NewsModel
    {

        public DataSet GetIQNews()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();
                _DataSet = this.GetDataSet("usp_IQ_News_Search", _ListOfDataType);
                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }

    }

}