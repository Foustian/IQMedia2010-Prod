using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sentiment.Base;
using Sentiment.Domain.Interface;
using Sentiment.HelperClasses;
using System.Data;

namespace Sentiment.Domain.Implementation
{
    internal class IQ_SentimentsModel : IQMediaGroupDataLayer, IIQ_SentimentsModel
    {
        public List<IQ_Sentiment> GetSentimentsByClientGuid(Guid p_ClientGuid)
        {
            try
            {
                List<IQ_Sentiment> _ListOfIQSentiments = new List<IQ_Sentiment>();
                List<DataType> _ListOfDataType = new List<DataType>();
                _ListOfDataType.Add(new DataType("@ClientGuid", DbType.Guid, p_ClientGuid, ParameterDirection.Input));
                using (IDataReader _IDataReader = GetDataReader("usp_IQ_Sentiments_SelectByClientGuid", _ListOfDataType))
                {
                    _ListOfIQSentiments = FillIQ_Sentiment(_IDataReader);
                }
                return _ListOfIQSentiments;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<IQ_Sentiment> FillIQ_Sentiment(IDataReader _IDataReader)
        {
            try
            {
                List<IQ_Sentiment> _ListOfIQSentiments = new List<IQ_Sentiment>();

                while (_IDataReader.Read())
                {
                    IQ_Sentiment _IQ_Sentiment = new IQ_Sentiment();
                    _IQ_Sentiment.Word =_IDataReader.GetString(0);
                    _IQ_Sentiment.Value = _IDataReader.GetInt32(1);
                    _ListOfIQSentiments.Add(_IQ_Sentiment);
                }

                

                return _ListOfIQSentiments;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
