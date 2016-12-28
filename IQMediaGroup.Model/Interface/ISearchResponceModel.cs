using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
namespace IQMediaGroup.Model.Interface
{
    /// <summary>
    /// Interface of Client Role
    /// </summary>
    public interface ISearchResponceModel
    {
       /// <summary>
        /// Description:This method inserts Search Responce Information.
        /// Added By:Maulik Gandhi
       /// </summary>
        /// <param name="_SearchRequest">object of Search Responce</param>
        /// <returns>Search Responce key</returns>
        string InsertSearchResponce(IQAgentSearchResponce _SearchResponce);
    }
}
