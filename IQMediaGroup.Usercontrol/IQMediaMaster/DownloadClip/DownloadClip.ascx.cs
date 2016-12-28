using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Core.HelperClasses;
using System.Xml.Linq;
using System.Data.SqlTypes;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.IO;
using IQMediaGroup.Core.Enumeration;
using System.Configuration;
using System.Threading;
using IQMediaGroup.Usercontrol.Base;
using System.Text;
using System.Data;
using System.Collections;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.DownloadClip
{
    public partial class DownloadClip : BaseControl
    {
        ControllerFactory _ControllerFactory = new ControllerFactory();
        string _ErrorMsg = "Some error occurs,please try again later.";

        protected override void OnLoad(EventArgs e)
        {
            try
            {
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

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
                    SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                    IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();

                    if (_SessionInformation.ListOfSelectedClipsFDownlLoad != null && _SessionInformation.ListOfSelectedClipsFDownlLoad.Count > 0)
                    {

                        XDocument xmlDocument = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                          new XElement("ClipDownload",
                           from _ClipGUID in _SessionInformation.ListOfSelectedClipsFDownlLoad
                           select new XElement("Clip",
                           new XAttribute("ClipID", _ClipGUID)
                               )));

                        string _Result = _IClipDownloadController.Insert(new Guid(_SessionInformation.CustomerGUID), new SqlXml(xmlDocument.CreateReader()));
                        Logger.Info("ClipDownLoad Insert Result : " + _Result);
                    }

                    BindClipDownloadGrids();

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
                IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();

                _IClipDownloadController.DeactivateClip(new Guid(e.CommandArgument.ToString()));

                BindClipDownloadGrids();
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }

        protected void BtnFormat_Click(object sender, EventArgs e)
        {
            try
            {
                XDocument xDocument = new XDocument();
                XElement rootElement = new XElement("root");

                foreach (GridViewRow _GridViewRow in grvFormat.Rows)
                {

                    string _IQ_ClipDownload_Key = grvFormat.DataKeys[_GridViewRow.RowIndex].Values["IQ_ClipDownload_Key"].ToString();
                    string _ClipID = grvFormat.DataKeys[_GridViewRow.RowIndex].Values["ClipID"].ToString();
                    string _Format = ((DropDownList)_GridViewRow.FindControl("ddlFormat")).SelectedValue;


                    XElement clipElement = new XElement("Clip", new XAttribute("guid", new Guid(_ClipID)), new XAttribute("IQClipDownloadKey", _IQ_ClipDownload_Key));
                    rootElement.Add(clipElement);
                }
                
                xDocument.Add(rootElement);

                XDocument xmlDocument = new XDocument();
                xmlDocument.Declaration = new XDeclaration("1.0", "UTF-8", "yes");

                XElement _XElement = new XElement("ClipDownload");
                xmlDocument.Add(_XElement);

                IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();
                bool isServiceInProgress = false;


                _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();
                List<ClipMeta> resultDataset = _IClipDownloadController.GetFileLocationFromClipMeta(xDocument.ToString());

                foreach (GridViewRow _GridViewRow in grvFormat.Rows)
                {
                    string _IQ_ClipDownload_Key = grvFormat.DataKeys[_GridViewRow.RowIndex].Values["IQ_ClipDownload_Key"].ToString();
                    string _ClipID = grvFormat.DataKeys[_GridViewRow.RowIndex].Values["ClipID"].ToString();
                    string _Format = ((DropDownList)_GridViewRow.FindControl("ddlFormat")).SelectedValue;
                    string fileLocation = string.Empty;
                    
                    
                    if (resultDataset != null)
                    {
                        var filterdData = resultDataset.Where(x => x.clipGUID.Equals(new Guid(_ClipID))).FirstOrDefault();
                        if (filterdData != null)
                        {
                            if (!string.IsNullOrWhiteSpace(filterdData.Location))
                                fileLocation = filterdData.Location;
                            if (!string.IsNullOrWhiteSpace(filterdData.UGCLocation))
                                fileLocation = filterdData.UGCLocation;
                            Logger.Debug("File location: " + (!string.IsNullOrWhiteSpace(fileLocation) ? fileLocation : "No FileLocation Found From IQCore_ClipMeta"));

                        }
                    }

                    if(string.IsNullOrWhiteSpace(fileLocation))
                    {
                        Logger.Debug("Lets Check in Config ClipDownload Location");
                        fileLocation = ConfigurationManager.AppSettings[CommonConstants.ConfigClip_Download_Location];
                    }

                    FileInfo fileInfo = new FileInfo(fileLocation + _ClipID + "." + _Format);
                    if (fileInfo.Exists)
                    {
                        Logger.Debug("Clop Download File Exists");
                        if (fileInfo.Length > 0)
                        {
                            XElement _XElementInner = new XElement("Clip", new XAttribute("IQ_ClipDownload_Key", _IQ_ClipDownload_Key), new XAttribute("ClipDownloadStatus", 3), new XAttribute("ClipDLFormat", _Format), new XAttribute("ClipFileLocation", fileLocation));
                            _XElement.Add(_XElementInner);
                        }
                        else
                        {
                            XElement _XElementInner = new XElement("Clip", new XAttribute("IQ_ClipDownload_Key", _IQ_ClipDownload_Key), new XAttribute("ClipDownloadStatus", 2), new XAttribute("ClipDLFormat", _Format), new XAttribute("ClipFileLocation", fileLocation));
                            _XElement.Add(_XElementInner);

                        }
                    }
                    else
                    {
                        Logger.Debug("Clop Download File Does not Exists");
                        _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();
                        isServiceInProgress = _IClipDownloadController.CheckForExistingStatusOfService(new Guid(_ClipID), _Format);
                        if (!isServiceInProgress)
                        {
                            Logger.Debug("Lets Make Clip Export Request");
                            bool hasRequest = MakeClipExportRequest(_ClipID, _Format);
                            if (hasRequest)
                            {
                                XElement _XElementInner = new XElement("Clip", new XAttribute("IQ_ClipDownload_Key", _IQ_ClipDownload_Key), new XAttribute("ClipDownloadStatus", 2), new XAttribute("ClipDLFormat", _Format), new XAttribute("ClipFileLocation", fileLocation));
                                _XElement.Add(_XElementInner);
                            }
                        }
                        else
                        {
                            Logger.Debug("Clip Export Request is On Progess");
                            XElement _XElementInner = new XElement("Clip", new XAttribute("IQ_ClipDownload_Key", _IQ_ClipDownload_Key), new XAttribute("ClipDownloadStatus", 2), new XAttribute("ClipDLFormat", _Format), new XAttribute("ClipFileLocation", fileLocation));
                            _XElement.Add(_XElementInner);
                        }
                    }
                }
                if (_XElement.HasElements)
                {
                    _IClipDownloadController.Update(new SqlXml(xmlDocument.CreateReader()));
                }


                BindClipDownloadGrids();
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                this.WriteException(_Exception);
                lblMsg.Text = _ErrorMsg;
                lblMsg.Visible = true;
            }
        }

        private static bool MakeClipExportRequest(string _ClipID, string _Format)
        {
            try
            {
                string _URL = ConfigurationManager.AppSettings[CommonConstants.ConfigExportClip] + "?fid=" + _ClipID + "&fmt=" + _Format;
                string _Response = CommonFunctions.GetHttpResponse(_URL);
                Logger.Debug("Response For ClipID:" + _ClipID + " Response:" + _Response);
                if (_Response.Trim().ToLower() == ConfigurationManager.AppSettings[CommonConstants.ConfigExportClipMsg].Trim().ToLower())
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception _Exception)
            {
                Logger.Error(_Exception);
                throw;
            }
        }


        private void BindClipDownloadGrids()
        {
            try
            {

                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();
                IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();

                List<ClipDownload> _ListOfClipDownload = _IClipDownloadController.SelectByCustomer(new Guid(_SessionInformation.CustomerGUID));

                List<ClipDownload> _ListOfClipDownloadFormat = null;
                List<ClipDownload> _ListOfClipDownloadPendingRequests = null;
                List<ClipDownload> _ListOfClipDownloadReady = null;

                if (_ListOfClipDownload != null)
                {
                    _ListOfClipDownloadFormat = _ListOfClipDownload.FindAll(delegate(ClipDownload _ClipDownload)
                            {
                                return _ClipDownload.ClipDownloadStatus == 1;
                            });

                    _ListOfClipDownloadPendingRequests = _ListOfClipDownload.FindAll(delegate(ClipDownload _ClipDownload)
                    {
                        return _ClipDownload.ClipDownloadStatus == 2;
                    });

                    _ListOfClipDownloadReady = _ListOfClipDownload.FindAll(delegate(ClipDownload _ClipDownload)
                    {
                        return _ClipDownload.ClipDownloadStatus == 3;
                    });
                }


                grvFormat.DataSource = _ListOfClipDownloadFormat;
                grvFormat.DataBind();



                grvPendingRequests.DataSource = _ListOfClipDownloadPendingRequests;
                grvPendingRequests.DataBind();


                grvDownload.DataSource = _ListOfClipDownloadReady;
                grvDownload.DataBind();

                if (grvFormat.Rows.Count > 0)
                {
                    BtnFormat.Visible = true;
                    trNote.Visible = true;
                }
                else
                {
                    BtnFormat.Visible = false;
                    trNote.Visible = false;
                }

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

        protected void LbtnClipTitle_Command(object sender, CommandEventArgs e)
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

                    IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();

                    string _Result = _IClipDownloadController.UpdateClipDownloadStatus(Convert.ToInt64(grvDownload.DataKeys[_GridViewRow.RowIndex].Values["IQ_ClipDownload_Key"].ToString()), 4, string.Empty);

                    if (!string.IsNullOrEmpty(_Result) && CommonFunctions.GetIntValue(_Result) != null && CommonFunctions.GetIntValue(_Result) > 0)
                    {
                        Response.ClearContent();

                        // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + _FileNamewithFormate + "\"");

                        // Add the file size into the response header
                        Response.AddHeader("Content-Length", _FileInfo.Length.ToString());

                        // Set the ContentType
                        Response.ContentType = ReturnExtension(_FileInfo.Extension.ToLower());

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

        private static string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                case ".wmv":
                    return "video/wmv";
                case ".mp4":
                    return "video/mp4";
                default:
                    return "application/octet-stream";
            }
        }

        protected void BtnRefresh_Click(object sender, EventArgs e)
        {
            try
            {

                IClipDownloadController _IClipDownloadController = _ControllerFactory.CreateObject<IClipDownloadController>();
                XDocument xDocClipID = new XDocument();
                XElement rootElement = new XElement("root");

                foreach (GridViewRow _GridViewRow in grvPendingRequests.Rows)
                {
                    string _ClipID = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["ClipID"].ToString();
                    string _IQ_ClipDownload_Key = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["IQ_ClipDownload_Key"].ToString();



                    XElement clipElement = new XElement("Clip", new XAttribute("guid", new Guid(_ClipID)), new XAttribute("IQClipDownloadKey", _IQ_ClipDownload_Key));
                    rootElement.Add(clipElement);
                }
                xDocClipID.Add(rootElement);

                List<ClipMeta> resultDataset = _IClipDownloadController.GetFileLocationFromClipMeta(xDocClipID.ToString());

                foreach (GridViewRow _GridViewRow in grvPendingRequests.Rows)
                {

                    string _IQ_ClipDownload_Key = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["IQ_ClipDownload_Key"].ToString();

                    string _ClipID = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["ClipID"].ToString();

                    string fileName = grvPendingRequests.DataKeys[_GridViewRow.RowIndex].Values["ClipID"].ToString() + "." + _GridViewRow.Cells[2].Text;

                    string fileLocation = string.Empty;
                    
                    if (resultDataset != null)
                    {
                        var filterdData = resultDataset.Where(x => x.clipGUID.Equals(new Guid(_ClipID))).FirstOrDefault();
                        if (filterdData != null)
                        {

                            if (!string.IsNullOrWhiteSpace(filterdData.Location))
                                fileLocation = filterdData.Location;
                            if (!string.IsNullOrWhiteSpace(filterdData.UGCLocation))
                                fileLocation = filterdData.UGCLocation;
                        }
                    }

                    if(string.IsNullOrWhiteSpace(Convert.ToString(fileLocation)))
                    {
                        fileLocation = ConfigurationManager.AppSettings[CommonConstants.ConfigClip_Download_Location];
                    }

                    if (!string.IsNullOrWhiteSpace(Convert.ToString(fileLocation)))
                    {
                        FileInfo fileInfo = new FileInfo(fileLocation + fileName);
                        if (fileInfo.Exists)
                        {
                            if (fileInfo.Length > 0)
                            {
                                _IClipDownloadController.UpdateClipDownloadStatus(Convert.ToInt64(_IQ_ClipDownload_Key), 3, fileLocation);
                            }
                        }
                    }
                }

                BindClipDownloadGrids();
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