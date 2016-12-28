var IsChartActive = 1;
var IsResultInitialLoad = 1;
var IsChartUpdated = false;
var searchTermCount = 0;
var _IsTabChange = false;
var _searchTerm = new Array();
var _searchTermbkp = '';
var _SearchTermValidationMessage = '';
var disabledDays = new Array();
var _SaveSearchTitle = '';

var _IsToggle = false;

var _SearchDate = '';
var _SearchDatebkp = '';
var _fromDate = '';
var _toDate = '';

var _SearchMedium = '';
var _SearchMediumDesc = '';
var _SearchMediumbkp = '';

var _SearchTVMarket = '';
var _SearchTVMarketbkp = '';

var _SearchTermIndex = 0;

var _SearchTermResult = '';
var msg = '';
var _NeedToValidateSearchTerm = true;
var _imgLoading = '<img alt="" id="imgSaveSearchLoading" class="marginRight10" src="../../Images/Loading_1.gif" />'
var _DateMessage = '';
var _ChartDate = '';
var _IsDefaultLoad = true;

var _MaxDiscoveryReportItems = 0;

function AddNewSearchTermTextBox() {
    $('#divPopover').remove();
    searchTermCount = searchTermCount + 1;
    $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + searchTermCount + "\" ><img id=\"imgRemoveSearchTermTextBox_" + searchTermCount + "\" src=\"../../Images/Delete.png\" alt=\"\" onclick=\"$('#lisearchTerm_" + searchTermCount + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Term\" id=\"txtSearchTerm_" + searchTermCount + "\" onclick=\"ShowAddSearchTermPopup(this.id);\" readonly=\"readonly\" /></li>");

    ShowAddSearchTermPopup("txtSearchTerm_" + searchTermCount);
    CheckForMaxSearchTerm();
}

function CheckForMaxSearchTerm() {
    if ($('#ulSearchTerm li').length >= 5) {
        $('#divAddItem').hide();
    }
    else {
        $('#divAddItem').show();
    }
}


function ShowAddSearchTermPopup(elementID) {

    $('#divPopover').remove();
    $('#' + elementID).popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divPopover" class="popover width50p"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
        content: '<div><input type=\"text\" class=\"popOverTextBox\" placeholder=\"Search Term\" id=\"txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '') + '\" onkeypress="GetChartData(event);" onblur="SetCurrentSearchTermTextBox(txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '') + ',txtSearchTerm_' + elementID.replace('txtSearchTerm_', '') + ');" /></div>'
    });


    $('#' + elementID).popover('show');
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).focus();
    $('#txtSearchTermPopup_' + elementID.replace('txtSearchTerm_', '')).val($('#' + elementID).val());

}

var customCategoryObject = '';
var discoveryReportListObject = "";

$(document).ready(function () {

    //RenderLineChart('');

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuDiscovery').attr("class", "active");

    $("#dpFrom").datepicker({
        //beforeShowDay: enableAllTheseDays,
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        minDate: new Date(_constTVContentMinDate),
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            /*$('#dpFrom').val(dateText);
            SetDateVariable();*/
        },
        onClose: function (dateText) {
            //$('#dpFrom').focus();
            // _fromDate = dateText;
            _SearchDate = '';
            SetDateVariable();
        }
    });

    $("#dpTo").datepicker({
        //beforeShowDay: enableAllTheseDays,        
        showOn: "button",
        buttonImage: "../images/calender.png",
        buttonImageOnly: true,
        minDate: new Date(_constTVContentMinDate),
        changeMonth: true,
        changeYear: true,
        onSelect: function (dateText, inst) {
            /*$('#dpTo').val(dateText);
            SetDateVariable();*/
        },
        onClose: function (dateText) {
            //$('#dpTo').focus();
            //_toDate = dateText;
            _SearchDate = '';
            SetDateVariable();
        }
    });

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());


    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    $("#divCalender").datepicker({
        minDate: new Date(_constTVContentMinDate),
        changeMonth: true,
        changeYear: true,
        beforeShowDay: enableAllTheseDays,
        onSelect: function (dateText, inst) {
            IsChartUpdated = false;
            ResetSearchTermClassToFalse();
            _SearchDate = dateText;
            $("#dpFrom").datepicker("setDate", dateText);
            $("#dpTo").datepicker("setDate", dateText);
            SetDateVariable();
            $('#ulCalender').parent().removeClass('open');
        }
    });

    $('#imgResult').hover(function () {
        $('#imgResult').attr('src', '../../Images/Result-hover.png');
    }, function () {
        $('#imgResult').attr('src', '../../Images/Result.png');
    });

    $('#imgChart').hover(function () {
        $('#imgChart').attr('src', '../../Images/Chart-hover.png');
    }, function () {
        $('#imgChart').attr('src', '../../Images/Chart.png');
    });

    AddNewSearchTermTextBox();

    $("#divPieChartHeader").delegate('div', 'click', function () {
        ShowPieChart($(this).index());
    });


    $("#divResultHeader").delegate('div', 'click', function () {

        _SearchTermIndex = $(this).index();
        GetDataOnTabChange();
    });


    /*var documentHeight = $(window).height();
    $('#divMainContent').css({ 'height': documentHeight - 200 });
    //$('#mCSB_1').css({ 'max-height': '' });
    $("#divMainContent").mCustomScrollbar({
    advanced: {
    updateOnContentResize: true,
    autoScrollOnFocus: false
    }
    });*/

    $('.ndate').click(function () {
        $("#divCalender").datepicker("refresh");
    });

    if (customCategoryObject == '') {

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlCommonBindCategoryDropDown,
            contentType: 'application/json; charset=utf-8',
            global: false,

            success: OnCategoryBindComplete,
            error: OnCategoryBindFail
        });
    }

    function OnCategoryBindComplete(result) {

        if (result.isSuccess) {

            customCategoryObject = result.customCategory;
            var categoryOptions = '<option value="0">Select Category</option>';
            $.each(customCategoryObject, function (eventID, eventData) {
                categoryOptions = categoryOptions + '<option value="' + eventData.CategoryGUID + '">' + eventData.CategoryName + '</option>';
            });

            $('#ddlReportCategory').append(categoryOptions);
            $('#ddlLibraryCategory').append(categoryOptions);
        }
        else {
            if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
                RedirectToUrl(result.redirectURL);
            }
            else {
                ShowNotification(_msgErrorOccured);
            }
        }

    }

    function OnCategoryBindFail(result) {
        //alert('Category Bind Fail  -- Local');
        ShowNotification(_msgErrorOccured);
    }

    GetSavedSearch(false, true);
    GetDiscoveryReportLimit();
    GetDiscoveryReport();

});

function SetMedium(mediumName, mediumDesc) {
    if (mediumName != "TV") {
        _SearchTVMarket = '';
        _SearchMediumDesc = '';
    }
    //ClearChartDateVariable();

    _SearchMedium = mediumName;
    _SearchMediumDesc = mediumDesc;
    IsChartUpdated = false;
    ResetSearchTermClassToFalse();

    SearchResult();
}

function SetTVMarket(tvMarket) {
    //ClearChartDateVariable();
    _SearchTVMarket = tvMarket;

    IsChartUpdated = false;
    ResetSearchTermClassToFalse();

    SearchResult();
}

function SetCurrentSearchTermTextBox(popupControlID, searchTermTextBoxID) {

    //alert('blur');
    //alert(_NeedToValidateSearchTerm);
    //setTimeout(function () {
    $('#' + searchTermTextBoxID.id).val($('#' + popupControlID.id).val());

    //if (_NeedToValidateSearchTerm)
    $('#divPopover').remove();
    PushSearchTermintoArray();
    //}, 1000)
}

function PushSearchTermintoArray() {
    //if (_NeedToValidateSearchTerm) {
    $('#divNoDataChart').html('');
    //$('#divNoDataResult').html('');

    _SearchTermIndex = 0;
    _searchTerm = new Array();

    IsChartUpdated = false;
    _IsToggle = false;

    var isCurrentSearchTermInArray = false;
    var index = 0;
    //var sTermID = 1;
    $('#ulSearchTerm li input[type=text]').each(function () {
        if (_searchTerm.length < 5) {

            if ($(this).val().trim() == _SearchTermResult) {
                _SearchTermIndex = index;
                isCurrentSearchTermInArray = true;
            }
            index = index + 1;
            var SearchTermClass = new Object();
            SearchTermClass.SearchTerm = $(this).val().trim();
            SearchTermClass.ResultShown = false;
            SearchTermClass.IsCurrentTab = false;

            SearchTermClass.ShownRecords = 0;
            SearchTermClass.AvailableRecords = 0;
            SearchTermClass.TotalRecords = 0;
            SearchTermClass.DisplayPageSize = 0;

            /*SearchTermClass.ID = sTermID;
            sTermID += 1;*/
            _searchTerm.push(SearchTermClass);

        }
    });

    if (!isCurrentSearchTermInArray && _searchTerm.length > 0) {
        _SearchTermResult = _searchTerm[0].SearchTerm;
    }

    GenerateSearchTermTab();
    //ClearChartDateVariable();

    if (_searchTerm.length > 0) {
        SearchResult();
    }
    else {
        $('#divDiscoveryClearAll').hide();
        $('#divDiscoveryUtility').hide();
        ClearAllData();
    }
    // }
    //_NeedToValidateSearchTerm = true;
}

