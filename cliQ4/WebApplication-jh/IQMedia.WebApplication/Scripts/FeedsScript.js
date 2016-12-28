﻿var _Keyword = "";
var _fromDate = null;
var _toDate = null;
var _QueryName = [];
var _Medium = [];
var _MediumDescription = [];
var _RequestID = [];
var _OldRequestID = [];
var _Sentiment = null;
var _IsAsc = false;
var _MaxFeedsReportItems = 0;
var _PageSize = null;
var _CurrentRequestFilter = new Array();
var _Dma = "";
var _Station = "";
var _CompeteUrl = "";
var _IQDMAID = "";
var _IQDMAName = "";
var _TwitterHandle = "";
var _Publication = "";
var _Author = "";
var _IsRead = null;
var _IsSelectAll = false;
var _IsSelectAllParent = true;
var _ProminenceValue = null;
var _IsProminenceAudience = false;
var _CurrentProminenceValue = 0;
var _CurrentProminenceAudience = false;
var _IsAudienceSort = null;
var _MediaIDs = [];
var _PieChartTotalHits = 0; // Used by IQMediaCommon.js for Dashboard popup
var _CurrentNumPages = 1;
var _ScrollToTop = true;


var _PositiveSentimentActiveFilter = '<div id="divSentimentAF" class="clear"><div class="right" id="divSentimentAF"><div class="divSentimentMain"><div class="divSentimentNeg"><div class="width0">&nbsp;</div></div><div class="divSentimentPos"><div class="width74">&nbsp;</div></div></div></div></div>';
var _NegativeSentimentActiveFilter = '<div id="divSentimentAF" class="clear"><div class="right" id="divSentimentAF"><div class="divSentimentMain"><div class="divSentimentNeg width50p"><div class="width74">&nbsp;</div></div><div class="divSentimentPos"><div class="width0"></div></div></div></div></div>';
var _NuetralSentimentActiveFilter = '<div id="divSentimentAF" class="clear"><div class="" id="divSentimentN" class="right"><div class="divSentimentMain" class="divSentimentMain"><div class="width50p divSentimentNeg"><div class="width0">&nbsp;</div></div><div class="divSentimentPos"><div class="width0"></div></div></div></div></div>';

var _NegativeSentimentFilter = '<li role="presentation" onclick="SetSentiment(-1);"><a href="#" tabindex="-1" role="menuitem"><div id="divSentiment_Neg" class="clear"><div class="right width100p" id="divSentimentR"><div class="divSentimentMain width100p"><div class="divSentimentNeg width50p"><div class="width74">&nbsp;</div></div><div class="divSentimentPos"><div class="width0"></div></div></div></div></div></a></li>';
var _PositiveSentimentFilter = '<li role="presentation" onclick="SetSentiment(1);"><a href="#" tabindex="-1" role="menuitem"><div id="divSentiment_Pos" class="clear"><div class="right width100p" id="divSentimentG"><div class="divSentimentMain width100p"><div class="divSentimentNeg width50p"><div class="width0">&nbsp;</div></div><div class="divSentimentPos width50p"><div class="width74">&nbsp;</div></div></div></div></div></a></li>';
var _NuetralSentimentFilter = '<li role="presentation" onclick="SetSentiment(0);"><a href="#" tabindex="-1" role="menuitem"><div id="divSentiment_Neutral" class="clear"><div class="right width100p" id="divSentimentN"><div class="divSentimentMain width100p"><div class="divSentimentNeg width50p"><div class="width0">&nbsp;</div></div><div class="divSentimentPos"><div class="width0"></div></div></div></div></div></a></li>';

function enableAllTheseDays(date) {

    date = $.datepicker.formatDate('mm/dd/yy', date);

    return [$.inArray(date, disabledDays) !== -1];
}

var customCategoryObject = "";

$(document).ready(function () {

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuFeeds').attr("class", "active");

    //documentHeight = $(window).height();
    if (screen.height >= 768) {
        if (getParameterByName("medium") == '' && getParameterByName("date") == '') {
            $('#divMainContent').css({ 'height': documentHeight - 250 });
        }
        else {
            $('#divMainContent').css({ 'height': documentHeight - 120 });
        }
    }

    $('#mCSB_1').css({ 'max-height': '' });
    $('#divMessage').html('');
    $('#dpFrom').val('');
    $('#dpTo').val('');

    $("#divCalender").datepicker({
        beforeShowDay: enableAllTheseDays,
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            $('#dpFrom').val(dateText);
            $('#dpTo').val(dateText);
            SetDateVariable();
        }

    });

    $('.ndate').click(function () {
        $("#divCalender").datepicker("refresh");
    });

    $("body").click(function (e) {
        if ((e.target.id != "aPageSize" && e.target.id != "divPageSizePopover") && $(e.target).parents("#divPageSizePopover").size() <= 0) {
            $('#divPageSizePopover').remove();
        }

        if (e.target.id == "liAgentFilter" || $(e.target).parents("#liAgentFilter").size() > 0) {
            if ($('#ulAgent').is(':visible')) {
                $('#ulAgent').hide();
            }
            else {
                $('#ulAgent').show();
            }
        }
        else if ((e.target.id !== "liAgentFilter" && e.target.id !== "ulAgent" && $(e.target).parents("#ulAgent").size() <= 0) || e.target.id == "btnSearchAgent") {
            $('#ulAgent').hide();
            if (e.target.id != "btnSearchCategory") {
                var agentLI = "";
                $.each(_CurrentRequestFilter, function (eventID, eventData) {
                    if (_OldRequestID.length > 0 && $.inArray(eventData.ID, _OldRequestID) !== -1 && $.inArray(eventData.ID, _RequestID) == -1) {
                        _RequestID.push(eventData.ID);
                        _QueryName.push(eventData.QueryName);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.ID, _RequestID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    agentLI = agentLI + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetQueryName(this,\'' + eventData.QueryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\',' + eventData.ID + ');" tabindex=\"-1\" role=\"menuitem\">';
                    agentLI += EscapeHTML(eventData.QueryName) + ' (' + eventData.CountFormatted + ') </a></li>';
                });

                if (agentLI != "") {
                    $('#ulAgentList').html(agentLI);
                    $('#liAgentSearch').show();
                }
                else {
                    $('#ulAgentList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liAgentSearch').hide();
                }
            }
        }

        if (e.target.id == "liProminenceFilter" || $(e.target).parents("#liProminenceFilter").size() > 0) {
            if ($('#ulProminence').is(':visible')) {
                $('#ulProminence').hide();
            }
            else {
                $('#ulProminence').show();
            }
        }
        else if ((e.target.id !== "liProminenceFilter" && e.target.id !== "ulProminence" && $(e.target).parents("#ulProminence").size() <= 0) || e.target.id == "btnSearchProminence") {
            $('#ulProminence').hide();
        }
    });


    if (getParameterByName("searchrequest")) {
        _QueryName = $.parseJSON(getParameterByName("searchrequestDesc"));
        _RequestID = $.parseJSON(getParameterByName("searchrequest"));
        _OldRequestID = _RequestID.slice(0);
        var agentLI = "";
        $.each(_CurrentRequestFilter, function (eventID, eventData) {
            var liStyle = "";
            if ($.inArray(eventData.ID, _RequestID) !== -1) {
                liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
            }
            agentLI = agentLI + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetQueryName(this,\'' + eventData.QueryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\',' + eventData.ID + ');" tabindex=\"-1\" role=\"menuitem\">';
            agentLI += EscapeHTML(eventData.QueryName) + ' (' + eventData.CountFormatted + ') </a></li>';
        });

        if (agentLI != "") {
            $('#ulAgentList').html(agentLI);
            $('#liAgentSearch').show();
        }
        else {
            $('#ulAgentList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
            $('#liAgentSearch').hide();
        }
    }

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
        }
    });

    $("#dpFrom").keypress(function (e) {
        if (e.keyCode == 13) {
            SetDateVariable();
        }
    });

    $("#dpTo").keypress(function (e) {
        if (e.keyCode == 13) {
            SetDateVariable();
        }
    });

    $("#txtKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword();
        }
    });

    $("#imgKeyword").click(function (e) {
        SetKeyword();
    });

    GetCustomCategory();
    GetFeedsReport();
    GetFeedsReportLimit();


    var qryMedium = getParameterByName("medium")
    _Medium = [];
    _MediumDescription = [];

    if (qryMedium != '') {

        _Medium = $.parseJSON(qryMedium);
        _MediumDescription = $.parseJSON(getParameterByName("mediumDesc"));
    }

    var qryDma = getParameterByName("dma")
    _Dma = '';

    if (qryDma != '') {
        _Dma = qryDma;
    }

    var qryStation = getParameterByName("station")
    _Station = '';

    if (qryStation != '') {
        _Station = qryStation;
    }

    var qryCompete = getParameterByName("competeurl")
    _CompeteUrl = '';

    if (qryCompete != '') {
        _CompeteUrl = qryCompete;
    }

    var qryIQDMA = getParameterByName("iqdmaid")
    _IQDMAID = '';
    _IQDMAName = '';

    if (qryIQDMA != '') {
        _IQDMAID = qryIQDMA;
        _IQDMAName = getParameterByName("iqdmaname")
    }

    var qryTwitter = getParameterByName("handle")
    _TwitterHandle = '';

    if (qryTwitter != '') {
        _TwitterHandle = qryTwitter;
    }

    var qryPublication = getParameterByName("publication")
    _Publication = '';

    if (qryPublication != '') {
        _Publication = qryPublication;
    }

    var qryAuthor = getParameterByName("author")
    _Author = '';

    if (qryAuthor != '') {
        _Author = qryAuthor;
    }

    if (getParameterByName("date")) {

        _fromDate = getParameterByName("date");
        _toDate = _fromDate;

        $("#dpFrom").datepicker("setDate", _fromDate);
        $("#dpTo").datepicker("setDate", _toDate);

        SetActiveFilter();
    }
    else if (getParameterByName("fromDate") && getParameterByName("toDate")) {
        _fromDate = getParameterByName("fromDate");
        _toDate = getParameterByName("toDate");

        $("#dpFrom").datepicker("setDate", _fromDate);
        $("#dpTo").datepicker("setDate", _toDate);

        SetActiveFilter();
    }
    else {
        $("#dpFrom").datepicker("setDate", _fromDate);
        $("#dpTo").datepicker("setDate", _toDate);
        $('#aDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // When drilling down from Dashboard, sort by Outlet Weight by default  
    if (getParameterByName("date") != '' || getParameterByName("medium") != '') {
        _IsAsc = false;
        _IsAudienceSort = true;
        $('#aSortDirection').html('Outlet Weight&nbsp;&nbsp;<span class="caret"></span>');
    }

    if (screen.height >= 768) {
        $("#divMainContent").mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            },
            scrollInertia: 60
        });
    }

    $('#ddlReportTitle').change(function () {
        $('#ddlReportTitle').removeClass('warningInput');
    });

    $("#chkArticleAddToLibrary").click(function () {
        $('label[for="ddLibraryCategory"]').toggleClass('greyOut', !this.checked);
        $("#ddLibraryCategory").prop('disabled', !this.checked);
    });

    GetFilterData();

    // below function is called from IQMediaCommon.js
    // to set Height of content of the modal popup
    SetModalBodyScrollBarForPopUp();
    SetImageSrc();
});

