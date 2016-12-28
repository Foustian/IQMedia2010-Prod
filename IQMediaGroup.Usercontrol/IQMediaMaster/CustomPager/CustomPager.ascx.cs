using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace IQMediaGroup.Usercontrol.IQMediaMaster.CustomPager
{
    public partial class CustomPager : System.Web.UI.UserControl
    {

        private int startIndex, endIndex, lastPage;

        #region Public Property


        public int NoOfPagesToDisplay
        {
            
            get;
            set;
        }

        public Int64 TotalRecords { get; set; }
        public int PageSize { get; set; }
        public int? CurrentPage { get; set; }

        #endregion

        #region Event & Delegate declaration

        /*public delegate void PageIndexChangeEvent(object sender, int currentpageNumber);

        public event PageIndexChangeEvent _PageIndexChange;*/

        public event EventHandler _PageIndexChange;

        //public event EventHandler PageIndexChangeEventHandler
        //{
        //    add { this.Events.AddHandler(_PageIndexChange, value); }
        //    remove { this.Events.RemoveHandler(_PageIndexChange, value); }
        //}

        #endregion

        #region Function

        public void BindDataList()
        {
            try
            {

                //if (TotalRecords > 0)
                //{
                    CalculatePagingParameters();

                    List<int> ListPages = Enumerable.Range(startIndex + 1, endIndex - startIndex).ToList();

                    ListPager.RepeatColumns = ListPages.Count();
                    ListPager.DataSource = ListPages.ToList();
                    ListPager.DataBind();
                //}
                
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private void CalculatePagingParameters()
        {
            startIndex = Convert.ToInt32(CurrentPage / NoOfPagesToDisplay) * NoOfPagesToDisplay;
            endIndex = startIndex + NoOfPagesToDisplay; 
            lastPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalRecords) / PageSize));

            if (endIndex > lastPage)
            {
                endIndex = lastPage;
            }
        }

        #endregion

        #region Events

        public void ListPager_Command(object sender, CommandEventArgs e)
        {
            try
            {                

                if (_PageIndexChange != null)
                {
                    CurrentPage = Convert.ToInt32(e.CommandArgument);
                    _PageIndexChange(this, e);

                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void lbPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (_PageIndexChange != null)
                {                
                    CurrentPage = ((Convert.ToInt32(CurrentPage / NoOfPagesToDisplay) - 1) * PageSize) ;
                    _PageIndexChange(this, e);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void lbNext_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (_PageIndexChange != null)
                {
                
                    CurrentPage = ((Convert.ToInt32(CurrentPage / NoOfPagesToDisplay) + 1) * PageSize);
                    _PageIndexChange(this, e);
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        protected void ListPager_OnItemDataBound(object sender, DataListItemEventArgs e)
        {
            try
            {
                if (e.Item.ItemType == ListItemType.Item ||
                         e.Item.ItemType == ListItemType.AlternatingItem)
                {

                    LinkButton lbPageNumber = (LinkButton)e.Item.FindControl("lbPageNumber");
                    if (Convert.ToInt32(lbPageNumber.Text) == CurrentPage + 1)
                    {
                        lbPageNumber.Enabled = false;
                        lbPageNumber.CssClass = "active";
                    }
                }
                else if (e.Item.ItemType==ListItemType.Header)
                {                   
                    LinkButton lbtnFirstPage = (LinkButton)e.Item.FindControl("lbtnFirst");
                    LinkButton lbtnPreviousPage = (LinkButton)e.Item.FindControl("lbtnPrevious");

                    if (startIndex == 0)
                    {
                        lbtnFirstPage.Enabled = false;
                        lbtnPreviousPage.Enabled = false;
                        lbtnFirstPage.CssClass = "disabled";
                        lbtnPreviousPage.CssClass = "disabled";
                    }
                    else
                    {
                        lbtnPreviousPage.CommandArgument = Convert.ToString((Convert.ToInt32(CurrentPage / NoOfPagesToDisplay) - 1) * NoOfPagesToDisplay);

                        lbtnFirstPage.Enabled = true;
                        lbtnPreviousPage.Enabled = true;
                        lbtnFirstPage.CssClass = string.Empty;
                        lbtnPreviousPage.CssClass = string.Empty;

                    }
                }
                else if (e.Item.ItemType==ListItemType.Footer)
                {
                    LinkButton lbtnLastPage = (LinkButton)e.Item.FindControl("lbtnLast");
                    LinkButton lbtnNextPage = (LinkButton)e.Item.FindControl("lbtnNext");

                    lbtnLastPage.CommandArgument = Convert.ToString(lastPage - 1);
                    lbtnNextPage.CommandArgument = Convert.ToString((Convert.ToInt32(CurrentPage / NoOfPagesToDisplay) + 1) * NoOfPagesToDisplay);

                    if (lastPage > endIndex)
                    {
                        lbtnLastPage.Enabled = true;
                        lbtnNextPage.Enabled = true;

                        lbtnLastPage.CssClass = string.Empty;
                        lbtnNextPage.CssClass = string.Empty;

                    }
                    else
                    {
                        lbtnLastPage.Enabled = false;
                        lbtnNextPage.Enabled = false;

                        lbtnLastPage.CssClass = "disabled";
                        lbtnNextPage.CssClass = "disabled";
                    } 
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected override object SaveControlState()
        {
            // Invoke the base class's method and
            // get the contribution to control state
            // from the base class.
            // If the indexValue field is not zero
            // and the base class's control state is not null,
            // use Pair as a convenient data structure
            // to efficiently save 
            // (and restore in LoadControlState)
            // the two-part control state
            // and restore it in LoadControlState.

            object obj = base.SaveControlState();

            Dictionary<string, object> dicObj = new Dictionary<string, object>();
            dicObj.Add("obj", obj);


            dicObj.Add("NoOfPagesToDisplay", NoOfPagesToDisplay);
            dicObj.Add("CurrentPage", CurrentPage);


            return dicObj;

        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresControlState(this);
        }

        protected override void LoadControlState(object state)
        {
            if (state != null)
            {
                Dictionary<string, object> dicObj = state as Dictionary<string, object>;

                if (dicObj!=null)
                {
                    base.LoadControlState(dicObj["obj"]);
                    NoOfPagesToDisplay = Convert.ToInt32(dicObj["NoOfPagesToDisplay"]);
                    CurrentPage = Convert.ToInt32(dicObj["CurrentPage"]);
                }
               
            }
        }

        #endregion


    }
}