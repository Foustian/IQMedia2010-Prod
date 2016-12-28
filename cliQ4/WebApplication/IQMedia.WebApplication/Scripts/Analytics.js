/* Analytics ChartTypes */
// Bar
// Column
// Line
// Pie
// Daytime
// Growth
// Daypart
// US
// Canada

/* HCChartTypes */
// area
// arearange
// areaspline
// areasplinerange
// bar = used
// boxplot
// bubble
// column = used
// columnrange
// errorbar
// funnel
// gauge
// heatmap = used
// line
// pie = used
// polygon
// pyramid
// scatter
// solidgauge
// spline = used
// treemap
// waterfall
// fusionMap = used - not actual HCChart Type

/* Chart table */
// Chart ID | Analytics Type | HC Type
//----------+----------------+------------
//        1 |           Line | spline
//        2 |             US | fusionMaps
//        2 |         Canada | fusionMaps
//        4 |            Pie | pie
//        5 |            Bar | bar
//        6 |         Column | column
//        7 |        Daypart | heatmap
//        7 |        Daytime | heatmap
//        8 |         Growth | spline

/* 3 Server updates of */
// 1. Update main table, main table data change will require tss table and chart update (they are dependent on data of main table)
// Updated on: filter change (including dates), page change
// 2. Update tab specific secondary table (tss table), tss table data change will require chart update (chart dependent on tss table data)
// Updated on: when main table updated (any event that will cause this), tab change, selected agent change (OT tab will handle MST like this, make server request)
// 3. Update of chart, chart data change will not require any table change

/* URLs for AJAX requests */
var _urlGetMainTable = "/Analytics/GetMainTable/";
var _urlGetTabTable = "/Analytics/GetTabSpecificTable/";
var _urlGetNetworkShowTabTable = "/Analytics/GetNetworkShowTabTable/";
var _urlGetChart = "/Analytics/GetChart/";
var _urlGeneratePDF = "/Analytics/GeneratePDF/";
var _urlDownloadPDF = "/Analytics/DownloadPDF/";
var _urlGetOverlay = "/Analytics/GetOverlay/";
var _urlOpenIFrame = "/Analytics/OpenIFrame/";
var _urlSendEmail = "/Analytics/SendEmail/";
var _urlQuerySolr = "/Analytics/QuerySolr/";
var _urlGetActiveElements = "/Analytics/GetActiveElements/";

/* Page Info */
var _tab = "OverTime";
var _pageType = null;
var _prevTab = "OverTime";
var _IsExporting = false;
var _IsEmail = false;

//configuration 
var _ActiveElements = [];

//permissions
var _IsLRAccess = false;
var _IsAdsAccess = false;

// Filtering
var _isFilterActive = false;
var _subMediaType = "";
var _isCompare = false;

/* User selections */
var _selectedAgents = [];
var _dateInterval = null;
var _dateFrom = null;
var _dateTo = null;
var _ActivePESHTypes = [];
var _ActiveSourceGroups = [];
var _dateInterval = "day";
var _graphType = "Line";
var _ActiveOverlayType = null;
var _NumOverlaySeries = 0;
var _HCChartType = "spline";  // default to spline
var _AnalyticsChartType = "Line"; // default to line
var _isMap = false;
var _UserSetTimeSpan = '';
var _prevRequest = null;

/* Saved chart series */
var _TSSTSeries = [];

/* DEV */
var _DEVTable = [];
var _DEVJsonMaps = [];
var _DEVMaps = [];
var _DEVJsonChart = "";
var _demoSubTab = "gender"; // default to gender
var _DEVSeries = null;
var _DEVSolrResult = null;
var _HCColors = ["#598ea2", "#f3b350", "#c7d36a", "#b4b4da", "#d8635d", "#f3da72", "#9ad1dc", "#e1cba4", "#ff9bb8", "#808285", "#da3ab3", "#6ecdb2", "#e2cc00", "#ff6c36", "#3b5cad", "#9778d3", "#00bfd6", "#5b2c3f", "#4c8b2b", "#d9a460"];

//global selector
var _highChart = "";

// Only called on page load, which
$(document).ready(function () {
    _highChart = $(".divPrimaryChartSpecificClass");
    // Set thousands separator for HC
    Highcharts.setOptions({
        lang: {
            thousandsSep: ","
        }
    });

    _pageType = getParameterByName("type");

    // Make analytics main tab active
    $("#ulMainMenu li").removeClass("active");
    $("#liMenuAnalytics").addClass("active");

    // Initialize OverTime tab as active
    $("#ampOverTime").addClass("active");

    // Initialize QuickFilters and chart type
    $("#primaryNavAll").addClass("active");
    $("#primaryChartNav1").addClass("active");

    // Initialize date pickers for date range
    $("#dpDateFrom").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });
    $("#dpDateTo").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });

    // Hide TSST elements on page load
    $("#TSSTableTab").hide();
    $("#TSSTable").hide();

    // Add event listeners
    $("#filterLink").on("click", ToggleFilterDiv);

    $("#linkApplyFilters").on("click", SetFilter);
    $("#linkRemoveFilters").on("click", RemoveFilter);
    $("#linkAddAgentFilter0").on("click", ToggleFilterAgentCompare);

    // Need to add all individually so they can be removed later if needed
    $("#dateRange1days").on("click", DateRangeListClick);
    $("#dateRange7days").on("click", DateRangeListClick);
    $("#dateRange30days").on("click", DateRangeListClick);
    $("#dateRange90days").on("click", DateRangeListClick);
    $("#dateRange12months").on("click", DateRangeListClick);

    // Default date range selection
    $("#dateRange30days").addClass("active");
    $("#dateRangeList").data("previous", "dateRange30days");

    // Adds function to listen to click on date interval
    $("#dateIntervalList").children().click(function (e) {
        $("#dateIntervalList li").children().removeClass("active");
        $(this).children().addClass("active");
        ChangeDateInterval(e);
    });
    $("#dateIntervalDay").addClass("active");

    // Add on change listeners to filter dds and dps to change color of apply filter link
    $("#ddAgentFilter0").on("change", FilterCriteriaChange);
    $("#ddAgentFilter1").on("change", FilterCriteriaChange);
    $("#ddSourceFilter0").on("change", FilterCriteriaChange);
    $("#ddSourceFilter1").on("change", FilterCriteriaChange);

    // Bottom chart nave not applicable on page load
    $("#divBottomChartNav ul").hide();

    $("#ddSourceFilter0").on("change", ChangeSubMediaType);
    $("#ddSourceFilter1").attr("disabled", true);

    switch (_pageType)
    {
        case "amplification":
            SetDefaultDateRange();

            $("#linkCloseDateRange").on("click", CloseDateRange);
            $("#linkOpenDateRange").on("click", OpenDateRange);
            $("#linkApplyDateRange").on("click", ApplyDateRange);

            $("#dpDateFrom").on("change", ChangeDPValue);
            $("#dpDateTo").on("change", ChangeDPValue);
            $("#ampShows").show();
            $("#ampNetworks").show();
            break;
        case "campaign":
            $("#ampShows").hide();
            $("#ampNetworks").hide();
            break;
    }

    GetDateRange();

    // Initialize Main Table
    GetMainTable();
    $("#MSTableTab").addClass("active");

    $("#divSecondaryChart").hide();
});

/*-------------------------- AJAX --------------------------*/

