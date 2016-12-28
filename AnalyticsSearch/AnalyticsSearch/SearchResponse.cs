using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalyticsSearch
{
    public class FacetResponse
    {
        Int64 _count = 0;
        string _dmaName = null;
        Int64? _dmaID = null;
        Int64 _heardPaid = 0;
        Int64 _heardEarned = 0;
        Int64 _seenEarned = 0;
        Int64 _seenPaid = 0;
        Int64 _totalHits = 0;
        decimal _mediaValue = 0;
        Int64 _audience = 0;
        Int64 _negSentiment = 0;
        Int64 _posSentiment = 0;

        public Int64 Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public string Market
        {
            get { return _dmaName; }
            set { _dmaName = value; }
        }

        public Int64? MarketID
        {
            get { return _dmaID; }
            set { _dmaID = value; }
        }

        public Int64 HeardPaidHits
        {
            get { return _heardPaid; }
            set { _heardPaid = value; }
        }

        public Int64 HeardEarned
        {
            get { return _heardEarned; }
            set { _heardEarned = value; }
        }

        public Int64 SeenEarnedHits
        {
            get { return _seenEarned; }
            set { _seenEarned = value; }
        }

        public Int64 SeenPaidHits
        {
            get { return _seenPaid; }
            set { _seenPaid = value; }
        }

        public Int64 TotalHitCount
        {
            get { return _totalHits; }
            set { _totalHits = value; }
        }

        public decimal MediaValue
        {
            get { return _mediaValue; }
            set { _mediaValue = value; }
        }

        public Int64 Audience
        {
            get { return _audience; }
            set { _audience = value; }
        }

        public Int64 NegativeSentiment
        {
            get { return _negSentiment; }
            set { _negSentiment = value; }
        }

        public Int64 PositiveSentiment
        {
            get { return _posSentiment; }
            set { _posSentiment = value; }
        }

        public Int64 Seen
        {
            get { return _seenEarned + _seenPaid; }
        }

        public Int64 Heard
        {
            get { return _heardEarned + _heardPaid; }
        }

        public Int64 Read
        {
            get { return _totalHits - (_seenEarned + _seenPaid + _heardEarned + _heardPaid); }
        }

        public Int64 Paid
        {
            get { return _seenPaid + _heardPaid; }
        }

        public Int64 Earned
        {
            get { return _seenEarned + _heardEarned; }
        }

        public string OnAirTime
        {
            get
            {
                long onAir = this.Heard * 8 + this.Seen;
                return string.Format("{0:00}:{1:00}:{2:00}", onAir / 3600, (onAir / 60) % 60, onAir % 60);
            }
        }
    }

    /// <summary>
    /// Used to properly parse json facets
    /// </summary>
    public class RawFacet
    {
        public string val { get; set; }
        public Int64 count { get; set; }
        public Int64 hits { get; set; }
        public Int64 seen_earned { get; set; }
        public Int64 seen_paid { get; set; }
        public Int64 heard_earned { get; set; }
        public Int64 heard_paid { get; set; }
        public Int64 audience { get; set; }
        public decimal ad_value { get; set; }
        public Int64 neg_sentiment { get; set; }
        public Int64 pos_sentiment { get; set; }
    }

    public class RawDemoFacet
    {
        public DateTime val { get; set; }
        public Int64 count { get; set; }
        public Int64 af18_20 { get; set; }
        public Int64 af21_24 { get; set; }
        public Int64 af25_34 { get; set; }
        public Int64 af35_49 { get; set; }
        public Int64 af50_54 { get; set; }
        public Int64 af55_64 { get; set; }
        public Int64 af65_plus { get; set; }
        public Int64 am18_20 { get; set; }
        public Int64 am21_24 { get; set; }
        public Int64 am25_34 { get; set; }
        public Int64 am35_49 { get; set; }
        public Int64 am50_54 { get; set; }
        public Int64 am55_64 { get; set; }
        public Int64 am65_plus { get; set; }

        public object compare { get; set; }
    }

    public class RawSubFacet
    {
        public string val { get; set; }
        public Int64 count { get; set; }
        public Int64 numberofhits { get; set; }
        public List<object> facet2 { get; set; }
    }

    public class RawDateBuckets
    {
        public string val { get; set; }
        public Int64 count { get; set; }
        public Int64 numberofhits { get; set; }
    }
}
