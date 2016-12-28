using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using IQMediaGroup.Admin.Controller.Interface;
using IQMediaGroup.Admin.Controller.Factory;
using IQMediaGroup.Admin.Core.HelperClasses;

namespace IQMediaGroup.Admin.Usercontrol.Base
{
    public partial class BaseControl : System.Web.UI.UserControl
    {
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();
        private static string _Ascending = "ASC";
        private static string _Decending = "DESC";
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        
        /// <summary>
        ///Descriptin:This method will write expections in DataBase.
        ///Added By:Maulik Gandhi
        /// </summary>
        public void WriteException(Exception _Exception)
        {
            IQMediaGroup.Admin.Core.HelperClasses.IQMediaGroupExceptions _IQMediaGroupExceptions = new IQMediaGroup.Admin.Core.HelperClasses.IQMediaGroupExceptions();
            _IQMediaGroupExceptions.ExceptionStackTrace = _Exception.StackTrace;
            _IQMediaGroupExceptions.ExceptionMessage = _Exception.Message;
            _IQMediaGroupExceptions.CreatedBy = "Base - Write Exception";
            _IQMediaGroupExceptions.ModifiedBy = "Base - Write Exception";

            string _ReturnValue = string.Empty;
            IIQMediaGroupExceptionsController _IIQMediaGroupExceptionsController = _ControllerFactory.CreateObject<IIQMediaGroupExceptionsController>();
            _ReturnValue = _IIQMediaGroupExceptionsController.AddIQMediaGroupException(_IQMediaGroupExceptions);
        }

        /// <summary>
        /// Descriptin:Fills Combo From DataSet.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <param name="p_DropDownList">The Dropdown list that we need to fill</param>
        /// <param name="p_ListofDataItems">Object from which we need to bind the dropdown</param>
        /// <param name="p_DataValueField">Value Iteam for Dropdown. For example ID that will be bind in background for record</param>
        /// <param name="p_DataTextField">Text value that will appear in List of dropdown</param>
        /// <param name="p_DisplayText">Text that we need to display for selected iteam</param>
        /// <param name="p_SelectedItem">Iteam that we need to make by default selected in state</param>
        public static void FillComboFromDataSet(ref DropDownList p_DropDownList, object p_ListofDataItems, string p_DataValueField, string p_DataTextField, string p_DisplayText, string p_SelectedItem)
        {
            try
            {
                p_DropDownList.DataSource = p_ListofDataItems;
                p_DropDownList.DataTextField = p_DataTextField;
                p_DropDownList.DataValueField = p_DataValueField;
                p_DropDownList.DataBind();
                if (p_DisplayText != "")
                {
                    ListItem _LiDefault = new ListItem();
                    _LiDefault.Text = p_DisplayText;
                    _LiDefault.Value = "";
                    p_DropDownList.Items.Insert(0, _LiDefault);
                }
                if (p_SelectedItem != "")
                {
                    p_DropDownList.SelectedValue = p_SelectedItem;
                }
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description:This function Get Direction.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="column">column on which sorting happen</param>
        /// <returns>Sorting Direction.</returns>
        public static string GetGridSortDirection(string _column)
        {
            try
            {
                string _sortDirection = _Ascending;
                IQMediaGroup.Admin.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Admin.Core.HelperClasses.CommonFunctions.GetSessionInformation();
                string sortExpression = _SessionInformation.SortExpression as string;
                if (sortExpression != null)
                {
                    if (sortExpression == _column)
                    {
                        string lastDirection = _SessionInformation.SortDirection as string;
                        if ((lastDirection != null) && (lastDirection == _Ascending))
                        {
                            _sortDirection = _Decending;
                        }
                    }
                }
                _SessionInformation.SortDirection = _sortDirection;

                _SessionInformation.SortExpression = _column;

                return _sortDirection;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// This method used for Add grid image data.
        /// </summary>
        public static void AddGridSortImage(GridView gv, GridViewRow headerRow)
        {
            int index = -1;
            foreach (DataControlField field in gv.Columns)
            {
                if (field.SortExpression == (string)HttpContext.Current.Session["SortExpression"])
                {
                    index = gv.Columns.IndexOf(field);
                    break;
                }
            }
            if (index != -1)
            {
                for (int i = 0; i < headerRow.Cells.Count; i++)
                {
                    if (headerRow.Cells[i] is DataControlFieldCell)
                    {
                        DataControlField field = ((DataControlFieldCell)headerRow.Cells[i]).ContainingField;

                        if (field.SortExpression != null && field.SortExpression == HttpContext.Current.Session["SortExpression"].ToString())
                        {
                            Image sortImage = new Image();
                            if (HttpContext.Current.Session["SortDirection"].ToString() == "ASC")
                            {
                                sortImage.ImageUrl = "~/Images/arrowup.gif";
                                sortImage.AlternateText = "Ascending Order";
                            }
                            else
                            {
                                sortImage.ImageUrl = "~/Images/arrowdown.gif";
                                sortImage.AlternateText = "Descending Order";
                            }
                            // Add the image to the appropriate header cell.
                            headerRow.Cells[i].Controls.Add(sortImage);
                            break;
                        }
                    }
                }
            }
            // Create the sorting image based on the sort direction.


        }

        /// <summary>
        /// This method used for sorting grid data.
        /// </summary>
        public static DataTable GridSorting(DataTable dt, string se)
        {
            dt.DefaultView.Sort = se + " " + GetGridSortDirection(se);
            return dt;
        }

        /// <summary>
        /// This method used for Updating panel.
        /// </summary>
        public void UpdateUpdatePanel(UpdatePanel p_UpdatePanel)
        {
            if (p_UpdatePanel.UpdateMode == UpdatePanelUpdateMode.Conditional)
            {
                p_UpdatePanel.Update();
            }
        }

        /// <summary>
        /// Description: This method will set the ViewState Informaiton.
        /// Added By:Maulik Gandhi
        /// </summary>
        /// <param name="p_ViewstateInformation">Contains the ViewstateInformation that needs to be set in Viewstate.</param>
        public void SetViewstateInformation(IQMediaGroup.Admin.Core.HelperClasses.ViewstateInformation p_ViewstateInformation)
        {
            try
            {
                ViewState["ViewstateInformation"] = p_ViewstateInformation;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Description: This method Gets the ViewState information from session.
        /// Added By: Maulik Gandhi
        /// </summary>
        /// <returns>SessionInformation Object</returns>
        public IQMediaGroup.Admin.Core.HelperClasses.ViewstateInformation GetViewstateInformation()
        {
            ViewstateInformation _ViewstateInformation  = null;

            try
            {
                _ViewstateInformation = (ViewstateInformation)ViewState["ViewstateInformation"];

                if (_ViewstateInformation == null) _ViewstateInformation = new ViewstateInformation();
            }
            catch (Exception _Exception)
            {

                throw _Exception;
            }

            return _ViewstateInformation;
        }

        /// <summary>
        /// this method generate bread crumb string.
        /// </summary>
        /// <param name="p_BreadCrumb"></param>
        public void GenerateBreadCrumb(string p_BreadCrumb)
        {
            Label lblBreadCrumb = this.Page.Master.FindControl("lblBreadCrumb") as Label;
            if (lblBreadCrumb != null)
            {
                lblBreadCrumb.Text = p_BreadCrumb;
            }
        }
    }
}