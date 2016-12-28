﻿using System.Xml.Serialization;

namespace IQMediaGroup.Services.IOS.Web.Config.Sections.Mappings
{
    public class UrlMapping
    {
        /// <summary>
        /// The URL path to be mapped by services
        /// </summary>
        [XmlAttribute]
        public string Url { get; set; }

        /// <summary>
        /// The ActionClass object specifying the class (command) to instantiate
        /// and how to pass the parameters to it.
        /// </summary>
        public ActionClass ActionClass { get; set; }
    }
}