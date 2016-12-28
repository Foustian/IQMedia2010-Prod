using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using System.Web;
using System.Net;

namespace IQMediaGroup.Admin.Controller.Interface
{
    /// <summary>
    /// Interface for Clip
    /// </summary>
    public interface IClipController
    { 

        /// <summary>
        /// This method is used to get caption information
        /// </summary>
        /// <param name="p_WebResponse">Web Response from the web service</param>
        /// <returns>Caption Information</returns>
        List<CaptionInformation> GetCaptions(string p_WebResponse);
    }
}
