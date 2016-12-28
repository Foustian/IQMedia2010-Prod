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
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;

namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class MediaCategoryController : IMediaCategoryController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();        
        private readonly IMediaCategoryModel _IMediaCategoryModel;
        string _SuccessMessage = "Your password has been sent to your email.";
        public MediaCategoryController()
        {
            _IMediaCategoryModel = _ModelFactory.CreateObject<IMediaCategoryModel>();            
        }

        /// <summary>
        /// Description: This Methods gets Media Category Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>List of  Media Category Information</returns>
        public List<MediaCategory> GetMediaCategoryDetail()
        {
            DataSet _DataSet = null;
            List<MediaCategory> _ListOfMediaCategory = null;

            try
            {
                _DataSet = _IMediaCategoryModel.GetMediaCategoryDetail();
                _ListOfMediaCategory = FillMediaCategoryInformation(_DataSet);
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }

            return _ListOfMediaCategory;
        }
        /// <summary>
        /// Description: This Methods Fills Media Category Information from DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="_DataSet">Dataset for  Media Category Information</param>
        /// <returns>List of  Media Category Information</returns>
        private List<MediaCategory> FillMediaCategoryInformation(DataSet _DataSet)
        {
            List<MediaCategory> _ListOfMediaCategoryInformation = new List<MediaCategory>();

            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {
                        MediaCategory _MediaCategory = new MediaCategory();
                        _MediaCategory.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        _MediaCategory.CategoryKey = Convert.ToInt32(_DataRow["MediaCategoryKey"]);
                        _MediaCategory.CategoryCode = Convert.ToString(_DataRow["CategoryCode"]);
                        _ListOfMediaCategoryInformation.Add(_MediaCategory);
                    }
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return _ListOfMediaCategoryInformation;
        }

    }
}
