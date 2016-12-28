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
    internal class BillTypeController : IBillTypeController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IBillTypeModel _IBillTypeModel;

        public BillTypeController()
        {
            _IBillTypeModel = _ModelFactory.CreateObject<IBillTypeModel>();
        }

        /// <summary>
        /// This method gets BillType Information
        /// Added By:Bhavik Barot   
        /// </summary>
        /// <param name="p_IsActive">Status of the BillType</param>
        /// <returns>List of object of BillType Class</returns>
        public List<BillType> GetBillTypeInformation()
        {
            try
            {
                List<BillType> _ListOfBillType = null;

                DataSet _DataSet = _IBillTypeModel.GetBillTypeInfo();

                _ListOfBillType = FillListOfBillType(_DataSet);

                return _ListOfBillType;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object BillType from DataSet
        /// Added By:Bhavik Barot 
        /// </summary>
        /// <param name="p_DataSet">DataSet contains BillTypeInformation</param>
        /// <returns>List of Object of class BillTypes</returns>
        private List<BillType> FillListOfBillType(DataSet p_DataSet)
        {
            try
            {
                List<BillType> _ListOfBillType = new List<BillType>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        BillType _BillType = new BillType();

                        _BillType.BillTypeKey = Convert.ToInt32(_DataRow["BillTypeKey"]);
                        _BillType.Bill_Type = _DataRow["Bill_Type"].ToString();
                        _BillType.Bill_Type_Description = _DataRow["Bill_Type_Description"].ToString();

                        _ListOfBillType.Add(_BillType);
                    }
                }

                return _ListOfBillType;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        
    }
}
