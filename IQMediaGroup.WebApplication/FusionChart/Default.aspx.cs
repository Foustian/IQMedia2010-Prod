using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.IO;
using System.Text;
using System.Xml.Linq;
using InfoSoftGlobal;
using System.Threading.Tasks;
using IQMediaGroup.Core.HelperClasses;
using IQMediaGroup.Controller.Interface;
using IQMediaGroup.Controller.Factory;
using System.Web.UI.HtmlControls;
using System.Configuration;
using IQMediaGroup.Core.Enumeration;


namespace IQMediaGroup.WebApplication.FusionChart
{
    public class affiliateZoomResponse { public string solrResponse { get; set; } public string name { get; set; } }
    public class affiliateMsLineResponse { public string solrResponse { get; set; } public string name { get; set; } }



    public partial class Default : System.Web.UI.Page
    {
        public string msg = "";

        bool isXaxisAdded = false;
        bool isXaxisAddedMsLine = false;
        string chartWidth = "800";
        string chartHeight = "550";
        List<affiliateZoomResponse> listOfAffiliateResponse = null;
        List<affiliateMsLineResponse> listOfAffiliateMsLineResponse = null;
        private readonly ControllerFactory _ControllerFactory = new ControllerFactory();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "rr", "Loadchart(<chart caption='2012-06-01 To 2012-09-27' xAxisName='' yAxisName='Hits' showValues='0' formatNumberScale='0' showBorder='1' labelDisplay='Rotate' numVisibleLabels='6'>  <categories>    <category name='2012-06-01T00:00:00Z' />    <category name='2012-06-02T00:00:00Z' />    <category name='2012-06-03T00:00:00Z' />    <category name='2012-06-04T00:00:00Z' />    <category name='2012-06-05T00:00:00Z' />    <category name='2012-06-06T00:00:00Z' />    <category name='2012-06-07T00:00:00Z' />    <category name='2012-06-08T00:00:00Z' />    <category name='2012-06-09T00:00:00Z' />    <category name='2012-06-10T00:00:00Z' />    <category name='2012-06-11T00:00:00Z' />    <category name='2012-06-12T00:00:00Z' />    <category name='2012-06-13T00:00:00Z' />    <category name='2012-06-14T00:00:00Z' />    <category name='2012-06-15T00:00:00Z' />    <category name='2012-06-16T00:00:00Z' />    <category name='2012-06-17T00:00:00Z' />    <category name='2012-06-18T00:00:00Z' />    <category name='2012-06-19T00:00:00Z' />    <category name='2012-06-20T00:00:00Z' />    <category name='2012-06-21T00:00:00Z' />    <category name='2012-06-22T00:00:00Z' />    <category name='2012-06-23T00:00:00Z' />    <category name='2012-06-24T00:00:00Z' />    <category name='2012-06-25T00:00:00Z' />    <category name='2012-06-26T00:00:00Z' />    <category name='2012-06-27T00:00:00Z' />    <category name='2012-06-28T00:00:00Z' />    <category name='2012-06-29T00:00:00Z' />    <category name='2012-06-30T00:00:00Z' />    <category name='2012-07-01T00:00:00Z' />    <category name='2012-07-02T00:00:00Z' />    <category name='2012-07-03T00:00:00Z' />    <category name='2012-07-04T00:00:00Z' />    <category name='2012-07-05T00:00:00Z' />    <category name='2012-07-06T00:00:00Z' />    <category name='2012-07-07T00:00:00Z' />    <category name='2012-07-08T00:00:00Z' />    <category name='2012-07-09T00:00:00Z' />    <category name='2012-07-10T00:00:00Z' />    <category name='2012-07-11T00:00:00Z' />    <category name='2012-07-12T00:00:00Z' />    <category name='2012-07-13T00:00:00Z' />    <category name='2012-07-14T00:00:00Z' />    <category name='2012-07-15T00:00:00Z' />    <category name='2012-07-16T00:00:00Z' />    <category name='2012-07-17T00:00:00Z' />    <category name='2012-07-18T00:00:00Z' />    <category name='2012-07-19T00:00:00Z' />    <category name='2012-07-20T00:00:00Z' />    <category name='2012-07-21T00:00:00Z' />    <category name='2012-07-22T00:00:00Z' />    <category name='2012-07-23T00:00:00Z' />    <category name='2012-07-24T00:00:00Z' />    <category name='2012-07-25T00:00:00Z' />    <category name='2012-07-26T00:00:00Z' />    <category name='2012-07-27T00:00:00Z' />    <category name='2012-07-28T00:00:00Z' />    <category name='2012-07-29T00:00:00Z' />    <category name='2012-07-30T00:00:00Z' />    <category name='2012-07-31T00:00:00Z' />    <category name='2012-08-01T00:00:00Z' />    <category name='2012-08-02T00:00:00Z' />    <category name='2012-08-03T00:00:00Z' />    <category name='2012-08-04T00:00:00Z' />    <category name='2012-08-05T00:00:00Z' />    <category name='2012-08-06T00:00:00Z' />    <category name='2012-08-07T00:00:00Z' />    <category name='2012-08-08T00:00:00Z' />    <category name='2012-08-09T00:00:00Z' />    <category name='2012-08-10T00:00:00Z' />    <category name='2012-08-11T00:00:00Z' />    <category name='2012-08-12T00:00:00Z' />    <category name='2012-08-13T00:00:00Z' />    <category name='2012-08-14T00:00:00Z' />    <category name='2012-08-15T00:00:00Z' />    <category name='2012-08-16T00:00:00Z' />    <category name='2012-08-17T00:00:00Z' />    <category name='2012-08-18T00:00:00Z' />    <category name='2012-08-19T00:00:00Z' />    <category name='2012-08-20T00:00:00Z' />    <category name='2012-08-21T00:00:00Z' />    <category name='2012-08-22T00:00:00Z' />    <category name='2012-08-23T00:00:00Z' />    <category name='2012-08-24T00:00:00Z' />    <category name='2012-08-25T00:00:00Z' />    <category name='2012-08-26T00:00:00Z' />    <category name='2012-08-27T00:00:00Z' />    <category name='2012-08-28T00:00:00Z' />    <category name='2012-08-29T00:00:00Z' />    <category name='2012-08-30T00:00:00Z' />    <category name='2012-08-31T00:00:00Z' />    <category name='2012-09-01T00:00:00Z' />    <category name='2012-09-02T00:00:00Z' />    <category name='2012-09-03T00:00:00Z' />    <category name='2012-09-04T00:00:00Z' />    <category name='2012-09-05T00:00:00Z' />    <category name='2012-09-06T00:00:00Z' />    <category name='2012-09-07T00:00:00Z' />    <category name='2012-09-08T00:00:00Z' />    <category name='2012-09-09T00:00:00Z' />    <category name='2012-09-10T00:00:00Z' />    <category name='2012-09-11T00:00:00Z' />    <category name='2012-09-12T00:00:00Z' />    <category name='2012-09-13T00:00:00Z' />    <category name='2012-09-14T00:00:00Z' />    <category name='2012-09-15T00:00:00Z' />    <category name='2012-09-16T00:00:00Z' />    <category name='2012-09-17T00:00:00Z' />    <category name='2012-09-18T00:00:00Z' />    <category name='2012-09-19T00:00:00Z' />    <category name='2012-09-20T00:00:00Z' />    <category name='2012-09-21T00:00:00Z' />    <category name='2012-09-22T00:00:00Z' />    <category name='2012-09-23T00:00:00Z' />    <category name='2012-09-24T00:00:00Z' />    <category name='2012-09-25T00:00:00Z' />    <category name='2012-09-26T00:00:00Z' />    <category name='2012-09-27T00:00:00Z' />  </categories>  <dataset seriesName='FOX'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='NBC'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='CBS'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='Entertainment'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='CW'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='ABC'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' /><set value='0' /><set value='0' /><set value='0' /></dataset></chart>)", true);

