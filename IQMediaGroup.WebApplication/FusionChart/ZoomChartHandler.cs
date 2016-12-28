using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Net;
using System.IO;
using System.Linq;

namespace IQMediaGroup.WebApplication.FusionChart
{
    public class ZoomChartHandler : IHttpHandler
    {
        List<affiliateZoomResponse> listOfAffiliateResponse = null;
        List<affiliateMsLineResponse> listOfAffiliateMsLineResponse = null;
        bool isXaxisAdded = false;
        bool isXaxisAddedMsLine = false;

        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                //=============================================//
                // get start and end date through query string //
                //=============================================//

                if (!string.IsNullOrWhiteSpace(context.Request.QueryString["endDate"]))
                {
                    Getdata(Convert.ToDateTime(context.Request.QueryString["endDate"]), context.Request.QueryString["searchterm"], context.Request.QueryString["affiliates"]);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void Getdata(DateTime dtenddate, string searchterm, string affiliates)
        {
            listOfAffiliateResponse = new List<affiliateZoomResponse>();
            listOfAffiliateMsLineResponse = new List<affiliateMsLineResponse>();
            isXaxisAdded = false;
            isXaxisAddedMsLine = false;
            List<string> affiliatelist = new List<string>();
            
            affiliatelist.AddRange(affiliates.Split(','));
            if (affiliatelist != null && affiliatelist.Count > 0)
            {
                //string startdate = txtfromdate.text + "t00:00:00z";
                //string enddate = txttodate.text + "t23:59:59z";

                /*parallel.foreach(affiliatelist, item => getresponse("http://10.100.1.39:8080/solr/core0/select?q=" + txtsearchterm.text + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.rl_station_datetime_dt.facet.range.start=" + startdate + "&facet.range=rl_station_datetime_dt&facet.range.end=" + enddate + "&facet.range.gap=%2b1day", item));
                parallel.foreach(affiliatelist, item => getmslineresponse("http://10.100.1.39:8080/solr/core0/select?q=" + txtsearchterm.text + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.rl_station_datetime_dt.facet.range.start=" + txttodate.text.trim() + "t00:00:00z" + "&facet.range=rl_station_datetime_dt&facet.range.end=" + txttodate.text.trim() + "t23:59:59z" + "&facet.range.gap=%2b1hour", item));*/

                Parallel.ForEach(affiliatelist, item => GetMsLineResponse("http://192.168.1.59:8085/solr/core0/select?q=" + searchterm + "&rows=0&fq=affiliate:" + item + "&facet.range.other=all&facet=on&f.rl_station_datetime_dt.facet.range.start=" + dtenddate.ToString() + "t00:00:00z" + "&facet.range=rl_station_datetime_dt&facet.range.end=" + dtenddate.ToString() + "t23:59:59z" + "&facet.range.gap=%2b1hour", item));

                createmslinexml();
                //trchartmode.visible = true;
            }
            else
            {
                //lblmessage.text = "select atleast one affiliate.";
                //lblmessage.visible = true;
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

                /*  lblMessage.Text = "An error occurred, Please try again later.";
                  lblMessage.Visible = true;*/
            }

        }

        public void createmslinexml()
        {
            try
            {
                XDocument xdocsource = null;
                XDocument xdocfinal = new XDocument();

                if (listOfAffiliateMsLineResponse != null && listOfAffiliateMsLineResponse.Count > 0)
                {
                    xdocsource = XDocument.Parse(listOfAffiliateMsLineResponse[0].solrResponse);
                    var listxaxis = (from element in xdocsource.Descendants("response").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("int")
                                     select element.Attribute("name").Value).ToList();

                    if (!isXaxisAddedMsLine && listxaxis != null && listxaxis.Count > 0)
                    {
                        xdocfinal = new XDocument(
                            new XElement("chart", new XAttribute("caption", HttpContext.Current.Request.QueryString["endDate"]), new XAttribute("xaxisname", ""), new XAttribute("yaxisname", "hits"), new XAttribute("showvalues", "0"),
                                new XAttribute("showborder", "1"), new XAttribute("labeldisplay", "rotate"), new XAttribute("showlegend", "0"),
                               new XElement("categories",
                                        listxaxis.Select(x => new XElement("category", new XAttribute("name", GetTimeFormat(x))))
                                    )
                                    )
                                );
                        isXaxisAddedMsLine = true;
                    }
                }

                foreach (affiliateMsLineResponse response in listOfAffiliateMsLineResponse)
                {
                    xdocsource = XDocument.Parse(response.solrResponse);

                    var listyaxis = (from element in xdocsource.Descendants("response").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("lst").Descendants("int")
                                     select element.Value).ToList();




                    if (listyaxis != null && listyaxis.Count > 0)
                    {
                        xdocfinal.Root.Add(new XElement("dataset", new XAttribute("seriesname", response.name),
                            listyaxis.Select(y => new XElement("set", new XAttribute("value", y))))
                            );
                    }
                }

                if (!string.IsNullOrWhiteSpace(Convert.ToString(xdocfinal)))
                {

                    HttpContext.Current.Response.ContentType = "text/plain";
                    HttpContext.Current.Response.Write(Convert.ToString(xdocfinal));
                    //string filename = DateTime.now.year.tostring() + DateTime.now.month.tostring() + DateTime.now.day.tostring() + DateTime.now.hour.tostring()
                    //        + datetime.now.minute.tostring() + datetime.now.second.tostring() + datetime.now.millisecond.tostring();

                    //divmslinechart.text = fusioncharts.renderchart(@"/fusionchart/msline.swf", "/fusionchart/chartxml/" + filename + ".xml", "", "divmslinechart", chartwidth, chartheight, false, true);
                    //divmslinechart.text = fusioncharts.renderchart(@"/fusionchart/column2d.swf", "", convert.tostring(xdocfinal).replace("\"", "'").replace("\r", "").replace("\n", ""), "divmslinechart", chartwidth, chartheight, false, true);


                }
            }
            catch (Exception ex)
            {
                /* lblmessage.text = "an error occurred, please try again later.";
                 lblmessage.visible = true;*/
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
                    result = Convert.ToString(dt.Hour - 12) + ":" + result + "pm";
                }
                else
                {
                    result = Convert.ToString(dt.Hour) + ":" + result + "am";
                }

                return result;
            }
            return result;
        }
        #endregion
    }
}