$(window).resize(function () {
    if (screen.height >= 768) {
        if(getParameterByName("medium") == '' && getParameterByName("date") == '')
        {
            $('#divMainContent').css({ 'height': documentHeight - 250 });
        }
        else
        {
            $('#divMainContent').css({ 'height': documentHeight - 120 });
        }
    }
});

function GetFilterData() {
    var jsonPostData = {
        QueryName: _QueryName,
        fromDate: _fromDate,
        ToDate: _toDate,
        searchRequestID: _RequestID,
        mediumTypes: _Medium,
        keyword: _Keyword,
        sentiment: _Sentiment,
        Dma : _Dma,
        Station: _Station,
        CompeteUrl: _CompeteUrl,
        _IQDMAID : _IQDMAID,
        Handle : _TwitterHandle,
        publication: _Publication,
        author: _Author,
        prominenceValue : _ProminenceValue,
        isProminenceAudience : _IsProminenceAudience
    }

    $.ajax({
        url: "/Feeds/GetFilterData/",
        type: "post", 
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                ModifyFilters(result.filter);
                if (result.filter.ListOfSearchRequestFilter != null) {
                    _CurrentRequestFilter = result.filter.ListOfSearchRequestFilter.slice(0);
                }
                else {
                    _CurrentRequestFilter = [];
                }
            }
            else {
                ShowErrorMessage();
                ClearResultsOnError('ulMediaResults', 'divRecordCount', 'divShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "CallHandler()"));
            }

        },
        error: function (a, b, c) {
            ShowErrorMessage(a);
            ClearResultsOnError('ulMediaResults', 'divRecordCount', 'divShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "CallHandler()"));
        }
    });
}

function GetCustomCategory(callback,forceBind)
{
    if (customCategoryObject == '' || customCategoryObject == null || forceBind == true) {

        $(".media #divPopover .popover-content").addClass("blurOnlyControls");
        $("#imgSaveTweetLoading").show();

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlCommonBindCategoryDropDown,
            contentType: 'application/json; charset=utf-8',
            global: false,

            success: function(res){
                OnCategoryBindComplete(res,callback);
            },
            error: OnCategoryBindFail
        });
    }
}


function OnCategoryBindComplete(result,callback) {
        if (result.isSuccess) {

            $(".media #divPopover .popover-content").removeClass("blurOnlyControls");
            $("#imgSaveTweetLoading").hide();

            customCategoryObject = result.customCategory;
            var categoryOptions = '<option value="0">Select Category</option>';

            $.each(customCategoryObject, function (eventID, eventData) {
                categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
            });

            $('#ddlReportCategory').html(categoryOptions);
            $('#ddlLibraryCategory').html(categoryOptions);

            if(typeof(callback) != 'undefined')
            {
                callback();
            }
            
        }
        else {
            if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
                RedirectToUrl(result.redirectURL);
            }
            else {
                ShowErrorMessage();
            }
        }
    }
    function OnCategoryBindFail(a, b, c) {
        ShowErrorMessage(a);
    }

function SetQueryName(eleQuery, p_queryName, p_requestID) {
    if ($.inArray(p_requestID, _RequestID) == -1) {
        _RequestID.push(parseInt(p_requestID));
        _QueryName.push(p_queryName);
        $(eleQuery).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestID.indexOf(p_requestID);
        if (catIndex > -1) {
            _RequestID.splice(catIndex, 1);
            _QueryName.splice(catIndex, 1);
            $(eleQuery).css("background-color", "#ffffff");
        }
    }
}

function SearchAgent() {
    if ($(_RequestID).not(_OldRequestID).length != 0 || $(_OldRequestID).not(_RequestID).length != 0) {
        _OldRequestID = _RequestID.slice(0);
        CallHandler();
    }
}

function SetMedium(p_Medium, p_MediumDescription) {
    if (_Medium.length == 0 || _Medium.length > 1 || _Medium[0] != p_Medium) {
        _Medium = [p_Medium];
        _MediumDescription = [p_MediumDescription];
        CallHandler();
    }
}

function SetSentiment(p_Sentiment) {
    if (_Sentiment != p_Sentiment) {
        _Sentiment = p_Sentiment;
        CallHandler();
    }
}

function SetProminence(element, p_ProminenceValue) {    
    if (_ProminenceValue != p_ProminenceValue) {
        var prevValue = _ProminenceValue;

        _ProminenceValue = p_ProminenceValue;
        $(element).css("background-color", "#E9E9E9");

        if (prevValue != null) {
            $("#liProminence" + prevValue).css("background-color", "#ffffff");
        }
    }
    else {
        _ProminenceValue = null;
        $(element).css("background-color", "#ffffff");
    }
}

function SetProminenceType() {
    _IsProminenceAudience = $("#rdoProminenceAudience").is(":checked");
}

function SearchProminence() {
    _IsProminenceAudience = $("#rdoProminenceAudience").is(":checked");

    if (_ProminenceValue != _CurrentProminenceValue || (_IsProminenceAudience != _CurrentProminenceAudience && _ProminenceValue != null)) {
        CallHandler();
    }
}

