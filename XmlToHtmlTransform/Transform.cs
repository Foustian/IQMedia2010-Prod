using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.IO;
using System.Xml;
using RazorEngine;
using RazorEngine.Templating;
using System.Xml.Serialization;
using System.Net;
using System.Drawing;
/*using System.Web.Mvc;
using System.Web;
using System.Collections.Specialized;
using System.Web.Routing;
*/
namespace XmlToHtmlTransform
{
    public static class Transform
    {
        public static string V5TransformXMLToHTML(string strXmlDoc, string strXmlFilePath = null)
        {
            try
            {

                string template = string.Empty;
                using (StreamReader readerTemplate = new StreamReader(System.AppDomain.CurrentDomain.BaseDirectory + "_AgentEmail.txt"))
                {
                    template = readerTemplate.ReadToEnd();
                }
                //Razor.SetTemplateBase(typeof(HtmlTemplateBase<>));

                //                string template =
                //                @"<html>
                //                       <head>
                //                            <title>Current Time</title>
                //                        </head>
                //                        <body>
                //                        @{
                //                                string Month = Model.MediaDate != null ? Model.MediaDate.ToString(""MMM"") : string.Empty;
                //                                string Day = Model.MediaDate != null ? Model.MediaDate.Day.ToString() : string.Empty;
                //                                string Time = Model.MediaDate != null ? Model.MediaDate.ToString(""hh:mm tt"") : string.Empty;
                //                         }
                //                        Current Date is: @(Month + "" "" + Day + "" "" + Time) <br/>
                //                        Hello All : @Model.MediaDate.ToShortDateString()
                //                      </body>
                //                    </html>";

                IQAgentDigest iQAgentDigest = new IQAgentDigest();
                if (!string.IsNullOrEmpty(strXmlDoc))
                {
                    iQAgentDigest = (IQAgentDigest)DeserialiazeXml(strXmlDoc, iQAgentDigest);
                }
                else if (!string.IsNullOrEmpty(strXmlFilePath))
                {
                    using (StreamReader readerXml = new StreamReader(strXmlFilePath))
                    {
                        iQAgentDigest = (IQAgentDigest)DeserialiazeXml(readerXml.ReadToEnd(), iQAgentDigest);
                    }
                }
                else
                {
                    throw new Exception("Invalid Input");
                }

                if (iQAgentDigest.ClientHeader != null)
                {
                    Image imgClientHeader = Image.FromStream(new MemoryStream(new WebClient().DownloadData(iQAgentDigest.ClientHeader)));
                    string imgName = System.IO.Path.GetFileName(iQAgentDigest.ClientHeader);
                    string imgExtension = System.IO.Path.GetExtension(iQAgentDigest.ClientHeader);
                    imgName = imgName.Replace(imgExtension, "");

                    if (imgClientHeader.Width > 650)
                    {
                        if (!System.IO.File.Exists(iQAgentDigest.ClientHeaderImagePath + imgName + "_650" + imgExtension))
                        {
                            using (System.Drawing.Bitmap bitMap = new System.Drawing.Bitmap(650, Convert.ToInt32(Math.Ceiling((650 * Convert.ToDouble(imgClientHeader.Height)) / imgClientHeader.Width))))
                            {
                                using (System.Drawing.Graphics g = Graphics.FromImage(bitMap))
                                {
                                    g.DrawImage(imgClientHeader, 0, 0, bitMap.Width, bitMap.Height);
                                    if (iQAgentDigest.ClientHeaderImagePath != null)
                                    {
                                        bitMap.Save(iQAgentDigest.ClientHeaderImagePath + imgName + "_650" + imgExtension);
                                        iQAgentDigest.ClientHeader = iQAgentDigest.ClientHeader.Replace(imgName + imgExtension, imgName + "_650" + imgExtension);
                                    }
                                }
                            }
                        }
                        else
                        {
                            iQAgentDigest.ClientHeader = iQAgentDigest.ClientHeader.Replace(imgName + imgExtension, imgName + "_650" + imgExtension);
                        }
                    }
                }

                return Razor.Parse(template, iQAgentDigest);

                //return RenderPartialToString(@"_AgentEmail.cshtml", iQAgentDigest);


                //XslCompiledTransform transform = new XslCompiledTransform();
                //transform.Load(xsltpath);
                //StringWriter results = new StringWriter();
                //using (XmlReader reader = XmlReader.Create(new StringReader(strXmlDoc)))
                //{
                //    transform.Transform(reader, null, results);   
                //}

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string ProcessHighlightingText(string p_HighlightingText, string p_SubHighlightingText)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(p_HighlightingText) && !string.IsNullOrWhiteSpace(p_SubHighlightingText))
                {
                    // Step - 1. Try to complete word if it cuts while performing Substring operation
                    int WordCompletedIndex = -1;
                    if (p_HighlightingText.Length > p_SubHighlightingText.Length)
                    {
                        WordCompletedIndex = p_HighlightingText.IndexOf(" ", p_SubHighlightingText.Length);
                    }

                    if (!p_SubHighlightingText.EndsWith(" ") && WordCompletedIndex > 0)
                    {
                        p_SubHighlightingText = p_HighlightingText.Substring(0, WordCompletedIndex);
                    }

                    // Step - 2. Load Substring into HTMLAgilityPack. If any unclosed or invalid HTML tags occurs, just trim Substring from that position.

                    HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
                    doc.LoadHtml(p_SubHighlightingText);

                    if (doc.ParseErrors.Count() > 0)
                    {
                        foreach (HtmlAgilityPack.HtmlParseError error in doc.ParseErrors)
                        {
                            if (error.StreamPosition > 0)
                            {
                                p_SubHighlightingText = p_SubHighlightingText.Substring(0, error.StreamPosition);
                                break;
                            }
                        }
                    }

                    return p_SubHighlightingText;
                }
            }
            catch (Exception)
            {
                throw;
            }

