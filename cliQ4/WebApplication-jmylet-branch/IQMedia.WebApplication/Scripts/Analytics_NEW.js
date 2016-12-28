/* Analytics icon Chart types */
// ID: type
// 1: multi-line
// 2: US map
// 3: Canada map
// 4: pie
// 5: bar
// 6: column
// 7: daytime heat map
// 8: growth
// 9: daypart heat map

/* Analytics ChartTypes */
// Bar
// Column
// Line
// Pie
// Daytime
// Growth
// Daypart

/* HCChartTypes */
// area
// arearange
// areaspline
// areasplinerange
// bar
// boxplot
// bubble
// column
// columnrange
// errorbar
// funnel
// gauge
// heatmap
// line
// pie
// polygon
// pyramid
// scatter
// solidgauge
// spline
// treemap
// waterfall

/* 3 Server updates of */
// 1. Update main table, main table data change will require tss table and chart update (they are dependent on data of main table)
// Updated on: filter change (including dates), page change
// 2. Update tab specific secondary table (tss table), tss table data change will require chart update (chart dependent on tss table data)
// Updated on: when main table updated (any event that will cause this), tab change, selected agent change (OT tab will handle MST like this, make server request)
// 3. Update of chart, chart data change will not require any table change

/* URLs for AJAX requests */
var _urlGetMainTable = "/Analytics/GetMainTable/";
var _urlGetTabTable = "/Analytics/GetTabSpecificTable/";
var _urlGetChart = "/Analytics/GetChart/";

/* Page Info */
var _tab = "OverTime";
var _pageType = null;

/* User selections */
var _selectedAgents = [];
var _dateInterval = null;

/* Saved chart series */
var _TSSTSeries = [];
var _MSTSeries = [];

/* DEV */
var _DEVJsonChart = "";
var _AnalyticsChartType = "Line";  // Default to Line
var _HCChartType = "spline";  // default to line
var _demoSubTab = "gender"; // default to gender

$(document).ready(function () {
    // Set thousands separator for HC
    Highcharts.setOptions({
        lang: {
            thousandsSep: ","
        }
    });

    // Make analytics main tab active
    $("#ulMainMenu li").removeClass("active");
    $("#liMenuAnalytics").addClass("active");

    // Initialize OverTime tab as active
    $("#ampOverTime").addClass("active");

    // Initialize QuickFilters and chart type
    $("#primaryNavAll").addClass("active");
    $("#primaryChartNav1").addClass("active");

    // ONLY HIDING BECAUSE OLD PROCESS STILL USED
    $("#AnalyticsSecondaryDetails").html("");

    // Hide TSST elements on page load
    $("#TSSTableTab").hide();
    $("#TSSTable").hide();

    // Initialize Main Table
    GetMainTable();
    $("#MSTableTab").addClass("active");
});

/*-------------------------- AJAX functions --------------------------*/

// Function will update both secondary tables as well as the chart
function GetMainTable() {
    // Only used while main JS file is Analytics.js //
    _selectedAgents = _searchAgents;
    _pageType = _PageType;
    //----------------------------------------------//

    var gRequest = {
        GraphType: _graphType,
        Tab: _tab,
        DateInterval: _dateInterval,
        AgentList: _selectedAgents,
        PageType: _pageType,
        HCChartType: _HCChartType
    }

    if (_tab == "Demographic")
    {
        gRequest.SubTab = $("#demoShortNav > li.active").attr("eleVal");
    }

    var jsonPostData = {
        graphRequest: gRequest,
        dateFrom: _dateFrom,
        dateTo: _dateTo
    }

    $("#AnalyticsSecondaryDetails").html("");
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetMainTable,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            $("#MSTable").html(result.MSTable);
            $("#MSTableTabLink").text(result.MSTableTab);
            _DEVJsonChart = result.chartJSON;

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
            }

            // If HTML string of TSSTable is not empty - otherwise no TSST for this tab
            if (result.TSSTable.length > 0)
            {
                $("#TSSTable").html(result.TSSTable);
                $("#TSSTableTabLink").text(result.TSSTableTab);
                $("#TSSTableTab").show();
                _TSSTSeries = result.TSSTSeries;
            }
            else
            {
                $("#TSSTable").hide();
                $("#TSSTableTab").hide();
                _MSTSeries = result.MSTSeries;
            }

            var selectorPrefix = _pageType == "campaign" ? "campaignCB_" : "agentCB_";
            $("#AnalyticsSecondaryDetails").html("");

            _selectedAgents = [];
            $("[id^='" + selectorPrefix + "']:checked").each(function (i, el) {
                var selectedID = Number(el.id.slice(el.id.indexOf('_') + 1));
                _selectedAgents.push(selectedID);
            });

            SetChartSeriesIDs();
            SetColorsFromChart();
            FormatLegend();
        },
        error: function (result) {
            console.log("GetMainTable ajax error");
        }
    });
}

