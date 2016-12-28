using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IQMediaGroup.Common.Util;
using System.Configuration;
using IQMediaGroup.Domain;
using IQMediaGroup.Logic;
using IQMediaGroup.Services.Serializers;
using IQMediaGroup.Services.Util;

namespace IQMediaGroup.Services.Commands
{
    public class CategoryService : ICommand
    {

        #region ICommand Members

        public Guid? _ClientGUID { get; private set; }

        public CategoryService(object ClientGUID)
        {
            _ClientGUID = (ClientGUID is NullParameter) ? null : (Guid?)ClientGUID;
        }

        public void Execute(HttpRequest HttpRequest, HttpResponse HttpResponse)
        {
            string _JSONResult = string.Empty;
            CLogger.Debug("Category Request Started");
            try
            {
                if (_ClientGUID.HasValue)
                {
                    
                    if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
                    {
                        CLogger.Debug("{\"ClientGUID\":\"" + _ClientGUID + "\"");
                    }

                    CategoriesServiceLogic _CategoriesServiceLogic = (CategoriesServiceLogic)LogicFactory.GetLogic(LogicType.CategoryService);

                     List<CustomCategoryData> _ListOfCustomCategory = _CategoriesServiceLogic.FillCategories(Convert.ToString(_ClientGUID));

                     _JSONResult = Serializer.Searialize(_ListOfCustomCategory);

                }
                else
                {
                    throw new CustomException("Error during parsing input");
                }
            }
            catch (Exception ex)
            {
                CLogger.Error("Error : " + ex.Message + " stack : " + ex.StackTrace);
                _JSONResult = Serializer.Searialize("An error occurred, please try again.");
            }

            if (ConfigurationManager.AppSettings["IsLogRequestResponse"] == "true")
            {
                CLogger.Debug(_JSONResult);
            }

            HttpResponse.ContentEncoding = System.Text.Encoding.UTF8;
            HttpResponse.ContentType = "application/json";
            CLogger.Debug("Email Request Ended");

            HttpResponse.Output.Write(_JSONResult);

        }
        #endregion
    }
}