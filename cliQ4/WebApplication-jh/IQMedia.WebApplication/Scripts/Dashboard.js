    var _FromDate = null;
var _ToDate = null;
var _Medium = '';
var _MediumDesc = '';
var _SearchType = 1;
var _IsDefaultLoad = true;
var CONST_ZERO = "0";
var _SearchRequests = [];
var _QueryNameList = [];
var _QueryNames = "";
var _OldSearchRequests = [];
var _SelectedDmas = new Array();
var _PieChartTotalHits = 0;
var _SelectedProvinces = new Array();

var _DmaChartColors = ["#BDD94E","#E61061"]

var _Months = ["Jan","Feb","Mar","Apr","May","June","July","Aug","Sept","Oct","Nov","Dec"];

$(document).ready(function () {
    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuDashboard').attr("class", "active");
    $("#dpFrom").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFrom').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            $('#dpFrom').focus();
            SetDateVariable();
        }
    });

    $("#dpTo").datepicker({
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpTo').val(dateText);
            SetDateVariable();
        },
        onClose: function (dateText) {
            $('#dpTo').focus();
            SetDateVariable();
        }
    });

    $("body").click(function(e) {
        if (e.target.id == "liSearchRequestFilter" || $(e.target).parents("#liSearchRequestFilter").size() > 0)
        {
            if ($('#ulSearchRequest').is(':visible')) {            
                $('#ulSearchRequest').hide();
            }
            else {
                $('#ulSearchRequest').show();
            }
        }
        else if ((e.target.id !== "liSearchRequestFilter" && e.target.id !== "ulSearchRequest"   && $(e.target).parents("#ulSearchRequest").size() <= 0) || e.target.id == "btnSearchRequest") { 
            $('#ulSearchRequest').hide();
        } 
    });

    if (getParameterByName("source") == "") {
        // Main Dashboard Page
        GetDataMediumWise('Overview','Overview');
        AddHighlightToMenu('Overview');
    }
    else {
        // Dashboard IFrame
        GetAdhocSummaryData();

        $("#dpFrom").css({"margin-top":"5px"});
        $("#dpTo").css({"margin-top":"5px"});
        $(".ui-datepicker-trigger").hide();
    }

    $('#divPrintableArea').css({ 'min-height': documentHeight - 100 });
});

$(window).resize(function () {
    if (screen.height >= 768) {
        $('#divPrintableArea').css({ 'min-height': documentHeight - 100 });
    }
});

function AddHighlightToMenu(mediaType) {



    RemoveHighlightFromMenu();
    if (mediaType == 'Overview') {
        $('#liOverview').addClass('highlightedli');
    }
    else if (mediaType == 'TV') {
        $('#liTV').addClass('highlightedli');
    }
    else if (mediaType == 'NM') {
        $('#liNM').addClass('highlightedli');
    }
    else if (mediaType == 'Blog') {
        $('#liBlog').addClass('highlightedli');
    }
    else if (mediaType == 'Forum') {

        $('#liForum').addClass('highlightedli');
    }
    else if (mediaType == 'SocialMedia') {
        $('#liSM').addClass('highlightedli');
    }
    else if (mediaType == 'TW') {
        $('#liTW').addClass('highlightedli');
    }
    else if (mediaType == 'Radio') {
        $('#liTM').addClass('highlightedli');
    }
    else if (mediaType == 'PM') {
        $('#liPM').addClass('highlightedli');
    }

}
function RemoveHighlightFromMenu() {
    $('#ulMenu li').each(function () {
        $(this).removeClass('highlightedli');
    });
}
function SetDateVariable() {

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        _IsDefaultLoad = false;
        $('#dpFrom').removeClass('warningInput');
        $('#dpTo').removeClass('warningInput');
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();

            // change the _SearchType
            // 0-hour 1-day 3-month
            if (DateValidation()) {
                //From Date
                var mdy = _FromDate.split('/');
                var dateFrom = new Date(mdy[2], mdy[0] - 1, mdy[1]);
                dateFrom.setMinutes(dateFrom.getMinutes() - dateFrom.getTimezoneOffset());

                //To Date
                mdy = _ToDate.split('/');
                var dateTo = new Date(mdy[2], mdy[0] - 1, mdy[1]);
                dateTo.setMinutes(dateTo.getMinutes() - dateTo.getTimezoneOffset());

                //Day diff
                var millisecondsPerDay = 24 * 60 * 60 * 1000;
                var dayDiff = Math.round((dateTo - dateFrom) / millisecondsPerDay);

                //Month diff
                var monthDiff = dateTo.getMonth() - dateFrom.getMonth() + (12 * (dateTo.getFullYear() - dateFrom.getFullYear()));
                if (dateTo.getDate() < dateFrom.getDate()) monthDiff--;

                if (_SearchType == 0)//hour
                {
                    if (dayDiff >= 8 && monthDiff < 6) ChangeSearchType(1);
                    else if (monthDiff >= 6) ChangeSearchType(3);
                }
                else if(_SearchType == 1)//day
                {
                    if (dayDiff < 1) ChangeSearchType(0);
                    else if (monthDiff >= 6) ChangeSearchType(3);
                }
                else if (_SearchType == 3)//month
                {
                    if (dayDiff < 1) ChangeSearchType(0);
                    else if (dayDiff >= 1 && monthDiff < 3) ChangeSearchType(1);

                }
            }

            GetDataMediumWise(_Medium,_MediumDesc);
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else if ($("#dpFrom").val() != "" && $("#dpTo").val() == "") {
        $("#dpTo").addClass("warningInput");
    }
    else if ($("#dpTo").val() != "" && $("#dpFrom").val() == "") {
        $("#dpFrom").addClass("warningInput");
    }
}

function SetSearchRequest(eleRequest,p_SearchRequestID,p_QueryName)
{
    if ($.inArray(p_SearchRequestID, _SearchRequests) == -1) 
    {
        _SearchRequests.push(p_SearchRequestID);
        _QueryNameList.push(p_QueryName);
        $(eleRequest).css("background-color","#E9E9E9");
    }
    else
    {
        var catIndex = _SearchRequests.indexOf(p_SearchRequestID);
        if (catIndex > -1) {
            _SearchRequests.splice(catIndex, 1);
            _QueryNameList.splice(catIndex, 1);
            $(eleRequest).css("background-color","#ffffff");
        }
    }
}

