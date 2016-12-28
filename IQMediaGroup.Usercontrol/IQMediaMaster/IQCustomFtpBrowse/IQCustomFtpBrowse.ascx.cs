using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FtpLib;
using IQMediaGroup.Usercontrol.Base;
using IQMediaGroup.Core.HelperClasses;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;

namespace IQMediaGroup.Usercontrol.IQMediaMaster.IQCustomFtpBrowse
{
    public partial class IQCustomFtpBrowse : BaseControl
    {
        protected string _FtpUrl = string.Empty;
        protected string _FileName = string.Empty;
        protected string _FilePath = string.Empty;

        public event EventHandler FileSelected;
        public event EventHandler FileCancel;

        public string FtpUrl
        {
            get { return (string)ViewState["FtpUrl"]; }
            set { ViewState["FtpUrl"] = value; }
        }

        public string FileName
        {
            get { return _FileName; }
            set { _FileName = value; }
        }

        public string FilePath
        {
            get { return _FilePath; }
            set { _FilePath = value; }
        }

        List<FtpDirectoryInfo> ListOfDirectory;
        List<FtpFileInfo> ListOfFiles;

        [Serializable]
        class FtpDirectoryInfo
        {
            public string Name;
            public string Path;


        }

        [Serializable]
        class FtpFileInfo
        {
            public string Name { get; set; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(FtpUrl))
                {
                    if (!Page.IsPostBack)
                    {
                        trVDir.Nodes.Add(new TreeNode("..", FtpUrl));
                        BindDir(FtpUrl, true);
                        BindFiles(FtpUrl);
                    }
                    lblerr.Visible = false;
                }
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }

        public void ReSet()
        {
            try
            {
                trVDir.CollapseAll();
                trVDir.Nodes[0].Expand();
                trVDir.Nodes[0].Selected = true;
                BindFiles(FtpUrl);
                txtFileName.Text = string.Empty;
                //upFtpBrowse.Update();
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }

            
        }

        public void BindFiles(string FtpURL)
        {
            try
            {
                string[] _files = Directory.GetFiles(FtpURL);
                ListOfFiles = (from file in _files select new FtpFileInfo { Name = file.Substring(file.LastIndexOf("\\") + 1) }).ToList();

                grdFiles.DataSource = ListOfFiles;
                grdFiles.DataBind();
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }

        public void BindDir(string FtpURL, bool IsParent)
        {
            try
            {
                string[] _directories = Directory.GetDirectories(FtpURL);
                ListOfDirectory = (from dir in _directories select new FtpDirectoryInfo { Name = dir.Substring(dir.LastIndexOf("\\") + 1), Path = dir.Substring(0, dir.LastIndexOf("\\") + 1) }).ToList();

                foreach (var ftpdin in ListOfDirectory)
                {
                    TreeNode trParent = new TreeNode();
                    trParent.Text = ftpdin.Name;
                    trParent.Value = ftpdin.Path + ftpdin.Name;
                    if (IsParent)
                        trVDir.Nodes[0].ChildNodes.Add(trParent);
                    else
                        trVDir.SelectedNode.ChildNodes.Add(trParent);
                }
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
        }

        // capturing SelectedNodeChanged even to get the directory and files in selected directory
        protected void trVDir_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                BindFiles(trVDir.SelectedValue.Trim());
                if (trVDir.SelectedNode.ChildNodes.Count <= 0)
                    BindDir(trVDir.SelectedValue.Trim(), false);
                trVDir.SelectedNode.Expand(); //expand the selected node
                txtFileName.Text = string.Empty;
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
            
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.FileName = txtFileName.Text;
                this.FilePath = trVDir.SelectedNode == null ? FtpUrl : trVDir.SelectedNode.Value;

                if (FileSelected != null)
                {
                    FileSelected(sender, e);
                }  
            }
            catch (Exception _Exception)
            {
                lblerr.Visible = true;
                lblerr.Text = "An error occured, please try again.";
                this.WriteException(_Exception);
            }
                 
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            if (FileCancel != null)
            {
                FileCancel(sender, e);
            }        
        }

        protected void lnkFilename_Click(object sender, EventArgs e)
        {
            LinkButton lnkbutton = (LinkButton)sender;
            txtFileName.Text = lnkbutton.Text;
            //upFtpBrowse.Update();
        }
    }
}