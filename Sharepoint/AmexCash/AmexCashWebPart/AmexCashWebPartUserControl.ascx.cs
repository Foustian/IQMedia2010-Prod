using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using Microsoft.SharePoint;
using System.Data;
using System.Configuration;
using Microsoft.SharePoint.WebControls;
using System.Web;
using System.Globalization;
using AmexCash.Helper;
using System.Collections.Generic;
using Microsoft.SharePoint.Publishing.WebControls;



namespace AmexCash.AmexCashWebPart
{
    public partial class AmexCashWebPartUserControl : UserControl
    {
        string url = System.Configuration.ConfigurationSettings.AppSettings["SiteUrl"].ToString();
        string subSite = System.Configuration.ConfigurationSettings.AppSettings["AmexCashSubsite"].ToString();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {

                if (!IsPostBack)
                {


                    FillYearDropDown();
                    string strusergroup = string.Empty;
                    if (AmexCash.Helper.Helper.IsAdmin(url))
                    {

                        strusergroup = "Admin";
                    }
                    else
                    {
                        strusergroup = AmexCash.Helper.Helper.getGroup(url);
                    }

                    if (strusergroup.Length > 0)
                    {

                        if (strusergroup.Equals("Entry"))
                        {
                            btnAdd.Visible = true;
                        }
                        else
                        {
                            btnAdd.Visible = false;
                        }

                        ViewState["UserGroup"] = strusergroup;
                        FillGrid();
                        //bindMonthandYear();
                    }
                    else
                    {
                        butexp.Visible = false;
                        btnAdd.Visible = false;
                        tblfilter.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }

        }

        private void FillYearDropDown()
        {
            try
            {
                Int32 startYear = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["StartYear"]);
                Int32 endYear = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["EndYear"]);
                List<ListItem> listItemCollection = new List<ListItem>();
                ListItem lstItem = new ListItem("--Select Year--", "0");
                listItemCollection.Add(lstItem);
                for (int i = startYear; i <= endYear; i++)
                {
                    ListItem lstItemloop = new ListItem(i.ToString(), i.ToString());
                    listItemCollection.Add(lstItemloop);
                }
                drpyear.DataSource = listItemCollection;
                //drpyear.DataTextField = "Year";
                //drpyear.DataValueField = "Year";
                drpyear.DataBind();


            }
            catch (Exception)
            {

                throw;
            }

        }

