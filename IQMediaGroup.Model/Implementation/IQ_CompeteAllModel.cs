using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Model.Interface;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Model.Implementation
{
    internal class IQ_CompeteAllModel : IQMediaGroupDataLayer, IIQ_CompeteAllModel
    {
        public DataSet GetArtileAdShareValueByClientGuidAndXml(Guid p_ClientGuid, string p_WebSiteURLXml,string p_MediaType)
        {
            try
            {
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@PublicationXml", DbType.Xml, p_WebSiteURLXml, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@MediaType", DbType.String, p_MediaType, ParameterDirection.Input));
                DataSet _Result = this.GetDataSet("usp_IQ_CompeteAll_SelectArtileAdShareByClientGuidAndXml", _ListOfDataType);

                return _Result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
