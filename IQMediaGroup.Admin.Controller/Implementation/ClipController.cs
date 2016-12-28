using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using IQMediaGroup.Core.Enumeration;
using System.Xml;
using IQMediaGroup.Admin.Controller.Factory;
using System.Threading;


namespace IQMediaGroup.Admin.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IClipController
    /// </summary>
    public  class ClipController : IClipController
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
      
        
        /// <summary>
        /// This method is used to get caption information
        /// </summary>
        /// <param name="p_WebResponse">Webresponse from webservice</param>
        /// <returns>Caption Information</returns>
        public List<CaptionInformation> GetCaptions(string p_WebResponse)
        {
            try
            {
                List<CaptionInformation> _Caption = new List<CaptionInformation>();

                int _Index = p_WebResponse.IndexOf("Caption");
                string _SubResponsevideo = p_WebResponse.Substring(_Index);
                List<string> _FinalCaptionArray = new List<string>();
                int _IndexOffset = 0;
                while (_Index != -1)
                {
                    _Index = _SubResponsevideo.IndexOf("Caption");
                    if (_Index != -1)
                    {
                        _SubResponsevideo = _SubResponsevideo.Substring(_Index + 10);
                        int _IndexSub = _SubResponsevideo.IndexOf("StartOffset");
                        string _Result = _SubResponsevideo.Remove(_IndexSub);
                        _Result = _Result.Remove(_Result.Length - 2);
                        _IndexOffset = _IndexSub + 13;
                        string _OffSet = _SubResponsevideo.Substring(_IndexOffset, 3);
                        _FinalCaptionArray.Add(_Result);
                        _Caption.Add(new CaptionInformation { CaptionMessage = _Result, CaptionStartOffset = Convert.ToInt32(_OffSet) });
                        _Index = _Index + 5;
                    }
                }

                return _Caption;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

        }
       
    }
}