function ClearAllData() {
    $('#divColumnChart').html('');
    $('#divLineChart').html('');
    $('#divPieChartHeader').html('');

    $('#divPieChartData').html('');
    $('#divPieChartData').removeAttr('style');

    $('#divChartTotal').html('');
    $('#divResult').html('');

    $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    $('#ulTVMarket').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    disabledDays = [];
    _SearchMedium = ''
    _SearchMediumDesc = '';
    _SearchTVMarket = '';
    _SearchDate = '';
    /*_fromDate = '';
    _toDate = '';*/
    $("#dpFrom").datepicker('option', 'beforeShowDay', '');
    $("#dpTo").datepicker('option', 'beforeShowDay', '');
    $('#divActiveFilter').html('');
}

function SearchResult() {
    _SearchTermValidationMessage = '';
    if (DateValidation()) {
        if (ValidateSearchTerm()) {
            _IsDefaultLoad = false;
            if (ShowDateRangeExpansion()) {
                $('#divDiscoveryClearAll').show();
                SearchResultAjaxRequest();
            }
            else {
                getConfirm("Confirm Search", _DateMessage, "Continue", "Cancel", function (res) {
                    if (res == true) {
                        $('#divDiscoveryClearAll').show();
                        SearchResultAjaxRequest();
                    }
                });
            }
        }
        else {
            //$('#divDiscoveryClearAll').hide();
            ShowNotification(_SearchTermValidationMessage);
        }
    }
    else {
        //$('#divDiscoveryClearAll').hide();
    }
}

function OnResultSearchFail(result) {
    //alert('OnResultSearch Fail - Local');
    _IsTabChange = false;
    _IsToggle = false;

    ShowNotification(_msgErrorOccured);
}
function OnResultSearchComplete(result) {


    $('#divResult_Child_NoData_' + result.searchedIndex).html('');

    /*if (result.notAvailableDataResult) {
    SetNoDataAvailableMessage(result.notAvailableDataResult, 'divResult_Child_NoData_' + result.searchedIndex);
    }*/

    if (result.availableDataResult) {
        SetNoDataAvailableMessage(result.availableDataResult, 'divResult_Child_NoData_' + result.searchedIndex);
    }

    if (result.isSuccess) {

        $('#divDiscoveryUtility').show();

        SetDiscoveryResultStatus(result);

        // for (var i = 0; i < _searchTerm.length; i++) {
        _searchTerm[result.searchedIndex].IsCurrentTab = false;

        // if (_searchTerm[i].SearchTerm.trim() == result.searchedTerm) {

        _searchTerm[result.searchedIndex].IsCurrentTab = true;
        _searchTerm[result.searchedIndex].ResultShown = true;
        _searchTerm[result.searchedIndex].ShownRecords = result.searchTermShownRecords;
        _searchTerm[result.searchedIndex].AvailableRecords = result.searchTermAvailableRecords;
        _searchTerm[result.searchedIndex].TotalRecords = result.searchTermTotalRecords;
        _searchTerm[result.searchedIndex].DisplayPageSize = result.displayPageSize;


        $('#divResult_Child_Data_' + result.searchedIndex + ' > div').slice(0, _searchTerm[result.searchedIndex].ShownRecords).removeClass("displayNone");

        if ((_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].AvailableRecords)
               ||
                (_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].TotalRecords)) {
            result.hasMoreResults = true;
        }
        else {
            result.hasMoreResults = false;
        }
        //}
        //}


        $('#divMoreResult_' + result.searchedIndex).remove();
        $('#divResult_Child_Data_' + result.searchedIndex).append('<div id="divMoreResult_' + result.searchedIndex + '" align="center"><img alt="" id="imgMoreResultLoading_' + result.searchedIndex + '" class="marginRight10 visibilityHidden" src="../../Images/Loading_1.gif"><input value="No More Results" class="loadmore displayNone" id="btnShowMoreResults_' + result.searchedIndex + '" style="display: inline;" type="button"></div></div>');
        $('#btnShowMoreResults' + result.searchedIndex).show();
        if (!result.hasMoreResults) {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgNoMoreResult);
            $('#btnShowMoreResults_' + result.searchedIndex).removeAttr('onclick');

        }
        else {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgShowMoreResults);
            $('#btnShowMoreResults_' + result.searchedIndex).attr('onclick', 'ShowMoreResult();');
        }

        var documentHeight = $(window).height();
        $('#divResult_Child_Scroll_' + result.searchedIndex).css({ 'height': documentHeight - 200 });
        $('#divResult_Child_Scroll_' + result.searchedIndex).mCustomScrollbar("destroy");
        $('#divResult_Child_Scroll_' + result.searchedIndex).mCustomScrollbar({
            advanced: {
                updateOnContentResize: true,
                autoScrollOnFocus: false
            }
        });

        setTimeout(function () {
            $('#divResult_Child_Scroll_' + result.searchedIndex).mCustomScrollbar("scrollTo", "top");
        }, 200);

        _SearchDatebkp = _SearchDate;
        _searchTermbkp = _searchTerm;
        _SearchMediumbkp = _SearchMedium;
        _SearchTVMarketbkp = _SearchTVMarket;

        if (!_IsToggle) {
            $('#divChartTotal').html('Total Records :: ' + result.chartTotal);
            RenderColumnChart(result.columnChartJson);
            RenderLineChart(result.lineChartJson);
            RenderPieChart(result.pieChartJson);
            SetDateFilter(result);
            SetMediumFilter(result);
            SetTVMarketFilter(result);

            $('#divNoDataChart').html('');

            if (result.availableDataChart) {
                SetNoDataAvailableMessage(result.availableDataChart, 'divNoDataChart');
            }
        }
        _IsTabChange = false;

    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }

    _IsToggle = false;

}

function OnMediaSearchComplete(result) {

    /*$("#dpFrom").datepicker('option', 'beforeShowDay', enableAllTheseDays);
    $("#dpTo").datepicker('option', 'beforeShowDay', enableAllTheseDays);*/


    IsChartUpdated = true;
    $('#divNoDataChart').html('');

    if (result.availableDataChart) {
        SetNoDataAvailableMessage(result.availableDataChart, 'divNoDataChart');
    }
    if (result.isSuccess) {
        if (result.isSearchTermValid) {
            if (!_IsDefaultLoad) {
                $('#divDiscoveryUtility').show();
                _SearchDatebkp = _SearchDate;
                _searchTermbkp = _searchTerm;
                _SearchMediumbkp = _SearchMedium;
                _SearchTVMarketbkp = _SearchTVMarket;


                RenderColumnChart(result.columnChartJson);
                RenderLineChart(result.lineChartJson);
                RenderPieChart(result.pieChartJson);
                SetDateFilter(result);
                SetMediumFilter(result);
                SetTVMarketFilter(result);

                $('#divChartTotal').html('Total Records :: ' + result.chartTotal);
            }
            else {
                $('#divPieChartHeader').hide();
                $('#divPieChartData').hide();
                $('#divDiscoveryClearAll').hide();
                $('#divDiscoveryUtility').hide();
            }

        } else {

            ShowNotification(_msgSearchTermAlreadyEntered);
        }
    }
    else {
        /*_SearchDate = _SearchDatebkp;
        _searchTerm = _searchTermbkp;
        _SearchMedium = _SearchMediumbkp;
        _SearchTVMarket = _SearchTVMarketbkp;*/
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
    setTimeout(function () {
        //$("#divMainContent").mCustomScrollbar("scrollTo", "top");
    }, 200);
    _IsToggle = false;
}

function OnFail(result) {
    _IsTabChange = false;
    _IsToggle = false;
    //alert('On Chart Fail - Local');
    ShowNotification(_msgErrorOccured);
}


function SearchResultAjaxRequest() {

    /*var finalFromDate = '';
    var finalToDate = '';

    if (_ChartDate) {
    finalFromDate = _ChartDate;
    finalToDate = _ChartDate;
    }
    else {
    finalFromDate = _fromDate;
    finalToDate = _toDate;
    }*/
    _urlDiscoveryMediaJsonChart = "/Discovery2/MediaJsonChart/";

    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }

    SetActiveFilter();
    ClearCheckboxSelection();

    if (IsChartActive == 0) {
        var jsonPostData = {
            searchTermIndex: _SearchTermIndex,
            searchTermArray: mySearchTermArray,
            fromDate: _fromDate,
            toDate: _toDate,
            medium: _SearchMedium,
            tvMarket: _SearchTVMarket,
            IsTabChange: _IsTabChange,
            IsToggle: _IsToggle
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryMediaJsonResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnResultSearchComplete,
            error: OnResultSearchFail
        });
    }
    else {
        //if (!IsChartUpdated) {
        //IsChartUpdated = true;
        var jsonPostData = {
            searchTerm: mySearchTermArray,
            fromDate: _fromDate,
            toDate: _toDate,
            medium: _SearchMedium,
            tvMarket: _SearchTVMarket,
            isDefaultLoad: _IsDefaultLoad
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryMediaJsonChart,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnMediaSearchComplete,
            error: OnFail
        });
        //}


    }
}

