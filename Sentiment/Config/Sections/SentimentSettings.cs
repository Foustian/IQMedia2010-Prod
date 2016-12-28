using System;
using System.Collections.Generic;

namespace Sentiment.Config.Sections
{
    public class SentimentSettings
    {
        public Sentiment TV { get; set; }

        public Sentiment SM { get; set; }

        public Sentiment NM { get; set; }

        public Sentiment Twitter { get; set; }
    }
}
