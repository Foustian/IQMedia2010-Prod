﻿using System.Collections.Generic;

namespace IQMedia.Services.SMS.Config.Sections.Mappings
{
    public class Mappings
    {
        /// <summary>
        /// List of UrlMappings specified in Web.config for mapping
        /// the services URL to the specified command.
        /// </summary>
        public List<UrlMapping> UrlMappings { get; set; }
    }
}