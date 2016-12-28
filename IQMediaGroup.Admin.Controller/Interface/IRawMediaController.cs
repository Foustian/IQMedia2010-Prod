using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Net;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for RawMedia
    /// </summary>
    public interface IRawMediaController
    {
        /// <summary>
        /// This method gets RawMediaID and return param Value
        /// </summary>
        /// <param name="p_RawMediaID">RawMediaID</param>
        /// <returns>Param Value</returns>
        //string GetRawMediaPlayParamValue(string p_RawMediaID);

        /// <summary>
        /// This method gets Captions of particular RawMedia
        /// </summary>
        /// <param name="p_CacheKey">CacheKey of RawMedia</param>
        /// <returns>List of Object of Class Caption</returns>
        //List<Caption> GetRawMediaCaption(string p_CacheKey);

        /// <summary>
        /// This method Search RawMedia By CacheKey
        /// </summary>
        /// <param name="p_CacheKeyValue">CacheKeys</param>
        /// <returns>WebResponse</returns>
        //string SearchRawMediaByCacheKey(string p_CacheKeyValue);

        string GenerateTimeFromDate(string p_DateMin, string p_DateMax, int? p_TimeMin, int? p_TimeMax, bool IsDateTime);
       
    }
}
