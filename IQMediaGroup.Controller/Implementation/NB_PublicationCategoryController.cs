using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Model.Interface;
using IQMediaGroup.Model.Factory;

namespace IQMediaGroup.Controller.Implementation
{
    internal class NB_PublicationCategoryController : INB_PublicationCategoryController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly INB_PublicationCategoryModel _INB_PublicationCategoryModel;

        public NB_PublicationCategoryController()
        {
            _INB_PublicationCategoryModel = _ModelFactory.CreateObject<INB_PublicationCategoryModel>();
        }

        public List<NB_PublicationCategory> GetAllPublicationCategory()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _INB_PublicationCategoryModel.GetAllPublicationCategory();

                List<NB_PublicationCategory> _ListOfNB_NewsCategory = FillNB_PublicationCategoryInfo(_DataSet);

                return _ListOfNB_NewsCategory;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<NB_PublicationCategory> FillNB_PublicationCategoryInfo(DataSet _DataSet)
        {
            List<NB_PublicationCategory> _ListOfNB_PublicationCategory = new List<NB_PublicationCategory>();
            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {

                        NB_PublicationCategory _NB_PublicationCategory = new NB_PublicationCategory();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && _DataRow["ID"] != null)
                            _NB_PublicationCategory.ID = Convert.ToInt32(_DataRow["ID"]);

                        if (_DataSet.Tables[0].Columns.Contains("Name") && _DataRow["Name"] != null)
                            _NB_PublicationCategory.Name = Convert.ToString(_DataRow["Name"]);

                        if (_DataSet.Tables[0].Columns.Contains("IsActive") && _DataRow["IsActive"] != null)
                            _NB_PublicationCategory.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfNB_PublicationCategory.Add(_NB_PublicationCategory);
                    }
                }
                return _ListOfNB_PublicationCategory;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
