using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Text;
using System.IO;
using System.Data;
using System.Xml;
using System.Configuration;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Model.Factory;
using IQMediaGroup.Model.Interface;

namespace IQMediaGroup.Controller.Implementation
{
    internal class ContactUsController : IContactUsController
    {
        private readonly ModelFactory _ModelFactory = new ModelFactory();
        private readonly IContactUsModel _IContactUsModel;
        public ContactUsController()
        {
            _IContactUsModel = _ModelFactory.CreateObject<IContactUsModel>();            
        }

        /// <summary>
        ///  This method inserts contact details.
        /// </summary>
        /// <param name="p_ContactUs">Object of Core class of ContactUs</param>
        /// <returns>ContactMemberID if added successfully.</returns>
        public string InsertContactDetails(IQMediaContactUs p_ContactUs)
        {
            try
            {
                string _Result = string.Empty;

                _Result = _IContactUsModel.InsertContactDetails(p_ContactUs);

                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method updates ContactUs Content
        /// </summary>
        /// <param name="p_ContactUsText">ContactUs Content Information</param>
        /// <returns></returns>
        public string UpdateContactUsText(string p_ContactUsText)
        {
            try
            {
                string _Result = _IContactUsModel.UpdateContactUsText(p_ContactUsText);
                return _Result;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This meyhod gets Contact Us details.
        /// </summary>
        /// <returns>List of object of Contact Us Details</returns>
        public List<IQMediaContactUs> GetContactUsDetails()
        {
            try
            {
                List<IQMediaContactUs> _ListOfContactUsDetails = null;

                DataSet _DataSet = _IContactUsModel.GetContactUsDetails();

                _ListOfContactUsDetails = FillListOfContactUs(_DataSet);

                return _ListOfContactUsDetails;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method fills List of object ContactUs from DataSet
        /// </summary>
        /// <param name="p_DataSet">DataSet contains ContactUs Information</param>
        /// <returns>List of Object of class Clients</returns>
        private List<IQMediaContactUs> FillListOfContactUs(DataSet p_DataSet)
        {
            try
            {
                List<IQMediaContactUs> _ListOfIQMediaContactUs = new List<IQMediaContactUs>();

                if (p_DataSet != null && p_DataSet.Tables.Count > 0)
                {
                    foreach (DataRow _DataRow in p_DataSet.Tables[0].Rows)
                    {
                        IQMediaContactUs _IQMediaContactUs = new IQMediaContactUs();

                        _IQMediaContactUs.ContactUsText  = Convert.ToString(_DataRow[("ContactUsText")]);

                        _ListOfIQMediaContactUs.Add(_IQMediaContactUs);
                    }
                }

                return _ListOfIQMediaContactUs;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }
    }
}