function SortDirection(p_IsAsc) {
    if (_IsAsc != p_IsAsc || _IsAudienceSort != null) {
        _IsAudienceSort = null;
        _IsAsc = p_IsAsc;
        if (_IsAsc) {
            $('#aSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            $('#aSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        CallHandler();
    }
}

function SetProminenceSort(p_IsAudienceSort) {
    if (_IsAudienceSort != p_IsAudienceSort)     {
        _IsAsc = false;
        _IsAudienceSort = p_IsAudienceSort;
        if (_IsAudienceSort) {
            $('#aSortDirection').html('Outlet Weight&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            $('#aSortDirection').html('Article Weight&nbsp;&nbsp;<span class="caret"></span>');
        }
        CallHandler();
    }
}


function SetDateVariable() {

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        if (_fromDate != $("#dpFrom").val() || _toDate != $("#dpTo").val()) {
            _fromDate = $("#dpFrom").val();
            _toDate = $("#dpTo").val();
            CallHandler();
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
    }
    else
        if ($("#dpFrom").val() != "" && $("#dpTo").val() == "") {
            $("#dpTo").addClass("warningInput");
        }
        else if ($("#dpTo").val() != "" && $("#dpFrom").val() == "") {
            $("#dpFrom").addClass("warningInput");
        }
}

function SetIsRead(isRead) {
    if (_IsRead != isRead) {
        _IsRead = isRead;
        CallHandler();
    }
}

function CallHandler(numPages, resultMsg) {
    $("#dpFrom").removeClass("warningInput");
    $("#dpTo").removeClass("warningInput");

    if (DateValidation()) {

        _CurrentNumPages = 1;
        $("#chkInputAll").attr("checked", false);

        var jsonPostData = {
            QueryName: _QueryName,
            fromDate: _fromDate,
            ToDate: _toDate,
            searchRequestID: _RequestID,
            mediumTypes: _Medium,
            keyword: _Keyword,
            sentiment: _Sentiment,
            isAsc: _IsAsc,
            pageSize: _PageSize,
            Dma : _Dma,
            Station: _Station,
            CompeteUrl: _CompeteUrl,
            _IQDMAID : _IQDMAID,
            Handle : _TwitterHandle,
            publication: _Publication,
            author: _Author,
            isRead: _IsRead,
            prominenceValue : _ProminenceValue,
            isProminenceAudience : _IsProminenceAudience,
            isAudienceSort: _IsAudienceSort,
            numPages: numPages != undefined ? numPages : 1 // Used to reload the page after deletes
        }

        $.ajax({

            url: _urlFeedsMediaJsonResults,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonPostData),
            success: function (result) {

                // Set filter irrespective of success or failure
                SetActiveFilter();

                if (result.isSuccess) {
                    if (result.isValidResponse) {
                        $("#divNoData").hide();
                    }
                    else {
                        $("#divNoData").show();
                    }

                    $("#divShowResult").show();
                    $("#divRecordCount").show();
                    $("#ulMediaResults").html(result.html);
                    ModifyFilters(result.filter);
                    if (result.filter.ListOfSearchRequestFilter != null) {
                        _CurrentRequestFilter = result.filter.ListOfSearchRequestFilter.slice(0);
                    }
                    else {
                        _CurrentRequestFilter = [];
                    }
                    _CurrentProminenceValue = _ProminenceValue;
                    _CurrentProminenceAudience = _IsProminenceAudience;
                    ShowHideMoreResults(result);
                    ShowNoofRecords(result);
                    SetMediaClickEvent()

                    $("#chkInputAll").prop("checked", false);

                    if (screen.height >= 768 && _ScrollToTop) {
                        setTimeout(function () {
                            $("#divMainContent").mCustomScrollbar("scrollTo", "top");
                        }, 200);
                    }
                    else {
                        _ScrollToTop = true;
                    }

                    $(".media input[type=checkbox]").each(function () {
                        $(this).prop("checked", false);
                    });

                    SetImageSrc();

                    // If reloading the page after an action, display the action's result message once the load is complete
                    if (resultMsg != undefined) {
                        ShowNotification(resultMsg);
                    }

                    if (_IsRead != null && result.isReadLimitExceeded) {
                        $("#divIsReadMsg").show();
                    }
                    else {
                        $("#divIsReadMsg").hide();
                    }
                }
                else {
                    ShowErrorMessage();
                    ClearResultsOnError('ulMediaResults', 'divRecordCount', 'divShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "CallHandler()"));
                }

            },
            error: function (a, b, c) {
                ShowErrorMessage(a);
                ClearResultsOnError('ulMediaResults', 'divRecordCount', 'divShowResult', _msgErrorOnSearch.replace(/@@MethodName@@/g, "CallHandler()"));
            }
        });
    }
}

function GetMoreResults() {

    $("#btnShowMoreResults").attr("disabled", "disabled");
    $("#btnShowMoreResults").attr("class", "disablebtn");
    $('#imgMoreResultLoading').removeClass("displayNone");


    $.ajaxSetup({ cache: false });

    var jsonPostData = {
        QueryName: _QueryName,
        fromDate: _fromDate,
        ToDate: _toDate,
        searchRequestID: _RequestID,
        mediumTypes: _Medium,
        keyword: _Keyword,
        sentiment: _Sentiment,
        isAsc: _IsAsc,
        pageSize: _PageSize,
        Dma: _Dma,
        Station: _Station,
        CompeteUrl: _CompeteUrl,
        _IQDMAID: _IQDMAID,
        Handle: _TwitterHandle,
        publication: _Publication,
        author: _Author,
        isRead: _IsRead,
        prominenceValue: _ProminenceValue,
        isProminenceAudience: _IsProminenceAudience,
        isAudienceSort: _IsAudienceSort
    }


    $.ajax({

        url: _urlFeedsMoreMediaJsonResults,
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        contentType: "application/json; charset=utf-8",
        global: false,
        success: function (result) {

            $("#imgMoreResultLoading").addClass("displayNone");
            $("#btnShowMoreResults").removeAttr("disabled");
            $("#btnShowMoreResults").attr("class", "loadmore");

            if (result.isSuccess) {
                if (result.isValidResponse)
                {
                    $("#ulMediaResults").append(result.html);
                    ShowHideMoreResults(result);
                    ShowNoofRecords(result);
                    SetMediaClickEvent();

                    SetImageSrc();

                    if ($("#chkInputAll").is(":checked")) {
                        checkUncheckAll('ulMediaResults', 'chkInputAll');
                    }

                    if (_IsRead != null && result.isReadLimitExceeded) {
                        $("#divIsReadMsg").show();
                    }
                    else {
                        $("#divIsReadMsg").hide(); 
                    }

                    _CurrentNumPages++;
                }
                else
                {
                    ShowNotification(_msgFeedsSolrTimeout);                
                }
            }
            else {
                ShowErrorMessage();
            }
        },
        error: function (a, b, c) {
            ShowErrorMessage();
            $("#imgMoreResultLoading").addClass("displayNone");
            $("#btnShowMoreResults").removeAttr("disabled");
            $("#btnShowMoreResults").attr("class", "loadmore");
        }
    });

}

function SetActiveFilter() {

    var isFilterActive = false;

    $("#divActiveFilter").html("");

    if (_Keyword != "") {
        $('#divActiveFilter').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_Keyword) + '<span class="cancel" onclick="RemoveFilter(1);"></span></div>');
        isFilterActive = true;
    }

    if ((_fromDate != null && _fromDate != "") && (_toDate != null && _toDate != "")) {
        isFilterActive = true;
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _fromDate + ' To ' + _toDate + '<span class="cancel" onclick="RemoveFilter(2);"></span></div>');
    }

    if (_QueryName != "") {
        $('#divActiveFilter').append('<div id="divAgentActiveFilter" class="filter-in">' + EscapeHTML(_QueryName.join(", ")) + '<span class="cancel" onclick="RemoveFilter(4);"></span></div>');
        isFilterActive = true;
    }

    if (_MediumDescription != "") {
        $('#divActiveFilter').append('<div id="divMediumActiveFilter" class="filter-in">' + _MediumDescription + '<span class="cancel" onclick="RemoveFilter(3);"></span></div>');
        isFilterActive = true;
    }

    var sentiHTML = "";

    if (_Sentiment == 0) {
        sentiHTML = _NuetralSentimentActiveFilter;
    }
    else if (_Sentiment > 0) {
        sentiHTML = _PositiveSentimentActiveFilter;
    }
    else if (_Sentiment < 0) {
        sentiHTML = _NegativeSentimentActiveFilter;
    }

    if (sentiHTML != "") {
        $('#divActiveFilter').append('<div id="divSentimentActiveFilter" class="filter-in">' + sentiHTML + '<span class="cancel" onclick="RemoveFilter(5);"></span></div>');
        isFilterActive = true;
    }

    if (_Dma != "") {
        $('#divActiveFilter').append('<div id="divDmaActiveFilter" class="filter-in">' + _Dma + '<span class="cancel" onclick="RemoveFilter(6);"></span></div>');
        isFilterActive = true;
    }

    if (_Station != "") {
        $('#divActiveFilter').append('<div id="divStationActiveFilter" class="filter-in">' + _Station + '<span class="cancel" onclick="RemoveFilter(7);"></span></div>');
        isFilterActive = true;
    }

    if (_CompeteUrl != "") {
        $('#divActiveFilter').append('<div id="divUrlActiveFilter" class="filter-in">' + _CompeteUrl + '<span class="cancel" onclick="RemoveFilter(8);"></span></div>');
        isFilterActive = true;
    }

    if (_IQDMAID != "") {
        $('#divActiveFilter').append('<div id="divDmaIDActiveFilter" class="filter-in">' + _IQDMAName + '<span class="cancel" onclick="RemoveFilter(9);"></span></div>');
        isFilterActive = true;
    }

    if (_TwitterHandle != "") {
        $('#divActiveFilter').append('<div id="divHandleActiveFilter" class="filter-in">' + '@' + EscapeHTML(_TwitterHandle) + '<span class="cancel" onclick="RemoveFilter(10);"></span></div>');
        isFilterActive = true;
    }

    if (_Publication != "") {
        $('#divActiveFilter').append('<div id="divPublicationActiveFilter" class="filter-in">' + _Publication + '<span class="cancel" onclick="RemoveFilter(12);"></span></div>');
        isFilterActive = true;
    }

    if (_Author != "") {
        $('#divActiveFilter').append('<div id="divAuthorActiveFilter" class="filter-in">' + _Author + '<span class="cancel" onclick="RemoveFilter(13);"></span></div>');
        isFilterActive = true;
    }

    if (_IsRead != null) {
        $('#divActiveFilter').append('<div id="divIsReadActiveFilter" class="filter-in">' + (_IsRead ? 'Read Articles' : 'Unread Articles') + '<span class="cancel" onclick="RemoveFilter(14);"></span></div>');
        isFilterActive = true;
    }

    if (_ProminenceValue != null) {
        var displayValue = 100 - _ProminenceValue;
        var isProminenceAudience = " - Article Weight";
        if (_IsProminenceAudience) {
            isProminenceAudience = " - Outlet Weight";
        }
        $('#divActiveFilter').append('<div id="divProminenceActiveFilter" class="filter-in">' + 'Top ' + displayValue + '%' + isProminenceAudience + '<span class="cancel" onclick="RemoveFilter(11);"></span></div>');
        isFilterActive = true;
    }

    if (isFilterActive) {
        $('#divActiveFilter').css({ 'border-bottom': '1px solid #ECECEC' });
    }
    else {
        $('#divActiveFilter').css({ 'border-bottom': '0px solid #ECECEC' });
    }

    if (screen.height >= 768) {
        if ($('#divActiveFilter').children().length > 0) {
            if(getParameterByName("medium") == '' && getParameterByName("date") == '')
            {
                $('#divMainContent').css({ 'height': $(window).height() - 250 });
            }
            else
            {
                $('#divMainContent').css({ 'height': $(window).height() - 120 });
            }
        }
    }
}