function GetTabSpecificTable() {
    // Only used while main JS file is Analytics.js //
    _selectedAgents = _searchAgents;
    _pageType = _PageType;
    //----------------------------------------------//
    var gRequest = {
        GraphType: _graphType,
        Tab: _tab,
        DateInterval: _dateInterval,
        AgentList: _selectedAgents,
        PageType: _pageType,
        HCChartType: _HCChartType,
        chartType: _AnalyticsChartType
    }

    if (_tab == "Demographic")
    {
        gRequest.SubTab = $("#demoShortNav > li.active").attr("eleVal");
    }

    var jsonPostData = {
        graphRequest: gRequest,
        dateFrom: _dateFrom,
        dateTo: _dateTo
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetTabTable,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            $("#TSSTable").html(result.TSSTable);
            $("#TSSTableTab").text(result.TSSTableTab)
        },
        error: function (result) {
            console.log("GetTabSpecificTable ajax error");
        }
    });
}

function GetChart() {
    var chartID = $("#primaryChartNav > li > a.active").attr("id");
    var chartSelected = chartID.charAt(chartID.length - 1);
    var hcChartType = "spline"; // Generally the initial chart type

    switch (chartSelected)
    {
        case 1: // Multi-line
        case 8: // Growth
            hcChartType = "spline";
            break;
        case 4:
            hcChartType = "pie";
            break;
        case 5:
            hcChartType = "bar";
            break;
        case 6:
            hcChartType = "column";
            break;
        case 7: // Daytime
        case 9: // Daypart
            hcChartType = "heatmap";
            break;
    }

    _pageType = _PageType;

    var gRequest = {
        GraphType: _graphType,
        Tab: _tab,
        DateInterval: _dateInterval,
        AgentList: _selectedAgents,
        PageType: _pageType,
        HCChartType: hcChartType,
        chartType: _AnalyticsChartType
    };

    if (_tab == "Demographic")
    {
        gRequest.SubTab = $("#demoShortNav > li.active").attr("eleVal");
    }

    var jsonPostData = {
        graphRequest: gRequest,
        dateFrom: _dateFrom,
        dateTo: _dateTo
    };

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetChart,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            _TSSTSeries = result.TSSTSeries;
            _DEVJsonChart = result.chartJSON;

            switch (HCChartType)
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
                    if (analyticsChartType == "Daytime")
                    {
                        RenderHighChartsDaytimeHeatMap(result.chartJSON);
                    }
                    else
                    {
                        RenderHighChartsDaypartHeatMap(result.chartJSON);
                    }
                    break;
            }

            SetChartSeriesIDs();
            SetColorsFromChart();
            FormatLegend();
        },
        error: function (result) {
            console.log("GetChart ajax error");
        }
    });
}

/*-------------------------- Other functions --------------------------*/

// To be called when trying to toggle a TSST series that is not present in the chart
function AddSeriesToChart(id) {
    var chartMain = $("#divPrimaryChart").highcharts();

    if (id == "65+")
    {
        id = "65\\+";
    }

    if (_tab == "OverTime")
    {
        var newSeries = _MSTSeries.find(function (e, i) {
            return e.id == id;
        });
    }
    else
    {
        var newSeries = _TSSTSeries.find(function (e, i) {
            return e.id == id;
        });
    }

    chartMain.addSeries(newSeries);

}

// Copies id from id prop of data in each series into series itself
function SetStoredSeriesIDs() {
    if (_tab == "OverTime")
    {
        $.each(_MSTSeries, function (i, s) {
            s.id = s.data[0].Value;
        });
    }
    else
    {
        $.each(_TSSTSeries, function (i, s) {
            var seriesID = s.data[0].Value;
            if (seriesID == "65+")
            {
                seriesID = "65\\+";
            }
            s.id = seriesID;
        });
    }
}

function TableSwitch(table) {
    $("#bottomShortNav").children().removeAttr("class");
    if (table == "MST")
    {
        $("#TSSTable").hide();
        $("#MSTable").show();
        $("#MSTTableTab").addClass("active");
    }
    else
    {
        $("#TSSTable").show();
        $("#MSTable").hide();
        $("#TSSTableTab").addClass("active");
    }
}

function SetColorsFromChart() {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series;
    var cbIDPrefix = "#agentCB_";   // TODO - add switch case changes to 
    // TODO - when hooked up to chart icons switch series to 'series = chartMain.series[0].data;'

    var visibleSeries = series.filter(function (s, i) {
        return s.visible == true;
    });

    // Color only CBs for visible series
    $.each(visibleSeries, function (i, s) {
        $(cbIDPrefix + s.options.id + " + label div span").css("background-color", s.color);
        $(cbIDPrefix + s.options.id + " + label div span").prop("title", "select to remove from graph");
    });

    // If tab is not overtime then color selected agents gray
    if (_tab !== "OverTime")
    {
        $("[id^='agentCB_']:checked + label div span").css("background-color", "#6D6E71");
        $("[id^='agentCB_']:checked + label div span").prop("title", "select to remove from graph");
    }
}

