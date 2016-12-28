using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Controller.Interface;

namespace IQMediaGroup.WebApplication.Subscription
{
    public partial class Default : System.Web.UI.Page
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (Request.QueryString["ID"] != null && Request.QueryString["Action"] != null)
                {
                    if (Request.QueryString["Action"].ToLower() == "s" || Request.QueryString["Action"].ToLower() == "u")
                    {
                        byte[] Key = Convert.FromBase64String("pF6tvq4GexXSUaGFKXGqaFmiG2X6ihG3joVZ8RiSwpk=");
                        byte[] IV = Convert.FromBase64String("g3tqe+8V4H/JwMe0X69TGw==");
                        string encryptedText = Request.QueryString["ID"];
                        string decryptedText = CommonFunctions.DecryptStringFromBytes_Aes(encryptedText, Key, IV);

                        if (!string.IsNullOrWhiteSpace(decryptedText))
                        {
                            string[] InputIDs = decryptedText.Split('&');

                            if (InputIDs.Length > 1)
                            {
                                Int64 searchRequestID;
                                Int64 hubSpotID;

                                if (Int64.TryParse(InputIDs[0], out searchRequestID) && Int64.TryParse(InputIDs[1], out hubSpotID))
                                {
                                    IIQ_SMSCampaignController _IIQ_SMSCampaignController = _ControllerFactory.CreateObject<IIQ_SMSCampaignController>();

                                    bool isActivated = string.Compare(Request.QueryString["Action"], "s", true) == 0 ? true : false;

                                    string Result = _IIQ_SMSCampaignController.UpdateIIQ_SMSCampaignIsActive(searchRequestID, hubSpotID, isActivated);

                                    if (Convert.ToInt32(Result) > 0)
                                    {
                                        if (Request.QueryString["Action"].ToLower() == "s")
                                        {
                                            //lblSuccessMessge.Text = "You have successfully subscribed to SMS text alerts.";
                                            lblSuccessMessge.Text = IQMediaGroup.Common.Config.ConfigSettings.MessagesSection.Messages.FirstOrDefault(a => a.Key == "SubscriptionSuccess").Value;
                                        }
                                        else
                                        {
                                            //lblSuccessMessge.Text = "You have successfully unsubscribed from SMS text alerts.";
                                            lblSuccessMessge.Text = IQMediaGroup.Common.Config.ConfigSettings.MessagesSection.Messages.FirstOrDefault(a => a.Key == "UnSubscriptionSuccess").Value;
                                        }
                                    }
                                    else
                                    {
                                        if (Request.QueryString["Action"].ToLower() == "s")
                                        {
                                            lblSuccessMessge.Text = "Subscription failed.";
                                        }
                                        else
                                        {
                                            lblSuccessMessge.Text = "Unsubscription failed.";
                                        }
                                    }
                                }
                                else
                                {
                                    lblErrorMessage.Text = "Invalid ID";
                                }
                            }
                            else
                            {
                                lblErrorMessage.Text = "Invalid ID";
                            }
                        }
                        else
                        {
                            lblErrorMessage.Text = "Invalid Request";
                        }
                    }
                    else
                    {
                        lblErrorMessage.Text = "Invalid Action";
                    }
                }
                else
                {
                    lblErrorMessage.Text = "Invalid Input";
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "An error occured, please try again!!";//+ex.Message;
            }
        }
    }
}