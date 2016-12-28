using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Configuration;

namespace FeedsSearch
{
    public class SearchEngine
    {
        /// <summary>
        /// URL of the RESTSearch web service to connect to
        /// </summary>
        public System.Uri Url
        {
            get { return _Url; }
            set { _Url = value; }
        } System.Uri _Url;

        private bool Usev5MediaTypes;
        private string MediaCategoryField;
        private string MediaTypeField;

        public SearchEngine(System.Uri url)
        {
            this.Url = url;
            this.Usev5MediaTypes = true;

            MediaCategoryField = "mediacategoryv5";
            MediaTypeField = "mediatypev5";
        }

        public SearchEngine(System.Uri url, bool Usev5MediaTypes)
        {
            this.Url = url;
            this.Usev5MediaTypes = Usev5MediaTypes;

            if (Usev5MediaTypes)
            {
                MediaCategoryField = "mediacategoryv5";
                MediaTypeField = "mediatypev5";
            }
            else
            {
                MediaCategoryField = "mediacategory";
                MediaTypeField = "mediatype";
            }
        }

        public Dictionary<string, SearchResult> Search(SearchRequest request, Int32? timeOutPeriod = null, string CustomSolrFl = "")
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                UTF8Encoding enc = new UTF8Encoding();
                Dictionary<string, SearchResult> dictResults = new Dictionary<string, SearchResult>();
                List<Task> lstTasks = new List<Task>();

                CommonFunction.LogInfo("Feeds Call Start", request.IsLogging, request.LogFileLocation);

                List<KeyValuePair<string, string>> vars = new List<KeyValuePair<string, string>>();

                // 'Query' , we will pass in q query parameter of solr and 
                // 'FQuery' we will pass in the fq query parameter of solr 
                StringBuilder Query = new StringBuilder();
                StringBuilder FQuery = new StringBuilder();

                if (!request.IncludeDeleted)
                {
                    Query = Query.Append("isdeleted:0");
                }

                if (String.IsNullOrEmpty(Query.ToString()))
                {
                    Query = Query.Append("*:*");
                }

                // FQuery Fields
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    FQuery = FQuery.AppendFormat(" ({0}:\"{2}\" OR {1}:\"{2}\")", "titleandactornamegen", "highlightingtextgen", request.Keyword);
                }

