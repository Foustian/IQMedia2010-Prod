var _urlMediaJsonChart = "/Analytics/MediaJsonChart/";
var _urlGetChartData = "/Analytics/GetChartData/";
var _urlGetOverlayData = "/Analytics/GetOverlayData/";
var _urlOpenIFrame = "/Analytics/OpenIFrame/";
var _urlGetSecondaryTables = "/Analytics/GetSecondaryTables/";
var _urlGeneratePDF = "/Analytics/GeneratePDF/";
var _urlDownloadPDF = "/Analytics/DownloadPDF/";

var _searchAgents = [];
var _isFilterActive = false;
var _dateFrom = null;
var _dateTo = null;
var _subMediaType = null;
var _responseJSON = null;
var _SelectedAgentIDs = [];
var _FilterSetAgentIDs = [];
var _tab = "OverTime";
var _previousTab = null;
var _IsNewSearch = true; // Indicates if new data should be fetched from the database, due to updated filter criteria

var _IsOverlayEnabled = true;
var _ActiveOverlayType = null;
var _NumOverlaySeries = 0;
var _ActivePESHTypes = [];
var _ActiveSourceGroups = [];
var _dateInterval = "day";  // Default to day
var _isCompare = false; // Used to signal an uneven date comparison
var _defaultGraphType = null; // Holds the default graph type to be used if not doing an agent comparison
var _graphType = "line"; // Holds the current graph type that is passed to the server when building charts
var _PageType = null;
var _IsExporting = false;

// Current Chart Selection
var _SelectedChartType = null; // The type of the currently selected chart
var _SelectedChartOverlayEnabled = null;  // A flag indicating if the currently selected chart allows overlays
var _SelectedChartIsMap = null; // A flag indicating if the currently selected chart is a map

//permissions
var _IsLRAccess = false;
var _IsAdsAccess = false;

//configuration 
var _urlGetActiveElements = "/Analytics/GetActiveElements/";
var _ActiveElements = [];

var _UserSetTimeSpan = '';

// On Page Load and ready
$(document).ready(function () {
    // Set thousands separator for highcharts
    Highcharts.setOptions({
        lang: {
            thousandsSep: ","
        }
    });

    // Makes main analytics tab show as active
    $("#ulMainMenu li").removeClass("active");
    $("#liMenuAnalytics").addClass("active");

    // Initialize over time tab to be active
    $("#ampOverTime").addClass("active");

    $("#primaryNavAll").addClass("active");
    $("#primaryChartNav1").addClass("active");

    // Adds function to listen to click on date range
    $("#dateRangeList").children().click(function (e) {
        $("#dateRangeList li").children().removeClass("active");
        $(this).children().addClass("active");
        ChangeTrailingDateRange(e);
    });
    $("#dateRange30days").addClass("active");

    // Adds function to listen to click on date interval
    $("#dateIntervalList").children().click(function (e) {
        $("#dateIntervalList li").children().removeClass("active");
        $(this).children().addClass("active");
        ChangeDateInterval(e);
    });
    $("#dateIntervalDay").addClass("active");

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

    $("#dpDateFromFilter0").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });
    $("#dpDateToFilter0").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });

    $("#dpDateFromFilter1").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });
    $("#dpDateToFilter1").datepicker({
        showOn: "focus",
        changeMonth: true,
        changeYear: true
    });

    $("#linkAddAgentFilter0").on("click", ToggleFilterAgentCompare);

    $("#ddSourceFilter0").on("change", ChangeSubMediaType);
    $("#ddSourceFilter1").attr("disabled", true);

    //set defaults
    $("li[name='liSecondaryTab']").hide();

    $("#linkApplyFilters").on("click", ApplyFilters);
    $("#linkRemoveFilters").on("click", ClearFiltersAndReload);

    $("#filterLink").on("click", ToggleFilterDiv);

    _PageType = getParameterByName("type");

    switch (_PageType)
    {
        case "amplification":
            _defaultGraphType = "line";

            SetDefaultDateRange();

            $("#linkCloseDateRange").on("click", CloseDateRange);
            $("#linkOpenDateRange").on("click", OpenDateRange);
            $("#linkApplyDateRange").on("click", ApplyDateRange);

            $("#dpDateFrom").on("change", SpecifyDateRange);
            $("#dpDateTo").on("change", SpecifyDateRange);

            break;
        case "campaign":
            _defaultGraphType = "comparison";

            $("#dpDateFromFilter0").datepicker("disable");
            $("#dpDateToFilter0").datepicker("disable");
            $("#dpDateFromFilter1").datepicker("disable");
            $("#dpDateToFilter1").datepicker("disable");

            // Map charts and Google overlay data should be unavailable in Campaign
            ToggleIconStatus("primaryChartNav2", false);
            ToggleIconStatus("primaryOverlayNav1", false);
            break;
    }

    _graphType = _defaultGraphType;

    // Add on change listeners to filter dds and dps to change color of apply filter link
    $("#ddAgentFilter0").on("change", FilterCriteriaChange);
    $("#ddAgentFilter1").on("change", FilterCriteriaChange);
    $("#ddSourceFilter0").on("change", FilterCriteriaChange);
    $("#ddSourceFilter1").on("change", FilterCriteriaChange);
    $("#dpDateFromFilter0").on("change", FilterDateCriteriaChange);
    $("#dpDateToFilter0").on("change", FilterDateCriteriaChange);
    $("#dpDateFromFilter1").on("change", FilterDateCriteriaChange);
    $("#dpDateToFilter1").on("change", FilterDateCriteriaChange);

    ChangeTab("OverTime");
});

function GeneratePDF() {
    _IsExporting = true;
    if (_SelectedChartIsMap)
    {
        $("#divPrimaryChart").updateFusionCharts({ width: 950, height: 350 });
    }
    else
    {
        GeneratePDFHelper();
    }
}