function ModifyFilters(filter) {

    disabledDays = [];

    if (filter.FilterMediaDate != null) {
        $.each(filter.FilterMediaDate, function (eventID, eventData) {
            disabledDays.push(eventData);
        });
    }

    var agentLI = "";
    if (filter.ListOfSearchRequestFilter != null) {
        $.each(filter.ListOfSearchRequestFilter, function (eventID, eventData) {
            if (eventData.QueryName != '' && eventData.ID != '') {
                var liStyle = "";
                if ($.inArray(eventData.ID, _RequestID) !== -1) {
                    liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                }
                agentLI = agentLI + '<li role=\"presentation\"><a ' + liStyle + ' href=\"javascript:void(0)\" onclick="SetQueryName(this,\'' + eventData.QueryName.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + '\',' + eventData.ID + ');" tabindex=\"-1\" role=\"menuitem\">';
                agentLI += eventData.QueryName + ' (' + eventData.CountFormatted + ') </a></li>';
            }
        });
    }


    if (agentLI != "") {
        $('#ulAgentList').html(agentLI);
        $('#liAgentSearch').show();
    }
    else {
        $('#ulAgentList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        $('#liAgentSearch').hide();
    }

    var mediumLI = "";
    if (filter.FilterListCategory != null) {
        $.each(filter.FilterListCategory, function (eventID, eventData) {

            mediumLI = mediumLI + '<li onclick="SetMedium(\'' + eventData.Value + '\',\'' + eventData.Key + '\');" role=\"presentation\"><a href=\"#\" tabindex=\"-1\" role=\"menuitem\">';
            mediumLI += eventData.Key + ' (' + eventData.CountFormatted + ') </a></li>';

        });
    }

    if (mediumLI == "") {
        $("#ulMedium").html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
    else {
        $("#ulMedium").html(mediumLI);
    }

    var sentiLI = "";
    if (filter.PositiveSentiment > 0) {
        sentiLI = sentiLI + _PositiveSentimentFilter + "(" + filter.PositiveSentimentFormatted + ")";
    }

    if (filter.NegativeSentiment > 0) {
        sentiLI = sentiLI + _NegativeSentimentFilter + "(" + filter.NegativeSentimentFormatted + ")";
    }

    if (filter.NullSentiment > 0) {
        sentiLI = sentiLI + _NuetralSentimentFilter + "(" + filter.NullSentimentFormatted + ")";
    }

    if (!sentiLI) {
        $("#ulSentiment").html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
    else {
        $("#ulSentiment").html(sentiLI);
    }

    var isReadLI = "";
    if (filter.Read > 0) {
        isReadLI = isReadLI + '<li onclick="SetIsRead(true);" role="presentation"><a href="#" tabindex="-1" role="menuitem">Read (' + filter.ReadFormatted + ')</a></li>';
    }
    if (filter.Unread > 0) {
        isReadLI = isReadLI + '<li onclick="SetIsRead(false);" role="presentation"><a href="#" tabindex="-1" role="menuitem">Unread (' + filter.UnreadFormatted + ')</a></li>';
    }

    if (!isReadLI) {
        $("#ulIsRead").html('<li role="presentation"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
    else {
        $("#ulIsRead").html(isReadLI);
    }
}

function RemoveFilter(filtertype) {

    // Represent SearchKeyword
    if (filtertype == 1) {
        $("#txtKeyword").val("");
        _Keyword = "";
    }

    // Represent Date filter(From Date,To Date)
    if (filtertype == 2) {
        var dtCurrent = new Date();
        var fDate = new Date(dtCurrent.getFullYear(), dtCurrent.getMonth() - 3, dtCurrent.getDate());

        $("#dpFrom").datepicker("setDate", fDate);
        $("#dpTo").datepicker("setDate", dtCurrent);
        $("#divCalender").datepicker("setDate", null);
        
        $('#aDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
        
        _fromDate = $("#dpFrom").val();
        _toDate = $("#dpTo").val();
    }

    // Represent Category/Medium Filter
    if (filtertype == 3) {
        _Medium = [];
        _MediumDescription = [];
    }

    // Represent SearchRequest Filter
    if (filtertype == 4) {
        _QueryName = [];
        _RequestID = [];
        _OldRequestID = [];
    }

    // Represent Sentiment Filter
    if (filtertype == 5) {
        _Sentiment = null;
    }


    if (filtertype == 6) {
        _Dma = "";
    }

    if (filtertype == 7) {
        _Station = "";
    }

    if (filtertype == 8) {
        _CompeteUrl = "";
    }

    if (filtertype == 9) {
        _IQDMAID = "";
        _IQDMAName = "";
    }

    if (filtertype == 10) {
        _TwitterHandle = "";
    }

    if (filtertype == 11) {
        _IsProminenceAudience = false;
        _ProminenceValue = null;

        $("#rdoProminenceMultiplier").prop("checked", "checked");
        $.each($("#ulProminenceList li"), function (eventID, eventData) {
            $(eventData).css("background-color", "#ffffff");
        });
    }

    if (filtertype == 12) {
        _Publication = "";
    }

    if (filtertype == 13) {
        _Author = "";
    }

    if (filtertype == 14) {
        _IsRead = null;
    }

    CallHandler();
}

function ShowNoofRecords(result) {
    if (result != null && result.totalRecordsDisplay != null && result.totalRecordsDisplay != "0" && result.currentRecords != null) {
        $("#divRecordCount").show();
        $("#spanCurrentRecords").html(result.currentRecords + " (" + result.currentRecordsDisplay + ")");
        $("#spanTotalRecords").html(result.totalRecords + " (" + result.totalRecordsDisplay + ")");
        $("#aPageSize").show();
    }
    else {
        $("#divRecordCount").hide();
        $("#aPageSize").hide();
    }
}

function ShowHideMoreResults(result) {
    if (!result.hasMoreResults) {
        $("#btnShowMoreResults").attr("value", _msgNoMoreResult);
        $("#btnShowMoreResults").removeAttr("onclick");
    }
    else {
        $("#btnShowMoreResults").attr("value", _msgShowMoreResults);
        $("#btnShowMoreResults").attr("onclick", "GetMoreResults();");
    }
}

function RemoveTVResult() {
    var tvresultIDs = [];
    $('#ulMediaResults input[type=checkbox]').each(function () {
        if ($(this).is(':checked')) {
            tvresultIDs.push($(this).val());
        }
    });

    if (tvresultIDs.length < 400) {
        RemoveTVResultHandler(tvresultIDs);
    }
    else {
        ShowNotification("Selection is too large. Please select less than 400 items.");
    }
}

function checkUncheckAll(divID, mainCheckBox) {

    var checkBoxValue = false;
    checkBoxValue = $("#" + mainCheckBox).is(":checked");
    if (_IsSelectAll) {
        $("#" + divID + " input[type=checkbox]").each(function () {
            this.checked = checkBoxValue;
            if (checkBoxValue) {
                $(this).closest('.media').css('background', '#F4F4F4');
            }
            else {
                $(this).closest('.media').css('background', '');
            }
        });
    }
    else {
        $("#" + divID + " input[type=checkbox][id^='chkdivResults_']").each(function () {
            this.checked = checkBoxValue;
            if (checkBoxValue) {
                $(this).closest('.media').css('background', '#F4F4F4');
            }
            else {
                $(this).closest('.media').css('background', '');
            }
        });
    }
}

function ClearCheckboxSelection() {
    $("#chkInputAll").removeAttr("checked");
    $("#ulMediaResults input[type=checkbox]").each(function () {
        this.checked = false;
        $(this).closest('.media').css('background', '');
    });
}

function CheckUncheckMasterCheckBox(checkboxID, divID, mainCheckBox) {
    
    if(_IsSelectAll){
        if (!$('#' + checkboxID).is(":checked")) {
            $("#" + mainCheckBox).prop("checked", false);
            $("#" + checkboxID).closest('.media').find('input').prop("checked", false);
            $("#" + checkboxID).closest('.media').css('background', '');
        }
        else {
            var isChecked = true;
            $("#" + checkboxID).closest('.media').find('input').prop("checked", true);
            $("#" + checkboxID).closest('.media').css('background', '#F4F4F4');
            $("#" + divID + " input[type=checkbox]").each(function () {
                if (!this.checked) {
                    isChecked = false;
                }
            });

            if (isChecked) {
                $("#" + mainCheckBox).prop("checked", true);
            }
            else {
                $("#" + mainCheckBox).prop("checked", false);
            }
        }
    }
    else{
        if($('#' + checkboxID + '[id^=chkdivResults_]').length > 0)
        {
            if (!$('#' + checkboxID).is(":checked")) {
                $("#" + mainCheckBox).attr("checked", false);
                $("#" + checkboxID).closest('.media').css('background', '');
            }
            else {
                $("#" + checkboxID).closest('.media').css('background', '#F4F4F4');
                var isChecked = true;
                $("#" + divID + " input[type=checkbox][id^='chkdivResults_']").each(function () {
                    if (!this.checked) {
                        isChecked = false;
                    }
                });

                if (isChecked) {
                    $("#" + mainCheckBox).prop("checked", true);
                }
                else {
                    $("#" + mainCheckBox).prop("checked", false);
                }
            }
        }
    }
}

function RemoveTVResultHandler(IDs) {
    if (validateGrid('ulMediaResults')) {
        var jsonPostData = {
            selectedRecords: IDs,
            isAsc: _IsAsc,
            isAudienceSort: _IsAudienceSort,
            isSelectAll: $("#chkInputAll").is(":checked") && _IsSelectAll
        }

        getConfirm("Delete Feeds", "Are you sure you want to delete the selected results? Note: Deleting parent results will also delete all associated duplicates.", "Confirm Delete", "Cancel", function (res) {
            if (res) {
                $.ajax({
                    url: _urlFeedsDeleteMediaResults,
                    type: "post",
                    dataType: "json",
                    contentType: "application/json; charset=utf-8",
                    data: JSON.stringify(jsonPostData),
                    success: function (result) {
                        if (result.isSuccess) {
                           CallHandler(_CurrentNumPages, result.recordCount + _msgFeedsDeleteSuccess)
                        }
                        else {
                            ShowNotification(result.errorMsg);
                        }
                    },
                    error: function (a, b, c) {
                        ShowErrorMessage();
                    }
                });
            }
        });
    }
    else {
        alert(_msgSelectRecordToDelete);
    }
}

function ShowErrorMessage(a) {
    ShowNotification(_msgErrorOccured, a);
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

            ShowNotification(_msgFromDateNotSelected);
            $('#dpFrom').addClass('warningInput');
        }

        if ($('#dpTo').val() == '') {

            ShowNotification(_msgToDateNotSelected);
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
                ShowNotification(_msgFromDateLessThanToDate);
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

function SetKeyword() {
    if ($("#txtKeyword").val() == "") {
        RemoveFilter(1);
    }
    else {
        _Keyword = $("#txtKeyword").val();
        CallHandler();
    }
}

function DoLogin(event) {
    if (event.keyCode == 13) {
        Login();
    }
}

function GetResultOnDuration(duration) {

    $("#dpFrom").removeClass("warningInput");
    $("#dpTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _Duration = duration;

    // 24 hours
    if (duration == 1) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 1);
        $('#aDuration').html(_msgLast24Hours + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Last week
    else if (duration == 2) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth(), dtcurrent.getDate() - 7);
        $('#aDuration').html(_msgLastWeek + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last Month
    else if (duration == 3) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 1, dtcurrent.getDate());
        $('#aDuration').html(_msgLastMonth + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // Last 3 Months
    else if (duration == 4) {
        fDate = new Date(dtcurrent.getFullYear(), dtcurrent.getMonth() - 3, dtcurrent.getDate());
        $('#aDuration').html(_msgLast3Month + '&nbsp;&nbsp;<span class="caret"></span>');
    }
    // Custom
    else if (duration == 5) {

        dtcurrent = null;
        if ($("#dpFrom").val() != "" && $("#dpTo").val() != "") {
            $('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else {
            if ($("#dpFrom").val() == "") {
                $("#dpFrom").addClass("warningInput");
            }
            if ($("#dpTo").val() == "") {
                $("#dpTo").addClass("warningInput");
            }
            ShowNotification(_msgSelectDate);
        }
    }

    $("#dpFrom").datepicker("setDate", fDate);
    $("#dpTo").datepicker("setDate", dtcurrent);

    if ($("#dpFrom").val() != "" && $("#dpTo").val() != "") {

        if (_fromDate != $("#dpFrom").val() || _toDate != $("#dpTo").val()) {
            _fromDate = $("#dpFrom").val();
            _toDate = $("#dpTo").val();
            CallHandler();
        }
    }
}

function SelectPageSize(pageSize) {
    if (_PageSize != pageSize) {
        _PageSize = pageSize;
        $('#aPageSize').html(_PageSize + ' Items Per Page&nbsp;&nbsp;<span class="caret"></span>');
        $('#divPageSizePopover').remove();
        CallHandler();
    }
}

function clearDate() {

    $('#dpFrom').val(_fromDate);
    $('#dpTo').val(_toDate);

    $('#dpFrom').removeClass('warningInput');
    $('#dpTo').removeClass('warningInput');
}

function ShowLogINPopup() {

    var logINHTML = '<div aria-hidden="false" style="display: block;" id="divLogin" class="modal fade hide header-right in">'
                               + '<form class="margin10 bs-docs-example form-horizontal">'
            + '<div class="control-group">'
              + '<label for="inputEmail" class="control-label">Email</label>'
              + '<div class="controls">'
                + '<input type="text" placeholder="Email" id="txtModalEmail" onkeypress="DoLogin(event);"><span id="spnEmail" style="color:red;"></span>'
              + '</div>'
             + '</div>'
            + '<div class="control-group">'
              + '<label for="inputPassword" class="control-label">Password</label>'
              + '<div class="controls">'
                + '<input type="password" placeholder="Password" id="txtModalPassword" onkeypress="DoLogin(event);"><span style="color:red;" id="spnPassword"></span>'
              + '</div>'
            + '</div>'
            + '<div class="control-group">'
            + '<img alt="" id="imgLoginLoading" class="ImgLoginModel marginRight10 displayNone" src="../../Images/Loading_1.gif">'
              + '<div class="customButton">'
                + '<input type="button" id="btnLogIn" title="Log in" class="LogInModelBtn" value="Log in" onclick="Login();"><div id="divLoginPopupModel" class="displayNone" style="display: none;color:red;">Wrong credential</div>'
              + '</div>'
            + '</div>'
          + '</form>'
    + '</div>';


    closeModal('divLogin');
    $('#divLogin').remove();
    $(document.body).append(logINHTML);
    $('#divLogin').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function Login() {
    $('#divLoginPopupModel').hide();
    $('#spnEmail').html('');
    $('#spnPassword').html('');

    if ($('#txtModalEmail').val() && $('#txtModalPassword').val()) {

        var jsonPostData = {
            Email: $('#txtModalEmail').val(),
            Password: $('#txtModalPassword').val()
        }

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlLogInLogInModel,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnLogInSuccess,
            error: OnLogInFail
        });
        $('#imgLoginLoading').show();

    }
    else {
        if (!$('#txtModalEmail').val()) {
            //alert($('#spnEmail'));

            $('#spnEmail').html('*');
        }

        if (!$('#txtModalPassword').val()) {
            $('#spnPassword').html('*');
        }
    }

}

function OnLogInSuccess(result) {

    $('#imgLoginLoading').hide();
    if (result.isSuccess) {
        closeModal('divLogin');
        $('#divLogin').remove();
        if (!result.isAuthorizedVersionValid) {
            window.location.href = '../Error/Unauthorized';
        }
    }
    else {
        $('#divLoginPopupModel').show();
    }
}

function OnLogInFail(result) {

    $('#imgLoginLoading').hide();

}

function SetSelectionType(type) {
    if (type == 0) {
        _IsSelectAllParent = true;
        _IsSelectAll = false;
        $('#aSelectAll').html(_msgSelectAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    if (type == 1) {
        _IsSelectAll = true;
        _IsSelectAllParent = false;
        $('#aSelectAll').html(_msgSelectAllWithDupes + '&nbsp;&nbsp;<span class="caret"></span>');

    }
    if($("#chkInputAll").is(":checked")){
        $("#ulMediaResults input[type=checkbox]").each(function () {
            this.checked = false;
        });
        checkUncheckAll('ulMediaResults', 'chkInputAll');
    }
    
}


function ShowSaveReportPopup() {

    if (ValidateCheckBoxSelection()) {
        var checkedChecboxCount = 0;
        $("#ulMediaResults input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                checkedChecboxCount++;
            }
        });

        $('#txtReportTitle').val('');
        $('#txtReportKeywords').val('');
        $('#txtReportDescription').val('');
        $('#ddlReportCategory').val(0);
        $("#spanReportFolder").html("").hide();
        $('#ddlReportFolder option').filter(function () { return $(this).html() == rootFolderName; }).prop("selected", "selected");
        $('#ddlReportImage').val('');


        if(_MaxFeedsReportItems > 0){
            if (checkedChecboxCount > _MaxFeedsReportItems) {
                getConfirm("Max Limit Exceeded", _FeedsMaxFeedsReportItemsMessage.replace(/@@MaxFeedsReportItems@@/g, _MaxFeedsReportItems), "Confirm", "Cancel", function (res) {
                    if (res) {
                        $('#divReportPopup').modal({
                            backdrop: 'static',
                            keyboard: true,
                            dynamic: true
                        });
                    }
                });
            }
            else {
                $('#divReportPopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
        }
        else{
            getConfirm("Max Limit Exceeded", _CommonLimitExceedMessage, "Confirm", "Cancel", function (res) {
                        if (res) {
                            $('#divReportPopup').modal({
                                backdrop: 'static',
                                keyboard: true,
                                dynamic: true
                            });
                        }
                    });
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }

}

function ShowAddToReportPopup() {

    if (ValidateCheckBoxSelection()) {    
        GetFeedsReport();     

        $('#ddlReportTitle').val(0);
        $('#ddlReportTitle').removeClass('warningInput');
        $('#divAddToReportPopup').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function ValidateCheckBoxSelection() {

    var isChecked = false;
    $("#ulMediaResults input[type=checkbox]").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });
    return isChecked;

}

function ValidateParentCheckBoxSelection() {
    var isChecked = false;
    $("#ulMediaResults input[type=checkbox][id^='chkdivResults_']").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });
    return isChecked;
}

function ValidateReportInputs() {

    var isValid = true;

    $("#txtReportTitle").removeClass('warningInput');
    $("#txtReportKeywords").removeClass('warningInput');
    $("#txtReportDescription").removeClass('warningInput');
    $("#ddlReportCategory").removeClass('warningInput');
    $("#ddlReportFolder").removeClass('warningInput');

    if (!$('#txtReportTitle').val()) {
        isValid = false;
        $("#txtReportTitle").addClass('warningInput');
    }

    if (!$('#txtReportKeywords').val()) {
        isValid = false;
        $("#txtReportKeywords").addClass('warningInput');
    }

    if (!$('#txtReportDescription').val()) {
        isValid = false;
        $("#txtReportDescription").addClass('warningInput');
    }

    if ($('#ddlReportCategory').val() <= 0) {
        isValid = false;
        $("#ddlReportCategory").addClass('warningInput');
    }

    if ($("#ddlReportFolder").val() == "0") {
        isValid = false;
        $("#ddlReportFolder").addClass('warningInput');
    }

    return isValid;

}

function InsertFeedsReport() {
    if (ValidateReportInputs()) {
        $('#imgCreateReportLoading').show();
        var mediaID = new Array();
        $("#ulMediaResults input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                mediaID.push($(this).val());
            }
        });

        var jsonPostData = {
            p_Title: $('#txtReportTitle').val(),
            p_Keywords: $('#txtReportKeywords').val(),
            p_Description: $('#txtReportDescription').val(),
            p_CategoryGuid: $('#ddlReportCategory').val(),
            p_SelectedRecords: mediaID,
            p_ReportImage: $("#ddlReportImage").val(),
            p_FolderID: $("#ddlReportFolder").val(),
            p_IsAsc: _IsAsc,
            p_IsAudienceSort: _IsAudienceSort,
            p_IsSelectAllChecked: $("#chkInputAll").is(":checked"),
            p_IsSelectAll: _IsSelectAll,
            p_IsProminenceSearch: _CurrentProminenceValue != 0 && _CurrentProminenceValue != null,
            p_IsRead: _IsRead
        }

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlFeedsInsert_FeedReport,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnInsert_FeedReportSuccess,
            error: OnInsert_FeedReportFail
        });
    }
    
}

function OnInsert_FeedReportSuccess(result) {
    $('#imgCreateReportLoading').hide();    
    if (result.isSuccess) {
        ClosePopUp('divReportPopup');
        ShowNotification(result.message);
        GetFeedsReport();
        ExpandAllParents(result.childHTML);
        ClearCheckboxSelection();
    }
    else {
        ClosePopUp('divReportPopup');
        ShowErrorMessage();        
    }
}

function OnInsert_FeedReportFail(result) {
    $('#imgCreateReportLoading').hide();
    ShowErrorMessage();
}

function ClosePopUp(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).unbind().modal();
    $('#' + divID).modal('hide');
}

function GetFeedsReportLimit() {
    var jsonPostData = {
        QueryName: _QueryName,
        fromDate: _fromDate,
        ToDate: _toDate,
        searchRequestID: _RequestID,
        mediumType: _Medium,
        keyword: _Keyword,
        sentiment: _Sentiment,
        isAsc: _IsAsc
    }

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlFeedsGetFeedsReportLimit,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        global: false,
        success: OnGetFeedsReportLimitSuccess,
        error: OnGetFeedsReportLimitFail
    });
}

function OnGetFeedsReportLimitSuccess(result) {
    if (result.isSuccess) {
        _MaxFeedsReportItems = result.MaxFeedsReportItems;
    }
}

function OnGetFeedsReportLimitFail(a, b, c) {
    ShowErrorMessage(a);
}

function AddToFeedsReport() {
    if ($('#ddlReportTitle').val() > 0) {
        var selectedReport = $('#ddlReportTitle option:selected');
        if (selectedReport.attr("status") == "processing")
        {
            ShowNotification("Report still processing. Please try again later.");    
            GetFeedsReport();
        }
        else if (selectedReport.attr("status") == "failed")
        {
            ShowNotification("Report generation failed. View the Job Status page to retry.");    
            GetFeedsReport();
        }
        else
        {
            $('#ddlReportTitle').removeClass('warningInput');
            $('#imgAddToReportLoading').show();
            var mediaID = new Array();
            $("#ulMediaResults input[type=checkbox]").each(function () {
                if ($(this).is(':checked')) {
                    mediaID.push($(this).val());
                }
            });

            var jsonPostData = {
                p_ReportID: $('#ddlReportTitle').val(),
                p_RecordList: mediaID,
                p_IsAsc: _IsAsc,
                p_IsAudienceSort: _IsAudienceSort,
                p_IsSelectAllChecked: $("#chkInputAll").is(":checked"),
                p_IsSelectAll: _IsSelectAll,
                p_IsProminenceSearch: _CurrentProminenceValue != 0 && _CurrentProminenceValue != null,
                p_IsRead: _IsRead
            }

            $.ajax({

                type: 'POST',
                dataType: 'json',
                url: _urlFeedsAddToFeedsReport,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),
                global: false,
                success: OnAddToFeedsReportSuccess,
                error: OnAddToFeedsReportFail
            });
        }
    }
    else {
        $('#ddlReportTitle').addClass('warningInput');
        $('#ddlReportTitle').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlReportTitle').removeClass('boxshadow');
        }, 2000);
    }
}