function ShowMoreResult() {
    $("#chkInputAll").prop("checked", false);
    $("#divResult input[type=checkbox]").each(function () {
        this.checked = false;
    });

    if (_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].AvailableRecords) {

        _searchTerm[_SearchTermIndex].ShownRecords += _searchTerm[_SearchTermIndex].DisplayPageSize;

        if (_searchTerm[_SearchTermIndex].ShownRecords > _searchTerm[_SearchTermIndex].TotalRecords) {
            _searchTerm[_SearchTermIndex].ShownRecords = _searchTerm[_SearchTermIndex].TotalRecords;
        }

        //alert('available ' + _searchTerm[_SearchTermIndex].AvailableRecords);

        if (_searchTerm[_SearchTermIndex].AvailableRecords > _searchTerm[_SearchTermIndex].TotalRecords) {
            _searchTerm[_SearchTermIndex].AvailableRecords = _searchTerm[_SearchTermIndex].TotalRecords;
        }

        //alert('after more results :: Shown records' + _searchTerm[_SearchTermIndex].ShownRecords + "-- Available Results :: " + _searchTerm[_SearchTermIndex].AvailableRecords);

        $('#divResult_Child_Data_' + _SearchTermIndex + ' > div').slice(0, _searchTerm[_SearchTermIndex].ShownRecords).removeClass("displayNone");
        $('#spnNoOfRecords_' + _SearchTermIndex).html(numberWithCommas(_searchTerm[_SearchTermIndex].ShownRecords) + ' of ' + numberWithCommas(_searchTerm[_SearchTermIndex].TotalRecords));

        var hasMoreResults = false;
        /*alert('ShownRecords '+ _searchTerm[_SearchTermIndex].ShownRecords);
        alert('AvailableRecords ' + _searchTerm[_SearchTermIndex].AvailableRecords);
        alert('TotalRecords ' + _searchTerm[_SearchTermIndex].TotalRecords);

        if (_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].AvailableRecords) {
        alert('shown is less than available');
        }

        if (_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].TotalRecords) {
        alert('shown is less than total');
        }*/

        if ((_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].AvailableRecords)
               ||
                (_searchTerm[_SearchTermIndex].ShownRecords < _searchTerm[_SearchTermIndex].TotalRecords)) {
            hasMoreResults = true;
        }
        else {
            hasMoreResults = false;
        }


        if (!hasMoreResults) {

            $('#btnShowMoreResults_' + _SearchTermIndex).attr('value', _msgNoMoreResult);
            $('#btnShowMoreResults_' + _SearchTermIndex).removeAttr('onclick');

        }
        else {

            $('#btnShowMoreResults_' + _SearchTermIndex).attr('value', _msgShowMoreResults);
            $('#btnShowMoreResults_' + _SearchTermIndex).attr('onclick', 'ShowMoreResult();');
        }

    }
    else {

        var mySearchTermArray = new Array();
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
        }

        var jsonPostData = {
            searchTermIndex: _SearchTermIndex,
            fromDate: _fromDate,
            toDate: _toDate,
            medium: _SearchMedium,
            tvMarket: _SearchTVMarket,
            searchTermArray: mySearchTermArray
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryMoreResult,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnMoreResultComplete,

            error: OnMoreResultFail
        });

        $('#imgMoreResultLoading_' + _SearchTermIndex).removeClass("visibilityHidden");
        $('#btnShowMoreResults_' + _SearchTermIndex).attr("disabled", "disabled");
        $('#btnShowMoreResults_' + _SearchTermIndex).attr("class", "disablebtn");
    }
}

function OnMoreResultComplete(result) {

    $('#imgMoreResultLoading_' + result.searchedIndex).addClass("visibilityHidden");
    //$('#divNoDataResult').html('');
    $('#divResult_Child_NoData_' + result.searchedIndex).html('');


    if (result.isSuccess) {

        if (result.availableData) {
            SetNoDataAvailableMessage(result.availableData, 'divResult_Child_NoData_' + result.searchedIndex);
        }
        else {
            $('#divResult_Child_NoData_' + result.searchedIndex).html('');
            //SetNoDataAvailableMessage('', 'divResult_Child_NoData_' + result.searchedIndex);

        }

        SetDiscoveryResultStatus(result);


        $('#divResult_Child_Data_' + result.searchedIndex).html(result.HTML);

        _searchTerm[result.searchedIndex].ShownRecords += result.searchTermShownRecords;
        _searchTerm[result.searchedIndex].AvailableRecords = result.searchTermAvailableRecords;
        _searchTerm[result.searchedIndex].TotalRecords = result.searchTermTotalRecords;

        if (_searchTerm[result.searchedIndex].ShownRecords > _searchTerm[result.searchedIndex].TotalRecords) {
            _searchTerm[result.searchedIndex].ShownRecords = _searchTerm[result.searchedIndex].TotalRecords;
        }


        if (_searchTerm[result.searchedIndex].AvailableRecords > _searchTerm[result.searchedIndex].TotalRecords) {
            _searchTerm[result.searchedIndex].AvailableRecords = _searchTerm[result.searchedIndex].TotalRecords;
        }
        _searchTerm[result.searchedIndex].TotalRecords = result.searchTermTotalRecords;

        $('#divResult_Child_Data_' + result.searchedIndex + ' > div').slice(0, _searchTerm[result.searchedIndex].ShownRecords).removeClass("displayNone");

        if ((_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].AvailableRecords)
               ||
                (_searchTerm[result.searchedIndex].ShownRecords < _searchTerm[result.searchedIndex].TotalRecords)) {
            result.hasMoreResults = true;
        }
        else {
            result.hasMoreResults = false;
        }

        _searchTerm[result.searchedIndex].DisplayPageSize = result.displayPageSize;

        //alert('after more results :: Shown records' + _searchTerm[result.searchedIndex].ShownRecords + "-- Available Results :: " + _searchTerm[result.searchedIndex].AvailableRecords);

        if (result.isAnyDataAvailable) {
            $('#spnNoOfRecords_' + result.searchedIndex).html(numberWithCommas(_searchTerm[result.searchedIndex].ShownRecords) + ' of ' + numberWithCommas(_searchTerm[result.searchedIndex].TotalRecords));
        }



        $('#divMoreResult_' + result.searchedIndex).remove();
        $('#divResult_Child_Data_' + result.searchedIndex).append('<div id="divMoreResult_' + result.searchedIndex + '" align="center"><img alt="" id="imgMoreResultLoading_' + result.searchedIndex + '" class="marginRight10 visibilityHidden" src="../../Images/Loading_1.gif"><input value="No More Results" class="loadmore displayNone" id="btnShowMoreResults_' + result.searchedIndex + '" style="display: inline;" type="button"></div></div>');
        $('#btnShowMoreResults' + result.searchedIndex).show();
        if (!result.hasMoreResults) {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgNoMoreResult);
            $('#btnShowMoreResults_' + result.searchedIndex).removeAttr('onclick');

        }
        else {

            $('#btnShowMoreResults_' + result.searchedIndex).attr('value', _msgShowMoreResults);
            $('#btnShowMoreResults_' + result.searchedIndex).attr('onclick', 'ShowMoreResult();');
        }
    }
    else {
        CheckForAuthentication(result, 'Some error occured, try again later');
        //ShowNotification('Some error occured, try again later');
    }

    //    setTimeout(function () {
    //        $("#divMainContent").mCustomScrollbar("scrollTo", "top");
    //    }, 200);
}

function OnMoreResultFail(result) {
    $('#imgMoreResultLoading_' + result.searchedIndex).addClass("visibilityHidden");
    //alert('On More Result Fail - Local');
    ShowNotification(_msgErrorOccured);
}

function ValidateSearchTerm() {
    var returnValue = true;

    if (_searchTerm.length <= 0) {
        _SearchTermValidationMessage = _msgEnterSearchTerm;
        returnValue = false;
    }

    for (var i = 0; i < _searchTerm.length; i++) {

        if (_searchTerm[i].SearchTerm.trim() == '') {

            returnValue = false;
            _SearchTermValidationMessage = _msgEnterSearchTerm;
            break;
        }

        for (var j = 0; j < _searchTerm.length; j++) {
            if (i != j) {

                if (_searchTerm[i].SearchTerm.trim() == _searchTerm[j].SearchTerm.trim()) {
                    returnValue = false;
                    _SearchTermValidationMessage = _msgSearchTermAlreadyEntered;
                    break;
                }
            }
        }
    }

    return returnValue;
}

function SetTVMarketFilter(result) {
    $('#ulTVMarket').html('');

    var tvMarketLI = '';
    if (result.discoveryTVMarketFilter) {
        $.each(result.discoveryTVMarketFilter, function (eventID, eventData) {
            tvMarketLI = tvMarketLI + '<li onclick="SetTVMarket(\'' + eventData + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData + '</a></li>';
        });

        if (tvMarketLI != '') {
            $('#ulTVMarket').html(tvMarketLI);
        }
        else {
            $('#ulTVMarket').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
        }
    }
    else {
        $('#ulTVMarket').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }

}
function SetActiveFilter() {
    $('#divActiveFilter').html('');
    $('#divActiveFilter').removeClass("bottomBorderColor");

    if (_SearchDate)
        $('#divActiveFilter').append('<div class=\"filter-in\" id=\"divMainDateFilter\">' + _SearchDate + '<span onclick="RemoveDateFilter();" class="cancel"></span></div>');

    /*if (_ChartDate && IsChartActive == 0)
    $('#divActiveFilter').append('<div id=\"divChartDateFilter\" class=\"filter-in\">' + _ChartDate.substring(0, 10) + '<span onclick="RemoveChartDate();" class="cancel"></span></div>');*/

    if (_SearchMediumDesc)
        $('#divActiveFilter').append('<div class=\"filter-in\">' + _SearchMediumDesc + '<span onclick="RemoveMediumFilter();" class="cancel"></span></div>');


    if (_SearchTVMarket)
        $('#divActiveFilter').append('<div class=\"filter-in\">' + _SearchTVMarket + '<span onclick="RemoveTVMarketFilter();" class="cancel"></span></div>');

    if ($('#divActiveFilter').html()) {
        $('#divActiveFilter').addClass("bottomBorderColor");
    }

    if (_ChartDate) {
        //      alert('main date wil be hide');
        $('#divMainDateFilter').hide();
    }
    else {
        //        alert('main date wil be shown');
        $('#divMainDateFilter').show();
    }

}


