using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace IQMediaGroup.Process_MalformedXml.Util
{
    public class CommonFun
    {
        ///================================================================================
        /// Function Name : MakeDeserialiazation
        /// 
        /// <summary>
        /// This fucntion performs DeSerialization of XML string and returns the object        
        /// </summary>
        /// <param name="p_sXMLString">XML String to be Deserialized</param>
        /// <param name="p_Deserialization">Deserialization object type</param>
        /// <returns>Deserialized object </returns>
        ///================================================================================
        public static object MakeDeserialiazation(string p_XMLString, object p_Deserialization)
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

        public static string AddClosingTag(string p_malFormedXML,RegexExp p_regexExp)
        {
            string resultString = string.Empty;
            resultString = p_malFormedXML;

            try
            {
                p_malFormedXML = System.Text.RegularExpressions.Regex.Replace(p_malFormedXML, @"(?<!</tt>\s*)(?=$)", "</tt>");
                p_malFormedXML = System.Text.RegularExpressions.Regex.Replace(p_malFormedXML, @"(?<!</body>\s*)(?=</tt>)", "</body>\r\n");
                p_malFormedXML = System.Text.RegularExpressions.Regex.Replace(p_malFormedXML, @"(?<!</div>\s*)(?=</body>)", "</div>\r\n");
                p_malFormedXML = System.Text.RegularExpressions.Regex.Replace(p_malFormedXML, @"<p[^>]*[^/]\>[^<]+?(?=\s*\<([^/]|/(?!p)))", "$0</p>", RegexOptions.Multiline);
                resultString = p_malFormedXML;
            }
            catch (Exception ex)
            {
                return resultString;
            }
            return resultString;

        }
    }
}
