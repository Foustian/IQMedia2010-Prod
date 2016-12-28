using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Newtonsoft.Json;

namespace AnalyticsSearch
{
    public class SearchEngine
    {
        Uri _Url;
        string _shards;

        public Uri Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        public string shards
        {
            get { return _shards; }
            set { _shards = value; }
        }

        public SearchEngine(Uri url)
        {
            this.Url = url;
        }

        public List<FacetResponse> Search(SearchRequest request)
        {
            try
            {
                CommonFunctions.LogDebug("AnalyticsSearch.Search");
                Stopwatch sw = new Stopwatch();
                sw.Start();

                UTF8Encoding enc = new UTF8Encoding();
                List<FacetResponse> response = new List<FacetResponse>();
                List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
                List<Task> tasks = new List<Task>();

                // query passes in q parameter
                string query = "*:*";
                parameters.Add(new KeyValuePair<string, string>("q", query));

                // fQuery passes in fq parameter
                StringBuilder fQuery = new StringBuilder();

                if (request.SearchRequestIDs != null && request.SearchRequestIDs.Count > 0)
                {
                    fQuery.Append("(searchrequestid:");

                    int i = 0;
                    foreach (string SRID in request.SearchRequestIDs)
                    {
                        if (i == 0)
                        {
                            fQuery.Append(SRID);
                        }
                        else
                        {
                            fQuery.Append(" OR searchrequestid:" + SRID);
                        }

                        i++;
                    }

                    fQuery.Append(")");
                }

                if (request.FromDate != null && request.ToDate != null)
                {
                    if (!string.IsNullOrEmpty(fQuery.ToString()))
                    {
                        fQuery.Append(" AND");
                    }
                    if (request.DateInterval == "hour")
                    {
                        fQuery.Append(" gmthourdatetimetd:[");
                    }
                    else
                    {
                        fQuery.Append(" daydatetd:[");
                    }

                    fQuery.Append(request.FromDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                    fQuery.Append("Z TO ");
                    fQuery.Append(request.ToDate.Value.ToString("s", System.Globalization.CultureInfo.CurrentCulture));
                    fQuery.Append("Z]");
                }

                parameters.Add(new KeyValuePair<string, string>("fq", fQuery.ToString()));

                string routing = string.Format("{0}!", request.ClientGUID);
                if (request.FromDate.Value.Year == request.ToDate.Value.Year)
                {
                    routing = string.Format("{0}{1}!", routing, request.FromDate.Value.Year);
                }
                else
                {
                    // TODO - what/how would we handle a request across multiple years?
                }

                routing = string.Format("\"{0}\"", routing);

                parameters.Add(new KeyValuePair<string, string>("_route_", routing));
                parameters.Add(new KeyValuePair<string, string>("wt", "json"));
                parameters.Add(new KeyValuePair<string, string>("indent", "true"));
                parameters.Add(new KeyValuePair<string, string>("rows", "0"));

                string overviewDayfacet = "{overtime: {type: terms,field: searchrequestid,limit: -1,facet: {hits: \"sum(ctnumberofhits)\",seen_earned: \"sum(ctseen_earned)\",seen_paid: \"sum(ctseen_paid)\",heard_earned: \"sum(ctheard_earned)\",heard_paid: \"sum(ctheard_paid)\",audience: \"sum(cttotalaudiences)\",ad_value: \"sum(ctiqmediavalue)\",neg_sentiment: \"sum(negativesentiment)\",pos_sentiment: \"sum(positivesentiment)\"}}}";

                string overviewHourFacet = "{overtime: {type: terms,field: searchrequestid,limit: -1,facet: {hits: \"sum(numberofhits)\",seen_earned: \"sum(seen_earned)\",seen_paid: \"sum(seen_paid)\",heard_earned: \"sum(heard_earned)\",heard_paid: \"sum(heard_paid)\",audience: \"sum(totalaudiences)\",advalue: \"sum(iqmediavalue)\",neg_sentiment: \"sum(negativesentiment)\",pos_sentiment: \"sum(positivesentiment)\"}}}";

                string demofacet = "{demographic: {type: terms,field: daydate,limit: -1,facet: {af18_20: \"sum(ctaf18_20)\",af21_24: \"sum(ctaf21_24)\",af25_34: \"sum(ctaf25_34)\",af35_49: \"sum(ctaf35_49)\",af50_54: \"sum(ctaf50_54)\",af55_64: \"sum(ctaf55_64)\",af65_plus: \"sum(ctaf65_plus)\",am18_20: \"sum(ctam18_20)\",am21_24: \"sum(ctam21_24)\",am25_34: \"sum(ctam25_34)\",am35_49: \"sum(ctam35_49)\",am50_54: \"sum(ctam50_54)\",am55_64: \"sum(ctam55_64)\",am65_plus: \"sum(ctam65_plus)\"}}}";

                switch(request.Tab)
                {
                    case "OverTime":
                        if (request.DateInterval == "day")
                        {
                            parameters.Add(new KeyValuePair<string, string>("json.facet", overviewDayfacet));
                        }
                        else if (request.DateInterval == "hour")
                        {
                            parameters.Add(new KeyValuePair<string, string>("json.facet", overviewHourFacet));
                        }
                        break;
                    case "Market":
                        if (request.DateInterval == "day")
                        {

                        }
                        else if (request.DateInterval == "hour")
                        {

                        }
                        break;
                    case "Demographic":
                        if (request.DateInterval == "day")
                        {
                            parameters.Add(new KeyValuePair<string, string>("json.facet", demofacet));
                        }
                        else if (request.DateInterval == "hour")
                        {

                        }
                        break;
                }

                //if (!request.SubFacet)
                //{
                //    parameters.Add(new KeyValuePair<string, string>("json.facet", "{buckets:{terms:{field:market,limit:250,sort:{numberofhits:desc},facet:{numberofhits:'sum(numberofhits)',mediavalue:'sum(mediavalue)',audience:'sum(audience)',positivesentiment:'sum(positivesentiment)',negativesentiment:'sum(negativesentiment)',dmaid:'max(dmaid)',heardearnedhits:'sum(heardearnedhits)',heardpaidhits:'sum(heardpaidhits)',seenpaidhits:'sum(seenpaidhits)',seenearnedhits:'sum(seenearnedhits)'}}}}"));
                //}
                //else
                //{
                //    parameters.Add(new KeyValuePair<string, string>("json.facet", "{buckets:{terms:{field:market,limit:250,sort:{numberofhits:desc},facet:{numberofhits:'sum(numberofhits)',facet2:{terms:{field:mediadate,sort:{index:asc},limit:24,facet:{numberofhits:'sum(numberofhits)'}}}}}}}"));
                //}
                CommonFunctions.LogDebug("Adding Execute search task");
                tasks.Add(Task.Factory.StartNew((object obj) => ExecuteSearch(parameters), "Results", TaskCreationOptions.AttachedToParent));

                try
                {
                    Task.WaitAll(tasks.ToArray(), 90000);
                }
                catch (Exception exc)
                {

                }

                string result = string.Empty;
                foreach (var tsk in tasks)
                {
                    result = ((Task<string>)tsk).Result;
                }

                response = ParseJSONResponse(result, request.Tab);

                sw.Stop();
                CommonFunctions.LogDebug(string.Format("AnalyticsSearchEngine.Search: {0} ms", sw.ElapsedMilliseconds));
                return response;
            }
            catch (Exception exc)
            {
                CommonFunctions.LogError(exc.ToString());
                return new List<FacetResponse>();
            }
        }

        private string ExecuteSearch(List<KeyValuePair<string, string>> vars)
        {
            try
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                string response = RESTClient.getReponse(Url.AbsoluteUri, vars);

                sw.Stop();
                CommonFunctions.LogDebug(string.Format("Executed Search in {0} ms", sw.ElapsedMilliseconds));
                return response;
            }
            catch (Exception exc)
            {
                CommonFunctions.LogError(exc.ToString());
                return string.Empty;
            }
        }