                if (request.ClientGUID.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" clientguid:{0}", request.ClientGUID.Value.ToString().ToUpper());
                }

                if (request.ParentID.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" (parentid:{0} OR iqseqid:{0})", request.ParentID.Value);
                }

                if (request.MediaIDs != null && request.MediaIDs.Count > 0)
                {
                    string idList = String.Empty;
                    foreach (string ID in request.MediaIDs)
                    {
                        idList += ID + " ";
                    }

                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" (iqseqid:({0})", idList);
                    if (request.SearchOnParentID)
                    {
                        FQuery = FQuery.AppendFormat(" OR parentid:({0})", idList);
                    }
                    FQuery = FQuery.Append(")");
                }

                if (request.SearchRequestIDs != null && request.SearchRequestIDs.Count > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" +requestid:(");
                    foreach (string searchRequestID in request.SearchRequestIDs)
                    {
                        FQuery = FQuery.Append(searchRequestID + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (request.FromDate != null && request.ToDate != null)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" mediadatedt:[");
                    FQuery = FQuery.Append(request.FromDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                    FQuery = FQuery.Append("Z TO ");
                    FQuery = FQuery.Append(request.ToDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                    FQuery = FQuery.Append("Z]");
                }

                // BEFORE EXTRA AND
                if (request.MediaCategories != null && request.MediaCategories.Count > 0)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" +{0}:(", MediaCategoryField);
                    foreach (string mediaCategory in request.MediaCategories)
                    {
                        FQuery = FQuery.Append(mediaCategory + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (request.ExcludeMediaCategories != null && request.ExcludeMediaCategories.Count > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" -{0}:(", MediaCategoryField);
                    foreach (string mediaCategory in request.ExcludeMediaCategories)
                    {
                        FQuery = FQuery.Append(mediaCategory + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (request.SentimentFlag.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    if (request.SentimentFlag.Value == -1)
                    {
                        FQuery.Append(" negativesentiment:[1 TO *]");
                    }
                    else if (request.SentimentFlag.Value == 1)
                    {
                        FQuery.Append(" positivesentiment:[1 TO *]");
                    }
                    else
                    {
                        FQuery.Append(" negativesentiment:0 AND positivesentiment:0");
                    }
                }

                if (!String.IsNullOrEmpty(request.Dma))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" market:{0}", request.Dma);
                }

                if (request.DmaIDs != null && request.DmaIDs.Count > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" +dmaid:(");
                    foreach (string id in request.DmaIDs)
                    {
                        FQuery = FQuery.Append(id + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (!String.IsNullOrEmpty(request.Outlet))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" outlet:{0}", request.Outlet);
                }

                if (!String.IsNullOrEmpty(request.Station))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" stationid:{0}", request.Station);
                }

                if (!String.IsNullOrEmpty(request.TwitterHandle))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" actorpreferrednamegen:{0}", request.TwitterHandle);
                }

                if (!String.IsNullOrEmpty(request.Publication))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" publicationgen:{0}", request.Publication);
                }

                if (!String.IsNullOrEmpty(request.Author))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" authorgen:{0}", request.Author);
                }

                if (!string.IsNullOrEmpty(request.ShowTitle))
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" titlestr:{0}", request.ShowTitle);
                }

                if (!string.IsNullOrEmpty(request.stationAffil))
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" stationaffil:{0}", request.stationAffil);
                }

                if (!string.IsNullOrEmpty(request.demographic))
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    switch (request.demographic)
                    {
                        case "male":
                            FQuery = FQuery.Append("(am1:[1 TO *] OR am2:[1 TO *] OR am3:[1 TO *] OR am4:[1 TO *] OR am5:[1 TO *] OR am6:[1 TO *])");
                            break;
                        case "female":
                            FQuery = FQuery.Append("(af1:[1 TO *] OR af2:[1 TO *] OR af3:[1 TO *] OR af4:[1 TO *] OR af5:[1 TO *] OR af6:[1 TO *])");
                            break;
                        case "18-24":
                            FQuery = FQuery.Append("(am1:[1 TO *] OR af1:[1 TO *])");
                            break;
                        case "25-34":
                            FQuery = FQuery.Append("(am2:[1 TO *] OR af2:[1 TO *])");
                            break;
                        case "35-49":
                            FQuery = FQuery.Append("(am3:[1 TO *] OR af3:[1 TO *])");
                            break;
                        case "50-54":
                            FQuery = FQuery.Append("(am4:[1 TO *] OR af4:[1 TO *])");
                            break;
                        case "55-64":
                            FQuery = FQuery.Append("(am5:[1 TO *] OR af5:[1 TO *])");
                            break;
                        case "65+":
                            FQuery = FQuery.Append("(am6:[1 TO *] OR af6:[1 TO *])");
                            break;
                    }
                }

                if (request.DayOfWeek != null && request.DayOfWeek.Count > 0)
                {
                    StringBuilder days = new StringBuilder();
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND (");
                    }

                    foreach (var dow in request.DayOfWeek)
                    {
                        if (!string.IsNullOrEmpty(days.ToString()))
                        {
                            days = days.Append(" OR");
                        }

                        // useGMT to distinguish between Analytics daytime/daypart tabs - both use the DayOfWeek prop but use different solr fields
                        if (request.useGMT != null && (bool)request.useGMT)
                        {
                            days = days.AppendFormat(" gmtdow:{0}", dow);
                        }
                        else
                        {
                            days = days.AppendFormat(" localdow:{0}", dow);
                        }
                    }

                    FQuery = FQuery.AppendFormat("{0})", days);
                }

                if (request.TimeOfDay != null && request.TimeOfDay.Count > 0)
                {
                    StringBuilder times = new StringBuilder();
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND (");
                    }

                    foreach (var tod in request.TimeOfDay)
                    {
                        if (!string.IsNullOrEmpty(times.ToString()))
                        {
                            times.Append(" OR");
                        }

                        // useGMT to distinguish between Analytics daytime/daypart tabs - both use the TimeOfDay prop but use different solr fields
                        if (request.useGMT != null && (bool)request.useGMT)
                        {
                            times.AppendFormat(" gmttod:{0}", tod);
                        }
                        else
                        {
                            times.AppendFormat(" localtod:{0}", tod);
                        }
                    }

                    FQuery = FQuery.AppendFormat("{0})", times);
                }

                if (request.ExcludeIDs != null && request.ExcludeIDs.Count > 0)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" -iqseqid:(");
                    foreach (string ID in request.ExcludeIDs)
                    {
                        FQuery = FQuery.Append(ID + " ");
                    }
                    FQuery = FQuery.Append(")");
                }

                if (request.ExcludeSearchRequestIDs != null && request.ExcludeSearchRequestIDs.Count > 0)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" -requestid:(");
                    foreach (string ID in request.ExcludeSearchRequestIDs)
                    {
                        FQuery = FQuery.Append(ID + " ");
                    }
                    FQuery = FQuery.Append(")");
                }

                if (request.IsRead.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" {0}(isread:1", request.IsRead.Value ? String.Empty : "-");
                    
                    if (request.IsReadIncludeIDs != null && request.IsReadIncludeIDs.Count > 0)
                    {
                        FQuery = FQuery.AppendFormat(" OR {0}iqseqid:(", request.IsRead.Value ? String.Empty : "!");
                        foreach (string ID in request.IsReadIncludeIDs)
                        {
                            FQuery = FQuery.Append(ID + " ");
                        }
                        FQuery = FQuery.Append(")");
                    }

                    FQuery = FQuery.Append(")");
                }

                if (request.usePESHFilters)
                {
                    StringBuilder PESHQuery = new StringBuilder();
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND (");
                    }
                    else
                    {
                        FQuery = FQuery.Append("(");
                    }

                    if (request.IsHeardFilter)
                    {
                        PESHQuery.AppendFormat("({0}:TV AND (heardstatus:0", MediaCategoryField);
                        if (request.isPaidFilter && !request.isEarnedFilter)
                        {
                            PESHQuery.Append(" OR heardpaidhits:[1 TO *]");
                        }
                        else if (!request.isPaidFilter && request.isEarnedFilter)
                        {
                            PESHQuery.Append(" OR heardearnedhits:[1 TO *]");
                        }
                        else
                        {
                            PESHQuery.Append(" OR heardearnedhits:[1 TO *] OR heardpaidhits:[1 TO *]");
                        }
                        PESHQuery.Append("))");
                    }

                    if (request.isSeenFilter)
                    {
                        if (!string.IsNullOrEmpty(PESHQuery.ToString()))
                        {
                            PESHQuery.Append(" OR");
                        }
                        if (request.isPaidFilter && !request.isEarnedFilter)
                        {
                            PESHQuery.Append(" seenpaidhits:[1 TO *]");
                        }
                        else if (!request.isPaidFilter && request.isEarnedFilter)
                        {
                            PESHQuery.Append(" seenearnedhits:[1 TO *]");
                        }
                        else
                        {
                            PESHQuery.Append(" seenearnedhits:[1 TO *] OR seenpaidhits:[1 TO *]");
                        }
                    }

                    if (!request.isSeenFilter && !request.IsHeardFilter)
                    {
                        if (request.isPaidFilter)
                        {
                            PESHQuery.Append(" heardpaidhits:[1 TO *] OR seenpaidhits:[1 TO *]");
                        }
                        if (request.isEarnedFilter)
                        {
                            if (!String.IsNullOrEmpty(PESHQuery.ToString()))
                            {
                                FQuery = FQuery.Append(" OR");
                            }

                            PESHQuery.AppendFormat(" heardearnedhits:[1 TO *] OR seenearnedhits:[1 TO *] OR {0}:(NM SM BL PR TM FO)", MediaTypeField);
                        }
                    }

                    if (request.isInProgramFilter)
                    {
                        if (!string.IsNullOrEmpty(PESHQuery.ToString()))
                        {
                            PESHQuery.Append(" OR");
                        }
                        if (request.isSeenFilter)
                        {
                            PESHQuery.Append(" seeninprogramhits:[1 TO *]");
                        }
                        if (request.IsHeardFilter)
                        {
                            if (!string.IsNullOrEmpty(PESHQuery.ToString()))
                            {
                                FQuery = FQuery.Append(" OR");
                            }

                            PESHQuery.Append(" heardinprogramhits:[1 TO *]");
                        }
                    }

                    FQuery.AppendFormat("{0})", PESHQuery.ToString());
                }

                if (request.SinceID.HasValue && request.SinceID.Value > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" iqseqid:[1 TO {0}]", request.SinceID.Value);
                }

                if (request.SinceIDAsc.HasValue)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" iqseqid:[{0} TO *]", request.SinceIDAsc.Value);
                }

                string SortFields = String.Empty;

                if (request.ParentID.HasValue && request.MediaCategories != null && request.MediaCategories.Count > 0)
                {
                    if (request.MediaCategories.IndexOf("TV") >= 0)
                    {
                        // If getting TV child results, the highest ranked DMA with the latest local time should be the first result
                        SortFields = "dmaid asc, localdatedt desc, parentid asc, ";
                    }
                    else
                    {
                        // If getting NM child results, the parent should be the first result
                        SortFields = "parentid asc, ";
                    }
                }

                // Sort Fields
                switch (request.SortType)
                {
                    case SortType.DATE:
                        SortFields += "mediadatedt " + (request.IsSortAsc ? "asc" : "desc");
                        break;
                    case SortType.ARTICLE_WEIGHT:
                        SortFields += "iqprominencemultiplier desc, mediadatedt desc";
                        break;
                    case SortType.OUTLET_WEIGHT:
                        SortFields += "iqprominence desc, mediadatedt desc";
                        break;
                    case SortType.IQSEQID:
                        SortFields += "iqseqid " + (request.IsSortAsc ? "asc" : "desc");
                        break;
                }

                vars.Add(new KeyValuePair<string, string>("sort", SortFields));

                vars.Add(new KeyValuePair<string, string>("q", Query.ToString()));

                if (!string.IsNullOrWhiteSpace(FQuery.ToString()))
                {
                    vars.Add(new KeyValuePair<string, string>("fq", FQuery.ToString()));
                }

                // If filtering on prominence, it's necessary to first run a query to get the cutoff value, which can then be used to get the actual results
                if (request.IQProminence.HasValue)
                {
                    string searchField = request.IsProminenceAudience ? "iqprominence" : "iqprominencemultiplier";

                    List<KeyValuePair<string, string>> prominenceVars = new List<KeyValuePair<string, string>>(vars);
                    prominenceVars.Add(new KeyValuePair<string, string>("rows", "0"));
                    prominenceVars.Add(new KeyValuePair<string, string>("stats", "on"));
                    prominenceVars.Add(new KeyValuePair<string, string>("stats.field", String.Format("{{!percentiles=\"{0}\"}}{1}", request.IQProminence.Value, searchField)));

                    CommonFunction.LogInfo("Prominence Percentile Call", request.IsLogging, request.LogFileLocation);
                    string prominenceXml = RestClient.getXML(Url.AbsoluteUri, prominenceVars, request.IsLogging, request.LogFileLocation, timeOutPeriod);
                    string cutoffValue = GetNodeValue(prominenceXml, "/response/lst[@name='stats']/lst/lst/lst/double");

                    if (!String.IsNullOrEmpty(cutoffValue))
                    {
                        vars.Add(new KeyValuePair<string, string>("fq", String.Format("{0}:[{1} TO *]", searchField, cutoffValue)));
                    }
                }

                // Displayed Results
                List<KeyValuePair<string, string>> resultVars = new List<KeyValuePair<string, string>>(vars);
                if (request.FromRecordID.HasValue)
                {
                    resultVars.Add(new KeyValuePair<string, string>("start", Convert.ToString(request.FromRecordID, System.Globalization.CultureInfo.CurrentCulture)));
                }
                if (!String.IsNullOrEmpty(request.FieldList))
                {
                    resultVars.Add(new KeyValuePair<string, string>("fl", request.FieldList));
                }
                if (request.IsOnlyParents)
                {
                    resultVars.Add(new KeyValuePair<string, string>("fq", "parentid:0"));
                }
                if (!request.ParentID.HasValue)
                {
                    resultVars.Add(new KeyValuePair<string, string>("rows", Convert.ToString(request.PageSize, System.Globalization.CultureInfo.CurrentCulture)));
                }
                else
                {
                    // Child Search - Don't know exact result count so set rows to extremely high value
                    resultVars.Add(new KeyValuePair<string, string>("rows", "10000"));
                }

                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(resultVars, "Displayed Results Call", request.IsLogging, request.LogFileLocation), "Results", TaskCreationOptions.AttachedToParent));

                // Facet Counts - Only needed for new searches, not for paging requests
                if (request.IsFaceting)
                {
                    vars.Add(new KeyValuePair<string, string>("facet", "on"));
                    vars.Add(new KeyValuePair<string, string>("facet.limit", "-1"));
                    vars.Add(new KeyValuePair<string, string>("facet.mincount", "1"));
                    vars.Add(new KeyValuePair<string, string>("rows", "0"));
                    if (request.ResponseType != null)
                    {
                        vars.Add(new KeyValuePair<string, string>("wt", request.ResponseType.ToString()));
                    }

                    // In order to load the page faster, defer faceting on initial load until after the results have been retrieved
                    if (!request.IsInitialSearch)
                    {
                        // Simultaneously run each facet as it's own query to improve performance
                        List<KeyValuePair<string, string>> catVars = new List<KeyValuePair<string, string>>(vars);
                        if (Usev5MediaTypes)
                        {
                            catVars.Add(new KeyValuePair<string, string>("facet.pivot", "mediatypev5,mediacategoryv5"));
                        }
                        else
                        {
                            catVars.Add(new KeyValuePair<string, string>("facet.field", "mediacategory"));
                        }
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(catVars, "MediaCategory Facet Call", request.IsLogging, request.LogFileLocation), "MediaCategoryFacet", TaskCreationOptions.AttachedToParent));
                   
                        List<KeyValuePair<string, string>> sentVars = new List<KeyValuePair<string, string>>(vars);
                        sentVars.Add(new KeyValuePair<string, string>("facet.field", "positivesentiment"));
                        sentVars.Add(new KeyValuePair<string, string>("facet.field", "negativesentiment"));
                        sentVars.Add(new KeyValuePair<string, string>("facet.query", "positivesentiment:0 AND negativesentiment:0"));
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(sentVars, "Sentiment Facet Call", request.IsLogging, request.LogFileLocation), "SentimentFacet", TaskCreationOptions.AttachedToParent));

                        List<KeyValuePair<string, string>> dmaVars = new List<KeyValuePair<string, string>>(vars);
                        dmaVars.Add(new KeyValuePair<string, string>("facet.pivot", "dmaid,market"));
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(dmaVars, "DMA Facet Call", request.IsLogging, request.LogFileLocation), "DmaFacet", TaskCreationOptions.AttachedToParent));

                        List<KeyValuePair<string, string>> agentVars = new List<KeyValuePair<string, string>>(vars);
                        agentVars.Add(new KeyValuePair<string, string>("facet.pivot", "requestid,searchagentname"));
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(agentVars, "Agent Facet Call", request.IsLogging, request.LogFileLocation), "AgentFacet", TaskCreationOptions.AttachedToParent));

                        List<KeyValuePair<string, string>> dateVars = new List<KeyValuePair<string, string>>(vars);
                        dateVars.Add(new KeyValuePair<string, string>("facet.range", "localdatedt"));
                        dateVars.Add(new KeyValuePair<string, string>("facet.range.start", request.FromDateLocal.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")));
                        dateVars.Add(new KeyValuePair<string, string>("facet.range.end", request.ToDateLocal.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")));
                        dateVars.Add(new KeyValuePair<string, string>("facet.range.gap", "+1DAY"));
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(dateVars, "Date Facet Call", request.IsLogging, request.LogFileLocation), "DateFacet", TaskCreationOptions.AttachedToParent));

                        string readIDs = "";
                        string unreadIDs = "";
                        List<KeyValuePair<string, string>> readVars = new List<KeyValuePair<string, string>>(vars);
                        if (request.QueuedAsReadIDs != null && request.QueuedAsReadIDs.Count > 0)
                        {
                            readIDs = String.Join(" ", request.QueuedAsReadIDs);
                        }
                        if (request.QueuedAsUnreadIDs != null && request.QueuedAsUnreadIDs.Count > 0)
                        {
                            unreadIDs = String.Join(" ", request.QueuedAsUnreadIDs);
                        }
                        readVars.Add(new KeyValuePair<string, string>("facet.query", String.Format("(isread:1 OR iqseqid:(0 {0})) AND -iqseqid:(0 {1})", readIDs, unreadIDs)));
                        readVars.Add(new KeyValuePair<string, string>("facet.query", String.Format("-((isread:1 OR iqseqid:(0 {0})) AND -iqseqid:(0 {1}))", readIDs, unreadIDs)));
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(readVars, "IsRead Facet Call", request.IsLogging, request.LogFileLocation), "IsReadFacet", TaskCreationOptions.AttachedToParent));

                        // These parameters must be added in this order, since the result xml will be parsed using that expectation.
                        // Don't facet on seen if only filtering to heard and vice versa.
                        List<KeyValuePair<string, string>> seenHeardVars = new List<KeyValuePair<string, string>>(vars);
                        seenHeardVars.Add(new KeyValuePair<string, string>("fq", String.Format("{0}:TV", MediaCategoryField)));
                        if (request.IsHeardFilter || !request.isSeenFilter) { seenHeardVars.Add(new KeyValuePair<string, string>("facet.query", "heardstatus:0 OR heardpaidhits:[1 TO *] OR heardearnedhits:[1 TO *]")); }
                        if (request.isSeenFilter || !request.IsHeardFilter) { seenHeardVars.Add(new KeyValuePair<string, string>("facet.query", "seenpaidhits:[1 TO *] OR seenearnedhits:[1 TO *]")); }
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(seenHeardVars, "SeenHeard Facet Call", request.IsLogging, request.LogFileLocation), "SeenHeardFacet", TaskCreationOptions.AttachedToParent));
                    }

                    // Get child counts even on initial load, so that we know which results have children
                    if (request.IsOnlyParents)
                    {
                        List<KeyValuePair<string, string>> parentVars = new List<KeyValuePair<string, string>>(vars);
                        parentVars.Add(new KeyValuePair<string, string>("facet.field", "parentid"));
                        lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(parentVars, "Parent Facet Call", request.IsLogging, request.LogFileLocation), "ParentFacet", TaskCreationOptions.AttachedToParent));
                    }
                }

                // If this is the first search after page load, run a query to get the max existing record ID
                if (request.IsInitialSearch)
                {
                    List<KeyValuePair<string, string>> sinceVars = new List<KeyValuePair<string, string>>();
                    sinceVars.Add(new KeyValuePair<string, string>("sort", "iqseqid desc"));
                    sinceVars.Add(new KeyValuePair<string, string>("fl", "iqseqid"));
                    sinceVars.Add(new KeyValuePair<string, string>("rows", "1"));
                    sinceVars.Add(new KeyValuePair<string, string>("q", "*:*"));

                    lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(sinceVars, "SinceID Call", request.IsLogging, request.LogFileLocation), "SinceID", TaskCreationOptions.AttachedToParent));
                }

                // Run each query in parallel to minimize processing time
                try
                {
                    Task.WaitAll(lstTasks.ToArray(), 90000);
                }
                catch (AggregateException _Exception)
                {
                    CommonFunction.LogError("Exception:" + _Exception.Message + " :: " + _Exception.StackTrace, request.IsLogging, request.LogFileLocation);
                }
                catch (Exception _Exception)
                {
                    CommonFunction.LogError("Exception:" + _Exception.Message + " :: " + _Exception.StackTrace, request.IsLogging, request.LogFileLocation);
                }

                Dictionary<string, SearchResult> dictResponses = new Dictionary<string, SearchResult>();
                foreach (var tsk in lstTasks)
                {
                    SearchResult taskRes = ((Task<SearchResult>)tsk).Result;
                    string taskType = (string)tsk.AsyncState;

                    dictResponses.Add(taskType, taskRes);
                }

                CommonFunction.LogInfo("Solr Response - TimeTaken - for get response" + string.Format("with thread : Minutes :{0}  Seconds :{1}  Milliseconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds), request.IsLogging, request.LogFileLocation);

                SearchResult res = dictResponses["Results"];
                XmlDocument xDoc = new XmlDocument();

                res.OriginalRequest = request;
                if (request.SinceID.HasValue)
                {
                    res.SinceID = request.SinceID.Value;
                }

                // lets load solr response to xml so we can get data in xml format
                xDoc.Load(new MemoryStream(enc.GetBytes(res.ResponseXml)));

                CommonFunction.LogInfo("Parse Response", request.IsLogging, request.LogFileLocation);

                parseResponse(xDoc, res);
                dictResults.Add("Results", res);

                // Set new SinceID if necessary
                if (dictResponses.ContainsKey("SinceID"))
                {
                    string sinceID = GetNodeValue(dictResponses["SinceID"].ResponseXml, "/response/result/doc/long");
                    if (!String.IsNullOrEmpty(sinceID))
                    {
                        res.SinceID = Convert.ToInt64(sinceID);
                    }
                }

                // Create facet result objects
                foreach (KeyValuePair<string, SearchResult> kvResponse in dictResponses)
                {
                    switch (kvResponse.Key)
                    {
                        case "MediaCategoryFacet":
                        case "SentimentFacet":
                        case "DmaFacet":
                        case "AgentFacet":
                        case "DateFacet":
                        case "ParentFacet":
                        case "IsReadFacet":
                        case "SeenHeardFacet":
                            SearchResult resFacet = kvResponse.Value;
                            XmlDocument xDocFacet = new XmlDocument();
                            xDocFacet.Load(new MemoryStream(enc.GetBytes(resFacet.ResponseXml)));
                            parseResponse(xDocFacet, resFacet);
                            dictResults.Add(kvResponse.Key, resFacet);

                            if (kvResponse.Key == "ParentFacet")
                            {
                                res.TotalHitCount = resFacet.TotalDisplayedHitCount;
                            }
                            break;
                    }
                }

                sw.Stop();

                CommonFunction.LogInfo("Solr Response - TimeTaken - for parse response" + string.Format("with thread : Minutes :{0}  Seconds :{1}  Milliseconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds), request.IsLogging, request.LogFileLocation);

                CommonFunction.LogInfo(string.Format("Total Hit Count: {0} ({1})", res.TotalDisplayedHitCount, res.TotalHitCount), request.IsLogging, request.LogFileLocation);

                CommonFunction.LogInfo("Feeds Call End", request.IsLogging, request.LogFileLocation);

                return dictResults;
            }
            catch (Exception _Exception)
            {
                CommonFunction.LogError("Exception:" + _Exception.Message + " :: " + _Exception.StackTrace, request.IsLogging, request.LogFileLocation);

                SearchResult res = new SearchResult();
                res.ResponseXml = "<response status=\"0\">" + _Exception.Message + "</response>";

                Dictionary<string, SearchResult> dictResults = new Dictionary<string, SearchResult>();
                dictResults.Add("Results", res);
                return dictResults;
            }
        }

        public Dictionary<string, SearchResult> SearchFacets(SearchRequest request, Int32? timeOutPeriod = null, string CustomSolrFL = "")
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                UTF8Encoding enc = new UTF8Encoding();
                Dictionary<string, SearchResult> dictResults = new Dictionary<string, SearchResult>();
                List<Task> lstTasks = new List<Task>();

                CommonFunction.LogInfo("Feeds Call Start", request.IsLogging, request.LogFileLocation);

                List<KeyValuePair<string, string>> vars = new List<KeyValuePair<string, string>>();

                // 'Query' , we will pass in q query parameter of solr and 
                // 'FQuery' we will pass in the fq query parameter of solr 
                StringBuilder Query = new StringBuilder();
                StringBuilder FQuery = new StringBuilder();

                if (!request.IncludeDeleted)
                {
                    Query = Query.Append("isdeleted:0");
                }

                if (String.IsNullOrEmpty(Query.ToString()))
                {
                    Query = Query.Append("*:*");
                }

                // FQuery Fields
                if (!string.IsNullOrWhiteSpace(request.Keyword))
                {
                    FQuery = FQuery.AppendFormat(" ({0}:\"{2}\" OR {1}:\"{2}\")", "titleandactornamegen", "highlightingtextgen", request.Keyword);
                }

                if (request.ClientGUID.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" clientguid:{0}", request.ClientGUID.Value.ToString().ToUpper());
                }

                if (request.MediaIDs != null && request.MediaIDs.Count > 0)
                {
                    string idList = String.Empty;
                    foreach (string ID in request.MediaIDs)
                    {
                        idList += ID + " ";
                    }

                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" (iqseqid:({0})", idList);
                    if (request.SearchOnParentID)
                    {
                        FQuery = FQuery.AppendFormat(" OR parentid:({0})", idList);
                    }
                    FQuery = FQuery.Append(")");
                }

                if (request.SearchRequestIDs != null && request.SearchRequestIDs.Count > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" +requestid:(");
                    foreach (string searchRequestID in request.SearchRequestIDs)
                    {
                        FQuery = FQuery.Append(searchRequestID + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (request.FromDate != null && request.ToDate != null)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" mediadatedt:[");
                    FQuery = FQuery.Append(request.FromDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                    FQuery = FQuery.Append("Z TO ");
                    FQuery = FQuery.Append(request.ToDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                    FQuery = FQuery.Append("Z]");
                }

                if (request.MediaCategories != null && request.MediaCategories.Count > 0)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" +{0}:(", MediaCategoryField);
                    foreach (string mediaCategory in request.MediaCategories)
                    {
                        FQuery = FQuery.Append(mediaCategory + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (request.ExcludeMediaCategories != null && request.ExcludeMediaCategories.Count > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" -{0}:(", MediaCategoryField);
                    foreach (string mediaCategory in request.ExcludeMediaCategories)
                    {
                        FQuery = FQuery.Append(mediaCategory + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (request.SentimentFlag.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    if (request.SentimentFlag.Value == -1)
                    {
                        FQuery.Append(" negativesentiment:[1 TO *]");
                    }
                    else if (request.SentimentFlag.Value == 1)
                    {
                        FQuery.Append(" positivesentiment:[1 TO *]");
                    }
                    else
                    {
                        FQuery.Append(" negativesentiment:0 AND positivesentiment:0");
                    }
                }

                if (!String.IsNullOrEmpty(request.Dma))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" market:{0}", request.Dma);
                }

                if (request.DmaIDs != null && request.DmaIDs.Count > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" +dmaid:(");
                    foreach (string id in request.DmaIDs)
                    {
                        FQuery = FQuery.Append(id + " ");
                    }
                    FQuery = FQuery.Append(" )");
                }

                if (!String.IsNullOrEmpty(request.Outlet))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" outlet:{0}", request.Outlet);
                }

                if (!String.IsNullOrEmpty(request.Station))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" stationid:{0}", request.Station);
                }

                if (!String.IsNullOrEmpty(request.TwitterHandle))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" actorpreferrednamegen:{0}", request.TwitterHandle);
                }

                if (!String.IsNullOrEmpty(request.Publication))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" publicationgen:{0}", request.Publication);
                }

                if (!String.IsNullOrEmpty(request.Author))
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" authorgen:{0}", request.Author);
                }

                if (!string.IsNullOrEmpty(request.ShowTitle))
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" titlestr:{0}", request.ShowTitle);
                }

                if (!string.IsNullOrEmpty(request.stationAffil))
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" stationaffil:{0}", request.stationAffil);
                }

                if (!string.IsNullOrEmpty(request.demographic))
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    switch (request.demographic)
                    {
                        case "male":
                            FQuery = FQuery.Append("(am1:[1 TO *] OR am2:[1 TO *] OR am3:[1 TO *] OR am4:[1 TO *] OR am5:[1 TO *] OR am6:[1 TO *])");
                            break;
                        case "female":
                            FQuery = FQuery.Append("(af1:[1 TO *] OR af2:[1 TO *] OR af3:[1 TO *] OR af4:[1 TO *] OR af5:[1 TO *] OR af6:[1 TO *])");
                            break;
                        case "18-24":
                            FQuery = FQuery.Append("(am1:[1 TO *] OR af1:[1 TO *])");
                            break;
                        case "25-34":
                            FQuery = FQuery.Append("(am2:[1 TO *] OR af2:[1 TO *])");
                            break;
                        case "35-49":
                            FQuery = FQuery.Append("(am3:[1 TO *] OR af3:[1 TO *])");
                            break;
                        case "50-54":
                            FQuery = FQuery.Append("(am4:[1 TO *] OR af4:[1 TO *])");
                            break;
                        case "55-64":
                            FQuery = FQuery.Append("(am5:[1 TO *] OR af5:[1 TO *])");
                            break;
                        case "65+":
                            FQuery = FQuery.Append("(am6:[1 TO *] OR af6:[1 TO *])");
                            break;
                    }
                }

                if (request.DayOfWeek != null && request.DayOfWeek.Count > 0)
                {
                    StringBuilder days = new StringBuilder();
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND (");
                    }

                    foreach (var dow in request.DayOfWeek)
                    {
                        if (!string.IsNullOrEmpty(days.ToString()))
                        {
                            days = days.Append(" OR");
                        }

                        // useGMT to distinguish between Analytics daytime/daypart tabs - both use the DayOfWeek prop but use different solr fields
                        if (request.useGMT != null && (bool)request.useGMT)
                        {
                            days = days.AppendFormat(" gmtdow:{0}", dow);
                        }
                        else
                        {
                            days = days.AppendFormat(" localdow:{0}", dow);
                        }
                    }

                    FQuery = FQuery.AppendFormat("{0})", days);
                }

                if (request.TimeOfDay != null && request.TimeOfDay.Count > 0)
                {
                    StringBuilder times = new StringBuilder();
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND (");
                    }

                    foreach (var tod in request.TimeOfDay)
                    {
                        if (!string.IsNullOrEmpty(times.ToString()))
                        {
                            times.Append(" OR");
                        }

                        // useGMT to distinguish between Analytics daytime/daypart tabs - both use the TimeOfDay prop but use different solr fields
                        if (request.useGMT != null && (bool)request.useGMT)
                        {
                            times.AppendFormat(" gmttod:{0}", tod);
                        }
                        else
                        {
                            times.AppendFormat(" localtod:{0}", tod);
                        }
                    }

                    FQuery = FQuery.AppendFormat("{0})", times);
                }

                if (request.ExcludeIDs != null && request.ExcludeIDs.Count > 0)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" -iqseqid:(");
                    foreach (string ID in request.ExcludeIDs)
                    {
                        FQuery = FQuery.Append(ID + " ");
                    }
                    FQuery = FQuery.Append(")");
                }

                if (request.ExcludeSearchRequestIDs != null && request.ExcludeSearchRequestIDs.Count > 0)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.Append(" -requestid:(");
                    foreach (string ID in request.ExcludeSearchRequestIDs)
                    {
                        FQuery = FQuery.Append(ID + " ");
                    }
                    FQuery = FQuery.Append(")");
                }

                if (request.IsRead.HasValue)
                {
                    if (!String.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" {0}(isread:1", request.IsRead.Value ? String.Empty : "-");

                    if (request.IsReadIncludeIDs != null && request.IsReadIncludeIDs.Count > 0)
                    {
                        FQuery = FQuery.AppendFormat(" OR {0}iqseqid:(", request.IsRead.Value ? String.Empty : "!");
                        foreach (string ID in request.IsReadIncludeIDs)
                        {
                            FQuery = FQuery.Append(ID + " ");
                        }
                        FQuery = FQuery.Append(")");
                    }

                    FQuery = FQuery.Append(")");
                }

                if (request.usePESHFilters)
                {
                    StringBuilder PESHQuery = new StringBuilder();
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND (");
                    }
                    else
                    {
                        FQuery = FQuery.Append("(");
                    }

                    if (request.IsHeardFilter)
                    {
                        PESHQuery.AppendFormat("({0}:TV AND (heardstatus:0", MediaCategoryField);
                        if (request.isPaidFilter && !request.isEarnedFilter)
                        {
                            PESHQuery.Append(" OR heardpaidhits:[1 TO *]");
                        }
                        else if (!request.isPaidFilter && request.isEarnedFilter)
                        {
                            PESHQuery.Append(" OR heardearnedhits:[1 TO *]");
                        }
                        else
                        {
                            PESHQuery.Append(" OR heardearnedhits:[1 TO *] OR heardpaidhits:[1 TO *]");
                        }
                        PESHQuery.Append("))");
                    }

                    if (request.isSeenFilter)
                    {
                        if (!string.IsNullOrEmpty(PESHQuery.ToString()))
                        {
                            PESHQuery.Append(" OR");
                        }
                        if (request.isPaidFilter && !request.isEarnedFilter)
                        {
                            PESHQuery.Append(" seenpaidhits:[1 TO *]");
                        }
                        else if (!request.isPaidFilter && request.isEarnedFilter)
                        {
                            PESHQuery.Append(" seenearnedhits:[1 TO *]");
                        }
                        else
                        {
                            PESHQuery.Append(" seenearnedhits:[1 TO *] OR seenpaidhits:[1 TO *]");
                        }
                    }

                    if (!request.isSeenFilter && !request.IsHeardFilter)
                    {
                        if (request.isPaidFilter)
                        {
                            PESHQuery.Append(" heardpaidhits:[1 TO *] OR seenpaidhits:[1 TO *]");
                        }
                        if (request.isEarnedFilter)
                        {
                            if (!String.IsNullOrEmpty(PESHQuery.ToString()))
                            {
                                FQuery = FQuery.Append(" OR");
                            }

                            PESHQuery.AppendFormat(" heardearnedhits:[1 TO *] OR seenearnedhits:[1 TO *] OR {0}:(NM SM BL PR TM FO)", MediaTypeField);
                        }
                    }
                    FQuery.AppendFormat("{0})", PESHQuery.ToString());
                }

                if (request.SinceID.HasValue && request.SinceID.Value > 0)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" iqseqid:[1 TO {0}]", request.SinceID.Value);
                }

                if (request.SinceIDAsc.HasValue)
                {
                    if (!string.IsNullOrEmpty(FQuery.ToString()))
                    {
                        FQuery = FQuery.Append(" AND");
                    }

                    FQuery = FQuery.AppendFormat(" iqseqid:[{0} TO *]", request.SinceIDAsc.Value);
                }

                vars.Add(new KeyValuePair<string, string>("q", Query.ToString()));

                if (!string.IsNullOrWhiteSpace(FQuery.ToString()))
                {
                    vars.Add(new KeyValuePair<string, string>("fq", FQuery.ToString()));
                }

                // If filtering on prominence, it's necessary to first run a query to get the cutoff value, which can then be used to get the actual results
                if (request.IQProminence.HasValue)
                {
                    string searchField = request.IsProminenceAudience ? "iqprominence" : "iqprominencemultiplier";

                    List<KeyValuePair<string, string>> prominenceVars = new List<KeyValuePair<string, string>>(vars);
                    prominenceVars.Add(new KeyValuePair<string, string>("rows", "0"));
                    prominenceVars.Add(new KeyValuePair<string, string>("stats", "on"));
                    prominenceVars.Add(new KeyValuePair<string, string>("stats.field", String.Format("{{!percentiles=\"{0}\"}}{1}", request.IQProminence.Value, searchField)));

                    CommonFunction.LogInfo("Prominence Percentile Call", request.IsLogging, request.LogFileLocation);
                    string prominenceXml = RestClient.getXML(Url.AbsoluteUri, prominenceVars, request.IsLogging, request.LogFileLocation, timeOutPeriod);
                    string cutoffValue = GetNodeValue(prominenceXml, "/response/lst[@name='stats']/lst/lst/lst/double");

                    if (!String.IsNullOrEmpty(cutoffValue))
                    {
                        vars.Add(new KeyValuePair<string, string>("fq", String.Format("{0}:[{1} TO *]", searchField, cutoffValue)));
                    }
                }

                // Perform facet queries
                vars.Add(new KeyValuePair<string, string>("facet", "on"));
                vars.Add(new KeyValuePair<string, string>("facet.limit", "-1"));
                vars.Add(new KeyValuePair<string, string>("facet.mincount", "1"));
                vars.Add(new KeyValuePair<string, string>("rows", "0"));
                if (request.ResponseType != null)
                {
                    vars.Add(new KeyValuePair<string, string>("wt", request.ResponseType.ToString()));
                }

                // Simultaneously run each facet as it's own query to improve performance
                List<KeyValuePair<string, string>> catVars = new List<KeyValuePair<string, string>>(vars);
                if (Usev5MediaTypes)
                {
                    catVars.Add(new KeyValuePair<string, string>("facet.pivot", "mediatypev5,mediacategoryv5"));
                }
                else
                {
                    catVars.Add(new KeyValuePair<string, string>("facet.field", "mediacategory"));
                }
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(catVars, "MediaCategory Facet Call", request.IsLogging, request.LogFileLocation), "MediaCategoryFacet", TaskCreationOptions.AttachedToParent));

                List<KeyValuePair<string, string>> sentVars = new List<KeyValuePair<string, string>>(vars);
                sentVars.Add(new KeyValuePair<string, string>("facet.field", "positivesentiment"));
                sentVars.Add(new KeyValuePair<string, string>("facet.field", "negativesentiment"));
                sentVars.Add(new KeyValuePair<string, string>("facet.query", "positivesentiment:0 AND negativesentiment:0"));
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(sentVars, "Sentiment Facet Call", request.IsLogging, request.LogFileLocation), "SentimentFacet", TaskCreationOptions.AttachedToParent));

                List<KeyValuePair<string, string>> dmaVars = new List<KeyValuePair<string, string>>(vars);
                dmaVars.Add(new KeyValuePair<string, string>("facet.pivot", "dmaid,market"));
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(dmaVars, "DMA Facet Call", request.IsLogging, request.LogFileLocation), "DmaFacet", TaskCreationOptions.AttachedToParent));

                List<KeyValuePair<string, string>> agentVars = new List<KeyValuePair<string, string>>(vars);
                agentVars.Add(new KeyValuePair<string, string>("facet.pivot", "requestid,searchagentname"));
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(agentVars, "Agent Facet Call", request.IsLogging, request.LogFileLocation), "AgentFacet", TaskCreationOptions.AttachedToParent));

                List<KeyValuePair<string, string>> dateVars = new List<KeyValuePair<string, string>>(vars);
                dateVars.Add(new KeyValuePair<string, string>("facet.range", "localdatedt"));
                dateVars.Add(new KeyValuePair<string, string>("facet.range.start", request.FromDateLocal.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")));
                dateVars.Add(new KeyValuePair<string, string>("facet.range.end", request.ToDateLocal.Value.ToString("yyyy-MM-dd'T'HH:mm:ss'Z'")));
                dateVars.Add(new KeyValuePair<string, string>("facet.range.gap", "+1DAY"));
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(dateVars, "Date Facet Call", request.IsLogging, request.LogFileLocation), "DateFacet", TaskCreationOptions.AttachedToParent));

                string readIDs = "";
                string unreadIDs = "";
                List<KeyValuePair<string, string>> readVars = new List<KeyValuePair<string, string>>(vars);
                if (request.QueuedAsReadIDs != null && request.QueuedAsReadIDs.Count > 0)
                {
                    readIDs = String.Join(" ", request.QueuedAsReadIDs);
                }
                if (request.QueuedAsUnreadIDs != null && request.QueuedAsUnreadIDs.Count > 0)
                {
                    unreadIDs = String.Join(" ", request.QueuedAsUnreadIDs);
                }
                readVars.Add(new KeyValuePair<string, string>("facet.query", String.Format("(isread:1 OR iqseqid:(0 {0})) AND -iqseqid:(0 {1})", readIDs, unreadIDs)));
                readVars.Add(new KeyValuePair<string, string>("facet.query", String.Format("-((isread:1 OR iqseqid:(0 {0})) AND -iqseqid:(0 {1}))", readIDs, unreadIDs)));
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(readVars, "IsRead Facet Call", request.IsLogging, request.LogFileLocation), "IsReadFacet", TaskCreationOptions.AttachedToParent));

                // These parameters must be added in this order, since the result xml will be parsed using that expectation
                // Don't facet on seen if only filtering to heard and vice versa.
                List<KeyValuePair<string, string>> seenHeardVars = new List<KeyValuePair<string, string>>(vars);
                seenHeardVars.Add(new KeyValuePair<string,string>("fq", String.Format("{0}:TV", MediaCategoryField)));
                if (request.IsHeardFilter || !request.isSeenFilter) { seenHeardVars.Add(new KeyValuePair<string, string>("facet.query", "heardstatus:0 OR heardpaidhits:[1 TO *] OR heardearnedhits:[1 TO *]")); }
                if (request.isSeenFilter || !request.IsHeardFilter) { seenHeardVars.Add(new KeyValuePair<string, string>("facet.query", "seenpaidhits:[1 TO *] OR seenearnedhits:[1 TO *]")); }
                lstTasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(seenHeardVars, "SeenHeard Facet Call", request.IsLogging, request.LogFileLocation), "SeenHeardFacet", TaskCreationOptions.AttachedToParent));

                try
                {
                    Task.WaitAll(lstTasks.ToArray(), 90000);
                }
                catch (AggregateException _Exception)
                {
                    CommonFunction.LogError("Exception:" + _Exception.Message + " :: " + _Exception.StackTrace, request.IsLogging, request.LogFileLocation);
                }
                catch (Exception _Exception)
                {
                    CommonFunction.LogError("Exception:" + _Exception.Message + " :: " + _Exception.StackTrace, request.IsLogging, request.LogFileLocation);
                }

                Dictionary<string, SearchResult> dictResponses = new Dictionary<string, SearchResult>();
                foreach (var tsk in lstTasks)
                {
                    SearchResult taskRes = ((Task<SearchResult>)tsk).Result;
                    string taskType = (string)tsk.AsyncState;

                    dictResponses.Add(taskType, taskRes);
                }

                CommonFunction.LogInfo("Solr Response - TimeTaken - for get response" + string.Format("with thread : Minutes :{0}  Seconds :{1}  Milliseconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds), request.IsLogging, request.LogFileLocation);

                // Create facet result objects
                foreach (KeyValuePair<string, SearchResult> kvResponse in dictResponses)
                {
                    SearchResult resFacet = kvResponse.Value;
                    XmlDocument xDocFacet = new XmlDocument();
                    xDocFacet.Load(new MemoryStream(enc.GetBytes(resFacet.ResponseXml)));
                    parseResponse(xDocFacet, resFacet);
                    dictResults.Add(kvResponse.Key, resFacet);
                }

                sw.Stop();

                CommonFunction.LogInfo("Solr Response - TimeTaken - for parse response" + string.Format("with thread : Minutes :{0}  Seconds :{1}  Milliseconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds), request.IsLogging, request.LogFileLocation);
                
                CommonFunction.LogInfo("Feeds Call End", request.IsLogging, request.LogFileLocation);

                return dictResults;
            }
            catch (Exception _Exception)
            {
                CommonFunction.LogError("Exception:" + _Exception.Message + " :: " + _Exception.StackTrace, request.IsLogging, request.LogFileLocation);

                SearchResult res = new SearchResult();
                res.ResponseXml = "<response status=\"0\">" + _Exception.Message + "</response>";

                return new Dictionary<string, SearchResult>();
            }
        }

        public SearchResult SearchForDashboard(SearchRequest request, Int32? timeOutPeriod = null, string CustomSolrFl = "")
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            try
            {
                CommonFunction.LogInfo("Feeds Dashboard Call Start", request.IsLogging, request.LogFileLocation);

                UTF8Encoding enc = new UTF8Encoding();
                Dictionary<string, SearchResult> dictResults = new Dictionary<string, SearchResult>();

                List<KeyValuePair<string, string>> vars = new List<KeyValuePair<string, string>>();

                // 'Query' , we will pass in q query parameter of solr and 
                // 'FQuery' we will pass in the fq query parameter of solr 
                StringBuilder Query = new StringBuilder();
                StringBuilder FQuery = new StringBuilder();

                if (request.MediaIDs == null || request.MediaIDs.Count == 0)
                {
                    if (!request.IncludeDeleted)
                    {
                        Query = Query.Append("isdeleted:0");
                    }

                    if (String.IsNullOrEmpty(Query.ToString()))
                    {
                        Query = Query.Append("*:*");
                    }

                    // FQuery Fields
                    if (!string.IsNullOrWhiteSpace(request.Keyword))
                    {
                        FQuery = FQuery.AppendFormat(" ({0}:\"{2}\" OR {1}:\"{2}\")", "titleandactornamegen", "highlightingtextgen", request.Keyword);
                    }

                    if (request.ClientGUID.HasValue)
                    {
                        if (!String.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.AppendFormat(" clientguid:{0}", request.ClientGUID.Value.ToString().ToUpper());
                    }

                    if (request.SearchRequestIDs != null && request.SearchRequestIDs.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" +requestid:(");
                        foreach (string searchRequestID in request.SearchRequestIDs)
                        {
                            FQuery = FQuery.Append(searchRequestID + " ");
                        }
                        FQuery = FQuery.Append(" )");
                    }

                    if (request.SearchRequestIDs != null && request.SearchRequestIDs.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" +requestid:(");
                        foreach (string searchRequestID in request.SearchRequestIDs)
                        {
                            FQuery = FQuery.Append(searchRequestID + " ");
                        }
                        FQuery = FQuery.Append(" )");
                    }

                    if (request.FromDate != null && request.ToDate != null)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" mediadatedt:[");
                        FQuery = FQuery.Append(request.FromDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                        FQuery = FQuery.Append("Z TO ");
                        FQuery = FQuery.Append(request.ToDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                        FQuery = FQuery.Append("Z]");
                    }

                    if (request.MediaCategories != null && request.MediaCategories.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.AppendFormat(" +{0}:(", MediaCategoryField);
                        foreach (string mediaCategory in request.MediaCategories)
                        {
                            FQuery = FQuery.Append(mediaCategory + " ");
                        }
                        FQuery = FQuery.Append(" )");
                    }

                    if (request.ExcludeMediaCategories != null && request.ExcludeMediaCategories.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.AppendFormat(" -{0}:(", MediaCategoryField);
                        foreach (string mediaCategory in request.ExcludeMediaCategories)
                        {
                            FQuery = FQuery.Append(mediaCategory + " ");
                        }
                        FQuery = FQuery.Append(" )");
                    }

                    if (request.SentimentFlag.HasValue)
                    {
                        if (!String.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        if (request.SentimentFlag.Value == -1)
                        {
                            FQuery.Append(" negativesentiment:[1 TO *]");
                        }
                        else if (request.SentimentFlag.Value == 1)
                        {
                            FQuery.Append(" positivesentiment:[1 TO *]");
                        }
                        else
                        {
                            FQuery.Append(" negativesentiment:0 AND positivesentiment:0");
                        }
                    }

                    if (request.DmaIDs != null && request.DmaIDs.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" +dmaid:(");
                        foreach (string id in request.DmaIDs)
                        {
                            FQuery = FQuery.Append(id + " ");
                        }
                        FQuery = FQuery.Append(" )");
                    }

                    if (request.ExcludeIDs != null && request.ExcludeIDs.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" -iqseqid:(");
                        foreach (string ID in request.ExcludeIDs)
                        {
                            FQuery = FQuery.Append(ID + " ");
                        }
                        FQuery = FQuery.Append(")");
                    }

                    if (request.ExcludeSearchRequestIDs != null && request.ExcludeSearchRequestIDs.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" -requestid:(");
                        foreach (string ID in request.ExcludeSearchRequestIDs)
                        {
                            FQuery = FQuery.Append(ID + " ");
                        }
                        FQuery = FQuery.Append(")");
                    }

                    if (request.IsRead.HasValue)
                    {
                        if (!String.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.AppendFormat(" {0}(isread:1", request.IsRead.Value ? String.Empty : "-");

                        if (request.IsReadIncludeIDs != null && request.IsReadIncludeIDs.Count > 0)
                        {
                            FQuery = FQuery.AppendFormat(" OR {0}iqseqid:(", request.IsRead.Value ? String.Empty : "!");
                            foreach (string ID in request.IsReadIncludeIDs)
                            {
                                FQuery = FQuery.Append(ID + " ");
                            }
                            FQuery = FQuery.Append(")");
                        }

                        FQuery = FQuery.Append(")");
                    }

                    if (request.usePESHFilters)
                    {
                        StringBuilder PESHQuery = new StringBuilder();
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND (");
                        }
                        else
                        {
                            FQuery = FQuery.Append("(");
                        }

                        if (request.IsHeardFilter)
                        {
                            PESHQuery.AppendFormat("({0}:TV AND (heardstatus:0", MediaCategoryField);
                            if (request.isPaidFilter && !request.isEarnedFilter)
                            {
                                PESHQuery.Append(" OR heardpaidhits:[1 TO *]");
                            }
                            else if (!request.isPaidFilter && request.isEarnedFilter)
                            {
                                PESHQuery.Append(" OR heardearnedhits:[1 TO *]");
                            }
                            else
                            {
                                PESHQuery.Append(" OR heardearnedhits:[1 TO *] OR heardpaidhits:[1 TO *]");
                            }
                            PESHQuery.Append("))");
                        }

                        if (request.isSeenFilter)
                        {
                            if (!string.IsNullOrEmpty(PESHQuery.ToString()))
                            {
                                PESHQuery.Append(" OR");
                            }
                            if (request.isPaidFilter && !request.isEarnedFilter)
                            {
                                PESHQuery.Append(" seenpaidhits:[1 TO *]");
                            }
                            else if (!request.isPaidFilter && request.isEarnedFilter)
                            {
                                PESHQuery.Append(" seenearnedhits:[1 TO *]");
                            }
                            else
                            {
                                PESHQuery.Append(" seenearnedhits:[1 TO *] OR seenpaidhits:[1 TO *]");
                            }
                        }

                        if (!request.isSeenFilter && !request.IsHeardFilter)
                        {
                            if (request.isPaidFilter)
                            {
                                PESHQuery.Append(" heardpaidhits:[1 TO *] OR seenpaidhits:[1 TO *]");
                            }
                            if (request.isEarnedFilter)
                            {
                                if (!String.IsNullOrEmpty(PESHQuery.ToString()))
                                {
                                    FQuery = FQuery.Append(" OR");
                                }

                                PESHQuery.AppendFormat(" heardearnedhits:[1 TO *] OR seenearnedhits:[1 TO *] OR {0}:(NM SM BL PR TM FO)", MediaTypeField);
                            }
                        }
                        FQuery.AppendFormat("{0})", PESHQuery.ToString());
                    }

                    if (!string.IsNullOrEmpty(request.ShowTitle))
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.AppendFormat(" titlestr:{0}", request.ShowTitle);
                    }

                    if (request.DayOfWeek != null && request.DayOfWeek.Count > 0)
                    {
                        StringBuilder days = new StringBuilder();
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        foreach (var dow in request.DayOfWeek)
                        {
                            if (!string.IsNullOrEmpty(days.ToString()))
                            {
                                days = days.Append(" OR");
                            }

                            // useGMT to distinguish between Analytics daytime/daypart tabs - both use the DayOfWeek prop but use different solr fields
                            if (request.useGMT != null && (bool)request.useGMT)
                            {
                                days = days.AppendFormat(" gmtdow:{0}", dow);
                            }
                            else
                            {
                                days = days.AppendFormat(" localdow:{0}", dow);
                            }
                        }

                        FQuery = FQuery.AppendFormat("{0})", days);
                    }

                    if (request.TimeOfDay != null && request.TimeOfDay.Count > 0)
                    {
                        StringBuilder times = new StringBuilder();
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        foreach (var tod in request.TimeOfDay)
                        {
                            if (!string.IsNullOrEmpty(times.ToString()))
                            {
                                times = times.Append(" OR");
                            }

                            // useGMT to distinguish between Analytics daytime/daypart tabs - both use the DayOfWeek prop but use different solr fields
                            if (request.useGMT != null && (bool)request.useGMT)
                            {
                                times = times.AppendFormat(" gmttod:{0}", tod);
                            }
                            else
                            {
                                times = times.AppendFormat(" localtod:{0}", tod);
                            }
                        }

                        FQuery = FQuery.AppendFormat("{0})", times);
                    }

                    if (request.SinceID.HasValue && request.SinceID.Value > 0)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.AppendFormat(" iqseqid:[1 TO {0}]", request.SinceID.Value);
                    }

                    if (request.IsOnlyParents)
                    {
                        if (!string.IsNullOrEmpty(FQuery.ToString()))
                        {
                            FQuery = FQuery.Append(" AND");
                        }

                        FQuery = FQuery.Append(" parentid:0");
                    }

                    // If filtering on prominence, it's necessary to first run a query to get the cutoff value, which can then be used to get the actual results
                    if (request.IQProminence.HasValue)
                    {
                        string searchField = request.IsProminenceAudience ? "iqprominence" : "iqprominencemultiplier";

                        List<KeyValuePair<string, string>> prominenceVars = new List<KeyValuePair<string, string>>();
                        prominenceVars.Add(new KeyValuePair<string, string>("q", Query.ToString()));
                        if (!string.IsNullOrWhiteSpace(FQuery.ToString()))
                        {
                            prominenceVars.Add(new KeyValuePair<string, string>("fq", FQuery.ToString()));
                        }
                        prominenceVars.Add(new KeyValuePair<string, string>("rows", "0"));
                        prominenceVars.Add(new KeyValuePair<string, string>("stats", "on"));
                        prominenceVars.Add(new KeyValuePair<string, string>("stats.field", String.Format("{{!percentiles=\"{0}\"}}{1}", request.IQProminence.Value, searchField)));

                        CommonFunction.LogInfo("Prominence Percentile Call", request.IsLogging, request.LogFileLocation);
                        string prominenceXml = RestClient.getXML(Url.AbsoluteUri, prominenceVars, request.IsLogging, request.LogFileLocation, timeOutPeriod);
                        string cutoffValue = GetNodeValue(prominenceXml, "/response/lst[@name='stats']/lst/lst/lst/double");

                        if (!String.IsNullOrEmpty(cutoffValue))
                        {
                            if (!string.IsNullOrEmpty(FQuery.ToString()))
                            {
                                FQuery = FQuery.Append(" AND");
                            }

                            FQuery = FQuery.AppendFormat(" {0}:[{1} TO *]", searchField, cutoffValue);
                        }
                    }
                }
                else
                {
                    Query = Query.Append("*:*");
                    FQuery = FQuery.Append(" +iqseqid:(");
                    foreach (string ID in request.MediaIDs)
                    {
                        FQuery = FQuery.Append(ID + " ");
                    }
                    FQuery = FQuery.Append(")");
                }

                vars.Add(new KeyValuePair<string, string>("q", Query.ToString()));

                if (!string.IsNullOrWhiteSpace(FQuery.ToString()))
                {
                    vars.Add(new KeyValuePair<string, string>("fq", FQuery.ToString()));
                }

                // Run a query to get the min and max dates of all the selected records
                List<KeyValuePair<string, string>> rangeVars = new List<KeyValuePair<string, string>>(vars);
                rangeVars.Add(new KeyValuePair<string, string>("rows", "0"));
                rangeVars.Add(new KeyValuePair<string, string>("stats", "on"));
                rangeVars.Add(new KeyValuePair<string, string>("stats.field", "{!min=true max=true}daydt"));

                CommonFunction.LogInfo("Min/Max Date Call", request.IsLogging, request.LogFileLocation);
                string rangeXml = RestClient.getXML(Url.AbsoluteUri, rangeVars, request.IsLogging, request.LogFileLocation, timeOutPeriod);
                DateTime minDate = Convert.ToDateTime(GetNodeValue(rangeXml, "/response/lst[@name='stats']/lst/lst/date[@name='min']")).ToUniversalTime();
                DateTime maxDate = Convert.ToDateTime(GetNodeValue(rangeXml, "/response/lst[@name='stats']/lst/lst/date[@name='max']")).ToUniversalTime();
                bool isHourRange = (maxDate - minDate).TotalHours <= 48;

                // Run a query to get the faceted results
                vars.Add(new KeyValuePair<string, string>("rows", "0"));
                vars.Add(new KeyValuePair<string, string>("stats", "on"));
                vars.Add(new KeyValuePair<string, string>("stats.field", "{!tag=facet sum=true}audience"));
                vars.Add(new KeyValuePair<string, string>("stats.field", "{!tag=facet sum=true}" + (request.UseProminenceMediaValue == true ? "prominencemediavalue" : "mediavalue")));
                vars.Add(new KeyValuePair<string, string>("stats.field", "{!tag=facet sum=true}numberofhits"));
                vars.Add(new KeyValuePair<string, string>("facet", "on"));
                vars.Add(new KeyValuePair<string, string>("facet.limit", "-1"));
                vars.Add(new KeyValuePair<string, string>("facet.pivot", "{!stats=facet}" + MediaCategoryField + "," + (isHourRange ? "hourdt" : "daydt")));

                CommonFunction.LogInfo("Dashboard Results Call", request.IsLogging, request.LogFileLocation);

                SearchResult res = new SearchResult();
                res.ResponseXml = RestClient.getXML(Url.AbsoluteUri, vars, request.IsLogging, request.LogFileLocation, timeOutPeriod);

                CommonFunction.LogInfo("Solr Response - TimeTaken - for get response" + string.Format("with thread : Minutes :{0}  Seconds :{1}  Milliseconds :{2}", sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.TotalMilliseconds), request.IsLogging, request.LogFileLocation);
                
                return res;
            }
            catch (Exception ex)
            {
                CommonFunction.LogError("Exception:" + ex.Message + " :: " + ex.StackTrace, request.IsLogging, request.LogFileLocation);

                SearchResult res = new SearchResult();
                res.ResponseXml = "<response status=\"0\">" + ex.Message + "</response>";

                return res;
            }
        }

        private SearchResult ExecuteSearch(List<KeyValuePair<string, string>> vars, string logMessage, bool isLogging, string logFileLocation)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();

                string requestUrl;
                string xml = RestClient.getXML(Url.AbsoluteUri, vars, isLogging, logFileLocation, out requestUrl);
                CommonFunction.LogInfo("\"" + logMessage + " (" + sw.ElapsedMilliseconds + "ms),\"" + requestUrl, isLogging, logFileLocation);
                sw.Stop();

                SearchResult res = new SearchResult();
                res.ResponseXml = xml;
                return res;
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains("RequestUrl"))
                {
                    CommonFunction.LogError("\"" + logMessage + " - ERROR,\"Error occurred for request url " + ex.Data["RequestUrl"], OverrideConfig: true);
                }
                throw;
            }
        }

        private static void parseResponse(XmlDocument doc, SearchResult res)
        {
            XmlNode root = doc.SelectSingleNode("/response/result");

            if (root != null) try { 
                res.TotalDisplayedHitCount = Convert.ToInt32(root.Attributes.GetNamedItem("numFound").Value, System.Globalization.CultureInfo.CurrentCulture);
                res.TotalHitCount = res.TotalDisplayedHitCount;
            }
            catch (Exception) { }

            // lets get list of all the hits we get 
            XmlNodeList hitNodes = doc.SelectNodes("/response/result/doc");

            res.Hits = new List<Hit>();

            // now we will parse each hit one by one. 
            foreach (XmlNode hitNode in hitNodes)
            {
                Hit hit = parseHit(hitNode);
                res.Hits.Add(hit);
            }
        }

        private static Hit parseHit(XmlNode node)
        {
            Hit hit = new Hit();
            foreach (XmlNode cn in node.ChildNodes)
            {
                try
                {
                    // we will set all properties for our Hit object
                    // which we can parse by getting 'name' attribute of child node of signle search node. 
                    switch (cn.Attributes["name"].Value.ToLower(System.Globalization.CultureInfo.CurrentCulture))
                    {
                        case "iqseqid":
                            hit.ID = Convert.ToInt64(cn.InnerText);
                            break;
                        case "mediaid":
                            hit.MediaID = Convert.ToInt64(cn.InnerText);
                            break;
                        case "articleid":
                            hit.ArticleID = cn.InnerText;
                            break;
                        case "parentid":
                            hit.ParentID = Convert.ToInt32(cn.InnerText);
                            break;
                        case "mediadate":
                            hit.MediaDate = Convert.ToDateTime(cn.InnerText);
                            break;
                        case "localdate":
                            hit.LocalDate = Convert.ToDateTime(cn.InnerText);
                            break;
                        case "mediatype":
                            hit.MediaType = cn.InnerText;
                            break;
                        case "mediacategory":
                            hit.MediaCategory = cn.InnerText;
                            break;
                        case "mediatypev5":
                            hit.v5MediaType = cn.InnerText;
                            break;
                        case "mediacategoryv5":
                            hit.v5MediaCategory = cn.InnerText;
                            break;
                        case "title":
                            hit.Title = cn.InnerText;
                            break;
                        case "positivesentiment":
                            hit.PositiveSentiment = Convert.ToInt16(cn.InnerText);
                            break;
                        case "negativesentiment":
                            hit.NegativeSentiment = Convert.ToInt16(cn.InnerText);
                            break;
                        case "numberofhits":
                            hit.NumberOfHits = Convert.ToInt32(cn.InnerText);
                            break;
                        case "mediavalue":
                            hit.MediaValue = Decimal.Parse(cn.InnerText, NumberStyles.Any);
                            break;
                        case "iqprominence":
                            hit.IQProminence = Decimal.Parse(cn.InnerText, NumberStyles.Any);
                            break;
                        case "iqprominencemultiplier":
                            hit.IQProminenceMultiplier = Decimal.Parse(cn.InnerText, NumberStyles.Any);
                            break;
                        case "highlightingtext":
                            hit.HighlightingText = cn.InnerText;
                            break;
                        case "requestid":
                            hit.SearchRequestID = Convert.ToInt64(cn.InnerText);
                            break;
                        case "searchrequest":
                            hit.SearchRequest = cn.InnerText;
                            break;
                        case "searchagentname":
                            hit.SearchAgentName = cn.InnerText;
                            break;
                        case "audience":
                            hit.Audience = Convert.ToInt32(cn.InnerText);
                            break;
                        case "audiencetype":
                            hit.AudienceType = cn.InnerText;
                            break;
                        case "thumburl":
                            hit.ThumbnailUrl = cn.InnerText;
                            break;
                        case "stationid":
                            hit.StationID = cn.InnerText;
                            break;
                        case "outlet":
                            hit.Outlet = cn.InnerText;
                            break;
                        case "market":
                            hit.Market = cn.InnerText;
                            break;
                        case "videoguid":
                            hit.VideoGUID = new Guid(cn.InnerText);
                            break;
                        case "timezone":
                            hit.TimeZone = cn.InnerText;
                            break;
                        case "iqlicense":
                            hit.IQLicense = Convert.ToInt16(cn.InnerText);
                            break;
                        case "url":
                            hit.Url = cn.InnerText;
                            break;
                        case "playerurl":
                            hit.PlayerUrl = cn.InnerText;
                            break;
                        case "filelocation":
                            hit.FileLocation = cn.InnerText;
                            break;
                        case "publication":
                            hit.Publication = cn.InnerText;
                            break;
                        case "actorpreferredname":
                            hit.ActorPreferredName = cn.InnerText;
                            break;
                        case "actorfriendscount":
                            hit.ActorFriendsCount = Convert.ToInt32(cn.InnerText);
                            break;
                        case "dmaid":
                            hit.DmaID = cn.InnerText;
                            break;
                        case "audiencenational":
                            hit.NationalAudience = Convert.ToInt64(cn.InnerText);
                            break;
                        case "audiencetypenational":
                            hit.NationalAudienceType = cn.InnerText;
                            break;
                        case "mediavaluenational":
                            hit.NationalMediaValue = Decimal.Parse(cn.InnerText, NumberStyles.Any);
                            break;
                        case "searchterm":
                            hit.SearchTerm = GetNodeValue(cn.InnerText, "/SearchRequest/SearchTerm");
                            break;
                        case "iqcckey":
                            hit.IQ_CC_Key = cn.InnerText;
                            break;
                        case "content":
                            hit.Content = cn.InnerText;
                            break;
                        case "stationidnum":
                            hit.StationIDNum = Convert.ToInt32(cn.InnerText);
                            break;
                        case "duration":
                            hit.Duration = Convert.ToInt32(cn.InnerText);
                            break;
                        case "copyright":
                            hit.Copyright = cn.InnerText;
                            break;
                        case "author":
                            XDocument xDoc = XDocument.Parse(cn.OuterXml);
                            hit.Authors = xDoc.Descendants("str").Where(s => !String.IsNullOrEmpty(s.Value)).Select(s => s.Value).ToList();
                            break;
                        case "availdate":
                            hit.AvailableDate = Convert.ToDateTime(cn.InnerText);
                            break;
                        case "languagenum":
                            hit.LanguageNum = Convert.ToInt16(cn.InnerText);
                            break;
                        case "articlestats":
                            hit.ArticleStats = cn.InnerText;
                            break;
                    }
                }
                catch (Exception)
                {

                } 
            }
            return hit;
        }

        private static string GetNodeValue(string xml, string nodePath)
        {
            UTF8Encoding enc = new UTF8Encoding();
            XmlDocument xDoc = new XmlDocument();

            xDoc.Load(new MemoryStream(enc.GetBytes(xml)));
            XmlNode xNode = xDoc.SelectSingleNode(nodePath);

            if (xNode != null)
            {
                return xNode.InnerText;
            }
            return String.Empty;
        }
    }    
}