function GeneratePDFHelper() {
    if (!_SelectedChartIsMap)
    {
        $("#divPrimaryChart").css("width", "950px");
        $("#divPrimaryChart").highcharts().reflow();
    }

    var jsonPostData = {
        HTML: $("#divContent").html(),
        dateFrom0: $("#dpDateFromFilter0").val(),
        dateFrom1: $("#dpDateFromFilter1").val(),
        dateTo0: $("#dpDateToFilter0").val(),
        dateTo1: $("#dpDateToFilter1").val(),
        agent0: $("#ddAgentFilter0").children().filter(":selected").text(),
        agent1: $("#ddAgentFilter1").children().filter(":selected").text(),
        source0: $("#ddSourceFilter0").children().filter(":selected").text(),
        source1: $("#ddSourceFilter1").children().filter(":selected").text()
    }

    _IsExporting = false;

    if (_SelectedChartIsMap)
    {
        $("#divPrimaryChart").updateFusionCharts({ width: "100%", height: 350 });
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
        error: function (result) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function ChangeTab(SecondaryTabID) {
    var tabIDName = "#amp" + SecondaryTabID;
    _previousTab = _tab;
    _tab = SecondaryTabID;

    $("li[name='liSecondaryTab']").hide();

    // Clear page of things that may not be available on other tabs
    // Clear any active overlays
    RemoveChartOverlay();
    // Clear any active PESH filters
    _ActivePESHTypes = [];
    _ActiveSourceGroups = [];
    $("#primaryNavBar a").removeClass("active");
    $("#primaryNavAll").addClass("active");

    // Removes class from <a> children of <li>s on old tab
    $("#ampBar li").children().removeClass("active");
    // Attaches active to <a>s inside <li>s for new tab
    $(tabIDName).addClass("active");

    // Hide tabs to switch primary sub chart
    $("#divBottomChartNav ul").hide();

    // Hide Daytime/Daypart chart icon, enable all chart icons
    $("#primaryChartNav7").addClass("displayNoneImp");
    $("#primaryChartNav9").addClass("displayNoneImp");
    ToggleIconStatusByGroup($('a[name="primaryChartNav"]'), true);

    // Maps are disabled in Campaign mode
    if (_PageType == "campaign") {
        ToggleIconStatus("primaryChartNav2", false);
    }

    // Set initial selected chart values - Line Chart
    _SelectedChartType = 1;
    _SelectedChartOverlayEnabled = true;
    _SelectedChartIsMap = false;

    // If switching to or away from Daytime tab, data may change, so set to new search
    if ((_tab != "Daytime" && _previousTab == "Daytime") || (_tab == "Daytime" && _previousTab != "Daytime") || (_tab != "Daypart" && _previousTab == "Daypart") || (_tab == "Daypart" && _previousTab != "Daypart"))
    {
        _IsNewSearch = true;
    }

    $("#dateIntervalLink").removeClass("disabled");
    $("#primaryNavPrint").show(); // Show Quick Filter (print), only hide on demographic

    //Set default time span to day (this is overwritten for day/part and day/time)
    if (_UserSetTimeSpan == '') {
        $("#dateIntervalLink").text("day");
        $("#dateIntervalList li").children().removeClass("active");
        $("#dateIntervalDay").addClass("active");
        _dateInterval = "day";
    }
    else {
        $("#dateIntervalLink").text(_UserSetTimeSpan);
        $("#dateIntervalList li").children().removeClass("active");
        if (_UserSetTimeSpan == 'hour') {
            $("#dateIntervalHour").addClass("active");
        }
        else if (_UserSetTimeSpan == 'day') {
            $("#dateIntervalDay").addClass("active");
        }
        else if (_UserSetTimeSpan == 'month') {
            $("#dateIntervalMonth").addClass("active");
        }
        _dateInterval = _UserSetTimeSpan;
    }

    // Enable/Disable based on destination tab
    switch (SecondaryTabID)
    {
        case "OverTime":
            SetPESHFiltersEnabledDisabled(true);
            $("#divAddAgentFilter0").show();

            break;
        case "Demographic":
            SetPESHFiltersEnabledDisabled(false);
            $("#primaryNavPrint").hide(); // Hide Quick Filter (print)

            // Disabling second agent/campaign filter for demographic
            $("#divAddAgentFilter0").hide();
            if (!$("#divAgentFilter1").hasClass("hidden"))
            {
                ToggleFilterAgentCompare();
            }
            // Remove second agent from agents to be requested
            //AddFilterAgentsToSearch();

            //disable map in demographics
            ToggleIconStatus("primaryChartNav2", false);
            ToggleIconStatus("primaryChartNav8", false);

            // Show Gender/Age tabs
            $("#demoShortNav").show();
            $("#demoShortNav li").removeClass("active");
            $("#liGender").addClass("active");
            break;
        case "Daytime":
            // Disabling second agent/campaign filter for daytime
            $("#divAddAgentFilter0").hide();
            if (!$("#divAgentFilter1").hasClass("hidden"))
            {
                ToggleFilterAgentCompare();
            }

            SetPESHFiltersEnabledDisabled(true);
            $("#primaryChartNav7").removeClass("displayNoneImp");

            // Set initial selected chart values - Daytime Chart
            _SelectedChartType = 7;
            _SelectedChartOverlayEnabled = false;
            _SelectedChartIsMap = false;

            // Disable Date Interval ddl, since it should always be set to hour
            $("#dateIntervalLink").addClass("disabled");
            $("#dateIntervalLink").text("hour");
            $("#dateIntervalList li").children().removeClass("active");
            $("#dateIntervalHour").addClass("active");
            _dateInterval = "hour";

            // Disable all chart icons other than daytime
            ToggleIconStatusByGroup($('a[name="primaryChartNav"]').not('[isDaytime="true"]'), false);

            // Hide all CB labels

            break;
        case "Daypart":
            // Disabling second agent/campaign filter for daypart
            $("#divAddAgentFilter0").hide();
            if (!$("#divAgentFilter1").hasClass("hidden")) 
            {
                ToggleFilterAgentCompare();
            }

            SetPESHFiltersEnabledDisabled(true);
            $("#primaryChartNav9").removeClass("displayNoneImp");

            // Set initial selected chart values - Daypart Chart
            _SelectedChartType = 9;
            _SelectedChartOverlayEnabled = false;
            _SelectedChartIsMap = false;

            // Disable Date Interval ddl, since it should always be set to hour
            $("#dateIntervalLink").addClass("disabled");
            $("#dateIntervalLink").text("hour");
            $("#dateIntervalList li").children().removeClass("active");
            $("#dateIntervalHour").addClass("active");
            _dateInterval = "hour";

            // Disable all chart icons other than daypart
            ToggleIconStatusByGroup($('a[name="primaryChartNav"]').not('[isDaypart="true"]'), false);
            break;
        case "Sources":
            $("#divAddAgentFilter0").show();
            SetPESHFiltersEnabledDisabled(true);
            break;
        case "Market":  // Very similar to source
            $("#divAddAgentFilter0").show();
            SetPESHFiltersEnabledDisabled(true);
            break;
    }

    GetSecondaryTables();
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

function FilterCriteriaChange() {
    $("#linkApplyFilters").css("color", "red");
}

function ChangeSubMediaType() {
    var selected = $("#ddSourceFilter0").children().filter(":selected");

    if (selected.val() == "null")
        _subMediaType = null;
    else
        _subMediaType = selected.val();

    // Set the second source filter to the value of the first
    $("#ddSourceFilter1").find(":contains('" + selected.text() + "')").prop("selected", true);
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

// Called on clear filters button click
function ClearFiltersAndReload() {
    ClearFilters();
    GetSecondaryTables();
}

// Used to clear filter criteria
function ClearFilters() {
    _isFilterActive = false;
    _IsNewSearch = true;
    _graphType = _defaultGraphType;
    // Clear Search Agents
    _searchAgents = [];
    // Reset sub media type
    _subMediaType = null;
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

    // Set date ranges to default
    SetDefaultDateRange();

    $("#filterLink").text("no filters applied");

    if (_PageType != "campaign") {
        ToggleFilterDiv();
    }
    else
    {
        if (!$("#divAgentFilter1").hasClass("hidden"))
        {
            ToggleFilterAgentCompare();
        }
        ToggleFilterDiv();
    }

}

// Called on apply filters button click
function ApplyFilters() {
    _isFilterActive = true;
    _IsNewSearch = true;
    _graphType = _defaultGraphType;
    GetDateRange();
    AddFilterAgentsToSearch();

    $("#filterLink").text("filters applied");
    $("#linkApplyFilters").css("color", "");

    // Campaign filter doesn't have date range text to update
    if (_PageType == "amplification")
    {
        UpdateDateRangeFromFilter();
    }

    GetSecondaryTables();
}

// Updates date range text "Results in the ..." and date range drop date pickers from filter being applied
function UpdateDateRangeFromFilter() {
    var filterStartDate = $("#dpDateFromFilter0").datepicker("getDate");
    var filterEndDate = $("#dpDateToFilter0").datepicker("getDate");

    if (!$("#divAgentFilter1").hasClass("hidden"))
    {
        var agent1StartDate = $("#dpDateFromFilter1").datepicker("getDate");
        var agent1EndDate = $("#dpDateToFilter1").datepicker("getDate");

        // Set overall filter date range if second agent has a start date prior to first agent or an end date after the first agent
        filterStartDate = filterStartDate < agent1StartDate ? filterStartDate : agent1StartDate;
        filterEndDate = filterEndDate > agent1EndDate ? filterEndDate : agent1EndDate;
    }

    // Update date range drop date selectors with overall date range
    $("#dpDateFrom").datepicker("setDate", filterStartDate);
    $("#dpDateTo").datepicker("setDate", filterEndDate);

    var startDateTxt = (filterStartDate.getMonth() + 1) + "/" + filterStartDate.getDate() + "/" + filterStartDate.getFullYear();
    var endDateTxt = (filterEndDate.getMonth() + 1) + "/" + filterEndDate.getDate() + "/" + filterEndDate.getFullYear();

    $("#linkOpenDateRange").text("range " + startDateTxt + " to " + endDateTxt);
}

// Updates the date range of filter if date range drop down is changed or filter cleared
function UpdateFilterDateRanges() {
    $("#dpDateFromFilter0").datepicker("setDate", _dateFrom);
    $("#dpDateToFilter0").datepicker("setDate", _dateTo);

    if (!$("#divAgentFilter1").hasClass("hidden"))
    {
        $("#dpDateFromFilter1").datepicker("setDate", _dateFrom);
        $("#dpDateToFilter1").datepicker("setDate", _dateTo);
    }
}

// Update the start/end dates of every item in _searchAgents
function UpdateAgentsDateRange() {
    // Assuming that only date range was changed
    for (var i = 0; i < _searchAgents.length; i++)
    {
        _searchAgents[i].DateFrom = _dateFrom;
        _searchAgents[i].DateTo = _dateTo;
    }
}

// Now only closes date range drop without applying range
function CloseDateRange() {
    $("#dateRangeDD").css("display", "none");
}

// Used if the user purposefully applies a new date range
function ApplyDateRange() {
    _IsNewSearch = true;

    $("#dateRangeDD").css("display", "none");
    GetDateRange();
    UpdateAgentsDateRange();
    UpdateFilterDateRanges();
    GetSecondaryTables();
}

// Toggle hidden class of filter div
function ToggleFilterDiv() {
    $("#divFilters").toggleClass("hidden");
}

// Updates date range text "Results in the ..." from change to date range drop down
function SpecifyDateRange() {
    GetDateRange();

    // If date pickers are left empty default to prev 30 days
    if (_dateFrom == null || _dateFrom == "")
    {
        $("#dpDateFrom").datepicker("setDate", -30);
        GetDateRange();
    }

    if (_dateTo == null || _dateTo == "")
    {
        $("#dpDateTo").datepicker("setDate", Date.now());
        GetDateRange();
    }

    var startDate = (_dateFrom.getMonth() + 1) + "/" + _dateFrom.getDate() + "/" + _dateFrom.getFullYear();
    var endDate = (_dateTo.getMonth() + 1) + "/" + _dateTo.getDate() + "/" + _dateTo.getFullYear();
    $("#linkOpenDateRange").text("range " + startDate + " to " + endDate);
}

// Used only when user clicks a range under "trailing" - will update date picker values
function ChangeTrailingDateRange(e) {
    // Change text of link
    var range = e.target.innerHTML;
    $("#linkOpenDateRange").text("trailing " + range);

    // Trailing will always have an end date of today
    $("#dpDateTo").datepicker("setDate", Date.now());

    // Reset

    // Set dateFrom based on range chosen
    switch (range)
    {
        // 12 months assumed to be 365 days
        case "12 months":
            $("#dpDateFrom").datepicker("setDate", -365);
            break;
        case "90 days":
            $("#dpDateFrom").datepicker("setDate", -90);
            break;
        case "30 days":
            $("#dpDateFrom").datepicker("setDate", -30);
            break;
        case "7 days":
            $("#dpDateFrom").datepicker("setDate", -7);
            break;
        case "day":
            $("#dpDateFrom").datepicker("setDate", -1);
            break;
        // Default is trailing 30 days
        default:
            $("#dpDateFrom").datepicker("setDate", -30);
            break;
    }
}

function ChangeDateInterval(e) {
    _IsNewSearch = true;

    // Change text of link
    var interval = e.target.innerHTML;
    $("#dateIntervalLink").text(interval);
    
    _UserSetTimeSpan = interval;
    _dateInterval = interval;
    GetDateRange();

    GetSecondaryTables();
}

// Updated date range global variables
function GetDateRange() {
    if (_PageType != "campaign")
    {
        _dateFrom = $("#dpDateFrom").datepicker("getDate");
        _dateTo = $("#dpDateTo").datepicker("getDate");
    }
    else
    {
        _dateFrom = null;
        _dateTo = null;
    }
}

// Show the date range choices drop down
function OpenDateRange() {
    $("#dateRangeDD").css("display", "block");
}

// Used to determine if a series is in the chart based on id  - returns series object or null
function IsSeriesInChart(seriesID) {
    var chartID = $("#primaryChartNav > li > a.active").attr("id");
    var chartSelected = chartID.charAt(chartID.length - 1);
    //console.log("IsSeriesInChart(" + seriesID + ")");
    var series = $("#divPrimaryChart").highcharts().series;

    if (chartSelected == 4) // Pie chart
    {
        series = series[0].data;
    }
    else if (_SelectedChartIsMap)    // Maps
    {
        // Maps do not contain "series" 
        return null;
    }

    for (var i = 0; i < series.length; i++)
    {
        // If series with seriesID is present in chart return series
        if (seriesID == series[i].options.id)
        {
            //console.log("series IS IN chart");
            return series[i];
        }
    }
    //console.log("series NOT IN chart");
    // If series with id seriesID not encountered in chart return null
    return null;
}

function addCommas(str) {
    str = str.toString();
    if (str.toLowerCase() == 'nan' || str == '') {
        return 0;
    }
    else {
        var parts = (str + "").split("."),
        main = parts[0],
        len = main.length,
        output = "",
        first = main.charAt(0),
        i;

        if (first === '-') {
            main = main.slice(1);
            len = main.length;
        } else {
            first = "";
        }
        i = len - 1;
        while (i >= 0) {
            output = main.charAt(i) + output;
            if ((len - i) % 3 === 0 && i > 0) {
                output = "," + output;
            }
            --i;
        }
        // put sign back
        output = first + output;
        // put decimal part back
        if (parts.length > 1) {
            output += "." + parts[1];
        }
        return output;
    }
}

function UpdatePESHFilters(id, checked) {
    var oldNum = 0;
    var changeNum = 0;
    var newNum = 0;

    //OCCURRENCES
    oldNum = Number($("#primaryNavAll_Results").text().replace(/,/g, ''));
    changeNum = Number($("#OCCURRENCES_" + id).text().replace(/,/g, ''));
    newNum = 0;
    if (checked) {
        newNum = oldNum + changeNum;
    }
    else {
        newNum = oldNum - changeNum;
    }
    $("#primaryNavAll_Results").text(addCommas(newNum));

    if (_IsLRAccess) {
        //SEEN
        oldNum = Number($("#primaryNavSeen_Results").text().replace(/,/g, ''));
        changeNum = Number($("#SEEN_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked) {
            newNum = oldNum + changeNum;
        }
        else {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavSeen_Results").text(addCommas(newNum));

        //HEARD
        oldNum = Number($("#primaryNavHeard_Results").text().replace(/,/g, ''));
        changeNum = Number($("#HEARD_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked) {
            newNum = oldNum + changeNum;
        }
        else {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavHeard_Results").text(addCommas(newNum));

        //READ
        oldNum = Number($("#primaryNavRead_Results").text().replace(/,/g, ''));
        changeNum = Number($("#READ_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked) {
            newNum = oldNum + changeNum;
        }
        else {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavRead_Results").text(addCommas(newNum));
    }

    if (_IsAdsAccess) {
        //PAID
        oldNum = Number($("#primaryNavPaid_Results").text().replace(/,/g, ''));
        changeNum = Number($("#PAID_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked) {
            newNum = oldNum + changeNum;
        }
        else {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavPaid_Results").text(addCommas(newNum));

        //EARNED
        oldNum = Number($("#primaryNavEarned_Results").text().replace(/,/g, ''));
        changeNum = Number($("#EARNED_" + id).text().replace(/,/g, ''));
        newNum = 0;
        if (checked) {
            newNum = oldNum + changeNum;
        }
        else {
            newNum = oldNum - changeNum;
        }
        $("#primaryNavEarned_Results").text(addCommas(newNum));
    }

    //ON AIR TIME
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

    //ONLINE
    oldNum = Number($("#primaryNavOnline_Results").text().replace(/,/g, ''));
    changeNum = Number($("#OCCURRENCES_" + id).attr('OnlineCount').replace(/,/g, ''));
    newNum = 0;
    if (checked) {
        newNum = oldNum + changeNum;
    }
    else {
        newNum = oldNum - changeNum;
    }
    $("#primaryNavOnline_Results").text(addCommas(newNum));

    //PRINT
    oldNum = Number($("#primaryNavPrint_Results").text().replace(/,/g, ''));
    changeNum = Number($("#OCCURRENCES_" + id).attr('PrintCount').replace(/,/g, ''));
    newNum = 0;
    if (checked) {
        newNum = oldNum + changeNum;
    }
    else {
        newNum = oldNum - changeNum;
    }
    $("#primaryNavPrint_Results").text(addCommas(newNum));
}

function ToggleSeries(id) {
    //console.log("ToggleSeries(" + id + ")");
    if (id == "65+")
    {
        id = "65\\+";
    }
    var secondaryTab = $("#bottomShortNav > li.active").text().trim();
    var cbIDPrefix = "#agentCB_";
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series;
    var seriesColor;
    var checked;

    if (_PageType == "campaign")
    {
        cbIDPrefix = "#campaignCB_";
    }

    //MANUALLY UPDATE PESH FILTERS
    if ($('li[name="liSecondaryTab"].active').text() == 'agent' || $('li[name="liSecondaryTab"].active').text() == 'campaign') {
        UpdatePESHFilters(id, $(cbIDPrefix + id).prop("checked"));
    }

    if (_SelectedChartIsMap)
    {
        // If chart is map and trying to toggle series, toggle agent from _searchAgents and reload chart
        checked = $(cbIDPrefix + id).prop("checked");
        // Toggle presence in _searchAgents
        AddAgentToSearch(id);
        // Update color and title based on if checked
        var newColor = checked ? "#6D6E71" : "transparent";
        var newTitle = checked ? "select to remove from graph" : "select to add to graph";

        $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
        $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
        LoadSelectedChart();
        return; // End function
    }

    if (_tab == "OverTime")
    {
        // Is agent checked
        checked = $(cbIDPrefix + id).prop("checked");
        //console.log("overtime " + (checked ? "is" : "is not") + " checked");

        // Determine if agent series is in chart
        var agentSeries = IsSeriesInChart(id);
        if (agentSeries !== null)  // Agent is in chart
        {
            // Toggle presence in _searchAgents - possible that an agent is a series in the chart but not in _searchAgents
            AddAgentToSearch(id);
            // Toggle visibility in chart
            agentSeries.update({
                visible: checked
            });
            // Update color & title based on if checked
            var newColor = checked ? agentSeries.color : "transparent";
            var newTitle = checked ? "select to remove from graph" : "select to add to graph";

            $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
            $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
        }
        else  // Toggle agent is not present in chart
        {
            // If filter active clear it and make sure agents selected from filter are still selected
            if (_isFilterActive)
            {
                var tempSearch = _searchAgents;
                ClearFilters();
                _searchAgents = tempSearch;
            }
            // Add agent to _searchAgents and get new chart & secondary
            AddAgentToSearch(id);
            LoadSelectedChart();
        }
    }
    else  // Series in chart do not represent agents
    {
        switch (secondaryTab)
        {
            case "agent":
                // Add/remove agent from searchAgents then get new chart/secondary
                AddAgentToSearch(id);
                checked = $(cbIDPrefix + id).prop("checked");
                if (!checked)
                {
                    $(cbIDPrefix + id + " + label div span").css("background-color", "transparent");
                    $(cbIDPrefix + id + " + label div span").prop("title", "select to add to graph");
                }
                LoadSelectedChart();
                break;
            case "campaign":
                // Add remove campaign from search
                AddAgentToSearch(id);
                checked = $(cbIDPrefix + id).prop("checked");
                if (!checked)
                {
                    $(cbIDPrefix + id + " + label div span").css("background-color", "transparent");
                    $(cbIDPrefix + id + " + label div span").prop("title", "select to add to graph");
                }
                LoadSelectedChart();
                break;
            case "demographic":
                cbIDPrefix = "#demographicCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var ageSeries = IsSeriesInChart(id);
                // Toggle visibility in chart
                ageSeries.update({
                    visible: checked
                });
                // Update title and color based on if checked
                var newColor = checked ? ageSeries.color : "transparent";
                var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                break;
            case "daytime":
                cbIDPrefix = "daytimeCB_";
                var daytimeID = id.slice(0, id.indexOf("_"));
                checked = $("#" + cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var daytimeSeries = IsSeriesInChart(daytimeID);
                // Toggle visibility in chart
                daytimeSeries.update({
                    visible: checked
                });
                // Update title and color based on if checked
                var newColor = checked ? daytimeSeries.color : "transparent";
                var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                $("[id^='" + cbIDPrefix + daytimeID + "'] + label div span").css("background-color", newColor);
                $("[id^='" + cbIDPrefix + daytimeID + "'] + label div span").prop("title", newTitle);
                // Make sure all checkboxes for series are synced on check
                $("[id^='" + cbIDPrefix + daytimeID + "']").prop("checked", checked);
                break;
            case "daypart":
                cbIDPrefix = "daypartCB_";
                //var daypartID = id.slice(0, id.indexOf("_"));
                var daypartID = id;
                checked = $("#" + cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var daypartSeries = IsSeriesInChart(daypartID);
                // Toggle visibility in chart
                daypartSeries.update({
                    visible: checked
                });
                // Update title and color based on if checked
                var newColor = checked ? daypartSeries.color : "transparent";
                var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                $("[id^='" + cbIDPrefix + daypartID + "'] + label div span").css("background-color", newColor);
                $("[id^='" + cbIDPrefix + daypartID + "'] + label div span").prop("title", newTitle);
                // Make sure all checkboxes for series are synced on check
                $("[id^='" + cbIDPrefix + daypartID + "']").prop("checked", checked);
                break;
            case "sources":
                cbIDPrefix = "#sourceCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var sourceSeries = IsSeriesInChart(id);
                // Toggle visibility in chart
                sourceSeries.update({
                    visible: checked
                });
                // Update title and color based on if checked
                var newColor = checked ? sourceSeries.color : "transparent";
                var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                break;
            case "market":
                cbIDPrefix = "#marketCB_";
                checked = $(cbIDPrefix + id).prop("checked");
                // Get series of toggle id
                var marketSeries = IsSeriesInChart(id);
                // Toggle visibility in chart
                marketSeries.update({
                    visible: checked
                });
                // Update title and color based on if checked
                var newColor = checked ? marketSeries.color : "transparent";
                var newTitle = checked ? "select to remove from graph" : "select to add to graph";

                $(cbIDPrefix + id + " + label div span").css("background-color", newColor);
                $(cbIDPrefix + id + " + label div span").prop("title", newTitle);
                break;
            default:
                break;
        }
    }

    if (_NumOverlaySeries > 0 && IsSeriesInChart(id) !== null) {
        // If an overlay is active, reload  it                
        var item = $("a[name='primaryOverlayNav'].active");
        if (item != null) {
            RemoveChartOverlay();
            eval($(item).attr("href"));
        }
    }
}

function SetDefaultDateRange() {
    if (_PageType != "campaign") {
        $("#dpDateFrom").datepicker("setDate", -30);
        $("#dpDateTo").datepicker("setDate", Date.now());

        $("#dpDateFromFilter0").datepicker("setDate", -30);
        $("#dpDateFromFilter0").data("prevDate", $("#dpDateFromFilter0").datepicker("getDate"));
        $("#dpDateToFilter0").datepicker("setDate", Date.now());
        $("#dpDateToFilter0").data("prevDate", $("#dpDateToFilter0").datepicker("getDate"));

        $("#dpDateFromFilter1").datepicker("setDate", -30);
        $("#dpDateFromFilter1").data("prevDate", $("#dpDateFromFilter1").datepicker("getDate"));
        $("#dpDateToFilter1").datepicker("setDate", Date.now());
        $("#dpDateToFilter1").data("prevDate", $("#dpDateToFilter1").datepicker("getDate"));

        GetDateRange();

        $("#linkOpenDateRange").text("trailing 30 days");
    }
    else {
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

function ClearAgentCheckBoxes() {
    var cbs = $("[id^='agentCB_']:checked");

    for (var i = 0; i < cbs.length; i++)
    {
        cbs[i].checked = false;
    }
}

// Change the date range for a specific agent in _searchAgents
function ChangeAgentDateRange(agentID, dateFrom, dateTo) {
    for (var i = 0; i < _searchAgents.length; i++)
    {
        if (_searchAgents[i].ID == agentID)
        {
            _searchAgents[i].DateFrom = new Date(dateFrom);
            _searchAgents[i].DateTo = new Date(dateTo);
        }
    }
}

// Used to add multiple filter agents to search
function AddFilterAgentsToSearch() {
//    console.log("Add Filter Agents to Search");
    var agentCompareDiv = $("#divAgentFilter1");

    // Clear current search agents to add only filter agents, both of them
    _searchAgents = [];

    // Add first agent to filter
    var filterAgentID0 = $("#ddAgentFilter0").children().filter(":selected").val();
    var filterSource0 = $("#ddSourceFilter0").children().filter(":selected").val();
    var filterDateFrom0 = new Date($("#dpDateFromFilter0").datepicker("getDate"));
    var filterDateTo0 = new Date($("#dpDateToFilter0").datepicker("getDate"));

//    console.log("dpDateFromFilter0.getDate " + $("#dpDateFromFilter0").datepicker("getDate"));
//    console.log("dpDateToFilter0.getDate " + $("#dpDateToFilter0").datepicker("getDate"));
//    console.log("filterFromDate0: " + filterDateFrom0.toLocaleDateString());
//    console.log("filterToDate0: " + filterDateTo0.toLocaleDateString());

    if (filterAgentID0 != "null")
    {
//        console.log("filterAgentID0 != null");
        AddFilterAgentToSearch(filterAgentID0, filterDateFrom0, filterDateTo0, filterSource0);
    }

    // Add second agent if not hidden
    if (!agentCompareDiv.hasClass("hidden"))
    {
        var filterAgentID1 = $("#ddAgentFilter1").children().filter(":selected").val();
        var filterSource1 = $("#ddSourceFilter1").children().filter(":selected").val();
        var filterDateFrom1 = new Date($("#dpDateFromFilter1").datepicker("getDate"));
        var filterDateTo1 = new Date($("#dpDateToFilter1").datepicker("getDate"));

//        console.log("filterFromDate1: " + filterDateFrom1.toLocaleDateString());
//        console.log("filterToDate1: " + filterDateTo1.toLocaleDateString());

        if (filterAgentID1 != "null")
        {
            AddFilterAgentToSearch(filterAgentID1, filterDateFrom1, filterDateTo1, filterSource1);

            if (filterDateFrom1.getTime() !== filterDateFrom0.getTime())
            {
                _graphType = "comparison";
            }

            if (filterDateTo1.getTime() !== filterDateTo0.getTime())
            {
                _graphType = "comparison";
            }
        }
    }
}

// Only adds a single specified filter agent and date range to search
function AddFilterAgentToSearch(agentID, dateFrom, dateTo, source) {
//    console.group("AddFilterAgentToSearch");
//    console.log("-Parameters");
//    console.log("agentID: " + agentID);
//    console.log("dateFrom: " + dateFrom);
//    console.log("dateTo: " + dateTo);
//    console.log("source: " + source);
//    console.groupEnd();
    // Assuming agent is not already in filter
    var agent = new Object();
    agent.ID = agentID;
    agent.DateFrom = dateFrom;
    agent.DateTo = dateTo;
    agent.MediaType = source;

    _searchAgents.push(agent);
}

// Will only add/remove one agent to/from _searchAgents global
function AddAgentToSearch(agentID) {
    GetDateRange();
    // Temp array to use in agent removal
    var tempAgents = [];

    var newAgentInArray = false;
    // Check if agent already searched
    for (var i = 0; i < _searchAgents.length; i++)
    {
        // Agent is not already in search array
        if (_searchAgents[i].ID != agentID)
        {
            // Add agent to tempArray of agents
            tempAgents.push(_searchAgents[i]);
        }
        // Agent is already searched
        else
        {
            // Do not add agent to temp array, i.e. remove it from search
            newAgentInArray = true;
        }
    }

    // If agent was not already in array, i.e. searched
    if (!newAgentInArray) {
        var filterAgent = $.grep(_FilterSetAgentIDs, function (obj, index) {
            return obj.ID == agentID;
        });
        _IsNewSearch = true;    // Is new search
        // Create a new agent object. If the agent is set as a filter agent, use the filter information
        var agent = new Object();
        agent.ID = agentID;
        if (filterAgent.length == 0) {
            agent.DateFrom = _dateFrom;
            agent.DateTo = _dateTo;
            agent.MediaType = _subMediaType;
        }
        else {
            agent.DateFrom = filterAgent[0].DateFrom;
            agent.DateTo = filterAgent[0].DateTo;
            agent.MediaType = _subMediaType;
        }

        // Add agent to temp array
        tempAgents.push(agent);
    }

    // Clear old _searchAgents array
    _searchAgents = [];

    // Set global to temp array, should toggle agents depending on prior presence
    for (var i = 0; i < tempAgents.length; i++)
    {
        _searchAgents.push(tempAgents[i]);
    }
}

function LoadMap() {
    // Check if map functionality is disabled
    if (!$("#primaryChartNav2").hasClass("inactive")) {
        //US Map ID is 2
        $('#liMap2 a').click();
    }
}

function LoadDemographicChart(element) {
    $("#demoShortNav li").removeClass("active");
    $(element).addClass("active");

    var subTab = $("#demoShortNav > li.active").attr("eleVal");

    // Check all demographic cbs - new chart created on each switch between gender/age
    $("[id^='demographicCB_']").prop("checked", true);

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

    LoadSelectedChart();
}

function ToggleIconStatusByGroup(lstIcons, isEnabled) {
    $.each(lstIcons, function (index, obj) {
        ToggleIconStatus($(obj).prop("id"), isEnabled);
    });
}

function ToggleIconStatus(iconID, isEnabled) {
    if (isEnabled) {
        $("#" + iconID).css("cursor", "auto");
        $("#" + iconID).prop('onclick', null).off('click');
        $("#" + iconID).removeClass("inactive");
    }
    else {
        $("#" + iconID).css("cursor", "not-allowed");
        $("#" + iconID).click(function (e) { e.preventDefault(); });
        $("#" + iconID).addClass("inactive");
    }
}

function ToggleActiveIconsForPESHFilters(isEnabled) {
    if (isEnabled) {
        if (_tab != "Daytime" && _tab != "Daypart") {
            // Map Chart are permanently disabled in Daytime & Daypart tab.
            if (_PageType != "campaign") {
                // Map Chart is permanently disabled in Campaign view
                ToggleIconStatus("primaryChartNav2", isEnabled);
            }

            // Overlays are permanently disabled in Daytime & Daypart tab. Otherwise, enable all except Google
            ToggleIconStatusByGroup($('a[name="primaryOverlayNav"]').not('[id="primaryOverlayNav1"]'), isEnabled);
        }
    }
    else {
        ToggleIconStatus("primaryChartNav2", isEnabled);

        // Disable all overlays except Google
        ToggleIconStatusByGroup($('a[name="primaryOverlayNav"]').not('[id="primaryOverlayNav1"]'), isEnabled);
    }
}

function ClearPrimaryNavFilters() {
    // If all is clicked while already active, do nothing
    if (_ActivePESHTypes.length > 0 || _ActiveSourceGroups.length > 0) {
        _ActivePESHTypes = [];
        _ActiveSourceGroups = [];

        // Attach a done, fail, and progress handler for the asyncEvent
        $.when(LoadSelectedChart()).then(
            function (status) {
                $("#primaryNavBar a").removeClass("active");
                $("#primaryNavAll").addClass("active");

                ToggleActiveIconsForPESHFilters(true);
            },
            function (status) {
            },
            function (status) {
            }
        );
    }

    $("#primaryNavAll").trigger("blur");
}

function ToggleSourceGroupFilter(eleVal) {
    var index = $.inArray(eleVal, _ActiveSourceGroups);
    if (index == -1) {
        _ActiveSourceGroups.push(eleVal);
    }
    else {
        _ActiveSourceGroups.splice(index, 1);
    }

    RemoveChartOverlay();

    // Attach a done, fail, and progress handler for the asyncEvent
    $.when(LoadSelectedChart()).then(
        function (status) {
            $("a[name='navSourceGroup']").removeClass("active");
            $.each(_ActiveSourceGroups, function (index, obj) {
                $("#primaryNav" + obj).addClass("active");
            });

            if (_ActivePESHTypes.length == 0 && _ActiveSourceGroups.length == 0) {
                $("#primaryNavAll").addClass("active");

                ToggleActiveIconsForPESHFilters(true);
            }
            else {
                $("#primaryNavAll").removeClass("active");

                // Disable maps and audience/media value overlays if any filter is selected
                ToggleActiveIconsForPESHFilters(false);
            }
        },
        function (status) {
        },
        function (status) {
        }
    );

    $("#primaryNav" + eleVal).trigger("blur");
}

function TogglePESHTypeFilter(eleVal) {
    var index = $.inArray(eleVal, _ActivePESHTypes);
    if (index == -1) {
        _ActivePESHTypes.push(eleVal);
    }
    else {
        _ActivePESHTypes.splice(index, 1);
    }

    RemoveChartOverlay();

    // Attach a done, fail, and progress handler for the asyncEvent
    $.when(LoadSelectedChart()).then(
        function (status) {
            $("a[name='navPESHType']").removeClass("active");
            $.each(_ActivePESHTypes, function (index, obj) {
                $("#primaryNav" + obj).addClass("active");
            });

            if (_ActivePESHTypes.length == 0 && _ActiveSourceGroups.length == 0) {
                $("#primaryNavAll").addClass("active");

                ToggleActiveIconsForPESHFilters(true);
            }
            else {
                $("#primaryNavAll").removeClass("active");

                // Disable maps and audience/media value overlays if any filter is selected
                ToggleActiveIconsForPESHFilters(false);
            }
        },
        function (status) {
        },
        function (status) {
        }
    );

    $("#primaryNav" + eleVal).trigger("blur");
}

function LoadSelectedChart() {
    return LoadPrimaryChart(_SelectedChartType, _SelectedChartOverlayEnabled, _SelectedChartIsMap);
}

// Before passing agents to controller, convert start/end dates to strings and remove TZ info to prevent auto TZ conversions on de-serialization
function ConvertAgentDatesToStrings() {
    //console.log("ConvertAgentDatesToString()");
    for (var i = 0; i < _searchAgents.length; i++)
    {
        var startDate = new Date(_searchAgents[i].DateFrom);
        var endDate = new Date(_searchAgents[i].DateTo);
        //console.log("BEFORE agent " + _searchAgents[i].ID + " start " + startDate.toISOString() + " end " + endDate.toISOString());
        _searchAgents[i].DateFrom = startDate.toISOString().substr(0, 10);
        _searchAgents[i].DateTo = endDate.toISOString().substr(0, 10);
        //console.log("AFTER agent " + _searchAgents[i].ID + " start " + _searchAgents[i].DateFrom + " end " + _searchAgents[i].DateTo);
    }
}

function LoadPrimaryChart(chartType, isOverlayEnabled, isMap) {
    var deferred = $.Deferred();
    _SelectedChartType = chartType;
    var gRequest = new Object();
    gRequest.GraphType = _graphType;
    gRequest.Tab = _tab;
    gRequest.DateInterval = _dateInterval;
    ConvertAgentDatesToStrings();
    gRequest.AgentList = _searchAgents;
    gRequest.PageType = _PageType;

    if (_tab == "Demographic")
    {
        if (_PageType != "campaign")
        {
            gRequest.GraphType = "line";
            _graphType = "line";
        }
        gRequest.SubTab = $("#demoShortNav>li.active").attr("eleVal");
        //gRequest.SubTab = "gender";
    }

    var jsonPostData = {
        graphRequest: gRequest,
        subMediaType: _tab == "Daypart" ? "TV" : _subMediaType,
        PESHTypes: _ActivePESHTypes,
        sourceGroups: _ActiveSourceGroups,
        chartType: chartType,
        pageType: _PageType,
        isNewSearch: _IsNewSearch
    };

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlMediaJsonChart,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                _IsNewSearch = false;
                _SelectedChartIsMap = isMap;

                $("a[name='primaryChartNav']").removeClass("active");
                if (chartType == 3) {
                    // The US and Canada maps share an icon
                    $("#primaryChartNav2").addClass("active");
                }
                else {
                    $("#primaryChartNav" + chartType).addClass("active");
                }

                _IsOverlayEnabled = isOverlayEnabled;
                if (!isOverlayEnabled) {
                    RemoveChartOverlay();
                    ToggleIconStatusByGroup($('a[name="primaryOverlayNav"]'), false);
                }
                else {
                    ToggleIconStatusByGroup($('a[name="primaryOverlayNav"]:not([name="primaryOverlayNav1"])'), true);
                    if (_PageType == "campaign") {
                        // Google overlay data is permanently disabled in Campaign view
                        ToggleIconStatus("primaryOverlayNav1", false);
                    }

                    if (_tab == "Demographic") {
                        // Growth currently disabled in demographic tab
                        ToggleIconStatus("primaryOverlayNav2", false);
                        ToggleIconStatus("primaryChartNav8", false);
                    }

                    if (_tab == "Sources") {
                        ToggleIconStatus("primaryOverlayNav1", false);
                        ToggleIconStatus("primaryOverlayNav2", false);
                        ToggleIconStatus("primaryOverlayNav3", false);
                    }
                }

                $("#divPrimaryChart").html('');

                if (result.jsonMediaRecord.length > 0) {
                    switch (chartType) {
                        case 1: // Multi-line Chart
                            RenderHighChartsLineChart(result.jsonMediaRecord);
                            break;
                        case 2: // US Map
                            RenderDmaMapChart(result.jsonMediaRecord);
                            break;
                        case 3: // Canada Map
                            RenderCanadaMapChart(result.jsonMediaRecord);
                            break;
                        case 4: // Pie Chart
                            RenderHighChartsPieChart(result.jsonMediaRecord);
                            break;
                        case 5: // Bar Chart
                        case 6: // Column Chart
                            RenderHighChartsColumnOrBarChart(result.jsonMediaRecord);
                            break;
                        case 7: // Daytime Heat Map
                            RenderHighChartsDaytimeHeatMap(result.jsonMediaRecord);
                            break;
                        case 8: // Growth Chart
                            RenderHighChartsLineChart(result.jsonMediaRecord);
                            break;
                        case 9: // Daypart Heat Map
                            RenderHighChartsDaypartHeatMap(result.jsonMediaRecord);
                            break;
                    }

                    if (!isMap) {
                        SetChartSeriesIDs();
                        newSetCBColors();
                        FormatLegend();
                    }
                    else {
                        //SetChartSeriesIDs();
                        //newSetCBColors();

                        // If map, destroy old HC chart - prevents HC chart from suddenly appearing on window resize when viewing map
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
                                    // Possible to have HC undefined for divPrimaryChart if added an overlay to chart and already destroyed chart in divPrimary
                                    // since overlays live in a different div - having overlays present in this div does not pose issues with resizing (unlike divPrimary)
                                    console.log(err);
                                }
                            }
                        }
                    }
                }
                else {
                    $("#divPrimaryChart").html('<div class="chartNoData">No Data Available<br/><br/>Please Select At Least One Agent</div>');
                }

                // Update NavTab values
                $("#primaryNavAll_Results").text(result.hitsTotal);
                $("#primaryNavOnAir_Results").text(result.onAirTotal);
                $("#primaryNavOnline_Results").text(result.onlineTotal);
                $("#primaryNavPrint_Results").text(result.printTotal);
                $("#primaryNavRead_Results").text(result.readTotal);

                if (result.isLRAccess) {
                    $("#primaryNavSeen_Results").text(result.seenTotal);
                    $("#primaryNavHeard_Results").text(result.heardTotal);
                }
                if (result.isAdsAccess) {
                    $("#primaryNavPaid_Results").text(result.paidTotal);
                    $("#primaryNavEarned_Results").text(result.earnedTotal);
                }

                // If an overlay is active, reenable it                
                var item = $("a[name='primaryOverlayNav'].active");
                if (item != null) {
                    _NumOverlaySeries = 0;
                    _ActiveOverlayType = null;
                    eval($(item).attr("href"));
                }

                deferred.resolve("success");
            }
            else {
                deferred.reject("fail");
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (result) {
            deferred.reject("fail");
            ShowNotification(_msgErrorOccured);
        }
    });

    if (isMap) {
        $("#mapShortNav>li.active").removeClass("active");
        $("#liMap" + chartType).attr("class", "active");

        $("#mapShortNav").show();
    }
    else {
        $("#mapShortNav").hide();
    }

    return deferred.promise();
}

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

function ToggleOverlayData(overlayType) {
    if (_IsOverlayEnabled) {
        var chartMain = $("#divPrimaryChart").highcharts();

        if ((_ActiveOverlayType !== overlayType)) {
            var gRequest = new Object();
            gRequest.GraphType = _graphType;
            gRequest.DateInterval = _dateInterval;
            gRequest.AgentList = _searchAgents;
            gRequest.Tab = _tab;

            if (_tab == "Demographic")
            {
                gRequest.SubTab = $("#demoShortNav>li.active").attr("eleVal");
            }

            var jsonPostData = {
                overlayType: overlayType,
                graphRequest: gRequest,
                pageType: _PageType,
                fromDate: _dateFrom,
                toDate: _dateTo,
                PESHTypes: _ActivePESHTypes,
                sourceGroups: _ActiveSourceGroups
            }

            $.ajax({
                type: "POST",
                dataType: "json",
                url: _urlGetOverlayData,
                contentType: "application/json; charset=utf-8",
                data: JSON.stringify(jsonPostData),
                success: function (result) {
                    if (result.isSuccess)
                    {
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
                                case 4:
                                    yAxisName = "Growth";
                                    break;
                            }

                            chartMain.addAxis({
                                id: 'oppAxis',
                                title: {
                                    text: yAxisName
                                },
                                min: 0,
                                minRange: 0.1,
                                gridLineWidth: 0,
                                opposite: true
                            });

                            // For each series in overlay chart
                            $.each($("#divOverlayChart").highcharts().series, function (indexS, series) {
                                var seriesName = series.name;
                                var newSeries = new Array();
                                var seriesColor = null;

                                // For Audience and Media Value overlays, set the overlay series color to the same one as the main agent series
                                if (overlayType != 1)
                                {
                                    // Get array of series in main chart (obj)
                                    var mainSeries = $.grep(chartMain.series, function (obj, i) {
                                        // Match on agent ID
                                        return obj.data.length > 0 && series.data[0].Value == obj.data[0].Value;
                                    });

                                    // If only one main series set color of new series to that color in main series
                                    if (mainSeries.length == 1)
                                    {
                                        seriesColor = mainSeries[0].color;
                                    }
                                }

                                $.each(series.data, function (indexD, data) {
                                    newSeries.push({
                                        y: data.y,
                                        category: data.category,
                                        Date: data.Date
                                    });
                                });

                                _NumOverlaySeries += 1;
                                chartMain.addSeries({
                                    name: seriesName,
                                    data: newSeries,
                                    yAxis: 'oppAxis',
                                    color: seriesColor,
                                    dashStyle: 'shortdash',
                                    showInLegend: true // Only want to add overlays to label
                                });
                            });

                            _ActiveOverlayType = overlayType;
                            $("#primaryOverlayNav" + overlayType).addClass("active");
                        }
                    }
                    else
                    {
                        ShowNotification(_msgErrorOccured);
                    }
                },
                error: function (result) {
                    ShowNotification(_msgErrorOccured);
                }
            });        
        }
        else {
            RemoveChartOverlay();
        }        
    }
}

function RemoveChartOverlay() {
    if (_ActiveOverlayType != null) {
        var chartMain = $("#divPrimaryChart").highcharts();

        // Remove the active overlay series and axis. They will always be the most recently added.
        for (var i = 0; i < _NumOverlaySeries; i++) {
            chartMain.series[chartMain.series.length - 1].remove();
        }
        chartMain.yAxis[1].remove();
    }

    $("a[name='primaryOverlayNav']").removeClass("active");
    _ActiveOverlayType = null;
    _NumOverlaySeries = 0;
}

// Used to display tooltips for Fusion maps
var x, y, zInterval;
var Interval = 0;
document.onmousemove = setMouseCoords;

function setMouseCoords(e) {
    if (document.all) {
        tooltipx = window.event.clientX;
        tooltipy = window.event.clientY + 600;
    } else {
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
    if (Interval != 1000) {
        document.getElementById("mapToolTip").style.display = "none";
        clearInterval(zInterval);
        Interval = 0;
    }
}
// End Fusion tooltips

function CloseIFramePopup() {
    $("#divFeedsPage").css("display", "none");
    $("#divFeedsPage").modal("hide");
    $("#iFrameFeeds").attr("src", "");
}

function OpenFeed() {
    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlOpenIFrame,
        contentType: "application/json; charset=utf-8",
        success: OpenFeedSuccess,
        error: function () {
            console.error("OpenFeed error");
        }
    });

    var startDate = new Date(this.category);
    var mediumQS = "";
    if (_subMediaType != null) {
        mediumQS = '&medium=["' + _subMediaType + '"]&mediumDesc=["' + _subMediaType + '"]';
    }
    else {
        var subMediaTypes = [];
        var isAllTypes = true;

        if ($.inArray("OnAir", _ActiveSourceGroups) > -1) {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "OnAir";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if ($.inArray("Online", _ActiveSourceGroups) > -1) {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "Online";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if ($.inArray("Print", _ActiveSourceGroups) > -1) {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "Print";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if ($.inArray("Read", _ActivePESHTypes) > -1) {
            $.each($.grep(_MasterMediaTypes, function (obj) {
                return obj[0] == "Print" || obj[0] == "Online";
            }), function (index, obj) {
                subMediaTypes.push(obj[1]);
            });
            isAllTypes = false;
        }

        if (!isAllTypes) {
            mediumQS = '&medium=["' + subMediaTypes.join('","') + '"]&mediumDesc=["' + subMediaTypes.join('","') + '"]';
        }
    }

    if ($.inArray("Heard", _ActivePESHTypes) > -1) {
        mediumQS += '&heard=true';
    }
    if ($.inArray("Seen", _ActivePESHTypes) > -1) {
        mediumQS += '&seen=true';
    }
    if ($.inArray("Paid", _ActivePESHTypes) > -1) {
        mediumQS += '&paid=true';
    }
    if ($.inArray("Earned", _ActivePESHTypes) > -1) {
        mediumQS += '&earned=true';
    }

    $("#iFrameFeeds").attr(
        "src",
        "//" + window.location.hostname + "/Feeds?date=" + (startDate.getMonth() + 1) + "/" + startDate.getDate() + "/" + startDate.getFullYear() + '&isShowMTChart=true&searchrequest=["' + this.Value + '"]&searchrequestDesc=' + encodeURIComponent('["' + this.SearchTerm.split('+').join(' ') + '"]') + mediumQS
    );

    $("#divFeedsPage").css("height", documentHeight - 200);
    $("#iFrameFeeds").css("height", documentHeight - 200);
}

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

function FormatComparisonTooltip() {
    var header = "";
    if (_PageType == "campaign")
    {
        header = "Campaign Day: ";
    }
    else
    {
        header = "Comparison Day: ";
    }
    header += this.point.x + "<br />";
    if (_tab == "OverTime")
    {
        header = header + "Calendar Day: " + this.point.Date + "<br />";
    }

    var point = "<span style=\"color:" + this.point.color + ";\">\u25CF </span>" + this.series.name + ": <b>";

    if (_SelectedChartType == 8) // Growth chart
    {
        // Limit growth to only display two decimal points
        point += this.point.y.toFixed(2) + "%";
    }
    else
    {
        point += this.point.y;
    }

    return header + point + "</b><br/>";
}

function FormatDaytimeTooltip() {
    return this.point.value;
}

function FormatDaypartTooltip() {
    var item = "<b>" + this.point.name + "</b><br/><span>" + this.point.value + "</span>";
    return item;
}

function FormatDaypartDataLabel() {
    return this.point.code;
}

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
    //set default enable and hidden properties
    if ($("#primaryChartNav1").hasClass("inactive") == false) $("#primaryChartNav1").addClass("inactive");
    if ($("#primaryChartNav2").hasClass("inactive") == false) $("#primaryChartNav2").addClass("inactive");
    if ($("#primaryChartNav4").hasClass("inactive") == false) $("#primaryChartNav4").addClass("inactive");
    if ($("#primaryChartNav5").hasClass("inactive") == false) $("#primaryChartNav5").addClass("inactive");
    if ($("#primaryChartNav6").hasClass("inactive") == false) $("#primaryChartNav6").addClass("inactive");
    if ($("#primaryChartNav7").hasClass("inactive") == false) $("#primaryChartNav7").addClass("inactive");
    if ($("#primaryChartNav8").hasClass("inactive") == false) $("#primaryChartNav8").addClass("inactive");
    if ($("#primaryChartNav9").hasClass("inactive") == false) $("#primaryChartNav9").addClass("inactive");
    if ($("#primaryChartNav1").hasClass("displayNoneImp")) { $("#primaryChartNav1").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav2").hasClass("displayNoneImp")) { $("#primaryChartNav2").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav4").hasClass("displayNoneImp")) { $("#primaryChartNav4").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav5").hasClass("displayNoneImp")) { $("#primaryChartNav5").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav6").hasClass("displayNoneImp")) { $("#primaryChartNav6").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav7").hasClass("displayNoneImp")) { $("#primaryChartNav7").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav8").hasClass("displayNoneImp")) { $("#primaryChartNav8").removeClass("displayNoneImp"); }
    if ($("#primaryChartNav9").hasClass("displayNoneImp")) { $("#primaryChartNav9").removeClass("displayNoneImp"); }
    SetPESHFiltersEnabledDisabled(false);

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
    var isPESH = $("#primaryNavAll").hasClass("active") == false;
    var isMap = $("#primaryChartNav2").hasClass("active");
    var isLineChart = $("#primaryChartNav1").hasClass("active") || $("#primaryChartNav8").hasClass("active");
    var isOtherChart = $("#primaryChartNav1").hasClass("active") == false && $("#primaryChartNav8").hasClass("active") == false;
    $.each(_ActiveElements, function (index, value) {
        if (value.ActivePage.toLowerCase() == _PageType.toLowerCase()) {
            //LineChart
            if (value.ElementSelector == "Line Chart") {
                SetIconEnabledDisabledHidden("primaryChartNav1", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //MapChart
            if (value.ElementSelector == "Map Chart") {
                SetIconEnabledDisabledHidden("primaryChartNav2", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //PieChart
            if (value.ElementSelector == "Pie Chart") {
                SetIconEnabledDisabledHidden("primaryChartNav4", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //BarChart
            if (value.ElementSelector == "Bar Chart") {
                SetIconEnabledDisabledHidden("primaryChartNav5", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //ColumnChart
            if (value.ElementSelector == "Column Chart") {
                SetIconEnabledDisabledHidden("primaryChartNav6", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //HeatMap
            if (value.ElementSelector == "Heatmap") {
                SetIconEnabledDisabledHidden("primaryChartNav7", value, isPESH, isMap, isLineChart, isOtherChart);
                SetIconEnabledDisabledHidden("primaryChartNav9", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //GrowthChart
            if (value.ElementSelector == "Growth Chart") {
                SetIconEnabledDisabledHidden("primaryChartNav8", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //GoogleOverlay
            if (value.ElementSelector == "Google Overlay") {
                SetIconEnabledDisabledHidden("primaryOverlayNav1", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //AudienceOverlay
            if (value.ElementSelector == "Audience Overlay") {
                SetIconEnabledDisabledHidden("primaryOverlayNav2", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //MediaValueOverlay
            if (value.ElementSelector == "Media Value Overlay") {
                SetIconEnabledDisabledHidden("primaryOverlayNav3", value, isPESH, isMap, isLineChart, isOtherChart);
            }

            //PESH Filters
            if (value.ElementSelector == "PESH Filters") {
                if (value.ActiveTabs.indexOf(_tab) > -1 && (!isPESH || (isPESH && value.IsActiveWithPESH)) && (!isMap || (isMap && value.IsActiveWithMaps)) && (!isLineChart || (isLineChart && value.IsActiveWithLineCharts)) && (!isOtherChart || (isOtherChart && value.IsActiveWithOtherCharts))) {
                    SetPESHFiltersEnabledDisabled(true);
                }
                else {
                    SetPESHFiltersEnabledDisabled(false);
                }
            }
        }
    });
}
function SetIconEnabledDisabledHidden(name, value, isPESH, isMap, isLineChart, isOtherChart) {
    if (value.ActiveTabs.indexOf(_tab) > -1 && (!isPESH || (isPESH && value.IsActiveWithPESH)) && (!isMap || (isMap && value.IsActiveWithMaps)) && (!isLineChart || (isLineChart && value.IsActiveWithLineCharts)) && (!isOtherChart || (isOtherChart && value.IsActiveWithOtherCharts))) {
        if ($("#" + name).hasClass("inactive")) {
            $("#" + name).removeClass("inactive");
        }
    }
    else {
        $("#" + name).addClass("inactive");
    }

    if (value.HiddenTabs.indexOf(_tab) > -1) {
        if ($("#" + name).hasClass("displayNoneImp") == false) { $("#" + name).addClass("displayNoneImp"); }
    }
}
function SetPESHFiltersEnabledDisabled(isEnabled) {
    // Setting the on click function multiple times will cause the event to fire once for each time it was added.
    // To prevent this, always unbind the event beforehand.
    $("a[name='navPESHType']").off('click');
    $("a[name='navSourceGroup']").off('click');

    if (!isEnabled) {
        $("a[name='navPESHType']").css('cursor', 'not-allowed');
        $("a[name='navSourceGroup']").css('cursor', 'not-allowed');
    }
    else {
        // Adds function to activate PESH filter buttons in primary nav bar
        $("a[name='navPESHType']").click(function () {
            TogglePESHTypeFilter($(this).attr("eleVal"));
        });
        $("a[name='navPESHType']").css('cursor', 'auto');

        // Adds function to activate source group filter buttons in primary nav bar
        $("a[name='navSourceGroup']").click(function () {
            ToggleSourceGroupFilter($(this).attr("eleVal"));
        });
        $("a[name='navSourceGroup']").css('cursor', 'auto');
    }
}
//*********************************************************** END CONFIGURATION  ********************************************************************

//************************************************************* AMPLIFICATION  **********************************************************************
function ChangeAgent(ddlAgent, dpFromID, dpToID, ddlAgentOtherID) {
    $("#" + ddlAgentOtherID + " option").removeAttr("disabled");
    $("#" + ddlAgentOtherID + " option").removeClass("disabledOption");

    if ($(ddlAgent).val() === "null") {
        $("#" + dpFromID).attr("disabled", "disabled");
        $("#" + dpToID).attr("disabled", "disabled");
    }
    else {
        $("#" + dpFromID).removeAttr("disabled");
        $("#" + dpToID).removeAttr("disabled");

        var option = $("#" + ddlAgentOtherID + " option[value='" + $(ddlAgent).val() + "']");
        $(option).attr("disabled", "disabled");
        $(option).addClass("disabledOption");
    }
}

//*********************************************************** END AMPLIFICATION  ********************************************************************

//************************************************************* CAMPAIGN  **********************************************************************
function ChangeCampaign(ddlCampaign, dpFromID, dpToID, ddlCampaignOtherID) {
    $("#" + ddlCampaignOtherID + " option").removeAttr("disabled");
    $("#" + ddlCampaignOtherID + " option").removeClass("disabledOption");

    if ($(ddlCampaign).val() == "null") {
        $("#" + dpFromID).val("");
        $("#" + dpToID).val("");
    }
    else {
        var selected = $(ddlCampaign).find(":selected");
        $("#" + dpFromID).val($(selected).attr("startdate"));
        $("#" + dpToID).val($(selected).attr("enddate"));

        var option = $("#" + ddlCampaignOtherID + " option[value='" + $(ddlCampaign).val() + "']");
        $(option).attr("disabled", "disabled");
        $(option).addClass("disabledOption");
    }
}

//*********************************************************** END CAMPAIGN  ********************************************************************

// New Secondary Functions //
function GetSecondaryTables() {
    GetDateRange();
    var jsonPostData = {
        tab: _tab,
        selectedAgents: _searchAgents,
        // Remove TimeZone information from date time strings
        dateFrom: _dateFrom == null ? null : new Date(_dateFrom).toISOString().substr(0, 10),
        dateTo: _dateTo == null ? null : new Date(_dateTo).toISOString().substr(0, 10),
        pageType: _PageType,
        interval: _dateInterval,
        subMediaType: _tab == "Daypart" ? "TV" : _subMediaType
    }

    $.ajax({
        type: "POST",
        dataType: "json",
        url: _urlGetSecondaryTables,
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            // Set secondary tabs
            $("#bottomShortNav2").html(result.tableTabs);

            var htmlTableString = "";
            for (var i = 0; i < result.tables.length; i++)
            {
                htmlTableString += result.tables[i];
            }

            $("#AnalyticsSecondaryDetails").html(htmlTableString);

            var selectorPrefix = "";

            if (_PageType == "campaign")
            {
                // Get all checked campaigns
                selectorPrefix = "campaignCB_";
            }
            else
            {
                selectorPrefix = "agentCB_";
            }

            // Get all checked agents/campaigns in _searchAgents to get proper selected agents to put data in graph
            _searchAgents = [];
            var filterAgent0ID = $("#ddAgentFilter0").children().filter(":selected").val();
            if (_isFilterActive && filterAgent0ID != "null")
            {
                // If actually filtering on specific agent and not just a source, only keep agents specifically filtered on - needed to preserve filter
                // comparison ability between two agents over different time period (AddAgentToSearch will set each agent date range from global which 
                // means graph is not comparison even if comparison is desired behavior)
                AddFilterAgentsToSearch();
            }
            else
            {
                // If not filtering to specific agents, _searchAgents should be the top x agents
                $("[id^='" + selectorPrefix + "']:checked").each(function (i, el) {
                    var agentID = Number(el.id.slice(el.id.indexOf('_') + 1));
                    AddAgentToSearch(agentID);
                });
            }

            _IsNewSearch = true;
            LoadSelectedChart();
        },
        error: function () {
            console.log("GetSecondaryTables ajax error");
        }
    });
}

function SwitchSecondaryTable(table) {
    // Hide all tables
    $("[id^='tbl_']").hide();

    // Show desired table
    $("#tbl_" + table).show();

    // Switch active bottom short nav - remove all classes then add to desired
    $("#bottomShortNav2").children().removeAttr("class");
    $("#bottomShortNav2").children(":contains('" + table + "')").addClass("active");
}

// Function that maps series to rows
function SetChartSeriesIDs() {
    var chartMain = $("#divPrimaryChart").highcharts();
    var series = chartMain.series;
    var chartID = $("#primaryChartNav > li > a.active").attr("id");
    var chartSelected = chartID.charAt(chartID.length - 1);
    var secondaryTab = $("#bottomShortNav > li.active").text().trim();

    if (chartSelected == 4) // Chart selected is a pie chart
    {
        var lstSeries = [];
        // Get Names and IDs from secondary
        var selectorPrefix = "";
        if (_PageType == "campaign")
        {
            selectorPrefix = "campaign";
        }
        else
        {
            selectorPrefix = "agent";
        }

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
        // Selected chart is a pie chart, not on the demographic tab
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

function newSetCBColors() {
    var chartID = $("#primaryChartNav > li > a.active").attr("id");
    var chartSelected = chartID.charAt(chartID.length - 1);
    var selectorPrefix = "";

    var lstSeries = [];

    // Should not show daytime CBs for this chart type - unable to toggle on/off series/data for this type
    if (chartSelected == 7 || chartSelected == 9) // Heatmap chart, only found in daytime and daypart
    {
        if (_tab == "Daytime")
        {
            $("[id^='daytimeCB_'] + label").css("display", "none"); 
        }
        if (_tab == "Daypart")
        {
            $("[id^='daypartCB_'] + label").css("display", "none");
        }
    }
    else
    {
        if (_tab == "Daytime")
        {
            $("[id^='daytimeCB_'] + label").css("display", "block");
        }
        if (_tab == "Daypart")
        {
            $("[id^='daypartCB_'] + label").css("display", "block");
        }
    }

    if (_SelectedChartIsMap) // Map selected
    {
        //console.log("Map Selected");
        // Map doesn't have series, color each checked agent gray
        $("[id^='agentCB_']:checked + label div span").css("background-color", "#6D6E71");
        $("[id^='agentCB_']:checked + label div span").prop("title", "select to remove from graph");
        return; // Do not proceed
    }

    // Get info about chart series
    $.each($("#divPrimaryChart").highcharts().series, function (indexS, series) {
        var seriesName = series.name;
        if (seriesName == "65+")
        {
            seriesName = "65\\+";   // jQuery doesn't play well with having '+' in id
        }

        if (chartSelected == 4) // Chart selected is a pie chart
        {
            // Unlike line, column, bar, or growth the pie chart will only ever have a single series with each pie section stored in data
            // Go through each datum and act as if they were the series
            $.each(series.data, function (indexD, datum) {
                lstSeries.push({
                    ID: datum.id,
                    name: datum.name,
                    color: datum.color
                });
            });
        }
        else
        {
            lstSeries.push({
                ID: series.options.id,
                name: series.name,
                color: series.color
            });
        }
    });

    // Color each agent/campaign checked
    $.each(_searchAgents, function (indexA, agent) {
        var seriesColor = "#6D6E71";

        // Only OverTime tab will color agentCBs from chart
        if (_tab == "OverTime")
        {
            // get color of agent series from agent ID
            $.each(lstSeries, function (indexS, series) {
                if (series.ID == agent.ID)
                {
                    seriesColor = series.color;
                    return false;   // Break out of loop
                }
            });
        }
        if (_PageType == "campaign")
        {
            selectorPrefix = "#campaignCB_";
        }
        else
        {
            selectorPrefix = "#agentCB_";
        }

        $(selectorPrefix + agent.ID + ":checked + label div span").css("background-color", seriesColor);
        $(selectorPrefix + agent.ID + ":checked + label div span").prop("title", "select to remove from graph");
    });

    selectorPrefix = "";
    switch (_tab)
    {
        case "OverTime":
            selectorPrefix = "#agentCB_";
            break;
        case "Sources":
            selectorPrefix = "#sourceCB_";
            break;
        case "Demographic":
            selectorPrefix = "#demographicCB_";
            break;
        case "Daytime":
            selectorPrefix = "daytimeCB_"
            break;
        case "Daypart":
            selectorPrefix = "daypartCB_"
            break;
        case "Market":
            selectorPrefix = "#marketCB_";
            break;
    }

    // Color each other color CB, specific to tab
    $.each(lstSeries, function (indexS, series) {
        if (_tab == "Daytime" || _tab == "Daypart")
        {
            // For daytime, multiple checkboxes will have same color since chart series grouped by day of week only
            $("[id^='" + selectorPrefix + series.ID + "']:checked + label div span").css("background-color", series.color);
            $("[id^='" + selectorPrefix + series.ID + "']:checked + label div span").prop("title", "select to remove from graph");
        }
        else
        {
            $(selectorPrefix + series.ID + ":checked + label div span").css("background-color", series.color);
            $(selectorPrefix + series.ID + ":checked + label div span").prop("title", "select to remove from graph");
        }
    });
}

//************************************************************* Chart Rendering **********************************************************************

function RenderHighChartsDaypartHeatMap(jsonChartData) {
    var start = new Date().getTime();
    var jsonChart = JSON.parse(jsonChartData);
    jsonChart.tooltip.formatter = FormatDaypartTooltip;
    jsonChart.series[0].dataLabels.formatter = FormatDaypartDataLabel;
    $('#divPrimaryChart').highcharts(jsonChart);
    var end = new Date().getTime();
    console.log("RenderHighChartsDaypartHeatMap: " + (end - start) + " ms");
}

function RenderHighChartsPieChart(jsonChartData) {
    var start = new Date().getTime();
    var jsonPieChart = JSON.parse(jsonChartData);
    jsonPieChart.chart.height = $("#divPrimaryChart").height();
    //jsonPieChart.chart.width = $("#divPrimaryChart").width();
    $("#divPrimaryChart").highcharts(jsonPieChart);
    // Call reflow to center pie chart in container (resizes svg to consume all container space) -- only type to not do this automatically
    $("#divPrimaryChart").highcharts().reflow();
    var end = new Date().getTime();
    console.log("RenderHighChartsPieChart: " + (end - start) + " ms");
}

function RenderHighChartsColumnOrBarChart(jsonChartData) {
    var start = new Date().getTime();
    var jsonChart = JSON.parse(jsonChartData);
    $("#divPrimaryChart").highcharts(jsonChart);
    var end = new Date().getTime();
    console.log("RenderHighChartsColumnOrBarChart: " + (end - start) + " ms");
}

function RenderHighChartsDaytimeHeatMap(jsonChartData) {
    var start = new Date().getTime();
    var jsonChart = JSON.parse(jsonChartData);
    jsonChart.tooltip.formatter = FormatDaytimeTooltip;
    $('#divPrimaryChart').highcharts(jsonChart);
    var end = new Date().getTime();
    console.log("RenderHighChartsDayTimeHeatMap: " + (end - start) + " ms");
}

function RenderHighChartsLineChart(jsonChartData) {
    var start = new Date().getTime();
    var jsonLineChart = JSON.parse(jsonChartData);

    jsonLineChart.legend.floating = true;
    if (_PageType != "campaign" && _tab != "Demographic")
    {
        // Demographic drilldown doesn't make sense
        // Campaign drilldown needs agent ID to work
        jsonLineChart.plotOptions.series.point.events.click = OpenFeed;
    }

    if (_graphType == "comparison")
    {
        jsonLineChart.tooltip.formatter = FormatComparisonTooltip;
    }

    _devJSONChart = jsonLineChart;
    $("#divPrimaryChart").highcharts(jsonLineChart);
    var end = new Date().getTime();
    console.log("RenderHighChartsLineChart: " + (end - start) + " ms");
}

function RenderDmaMapChart(jsonMapData) {
    var populationMap = new FusionCharts({
        type: 'maps/usadma',
        renderAt: 'divPrimaryChart',
        width: '100%',
        height: '350',
        dataFormat: 'json',
        dataSource: jsonMapData
    }).render();
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

function RenderCanadaMapChart(jsonMapData) {
    var populationMap = new FusionCharts({
        type: 'maps/canada',
        renderAt: 'divPrimaryChart',
        width: '100%',
        height: '350',
        dataFormat: 'json',
        dataSource: jsonMapData
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