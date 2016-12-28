using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FeedsSearch
{
    public class SearchResult
    {
        /// <summary>
        /// Raw XML response from the web service
        /// </summary>
        public string ResponseXml
        {
            get { return _responseXML; }
            set { _responseXML = value; }
        } string _responseXML = null;

        /// <summary>
        /// Number of documents that matched this search request and will be displayed on the page. 
        /// </summary>
        public int TotalDisplayedHitCount
        {
            get { return _displayedHitCount; }
            set { _displayedHitCount = value; }
            
        } int _displayedHitCount = 0;

        /// <summary>
        /// Number of documents that matched this search request.
        /// </summary>
        public int TotalHitCount
        {
            get { return _hitCount; }
            set { _hitCount = value; }
        } int _hitCount = 0;

        /// <summary>
        /// The max ID of all existing records at the time of initial page load.
        /// </summary>
        public Int64 SinceID
        {
            get { return _sinceID; }
            set { _sinceID = value; }
        } Int64 _sinceID = 0;

        /// <summary>
        /// Original Search Request, including search terms and parameters
        /// </summary>
        public SearchRequest OriginalRequest
        {
            get { return _req; }
            set { _req = value; }
        } SearchRequest _req = null;

        /// <summary>
        /// List of matching documents.
        /// </summary>
        public List<Hit> Hits
        {
            get { return _hits; }
            set { _hits = value; }
        } List<Hit> _hits = new List<Hit>();
    }
}