function SetNoDataAvailableMessage(message, divID) {
    var noDataHTML = '<div class="alert" id="divNoData">'
    //                    + '<button type="button" class="close" data-dismiss="alert">'
    //                      + '&times;</button>'
                     + '<div id="divNotAvailableDataMessage" class="row-fluid filter margin0">' + message
    //+ '<input value="Go" class="RefreshResult" id="btnRefresh Data" type="button" alt="Go" onclick="RefreshResult();" />'
                     + '</div>'

                 + '</div>';
    $('#' + divID).append(noDataHTML);
    //onclick="var documentHeight = $(window).height();$(\'#divMainContent\').css({ \'height\': documentHeight - 200 });"
    //$('#divNotAvailableDataMessage').html(message);
}

function RenderColumnChart(jsonChartData) {

    $('#divColumnChart').highcharts(JSON.parse(jsonChartData));
    //jsonChartData = "{\"chart\": {\"palette\": \"2\",\"caption\": \"Monthly Unit Sales\",\"xaxisname\": \"Month\",\"yaxisname\": \"Units\",\"showvalues\": \"0\",\"decimals\": \"0\",\"formatnumberscale\": \"0\"},\"data\": [{\"label\": \"Jan\",\"value\": \"462\"},{\"label\": \"Feb\",\"value\": \"857\"},{\"label\": \"Mar\",\"value\": \"671\"},{\"label\": \"Apr\",\"value\": \"494\"},{\"label\": \"May\",\"value\": \"761\"},{\"label\": \"Jun\",\"value\": \"960\"},{\"dashed\": \"1\",\"color\": \"00AACC\",\"thickness\": \"2\",\"dashlen\": \"2\",\"dashgap\": \"6\",\"vline\": \"true\"},{\"label\": \"Jul\",\"value\": \"629\"},{\"label\": \"Aug\",\"value\": \"622\"},{\"label\": \"Sep\",\"value\": \"376\"},{\"label\": \"Oct\",\"value\": \"494\"        },{\"label\": \"Nov\",\"value\": \"761\"},{\"label\": \"Dec\",\"value\": \"960\"}],\"styles\": {\"definition\": [{\"name\": \"myAnim\",\"type\": \"animation\",\"param\": \"_yScale\",\"start\": \"0\",\"duration\": \"1\"}],\"application\": [{\"toobject\": \"VLINES\",\"styles\": \"myAnim\"}]}}";

    /*FusionCharts.setCurrentRenderer('javascript');

    var myChart = new FusionCharts("Column2D", "myChartId", "150", "300");
    myChart.render("divColumnChart");
    myChart.setJSONData(jsonChartData);*/

}


function RenderLineChart(jsonLineChartData) {


    //{"chart":{"height":300,"type":null,"width":750},"legend":{"align":"center","borderWidth":"0","layout":"horizontal","verticalAlign":"bottom","width":150},"plotOptions":{"column":null,"cursor":null,"series":{"cursor":"pointer","point":{"Events":{"click":"ChartClick(this)"}}}},"series":[{"data":[{"SearchTerm":"music","y":60879},{"SearchTerm":"music","y":58347},{"SearchTerm":"music","y":44325},{"SearchTerm":"music","y":37540},{"SearchTerm":"music","y":52416},{"SearchTerm":"music","y":60216},{"SearchTerm":"music","y":59920},{"SearchTerm":"music","y":62250},{"SearchTerm":"music","y":58824},{"SearchTerm":"music","y":35306},{"SearchTerm":"music","y":39370},{"SearchTerm":"music","y":53062},{"SearchTerm":"music","y":56602},{"SearchTerm":"music","y":78093},{"SearchTerm":"music","y":62970},{"SearchTerm":"music","y":54107},{"SearchTerm":"music","y":39950},{"SearchTerm":"music","y":35365},{"SearchTerm":"music","y":54992},{"SearchTerm":"music","y":65466},{"SearchTerm":"music","y":62960},{"SearchTerm":"music","y":65374},{"SearchTerm":"music","y":60293},{"SearchTerm":"music","y":49793},{"SearchTerm":"music","y":38416},{"SearchTerm":"music","y":62481},{"SearchTerm":"music","y":63929},{"SearchTerm":"music","y":63634},{"SearchTerm":"music","y":59611},{"SearchTerm":"music","y":59369},{"SearchTerm":"music","y":37695},{"SearchTerm":"music","y":35580},{"SearchTerm":"music","y":55821},{"SearchTerm":"music","y":56792},{"SearchTerm":"music","y":59640},{"SearchTerm":"music","y":59290},{"SearchTerm":"music","y":55396},{"SearchTerm":"music","y":39584},{"SearchTerm":"music","y":35605},{"SearchTerm":"music","y":54471},{"SearchTerm":"music","y":60378},{"SearchTerm":"music","y":57262},{"SearchTerm":"music","y":61901},{"SearchTerm":"music","y":59132},{"SearchTerm":"music","y":40720},{"SearchTerm":"music","y":36610},{"SearchTerm":"music","y":51578},{"SearchTerm":"music","y":59765},{"SearchTerm":"music","y":60427},{"SearchTerm":"music","y":59244},{"SearchTerm":"music","y":54878},{"SearchTerm":"music","y":37984},{"SearchTerm":"music","y":36098},{"SearchTerm":"music","y":56021},{"SearchTerm":"music","y":58689},{"SearchTerm":"music","y":57487},{"SearchTerm":"music","y":58662},{"SearchTerm":"music","y":59012},{"SearchTerm":"music","y":38189},{"SearchTerm":"music","y":34835},{"SearchTerm":"music","y":56534},{"SearchTerm":"music","y":61010},{"SearchTerm":"music","y":60457},{"SearchTerm":"music","y":59404},{"SearchTerm":"music","y":55730},{"SearchTerm":"music","y":34326},{"SearchTerm":"music","y":38773},{"SearchTerm":"music","y":66383},{"SearchTerm":"music","y":63346},{"SearchTerm":"music","y":60122},{"SearchTerm":"music","y":58964},{"SearchTerm":"music","y":56718},{"SearchTerm":"music","y":38739},{"SearchTerm":"music","y":37981},{"SearchTerm":"music","y":44843},{"SearchTerm":"music","y":58034},{"SearchTerm":"music","y":62225},{"SearchTerm":"music","y":62959},{"SearchTerm":"music","y":58513},{"SearchTerm":"music","y":42022},{"SearchTerm":"music","y":40204},{"SearchTerm":"music","y":61217},{"SearchTerm":"music","y":61279},{"SearchTerm":"music","y":60732},{"SearchTerm":"music","y":65276},{"SearchTerm":"music","y":58628},{"SearchTerm":"music","y":39640},{"SearchTerm":"music","y":35524},{"SearchTerm":"music","y":57711},{"SearchTerm":"music","y":63455},{"SearchTerm":"music","y":64538},{"SearchTerm":"music","y":64631},{"SearchTerm":"music","y":13836}],"name":"music"}],"subtitle":{"text":"","x":"-20"},"title":{"text":"Trend over Time","x":-20},"tooltip":{"valueSuffix":""},"xAxis":{"categories":["6\/20\/2013","6\/21\/2013","6\/22\/2013","6\/23\/2013","6\/24\/2013","6\/25\/2013","6\/26\/2013","6\/27\/2013","6\/28\/2013","6\/29\/2013","6\/30\/2013","7\/1\/2013","7\/2\/2013","7\/3\/2013","7\/4\/2013","7\/5\/2013","7\/6\/2013","7\/7\/2013","7\/8\/2013","7\/9\/2013","7\/10\/2013","7\/11\/2013","7\/12\/2013","7\/13\/2013","7\/14\/2013","7\/15\/2013","7\/16\/2013","7\/17\/2013","7\/18\/2013","7\/19\/2013","7\/20\/2013","7\/21\/2013","7\/22\/2013","7\/23\/2013","7\/24\/2013","7\/25\/2013","7\/26\/2013","7\/27\/2013","7\/28\/2013","7\/29\/2013","7\/30\/2013","7\/31\/2013","8\/1\/2013","8\/2\/2013","8\/3\/2013","8\/4\/2013","8\/5\/2013","8\/6\/2013","8\/7\/2013","8\/8\/2013","8\/9\/2013","8\/10\/2013","8\/11\/2013","8\/12\/2013","8\/13\/2013","8\/14\/2013","8\/15\/2013","8\/16\/2013","8\/17\/2013","8\/18\/2013","8\/19\/2013","8\/20\/2013","8\/21\/2013","8\/22\/2013","8\/23\/2013","8\/24\/2013","8\/25\/2013","8\/26\/2013","8\/27\/2013","8\/28\/2013","8\/29\/2013","8\/30\/2013","8\/31\/2013","9\/1\/2013","9\/2\/2013","9\/3\/2013","9\/4\/2013","9\/5\/2013","9\/6\/2013","9\/7\/2013","9\/8\/2013","9\/9\/2013","9\/10\/2013","9\/11\/2013","9\/12\/2013","9\/13\/2013","9\/14\/2013","9\/15\/2013","9\/16\/2013","9\/17\/2013","9\/18\/2013","9\/19\/2013","9\/20\/2013"],"labels":{"rotation":270}},"yAxis":{"plotLines":[{"color":"#808080","value":"0","width":"1"}],"title":{"text":null}}}

    var JsonLineChart = JSON.parse(jsonLineChartData);

    //var JsonLineChart = JSON.parse(jsonLineChartData);
    //JsonLineChart.plotOptions.series.point.Events.click = ChartClick;

    //alert(jsonLineChartData);
    // var JsonLineChart = JSON.parse(jsonLineChartData);
    //alert(JsonLineChart);
    JsonLineChart.plotOptions.series.point.events.click = ChartClick;
    $('#divLineChart').highcharts(JsonLineChart);

//    $('#divLineChart').highcharts({
//        chart: {
//            height: 300,
//            type: null,
//            width: 750
//        },
//        legend: {
//            align: "center",
//            borderWidth: 0,
//            layout: "horizontal",
//            verticalAlign: "bottom",
//            width: 150
//        },
//        plotOptions: {
//            series: {
//                cursor: "pointer",
//                point: {
//                    events: {
//                        click: function () {
//                            alert("hi");
//                        }
//                    }
//                }
//            }
//        },
//        series: JsonLineChart.series,
//        subtitle: {
//            text: "",
//            x: -20
//        },
//        title: {
//            text: "Trend over Time",
//            x: -20
//        },
//        tooltip: {
//            valueSuffix: ""
//        },
//        xAxis: JsonLineChart.xAxis,
//        yAxis: {
//            plotLines: [{
//                color: "#808080",
//                value: "0",
//                width: 1
//            }],
//            title: {
//                text: ""
//            }
//        }
//    });

    /*FusionCharts.setCurrentRenderer('javascript');

    var myChart = new FusionCharts("MSLine", "myLineChartID", "750", "300");
    myChart.render("divLineChart");
    myChart.setJSONData(jsonLineChartData);*/


}

