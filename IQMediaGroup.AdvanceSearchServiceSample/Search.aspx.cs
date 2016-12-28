using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Runtime.Serialization;
using System.Configuration;
using System.Web.UI.HtmlControls;
using IQMediaGroup.AdvanceSearchServiceSample.Class;


namespace IQMediaGroup.AdvanceSearchServiceSample
{
    public partial class Search : System.Web.UI.Page
    {

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {

            try
            {
                lblMessage.Visible = false;
                if (Session["SessionAdvanceSearch"] == null)
                {
                    //lnkblogout.Visible = false;
                    tbllogin.Visible = true;
                    tbldata.Visible = false;
                }
                else
                {
                    //lnkblogout.Visible = true;
                    //BindDate();
                    if (!IsPostBack)
                    {
                        BindDate();
                        if (Convert.ToInt16(rblTvOrRadio.SelectedValue) == 0)
                        {
                            BindStatSkedProgData();
                            rptRadioStations.Visible = false;
                            rdoRadioStations.Visible = false;
                        }
                        else
                        {
                            GetRadioStation();
                            ScriptManager.RegisterStartupScript(rptRadioStations, rptRadioStations.GetType(), "rptRadioStations", "CheckUnCheckAll(true,'" + rptRadioStations.ClientID + "');", true);
                            rptRadioStations.Visible = true;
                            rdoRadioStations.Visible = true;
                        }
                        tblvideo.Visible = false;
                        upVideo.Update();

                        tblGrid.Visible = false;
                        upGrid.Update();

                        tblResponse.Visible = false;
                        upResponse.Update();
                    }
                    tbldata.Visible = true;
                    tbllogin.Visible = false;
                    ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                    ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                    ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);


                    //if (!IsPostBack)
                    // {
                    // BindStatSkedProgData();

                    //}
                }


                rdoMarket.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "')");
                rdoMarket.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptMarket.ClientID + "')");

