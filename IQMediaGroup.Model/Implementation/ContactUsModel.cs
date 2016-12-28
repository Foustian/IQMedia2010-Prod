using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Model.Base;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Model.Interface;
using System.Data;


namespace IQMediaGroup.Model.Implementation
{
    /// <summary>
    /// /// <summary>
    /// Implementation of Interface IContactUsModel
    /// </summary>
    /// </summary>
    internal class ContactUsModel : IQMediaGroupDataLayer, IContactUsModel
    {
        /// <summary>
        ///  This method inserts contactus details.
        /// Added By: Bhavik Barot
        /// </summary>
        /// <param name="p_ContactUs">Object of Core class of ContactUs</param>
        /// <returns>ContactMemberID if added successfully.</returns>
        public string InsertContactDetails(IQMediaContactUs p_ContactUs)
        {
            try
            {
                string _Result = string.Empty;

                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@FirstName", DbType.String, p_ContactUs.FirstName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@LastName", DbType.String, p_ContactUs.LastName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Email", DbType.String, p_ContactUs.Email, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@CompanyName", DbType.String, p_ContactUs.CompanyName, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Title", DbType.String, p_ContactUs.Title, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactNo", DbType.String, p_ContactUs.TelephoneNo, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@Comments", DbType.String, p_ContactUs.Comment, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactUsInfo", DbType.Xml, p_ContactUs.ContactUsInfo, ParameterDirection.Input));

                _ListOfDataType.Add(new DataType("@ContactUsKey", DbType.Int64, p_ContactUs.ContactMemberID, ParameterDirection.Output));

                _Result = ExecuteNonQuery("usp_ContactUs_Insert", _ListOfDataType);

               

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates ContactUs Content Information.
        /// </summary>
        /// <param name="p_ContactUsText">ContactUs Content</param>
        /// <returns></returns>
        public string UpdateContactUsText(string p_ContactUsText)
        {
            try
            {
                int _ContactUsKey = 0;
                List<DataType> _ListOfDataType = new List<DataType>();

                _ListOfDataType.Add(new DataType("@ContactUsContent", DbType.String, p_ContactUsText, ParameterDirection.Input));
                _ListOfDataType.Add(new DataType("@ContactUsKey", DbType.Int32, _ContactUsKey, ParameterDirection.Output));
                string _Result = this.ExecuteNonQuery("usp_ContactUs_Update", _ListOfDataType);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This method will get Contact us details.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <returns>Dataset of Contact Us details.</returns>
        public DataSet GetContactUsDetails()
        {
            try
            {
                DataSet _DataSet = null;

                _DataSet = this.GetDataSetByProcedure("usp_ContactUs_SelectAll");

                return _DataSet;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