function SearchRequest()
{
    if($(_SearchRequests).not(_OldSearchRequests).length != 0 || $(_OldSearchRequests).not(_SearchRequests).length != 0)
    {
        _OldSearchRequests = _SearchRequests.slice(0);
        $.each(_QueryNameList, function(index, val){
            if(_QueryNames == "")
            {
                _QueryNames = val;
            }
            else
            {   
            _QueryNames = _QueryNames + ', ' + val;
            }
        });
        GetDataMediumWise(_Medium,_MediumDesc);
    }
}

function GetSummaryReport() {
    if (_FromDate == null || _ToDate == null) {
        var currDate = new Date();
        var previousDate = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate() - 29);
        _FromDate = previousDate;
        _ToDate = currDate;

        //previousDate.setDate(previousDate.getDate() - 29);
        /*var dd = previousDate.getDate();
        var mm = previousDate.getMonth() + 1;
        var y = previousDate.getFullYear();
        var formattedDate = mm + '/' + dd + '/' + y;
        _FromDate = formattedDate;
        $("#dpFrom").val(_FromDate);

        dd = currDate.getDate();
        mm = currDate.getMonth() + 1;
        y = currDate.getFullYear();
        formattedDate = mm + '/' + dd + '/' + y;
        _ToDate = formattedDate;
        $("#dpTo").val(_ToDate)*/

        $("#dpFrom").datepicker("setDate", previousDate);
        $("#dpTo").datepicker("setDate", currDate);
    }
    if (DateValidation()) {
        
        var jsonPostData = {
            p_FromDate: _FromDate,
            p_ToDate: _ToDate,
            p_SearchRequestIDs : _SearchRequests

        }
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDashboardSummaryReportResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }
}

function ChangeSearchType(sType) {
    _SearchType = sType;
    GetDataMediumWise(_Medium,_MediumDesc);
}

function GetDataMediumWise(mediumType,mediumDesc) {
    AddHighlightToMenu(mediumType);

    $('#divDateSelector').show();
    _Medium = mediumType;
    _MediumDesc = mediumDesc;

    /*if (_Medium != 'Overview' && _SearchType == 0) {
    _SearchType = 1;
    }*/

    SetActiveDuration();

    if (DateValidation()) {        
        var jsonPostData = {
            p_FromDate: _FromDate,
            p_ToDate: _ToDate,
            p_SearchRequestIDs : _SearchRequests,
            p_Medium: mediumType,
            p_SearchType: _SearchType,
            p_IsDefaultLoad: _IsDefaultLoad
        }
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDashboardGetMediumData,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnDataMediumWiseComplete,
            error: OnDataMediumWiseFail
        });
    }
}

function OnDataMediumWiseComplete(result) {
    CheckForSessionExpired(result);
    $('#divHeader').html('<b>' + result.CategoryDescription + '</b>');
    SetActiveFilter();
    /*if (_FromDate == null || _ToDate == null) {
    var currDate = new Date();
    var previousDate = new Date(currDate.getFullYear(), currDate.getMonth(), currDate.getDate() - 29);
    _FromDate = previousDate;
    _ToDate = currDate;
    }*/

    if (_FromDate == null || _FromDate != result.fromDate) {
        _FromDate = result.fromDate;
        $("#dpFrom").datepicker("setDate", _FromDate);
    }

    if (_ToDate == null || _ToDate != result.toDate) {
        _ToDate = result.toDate;
        $("#dpTo").datepicker("setDate", _ToDate);
    }

    if (_Medium == 'Overview') {
        $('#divHeader').html('<b>Source Overview</b>');
        OnResultSearchComplete(result);
    }
    else {
        if (result.isSuccess) {
            $('#divMediumData').html('');
            $('#divMediumData').html(result.HTML);
            SetActiveDuration();

            if (_Medium == "TV" || _Medium == "NM") {
                _SelectedDmas = new Array();
                _SelectedProvinces = new Array();
            }

            SetDashboardMediumHTML(result, _Medium);   
        }
        else {
            ShowNotification(_msgErrorOccured); //'Some error occured, try again later');
        }
    }
}

function OnDataMediumWiseFail(a, b, c) {
    ShowNotification(_msgErrorOccured,a); //'Some error occured, try again later');
}

function SetSentimentValue(negValue, posValue,Position) {
    $('#divNegativeSentimentValue'+Position).html(negValue);
    if (negValue.replace(/,/g, "") > 0) {
        $('#divNegativeSentimentValue'+Position).css({ 'background-color': 'red' });
    }
    else {
        $('#divNegativeSentimentValue'+Position).css({ 'background-color': '#DEDEDE' });
    }

    $('#divPositiveSentimentValue'+Position).html(posValue);
    if (posValue.replace(/,/g, "") > 0) {
        $('#divPositiveSentimentValue'+Position).css({ 'background-color': 'green' });
    }
    else {
        $('#divPositiveSentimentValue'+Position).css({ 'background-color': '#DEDEDE' });
    }
}

function DateValidation() {
    $('#dpFrom').removeClass('warningInput');
    $('#dpTo').removeClass('warningInput');

    // if both empty
    if (($('#dpTo').val() == '') && ($('#dpFrom').val() == '')) {
        return true;

    }
    //if one empty not other
    else if (($('#dpFrom').val() != '') && ($('#dpTo').val() == '')
                    ||
                    ($('#dpFrom').val() == '') && ($('#dpTo').val() != '')
                    ) {
        if ($('#dpFrom').val() == '') {

            ShowNotification(_msgFromDateNotSelected); //'From Date not selected');
            $('#dpFrom').addClass('warningInput');
        }

        if ($('#dpTo').val() == '') {

            ShowNotification(_msgToDateNotSelected); //'To Date not selected');
            $('#dpTo').addClass('warningInput');

        }
        return false;
    }
    //if both not empty
    else {
        var isFromDateValid = isValidDate($('#dpFrom').val().toString());
        var isToDateValid = isValidDate($('#dpTo').val().toString());
        if (isFromDateValid && isToDateValid) {
            var fromDate = new Date($('#dpFrom').val());
            var toDate = new Date($('#dpTo').val());
            if (fromDate > toDate) {
                ShowNotification(_msgFromDateLessThanToDate); //'From Date should be less than To Date');
                $('#dpFrom').addClass('warningInput');
                $('#dpTo').addClass('warningInput');
                return false;
            }
            else {
                return true;

            }
        }
        else {
            if (!isFromDateValid) {
                $('#dpFrom').addClass('warningInput');
            }
            if (!isToDateValid) {
                $('#dpTo').addClass('warningInput');
            }
            ShowNotification(_msgInvalidDate);
            return false;
        }
    }
}

