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
using IQMediaGroup.Reports.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using Microsoft.CSharp;
using IQMediaGroup.Reports.Model.Factory;
using IQMediaGroup.Reports.Model.Interface;

namespace IQMediaGroup.Reports.Controller.Implementation
{
    internal class CustomCategoryController : ICustomCategoryController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly ICustomCategoryModel _ICustomCategoryModel;
        List<Clip> _ListOfTempClip = new List<Clip>();

        public CustomCategoryController()
        {
            _ICustomCategoryModel = _ModelFactory.CreateObject<ICustomCategoryModel>();
        }

        public List<CustomCategory> SelectByClientGUID(Guid p_ClientGUID)
        {
            try
            {
                List<CustomCategory> _ListOfCustomCategory = null;

                DataSet _DataSet = _ICustomCategoryModel.SelectByClientGUID(p_ClientGUID);

                _ListOfCustomCategory = FillListOfCustomCategoryByClientGUID(_DataSet);

                return _ListOfCustomCategory;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<CustomCategory> FillListOfCustomCategoryByClientGUID(DataSet p_DataSet)
        {
            try
            {
                List<CustomCategory> _ListOfCustomCategory = new List<CustomCategory>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        CustomCategory _CustomCategory = new CustomCategory();
                        _CustomCategory.CategoryKey = Convert.ToInt64(_DataRow["CategoryKey"]);
                        _CustomCategory.ClientGUID = new Guid(Convert.ToString(_DataRow["ClientGUID"]));
                        _CustomCategory.CategoryGUID = new Guid(Convert.ToString(_DataRow["CategoryGUID"]));
                        _CustomCategory.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        _CustomCategory.CategoryDescription = Convert.ToString(_DataRow["CategoryDescription"]);
                        _ListOfCustomCategory.Add(_CustomCategory);
                    }
                }

                return _ListOfCustomCategory;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }


    }

}