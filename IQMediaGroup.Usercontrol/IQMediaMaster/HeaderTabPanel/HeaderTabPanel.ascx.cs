using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using IQMediaGroup.Controller.Interface;
using System.Web.UI.HtmlControls;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel
{
    public partial class HeaderTabPanel : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        public string ActiveTab
        {
            set
            {

                //lnkiQMediaAdvance.CssClass = CommonConstants.CssInActiveTab;
                //lnkiQMediaArchieve.CssClass = CommonConstants.CssInActiveTab;
                //lnkiQMediaAgent.CssClass = CommonConstants.CssInActiveTab;
                //lnkIQMediaClipper.CssClass = CommonConstants.CssInActiveTab;
                //lnkiQMediaCustom.CssClass = CommonConstants.CssInActiveTab;

                aMYIQ.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                aIQBasic.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                aIQAdvanced.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                aIQAgent.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                aIQCustom.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                aIQPremium.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                aMYIQnew.Attributes["class"] = CommonConstants.CssStaticInActiveTab;
                HtmlGenericControl htmlGenericControl = (HtmlGenericControl)this.FindControl(value);
                /*HtmlAnchor anchorSelected = (HtmlAnchor)this.FindControl(value);*/

                if (htmlGenericControl != null)
                {
                    htmlGenericControl.Attributes["class"] = CommonConstants.CssStaticActiveTab;
                }

                //LinkButton _LinkButton = (LinkButton)this.FindControl(value);

                //if (_LinkButton!=null)
                //{
                //    _LinkButton.CssClass = CommonConstants.CssActiveTab;   
                //}                
            }
        }

        #region "Page Events"
        protected override void OnLoad(EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {

                    IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                    ICustomerRoleController _ICustomerRoleController = _ControllerFactory.CreateObject<ICustomerRoleController>();
                    List<CustomerRoles> _ListOfCustomerRoles = _SessionInformation.CustomerRoles;

                    //if (_SessionInformation.IsAccessRights == false)
                    //{
                    //    lnkIQMediaClipper.Visible = false;
                    //}

                    if (_SessionInformation != null)
                    {
                        if (_SessionInformation.IsMyIQnew == false)
                        {
                            aMYIQnew.Visible = false;
                        }
                        if (_SessionInformation.IsiQPremium == false)
                        {
                            aIQPremium.Visible = false;
                        }

                        if (_SessionInformation.IsMyIQnew == false && _SessionInformation.IsiQPremium == false)
                        {
                            aMYIQ.Attributes.Add("style", "width:20%");
                            aIQBasic.Attributes.Add("style", "width:20%");
                            aIQAdvanced.Attributes.Add("style", "width:20%");
                            aIQAgent.Attributes.Add("style", "width:20%");
                            aIQCustom.Attributes.Add("style", "width:20%");
                        }
                        else if (_SessionInformation.IsMyIQnew == false || _SessionInformation.IsiQPremium == false)
                        {
                            aMYIQ.Attributes.Add("style", "width:16.66%");
                            aIQPremium.Attributes.Add("style", "width:16.66%");
                            aMYIQnew.Attributes.Add("style", "width:16.66%");
                            aIQBasic.Attributes.Add("style", "width:16.66%");
                            aIQAdvanced.Attributes.Add("style", "width:16.66%");
                            aIQAgent.Attributes.Add("style", "width:16.66%");
                            aIQCustom.Attributes.Add("style", "width:16.66%");
                        }

                        if (_SessionInformation.IsiQBasic == false)
                        {
                            aIQBasic.Visible = false;
                        }

                        if (_SessionInformation.IsmyIQ == false)
                        {
                            aMYIQ.Visible = false;
                        }

                        if (_SessionInformation.IsiQAdvance == false)
                        {
                            aIQAdvanced.Visible = false;
                        }

                        if (_SessionInformation.IsiQAgent == false)
                        {
                            aIQAgent.Visible = false;
                        }

                        if (_SessionInformation.IsiQCustom == false)
                        {
                            aIQCustom.Visible = false;
                        }


                        //    if (_SessionInformation.IsmyIQ == false)
                        //    {
                        //        lnkiQMediaArchieve.Visible = false;
                        //    }
                        //    if (_SessionInformation.IsiQBasic == false)
                        //    {
                        //        lnkIQMediaClipper.Visible = false;
                        //    }
                        //    if (_SessionInformation.IsiQAdvance == false)
                        //    {
                        //        lnkiQMediaAdvance.Visible = false;
                        //    }
                        //    if (_SessionInformation.IsiQAgent == false)
                        //    {
                        //        lnkiQMediaAgent.Visible = false;
                        //    }
                        //    if (_SessionInformation.IsiQCustom==false)
                        //    {
                        //        lnkiQMediaCustom.Visible = false;
                        //    }
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

        #region "Button Events"
        protected void lnkiQMediaArchieve_Click(object sender, EventArgs e)
        {
            try
            {
                /* lnkiQMediaAdvance.CssClass = "tab";
                 lnkiQMediaArchieve.CssClass = "tab-active";
                 lnkiQMediaAgent.CssClass = "tab";
                 lnkIQMediaClipper.CssClass = "tab";
                 lnkiQMediaCustom.CssClass = "tab";*/

                Response.Redirect("~/" + "MyClips/", false);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lnkIQMediaClipper_Click(object sender, EventArgs e)
        {
            try
            {
                string _Link = "Active";
                /* lnkiQMediaAdvance.CssClass = "tab";
                 lnkiQMediaArchieve.CssClass = "tab";
                 lnkiQMediaAgent.CssClass = "tab";
                 lnkIQMediaClipper.CssClass = "tab-active";
                 lnkiQMediaCustom.CssClass = "tab";*/

                Response.Redirect("~/" + "ClipandRawMedia/", false);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lnkiQMediaAgent_Click(object sender, EventArgs e)
        {
            try
            {
                /* lnkiQMediaAdvance.CssClass = "tab";
                 lnkiQMediaArchieve.CssClass = "tab";
                 lnkiQMediaAgent.CssClass = "tab-active";
                 lnkIQMediaClipper.CssClass = "tab";
                 lnkiQMediaCustom.CssClass = "tab";*/

                Response.Redirect("~/IQAgent", false);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
            }
        }

        protected void lnkiQMediaAdvance_Click(object sender, EventArgs e)
        {
            try
            {
                /* lnkiQMediaAdvance.CssClass = "tab-active";
                 lnkiQMediaArchieve.CssClass = "tab";
                 lnkiQMediaAgent.CssClass = "tab";
                 lnkIQMediaClipper.CssClass = "tab";
                 lnkiQMediaCustom.CssClass = "tab";*/

                Response.Redirect("~/IQAdvance", false);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        protected void lnkiQMediaCustom_Click(object sender, EventArgs e)
        {
            try
            {
                /* lnkiQMediaAdvance.CssClass = "tab";
                 lnkiQMediaArchieve.CssClass = "tab";
                 lnkiQMediaAgent.CssClass = "tab";
                 lnkIQMediaClipper.CssClass = "tab";
                 lnkiQMediaCustom.CssClass = "tab-active";*/

                Response.Redirect("~/IQCustom", false);
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