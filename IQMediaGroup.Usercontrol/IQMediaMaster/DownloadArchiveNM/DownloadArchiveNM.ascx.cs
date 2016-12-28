﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Controller.Factory;
using System.Configuration;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using System.Text;
using System.IO;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Interface;
using System.Xml.Linq;
using System.Data.SqlTypes;
using System.Threading;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.DownloadArchiveNM
{
    public partial class DownloadArchiveNM : BaseControl
    {
        ControllerFactory _ControllerFactory = new ControllerFactory();
        string _ErrorMsg = "Some error occurs,please try again later.";
        SessionInformation _SessionInformation;

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                _SessionInformation = CommonFunctions.GetSessionInformation();

                if (_SessionInformation == null || _SessionInformation.IsLogIn != true)
                {
                    lblMsg.Text = "User is not authenticated";
                    lblMsg.Visible = true;
                }
                else
                {

                    if (!IsPostBack)
                    {
                        if (File.Exists(ConfigurationManager.AppSettings[CommonConstants.ConfigPolicyFileLocation]))
                        {
                            divPolicy.InnerHtml = File.ReadAllText(ConfigurationManager.AppSettings[CommonConstants.ConfigPolicyFileLocation], Encoding.UTF8);
                        }

                        pnlPolicy.Visible = true;
                    }
                    else
                    {
                        pnlPolicy.Visible = false;
                    }

                    lblMsg.Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }

        protected void BtnPolicyContinue_Click(object sender, EventArgs e)
        {
            try
            {
                if (chkPolicy.Checked)
                {
                    IArchiveNMDownloadController _IArchiveNMDownloadController = _ControllerFactory.CreateObject<IArchiveNMDownloadController>();

                    if (_SessionInformation.ListOfSelectedArchiveNMDownload != null && _SessionInformation.ListOfSelectedArchiveNMDownload.Count > 0)
                    {

                        XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                          new XElement("ArticleDownload",
                           from _ArtileID in _SessionInformation.ListOfSelectedArchiveNMDownload
                           select new XElement("Article",
                           new XAttribute("ArticleID", _ArtileID)
                               )));

                        string _Result = _IArchiveNMDownloadController.InsertList(new Guid(_SessionInformation.CustomerGUID), new SqlXml(xmlDocument.CreateReader()));
                        Logger.Info("ClipDownLoad Insert Result : " + _Result);
                    }

                    BindArticleDownloadGrids();

                    pnlDownload.Visible = true;
                }
                else
                {
                    lblMsg.Text = "Please accept terms and conditions.";
                    lblMsg.Visible = true;

                    pnlPolicy.Visible = true;
                }
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }

        protected void ImgBtnDelete_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "DeleteArticle")
                {
                    IArchiveNMDownloadController _IArchiveNMDownloadController = _ControllerFactory.CreateObject<IArchiveNMDownloadController>();

                    _IArchiveNMDownloadController.DeactivateArticle(new Guid(e.CommandArgument.ToString()));

                    BindArticleDownloadGrids();
                }
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }

        protected void LbtnArticleTitle_Command(object sender, CommandEventArgs e)
        {
            try
            {
                LinkButton _LinkButton = (LinkButton)sender;
                GridViewRow _GridViewRow = _LinkButton.NamingContainer as GridViewRow;


                string _Name = _LinkButton.Text;
                string _FileName = Convert.ToString(e.CommandArgument);
                string _Formate = _FileName.Substring(_FileName.LastIndexOf('.'));
                string _FileNamewithFormate = _Name + _Formate;
                FileInfo _FileInfo = new FileInfo(_FileName.Trim());

                // Checking if file exists
                if (_FileInfo.Exists && _FileInfo.Length > 0)
                {
                    // Clear the content of the response

                    IArchiveNMDownloadController _IArchiveNMDownloadController = _ControllerFactory.CreateObject<IArchiveNMDownloadController>();

                    string _Result = _IArchiveNMDownloadController.UpdateDownloadStatus(Convert.ToInt64(grvDownload.DataKeys[_GridViewRow.RowIndex].Values["ID"].ToString()), 3,string.Empty);

                    if (!string.IsNullOrEmpty(_Result) && CommonFunctions.GetIntValue(_Result) != null && CommonFunctions.GetIntValue(_Result) > 0)
                    {
                        Response.ClearContent();

                        // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + _FileNamewithFormate + "\"");

                        // Add the file size into the response header
                        Response.AddHeader("Content-Length", _FileInfo.Length.ToString());

                        // Set the ContentType
                        Response.ContentType = "application/pdf";

                        // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                        Response.WriteFile(_FileInfo.FullName);

                        // End the response
                        Response.End();

                        //Server.Transfer(Server.MapPath("~/"), true);
                        /*Response.Flush();*/
                        //HttpContext.Current.ApplicationInstance.CompleteRequest();
                    }
                }
                else
                {
                    Logger.Debug("File not exists: " + _FileName);
                    lblMsg.Text = "File not found at :" + _FileName;
                    lblMsg.Visible = true;
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }

        protected void BindArticleDownloadGrids()
        {
            try
            {

                IArchiveNMDownloadController _IArchiveNMDownloadController = _ControllerFactory.CreateObject<IArchiveNMDownloadController>();

                List<ArchiveNMDownload> _ListOfClipDownload = _IArchiveNMDownloadController.GetByCustomerGuid(new Guid(_SessionInformation.CustomerGUID));

                List<ArchiveNMDownload> _ListOfArchiveNMDownloadPendingRequests = null;
                List<ArchiveNMDownload> _ListOfArchiveNMDownloadReady = null;

                if (_ListOfClipDownload != null)
                {
                    
                    _ListOfArchiveNMDownloadPendingRequests = _ListOfClipDownload.FindAll(delegate(ArchiveNMDownload _ClipDownload)
                    {
                        return _ClipDownload.DownloadStatus == 1;
                    });

                    _ListOfArchiveNMDownloadReady = _ListOfClipDownload.FindAll(delegate(ArchiveNMDownload _ClipDownload)
                    {
                        return _ClipDownload.DownloadStatus == 2;
                    });
                }

                grvPendingRequests.DataSource = _ListOfArchiveNMDownloadPendingRequests;
                grvPendingRequests.DataBind();


                grvDownload.DataSource = _ListOfArchiveNMDownloadReady;
                grvDownload.DataBind();

                
                if (grvPendingRequests.Rows.Count > 0 || grvDownload.Rows.Count > 0)
                {
                    BtnRefresh.Visible = true;
                }
                else
                {
                    BtnRefresh.Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                throw;
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {

                IArchiveNMDownloadController _IArchiveNMDownloadController = _ControllerFactory.CreateObject<IArchiveNMDownloadController>();
                XDocument xDocArticleID = new XDocument();
                XElement rootElement = new XElement("root");

                foreach (GridViewRow _GridViewRow in grvPendingRequests.Rows)
                {
                    string _ArticleID = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["ArticleID"].ToString();


                    XElement ArticleElement = new XElement("Article", new XAttribute("ArticleID", _ArticleID));
                    rootElement.Add(ArticleElement);
                }
                xDocArticleID.Add(rootElement);

                List<ArchiveNMDownload> resultDataset = _IArchiveNMDownloadController.GetArticleFileLocationAndStatus(xDocArticleID.ToString());

                Boolean IsServiceCall = false;

                foreach (GridViewRow _GridViewRow in grvPendingRequests.Rows)
                {

                    string _ArticleDownloadID = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["ID"].ToString();

                    string _ArticleID = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["ArticleID"].ToString();

                    string fileLocation = string.Empty;

                    if (resultDataset != null)
                    {
                        var filterdData = resultDataset.Where(x => x.ArticleID.Equals(_ArticleID)).FirstOrDefault();
                        if (filterdData != null)
                        {

                            if (!string.IsNullOrWhiteSpace(filterdData.FileLocation))
                                fileLocation = filterdData.FileLocation;
                        }

                        if (!string.IsNullOrWhiteSpace(Convert.ToString(fileLocation)))
                        {
                            FileInfo fileInfo = new FileInfo(fileLocation);
                            if (fileInfo.Exists)
                            {
                                if (fileInfo.Length > 0)
                                {
                                    _IArchiveNMDownloadController.UpdateDownloadStatus(Convert.ToInt64(_ArticleDownloadID), 2,fileLocation);
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(filterdData.PdfSvcStatus) && (filterdData.PdfSvcStatus != "Generated" && filterdData.PdfSvcStatus != "IN_PROCESS" && filterdData.PdfSvcStatus != "QUEUED"))
                        {
                            IsServiceCall = true;
                            IArchiveNMController _IArchiveNMController = _ControllerFactory.CreateObject<IArchiveNMController>();
                            _IArchiveNMController.UpdateIQCoreNMStatus(_ArticleID);

                        }
                    }
                }

                if (IsServiceCall)
                {
                    var newsGeneratePDFsvc = new NewsGeneratePDFWebServiceClient();
                    newsGeneratePDFsvc.WakeupService();
                }

                BindArticleDownloadGrids();
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }
    }
}