// Updates both secondary tables as well as the chart
function GetMainTable() {
    //console.log("GetMainTable()");
    console.time("GetMainTable");
    console.time("GetMain Ajax Call");
    var subTab;
    // Need to slice _selectedAgents so RequestIDs is own array and changes to _selectedAgents won't propagate to this array (held in _prevRequest)
    var gRequest = {
        Tab: _tab,
        DateInterval: _dateInterval,
        RequestIDs: _selectedAgents.slice(),
        PageType: _pageType,
        HCChartType: _HCChartType,
        ChartType: _AnalyticsChartType,
        DateFrom: _dateFrom,
        DateTo: _dateTo,
        PESHTypes: _ActivePESHTypes,
        SourceGroups: _ActiveSourceGroups,
        IsFilter: _isFilterActive,
        SubMediaType: (_tab == "Daypart" ? 'TV' : _subMediaType),
        IsCompareMode: _isCompare
    };

    if (_tab === "Demographic")
    {
        subTab = $("#demoShortNav > li.active").attr("eleVal");
        gRequest.SubTab = subTab;
    }

    var jsonPostData = {
        graphRequest: gRequest,
        isNewRequest: IsNewRequest(_prevRequest, gRequest),
        isNewSecondary: IsNewSecondary(_prevRequest, gRequest)
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetMainTable,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                console.timeEnd("GetMain Ajax Call");
                _prevRequest = gRequest;

                _IsLRAccess = result.isLRAccess;
                _IsAdsAccess = result.isAdsAccess;

                $("#MSTable").html(result.MSTable);
                $("#MSTableTabLink").text(result.MSTableTab);
                _DEVJsonChart = result.chartJSON;
                ResetQuickFilterValues();
                $("#divPrimaryChart").html('');

                if (result.chartJSON !== null && result.chartJSON.length > 0)
                {
                    switch (_HCChartType)
                    {
                        case "spline":
                            RenderHighChartsLineChart(result.chartJSON);
                            break;
                        case "bar":
                        case "column":
                            RenderHighChartsColumnOrBarChart(result.chartJSON);
                            break;
                        case "pie":
                            RenderHighChartsPieChart(result.chartJSON);
                            break;
                        case "heatmap":
                            if (_tab == "Daytime")
                            {
                                RenderHighChartsDaytimeHeatMap(result.chartJSON);
                            }
                            else
                            {
                                RenderHighChartsDaypartHeatMap(result.chartJSON);
                            }
                            break;
                        case "fusionMap":
                            if (!_isCompare)
                            {
                                $("#divSecondaryChart").hide();
                                $("#divPrimaryChart").css("width", "90%");
                            }
                            else
                            {
                                $("#divPrimaryChart").css("width", "50%");
                                $("#divSecondaryChart").show();
                                $("#divSecondaryChart").css("width", "50%");
                            }

                            if (_AnalyticsChartType == "US")
                            {
                                RenderDmaMapChart(result.chartJSON);
                            }
                            else
                            {
                                RenderCanadaMapChart(result.chartJSON);
                            }
                            break;
                    }
                }
                else
                {
                    // If old chart not destroyed and trying to render no data message will still display chart
                    try
                    {
                        $("#divPrimaryChart").highcharts().destroy();
                    }
                    catch (err)
                    {
                        console.error(err);
                    }
                    $("#divPrimaryChart").html('<div class="chartNoData">No Data Available<br/><br/>Please Select At Least One Agent</div>');
                }

                // If HTML string of TSSTable is not empty - otherwise no TSST for this tab
                if (result.TSSTable.length > 0)
                {
                    _TSSTSeries = result.TSSTSeries;

                    $("#TSSTable").html(result.TSSTable);
                    $("#TSSTable").show();
                    $("#MSTable").hide();

                    $("#TSSTableTabLink").text(result.TSSTableTab);
                    $("#TSSTableTab").show();

                    $("#bottomShortNav li").removeClass("active");
                    $("#TSSTableTab").addClass("active");
                    if (result.hasMSTData)
                    {
                        SetStoredSeriesIDs();
                    }
                }
                else
                {
                    // If no TSST for tab then MST is main table
                    $("#MSTable").show();
                    $("#TSSTable").hide();
                    $("#TSSTableTab").hide();
                    $("#bottomShortNav li").removeClass("active");

                    $("#MSTableTab").addClass("active");
                }

                var selectorPrefix = _pageType == "campaign" ? "campaignCB_" : "agentCB_";

                _selectedAgents = [];
                $("[id^='" + selectorPrefix + "']:checked").each(function (i, el) {
                    var selectedID = Number(el.id.slice(el.id.indexOf('_') + 1));

                    _selectedAgents.push(selectedID);
                    // For each selected agent update Quick filters with these values
                    UpdatePESHFilters(selectedID, true);
                });

                if (_isMap)
                {
                    if (!_isCompare)
                    {
                        $("#divPrimaryChart").css("width", "90%");
                        $("#divSecondaryChart").hide();
                    }

                    SetColorsForMap();
                    // Destroy old HC charts if present - prevents HC chart from suddenly appearing on window resize when viewing map
                    for (var i = 0; i < Highcharts.charts.length; i++)
                    {
                        if (Highcharts.charts[i] !== undefined)
                        {
                            try
                            {
                                $("#divPrimaryChart").highcharts().destroy();
                            }
                            catch (err)
                            {
                                console.error(err);
                            }
                        }
                    }
                }
                else
                {
                    if (result.hasMSTData)
                    {
                        SetChartSeriesIDs();
                        FormatLegend();
                        SetColorsFromChart();
                    }
                }

                if (subTab !== undefined)
                {
                    // Show/hide elements based on sub tab choice
                    if (subTab == "age")
                    {
                        var demoLbls = $("[id^='demographicLbl_']");
                        demoLbls.css("display", "block");
                        demoLbls.filter("[id$='male']").css("display", "none");
                    }
                }

                // This has to come before the overlay is checked, since it may disable overlay functionality
                SetActiveElements();

                // If an overlay is active re-enable it
                var item = $("a[name='primaryOverlayNav'].active")[0];

                if (item !== undefined)
                {
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                    eval($(item).attr("href"));
                }
                else
                {
                    // Ensure these values are cleared, due to situations where the overlay may be removed without calling RemoveChartOverlay
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                }

                console.timeEnd("GetMainTable");
            }
            else
            {
                console.log("GetMainTable failed");
            }
        },
        error: function (a, b, c) {
            console.error("GetMainTable ajax error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}

// Updates the TSST and chart
function GetTabSpecificTable() {
    console.time("GetTabSpecificTable");
    console.time("GetTST Ajax Call");
    //console.log("GetTabSpecificTable()");
    // Need to slice _selectedAgents so RequestIDs is own array and changes to _selectedAgents won't propagate to this array (held in _prevRequest)
    var gRequest = {
        Tab: _tab,
        DateInterval: _dateInterval,
        RequestIDs: _selectedAgents.slice(),
        PageType: _pageType,
        HCChartType: _HCChartType,
        ChartType: _AnalyticsChartType,
        DateFrom: _dateFrom,
        DateTo: _dateTo,
        PESHTypes: _ActivePESHTypes,
        SourceGroups: _ActiveSourceGroups,
        IsFilter: _isFilterActive,
        SubMediaType: (_tab == "Daypart" ? 'TV' : _subMediaType),
        IsCompareMode: _isCompare
    };

    if (_tab === "Demographic")
    {
        gRequest.SubTab = $("#demoShortNav > li.active").attr("eleVal");
    }

    var jsonPostData = {
        graphRequest: gRequest,
        isNewSecondary: IsNewSecondary(_prevRequest, gRequest),
        isNewRequest: IsNewRequest(_prevRequest, gRequest)
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetTabTable,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                console.timeEnd("GetTST Ajax Call");

                _prevRequest = gRequest;
                _IsLRAccess = result.isLRAccess;
                _IsAdsAccess = result.isAdsAccess;
                _TSSTSeries = result.TSSTSeries;

                if (result.isSuccess && result.TSSTSeries !== null)
                {
                    SetStoredSeriesIDs();
                }

                $("#TSSTable").html(result.TSSTable);
                $("#TSSTableTabLink").text(result.TSSTableTab)

                if (result.chartJSON !== null && result.chartJSON.length > 0)
                {
                    switch (_HCChartType)
                    {
                        case "spline":
                            RenderHighChartsLineChart(result.chartJSON);
                            break;
                        case "bar":
                        case "column":
                            RenderHighChartsColumnOrBarChart(result.chartJSON);
                            break;
                        case "pie":
                            RenderHighChartsPieChart(result.chartJSON);
                            break;
                        case "heatmap":
                            if (_tab == "Daytime")
                            {
                                RenderHighChartsDaytimeHeatMap(result.chartJSON);
                            }
                            else
                            {
                                RenderHighChartsDaypartHeatMap(result.chartJSON);
                            }
                            break;
                        case "fusionMap":
                            if (_AnalyticsChartType == "US")
                            {
                                RenderDmaMapChart(result.chartJSON);
                            }
                            else
                            {
                                RenderCanadaMapChart(result.chartJSON);
                            }
                            break;
                    }
                }
                else
                {
                    // If old chart not destroyed and trying to render no data message will still display chart
                    try
                    {
                        $("#divPrimaryChart").highcharts().destroy();
                    }
                    catch (err)
                    {
                        console.error(err);
                    }
                    $("#divPrimaryChart").html('<div class="chartNoData">No Data Available<br/><br/>Please Select At Least One Agent</div>');
                }

                if (_isMap)
                {
                    SetColorsForMap();
                    // Destroy old HC charts if present - prevents HC chart from suddenly appearing on window resize when viewing map
                    for (var i = 0; i < Highcharts.charts.length; i++)
                    {
                        if (Highcharts.charts[i] !== undefined)
                        {
                            try
                            {
                                $("#divPrimaryChart").highcharts().destroy();
                            }
                            catch (err)
                            {
                                console.error(err);
                            }
                        }
                    }
                }
                else if (result.chartJSON !== null && result.chartJSON.length > 0)
                {
                    SetChartSeriesIDs();
                    FormatLegend();
                    SetColorsFromChart();
                }

                // This has to come before the overlay is checked, since it may disable overlay functionality
                SetActiveElements();

                // If an overlay is active re-enable it
                var item = $("a[name='primaryOverlayNav'].active")[0];

                if (item !== undefined)
                {
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                    eval($(item).attr("href"));
                }
                else
                {
                    // Ensure these values are cleared, due to situations where the overlay may be removed without calling RemoveChartOverlay
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                }

                console.timeEnd("GetTabSpecificTable");
            }
            else
            {
                console.log("GetTabSpecificTable failed");
            }
        },
        error: function (a, b, c) {
            console.error("GetTabSpecificTable ajax error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}

// Updates the TSST and chart
function GetNetworkShowSpecificTable() {
    console.time("GetNetworkShowTable");
    console.time("GetNetworkShowTable Ajax Call");
    // Need to slice _selectedAgents so RequestIDs is own array and changes to _selectedAgents won't propagate to this array (held in _prevRequest)
    var gRequest = {
        Tab: _tab,
        DateInterval: _dateInterval,
        RequestIDs: _selectedAgents.slice(),
        PageType: _pageType,
        HCChartType: _HCChartType,
        ChartType: _AnalyticsChartType,
        DateFrom: _dateFrom,
        DateTo: _dateTo,
        PESHTypes: _ActivePESHTypes,
        SourceGroups: _ActiveSourceGroups,
        IsFilter: _isFilterActive,
        SubMediaType: 'TV',
        IsCompareMode: _isCompare
    };

    var jsonPostData = {
        graphRequest: gRequest
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetNetworkShowTabTable,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                console.timeEnd("GetNetworkShowTable Ajax Call");

                _prevRequest = gRequest;
                _TSSTSeries = result.TSSTSeries;
                if (result.TSSTSeries !== null)
                {
                    SetStoredSeriesIDs();
                }

                $("#MSTable").html(result.MSTable);
                $("#MSTableTabLink").text(result.MSTableTab);
                $("#TSSTable").html(result.TSSTable);
                $("#TSSTableTabLink").text(result.TSSTableTab)

                if (result.chartJSON !== null && result.chartJSON.length > 0)
                {
                    switch (_HCChartType)
                    {
                        case "spline":
                            RenderHighChartsLineChart(result.chartJSON);
                            break;
                        case "bar":
                        case "column":
                            RenderHighChartsColumnOrBarChart(result.chartJSON);
                            break;
                        case "pie":
                            RenderHighChartsPieChart(result.chartJSON);
                            break;
                    }
                }
                else
                {
                    $("#divPrimaryChart").html('<div class="chartNoData">No Data Available<br/><br/>Please Select At Least One Agent</div>');
                }

                ResetQuickFilterValues();
                $.each(_selectedAgents, function (index, value) {
                    UpdatePESHFilters(value, true);
                });

                $("#TSSTable").show();
                $("#MSTable").hide();

                $("#TSSTableTab").show();
                $("#bottomShortNav li").removeClass("active");
                $("#TSSTableTab").addClass("active");

                if (result.chartJSON !== null && result.chartJSON.length > 0 && result.TSSTSeries !== null && result.TSSTSeries.length > 0)
                {
                    SetChartSeriesIDs();
                    SetColorsFromChart();
                    FormatLegend();
                }

                SetActiveElements();

                console.timeEnd("GetNetworkShowTable");
            }
            else
            {
                console.log("GetNetworkShowSpecificTable failed");
            }
        },
        error: function (a, b, c) {
            console.error("GetNetworkShowSpecificTable ajax error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}

// Only updates chart
function GetChart() {
    //    console.log("GetChart");
    console.time("GetChart");
    console.time("GetChart Ajax Call");
    var gRequest = {
        Tab: _tab,
        DateInterval: _dateInterval,
        RequestIDs: _selectedAgents.slice(),
        PageType: _pageType,
        HCChartType: _HCChartType,
        ChartType: _AnalyticsChartType,
        DateFrom: _dateFrom,
        DateTo: _dateTo,
        PESHTypes: _ActivePESHTypes,
        SourceGroups: _ActiveSourceGroups,
        IsFilter: _isFilterActive,
        SubMediaType: (_tab == "Daypart" ? 'TV' : _subMediaType),
        IsCompareMode: _isCompare
    };

    if (_tab === "Demographic")
    {
        gRequest.SubTab = $("#demoShortNav > li.active").attr("eleVal");
    }

    var jsonPostData = {
        graphRequest: gRequest
    };

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetChart,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                console.timeEnd("GetChart Ajax Call");
                _prevRequest = gRequest;

                _IsLRAccess = result.isLRAccess;
                _IsAdsAccess = result.isAdsAccess;

                _TSSTSeries = result.TSSTSeries;
                if (result.TSSTSeries !== null)
                {
                    SetStoredSeriesIDs();
                }
                _DEVJsonChart = result.chartJSON;

                if (result.chartJSON !== null && result.chartJSON.length > 0)
                {
                    switch (_HCChartType)
                    {
                        case "spline":
                            RenderHighChartsLineChart(result.chartJSON);
                            break;
                        case "bar":
                        case "column":
                            RenderHighChartsColumnOrBarChart(result.chartJSON);
                            break;
                        case "pie":
                            RenderHighChartsPieChart(result.chartJSON);
                            break;
                        case "heatmap":
                            if (_tab == "Daytime")
                            {
                                RenderHighChartsDaytimeHeatMap(result.chartJSON);
                            }
                            else
                            {
                                RenderHighChartsDaypartHeatMap(result.chartJSON);
                            }
                            break;
                        case "fusionMap":
                            if (_AnalyticsChartType == "US")
                            {
                                RenderDmaMapChart(result.chartJSON);
                            }
                            else
                            {
                                RenderCanadaMapChart(result.chartJSON);
                            }
                            break;
                    }
                }
                else
                {
                    // If old chart not destroyed and trying to render no data message will still display chart
                    try
                    {
                        $("#divPrimaryChart").highcharts().destroy();
                    }
                    catch (err)
                    {
                        console.error(err);
                    }
                    $("#divPrimaryChart").html('<div class="chartNoData">No Data Available<br/><br/>Please Select At Least One Agent</div>');
                }

                if (_isMap)
                {
                    SetColorsForMap();
                    // Destroy old HC charts if present - prevents HC chart from suddenly appearing on window resize when viewing map
                    for (var i = 0; i < Highcharts.charts.length; i++)
                    {
                        if (Highcharts.charts[i] !== undefined)
                        {
                            try
                            {
                                $("#divPrimaryChart").highcharts().destroy();
                            }
                            catch (err)
                            {
                                console.log(err);
                            }
                        }
                    }
                }

                if (_HCChartType !== "heatmap" && !_isMap && result.chartJSON !== null && result.chartJSON.length > 0)
                {
                    SetChartSeriesIDs();
                    if (_tab !== "OverTime" && (_HCChartType !== "heatmap" || _HCChartType !== "fusionMaps"))
                    {
                        // Only applies to non-OT tab
                        PerserveTSSTSeries();
                    }
                    FormatLegend();
                    SetColorsFromChart();
                }

                // This has to come before the overlay is checked, since it may disable overlay functionality
                SetActiveElements();

                // If an overlay is active re-enable it
                var item = $("a[name='primaryOverlayNav'].active")[0];

                if (item !== undefined && !_isMap)
                {
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                    eval($(item).attr("href"));
                }
                else
                {
                    // Ensure these values are cleared, due to situations where the overlay may be removed without calling RemoveChartOverlay
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                }

                console.timeEnd("GetChart");
            }
            else
            {
                console.log("GetChart failed");
            }
        },
        error: function (a, b, c) {
            console.error("GetChart ajax error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}

function GeneratePDFHelper() {
    $("#saveAsPDF").hide();
    $("#sendEmailLink").hide();

    if (!_isMap)
    {
        $("#divPrimaryChart").css("width", "950px");
        $("#divPrimaryChart").highcharts().reflow();
    }

    var jsonPostData = {
        HTML: $("#divContent").html(),
        agent0: $("#ddAgentFilter0").children().filter(":selected").text(),
        agent1: $("#ddAgentFilter1").children().filter(":selected").text(),
        source0: $("#ddSourceFilter0").children().filter(":selected").text(),
        source1: $("#ddSourceFilter1").children().filter(":selected").text()
    }

    _IsExporting = false;

    $("#saveAsPDF").show();
    $("#sendEmailLink").show();

    if (_isMap)
    {
        $("#divPrimaryChart").updateFusionCharts({
            width: "100%",
            height: 350
        });
    }
    else
    {
        $("#divPrimaryChart").css("width", "90%");
        $("#divPrimaryChart").highcharts().reflow();
    }

    $.ajax({
        url: _urlGeneratePDF,
        contentType: "application/json; charset=utf-8",
        type: "POST",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess)
            {
                window.location = _urlDownloadPDF;
            }
            else
            {
                console.log("Error downloading file");
            }
        },
        error: function (a, b, c) {
            console.error("GeneratePDFHelper ajax error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });
}

function ToggleOverlayData(overlayType) {
    //console.log("ToggleOverlayData(" + overlayType + ")");
    var chartMain = $("#divPrimaryChart").highcharts();
    // Need to slice _selectedAgents so RequestIDs is own array and changes to _selectedAgents won't propagate to this array (held in _prevRequest)
    if (_ActiveOverlayType !== overlayType)
    {
        var gRequest = {
            Tab: _tab,
            DateInterval: _dateInterval,
            RequestIDs: _selectedAgents.slice(),
            PageType: _pageType,
            HCCHartType: _HCChartType,
            ChartType: _AnalyticsChartType,
            DateFrom: _dateFrom,
            DateTo: _dateTo,
            PESHTypes: _ActivePESHTypes,
            SourceGroups: _ActiveSourceGroups,
            IsFilter: _isFilterActive,
            SubMediaType: _subMediaType,
            IsCompareMode: _isCompare
        };

        if (_tab === "Demographic")
        {
            gRequest.SubTab = $("#demoShortNav > li.active").attr("eleVal");
        }

        var jsonPostData = {
            graphRequest: gRequest,
            overlayType: overlayType
        };

        $.ajax({
            type: "POST",
            dataType: "json",
            url: _urlGetOverlay,
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess)
                {
                    _prevRequest = gRequest;
                    _IsLRAccess = result.isLRAccess;
                    _IsAdsAccess = result.isAdsAccess;
                    RemoveChartOverlay();

                    if (result.overlayData.length > 0)
                    {
                        var jsonChartData = JSON.parse(result.overlayData);
                        var yAxisName;
                        $("#divOverlayChart").highcharts(jsonChartData);

                        switch (overlayType)
                        {
                            case 1:
                                yAxisName = "Google Analytics";
                                break;
                            case 2:
                                yAxisName = "Audience";
                                break;
                            case 3:
                                yAxisName = "Ad Value";
                                break;
                        }

                        var overlayYAxis = {
                            id: "oppAxis",
                            title: {
                                text: yAxisName
                            },
                            minRange: 0.1,
                            gridLineWidth: 0,
                            opposite: true,
                            min: 0
                        };

                        chartMain.addAxis(overlayYAxis);

                        $.each($("#divOverlayChart").highcharts().series, function (i, s) {
                            // Get overlay data
                            var sName = s.name;
                            var sData = [];
                            var sColor = null;
                            var sID = null;

                            // For Audience and MediaValue, set overlay series color to same as main series
                            if (overlayType !== 1)
                            {
                                // Get the main series by matching 
                                var mainSeries = $.grep(chartMain.series, function (mainS, mainI) {
                                    // match on ID (Values)
                                    return mainS.data.length > 0 && s.data[0].Value == mainS.data[0].Value;
                                });

                                if (mainSeries.length === 1)
                                {
                                    sColor = mainSeries[0].color;
                                    sID = mainSeries[0].options.id + "_overlay";
                                }
                                else if (mainSeries.length === 0)
                                {
                                    sID = "_overlay";
                                }
                            }

                            // Reformat data point
                            $.each(s.data, function (iData, data) {
                                sData.push({
                                    y: data.y,
                                    category: data.category,
                                    Date: data.Date,
                                    Value: data.Value
                                });
                            });

                            var newSeries = {
                                name: sName,
                                data: sData,
                                yAxis: 'oppAxis',
                                color: sColor,
                                dashStyle: 'shortdot',
                                showInLegend: true,
                                id: sID
                            };

                            var hasCalendarDay = _tab === "OverTime" || _isCompare;

                            var campPointFormat = "Campaign Day: {point.x}<br />" + (hasCalendarDay ? "Calendar Day: {point.Date}<br />" : "");

                            if (overlayType === 3)  // Need to format ad value as money
                            {
                                newSeries.tooltip = {
                                    pointFormat: (_pageType === "campaign" ? campPointFormat : "") + "<span style=\"color:{point.color}\">\u25CF</span> {series.name}: <b>${point.y:,.2f}</b><br/>"
                                };
                            }
                            else if (overlayType === 2 && _AnalyticsChartType === "Growth") // Need to set formatting in case overlaying growth
                            {
                                newSeries.tooltip = {
                                    pointFormat: (_pageType === "campaign" ? campPointFormat : "") + "<span style=\"color:{point.color}\">\u25CF</span> {series.name}: <b>{point.y}</b><br/>"
                                };
                            }

                            _NumOverlaySeries += 1;
                            chartMain.addSeries(newSeries);
                        });

                        _ActiveOverlayType = overlayType;
                        $("#primaryOverlayNav" + overlayType).addClass("active");
                    }
                }
                else
                {
                    console.log("ToggleOverlayData failed");
                }
            },
            error: function (a, b, c) {
                console.error("ToggleOverlayData ajax error - " + c);
                ShowNotification(_msgErrorOccured);
            }
        });
    }
    else
    {
        RemoveChartOverlay();
    }
}

function OpenFeed() {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlOpenIFrame,
        contentType: "application/json; charset=utf-8",
        success: OpenFeedSuccess,
        error: function (a, b, c) {
            console.error("OpenFeed error - " + c);
            ShowNotification(_msgErrorOccured);
        }
    });

    var startDate = new Date(this.category);

    if (_pageType === "campaign")
    {
        startDate = new Date(this.Date);
    }

    var mediumQS = "";
    if (_subMediaType != null && _subMediaType != "")
    {
        mediumQS = '&medium=["' + _subMediaType + '"]&mediumDesc=["' + _subMediaType + '"]';
    }
    else
    {
        var subMediaTypes = [];
        var isAllTypes = true;

        if ($.inArray("OnAir", _ActiveSourceGroups) > -1)
        {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "OnAir";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if ($.inArray("Online", _ActiveSourceGroups) > -1)
        {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "Online";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if ($.inArray("Print", _ActiveSourceGroups) > -1)
        {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "Print";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if ($.inArray("Read", _ActivePESHTypes) > -1)
        {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "Print" || obj[0] == "Online";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if (!isAllTypes)
        {
            mediumQS = '&medium=["' + subMediaTypes.join('","') + '"]&mediumDesc=["' + subMediaTypes.join('","') + '"]';
        }
    }

    if ($.inArray("Heard", _ActivePESHTypes) > -1)
    {
        mediumQS += '&heard=true';
    }
    if ($.inArray("Seen", _ActivePESHTypes) > -1)
    {
        mediumQS += '&seen=true';
    }
    if ($.inArray("Paid", _ActivePESHTypes) > -1)
    {
        mediumQS += '&paid=true';
    }
    if ($.inArray("Earned", _ActivePESHTypes) > -1)
    {
        mediumQS += '&earned=true';
    }

    var searchDescs = $.map(_selectedAgents, function (a) {
        return '"' + $("#agentTD_" + a).text().split('+').join(' ') + '"';
    });

    var agents = $.map(_selectedAgents, function (a) {
        var agID = a;
//        if (agID === "6817")
//        {
//            console.log("Manual switch to ID 7893");
//            agID = 7893;
//        }
        return '"' + agID + '"';
    });

    if (_pageType === "campaign")
    {
        agents = $.map(_selectedAgents, function (a) {
            var agentTDid = $("#campaignTR_" + a).children("[id^='agent_']").attr("id");
            var agentID = agentTDid.slice(agentTDid.indexOf("_") + 1, agentTDid.length);
            return '"' + agentID + '"';
        });

        searchDescs = $.map(_selectedAgents, function (a) {
            var agentTD = $("#campaignTR_" + a);
            return '"' + agentTD.text().split('+').join(' ') + '"';
        });
    }

    var interval = "";
    var dateStr = "date=";
    // Only spline charts will have a single interval to drill into - all others are usually of the entire date range
    if (_HCChartType === "bar" || _HCChartType === "column")
    {
        if (_dateInterval === "month")
        {
            interval = "&isMonth=true";
        }

        GetDateRange();
        // Dates formatted as Year-Month-Day strings
        var start = _dateFrom.split("-");
        var end = _dateTo.split("-");

        dateStr = "fromDate=" + start[1] + "/" + start[2] + "/" + start[0] + "&toDate=" + end[1] + "/" + end[2] + "/" + end[0];
    }
    else
    {
        if (_dateInterval === "month")
        {
            var thisDate = this.Date.split("/");
            // DateFrom formatted as M/D/YYYY
            interval = "&isMonth=true";
            dateStr = "date=" + thisDate[0] + "/01/" + thisDate[2];
        }
        else if (_dateInterval === "hour")
        {
            // Safari will break, Invalid date
            if (_tab !== "Daytime")
            {
                dateStr = "date=" + startDate.toJSON();
            }
            interval = "&isHour=true";
        }
        else
        {
            dateStr = "date=" + (startDate.getMonth() + 1) + "/" + startDate.getDate() + "/" + startDate.getFullYear();
        }
    }

    if (_tab === "Sources")
    {
        $("#iFrameFeeds").attr("src",
            "//" + window.location.hostname + "/Feeds?" + dateStr + "&searchrequest=[" + agents + "]&searchrequestDesc=" +
            encodeURIComponent("[" + searchDescs + "]") + '&medium=["' + this.Value + '"]&mediumDesc=["' + this.Value + '"]&isDD=true' + interval
        );
    }
    else if (_tab === "Market")
    {
        $("#iFrameFeeds").attr("src",
            "//" + window.location.hostname + "/Feeds?" + dateStr + "&searchrequest=[" + agents + "]&searchrequestDesc=" +
            encodeURIComponent("[" + searchDescs + "]") + mediumQS + '&dma="' + this.SearchTerm + '"&isDD=true' + interval
        );
    }
    else if (_tab === "Shows")
    {
        mediumQS = '&medium=["TV"]&mediumDesc=["TV"]';
        $("#iFrameFeeds").attr("src",
            "//" + window.location.hostname + "/Feeds?" + dateStr + "&searchrequest=[" + agents + "]&searchrequestDesc=" +
            encodeURIComponent("[" + searchDescs + "]") + mediumQS + '&showTitle="' + this.SearchTerm + '"&isDD=true' + interval
        );
    }
    else if (_tab === "Daytime")
    {
        GetDateRange();
        // Dates formatted as Year-Month-Day strings
        var start = _dateFrom.split("-");
        var end = _dateTo.split("-");

        var dayOfWeek = this.y;

        // The dayOfWeek value from the chart is different from what is stored in solr, so the values need to be mapped
        if (dayOfWeek == 1) { dayOfWeek = 6; }
        else if (dayOfWeek == 2) { dayOfWeek = 5; }
        else if (dayOfWeek == 3) { dayOfWeek = 4; }
        else if (dayOfWeek == 4) { dayOfWeek = 3; }
        else if (dayOfWeek == 5) { dayOfWeek = 2; }
        else if (dayOfWeek == 6) { dayOfWeek = 1; }

        $("#iFrameFeeds").attr("src",
            "//" + window.location.hostname + "/Feeds?fromDate=" + start[1] + "/" + start[2] + "/" + start[0] +
            "&toDate=" + end[1] + "/" + end[2] + "/" + end[0] + "&searchrequest=[" + agents + "]&searchrequestDesc=" +
            encodeURIComponent("[" + searchDescs + "]") + mediumQS + "&timeOfDay=" + this.x + "&dayOfWeek=" + dayOfWeek + "&isDD=true" + interval
        );
    }
    else
    {
        var reqID = this.Value;
//        if (reqID === "6817")
//        {
//            // TODO - remove for QA
//            console.log("Manual switch ID to 7893");
//            reqID = 7893;
//        }
        if (_pageType === "campaign")
        {
            // Find agent ID for row
            var rawID = $("#campaignTR_" + this.Value).children("[id^='agent_']").attr("id");
            reqID = rawID.slice(rawID.indexOf("_") + 1, rawID.length);
        }

        $("#iFrameFeeds").attr("src",
            "//" + window.location.hostname + "/Feeds?" + dateStr + '&searchrequest=["' + reqID + '"]&searchrequestDesc=' + 
            encodeURIComponent('["' + this.SearchTerm.split('+').join(' ') + '"]') + mediumQS + '&isDD=true' + interval
        );
    }

    $("#divFeedsPage").css("height", documentHeight - 200);
    $("#iFrameFeeds").css("height", documentHeight - 200);
}

function SendEmail() {
    if (ValidateSendEmail())
    {
        $("#saveAsPDF").hide();
        $("#sendEmailLink").hide();

        if (!_isMap)
        {
            $("#divPrimaryChart").css("width", "950px");
            $("#divPrimaryChart").highcharts().reflow();
        }

        var jsonPostData = {
            HTML: $("#divContent").html(),
            agent0: $("#ddAgentFilter0").children().filter(":selected").text(),
            agent1: $("#ddAgentFilter1").children().filter(":selected").text(),
            source0: $("#ddSourceFilter0").children().filter(":selected").text(),
            source1: $("#ddSourceFilter1").children().filter(":selected").text(),
            fromEmail: $("#txtFromEmail").val(),
            toEmail: $("#txtToEmail").val(),
            bccEmail: $("#txtBCCEmail").val(),
            subject: $("#txtSubject").val(),
            userBody: $("#txtMessage").val()
        }

        _IsEmail = false;

        $("#saveAsPDF").show();
        $("#sendEmailLink").show();

        if (!_isMap)
        {
            $("#divPrimaryChart").css("width", "90%");
            $("#divPrimaryChart").highcharts().reflow();
        }
        else
        {
            $("#divPrimaryChart").updateFusionCharts({
                width: "100%",
                height: 350
            });
        }

        $.ajax({
            url: _urlSendEmail,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnEmailSendComplete,
            error: OnEmailSendFail
        });
    }
}

/*-------------------------- END AJAX --------------------------*/

/*-------------------------- MISC --------------------------*/

function DateRangeListClick(e) {
    $("#dateRangeList li").children().removeClass("active");
    $(this).addClass("active");
    ChangeTrailingDateRange(e);
}

// Called on dp change - used to auto change start/end dates to first/last day of month if on month interval
function ChangeDPValue() {
    $("#dateRangeList > li > a.active").removeClass("active");

    if (_dateInterval === "month")
    {
        var now = new Date(Date.now());
        var start = new Date($("#dpDateFrom").datepicker("getDate"));
        var end = new Date($("#dpDateTo").datepicker("getDate"));

        var newStart = new Date(start.getFullYear(), (start.getMonth()), 1);
        var newEnd = new Date(end.getFullYear(), (end.getMonth() + 1), 0);  // Date of 0 gets last date of month

        if (end.getMonth() === now.getMonth() && end.getFullYear() === now.getFullYear())
        {
            // End month is this month, set end date to today
            newEnd = new Date(end.getFullYear(), end.getMonth(), now.getDate());
        }

        $("#dpDateFrom").datepicker("setDate", newStart);
        $("#dpDateTo").datepicker("setDate", newEnd);
    }
}

function PerserveTSSTSeries() {
    var chart = $("#divPrimaryChart").highcharts();
    var series = _HCChartType === "pie" ? chart.series[0].data : chart.series;

    // Compare Mode for pie
    if (_isCompare && _HCChartType === "pie")
    {
        series = [];
        $.each(chart.series, function (i, e) {
            $.each(e.data, function (ind, ele) {
                series.push(ele);
            });
        });
    }

    // Hide visible series in chart if not checked
    $.each(series, function (i, s) {
        if (s.options !== null)
        {
            var id = s.options.id;
            if (_tab === "Daytime")
            {
                var split = id.split("_");
                id = split[0] + "_" + split[1];
            }
            else
            {
                id = id.split("_")[0];
            }

            if (s.options.id === "65+")
            {
                id = "65\\+";
            }
            var chk = $("#TSSTable input:checked[id$='_" + id + "']");

            if (chk.length === 0)
            {
                s.update({ visible: false });
            }
        }
    });

    var checkedBoxes = $("#TSSTable input:checked");
    if (_tab === "Demographic")
    {
        var subTab = $("#demoShortNav > li.active").attr("eleVal");
        if (subTab === "gender")
        {
            // Only keep male/female checkboxes
            checkedBoxes = checkedBoxes.filter("[id$='male']");
        }
        else
        {
            // Exclude male/female checkboxes
            checkedBoxes = checkedBoxes.not("[id$='male']");
        }
    }

    // Keep checked series in chart - do this last, adding series to chart will check all visible series in chart
    checkedBoxes.each(function (i, s) {
        var id = s.id;
        if (_tab === "Daytime")
        {
            var split = id.split("_");
            id = split[1] + "_" + split[2];
        }
        else
        {
            id = id.split("_")[1];
        }

        if (IsSeriesInChart(id) === null)
        {
            AddSeriesToChart(id);
        }
    });
}

// To be called when trying to toggle a TSST series that is not present in the chart
function AddSeriesToChart(id) {
    //console.log("AddSeriesToChart(" + id + ")");
    var chartMain = $("#divPrimaryChart").highcharts();

    if (id == "65+")
    {
        id = "65\\+";
    }
    var newSeries = [];
    var subSeries = [];
    var tsstSeries = FindTSSTSeries(id);

    if (_HCChartType === "pie")
    {
        for (var i = 0; i < tsstSeries.length; i++)
        {
            if (_isCompare)
            {
                var agentID = tsstSeries[i].id.split("_")[1];
                if (_tab === "Daytime")
                {
                    agentID = tsstSeries[i].id.split("_")[2];
                }

                for (var j = 0; j < chartMain.series.length; j++)
                {
                    var sID = chartMain.series[j].name;
                    if (agentID === sID)
                    {
                        chartMain.series[j].addPoint(tsstSeries[i]);
                    }
                }
            }
            else
            {
                chartMain.series[0].addPoint(tsstSeries[i]);
            }
        }
    }
    else
    {
        $.each(tsstSeries, function (i, s) {
            chartMain.addSeries(s);
        });
    }

    SetColorsFromChart();
    if (_HCChartType === "spline")
    {
        FormatLegend();
    }
}

function FindTSSTSeries(id) {
    var seriesList = [];

    _TSSTSeries.find(function (e, i) {
        var sID = e.id;
        if (_isCompare)
        {
            if (_tab === "Daytime")
            {
                var split = sID.split("_");
                sID = split[0] + "_" + split[1];
            }
            else
            {
                sID = sID.split("_")[0];
            }
        }
        if (sID === id)
        {
            seriesList.push(e);
        }
    });

    return seriesList;
}

// Copies id from id prop of data in each series into series itself
function SetStoredSeriesIDs() {
    //console.log("SetStoredSeriesIDs");
    var count = 0;
    var tempSeries = [];
    $.each(_TSSTSeries, function (i, s) {
        var seriesID = null;
        if (_HCChartType === "pie")
        {
            // If pie, need to convert "series" data from array of values to config object that HC will accept
            tempSeries.push({
                name: s[0],
                y: s[1],
                id: s[2]
            });
        }
        else if (_HCChartType === "heatmap")
        {
            // TODO - Remove TSSTSeries passing in on heatmap creation? Getting a different chart will get new types for appropriate chart type
        }
        else
        {
            if (s.data.length !== 0)
            {
                var seriesID = s.data[0].Value;
                if (seriesID == "65+")
                {
                    seriesID = "65\\+";
                }
                s.id = seriesID;
            }
        }
    });

    // Pie chart is a special case for structure of series/point data
    if (_HCChartType === "pie")
    {
        _TSSTSeries = tempSeries;
    }
}

// Switches between MST and TSST
function TableSwitch(table) {
    $("#bottomShortNav").children().removeAttr("class");
    if (table == "MST")
    {
        $("#TSSTable").hide();
        $("#MSTable").show();
        $("#MSTableTab").addClass("active");
    }
    else
    {
        $("#TSSTable").show();
        $("#MSTable").hide();
        $("#TSSTableTab").addClass("active");

        if (_tab == "Demographic")
        {
            var subTab = $("#demoShortNav > li.active").attr("eleVal");
            // Get and hide all demographic labels
            var demoLbls = $("[id^='demographicLbl_']");

            // Depending on sub tab, display applicable CBs
            if (subTab == "gender")
            {
                demoLbls.css("display", "none");
                demoLbls.filter("[id$='male']").css("display", "inline");
            }
            else
            {
                demoLbls.css("display", "block");
                demoLbls.filter("[id$='male']").css("display", "none");
            }
        }
    }
}

// Gets colors of visible series in chart and applies them to corresponding cbs
function SetColorsFromChart() {
    //console.log("setcolorsfromchart");
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series;
    var cbIDPrefix = _pageType == "amplification" ? "#agentCB_" : "#campaignCB_";

    if (_HCChartType == "pie")
    {
        series = [];
        for (var i = 0; i < chartMain.series.length; i++)
        {
            var pieSeries = chartMain.series[i];
            for (var j = 0; j < pieSeries.data.length; j++)
            {
                series.push(pieSeries.data[j]);
            }
        }
    }

    // Get an array of series that are visible in chart
    var visibleSeries = series.filter(function (s, i) {
        return s.visible == true;
    });

    switch (_tab)
    {
        case "Demographic":
            cbIDPrefix = "#demographicCB_";
            break;
        case "Sources":
            cbIDPrefix = "#sourceCB_";
            break;
        case "Market":
            cbIDPrefix = "#marketCB_";
            break;
        case "Networks":
            cbIDPrefix = "#networksCB_";
            break;
        case "Shows":
            cbIDPrefix = "#showsCB_";
            break;
        case "Daytime":
            // Do not include id selector for daytime or daypart since there can be multiple table rows for a single series
            cbIDPrefix = "daytimeCB_";
            lblPrefix = "daytimeLbl_";
            break;
        case "Daypart":
            // Do not include id selector for daytime or daypart since there can be multiple table rows for a single series
            cbIDPrefix = "daypartCB_";
            lblPrefix = "daypartLbl_";
            break;
    }

    if (_tab == "Daytime" || _tab == "Daypart")
    {
        if (_AnalyticsChartType == "Daytime" || _AnalyticsChartType == "Daypart")
        {
            $("[id^='" + cbIDPrefix + "'] + label").hide();
        }
        else
        {
            $("[id^='" + cbIDPrefix + "'] + label").show();
        }
    }


    // Color only CBs for visible series
    $.each(visibleSeries, function (i, s) {
        if (_tab == "Daytime" || _tab == "Daypart")
        {
            var seriesID = s.options.id;
            if (_isCompare && seriesID !== undefined)
            {
                var split = seriesID.split("_");
                if (_tab === "Daytime")
                {
                    seriesID = split[0] + "_" + split[1];
                }
                else
                {
                    seriesID = split[0];
                }

            }
            // Make sure visible series CBs are checked - or else would need to click twice to correctly remove/add
            $("#" + cbIDPrefix + seriesID).prop("checked", true);
            $("#" + cbIDPrefix + seriesID + " + label div span").css("background-color", s.color);
            $("#" + cbIDPrefix + seriesID + " + label div span").prop("title", "select to remove from graph");
        }
        else
        {
            var id = s.options.id;

            if (_isCompare || _tab === "Demographic")
            {
                id = s.options.id.split('_')[0];
            }

            if (id === "65+")
            {
                id = "65\\+";
            }
            // Make sure visible series CBs are checked - or else would need to click twice to correctly remove/add
            $(cbIDPrefix + id).prop("checked", true);
            //console.log("coloring series " + s.options.id + ": " + s.name + " the color " + s.color);
            $(cbIDPrefix + id + " + label div span").css("background-color", s.color);
            $(cbIDPrefix + id + " + label div span").prop("title", "select to remove from graph");
        }
    });

    // If tab is not overtime then color selected agents gray
    if (_tab !== "OverTime")
    {
        if (_pageType === "amplification")
        {
            $("[id^='agentCB_']:checked + label div span").css("background-color", "#6D6E71");
            $("[id^='agentCB_']:checked + label div span").prop("title", "select to remove from graph");
        }
        else
        {
            $("[id^='campaignCB_']:checked + label div span").css("background-color", "#6D6E71");
            $("[id^='campaignCB_']:checked + label div span").prop("title", "select to remove from graph");
        }
    }
}

function SetColorsForMap() {
    // Because maps contain no "series" like other chart types, need to color the table series gray based on if checked
    var cbIDPrefix = _pageType == "amplification" ? "agentCB_" : "campaignCB_";

    // Color all checked agents/campaigns gray
    $("[id^='" + cbIDPrefix + "']:checked + label div span").css("background-color", "#6D6E71");
    $("[id^='" + cbIDPrefix + "']:checked + label div span").prop("title", "select to remove from graph");

    // Color TSST series as well if they exist
    if (_tab !== "OverTime")
    {
        // While maps are not enabled on tabs other than OT, needed if wanted to enable in future
        switch (_tab)
        {
            case "Demographic":
                cbIDPrefix = "demographicCB_";
                break;
            case "Sources":
                cbIDPrefix = "sourceCB_";
                break;
            case "Market":
                cbIDPrefix = "marketCB_";
                break;
            case "Networks":
                cbIDPrefix = "networksCB_";
                break;
            case "Shows":
                cbIDPrefix = "showsCB_";
                break;
            case "Daytime":
                cbIDPrefix = "daytimeCB_";
                break;
            case "Daypart":
                cbIDPrefix = "daypartCB_";
                break;
        }

        $("[id^='" + cbIDPrefix + "']:checked + label div span").css("background-color", "#6D6E71");
        $("[id^='" + cbIDPrefix + "']:checked + label div span").prop("title", "select to remove from graph");
    }
}

// Called on applying filter - calls to get main table
function SetFilter() {
    _isFilterActive = true;

    // Set _selectedAgents to be from filter
    SetFilterRequest();

    // Update filter link appearance
    $("#filterLink").text("filters applied");
    $("#linkApplyFilters").css("color", "");

    if (_tab == "Networks" || _tab == "Shows") {
        GetNetworkShowSpecificTable();
    }
    else {
        GetMainTable();
    }
}

// Clears filter and reloads main table
function RemoveFilter() {
    ClearFilter();
    if (_tab == "Networks" || _tab == "Shows") {
        GetNetworkShowSpecificTable();
    }
    else {
        GetMainTable();
    }
}

// Only clears filter - removes search agents and filter visuals
function ClearFilter() {
    _isFilterActive = false;
    // Clear filter/search agents to reset to default
    _selectedAgents = [];
    _subMediaType = "";
    _isCompare = false;

    $("#linkApplyFilters").css("color", "");

    // Set dd filters to default and enable any disabled options
    $("#ddAgentFilter0 option").removeAttr("disabled");
    $("#ddAgentFilter0 option").removeClass("disabledOption");
    $("#ddAgentFilter1 option").removeAttr("disabled");
    $("#ddAgentFilter1 option").removeClass("disabledOption");
    $("#ddAgentFilter0").children().eq(0).prop("selected", true);
    $("#ddAgentFilter1").children().eq(0).prop("selected", true);
    $("#ddSourceFilter0").children().eq(0).prop("selected", true);
    $("#ddSourceFilter1").children().eq(0).prop("selected", true);

    // Hide secondary chart area
    $("#divPrimaryChart").css("width", "90%");
    $("#divSecondaryChart").hide();

    if (_pageType === "campaign")
    {
        $("#dpDateFromFilter0").val('');
        $("#dpDateToFilter0").val('');
        $("#dpDateFromFilter1").val('');
        $("#dpDateToFilter1").val('');

        if (!$("#divAgentFilter1").hasClass("hidden"))
        {
            ToggleFilterAgentCompare();
        }
        ToggleFilterDiv();
    }
    else
    {
        ToggleFilterDiv();
    }

    $("#filterLink").text("no filters applied");
}

// Called specifically to replace _selectedAgents with only those filtered by
function SetFilterRequest() {
    // Clear out old request agents
    _selectedAgents = [];

    // Add first agent to filter
    var filterID0 = $("#ddAgentFilter0").children().filter(":selected").val();
    var filterSource0 = $("#ddSourceFilter0").children().filter(":selected").val();

    if (filterID0 != "null")
    {
        _selectedAgents.push(Number(filterID0));
    }

    if (filterSource0 != "null")
    {
        _subMediaType = filterSource0;
    }
    else
    {
        _subMediaType = '';
    }

    if (!$("#divAgentFilter1").hasClass("hidden"))
    {
        var filterID1 = $("#ddAgentFilter1").children().filter(":selected").val();

        if (filterID1 != "null")
        {
            _selectedAgents.push(Number(filterID1));
        }
    }

    // To stop filter activating w/o any filter criteria
    if (_selectedAgents.length === 0 && _subMediaType === '')
    {
        _isFilterActive = false;
    }

    if (_selectedAgents.length > 1)
    {
        _isCompare = true;
    }
    else
    {
        _isCompare = false;
    }
}

function ChangeTab(SecondaryTabID) {
    var tabIDName = "#amp" + SecondaryTabID;
    _prevTab = _tab;
    _tab = SecondaryTabID;

    // if previously compare mode for a map: hide secondary chart area and set primary back to full size
    if (_isCompare && _isMap)
    {
        $("#divPrimaryChart").css("width", "90%");
        $("#divSecondaryChart").hide();
    }

    // Map isn't default chart type for any tab
    _isMap = false;

    // Remove any overlays
    RemoveChartOverlay();

    // Clear any active PESH filters - Removed to keep selected Quick Filter across tabs
//    _ActivePESHTypes = [];
//    _ActiveSourceGroups = [];
//    $("#primaryNavBar a").removeClass("active");
//    $("#primaryNavAll").addClass("active");
    //ResetQuickFilterValues();

    // Removes class from <a> children of <li>s on old tab
    $("#ampBar li").children().removeClass("active");
    // Attaches to active <a>s inside <li>s for new tab
    $(tabIDName).addClass("active");

    // De-activate previously selected chart and default back to line chart
    $("a[name='primaryChartNav']").removeClass("active");
    $("#primaryChartNav1").addClass("active");
    _HCChartType = "spline";
    _AnalyticsChartType = "Line";

    // Remove Demo short nav active tab
    $("#demoShortNav li").removeClass("active");

    // Hide shortNavs everywhere not applicable
    $("#demoShortNav").hide();
    $("#mapShortNav").hide();

    //Set default time span to day (this is overwritten for day/part and day/time)
    $("#dateIntervalLink").removeClass("disabled");
    $("#linkOpenDateRange").removeClass("disabled");

    if (_UserSetTimeSpan == '')
    {
        $("#dateIntervalLink").text("day");
        $("#dateIntervalList li").children().removeClass("active");
        $("#dateIntervalDay").addClass("active");
        _dateInterval = "day";
        SetRangesFromInterval(_dateInterval);
    }
    else
    {
        $("#dateIntervalLink").text(_UserSetTimeSpan);
        $("#dateIntervalList li").children().removeClass("active");
        if (_UserSetTimeSpan === 'hour')
        {
            $("#dateIntervalHour").addClass("active");
        }
        else if (_UserSetTimeSpan === 'day')
        {
            $("#dateIntervalDay").addClass("active");
        }
        else if (_UserSetTimeSpan === 'month')
        {
            $("#dateIntervalMonth").addClass("active");
            
        }
        _dateInterval = _UserSetTimeSpan;
        SetRangesFromInterval(_UserSetTimeSpan);
    }

    if (_pageType === "amplification")
    {
        if (_tab === "Networks" || _tab === "Shows" || _tab === "Daypart")
        {
            $("#ampFilterText").text("How many occurrences are there on TV?");
        }
        else
        {
            $("#ampFilterText").text("How many occurrences are there across all mediums and engagement types?");
        }
    }

    if (_tab == "Demographic")
    {
        $("#demoShortNav").show();
        $("#liGender").addClass("active");  // Default active sub-tab for demographic
    }
    else if (_tab == "Daytime" || _tab == "Daypart")
    {
        // Heatmap is default chart type for this type of tab, different from all other tabs
        $("a[name='primaryChartNav']").removeClass("active");
        $("#primaryChartNav7").addClass("active");

        _HCChartType = "heatmap";
        _AnalyticsChartType = _tab;

        // Ensure that dateInterval is set to hour, since these tabs don't apply to day or month data
        if (_dateInterval != "hour") {
            _dateInterval = "hour";
            $("#dateIntervalLink").addClass("disabled");
            $("#dateIntervalLink").text(_dateInterval);
            $("#dateIntervalList li").children().removeClass("active");
            $("#dateIntervalHour").addClass("active");

            SetRangesFromInterval(_dateInterval);
        }

        $("#dpDateFrom").datepicker("setDate", -7);
        $("#linkOpenDateRange").text("trailing 7 days");

        // Fix for safari not recognizing Date.now() as a valid date input string
        var now = new Date(Date.now());
        $("#dpDateTo").datepicker("setDate", now);

        $("#dateRangeList li").children().removeClass("active");
        $("#dateRangeList li").children(":contains(7 days)").addClass("active");
        GetDateRange();

        // Hide all labels - initially on heatmap so no series can be toggled
        if (_tab === "Daytime")
        {
            $("[id^='daytimeCB_'] + label").hide();
        }
        else
        {
            $("[id^='daypartCB_'] + label").hide();
        }
    }

    // If moving away from DT/DP will need to switch range back to month interval ranges
    if ((_prevTab === "Daytime" || _prevTab === "Daypart") && _dateInterval === "month")
    {
        ChangeDPValue();
        SpecifyDateRange();
    }

    if (_tab == "OverTime")
    {
        GetMainTable();
    }
    else {

        if (_tab == "Networks" || _tab == "Shows") {
            GetNetworkShowSpecificTable();
        }
        else if (_prevTab != "Networks" && _prevTab != "Shows" && _prevTab != "Daypart" && _tab != "Daypart" && _prevTab != "Daytime" && _tab != "Daytime") {
            GetTabSpecificTable();
        }
        else {
            // The Networks and Shows tabs hide the PESH columns of the main table, so when switching away from them, reload that table to add the columns back
            GetMainTable();
        }

        $("#TSSTable").show();
        $("#MSTable").hide();

        $("#TSSTableTab").show();
        $("#bottomShortNav li").removeClass("active");
        $("#TSSTableTab").addClass("active");
    }
}

// Called upon applying date range to get a new request
function ApplyDateRange() {
    // Is new range a trailing option?
    var range = $("#dateRangeList > li > a.active");
    if (range.length === 1)
    {
        $("#linkOpenDateRange").text("trailing " + range[0].innerHTML);
    }
    else
    {
        SpecifyDateRange();
    }

    $("#dateRangeDD").css("display", "none");
    GetDateRange();
    if (_tab == "Networks" || _tab == "Shows") {
        GetNetworkShowSpecificTable();
    }
    else {
        GetMainTable();
    }
}

function SwitchDemoSubTab(element) {
    $("#demoShortNav li").removeClass("active");
    $(element).addClass("active");

    var subTab = $("#demoShortNav > li.active").attr("eleVal");

    // Get and hide all demographic labels
    var demoLbls = $("[id^='demographicLbl_']");

    // Depending on sub tab, display applicable CBs
    if (subTab == "gender")
    {
        demoLbls.css("display", "none");
        demoLbls.filter("[id$='male']").css("display", "inline");
    }
    else
    {
        demoLbls.css("display", "block");
        demoLbls.filter("[id$='male']").css("display", "none");
    }

    GetChart();
}

function ChangeChartType(chartSelected) {
    // Change active type
    $("a[name='primaryChartNav']").removeClass("active");
    $("#primaryChartNav" + chartSelected).addClass("active");

    // Hide map short nav everywhere except for maps
    $("#mapShortNav").hide();
    $("#mapShortNav li").removeClass("active");
    var chartType = Number(chartSelected);

    switch (chartType)
    {
        case 1: // Multi-line
            _HCChartType = "spline";
            _AnalyticsChartType = "Line";
            _isMap = false;
            break;
        case 2: // US Map
            $("#mapShortNav").show();
            $("#liMap2").addClass("active");
            _HCChartType = "fusionMap";
            _AnalyticsChartType = "US";
            _isMap = true;
            break;
        case 3: // Canadian Map
            $("#mapShortNav").show();
            $("#primaryChartNav2").addClass("active");
            $("#liMap3").addClass("active");
            _HCChartType = "fusionMap";
            _AnalyticsChartType = "Canada";
            _isMap = true;
            break;
        case 4:
            _HCChartType = "pie";
            _AnalyticsChartType = "Pie";
            _isMap = false;
            break;
        case 5:
            _HCChartType = "bar";
            _AnalyticsChartType = "Bar";
            _isMap = false;
            break;
        case 6:
            _HCChartType = "column";
            _AnalyticsChartType = "Column";
            _isMap = false;
            break;
        case 7: // Daytime/Daypart
            _HCChartType = "heatmap";
            _AnalyticsChartType = _tab;
            _isMap = false;
            break;
        case 8: // Growth
            _HCChartType = "spline";
            _AnalyticsChartType = "Growth";
            _isMap = false;
            break;
    }

    if (_isMap && _isCompare)
    {
        $("#divPrimaryChart").css("width", "50%");
        $("#divSecondaryChart").css("width", "50%");
        $("#divSecondaryChart").show();
    }
    else
    {
        $("#divPrimaryChart").css("width", "90%");
        $("#divSecondaryChart").hide();
    }

    GetChart();
}

// Determines if new summaries are required for the current graph request
function IsNewRequest(previousRequest, currentRequest) {
    //console.log("IsNewRequest");
    if (previousRequest === null)
    {
        return true;
    }
    else if (previousRequest.PageType === currentRequest.PageType)
    {
        var sameFilter = previousRequest.IsFilter === currentRequest.IsFilter;
        var sameInterval = previousRequest.DateInterval === currentRequest.DateInterval;
        var sameSMT = previousRequest.SubMediaType === currentRequest.SubMediaType;
        var sameIDs = IsSameArray(previousRequest.RequestIDs, currentRequest.RequestIDs);
        var sameRange = (previousRequest.DateFrom === currentRequest.DateFrom) && (previousRequest.DateTo === currentRequest.DateTo);

        if (_pageType === "campaign")
        {
            if (sameFilter && sameInterval && sameSMT && sameIDs)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            if (sameFilter && sameInterval && sameSMT && sameIDs && sameRange)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
    else
    {
        return true;
    }
}

// Compares two arrays and returns true if they contain the same items or false if they do not
function IsSameArray(array1, array2) {
    //console.log("IsSameArray");
    // Method only intended to handle arrays with standard items (i.e. no objects or nested arrays)
    array1 = array1 === null ? [] : array1;
    array2 = array2 === null ? [] : array2;

    if (array1.length !== array2.length)
    {
        return false;
    }
    else
    {
        for (var i = 0; i < array1.length; i++)
        {
            if (array1[i] !== array2[i])
            {
                return false;
            }
        }

        return true;
    }
}

// Determines if new secondary tables are required for current graph request
function IsNewSecondary(previousRequest, currentRequest) {
    if (previousRequest === null)
    {
        return true;
    }
    else
    {
        var samePage = previousRequest.PageType === currentRequest.PageType;
        var sameTab = previousRequest.Tab === currentRequest.Tab;

        if (samePage && sameTab)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

/*-------------------------- END MISC --------------------------*/

/*-------------------------- OLD --------------------------*/

function FormatLegend() {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series;

    for (var i = 0; i < series.length; i++)
    {
        series[i].update({
            showInLegend: false
        });
    }
}

// Used to determine if a series is in the chart based on id  - returns series object or null
function IsSeriesInChart(seriesID) {
    if (seriesID === "65+")
    {
        seriesID = "65\\+";
    }
    //console.log("IsSeriesInChart(" + seriesID + ")");
    var chartSeries = $("#divPrimaryChart").highcharts().series;
    var series = [];
    var seriesList = [];

    if (_AnalyticsChartType == "Pie")
    {
        for (var i = 0; i < chartSeries.length; i++)
        {
            // For each data point (slice) in the pie chart series, push to array of all "series"
            $.each(chartSeries[i].data, function (i, d) {
                series.push(d);
            });
            //series.push(chartSeries[i].data);
        }
    }
    else if (_isMap)    // Maps
    {
        // Maps do not contain "series" 
        return null;
    }
    else
    {
        series = chartSeries;
    }

    for (var i = 0; i < series.length; i++)
    {
        // If series with seriesID is present in chart return series
        var sID = series[i].options.id;
        if (_isFilterActive && _selectedAgents.length > 1)
        {
            if (_tab === "Daytime")
            {
                var split = sID.split("_");
                sID = split[0] + "_" + split[1];
            }
            else
            {
                sID = sID.split('_')[0];
            }

            if (sID === "65+")
            {
                sID = "65\\+";
            }
        }

        if (seriesID === sID)
        {
            //console.log("series IS IN chart");
            seriesList.push(series[i]);
        }
    }
    //console.log("series NOT IN chart");
    // If series with id seriesID not encountered in chart return null
    if (seriesList.length > 0)
    {
        return seriesList;
    }
    else
    {
        return null;
    }
}

function IsTSSTSeriesChecked(seriesID) {
    var checks = $("#TSSTable input:checked").prop("id");
    var isChecked = checks.indexOf(seriesID);
    if (checks.indexOf(seriesID) !== -1)
    {
        return true;
    }
    else
    {
        return false
    }
}

function FilterDateCriteriaChange(e) {
    var target = e.target || e.srcElement;
    var newDate = new Date($("#" + target.id).datepicker("getDate"));
    var prevDate = new Date($("#" + target.id).data("prevDate"));

    if (prevDate.valueOf() != newDate.valueOf())
    {
        $("#linkApplyFilters").css("color", "red");
        $("#" + target.id).data("prevDate", newDate);
    }
}

// Used to change color of filter link to indicate change to filter
function FilterCriteriaChange() {
    $("#linkApplyFilters").css("color", "red");
}

// Toggle hidden class of filter div
function ToggleFilterDiv() {
    $("#divFilters").toggleClass("hidden");
}

// Will toggle visibility of second agent filtering criteria
function ToggleFilterAgentCompare() {
    $("#divAgentFilter1").toggleClass("hidden");
    var compareLinkTxt = $("#linkAddAgentFilter0").text();

    if (compareLinkTxt.indexOf("+") == 0)
        $("#linkAddAgentFilter0").text("- compare");
    else
        $("#linkAddAgentFilter0").text("+ compare");
}

function GeneratePDF() {
    _IsExporting = true;
    if (_isMap) {
        $("#divPrimaryChart").updateFusionCharts({
            width: 950,
            height: 350
        });
    }
    else {
        GeneratePDFHelper();
    }
}

function GenerateEmail() {
    _IsEmail = true;
    if (_isMap)
    {
        $("#divPrimaryChart").updateFusionCharts({
            width: 950,
            height: 350
        });
    }
    else
    {
        SendEmail();
    }
}

// Closes date range drop without applying range
function CloseDateRange() {
    //console.log("closing date range");
    $("#dateRangeDD").css("display", "none");

    // If canceling range selection revert to previous value
    $("#dateRangeList > li > a.active").removeClass("active");
    $("#" + $("#dateRangeList").data("previous")).addClass("active");

    $("#dpDateFrom").datepicker("setDate", $("#dpDateFrom").data("previous"));
    $("#dpDateTo").datepicker("setDate", $("#dpDateTo").data("previous"));
}

// Show the date range choices drop down
function OpenDateRange() {
    // If drop already open - close it
    if ($("#dateRangeDD").css("display") === "block")
    {
        CloseDateRange();
    }
    else
    {
        $("#dateRangeDD").css("display", "block");

        // Save previous values
        var trailing = $("#dateRangeList > li > a.active").prop("id");
        if (trailing !== undefined)
        {
            $("#dateRangeList").data("previous", trailing);
        }
        $("#dpDateFrom").data("previous", $("#dpDateFrom").datepicker("getDate"));
        $("#dpDateTo").data("previous", $("#dpDateTo").datepicker("getDate"));
    }
}

// Used only when user clicks a range under "trailing" - will update date picker values
function ChangeTrailingDateRange(e) {
    // Change text of link
    var range = e.target.innerHTML;

    // Trailing will always have an end date of today
    // Use new date as object to set date as - fix for safari not working with setting dp on Date.now() directly
    var now = new Date(Date.now());
    $("#dpDateTo").datepicker("setDate", now);

    // Set dateFrom based on range chosen
    switch (e.target.id)
    {
        // 12 months assumed to be 365 days
        case "dateRange12months":
            if (_dateInterval === "month")
            {
                $("#dpDateFrom").datepicker("setDate", new Date((now.getFullYear() - 1), now.getMonth(), 1));
            }
            else
            {
                $("#dpDateFrom").datepicker("setDate", -365);
            }
            break;
        case "dateRange90days":
            if (_dateInterval === "month")
            {
                $("#dpDateFrom").datepicker("setDate", new Date(now.getFullYear(), (now.getMonth() - 3), 1));
            }
            else
            {
                $("#dpDateFrom").datepicker("setDate", -90);
            }
            break;
        case "dateRange30days":
            if (_dateInterval === "month")
            {
                $("#dpDateFrom").datepicker("setDate", new Date(now.getFullYear(), (now.getMonth() - 1), 1));
            }
            else
            {
                $("#dpDateFrom").datepicker("setDate", -30);
            }
            break;
        case "dateRange7days":
            $("#dpDateFrom").datepicker("setDate", -7);
            break;
        case "dateRange1days":
            $("#dpDateFrom").datepicker("setDate", -1);
            break;
        // Default is trailing 30 days
        default:
            $("#dpDateFrom").datepicker("setDate", -30);
            break;
    }
}

// Called when changing the date interval from drop down list
function ChangeDateInterval(e) {
//    console.log("ChangeDateInterval");

    // Change text of link
    var interval = e.target.innerHTML;
    $("#dateIntervalLink").text(interval);

    _UserSetTimeSpan = interval;
    _dateInterval = interval;
    GetDateRange();
    SetRangesFromInterval(interval);

    // Assumed dates are ISO formatted as YYYY-MM-DD
    var startSplit = _dateFrom.split("-");
    var endSplit = _dateTo.split("-");

    // Set DP values to be correctly aligned with start/end of month
    if (interval === "month")
    {
        var newStart = startSplit[1] + "/01/" + startSplit[0];
        $("#dpDateFrom").datepicker("setDate", newStart);

        var newEnd = new Date(endSplit[0], endSplit[1], 0); // Setting date portion to 0 will set as last day in month
        var now = new Date(Date.now());

        if (newEnd.getMonth() === now.getMonth() && newEnd.getFullYear() === now.getFullYear())
        {
            newEnd = new Date(endSplit[0], now.getMonth(), now.getDate());
        }
        $("#dpDateTo").datepicker("setDate", newEnd);

        // Save as previous values
        $("#dpDateFrom").data("previous", $("#dpDateFrom").datepicker("getDate"));
        $("#dpDateTo").data("previous", $("#dpDateTo").datepicker("getDate"));
    }

    SpecifyDateRange();

    var end = new Date(endSplit[0], (endSplit[1] - 1), endSplit[2]);
    var now = new Date(Date.now());

    // Set correct trailing option - as well as open range drop down text
    if (interval === "month" && (end.getMonth() === now.getMonth()))
    {
        // Will set the active date range list from number of months in date range
        var months = MonthsBetween(_dateFrom, _dateTo);

        $("#dateRangeList > li > a.active").removeClass("active");

        if (months === 1)
        {
            $("#dateRange30days").addClass("active");
            $("#dateRangeList").data("previous", "dateRange30days");
        }
        else if (months === 3)
        {
            $("#dateRange90days").addClass("active");
            $("#dateRangeList").data("previous", "dateRange90days");
        }
        else if (months === 12)
        {
            $("#dateRange12months").addClass("active");
            $("#dateRangeList").data("previous", "dateRange12months");
        }
        else
        {
            // Not a matching trailing option then clear the previous range selection
            $("#dateRangeList").data("previous", "");
        }

        var activeTrailing = $("#dateRangeList > li > a.active");

        if (activeTrailing.length > 0)
        {
            $("#linkOpenDateRange").text("trailing " + activeTrailing[0].innerHTML);
        }
    }
    else if (interval === "day" && (end.getDate() === now.getDate()))
    {
        var days = DaysBetween(_dateFrom, _dateTo);
        console.log(days + " days between start and end date");
        if (days === 1)
        {
            $("#dateRange1days").addClass("active");
            $("#dateRangeList").data("previous", "dateRange1days");
        }
        else if (days === 7)
        {
            $("#dateRange7days").addClass("active");
            $("#dateRangeList").data("previous", "dateRangeList7days");
        }
        else if (days === 30)
        {
            $("#dateRange30days").addClass("active");
            $("#dateRangeList").data("previous", "dateRange30days");
        }
        else if (days === 90)
        {
            $("#dateRange90days").addClass("active");
            $("#dateRangeList").data("previous", "dateRange90days");
        }
        else if (days === 365)
        {
            $("#daetRange12months").addClass("active");
            $("#dateRangeList").data("previous", "dateRange12months");
        }
        else
        {
            // Not a matching trailing option then clear the previous range selection
            $("#dateRangeList").data("previous", "");
        }

        var activeTrailing = $("#dateRangeList > li > a.active");
        if (activeTrailing.length > 0)
        {
            $("#linkOpenDateRange").text("trailing " + activeTrailing[0].innerHTML);
        }
    }

    if (_tab == "Networks" || _tab == "Shows") {
        GetNetworkShowSpecificTable();
    }
    else {
        GetMainTable();
    }
}

function MonthsBetween(startDate, endDate) {
    // Dates assumed to be in YYYY-MM-DD format (same as globals)
    var startSplit = startDate.split("-");
    var endSplit = endDate.split("-");

    var start = new Date(startSplit[0], startSplit[1], 1); // Date portion discarded
    var end = new Date(endSplit[0], endSplit[1], 1);

    if (start.getFullYear() === end.getFullYear())
    {
        return end.getMonth() - start.getMonth();
    }
    else
    {
        var years = end.getFullYear() - start.getFullYear();
        return (years * 12) + (end.getMonth() - start.getMonth());
    }
}

function DaysBetween(startDate, endDate) {
//    console.log("DaysBetween(" + startDate + ", " + endDate + ")");
    // Dates assumed to be in YYYY-MM-DD format (same as globals)
    var msPerDay = 24 * 60 * 60 * 1000;
    var startSplit = startDate.split("-");
    var endSplit = endDate.split("-");
    // Need to split into components and create dates this way to avoid auto conversion to EST from what is assumed to be GMT
    var start = new Date(startSplit[0], startSplit[1] - 1, startSplit[2]);
    var end = new Date(endSplit[0], endSplit[1] - 1, endSplit[2]);

    start.setMinutes(start.getMinutes() - start.getTimezoneOffset());
    end.setMinutes(end.getMinutes() - end.getTimezoneOffset());
    return (end - start) / msPerDay;
}

function GetDaysInMonth(year, month) {
    // Months on 1 - 12 scale
    var days = new Date(year, month, 0);
    return days.getDate();
}

// Changes date range selections from chosen interval
function SetRangesFromInterval(interval) {
    switch (interval)
    {
        case "hour":
        case "day":
            $("#dateRange30days").text("30 days");
            $("#dateRange90days").text("90 days");
            $("#dateRange12months").text("365 days");

            // Show strictly day ranges - may have been hidden
            $("#dateRange1days").show();
            $("#dateRange7days").show();
            break;
        case "month":
            // Change trailing options to be month specific
            $("#dateRange30days").text("1 month");
            $("#dateRange90days").text("3 months");
            $("#dateRange12months").text("12 months");

            // Hide strictly day ranges
            $("#dateRange1days").hide();
            $("#dateRange7days").hide();
            break;
    }
}

function SetDefaultDateRange() {
    if (_pageType != "campaign")
    {
        $("#dpDateFrom").datepicker("setDate", -30);

        // Fix for Safari not recognizing Date.now() as a valid date input string for setting DP date
        var now = new Date(Date.now());
        $("#dpDateTo").datepicker("setDate", now);

        GetDateRange();

        $("#linkOpenDateRange").text("trailing 30 days");
    }
    else
    {
        $("#dpDateFromFilter0").val('');
        $("#dpDateFromFilter0").data("prevDate", '');
        $("#dpDateToFilter0").val('');
        $("#dpDateToFilter0").data("prevDate", '');
        $("#dpDateFromFilter1").val('');
        $("#dpDateFromFilter1").data("prevDate", '');
        $("#dpDateToFilter1").val('');
        $("#dpDateToFilter1").data("prevDate", '');
    }
}

// Updated date range global variables
function GetDateRange() {
    if (_pageType != "campaign")
    {
        var newStart = new Date($("#dpDateFrom").datepicker("getDate"));
        var newEnd = new Date($("#dpDateTo").datepicker("getDate"));
        _dateFrom = newStart.toISOString().substr(0, 10);
        _dateTo = newEnd.toISOString().substr(0, 10);
    }
    else
    {
        _dateFrom = null;
        _dateTo = null;
    }
}

// Function that maps series to rows
function SetChartSeriesIDs() {
    var chartMain = _highChart.highcharts();
    var series = chartMain.series;
    var secondaryTab = $("#bottomShortNav > li.active").text().trim();

    if (_AnalyticsChartType == "Pie")
    {
        for (var i = 0; i < series.length; i++)
        {
            $.each(series[i].data, function (i, d) {
                var matchedSeries = _TSSTSeries.find(function (e, i) {
                    // find series in array of series with the same name as data point
                    return e.name === d.name;
                });

                if (matchedSeries !== undefined)
                {
                    if (matchedSeries.id === "65+")
                    {
                        matchedSeries.id = "65\\+";
                    }

                    d.update({
                        id: matchedSeries.id
                    });
                }
                else
                {
                    //console.log("matchedSeries for " + d.name + " has failed");
                }
            });
        }
    }
    else
    {
        // If not a pie chart, simply update each series from first data point value
        for (var i = 0; i < series.length; i++) {
            var seriesID = series[i].data[0].Value;
            if (seriesID == "65+")
            {
                seriesID = "65\\+";
            }
            series[i].update({
                id: seriesID
            });
        }
    }
}

function FormatDaytimeTooltip() {
    return this.point.value;
}

function FormatDaypartTooltip() {
    if (this.point.y !== 2)
    {
        var item = "<b>" + this.point.name + "</b><br/><span>" + this.point.value + "</span>";
        return item;
    }
    else
    {
        return 0;
    }
}

function FormatDaypartDataLabel() {
    return this.point.code;
}

// Updates date range text "Results in the ..." from change to date range drop downs, and only from the date range drops
function SpecifyDateRange() {
    GetDateRange(); // Update globals

    // If date pickers are left empty default to prev 30 days
    if (_dateFrom == null || _dateFrom == "")
    {
        $("#dpDateFrom").datepicker("setDate", -30);
    }

    if (_dateTo == null || _dateTo == "")
    {
        // Fix for safari not recognizing Date.now() as a valid input string to set date as
        var now = new Date(Date.now());
        $("#dpDateTo").datepicker("setDate", now);
    }

    $("#dateRangeList > li > a.active").removeClass("active");

    GetDateRange(); // Update globals after altering datepicker values

    // Getting dates directly from dps since using _dateFrom and _dateTo, not having a time component, will assume GMT and convert it to EST (which will be wrong)
    var startDate = new Date($("#dpDateFrom").datepicker("getDate"));
    var endDate = new Date($("#dpDateTo").datepicker("getDate"));

    var startDateTxt = "";
    var endDateTxt = "";

    if (_dateInterval === "month")
    {
        startDateTxt = GetMonthName(startDate.getMonth()) + " - " + startDate.getFullYear();
        endDateTxt = GetMonthName(endDate.getMonth()) + " - " + endDate.getFullYear();
    }
    else
    {
        startDateTxt = (startDate.getMonth() + 1) + "/" + (startDate.getDate()) + "/" + startDate.getFullYear();
        endDateTxt = (endDate.getMonth() + 1) + "/" + (endDate.getDate()) + "/" + endDate.getFullYear();
    }

    $("#linkOpenDateRange").text("range " + startDateTxt + " to " + endDateTxt);
}

function GetMonthName(month) {
    switch (month)
    {
        case 0:
            return "Jan";
            break;
        case 1:
            return "Feb";
            break;
        case 2:
            return "Mar";
            break;
        case 3:
            return "Apr";
            break;
        case 4:
            return "May";
            break;
        case 5:
            return "Jun";
            break;
        case 6:
            return "Jul";
            break;
        case 7:
            return "Aug";
            break;
        case 8:
            return "Sep";
            break;
        case 9:
            return "Oct";
            break;
        case 10:
            return "Nov";
            break;
        case 11:
            return "Dec";
            break;
    }
}

function RemoveChartOverlay() {
    if (_ActiveOverlayType != null)
    {
        var chartMain = $("#divPrimaryChart").highcharts();

        // Remove the active overlay series and axis. They will always be the most recently added.
        for (var i = 0; i < _NumOverlaySeries; i++)
        {
            chartMain.series[chartMain.series.length - 1].remove();
        }
        chartMain.yAxis[1].remove();
    }

    $("a[name='primaryOverlayNav']").removeClass("active");
    _ActiveOverlayType = null;
    _NumOverlaySeries = 0;
}

function ClearPrimaryNavFilters() {
    // If all is clicked while already active, do nothing
    if (_ActivePESHTypes.length > 0 || _ActiveSourceGroups.length > 0)
    {
        _ActivePESHTypes = [];
        _ActiveSourceGroups = [];

        GetChart();
    }
}

// Will hide/show or series in chart or add/remove agent/campaign from requested IDs
function ToggleSeries(id) {
    //console.log("ToggleSeries(" + id + ")");
    if (id == "65+")
    {
        id = "65\\+";
    }
    var secondaryTab = $("#bottomShortNav > li.active").text().trim();
    var cbIDPrefix = _pageType == "campaign" ? "#campaignCB_" : "#agentCB_";
    var chartMain = $("#divPrimaryChart").highcharts();
    if (chartMain === undefined)
    {
        _selectedAgents.push(Number(id));
        if (_tab === "OverTime")
        {
            GetChart();
        }
        else
        {
            GetTabSpecificTable();
        }
        return;
    }
    var series = _isMap ? null : chartMain.series;
    var seriesColor;
    var checked;

    //MANUALLY UPDATE PESH FILTERS
    if (secondaryTab == "agent" || secondaryTab == "campaign")
    {
        checked = $(cbIDPrefix + id).prop("checked");
        UpdatePESHFilters(id, checked);
    }

    if (_isMap)
    {
        // If chart is map and trying to toggle series, toggle agent from _selectedAgents and reload chart
        checked = $(cbIDPrefix + id).prop("checked");
        // Toggle presence in _selectedAgents - possible that an agent is a series in the chart but not in _selectedAgents - will be removed if hiding from chart
        if (!checked)
        {
            _selectedAgents = _selectedAgents.filter(function (sa, i) {
                return sa !== Number(id);
            });
            //console.dir(_selectedAgents);
        }
        else
        {
            _selectedAgents.push(Number(id));
        }
        // Update color and title based on if checked
        var newColor = checked ? "#6D6E71" : "transparent";
        var newTitle = checked ? "select to remove from graph" : "select to add to graph";

        $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
        $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
        GetChart();
        return; // End function
    }

    if (_tab === "OverTime")
    {
        // Is agent checked
        checked = $(cbIDPrefix + id).prop("checked");
        //console.log("overtime " + (checked ? "is" : "is not") + " checked");

        // Determine if agent series is in chart
        var agentSeries = IsSeriesInChart(id);

        if (agentSeries !== null)  // Agent is in chart
        {
            // Toggle presence in _selectedAgents - possible that an agent is a series in the chart but not in _selectedAgents - will be removed if hiding from chart
            if (!checked)
            {
                _selectedAgents = _selectedAgents.filter(function (sa, i) {
                    return sa !== Number(id);
                });
                //console.dir(_selectedAgents);
            }
            else
            {
                _selectedAgents.push(Number(id));
            }

            if (_NumOverlaySeries > 0)
            {
                var overlaySeries = IsSeriesInChart(id + "_overlay");
                if (overlaySeries !== null)
                {
                    overlaySeries.update({
                        visible: checked,
                        showInLegend: checked
                    });
                }
            }

            // Toggle visibility in chart
            for (var i = 0; i < agentSeries.length; i++)
            {
                agentSeries[i].update({
                    visible: checked
                });
            }
            // Update color & title based on if checked
            var newColor = checked ? agentSeries[0].color : "transparent";
            var newTitle = checked ? "select to remove from graph" : "select to add to graph";

            $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
            $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
        }
        else  // Toggle agent is not present in chart
        {
            // Add agent to _searchAgents and get new chart & secondary
            _selectedAgents.push(Number(id));
            GetChart();
        }
    }
    else  // Series in chart do not represent agents
    {
        switch (secondaryTab)
        {
            case "agent":
                checked = $(cbIDPrefix + id).prop("checked");
                if (!checked)
                {
                    _selectedAgents = _selectedAgents.filter(function (sa, i) {
                        return sa !== Number(id);
                    });
                    $(cbIDPrefix + id + " + label div span").css("background-color", "transparent");
                    $(cbIDPrefix + id + " + label div span").prop("title", "select to add to graph");
                }
                else
                {
                    _selectedAgents.push(Number(id));
                }
                if (_tab == "Networks" || _tab == "Shows") {
                    GetNetworkShowSpecificTable();
                }
                else {
                    GetTabSpecificTable();
                }
                break;
            case "campaign":
                // Add remove campaign from search
                checked = $(cbIDPrefix + id).prop("checked");
                if (!checked)
                {
                    _selectedAgents = _selectedAgents.filter(function (sa, i) {
                        return sa !== Number(id);
                    });
                    $(cbIDPrefix + id + " + label div span").css("background-color", "transparent");
                    $(cbIDPrefix + id + " + label div span").prop("title", "select to add to graph");
                }
                else
                {
                    _selectedAgents.push(Number(id));
                }
                GetTabSpecificTable();
                break;
            case "demographic":
                cbIDPrefix = "#demographicCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id - demo series should always be present in chart - special case
                var ageSeries = IsSeriesInChart(id);
                //console.dir(ageSeries);
                for (var i = 0; i < ageSeries.length; i++)
                {
                    // Toggle visibility in chart
                    ageSeries[i].update({
                        visible: checked
                    });
                }

                // Update title and color based on if checked
                var newColor = checked ? ageSeries[0].color : "transparent";
                var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                break;
            case "daytime":
                cbIDPrefix = "#daytimeCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var daytimeSeries = IsSeriesInChart(id);

                // If daytimeSeries in chart
                if (daytimeSeries !== null)
                {
                    for (var i = 0; i < daytimeSeries.length; i++)
                    {
                        // Toggle visibility in chart
                        daytimeSeries[i].update({
                            visible: checked
                        });
                    }

                    // Update title and color based on if checked
                    var newColor = checked ? daytimeSeries[0].color : "transparent";
                    var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                    $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                    $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                }
                else
                {
                    // If daytime series not in chart then add it
                    AddSeriesToChart(id);
                }
                break;
            case "daypart":
                cbIDPrefix = "#daypartCB_";
                //var daypartID = id.slice(0, id.indexOf("_"));
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var daypartSeries = IsSeriesInChart(id);

                // DaypartSeries is in chart
                if (daypartSeries !== null)
                {
                    for (var i = 0; i < daypartSeries.length; i++)
                    {
                        // Toggle visibility in chart
                        daypartSeries[i].update({
                            visible: checked
                        });
                    }

                    // Update title and color based on if checked
                    var newColor = checked ? daypartSeries[0].color : "transparent";
                    var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                    $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                    $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                }
                else
                {
                    // DaypartSeries not in chart, add it
                    AddSeriesToChart(id);
                }
                break;
            case "sources":
                //console.log("sources toggle");
                cbIDPrefix = "#sourceCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var sourceSeries = IsSeriesInChart(id);
                //console.log("sourceSeries === null: " + (sourceSeries === null));
                // sourceSeries is in chart
                if (sourceSeries !== null)
                {
                    for (var i = 0; i < sourceSeries.length; i++)
                    {
                        // Toggle visibility in chart
                        sourceSeries[i].update({
                            visible: checked
                        });
                    }

                    // Update title and color based on if checked
                    var newColor = checked ? sourceSeries[0].color : "transparent";
                    var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                    $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                    $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                }
                else
                {
                    // sourceSeries not in chart, add it
                    AddSeriesToChart(id);
                }

                break;
            case "market":
                cbIDPrefix = "#marketCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var marketSeries = IsSeriesInChart(id);

                if (marketSeries !== null)
                {
                    for (var i = 0; i < marketSeries.length; i++)
                    {
                        // Toggle visibility in chart
                        marketSeries[i].update({
                            visible: checked
                        });
                    }

                    // Update title and color based on if checked
                    var newColor = checked ? marketSeries[0].color : "transparent";
                    var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                    $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                    $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                }
                else
                {
                    // marketSeries not in chart, add it
                    AddSeriesToChart(id);
                }

                break;
            case "networks":
                cbIDPrefix = "#networksCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var networksSeries = IsSeriesInChart(id);

                if (networksSeries !== null) {
                    for (var i = 0; i < networksSeries.length; i++)
                    {
                        // Toggle visibility in chart
                        networksSeries[i].update({
                            visible: checked
                        });
                    }

                    // Update title and color based on if checked
                    var newColor = checked ? networksSeries[0].color : "transparent";
                    var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                    $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                    $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                }
                else {
                    // marketSeries not in chart, add it
                    AddSeriesToChart(id);
                }

                break;
            case "shows":
                cbIDPrefix = "#showsCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var showsSeries = IsSeriesInChart(id);

                if (showsSeries !== null) {
                    for (var i = 0; i < showsSeries.length; i++)
                    {
                        // Toggle visibility in chart
                        showsSeries[i].update({
                            visible: checked
                        });
                    }

                    // Update title and color based on if checked
                    var newColor = checked ? showsSeries[0].color : "transparent";
                    var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                    $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                    $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                }
                else {
                    // marketSeries not in chart, add it
                    AddSeriesToChart(id);
                }

                break;
            default:
                break;
        }
    }

    // Needed for when just toggling visibility of agents
//    if (_NumOverlaySeries > 0 && IsSeriesInChart(id) !== null)
//    {
//        //console.log("Active overlay to reload");
//        // If an overlay is active, reload  it
//        var item = $("a[name='primaryOverlayNav'].active");
//        if (item != null)
//        {
//            RemoveChartOverlay();
//            eval($(item).attr("href"));
//        }
//    }
}

// Changes all quick fiter values to 0
function ResetQuickFilterValues() {
    $("#primaryNavAll_Results").text(0);
    $("#primaryNavOnAir_Results").text(0);
    $("#primaryNavOnline_Results").text(0);
    $("#primaryNavPrint_Results").text(0);
    $("#primaryNavSeen_Results").text(0);
    $("#primaryNavHeard_Results").text(0);
    $("#primaryNavRead_Results").text(0);
    $("#primaryNavPaid_Results").text(0);
    $("#primaryNavEarned_Results").text(0);
}

// Will change values in quick filters based on an agent/campaign and if it was checked or deselected (+ or -)
function UpdatePESHFilters(id, checked) {
    //console.log("UpdatePESHFilters(" + id + ", " + checked + ")");
    var oldNum = 0;
    var changeNum = 0;
    var newNum = 0;

    //OCCURRENCES
    oldNum = Number($("#primaryNavAll_Results").text().replace(/,/g, ''));
    changeNum = Number($("#OCCURRENCES_" + id).text().replace(/,/g, ''));
    newNum = 0;
    // checked parameter determines whether or not to increment or decrement QuickFilter values by series PESH values
    if (checked)
    {
        newNum = oldNum + changeNum;
    }
    else
    {
        newNum = oldNum - changeNum;
    }
    $("#primaryNavAll_Results").text(addCommas(newNum));

    if (_IsLRAccess)
    {
        //SEEN
        oldNum = Number($("#primaryNavSeen_Results").text().replace(/,/g, ''));
        changeNum = Number($("#SEEN_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked)
        {
            newNum = oldNum + changeNum;
        }
        else
        {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavSeen_Results").text(addCommas(newNum));

        //HEARD
        oldNum = Number($("#primaryNavHeard_Results").text().replace(/,/g, ''));
        changeNum = Number($("#HEARD_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked)
        {
            newNum = oldNum + changeNum;
        }
        else
        {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavHeard_Results").text(addCommas(newNum));

        //READ
        oldNum = Number($("#primaryNavRead_Results").text().replace(/,/g, ''));
        changeNum = Number($("#READ_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked)
        {
            newNum = oldNum + changeNum;
        }
        else
        {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavRead_Results").text(addCommas(newNum));
    }

    if (_IsAdsAccess)
    {
        //PAID
        oldNum = Number($("#primaryNavPaid_Results").text().replace(/,/g, ''));
        changeNum = Number($("#PAID_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked)
        {
            newNum = oldNum + changeNum;
        }
        else
        {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavPaid_Results").text(addCommas(newNum));

        //EARNED
        oldNum = Number($("#primaryNavEarned_Results").text().replace(/,/g, ''));
        changeNum = Number($("#EARNED_" + id).text().replace(/,/g, '')) + Number($("#READ_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked)
        {
            newNum = oldNum + changeNum;
        }
        else
        {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavEarned_Results").text(addCommas(newNum));
    }

    //ON AIR TIME
    if ($("#ONAIRTIME_" + id).attr('PESHvalue') !== undefined) {
        oldNum = Number($("#primaryNavOnAir_Results").text().replace(/,/g, ''));
        changeNum = Number($("#ONAIRTIME_" + id).attr('PESHvalue').replace(/,/g, ''));
        newNum = 0;
        if (checked) {
            newNum = oldNum + changeNum;
        }
        else {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavOnAir_Results").text(addCommas(newNum));
    }

    //ONLINE
    oldNum = Number($("#primaryNavOnline_Results").text().replace(/,/g, ''));
    changeNum = Number($("#OCCURRENCES_" + id).attr('OnlineCount').replace(/,/g, ''));
    newNum = 0;
    if (checked)
    {
        newNum = oldNum + changeNum;
    }
    else
    {
        newNum = oldNum - changeNum;
    }
    $("#primaryNavOnline_Results").text(addCommas(newNum));

    //PRINT
    oldNum = Number($("#primaryNavPrint_Results").text().replace(/,/g, ''));
    changeNum = Number($("#OCCURRENCES_" + id).attr('PrintCount').replace(/,/g, ''));
    newNum = 0;
    if (checked)
    {
        newNum = oldNum + changeNum;
    }
    else
    {
        newNum = oldNum - changeNum;
    }
    $("#primaryNavPrint_Results").text(addCommas(newNum));
}

function addCommas(str) {
    str = str.toString();
    if (str.toLowerCase() == 'nan' || str == '')
    {
        return 0;
    }
    else
    {
        var parts = (str + "").split("."),
        main = parts[0],
        len = main.length,
        output = "",
        first = main.charAt(0),
        i;

        if (first === '-')
        {
            main = main.slice(1);
            len = main.length;
        } else
        {
            first = "";
        }
        i = len - 1;
        while (i >= 0)
        {
            output = main.charAt(i) + output;
            if ((len - i) % 3 === 0 && i > 0)
            {
                output = "," + output;
            }
            --i;
        }
        // put sign back
        output = first + output;
        // put decimal part back
        if (parts.length > 1)
        {
            output += "." + parts[1];
        }
        return output;
    }
}

// Used to disable other filter agent selection when agent changed
function ChangeAgent(ddlAgent, ddlAgentOtherID) {
    $("#" + ddlAgentOtherID + " option").removeAttr("disabled");
    $("#" + ddlAgentOtherID + " option").removeClass("disabledOption");

    if ($(ddlAgent).val() !== "null")
    {
        var option = $("#" + ddlAgentOtherID + " option[value='" + $(ddlAgent).val() + "']");
        $(option).attr("disabled", "disabled");
        $(option).addClass("disabledOption");
    }
}

// Used to disable other filter campaign selection when changed
function ChangeCampaign(ddlCampaign, dpFromID, dpToID, ddlCampaignOtherID) {
    $("#" + ddlCampaignOtherID + " option").removeAttr("disabled");
    $("#" + ddlCampaignOtherID + " option").removeClass("disabledOption");

    if ($(ddlCampaign).val() == "null")
    {
        $("#" + dpFromID).val("");
        $("#" + dpToID).val("");
    }
    else
    {
        var selected = $(ddlCampaign).find(":selected");
        $("#" + dpFromID).val($(selected).attr("startdate"));
        $("#" + dpToID).val($(selected).attr("enddate"));

        var option = $("#" + ddlCampaignOtherID + " option[value='" + $(ddlCampaign).val() + "']");
        $(option).attr("disabled", "disabled");
        $(option).addClass("disabledOption");
    }
}

// Used to set all relevant sub media values when smt filter added/changed
function ChangeSubMediaType() {
    var selected = $("#ddSourceFilter0").children().filter(":selected");

    if (selected.val() == "null")
        _subMediaType = null;
    else
        _subMediaType = selected.val();

    // Set the second source filter to the value of the first
    $("#ddSourceFilter1").find(":contains('" + selected.text() + "')").prop("selected", true);
}

/*-------------------------- END OLD --------------------------*/

/*-------------------------- CHART RENDERING --------------------------*/

function RenderHighChartsDaypartHeatMap(jsonChartData) {
    console.time("Render Daypart Heat-Map");
    var jsonChart = JSON.parse(jsonChartData);
    jsonChart.tooltip.formatter = FormatDaypartTooltip;
    jsonChart.series[0].dataLabels.formatter = FormatDaypartDataLabel;
    $('#divPrimaryChart').highcharts(jsonChart);
    console.timeEnd("Render Daypart Heat-Map");
}

function RenderHighChartsPieChart(jsonChartData) {
    console.time("Render Pie Chart");
    var jsonPieChart = JSON.parse(jsonChartData);
    jsonPieChart.tooltip.formatter = FormatPieTooltip;

    jsonPieChart.chart.height = $("#divPrimaryChart").height();
    if (jsonPieChart.series.length > 1)
    {
        var primaryLength = jsonPieChart.series[0].data.length;
        // Manipulate pie chart colors - secondary series should loop through colors (each agent has the same color)
        var subColors = _HCColors.slice(primaryLength, primaryLength + _selectedAgents.length);
        jsonPieChart.series[1].colors = _HCColors;

        // Keep both centers matched
        jsonPieChart.series[0].center = ["50%", "50%", "50%", "50%"];
        jsonPieChart.series[1].center = ["50%", "50%", "50%", "50%"];
    }


    $("#divPrimaryChart").highcharts(jsonPieChart);
    // Call reflow to center pie chart in container (resizes svg to consume all container space) -- only type to not do this automatically
    $("#divPrimaryChart").highcharts().reflow();

    var chart = $("#divPrimaryChart").highcharts();
    if (chart.series.length > 1)
    {
        chart.series[0].update({ size: "60%" });
        chart.series[0].update({ innerSize: "50%" });

        chart.series[0].update({ allowPointSelect: false });
        chart.series[1].update({ allowPointSelect: false });
    }

    console.timeEnd("Render Pie Chart");
}

function RenderHighChartsColumnOrBarChart(jsonChartData) {
    console.time("Render Bar/Column Chart");
    var jsonChart = JSON.parse(jsonChartData);
    jsonChart.tooltip.formatter = FormatBarColumnTooltip;

//    if (_tab === "OverTime" || ((_tab === "Sources" || _tab === "Market" || _tab === "Shows") && _pageType !== "campaign"))
//    {
//        jsonChart.plotOptions.series.point.events.click = OpenFeed;
//    }

    $("#divPrimaryChart").highcharts(jsonChart);
    console.timeEnd("Render Bar/Column Chart");
}

function RenderHighChartsDaytimeHeatMap(jsonChartData) {
    console.time("Render Daytime Heat-Map");
    var jsonChart = JSON.parse(jsonChartData);
    jsonChart.tooltip.formatter = FormatDaytimeTooltip;
    if (_pageType !== "campaign")
    {
        jsonChart.plotOptions = { series: { point: { events: { click: OpenFeed}}} };
    }

    $('#divPrimaryChart').highcharts(jsonChart);
    console.timeEnd("Render Daytime Heat-Map");
}

function RenderHighChartsLineChart(jsonChartData) {
    console.time("Render Line Chart");

    var jsonLineChart = JSON.parse(jsonChartData);
    jsonLineChart.tooltip.formatter = FormatSplineTooltip;
    jsonLineChart.legend.floating = true;

    if (_tab === "OverTime" || ((_tab === "Sources" || _tab === "Market" || _tab === "Shows") && _pageType !== "campaign"))
    {
        // Demographic drilldown doesn't make sense
        // Campaign drilldown needs agent ID to work
        jsonLineChart.plotOptions.series.point.events.click = OpenFeed;
    }

    $("#divPrimaryChart").highcharts(jsonLineChart);
    console.timeEnd("Render Line Chart");
    //console.log("RenderHighChartsLineChart: " + (end - start) + " ms");
}

function RenderDmaMapChart(jsonMapData) {
    console.time("Render US Map");

    if (_isCompare)
    {
        $("#divPrimaryChart").css("width", "50%");
        $("#divSecondaryChart").css("width", "50%");
        $("#divSecondaryChart").show();
    }

    var chartRender = ["divPrimaryChart", "divSecondaryChart"];

    for (var i = 0; i < jsonMapData.length; i++)
    {
        _DEVJsonMaps.push(jsonMapData[i]);
        var populationMap = new FusionCharts({
            type: 'maps/usadma',
            renderAt: chartRender[i],
            width: '100%',
            height: '350',
            dataFormat: 'json',
            dataSource: jsonMapData[i]
        }).render();
        _DEVMaps.push(populationMap);
        //populationMap.addEventListener('entityClick',CheckUncheckDma);
        populationMap.addEventListener('entityRollOut', function (evt, data) {
            //SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
            hideToolTip();
        });
        populationMap.addEventListener('entityRollOver', function (evt, data) {
            //SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
            showToolTipOnChart("Market Area Name : " + data.label + "<br/>" + "Mention : " + data.value);
        });
        populationMap.addEventListener('drawComplete', function (evt, data) {
            if (_IsExporting)
            {
                setTimeout(function () {
                    GeneratePDFHelper();
                }, 500);
            }
            else if (_IsEmail)
            {
                setTimeout(function () {
                    SendEmail();
                }, 500);
            }
        });
        /*
        populationMap.addEventListener('chartRollOver',function(evt,data){
        SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
        });
        populationMap.addEventListener('chartRollOut',function(evt,data){
        SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
        });
        */
    }

    console.timeEnd("Render US Map");
}

function RenderCanadaMapChart(jsonMapData) {
    console.time("Render Canada Map");
    var chartRender = ["divPrimaryChart", "divSecondaryChart"];

    for (var i = 0; i < jsonMapData.length; i++)
    {
        var populationMap = new FusionCharts({
            type: 'maps/canada',
            renderAt: chartRender[i],
            width: '100%',
            height: '350',
            dataFormat: 'json',
            dataSource: jsonMapData[i]
        }).render();
        //populationMap.addEventListener('entityClick', CheckUncheckProvince);
        populationMap.addEventListener('entityRollOut', function (evt, data) {
            //SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
            hideToolTip();
        });
        populationMap.addEventListener('entityRollOver', function (evt, data) {
            //SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
            showToolTipOnChart("Province Name : " + data.label + "<br/>" + "Mention : " + data.value);
        });
        populationMap.addEventListener('drawComplete', function (evt, data) {
            if (_IsExporting)
            {
                setTimeout(function () {
                    GeneratePDFHelper();
                }, 500);
            }
            else if (_IsEmail)
            {
                setTimeout(function () {
                    SendEmail();
                }, 500);
            }
        });
        /*
        populationMap.addEventListener('chartRollOver', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
        });
        populationMap.addEventListener('chartRollOut', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
        });
        */
    }
    console.timeEnd("Render Canada Map");
}

/*-------------------------- END CHART RENDERING --------------------------*/

/*-------------------------- CHART FORMATTING --------------------------*/

function FormatLegend() {
    if (_HCChartType !== "pie")
    {
        var chartMain = $("#divPrimaryChart").highcharts();
        var series = chartMain.series;


        for (var i = 0; i < series.length; i++)
        {
            series[i].update({
                showInLegend: false
            });
        }
    }
}

function FormatDaytimeTooltip() {
    return this.point.value;
}

function FormatDaypartTooltip() {
    if (this.point.y !== 2)
    {
        var item = "<b>" + this.point.name + "</b><br/><span>" + this.point.value + "</span>";
        return item;
    }
    else
    {
        return 0;
    }
}

function FormatDaypartDataLabel() {
    return this.point.code;
}

function FormatBarColumnTooltip() {
    var tooltip = "<span style=\"color:" + this.series.color + "\">\u25CF<span> " + this.series.name + ": <b>" +
        addCommas(this.point.y) + "</b><br/>";
    return tooltip;
}

function FormatPieTooltip() {
    var innerIndex = (this.point.x - (this.point.x % 2)) / 2;
    var innerSlice = this.point.series.chart.series[0].points[innerIndex];

//    var tooltip = "<span style=\"color:" + this.point.color + "\">\u25CF<span> " + this.point.name + ": <br/>" +
//        "<b>" + addCommas(this.point.y) + "/" + addCommas(this.series.total) + " = " + addCommas(this.point.percentage.toFixed(2)) + "%</b> of total<br/>" +
//        "<b>" + addCommas(this.point.y) + "/" + addCommas(innerSlice.y) + " = " + addCommas((this.point.y / innerSlice.y * 100).toFixed(2)) + "%</b> of " + innerSlice.name;

    var tooltip = "<span style=\"color:" + this.point.color + "\">\u25CF<span> " + this.point.name + ": <br/>" +
        "<b>" + addCommas(this.point.y) + "/" + addCommas(this.series.total) + " = " + addCommas(this.point.percentage.toFixed(2)) + "%</b> of total";

    return tooltip;
}

function FormatSplineTooltip() {
    var hasCalendarDay = _tab === "OverTime" || _isCompare;
    var pointDate = this.point.Date;
    if (_dateInterval === "month")
    {
        pointDate = this.point.category;
    }
    var campaignHeader = "Campaign Day: " + this.point.x + (hasCalendarDay ? ("<br/>CalendarDay: " + pointDate + "<br/>") : "<br/>");
    var amplificationHeader = "<span style=\"font-size:10px\">" + pointDate + "</span><br/>";
    var commonContent = "<span style=\"color:" + this.series.color + "\">\u25CF<span> " + this.series.name + ": <b>";
    var count = addCommas(this.point.y) + "</b><br/>";

    // If formatting an ad value overlay tooltip
    if (this.series.options.id.search("_overlay") > -1 && _ActiveOverlayType === 3)
    {
        count = "$" + addCommas(this.point.y.toFixed(2)) + "</b><br/>";
    }

    // If formatting the tooltip of a growth non-overlay series
    if (_AnalyticsChartType === "Growth" && this.series.options.id.search("_overlay") === -1)
    {
        count = addCommas(this.point.y.toFixed(2)) + "%</b><br/>";
    }

    return (_pageType === "campaign" ? campaignHeader : amplificationHeader) + commonContent + count;
}

function FormatPieDataLabel() {
    var tt = "Name: " + this.point.name + "<br/>Y: " + this.y;

    return tt;
}

/*-------------------------- END CHART FORMATTING --------------------------*/

//************************************************************* CONFIGURATION  **********************************************************************
// Only need to call SetActiveElements()
function SetActiveElements() {
    if (_ActiveElements.length == 0) {
        $.ajax({
            type: "POST",
            dataType: "json",
            url: _urlGetActiveElements,
            contentType: "application/json; charset=utf-8",
            success: function (result) {
                _ActiveElements = result.activeElements;
                toggleEnabledAndHidden();
            },
            error: function () {
                console.log("GetActiveElements ajax error");
            }
        });
    }
    else {
        toggleEnabledAndHidden();
    }
}
function toggleEnabledAndHidden() {
    var updateChart = false;

    // Update PDF/Email icon visibility. Currently, maps don't display correctly when using compare mode.
    if (_isCompare && _isMap) {
        $("#sendEmailLink").hide();
        $("#saveAsPDF").hide();
    }
    else {
        $("#sendEmailLink").show();
        $("#saveAsPDF").show();
    }

    //set default enable and hidden properties
    if ($("#primaryChartNav1").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav1", false); }
    if ($("#primaryChartNav2").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav2", false); }
    if ($("#primaryChartNav4").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav4", false); }
    if ($("#primaryChartNav5").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav5", false); }
    if ($("#primaryChartNav6").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav6", false); }
    if ($("#primaryChartNav7").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav7", false); }
    if ($("#primaryChartNav8").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav8", false); }
    if ($("#primaryChartNav9").hasClass("inactive") == false) { ToggleIconStatus("primaryChartNav9", false); }
    if ($("#primaryChartNav1").hasClass("displayNoneImp")) { $("#primaryChartNav1").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav2").hasClass("displayNoneImp")) { $("#primaryChartNav2").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav4").hasClass("displayNoneImp")) { $("#primaryChartNav4").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav5").hasClass("displayNoneImp")) { $("#primaryChartNav5").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav6").hasClass("displayNoneImp")) { $("#primaryChartNav6").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav7").hasClass("displayNoneImp")) { $("#primaryChartNav7").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav8").hasClass("displayNoneImp")) { $("#primaryChartNav8").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav9").hasClass("displayNoneImp")) { $("#primaryChartNav9").removeClass("displayNoneImp"); }
    SetPESHFiltersEnabledDisabled(false);

    // Setting the on click function multiple times will cause the event to fire once for each time it was added.
    // To prevent this, always unbind the event beforehand.
    $("a[name='navPESHType']").off('click');
    $("a[name='navSourceGroup']").off('click');

    //update PESH hidden properties based on roles
    if (_IsAdsAccess) {
        $("#primaryNavPaid").show();
        $("#primaryNavEarned").show();
    }
    else {
        $("#primaryNavPaid").hide();
        $("#primaryNavEarned").hide();
    }
    if (_IsLRAccess) {
        $("#primaryNavSeen").show();
        $("#primaryNavHeard").show();
        $("#primaryNavRead").show();
    }
    else {
        $("#primaryNavSeen").hide();
        $("#primaryNavHeard").hide();
        $("#primaryNavRead").hide();
    }

    //update enable and hidden properties based on configuration table properties
    var isPESH = _ActivePESHTypes.length > 0 || _ActiveSourceGroups.length > 0;
    var isLineChart = $("#primaryChartNav1").hasClass("active") || $("#primaryChartNav8").hasClass("active");
    var isOtherChart = $("#primaryChartNav1").hasClass("active") == false && $("#primaryChartNav8").hasClass("active") == false;
    $.each(_ActiveElements, function (index, value) {
        if (value.ActivePage.toLowerCase() == _pageType.toLowerCase())
        {
            if (value.ElementSelector.indexOf("PESH") > -1) // If is PESH?
            {
                var activeTabs = value.ActiveTabs.indexOf(_tab) > -1;
                var pesh = !isPESH || (isPESH && value.IsActiveWithPESH);
                var map = !_isMap || (_isMap && value.IsActiveWithMaps);
                var lineChart = !isLineChart || (isLineChart && value.IsActiveWithLineCharts);
                var otherChart = !isOtherChart || (isOtherChart && value.IsActiveWithOtherCharts);

                if (activeTabs && pesh && map && lineChart && otherChart)
                {
                    $("#" + value.ElementSelectorID).css('cursor', 'pointer');

                    if ($("#" + value.ElementSelectorID).attr('name') == 'navPESHType') {
                        $("#" + value.ElementSelectorID).click(function () {
                            TogglePESHTypeFilter($(this).attr("eleVal"), true);
                        });
                    }
                    if ($("#" + value.ElementSelectorID).attr('name') == 'navSourceGroup') {
                        $("#" + value.ElementSelectorID).click(function () {
                            ToggleSourceGroupFilter($(this).attr("eleVal"), true);
                        });
                    }
                }
                else {
                    //keep filter disabled and remove from active if necessary
                    $("#" + value.ElementSelectorID).css('cursor', 'not-allowed');

                    if ($("#" + value.ElementSelectorID).attr('name') == 'navPESHType') {
                        if ($.inArray($("#" + value.ElementSelectorID).attr("eleVal"), _ActivePESHTypes) > -1) {
                            TogglePESHTypeFilter($("#" + value.ElementSelectorID).attr("eleVal"), false);
                        }
                    }
                    if ($("#" + value.ElementSelectorID).attr('name') == 'navSourceGroup') {
                        if ($.inArray($("#" + value.ElementSelectorID).attr("eleVal"), _ActiveSourceGroups) > -1) {
                            ToggleSourceGroupFilter($("#" + value.ElementSelectorID).attr("eleVal"), false);
                        }
                    }

                    if (value.HiddenTabs.indexOf(_tab) > -1)
                    {
                        $("#" + value.ElementSelectorID).hide();
                    }
                    else
                    {
                        $("#" + value.ElementSelectorID).show();
                    }
                }
            }
            else
            {
                var activeTabs = value.ActiveTabs.indexOf(_tab) > -1;
                var pesh = !isPESH || (isPESH && value.IsActiveWithPESH)
                var map = !_isMap || (_isMap && value.IsActiveWithMaps);
                var lineChart = !isLineChart || (isLineChart && value.IsActiveWithLineCharts);
                var otherChart = !isOtherChart || (isOtherChart && value.IsActiveWithOtherCharts);

                //Set Icon Enabled/Disabled and Hidden properties
                if (activeTabs && pesh && map && lineChart && otherChart)
                {
                    console.assert(activeTabs && pesh && map && lineChart && otherChart, "element is inactive");

                    if ($("#" + value.ElementSelectorID).hasClass("inactive"))
                    {
                        ToggleIconStatus(value.ElementSelectorID, true);
                    }
                }
                else {
                    ToggleIconStatus(value.ElementSelectorID, false);
                    $("#" + value.ElementSelectorID).removeClass("active");
                }

                if (value.HiddenTabs.indexOf(_tab) > -1)
                {
                    if ($("#" + value.ElementSelectorID).hasClass("displayNoneImp") == false)
                    {
                        $("#" + value.ElementSelectorID).addClass("displayNoneImp");
                    }

                }
                else
                {
                    if ($("#" + value.ElementSelectorID).hasClass("displayNoneImp"))
                    {
                        $("#" + value.ElementSelectorID).removeClass("displayNoneImp");
                    }
                }
            }
        }
    });

    $("a[name='navSourceGroup']").removeClass("active");
    $.each(_ActiveSourceGroups, function (index, obj) {
        $("#primaryNav" + obj).addClass("active");
    });

    $("a[name='navPESHType']").removeClass("active");
    $.each(_ActivePESHTypes, function (index, obj) {
        $("#primaryNav" + obj).addClass("active");
    });

    if (_ActivePESHTypes.length == 0 && _ActiveSourceGroups.length == 0) {
        $("#primaryNavAll").addClass("active");
    }
    else {
        $("#primaryNavAll").removeClass("active");
    }

    $("#primaryNavBar li a").trigger("blur");
}
function SetPESHFiltersEnabledDisabled(isEnabled) {
    // Setting the on click function multiple times will cause the event to fire once for each time it was added.
    // To prevent this, always unbind the event beforehand.
    $("a[name='navPESHType']").off('click');
    $("a[name='navSourceGroup']").off('click');

    if (isEnabled)
    {
        // Adds function to activate PESH filter buttons in primary nav bar
        $("a[name='navPESHType']").click(function () {
            TogglePESHTypeFilter($(this).attr("eleVal"), true);
        });
        $("a[name='navPESHType']").css('cursor', 'pointer');

        // Adds function to activate source group filter buttons in primary nav bar
        $("a[name='navSourceGroup']").click(function () {
            ToggleSourceGroupFilter($(this).attr("eleVal"), true);
        });
        $("a[name='navSourceGroup']").css('cursor', 'pointer');
    }
    else
    {
        $("a[name='navPESHType']").css('cursor', 'not-allowed');
        $("a[name='navSourceGroup']").css('cursor', 'not-allowed');
    }
}
function TogglePESHTypeFilter(eleVal, getChart) {
    var index = $.inArray(eleVal, _ActivePESHTypes);
    if (index == -1) {
        _ActivePESHTypes.push(eleVal);
    }
    else {
        _ActivePESHTypes.splice(index, 1);
    }

    if (getChart) {
        GetChart();
    }
}
function ToggleSourceGroupFilter(eleVal, getChart) {
    var index = $.inArray(eleVal, _ActiveSourceGroups);
    if (index == -1) {
        _ActiveSourceGroups.push(eleVal);
    }
    else {
        _ActiveSourceGroups.splice(index, 1);
    }

    if (getChart) {
        GetChart();
    }
}

function ToggleIconStatus(iconID, isEnabled) {
    if (isEnabled)
    {
        $("#" + iconID).css("cursor", "pointer");
        $("#" + iconID).prop('onclick', null).off('click');
        $("#" + iconID).removeClass("inactive");
    }
    else {
        $("#" + iconID).css("cursor", "not-allowed");
        $("#" + iconID).click(function (e) {
            e.preventDefault();
        });
        $("#" + iconID).addClass("inactive");
    }
}

//*********************************************************** END CONFIGURATION  ********************************************************************

/*-------------------------- FUSION MAPS ANCILLARY --------------------------*/

// Used to display tooltips for Fusion maps
var x, y, zInterval;
var Interval = 0;
document.onmousemove = setMouseCoords;

function setMouseCoords(e) {
    if (document.all)
    {
        tooltipx = window.event.clientX;
        tooltipy = window.event.clientY + 600;
    }
    else
    {
        tooltipx = e.pageX;
        tooltipy = e.pageY;
    }
}

function showToolTipOnChart(zText) {
    clearInterval(zInterval);
    zInterval = setTimeout("doShowToolTip('" + zText.trim() + "')", 0);
    Interval = 0;
}

function doShowToolTip(zText) {
    clearInterval(zInterval);
    document.getElementById("mapToolTip").style.top = (tooltipy + 10) + "px";
    document.getElementById("mapToolTip").style.left = tooltipx + "px";
    document.getElementById("mapToolTip").innerHTML = zText.trim();
    document.getElementById("mapToolTip").style.display = "block";
    zInterval = setTimeout("hideToolTip()", 500000);
}

function hideToolTipDiv() {
    zInterval = setTimeout("hideToolTip1()", 100000);
    Interval = 1000;
}

function hideToolTip() {
    zInterval = setTimeout("hideToolTip1()", 0);
    Interval = 0;
}

function hideToolTip1() {
    if (Interval != 1000)
    {
        document.getElementById("mapToolTip").style.display = "none";
        clearInterval(zInterval);
        Interval = 0;
    }
}
// End Fusion tooltips

/*-------------------------- END FUSION MAPS ANCILLARY --------------------------*/

/*-------------------------- FEEDS --------------------------*/

function OpenFeedSuccess(result) {
    $("#divFeedsPage").modal({
        backdrop: "static",
        keyboard: true,
        dynamic: true
    });

    $("#divFeedsPage").resizable({
        handles: "e,se,s,w",
        iframeFix: true,
        start: OpenFeedsStart,
        stop: OpenFeedsStop,
        resize: OpenFeedsResize
    }).draggable({
        iframeFix: true,
        start: OpenFeedsStart,
        stop: OpenFeedsStop
    });

    $("#divFeedsPage").css("position", "static");
}

function OpenFeedsStart() {
    var ifr = $("#iFrameFeeds");
    var d = $("<div></div>");

    $("#divFeedsPage").append(d[0]);
    d[0].id = "divTemp";
    d.css({
        position: "absolute",
        top: ifr.position().top,
        left: 0
    });
    d.height(ifr.height());
    d.width("100%");
}

function OpenFeedsStop() {
    $("#divTemp").remove();
}

function OpenFeedsResize(event, ui) {
    var newWidth = ui.size.width - 10;
    var newHeight = ui.size.height - 20;
    $("#iFrameFeeds").width(newWidth).height(newHeight);
}

function CloseIFramePopup() {
    $("#divFeedsPage").css("display", "none");
    $("#divFeedsPage").modal("hide");
    $("#iFrameFeeds").attr("src", "");
}

/*-------------------------- END FEEDS --------------------------*/

/*-------------------------- EMAIL --------------------------*/

function ShowEmailPopup() {
    $("#txtFromEmail").val($("#hdnDefaultSender").val());
    $("#txtToEmail").val("");
    $("#txtBCCEmail").val('');
    $("#txtSubject").val('');
    $("#txtMessage").val('');
    $('#divEmailPopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function CancelEmailpopup() {
    $("#divEmailPopup").css("display", "none");
    $("#divEmailPopup").modal("hide");
}

function ValidateSendEmail() {
    var isValid = true;

    $("#spanFromEmail").html("").hide();
    $("#spanToEmail").html("").hide();
    $("#spanBCCEmail").html("").hide();
    $("#spanSubject").html("").hide();
    $("#spanMessage").html("").hide();

    if ($("#txtFromEmail").val() == "")
    {
        $("#spanFromEmail").show().html(_msgFromEmailRequired);
        isValid = false;
    }
    if ($("#txtToEmail").val() == "")
    {
        $("#spanToEmail").show().html(_msgToEmailRequired);
        isValid = false;
    }

    if ($("#txtSubject").val() == "")
    {
        $("#spanSubject").show().html(_msgSubjectRequired);
        isValid = false;
    }
    if ($("#txtMessage").val() == "")
    {
        $("#spanMessage").show().html(_msgMessageRequired);
        isValid = false;
    }

    if ($("#txtFromEmail").val() != "" && !CheckEmailAddress($("#txtFromEmail").val()))
    {
        $("#spanFromEmail").show().html(_msgIncorrectEmail);
        isValid = false;
    }

    if ($("#txtToEmail").val() != "")
    {

        var toEmail = $("#txtToEmail").val();
        if (toEmail.substr(toEmail.length - 1) == ";")
        {
            toEmail = toEmail.slice(0, -1);
        }

        $(toEmail.split(';')).each(function (index, value) {
            if (!CheckEmailAddress(value))
            {
                $("#spanToEmail").show().html(_msgOneEmailAddressInCorrect);
                isValid = false;
                return;
            }
        });

        if (toEmail.split(';').length > _MaxEmailAdressAllowed)
        {
            $("#spanToEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
            isValid = false;
        }
    }

    if ($("#txtBCCEmail").val() != "")
    {
        if ($("#txtToEmail").val() == "")
        {
            $("#spanBCCEmail").show().html(_msgBCCEmailMissingTo);
            isValid = false;
        }
        else
        {
            var BCCemail = $("#txtBCCEmail").val();
            if (BCCemail.substr(BCCemail.length - 1) == ";")
            {
                BCCemail = BCCemail.slice(0, -1);
            }

            $(BCCemail.split(';')).each(function (index, value) {
                if (!CheckEmailAddress(value))
                {
                    $("#spanBCCEmail").show().html(_msgOneEmailAddressInCorrect);
                    isValid = false;
                    return;
                }
            });

            if (BCCemail.split(';').length > _MaxEmailAdressAllowed)
            {
                $("#spanBCCEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
                isValid = false;
            }
        }
    }

    return isValid;
}

function OnEmailSendComplete(result) {
    CancelEmailpopup();
    if (result.isSuccess)
    {
        ShowNotification(_msgEmailSent.replace(/@@emailSendCount@@/g, result.emailSendCount));

    }
    else
    {
        ShowNotification(result.errorMessage);
    }
}

function OnEmailSendFail(a, b, c) {
    CancelEmailpopup();
    ShowNotification(_msgErrorOccured);
    console.error("SendEmail ajax error - " + c);
}

/*-------------------------- END EMAIL --------------------------*/

// DEV
function InitializeTest() {
    // set requests of top 5 agents of past 90 days
    _selectedAgents = [202, 211, 6812, 652, 6813];
    _dateFrom = "2016-04-07";
    _dateTo = "2016-07-06";
}

function DEVChartSeries() {
    var chart = $("#divPrimaryChart").highcharts();
    var series = _HCChartType === "pie" ? chart.series[0].data : chart.series;
    console.groupCollapsed("chart has " + series.length + " series");
    for (var i = 0; i < series.length; i++)
    {
        var sColor = series[i].color;
        console.log("series " + series[i].name + " has id " + series[i].options.id + " and color " + sColor);
    }
    console.groupEnd();
}

function DEVPieTest() {
    _selectedAgents = [6817, 6816];
    _dateFrom = "2016-04-01";
    _dateTo = "2016-06-01";
    _isFilterActive = true;
    _HCChartType = "pie";
    _AnalyticsChartType = "Pie";
    GetMainTable();
}

function DEVFindTSSTSeries(id) {
    var seriesList = [];

    _TSSTSeries.find(function (e, i) {
        var sID = e.id;
        if (_isCompare)
        {
            sID = sID.split("_")[0];
        }
        if (sID === id)
        {
            seriesList.push(e);
        }
    });

    return seriesList;
}

function DEVChart() {
    return $("#divPrimaryChart").highcharts();
}

function TestForEmptyPie() {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series[0].data;
    var isEmpty = true;
    $.each(chartMain.series, function (si, sd) {
        $.each(sd.data, function (i, d) {
            if (d.y !== 0)
            {
                isEmpty = false;
            }
        });
    });

    return isEmpty;
}

function RoughSizeOfObject(object) {
    var objList = [];
    var stack = [object];
    var bytes = 0;

    while (stack.length)
    {
        var value = stack.pop();

        if (typeof value === "boolean")
        {
            bytes += 4;
        }
        else if (typeof value === "string")
        {
            bytes += value.length * 2;
        }
        else if (typeof value === "number")
        {
            bytes += 8;
        }
        else if (typeof value === "object" && objList.indexOf(value) === -1)
        {
            objList.push(value);

            for (var i in value)
            {
                stack.push(value[i]);
            }
        }
    }
    return FormatByteSize(bytes);
}

function FormatByteSize(bytes) {
    if (bytes > Math.pow(10, 9))    // Giga
    {
        var gb = bytes / Math.pow(10, 9);
        return gb + " GB";
    }
    else if (bytes > Math.pow(10, 6))   // Mega
    {
        var mb = bytes / Math.pow(10, 6);
        return mb + " MB";
    }
    else if (bytes > Math.pow(10, 3))    // Kilo
    {
        var kb = bytes / Math.pow(10, 3);
        return kb + " KB";
    }
    else
    {
        return bytes + " b";
    }
}

function ConvertTableToObjects() {
    // Select all MST rows
    var rows = $("#tbl_agent tr.secondaryDetailRow");

    rows.each(function (i, e) {
        var $agRow = $("tr[id^='agentTD_']");

        var $ocRow = $("td[id^='OCCURRENCES_']");
        var row = {
            r: e,
            agent: $(e).find($("td[id^='agentTD_']")).text(),
            occurrences: Number($(e).find($ocRow).text())
        };
        _DEVTable.push(row);
    });
}

function SortTableByAgent() {
    ConvertTableToObjects();

    var header = $("#tbl_agent tr.secondaryHeaderRow");
    $("#tbl_agent").html("");
    $(header).appendTo($("#tbl_agent"));

    _DEVTable.sort(function (a, b) {
        var nameA = a.agent.toLowerCase();
        var nameB = b.agent.toLowerCase();
        if (nameA < nameB) { return -1; }   // A comes before B
        if (nameA > nameB) { return 1; }    // A comes after B
        return 0;   // A === B
    });

    $(_DEVTable).each(function (i, e) {
        $("#tbl_agent").append(e.r);
    });
}

function AgentSort(a, b) {
    var nameA = $(a).find($("td[id^='agentTD_']")).text();
    var nameB = $(b).find($("td[id^='OCCURRENCES_']")).text();

    // Standard ASC
    if (nameA < nameB)
    {
        return -1;
    }
    if (nameA > nameB)
    {
        return 1;
    }
    return 0;
}

