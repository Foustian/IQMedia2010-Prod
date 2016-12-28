using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;

namespace IQMediaGroup.Controller.Interface
{
    public interface ISearchResponceController
    {
        /// <summary>
        /// This method inserts Search Responce Information
        /// </summary>
        /// <param name="p_SearchRequests">Object Of Search Responce Class</param>
        /// <returns>SearchResponceKey</returns>
        string InsertSearchResponce(IQAgentSearchResponce p_SearchResponce);
    }
}