        public void FillGrid()
        {
            try
            {



                trGrd.Visible = true;
                fsIns.Visible = false;
                tblfilter.Visible = true;
                Helper.Helper objHelper = new Helper.Helper();

                DataTable dtinvoice = objHelper.GetAmexCashData(url + subSite, drpmonth, drpyear);
                if (dtinvoice != null && dtinvoice.Rows.Count > 0)
                {
                    ViewState["CurrentTable"] = dtinvoice;
                    grdContact.PageSize = Convert.ToInt16(ConfigurationSettings.AppSettings["PageSize"]);
                    grdContact.DataSource = dtinvoice;
                    grdContact.DataBind();

                    string usergroup = Convert.ToString(ViewState["UserGroup"]);

                    if (!usergroup.Equals("Admin"))
                    {
                        grdContact.Columns[grdContact.Columns.Count - 1].Visible = false;
                    }


                    butexp.Visible = true;
                    /* DataColumn dc = new DataColumn("Seqno");
                    dc.DataType = typeof(string);
                    dtinvoice.Columns.Add(dc);

                    DataColumn dcMonth = new DataColumn("Month");
                    dcMonth.DataType = typeof(string);
                    dtinvoice.Columns.Add(dcMonth);

                    DataColumn dcYear = new DataColumn("Year");
                    dcYear.DataType = typeof(string);
                    dtinvoice.Columns.Add(dcYear);

                    for (int i = 0; i < dtinvoice.Rows.Count; i++)
                    {
                        dtinvoice.Rows[i]["Seqno"] = Convert.ToString(i + 1);
                        string entrydate = Convert.ToString(dtinvoice.Rows[i]["Billdate"]);
                        if (!string.IsNullOrEmpty(entrydate))
                        {
                            dtinvoice.Rows[i]["Month"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToDateTime(dtinvoice.Rows[i]["Billdate"]).Month);
                            dtinvoice.Rows[i]["Year"] = Convert.ToDateTime(dtinvoice.Rows[i]["Billdate"]).Year;
                        }
                        else
                        {
                            dtinvoice.Rows[i]["Month"] = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName((DateTime.Now).Month);
                            dtinvoice.Rows[i]["Year"] = Convert.ToDateTime(DateTime.Now).Year;
                        }
                    }


                    ViewState["CurrentTable"] = dtinvoice;
                    DataView view = dtinvoice.DefaultView;

                    if (drpmonth.SelectedValue != "0")
                    {

                        if (drpyear.SelectedValue != "0")
                        {
                            view.RowFilter = "Month = '" + Convert.ToString(drpmonth.SelectedItem.Text) + "' AND Year = '" + Convert.ToString(drpyear.SelectedItem.Text) + "'";
                            grdContact.DataSource = view.ToTable();
                        }
                        else
                        {
                            view.RowFilter = "Month = '" + Convert.ToString(drpmonth.SelectedItem.Text) + "'";
                            grdContact.DataSource = view.ToTable();
                        }
                    }
                    else if (drpyear.SelectedValue != "0")
                    {
                        view.RowFilter = "Year = '" + Convert.ToString(drpyear.SelectedItem.Text) + "'";
                        grdContact.DataSource = view.ToTable();
                    }

                    if (drpmonth.SelectedValue == "0" && drpyear.SelectedValue == "0")
                    {
                        grdContact.DataSource = dtinvoice;

                    }
                    grdContact.DataBind();

                    butexp.Visible = true;

                    //   grdContact.DataSource = dtinvoice;
                    //  grdContact.DataBind();*/
                }
                else
                {
                    ViewState["CurrentTable"] = null;
                    butexp.Visible = false;
                    grdContact.EmptyDataText = "No record(s) found in this view.";
                    grdContact.DataSource = null;
                    grdContact.DataBind();
                }
            }
            catch (Exception ex)
            {

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
                // String cstext1 = "<script type=\"text/javascript\">" +
                //"alert('Message: " + ex.Message + "at: " + ex.StackTrace + "');</" + "script>";
                // Page.RegisterClientScriptBlock("Popup", cstext1);
            }

        }