function RenderPieChart(jsonPieChartData) {
    var chartCount = 0;
    var pieChartHeaderDivHTML = '';
    var pieChartChartDivHTML = '';

    //$('#divPieChart').html('');
    $('#divPieChartHeader').html('');
    $('#divPieChartData').html('');
    FusionCharts.setCurrentRenderer('javascript');

    $.each(jsonPieChartData, function (eventID, eventData) {

        if (chartCount == 0) {
            pieChartHeaderDivHTML = '<div id=\'divPieChart_' + chartCount + '\' align="center" style="float: left; padding: 5px;cursor:pointer;min-width:50px;" class="pieChartActive" >' + eventData.SearchTerm + '</div>';
            //pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop34 float-right width50p wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';
        }
        else {
            pieChartHeaderDivHTML = '<div id=\'divPieChart_' + chartCount + '\' align="center" style="float: left; padding: 5px;cursor:pointer;min-width:50px;" >' + eventData.SearchTerm + '</div>';
            //pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\' style="visibility:hidden;height:0px;border-top:1px solid gray;"><div id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';

        }
        //pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop10 float-right width50p wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';
        pieChartChartDivHTML = '<div id=\'divPieChart_Child_' + chartCount + '\'><div  class="float-left" id=\'divPieChart_Child_Data_' + chartCount + '\'></div><div class=\'margintop10 float-left divtopres wordWrap\' id=\'divPieChart_Child_TopResult_' + chartCount + '\'></div></div>';


        $('#divPieChartHeader').append(pieChartHeaderDivHTML);
        $('#divPieChartHeader').show();

        $('#divPieChartData').append(pieChartChartDivHTML);
        $('#divPieChartData').show();
        /*var myChart = new FusionCharts("Pie2D", "myPieChartID_" + chartCount, "400", "300");
        myChart.render("divPieChart_Child_Data_" + chartCount);
        myChart.setJSONData(eventData.JsonResult);*/

        $("#divPieChart_Child_Data_" + chartCount).highcharts(JSON.parse(eventData.JsonResult));
        $('#divPieChart_Child_TopResult_' + chartCount).html(eventData.TopResultHtml);

        chartCount = chartCount + 1;
    });

    ShowPieChart(0);
}


function ShowPieChart(elementIndex) {
    $('#divPieChartHeader div').each(function () {
        $(this).removeAttr("class");
    });

    $('#divPieChartData > div').each(function () {
        $(this).hide();
    });

    $('#divPieChartHeader').children().eq(elementIndex).attr("class", "pieChartActive");
    $('#divPieChartData').children().eq(elementIndex).css("visibility", "");
    $('#divPieChartData').children().eq(elementIndex).css("height", "");
    $('#divPieChartData').children().eq(elementIndex).show();
}


function SetDateFilter(result) {
    disabledDays = [];
    $.each(result.discoveryDateFilter, function (eventID, eventData) {

        disabledDays.push(eventData.Date);

    });
}

function SetMediumFilter(result) {
    $('#ulMedium').html('');
    var discoveryMedium = '';
    $.each(result.discoveryMediumFilter, function (eventID, eventData) {

        discoveryMedium = discoveryMedium + ' <li onclick="SetMedium(\'' + eventData.Medium + '\',\'' + eventData.MediumValue + '\');" role="presentation"><a href="#" tabindex="-1" role="menuitem">' + eventData.MediumValue + '</a></li>';
    });

    if (discoveryMedium != '') {
        $('#ulMedium').html(discoveryMedium);
    }
    else {
        $('#ulMedium').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
    }
}

function enableAllTheseDays(date) {
    date = $.datepicker.formatDate('mm/dd/yy', date);
    return [$.inArray(date, disabledDays) !== -1];
}



function GetChartData(e) {
    if (e.keyCode == 13) {
        $('#' + $(e.target).attr("id")).blur();
        //$('#divAddItem').focus();
    }
}

function RemoveSearchTerm(divID) {

    var textBoxLiID = divID.replace('divFilterSearchTerm_', 'lisearchTerm_');

    $('#' + textBoxLiID).remove();
    PushSearchTermintoArray();
}

function RemoveDateFilter() {
    _SearchDate = '';
    /*_fromDate = '';
    _toDate = '';*/

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());


    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    IsChartUpdated = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function RemoveMediumFilter() {
    _SearchMedium = '';
    _SearchMediumDesc = ''
    IsChartUpdated = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function RemoveTVMarketFilter() {
    _SearchTVMarket = '';

    IsChartUpdated = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}

function ToggleChartResult() {

    if (IsChartActive == 0) {

        IsChartActive = 1;
        //$('#divMainDateFilter').show();

        /*$('#divChartMain').show();
        $('#divResultMain').hide();*/

        $('#divChartMain').removeAttr('class');
        $('#divResultMain').attr('class', 'heightWidth0');

        //$('#imgChartResult').attr('src', '../../Images/Result.png');
        /*if (!IsChartUpdated) {
        SearchResult();
        }*/
    }
    else {
        IsChartActive = 0;


        //alert('_ChartDate ' +_ChartDate);
        /*if (_ChartDate) {

        $('#divMainDateFilter').hide();
        }
        else {

        $('#divMainDateFilter').show();
        }*/


        /*$('#divChartMain').hide();
        $('#divResultMain').show();*/

        $('#divChartMain').attr('class', 'heightWidth0');
        $('#divResultMain').removeAttr('class');

        //$('#imgChartResult').attr('src', '../../Images/Chart.png');

        for (var i = 0; i < _searchTerm.length; i++) {

            if (_searchTerm[i].SearchTerm.trim() == _SearchTermResult && _searchTerm[i].ResultShown == false) {
                //_SearchTermResult = _searchTerm[i].SearchTerm.trim();
                _searchTerm[i].IsCurrentTab = true;
                _searchTerm[i].ResultShown = true;
                //_IsTabChange = true;
                _IsToggle = true;
                SearchResult();
            }
        }
    }
}

function GenerateSearchTermTab() {

    //if (IsResultInitialLoad == 1) {
    /*if (_searchTerm.length > 0) {
    _SearchTermResult = _searchTerm[0].SearchTerm.trim();
    _searchTerm[0].IsCurrentTab = true;
    _searchTerm[0].ResultShown = true;
    }
    SearchResult();*/

    $('#divResultHeader').html('');
    $('#divResult').html('');
    for (var i = 0; i < _searchTerm.length; i++) {
        if (_searchTerm[i].SearchTerm.trim()) {
            if (_searchTerm[i].SearchTerm.trim() == _SearchTermResult) {
                $('#divResultHeader').append('<div id=\'divResult_' + i + '\' align="center" style="float: left; padding: 5px;cursor:pointer;min-width:50px;" class="pieChartActive" >' + _searchTerm[i].SearchTerm.trim() + '</div>');
            }
            else {
                $('#divResultHeader').append('<div id=\'divResult_' + i + '\'  align="center" style="float: left; padding: 5px;cursor:pointer;min-width:50px;">' + _searchTerm[i].SearchTerm.trim() + '</div>');
            }
            $('#divResult').append('<div id=\'divResult_Child_' + i + '\'><div id=\'divResult_Child_NoData_' + i + '\'></div><span class="resultTotal float-right" id="spnNoOfRecords_' + i + '"></span><div class="clear" id="divResult_Child_Scroll_' + i + '"><div class="padding5" id="divResult_Child_Data_' + i + '"></div></div></div>');
        }
    }
    //}

}

function GetDataOnTabChange() {

    $('#divResultHeader div').each(function () {
        $(this).removeAttr("class");
    });

    $('#divResult > div').each(function () {
        $(this).hide();
    });


    // for (var i = 0; i < _searchTerm.length; i++) {
    // _searchTerm[i].IsCurrentTab = false;

    // if (_searchTerm[_SearchTermIndex].SearchTerm.trim() == p_searchTerm) {

    //     _searchTerm[_SearchTermIndex].IsCurrentTab = true;

    _SearchTermResult = _searchTerm[_SearchTermIndex].SearchTerm.trim();

    if (!_searchTerm[_SearchTermIndex].ResultShown) {

        IsChartUpdated = true;
        _IsToggle = true;
        _IsTabChange = true;
        SearchResult();
        _searchTerm[_SearchTermIndex].ResultShown = true;
    }

    $('#divResultHeader').children().eq(_SearchTermIndex).attr("class", "pieChartActive");
    $('#divResult').children().eq(_SearchTermIndex).show();

    //$('#divResult_Child_' + p_searchTerm.replace(/ /g, "_")).show();
    // }
    //}
}

function ResetSearchTermClassToFalse() {
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        _searchTerm[zz].ResultShown = false;
    }
}