/*-------------------------- DEV functions --------------------------*/

function DEVGetChart(HCChartType, analyticsChartType) {
    _selectedAgents = _searchAgents;
    _pageType = _PageType;

    var gRequest = {
        GraphType: _graphType,
        Tab: _tab,
        DateInterval: _dateInterval,
        AgentList: _selectedAgents,
        PageType: _pageType,
        HCChartType: HCChartType,
        chartType: analyticsChartType,
        SubTab: _demoSubTab
    }

    var jsonPostData = {
        graphRequest: gRequest,
        dateFrom: _dateFrom,
        dateTo: _dateTo
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetChart,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            _TSSTSeries = result.TSSTSeries;
            _DEVJsonChart = result.chartJSON;

            switch (HCChartType)
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
                    if (analyticsChartType == "Daytime")
                    {
                        RenderHighChartsDaytimeHeatMap(result.chartJSON);
                    }
                    else
                    {
                        RenderHighChartsDaypartHeatMap(result.chartJSON);
                    }
                    break;
            }
        },
        error: function (result) {
            console.log("DevGetChart ajax error");
        }
    });
}

function DEVDisplayChartSeries() {
    var chartMain = $("#divPrimaryChart").highcharts();
    console.log("ChartMain");
    console.dir(chartMain);
    for (var i = 0; i < chartMain.series.length; i++)
    {
        console.log("series: " + chartMain.series[i].name);
        console.dir(chartMain.series[i].data);
    }
}

function DEVAddPiePoint(seriesIndex) {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = _TSSTSeries[seriesIndex];
    var seriesTotal = 0;

    series.data.forEach(function (item, index) {
        if (item.y > 0)
        {
            seriesTotal += item.y;
        }
    });

    console.log("seriesTotal " + seriesTotal);

    chartMain.series[0].addPoint({
        name: series.name,
        y: seriesTotal
    });
}

function DEVAddSeries(seriesIndex) {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = _TSSTSeries[seriesIndex];
    chartMain.addSeries({
        name: series.name,
        keys: ['','','','','','','','y'],
        data: series.data
    });
    chartMain.xAxis[0].setExtremes(0, 0);
}

function DEVSetChartSeriesIDs(analyticsChartType) {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series;
    var secondaryTab = $("#bottomShortNav > li.active").text().trim();

    if (analyticsChartType == 4) // Pie chart
    {
        var lstSeries = [];
        // Get names and IDs from secondary
        var selectorPrefix = "agent";

        switch (_tab)
        {
            case "Sources":
                selectorPrefix = "source";
                break;
            case "Demographic":
                selectorPrefix = "demographic";
                break;
            case "Daytime":
                selectorPrefix = "daytime";
                break;
            case "Daypart":
                selectorPrefix = "daypart";
                break;
            case "Market":
                selectorPrefix = "market";
                break;
        }

        if (_tab == "Demographic")
        {
            // Demographic should always have the same series name and ID
            $.each(series[0].data, function (i, dat) {
                var demoID = dat.name;
                if (demoID == "65+")
                {
                    demoID = "65\\+";
                }
                lstSeries.push({
                    ID: demoID,
                    name: dat.name
                });
            });
        }
        // Selected chart is a pie chart, not on demo tab
        else
        {
            // Get all checked checkboxes on tab
            var cbs = $("[id^='" + selectorPrefix + "CB_']:checked");
            for (var i = 0; i < cbs.length; i++)
            {
                // For each checkbox, get the containing table cell
                var row = cbs.eq(i).parent().parent();
                var td = row.children("[id^='" + selectorPrefix + "TD_']");
                var tdID = td.prop("id");

                var sID = tdID.substr(tdID.indexOf('_')).substr(1);
                var sName = td.text().trim();

                if (_tab == "Daytime")
                {
                    // Remove hour from ID and name - want to just be day of week
                    sID = sID.slice(0, sID.indexOf('_'));
                    sName = sID;
                }
                // Add the ID and text of each cell to list of series
                lstSeries.push({
                    ID: sID,
                    name: sName
                });
            }
        }

        $.each(series[0].data, function (indexD, datum) {
            $.each(lstSeries, function (indexS, s) {
                if (s.name == datum.name)
                {
                    datum.update({
                        id: s.ID
                    });
                }
            });
        });
    }
    else
    {
        // If not a pie chart, simply update each series from first data point value
        for (var i = 0; i < series.length; i++)
        {
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