function isValidDate(s) {
    var bits = s.split('/');
    var y = bits[2], m = bits[0], d = bits[1];

    // Assume not leap year by default (note zero index for Jan) 
    var daysInMonth = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];

    // If evenly divisible by 4 and not evenly divisible by 100, 
    // or is evenly divisible by 400, then a leap year 
    if ((!(y % 4) && y % 100) || !(y % 400)) {
        daysInMonth[1] = 29;
    }

    return d <= daysInMonth[--m]
}


function OnResultSearchComplete(result, onClickFunction) {
    SetDashboardOverviewHTML(result, "divMediumData", onClickFunction);
    SetActiveDuration();
}

function OnFail(a,b,c) {

    ShowNotification(_msgErrorOccured,a);
}

function RenderPieChart(jsonPieChartData) {
    $('#divPieChartData').html('');
    var myChart = new FusionCharts("Pie2D", "SubMediaPieChart", "300", "380");
    myChart.render("divPieChartData");
    myChart.setJSONData(jsonPieChartData);
}

function ChangeMediumType(){
    GetDataMediumWise(this.options.series[0].data[0].Value,this.options.series[0].data[0].SearchTerm);
}

function ShowTooltip()
{
    return '<div class="tooltip">'+this.x+'<br/><b>'+this.series.name+': </b>'+this.y+'</div>';
}

function ChangeMediumTypeOnPointClick(){
    GetDataMediumWise(this.options.Value,this.options.SearchTerm);
}

function HandleChartMouseHover()
{
    //if($(this.chart.renderTo).children('.highcharts-container') > 0)
    //{
        $('.highcharts-container').css("z-index","0");
        $(this.chart.renderTo).find('.highcharts-container').css("z-index","1");
    //}
}

function HandleChartMouseOut()
{
     //$(this.chart.renderTo).find('.highcharts-container').css("z-index","0");
}

function RenderSparkChart(jsonIQMediaValueRecords, divSparkChartID, ChartID) {

    var myChart = new FusionCharts("../../Content/Fusioncharts/SparkLine.swf", ChartID, "120", "100", "0", "1");
    // var ss = '{  "chart": {    "palette": "2","caption": "Cisco",    "setadaptiveymin": "1"  },  "dataset": [    {      "data": [         {          "value": "27.26"        },        {          "value": "37.88"        },        {          "value": "38.88"        },        {          "value": "22.9"        },        {          "value": "39.02"        },        {          "value": "23.31"        },        {          "value": "30.85"        },        {          "value": "27.01"        },        {          "value": "33.2"        },        {          "value": "21.93"        },        {          "value": "34.51"        },        {          "value": "24.84"        },        {          "value": "39.32"        },        {          "value": "37.04"        },        {          "value": "27.81"        },        {          "value": "22.95"        },        {          "value": "24.73"        },        {          "value": "37.63"        },        {          "value": "29.75"        },        {          "value": "22.35"        },        {          "value": "34.35"        },        {          "value": "27.6"        },        {          "value": "27.97"        },        {          "value": "32.36"        },        {          "value": "22.56"        },        {          "value": "24.15"        },        {          "value": "24.93"        },        {          "value": "35.82"        },        {          "value": "23.45"        },        {          "value": "37.64"        },        {          "value": "26.99"        },       {          "value": "29.48"        },        {          "value": "36.63"        },        {          "value": "35.58"        },        {         "value": "32.19"        },        {         "value": "27.59"        },        {          "value": "26.94"       },        {          "value": "32.35"        },      {          "value": "22.63"        },        {          "value": "25.97"        },        {          "value": "25.28"        },        {          "value": "26.73"        },        {          "value": "23.47"        },        {          "value": "20.55"        },        {          "value": "34.58"        },        {          "value": "29.16"        },        {          "value": "34.97"        },        {          "value": "24.57"        },        {          "value": "20.7"        },        {          "value": "32.61"        }      ]    }  ]}';
    myChart.setJSONData(jsonIQMediaValueRecords);
    myChart.render(divSparkChartID);

}

function changeTab(tabNumber) {
    if (tabNumber == '1') {
        $('#divLineChartSubMedia').hide();
        $('#divLineChartSubMedia').css("height", "0");
        $('#divLineChartSubMedia').css("width", "0");
        $('#divLineChartSubMedia').css("overflow", "hidden");
        $('#divLineChartMedia').css("height", "300px");
        $('#divLineChartMedia').css("width", "100%");
        $('#divLineChartMedia').css("overflow", "visible");
        $('#imgSingleLine').attr("src", "../../Images/Dashboard/single-line-normal.png");
        $('#imgMultipleLine').attr("src", "../../Images/Dashboard/multiple-line-active.png");

        $('#divLineChartMedia').show();


    }
    else {
        $('#divLineChartMedia').hide();
        $('#divLineChartMedia').css("height", "0");
        $('#divLineChartMedia').css("width", "0");
        $('#divLineChartMedia').css("overflow", "hidden");
        $('#divLineChartSubMedia').css("height", "300px");
        $('#divLineChartSubMedia').css("width", "100%");
        $('#divLineChartSubMedia').css("overflow", "visible");
        $('#imgSingleLine').attr("src", "../../Images/Dashboard/single-line-active.png");
        $('#imgMultipleLine').attr("src", "../../Images/Dashboard/multiple-line-normal.png");

        $('#divLineChartSubMedia').show();

    }

}

