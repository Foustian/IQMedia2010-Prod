using System;
using System.Xml.Serialization;

namespace IQMediaGroup.Services.IOS.Web.Serializers
{
    public class ErrorSerializer : IXmlSerializable
    {
        private readonly Guid _assetGuid;
        private readonly Exception _exception;

        public ErrorSerializer(Exception ex)
            : this(ex, Guid.Empty) { }

        public ErrorSerializer(Exception ex, Guid assetGuid)
        {
            _exception = ex;
            _assetGuid = assetGuid;
        }

        #region IXmlSerializable Members

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("root");
            writer.WriteAttributeString("status", (null == _exception) ? "1" : "0");
            writer.WriteAttributeString("msg", (null == _exception) ? "" : _exception.Message);
            writer.WriteAttributeString("rid", _assetGuid.ToString());
            writer.WriteEndElement();
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}