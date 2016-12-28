using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sentiment.HelperClasses;

namespace Sentiment.Domain.Interface
{
    public interface IIQ_SentimentsModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="p_ListOfSentimentWords"></param>
        /// <returns></returns>
        List<IQ_Sentiment> GetSentimentsByClientGuid(Guid p_ClientGuid);
    }
}