                /*lblMessage.Visible = false;
                StreamReader _streamReader = new StreamReader(@"E:\E Drive\All Projects\IQMedia - TFS\IQMediaGroup.WebApplication\FusionChart\ChartXML\201296184710826MsLine.xml");
                string xmlstring1 = _streamReader.ReadToEnd().Replace("\"", "'").Replace("\r","").Replace("\n","");
                string xmlstring = "<chart caption='2012-06-10' xAxisName='' yAxisName='Hits' showValues='0' formatNumberScale='0' showLegend='0' showBorder='1' labelDisplay='Rotate'>";
                xmlstring += "<categories><category name='2012-06-10T00:00:00Z' /><category name='2012-06-10T01:00:00Z' /><category name='2012-06-10T02:00:00Z' /><category name='2012-06-10T03:00:00Z' /><category name='2012-06-10T04:00:00Z' />";
                xmlstring += "<category name='2012-06-10T05:00:00Z' /><category name='2012-06-10T06:00:00Z' /><category name='2012-06-10T07:00:00Z' /><category name='2012-06-10T08:00:00Z' /><category name='2012-06-10T09:00:00Z' /><category name='2012-06-10T10:00:00Z' /><category name='2012-06-10T11:00:00Z' /><category name='2012-06-10T12:00:00Z' />";
                xmlstring += "<category name='2012-06-10T13:00:00Z' />";
                xmlstring += "<category name='2012-06-10T14:00:00Z' />";
                xmlstring += "<category name='2012-06-10T15:00:00Z' />";
                xmlstring += "<category name='2012-06-10T16:00:00Z' />";
                xmlstring += "<category name='2012-06-10T17:00:00Z' />";
                xmlstring += "<category name='2012-06-10T18:00:00Z' />";
                xmlstring += "<category name='2012-06-10T19:00:00Z' />";
                xmlstring += "<category name='2012-06-10T20:00:00Z' />";
                xmlstring += "<category name='2012-06-10T21:00:00Z' />";
                xmlstring += "<category name='2012-06-10T22:00:00Z' />";
                xmlstring += "<category name='2012-06-10T23:00:00Z' />";
                xmlstring += "</categories>";
                xmlstring += "<dataset seriesName='ABC'>";
                xmlstring += "<set value='23' />";
                xmlstring += "<set value='23' />";
                xmlstring += "<set value='22' />";
                xmlstring += "<set value='29' />";
                xmlstring += "<set value='28' />";
                xmlstring += "<set value='26' />";
                xmlstring += "<set value='24' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='26' />";
                xmlstring += "<set value='29' />";
                xmlstring += "<set value='29' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='31' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='26' />";
                xmlstring += "<set value='22' />";
                xmlstring += "<set value='16' />";
                xmlstring += "<set value='35' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='23' />";
                xmlstring += "<set value='12' />";
                xmlstring += "<set value='36' />";
                xmlstring += "<set value='30' />";
                xmlstring += "<set value='18' />";
                xmlstring += "</dataset>";

                xmlstring += "<dataset seriesName='hjk'>";
                xmlstring += "<set value='45' />";
                xmlstring += "<set value='66' />";
                xmlstring += "<set value='22' />";
                xmlstring += "<set value='44' />";
                xmlstring += "<set value='12' />";
                xmlstring += "<set value='25' />";
                xmlstring += "<set value='63' />";
                xmlstring += "<set value='22' />";
                xmlstring += "<set value='54' />";
                xmlstring += "<set value='11' />";
                xmlstring += "<set value='33' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='31' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='26' />";
                xmlstring += "<set value='22' />";
                xmlstring += "<set value='16' />";
                xmlstring += "<set value='35' />";
                xmlstring += "<set value='32' />";
                xmlstring += "<set value='23' />";
                xmlstring += "<set value='12' />";
                xmlstring += "<set value='36' />";
                xmlstring += "<set value='30' />";
                xmlstring += "<set value='18' />";
                xmlstring += "</dataset>";
                xmlstring += "</chart>";
                _streamReader.Close();
                _streamReader.Dispose();

                divMsLineChart.Text = FusionCharts.RenderChart(@"/fusionchart/MSLine.swf", "", xmlstring1, "divMsLineChart", chartWidth, chartHeight, false, true);*/
                //trChartMode.Visible = false;
                if (!IsPostBack)
                {
                    txtFromDate.Attributes.Add("readonly", "true");
                    txtToDate.Attributes.Add("readonly", "true");
                    txtFromDate.Text = "2012-06-01";// DateTime.Today.AddDays(-1).ToString("yyyy-MM-dd");
                    txtToDate.Text = "2012-06-10"; //System.DateTime.Now.ToString("yyyy-MM-dd");
                    BindAffiliate();

                }
            }
            catch (Exception ex)
            {

                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                listOfAffiliateResponse = new List<affiliateZoomResponse>();
                listOfAffiliateMsLineResponse = new List<affiliateMsLineResponse>();
                isXaxisAdded = false;
                isXaxisAddedMsLine = false;
                List<String> affiliateList = new List<string>();

                foreach (DataListItem _item in rptAffil.Items)
                {
                    HtmlInputCheckBox _chk = (HtmlInputCheckBox)_item.FindControl("chkAffil");
                    Label _lbl = (Label)_item.FindControl("lblAffiliateName");
                    if (_chk.Checked)
                    {
                        affiliateList.Add(_lbl.Text);
                        if (!string.IsNullOrWhiteSpace(hfAffilValue.Value))
                        {
                            hfAffilValue.Value = _lbl.Text;
                        }
                        else
                        {
                            hfAffilValue.Value = "," + _lbl.Text;
                        }
                    }
                }

                if (affiliateList != null && affiliateList.Count > 0)
                {
                    string startDate = txtFromDate.Text + "T00:00:00Z";
                    string endDate = txtToDate.Text + "T23:59:59Z";

                    Parallel.ForEach(affiliateList, item => GetResponse("http://10.100.1.42:8080/solr/core0/select?q=" + txtSearchTerm.Text + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.RL_Station_DateTime_DT.facet.range.start=" + startDate + "&facet.range=RL_Station_DateTime_DT&facet.range.end=" + endDate + "&facet.range.gap=%2B1DAY", item));
                    Parallel.ForEach(affiliateList, item => GetMsLineResponse("http://10.100.1.42:8080/solr/core0/select?q=" + txtSearchTerm.Text + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.RL_Station_DateTime_DT.facet.range.start=" + txtToDate.Text.Trim() + "T00:00:00Z" + "&facet.range=RL_Station_DateTime_DT&facet.range.end=" + txtToDate.Text.Trim() + "T23:59:59Z" + "&facet.range.gap=%2B1HOUR", item));

                    /*Parallel.ForEach(affiliateList, item => GetResponse("http://192.168.1.59:8085/solr/core0/select?q=" + txtSearchTerm.Text + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.RL_Station_DateTime_DT.facet.range.start=" + startDate + "&facet.range=RL_Station_DateTime_DT&facet.range.end=" + endDate + "&facet.range.gap=%2B1DAY", item));
                    Parallel.ForEach(affiliateList, item => GetMsLineResponse("http://192.168.1.59:8085/solr/core0/select?q=" + txtSearchTerm.Text + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.RL_Station_DateTime_DT.facet.range.start=" + txtToDate.Text.Trim() + "T00:00:00Z" + "&facet.range=RL_Station_DateTime_DT&facet.range.end=" + txtToDate.Text.Trim() + "T23:59:59Z" + "&facet.range.gap=%2B1HOUR", item));*/
                    CreateXML();
                    CreateMsLineXML();
                    //trChartMode.Visible = true;
                }
                else
                {
                    lblMessage.Text = "Select atleast one Affiliate.";
                    lblMessage.Visible = true;
                }

            }
            catch (Exception ex)
            {

                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }

        }

        public void CreateXML()
        {
            try
            {
                XDocument xdocSource = null;
                XDocument xdocFinal = new XDocument();

                if (listOfAffiliateResponse != null && listOfAffiliateResponse.Count > 0)
                {
                    xdocSource = XDocument.Parse(listOfAffiliateResponse[0].solrResponse);
                    var listXAxis = (from element in xdocSource.Descendants("response").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("int")
                                     select element.Attribute("name").Value).ToList();

                    if (!isXaxisAdded && listXAxis != null && listXAxis.Count > 0)
                    {
                        xdocFinal = new XDocument(
                            new XElement("chart", new XAttribute("caption", txtFromDate.Text + " To " + txtToDate.Text), new XAttribute("xAxisName", ""), new XAttribute("yAxisName", "Hits"), new XAttribute("showValues", "0"), new XAttribute("formatNumberScale", "0"),
                                new XAttribute("showBorder", "1"), new XAttribute("labelDisplay", "Rotate"), new XAttribute("numVisibleLabels", ConfigurationManager.AppSettings[CommonConstants.ConfigZoomChartNoOfLables]),
                               new XElement("categories",
                                        listXAxis.Select(x => new XElement("category", new XAttribute("name", x)))
                                    )
                                    )
                                );
                    }
                }

                foreach (affiliateZoomResponse response in listOfAffiliateResponse)
                {
                    xdocSource = XDocument.Parse(response.solrResponse);

                    var listYAxis = (from element in xdocSource.Descendants("response").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("int")
                                     select element.Value).ToList();



                    isXaxisAdded = true;
                    if (listYAxis != null && listYAxis.Count > 0)
                    {
                        xdocFinal.Root.Add(new XElement("dataset", new XAttribute("seriesName", response.name),
                            listYAxis.Select(y => new XElement("set", new XAttribute("value", y))))
                            );

                    }
                }

                if (!string.IsNullOrWhiteSpace(Convert.ToString(xdocFinal)))
                {
                    // string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString()
                    //      + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                    //ViewState["fileName"] = fileName;
                    //xdocFinal.Save(Server.MapPath("/FusionChart/ChartXML/" + fileName + ".xml"));
                    //string registerscript = "var myZoomChart = new FusionCharts(\"/ZoomLine.swf\", \"ZoomChart\", \"800\", \"200\");myZoomChart.setXMLData(\"" + Convert.ToString(xdocFinal).Replace("\"", "'").Replace("\r", "").Replace("\n", "") + "\");myZoomChart.render(\"divChart\"); function myChartListener(eventObject, argumentsObject)  {alert( argumentsObject.datasetName);  }FusionCharts(\"ZoomChart\").addEventListener (\"LegendItemClicked\" , myChartListener );";
                    //ScriptManager.RegisterStartupScript(this, GetType(), "renderchart", "Loadchart(" + Convert.ToString(xdocFinal).Replace("\"", "'").Replace("\r", "").Replace("\n", "") + ")", true);
                    ScriptManager.RegisterStartupScript(this, GetType(), "rr", "Loadchart(<chart caption='2012-06-01 To 2012-09-27' xAxisName='' yAxisName='Hits' showValues='0' formatNumberScale='0' showBorder='1' labelDisplay='Rotate' numVisibleLabels='6'>  <categories>    <category name='2012-06-01T00:00:00Z' />    <category name='2012-06-02T00:00:00Z' />    <category name='2012-06-03T00:00:00Z' />    <category name='2012-06-04T00:00:00Z' />    <category name='2012-06-05T00:00:00Z' />    <category name='2012-06-06T00:00:00Z' />    <category name='2012-06-07T00:00:00Z' />    <category name='2012-06-08T00:00:00Z' />    <category name='2012-06-09T00:00:00Z' />    <category name='2012-06-10T00:00:00Z' />    <category name='2012-06-11T00:00:00Z' />    <category name='2012-06-12T00:00:00Z' />    <category name='2012-06-13T00:00:00Z' />    <category name='2012-06-14T00:00:00Z' />    <category name='2012-06-15T00:00:00Z' />    <category name='2012-06-16T00:00:00Z' />    <category name='2012-06-17T00:00:00Z' />    <category name='2012-06-18T00:00:00Z' />    <category name='2012-06-19T00:00:00Z' />    <category name='2012-06-20T00:00:00Z' />    <category name='2012-06-21T00:00:00Z' />    <category name='2012-06-22T00:00:00Z' />    <category name='2012-06-23T00:00:00Z' />    <category name='2012-06-24T00:00:00Z' />    <category name='2012-06-25T00:00:00Z' />    <category name='2012-06-26T00:00:00Z' />    <category name='2012-06-27T00:00:00Z' />    <category name='2012-06-28T00:00:00Z' />    <category name='2012-06-29T00:00:00Z' />    <category name='2012-06-30T00:00:00Z' />    <category name='2012-07-01T00:00:00Z' />    <category name='2012-07-02T00:00:00Z' />    <category name='2012-07-03T00:00:00Z' />    <category name='2012-07-04T00:00:00Z' />    <category name='2012-07-05T00:00:00Z' />    <category name='2012-07-06T00:00:00Z' />    <category name='2012-07-07T00:00:00Z' />    <category name='2012-07-08T00:00:00Z' />    <category name='2012-07-09T00:00:00Z' />    <category name='2012-07-10T00:00:00Z' />    <category name='2012-07-11T00:00:00Z' />    <category name='2012-07-12T00:00:00Z' />    <category name='2012-07-13T00:00:00Z' />    <category name='2012-07-14T00:00:00Z' />    <category name='2012-07-15T00:00:00Z' />    <category name='2012-07-16T00:00:00Z' />    <category name='2012-07-17T00:00:00Z' />    <category name='2012-07-18T00:00:00Z' />    <category name='2012-07-19T00:00:00Z' />    <category name='2012-07-20T00:00:00Z' />    <category name='2012-07-21T00:00:00Z' />    <category name='2012-07-22T00:00:00Z' />    <category name='2012-07-23T00:00:00Z' />    <category name='2012-07-24T00:00:00Z' />    <category name='2012-07-25T00:00:00Z' />    <category name='2012-07-26T00:00:00Z' />    <category name='2012-07-27T00:00:00Z' />    <category name='2012-07-28T00:00:00Z' />    <category name='2012-07-29T00:00:00Z' />    <category name='2012-07-30T00:00:00Z' />    <category name='2012-07-31T00:00:00Z' />    <category name='2012-08-01T00:00:00Z' />    <category name='2012-08-02T00:00:00Z' />    <category name='2012-08-03T00:00:00Z' />    <category name='2012-08-04T00:00:00Z' />    <category name='2012-08-05T00:00:00Z' />    <category name='2012-08-06T00:00:00Z' />    <category name='2012-08-07T00:00:00Z' />    <category name='2012-08-08T00:00:00Z' />    <category name='2012-08-09T00:00:00Z' />    <category name='2012-08-10T00:00:00Z' />    <category name='2012-08-11T00:00:00Z' />    <category name='2012-08-12T00:00:00Z' />    <category name='2012-08-13T00:00:00Z' />    <category name='2012-08-14T00:00:00Z' />    <category name='2012-08-15T00:00:00Z' />    <category name='2012-08-16T00:00:00Z' />    <category name='2012-08-17T00:00:00Z' />    <category name='2012-08-18T00:00:00Z' />    <category name='2012-08-19T00:00:00Z' />    <category name='2012-08-20T00:00:00Z' />    <category name='2012-08-21T00:00:00Z' />    <category name='2012-08-22T00:00:00Z' />    <category name='2012-08-23T00:00:00Z' />    <category name='2012-08-24T00:00:00Z' />    <category name='2012-08-25T00:00:00Z' />    <category name='2012-08-26T00:00:00Z' />    <category name='2012-08-27T00:00:00Z' />    <category name='2012-08-28T00:00:00Z' />    <category name='2012-08-29T00:00:00Z' />    <category name='2012-08-30T00:00:00Z' />    <category name='2012-08-31T00:00:00Z' />    <category name='2012-09-01T00:00:00Z' />    <category name='2012-09-02T00:00:00Z' />    <category name='2012-09-03T00:00:00Z' />    <category name='2012-09-04T00:00:00Z' />    <category name='2012-09-05T00:00:00Z' />    <category name='2012-09-06T00:00:00Z' />    <category name='2012-09-07T00:00:00Z' />    <category name='2012-09-08T00:00:00Z' />    <category name='2012-09-09T00:00:00Z' />    <category name='2012-09-10T00:00:00Z' />    <category name='2012-09-11T00:00:00Z' />    <category name='2012-09-12T00:00:00Z' />    <category name='2012-09-13T00:00:00Z' />    <category name='2012-09-14T00:00:00Z' />    <category name='2012-09-15T00:00:00Z' />    <category name='2012-09-16T00:00:00Z' />    <category name='2012-09-17T00:00:00Z' />    <category name='2012-09-18T00:00:00Z' />    <category name='2012-09-19T00:00:00Z' />    <category name='2012-09-20T00:00:00Z' />    <category name='2012-09-21T00:00:00Z' />    <category name='2012-09-22T00:00:00Z' />    <category name='2012-09-23T00:00:00Z' />    <category name='2012-09-24T00:00:00Z' />    <category name='2012-09-25T00:00:00Z' />    <category name='2012-09-26T00:00:00Z' />    <category name='2012-09-27T00:00:00Z' />  </categories>  <dataset seriesName='FOX'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='NBC'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='CBS'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='Entertainment'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='CW'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset>  <dataset seriesName='ABC'>    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />    <set value='0' />  </dataset></chart>)", true);


                    //                    ScriptManager.RegisterStartupScript(this, GetType(), "renderchart", registerscript, true);
                    //divChart.Text = FusionCharts.RenderChart(@"/fusionchart/ZoomLine.swf", "", Convert.ToString(xdocFinal).Replace("\"", "'").Replace("\r", "").Replace("\n", ""), "divChart", chartWidth, chartHeight, false, true);                    


                    /* string eventListnerString = "FusionCharts(\"divChart\").addEventListener(\"LegendItemClicked\", myChartListener);";
                     ScriptManager.RegisterStartupScript(this, GetType(), "addEventListner", eventListnerString, true);*/
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }
        }

        public void CreateMsLineXML()
        {
            try
            {
                XDocument xdocSource = null;
                XDocument xdocFinal = new XDocument();

                if (listOfAffiliateMsLineResponse != null && listOfAffiliateMsLineResponse.Count > 0)
                {
                    xdocSource = XDocument.Parse(listOfAffiliateMsLineResponse[0].solrResponse);
                    var listXAxis = (from element in xdocSource.Descendants("response").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("int")
                                     select element.Attribute("name").Value).ToList();

                    if (!isXaxisAddedMsLine && listXAxis != null && listXAxis.Count > 0)
                    {
                        xdocFinal = new XDocument(
                            new XElement("chart", new XAttribute("caption", txtToDate.Text.Trim()), new XAttribute("xAxisName", ""), new XAttribute("yAxisName", "Hits"), new XAttribute("showValues", "0"),
                                new XAttribute("showBorder", "1"), new XAttribute("labelDisplay", "Rotate"), new XAttribute("showLegend", "0"),
                               new XElement("categories",
                                        listXAxis.Select(x => new XElement("category", new XAttribute("name", GetTimeFormat(x))))
                                    )
                                    )
                                );
                        isXaxisAddedMsLine = true;
                    }
                }

                foreach (affiliateMsLineResponse response in listOfAffiliateMsLineResponse)
                {
                    xdocSource = XDocument.Parse(response.solrResponse);

                    var listYAxis = (from element in xdocSource.Descendants("response").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("int")
                                     select element.Value).ToList();




                    if (listYAxis != null && listYAxis.Count > 0)
                    {
                        xdocFinal.Root.Add(new XElement("dataset", new XAttribute("seriesName", response.name),
                            listYAxis.Select(y => new XElement("set", new XAttribute("value", y))))
                            );
                    }
                }

                if (!string.IsNullOrWhiteSpace(Convert.ToString(xdocFinal)))
                {


                    string fileName = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString()
                            + DateTime.Now.Minute.ToString() + DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString();
                    ViewState["fileName"] = fileName;
                    xdocFinal.Save(Server.MapPath("/FusionChart/ChartXML/" + fileName + ".xml"));
                    divMsLineChart.Text = FusionCharts.RenderChart(@"/fusionchart/MSLine.swf", "/fusionchart/ChartXML/" + fileName + ".XML", "", "divMsLineChart", chartWidth, chartHeight, false, true);
                    //divMsLineChart.Text = FusionCharts.RenderChart(@"/fusionchart/Column2D.swf", "", Convert.ToString(xdocFinal).Replace("\"", "'").Replace("\r", "").Replace("\n", ""), "divMsLineChart", chartWidth, chartHeight, false, true);


                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }
        }

        public string GetTimeFormat(object _value)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(Convert.ToString(_value)))
            {
                DateTime dt = System.DateTime.Parse(Convert.ToString(_value));
                result = Convert.ToString(dt.Minute);
                if (dt.Hour > 12)
                {
                    result = Convert.ToString(dt.Hour - 12) + ":" + result + "PM";
                }
                else
                {
                    result = Convert.ToString(dt.Hour) + ":" + result + "AM";
                }

                return result;
            }
            return result;
        }


        //protected void rbChartMode_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (ViewState["fileName"] != null)
        //        {
        //            trChartMode.Visible = true;
        //            if (rbChartMode.SelectedIndex == 1)
        //            {
        //                divChart.Text = FusionCharts.RenderChart(@"/fusionchart/MSLine.swf", "/fusionchart/ChartXML/" + Convert.ToString(ViewState["fileName"]) + ".XML", "", "myFirst", chartWidth, chartHeight, false, true);
        //            }
        //            else
        //            {
        //                divChart.Text = FusionCharts.RenderChart(@"/fusionchart/ZoomLine.swf", "/fusionchart/ChartXML/" + Convert.ToString(ViewState["fileName"]) + ".XML", "", "myFirst", chartWidth, chartHeight, false, true);
        //            }
        //        }
        //        else
        //        {
        //            divChart.Text = "No Data Available";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        lblMessage.Text = "An error occurred, Please try again later.";
        //        lblMessage.Visible = true;
        //    }
        //}

        public void GetResponse(string url, string affiliate)
        {
            try
            {
                Uri _Uri = new Uri(url);
                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);
                string responseStrting = string.Empty;
                using (HttpWebResponse response = _objWebRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseStrting = reader.ReadToEnd();
                    response.Close();
                    reader.Close();
                    reader.Dispose();
                }

                affiliateZoomResponse affiliateResponse = new WebApplication.FusionChart.affiliateZoomResponse();
                affiliateResponse.name = affiliate;
                affiliateResponse.solrResponse = responseStrting;

                listOfAffiliateResponse.Add(affiliateResponse);

            }
            catch (Exception ex)
            {

                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }

        }

        public void GetMsLineResponse(string url, string affiliate)
        {
            try
            {
                Uri _Uri = new Uri(url);
                HttpWebRequest _objWebRequest = (HttpWebRequest)WebRequest.Create(_Uri);
                string responseStrting = string.Empty;
                using (HttpWebResponse response = _objWebRequest.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    responseStrting = reader.ReadToEnd();
                    response.Close();
                    reader.Close();
                    reader.Dispose();
                }

                affiliateMsLineResponse affiliateMslineResponse = new WebApplication.FusionChart.affiliateMsLineResponse();
                affiliateMslineResponse.name = affiliate;
                affiliateMslineResponse.solrResponse = responseStrting;

                listOfAffiliateMsLineResponse.Add(affiliateMslineResponse);


            }
            catch (Exception ex)
            {

                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }

        }

        protected void BindAffiliate()
        {
            try
            {
                MasterStatSkedProg _MasterStatSkedProg = null;
                IStatSkedProgController _IStatSkedProgController = _ControllerFactory.CreateObject<IStatSkedProgController>();
                _MasterStatSkedProg = _IStatSkedProgController.GetAllDetail();
                if (_MasterStatSkedProg._ListofAffil.Count > 0)
                {
                    rptAffil.DataSource = _MasterStatSkedProg._ListofAffil;
                    rptAffil.DataBind();


                    for (int i = 0; i < rptAffil.Items.Count; i++)
                    {
                        if (i <= 5)
                        {
                            HtmlInputCheckBox _chkbox = (HtmlInputCheckBox)rptAffil.Items[i].FindControl("chkAffil");
                            _chkbox.Checked = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = "An error occurred, Please try again later.";
                lblMessage.Visible = true;
            }

        }

        public void RemoveDataset(string seriesName)
        {
            try
            {
                XDocument xdoc = XDocument.Parse(Server.MapPath("/FusionChart/ChartXML/abc.xml"));
                xdoc.Root.Descendants("dataset ").Where(x => x.Attribute("seriesName").Equals(seriesName)).Remove();
                string abc = "";
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}