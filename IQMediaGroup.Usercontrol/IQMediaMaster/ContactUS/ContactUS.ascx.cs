using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Core.HelperClasses;


namespace IQMediaGroup.Usercontrol.IQMediaMaster.ContactUS
{
    public partial class ContactUS : BaseControl
    {
        
        string _ErrorMessage = "Contact Information is not inserted. Try Again!";
        string _SuccessMessage = "Contact Information is inserted successfully.";
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        
        #region Page Events
        protected override void OnLoad(EventArgs e)
        {
            
        }
        #endregion

        #region Button Events
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                lblError.Text = "";
                if (Page.IsValid)
                {
                    IQMediaGroup.Core.HelperClasses.IQMediaContactUs _ContactUs = new IQMediaGroup.Core.HelperClasses.IQMediaContactUs();
                    string _Result = string.Empty;
                    _ContactUs.FirstName = txtFirstName.Text.Trim();
                    _ContactUs.LastName = txtLastName.Text.Trim();
                    _ContactUs.Email = txtEmailAddress.Text;
                    _ContactUs.CompanyName = txtCompanyName.Text;
                    _ContactUs.Title = txtTitle.Text;
                    _ContactUs.TelephoneNo = txtTelephone.Text;
                    _ContactUs.Comment = txtComments.Text;
                    _ContactUs.TimeToContact = txtTimeToContact.Text; ;
                    _ContactUs.HearAboutUs = txtHearAboutUs.Text;

                    ContactUsInfo _ContactUsInfo = new ContactUsInfo();

                    _ContactUsInfo.FirstName = _ContactUs.FirstName;
                    _ContactUsInfo.LastName = _ContactUs.LastName;
                    _ContactUsInfo.Email = _ContactUs.Email;
                    _ContactUsInfo.CompanyName = _ContactUs.CompanyName;
                    _ContactUsInfo.Title = _ContactUs.Title;
                    _ContactUsInfo.TelephoneNo = _ContactUs.TelephoneNo;
                    _ContactUsInfo.Comment = _ContactUs.Comment;
                    _ContactUsInfo.TimeToContact = _ContactUs.TimeToContact;
                    _ContactUsInfo.HearAboutUs = _ContactUs.HearAboutUs;

                    _ContactUs.ContactUsInfo = CommonFunctions.MakeSerialization(_ContactUsInfo);

                    _ContactUs.ContactUsInfo=_ContactUs.ContactUsInfo.Replace("<?xml version=\"1.0\" encoding=\"utf-8\"?>",string.Empty);

                    IContactUsController _IContactUsController = _ControllerFactory.CreateObject<IContactUsController>();
                    _Result = _IContactUsController.InsertContactDetails(_ContactUs);
                    if (string.IsNullOrEmpty(_Result))
                    {
                        lblError.Visible = true;
                        lblError.Text = _ErrorMessage;
                    }
                    else
                    {
                        lblError.Visible = true;
                        lblError.ForeColor = System.Drawing.Color.Green;
                        lblError.Text = _SuccessMessage;

                        txtFirstName.Text = string.Empty;
                        txtLastName.Text = string.Empty;
                        txtCompanyName.Text = string.Empty;
                        txtEmailAddress.Text = string.Empty;
                        txtTitle.Text = string.Empty;
                        txtTelephone.Text = string.Empty;
                        txtComments.Text = string.Empty;
                        txtTimeToContact.Text = string.Empty;
                        txtHearAboutUs.Text = string.Empty;
                    }
                }

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }
        #endregion

       
    }

    public class ContactUsInfo
    {
        /// <summary>
        /// Represents First Name of Customer
        /// </summary>
        public string FirstName { get; set; }

        /// <summary> 
        /// Represents LastName of Customer
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Represents Email of Customer
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Represents Company Name of Customer
        /// </summary>
        public string CompanyName { get; set; }

        /// <summary>
        /// Represents Title of Customer
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Represents ContactNo of Customer
        /// </summary>
        public string TelephoneNo { get; set; }

        /// <summary>
        /// Represents Comment given by Customer
        /// </summary>
        public string Comment { get; set; }        

        /// <summary>
        /// Represents TimeToContact
        /// </summary>
        public string TimeToContact { get; set; }

        /// <summary>
        /// Represents HearAboutUs
        /// </summary>
        public string HearAboutUs { get; set; }

    }
}