function SetActiveDuration() {
    $('#divDuration div').each(function () {
        $(this).removeClass('activeDuration');
    });
    if (_SearchType == 0) {
        $('#divHourlyDuration').addClass('activeDuration');
    }
    else if (_SearchType == 1) {
        $('#divDayDuration').addClass('activeDuration');
    }
    if (_SearchType == 2) {
        $('#divWeekDuration').addClass('activeDuration');
    }
    if (_SearchType == 3) {
        $('#divMonthDuration').addClass('activeDuration');
    }

}


function GenerateDashboardPDF() {
    //FusionCharts("SubMediaLineChart").ref.getSVGString();

    var jsonPostData = {
        p_HTML: $("#divPrintableArea").html(),
        p_FromDate: $("#dpFrom").val(),
        p_ToDate: $("#dpTo").val(),
        p_SearchRequests : _QueryNames
    }

    $.ajax({
        url: _urlDashboardGenerateDashboardPDF,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                window.location = _urlDashboardDownloadPDFFile;
            }
            else {
                ShowNotification(_msgErroWhileDownloadingFile);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing,a);
        }
    });
}


function ShowDashboardEmailPopup() {

    $('#txtFromEmail').val($('#hdnLoggedInUserEmail').val());

    $("#txtToEmail").val('');
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
    $("#divEmailPopup").css({ "display": "none" });
    $("#divEmailPopup").modal("hide");
}

function ValidateSendEmail() {
    var isValid = true;

    $("#spanFromEmail").html("").hide();
    $("#spanToEmail").html("").hide();
    $("#spanBCCEmail").html("").hide();
    $("#spanSubject").html("").hide();
    $("#spanMessage").html("").hide();

    if ($("#txtFromEmail").val() == "") {
        $("#spanFromEmail").show().html(_msgFromEmailRequired);
        isValid = false;
    }
    if ($("#txtToEmail").val() == "") {
        $("#spanToEmail").show().html(_msgToEmailRequired);
        isValid = false;
    }

    if ($("#txtSubject").val() == "") {
        $("#spanSubject").show().html(_msgSubjectRequired);
        isValid = false;
    }
    if ($("#txtMessage").val() == "") {
        $("#spanMessage").show().html(_msgMessageRequired);
        isValid = false;
    }

    if ($("#txtFromEmail").val() != "" && !CheckEmailAddress($("#txtFromEmail").val())) {
        $("#spanFromEmail").show().html(_msgIncorrectEmail);
        isValid = false;
    }

    if ($("#txtToEmail").val() != "") {
        
        var Toemail = $("#txtToEmail").val();
        if (Toemail.substr(Toemail.length - 1) == ";") {
            Toemail = Toemail.slice(0, -1);
        }
        
        $(Toemail.split(';')).each(function (index, value) {
            if (!CheckEmailAddress(value)) {
                $("#spanToEmail").show().html(_msgOneEmailAddressInCorrect);
                isValid = false;
                return;
            }
        });

        if (Toemail.split(';').length > _MaxEmailAdressAllowed) {
            $("#spanToEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
            isValid = false;
        }
    }

    if ($("#txtBCCEmail").val() != "") {
        if ($("#txtToEmail").val() == "") {
            $("#spanBCCEmail").show().html(_msgBCCEmailMissingTo);
            isValid = false;
        }
        else {            
            var BCCemail = $("#txtBCCEmail").val();
            if (BCCemail.substr(BCCemail.length - 1) == ";") {
                BCCemail = BCCemail.slice(0, -1);
            }
        
            $(BCCemail.split(';')).each(function (index, value) {
                if (!CheckEmailAddress(value)) {
                    $("#spanBCCEmail").show().html(_msgOneEmailAddressInCorrect);
                    isValid = false;
                    return;
                }
            });

            if (BCCemail.split(';').length > _MaxEmailAdressAllowed) {
                $("#spanBCCEmail").show().html(_msgMaxEmailAdressLimitExceeds.replace(/@@MaxLimit@@/g, _MaxEmailAdressAllowed));
                isValid = false;
            }
        }
    }

    return isValid;
}

function CheckEmailAddress(email) {
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    return emailPattern.test(email);
}

function SendEmail() {

    if (ValidateSendEmail()) {
        var jsonPostData = {
            p_HTML: $("#divPrintableArea").html(),
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_FromEmail: $("#txtFromEmail").val(),
            p_ToEmail: $("#txtToEmail").val(),
            p_BCCEmail: $("#txtBCCEmail").val(),
            p_Subject: $("#txtSubject").val(),
            p_UserBody: $("#txtMessage").val(),
            p_SearchRequests: _QueryNames
        }

        $.ajax({
            url: _urlDashboardSendDashBoardEmail,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnEmailSendComplete,
            error: OnEmailSendFail
        });
    }
}

function OnEmailSendComplete(result) {
    CancelEmailpopup();
    if (result.isSuccess) {
        ShowNotification(_msgEmailSent.replace(/@@emailSendCount@@/g, result.emailSendCount));

    }
    else {
        ShowNotification(result.errorMessage);
    }
}

function OnEmailSendFail(result) {
    CancelEmailpopup();
    ShowNotification(_msgErrorOccured);
}

function OpenFeed(date, medium, mediumDesc,searchRequest,searchRequestDesc) {
    
    var selectedDate = new Date(date); 

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardOpenIFrame,
        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            if (result.isSuccess) {
                $('#divFeedsPage').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });

                $("#divFeedsPage").resizable({
                    handles : 'e,se,s,w',
                    iframeFix: true,
                    start: function(){
                        ifr = $('#iFrameFeeds');
                        var d = $('<div></div>');

                        $('#divFeedsPage').append(d[0]);
                        d[0].id = 'temp_div';
                        d.css({position:'absolute'});
                        d.css({top: ifr.position().top, left:0});
                        d.height(ifr.height());
                        d.width('100%');
                    },
                    stop: function(){
                        $('#temp_div').remove();
                    },
                    resize: function (event, ui) {
                        var newWd = ui.size.width - 10;
                        var newHt = ui.size.height - 20;
                        $("#iFrameFeeds").width(newWd).height(newHt);
                    }
                }).draggable({
                    iframeFix: true,
                    start: function(){
                        ifr = $('#iFrameFeeds');
                        var d = $('<div></div>');

                        $('#divFeedsPage').append(d[0]);
                        d[0].id = 'temp_div';
                        d.css({position:'absolute'});
                        d.css({top: ifr.position().top, left:0});
                        d.height(ifr.height());
                        d.width('100%');
                    },
                    stop: function(){
                        $('#temp_div').remove();
                    }
                });

                //$('#divFeedsPage').resizable({handles: 'e,w'});
                //$("#divFeedsPage").resizable({helper: "ui-resizable-helper"});
                $('#iFrameFeeds').attr("src", "//" + window.location.hostname + "/Feeds?date=" + (selectedDate.getMonth() + 1) + "/" + selectedDate.getDate() + "/" + selectedDate.getFullYear() + "&medium=" + medium + "&mediumDesc=" + mediumDesc + "&searchrequest=" + searchRequest + "&searchrequestDesc=" + encodeURIComponent(searchRequestDesc.split('+').join(' ')));
                //$('#iFrameFeeds').attr("src", "localhost:55188/Feeds?date=" + date.substring(0, 10) + "&medium=" + medium + "&mediumDesc=" + mediumDesc);
                
                $('#divFeedsPage').css("position", "");
                $('#divFeedsPage').css("height", documentHeight - 200);
                $('#iFrameFeeds').css("height", documentHeight - 200);

            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured,a);
        }
    });


}