function SaveSearch() {

    if (ValidateSaveSearchInput()) {
        $('#imgSaveSearchLoading').show();
        var mySearchTermArray = new Array();
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            mySearchTermArray.push(_searchTerm[zz].SearchTerm);
        }

        var jsonPostData = {
            title: $('#txtSaveSearchPopup').val().trim(),
            searchTerm: mySearchTermArray
            //date: _SearchDate,
            /*fromDate: _fromDate,
            toDate: _toDate,
            medium: _SearchMedium,
            tvMarket: _SearchTVMarket*/
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlDiscoverySaveSearch,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),

            success: OnSaveSearchComplete,
            error: OnSaveSearchFail
        });
    }
    else {
        if ($('#txtSaveSearchPopup').val().trim() == '') {
            $('#txtSaveSearchPopup').css('border', '1px red solid');
            $('#txtSaveSearchPopup').addClass('boxshadow');

            setTimeout(function () {
                $('#txtSaveSearchPopup').removeClass('boxshadow');
            }, 2000);
        }

        if (msg != '') {
            ShowNotification(msg);
        }
    }
}

function OnSaveSearchComplete(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    if (result.isSuccess) {
        ShowNotification(result.message);
        GetSavedSearch(false, true);
    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
}

function OnSaveSearchFail(result) {
    $('#imgSaveSearchLoading').hide();
    $('#divPopover').remove();
    //alert('On Save Search Fail - Local');
    ShowNotification(_msgErrorOccured);
}

function ShowSaveSearchDiscovery() {

    $('#divPopover').remove();
    $('#aSaveSearch').popover({
        trigger: 'manual',
        html: true,
        title: '',
        placement: 'right',
        template: '<div id="divPopover" class="popover"><div class="arrow"></div><div class="popover-inner"><div class="popover-content"><p></p></div></div></div>',
        content: '<input type="text" placeholder="Save Search Title" id="txtSaveSearchPopup" /><div><input type="button"  class="cancelButton marginbottom0"  style="margin-top:0px !important;" value="Cancel"  onclick="$(\'#divPopover\').remove();" /><input type="button" id="btnSaveSearch" class="button marginbottom0" style="margin-left:10px !important;margin-top:0px !important;" value="Submit" onclick="SaveSearch();" /><img src="../../Images/Loading_1.gif" class="displayNone" id="imgSaveSearchLoading" /></div>'
    });

    $('#aSaveSearch').popover('show');


}

function ValidateSaveSearchInput() {
    var isValid = true;
    msg = '';

    if (_searchTerm.length <= 0) {
        isValid = false;
        msg = _msgEnterSearchTerm;
    }
    if ($('#txtSaveSearchPopup').val().trim() == '') {
        msg = _msgEnterSaveSearchTitle;
        isValid = false;
    }

    if (isValid) {
        for (var zz = 0; zz < _searchTerm.length; zz++) {
            if (!_searchTerm[zz].SearchTerm.trim()) {
                msg = _msgEnterSearchTerm;
                isValid = false;
                break;
            }
        }
    }

    /*if (_searchTerm.length <= 0) {
    msg = 'Enter Search Term';
    isValid = false;
    }*/

    return isValid;
}

function GetSavedSearch(p_isNext, p_isInitialize) {
    $('#divSavedSearch').html(_imgLoading);
    $('#divSavedSearch').addClass('text-align-center');
    var jsonPostData = {
        isNext: p_isNext,
        isInitialize: p_isInitialize
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetSaveSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        global: false,
        success: OnGetSaveSearchComplete,
        error: OnGetSaveSearchFail
    });

    return false;
}

function OnGetSaveSearchComplete(result) {

    if (result.isSuccess) {
        SetSavedSearchHTML(result);
    }
    else {

        //CheckForAuthentication(result, 'Some error occured, try again later');
        $('#divSavedSearch').html('An error occured,<a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
        //ShowNotification('Some error occured, try again later');
    }
}

function OnGetSaveSearchFail(result) {
    //alert('GetSaveSearchFail - Local');
    //ShowNotification('Some error occured, try again later');
    $('#divSavedSearch').html('An error occured, <a class="cursorPointer" onclick="GetSavedSearch(false, true);">try again</a>');
}

function SetSavedSearchHTML(result) {
    $('#divSavedSearch').removeClass('text-align-center');
    $('#divSavedSearch').html(result.HTML);

    if (result.isPreviousAvailable) {
        $('#aSavedSearchPrevious').attr("onclick", "GetSavedSearch(false,false);");
        $('#aSavedSearchPrevious').show(); //  removeClass("inactiveLink");
    }
    else {
        $('#aSavedSearchPrevious').removeAttr("onclick");
        $('#aSavedSearchPrevious').hide(); // addClass("inactiveLink");
    }


    if (result.HasMoreResult) {
        $('#aSavedSearchNext').attr("onclick", "GetSavedSearch(true,false);");
        $('#aSavedSearchNext').show(); //  removeClass("inactiveLink");
    }
    else {
        $('#aSavedSearchNext').removeAttr("onclick");
        $('#aSavedSearchNext').hide(); //  addClass("inactiveLink");
    }

    $('#spnSavedSearchRecordDetail').html(result.saveSearchRecordDetail);
}


function LoadSavedSearch(ID) {
    IsChartActive = 1;

    ClearFilterVariable();
    //_IsToggle = 0;
    var jsonPostData = {
        p_ID: ID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryLoadSavedSearch,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnLoadSaveSearchComplete,
        error: OnLoadSaveSearchFail
    });
}

function OnLoadSaveSearchComplete(result) {
    if (result.isSuccess) {
        SetSavedSearchHTML(result)
        _searchTerm = new Array();
        _SearchMedium = ''; // result.discovery_SavedSearch.Medium;
        _SearchMediumDesc = ''; // result.discovery_SavedSearch.MediumDesc;
        _SearchDate = ''; // result.discovery_SavedSearch.SearchDate;
        _SearchTVMarket = ''; // = result.discovery_SavedSearch.TVMarket;


        /*_fromDate = GetDateFormatFromJsonString(result.discovery_SavedSearch.FromDate);
        _toDate = GetDateFormatFromJsonString(result.discovery_SavedSearch.ToDate);
        
        $("#dpFrom").datepicker("setDate", _fromDate);
        $("#dpTo").datepicker("setDate", _toDate);*/

        var _tDate = new Date();
        var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

        $("#dpFrom").datepicker("setDate", _fDate);
        $("#dpTo").datepicker("setDate", _tDate);

        _fromDate = $('#dpFrom').val();
        _toDate = $('#dpTo').val();



        $('#ulSearchTerm').html('');
        for (var zz = 0; zz < result.discovery_SavedSearch.SearchTermArray.length; zz++) {

            var SearchTermClass = new Object();
            SearchTermClass.SearchTerm = result.discovery_SavedSearch.SearchTermArray[zz];
            SearchTermClass.ResultShown = false;
            SearchTermClass.IsCurrentTab = false;
            _searchTerm.push(SearchTermClass);
            $('#ulSearchTerm').append("<li style=\"list-style:none outside none\" id=\"lisearchTerm_" + zz + "\" ><img id=\"imgRemoveSearchTermTextBox_" + zz + "\" src=\"../../Images/Delete.png\" alt=\"\" onclick=\"$('#lisearchTerm_" + zz + "').remove();PushSearchTermintoArray();CheckForMaxSearchTerm();\" class=\"marginTop-9 marginRight9 cursorPointer\" /><input type=\"text\" placeholder=\"Search Term\" id=\"txtSearchTerm_" + zz + "\" onclick=\"ShowAddSearchTermPopup(this.id);\" readonly=\"readonly\" /></li>");
            $('#txtSearchTerm_' + zz).val(result.discovery_SavedSearch.SearchTermArray[zz]);
            searchTermCount = zz;

        }
        CheckForMaxSearchTerm();

        _SearchTermResult = result.discovery_SavedSearch.SearchTermArray[0];
        GenerateSearchTermTab();
        $('#divChartMain').removeAttr('class');
        $('#divResultMain').attr('class', 'heightWidth0');

        $('#imgChartResult').attr('src', '../../Images/Result.png');


        setTimeout(function () {
            SearchResult();
        }, 1);



    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
}

function OnLoadSaveSearchFail(result) {
    //alert('OnLoadSaveSearchFail - Local');
    ShowNotification(_msgErrorOccured);
}

function DeleteDiscoverySavedSearchByID(ID) {
    var jsonPostData = {
        p_ID: ID
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryDeleteDiscoverySavedSearchByID,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),

        success: OnDeleteSaveSearchComplete,
        error: OnDeleteSaveSearchFail
    });
}

function OnDeleteSaveSearchComplete(result) {
    if (result.isSuccess) {
        ShowNotification(result.message);
        setTimeout(function () {
            GetSavedSearch(false, true);
        }, 1);
    }
    else {
        CheckForAuthentication(result, _msgErrorOccured);
        //ShowNotification('Some error occured, try again later');
    }
}

function OnDeleteSaveSearchFail(result) {
    //alert('OnDeleteSaveSearchFail - Local');
    ShowNotification(_msgErrorOccured);
}