        /* protected void bindMonthandYear()
         {
             try
             {
                 //DataTable dt = (DataTable)ViewState["CurrentTable"];
                 //if (dt != null && dt.Rows.Count > 0)
                 //{
                 //    DataView view = new DataView(dt);
                 //    DataTable dtmonth = view.ToTable(true, "Month");
                 //    drpmonth.Items.Clear();
                 //    drpmonth.DataSource = dtmonth;
                 //    drpmonth.DataTextField = "Month";
                 //    drpmonth.DataValueField = "Month";
                 //    drpmonth.DataBind();
                 //    drpmonth.Items.Insert(0, new ListItem("--Select Month--", "0"));

                 //    DataTable dtyear = view.ToTable(true, "Year");
                 //    drpyear.Items.Clear();
                 //    drpyear.DataSource = dtyear;
                 //    drpyear.DataTextField = "Year";
                 //    drpyear.DataValueField = "Year";
                 //    drpyear.DataBind();
                 //    drpyear.Items.Insert(0, new ListItem("--Select Year--", "0"));
                 //}
             }
             catch (Exception ex)
             {
                 String cstext1 = "<script type=\"text/javascript\">" +
                "alert('Message: " + ex.Message + "at: " + ex.StackTrace + "');</" + "script>";
                 Page.RegisterClientScriptBlock("Popup", cstext1);

             }        
         }*/

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillGrid();
        }
        protected void chkapproval_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                CheckBox chkapproval = (CheckBox)sender;
                GridViewRow row = (GridViewRow)chkapproval.NamingContainer;
                Label lblpurappr = row.FindControl("lblPurchaseAppr") as Label;
                Label lblpurapprdate = row.FindControl("lblPurchaseDate") as Label;

                bool status = chkapproval.Checked;
                if (status)
                {
                    lblpurappr.Text = SPContext.Current.Web.CurrentUser.Name;
                    lblpurapprdate.Text = DateTime.Now.ToShortDateString();
                }
                else
                {
                    lblpurappr.Text = string.Empty;
                    lblpurapprdate.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }
        }

        private void InsertAmexCashData()
        {

            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(url + subSite))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            try
                            {
                                web.AllowUnsafeUpdates = true;
                                SPList list = web.Lists[ConfigurationSettings.AppSettings["AmexCashList"].ToString()];
                                SPListItem itemToAdd = list.Items.Add();

                                if (!txtbilldate.IsDateEmpty)
                                    itemToAdd["Billdate"] = txtbilldate.SelectedDate;
                                else
                                    itemToAdd["Billdate"] = DateTime.Now;


                                itemToAdd["Title"] = drpVendor.SelectedItem.Text;

                                itemToAdd["Vendor"] = new SPFieldLookupValue(Convert.ToInt32(drpVendor.SelectedValue), drpVendor.SelectedItem.Text);


                                if (!txtPurchaseDate.IsDateEmpty)
                                    itemToAdd["Purchasedate"] = txtPurchaseDate.SelectedDate;
                                else
                                    itemToAdd["Purchasedate"] = DateTime.Now;

                                itemToAdd["Amount"] = txtamount.Text;
                                itemToAdd["Link_x0020_to_x0020_Bill"] = assetSelectedImageCustomLauncher.AssetUrl;
                                itemToAdd["GLCode"] = txtGlcode.Text;

                                itemToAdd.Update();

                                web.AllowUnsafeUpdates = false;

                            }
                            catch (Exception ex)
                            {

                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }
        }

        protected void grdContact_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                ViewState["isedit"] = "T";
                grdContact.EditIndex = e.NewEditIndex;
                FillGrid();
                butexp.Visible = false;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }

        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                butexp.Visible = false;
                trGrd.Visible = false;
                fsIns.Visible = true;
                tblfilter.Visible = false;
                btnAdd.Visible = false;
                txtGlcode.Text = string.Empty;
                txtamount.Text = string.Empty;
                txtPurchaseDate.ClearSelection();
                txtbilldate.SelectedDate = DateTime.Now;

                fillvendor(drpVendor, "Y");
                assetSelectedImageCustomLauncher.AssetUrl = string.Empty;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }
        }


        private void fillvendor(DropDownList drpVendor, string Isnew)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    using (SPSite site = new SPSite(url + subSite))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            //try
                            //{
                            SPList list = web.Lists[ConfigurationSettings.AppSettings["VendorList"].ToString()];

                            DataTable dtinvoice = list.Items.GetDataTable();
                            DataView dv = dtinvoice.DefaultView;
                            dv.Sort = "Title";

                            if (dtinvoice != null && dtinvoice.Rows.Count > 0)
                            {
                                drpVendor.DataSource = dv.ToTable();
                                drpVendor.DataTextField = "Title";
                                drpVendor.DataValueField = "ID";
                                drpVendor.DataBind();
                                if (Isnew.Equals("Y"))
                                    // drpVendor.Items.Clear();
                                    drpVendor.Items.Insert(0, "--Select--");
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                InsertAmexCashData();
                FillGrid();
                btnAdd.Visible = true;
                clearcontrol();

            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }


        }

        private void clearcontrol()
        {
            //txtInvoice.Text = string.Empty;
            txtbilldate.SelectedDate = DateTime.Now;
            drpVendor.SelectedIndex = 0;
            txtPurchaseDate.ClearSelection();
            txtGlcode.Text = string.Empty;
            txtamount.Text = string.Empty;
        }


        protected void grdContact_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdContact.EditIndex = -1;
            FillGrid();
        }
        protected void grdContact_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            if (UpdateAMexCashData(e)) { grdContact.EditIndex = -1; FillGrid(); butexp.Visible = true; }
        }

        protected void grdContact_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                string usergroup = Convert.ToString(ViewState["UserGroup"]);
                /*if (e.Row.RowType == DataControlRowType.Header)
                {
                    // Label LnkHeaderText = e.Row.Cells[16].Controls[0] as Label;

                    if (usergroup.Equals("Entry") || usergroup.Equals("Purchase"))
                    {
                        
                        e.Row.Cells[11].Visible = false;
                        e.Row.Cells[grdContact.Columns.Count].Visible = false;
                    }
                    if (usergroup.Equals("Admins"))
                    {
                        e.Row.Cells[11].Visible = true;
                        e.Row.Cells[grdContact.Columns.Count].Visible = true;
                    }
                }*/


                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //Label lblSeqnum = (Label)e.Row.FindControl("lblSeqnum");
                    //lblSeqnum.Text = (e.Row.RowIndex + 1).ToString();
                    CheckBox checkactive = (CheckBox)e.Row.FindControl("checkactive1");

                    /*if (usergroup.Equals("Entry") || usergroup.Equals("Purchase"))
                    {
                        if (checkactive != null)
                        {
                            checkactive.Visible = false;
                            e.Row.Cells[grdContact.Columns.Count].Visible = false;
                        }
                    }
                    if (usergroup.Equals("Admins"))
                    {
                        if (checkactive != null)
                        {
                            checkactive.Visible = true;
                            e.Row.Cells[grdContact.Columns.Count].Visible = true;
                        }
                    }*/


                    if ((e.Row.RowState & DataControlRowState.Edit) > 0 == false)
                    {

                        Label lblPurchaseAppr = (Label)e.Row.FindControl("lblPurchaseAppr");
                        Label lblPurchasedate = (Label)e.Row.FindControl("lblPurchaseDate");
                        CheckBox chkappr = (CheckBox)e.Row.FindControl("chkapproval");
                        HyperLink hypLinktoinvoice = (HyperLink)e.Row.FindControl("hypLinktoinvoice");
                        var hyplink2invoice = DataBinder.Eval(e.Row.DataItem, "Link_x0020_to_x0020_Bill");

                        Label lblPayAppr = (Label)e.Row.FindControl("lblPurchaseAppr");
                        Label lblPaymentdate = (Label)e.Row.FindControl("lblPurchaseDate");


                        Label lblAmount = (Label)e.Row.FindControl("lblAmount");

                        Label lblGlCode = (Label)e.Row.FindControl("lblGlCode");


                        if (hyplink2invoice != null && Convert.ToString(hyplink2invoice).Trim().Length > 0)
                        {
                            SPFieldUrlValue msdnValue = new SPFieldUrlValue(DataBinder.Eval(e.Row.DataItem, "Link_x0020_to_x0020_Bill").ToString());
                            hypLinktoinvoice.Text = "Link to Bill";
                            hypLinktoinvoice.NavigateUrl = msdnValue.Url;
                        }

                        var glcode = DataBinder.Eval(e.Row.DataItem, "GLCode");
                        if (glcode != null && Convert.ToString(glcode).Trim().Length > 0)
                        {
                            //chkappr.Enabled = true;
                            lblGlCode.Text = Convert.ToString(glcode);
                        }
                        else
                        {
                            // chkappr.Enabled = false;
                            lblGlCode.Text = string.Empty;
                        }

                        var lblpurchaseappr = DataBinder.Eval(e.Row.DataItem, "Purchase_x0020_Approval");
                        var lblpurchasedate = DataBinder.Eval(e.Row.DataItem, "PurchaseApprovalDate");

                        if (lblpurchaseappr != null && Convert.ToString(lblpurchaseappr).Trim().Length > 0 && lblpurchasedate != null && Convert.ToString(lblpurchasedate).Trim().Length > 0)
                        {
                            chkappr.Checked = true;
                        }
                        else
                        {
                            chkappr.Checked = false;
                        }


                    }

                }
                /*else if (e.Row.RowType == DataControlRowType.Footer)
                {
                    if (usergroup.Equals("Entry") || usergroup.Equals("Purchase"))
                    {
                        e.Row.Cells[grdContact.Columns.Count].Visible = false;

                    }
                    if (usergroup.Equals("Admins"))
                    {
                        e.Row.Cells[grdContact.Columns.Count].Visible = true;

                    }
                }*/

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    if (Convert.ToString(ViewState["isedit"]).Equals("T"))
                    {

                        CheckBox chkappr = (CheckBox)e.Row.FindControl("chkapproval");
                        CheckBox checkactive = (CheckBox)e.Row.FindControl("checkactive");

                        DropDownList drpVen = (DropDownList)e.Row.FindControl("drpVen");

                        fillvendor(drpVen, "N");
                        string vend = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "Title"));
                        drpVen.SelectedValue = drpVen.Items.FindByText(vend).Value;


                        DateTimeControl txtBillDate = (DateTimeControl)e.Row.FindControl("txtBillDate");
                        DateTimeControl txtPurcahseDate = (DateTimeControl)e.Row.FindControl("txtPurcahseDate");


                        TextBox txtAmount = (TextBox)e.Row.FindControl("txtAmount");
                        AssetUrlSelector assetSelectedImageCustomLauncher = (AssetUrlSelector)e.Row.FindControl("assetSelectedImageCustomLauncher");
                        TextBox txtGlCode = (TextBox)e.Row.FindControl("txtGlCode");



                        Label lblPurchaseAppr = (Label)e.Row.FindControl("lblPurchaseAppr");

                        Label lblPurchaseDate = (Label)e.Row.FindControl("lblPurchaseDate");

                        if (e.Row.RowType == DataControlRowType.DataRow)
                        {
                            var Billdate = DataBinder.Eval(e.Row.DataItem, "Billdate");
                            var Purchasedate = DataBinder.Eval(e.Row.DataItem, "Purchasedate");
                            var Amount = DataBinder.Eval(e.Row.DataItem, "Amount");
                            var GLCode = DataBinder.Eval(e.Row.DataItem, "GLCode");
                            //var paymentDate = DataBinder.Eval(e.Row.DataItem, "Payment_x0020_date");
                            var Purchase_x0020_Approval = DataBinder.Eval(e.Row.DataItem, "Purchase_x0020_Approval");
                            var PurchaseApprovalDate = DataBinder.Eval(e.Row.DataItem, "PurchaseApprovalDate");

                            var asseturl = DataBinder.Eval(e.Row.DataItem, "Link_x0020_to_x0020_Bill");
                            SPFieldUrlValue msdnValue = new SPFieldUrlValue(DataBinder.Eval(e.Row.DataItem, "Link_x0020_to_x0020_Bill").ToString());
                            assetSelectedImageCustomLauncher.AssetUrl = msdnValue.Url;

                            if (GLCode != null && Convert.ToString(GLCode).Trim().Length > 0)
                            {
                                chkappr.Enabled = true;
                                txtGlcode.Text = Convert.ToString(GLCode);
                            }
                            else
                            {
                                chkappr.Enabled = false;
                                txtGlcode.Text = string.Empty;
                            }

                            if (Purchase_x0020_Approval != null && Convert.ToString(Purchase_x0020_Approval).Trim().Length > 0 && Purchase_x0020_Approval != null && Convert.ToString(Purchase_x0020_Approval).Trim().Length > 0)
                            {
                                chkappr.Checked = true;
                            }
                            else
                            {
                                chkappr.Checked = false;
                            }


                            if (Billdate != null && Convert.ToString(Billdate).Trim().Length > 0)
                            {
                                txtBillDate.SelectedDate = Convert.ToDateTime(Billdate);
                            }
                            else
                            {
                                txtBillDate.ClearSelection();
                            }
                            if (Purchasedate != null && Convert.ToString(Purchasedate).Trim().Length > 0)
                            {
                                txtPurcahseDate.SelectedDate = Convert.ToDateTime(Purchasedate);
                            }
                            else
                            {
                                txtPurcahseDate.ClearSelection();
                            }

                            if (PurchaseApprovalDate != null && Convert.ToString(PurchaseApprovalDate).Trim().Length > 0)
                            {
                                lblPurchaseDate.Text = Convert.ToDateTime(PurchaseApprovalDate).ToShortDateString();
                            }
                            else
                            {
                                lblPurchaseDate.Text = string.Empty;
                            }


                            if (usergroup.Equals("Admins"))
                            {
                                if (checkactive != null)
                                {
                                    checkactive.Visible = true;
                                    checkactive.Enabled = true;
                                }
                            }

                            if (usergroup.Equals("Entry"))
                            {

                                string entrystatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "EntryStatus"));
                                if (entrystatus.Equals("0"))
                                {
                                    drpVen.Enabled = false;
                                    txtBillDate.Enabled = false;
                                    txtPurcahseDate.Enabled = false;
                                    txtAmount.Enabled = false;
                                    txtGlCode.Enabled = false;
                                    chkappr.Enabled = false;
                                    assetSelectedImageCustomLauncher.Enabled = false;

                                }
                                else
                                {
                                    chkappr.Enabled = false;
                                }
                                if (checkactive != null)
                                {
                                    checkactive.Visible = false;
                                }

                            }
                            else if (usergroup.Equals("Purchase"))
                            {
                                string purchaseStatus = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "PurchaseStatus"));
                                if (purchaseStatus.Equals("0"))
                                {
                                    drpVen.Enabled = false;
                                    txtBillDate.Enabled = false;
                                    txtPurcahseDate.Enabled = false;
                                    txtAmount.Enabled = false;
                                    txtGlCode.Enabled = false;
                                    chkappr.Enabled = false;
                                    assetSelectedImageCustomLauncher.Enabled = false;
                                }
                                else
                                {

                                }

                                if (checkactive != null)
                                {
                                    checkactive.Visible = false;
                                }

                            }


                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }
        }

        private bool UpdateAMexCashData(GridViewUpdateEventArgs e)
        {
            try
            {
                Label lblId = (Label)grdContact.Rows[e.RowIndex].FindControl("lblId");

                DropDownList drpVen = (DropDownList)grdContact.Rows[e.RowIndex].FindControl("drpVen");

                DateTimeControl txtBillDate = (DateTimeControl)grdContact.Rows[e.RowIndex].FindControl("txtBillDate");
                DateTimeControl txtPurcahseDate = (DateTimeControl)grdContact.Rows[e.RowIndex].FindControl("txtPurcahseDate");
                TextBox txtAmount = (TextBox)grdContact.Rows[e.RowIndex].FindControl("txtAmount");
                TextBox txtGlCode = (TextBox)grdContact.Rows[e.RowIndex].FindControl("txtGlCode");

                Label lblPurchaseAppr = (Label)grdContact.Rows[e.RowIndex].FindControl("lblPurchaseAppr");
                AssetUrlSelector assetSelectedImageCustomLauncher = (AssetUrlSelector)grdContact.Rows[e.RowIndex].FindControl("assetSelectedImageCustomLauncher");
                //DateTimeControl txtPurchaseDate = (DateTimeControl)grdContact.Rows[e.RowIndex].FindControl("txtPurchaseDate");
                Label lblPurchaseDate = (Label)grdContact.Rows[e.RowIndex].FindControl("lblPurchaseDate");

                CheckBox checkactive = (CheckBox)grdContact.Rows[e.RowIndex].FindControl("checkactive");
                CheckBox chkapproval = (CheckBox)grdContact.Rows[e.RowIndex].FindControl("chkapproval");

                bool statusappr = chkapproval.Checked;
                if (chkapproval.Checked && string.IsNullOrEmpty(txtGlCode.Text.Trim()))
                {
                    ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Update Validation", "alert('GL Code can not be NULL');", true);
                    return false;
                }
                else
                {
                    SPSecurity.RunWithElevatedPrivileges(delegate()
                    {
                        using (SPSite site = new SPSite(url + subSite))
                        {
                            using (SPWeb web = site.OpenWeb())
                            {
                                try
                                {
                                    web.AllowUnsafeUpdates = true;
                                    SPList list = web.Lists[ConfigurationSettings.AppSettings["AmexCashList"].ToString()];
                                    SPListItem itemToAdd = list.GetItemById(Convert.ToInt32(lblId.Text));

                                    itemToAdd["Title"] = drpVen.SelectedItem.Text;

                                    itemToAdd["Vendor"] = new SPFieldLookupValue(Convert.ToInt32(drpVen.SelectedValue), drpVen.SelectedItem.Text);
                                    if (!txtBillDate.IsDateEmpty)
                                    {
                                        itemToAdd["Billdate"] = Convert.ToDateTime(txtBillDate.SelectedDate.ToShortDateString());
                                    }
                                    else
                                    {
                                        itemToAdd["Billdate"] = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                                    }


                                    if (!txtBillDate.IsDateEmpty)
                                    {
                                        itemToAdd["Purchasedate"] = Convert.ToDateTime(txtPurcahseDate.SelectedDate.ToShortDateString());
                                    }
                                    else
                                    {
                                        itemToAdd["Purchasedate"] = Convert.ToDateTime(DateTime.Now.ToShortDateString());
                                    }


                                    itemToAdd["Amount"] = txtAmount.Text;
                                    itemToAdd["Link_x0020_to_x0020_Bill"] = assetSelectedImageCustomLauncher.AssetUrl;
                                    itemToAdd["GLCode"] = txtGlCode.Text;


                                    if ((Convert.ToString(ViewState["UserGroup"]) == "Purchase") || (Convert.ToString(ViewState["UserGroup"]) == "Payment")
                                                || (Convert.ToString(ViewState["UserGroup"]) == "PaymentProcessing"))
                                    {
                                        if (statusappr)
                                        {
                                            // itemToAdd["EntryStatus"] = "F";
                                            itemToAdd["EntryStatus"] = 0;
                                            itemToAdd["Purchase_x0020_Approval"] = web.EnsureUser(SPContext.Current.Web.CurrentUser.Name);
                                            itemToAdd["PurchaseApprovalDate"] = DateTime.Now;
                                        }
                                        else
                                        {
                                            itemToAdd["EntryStatus"] = 1;
                                            itemToAdd["Purchase_x0020_Approval"] = null;
                                            itemToAdd["PurchaseApprovalDate"] = null;
                                        }
                                    }

                                    if (Convert.ToString(ViewState["UserGroup"]).Equals("Admin"))
                                    {
                                        if (statusappr)
                                        {
                                            itemToAdd["EntryStatus"] = 0;
                                            itemToAdd["Purchase_x0020_Approval"] = web.EnsureUser(SPContext.Current.Web.CurrentUser.Name);
                                            itemToAdd["PurchaseApprovalDate"] = DateTime.Now;
                                            //itemToAdd["PurchaseStatus"] = "F";
                                            itemToAdd["PurchaseStatus"] = 0;
                                        }
                                        else
                                        {
                                            //itemToAdd["EntryStatus"] = "F";
                                            itemToAdd["EntryStatus"] = 0;
                                            //itemToAdd["PurchaseStatus"] = "T";
                                            itemToAdd["PurchaseStatus"] = 1;
                                            itemToAdd["Purchase_x0020_Approval"] = null;
                                            itemToAdd["PurchaseApprovalDate"] = null;
                                        }

                                        if (checkactive != null)
                                        {
                                            itemToAdd["IsActive"] = (bool)checkactive.Checked == true ? 1 : 0;
                                        }

                                    }




                                    itemToAdd.Update();
                                    web.AllowUnsafeUpdates = false;

                                }
                                catch (Exception ex)
                                {

                                }
                            }
                        }
                    });
                }
                return true;
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
                return false;
            }


        }

        public DateTime BindDate(object dataitem, string column)
        {
            try
            {
                DateTime? dt = null;

                dt = new DateTime();

                string value = Convert.ToString(DataBinder.Eval(dataitem, column));

                if (!string.IsNullOrEmpty(value))
                {
                    dt = Convert.ToDateTime(value);
                }

                if (dt.HasValue)
                {
                    return dt.Value;
                }
                else
                {
                    return DateTime.MinValue;
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }
            return DateTime.MinValue;

        }

        public string BindLabel(object dataitem, string column)
        {

            string value = Convert.ToString(DataBinder.Eval(dataitem, column));
            string bindval = string.Empty;
            if (!string.IsNullOrEmpty(value))
            {
                bindval = Convert.ToDateTime(value).ToShortDateString();
            }
            return bindval;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //tblfilter.Visible = true;
            FillGrid();
            btnAdd.Visible = true;
            clearcontrol();

        }


        public void ExportToExcel(string strFileName, GridView gv)
        {
            try
            {
                using (StringWriter sw = new StringWriter())
                {
                    using (HtmlTextWriter htw = new HtmlTextWriter(sw))
                    {
                        //  Create a form to contain the grid  
                        Table table = new Table();

                        //  add the header row to the table  
                        if (gv.HeaderRow != null)
                        {
                            PrepareControlForExport(gv.HeaderRow);
                            table.Rows.Add(gv.HeaderRow);
                        }

                        //  add each of the data rows to the table  
                        foreach (GridViewRow row in gv.Rows)
                        {
                            PrepareControlForExport(row);
                            table.Rows.Add(row);
                        }

                        //  add the footer row to the table  
                        if (gv.FooterRow != null)
                        {
                            PrepareControlForExport(gv.FooterRow);
                            table.Rows.Add(gv.FooterRow);
                        }

                        //  render the table into the htmlwriter  
                        for (int i = 0; i < table.Rows.Count; i++)
                        {
                            table.Rows[i].Cells[0].Visible = false;
                            table.Rows[i].Cells[1].Visible = false;
                            if (table.Rows[i].Cells[gv.Columns.Count] != null)
                            {
                                table.Rows[i].Cells[gv.Columns.Count].Visible = false;
                            }
                        }
                        table.RenderControl(htw);

                        HttpContext.Current.Response.Clear();
                        HttpContext.Current.Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", strFileName));
                        HttpContext.Current.Response.ContentType = "application/ms-excel";

                        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                        //render the htmlwriter into the response  
                        HttpContext.Current.Response.Write(sw.ToString());
                        HttpContext.Current.Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }

        }

        private void PrepareControlForExport(Control control)
        {
            try
            {
                for (int i = 0; i < control.Controls.Count; i++)
                {
                    Control current = control.Controls[i];
                    if (current is LinkButton)
                    {
                        control.Controls.Remove(current);
                        control.Controls.AddAt(i, new LiteralControl((current as LinkButton).Text));
                    }
                    else if (current is ImageButton)
                    {
                        control.Controls.Remove(current);
                        control.Controls.AddAt(i, new LiteralControl((current as ImageButton).AlternateText));
                    }
                    else if (current is HyperLink)
                    {
                        control.Controls.Remove(current);
                        control.Controls.AddAt(i, new LiteralControl((current as HyperLink).Text));
                    }
                    else if (current is DropDownList)
                    {
                        control.Controls.Remove(current);
                        control.Controls.AddAt(i, new LiteralControl((current as DropDownList).SelectedItem.Text));
                    }
                    else if (current is CheckBox)
                    {
                        control.Controls.Remove(current);
                        control.Controls.AddAt(i, new LiteralControl((current as CheckBox).Checked ? "True" : "False"));
                    }

                    if (current.HasControls())
                    {
                        PrepareControlForExport(current);
                    }
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }

        }

        protected void butexp_Click(object sender, EventArgs e)
        {
            string strdt = DateTime.Now.ToString("yyyyMMddHHmmss");
            string filename = "AmexCash-" + strdt + ".xls";
            ExportToExcel(filename, grdContact);
        }

        protected void grdContact_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                grdContact.PageIndex = e.NewPageIndex;
                if (ViewState["CurrentTable"] != null)
                {
                    grdContact.PageSize = Convert.ToInt16(ConfigurationSettings.AppSettings["PageSize"]);
                    grdContact.DataSource = (DataTable)ViewState["CurrentTable"];
                    grdContact.DataBind();
                }
            }
            catch (Exception ex)
            {
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "Error", "alert('" + ex.Message + "');", true);
            }
        }
    }
}
