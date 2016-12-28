using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Factory;
using System.Xml.Linq;

namespace IQMediaGroup.WebService
{
    /// <summary>
    /// Summary description for Categories
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Categories : System.Web.Services.WebService
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        [WebMethod]
        /*public string HelloWorld()
        {
            return "Hello World";
        }*/

        public string FillCategories(string _ClientGUID)
        {
            try
            {
                string _Categories = string.Empty;
                bool _isCategories = false;

                List<CustomCategory> _listofCustomCategory = new List<CustomCategory>();
                ICustomCategoryController _ICustomCategoryController = _ControllerFactory.CreateObject<ICustomCategoryController>();
                _listofCustomCategory = _ICustomCategoryController.SelectByClientGUID(new Guid(_ClientGUID));

                if (_listofCustomCategory.Count > 0)
                {
                    /*foreach (CustomCategory _CustomCategory in _listofCustomCategory)
                    {
                        if (_isCategories == false)
                        {
                            _isCategories = true;
                            _Categories = _CustomCategory.CategoryName + "|" + _CustomCategory.CategoryGUID;
                        }
                        else
                        {
                            _Categories = _Categories + "," + _CustomCategory.CategoryName + "|" + _CustomCategory.CategoryGUID;
                        }
                    }*/

                    XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                      new XElement("Categories",
                       from _CustomCategory in _listofCustomCategory
                       select new XElement("Cagegory",
                       new XAttribute("CategoryName", _CustomCategory.CategoryName),
                       new XAttribute("CategoryGUID", _CustomCategory.CategoryGUID)
                           )));

                    _Categories = xmlDocument.ToString();
                }

                return _Categories;
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        public string FillCategories1()
        {
            try
            {
                string _Categories = string.Empty;

                _Categories = "News|f68f9082-1c6a-4973-bbcb-b2ab10a60cfb,Sports|b99ea47c-cf48-4a4e-a384-22c9e7060542";

                return _Categories;
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }
    }
}
