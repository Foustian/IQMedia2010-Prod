using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using IQMedia.Data;
using IQMedia.Logic.Base;
using IQMedia.Model;
using IQMedia.Web.Logic.Base;
using IQMedia.Shared.Utility;

namespace IQMedia.Web.Logic
{
    public class DataImportLogic : ILogic
    {
        public DataImportClientModel GetDataImportClient(Guid clientGuid)
        {
            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetDataImportClient(clientGuid);
        }

        #region Sony

        public List<SonySummaryModel> GetSonySummaryData(Guid clientGuid, DateTime fromDate, DateTime toDate, int dateIntervalType, List<string> searchRequestIDs, List<string> artists, List<string> albums, List<string> tracks,
                                                    bool hasTM, bool hasTV, bool hasNM, bool hasTW, bool hasPM, bool hasPQ, bool hasSM)
        {
            string searchRequestIDXml = null;
            if (searchRequestIDs != null && searchRequestIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in searchRequestIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                searchRequestIDXml = doc.ToString();
            }

            string filterXml = null;
            if (artists != null && artists.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in artists
                    select new XElement(
                        "item",
                        new XAttribute("artist", i),
                        new XAttribute("album", ""),
                        new XAttribute("track", "")
                    )
                ));
                filterXml = doc.ToString();
            }
            else if (albums != null && albums.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in albums
                    select new XElement(
                        "item",
                        new XAttribute("artist", ""),
                        new XAttribute("album", i),
                        new XAttribute("track", "")
                    )
                ));
                filterXml = doc.ToString();
            }
            else if (tracks != null && tracks.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in tracks
                    select new XElement(
                        "item",
                        new XAttribute("artist", ""),
                        new XAttribute("album", ""),
                        new XAttribute("track", i)
                    )
                ));
                filterXml = doc.ToString();
            }
            else
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    new XElement(
                        "item",
                        new XAttribute("artist", ""),
                        new XAttribute("album", ""),
                        new XAttribute("track", "")
                    )
                ));
                filterXml = doc.ToString();
            }

            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetSonySummaryData(clientGuid, fromDate, toDate, dateIntervalType, searchRequestIDXml, filterXml, hasTM, hasTV, hasNM, hasTW, hasPM, hasPQ, hasSM);
        }

        public List<SonyTableModel> GetSonyTableData(Guid clientGuid, DateTime fromDate, DateTime toDate, List<string> searchRequestIDs, int pageSize, int startIndex, string tableType, out int numTotalRecords)
        {
            string searchRequestIDXml = null;
            if (searchRequestIDs != null && searchRequestIDs.Count > 0)
            {
                XDocument doc = new XDocument(new XElement(
                    "list",
                    from i in searchRequestIDs
                    select new XElement(
                        "item",
                        new XAttribute("id", i)
                    )
                ));
                searchRequestIDXml = doc.ToString();
            }

            DataImportDA dataImportDA = (DataImportDA)DataAccessFactory.GetDataAccess(DataAccessType.DataImport);
            return dataImportDA.GetSonyTableData(clientGuid, fromDate, toDate, searchRequestIDXml, pageSize, startIndex, tableType, out numTotalRecords);
        }

        public SummaryReportMulti SonyLineChart(List<SonySummaryModel> listOfSummaryReportData, DateTime p_FromDate, DateTime p_ToDate, int dateIntervalType, int? chartWidth, bool p_Isv4TMAccess, Dictionary<long, string> p_SearchRequests,
            bool p_Isv4NMAccess, bool p_Isv4SMAccess, bool p_Isv4TWAccess, bool p_Isv4TVAccess, bool p_Isv4BLPMAccess, bool p_Isv4PQAccess, List<string> p_Artists, List<string> p_Albums, List<string> p_Tracks)
        {
            try
            {
                List<DateTime> dateRange = new List<DateTime>();
                if (dateIntervalType == 1) // Day
                {
                    TimeSpan ts = p_ToDate.Subtract(p_FromDate);
                    for (int i = 0; i <= ts.Days; i++)
                    {
                        dateRange.Add(p_FromDate.AddDays(i));
                    }
                }
                else // Month
                {
                    for (var dt = p_FromDate; dt <= p_ToDate; dt = dt.AddMonths(1))
                    {
                        dateRange.Add(dt);
                    }
                }

                SummaryReportMulti lstSummaryReportMulti = new SummaryReportMulti();


                List<string> categories = new List<string>();

                foreach (var date in dateRange)
                {
                    categories.Add(date.ToShortDateString());
                }

                #region Media Chart

                // Create a chart to display a single series for the aggregate of all submedia types if no search requests are selected.
                // If search requests are selected, this chart will display a series for each one.
                HighLineChartOutput highLineChartOutput = new HighLineChartOutput();
                highLineChartOutput.title = new Title() { text = "", x = -20 };
                highLineChartOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() }, 
                                                                new YAxis() { title = new Title2() { text = "ITunes", rotation = 90 }, opposite = true },
                                                                new YAxis() { title = new Title2() { text = "Spotify", rotation = 90 }, opposite = true }};


                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories, // all x-axis values 
                    labels = new labels()
                    {
                        formatter = dateIntervalType == 1 ? null : "GetMonth"
                    }
                };

                // show default tooltip format x / y values
                highLineChartOutput.tooltip = new Tooltip() { valueSuffix = "" };

                // show legend in center (as no other property is applied, it will take default values of it) , with borderWidth  = 0
                highLineChartOutput.legend = new Legend() { borderWidth = "0", width = 750 };

                // set chart with height = 300 px and width = 100 % (as not applied it will take default to 100%)
                highLineChartOutput.hChart = new HChart() { height = 300, type = "spline" };

                // start to set series of data for medium chart (or multi line search request chart)
                List<Series> lstSeries = new List<Series>();

                // if one or more search requests are selected, then create a series for each one
                // with total no. of records for that search request on a particular date
                if (p_SearchRequests != null && p_SearchRequests.Count > 0)
                {
                    // set plot options and click event for series points (which will again assigned in JS as this is string value)
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        spline = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set list of data for each series 
                    int colorIndex = 0;
                    foreach (var searchRequest in p_SearchRequests)
                    {
                        // Search Request Series
                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = searchRequest.Value;
                        series.color = highLineChartOutput.colors[colorIndex % highLineChartOutput.colors.Count];
                        series.yAxis = 0;

                        // ITunes Series
                        Series iTunesAgentSeries = new Series();
                        iTunesAgentSeries.dashStyle = "shortdash";
                        iTunesAgentSeries.data = new List<HighChartDatum>();
                        iTunesAgentSeries.name = searchRequest.Value + " (ITunes)";
                        iTunesAgentSeries.color = series.color;
                        iTunesAgentSeries.yAxis = 1;

                        // Spotify Series
                        Series spotifyAgentSeries = new Series();
                        spotifyAgentSeries.dashStyle = "shortdot";
                        spotifyAgentSeries.data = new List<HighChartDatum>();
                        spotifyAgentSeries.name = searchRequest.Value + " (Spotify)";
                        spotifyAgentSeries.color = series.color;
                        spotifyAgentSeries.yAxis = 2;

                        // loop for each date to create list of data for selected search request series. 
                        foreach (var item in dateRange)
                        {
                            List<SonySummaryModel> lstSummaries = listOfSummaryReportData.Where(smr => smr.SearchRequestID == searchRequest.Key
                                    && ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).ToList();

                            // Search Request Data
                            var daywiseSum = lstSummaries.Where(smr => smr.SeriesType == "SubMedia"
                                    && (
                                        (p_Isv4TMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Radio.ToString()) ||
                                        (p_Isv4NMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.NM.ToString()) ||
                                        (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.SocialMedia.ToString()) ||
                                        (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Forum.ToString()) ||
                                        (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Blog.ToString()) ||
                                        (p_Isv4TWAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.TW.ToString()) ||
                                        (p_Isv4TVAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.TV.ToString()) ||
                                        ((p_Isv4BLPMAccess || p_Isv4PQAccess) && smr.SubMediaType == CommonFunctions.DashBoardMediumType.PM.ToString()) // BLPM and ProQuest data is combined
                                      )
                                ).Sum(s => s.NoOfDocs);

                            // set data point of current series 
                            /*
                                *  y = y series value of current point === total no. of records for current search request at particular date 
                                *  SearchTerm = query name  , used in chart drill down click event
                                *  Value = Search Request ID  , used in chart drill down click event
                                *  Type = "Media" / "SubMedia" ,used in chart drill down click event 
                            */
                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = daywiseSum;
                            highChartDatum.SearchTerm = searchRequest.Value;
                            highChartDatum.Value = searchRequest.Key.ToString();
                            highChartDatum.Type = "Media";
                            series.data.Add(highChartDatum);

                            // ITunes Data
                            HighChartDatum iTunesAgentDatum = new HighChartDatum();
                            iTunesAgentDatum.y = lstSummaries.Where(smr => smr.SubMediaType == "ITunes").Sum(s => s.NoOfDocs);
                            iTunesAgentDatum.Type = "Client";
                            iTunesAgentSeries.data.Add(iTunesAgentDatum);

                            // Spotify Data
                            HighChartDatum spotifyAgentDatum = new HighChartDatum();
                            spotifyAgentDatum.y = lstSummaries.Where(smr => smr.SubMediaType == "Spotify").Sum(s => s.NoOfDocs);
                            spotifyAgentDatum.Type = "Client";
                            spotifyAgentSeries.data.Add(spotifyAgentDatum);
                        }

                        colorIndex++;
                        lstSeries.Add(series);
                        lstSeries.Add(iTunesAgentSeries);
                        lstSeries.Add(spotifyAgentSeries);
                    }
                }
                else
                {
                    // as its single media chart, we will show it as area chart, by setting chart type to "area"
                    highLineChartOutput.hChart.type = "areaspline";

                    // set plot options for area chart, for series click event, and plot marker.
                    highLineChartOutput.plotOption = new PlotOptions()
                    {
                        area = new PlotSeries()
                        {
                            marker = new PlotMarker()
                            {
                                enabled = true
                            }
                        },
                        series = new PlotSeries()
                        {
                            cursor = "pointer",
                            point = new PlotPoint()
                            {
                                events = new PlotEvents()
                                {
                                    click = "LineChartClick"
                                }
                            }
                        }
                    };

                    // set sereies name as "Media" , will shown in legent and tooltip.
                    Series series = new Series();
                    series.data = new List<HighChartDatum>();
                    series.name = "Media";

                    // loop for each date to create list of data for media series
                    foreach (var item in dateRange)
                    {
                        var sumOfDocs = listOfSummaryReportData.Where(smr => ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))
                            && (
                                    (p_Isv4TMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Radio.ToString()) ||
                                    (p_Isv4NMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.NM.ToString()) ||
                                    (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.SocialMedia.ToString()) ||
                                    (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Forum.ToString()) ||
                                    (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Blog.ToString()) ||
                                    (p_Isv4TWAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.TW.ToString()) ||
                                    (p_Isv4TVAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.TV.ToString()) ||
                                    ((p_Isv4BLPMAccess || p_Isv4PQAccess) && smr.SubMediaType == CommonFunctions.DashBoardMediumType.PM.ToString()) // BLPM and ProQuest data is combined
                                )
                            ).Sum(s => s.NoOfDocs);

                        // set data point of current series 
                        /*
                            *  y = y series value of current point === total no. of records for current search request at perticular date 
                            *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            *  we will not set SearchTerm and Value properies of data, as this is signle medium chart , without any search request
                        */
                        HighChartDatum highChartDatum = new HighChartDatum();
                        highChartDatum.y = sumOfDocs;
                        highChartDatum.Type = "Media";
                        series.data.Add(highChartDatum);
                    }

                    lstSeries.Add(series);
                }

                // assign set of series data to medium chart (or multi line searchrequest chart)
                highLineChartOutput.series = lstSeries;

                lstSummaryReportMulti.MediaRecords = CommonFunctions.SearializeJson(highLineChartOutput);

                #endregion

                #region SubMedia Chart

                // This chart will include a series for each submedia type, plus ITunes and Spotify
                HighLineChartOutput highLineChartSubMediaOutput = new HighLineChartOutput();
                highLineChartSubMediaOutput.title = new Title() { text = "", x = -20 };
                highLineChartSubMediaOutput.subtitle = new Subtitle() { text = "", x = -20 };

                highLineChartSubMediaOutput.yAxis = new List<YAxis>() { new YAxis() { title = new Title2() }, 
                                                                        new YAxis() { title = new Title2() { text = "ITunes", rotation = 90 }, opposite = true },
                                                                        new YAxis() { title = new Title2() { text = "Spotify", rotation = 90 }, opposite = true }};

                /* to show date lables on x-axis , vertically, will apply rotation on label = 270 */
                /* 
                    if number of x-axis values exceeds some limit, then in x-axis labels overlapped on each other and chart do not look proper, 
                    to resolved that, applied tickInterval so that maximum 45 lables will come in chart x-axis.  
                    tickInterval = 2, will skip show alternative labels in x-axis , form category values. 
                */
                /* tickmarkPlacement = off  to force chart to show last value of category in x-axis, otherwise it may show values as per tickinterval */
                highLineChartSubMediaOutput.xAxis = new XAxis()
                {
                    tickInterval = Convert.ToInt32(Math.Floor(Convert.ToDouble(categories.Count()) / 7)),
                    tickmarkPlacement = "off",
                    tickWidth = 2,
                    categories = categories,
                    labels = new labels()
                    {
                        formatter = dateIntervalType == 1 ? null : "GetMonth"
                    }
                };

                highLineChartSubMediaOutput.tooltip = new Tooltip() { valueSuffix = "" };
                highLineChartSubMediaOutput.legend = new Legend() { borderWidth = "0" };
                highLineChartSubMediaOutput.hChart = new HChart() { height = 300, width = chartWidth, type = "spline" };

                // set plot options and click event for series points (which will again assigned in JS as this is string value)
                highLineChartSubMediaOutput.plotOption = new PlotOptions()
                {
                    spline = new PlotSeries()
                    {
                        marker = new PlotMarker()
                        {
                            enabled = true
                        }
                    },
                    series = new PlotSeries()
                    {
                        cursor = "pointer",
                        point = new PlotPoint()
                        {
                            events = new PlotEvents()
                            {
                                click = "LineChartClick"
                            }
                        }
                    }
                };

                List<CommonFunctions.DashBoardMediumType> lstMediaCategories = Enum.GetValues(typeof(CommonFunctions.DashBoardMediumType)).Cast<CommonFunctions.DashBoardMediumType>().ToList();
                Int64 totNumOfHits = listOfSummaryReportData.Where(smr =>
                                    (p_Isv4TMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Radio.ToString()) ||
                                    (p_Isv4NMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.NM.ToString()) ||
                                    (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.SocialMedia.ToString()) ||
                                    (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Forum.ToString()) ||
                                    (p_Isv4SMAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.Blog.ToString()) ||
                                    (p_Isv4TWAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.TW.ToString()) ||
                                    (p_Isv4TVAccess && smr.SubMediaType == CommonFunctions.DashBoardMediumType.TV.ToString()) ||
                                    ((p_Isv4BLPMAccess || p_Isv4PQAccess) && smr.SubMediaType == CommonFunctions.DashBoardMediumType.PM.ToString()) // BLPM and ProQuest data is combined
                                ).Sum(s => s.NoOfDocs);
                lstSummaryReportMulti.TotalNumOfHits = totNumOfHits.ToString("N0");

                // start to set series of data for  multi line medium chart
                List<Series> lstSeriesSubMediaType = new List<Series>();

                // SubMedia Data
                foreach (var subMedia in lstMediaCategories)
                {
                    if (
                        (p_Isv4TMAccess && subMedia == CommonFunctions.DashBoardMediumType.Radio) ||
                        (p_Isv4NMAccess && subMedia == CommonFunctions.DashBoardMediumType.NM) ||
                        (p_Isv4SMAccess && subMedia == CommonFunctions.DashBoardMediumType.SocialMedia) ||
                        (p_Isv4SMAccess && subMedia == CommonFunctions.DashBoardMediumType.Forum) ||
                        (p_Isv4SMAccess && subMedia == CommonFunctions.DashBoardMediumType.Blog) ||
                        (p_Isv4TWAccess && subMedia == CommonFunctions.DashBoardMediumType.TW) ||
                        (p_Isv4TVAccess && subMedia == CommonFunctions.DashBoardMediumType.TV) ||
                        ((p_Isv4BLPMAccess || p_Isv4PQAccess) && subMedia == CommonFunctions.DashBoardMediumType.PM)) // BLPM and ProQuest data is combined
                    {
                        // set series name of multiline medium chart as medium description, will be shown in legend and tooltip.
                        string enumDesc = CommonFunctions.GetEnumDescription(subMedia);
                        Series series = new Series();
                        series.data = new List<HighChartDatum>();
                        series.name = enumDesc;
                        series.yAxis = 0;

                        // loop for each date to create list of data for selected medium type
                        foreach (var item in dateRange)
                        {
                            var daywiseSum = listOfSummaryReportData.Where(smr => String.Compare(smr.SubMediaType, subMedia.ToString(), true) == 0
                                    && ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).Sum(s => s.NoOfDocs);

                            // set data point of current series 
                            /*
                                *  y = y series value of current point === total no. of records for current medium type at perticular date 
                                *  SearchTerm = medium description  , used in chart drill down click event
                                *  Value = medium tpye  , used in chart drill down click event
                                *  Type = "Medua" / "SubMedia" ,used in chart drill down click event 
                            */
                            HighChartDatum highChartDatum = new HighChartDatum();
                            highChartDatum.y = daywiseSum;
                            highChartDatum.SearchTerm = enumDesc;
                            highChartDatum.Value = subMedia.ToString();
                            highChartDatum.Type = "SubMedia";
                            series.data.Add(highChartDatum);
                        }

                        lstSeriesSubMediaType.Add(series);
                    }
                }

                // ITunes/Spotify Data
                int seriesColorIndex = lstSeriesSubMediaType.Count;
                List<string> lstSelections = new List<string>();
                string selectionType = String.Empty;
                if (p_Artists != null && p_Artists.Count > 0)
                {
                    lstSelections = p_Artists;
                    selectionType = "Artist";
                }
                else if (p_Albums != null && p_Albums.Count > 0)
                {
                    lstSelections = p_Albums;
                    selectionType = "Album";
                }
                else if (p_Tracks != null && p_Tracks.Count > 0)
                {
                    lstSelections = p_Tracks;
                    selectionType = "Track";
                }

                if (!String.IsNullOrEmpty(selectionType))
                {
                    foreach (string selection in lstSelections)
                    {
                        Series iTunesSeries = new Series();
                        iTunesSeries.dashStyle = "shortdash";
                        iTunesSeries.data = new List<HighChartDatum>();
                        iTunesSeries.name = selection + " (ITunes)";
                        iTunesSeries.yAxis = 1;
                        iTunesSeries.color = highLineChartOutput.colors[seriesColorIndex % highLineChartOutput.colors.Count];
                        lstSeriesSubMediaType.Add(iTunesSeries);

                        Series spotifySeries = new Series();
                        spotifySeries.dashStyle = "shortdot";
                        spotifySeries.data = new List<HighChartDatum>();
                        spotifySeries.name = selection + " (Spotify)";
                        spotifySeries.yAxis = 2;
                        spotifySeries.color = iTunesSeries.color;
                        lstSeriesSubMediaType.Add(spotifySeries);

                        foreach (var item in dateRange)
                        {
                            List<SonySummaryModel> lstSummaries = listOfSummaryReportData.Where(smr => ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) || (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).ToList();
                            switch (selectionType)
                            {
                                case "Artist":
                                    lstSummaries = lstSummaries.Where(smr => String.Compare(smr.Artist, selection, true) == 0).ToList();
                                    break;
                                case "Album":
                                    lstSummaries = lstSummaries.Where(smr => String.Compare(smr.Album, selection, true) == 0).ToList();
                                    break;
                                case "Track":
                                    lstSummaries = lstSummaries.Where(smr => String.Compare(smr.Track, selection, true) == 0).ToList();
                                    break;
                            }

                            HighChartDatum iTunesDatum = new HighChartDatum();
                            iTunesDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "ITunes", true) == 0).Sum(s => s.NoOfDocs);
                            iTunesDatum.Type = "Client";
                            iTunesSeries.data.Add(iTunesDatum);

                            HighChartDatum spotifyDatum = new HighChartDatum();
                            spotifyDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "Spotify", true) == 0).Sum(s => s.NoOfDocs);
                            spotifyDatum.Type = "Client";
                            spotifySeries.data.Add(spotifyDatum);
                        }

                        seriesColorIndex++;
                    }
                }
                else
                {
                    Series iTunesSeries = new Series();
                    iTunesSeries.dashStyle = "shortdash";
                    iTunesSeries.data = new List<HighChartDatum>();
                    iTunesSeries.name = "ITunes";
                    iTunesSeries.yAxis = 1;
                    lstSeriesSubMediaType.Add(iTunesSeries);

                    Series spotifySeries = new Series();
                    spotifySeries.dashStyle = "shortdash";
                    spotifySeries.data = new List<HighChartDatum>();
                    spotifySeries.name = "Spotify";
                    spotifySeries.yAxis = 2;
                    lstSeriesSubMediaType.Add(spotifySeries);

                    foreach (var item in dateRange)
                    {
                        List<SonySummaryModel> lstSummaries = listOfSummaryReportData.Where(smr => ((dateIntervalType == 1 && smr.GMTDateTime.Equals(item)) ||
                                                                        (dateIntervalType == 3 && smr.GMTDateTime.Month.Equals(item.Month) && smr.GMTDateTime.Year.Equals(item.Year)))).ToList();

                        HighChartDatum iTunesDatum = new HighChartDatum();
                        iTunesDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "ITunes", true) == 0).Sum(s => s.NoOfDocs);
                        iTunesDatum.Type = "Client";
                        iTunesSeries.data.Add(iTunesDatum);

                        HighChartDatum spotifyDatum = new HighChartDatum();
                        spotifyDatum.y = lstSummaries.Where(smr => String.Compare(smr.SubMediaType, "Spotify", true) == 0).Sum(s => s.NoOfDocs);
                        spotifyDatum.Type = "Client";
                        spotifySeries.data.Add(spotifyDatum);
                    }
                }

                // assign set of series data to multi line medium type chart
                highLineChartSubMediaOutput.series = lstSeriesSubMediaType;

                lstSummaryReportMulti.SubMediaRecords = CommonFunctions.SearializeJson(highLineChartSubMediaOutput);

                #endregion

                return lstSummaryReportMulti;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }
}