function OnAddToFeedsReportSuccess(result) {
    $('#imgAddToReportLoading').hide();
    if (result.isSuccess) {
        ExpandAllParents(result.childHTML);
        ClearCheckboxSelection();
        ClosePopUp('divAddToReportPopup');
        ShowNotification(result.message + " " + _msgRecordAddedToReport);
    }
    else {
        ShowErrorMessage();
        ClosePopUp('divAddToReportPopup');
    }
}

function OnAddToFeedsReportFail(result) {
    $('#imgAddToReportLoading').hide();
    ShowErrorMessage();
}

function GetFeedsReport() {

    $("#lnkFeedReport").attr("disabled", "disabled");
    $("#lnkFeedReport").attr("class", "disablelnk");
    $('#imgRefreshReportLoading').show();

    $("#divAddtoReport").addClass("blurOnlyControls");
    $("#divAddtoReportMsg").html("Please Wait...");

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlFeedsSelectFeedsReport,
        contentType: 'application/json; charset=utf-8',
        global: false,

        success: OnSelectFeedsReportComplete,
        error: OnSelectFeedsReportFail
    });

}

function OnSelectFeedsReportComplete(result) {
    $("#imgRefreshReportLoading").hide();
    $("#lnkFeedReport").removeAttr("disabled");
    $("#lnkFeedReport").attr("class", "cursorPointer");
    if (result.isSuccess) {
        
        $("#divAddtoReport").removeClass("blurOnlyControls");
        $("#divAddtoReportMsg").html("");
        
        var reportOptions = '<option value="0">Select Report</option>';

        if(result.reportList != null){
            $.each(result.reportList, function (eventID, eventData) {
                var status = eventData.Status.toLowerCase();
                var reportTitle = EscapeHTML(eventData.Title);
                if (status == "completed" || status == "queued") {
                    reportOptions = reportOptions + '<option value="' + eventData.ID + '" style="color:#3b3b3b;">' + reportTitle + '</option>';
                }
                else if (status == "exception" || status == "failed") {
                    reportOptions = reportOptions + '<option value="' + eventData.ID + '" status="failed" style="color:#aaaaaa;">' + reportTitle + ' (Failed)</option>';
                }
                else {
                    reportOptions = reportOptions + '<option value="' + eventData.ID + '" status="processing" style="color:#aaaaaa;">' + reportTitle + ' (Processing)</option>';
                }
            });
        }

        $('#ddlReportTitle').find('option').remove().end().append(reportOptions);
    }
    else {
        if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
            RedirectToUrl(result.redirectURL);
        }
        else {
            ShowErrorMessage();
        }
    }
}

