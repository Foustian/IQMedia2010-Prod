using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Xml.Linq;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQNielsenSquadModel : IQMediaGroupDataLayer, IIQNielsenSquadModel
    {
        public DataSet GetNielsenData(string iqCCKey)
        {
            DataSet _DataSet = new DataSet();
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQCCKeyList", DbType.String, iqCCKey, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_IQ_NIELSEN_SQAD_SelectByIQ_CC_Keys", _ListOfDataType);


                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public DataSet GetNielsenDataByXML(XDocument xmldata, Guid clientGuid)
        {
            DataSet _DataSet = new DataSet();
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@IQCCKeyList", DbType.Xml, xmldata.ToString(), ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, clientGuid, ParameterDirection.Input));
                _DataSet = this.GetDataSet("usp_IQ_NIELSEN_SQAD_SelectByIQCCKeyList", _ListOfDataType);


                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
