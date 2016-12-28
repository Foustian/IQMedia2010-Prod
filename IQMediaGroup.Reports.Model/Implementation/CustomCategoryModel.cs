using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Model.Base;
using IQMediaGroup.Reports.Model.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Data;

namespace IQMediaGroup.Model.Implementation
{
    internal class CustomCategoryModel : IQMediaGroupDataLayer, ICustomCategoryModel
    {

        public DataSet SelectByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ClientGUID", DbType.Guid, p_ClientGUID, ParameterDirection.Input));

                _DataSet = this.GetDataSet("usp_CustomCategory_SelectByClientGUID", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


    }

}