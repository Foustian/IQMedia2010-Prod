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
    internal class BillFrequencyController : IBillFrequencyController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IBillFrequencyModel _IBillFrequencyModel;

        public BillFrequencyController()
        {
            _IBillFrequencyModel = _ModelFactory.CreateObject<IBillFrequencyModel>();
        }

        /// <summary>
        /// This method gets BillFrequency Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the BillFrequency</param>
        /// <returns>List of object of BillFrequency Class</returns>
        public List<BillFrequency> GetBillFrequencyInformation()
        {
            try
            {
                List<BillFrequency> _ListOfBillFrequency = null;

                DataSet _DataSet = _IBillFrequencyModel.GetBillFrequencyInfo();

                _ListOfBillFrequency = FillListOfBillFrequency(_DataSet);

                return _ListOfBillFrequency;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object BillFrequency from DataSet
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_DataSet">DataSet contains BillFrequencyInformation</param>
        /// <returns>List of Object of class BillFrequencys</returns>
        private List<BillFrequency> FillListOfBillFrequency(DataSet p_DataSet)
        {
            try
            {
                List<BillFrequency> _ListOfBillFrequency = new List<BillFrequency>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        BillFrequency _BillFrequency = new BillFrequency();

                        _BillFrequency.BillFrequencyKey = Convert.ToInt32(_DataRow["BillFrequencyKey"]);
                        _BillFrequency.Bill_Frequency = _DataRow["Bill_Frequency"].ToString();
                        _BillFrequency.Bill_Frequency_Description = _DataRow["Bill_Frequency_Description"].ToString();

                        _ListOfBillFrequency.Add(_BillFrequency);
                    }
                }

                return _ListOfBillFrequency;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        
    }
}
