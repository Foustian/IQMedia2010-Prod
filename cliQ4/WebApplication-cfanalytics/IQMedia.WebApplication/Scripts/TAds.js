var _SearchTerm = "";
var _FromDate = null;
var _ToDate = null;
var _Class = "";
var _ClassName = "";
var _Date = null;
var _IsAsc = false;
var _Duration = null;
var _IsNext = false;
var _SortColumn = '';
var _Title = '';
var _IsDefaultLoad = true;
var isFilterEnable = false;
var _CurrentStationFilter = new Array();
var _CurrentDmaFilter = new Array();
var _RequestDma = [];
var _OldRequestDma = [];
var _RequestStation = [];
var _OldRequestStation = [];
var _Country = null;
var _CountryName = '';
var _Region = null;
var _RegionName = '';

var _RequestStationID = [];
var _RequestStationIDDisplay = [];
var _OldRequestStationID = [];
var _CurrentStationIDFilter = [];
var _OldRequestLogoSearch = [];
var _RequestLogoSearch = [];
var _OldRequestLogoSearch = [];
var _RequestBrand = [];
var _OldRequestBrand = [];
var _RequestIndustry = [];
var _OldRequestIndustry = [];
var _VisibleLRIndustries = [];
var _VisibleLRBrands = [];
var _RequestCompany = [];
var _OldRequestCompany = [];
var _RequestPE = '';

var _IsManualHover = false;
var _HasRadioLoaded = false;

var _logoHits = [];
var _adHits = [];
var _searchLogoHits = [];
var filterToEarned = true;
var filterToPaid = true;

var _FullSearchLogoList = [];
var _FullBrandList = [];
var _FullCompanyList = [];
var _FullIndustryList = [];
var _FullPEList = [];