function OpenFeedOutletDma(type,value,mediumdesc,iqdmaname) {

    
    var station = '';
    var dma = '';
    var competeurl = '';
    var _IQDmaID = '';    
    var handle = '';
    var publication = '';
    var author = '';
    var medium = _Medium;
    var searchRequestIDs = '[]';
    var searchRequestNames = '[]';

    if (typeof (iqdmaname) === 'undefined') {
        iqdmaname =''    
    }

    // ProQuest and BLPM data is combined
    if (medium == 'PM') {
        medium = 'PA';
    }
    
    switch(type)
    {
        case "st" : 
            station = value;
            break;
        case "dma" : 
            dma = value;
            break;
        case "comp" : 
            competeurl = value;
            break;
        case "dmaid" : 
            _IQDmaID = value;
            break;
        case "handle" : 
            handle = value;
            break;
        case "pub" :
            publication = value;
            break;
        case "author" :
            author = value;
            break;            
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardOpenIFrame,
        contentType: 'application/json; charset=utf-8',

        success: function (result) {
            if (result.isSuccess) {
                $('#divFeedsPage').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });

                var fromDate = new Date(_FromDate);
                var toDate = new Date(_ToDate);
                $('#iFrameFeeds').attr("src", "//" + window.location.hostname + "/Feeds?station=" + station + "&dma=" + dma + "&competeurl=" + competeurl + "&iqdmaname="+ iqdmaname  +"&iqdmaid=" + _IQDmaID + "&handle=" + handle + "&publication=" + publication + "&author=" + author + "&fromDate=" + (fromDate.getMonth() + 1) + "/" + fromDate.getDate() + "/" + fromDate.getFullYear()+ "&toDate=" + (toDate.getMonth() + 1) + "/" + toDate.getDate() + "/" + toDate.getFullYear() + "&medium=[" + JSON.stringify(medium) + "]&mediumDesc=" + JSON.stringify(mediumdesc) + "&searchrequest=" + JSON.stringify(_SearchRequests) + "&searchrequestDesc=" + encodeURIComponent(JSON.stringify(_QueryNameList).split('+').join(' ')));
                //$('#iFrameFeeds').attr("src", "http://localhost:55188/Feeds?date=" + date.substring(0, 10) + "&medium=" + medium + "&mediumDesc=" + mediumDesc);

                $('#divFeedsPage').css("height", documentHeight - 200);
                $('#iFrameFeeds').css("height", documentHeight - 200);

            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured,a);
        }
    });
}

function CancelIFramePopup() {
    $("#divFeedsPage").css({ "display": "none" });
    $("#divFeedsPage").modal("hide");
    $('#iFrameFeeds').attr("src", "");
}

