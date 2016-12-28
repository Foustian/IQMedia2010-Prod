using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Core.HelperClasses;
using System.Data;
using IQMediaGroup.Admin.Model.Factory;
using IQMediaGroup.Admin.Model.Interface;
namespace IQMediaGroup.Admin.Controller.Implementation
{
    internal class IndustryController : IIndustryController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IIndustryModel _IIndustryModel;

        public IndustryController()
        {
            _IIndustryModel = _ModelFactory.CreateObject<IIndustryModel>();
        }

        /// <summary>
        /// This method gets Industry Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the Industry</param>
        /// <returns>List of object of Industry Class</returns>
        public List<Industry> GetIndustryInformation()
        {
            try
            {
                List<Industry> _ListOfIndustry = null;

                DataSet _DataSet = _IIndustryModel.GetIndustryInfo();

                _ListOfIndustry = FillListOfIndustry(_DataSet);

                return _ListOfIndustry;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object Industry from DataSet
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_DataSet">DataSet contains IndustryInformation</param>
        /// <returns>List of Object of class Industrys</returns>
        private List<Industry> FillListOfIndustry(DataSet p_DataSet)
        {
            try
            {
                List<Industry> _ListOfIndustry = new List<Industry>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        Industry _Industry = new Industry();

                        _Industry.IndustryKey = Convert.ToInt32(_DataRow["IndustryKey"]);
                        _Industry.IndustryCode = _DataRow["IndustryCode"].ToString();
                        _Industry.Industry_Description = _DataRow["Industry_Description"].ToString();

                        _ListOfIndustry.Add(_Industry);
                    }
                }

                return _ListOfIndustry;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        
    }
}
