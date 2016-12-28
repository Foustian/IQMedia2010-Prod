using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using IQMediaGroup.Domain;
using System.Data.Objects;


namespace IQMediaGroup.Logic
{
    public class CategoriesServiceLogic : BaseLogic, ILogic
    {
        public List<CustomCategoryData> FillCategories(string _ClientGUID)
        {
            
            try
            {
                string _Categories = string.Empty;

                List<CustomCategoryData> _ListOfCustomCategory = Context.GetCustomCategoryByClientGUID(new Guid(_ClientGUID)).Select(
                                         a => new CustomCategoryData()
                                         {
                                             CategoryGUID = a.CategoryGUID,
                                             CategoryName = a.CategoryName,
                                         }).ToList();


                


                //if (_ListOfCustomCategory.Count > 0)
                //{
                   
                //    XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                //      new XElement("Categories",
                //       from _CustomCategory in _ListOfCustomCategory
                //       select new XElement("Cagegory",
                //       new XAttribute("CategoryName", _CustomCategory.CategoryName),
                //       new XAttribute("CategoryGUID", _CustomCategory.CategoryGUID)
                //           )));

                //    _Categories = xmlDocument.ToString();
                //}

                 ;
                 return _ListOfCustomCategory;
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }
        }

        public Dictionary<string, Guid> GetCategoriesByClientGUID(Guid p_ClientGUID)
        {
            Dictionary<string, Guid> categories = Context.GetCustomCategoryByClientGUID(p_ClientGUID).ToDictionary(a => a.CategoryName,
                                                                                                                  a => a.CategoryGUID
                                                                                                                      );
            return categories;
        }
    }
}