function SetActiveFilter() {
    var isFilterEnable = false;
    $("#divActiveFilter").html("");
    
     if (_SearchRequests.length > 0) {
        $.each(_SearchRequests, function(index, val){
            $('#divActiveFilter').append('<div class="filter-in">' + EscapeHTML(_QueryNameList[index]) + '<span class="cancel" onclick="RemoveFilter(\''+ val +'\');"></span></div>');
        });
        
        isFilterEnable = true;
    }


    if (isFilterEnable) {
        $("#divActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)' });
    }
    else {
        $("#divActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter(requestId) {

    // Represent SearchRequest Filter
    var catIndex = _SearchRequests.indexOf(requestId);
    if (catIndex > -1) {
        _SearchRequests.splice(catIndex, 1);
        _QueryNameList.splice(catIndex, 1);
        _OldSearchRequests = _SearchRequests.slice(0);
        $("#areq_" + requestId).css("background-color","#ffffff");
        GetDataMediumWise(_Medium,_MediumDesc);
    }
}

function LineChartClick()
{
    var searchRequestIDs = '';
    var searchRequestDescs = '';
    var medium = '';

    if (_Medium != 'Overview')
    {
        if (_Medium == 'PM')
        {
            // ProQuest and BLPM data is combined
            medium = '["PA"]';
        }
        else if (_Medium == 'SocialMedia')
        {
            // Social Media and Facebook data is combined
            medium = '["SocialMedia","FB"]';
        }
        else
        {
            medium = '["' + _Medium + '"]';
        }
    }

    if (this.Value != null)
    {
        searchRequestIDs = '["' + this.Value + '"]';
        searchRequestDescs = '["' + this.SearchTerm + '"]';
    }

    if (this.Type == 'Media')
    {
        OpenFeed(this.category, medium, medium, searchRequestIDs, searchRequestDescs);
    }
    if (this.Type == 'SubMedia')
    {
        if(_Medium == 'Overview')
        {
            if (this.Value == 'PM')
            {
                // ProQuest and BLPM data is combined
                medium = '["PA"]';
            }
            else if (this.Value == 'SocialMedia')
            {
                // Social Media and Facebook data is combined
                medium = '["SocialMedia","FB"]';
            }
            else
            {
                medium = '["' + this.Value + '"]';
            }
            OpenFeed(this.category, medium, '["' + this.SearchTerm + '"]', JSON.stringify(_SearchRequests), JSON.stringify(_QueryNameList));
        }
        else
        {   
            OpenFeed(this.category, medium, medium, searchRequestIDs, searchRequestDescs);
        }
    }
}

function GetMonth(){
    var date = new Date(this.value);
    return  _Months[date.getMonth()] + ' - ' + date.getFullYear().toString() + '';
}


function ExpandCollapseSparkChart(img,divChartEle)
{
    var chartMain = $('#divLineChartMedia').highcharts();
    var seriesName =$(img).closest('td').text().trim();
    if($(img).attr('src').indexOf('ex.gif') > 0)
    {
    
        var seriesLength = chartMain.series.length;
        for(var i = seriesLength - 1; i > 0; i--)
        {
            if(chartMain.series[i].name != _MediumDesc && $.inArray(chartMain.series[i].name, _QueryNameList) == -1)
            {
                chartMain.series[i].remove();
            }
        }

        var YAxesLength = chartMain.yAxis.length;
        for(var i = YAxesLength - 1; i > 0; i--)
        {
            chartMain.yAxis[i].remove();
        }

        $('.ulSubMediaCharts .broadcastSmallChartHeaderMedium img').attr('src','../images/ex.gif');
       
        var newSeries = new Array();
        $.each($('#'+divChartEle).highcharts().series[0].data,function(index,obj){
            newSeries.push({y:obj.y,category:obj.category});
        });

        chartMain.addAxis({ 
            id: 'oppAxis',
            title: {
                text: seriesName
            },
            min: 0,
            minRange: 0.1,
            gridLineWidth: 0,
            opposite: true
        });

        if(seriesName != 'Sentiment')
        {
            chartMain.addSeries({name:seriesName,data :newSeries,yAxis:'oppAxis'});
        }
        else
        {
            
            chartMain.addSeries({name:'Positive ' + seriesName,data :newSeries,yAxis:'oppAxis',color:'green'});

            var newSeries2 = new Array();
            $.each($('#'+divChartEle).highcharts().series[1].data,function(index,obj){
                newSeries2.push({y:obj.y,category:obj.category});
            });
            chartMain.addSeries({name:'Negative ' + seriesName,data :newSeries2,yAxis:'oppAxis',color:'red'});
        }
        
        
        //chartMain.xAxis[0].setExtremes();
        $(img).attr('src','../images/cl.gif');
    }
    else
    {
        var seriesLength = chartMain.series.length;
        for(var i = seriesLength - 1; i > -1; i--)
        {
            if(seriesName != 'Sentiment')
            {
                if(chartMain.series[i].name ==seriesName)
                {
                    chartMain.series[i].remove();
                    $(img).attr('src','../images/ex.gif');
                    break;
                }
            } 
            else
            {
                if(chartMain.series[i].name =='Positive ' + seriesName || chartMain.series[i].name =='Negative ' + seriesName)
                {
                    chartMain.series[i].remove();
                    $(img).attr('src','../images/ex.gif');
                    
                }
            }       
            
        }

        var YAxesLength = chartMain.yAxis.length;
        for(var i = YAxesLength - 1; i > 0; i--)
        {
            chartMain.yAxis[i].remove();
        }
    }

    if(_QueryNameList != null && _QueryNameList.length <= 0)
    {
        if(chartMain.series.length > 1)
        {
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes)[0].textContent ='Number of hits';
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes).text('Number of hits');
            chartMain.series[0].update({name:"Number of hits"}, false);
        }
        else
        {
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes)[0].textContent =_MediumDesc
            //$(chartMain.legend.allItems[0].legendItem.element.childNodes).text(_MediumDesc);
            chartMain.series[0].update({name:_MediumDesc}, false);
        }
    }

    chartMain.redraw();
}



function CompareDma()
{
    if(_SelectedDmas.length > 0){
        
        var jsonPostData = {
            p_Dmas: _SelectedDmas,
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_SearchRequests : _SearchRequests,
            p_SearchType: _SearchType,
            p_Medium : _Medium
        }

        $.ajax({
            url: _urlDashboardCompareDma,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnCompareDmaComplete,
            error: OnCompareDmaFail
        });
    }
    else
    {
        $("#divCompareDma").hide();
    }
}


function OnCompareDmaComplete(result)
{
    if(result.isSuccess)
    {
        RenderSparkHighChart(result.noOfDocsJson,'divDmaHitChart','DmaHitChart');
        RenderSparkHighChart(result.noOfHitsJson,'divDmaMentionChart','DmaMentionChart');
        RenderSparkHighChart(result.noOfViewJson,'divDmaAudienceChart','DmaAudienceChart');

        if(result.noOfMinOfAiringJson != "")
        {
            RenderSparkHighChart(result.noOfMinOfAiringJson,'divDmaAirTimeChart','DmaAirTimeChart');
        }


        $("#divSelectedDmas").html('<br/>');

        var chartMain = $('#divDmaHitChart').highcharts();

        $.each(_SelectedDmas,function(index,object){
            var dmaname =$("#divUsaMap")[0].children[0].FusionCharts.annotations._renderer.entities.items[eval(_SelectedDmas[index].id)].eJSON.label;
            $("#divSelectedDmas").append('<div>DMA ' + (index + 1) + ': <span style="color:'+_SelectedDmas[index].clickColor+'">' + dmaname +'</span></div>');
        });


        $("#divCompareDma").show();
    }
    else
    {
        ShowNotification(_msgErrorOccured);
    }
}

function OnCompareDmaFail(a,b,c)
{
    ShowNotification(_msgErrorOccured);
}