            return "";
        }

        internal static object DeserialiazeXml(string p_XMLString, object p_Deserialization)
        {
            StringReader _StringReader;
            XmlTextReader _XmlTextReader;

            try
            {
                XmlSerializer _XmlSerializer = new XmlSerializer(p_Deserialization.GetType());
                _StringReader = new StringReader(p_XMLString);
                _XmlTextReader = new XmlTextReader(_StringReader);
                p_Deserialization = (object)_XmlSerializer.Deserialize(_XmlTextReader);
                _StringReader.Close();
                _XmlTextReader.Close();
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
            return p_Deserialization;
        }
        /*
        public static string RenderPartialToString(string viewName, object model)
        {
            FakeControllerContext fakeControllercontext = new FakeControllerContext();

            fakeControllercontext.Controller = new FakeController();            

            fakeControllercontext.Controller.ViewData.Model = model;
            fakeControllercontext.RouteData = new System.Web.Routing.RouteData();
            fakeControllercontext.RouteData.Values.Add("controller", "FakeController");
            fakeControllercontext.RouteData.Values.Add("action", "XmlToHtml");

            using (StringWriter sw = new StringWriter())
            {
                ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(fakeControllercontext, viewName);
                var view = new WebFormView(viewName);
                

                ViewContext viewContext = new ViewContext(fakeControllercontext, view, fakeControllercontext.Controller.ViewData, new TempDataDictionary(), sw);  
              
                //viewResult.View.Render(viewContext, sw);
                viewContext.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }       

        
        public class FakeController : Controller
        {
            public ActionResult XmlToHtml(){ return View();}
        }

        public class FakeControllerContext : ControllerContext
        {
            HttpContextBase _fakeHttpContext = new FakeHttpContext();

            public override System.Web.HttpContextBase HttpContext
            {
                get { return _fakeHttpContext; }
                set { _fakeHttpContext = value; }
            }
        }

        class FakeHttpContext : HttpContextBase
        {
            readonly HttpRequestBase _fakeHttpRequest = new FakeHttpReqeuest();

            public override HttpRequestBase Request
            {
                get
                {
                    return _fakeHttpRequest;
                }
            }
        }

        class FakeHttpReqeuest : HttpRequestBase
        {
            public override string this[string key]
            {
                get { return null; }
            }

            public override NameValueCollection Headers
            {
                get { return new NameValueCollection(); }
            }
        }
        */
    }
}
