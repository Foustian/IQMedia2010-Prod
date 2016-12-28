using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class IQAgentSearchResponceController : ISearchResponceController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ISearchResponceModel _ISearchResponceModel;

        public IQAgentSearchResponceController()
        {
            _ISearchResponceModel = _ModelFactory.CreateObject<ISearchResponceModel>();
        }

        /// <summary>
        /// This method inserts Search Request Information
        /// </summary>
        /// <param name="p_SearchResponces">Object Of SearchResponces Class</param>
        /// <returns>SearchResponceKey</returns>
        public string InsertSearchResponce(IQAgentSearchResponce p_IQAgentSearchResponce)
        {
            try
            {
                string _Result = string.Empty;
                _Result = _ISearchResponceModel.InsertSearchResponce(p_IQAgentSearchResponce);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

    }
}
