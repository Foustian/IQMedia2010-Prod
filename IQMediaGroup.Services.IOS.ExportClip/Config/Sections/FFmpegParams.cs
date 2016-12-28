using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMediaGroup.Services.IOS.ExportClip.Config.Sections
{
    public class FFmpegParams
    {
        /// <summary>
        /// FFmpeg params
        /// </summary>
        [XmlAttribute]
        public int Params { get; set; }        
        /// <summary>
        /// Extention of Input File
        /// </summary>
        /// 
        [XmlAttribute]
        public string InputFileExt { get; set; }

        /// <summary>
        /// Extention of Output File
        /// </summary>        
        [XmlAttribute]
        public string OutputFileExt { get; set; }        
    }
}