        private List<FacetResponse> ParseJSONResponse(string json, string tab)
        {
            try
            {
                CommonFunctions.LogDebug(string.Format("ParseJSONResponse for {0}", tab));
                //CommonFunctions.LogDebug(string.Format("Parse: {0}", json));
                Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
                Dictionary<string, object> facets = JsonConvert.DeserializeObject<Dictionary<string, object>>(response["facets"].ToString());
                Dictionary<string, object> buckets = JsonConvert.DeserializeObject<Dictionary<string, object>>(facets[tab.ToLower()].ToString());
                List<FacetResponse> facetResponses = new List<FacetResponse>();
                // If there is just one key then no facets are present
                if (buckets.Keys.Count > 0)
                {
                    var raw = buckets["buckets"];

                    switch (tab)
                    {
                        case "OverTime":
                            List<RawFacet> rawFacets = JsonConvert.DeserializeObject<List<RawFacet>>(raw.ToString());
                            CommonFunctions.LogDebug(string.Format("Deserialized Facets into {0} objects", rawFacets.Count));
                            foreach (var facet in rawFacets)
                            {
                                facetResponses.Add(new FacetResponse() {
                                    Count = facet.count

                                });
                            }
                            break;
                        case "Demographic":
                            List<RawDemoFacet> rawDemos = JsonConvert.DeserializeObject<List<RawDemoFacet>>(raw.ToString());
                            break;
                    }
                    //CommonFunctions.LogDebug(string.Format("facets2 dictionary has {0} keys", facets2.Keys.Count));
                    //string f2Keys = "";
                    //foreach (var k in facets2.Keys)
                    //{
                    //    f2Keys += " ," + k;
                    //}
                    //CommonFunctions.LogDebug(string.Format("facet2 keys consist of {0}", f2Keys));

                    //Dictionary<string, List<RawFacet>> dictParams = JsonConvert.DeserializeObject<Dictionary<string, List<RawFacet>>>(facets2["buckets"].ToString());
                    //List<RawFacet> rawMarkets = dictParams["buckets"];

                    //foreach (RawFacet mkt in rawMarkets)
                    //{
                    //    markets.Add(new FacetResponse() {
                    //        Market = mkt.val,
                    //        Count = mkt.count,
                    //        TotalHitCount = mkt.numberofhits,
                    //        Audience = mkt.audience,
                    //        NegativeSentiment = mkt.negativesentiment,
                    //        PositiveSentiment = mkt.positivesentiment,
                    //        SeenEarnedHits = mkt.seenearnedhits,
                    //        MarketID = mkt.dmaid,
                    //        SeenPaidHits = mkt.seenpaidhits,
                    //        HeardPaidHits = mkt.heardpaidhits,
                    //        MediaValue = mkt.mediavalue
                    //    });
                    //}
                }
                else
                {
                    CommonFunctions.LogInfo("No facets in request");
                }

                return facetResponses;
            }
            catch (Exception exc)
            {
                CommonFunctions.LogError(exc.ToString());
                return new List<FacetResponse>();
            }
        }
    }
}
