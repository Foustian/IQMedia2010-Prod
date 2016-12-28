﻿using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using IQMediaGroup.Core.Enumeration;
using System.Xml;
using IQMediaGroup.Reports.Controller.Factory;
using System.Threading;


namespace IQMediaGroup.Reports.Controller.Implementation
{
    /// <summary>
    /// Implementation of interface IClipController
    /// </summary>
    public  class ClipController : IClipController
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory(); 

        
        /// <summary>
        /// This method gets Captions for particular Clip
        /// </summary>
        /// <param name="p_ClipID">ClipID</param>
        /// <returns>List of Object of Caption</returns>
        public List<Caption> GetClipCaption(string p_ClipID)
        {
            try
            {               
                List<Caption> _ListOfCaption = new List<Caption>();                

                string _CaptionURL = ConfigurationManager.AppSettings[CommonConstants.ConfigGetClosedCaptionFromIQ].ToString();
                _CaptionURL = _CaptionURL + p_ClipID;

                string _Response = CommonFunctions.GetHttpResponse(_CaptionURL);

                if (!string.IsNullOrEmpty(_Response))
                {
                    XmlDocument _XmlDocument = new XmlDocument();
                    _XmlDocument.LoadXml(_Response);

                    XmlNodeList _XmlNodeList = _XmlDocument.GetElementsByTagName(CommonConstants.ClipCaptionTagName);

                    foreach (XmlNode _XmlNode in _XmlNodeList)
                    {
                        Caption _Caption = new Caption();

                        _Caption.CaptionString = HttpUtility.HtmlDecode(_XmlNode.InnerText);

                        _Caption.StartTime = Convert.ToInt32(_XmlNode.Attributes[CommonConstants.BeginTxt].Value.Substring(0, (_XmlNode.Attributes[CommonConstants.BeginTxt].Value.Length - 1))) - Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ClipCaptionDelayTxt].ToString());
                        _Caption.EndTime = Convert.ToInt32(_XmlNode.Attributes[CommonConstants.EndTxt].Value.Substring(0, (_XmlNode.Attributes[CommonConstants.EndTxt].Value.Length - 1))) - Convert.ToInt32(ConfigurationSettings.AppSettings[CommonConstants.ClipCaptionDelayTxt].ToString());

                        if (_Caption.StartTime > 0)
                        {
                            _ListOfCaption.Add(_Caption);
                        }
                    } 
                }

                return _ListOfCaption;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
       
    }
}