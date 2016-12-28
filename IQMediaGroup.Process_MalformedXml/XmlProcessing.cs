using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IQMediaGroup.Process_MalformedXml.Util;
using System.Xml.Linq;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Web;
using System.Text.RegularExpressions;

namespace IQMediaGroup.Process_MalformedXml
{
    public class XmlProcessing
    {
        public static OutputXml ProcessMalformedXml(string RulesFilePath, string XmlString)
        {
            OutputXml OutputXml = new OutputXml();

            try
            {

                if (string.IsNullOrEmpty(XmlString) || string.IsNullOrWhiteSpace(XmlString) || string.IsNullOrEmpty(RulesFilePath))
                {
                    throw new Exception("Invalid Input");
                }
                else
                {
                    var RegexExp = new RegexExp();

                    FileInfo _FileInfo = new FileInfo(RulesFilePath);
                    string RegexXml = string.Empty;

                    if (_FileInfo.Exists)
                    {
                        XmlDocument _XmlDocument = new XmlDocument();

                        _XmlDocument.Load(RulesFilePath);
                        RegexXml = _XmlDocument.InnerXml;

                        RegexExp = (RegexExp)CommonFun.MakeDeserialiazation(RegexXml, RegexExp);
                    }
                    else
                    {
                        throw new Exception("File doesn't exist"); 
                    }


                    XmlString = XmlString.Trim().Replace("\0", string.Empty);
                    XmlString = XmlString.Replace("\r", string.Empty).Replace("\n", string.Empty);
                    string defaultDeclatation = @"<?xml version=""1.0"" encoding=""utf-8""?>" +
                                                     @"<tt xml:lang=""EN"" xmlns=""http://www.w3.org/2006/10/ttaf1"">" +
                                                     "<head>" +
                                                     @"<metadata xmlns:ttm=""http://www.w3.org/2006/10/ttaf1#metadata"">" +
                                                    "</metadata>" +
                                                    @"</head>" +
                                                    @"<body region=""subtitleArea""><div>";
                    foreach (var RegexExpression in RegexExp.Regex)
                    {
                        if (RegexExpression.Expression.Contains("&lt;"))
                        {
                            RegexExpression.Expression = RegexExpression.Expression.Replace("&lt;", "<");
                            RegexExpression.ReplaceWith = RegexExpression.ReplaceWith.Replace("&lt;", "<");
                        }

                        if (RegexExpression.Expression.Contains("&quot;"))
                        {
                            RegexExpression.Expression = RegexExpression.Expression.Replace("&quot;", "\"");
                            RegexExpression.ReplaceWith = RegexExpression.ReplaceWith.Replace("&quot;", "\"");
                        }



                        if (RegexExpression.Expression.Equals(@"<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*""\s*end=""\s*\w*"")>+(.*?)(?=</p>)"))
                        {
                            //Get All tag starting from <p
                            List<string> pTagList = System.Text.RegularExpressions.Regex.Split(XmlString, "(?<=<p)").ToList();
                            List<XMLData> xmlDataList = new List<XMLData>();
                            for (Int32 i = 0; i < pTagList.Count; i++)
                            {
                                if (!pTagList[i].Contains(@"<?xml version=""1.0"""))
                                {
                                    XMLData xmlData = new XMLData();
                                    pTagList[i] = pTagList[i].ToString().Replace("<p", string.Empty);
                                    pTagList[i] = "<p " + pTagList[i];
                                    xmlData.pTag = pTagList[i];
                                    if (System.Text.RegularExpressions.Regex.IsMatch(pTagList[i].ToString().Trim(), @"<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*""\s*end=""\s*\w*"")>+(.*?)(?=</p>)") || System.Text.RegularExpressions.Regex.IsMatch(pTagList[i].ToString().Trim(), @"<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*\.*\w*""\s*end=""\s*\w*\.*\w*""\s*timed_out=""\w*"")>+(.*?)(?=</p>)"))
                                    //if (System.Text.RegularExpressions.Regex.IsMatch(pTagList[i].ToString().Trim(), @"<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*""\s*end=""\s*\w*"")>+(.*?)(?=</p>)"))
                                    { xmlData.isValid = true; }
                                    else { xmlData.isValid = false; }
                                    xmlDataList.Add(xmlData);
                                }

                            }
                            int inValidTagCount = xmlDataList.Where(c => c.isValid == false).Count();
                            string xmlDeclaration = string.Empty;
                            if (pTagList[0].Contains(@"<?xml version=""1.0"""))
                            {
                                xmlDeclaration = pTagList[0].ToString().Replace("<p", string.Empty);
                                pTagList.RemoveAt(0);
                            }

                            List<Int32> removePositions = new List<Int32>();

                            //Loop through all the p tag to merge data.
                            #region Loop Through xmlDataList

                            for (Int32 outerIndex = xmlDataList.Count - 1; outerIndex >= 0; outerIndex--)
                            {
                                if (!xmlDataList[outerIndex].isValid)
                                {
                                    MatchCollection tagWithAllAttribute = System.Text.RegularExpressions.Regex.Matches(xmlDataList[outerIndex].pTag, @"<p\s*\w*xml:id=""\s*\w*""(.*?)begin=""\s*\w*""(.*?)end=""\s*\w*"">", RegexOptions.Singleline);
                                    if (tagWithAllAttribute.Count > 0) // check if all attribute available
                                    {
                                        xmlDataList[outerIndex].pTag = xmlDataList[outerIndex].pTag.Replace(tagWithAllAttribute[0].Value, System.Text.RegularExpressions.Regex.Replace(tagWithAllAttribute[0].Value, @"((?<=<p)(.*?)(?=xml:id))|((?<=xml:id=""\s*\w*"")(.*?)(?=begin))|((?<=begin=""\s*\w*"")(.*?)(?=end))", " "));
                                        xmlDataList[outerIndex].isValid = true;
                                    }
                                    else
                                    {

                                        MatchCollection dataInsideIncompleteTag = System.Text.RegularExpressions.Regex.Matches(xmlDataList[outerIndex].pTag, @"(?<=>)\w*(.*?)(?=</p>)", RegexOptions.Singleline);
                                        var xmlData = xmlDataList.Where(c => c.isValid == true);
                                        if (xmlData != null && xmlData.Count() > 0)
                                        {
                                            XMLData Innerxmldata = xmlDataList.Where(c => c.isValid == true).First();
                                            #region Go Next Valid Tag
                                            if (outerIndex == 0) // check if current p tag is first tag or not
                                            {

                                                MatchCollection dataInsideNextTag = System.Text.RegularExpressions.Regex.Matches(Innerxmldata.pTag, @"(?<=>)\w*(.*?)(?=</p>)", RegexOptions.Singleline);
                                                string incompleteTagData = string.Empty;
                                                if (dataInsideIncompleteTag.Count <= 0) // if current p tag does not contain data between > and </p>
                                                {
                                                    //go for attribute check
                                                    incompleteTagData = Innerxmldata.pTag;
                                                    incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"<p\s*", string.Empty);
                                                    incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"xml:id=""\s*\w*""", string.Empty);
                                                    incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"begin=""\s*\w*""", string.Empty);
                                                    incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"end=""\s*\w*""", string.Empty);
                                                    incompleteTagData = incompleteTagData.Replace("</p>", string.Empty);
                                                }

                                            if (dataInsideNextTag.Count > 0)
                                            {
                                                if (dataInsideIncompleteTag.Count > 0) // merge data with Next tag
                                                {
                                                    xmlDataList.Where(c => c.isValid == true).First().pTag = Innerxmldata.pTag.Replace(dataInsideNextTag[0].Value, dataInsideIncompleteTag[0].Value + " " + dataInsideNextTag[0].Value);
                                                }
                                                else // go for attribute check
                                                {
                                                    if (!string.IsNullOrEmpty(incompleteTagData))
                                                    {
                                                        xmlDataList.Where(c => c.isValid == true).First().pTag = Innerxmldata.pTag.Replace(dataInsideNextTag[0].Value, incompleteTagData.Trim() + " " + dataInsideNextTag[0].Value);
                                                    }
                                                }

                                            }
                                            else // go for attribute check for next tag
                                            {
                                                string incompleteNextData = Innerxmldata.pTag;
                                                incompleteNextData = System.Text.RegularExpressions.Regex.Replace(incompleteNextData, @"<p\s*", string.Empty);
                                                incompleteNextData = System.Text.RegularExpressions.Regex.Replace(incompleteNextData, @"xml:id=""\s*\w*""", string.Empty);
                                                incompleteNextData = System.Text.RegularExpressions.Regex.Replace(incompleteNextData, @"begin=""\s*\w*""", string.Empty);
                                                incompleteNextData = System.Text.RegularExpressions.Regex.Replace(incompleteNextData, @"end=""\s*\w*""", string.Empty);
                                                incompleteNextData = incompleteNextData.Replace("</p>", string.Empty);

                                                if (dataInsideIncompleteTag.Count > 0)
                                                {
                                                    xmlDataList.Where(c => c.isValid == true).First().pTag = Innerxmldata.pTag.Replace(incompleteNextData.Trim(), incompleteNextData.Trim() + " " + dataInsideIncompleteTag[0].Value);
                                                }
                                                else // go for attribute check
                                                {
                                                    if (!string.IsNullOrEmpty(incompleteTagData))
                                                    {
                                                        xmlDataList.Where(c => c.isValid == true).First().pTag = Innerxmldata.pTag.Replace(incompleteNextData.Trim(), incompleteNextData.Trim() + " " + incompleteTagData.Trim());
                                                    }
                                                }
                                            }

                                        }
                                        #endregion

                                        #region Go For Previous Tag
                                        else // Same logic as Next Tag
                                        {
                                            MatchCollection dataInsidePreviousTag = System.Text.RegularExpressions.Regex.Matches(xmlDataList[outerIndex - 1].pTag, @"(?<=>)\w*(.*?)(?=</p>)", RegexOptions.Singleline);
                                            string incompleteTagData = string.Empty;
                                            if (dataInsideIncompleteTag.Count <= 0) // if current p tag does not contain data between > and </p>
                                            {
                                                //go for attribute check
                                                incompleteTagData = xmlDataList[outerIndex].pTag;
                                                //incompleteTagData = Regex.Replace(Regex.Replace(Regex.Replace(Regex.Replace(incompleteTagData, @"<p\s*", string.Empty), @"xml:id=""\s*\w*""", string.Empty), @"begin=""\s*\w*""", string.Empty), @"end=""\s*\w*""", string.Empty).Replace("</p>", string.Empty);
                                                incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"<p\s*", string.Empty);
                                                incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"xml:id=""\s*\w*""", string.Empty);
                                                incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"begin=""\s*\w*""", string.Empty);
                                                incompleteTagData = System.Text.RegularExpressions.Regex.Replace(incompleteTagData, @"end=""\s*\w*""", string.Empty);
                                                incompleteTagData = incompleteTagData.Replace("</p>", string.Empty);
                                            }


                                            if (dataInsidePreviousTag.Count > 0) // Merge data with previous tag
                                            {
                                                if (dataInsideIncompleteTag.Count > 0)
                                                {
                                                    xmlDataList[outerIndex - 1].pTag = xmlDataList[outerIndex - 1].pTag.Replace(dataInsidePreviousTag[0].Value, dataInsidePreviousTag[0].Value + " " + dataInsideIncompleteTag[0].Value);
                                                }
                                                else // go for attribute check
                                                {
                                                    if (!string.IsNullOrEmpty(incompleteTagData)) // Merge data with previous tag
                                                    {
                                                        xmlDataList[outerIndex - 1].pTag = xmlDataList[outerIndex - 1].pTag.Replace(dataInsidePreviousTag[0].Value, dataInsidePreviousTag[0].Value + " " + incompleteTagData.Trim());
                                                    }
                                                }

                                            }
                                            else // go for attribute check for previous tag
                                            {
                                                string incompletePreviousData = xmlDataList[outerIndex - 1].pTag;
                                                incompletePreviousData = System.Text.RegularExpressions.Regex.Replace(incompletePreviousData, @"<p\s*", string.Empty);
                                                incompletePreviousData = System.Text.RegularExpressions.Regex.Replace(incompletePreviousData, @"xml:id=""\s*\w*""", string.Empty);
                                                incompletePreviousData = System.Text.RegularExpressions.Regex.Replace(incompletePreviousData, @"begin=""\s*\w*""", string.Empty);
                                                incompletePreviousData = System.Text.RegularExpressions.Regex.Replace(incompletePreviousData, @"end=""\s*\w*""", string.Empty);
                                                incompletePreviousData = incompletePreviousData.Replace("</p>", string.Empty);

                                                if (dataInsideIncompleteTag.Count > 0)
                                                {
                                                    xmlDataList[outerIndex - 1].pTag = xmlDataList[outerIndex - 1].pTag.Replace(incompletePreviousData.Trim(), incompletePreviousData.Trim() + " " + dataInsideIncompleteTag[0].Value);
                                                }
                                                else // go for attribute check
                                                {
                                                    if (!string.IsNullOrEmpty(incompleteTagData))
                                                    {
                                                        xmlDataList[outerIndex - 1].pTag = xmlDataList[outerIndex - 1].pTag.Replace(incompletePreviousData.Trim(), incompletePreviousData.Trim() + " " + incompleteTagData.Trim());
                                                    }
                                                }
                                            }

                                            }
                                            #endregion
                                        }
                                    }
                                }
                            }
                            #endregion



                            xmlDeclaration = ((!string.IsNullOrEmpty(xmlDeclaration)) == true ? xmlDeclaration : defaultDeclatation);
                            XmlString = xmlDeclaration + String.Join("", xmlDataList.Where(c => c.isValid == true).Select(c => c.pTag).ToArray());


                        }
                        else if (RegexExpression.Expression.Equals(@"(?<=</p>)(?!<p)(.*?)(?=<)"))
                        {
                            MatchCollection Nostarttag = System.Text.RegularExpressions.Regex.Matches(XmlString, @"(?<=</p>)(?!<p)(.*?)(?=<)", RegexOptions.Multiline);
                            foreach (Match _tag in Nostarttag)
                            {
                                if (!string.IsNullOrEmpty(_tag.Value.Trim()))
                                    XmlString = XmlString.Replace(_tag.Value, "<p " + _tag.Value);
                            }
                        }
                        else if (RegexExpression.Expression.Contains(@"<\?xml"))
                        {
                            //RegexExpression.Expression = RegexExpression.Expression.Replace("&lt;", "<");
                            MatchCollection xmlDeclaration = System.Text.RegularExpressions.Regex.Matches(XmlString, RegexExpression.Expression, RegexOptions.Multiline);
                            if (xmlDeclaration.Count > 0)
                            {
                                XmlString = System.Text.RegularExpressions.Regex.Replace(XmlString, RegexExpression.Expression, string.Empty);
                                XmlString = xmlDeclaration[0] + XmlString;
                            }
                        }
                        else if (RegexExpression.Expression.Equals(@"(<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*""\s*end=""\s*\w*"")>(.*?)(?=</div>))"))
                        {
                            MatchCollection lastTagCheck = System.Text.RegularExpressions.Regex.Matches(XmlString, @"(<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*""\s*end=""\s*\w*"")>(.*?)(?=</div>))", RegexOptions.RightToLeft);
                            foreach (Match match in lastTagCheck)
                            {
                                if (!string.IsNullOrEmpty(match.Value))
                                {
                                    if (System.Text.RegularExpressions.Regex.IsMatch(match.Value, "^((?!</p>).)*$"))
                                    {
                                        XmlString = System.Text.RegularExpressions.Regex.Replace(XmlString, @"(<p\s*(\bxml:id=""\s*\w*""\s*begin=""\s*\w*""\s*end=""\s*\w*"")>(.*?)(?=</div>))", "$0</p>", RegexOptions.RightToLeft);
                                        MatchCollection lastTagData = System.Text.RegularExpressions.Regex.Matches(match.Value, @"(?<=<p)(.*?)$", RegexOptions.Singleline);
                                        if (lastTagData.Count > 0)
                                        {
                                            XmlString = XmlString.Replace(lastTagData[0].Value, lastTagData[0].Value.Replace("<", "&lt;"));
                                        }
                                    }
                                }
                            }
                        }
                        else if (RegexExpression.Expression.Equals(@"(?<=</p>)(.*?)(?=</div>)"))
                        {
                            XmlString = System.Text.RegularExpressions.Regex.Replace(XmlString, RegexExpression.Expression, RegexExpression.ReplaceWith, RegexOptions.RightToLeft);
                        }
                        else
                        {

                            XmlString = System.Text.RegularExpressions.Regex.Replace(XmlString, RegexExpression.Expression, RegexExpression.ReplaceWith);
                        }

                    }


                    OutputXml.ProcessedXml = XmlString;

                    try
                    {
                        XDocument _xdoc = XDocument.Parse(XmlString);
                        OutputXml.IsSuccess = true;



                    }
                    catch (Exception InnerException)
                    {
                        OutputXml.Exception = InnerException.Message;
                        OutputXml.IsSuccess = false;
                    }



                }
            }
            catch (Exception _Exception)
            {
                OutputXml.IsSuccess = false;
                OutputXml.Exception = "Message : "+_Exception.Message +"\n Inner Exception : "+_Exception.InnerException+"\n Stack Trace : "+_Exception.StackTrace;
            }

            return OutputXml;
        }
    }

    [XmlType("RegexExp")]
    public class RegexExp
    {
        [XmlElement(ElementName = "Regex")]
        public List<Regex> Regex { get; set; }
    }

    public class Regex
    {
        [XmlAttribute]
        public string Expression { get; set; }

        [XmlAttribute]
        public string ReplaceWith { get; set; }
    }

    public class OutputXml
    {

        public string ProcessedXml { get; set; }
        public bool IsSuccess { get; set; }
        public string Exception { get; set; }
    }

    public class XMLData
    {
        public string pTag { get; set; }
        public Boolean isValid { get; set; }
    }
}
