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
    internal class PricingCodeController : IPricingCodeController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IPricingCodeModel _IPricingCodeModel;

        public PricingCodeController()
        {
            _IPricingCodeModel = _ModelFactory.CreateObject<IPricingCodeModel>();
        }

        /// <summary>
        /// This method gets PricingCode Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the PricingCode</param>
        /// <returns>List of object of PricingCode Class</returns>
        public List<PricingCode> GetPricingCodeInformation()
        {
            try
            {
                List<PricingCode> _ListOfPricingCode = null;

                DataSet _DataSet = _IPricingCodeModel.GetPricingCodeInfo();

                _ListOfPricingCode = FillListOfPricingCode(_DataSet);

                return _ListOfPricingCode;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object PricingCode from DataSet
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_DataSet">DataSet contains PricingCodeInformation</param>
        /// <returns>List of Object of class PricingCodes</returns>
        private List<PricingCode> FillListOfPricingCode(DataSet p_DataSet)
        {
            try
            {
                List<PricingCode> _ListOfPricingCode = new List<PricingCode>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        PricingCode _PricingCode = new PricingCode();

                        _PricingCode.PricingCodeKey = Convert.ToInt32(_DataRow["PricingCodeKey"]);
                        _PricingCode.Pricing_Code = _DataRow["Pricing_Code"].ToString();
                        _PricingCode.Pricing_Code_Description = _DataRow["Pricing_Code_Description"].ToString();

                        _ListOfPricingCode.Add(_PricingCode);
                    }
                }

                return _ListOfPricingCode;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        
    }
}
