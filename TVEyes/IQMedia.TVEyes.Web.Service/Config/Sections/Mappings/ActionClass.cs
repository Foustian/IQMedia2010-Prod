using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace IQMedia.TVEyes.Web.Service.Config.Sections.Mappings
{
    public class ActionClass
    {
        /// <summary>
        /// The Assembly type of the class completing the mapped command.
        /// </summary>
        [XmlAttribute]
        public string Type { get; set; }

        /// <summary>
        /// A list of 'Parameter' objects to be passed to the ActionClass
        /// </summary>
        public List<Parameter> Parameters { get; set; }
    }
}
