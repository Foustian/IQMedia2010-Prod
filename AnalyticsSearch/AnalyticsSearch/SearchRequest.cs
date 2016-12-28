using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnalyticsSearch
{
    public class SearchRequest
    {
        List<string> _searchRequestIDs = null;
        DateTime? _fromDate = null;
        DateTime? _toDate = null;
        MediaTypes? _mediaType = null;
        string _clientGUID = null;
        string _dateInterval = null;
        string _tab = null;
        string _route = null;

        public List<string> SearchRequestIDs
        {
            get { return _searchRequestIDs; }
            set { _searchRequestIDs = value; }
        }

        public DateTime? FromDate
        {
            get { return _fromDate; }
            set { _fromDate = value; }
        }

        public DateTime? ToDate
        {
            get { return _toDate; }
            set { _toDate = value; }
        }

        public string ClientGUID
        {
            get { return _clientGUID; }
            set { _clientGUID = value; }
        }

        public string DateInterval
        {
            get { return _dateInterval; }
            set { _dateInterval = value; }
        }

        public string Tab
        {
            get { return _tab; }
            set { _tab = value; }
        }
    }

    public enum MediaTypes
    {
        TV
    }

    public enum ResponseType
    {
        XML,
        json
    }
}