                rdoAffil.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "')");
                rdoAffil.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptAffil.ClientID + "')");

                rdoProgramType.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "')");
                rdoProgramType.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptProgramType.ClientID + "')");

                rdoRadioStations.Items[0].Attributes.Add("onclick", "CheckUnCheckAll(true,'" + rptRadioStations.ClientID + "')");
                rdoRadioStations.Items[1].Attributes.Add("onclick", "CheckUnCheckAll(false,'" + rptRadioStations.ClientID + "')");
            }
            catch (Exception _Exception)
            {
                lblMessage.Text = "An error occurred, please try again.";
            }

            //ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
            //ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
            //ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
            //rptRadioStations.Visible = false;
            //rdoRadioStations.Visible = false;

        }

        #endregion

        #region events

        protected void lbtnRawMediaPlay_Command(object sender, CommandEventArgs e)
        {
            try
            {
               // Clipframe.Attributes.Add("src", "http://qa.iqmediacorp.com/IFrameRawMedia/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + txtSearch.Text + "&IsUGC=false");

                //Clipframe.Attributes.Add("src", "http://" + Request.Url.Host.ToString() + "/IFrameRawMedia/Default.aspx?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + txtSearch.Text + "&IsUGC=false");

                Clipframe.Attributes.Add("src", ConfigurationManager.AppSettings["IframeURL"]+"?RawMediaID=" + e.CommandArgument.ToString() + "&SearchTerm=" + txtSearch.Text + "&IsUGC=false");
                Clipframe.Visible = true;
                upVideo.Update();

                tblvideo.Visible = true;
                upVideo.Update();



            }
            catch (Exception _Exception)
            {
                lblMessage.Text = "An error occurred, please try again.";
            }
        }
        protected void grvRawMediaPMGBasic_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        protected void cvFromDate_ServerValidate(object source, ServerValidateEventArgs args)
        {
            try
            {
                if (Convert.ToDateTime(txtFromDate.Text) > System.DateTime.Now)
                {
                    args.IsValid = false;
                }
                else
                {
                    args.IsValid = true;
                }
            }
            catch (Exception _Exception)
            {
                //this.WriteException(_Exception);
                lblMessage.Text = "An error occurred, please try again.";
            }
        }

        protected void rblTvOrRadio_SelectedIndexChanged(Object sender, EventArgs e)
        {
            if (Convert.ToInt16(rblTvOrRadio.SelectedValue) == 0)
            {
                trInitialSearch.Visible = true;
                trAffiliateNetwork.Visible = true;
                //trProgramCategory.Visible = true;
                trProgramSubCategory.Visible = true;
                trTimeZone.Visible = true;
                rptRadioStations.Visible = true;
                rptMarket.Visible = true;
                rdoMarket.Visible = true;

                rptRadioStations.Visible = false;
                rdoRadioStations.Visible = false;

                //pnlRadioStations.Visible = false;
                rdoMarket.SelectedIndex = 0;
                rdoAffil.SelectedIndex = 0;
                rdoProgramType.SelectedIndex = 0;
                ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptMarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
            }
            else
            {
                trInitialSearch.Visible = false;
                trAffiliateNetwork.Visible = false;
                //trProgramCategory.Visible = false;
                trProgramSubCategory.Visible = false;
                trTimeZone.Visible = false;

                GetRadioStation();

                ScriptManager.RegisterStartupScript(rptRadioStations, rptRadioStations.GetType(), "rptRadioStations", "CheckUnCheckAll(true,'" + rptRadioStations.ClientID + "');", true);
                rptRadioStations.Visible = true;
                rdoRadioStations.Visible = true;
                rdoMarket.Visible = false;
                rptMarket.Visible = false;
                rptRadioStations.Enabled = false;
            }

            tblGrid.Visible = false;
            upGrid.Update();

            tblResponse.Visible = false;
            upResponse.Update();


        }

        protected void grvRadioStations_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            string tt;

        }

        protected void grvRadioStations_Sorting(object sender, GridViewSortEventArgs e)
        {

        }


        #endregion

        #region Button Events

        protected void imgRawMediaNext_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentSearch _objCurrentSearch = (CurrentSearch)Session["CurrentSearch"];
                GetRawMedia(Convert.ToInt16(Convert.ToInt16(_objCurrentSearch.PageNumber) + 1), Convert.ToString(_objCurrentSearch.Sortfield));
            }
            catch (Exception ex)
            {

            }
        }

        protected void imgRawMediaPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentSearch _objCurrentSearch = (CurrentSearch)Session["CurrentSearch"];
                GetRawMedia(Convert.ToInt16(Convert.ToInt16(_objCurrentSearch.PageNumber) - 1), Convert.ToString(_objCurrentSearch.Sortfield));
            }
            catch (Exception ex)
            {

            }

        }

        protected void imgbtnradioprv_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentSearch _objCurrentSearch = (CurrentSearch)Session["CurrentSearch"];
                GetRadioRawMedia(Convert.ToInt16(Convert.ToInt16(_objCurrentSearch.PageNumber) - 1), Convert.ToString(_objCurrentSearch.Sortfield));
            }
            catch (Exception ex)
            {


            }
        }

        protected void imgbtnradionxt_Click(object sender, EventArgs e)
        {
            try
            {
                CurrentSearch _objCurrentSearch = (CurrentSearch)Session["CurrentSearch"];
                GetRadioRawMedia(Convert.ToInt16(Convert.ToInt16(_objCurrentSearch.PageNumber) + 1), Convert.ToString(_objCurrentSearch.Sortfield));
            }
            catch (Exception ex)
            {


            }
        }


        protected void btnlogin_click(object sender, EventArgs e)
        {
            try
            {
                //IQMediaGroup.Core.HelperClasses.SessionInformation _SessionInformation = IQMediaGroup.Core.HelperClasses.CommonFunctions.GetSessionInformation();

                Authenticate _objauthenticate = AuthenticateUser();
                if (_objauthenticate != null)
                {
                    if (_objauthenticate.SessionID != null)
                    {
                        Session["SessionAdvanceSearch"] = _objauthenticate.SessionID;
                        BindDate();
                        BindStatSkedProgData();
                        tbldata.Visible = true;
                        tbllogin.Visible = false;
                        ScriptManager.RegisterStartupScript(rptMarket, rptMarket.GetType(), "rptmarket", "CheckUnCheckAll(true,'" + rptMarket.ClientID + "');", true);
                        ScriptManager.RegisterStartupScript(rptAffil, rptAffil.GetType(), "rptAffil", "CheckUnCheckAll(true,'" + rptAffil.ClientID + "');", true);
                        ScriptManager.RegisterStartupScript(rptProgramType, rptProgramType.GetType(), "rptProgramType", "CheckUnCheckAll(true,'" + rptProgramType.ClientID + "');", true);
                        rptRadioStations.Visible = false;
                        rdoRadioStations.Visible = false;

                        tblvideo.Visible = false;
                        upVideo.Update();

                        tblGrid.Visible = false;
                        upGrid.Update();

                        tblResponse.Visible = false;
                        upResponse.Update();

                        //lnkblogout.Visible = true;
                        //uplogout.Update();
                    }
                    else
                    {
                        Session["SessionAdvanceSearch"] = null;
                        lblMessage.Text = "UserID/Password does not match";
                        lblMessage.Visible = true;
                    }
                }
                else
                {
                    Session["SessionAdvanceSearch"] = null;
                    lblMessage.Text = "UserID/Password does not match";
                    lblMessage.Visible = true;
                }

            }
            catch (Exception ex)
            {
                Session["SessionAdvanceSearch"] = null;
                lblMessage.Text = "An error occurred, please try again";
                lblMessage.Visible = true;
            }

        }

        private void BindDate()
        {
            try
            {

                int _CurrentTime = DateTime.Now.Hour;
                int? _FromTime = null;
                int? _ToTime = null;
                if (_CurrentTime > 12)
                {
                    _FromTime = _CurrentTime - 12;
                    rdAMPMToDate.SelectedValue = "24";
                }
                else
                {
                    _FromTime = _CurrentTime;
                    rdAMPMToDate.SelectedValue = "12";
                }
                _ToTime = _FromTime - 1;
                ddlEndTime.SelectedValue = _ToTime.ToString();

                txtFromDate.Text = DateTime.Today.AddDays(-1).ToShortDateString();
                txtToDate.Text = System.DateTime.Now.ToShortDateString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {


            // ValidateFromDateTime();

            // if (Page.IsValid)
            // {

            if (Convert.ToInt16(rblTvOrRadio.SelectedValue) == 0)
            {
                GetRawMedia(1, string.Empty);
            }
            else
            {
                GetRadioRawMedia(1, string.Empty);
            }

            tblGrid.Visible = true;
            upGrid.Update();

            tblResponse.Visible = true;
            upResponse.Update();


            //}

        }


        //protected void lnkblogout_click(object sender, EventArgs e)
        //{
        //    HttpContext.Current.Session.Abandon();
        //    Session["SessionAdvanceSearch"] = null;
        //    lnkblogout.Visible = false;
        //    tbldata.Visible = false;
        //    tbllogin.Visible = true;
        //    //uplogout.Update();
        //}





        #endregion

        #region Functions
        private void DisplayJson(string JsonInput, string JsonOutput)
        {
            try
            {
                txtRequest.ReadOnly = false;
                txtResponse.ReadOnly = false;
                txtRequest.Text = JsonInput;
                txtResponse.Text = JsonOutput;
                txtRequest.ReadOnly = true;
                txtResponse.ReadOnly = true;
                upjsonreponse.Update();
            }
            catch (Exception ex)
            {

            }
        }

        private Authenticate AuthenticateUser()
        {
            try
            {
                Uri _Uri;

                _Uri = new Uri(ConfigurationManager.AppSettings["serviceURL"] + "/Isvc/Login");


                //string JsonInput = "{\"UserID\":\"lv@probusys.com\",\"Password\":\"probeMe34\",\"SessionID\":\"\"}";
                string JsonInput = "{\"UserID\":\"" + txtUserID.Text.Trim() + "\",\"Password\":\"" + txtPassword.Text.Trim() + "\",\"SessionID\":\"\"}";

                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(JsonInput);

                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);
                _objWebRequest.CookieContainer = new CookieContainer();

                System.Net.ServicePointManager.Expect100Continue = false;

                _objWebRequest.Timeout = 100000000;

                _objWebRequest.Method = "POST";
                _objWebRequest.ContentType = "application/json";
                _objWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _objWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                HttpWebResponse _WebResponse = (HttpWebResponse)_objWebRequest.GetResponse();
                string _ResponseRawMedia = string.Empty;
                if ((_WebResponse.ContentLength > 0))
                {
                    StreamReader _StreamReader = new StreamReader(_WebResponse.GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                Authenticate _authenticate = new Authenticate();
                if (!string.IsNullOrEmpty(_ResponseRawMedia))
                    _authenticate = (Authenticate)Serializer.Deserialize(_ResponseRawMedia, _authenticate.GetType());
                return _authenticate;
            }
            catch (Exception _Exception)
            {                
                throw _Exception;
            }
            //if (_WebResponse.Cookies != null && _WebResponse.Cookies[".IQAUTH"] != null)
            //{
            //    HttpCookie _HttpCookie = new HttpCookie(".IQAUTH");

            //    _HttpCookie.Value = _WebResponse.Cookies[".IQAUTH"].Value;
            //    // _HttpCookie.Domain = ".iqmediacorp.com";

            //    //Response.Cookies.Add(_HttpCookie);

            //    Request.Cookies.Add(_HttpCookie);

            //    Response.AddHeader("Set-Cookie", ".IQAUTH=" + _WebResponse.Cookies[".IQAUTH"].Value + "; Domain=.iqmediacorp.com");
            //}
        }

        public void BindStatSkedProgData()
        {
            string url = string.Empty;
            try
            {
                url = ConfigurationManager.AppSettings["serviceURL"] + "/Isvc/Statskedprog/Getdata/";

                Uri _Uri = new Uri(url);
                string JsonInput = "{\"SessionID\":\"" + Convert.ToString(Session["SessionAdvanceSearch"]) + "\"}";

                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(JsonInput);

                HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_Uri);

                _httpWebRequest.Timeout = 100000000;

                _httpWebRequest.Method = "POST";
                _httpWebRequest.ContentType = "application/json";
                _httpWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _httpWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;
                if ((_httpWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_httpWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                RawMediaInput _objRawmediaInput = new RawMediaInput();
                //IQMediaGroup.Domain.IQ_Cat_Set _objIQ_Cat_Set = new Domain.IQ_Cat_Set();
                _objRawmediaInput = (RawMediaInput)Serializer.Deserialize(_ResponseRawMedia, _objRawmediaInput.GetType());

                if (_objRawmediaInput != null)
                {

                    if (_objRawmediaInput.Station_Affil_Set != null)
                    {
                        rptAffil.DataSource = _objRawmediaInput.Station_Affil_Set.ToList();
                        rptAffil.DataBind();
                    }

                    if (_objRawmediaInput.IQ_Dma_Set != null)
                    {
                        rptMarket.DataSource = _objRawmediaInput.IQ_Dma_Set.ToList();
                        rptMarket.DataBind();
                    }
                    if (_objRawmediaInput.IQ_Class_Set != null)
                    {
                        rptProgramType.DataSource = _objRawmediaInput.IQ_Class_Set.ToList();
                        rptProgramType.DataBind();
                    }
                }

                /* MasterStatSkedProg _MasterStatSkedProg = null;
                 IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
                 _MasterStatSkedProg = _IStatSkedProgController.GetAllDetail();

                 if (_MasterStatSkedProg._ListofMarket.Count > 0)
                 {
                     rptMarket.DataSource = _MasterStatSkedProg._ListofMarket;
                     rptMarket.DataBind();
                 }

                 if (_MasterStatSkedProg._ListofAffil.Count > 0)
                 {
                     rptAffil.DataSource = _MasterStatSkedProg._ListofAffil;
                     rptAffil.DataBind();
                 }
                 if (_MasterStatSkedProg._ListofType.Count > 0)
                 {
                     rptProgramType.DataSource = _MasterStatSkedProg._ListofType;
                     rptProgramType.DataBind();
                 }*/



            }
            catch (Exception _Exception)
            {

                //throw _Exception;
            }
        }

        private void GetRadioStation()
        {
            try
            {
                string url = string.Empty;


                url = ConfigurationManager.AppSettings["serviceURL"] + "/Isvc/RadioStation/GetRadioStation/";


                Uri _Uri = new Uri(url);
                string JsonInput = "{\"SessionID\":\"" + Convert.ToString(Session["SessionAdvanceSearch"]) + "\"}";

                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(JsonInput);

                HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_Uri);

                _httpWebRequest.Timeout = 100000000;

                _httpWebRequest.Method = "POST";
                _httpWebRequest.ContentType = "application/json";
                _httpWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _httpWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;
                if ((_httpWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_httpWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                List<RadioStationOut> _objradiostationout = new List<RadioStationOut>();

                if (!string.IsNullOrEmpty(_ResponseRawMedia))
                {
                    _objradiostationout = (List<RadioStationOut>)Serializer.Deserialize(_ResponseRawMedia, _objradiostationout.GetType());
                }

                //return _objradiostation;
                if (_objradiostationout != null && _objradiostationout.Count > 0)
                {
                    rptRadioStations.DataSource = _objradiostationout.ToList();
                    rptRadioStations.DataBind();
                }
            }
            catch (Exception ex)
            {


            }



        }

        private void ValidateFromDateTime()
        {
            if (txtFromDate.Text.Trim() != string.Empty && txtToDate.Text.Trim() != string.Empty)
            {
                string FromDate = txtFromDate.Text.Trim() + " " + ddlStartTime.SelectedValue + ":00:00" + rdAmPmFromDate.SelectedItem.Text;
                string ToDate = txtToDate.Text.Trim() + " " + ddlEndTime.SelectedValue + ":00:00" + rdAMPMToDate.SelectedItem.Text;

                if (Convert.ToDateTime(FromDate) > Convert.ToDateTime(ToDate))
                {
                    //pplEndDate.IsValid = false;
                    //pplEndDate.ErrorMessage = "To Date should not be earlier than From Date";
                }
            }
        }

        private void GetRadioRawMedia(Int16 pagenumber, string sortfield)
        {
            try
            {
                RadioRawMediaInput _objRadioRawMediaInput = new RadioRawMediaInput();
                _objRadioRawMediaInput.RadioStation = new List<RadioStationOut>();

                for (int _RadioStationsCount = 0; _RadioStationsCount < rptRadioStations.Items.Count; _RadioStationsCount++)
                {
                    RadioStationOut _objradiostation = new RadioStationOut();
                    HtmlInputCheckBox chkRadioStation = (HtmlInputCheckBox)rptRadioStations.Items[_RadioStationsCount].FindControl("chkRadioStation");
                    if (Convert.ToInt16(rdoRadioStations.SelectedValue) == 2)
                    {
                        if (chkRadioStation.Checked)
                        {

                            _objradiostation.StationID = chkRadioStation.Value;
                            _objRadioRawMediaInput.RadioStation.Add(_objradiostation);
                        }
                    }
                    else
                    {
                        _objradiostation.StationID = chkRadioStation.Value;
                        _objRadioRawMediaInput.RadioStation.Add(_objradiostation);
                    }
                }

                int _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                int _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                if (_FromTime == 24)
                {
                    _FromTime = 12;
                }

                if (_ToTime == 24)
                {
                    _ToTime = 12;
                }

                DateTime _FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime _ToDate = Convert.ToDateTime(txtToDate.Text);

                _objRadioRawMediaInput.FromDate = Convert.ToString(new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0));
                _objRadioRawMediaInput.ToDate = Convert.ToString(new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0));
                _objRadioRawMediaInput.SessionID = Convert.ToString(Session["SessionAdvanceSearch"]);
                //_objRadioRawMediaInput.SortField = "DateTime";
                if (!string.IsNullOrEmpty(sortfield))
                {
                    _objRadioRawMediaInput.SortField = sortfield;
                }
                else
                {
                    _objRadioRawMediaInput.SortField = txtSortField.Text.Trim() == string.Empty ? string.Empty : txtSortField.Text.Trim();
                }
                _objRadioRawMediaInput.PageNumber = pagenumber;
                String _responsestream = MakeHttpRequestForRadioStation(_objRadioRawMediaInput);

                RadioRawMediaOutput _objRadioRawMediaOutput = new RadioRawMediaOutput();
                _objRadioRawMediaOutput.RadioRawMedia = new List<RadioRawMedia>();
                _objRadioRawMediaOutput = (RadioRawMediaOutput)Serializer.Deserialize(_responsestream, _objRadioRawMediaOutput.GetType());

                grvRawMediaPMGBasic.Visible = false;
                grvRadioStations.Visible = true;
                CurrentSearch _objCurrentData = new CurrentSearch();
                _objCurrentData.PageNumber = pagenumber;

                if (_objRadioRawMediaOutput != null && _objRadioRawMediaOutput.RadioRawMedia.Count > 0)
                {
                    grvRadioStations.DataSource = _objRadioRawMediaOutput.RadioRawMedia.ToList();
                    grvRadioStations.DataBind();

                    SetNextPrvradio(Convert.ToInt16(pagenumber), _objRadioRawMediaOutput.HasNextPage, _objRadioRawMediaOutput.RadioRawMedia.Count);

                    _objCurrentData.TotalRecords = _objRadioRawMediaOutput.RadioRawMedia.Count;
                    _objCurrentData.HasNextPage = _objRadioRawMediaOutput.HasNextPage;

                }
                else
                {
                    grvRadioStations.DataSource = null;
                    grvRadioStations.DataBind();
                    SetNextPrvradio(pagenumber, false, 0);
                }
                Session["CurrentSearch"] = _objCurrentData;
                upGrid.Update();



            }
            catch (Exception ex)
            {


            }
        }

        private string MakeHttpRequestForRadioStation(RadioRawMediaInput _objRadioRawMediaInput)
        {
            string url = string.Empty;
            try
            {
                url = ConfigurationManager.AppSettings["serviceURL"] + "/Isvc/RadioStation/GetRadioRawMedia/";

                Uri _Uri = new Uri(url);
                string JsonInput = Serializer.Searialize(_objRadioRawMediaInput);
                JsonInput = JsonInput.Replace(@"\/", "/");
                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(JsonInput);

                HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_Uri);

                _httpWebRequest.Timeout = 100000000;

                _httpWebRequest.Method = "POST";
                _httpWebRequest.ContentType = "application/json";
                _httpWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _httpWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;
                if ((_httpWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_httpWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                DisplayJson(JsonInput, _ResponseRawMedia);
                return _ResponseRawMedia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetRawMedia(Int16 pagenumber, string Sortfield)
        {
            try
            {
                RawMediaInput _objrawmediaInput = new RawMediaInput();

                _objrawmediaInput.Appearing = txtAppearing.Text;

                //_objrawmediaInput.SortField = "DateTime;
                if (!string.IsNullOrEmpty(Sortfield))
                {
                    _objrawmediaInput.SortField = Sortfield;
                }
                else
                {
                    _objrawmediaInput.SortField = txtSortField.Text.Trim() == string.Empty ? string.Empty : txtSortField.Text.Trim();
                }
                _objrawmediaInput.SearchTerm = txtSearch.Text.Trim();
                _objrawmediaInput.Title120 = txtProgram.Text.Trim();
                _objrawmediaInput.SessionID = Convert.ToString(Session["SessionAdvanceSearch"]);
                int _FromTime = (Convert.ToInt32(rdAmPmFromDate.SelectedValue) - 12) + Convert.ToInt32(ddlStartTime.SelectedValue);
                int _ToTime = (Convert.ToInt32(rdAMPMToDate.SelectedValue) - 12) + Convert.ToInt32(ddlEndTime.SelectedValue);

                if (_FromTime == 24)
                {
                    _FromTime = 12;
                }

                if (_ToTime == 24)
                {
                    _ToTime = 12;
                }

                DateTime _FromDate = Convert.ToDateTime(txtFromDate.Text);
                DateTime _ToDate = Convert.ToDateTime(txtToDate.Text);

                _objrawmediaInput.FromDate = Convert.ToString(new DateTime(_FromDate.Year, _FromDate.Month, _FromDate.Day, _FromTime, 0, 0));
                _objrawmediaInput.ToDate = Convert.ToString(new DateTime(_ToDate.Year, _ToDate.Month, _ToDate.Day, _ToTime, 0, 0));
                _objrawmediaInput.IQ_Time_Zone = ddlTimeZone.SelectedItem.Text.ToString();


                //if (Convert.ToInt16(rdoMarket.SelectedValue) == 2)
                //{
                _objrawmediaInput.IQ_Dma_Set = new List<IQ_Dma_Set>();
                foreach (DataListItem _dlitem in rptMarket.Items)
                {
                    IQ_Dma_Set _objiqdmaset = new IQ_Dma_Set();
                    HtmlInputCheckBox _objcheckbox = (HtmlInputCheckBox)_dlitem.FindControl("chkMarket");
                    if (Convert.ToInt16(rdoMarket.SelectedValue) == 2)
                    {
                        if (_objcheckbox.Checked)
                        {
                            _objiqdmaset.IQ_Dma_Num = _objcheckbox.Value;
                            _objrawmediaInput.IQ_Dma_Set.Add(_objiqdmaset);
                        }
                    }
                    else
                    {
                        _objiqdmaset.IQ_Dma_Num = _objcheckbox.Value;
                        _objrawmediaInput.IQ_Dma_Set.Add(_objiqdmaset);
                    }
                }
                //}

                _objrawmediaInput.Station_Affil_Set = new List<Station_Affil_Set>();

                foreach (DataListItem _dlitem in rptAffil.Items)
                {
                    Station_Affil_Set _objStation_Affil_Set = new Station_Affil_Set();
                    HtmlInputCheckBox _objcheckbox = new HtmlInputCheckBox();
                    _objcheckbox = (HtmlInputCheckBox)_dlitem.FindControl("chkAffil");

                    if (Convert.ToInt16(rdoAffil.SelectedValue) == 2)
                    {

                        if (_objcheckbox.Checked)
                        {
                            _objStation_Affil_Set.Station_Affil = _objcheckbox.Value;
                            _objrawmediaInput.Station_Affil_Set.Add(_objStation_Affil_Set);
                        }
                    }
                    else
                    {
                        _objStation_Affil_Set.Station_Affil = _objcheckbox.Value;
                        _objrawmediaInput.Station_Affil_Set.Add(_objStation_Affil_Set);
                    }
                }


                _objrawmediaInput.IQ_Class_Set = new List<IQ_Class_Set>();

                foreach (DataListItem _dlitem in rptProgramType.Items)
                {
                    IQ_Class_Set _objIQ_Class_Set = new IQ_Class_Set();
                    HtmlInputCheckBox _objcheckbox = new HtmlInputCheckBox();
                    _objcheckbox = (HtmlInputCheckBox)_dlitem.FindControl("chkProgramType");

                    if (Convert.ToInt16(rdoProgramType.SelectedValue) == 2)
                    {
                        if (_objcheckbox.Checked)
                        {
                            _objIQ_Class_Set.IQ_Class_Num = _objcheckbox.Value;
                            _objrawmediaInput.IQ_Class_Set.Add(_objIQ_Class_Set);
                        }
                    }
                    else
                    {
                        _objIQ_Class_Set.IQ_Class_Num = _objcheckbox.Value;
                        _objrawmediaInput.IQ_Class_Set.Add(_objIQ_Class_Set);
                    }
                }


                _objrawmediaInput.PageNumber = pagenumber;
                string _ResponseString = MakeHttpRequest(_objrawmediaInput);
                RawMediaOutput _rawmediaoutput = new RawMediaOutput();
                _rawmediaoutput.RawMedia = new List<IQMediaGroup.AdvanceSearchServiceSample.Class.RawMedia>();
                _rawmediaoutput = (RawMediaOutput)Serializer.Deserialize(_ResponseString, _rawmediaoutput.GetType());

                grvRadioStations.Visible = false;
                grvRawMediaPMGBasic.Visible = true;

                CurrentSearch _objCurrentData = new CurrentSearch();
                _objCurrentData.PageNumber = pagenumber;
                _objCurrentData.Sortfield = _objrawmediaInput.SortField;

                if (_rawmediaoutput != null && _rawmediaoutput.RawMedia.Count > 0)
                {
                    grvRawMediaPMGBasic.DataSource = _rawmediaoutput.RawMedia.ToList();
                    grvRawMediaPMGBasic.DataBind();

                    SetNextPrv(Convert.ToInt16(_rawmediaoutput.PageNumber), _rawmediaoutput.HasNextPage, _rawmediaoutput.RawMedia.Count);

                    _objCurrentData.TotalRecords = _rawmediaoutput.RawMedia.Count;
                    _objCurrentData.HasNextPage = _rawmediaoutput.HasNextPage;

                }
                else
                {
                    grvRawMediaPMGBasic.DataSource = _rawmediaoutput.RawMedia.ToList();
                    grvRawMediaPMGBasic.DataBind();
                    SetNextPrv(pagenumber, false, 0);

                }
                Session["CurrentSearch"] = _objCurrentData;
                upGrid.Update();
            }
            catch (Exception ex)
            {

            }
        }

        protected void SetNextPrv(Int16 pagenumber, bool nextpage, Int64 totalrecords)
        {
            divradiorawmediapaging.Visible = false;
            divrawmediapaging.Visible = true;
            lblCurrentPageNo.Text = "Page " + Convert.ToString(pagenumber);
            lblCurrentPageNo.Visible = false;
            imgRawMediaPrevious.Visible = false;
            imgRawMediaNext.Visible = false;

            if (totalrecords > 0)
                lblCurrentPageNo.Visible = true;

            if (nextpage)
                imgRawMediaNext.Visible = true;

            if (pagenumber > 1)
                imgRawMediaPrevious.Visible = true;

        }

        protected void SetNextPrvradio(Int16 pagenumber, bool nextpage, Int64 totalrecords)
        {
            divradiorawmediapaging.Visible = true;
            divrawmediapaging.Visible = false;
            lblradiopage.Text = "Page " + Convert.ToString(pagenumber);
            lblradiopage.Visible = false;
            imgbtnradioprv.Visible = false;
            imgbtnradionxt.Visible = false;

            if (totalrecords > 0)
                lblradiopage.Visible = true;

            if (nextpage)
                imgbtnradionxt.Visible = true;

            if (pagenumber > 1)
                imgbtnradioprv.Visible = true;

        }

        private string MakeHttpRequest(RawMediaInput _objrawmediaInput)
        {
            string url = string.Empty;
            try
            {

                url = ConfigurationManager.AppSettings["serviceURL"] + "/Isvc/Statskedprog/GetRawMedia/";

                Uri _Uri = new Uri(url);
                string JsonInput = Serializer.Searialize(_objrawmediaInput);
                JsonInput = JsonInput.Replace(@"\/", "/");
                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(JsonInput);

                HttpWebRequest _httpWebRequest = (HttpWebRequest)HttpWebRequest.Create(_Uri);

                _httpWebRequest.Timeout = 100000000;

                _httpWebRequest.Method = "POST";
                _httpWebRequest.ContentType = "application/json";
                _httpWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _httpWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                StreamReader _StreamReader = null;

                string _ResponseRawMedia = string.Empty;
                if ((_httpWebRequest.GetResponse().ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_httpWebRequest.GetResponse().GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();
                }

                DisplayJson(JsonInput, _ResponseRawMedia);
                return _ResponseRawMedia;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #endregion

        #region class

        [DataContract]
        class Authenticate
        {
            [DataMember]
            public string Message { get; set; }

            [DataMember]
            public string SessionID { get; set; }

            [DataMember]
            public string Status { get; set; }

        }
        #endregion

    }
}