function CompareProvince() {
    if (_SelectedProvinces.length > 0) {

        var jsonPostData = {
            p_Provinces: _SelectedProvinces,
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_SearchRequests: _SearchRequests,
            p_SearchType: _SearchType,
            p_Medium: _Medium
        }

        $.ajax({
            url: _urlDashboardCompareProvince,
            contentType: "application/json; charset=utf-8",
            type: "post",
            dataType: "json",
            data: JSON.stringify(jsonPostData),
            success: OnCompareProvinceComplete,
            error: OnCompareDmaFail
        });
    }
    else {
        $("#divCompareProvince").hide();
    }
}

function OnCompareProvinceComplete(result) {
    if (result.isSuccess) {
        RenderSparkHighChart(result.noOfDocsJson, 'divProvinceHitChart', 'ProvinceHitChart');
        RenderSparkHighChart(result.noOfHitsJson, 'divProvinceMentionChart', 'ProvinceMentionChart');
        RenderSparkHighChart(result.noOfViewJson, 'divProvinceAudienceChart', 'ProvinceAudienceChart');

        if (result.noOfMinOfAiringJson != "") {
            RenderSparkHighChart(result.noOfMinOfAiringJson, 'divProvinceAirTimeChart', 'ProvinceAirTimeChart');
        }

        $("#divSelectedProvinces").html('<br/>');

        var chartMain = $('#divProvinceHitChart').highcharts();

        $.each(_SelectedProvinces, function (index, object) {
            var provinceName = $("#divCanadaMap")[0].children[0].FusionCharts.annotations._renderer.entities.items[_SelectedProvinces[index].id].eJSON.label;
            $("#divSelectedProvinces").append('<div>Province ' + (index + 1) + ': <span style="color:' + _SelectedProvinces[index].clickColor + '">' + provinceName + '</span></div>');
        });

        $("#divCompareProvince").show();
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}

function ShowDmaListMap(inp)
{
    $("#divTopCountries").hide();
    $("#divCAMap").hide();
    if(inp == 0)
    {
        $("#divTopDmas").show();
        $("#divDmaMap").hide();
    }

    if(inp == 1)
    {
        $("#divTopDmas").hide();
        $("#divDmaMap").show();
    }
}

function ShowTopCountryList() {
    $("#divDmaMap").hide();
    $("#divTopDmas").hide();
    $("#divCAMap").hide();
    $("#divTopCountries").show();
}

function ShowCanadaMap() {
    $("#divDmaMap").hide();
    $("#divTopDmas").hide();
    $("#divTopCountries").hide();
    $("#divCAMap").show();
}


//function RenderDmaMapChart(jsonMapData, divMapChartID, ChartID) {
//    
//    FusionCharts.setCurrentRenderer('javascript');
//    var chartObj=new FusionCharts("maps/usadma",ChartID,"100%","450","0","1");
//    chartObj.render(divMapChartID);
//    chartObj.setJSONData(jsonMapData);
//    chartObj.addEventListener('entityClick',CheckUncheckDma);
//}


function RenderDmaMapChart(jsonMapData, divMapChartID, ChartID) {
    //FusionCharts.ready(function() {
        
        var populationMap = new FusionCharts({
            type: 'maps/usadma',
            renderAt: divMapChartID,
            width: '100%',
            height: '350',
            dataFormat: 'json',
            dataSource:jsonMapData
        }).render();
        populationMap.addEventListener('entityClick',CheckUncheckDma);
        //populationMap.addEventListener('resized',SetLegendPos);
       // populationMap.addEventListener('drawComplete',SetLegendPos);
        populationMap.addEventListener('entityRollOut', function (evt, data) {
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
            hideToolTip()
        });
        populationMap.addEventListener('entityRollOver',function(evt,data){
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
            showToolTipOnChart("Market Area Name : " + data.label + "<br/>" + "Mention : " + data.value);
        });
        populationMap.addEventListener('chartRollOver',function(evt,data){
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
        });
        populationMap.addEventListener('chartRollOut',function(evt,data){
            SetCurrentSelectedItemsFillColor(this, _SelectedDmas);
        });
        
        
    //});

    //$(".raphael-group-24-dataset").attr("transform","matrix(1,0,0,1,"+Ypos+",0)");
}

function RenderCanadaMapChart(jsonMapData, divMapChartID, ChartID) {
    var populationMap = new FusionCharts({
        type: 'maps/canada',
        renderAt: divMapChartID,
        width: '100%',
        height: '350',
        dataFormat: 'json',
        dataSource: jsonMapData
    }).render();
    populationMap.addEventListener('entityClick', CheckUncheckProvince);
    populationMap.addEventListener('entityRollOut', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
        hideToolTip();
    });
    populationMap.addEventListener('entityRollOver', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
        showToolTipOnChart("Province Name : " + data.label + "<br/>" + "Mention : " + data.value);
    });
    populationMap.addEventListener('chartRollOver', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
    });
    populationMap.addEventListener('chartRollOut', function (evt, data) {
        SetCurrentSelectedItemsFillColor(this, _SelectedProvinces);
    });
}

function SetLegendPos(){
        var Ypos =  ($("#divUsaMap").width() / 2) - ($("g.fusioncharts-legend > rect").attr("width") / 2)
        $("g.fusioncharts-legend").attr("transform","matrix(1,0,0,1,"+Ypos+",0)");    
        //alert('set le pos');
    }

