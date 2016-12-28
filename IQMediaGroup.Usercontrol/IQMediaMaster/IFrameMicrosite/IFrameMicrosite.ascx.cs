using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Core.Enumeration;
using System.Configuration;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using System.Net;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IFrameMicrosite
{
    public partial class IFrameMicrosite : BaseControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        #region Member Variables
        private int MaxRows = 5;
        private int MinRows = 2;

        private int MaxCols = 10;
        private int MinCols = 4;

        #endregion

        #region Page Events
        protected override void OnLoad(EventArgs e)
        {

            if (!Page.IsPostBack)
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                _ViewstateInformation.IsMicrositeDownload = (_IClientRoleController.GetClientRoleByClientGUIDRoleName(new Guid(Request.QueryString["ClientID"].ToString()), RolesName.MicrositeDownload.ToString()) == 1 ? true : false);
                SetViewstateInformation(_ViewstateInformation);

                GetClientClip(true);
            }
            RegisterPosbBackForDownload();

            int Cols = (Request.QueryString["Cols"] != null && (Convert.ToInt32(Request.QueryString["Cols"]) <= MaxCols && Convert.ToInt32(Request.QueryString["Cols"]) >= MinCols)) ? Convert.ToInt32(Request.QueryString["Cols"].ToString()) : MinCols;
            string _script = "$('#btmnav').width(" + (Cols * 140) + ");";
            //_script += "alert($('#btmnav').width());";
            _script += "$('#IFrameMicrosite1_upClip').width(" + (Cols * 140) + ");";
            //_script += "alert($('#IFrameMicrosite1_upClip').width());";
            _script += "$('#IFrameMicrosite1_upClip').css('margin','0 auto');";
            ScriptManager.RegisterStartupScript(this.Page, this.Page.GetType(), "SetBtn", _script, true);




            trError.Visible = false;
        }
        #endregion

        #region User Defined Events
        private void GetClientClip(bool p_IsInitialization)
        {

            try
            {
                IClientRoleController _IClientRoleController = _ControllerFactory.CreateObject<IClientRoleController>();
                if (Request.QueryString["ClientID"] != null && _IClientRoleController.GetClientRoleByClientGUIDRoleName(new Guid(Request.QueryString["ClientID"].ToString()), RolesName.IframeMicrosite.ToString()) == 1)
                {

                    IArchiveClipController _IArchiveClipController = _ControllerFactory.CreateObject<IArchiveClipController>();
                    string ClientGUID = Request.QueryString["ClientID"].ToString();
                    string ClipTitle = Request.QueryString["Title"] == null ? string.Empty : Request.QueryString["Title"].ToString();
                    string CustomerGUID = Request.QueryString["CustID"] == null ? string.Empty : "'" + Request.QueryString["CustID"].ToString().Replace(",", "','") + "'";
                    string CategoryGUID = Request.QueryString["Cat"] == null ? string.Empty : "'" + Request.QueryString["Cat"].ToString().ToLower().Replace(",", "','") + "'";
                    string SubCategory1GUID = Request.QueryString["SubCat1"] == null ? string.Empty : "'" + Request.QueryString["SubCat1"].ToString().ToLower().Replace(",", "','") + "'";
                    string SubCategory2GUID = Request.QueryString["SubCat2"] == null ? string.Empty : "'" + Request.QueryString["SubCat2"].ToString().ToLower().Replace(",", "','") + "'";
                    string SubCategory3GUID = Request.QueryString["SubCat3"] == null ? string.Empty : "'" + Request.QueryString["SubCat3"].ToString().ToLower().Replace(",", "','") + "'";
                    int Rows = (Request.QueryString["Rows"] != null && (Convert.ToInt32(Request.QueryString["Rows"]) <= MaxRows && Convert.ToInt32(Request.QueryString["Rows"]) >= MinRows)) ? Convert.ToInt32(Request.QueryString["Rows"].ToString()) : MinRows;
                    int Cols = (Request.QueryString["Cols"] != null && (Convert.ToInt32(Request.QueryString["Cols"]) <= MaxCols && Convert.ToInt32(Request.QueryString["Cols"]) >= MinCols)) ? Convert.ToInt32(Request.QueryString["Cols"].ToString()) : MinCols;
                    int _pagesize = (Rows * Cols);

                    string Sort = Request.QueryString["Sort"] == null ? string.Empty : Request.QueryString["Sort"].ToString();

                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();

                    string _searchText = string.IsNullOrEmpty(_ViewstateInformation.IFrameMicroSiteSearchText) ? string.Empty : _ViewstateInformation.IFrameMicroSiteSearchText;

                    if (Sort.Contains("-"))
                    {
                        Sort = Sort.Replace("-", "");
                        _ViewstateInformation.IsSortDirecitonAsc = false;
                    }
                    else
                    {
                        Sort = Sort.Replace("+", "");
                        _ViewstateInformation.IsSortDirecitonAsc = true;
                    }

                    MicroMyIQSortFeilds tempVar = new MicroMyIQSortFeilds();

                    if (Enum.TryParse(Sort, true, out tempVar))
                    {
                        _ViewstateInformation.ClipSortExpression = Sort;
                    }
                    else
                    {
                        _ViewstateInformation.IsSortDirecitonAsc = false;
                    }


                    if (string.IsNullOrEmpty(_ViewstateInformation.ClipSortExpression))
                    {
                        _ViewstateInformation.ClipSortExpression = "ClipCreationDate";
                    }

                    if (_ViewstateInformation._CurrentClipPage == null)
                    {
                        _ViewstateInformation._CurrentClipPage = 0;
                    }

                    int _TotalRecordsClipCount = 0;
                    Guid? _ClipID = null;
                    List<ArchiveClip> _ListOfArchiveClip = _IArchiveClipController.GetArchiveClipByParams(new Guid(ClientGUID), CategoryGUID, SubCategory1GUID, SubCategory2GUID, SubCategory3GUID, CustomerGUID, _ViewstateInformation._CurrentClipPage.Value, _pagesize, _ViewstateInformation.ClipSortExpression, _ViewstateInformation.IsSortDirecitonAsc, _searchText, ClipTitle, out _ClipID, out _TotalRecordsClipCount);
                    _ViewstateInformation.TotalRecordsCountClip = _TotalRecordsClipCount;
                    if (_ClipID != null)
                    {
                        ClipPlayerControl.DefaultClipID = _ClipID.Value;
                        ClipPlayerControl.IsMicriSite = true;
                    }
                    SetViewstateInformation(_ViewstateInformation);

                    dlistClip.RepeatColumns = Cols;
                    dlistClip.Width = Cols * 140;

                    bool _HasRecords = true;
                    int _MaxPage = Convert.ToInt32(Math.Ceiling(Convert.ToDouble(Convert.ToDouble(_ViewstateInformation.TotalRecordsCountClip) / Convert.ToDouble(_pagesize))));
                    if (_ViewstateInformation.TotalRecordsCountClip == 0)
                    {
                        _HasRecords = false;

                        _ListOfArchiveClip = new List<ArchiveClip>();
                        dlistClip.ShowFooter = true;
                        dlistClip.DataSource = _ListOfArchiveClip;
                    }
                    else
                    {
                        dlistClip.ShowFooter = false;
                        dlistClip.DataSource = _ListOfArchiveClip;
                    }
                    dlistClip.DataBind();

                    if (_HasRecords == true)
                    {
                        if (_ViewstateInformation._CurrentClipPage < (_MaxPage - 1))
                        {
                            btnNext.Visible = true;
                        }
                        else
                        {
                            btnNext.Visible = false;
                        }

                        if (_ViewstateInformation._CurrentClipPage > 0)
                        {
                            btnPrevious.Visible = true;
                        }
                        else
                        {
                            btnPrevious.Visible = false;
                        }
                        pnlError.Visible = false;
                        pnlClips.Visible = true;
                        upClip.Update();
                        upMain.Update();
                    }
                    else
                    {
                        lblErrorMsg.Text = CommonConstants.HTMLBreakLine + CommonConstants.NoResultsFound;
                        pnlError.Visible = true;
                        pnlClips.Visible = false;

                        upClip.Update();
                        upMain.Update();
                    }


                }
                else
                {

                    pnlError.Visible = true;
                    pnlClips.Visible = false;
                    lblErrorMsg.Text = "You are not authorized to view this page";
                    upClip.Update();
                    upMain.Update();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                pnlError.Visible = true;
                pnlClips.Visible = false;
                lblErrorMsg.Text = "An error occurred, please try again!!";
                upClip.Update();
                upMain.Update();
            }
        }

        protected string GetImage(string ClipThumbNailImage)
        {

            if (string.IsNullOrEmpty(ClipThumbNailImage))
            {
                return "http://" + Request.Url.Host + "/ThumbnailImage/noimage.jpg";
            }
            else
            {
                return ClipThumbNailImage;
            }
        }

        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".mp3": return "audio/mpeg3";
                case ".mp4": return "video/mp4";
                case ".mpeg": return "video/mpeg";
                case ".mov": return "video/quicktime";
                case ".wmv":
                case ".avi": return "video/x-ms-wmv";
                //and so on          
                default: return "application/octet-stream";
            }

        }

        private void RegisterPosbBackForDownload()
        {
            try
            {
                ViewstateInformation viewstateInformation = GetViewstateInformation();
                SessionInformation _SessionInformation = CommonFunctions.GetSessionInformation();

                if (dlistClip != null && dlistClip.Items.Count > 0)
                {
                    foreach (DataListItem _DataListItem in dlistClip.Items)
                    {

                        ImageButton _LinkButton = _DataListItem.FindControl("lnkDownload") as ImageButton;
                        if (_LinkButton != null)
                        {
                            Boolean qrystrDownload = false;
                            if (string.IsNullOrWhiteSpace(Request.QueryString["Download"]))
                            {
                                qrystrDownload = true;
                            }
                            else if (Request.QueryString["Download"] != null)
                            {
                                Boolean.TryParse(Request.QueryString["Download"], out qrystrDownload);
                            }
                            if (viewstateInformation.IsMicrositeDownload && qrystrDownload)
                            {
                                _LinkButton.Visible = true;
                                ScriptManager.GetCurrent(this.Page).RegisterPostBackControl(_LinkButton);
                            }
                            else
                            {
                                _LinkButton.Visible = false;
                            }
                        }

                        Image _Image = _DataListItem.FindControl("thumbClip") as Image;
                        Panel _pnlClipDetail = _DataListItem.FindControl("pnlClipDetail") as Panel;

                        if (Request.QueryString["ClipDetail"] == null || Convert.ToString(Request.QueryString["ClipDetail"]).ToLower() != "off")
                        {
                            _pnlClipDetail.Attributes.Add("onmouseout", "HideShowPopUp('" + _Image.ClientID + "','" + _pnlClipDetail.ClientID + "',0)");
                            _pnlClipDetail.Attributes.Add("onmouseover", "HideShowPopUp('" + _Image.ClientID + "','" + _pnlClipDetail.ClientID + "',1)");
                            _Image.Attributes.Add("onmouseout", "HideShowPopUp('" + _Image.ClientID + "','" + _pnlClipDetail.ClientID + "',0)");
                            _Image.Attributes.Add("onmouseover", "HideShowPopUp('" + _Image.ClientID + "','" + _pnlClipDetail.ClientID + "',1)");
                        }
                    }
                }


            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                Response.Redirect(CommonConstants.CustomErrorPage);
            }
        }

        public void WriteBytesToFile(string fileName, byte[] content)
        {
            try
            {
                System.IO.FileStream fs = new System.IO.FileStream(fileName, System.IO.FileMode.Create);
                System.IO.BinaryWriter w = new System.IO.BinaryWriter(fs);
                w.Write(content);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                pnlError.Visible = true;
                pnlClips.Visible = false;
                lblErrorMsg.Text = "An error occurred, please try again!!";
                upMain.Update();
                upClip.Update();
            }



        }
        #endregion

        #region Events

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(txtSearch.Text.Trim()))
                {

                    ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                    _ViewstateInformation._CurrentClipPage = 0;
                    _ViewstateInformation.IFrameMicroSiteSearchText = txtSearch.Text.Trim();
                    SetViewstateInformation(_ViewstateInformation);
                    lblMsg.Text = string.Empty;
                    GetClientClip(true);
                    if (_ViewstateInformation.TotalRecordsCountClip == 0)
                    {
                        lblMsg.Text = "No Result found for search Term applied.<br/><br/>";
                        trError.Visible = true;
                        pnlError.Visible = false;
                        pnlClips.Visible = true;
                        btnNext.Visible = false;
                        btnPrevious.Visible = false;
                        upMain.Update();
                    }
                }
                else
                {

                    lblMsg.Text = "Please Enter Search Term.<br/><br/>";
                    pnlError.Visible = false;
                    pnlClips.Visible = true;
                    upMain.Update();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                pnlError.Visible = true;
                lblErrorMsg.Text = "An error occurred, please try again!!";
                upClip.Update();
                upMain.Update();
            }
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            ViewstateInformation _ViewstateInformation = GetViewstateInformation();
            _ViewstateInformation._CurrentClipPage = 0;
            txtSearch.Text = string.Empty;
            _ViewstateInformation.IFrameMicroSiteSearchText = string.Empty;
            SetViewstateInformation(_ViewstateInformation);
            GetClientClip(true);
        }

        protected void btnPrevious_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation._CurrentClipPage -= 1;
                SetViewstateInformation(_ViewstateInformation);
                GetClientClip(false);

            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                pnlError.Visible = true;
                pnlClips.Visible = false;
                lblErrorMsg.Text = "An error occurred, please try again!!";
                upClip.Update();
                upMain.Update();
            }
        }

        protected void btnNext_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                ViewstateInformation _ViewstateInformation = GetViewstateInformation();
                _ViewstateInformation._CurrentClipPage += 1;
                SetViewstateInformation(_ViewstateInformation);
                GetClientClip(false);
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                pnlError.Visible = true;
                pnlClips.Visible = false;
                lblErrorMsg.Text = "An error occurred, please try again!!";
                upClip.Update();
                upMain.Update();
            }
        }

        protected void dlistClip_ItemCommand(object source, DataListCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "play")
                {
                    ClipPlayerControl.IsMicriSite = true;
                    ClipPlayerControl.InitPlayerFromMicroSite(e.CommandArgument.ToString(), new Guid(Request.QueryString["ClientID"]));
                    upClipPlayer.Update();
                }
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
                pnlError.Visible = true;
                pnlClips.Visible = false;
                lblErrorMsg.Text = "An error occurred, please try again!!";
                upClip.Update();
                upMain.Update();
            }
        }

        protected void dlistClip_ItemDataBound(object sendder, EventArgs e)
        {
            try
            {
                RegisterPosbBackForDownload();
            }
            catch (Exception _Exception)
            {
                this.WriteException(_Exception);
            }
        }

        protected void lnkDownload_Command(object sender, CommandEventArgs e)
        {
            try
            {
                string[] ClipDetail = e.CommandArgument.ToString().Split(',');

                if (ClipDetail.Length > 1)
                {

                    Guid ClipGuid = new Guid(ClipDetail[0]);
                    string ClipTitle = ClipDetail[1];

                    IIQCoreClipMetaController _IIQCoreClipMetaController = _ControllerFactory.CreateObject<IIQCoreClipMetaController>();
                    Dictionary<string, string> p_OutputPath = _IIQCoreClipMetaController.GetClipPathByClipGUID(ClipGuid);

                    if (p_OutputPath.Count > 0)
                    {
                        if (!string.IsNullOrWhiteSpace(p_OutputPath["FTPFileLocation"]))
                        {
                            _IIQCoreClipMetaController.UpdateDownloadCountByClipGUID(ClipGuid);
                            ScriptManager.RegisterClientScriptBlock(this.Page, this.Page.GetType(), "FtpDwld", "window.open('" + p_OutputPath["FTPFileLocation"] + "','_blank');", true);
                        }
                        else if (!string.IsNullOrEmpty(p_OutputPath["FilePath"]))
                        {
                            FileInfo _FileInfo = new FileInfo(p_OutputPath["FilePath"]);

                            if (_FileInfo.Exists && _FileInfo.Length > 0)
                            {
                                _IIQCoreClipMetaController.UpdateDownloadCountByClipGUID(ClipGuid);

                                ClipTitle = Regex.Replace(ClipTitle.Trim().Replace("\"", "_"), @"[\/?:*|<>, ]", "_");
                                // Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
                                Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ClipTitle + _FileInfo.Extension + "\"");

                                Response.ContentType = ReturnExtension(_FileInfo.Extension.ToLower());

                                // Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
                                Response.WriteFile(_FileInfo.FullName);

                                // End the response
                                Response.End();
                            }
                            else
                            {
                                lblMsg.Text = "Clip is temporarily unavailable for download.<br/><br/>";
                                trError.Visible = true;
                            }
                        }
                        else
                        {
                            lblMsg.Text = "Clip is temporarily unavailable for download.<br/><br/>";
                            trError.Visible = true;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Clip is temporarily unavailable for download.<br/><br/>";
                        trError.Visible = true;
                    }
                }
            }
            catch (ThreadAbortException)
            {

            }
            catch (Exception _Exception)
            {
                Response.End();
                this.WriteException(_Exception);
                lblMsg.Text = "An error occurred, please try again.<br/><br/>";
                trError.Visible = true;


            }

        }

        //protected void DowloadFile(string FileDetails)
        //{
        //    string _FilePath = string.Empty;
        //    string[] ClipDetail = FileDetails.Split(',');

        //    if (ClipDetail.Length > 1)
        //    {

        //        Guid ClipGuid = new Guid(ClipDetail[0]);
        //        string ClipTitle = ClipDetail[1];

        //        IIQCoreClipMetaController _IIQCoreClipMetaController = _ControllerFactory.CreateObject<IIQCoreClipMetaController>();
        //        _FilePath = _IIQCoreClipMetaController.GetClipPathByClipGUID(ClipGuid);
        //        _FilePath = "a.mp4";

        //        if (!string.IsNullOrEmpty(_FilePath))
        //        {

        //            //FileInfo _FileInfo = new FileInfo(_FilePath);

        //            //if (_FileInfo.Exists && _FileInfo.Length > 0)
        //            //{

        //            _IIQCoreClipMetaController.UpdateDownloadCountByClipGUID(ClipGuid);




        //            ClipTitle = Regex.Replace(ClipTitle.Trim().Replace("\"", "_"), @"[\/?:*|<>, ]", "_");
        //            System.Windows.Forms.SaveFileDialog saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
        //            saveFileDialog1.InitialDirectory = Convert.ToString(Environment.SpecialFolder.MyDocuments);
        //            saveFileDialog1.Filter = "Video File (*.EXT)|*.ext|All Files (*.*)|*.*";
        //            saveFileDialog1.FilterIndex = 1;
        //            saveFileDialog1.CheckPathExists = true;
        //            saveFileDialog1.Title = "Save Video File";
        //            saveFileDialog1.FileName = ClipTitle + "." + Path.GetExtension(_FilePath);

        //            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://192.168.1.2/" + _FilePath);
        //                request.Method = WebRequestMethods.Ftp.DownloadFile;

        //                // This example assumes the FTP site uses anonymous logon.
        //                request.Credentials = new NetworkCredential("meghana", "mr12345");

        //                FtpWebResponse response = (FtpWebResponse)request.GetResponse();

        //                Stream responseStream = response.GetResponseStream();

        //                FileStream file = File.Create(saveFileDialog1.FileName);

        //                byte[] buffer = new byte[32 * 1024];
        //                int read;

        //                while ((read = responseStream.Read(buffer, 0, buffer.Length)) > 0)
        //                {
        //                    file.Write(buffer, 0, read);
        //                }

        //                file.Close();
        //                responseStream.Close();
        //                response.Close();
        //            }

        //            //ClipTitle = Regex.Replace(ClipTitle.Trim().Replace("\"", "_"), @"[\/?:*|<>, ]", "_");
        //            //// Add the file name and attachment, which will force the open/cancel/save dialog box to show, to the header
        //            //Response.AddHeader("Content-Disposition", "attachment; filename=\"" + ClipTitle + _FileInfo.Extension + "\"");

        //            //Response.ContentType = ReturnExtension(_FileInfo.Extension.ToLower());

        //            //// Write the file into the response (TransmitFile is for ASP.NET 2.0. In ASP.NET 1.1 you have to use WriteFile instead)
        //            //Response.WriteFile(_FileInfo.FullName);

        //            //// End the response
        //            //Response.End();
        //            //}
        //            //else
        //            //{
        //            //    lblMsg.Text = "Clip is temporarily unavailable for download.<br/><br/>";
        //            //    trError.Visible = true;
        //            //}
        //        }
        //        else
        //        {
        //            lblMsg.Text = "Clip is temporarily unavailable for download.<br/><br/>";
        //            trError.Visible = true;
        //        }
        //    }
        //}

        #endregion



    }
}