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


namespace IQMediaGroup.Usercontrol.IQMediaMaster.Contact
{
    public partial class Contact : BaseControl
    {
        #region Page Events
        string _ErrorMessage = "Contact Information is not inserted. Try Again!";
        string _SuccessMessage = "Contact Information is inserted successfully.";
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                Page.SetFocus(txtFirstName);
            }
            catch (Exception _Exception)
            {
                
               this.WriteException(_Exception);
               Response.Redirect(CommonConstants.CustomErrorPage);
            }
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
                    _ContactUs.FirstName = txtFirstName.Text;
                    _ContactUs.LastName = txtLastName.Text;
                    _ContactUs.Email = txtEmailAddress.Text;
                    _ContactUs.CompanyName = txtCompanyName.Text;
                    _ContactUs.Title = txtTitle.Text;
                    _ContactUs.TelephoneNo = txtTelephone.Text;
                    _ContactUs.Comment = txtComments.Text;
                    
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

                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        txtCompanyName.Text = "";
                        txtEmailAddress.Text = "";
                        txtTitle.Text = "";
                        txtTelephone.Text = "";
                        txtComments.Text = "";
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
}