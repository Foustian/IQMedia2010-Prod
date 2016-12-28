using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text;
using System.Net;
using System.Configuration;

namespace IQMediaGroup.AdvanceSearchServiceSample
{
    public partial class AdvanceSearchService : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                drpService.SelectedIndex = 0;
                drpCase.Enabled = false;
                drpCase.SelectedIndex = 0;
                StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetSSPData.txt"));
                StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetSSPData.txt"));
                txtDescription.Text = fp.ReadToEnd();
                txtRequest.Text = fpRequest.ReadToEnd();
                fp.Close();
                fp.Dispose();
                fpRequest.Close();
                fpRequest.Dispose();
                txtURL.Text = ConfigurationManager.AppSettings["GetSSPData"];
                //Screen.HRef = "~/ServiceScreenShot/Screen.JPG";
                Screen.Visible = false;

            }
        }

        protected void drpService_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtResponse.Text = "";
                if (drpService.SelectedValue == "GetSSPData")
                {
                    drpCase.Items[0].Enabled = true;
                    drpCase.SelectedIndex = 0;
                    drpCase.Enabled = false;
                    //txtDescription.Text = "Service1";
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetSSPData.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetSSPData.txt"));
                    txtDescription.Text = fp.ReadToEnd();
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    txtURL.Text = ConfigurationManager.AppSettings["GetSSPData"];
                    Screen.HRef = Server.MapPath("~/ServiceScreenShot/Screen.JPG");
                    Screen.Visible = false;

                }
                else if (drpService.SelectedValue == "GetRawMedia")
                {
                    drpCase.Items[0].Enabled = false;
                    drpCase.SelectedIndex = 1;
                    drpCase.Enabled = true;
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetRawMedia_Case1.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetRawMedia_Case1.txt"));
                    txtDescription.Text = fp.ReadToEnd();
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    txtURL.Text = ConfigurationManager.AppSettings["GetRawMedia"];
                    Screen.Visible = true;
                    Screen.HRef = "~/ServiceScreenShot/Case1.JPG";
                }
                else if (drpService.SelectedValue == "GetRadioStation")
                {
                    drpCase.Items[0].Enabled = true;
                    drpCase.SelectedIndex = 0;
                    drpCase.Enabled = false;
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetRadioStation.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetRadioStation.txt"));
                    txtDescription.Text = fp.ReadToEnd();

                    txtURL.Text = ConfigurationManager.AppSettings["GetRadioStation"];
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    Screen.Visible = false;

                }
                else
                {
                    drpCase.Items[0].Enabled = true;
                    drpCase.SelectedIndex = 0;
                    drpCase.Enabled = false;
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetRadioRawMedia.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetRadioRawMedia.txt"));
                    txtDescription.Text = fp.ReadToEnd();
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    txtURL.Text = ConfigurationManager.AppSettings["GetRadioRawMedia"];
                    Screen.Visible = false;
                }
            }
            catch (Exception _Exception)
            {

                txtResponse.Text = _Exception.Message + "_______" + "______________" + _Exception.StackTrace;
            }
        }

        protected void drpCase_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                txtResponse.Text = "";
                if (drpCase.SelectedIndex == 0)
                {
                    drpService.SelectedIndex = 0;
                    drpCase.Enabled = false;
                    /*txtDescription.Text = "Service1";
                    txtRequest.Text = "Request1";*/
                }
                else if (drpCase.SelectedIndex == 1)
                {
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetRawMedia_Case1.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetRawMedia_Case1.txt"));
                    txtDescription.Text = fp.ReadToEnd();
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    txtURL.Text = ConfigurationManager.AppSettings["GetRawMedia"];
                    Screen.Visible = true;
                    Screen.HRef = "~/ServiceScreenShot/Case1.JPG";
                }
                else if (drpCase.SelectedIndex == 2)
                {
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetRawMedia_Case2.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetRawMedia_Case2.txt"));
                    txtDescription.Text = fp.ReadToEnd();
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    txtURL.Text = ConfigurationManager.AppSettings["GetRawMedia"];
                    Screen.Visible = true;
                    Screen.HRef = "~/ServiceScreenShot/Case2.JPG";
                }
                else
                {
                    StreamReader fp = File.OpenText(Server.MapPath("~/ServiceDescription/GetRawMedia_Case3.txt"));
                    StreamReader fpRequest = File.OpenText(Server.MapPath("~/ServiceRequest/GetRawMedia_Case3.txt"));
                    txtDescription.Text = fp.ReadToEnd();
                    txtRequest.Text = fpRequest.ReadToEnd();
                    fp.Close();
                    fp.Dispose();
                    fpRequest.Close();
                    fpRequest.Dispose();
                    txtURL.Text = ConfigurationManager.AppSettings["GetRawMedia"];
                    Screen.Visible = true;
                    Screen.HRef = "~/ServiceScreenShot/Case3.JPG";
                }


            }
            catch (Exception _Exception)
            {

                txtResponse.Text = _Exception.Message + "_______" + "______________" + _Exception.StackTrace;
            }
        }

        protected void btnGo_Click(object sender, EventArgs e)
        {
            try
            {
                Uri _Uri = new Uri(txtURL.Text);
                //Uri _Uri = new Uri("http://localhost:2274/isvc/Statskedprog/GetRawMedia/");
           
                //Uri _Uri = new Uri("http://qaservices.iqmediacorp.com/isvc/Statskedprog/Getdata/");
                //string JsonInput = "{\"SearchTerm\":\"\",\"Title120\":\"\",\"Desc100\":\"\",\"FromDate\":\"\\/Date(1292956200000)\\/\",\"ToDate\":\"\\/Date(1293042599000)\\/\",\"IQ_Cat_Set\":[{\"IQ_Cat_Num\":21},{\"IQ_Cat_Num\":22},{\"IQ_Cat_Num\":23}],\"IQ_Class_Set\":[{\"IQ_Class_Num\":\"00\"},{\"IQ_Class_Num\":\"01\"},{\"IQ_Class_Num\":\"02\"},{\"IQ_Class_Num\":\"03\"},{\"IQ_Class_Num\":\"04\"},{\"IQ_Class_Num\":\"05\"},{\"IQ_Class_Num\":\"06\"},{\"IQ_Class_Num\":\"07\"},{\"IQ_Class_Num\":\"08\"},{\"IQ_Class_Num\":\"09\"},{\"IQ_Class_Num\":\"10\"}],\"IQ_Dma_Set\":[{\"IQ_Dma_Num\":\"000\"},{\"IQ_Dma_Num\":\"001\"},{\"IQ_Dma_Num\":\"002\"}],\"Station_Affil_Set\":[{\"Station_Affil_Num\":51},{\"Station_Affil_Num\":52},{\"Station_Affil_Num\":53}],\"IQ_Time_Zone\":\"\",\"PageSize\":10,\"PageNumber\":1,\"SortField\":\"\",\"IsSortAscending\":true}";
                // string JsonInput = "{\"SearchTerm\":\"obama\",\"Title120\":\"\",\"Desc100\":\"\",\"FromDate\":\"\\/Date(1292956200000)\\/\",\"ToDate\":\"\\/Date(1293042600000)\\/\",\"IQ_Cat_Set\":[{\"IQ_Cat_Num\":\"21\"}, {\"IQ_Cat_Num\":\"22\"}, {\"IQ_Cat_Num\":\"23\"}],\"IQ_Class_Set\":[{\"IQ_Class_Num\":\"00\"}, {\"IQ_Class_Num\":\"01\"}, {\"IQ_Class_Num\":\"02\"}, {\"IQ_Class_Num\":\"03\"}, {\"IQ_Class_Num\":\"04\"}, {\"IQ_Class_Num\":\"05\"}, {\"IQ_Class_Num\":\"06\"}, {\"IQ_Class_Num\":\"07\"}, {\"IQ_Class_Num\":\"08\"}, {\"IQ_Class_Num\":\"09\"}, {\"IQ_Class_Num\":\"10\"}],\"IQ_Dma_Set\":[{\"IQ_Dma_Num\":\"000\"}, {\"IQ_Dma_Num\":\"001\"}, {\"IQ_Dma_Num\":\"002\"}, {\"IQ_Dma_Num\":\"003\"}, {\"IQ_Dma_Num\":\"004\"}, {\"IQ_Dma_Num\":\"005\"}, {\"IQ_Dma_Num\":\"007\"}, {\"IQ_Dma_Num\":\"008\"}, {\"IQ_Dma_Num\":\"009\"}, {\"IQ_Dma_Num\":\"010\"}, {\"IQ_Dma_Num\":\"011\"}, {\"IQ_Dma_Num\":\"012\"}, {\"IQ_Dma_Num\":\"014\"}, {\"IQ_Dma_Num\":\"015\"}, {\"IQ_Dma_Num\":\"017\"}, {\"IQ_Dma_Num\":\"018\"}, {\"IQ_Dma_Num\":\"019\"}, {\"IQ_Dma_Num\":\"021\"}, {\"IQ_Dma_Num\":\"023\"}, {\"IQ_Dma_Num\":\"025\"}, {\"IQ_Dma_Num\":\"027\"}, {\"IQ_Dma_Num\":\"028\"}, {\"IQ_Dma_Num\":\"029\"}, {\"IQ_Dma_Num\":\"030\"}, {\"IQ_Dma_Num\":\"032\"}, {\"IQ_Dma_Num\":\"033\"}, {\"IQ_Dma_Num\":\"034\"}, {\"IQ_Dma_Num\":\"037\"}, {\"IQ_Dma_Num\":\"038\"}, {\"IQ_Dma_Num\":\"039\"}, {\"IQ_Dma_Num\":\"040\"}, {\"IQ_Dma_Num\":\"048\"}, {\"IQ_Dma_Num\":\"049\"}, {\"IQ_Dma_Num\":\"050\"}, {\"IQ_Dma_Num\":\"054\"}, {\"IQ_Dma_Num\":\"058\"}, {\"IQ_Dma_Num\":\"064\"}, {\"IQ_Dma_Num\":\"065\"}, {\"IQ_Dma_Num\":\"068\"}, {\"IQ_Dma_Num\":\"070\"}, {\"IQ_Dma_Num\":\"072\"}, {\"IQ_Dma_Num\":\"081\"}, {\"IQ_Dma_Num\":\"083\"}, {\"IQ_Dma_Num\":\"085\"}, {\"IQ_Dma_Num\":\"088\"}, {\"IQ_Dma_Num\":\"099\"}],\"Station_Affil_Set\":[{\"Station_Affil_Num\":\"51\"}, {\"Station_Affil_Num\":\"52\"}, {\"Station_Affil_Num\":\"53\"}, {\"Station_Affil_Num\":\"54\"}, {\"Station_Affil_Num\":\"55\"}, {\"Station_Affil_Num\":\"56\"}, {\"Station_Affil_Num\":\"57\"}, {\"Station_Affil_Num\":\"58\"}, {\"Station_Affil_Num\":\"59\"}, {\"Station_Affil_Num\":\"60\"}, {\"Station_Affil_Num\":\"61\"}, {\"Station_Affil_Num\":\"62\"}, {\"Station_Affil_Num\":\"63\"}, {\"Station_Affil_Num\":\"64\"}, {\"Station_Affil_Num\":\"65\"}, {\"Station_Affil_Num\":\"66\"}],\"IQ_Time_Zone\":\"all\",\"PageSize\":2,\"PageNumber\":1,\"SortField\":\"market\"}";
                //string JsonInput = "{\"SearchTerm\":\"obama\",\"Title120\":\"\",\"Desc100\":\"\",\"FromDate\":\"\\/Date(1293301800000+0530)\\/\",\"ToDate\":\"\\/Date(1293409800000+0530)\\/\",\"IQ_Cat_Set\":[{\"IQ_Cat_Num\":\"21\"}, {\"IQ_Cat_Num\":\"23\"}],\"IQ_Class_Set\":[{\"IQ_Class_Num\":\"00\"}, {\"IQ_Class_Num\":\"01\"}, {\"IQ_Class_Num\":\"02\"}, {\"IQ_Class_Num\":\"03\"}, {\"IQ_Class_Num\":\"04\"}, {\"IQ_Class_Num\":\"05\"}, {\"IQ_Class_Num\":\"06\"}, {\"IQ_Class_Num\":\"07\"}, {\"IQ_Class_Num\":\"08\"}, {\"IQ_Class_Num\":\"09\"}, {\"IQ_Class_Num\":\"10\"}],\"IQ_Dma_Set\":[{\"IQ_Dma_Num\":\"000\"}, {\"IQ_Dma_Num\":\"001\"}, {\"IQ_Dma_Num\":\"002\"}, {\"IQ_Dma_Num\":\"003\"}, {\"IQ_Dma_Num\":\"004\"}, {\"IQ_Dma_Num\":\"005\"}, {\"IQ_Dma_Num\":\"007\"}, {\"IQ_Dma_Num\":\"008\"}, {\"IQ_Dma_Num\":\"009\"}, {\"IQ_Dma_Num\":\"010\"}, {\"IQ_Dma_Num\":\"011\"}, {\"IQ_Dma_Num\":\"012\"}, {\"IQ_Dma_Num\":\"014\"}, {\"IQ_Dma_Num\":\"015\"}, {\"IQ_Dma_Num\":\"017\"}, {\"IQ_Dma_Num\":\"018\"}, {\"IQ_Dma_Num\":\"019\"}, {\"IQ_Dma_Num\":\"021\"}, {\"IQ_Dma_Num\":\"023\"}, {\"IQ_Dma_Num\":\"025\"}, {\"IQ_Dma_Num\":\"027\"}, {\"IQ_Dma_Num\":\"028\"}, {\"IQ_Dma_Num\":\"029\"}, {\"IQ_Dma_Num\":\"030\"}, {\"IQ_Dma_Num\":\"032\"}, {\"IQ_Dma_Num\":\"033\"}, {\"IQ_Dma_Num\":\"034\"}, {\"IQ_Dma_Num\":\"037\"}, {\"IQ_Dma_Num\":\"038\"}, {\"IQ_Dma_Num\":\"039\"}, {\"IQ_Dma_Num\":\"040\"}, {\"IQ_Dma_Num\":\"048\"}, {\"IQ_Dma_Num\":\"049\"}, {\"IQ_Dma_Num\":\"050\"}, {\"IQ_Dma_Num\":\"054\"}, {\"IQ_Dma_Num\":\"058\"}, {\"IQ_Dma_Num\":\"064\"}, {\"IQ_Dma_Num\":\"065\"}, {\"IQ_Dma_Num\":\"068\"}, {\"IQ_Dma_Num\":\"070\"}, {\"IQ_Dma_Num\":\"072\"}, {\"IQ_Dma_Num\":\"081\"}, {\"IQ_Dma_Num\":\"083\"}, {\"IQ_Dma_Num\":\"085\"}, {\"IQ_Dma_Num\":\"088\"}, {\"IQ_Dma_Num\":\"099\"}],\"Station_Affil_Set\":[{\"Station_Affil_Num\":\"51\"}, {\"Station_Affil_Num\":\"52\"}, {\"Station_Affil_Num\":\"53\"}, {\"Station_Affil_Num\":\"54\"}, {\"Station_Affil_Num\":\"55\"}, {\"Station_Affil_Num\":\"56\"}, {\"Station_Affil_Num\":\"57\"}, {\"Station_Affil_Num\":\"58\"}, {\"Station_Affil_Num\":\"59\"}, {\"Station_Affil_Num\":\"60\"}, {\"Station_Affil_Num\":\"61\"}, {\"Station_Affil_Num\":\"62\"}, {\"Station_Affil_Num\":\"63\"}, {\"Station_Affil_Num\":\"64\"}, {\"Station_Affil_Num\":\"65\"}, {\"Station_Affil_Num\":\"66\"}],\"IQ_Time_Zone\":\"all\",\"PageSize\":10,\"PageNumber\":1,\"SortField\":\"\"}";

                string JsonInput = txtRequest.Text;
                
                ASCIIEncoding _objEncodedData = new ASCIIEncoding();
                byte[] byteArray = _objEncodedData.GetBytes(JsonInput);

                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);

                _objWebRequest.Timeout = 100000000;                  

                _objWebRequest.Method = "POST";
                _objWebRequest.ContentType = "application/json";
                _objWebRequest.ContentLength = byteArray.Length;

                Stream _objStream = _objWebRequest.GetRequestStream();
                _objStream.Write(byteArray, 0, byteArray.Length);
                _objStream.Close();

                StreamReader _StreamReader = null;
                string _ResponseRawMedia = string.Empty;

                HttpWebResponse _HttpWebResponse = (HttpWebResponse)_objWebRequest.GetResponse();

                if ((_HttpWebResponse!= null && _HttpWebResponse.ContentLength > 0))
                {
                    _StreamReader = new StreamReader(_HttpWebResponse.GetResponseStream());
                    _ResponseRawMedia = _StreamReader.ReadToEnd();
                    _StreamReader.Dispose();

                    txtResponse.Text = _ResponseRawMedia;                    
                }
            }
            catch (Exception _Exception)
            {
                /*Response.Write("error");*/
                txtResponse.Text = _Exception.Message + "_______" + "______________" + _Exception.StackTrace;
            }
        }
    }
}