function OnSelectFeedsReportFail(a, b, c) {
    $("#imgRefreshReportLoading").hide();
    $("#lnkFeedReport").removeAttr("disabled");
    $("#lnkFeedReport").attr("class", "cursorPointer");
    ShowErrorMessage(a);
}

function ShowAddToLibraryPopup(itemid) {
    var isvaid = true;
    if (typeof (itemid) === 'undefined') {
        isvaid = ValidateCheckBoxSelection();
        $('#hdnfeedid').val('');
    }
    else {
        $('#hdnfeedid').val(itemid);
    }
    if (isvaid) {
        
        $('#txtLibraryKeywords').removeClass('warningInput');
        $('#txtLibraryDescription').removeClass('warningInput');
        $('#ddlLibraryCategory').removeClass('warningInput');

        $('#txtLibraryKeywords').val('');
        $('#txtLibraryDescription').val('');
        $('#ddlLibraryCategory').val(0);

        $('#divAddToLibraryPopup').modal({
            backdrop: 'static',
            keyboard: true,
            dynamic: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function ValidateLibraryInputs() {

    var isValid = true;

    $('#txtLibraryKeywords').removeClass('warningInput');
    $('#txtLibraryDescription').removeClass('warningInput');
    $('#ddlLibraryCategory').removeClass('warningInput');

    if ($.trim($('#txtLibraryKeywords').val()) == '') {
        isValid = false;
        $('#txtLibraryKeywords').addClass('warningInput');
        $('#txtLibraryKeywords').addClass('boxshadow');

        setTimeout(function () {
            $('#txtLibraryKeywords').removeClass('boxshadow');
        }, 2000);
    }

    if ($.trim($('#txtLibraryDescription').val()) == '') {
        isValid = false;
        $('#txtLibraryDescription').addClass('warningInput');
        $('#txtLibraryDescription').addClass('boxshadow');

        setTimeout(function () {
            $('#txtLibraryDescription').removeClass('boxshadow');
        }, 2000);
    }

    if ($('#ddlLibraryCategory').val() == 0) {
        isValid = false;
        $('#ddlLibraryCategory').addClass('warningInput');
        $('#ddlLibraryCategory').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlLibraryCategory').removeClass('boxshadow');
        }, 2000);
    }

    return isValid;

}

function AddToFeedsLibrary() {

    if (ValidateLibraryInputs()) {

        $('#txtLibraryKeywords').removeClass('warningInput');
        $('#txtLibraryDescription').removeClass('warningInput');
        $('#ddlLibraryCategory').removeClass('warningInput');

        $('#imgAddToLibraryLoading').show();

        var mediaID = new Array();
        if ($('#hdnfeedid').val() != '') {
            mediaID.push($('#hdnfeedid').val());
        }
        else {
            $("#ulMediaResults input[type=checkbox]").each(function () {
                if ($(this).is(':checked')) {
                    mediaID.push($(this).val());
                }
            });
        }

        var jsonPostData = {
            p_Keywords: $.trim($('#txtLibraryKeywords').val()),
            p_Description: $.trim($('#txtLibraryDescription').val()),
            p_CategoryGuid: $('#ddlLibraryCategory').val(),
            p_RecordList: mediaID,
            p_IsAsc: _IsAsc,
            p_IsAudienceSort: _IsAudienceSort,
            p_IsSelectAllChecked: $("#chkInputAll").is(":checked"),
            p_IsSelectAll: _IsSelectAll,
            p_IsProminenceSearch: _CurrentProminenceValue != 0 && _CurrentProminenceValue != null,
            p_IsRead: _IsRead
        }
        
        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlFeedsAddToFeedsLibrary,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnAddToFeedsLibrarySuccess,
            error: OnAddToFeedsLibraryFail
        });
    }
}

function OnAddToFeedsLibrarySuccess(result) {
    $('#imgAddToLibraryLoading').hide();
    if (result.isSuccess) {
        ExpandAllParents(result.childHTML);
        ClearCheckboxSelection();
        ClosePopUp('divAddToLibraryPopup');
        ShowNotification(result.message + " " + _msgRecordAddedToLibrary);

    }
    else {
        ShowErrorMessage();
        ClosePopUp('divAddToReportPopup');
    }
}

