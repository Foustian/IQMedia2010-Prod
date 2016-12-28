using System;
using System.IO;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using IQMedia.Web.Services.Serializers;

namespace IQMedia.Web.Services.Commands
{
    public abstract class AbstractXmlCommand : BaseCommand
    {
        protected IXmlSerializable Serializer;
        
        protected void WriteXmlOutput(HttpResponse response)
        {
            WriteXmlOutput(response, Serializer);
        }
        protected void WriteXmlOutput(HttpResponse response, IXmlSerializable serializer)
        {
            response.ContentEncoding = System.Text.Encoding.UTF8;
            response.ContentType = "text/xml";
            var sw = new StringWriter();
            var xtw = new XmlTextWriter(sw);
            serializer.WriteXml(xtw);
            response.Output.Write(sw.ToString());
        }

        protected void WriteXmlError(HttpResponse response, Exception ex)
        {
            WriteXmlError(response, ex, Guid.Empty);
        }
        protected void WriteXmlError(HttpResponse response, Exception ex, Guid assetGuid)
        {
            IXmlSerializable serializer = new ErrorSerializer(ex, assetGuid);
            WriteXmlOutput(response, serializer);
        }
    }
}