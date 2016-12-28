using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Web;
using System.Net;

namespace IQMediaGroup.Reports.Controller.Interface
{
    /// <summary>
    /// Interface for Clip
    /// </summary>
    public interface IClipController
    {
        /// <summary>
        /// This method gets Captions for particular Clip
        /// </summary>
        /// <param name="p_ClipID">ClipID</param>
        /// <returns>List of Object of Caption</returns>
        List<Caption> GetClipCaption(string p_ClipID);                
    }
}