function RefreshResult() {
    /*_SearchMedium = '';
    _SearchMediumDesc = '';
    _SearchTVMarket = '';
    _SearchDate = '';
    _IsToggle = false;

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();*/

    PushSearchTermintoArray();
}

function ClearFilterVariable() {
    _SearchMedium = '';
    _SearchMediumDesc = '';
    _SearchDate = '';

    _Searchtv = '';
    _SearchTermIndex = 0;

    _searchTerm = new Array();
    searchTermCount = 0;
}

function SetDateVariable() {

    _ChartDate = '';


    if ($("#dpFrom").val() && $("#dpTo").val()) {

        if (_fromDate != $("#dpFrom").val() || _toDate != $("#dpTo").val()) {
            _fromDate = $("#dpFrom").val();
            _toDate = $("#dpTo").val();

            SearchResult();
            $('#ulCalender').parent().removeClass('open');
            //$('#aDuration').html(_msgCustom + '&nbsp;&nbsp;<span class="caret"></span>');
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

function ShowDateRangeExpansion() {
    _DateMessage = '';
    if (_SearchMedium == '' || _SearchMedium == 'TW') {

        var twDate = new Date(_constTWContentMinDate);


        if (new Date(_fromDate) < twDate) {
            _DateMessage = _DateMessage + 'Twitter content will not be available prior to January 1st, 2013, would you like to continue with your search?';
        }
    }


    if (_SearchMedium == '' || _SearchMedium == 'Social Media' || _SearchMedium == 'Blog' || _SearchMedium == 'Forum') {
        var smDate = new Date(_constSMContentMinDate);

        if (new Date(_fromDate) < smDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'Social content is not available prior to July 1st, 2012, would you like to continue with your search?';
        }
    }

    if (_SearchMedium == '' || _SearchMedium == 'NM') {
        var nmDate = new Date(_constNMContentMinDate);

        if (new Date(_fromDate) < nmDate) {
            if (_DateMessage) {
                _DateMessage = _DateMessage + '<br/>';
            }
            _DateMessage = _DateMessage + 'Online News content is not available prior to July 1st, 2012, would you like to continue with your search?';
        }
    }


    if (_DateMessage) {
        //        var dateValidationMessage = '<div id="divDateValidationMessage" class="modal fade hide resizable modalPopupDiv"><div>' + _DateMessage + '</div><div><input class="span2" type="button" value="Continue" /></div><input class="span2" type="button" value="Cancel" /></div></div>';
        //        $(document.body).append(dateValidationMessage);
        return false;

    }
    else {

        return true;
    }
    //ShowModal('divDateValidationMessage');
}


function GenerateDashboardPDF() {
    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }

    var jsonPostData = {
        p_HTML: $("#divChartMain").html(),
        p_FromDate: $("#dpFrom").val(),
        p_ToDate: $("#dpTo").val(),
        p_SearchTerm: mySearchTermArray
    }

    $.ajax({
        url: _urlDiscoveryGenerateDiscoveryPDF,
        contentType: "application/json; charset=utf-8",
        type: "post",
        dataType: "json",
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result.isSuccess) {
                window.location = _urlDiscoveryDownloadPDFFile;
            }
            else {
                ShowNotification(_msgErroWhileDownloadingFile);
            }
        },
        error: function (a, b, c) {
            ShowNotification(_msgSomeErrorProcessing);
        }
    });
}


function ShowDashboardEmailPopup() {

    $('#txtFromEmail').val($('#hdnLoggedInUserEmail').val());

    $("#txtToEmail").val('');
    $("#txtSubject").val('');
    $("#txtMessage").val('');
    $('#divEmailPopup').modal({
        backdrop: 'static',
        keyboard: true
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
        $($("#txtToEmail").val().split(';')).each(function (index, value) {
            if (!CheckEmailAddress(value)) {
                $("#spanToEmail").show().html(_msgOneEmailAddressInCorrect);
                isValid = false;
                return;
            }
        });
    }

    return isValid;
}

function CheckEmailAddress(email) {
    //var emailPattern = /^\w+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$/;
    var emailPattern = /^([a-zA-Z0-9_.-])+@([a-zA-Z0-9_.-])+\.([a-zA-Z])+([a-zA-Z])+/;
    return emailPattern.test(email);
}

function SendEmail() {
    var mySearchTermArray = new Array();
    for (var zz = 0; zz < _searchTerm.length; zz++) {
        mySearchTermArray.push(_searchTerm[zz].SearchTerm.trim());
    }
    if (ValidateSendEmail()) {
        var jsonPostData = {
            p_HTML: $("#divChartMain").html(),
            p_FromDate: $("#dpFrom").val(),
            p_ToDate: $("#dpTo").val(),
            p_FromEmail: $("#txtFromEmail").val(),
            p_ToEmail: $("#txtToEmail").val(),
            p_Subject: $("#txtSubject").val(),
            p_UserBody: $("#txtMessage").val(),
            p_SearchTerm: mySearchTermArray
        }

        $.ajax({
            url: _urlDiscoverSendEmail,
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
        ShowNotification(_msgErrorOccured);
    }
}

function OnEmailSendFail(result) {
    CancelEmailpopup();
    ShowNotification(_msgErrorOccured);
}

function SearchByChartDate(cDate, sTerm) {
    _fromDate = cDate.substring(0, 10);
    _toDate = cDate.substring(0, 10);
    _SearchTermResult = sTerm;

    _SearchDate = cDate.substring(0, 10);
    $("#dpFrom").datepicker("setDate", _SearchDate);
    $("#dpTo").datepicker("setDate", _SearchDate);

    _IsToggle = false;
    IsChartActive = 0;
    $('#divChartMain').attr('class', 'heightWidth0');
    $('#divResultMain').removeAttr('class');
    PushSearchTermintoArray();
    /*for (var i = 0; i < _searchTerm.length; i++) {
    if (_searchTerm[i].SearchTerm.trim() == _SearchTermResult) {
    _SearchTermIndex = i;
    }
    _searchTerm[i].ResultShown = false;
    }
    GenerateSearchTermTab();
    ToggleChartResult();*/
}


function RemoveChartDate() {
    //_SearchDate = '';
    /*_fromDate = '';
    _toDate = '';*/

    //ClearChartDateVariable();

    IsChartUpdated = false;
    ResetSearchTermClassToFalse();
    SearchResult();
}


function ClearChartDateVariable() {
    _ChartDate = '';
    /* var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());


    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);*/

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();
}



function InsertDiscoveryReport() {

    if (ValidateReportInputs()) {
        var test = '';
        $('#imgCreateReportLoading').show();
        var mediaID = new Array();
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                var tempIDValue = $(this).val().trim().split(',');

                var mediaIDClass = new Object();
                mediaIDClass.MediaID = tempIDValue[0];
                mediaIDClass.MediaType = tempIDValue[1];

                mediaID.push(mediaIDClass);
            }
        });

        var jsonPostData = {
            p_Title: $('#txtReportTitle').val(),
            p_Keywords: $('#txtReportKeywords').val(),
            p_Description: $('#txtReportDescription').val(),
            p_CategoryGuid: $('#ddlReportCategory').val(),
            p_MediaID: mediaID
        }

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlDiscoverInsert_DiscoveryReport,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnInsertDiscoveryReportSuccess,
            error: OnInsertDiscoveryReportFail
        });
    }
}

function OnInsertDiscoveryReportSuccess(result) {
    $('#imgCreateReportLoading').hide();
    if (result.isSuccess) {
        GetDiscoveryReport();
        ClosePopUp('divReportPopup');
        ShowNotification(result.message);
        ClearCheckboxSelection();

    }
    else {
        ClosePopUp('divReportPopup');
        ShowNotification(_msgErrorOccured);

    }
}


function OnInsertDiscoveryReportFail(result) {
    $('#imgCreateReportLoading').hide();
    ShowNotification(_msgErrorOccured);
}

function ValidateReportInputs() {

    var isValid = true;

    if (!$('#txtReportTitle').val()) {
        isValid = false;
    }

    if (!$('#txtReportKeywords').val()) {
        isValid = false;
    }

    if (!$('#txtReportDescription').val()) {
        isValid = false;
    }

    if ($('#ddlReportCategory').val() <= 0) {
        isValid = false;
    }

    return isValid;

}


function ShowSaveReportPopup() {

    if (ValidateCheckBoxSelection()) {
        //_MaxFeedsReportItems

        var checkedChecboxCount = 0;
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                checkedChecboxCount++;
            }
        });

        if (checkedChecboxCount > _MaxDiscoveryReportItems) {
            $('#spnCreateReportNote').show();
        }
        else {
            $('#spnCreateReportNote').hide();
        }

        $('#txtReportTitle').val('');
        $('#txtReportKeywords').val('');
        $('#txtReportDescription').val('');
        $('#ddlReportCategory').val(0);


        $('#divReportPopup').modal({
            backdrop: 'static',
            keyboard: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }

}

function ValidateCheckBoxSelection() {

    var isChecked = false;
    $("#divResult input[type=checkbox]").each(function () {
        if (this.checked) {
            isChecked = true;
        }
    });
    return isChecked;
}

function ClosePopUp(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
}

function ShowAddToReportPopup() {

    if (ValidateCheckBoxSelection()) {

        $('#ddlReportTitle').val(0);
        $('#ddlReportTitle').removeClass('warningInput');
        $('#divAddToReportPopup').modal({
            backdrop: 'static',
            keyboard: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }

}

function GetDiscoveryReport() {

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlDiscoverySelectDiscoveryReport,
        contentType: 'application/json; charset=utf-8',
        global: false,

        success: OnSelectDiscoveryReportComplete,
        error: OnSelectDiscoveryReportFail
    });
}



