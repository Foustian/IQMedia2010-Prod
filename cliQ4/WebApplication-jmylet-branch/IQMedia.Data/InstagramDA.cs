using System;
using System.Collections.Generic;
using System.Data;
using IQMedia.Data.Base;

namespace IQMedia.Data
{
    public class InstagramDA : IDataAccess
    {
        public void InsertSources(string sourceXml)
        {
            try
            {
                List<DataType> dataTypeList = new List<DataType>();
                dataTypeList.Add(new DataType("@SourceXml", DbType.Xml, sourceXml, ParameterDirection.Input));
                DataAccess.ExecuteNonQuery("usp_v4_IQ_Instagram_InsertSources", dataTypeList);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
