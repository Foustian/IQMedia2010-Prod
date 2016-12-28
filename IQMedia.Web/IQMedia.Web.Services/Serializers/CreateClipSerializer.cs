using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using IQMedia.Common.Util;
using IQMedia.Domain;
using IQMedia.Logic;

namespace IQMedia.Web.Services.Serializers
{
    public class CreateClipSerializer : IXmlSerializable
    {
        public Guid RequestGuid { get; set; }
        public Guid SessionGuid { get; set; }
        public Clip Clip { get; set; }
        public Exception Exception { get; set; }
        
        #region IXmlSerializable Members

        /// <summary>
        /// Generates an clip from the XML request.
        /// </summary>
        /// <param name="reader">The <see cref="T:System.Xml.XmlReader"/> stream from which the object is deserialized.</param>
        public void ReadXml(XmlReader reader)
        {
            try
            {
                var xmlDoc = new XmlDocument();
                xmlDoc.Load(reader);
                var nav = xmlDoc.CreateNavigator();
                
                nav.MoveToChild(XPathNodeType.Element);
                RequestGuid = new Guid(nav.GetAttribute("rid", ""));
                SessionGuid = new Guid(nav.GetAttribute("sid", ""));

                Clip = new Clip { ClipInfo = new ClipInfo(), ClipMeta = new List<ClipMeta>() };
                nav.MoveToChild(XPathNodeType.Element);

                var fileGuid = new Guid(nav.GetAttribute("fileId", ""));
                var rfileLgc = (RecordfileLogic) LogicFactory.GetLogic(LogicType.Recordfile);
                Clip.Recordfile = rfileLgc.GetRecordfile(fileGuid);

                Logger.Info(
                    String.Format("Creating clip, given ST/ET of {0}/{1}", nav.GetAttribute("startTime", ""),
                                  nav.GetAttribute("endTime", "")));
                Clip.Guid = Guid.NewGuid();
                Clip.ClipInfo.ClipGuid = Clip.Guid;
                Clip.EndOffset = Convert.ToInt32(nav.GetAttribute("endTime", ""));
                Clip.StartOffset = Convert.ToInt32(nav.GetAttribute("startTime", ""));
                Clip.ClipInfo.Keywords = nav.GetAttribute("keywords", "");
                Clip.ClipInfo.CategoryKey = nav.GetAttribute("category", "");
                Clip.ClipInfo.Title = nav.GetAttribute("title", "");
                Clip.UserGuid = new Guid(nav.GetAttribute("userId", ""));
                Clip.ClipInfo.Description = nav.Value;
                Clip.DateCreated = DateTime.Now;
                Logger.Info(
                    String.Format("Creating clip ({0}) for user {1} using RF {2}. ST:{3} END:{4}", Clip.Guid, Clip.UserGuid,
                                  Clip.Recordfile.Guid, Clip.StartOffset, Clip.EndOffset));

                //look for metaData...
                //This is a complicated if statement... We're making sure that we can move to 
                //the next element, that its name is 'clipmeta' and then we're moving to its
                //first child.
                if (nav.MoveToNext() && nav.Name == "clipmeta" && nav.MoveToFirstChild())
                {
                    do
                    {
                        var meta = new ClipMeta {
                            ClipGuid = Clip.Guid,
                            Field = nav.GetAttribute("key", ""),
                            Value = nav.GetAttribute("value", "") };
                        Clip.ClipMeta.Add(meta);
                    } while (nav.MoveToNext());
                }
                
            }
            catch (Exception ex)
            {
                Logger.Error("An error occurred when deserializing the CreateClip request.", ex);
                throw;
            }
        }

        /// <summary>
        /// Serializes an XML response for the CreateClip request.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Xml.XmlWriter"/> stream to which the object is serialized.</param>
        public void WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("root");
                writer.WriteAttributeString("status", (null == Exception) ? "1" : "0");
                writer.WriteAttributeString("msg", (null == Exception) ? "" : Exception.Message);
                writer.WriteAttributeString("sid", SessionGuid.ToString());
                writer.WriteAttributeString("rid", RequestGuid.ToString());
                writer.WriteCData((null == Clip) ? "" : Clip.Guid.ToString());
            writer.WriteEndElement();
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}