function OnAddToFeedsLibraryFail(result) {
    $('#imgAddToLibraryLoading').hide();
    ShowErrorMessage();
}

function LoadTMPopup(IframeUrl) {
    $('#divLoadTMIframePopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
    $('#iFrameFeedsTM').attr('src', IframeUrl);

    $("#divLoadTMIframePopup").resizable({
        handles : 'e,se,s,w',
        iframeFix: true,
        start: function(){
            ifr = $('#iFrameFeedsTM');
            var d = $('<div></div>');

            $('#divLoadTMIframePopup').append(d[0]);
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
            $("#iFrameFeedsTM").width(newWd).height(newHt);
        }
    }).draggable({
        iframeFix: true,
        start: function(){
            ifr = $('#iFrameFeeds');
            var d = $('<div></div>');

            $('#divLoadTMIframePopup').append(d[0]);
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

    $("#divLoadTMIframePopup").css('height', documentHeight - 250);
    $("#iFrameFeedsTM").css('height', documentHeight - 250);
}

function ExportFeedsToCSV() {
    if (ValidateCheckBoxSelection()) {
        var mediaID = new Array();
        var isSelectAll = false;
        var isSelectAllParent = false;
        if ($("#chkInputAll").is(":checked")) {
            if (_IsSelectAll) {
                isSelectAll = true;
            }
            else {
                isSelectAllParent = true;
            }

        }
        else {
            $("#ulMediaResults input[type=checkbox]").each(function () {
                if ($(this).is(':checked')) {
                    mediaID.push($(this).val());
                }
            });
            isSelectAll = false;
        }

        var jsonPostData = {
            p_RecordList: mediaID,
            p_SelectAll: isSelectAll,
            p_SelectAllParent: isSelectAllParent,
            fromDate: _fromDate,
            ToDate: _toDate,
            searchRequestID: _RequestID,
            mediumTypes: _Medium,
            keyword: _Keyword,
            sentiment: _Sentiment,
            isAsc: _IsAsc,
            Dma: _Dma,
            Station : _Station,
            CompeteUrl: _CompeteUrl,
            _IQDMAID: _IQDMAID,
            Handle: _TwitterHandle,
            publication: _Publication,
            author: _Author,
            isRead: _IsRead,
            prominenceValue: _ProminenceValue,
            isProminenceAudience: _IsProminenceAudience,
            isAudienceSort: _IsAudienceSort
        }

        if ((isSelectAll || isSelectAllParent) || ((isSelectAll == false  && isSelectAllParent == false) && mediaID.length > parseInt($("#hdnMaxFeedsExportCSVLimit").val()))) {
            var confirmMsgExport = '';
            var confirmTitleExport = '';
            if (isSelectAll || isSelectAllParent) {
                confirmMsgExport = _FeedsSelectAllMaxFeedsExportItemsMessage
                confirmTitleExport = 'Max Items Export Confirmation'
            }
            else {
                confirmMsgExport = _FeedsMaxFeedsExportItemsMessage
                confirmTitleExport = 'Max Limit Exceeded'
            }
            getConfirm(confirmTitleExport, confirmMsgExport.replace(/@@MaxFeedsExportItems@@/g, parseInt($("#hdnMaxFeedsExportCSVLimit").val())), "Confirm", "Cancel", function (res) {
                if (res) {
                    ShowLoading();

                    $.ajax({

                        type: 'POST',
                        dataType: 'json',
                        url: _urlFeedsExportCSV,
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(jsonPostData),
                        global: false,
                        success: function (result) {
                            HideLoading();
                            if (result.isSuccess) {
                                ClearCheckboxSelection()
                                window.location = "/Feeds/DownloadCSVFile/";
                            }
                            else {
                                ShowNotification(result.errorMessage);
                            }
                        },
                        error: function (a, b, c) {
                            HideLoading();
                            ShowErrorMessage();
                        }
                    });
                }
            });
        }
        else {
            ShowLoading();

            $.ajax({

                type: 'POST',
                dataType: 'json',
                url: _urlFeedsExportCSV,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),
                global: false,
                success: function (result) {
                    HideLoading();
                    if (result.isSuccess) {
                        ClearCheckboxSelection()
                        window.location = "/Feeds/DownloadCSVFile/";
                    }
                    else {
                        ShowNotification(result.errorMessage);
                    }
                },
                error: function (a, b, c) {
                    HideLoading();
                    ShowErrorMessage();
                }
            });
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function showPopupOver() {
    if ($('#divPageSizePopover').length <= 0) {

        var drphtml = $("#divPageSizeDropDown").html();
        $('#aPageSize').popover({
            trigger: 'manual',
            html: true,
            title: '',
            placement: 'top',
            template: '<div id="divPageSizePopover" class="popover" style="width:125px;"><div class="popover-inner"><div class="popover-content" style="padding:0px;" ><p></p></div></div></div>',
            content: drphtml
        });
        $('#aPageSize').popover('show');
    }
    else {
        $('#divPageSizePopover').remove();
    }
}

function ExcludeDomains() {
    if (ValidateCheckBoxSelection()) {
        var lstOfDomains = "";
        var lstOfHandles = "";

        var mediaID = [];
        $("#ulMediaResults input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                var value = $(this).val();
                var id = value.substring(0, value.indexOf(":"));

                if ($("#lblCompete_" + id).length > 0) {
                    if (lstOfDomains.indexOf($("#lblCompete_" + id).html().replace(" -", "")) < 0) {
                        lstOfDomains = lstOfDomains + "<div class=\"tmStationDiv width100p float-left\">" + $("#lblCompete_" + id).html().replace(" -", "") + "</div>";
                    }
                    mediaID.push(id);
                }
                else if ($("#lblPrefferedUserName_" + id).length > 0) {
                    if (lstOfHandles.indexOf($("#lblPrefferedUserName_" + id).html()) < 0) {
                        lstOfHandles = lstOfHandles + "<div class=\"tmStationDiv width100p float-left\">" + "@" + $("#lblPrefferedUserName_" + id).html() + "</div>";
                    }
                    mediaID.push(id);
                }
            }
        });

        var strDomains = "";
        if (lstOfDomains != "") {
            strDomains = strDomains + "<div class=\"twodivsinline\"><b>Domains</b>" + lstOfDomains + "</div>";
        }

        if (lstOfHandles != "") {
            strDomains = strDomains + "<div class=\"twodivsinline\"><b>Twitter Handles</b>" + lstOfHandles + "</div>";
        }
        if (mediaID.length > 0) {
            getConfirm("Internet Domains or Twitter Handle Exclusion", "<div>" + _msgConfirmExclude + "</div>" + strDomains, "Confirm", "Cancel", function (res) {
                if (res) {
                    ShowLoading();
                    var jsonPostData = {
                        p_MediaID: mediaID,
                        p_SearchRequestIds: _RequestID
                    }

                    $.ajax({
                        type: 'POST',
                        dataType: 'json',
                        url: _urlFeedsExcludeDomains,
                        contentType: 'application/json; charset=utf-8',
                        data: JSON.stringify(jsonPostData),
                        success: function (result) {
                            HideLoading();
                            if (result.isSuccess) {
                                ClearCheckboxSelection()
                                ShowNotification(_msgDomainsExcluded);
                            }
                            else {
                                ShowErrorMessage();
                            }
                        },
                        error: function (a, b, c) {
                            HideLoading();
                            ShowErrorMessage();
                        }
                    });
                }
            });
        }
        else {
            ShowNotification(_msgInvalidArticlesForExcludeDomains);
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function ShowChild(parentid, mediaType) {
    
    var divChildMedia = $('#divChildMedia_' + parentid);

    if (divChildMedia.length)
    {
        if (divChildMedia.is(':visible')) {
            divChildMedia.hide();
            $('#expand_' + parentid).attr('src', '../images/expand.png');        
        }
        else
        {
            divChildMedia.show();
            $('#expand_' + parentid).attr('src', '../images/collapse.png');
        }
    }
    else {
        var isParentChecked = $("#chkdivResults_" + parentid).is(":checked");

        var jsonPostData = {
            parentID: parentid,
            mediaType: mediaType,
            isAsc: _IsAsc,
            isAudienceSort: _IsAudienceSort,
            isRead: _IsRead
        }

        $.ajax({

            url: _urlFeedsGetChildResults,
            type: "post",
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    if (result.isValidResponse)
                    {
                        $("#divMedia_" + parentid).html("");
                        $("#divMedia_" + parentid).html(result.html);

                        if ($("#chkInputAll").is(":checked"))
                        {
                            if (_IsSelectAllParent)
                            {
                                $("#divMedia_" + parentid + " input[type=checkbox][id^='chkdivResults_']").prop("checked", true);
                            }
                            else if (_IsSelectAll)
                            {
                                $("#divMedia_" + parentid + " input[type=checkbox]").prop("checked", true);
                            }
                        }
                        else if (isParentChecked)
                        {
                            $("#divMedia_" + parentid + " input[type=checkbox][id^='chkdivResults_']").prop("checked", true);                            
                        }
                    }
                    else
                    {
                        ShowNotification(_msgFeedsSolrTimeout); 
                    }
                }
                else {
                    ShowErrorMessage();
                }
            },
            error: function (a, b, c) {
                ShowErrorMessage(a);
            }
        });
    }
}

function ShowSaveMissingArticlePopup() {
    $('#txtArtcileTitle').val('');
    $('#txtArtcileContent').val('');
    $('#ddlArtcileSearchRequest').val(0);
    $('#txtArtcileUrl').val('');
    $('#txtArticleGMTDate').val('');
    $('#ddlArtcileCategory').val(0);

    //populate library categories
    var categoryOptions = '<option value="0">Select Category</option>';
    $.each(customCategoryObject, function (eventID, eventData) {
        categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + EscapeHTML(eventData.CategoryName) + '</option>';
    });
    $('#ddLibraryCategory').html(categoryOptions);

    //always trigger check event
    $('#chkArticleAddToLibrary').prop('checked', false);
    $('#chkArticleAddToLibrary').trigger('click');

    $('#txtArticleGMTDate').datetimepicker('setDate', (new Date()));
    
    $("#spanArtcileTitle").html("");
    $("#spanArtcileUrl").html("");
    $("#spanArtcileSearchRequest").html("");
    $("#spanArticleGMTDate").html("");
    $("#spanArtcileCategory").html("");
    $("#spanArtcileContent").html("");
    $("#spanLibraryCategory").html("");
	
    // Certain users had a problem with the Save Missing Article popup disabling the Save button after every successful save.
    // I couldn't recreate the problem to investigate it, but I confirmed that this fixes the issue. --CFurst
    $("#divSaveMissingArticlePopup #btnCreateReport").removeAttr('disabled', 'disabled');

    $('#divSaveMissingArticlePopup').modal({
        backdrop: 'static',
        keyboard: true,
        dynamic: true
    });
}

function SaveMissingArticle() {
    var LibraryCat = null;
    if($("#chkArticleAddToLibrary").prop("checked")) LibraryCat = $("#ddLibraryCategory").val();

    if (ValidateMissingArticle()) {
        var jsonPostData = {
            Title: $.trim($("#txtArtcileTitle").val()),
            _SearchRequestID: $("#ddlArtcileSearchRequest").val(),
            Content: $.trim($("#txtArtcileContent").val()),
            Url: $.trim($("#txtArtcileUrl").val()),
            harvest_time: $.trim($("#txtArticleGMTDate").val()),
            Category: $("#ddlArtcileCategory").val(),
            AddToLibrary: $("#chkArticleAddToLibrary").is(":checked"),
            LibraryCategory: LibraryCat
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlFeedsInsertMissingArticle,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: function (result) {
                if (result.isSuccess) {
                    ClearCheckboxSelection()
                    ClosePopUp('divSaveMissingArticlePopup');
                    ShowNotification(result.msg);
                }
                else {
                    ClosePopUp('divSaveMissingArticlePopup');
                    ShowNotification(result.msg);
                }
            },
            error: function (a, b, c) {
                ClosePopUp('divSaveMissingArticlePopup');
                ShowErrorMessage();
            }
        });
    }
}

function ValidateMissingArticle() {
    var flag = true;

    $("#spanArtcileTitle").html("");
    $("#spanArtcileUrl").html("");
    $("#spanArtcileSearchRequest").html("");
    $("#spanArticleGMTDate").html("");
    $("#spanArtcileCategory").html("");
    $("#spanArtcileContent").html("");
    $("#spanLibraryCategory").html("");

    if ($.trim($("#txtArtcileTitle").val()) == "") {
        $("#spanArtcileTitle").html(_msgTitleRequired).show();
        flag = false;
    }

    if ($.trim($("#txtArtcileUrl").val()) == "") {
        $("#spanArtcileUrl").html(_msgUrlRequired).show();
        flag = false;
    }
    else if(!ValidateUrl($.trim($("#txtArtcileUrl").val())))
    {
        $("#spanArtcileUrl").html(_msgInvalidUrl).show();
        flag = false;
    }

    if ($("#ddlArtcileSearchRequest").val() == "0") {
        $("#spanArtcileSearchRequest").html(_msgIQAgentRequired).show();
        flag = false;
    }

    if ($("#ddlArtcileCategory").val() == "0") {
        $("#spanArtcileCategory").html(_msgSourceTypeRequired).show();
        flag = false;
    }

    if ($.trim($("#txtArticleGMTDate").val()) == "") {
        $("#spanArticleGMTDate").html(_msgGMTDateRequired).show();
        flag = false;
    }

    if ($.trim($("#txtArtcileContent").val()) == "") {
        $("#spanArtcileContent").html(_msgContentRequired).show();
        flag = false;
    }

    if($("#chkArticleAddToLibrary").prop("checked") && $("#ddLibraryCategory").val() == "0"){
        $("#spanLibraryCategory").html(_msgCategoryRequired).show();
        flag = false;
    }

    return flag;
}

function SetMediaClickEvent() {
    $("#divMainContent .media").click(function (e) {
        if ($(e.target).closest("a").length <= 0 && e.target.type != "checkbox") {
            e.stopPropagation();
            if ($(e.target).closest('.media').find('input').is(':checked')) {
                $(e.target).closest('.media').find('input').removeAttr('checked');
                $(this).css("background", "");
            }
            else {
                $(e.target).closest('.media').find('input').prop('checked', true);
                $(this).css("background", "#F4F4F4");
            }
        }
    });
}

function BuildDashboard() {    
    if (ValidateCheckBoxSelection()) {    
        if ($("#chkInputAll").is(":checked")) {    
            _MediaIDs = null;

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlFeedsGetSinceID,
                contentType: 'application/json; charset=utf-8',
                global: false,
                success: function (result) {
                    if (result.isSuccess) {
                        OpenDashboardPopup("Feeds", _IsSelectAllParent, result.sinceID);
                    }
                    else {
                        ShowErrorMessage();
                    }
                },
                error: function (a, b, c) {
                    ShowErrorMessage();
                }
            });
        }
        else {
            _MediaIDs = $.map($("#ulMediaResults input[type=checkbox]:checked"), function (n, i) {
                return n.value.substring(0, n.value.indexOf(":"));
            });

            OpenDashboardPopup("Feeds", false, 0);
        }
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

// Called when performing an action, to automatically expand all displayed parent records
function ExpandAllParents(childHTML)
{
    for (var parentID in childHTML)
    {
        if (childHTML.hasOwnProperty(parentID))
        {
            $("#divMedia_" + parentID).html('');
            $("#divMedia_" + parentID).html(childHTML[parentID]);

            $("#divMedia_" + parentID + " div[id^='divChildMedia_']").hide();
            $("#divMedia_" + parentID + " img[id^='expand_']").attr('src', '../images/expand.png');    

            if (_IsSelectAllParent)
            {
                $("#divMedia_" + parentID + " input[type=checkbox][id^='chkdivResults_']").prop("checked", true);
            }
            else if (_IsSelectAll)
            {
                $("#divMedia_" + parentID + " input[type=checkbox]").prop("checked", true);
            }
        }
    }
}

function ShowViewArticleFeeds(mediaID) {
    var jsonPostData = {
        mediaID: mediaID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlFeedsGetProQuestResultByID,
        data: JSON.stringify(jsonPostData),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.isSuccess) {
                $("#divPQTitle").html(result.title);
                $("#divPQMediaDate").html(result.mediaDate);
                $("#divPQAuthor").html(result.authors);
                $("#divPQPublication").html(result.publication);
                $("#divPQContent").html(result.content);
                $("#divPQCopyright").html(result.copyright);

                $('#divViewArticlePopup').modal({
                    backdrop: 'static',
                    keyboard: true,
                    dynamic: true
                });
            }
            else {
                ShowNotification(result.errorMessage);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}

function CloseViewArticlePopup()
{    
    $('#divViewArticlePopup').css({ "display": "none" });
    $('#divViewArticlePopup').unbind().modal();
    $('#divViewArticlePopup').modal('hide');
}

function UpdateIsRead(isRead) {
    if (ValidateParentCheckBoxSelection()) {
        var needConfirmation = false;

        // Check if any child records are selected without having their parent selected
        $("#ulMediaResults input[type=checkbox][id^='chkdivChildResults_']:checked").each(function () {
            var divParent = $(this).closest("div[id^='divChildMedia_']").closest(".media")[0];
            if (!$(divParent).find("input[type=checkbox][id^='chkdivResults_']").is(":checked")) {
                needConfirmation = true;
            }
        });

        // Marking a child record as read/unread without updating the parent will lead to orphaned children and incorrect counts, so don't allow it and warn the user if they try
        if (needConfirmation) {
            getConfirm("Update Records", "The parent item’s read/unread status applies to both the parent item and the duplicate items.  Selected duplicate items will be ignored if the parent item is not selected. Do you want to continue? ", "Continue", "Cancel", function (res) {
                if (res) {
                    UpdateIsRead_Helper(isRead);
                }
            });
        }
        else {
            UpdateIsRead_Helper(isRead);
        }
    }
    else {
        ShowNotification(_msgAtleastOneParentRecordSelect);
    }
}

function UpdateIsRead_Helper(isRead) {
    var mediaIDs = [];

    $("#ulMediaResults input[type=checkbox][id^='chkdivResults_']").each(function () {
        if ($(this).is(':checked')) {
            var value = $(this).val();
            mediaIDs.push(value.substring(0, value.indexOf(":")));
        }
    });

    var jsonPostData = {
        mediaIDs: mediaIDs,
        isRead: isRead,
        isSelectAll: $("#chkInputAll").is(":checked") && _IsSelectAll
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlFeedsUpdateIsRead,
        data: JSON.stringify(jsonPostData),
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result.isSuccess) {
                var successMsg = isRead ? _msgFeedsMarkReadSuccess : _msgFeedsMarkUnreadSuccess;

                // If currently filtering to read/unread and articles were marked as the opposite, reload the page so they disappear
                if (_IsRead != null && _IsRead != isRead) {
                    _ScrollToTop = false;
                    CallHandler(_CurrentNumPages, result.recordCount + successMsg);
                }
                else {
                    if (result.childHTML) {
                        ExpandAllParents(result.childHTML);
                    }
                    ShowNotification(result.recordCount + successMsg);
                }
            }
            else {
                ShowNotification(result.errorMessage);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgErrorOccured);
        }
    });
}
