using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Xml.Schema;
using System.Runtime.Serialization;
using System.Xml.Linq;
using System.Dynamic;

namespace IQMediaGroup.ExposeApi.Services.Serializers
{
    public class Serializer 
    {
        /// <summary>
        /// Converts object into JSON String
        /// </summary>
        /// <param name="_object">Object needs to be serialize</param>
        /// <returns>JSON result</returns>
        public static string Searialize(object _object, NullValueHandling p_NullValH=NullValueHandling.Include)
        {
            try
            {
                return JsonConvert.SerializeObject(_object, new JsonSerializerSettings { NullValueHandling=p_NullValH});
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        /// <summary>
        /// Converts JSON String into object of target type
        /// </summary>
        /// <param name="_JSONString">JSON string</param>
        /// <param name="_TargetType"></param>
        /// <returns>object converted from JSON string</returns>
        public static object Deserialize(string _JSONString, Type _TargetType)
        {
            try
            {
                return JsonConvert.DeserializeObject(_JSONString, _TargetType);
                //DataContractJsonSerializer Deserializer = new DataContractJsonSerializer(_TargetType);
                //MemoryStream MemoryStream = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(_JSONString));

                //return (Deserializer.ReadObject(MemoryStream));

                /*JavaScriptSerializer _JavaScriptSerializer = new JavaScriptSerializer();

                return _JavaScriptSerializer.Deserialize(_JSONString, _TargetType);*/
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string SerializeToXml(object p_SerializationObject)
        {
            try
            {
                string _XMLString = string.Empty;

                System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
                XmlWriterSettings _XmlWriterSettings = new XmlWriterSettings();
                // _XmlWriterSettings.Encoding=_Encoding;
                _XmlWriterSettings.OmitXmlDeclaration = true;

                XmlSerializer _XmlSerializer = new XmlSerializer(p_SerializationObject.GetType(), "");

                try
                {
                    StringWriter _StringWriter = new StringWriter();
                    using (XmlWriter _XmlWriter = XmlWriter.Create(_StringWriter,
                    _XmlWriterSettings))
                    {
                        XmlSerializerNamespaces _XmlSerializerNamespaces = new XmlSerializerNamespaces();
                        _XmlSerializerNamespaces.Add("", "");

                        _XmlSerializer.Serialize(_XmlWriter, p_SerializationObject, _XmlSerializerNamespaces);
                    }

                    _XMLString = _StringWriter.ToString();
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }

                return _XMLString;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static object DeserialiazeXml(string p_XMLString, object p_Deserialization)
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

        public static object DeserialiazeXmlDC<T>(string p_XMLString, T p_Deserialization)
        {
            DataContractSerializer s = new DataContractSerializer(typeof(T));

            StringWriter sw = new StringWriter();
            sw.Write(p_XMLString);

            XmlWriter xw = XmlWriter.Create(sw);

            s.WriteObject(xw, p_Deserialization);

            return p_Deserialization;
        }

        public static string SerializeXmlDC(object root)
        {
            string xmlString;

            var serializer = new DataContractSerializer(root.GetType());
            using (var backing = new StringWriter())
            using (var writer = new XmlTextWriter(backing))
            {
                serializer.WriteObject(writer, root);
                xmlString = backing.ToString();
            }
            xmlString = RemoveAllNamespaces(xmlString);

            return xmlString;
        }

        public static string SerializeToXmlWithoutNameSpace(object p_SerializationObject,IEnumerable<Type> types = null)
        {
            try
            {
                string _XMLString = string.Empty;

                System.Text.UTF8Encoding _Encoding = new System.Text.UTF8Encoding();
                XmlWriterSettings _XmlWriterSettings = new XmlWriterSettings();
                // _XmlWriterSettings.Encoding=_Encoding;
                _XmlWriterSettings.OmitXmlDeclaration = true;

                XmlSerializer _XmlSerializer = types == null ? new XmlSerializer(p_SerializationObject.GetType()) : new XmlSerializer(p_SerializationObject.GetType(), types.ToArray());

                try
                {
                    StringWriter _StringWriter = new StringWriter();
                    using (XmlWriter _XmlWriter = XmlWriter.Create(_StringWriter,
                    _XmlWriterSettings))
                    {
                        XmlSerializerNamespaces _XmlSerializerNamespaces = new XmlSerializerNamespaces();
                        _XmlSerializerNamespaces.Add("", "");

                        _XmlSerializer.Serialize(new NonTypeTextWriter(_StringWriter), p_SerializationObject, _XmlSerializerNamespaces);
                    }

                    _XMLString = _StringWriter.ToString();
                }
                catch (Exception _Exception)
                {
                    throw _Exception;
                }
                return _XMLString;
            }
            catch (Exception _Exception)
            {
                throw _Exception;
            }
        }

        public static string RemoveAllNamespaces(string xmlDocument)
        {
            XElement xmlDocumentWithoutNs = RemoveAllNamespaces(XElement.Parse(xmlDocument));

            XDocument xDoc = XDocument.Parse(xmlDocumentWithoutNs.ToString());
            foreach (var node in xDoc.Descendants())
            {
                node.Attributes().Remove();
            }
            return xDoc.ToString();
        }

        //Core recursion function
        private static XElement RemoveAllNamespaces(XElement xmlDocument)
        {
            if (!xmlDocument.HasElements)
            {
                XElement xElement = new XElement(xmlDocument.Name.LocalName);
                xElement.Value = xmlDocument.Value;

                foreach (XAttribute attribute in xmlDocument.Attributes())
                    xElement.Add(attribute);

                return xElement;
            }
            return new XElement(xmlDocument.Name.LocalName, xmlDocument.Elements().Select(el => RemoveAllNamespaces(el)));
        }
        
    }

    public class NonTypeTextWriter : XmlTextWriter
    {
        public NonTypeTextWriter() : base(null) { }
        public NonTypeTextWriter( TextWriter w ) : base( w ) {}
        public NonTypeTextWriter(Stream w, Encoding encoding) : base(w, encoding) { }
        
        bool _skip = false;

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            if (localName == "type" || localName == "nil") // Omits XSD and XSI declarations.
            {
                _skip = true;
                return;
            }
            base.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteString(string text)
        {
            if (_skip) return;
            base.WriteString(text);
        }
        public override void WriteEndAttribute()
        {
            if (_skip)
            {
                // Reset the flag, so we keep writing.
                _skip = false;
                return;
            }
            base.WriteEndAttribute();
        }
    }
}