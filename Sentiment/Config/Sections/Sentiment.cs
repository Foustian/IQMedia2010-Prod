using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Sentiment.Config.Sections
{
    public class Sentiment
    {   
        public int LowThreshold { get; set; }

        public int HighThreshold { get; set; } 
    }
}
