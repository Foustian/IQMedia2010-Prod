using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class NB_GenreModel : IQMediaGroupDataLayer , INB_GenreModel
    {
        public DataSet GetAllGenre()
        {
            try
            {
                DataSet _DataSet = new DataSet();
                List<DataType> _ListOfDataType = new List<DataType>();

                _DataSet = this.GetDataSet("usp_NB_Genre_SelectAll", _ListOfDataType);

                return _DataSet;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
