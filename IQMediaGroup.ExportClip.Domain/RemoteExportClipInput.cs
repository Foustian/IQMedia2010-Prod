using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;


namespace IQMediaGroup.ExportClip.Domain
{
    [Serializable]
    [XmlRoot(ElementName = "ClipExport")]
    public class RemoteExportClipInput
    {
        public Guid ClipGUID { get; set; }

        public string ClipFTPLocation { get; set; }

        public ClipInfo ClipInfo { get; set; }
    }
}