function CheckUncheckDma(evt, data)
{
    var _dmaids = [];
    $.each(_SelectedDmas,function(index,object){
        _dmaids.push(object.id);
    });
    
    var graphEntity = this.annotations._renderer.entities;

    if($.inArray(data.id, _dmaids) == -1)
    {
        
        if(_SelectedDmas.length >= 2) {
            $.each(graphEntity.items[eval(_SelectedDmas[0].id)].svgElems, function (index, svgElem) {
                $(svgElem.graphic[0]).css("fill", "");
            });
            _dmaids.splice(0, 1);
            _SelectedDmas.splice(0, 1);
        }

        _dmaids.push(data.id);

        _SelectedDmas = new Array();
        $.each(_dmaids,function(index,object){
            
            _objDma = new Object();
            _objDma.id = object;
            _objDma.clickColor = _DmaChartColors[index];

            $.each(graphEntity.items[eval(object)].svgElems, function (index1, svgElem) {
                $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
            });

            _SelectedDmas.push(_objDma);
        });

        
        CompareDma();
    }
    else
    {
        var catIndex = _dmaids.indexOf(data.id);
        if (catIndex > -1) {
            _dmaids.splice(catIndex, 1);

            _SelectedDmas = new Array();
            $.each(_dmaids,function(index,object){
            
                _objDma = new Object();
                _objDma.id = object;
                _objDma.clickColor = _DmaChartColors[index];;

                $.each(graphEntity.items[eval(object)].svgElems, function (index1, svgElem) {
                    $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
                });

                _SelectedDmas.push(_objDma);
            });

            CompareDma();
        }
    }

}

function CheckUncheckProvince(evt, data) {
    var _provinceIDs = [];
    $.each(_SelectedProvinces, function (index, object) {
        _provinceIDs.push(object.id);
    });

    var graphEntity = this.annotations._renderer.entities;
    
    if ($.inArray(data.id, _provinceIDs) == -1) {
        if (_SelectedProvinces.length >= 2) {
            $.each(graphEntity.items[_SelectedProvinces[0].id].svgElems, function (index, svgElem) {
                $(svgElem.graphic[0]).css("fill", "");
            });

            _provinceIDs.splice(0, 1);
            _SelectedProvinces.splice(0, 1);
        }

        _provinceIDs.push(data.id);

        _SelectedProvinces = new Array();
        $.each(_provinceIDs, function (index, object) {
            var _objProvince = new Object();
            _objProvince.id = object;
            _objProvince.clickColor = _DmaChartColors[index];

            $.each(graphEntity.items[object].svgElems, function (index1, svgElem) {
                $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
            });

            _SelectedProvinces.push(_objProvince);
        });

        CompareProvince();
    }
    else {
        var catIndex = _provinceIDs.indexOf(data.id);
        if (catIndex > -1) {
            _provinceIDs.splice(catIndex, 1);

            _SelectedProvinces = new Array();
            $.each(_provinceIDs, function (index, object) {
                var _objProvince = new Object();
                _objProvince.id = object;
                _objProvince.clickColor = _DmaChartColors[index];

                $.each(graphEntity.items[object].svgElems, function (index1, svgElem) {
                    $(svgElem.graphic[0]).css("fill", _DmaChartColors[index]);
                });

                _SelectedProvinces.push(_objProvince);
            });

            CompareProvince();
        }
    }
}

var x,y,zInterval;
var Interval=0;
document.onmousemove = setMouseCoords;

function setMouseCoords(e) {
    if(document.all) {
        tooptipx = window.event.clientX;
        tooptipy = window.event.clientY + 600;
       
    } else {
   
        tooptipx = e.pageX;
        tooptipy = e.pageY;
    }
}
function showToolTipOnChart(zText) {
    clearInterval(zInterval);
    zInterval = setTimeout("doShowToolTip('" + zText.trim() + "')",0);
    Interval=0;
}
function doShowToolTip(zText) {
    clearInterval(zInterval);
//    if(zText=='divover'){
//   
//    }
//    else{
    document.getElementById("mapToolTip").style.top = (tooptipy+10) + "px";
    document.getElementById("mapToolTip").style.left = tooptipx + "px";
    document.getElementById("mapToolTip").innerHTML = zText.trim();
    document.getElementById("mapToolTip").style.display = "block";
    zInterval = setTimeout("hideToolTip()",500000);
//    }
}

function hideToolTip1() {
    if(Interval!=1000)
    {
        document.getElementById("mapToolTip").style.display = "none";
        clearInterval(zInterval);
        Interval=0;
    }
}
function hideToolTip() {
    zInterval = setTimeout("hideToolTip1()",0);
    Interval=0;
   
}
function hideToolTipDiv() {
    zInterval = setTimeout("hideToolTip1()",100000);
    Interval=1000;
}

function SetCurrentSelectedItemsFillColor(chartObj, selectedItems)
{
    $.each(selectedItems, function (index, object) {
        $.each(chartObj.annotations._renderer.entities.items[object.id].svgElems, function (index1, svgElem) {
            $(svgElem.graphic[0]).css("fill", selectedItems[index].clickColor);
        }); 
    });
}

function GetAdhocSummaryData() {
    $('#divDateSelector').show();

    var source = getParameterByName("source");
    var isOnlyParents = getParameterByName("isOnlyParents");
    var sinceID = getParameterByName("sinceID");
    var mediaIDs = null;
    var jsonPostData;

    if (parent._MediaIDs != null)
    {
        mediaIDs = parent._MediaIDs.join(",");
    }

    if (isOnlyParents != "" && sinceID != "")
    {
        jsonPostData = {
            mediaIDs: mediaIDs,
            source: source,
            fromDate: parent._fromDate,
            ToDate: parent._toDate,
            searchRequestID: parent._RequestID,
            mediumTypes: parent._Medium,
            keyword: parent._Keyword,
            sentiment: parent._Sentiment,
            prominenceValue : parent._ProminenceValue,
            isProminenceAudience : parent._IsProminenceAudience,
            isOnlyParents: isOnlyParents,
            isRead: parent._IsRead,
            sinceID : sinceID
        }
    }
    else
    {
        jsonPostData = {
            mediaIDs: mediaIDs,
            source: source
        }
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDashboardGetAdhocSummaryData,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function(result) { 
            if (result.isSuccess)
            {
                OnResultSearchComplete(result, function() { return false; });

                _FromDate = result.fromDate;
                $("#dpFrom").datepicker("setDate", _FromDate);
                _ToDate = result.toDate;
                $("#dpTo").datepicker("setDate", _ToDate);

                $("#divLineChartOptions").hide();

                $(".liSubMediaCharts").prop("onclick", null);
                $(".liSubMediaCharts").removeClass("cursorPointer");
            }
            else
            {
                ShowNotification(result.errorMessage);
            }
        },
        error: OnDataMediumWiseFail
    });    
}