function OnSelectDiscoveryReportComplete(result) {
    if (result.isSuccess) {
        discoveryReportListObject = '';
        discoveryReportListObject = result.reportList;
        var reportOptions = '<option value="0">Select Report</option>';

        $.each(discoveryReportListObject, function (eventID, eventData) {
            reportOptions = reportOptions + '<option value="' + eventData.ID + '">' + eventData.Title + '</option>';
        });

        $('#ddlReportTitle').find('option').remove().end().append(reportOptions);
    }
    else {
        if (typeof result.isAuthorized != 'undefined' && !result.isAuthorized) {
            RedirectToUrl(result.redirectURL);
        }
        else {
            ShowNotification(_msgErrorOccured);
        }
    }
}

function OnSelectDiscoveryReportFail(result) {
    ShowNotification(_msgErrorOccured);
}


function AddToDiscoveryReport() {

    if ($('#ddlReportTitle').val() > 0) {

        $('#ddlReportTitle').removeClass('warningInput');
        $('#imgAddToReportLoading').show();
        var mediaID = new Array();
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                mediaID.push($(this).val());
            }
        });

        var jsonPostData = {
            p_ReportID: $('#ddlReportTitle').val(),
            p_RecordList: mediaID
        }

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryAddToDiscoveryReport,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnAddToDiscoveryReportSuccess,
            error: OnAddToDiscoveryReportFail
        });
    }
    else {
        $('#ddlReportTitle').addClass('warningInput');
        $('#ddlReportTitle').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlReportTitle').removeClass('boxshadow');
        }, 2000);
    }
}

function OnAddToDiscoveryReportSuccess(result) {
    $('#imgAddToReportLoading').hide();
    ClearCheckboxSelection();
    if (result.isSuccess) {
        ClosePopUp('divAddToReportPopup');
        ShowNotification(result.message + " " + _msgRecordAddedToReport);

    }
    else {
        ShowNotification(_msgErrorOccured);
        ClosePopUp('divAddToReportPopup');
    }
}


function OnAddToDiscoveryReportFail(result) {
    $('#imgAddToReportLoading').hide();
    ShowNotification(_msgErrorOccured);
}

function ClearCheckboxSelection() {
    $("#chkInputAll").removeAttr("checked")
    $("#divResult input[type=checkbox]").each(function () {
        this.checked = false;
    });
}

function GetDiscoveryReportLimit() {

    $.ajax({

        type: 'POST',
        dataType: 'json',
        url: _urlDiscoveryGetDiscoveryReportLimit,
        contentType: 'application/json; charset=utf-8',
        //data: JSON.stringify(jsonPostData),

        global: false,
        success: OnGetDiscoveryReportLimitSuccess,
        error: OnGetDiscoveryReportLimitFail
    });
}

function OnGetDiscoveryReportLimitSuccess(result) {
    if (result.isSuccess) {
        _MaxDiscoveryReportItems = result.MaxDiscoveryReportItems;
        $('#spnCreateReportNote').html(_DiscoveryMaxDiscoveryReportItemsMessage.replace(/@@MaxDiscoveryReportItems@@/g, _MaxDiscoveryReportItems));
    }

}

function OnGetDiscoveryReportLimitFail(result) {
    ShowNotification(_msgErrorOccured);
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function checkUncheckAll(divID, mainCheckBox) {

    var checkBoxValue = false;
    checkBoxValue = $("#" + mainCheckBox).is(":checked");

    $("#" + divID + " input[type=checkbox]").each(function () {
        this.checked = checkBoxValue;
    });
}

function CheckUncheckMasterCheckBox(checkbox, divID, mainCheckBox) {

    if (!$(checkbox).is(":checked")) {
        $("#" + mainCheckBox).prop("checked", false);
    }
    else {
        var isChecked = true;

        for (var i = 0; i < _searchTerm.length; i++) {

            $("#divResult_Child_" + i + " input[type=checkbox]").slice(0, _searchTerm[i].ShownRecords).each(function () {
                if (!this.checked) {
                    isChecked = false;
                }
            });

        }

        if (isChecked == true) {
            $("#" + mainCheckBox).prop("checked", true);
        }
        else {
            $("#" + mainCheckBox).prop("checked", false);
        }
    }
}



function SetDiscoveryResultStatus(result) {
    if (result.isAnyDataAvailable) {
        if (result.searchTermShownRecords <= 0) {

            result.searchTermTotalRecords = 0;
            result.searchTermAvailableRecords = 0;
            result.searchTermTotalRecords = 0;
        }


        var totalRecordHTML = numberWithCommas(result.searchTermShownRecords) + ' of ' + numberWithCommas(result.searchTermTotalRecords);

        $('#spnNoOfRecords_' + result.searchedIndex).html(totalRecordHTML);
        $('#divResult_Child_Data_' + result.searchedIndex).html(result.HTML);
    }
    else {
        result.searchTermTotalRecords = 0;
        result.searchTermAvailableRecords = 0;
        result.searchTermTotalRecords = 0;
        $('#spnNoOfRecords_' + result.searchedIndex).html('');
    }
}

function ClearSearch() {
    _Searchtv = '';
    _SearchTermIndex = 0;
    _searchTerm = new Array();
    searchTermCount = 0;
    IsChartActive = 1;
    _IsDefaultLoad = true;

    $('#ulSearchTerm').html('');

    ClearAllData();
    AddNewSearchTermTextBox();

    var _tDate = new Date();
    var _fDate = new Date(_tDate.getFullYear(), _tDate.getMonth() - 3, _tDate.getDate());

    $("#dpFrom").datepicker("setDate", _fDate);
    $("#dpTo").datepicker("setDate", _tDate);

    _fromDate = $('#dpFrom').val();
    _toDate = $('#dpTo').val();

    SearchResultAjaxRequest()
    GetSavedSearch(false, true);
}

function UpdateDiscoverySavedSearch(p_id) {
    if (_searchTerm.length > 0) {
        getConfirm("Update Saved Search", "Are you sure to update Saved Search", "Continue", "Cancel", function (res) {
            if (res == true) {
                $('#imgSaveSearchLoading').show();
                var mySearchTermArray = new Array();
                for (var zz = 0; zz < _searchTerm.length; zz++) {
                    mySearchTermArray.push(_searchTerm[zz].SearchTerm);
                }

                var jsonPostData = {
                    p_ID: p_id,
                    p_SearchTerm: mySearchTermArray
                }

                $.ajax({
                    type: 'POST',
                    dataType: 'json',
                    url: _urlDiscoveryUpdateSavedSearch,
                    contentType: 'application/json; charset=utf-8',
                    data: JSON.stringify(jsonPostData),
                    success: function (result) {
                        $('#imgSaveSearchLoading').hide();
                        if (result.isSuccess) {
                            ShowNotification(result.message);
                            GetSavedSearch(false, true);
                        }
                        else {
                            CheckForAuthentication(result, _msgErrorOccured);
                            //ShowNotification('Some error occured, try again later');
                        }
                    },
                    error: function (a, b, c) {
                        ShowNotification(_msgErrorOccured);
                    }
                });
            }
        });
    }
    else {
        ShowNotification(_msgEnterSearchTerm);
    }
}

function ShowAddToLibraryPopup() {
    if (ValidateCheckBoxSelection()) {
        $('#divAddToLibraryPopup').modal({
            backdrop: 'static',
            keyboard: true
        });
    }
    else {
        ShowNotification(_msgAtleastOneRecordSelect);
    }
}

function AddToDiscoveryLibrary() {

    if ($('#ddlLibraryCategory').val() != 0) {

        $('#ddlLibraryCategory').removeClass('warningInput');
        $('#imgAddToLibraryLoading').show();
        var mediaID = new Array();
        $("#divResult input[type=checkbox]").each(function () {
            if ($(this).is(':checked')) {
                var tempIDValue = $(this).val().trim().split(',');

                var mediaIDClass = new Object();
                mediaIDClass.MediaID = tempIDValue[0];
                mediaIDClass.MediaType = tempIDValue[1];

                mediaID.push(mediaIDClass);
            }
        });

        var jsonPostData = {
            p_CategoryGuid: $('#ddlLibraryCategory').val(),
            p_MediaID: mediaID
        }

        $.ajax({

            type: 'POST',
            dataType: 'json',
            url: _urlDiscoveryAddToDiscoveryLibrary,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            global: false,
            success: OnAddToDiscoveryLibrarySuccess,
            error: OnAddToDiscoveryLibraryFail
        });
    }
    else {
        $('#ddlLibraryCategory').addClass('warningInput');
        $('#ddlLibraryCategory').addClass('boxshadow');

        setTimeout(function () {
            $('#ddlLibraryCategory').removeClass('boxshadow');
        }, 2000);
    }
}

function OnAddToDiscoveryLibrarySuccess(result) {
    $('#imgAddToLibraryLoading').hide();
    ClearCheckboxSelection();
    if (result.isSuccess) {
        ClosePopUp('divAddToLibraryPopup');
        ShowNotification(result.message + " " + _msgRecordAddedToLibrary);

    }
    else {
        ShowNotification(_msgErrorOccured);
        ClosePopUp('divAddToReportPopup');
    }
}


function OnAddToDiscoveryLibraryFail(result) {
    $('#imgAddToLibraryLoading').hide();
    ShowNotification(_msgErrorOccured);
}

function ChartClick() {
    alert("Category :" + this.category + "\r\n" + "Search Term : " + this.SearchTerm);
}