$(document).ready(function () {

    $('#ulMainMenu li').removeAttr("class");
    $('#liMenuTAds').attr("class", "active");

    var documentHeight = $(window).height();
    $("#divTVScrollContent").css({ "height": documentHeight - 250 });
    $("#divRadioScrollContent").css({ "height": documentHeight - 250 });

    $('#mCSB_1').css({ 'max-height': '' });
    $('#mCSB_2').css({ 'max-height': '' });

    $('#divMessage').html('');
    $('#dpFrom').val('');
    $('#dpTo').val('');

    $("#divCalender").datepicker({
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

    $("#txtKeyword").keypress(function (e) {
        if (e.keyCode == 13) {
            SetKeyword();

        }
    });
    $("#txtKeyword").blur(function () {
        SetKeyword();
    });
    $("#imgKeyword").click(function (e) {
        SetKeyword();
    });

    $("#txtTitle").keypress(function (e) {
        if (e.keyCode == 13) {
            SetTitle();

        }
    });
    $("#txtTitle").blur(function () {
        SetTitle();
    });
    $("#imgKeyword").click(function (e) {
        SetTitle();
    });


    $("#divTVScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });
    $("#divRadioScrollContent").mCustomScrollbar({
        advanced: {
            updateOnContentResize: true,
            autoScrollOnFocus: false
        }
    });

    $("body").click(function (e) {
        if (e.target.id == "liDmaFilter" || $(e.target).parents("#liDmaFilter").size() > 0) {
            if ($('#ulMarket').is(':visible')) {
                $('#ulMarket').hide();
            }
            else {
                $('#ulMarket').show();
            }
        }
        else if ((e.target.id !== "liDmaFilter" && e.target.id !== "ulMarket" && $(e.target).parents("#ulMarket").size() <= 0) || e.target.id == "btnSearchDma") {
            $('#ulMarket').hide();
            if (e.target.id != "btnSearchDma") {
                var iqdmalist = "";
                $.each(_CurrentDmaFilter, function (eventID, eventData) {
                    if (_OldRequestDma.length > 0 && $.inArray(eventData.Name, _OldRequestDma) !== -1 && $.inArray(eventData.Name, _RequestDma) == -1) {
                        _RequestDma.push(eventData.Name);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.Name, _RequestDma) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqdmalist = iqdmalist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetDma(this,'" + eventData.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Name + "</a></li>";
                });

                if (iqdmalist != "") {
                    $('#ulMarketList').html(iqdmalist);
                    $('#liDMASearch').show();
                }
                else {
                    $('#ulMarketList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liDMASearch').hide();
                }
            }
        }

        if (e.target.id == "liStationFilter" || $(e.target).parents("#liStationFilter").size() > 0) {
            if ($('#ulNetwork').is(':visible')) {
                $('#ulNetwork').hide();
            }
            else {
                $('#ulNetwork').show();
            }
        }
        else if ((e.target.id !== "liStationFilter" && e.target.id !== "ulNetwork" && $(e.target).parents("#ulNetwork").size() <= 0) || e.target.id == "btnSearchStation") {
            $('#ulNetwork').hide();
            if (e.target.id != "btnSearchStation") {
                var iqstationlist = "";
                $.each(_CurrentStationFilter, function (eventID, eventData) {
                    if (_OldRequestStation.length > 0 && $.inArray(eventData.Name, _OldRequestStation) !== -1 && $.inArray(eventData.Name, _RequestStation) == -1) {
                        _RequestStation.push(eventData.Name);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.Name, _RequestStation) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStation(this,'" + eventData.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Name + "</a></li>";
                });

                if (iqstationlist != "") {
                    $('#ulNetworkList').html(iqstationlist);
                    $('#liStationSearch').show();
                }
                else {
                    $('#ulNetworkList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liStationSearch').hide();
                }
            }
        }


        if (e.target.id == "liStationIDFilter" || $(e.target).parents("#liStationIDFilter").size() > 0) {
            if ($('#ulStation').is(':visible')) {
                $('#ulStation').hide();
            }
            else {
                $('#ulStation').show();
            }
        }
        else if ((e.target.id !== "liStationIDFilter" && e.target.id !== "ulStation" && $(e.target).parents("#ulStation").size() <= 0) || e.target.id == "btnSearchStationID") {
            $('#ulStation').hide();
            if (e.target.id != "btnSearchStationID") {
                var iqstationlist = "";
                $.each(_CurrentStationIDFilter, function (eventID, eventData) {
                    if (_OldRequestStationID.length > 0 && $.inArray(eventData.IQ_Station_ID, _OldRequestStationID) !== -1 && $.inArray(eventData.IQ_Station_ID, _RequestStationID) == -1) {
                        _RequestStationID.push(eventData.IQ_Station_ID);
                        _RequestStationIDDisplay.push(eventData.Station_Call_Sign);
                    }
                    var liStyle = "";
                    if ($.inArray(eventData.IQ_Station_ID, _RequestStationID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStationID(this,'" + eventData.IQ_Station_ID.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "','" + eventData.Station_Call_Sign.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + eventData.Station_Call_Sign + "</a></li>";
                });

                if (iqstationlist != "") {
                    $('#ulStationList').html(iqstationlist);
                    $('#liStationIDSearch').show();
                }
                else {
                    $('#ulStationList').html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">' + _msgNoFilterAvailable + '</a></li>');
                    $('#liStationIDSearch').hide();
                }
            }
        }

        if (e.target.id == "liBrandFilter" || $(e.target).parents("#liBrandFilter").size() > 0) {
            if ($('#ulBrandMain').is(':visible')) {
                $('#ulBrandMain').hide();
            }
            else {
                $('#ulBrandMain').show();
            }
        }
        else if ((e.target.id !== "liBrandFilter" && e.target.id !== "ulBrandMain" && $(e.target).parents("#ulBrandMain").size() <= 0) || e.target.id == "btnBrand") {
            $('#ulBrandMain').hide();
        }

        if (e.target.id == "liIndustryFilter" || $(e.target).parents("#liIndustryFilter").size() > 0) {
            if ($('#ulIndustryMain').is(':visible')) {
                $('#ulIndustryMain').hide();
            }
            else {
                $('#ulIndustryMain').show();
            }
        }
        else if ((e.target.id !== "liIndustryFilter" && e.target.id !== "ulIndustryMain" && $(e.target).parents("#ulIndustryMain").size() <= 0) || e.target.id == "btnIndustry") {
            $('#ulIndustryMain').hide();
        }

        if (e.target.id == "liCompanyFilter" || $(e.target).parents("#liCompanyFilter").size() > 0) {
            if ($('#ulCompanyMain').is(':visible')) {
                $('#ulCompanyMain').hide();
            }
            else {
                $('#ulCompanyMain').show();
            }
        }
        else if ((e.target.id !== "liCompanyFilter" && e.target.id !== "ulCompanyMain" && $(e.target).parents("#ulCompanyMain").size() <= 0) || e.target.id == "btnCompany") {
            $('#ulCompanyMain').hide();
        }

        if (e.target.id == "liLogoFilter" || $(e.target).parents("#liLogoFilter").size() > 0) {
            if ($('#ulSearchLogoMain').is(':visible')) {
                $('#ulSearchLogoMain').hide();
            }
            else {
                $('#ulSearchLogoMain').show();
            }
        }
        else if ((e.target.id !== "liLogoFilter" && e.target.id !== "ulSearchLogoMain" && $(e.target).parents("#ulSearchLogoMain").size() <= 0) || e.target.id == "btnSearchLogo") {
            $('#ulSearchLogoMain').hide();
        }

        if (e.target.id == "liPEFilter" || $(e.target).parents("#liPEFilter").size() > 0) {
            if ($('#ulPEMain').is(':visible')) {
                $('#ulPEMain').hide();
            }
            else {
                $('#ulPEMain').show();
            }
        }
        else if (e.target.id !== "liPEFilter" && e.target.id !== "ulPEMain" && $(e.target).parents("#ulPEMain").size() <= 0) {
            $('#ulPEMain').hide();
        }
    });

    $("#dpFrom").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    $("#dpTo").datepicker("setDate", new Date());
    _FromDate = $("#dpFrom").val();
    _ToDate = $("#dpTo").val();
    SetActiveFilter();
    GetFilters();
});

function CorrectTargetName(e) {
    var e = window.event || e;
    var targ = e.target || e.srcElement;
    return targ.name == "brandLogoItem";
}

function PopulateDefaultLogos(result) {
    if (result != null) {
        result = result.substring(3);
        result = result.substring(0, result.length - 2);
        result = result.replace(/&quot;/g,'"');
        _logoHits = JSON.parse(result);
    }
}
function PopulateDefaultAds(result) {
    if (result != null) {
        result = result.substring(3);
        result = result.substring(0, result.length - 2);
        result = result.replace(/&quot;/g, '"');
        _adHits = JSON.parse(result);
    }
}

function LoadSearchIDs(event, brandID, brandName, useSearchLogoHits) {
    // This event is fired whenever a logo item is selected. Only run it if a brand was selected.
    if (CorrectTargetName(event)) {
        //var iqLogolist = '<ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSubMedium">';     
        var iqLogolist =
            /*'<li role="presentation" id="liLogoBrandName" style="padding: 0px;">' +
                '<ul class="sideMenu sub-submenu" aria-labelledby="drop2" role="menu">' +
                    '<b>' + brandName + '</b>' +
                '</ul>' +
            '</li>' +*/
            '<li role="presentation" style="padding: 0px;">' +
                '<ul role="menu" class="sideMenu sub-submenu" id="ulSearchLogo" style="text-align:center;">';
        
        var count = 0;
        var possibleLogos = [];

        if (_RequestLogoSearch != null && _RequestLogoSearch.length > 0) {
            $.each(_RequestLogoSearch, function (index, value) {
                possibleLogos = $.grep(_FullSearchLogoList, function (e) { return e.ID == value; });
                $.each(possibleLogos, function (index, obj) {
                    if ($.inArray(obj.ID, _searchLogoHits) > -1) {
                        count++;
                        iqLogolist += "<li role=\"presentation\" class=\"cursorPointer\" onclick=\"SetLogo(this, " + obj.ID + ");\"><img src='" + EscapeHTML(obj.URL) + "' title='" + EscapeHTML(obj.Name) + "'/></li>";
                    }
                });
            });
        }
        else {
            possibleLogos = $.grep(_FullSearchLogoList, function (e) { return e.BrandID == brandID; });
            $.each(possibleLogos, function (index, obj) {
                if (!useSearchLogoHits || $.inArray(obj.ID, _searchLogoHits) > -1) {
                    count++;
                    iqLogolist += "<li role=\"presentation\" class=\"cursorPointer\" onclick=\"SetLogo(this, " + obj.ID + ");\"><img src='" + EscapeHTML(obj.URL) + "' title='" + EscapeHTML(obj.Name) + "'/></li>";
                }
            });
        }

        if (count == 0) {
            iqLogolist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
        }

        $("#ulSearchLogoSideMenu" + brandID).html(iqLogolist + "</ul></li>");
    }
}

$(window).resize(function () {
    if (screen.height >= 768) {
        $("#divTVScrollContent").css({ "height": documentHeight - 250 });
        $("#divRadioScrollContent").css({ "height": documentHeight - 250 });
    }
});

function CollapseExpandLeftSection(sectionid) {

    if (sectionid == 1) {

        $("#divTVSection").show("2000");
        $("#divTVContent").show("2000");

        $("#divRadioSection").hide("2000");
        $("#divRadioContent").hide("200");


        $("#h5TV").removeAttr("class");
        $("#h5TV").addClass("tvheader-active");

        $("#h5Radio").removeAttr("class");
        $("#h5Radio").addClass("radioheader-inactive");

        setTimeout(function () { $("#divTVScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
    }
    else {

        $("#divTVSection").hide("2000");
        $("#divTVContent").hide("2000");

        $("#divRadioSection").show("2000");
        $("#divRadioContent").show("200");

        $("#h5Radio").removeAttr("class");
        $("#h5Radio").addClass("radioheader-active");

        $("#h5TV").removeAttr("class");
        $("#h5TV").addClass("tvheader-inactive");

        if (!_HasRadioLoaded) {
            RefreshRadioResults(false, false);
            _HasRadioLoaded = true;
        }
        else {
            setTimeout(function () { $("#divRadioScrollContent").mCustomScrollbar("scrollTo", "top"); }, 200);
        }
    }
}

function SetKeyword() {
    if ($("#txtKeyword").val() != "" && _SearchTerm != $("#txtKeyword").val()) {
        _SearchTerm = $("#txtKeyword").val();
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetTitle() {
    if ($("#txtTitle").val() != "" && _Title != $("#txtTitle").val()) {
        _Title = $("#txtTitle").val();
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetDateVariable() {
    //do not get results from before 1/1/16
    var arrDate = $("#dpFrom").val().split("/");
    var fromDateFormat = new Date(arrDate[2], arrDate[0] - 1, arrDate[1]);
    var arrDate2 = $("#dpTo").val().split("/");
    var toDateFormat = new Date(arrDate2[2], arrDate2[0] - 1, arrDate2[1]);

    if ($("#dpFrom").val() == "") {
        $("#dpFrom").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    }
    else if (fromDateFormat <= new Date("December 31, 2015 23:59:59")) {
        $("#dpFrom").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    }

    if ($("#dpTo").val() == "") {
        $("#dpTo").datepicker("setDate", new Date());
    }
    else if (toDateFormat <= new Date("December 31, 2015 23:59:59")) {
        $("#dpTo").datepicker("setDate", new Date("January 1, 2016 00:00:00"));
    }

    if ($("#dpFrom").val() && $("#dpTo").val()) {
        $('#dpFrom').removeClass('warningInput');
        $('#dpTo').removeClass('warningInput');
        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            _IsDefaultLoad = false;
            SearchResult();
            $('#ulCalender').parent().removeClass('open');
            $('#aDuration').html('Custom&nbsp;&nbsp;<span class="caret"></span>');
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

function SetDma(eleDma, dmaname) {
    if ($.inArray(dmaname, _RequestDma) == -1) {
        _RequestDma.push(dmaname);
        $(eleDma).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestDma.indexOf(dmaname);
        if (catIndex > -1) {
            _RequestDma.splice(catIndex, 1);
            $(eleDma).css("background-color", "#ffffff");
        }
    }
}
function SearchDma() {
    if ($(_RequestDma).not(_OldRequestDma).length != 0 || $(_OldRequestDma).not(_RequestDma).length != 0) {
        _OldRequestDma = _RequestDma.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetStation(eleStation, stationname) {
    if ($.inArray(stationname, _RequestStation) == -1) {
        _RequestStation.push(stationname);
        $(eleStation).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestStation.indexOf(stationname);
        if (catIndex > -1) {
            _RequestStation.splice(catIndex, 1);
            $(eleStation).css("background-color", "#ffffff");
        }
    }
}
function SearchStation() {
    if ($(_RequestStation).not(_OldRequestStation).length != 0 || $(_OldRequestStation).not(_RequestStation).length != 0) {
        _OldRequestStation = _RequestStation.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetStationID(eleStation, stationid, stationiddisplay) {
    if ($.inArray(stationid, _RequestStationID) == -1) {
        _RequestStationID.push(stationid);
        _RequestStationIDDisplay.push(stationiddisplay);
        $(eleStation).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestStationID.indexOf(stationid);
        if (catIndex > -1) {
            _RequestStationID.splice(catIndex, 1);
            _RequestStationIDDisplay.splice(catIndex, 1);
            $(eleStation).css("background-color", "#ffffff");
        }
    }
}
function SearchStationID() {
    if ($(_RequestStationID).not(_OldRequestStationID).length != 0 || $(_OldRequestStationID).not(_RequestStationID).length != 0) {
        _OldRequestStationID = _RequestStationID.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetClass(classnum, classname) {
    _Class = classnum;
    _ClassName = classname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetCountry(countrynum, countryname) {
    _Country = countrynum;
    _CountryName = countryname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetRegion(regionnum, regionname) {
    _Region = regionnum;
    _RegionName = regionname;
    _IsDefaultLoad = false;
    SearchResult();
}

function SetLogo(eleName, id) {
/*    if ($.inArray(id, _RequestLogoSearch) == -1) {
        _RequestLogoSearch.push(id);
        $(eleName).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestLogoSearch.indexOf(id);
        if (catIndex > -1) {
            _RequestLogoSearch.splice(catIndex, 1);
            $(eleName).css("background-color", "#ffffff");
        }
    }*/
    _RequestLogoSearch = [];
    _RequestLogoSearch.push(id);
    SearchLogo();
}
function SearchLogo() {
    if ($(_RequestLogoSearch).not(_OldRequestLogoSearch).length != 0 || $(_OldRequestLogoSearch).not(_RequestLogoSearch).length != 0) {
        _OldRequestLogoSearch = _RequestLogoSearch.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetBrand(eleName, id) {
    /*if ($.inArray(id, _RequestBrand) == -1) {
        _RequestBrand.push(id);
        $(eleName).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestBrand.indexOf(id);
        if (catIndex > -1) {
            _RequestBrand.splice(catIndex, 1);
            $(eleName).css("background-color", "#ffffff");
        }
    }*/
    _RequestBrand = [];
    _RequestBrand.push(id);
    SearchBrand();
}
function SearchBrand() {
    if ($(_RequestBrand).not(_OldRequestBrand).length != 0 || $(_OldRequestBrand).not(_RequestBrand).length != 0) {
        _OldRequestBrand = _RequestBrand.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetIndustry(eleName, id) {
    /*if ($.inArray(name, _RequestIndustry) == -1) {
        _RequestIndustry.push(name);
        $(eleName).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestIndustry.indexOf(name);
        if (catIndex > -1) {
            _RequestIndustry.splice(catIndex, 1);
            $(eleName).css("background-color", "#ffffff");
        }
    }*/
    _RequestIndustry = [];
    _RequestIndustry.push(id);
    SearchIndustry();
}
function SearchIndustry() {
    if ($(_RequestIndustry).not(_OldRequestIndustry).length != 0 || $(_OldRequestIndustry).not(_RequestIndustry).length != 0) {
        _OldRequestIndustry = _RequestIndustry.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetCompany(eleName, id) {
    /*if ($.inArray(id, _RequestCompany) == -1) {
        _RequestCompany.push(id);
        $(eleName).css("background-color", "#E9E9E9");
    }
    else {
        var catIndex = _RequestCompany.indexOf(id);
        if (catIndex > -1) {
            _RequestCompany.splice(catIndex, 1);
            $(eleName).css("background-color", "#ffffff");
        }
    }*/
    _RequestCompany = [];
    _RequestCompany.push(id);
    SearchCompany();
}
function SearchCompany() {
    if ($(_RequestCompany).not(_OldRequestCompany).length != 0 || $(_OldRequestCompany).not(_RequestCompany).length != 0) {
        _OldRequestCompany = _RequestCompany.slice(0);
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function SetPE(type) {
    if (_RequestPE != type) {
        if (_RequestPE == "Earned") {
            filterToEarned = true;
            filterToPaid = false;
        }
        else if (_RequestPE == "Paid") {
            filterToEarned = false;
            filterToPaid = true;
        }
        _RequestPE = type;
        _IsDefaultLoad = false;
        SearchResult();
    }
}

function GetFilters() {
    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTAdsGetFilters,
        contentType: 'application/json; charset=utf-8',
        success: function (result) {
            if (result != null && result.isSuccess) {
                _FullSearchLogoList = result.logoList;
                _FullBrandList = result.brandList;
                _FullCompanyList = result.companyList;
                _FullIndustryList = result.industryList;
                _FullPEList = result.peList;
                _VisibleLRIndustries = result.visibleLRIndustries;
                _VisibleLRBrands = result.visibleLRBrands;

                //Logo Top Level
                if(_FullBrandList != null) {
                    var iqBrandlist = "";
                    var count = 0;
                    if(_VisibleLRBrands!=null){
                    for (i = 0; i < _VisibleLRBrands.length; i++) {
                        for (j = 0; j < _FullBrandList.length; j++) {
                            if (_VisibleLRBrands[i] == _FullBrandList[j].ID) {
                            count++
                                iqBrandlist += '<li id="liTopLevel' + _FullBrandList[j].ID + '" role=\"presentation\" class=\"cursorPointer\"><a data-toggle="dropdown" class="dropdown-toggle" onclick="LoadSearchIDs(event,' + _FullBrandList[j].ID + ',\'' + EscapeHTML(_FullBrandList[j].Name.replace("\'", "")) + '\', false);" tabindex="-1" role="button" name="brandLogoItem">' + EscapeHTML(_FullBrandList[j].Name) + '</a><ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSearchLogoSideMenu' + _FullBrandList[j].ID + '"></ul></li>';
                            break;
                            }
                        }
                      }
                    }else{
                    $.each(_FullBrandList, function (index, obj) {
                        count++;
                        iqBrandlist += '<li id="liTopLevel' + obj.ID + '" role=\"presentation\" class=\"cursorPointer\"><a data-toggle="dropdown" class="dropdown-toggle" onclick="LoadSearchIDs(event,' + obj.ID + ',\'' + EscapeHTML(obj.Name.replace("\'", "")) + '\', false);" tabindex="-1" role="button" name="brandLogoItem">' + EscapeHTML(obj.Name) + '</a><ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSearchLogoSideMenu' + obj.ID + '"></ul></li>';
                    });
                    }
                    if (count == 0) {
                        iqBrandlist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
                    }

                    $("#ulSearchLogo").html(iqBrandlist);
                }

                //Brand
                if (_FullBrandList != null) {
                    var iqBrandlist = "";
                    var count = 0;
                    if (_VisibleLRBrands != null) {
                        for (i = 0; i < _VisibleLRBrands.length; i++) {
                            for (j = 0; j < _FullBrandList.length; j++)  {                            
                                if (_FullBrandList[j].ID == _VisibleLRBrands[i]) {
                                   count++;
                                   iqBrandlist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetBrand(this," + _FullBrandList[j].ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(_FullBrandList[j].Name) + "</a></li>";
                                   break;
                                }
                            };
                        }
                    } else {
                        $.each(_FullBrandList, function (index, obj) {
                            count++;
                            iqBrandlist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetBrand(this," + obj.ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                        })
                    }
                    if (count == 0) {
                        iqBrandlist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
                    }

                    $("#ulBrand").html(iqBrandlist);
                }

                //Industry
                if (_FullIndustryList != null) {
                    var iqIndustrylist = "";
                    var count = 0;
                    if (_VisibleLRIndustries != null) {
                        for (i = 0; i < _VisibleLRIndustries.length; i++) {
                            for (j = 0; j < _FullIndustryList.length; j++) {
                                if (_FullIndustryList[j].ID == _VisibleLRIndustries[i]) {
                                   count++;
                                    iqIndustrylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetIndustry(this,'" + EscapeHTML(_FullIndustryList[j].ID) + "');\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(_FullIndustryList[j].Name) + "</a></li>"
                                    break;
                                }
                            }
                        }
                    } else {
                        $.each(_FullIndustryList, function (index, obj) {
                            count++;
                            iqIndustrylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetIndustry(this,'" + EscapeHTML(obj.ID) + "');\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                        });
                    }
                    if (count == 0) {
                        iqIndustrylist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
                    }

                    $("#ulIndustry").html(iqIndustrylist);
                }

                //Company
                if (_FullCompanyList != null) {
                    var iqCompanylist = "";
                    var count = 0;

                    $.each(_FullCompanyList, function (index, obj) {
                        count++;
                        iqCompanylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCompany(this," + obj.ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                    });

                    if (count == 0) {
                        iqCompanylist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
                    }

                    $("#ulCompany").html(iqCompanylist);
                }

                //PAID/EARNED
                var iqPElist = "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetPE('Paid');\" tabindex=\"-1\" role=\"menuitem\">Paid</a></li><li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetPE('Earned');\" tabindex=\"-1\" role=\"menuitem\">Earned</a></li>";
                $("#ulPE").html(iqPElist);
            }
        },
        error: OnFail
    });
}

function SearchResult() {
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_Title: _Title,
        p_Dma: _RequestDma,
        p_Station: _RequestStation,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_SortColumn: _SortColumn,
        p_IsDefaultLoad: _IsDefaultLoad,
        p_SearchLogo: _RequestLogoSearch,
        p_Brand: _RequestBrand,
        p_Industry: _RequestIndustry,
        p_Company: _RequestCompany,
        p_PaidEarned: _RequestPE
    }

    // alert('searchcalled');
    if (DateValidation()) {
        SetActiveFilter();

        if (isFilterEnable) {
            $('#divTAdsClearAll').show();
        }
        else {
            $('#divTAdsClearAll').hide();
        }

        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTAdsSearchResults,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}
function SearchResultPaging(isNextPage) {
    // alert(isNextPage);
    _IsNext = isNextPage;
    var jsonPostData = {
        p_FromDate: _FromDate,
        p_ToDate: _ToDate,
        p_SearchTerm: _SearchTerm,
        p_Title: _Title,
        p_Dma: _RequestDma,
        p_Station: _RequestStation,
        p_IQStationID: _RequestStationID,
        p_Class: _Class,
        p_Region: _Region,
        p_Country: _Country,
        p_IsAsc: _IsAsc,
        p_IsNext: _IsNext,
        p_SortColumn: _SortColumn,
        p_SearchLogo: _RequestLogoSearch,
        p_Brand: _RequestBrand,
        p_Industry: _RequestIndustry,
        p_Company: _RequestCompany,
        p_PaidEarned: _RequestPE
    }

    // alert('searchcalled');
    if (DateValidation()) {
        SetActiveFilter();
        $.ajax({
            type: 'POST',
            dataType: 'json',
            url: _urlTAdsSearchResultsPaging,
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(jsonPostData),
            success: OnResultSearchComplete,
            error: OnFail
        });
    }


}
function OnResultSearchComplete(result) {
    if (result.isSuccess) {
        $("#divPreviousNext").show();
        $('#lblRecords').html('');
        $('#ulTAdsResults').html('');
        $('#ulTAdsResults').html(result.HTML);
        if (result.hasMoreResult) {
            $('#btnNextPage').show();
        }
        else {
            $('#btnNextPage').hide();
        }

        if (result.hasPreviouResult) {
            $('#btnPreviousPage').show();
        }
        else {
            $('#btnPreviousPage').hide();
        }
        if (result.recordNumber != '') {
            $('#lblRecords').html(result.recordNumber);
        }

        if (result.filters != null) {
            if (result.filters["IQ_Dma"] != null) {
                var iqdmalist = "";
                $.each(result.filters["IQ_Dma"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.Name, _RequestDma) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqdmalist = iqdmalist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetDma(this,'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulMarketList").html(iqdmalist);
                _CurrentDmaFilter = result.filters["IQ_Dma"].slice(0);
            }
            if (result.filters["Station_Affil"] != null) {
                var iqstationlist = "";
                $.each(result.filters["Station_Affil"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.Name, _RequestStation) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStation(this,'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulNetworkList").html(iqstationlist);
                _CurrentStationFilter = result.filters["Station_Affil"].slice(0);
            }
            if (result.filters["IQ_Station"] != null) {
                var iqstationlist = "";
                $.each(result.filters["IQ_Station"], function (index, obj) {
                    var liStyle = "";
                    if ($.inArray(obj.IQ_Station_ID, _RequestStationID) !== -1) {
                        liStyle = "style=\"background-color: rgb(233, 233, 233);\"";
                    }
                    iqstationlist = iqstationlist + "<li role=\"presentation\" class=\"cursorPointer\"><a " + liStyle + " onclick=\"SetStationID(this,'" + obj.IQ_Station_ID.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "','" + obj.Station_Call_Sign.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Station_Call_Sign + "</a></li>";
                });

                $("#ulStationList").html(iqstationlist);
                _CurrentStationIDFilter = result.filters["IQ_Station"].slice(0);
            }
            if (result.filters["IQ_Country"] != null) {
                var iqcountrylist = "";
                $.each(result.filters["IQ_Country"], function (index, obj) {
                    iqcountrylist = iqcountrylist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCountry(" + obj.Num + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulCountry").html(iqcountrylist);
            }
            if (result.filters["IQ_Region"] != null) {
                var iqregionlist = "";
                $.each(result.filters["IQ_Region"], function (index, obj) {
                    iqregionlist = iqregionlist + "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetRegion(" + obj.Num + ",'" + obj.Name.replace(/"/g, '&quot;').replace(/'/g, '\\\'') + "');\" tabindex=\"-1\" role=\"menuitem\">" + obj.Name + "</a></li>";
                });
                $("#ulRegion").html(iqregionlist);
            }

            //Logo Top Level
            useSearchLogoHits = true;
            $("#ulSearchLogo").html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>');
            $("#ulBrand").html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>');
            if (result.filters["IQ_Logo"] != null) {
                _searchLogoHits = result.filters["IQ_Logo"];
            }
            if (_RequestLogoSearch != null && _RequestLogoSearch.length > 0) {
                var iqLogolist = "";
                var count = 0;
                $.each(_RequestLogoSearch, function (index, value) {
                    var posLogoItems = $.grep(_FullSearchLogoList, function (e) { return e.ID == value; });
                    var posBrandItems = [];

                    $.each(posLogoItems, function (index, logo) {
                        var brand = $.grep(_FullBrandList, function (e) { return e.ID == logo.BrandID; });

                        if (brand[0] != null && brand[0].ID != null && brand[0].Name != null) {
                            posBrandItems.push(brand[0]);
                        }
                    });

                    $.each(posBrandItems, function (index, obj) {
                        count++;
                        iqLogolist += '<li role=\"presentation\" class=\"cursorPointer\"><a data-toggle="dropdown" class="dropdown-toggle" onclick="LoadSearchIDs(event,' + obj.ID + ',\'' + EscapeHTML(obj.Name.replace("\'", "")) + '\', true);" tabindex="-1" role="button" name="brandLogoItem">' + EscapeHTML(obj.Name) + '</a><ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSearchLogoSideMenu' + obj.ID + '"></ul></li>';
                    });
                });

                if (count > 0) {
                    $("#ulSearchLogo").html(iqLogolist);
                }
            }
            else {
                if (result.filters["IQ_Brand"] != null) {
                    var iqLogolistArray = [];
                    var iqLogolist = "";
                    var count = 0;
                    $.each(result.filters["IQ_Brand"], function (index, value) {
                        var posObj = $.grep(_FullBrandList, function (e) { return e.ID == value; });

                        if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                            count++;
                            var obj = posObj[0];
                            iqLogolistArray.push(obj);
                        }
                    });

                    if (count > 0) {
                        iqLogolistArray.sort(SortByName);
                        $.each(iqLogolistArray, function (index, obj) {
                            iqLogolist += '<li role=\"presentation\" class=\"cursorPointer\"><a data-toggle="dropdown" class="dropdown-toggle" onclick="LoadSearchIDs(event,' + obj.ID + ',\'' + EscapeHTML(obj.Name.replace("\'", "")) + '\', true);" tabindex="-1" role="button" name="brandLogoItem">' + EscapeHTML(obj.Name) + '</a><ul aris-labelledby="drop2" role="menu" class="dropdown-menu sideMenu" id="ulSearchLogoSideMenu' + obj.ID + '"></ul></li>';
                        });
                        $("#ulSearchLogo").html(iqLogolist);
                    }
                }
            }

            //Brand
            if (_RequestBrand != null && _RequestBrand.length > 0) {
                var iqBrandlist = "";
                var count = 0;
                $.each(_RequestBrand, function (index, value) {
                    var posObj = $.grep(_FullBrandList, function (e) { return e.ID == value; });

                    if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                        count++;
                        var obj = posObj[0];
                        iqBrandlist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetBrand(this," + obj.ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                    }
                });

                if (count > 0) {
                    $("#ulBrand").html(iqBrandlist);
                }
            }
            else {
                if (result.filters["IQ_Brand"] != null) {
                    var iqBrandlistArray = [];
                    var iqBrandlist = "";
                    var count = 0;
                    $.each(result.filters["IQ_Brand"], function (index, value) {
                        var posObj = $.grep(_FullBrandList, function (e) { return e.ID == value; });

                        if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                            count++;
                            var obj = posObj[0];
                            iqBrandlistArray.push(obj);
                        }
                    });

                    if (count > 0) {
                        iqBrandlistArray.sort(SortByName);
                        $.each(iqBrandlistArray, function (index, obj) {
                            iqBrandlist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetBrand(this," + obj.ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                        });
                        $("#ulBrand").html(iqBrandlist);
                    }
                }
            }

            //Industry
            $("#ulIndustry").html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>');
            if (_RequestIndustry != null && _RequestIndustry.length > 0) {
                var iqIndustrylist = "";
                var count = 0;

                $.each(_RequestIndustry, function (index, value) {
                    var posObj = $.grep(_FullIndustryList, function (e) { return e.ID == value; });

                    if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                        count++;
                        var obj = posObj[0];
                        iqIndustrylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetIndustry(this,'" + EscapeHTML(obj.ID) + "');\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                    }
                });

                if (count > 0) {
                    $("#ulIndustry").html(iqIndustrylist);
                }
            }
            else {
                if (result.filters["IQ_Industry"] != null) {
                    var iqIndustrylistArray = [];
                    var iqIndustrylist = "";
                    var count = 0;

                    $.each(result.filters["IQ_Industry"], function (index, value) {
                        var posObj = $.grep(_FullIndustryList, function (e) { return e.ID == value; });

                        if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                            count++;
                            var obj = posObj[0];
                            iqIndustrylistArray.push(obj);
                        }
                    });

                    if (count > 0) {
                        iqIndustrylistArray.sort(SortByName);
                        $.each(iqIndustrylistArray, function (index, obj) {
                            iqIndustrylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetIndustry(this,'" + EscapeHTML(obj.ID) + "');\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                        });
                        $("#ulIndustry").html(iqIndustrylist);
                    }
                }
            }

            //Company
            $("#ulCompany").html('<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>');
            if (_RequestCompany != null && _RequestCompany.length > 0) {
                var iqCompanylist = "";
                var count = 0;

                $.each(_RequestCompany, function (index, value) {
                    var posObj = $.grep(_FullCompanyList, function (e) { return e.ID == value; });

                    if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                        count++;
                        var obj = posObj[0];
                        iqCompanylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCompany(this," + obj.ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                    }
                });

                if (count > 0) {
                    $("#ulCompany").html(iqCompanylist);
                }
            }
            else{
                if (result.filters["IQ_Company"] != null) {
                    var iqCompanylistArray = [];
                    var iqCompanylist = "";
                    var count = 0;

                    $.each(result.filters["IQ_Company"], function (index, value) {
                        var posObj = $.grep(_FullCompanyList, function (e) { return e.ID == value; });

                        if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                            count++;
                            var obj = posObj[0];
                            iqCompanylistArray.push(obj);
                        }
                    });

                    if (count > 0) {
                        iqCompanylistArray.sort(SortByName);
                        $.each(iqCompanylistArray, function (index, obj) {
                            iqCompanylist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetCompany(this," + obj.ID + ");\" tabindex=\"-1\" role=\"menuitem\">" + EscapeHTML(obj.Name) + "</a></li>";
                        });
                        $("#ulCompany").html(iqCompanylist);
                    }
                }
            }
        }

        //PAID/EARNED
        var iqPElist = "";
        if (filterToPaid) {
            iqPElist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetPE('Paid');\" tabindex=\"-1\" role=\"menuitem\">Paid</a></li>";
        }
        if (filterToEarned) {
            iqPElist += "<li role=\"presentation\" class=\"cursorPointer\"><a onclick=\"SetPE('Earned');\" tabindex=\"-1\" role=\"menuitem\">Earned</a></li>";
        }
        if (iqPElist.trim() == '') {
            iqPElist += '<li role="presentation" class="cursorPointer"><a tabindex="-1" role="menuitem">No Filter Available</a></li>';
        }
        $("#ulPE").html(iqPElist);

        //Logos and Ads
        if (result.logoHits != null && result.logoHits.length > 0) _logoHits = result.logoHits;
        if (result.adHits != null && result.adHits.length > 0) _adHits = result.adHits;
    }
    else {
        ClearResultsOnError('ulTAdsResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
    }
}
function OnFail(result) {
    _IsTabChange = false;

    ShowNotification(_msgErrorOccured);
    ClearResultsOnError('ulTAdsResults', 'divPreviousNext', '', _msgErrorOnSearch.replace(/@@MethodName@@/g, "SearchResult()"));
}

function SortByName(a, b) {
    var aName = a.Name.toLowerCase();
    var bName = b.Name.toLowerCase();
    return ((aName < bName) ? -1 : ((aName > bName) ? 1 : 0));
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
function SetActiveFilter() {

    isFilterEnable = false;
    $("#divActiveFilter").html("");


    if (_SearchTerm != null && _SearchTerm != "") {
        $('#divActiveFilter').append('<div id="divKeywordActiveFilter" class="filter-in">' + EscapeHTML(_SearchTerm) + '<span class="cancel" onclick="RemoveFilter(1);"></span></div>');
        isFilterEnable = true;
    }

    if ((_FromDate != null && _FromDate != "") && (_ToDate != null && _ToDate != "")) {
        $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">' + _FromDate + ' To ' + _ToDate + '<span class="cancel" onclick="RemoveFilter(2);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestDma != null && _RequestDma.length > 0) {
        $('#divActiveFilter').append('<div id="divDmaActiveFilter" class="filter-in">' + _RequestDma.join() + '<span class="cancel" onclick="RemoveFilter(3);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStation != null && _RequestStation.length > 0) {
        $('#divActiveFilter').append('<div id="divStationActiveFilter" class="filter-in">' + _RequestStation.join() + '<span class="cancel" onclick="RemoveFilter(4);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestStationIDDisplay != null && _RequestStationIDDisplay.length > 0) {
        $('#divActiveFilter').append('<div id="divStationIDActiveFilter" class="filter-in">' + _RequestStationIDDisplay.join() + '<span class="cancel" onclick="RemoveFilter(7);"></span></div>');
        isFilterEnable = true;
    }
    if (_Class != null && _Class != "") {
        $('#divActiveFilter').append('<div id="divClassActiveFilter" class="filter-in">' + _ClassName + '<span class="cancel" onclick="RemoveFilter(5);"></span></div>');
        isFilterEnable = true;
    }
    if (_Title != null && _Title != "") {
        $('#divActiveFilter').append('<div id="divTitleActiveFilter" class="filter-in">' + EscapeHTML(_Title) + '<span class="cancel" onclick="RemoveFilter(6);"></span></div>');
        isFilterEnable = true;
    }

    if (_Region != null && _Region != "") {
        $('#divActiveFilter').append('<div id="divRegionActiveFilter" class="filter-in">' + _RegionName + '<span class="cancel" onclick="RemoveFilter(8);"></span></div>');
        isFilterEnable = true;
    }
    if (_Country != null && _Country != "") {
        $('#divActiveFilter').append('<div id="divCountryActiveFilter" class="filter-in">' + _CountryName + '<span class="cancel" onclick="RemoveFilter(9);"></span></div>');
        isFilterEnable = true;
    }

    if (_RequestLogoSearch != null && _RequestLogoSearch.length > 0) {
        $('#divActiveFilter').append('<div id="divLogoSearchActiveFilter" class="filter-in">Search Images<span class="cancel" onclick="RemoveFilter(10);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestBrand != null && _RequestBrand.length > 0) {
        var brand = '';
        $.each(_RequestBrand, function (index, value) {
            var posObj = $.grep(_FullBrandList, function (e) { return e.ID == value; });

            if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                if (brand != '') brand += ","
                brand += posObj[0].Name;
            }
        });
        
        $('#divActiveFilter').append('<div id="divBrandActiveFilter" class="filter-in">' + brand + '<span class="cancel" onclick="RemoveFilter(11);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestCompany != null && _RequestCompany.length > 0) {
        var company = '';
        $.each(_RequestCompany, function (index, value) {
            var posObj = $.grep(_FullCompanyList, function (e) { return e.ID == value; });

            if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                if (company != '') company += ","
                company += posObj[0].Name;
            }
        });

        $('#divActiveFilter').append('<div id="divCompanyActiveFilter" class="filter-in">' + company + '<span class="cancel" onclick="RemoveFilter(12);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestIndustry != null && _RequestIndustry.length > 0) {
        var industry = '';
        $.each(_RequestIndustry, function (index, value) {
            var posObj = $.grep(_FullIndustryList, function (e) { return e.ID == value; });

            if (posObj[0] != null && posObj[0].ID != null && posObj[0].Name != null) {
                if (industry != '') industry += ","
                industry += posObj[0].Name;
            }
        });

        $('#divActiveFilter').append('<div id="divIndustryActiveFilter" class="filter-in">' + industry + '<span class="cancel" onclick="RemoveFilter(13);"></span></div>');
        isFilterEnable = true;
    }
    if (_RequestPE != null && _RequestPE.length > 0) {
        $('#divActiveFilter').append('<div id="divPEActiveFilter" class="filter-in">' + _RequestPE + '<span class="cancel" onclick="RemoveFilter(14);"></span></div>');
        isFilterEnable = true;
    }

    if (isFilterEnable) {
        $("#divActiveFilter").css({ 'border-bottom': '1px solid rgb(236, 236, 236)', 'margin-bottom': '5px' });
    }
    else {
        $("#divActiveFilter").css({ 'border-bottom': '' });
    }
}

function RemoveFilter(filterType) {

    var refreshResults = false;

    _IsDefaultLoad = false;
    // Represent SearchKeyword
    if (filterType == 1) {

        $("#txtKeyword").val("");
        _SearchTerm = "";
        refreshResults = true;
    }

    // Represent Date filter(From Date,To Date)
    if (filterType == 2) {

        $("#dpFrom").datepicker("setDate", null);
        $("#dpTo").datepicker("setDate", null);
        $("#divCalender").datepicker("setDate", null);

        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');

        _FromDate = null;
        _ToDate = null;
        refreshResults = true;
    }

    // Represent Dma Filter
    if (filterType == 3) {
        _RequestDma = [];
        _OldRequestDma = [];
        refreshResults = true;
    }

    // Represent Station Filter
    if (filterType == 4) {
        _RequestStation = [];
        _OldRequestStation = [];
        refreshResults = true;
    }
    // Represent Class Filter
    if (filterType == 5) {
        _Class = "";
        _ClassName = "";
        refreshResults = true;
    }

    if (filterType == 6) {

        $("#txtTitle").val("");
        _Title = "";
        refreshResults = true;
    }

    // Represent Station Filter
    if (filterType == 7) {
        _RequestStationID = [];
        _RequestStationIDDisplay = [];
        _OldRequestStationID = [];
        refreshResults = true;
    }

    if (filterType == 8) {
        _Region = null;
        _RegionName = "";
        refreshResults = true;
    }

    if (filterType == 9) {
        _Country = null;
        _CountryName = "";
        refreshResults = true;
    }

    // Represent Logo Search Filter
    if (filterType == 10) {
        _OldRequestLogoSearch = [];
        _RequestLogoSearch = [];
        refreshResults = true;
    }

    // Represent Brand Filter
    if (filterType == 11) {
        _OldRequestBrand = [];
        _RequestBrand = [];
        refreshResults = true;
    }
    
    // Represent Company Filter
    if (filterType == 12) {
        _OldRequestCompany = [];
        _RequestCompany = [];
        refreshResults = true;
    }
    
    // Represent Industry Filter
    if (filterType == 13) {
        _OldRequestIndustry = [];
        _RequestIndustry = [];
        refreshResults = true;
    }
    
    // Represent PE Filter
    if (filterType == 14) {
        _RequestPE = "";
        filterToEarned = true;
        filterToPaid = true;
        refreshResults = true;
    }

    if (refreshResults) {
        SearchResult();
    }
}
function SortDirection(p_SortColumn, p_IsAsc) {

    if (p_IsAsc != _IsAsc || _SortColumn != p_SortColumn) {
        _IsAsc = p_IsAsc;
        _SortColumn = p_SortColumn;

        if (_SortColumn == 'Date' && _IsAsc) {
            $('#aSortDirection').html(_msgOldestFirst + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Date' && !_IsAsc) {
            $('#aSortDirection').html(_msgMostRecent + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Market' && _IsAsc) {
            $('#aSortDirection').html(_msgMarketAscending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        else if (_SortColumn == 'Market' && !_IsAsc) {
            $('#aSortDirection').html(_msgMarketDescending + '&nbsp;&nbsp;<span class="caret"></span>');
        }
        _IsDefaultLoad = false;
        SearchResult();
    }
}
function GetResultOnDuration(duration) {

    $("#dpFrom").removeClass("warningInput");
    $("#dpTo").removeClass("warningInput");
    var dtcurrent = new Date();
    var fDate;
    _Duration = duration;

    // All
    if (duration == 0) {
        $("#dpFrom").val("");
        $("#dpTo").val("");
        dtcurrent = "";
        RemoveFilter(2);
        $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    }

    // 24 hours
    else if (duration == 1) {
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

        if (_FromDate != $("#dpFrom").val() || _ToDate != $("#dpTo").val()) {
            _FromDate = $("#dpFrom").val();
            _ToDate = $("#dpTo").val();
            _IsDefaultLoad = false;
            SearchResult();
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

function ClosePopUp(divID) {
    $('#' + divID).css({ "display": "none" });
    $('#' + divID).modal('hide');
}

function ClearSearch() {
    _FromDate = null;
    _ToDate = null;
    _SearchTerm = "";
    _RequestDma = [];
    _OldRequestDma = [];
    _RequestStation = [];
    _OldRequestStation = [];
    _RequestStationID = [];
    _RequestStationIDDisplay = [];
    _OldRequestStationID = [];
    _Class = "";
    _ClassName = "";
    _Country = null;
    _Region = null;
    _Title = "";
    _IsAsc = false;
    _SortColumn = "";
    _IsDefaultLoad = true;
    _RequestLogoSearch = [];
    _RequestBrand = [];
    _RequestCompany = [];
    _RequestIndustry = [];
    _RequestPE = "";
    filterToEarned = true;
    filterToPaid = true;


    $('#aDuration').html(_msgAll + '&nbsp;&nbsp;<span class="caret"></span>');
    $("#dpFrom").datepicker("setDate", null);
    $("#dpTo").datepicker("setDate", null);
    $("#divCalender").datepicker("setDate", null);
    $("#txtTitle").val("");
    $("#txtKeyword").val("");

    SearchResult();
}

var dfdPlayerInfoLoaded = new $.Deferred();

function GetRawData(iqcckey) {
    var jsonPostData = {
        iqcckey: iqcckey
    }

    $.ajax({
        type: 'POST',
        dataType: 'json',
        url: _urlTAdsGetRawData,
        contentType: 'application/json; charset=utf-8',
        data: JSON.stringify(jsonPostData),
        success: function (result) {
            if (result != null && result.isSuccess) {
                window.open(_urlTAdsGetRawDataXML);
                window.open(_urlTAdsGetRawDataTGZ);
            } 
            else {
                ShowNotification("There was an error getting the files.");
            }
        },
        error: function (result) {
            ShowNotification("There was an error getting the files."); 
        }
    });
}

function LoadChartNPlayer(rawMediaGuid, iqcckey, title) {
    dfdPlayerInfoLoaded = new $.Deferred();
    LoadPlayerbyGuidTS(rawMediaGuid, title);
    var logoHits = ParseLogoStrings(iqcckey);
    var adHits = ParseAdStrings(iqcckey);

    $.when(dfdPlayerInfoLoaded).then(
        function () {
            var jsonPostData = {
                IQ_CC_KEY: iqcckey,
                RAW_MEDIA_GUID: rawMediaGuid,
                sortByHitStart: true,
                lstSearchTermHits: _searchTermHits,
                lstLogoHitStrings: logoHits,
                lstAdHitStrings: adHits
            }

            $.ajax({
                type: 'POST',
                dataType: 'json',
                url: _urlTAdsGetChart,
                contentType: 'application/json; charset=utf-8',
                data: JSON.stringify(jsonPostData),
                success: OnGetChartComplete,
                error: OnGetChartFail
            });
        }
    );
}
function OnGetChartComplete(result) {
    if (result.isSuccess) {
        if (result.hasTAdsResults && result.tAdsResultsJson.length > 0) {
            $('.ads-chart-content').html('');
            $('.ads-chart-content').append('<div class="float-left" id="ads-results" style="width:100%; padding-right:20px; padding-left:20px; background-color: #FFFFFF""></div>');
            numOfVisibleLogos = result.yAxisCompanies.length;
            yAxisInfoList = [];
            $.each(result.yAxisCompanies, function (index, value) {
                var yAxisInfo = {};
                yAxisInfo.ID = [];
                yAxisInfo.Position = [];
                yAxisInfo.ID.push(index + 1);
                yAxisInfo.Position.push(index + 1);
                yAxisInfo.yAxisCompany = result.yAxisCompanies[index];
                yAxisInfo.yAxisLogoPath = result.yAxisLogoPaths[index];

                yAxisInfoList.push(yAxisInfo);
            });
            tAdsResultsJson = result.tAdsResultsJson;

            RenderHighCharts(result.tAdsResultsJson, 'ads-results');
        }
        else {
            $('.ads-chart-content').append('<table style="width:100%; background-color:#c0c0c0"><tr><td>There is no available data.</td></tr></table>');

        }
        if (result.isTimeSync && result.lineChartJson.length > 0) {
            $('.video-chart').closest('li').show();
            $('.chart-tabs').html('');
            $('.chart-tab-content').html('');
            $.each(result.lineChartJson, function (index, obj) {
                $('.chart-tabs').append('<div class="chartTabHeader" id="video-parent-tab-' + index + '"><div class="padding5" id="video-tab-' + index + '" onclick="changeChartTab(' + index + ');">' + obj.Type + '</div></div>');
                $('.chart-tab-content').append('<div class="float-left" id="video-chart-' + index + '" style="width:1020px;"></div>');
                RenderHighCharts(obj.Data, 'video-chart-' + index);
            });

            changeChartTab(0);
            $(".chart-tab-content").on("mouseout", function () {
                _IsManualHover = false;
            });
        }
    }
    else {
        ShowNotification(_msgErrorOccured);
    }
}
function OnGetChartFail(result) {
    ShowNotification(_msgErrorOccured);
}
function ParseLogoStrings(iqcckey) {
    var returnLogos = [];
    if (_logoHits.length > 0) {
        var logos = $.grep(_logoHits, function (e) { return iqcckey in e; });
        $.each(logos, function (index, value) {
            var logo = value[iqcckey];
            returnLogos.push(logo);
        });
    }

    return returnLogos;
}
function ParseAdStrings(iqcckey) {
    var returnAds = [];
    if (_adHits.length > 0) {
        var ads = $.grep(_adHits, function (e) { return iqcckey in e; });
        $.each(ads, function (index, value) {
            var date = value[iqcckey];
            returnAds.push(date);
        });
    }

    return returnAds;
}

function RenderHighCharts(jsonLineChartData, chartID) {

    var JsonLineChart = JSON.parse(jsonLineChartData);
    JsonLineChart.plotOptions.series.point.events.click = LineChartClick;
    JsonLineChart.xAxis.labels.formatter = FormatTime
    JsonLineChart.tooltip.formatter = tooltipFormat
    JsonLineChart.plotOptions.series.events.show = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.events.hide = HandleSeriesShowHide;
    JsonLineChart.plotOptions.series.point.events.mouseOver = ChartHoverManage;
    JsonLineChart.plotOptions.series.point.events.mouseOut = ChartHoverOutManage;

    $('#' + chartID).highcharts(JsonLineChart);
}

function HandleSeriesShowHide() {

    if (!_IsManualHover) {
        var chart = this.chart;
        xIndex = chart.axes[0].categories.indexOf(_currentTimeInt.toString());
        if (chart.series[0].visible || chart.series[1].visible) {
            if (chart.series[0].visible && chart.series[1].visible) {
                chart.tooltip.refresh([chart.series[0].data[xIndex], chart.series[1].data[xIndex]]);
            }
            else if (chart.series[0].visible) {
                chart.tooltip.refresh([chart.series[0].data[xIndex]]);
            }
            else {
                chart.tooltip.refresh([chart.series[1].data[xIndex]]);
            }
        }
        else {
            chart.series[0].data[xIndex].setState('');
            chart.series[1].data[xIndex].setState('');
            chart.tooltip.hide();
        }
    }

}

function tooltipFormat() {
    var s = [];

    var totalSeconds = this.x;
    var minutes = Math.floor(totalSeconds / 60);
    var seconds = totalSeconds - minutes * 60;
    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;


    str = minutes + ':' + seconds;
    $.each(this.points, function (i, point) {
        var seriesName = this.series.tooltipOptions.valuePrefix;

        /*if (point.series.index == 0) {
        seriesName = 'Kantar (second by second)';
        }
        else {
        seriesName = 'Nielsen (minute by minute)';
        }*/

        str += '<br/><span style="color:' + point.series.color + ';font-weight:bold;">' + seriesName + '</span><span style="color:' + point.series.color + ';"> = ' +
                    numberWithCommas(point.y) + '</span>';
    });
    return str;
}

function numberWithCommas(x) {
    return x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
}

function FormatTime() {
    var minutes = Math.floor(this.value / 60);
    var seconds = this.value - minutes * 60;

    minutes = minutes < 10 ? '0' + minutes : minutes;
    seconds = seconds < 10 ? '0' + seconds : seconds;
    return minutes + ':' + seconds;
}

function ChartHoverOutManage() {
    _IsManualHover = false;
    console.log("chart hover out");
}

function ChartHoverManage() {
    _IsManualHover = true;
    console.log("chart hover");
}


function LineChartClick() {
    setSeekPoint(this.category);
}

function changeChartTab(tabNumber) {

    // hide all tabs
    $("div[id ^= 'video-chart-']").css({ opacity: 0 })
    $("div[id ^= 'video-chart-']").css({ height: "0" });

    $("div[id ^= 'video-tab-']").removeClass('playerChartTabActive');
    //$("div[id ^= 'video-parent-tab-']").removeClass('playerChartTabActiveParent');


    // show current tab
    $('#video-tab-' + tabNumber).addClass('playerChartTabActive');
    //$('#video-parent-tab-' + tabNumber).addClass('playerChartTabActiveParent');
    $('#video-chart-' + tabNumber).css({ height: "auto", opacity: 1 })
}