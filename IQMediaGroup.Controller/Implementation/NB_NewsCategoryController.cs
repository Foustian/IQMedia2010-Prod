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
    internal class NB_NewsCategoryController : INB_NewsCategoryController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly INB_NewsCategoryModel _INB_NewsCategoryModel;

        public NB_NewsCategoryController()
        {
            _INB_NewsCategoryModel = _ModelFactory.CreateObject<INB_NewsCategoryModel>();
        }

        public List<NB_NewsCategory> GetAllNewsCategory()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = _INB_NewsCategoryModel.GetAllNewsCategory();

                List<NB_NewsCategory> _ListOfNB_NewsCategory = FillNB_NewsCategoryInfo(_DataSet);

                return _ListOfNB_NewsCategory;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        private List<NB_NewsCategory> FillNB_NewsCategoryInfo(DataSet _DataSet)
        {
            List<NB_NewsCategory> _ListOfNB_NewsCategory = new List<NB_NewsCategory>();
            try
            {
                if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow _DataRow in _DataSet.Tables[0].Rows)
                    {

                        NB_NewsCategory _NB_Region = new NB_NewsCategory();
                        if (_DataSet.Tables[0].Columns.Contains("ID") && _DataRow["ID"] != null)
                            _NB_Region.ID = Convert.ToInt32(_DataRow["ID"]);

                        if (_DataSet.Tables[0].Columns.Contains("Name") && _DataRow["Name"] != null)
                            _NB_Region.Name = Convert.ToString(_DataRow["Name"]);

                        if (_DataSet.Tables[0].Columns.Contains("IsActive") && _DataRow["IsActive"] != null)
                            _NB_Region.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);

                        _ListOfNB_NewsCategory.Add(_NB_Region);
                    }
                }
                return _ListOfNB_NewsCategory;
            }
            catch (Exception)
            {
                throw;
            }
            
        }
    }
}
