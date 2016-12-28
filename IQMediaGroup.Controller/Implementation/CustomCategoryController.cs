using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;
using System.Data;

namespace IQMediaGroup.Controller.Implementation
{
    internal class CustomCategoryController : ICustomCategoryController
    {
        ModelFactory _ModelFactory = new ModelFactory();
        ICustomCategoryModel _ICustomCategoryModel;

        public CustomCategoryController()
        {
            _ICustomCategoryModel = _ModelFactory.CreateObject<ICustomCategoryModel>();
        }

        /// <summary>
        /// Add New Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string InsertCustomCategory(CustomCategory p_CustomCategory)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomCategoryModel.InsertCustomCategory(p_CustomCategory);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Update Existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string UpdateCustomCategory(CustomCategory p_CustomCategory)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomCategoryModel.UpdateCustomCategory(p_CustomCategory);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Delete Existing Record
        /// </summary>
        /// <param name="p_CustomCategory"></param>
        /// <returns></returns>
        public string DeleteCustomCategory(Int64 p_CategoryKey)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _ICustomCategoryModel.DeleteCustomCategory(p_CategoryKey);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method gets CustomCategory information 
        /// </summary>
        /// <returns>List of object of Class CustomCategory</returns>
        public List<CustomCategory> SelectByClientID(Int64 p_ClientID)
        {
            try
            {
                List<CustomCategory> _ListOfCustomCategory = null;

                DataSet _DataSet = _ICustomCategoryModel.SelectByClientID(p_ClientID);

                _ListOfCustomCategory = FillListOfCustomCategory(_DataSet);

                return _ListOfCustomCategory;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object CustomCategory from DataSet
        /// </summary>
        /// <param name="p_DataSet">DataSet contains CustomCategory</param>
        /// <returns>List of Object of class CustomCategory</returns>
        private List<CustomCategory> FillListOfCustomCategory(DataSet p_DataSet)
        {
            try
            {
                List<CustomCategory> _ListOfCustomCategory = new List<CustomCategory>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        CustomCategory _CustomCategory = new CustomCategory();

                        if (p_DataSet.Tables[0].Columns.Contains("CategoryKey") && !_DataRow["CategoryKey"].Equals(DBNull.Value))
                        {
                            _CustomCategory.CategoryKey = Convert.ToInt64(_DataRow["CategoryKey"]);
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("ClientGUID") && !_DataRow["ClientGUID"].Equals(DBNull.Value))
                        {
                            _CustomCategory.ClientGUID = new Guid(Convert.ToString(_DataRow["ClientGUID"]));
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("CategoryGUID") && !_DataRow["CategoryGUID"].Equals(DBNull.Value))
                        {
                            _CustomCategory.CategoryGUID = new Guid(Convert.ToString(_DataRow["CategoryGUID"]));
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("CategoryName") && !_DataRow["CategoryName"].Equals(DBNull.Value))
                        {
                            _CustomCategory.CategoryName = Convert.ToString(_DataRow["CategoryName"]);
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("CategoryDescription") && !_DataRow["CategoryDescription"].Equals(DBNull.Value))
                        {
                            _CustomCategory.CategoryDescription = Convert.ToString(_DataRow["CategoryDescription"]);
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("CreatedDate") && !_DataRow["CreatedDate"].Equals(DBNull.Value))
                        {
                            _CustomCategory.CreatedDate = Convert.ToDateTime(_DataRow["CreatedDate"]);
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("ModifiedDate") && !_DataRow["ModifiedDate"].Equals(DBNull.Value))
                        {
                            _CustomCategory.ModifiedDate = Convert.ToDateTime(_DataRow["ModifiedDate"]);
                        }
                        if (p_DataSet.Tables[0].Columns.Contains("IsActive") && !_DataRow["IsActive"].Equals(DBNull.Value))
                        {
                            _CustomCategory.IsActive = Convert.ToBoolean(_DataRow["IsActive"]);
                        }
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

        /// <summary>
        /// This method gets CustomCategory information 
        /// </summary>
        /// <returns>List of object of Class CustomCategory</returns>
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
