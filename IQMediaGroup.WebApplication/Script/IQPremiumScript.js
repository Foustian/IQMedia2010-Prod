function Changed(dateobj) {
    var ChangedDate = new Date(dateobj.value);
    $find("CalendarExtenderFromDate").set_selectedDate(ChangedDate);
}

function ChangeTab(tabID, tabMainID, Filter, isGrid) {
    $('#' + tabMainID + ' div').removeClass('active');
    $('#' + tabID).addClass('active');
    GetDataOnTab(Filter, isGrid);
}

//Filter
//0 == TV
//1 == Radio
//2 == News
//3 == Social Media
//4 == Twitter

//By IsGrid we can change tab accordingly
function GetDataOnTab(Filter, isGrid) {
    document.getElementById('ctl00_Content_Data_ucIQpremium_hfcurrentTab').value = Filter;
    if (Filter == 0) {
        if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfTVStatus').value != 1) {
            $("#divTVResultInner").html('');
            $("#divTVChartInner").html('')
            $('html, body').animate({
                scrollTop: $("#divResult").offset().top - 600
            }, 1000);
            __doPostBack('ctl00_Content_Data_ucIQpremium_upGrid', "Tab");
        }

        showTVTab();
    }
    else if (Filter == 1) {
        if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfRadioStatus').value != 1) {
            $("#divRadioResultInner").html('');
            $("#divRadioChartInner").html('');
            $('html, body').animate({
                scrollTop: $("#divResult").offset().top - 600
            }, 1000);
            __doPostBack('ctl00_Content_Data_ucIQpremium_upRadio', "Tab");
        }
        showRadioTab();

    }
    else if (Filter == 2) {
        if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfOnlineNewsStatus').value != 1) {
            $("#divOnlineNewsResultInner").html('');
            $("#divOLChartInner").html('');
            $('html, body').animate({
                scrollTop: $("#divResult").offset().top - 600
            }, 1000);
            __doPostBack('ctl00_Content_Data_ucIQpremium_upOnlineNews', "Tab");
        }
        showOnlineNewsTab();

    }
    else if (Filter == 3) {
        if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfSocialMediaStatus').value != 1) {
            $("#divSocialMediaResultInner").html('');
            $("#divSMChartInner").html('');
            $('html, body').animate({
                scrollTop: $("#divResult").offset().top - 600
            }, 1000);
            __doPostBack('ctl00_Content_Data_ucIQpremium_upSocialMedia', "Tab");
        }
        showSocialMediaTab();

    }
    else if (Filter == 4) {
        if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfTwitterMediaStatus').value != 1) {
            $("#divTwitterResultInner").html('');
            $("#divTwitterChartInner").html('');
            $('html, body').animate({
                scrollTop: $("#divResult").offset().top - 600
            }, 1000);
            __doPostBack('ctl00_Content_Data_ucIQpremium_upTwitter', "Tab");
        }
        showTwitterTab();

    }

   
}

function SetNewsFilterStatus(status) {
    if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfOnlineNewsStatus') != null)
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfOnlineNewsStatus').value = status;
}

function SetTVFilterStatus(status) {
    if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfTVStatus') != null)
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfTVStatus').value = status;
}

function SetSMFilterStatus(status) {
    if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfSocialMediaStatus') != null)
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfSocialMediaStatus').value = status;
}

function SetTwitterFilterStatus(status) {
    if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfTwitterMediaStatus') != null)
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfTwitterMediaStatus').value = status;
}

function SetRadioFilterStatus(status) {
    if (document.getElementById('ctl00_Content_Data_ucIQpremium_hfRadioStatus') != null)
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfRadioStatus').value = status;
}

function showTVTab() {
    /*$('#divOnlineNewsResultInner').hide();
    $('#divSocialMediaResultInner').hide();
    $('#divTVResultInner').show('slow');
    $('#divOLChartInner').hide();
    $('#divSMChartInner').hide();
    $('#divTVChartInner').show('slow');*/

    $('#divOnlineNewsResultInner').hide();
    $('#divSocialMediaResultInner').hide();
    $('#divTwitterResultInner').hide();
    $('#divRadioResultInner').hide();
    $('#divTVResultInner').show('slow');


    $('#divOLChartInner').removeClass('display-block')
    $('#divOLChartInner').addClass('display-none');

    $('#divSMChartInner').removeClass('display-block')
    $('#divSMChartInner').addClass('display-none');

    $('#divTVChartInner').removeClass('display-none');
    $('#divTVChartInner').addClass('display-block');

    $('#divTwitterChartInner').removeClass('display-block');
    $('#divTwitterChartInner').addClass('display-none');

    $('#divRadioChartInner').removeClass('display-block');
    $('#divRadioChartInner').addClass('display-none');

    /*//Chart Style
    //TV Chart
    $('#divTVChartInner').css("height", "550px");
    $('#divLineChart').style.visibility = 'inherit';

    //Online News Chart
    $('#divNewsChartDiv').css("height", "0px");
    $('#divNewsChart').style.visibility = 'inherit';

    //Social Media Chart
    $('#divSMChartInner').css("height", "0px");
    $('#divSocialMediaChart').style.visibility = 'inherit';*/


    $('#divChartTab div').removeClass('active');
    $('#divGridTab div').removeClass('active');
    $('#tabTV').addClass('active');
    $('#tabChartTV').addClass('active');
}

function showOnlineNewsTab() {
    $('#divTVResultInner').hide(); $('#divSocialMediaResultInner').hide(); $('#divOnlineNewsResultInner').show('slow'); $('#divTwitterResultInner').hide();
    $('#divRadioResultInner').hide();
    //$('#divTVChartInner').style.visibility = 'hidden'; $('#divSMChartInner').style.visibility = 'hidden'; $('#divOLChartInner').style.visibility = 'visible';

    $('#divOLChartInner').addClass('display-block')
    $('#divOLChartInner').removeClass('display-none');

    $('#divSMChartInner').removeClass('display-block')
    $('#divSMChartInner').addClass('display-none');

    $('#divTVChartInner').addClass('display-none');
    $('#divTVChartInner').removeClass('display-block');

    $('#divTwitterChartInner').removeClass('display-block');
    $('#divTwitterChartInner').addClass('display-none');

    $('#divRadioChartInner').removeClass('display-block');
    $('#divRadioChartInner').addClass('display-none');


    $('#divChartTab div').removeClass('active');
    $('#divGridTab div').removeClass('active');
    $('#tabOnlineNews').addClass('active');
    $('#tabChartOL').addClass('active');

    /*//Chart Style
    //TV Chart
    $('#divTVChartInner').css("height", "0px");
    $('#divLineChart').style.visibility = 'inherit';

    //Online News Chart
    $('#divNewsChartDiv').css("height", "550px");
    $('#divNewsChart').style.visibility = 'inherit';

    //Social Media Chart
    $('#divSMChartInner').css("height", "0px");
    $('#divSocialMediaChart').style.visibility = 'inherit';*/
}


function showSocialMediaTab() {
    $('#divTVResultInner').hide(); $('#divOnlineNewsResultInner').hide(); $('#divSocialMediaResultInner').show('slow'); $('#divTwitterResultInner').hide();
    //$('#divTVChartInner').style.visibility = 'hidden'; $('#divOLChartInner').style.visibility = 'hidden'; $('#divSMChartInner').style.visibility = 'visible';
    $('#divRadioResultInner').hide();

    $('#divOLChartInner').removeClass('display-block')
    $('#divOLChartInner').addClass('display-none');

    $('#divSMChartInner').addClass('display-block')
    $('#divSMChartInner').removeClass('display-none');

    $('#divTVChartInner').addClass('display-none');
    $('#divTVChartInner').removeClass('display-block');

    $('#divTwitterChartInner').removeClass('display-block');
    $('#divTwitterChartInner').addClass('display-none');


    $('#divRadioChartInner').removeClass('display-block');
    $('#divRadioChartInner').addClass('display-none');

    /* //Chart Style
    //TV Chart
    $('#divTVChartInner').css("height", "0px");
    $('#divLineChart').style.visibility = 'inherit';

    //Online News Chart
    $('#divNewsChartDiv').css("height", "0px");
    $('#divNewsChart').style.visibility = 'inherit';

    //Social Media Chart
    $('#divSMChartInner').css("height", "550px");
    $('#divSocialMediaChart').style.visibility = 'inherit';*/


    $('#divChartTab div').removeClass('active');
    $('#divGridTab div').removeClass('active');
    $('#tabSocialMedia').addClass('active');
    $('#tabChartSM').addClass('active');
}

function showTwitterTab() {
    $('#divTVResultInner').hide(); $('#divOnlineNewsResultInner').hide(); $('#divSocialMediaResultInner').hide('slow'); $('#divTwitterResultInner').show('slow');
    $('#divRadioResultInner').hide();

    $('#divOLChartInner').removeClass('display-block')
    $('#divOLChartInner').addClass('display-none');

    $('#divSMChartInner').removeClass('display-block');
    $('#divSMChartInner').addClass('display-none')

    $('#divTVChartInner').removeClass('display-block');
    $('#divTVChartInner').addClass('display-none');

    $('#divRadioChartInner').removeClass('display-block');
    $('#divRadioChartInner').addClass('display-none');

    $('#divTwitterChartInner').removeClass('display-none');
    $('#divTwitterChartInner').addClass('display-block');


    $('#divChartTab div').removeClass('active');
    $('#divGridTab div').removeClass('active');
    $('#tabTwitter').addClass('active');
    $('#tabChartTwitter').addClass('active');
}

function showRadioTab() {
    $('#divTVResultInner').hide(); $('#divOnlineNewsResultInner').hide(); $('#divSocialMediaResultInner').hide('slow'); $('#divTwitterResultInner').hide('slow'); $('#divRadioResultInner').show('slow');

    $('#divRadioResultInner').removeClass('display-none');
    $('#divRadioResultInner').addClass('display-block');

    $('#divOLChartInner').removeClass('display-block')
    $('#divOLChartInner').addClass('display-none');

    $('#divSMChartInner').removeClass('display-block');
    $('#divSMChartInner').addClass('display-none')

    $('#divTVChartInner').removeClass('display-block');
    $('#divTVChartInner').addClass('display-none');

    $('#divTwitterChartInner').removeClass('display-block');
    $('#divTwitterChartInner').addClass('display-none');

    $('#divRadioChartInner').addClass('display-block');
    $('#divRadioChartInner').removeClass('display-none');



    $('#divChartTab div').removeClass('active');
    $('#divGridTab div').removeClass('active');
    $('#tabRadio').addClass('active');
    $('#tabChartRadio').addClass('active');
}

function PrintIframe() {
    var IframeUrl = document.getElementById("ctl00_Content_Data_ucIQpremium_iFrameOnlineNewsArticle").src;
    var docprint = window.open(IframeUrl, "_blank");
}

function CancelSaveSearch() {

    $('[id$="_vsSaveSearch"]')[0].style.display = 'none';
    $('#ctl00_Content_Data_ucIQpremium_lblmsg').text('');

    $('[id$="_txtTitle"]')[0].value = '';

    $('[id$="_txtDescription"]')[0].value = '';

    $('[id$="_ddlCategory"]')[0].value = '0';

    //$find('mpSaveSearch').hide();
    closeModal('pnlSaveSearch');


}


function OpenSaveArticlePopup(type) {

    $('[id$="_vlSummerySaveArticle"]')[0].style.display = 'none';
    $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = '';
    $('[id$="_txtArticleTitle"]')[0].value = '';
    $('[id$="_txtADescription"]')[0].value = '';
    $('[id$="_txtKeywords"]')[0].value = '';


    $('[id$="_ddlPCategory"]')[0].value = '0';
    $('[id$="_ddlSubCategory1"]')[0].value = '0';
    $('[id$="_ddlSubCategory2"]')[0].value = '0';
    $('[id$="_ddlSubCategory3"]')[0].value = '0';
    $('[id$="_chkArticlePreferred"]')[0].checked = false;


    if (type == 0) {
        var slider = $find('seArticle');
        slider.set_Value(1);
        CloseNewsIframe();
        $('#spnSaveArticleTitle').html('Article Details');
    }
    else {
        $('#spnSaveArticleTitle').html('Tweet Details');
    }
    ShowModal('pnlSaveArticle');
    //$find('mdlpopupSaveArticle').show();
    return false;
}

function OpenSaveArticlePopup1(Type) {

    var grid;
    if (Type == 'NM')
        grid = $('[id$="_gvOnlineNews"]');
    else
        grid = $('[id$="_gvSocialMedia"]');
    

    if (grid[0]) {
        var elements = grid[0].getElementsByTagName('input');
        var checkcount = 0;
        for (var i = 0; i < elements.length; i++) {
            //            if (elements[i].type == 'checkbox' && elements[i].id.toString().match('chkDelete') != null && elements[i].checked == true) {
            if (elements[i].type == 'checkbox' && elements[i].checked == true) {
                checkcount = checkcount + 1;
            }
        }
        if (checkcount <= 0) {
            alert('Please select atleast one article to save.');
            return false;
        }
    }


    $('[id$="_vlSummerySaveArticle1"]')[0].style.display = 'none';
    $('[id$="_lblSaveArticleErrMsg"]')[0].innerHTML = '';
    

    $('[id$="_ddlArticlePCategory"]')[0].value = '0';
    $('[id$="_ddlArticleSubCategory1"]')[0].value = '0';
    $('[id$="_ddlArticleSubCategory2"]')[0].value = '0';
    $('[id$="_ddlArticleSubCategory3"]')[0].value = '0';

    $('[id$="_hdnSaveArticleType"]')[0].value = Type;
    

    ShowModal('pnlSaveArticle1');
    //$find('mdlpopupSaveArticle').show();
    return false;
}

function CloseNewsIframe() {
    $("#ctl00_Content_Data_ucIQpremium_iFrameOnlineNewsArticle").html('');
    $("#ctl00_Content_Data_ucIQpremium_iFrameOnlineNewsArticle").attr('src', '');
    closeModal('diviFrameOnlineNewsArticle');    
}


function ShowHideDivResult(needtoClose) {

    if ($('#divResult').is(':visible')) {

        $('#divResultShowHideTitle').text('SHOW');
        $('#imgShowHideResult').attr('src', '../images/SHOW.png');

        if (needtoClose) {

            $('#divResult').hide('slow');
        }
        else {

            $('#divResultShowHideTitle').text('HIDE');
            $('#imgShowHideResult').attr('src', '../images/hiden.png');
        }
    }
    else {
        $('#divResult').show('slow');


        $('#divResultShowHideTitle').text('HIDE');
        $('#imgShowHideResult').attr('src', '../images/hiden.png');
    }
}

function ShowHideDivChart(needtoClose) {
    if ($('#divChartResult').is(':visible')) {

        $('#divChartShowHideTitle').text('SHOW');
        $('#imgShowHideChart').attr('src', '../images/SHOW.png');
        if (needtoClose) {
            $('#divChartResult').hide('slow');
        }
        else {
            $('#divChartShowHideTitle').text('HIDE');
            $('#imgShowHideChart').attr('src', '../images/hiden.png');
        }
    }
    else {
        $('#divChartResult').show('slow');
        $('#divChartShowHideTitle').text('HIDE');
        $('#imgShowHideChart').attr('src', '../images/hiden.png');
    }
}



function pageLoad() {
    //last_child();
    if ($('#ctl00_Content_Data_ucIQpremium_chkTV').attr('checked') == 'checked') {
        $('#ctl00_Content_Data_ucIQpremium_divMainTVFilter').show();
    }
    else {
        $('#ctl00_Content_Data_ucIQpremium_divMainTVFilter').hide();
    }

    if ($('#ctl00_Content_Data_ucIQpremium_chkNews').attr('checked') == 'checked') {
        $('#ctl00_Content_Data_ucIQpremium_divMainOnlineNewsFilter').show();
    }
    else {
        $('#ctl00_Content_Data_ucIQpremium_divMainOnlineNewsFilter').hide();
    }

    $(document).ready(function () {
        last_child();

        $('[id$="_pnlSaveArticle"]').on('show', function () {
            $('[id$="_pnlSaveArticle"]').css("top", "10%");
            $('[id$="_pnlSaveArticle"]').css("left", "21%");
            $('[id$="_pnlSaveArticle"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlSaveArticle"]').css("position", "fixed");
                $('[id$="_pnlSaveArticle"]').css("top", "10%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlSaveArticle"]').draggable({ handle: ".popup-hd" });
        });


        $('[id$="_pnlSaveArticle1"]').on('show', function () {
            $('[id$="_pnlSaveArticle1"]').css("top", "25%");
            $('[id$="_pnlSaveArticle1"]').css("left", "27%");
            $('[id$="_pnlSaveArticle1"]').css("width", "43%");
            $('[id$="_pnlSaveArticle1"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlSaveArticle1"]').css("position", "fixed");
                $('[id$="_pnlSaveArticle1"]').css("top", "25%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlSaveArticle1"]').draggable({ handle: ".popup-hd" });
        });

        $('[id$="_pnlSaveSearch"]').on('shown', function () {
            $('[id$="_pnlSaveSearch"]').css("top", "10%");
            $('[id$="_pnlSaveSearch"]').css("left", "21%");
            $('[id$="_pnlSaveSearch"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlSaveSearch"]').css("position", "fixed");
                $('[id$="_pnlSaveSearch"]').css("top", "10%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlSaveSearch"]').draggable({ handle: ".popup-hd" });
        });

        $('[id$="_diviFrameOnlineNewsArticle"]').on('show', function () {
            $('[id$="_diviFrameOnlineNewsArticle"]').css('top', '7%');
            $('[id$="_diviFrameOnlineNewsArticle"]').css("left", "10%");
            $('[id$="_diviFrameOnlineNewsArticle"]').css("height", "85%");
            $('[id$="_diviFrameOnlineNewsArticle"]').css("width", "82%");
            $('[id$="_diviFrameOnlineNewsArticle"]').resizable({ resize: function (e, ui) {
                $('[id$="_diviFrameOnlineNewsArticle"]').css("position", "fixed");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_diviFrameOnlineNewsArticle"]').draggable({ handle: "#diviFrameOnlineNewsArticle" });

        })



        var TotalliOpen = 0;
        $('#ctl00_Content_Data_ucIQpremium_cblAffil input[type=checkbox]').unbind("click").click(function () {
            var isChecked = true;
            $('#ctl00_Content_Data_ucIQpremium_cblAffil input[type=checkbox]').each(function () {
                if (this.checked == false) {
                    isChecked = false;
                }
            });

            if (isChecked == true) {
                $('#chkAffilAll').attr('checked', true);
            }
            else {
                $('#chkAffilAll').attr('checked', false);
            }
        });


        $('#chkAffilAll').unbind("click").click(function () {

            if (this.checked) {
                $('#divAffil input[type=checkbox]').each(function () {

                    this.checked = true;
                });
            }
            else {

                $('#divAffil input[type=checkbox]').each(function () {

                    this.checked = false;
                });
            }

        });



        $('#ctl00_Content_Data_ucIQpremium_chkCategoryAll').unbind("click").click(function () {
            if (this.checked) {
                $('#divCategory input[type=checkbox]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#divCategory input[type=checkbox]').each(function () {
                    this.checked = false;
                });
            }

        });


        $('#divCategory input[type=checkbox]').unbind("click").click(function () {
            var isChecked = true;
            $('#divCategory input[type=checkbox]').each(function () {
                if (this.checked == false) {
                    isChecked = false;
                }
            });

            if (isChecked == true) {
                $('#ctl00_Content_Data_ucIQpremium_chkCategoryAll').attr('checked', true);
            }
            else {
                $('#ctl00_Content_Data_ucIQpremium_chkCategoryAll').attr('checked', false);
            }
        });


        $('#chkRankFIlterSelectAll').unbind("click").click(function () {
            if (this.checked) {
                $('#divRankFilterMaster input[type=checkbox]').each(function () {
                    this.checked = true;
                });
            }
            else {
                $('#divRankFilterMaster input[type=checkbox]').each(function () {
                    this.checked = false;
                });
            }

        });

        function chkTopMarketClick(checkboxID, cblID) {
            // Here cblID is Checkbox List ID    
            if ($('#' + checkboxID).is(':checked')) {
                $('#' + cblID + ' input[type=checkbox]').attr('checked', true);
                MasterSelectallCheckUncheckInner(true, 'chkRankFIlterSelectAll', 'divRankFilterMaster');
            }
            else {

                $('#' + cblID + ' input[type=checkbox]').attr('checked', false);
                MasterSelectallCheckUncheckInner(false, 'chkRankFIlterSelectAll', 'divRankFilterMaster');
            }

        }

        function chkStationClick(checkboxID, cblID) {
            // Here cblID is Checkbox List ID    
            if ($('#' + checkboxID).is(':checked')) {
                $('#' + cblID + ' input[type=checkbox]').attr('checked', true);
                MasterSelectallCheckUncheckInner(true, 'chkAffilAll', 'divAffil');
            }
            else {

                $('#' + cblID + ' input[type=checkbox]').attr('checked', false);
                MasterSelectallCheckUncheckInner(false, 'chkAffilAll', 'divAffil');
            }

        }

        function checkUncheckStationCheckBox(checkBoxID, cblID) {
            // Here cblID is Checkbox List ID

            var isChecked = true;
            $('#' + cblID + ' input[type=checkbox]').each(function () {
                if (this.checked == false) {
                    isChecked = false;
                }
            });

            if (isChecked == true) {
                $('#' + checkBoxID).attr('checked', true);
                MasterSelectallCheckUncheckInner(true, 'chkAffilAll', 'divAffil');
            }
            else {
                $('#' + checkBoxID).attr('checked', false);
                MasterSelectallCheckUncheckInner(false, 'chkAffilAll', 'divAffil');
            }
        }

        function MasterSelectallCheckUncheckInner(checkedStatus, checkBoxID, divFilterID) {

            if (checkedStatus == false) {
                $('#' + checkBoxID).attr('checked', false);
            }
            else {
                var isChecked = true;
                $('#' + divFilterID + ' input[type=checkbox]').each(function () {
                    if (!this.checked) {
                        isChecked = false;
                    }
                });

                if (isChecked) {
                    $('#' + checkBoxID).attr('checked', true);
                }
                else {
                    $('#' + checkBoxID).attr('checked', false);
                }
            }
        }



        function checkUncheckTopMarket(checkBoxID, cblID) {
            // Here cblID is Checkbox List ID

            var isChecked = true;
            $('#' + cblID + ' input[type=checkbox]').each(function () {
                if (this.checked == false) {
                    isChecked = false;
                }
            });

            if (isChecked == true) {
                $('#' + checkBoxID).attr('checked', true);
                MasterSelectallCheckUncheckInner(true, 'chkRankFIlterSelectAll', 'divRankFilterMaster');
            }
            else {
                $('#' + checkBoxID).attr('checked', false);
                MasterSelectallCheckUncheckInner(false, 'chkRankFIlterSelectAll', 'divRankFilterMaster');
            }
        }


        $('#chkNational').unbind("click").click(function () {
            if (this.checked) {
                MasterSelectallCheckUncheckInner(true, 'chkRankFIlterSelectAll', 'divRankFilterMaster');
            }
            else {
                MasterSelectallCheckUncheckInner(false, 'chkRankFIlterSelectAll', 'divRankFilterMaster');
            }
        });
        $('#chkTop10').unbind("click").click(function () {
            chkTopMarketClick('chkTop10', 'divDmaTop10');
        });

        $('#chkTop20').unbind("click").click(function () {


            chkTopMarketClick('chkTop20', 'divDmaTop20');
        });

        $('#chkTop30').unbind("click").click(function () {


            chkTopMarketClick('chkTop30', 'divDmaTop30');
        });

        $('#chkTop40').unbind("click").click(function () {

            chkTopMarketClick('chkTop40', 'divDmaTop40');

        });

        $('#chkTop50').unbind("click").click(function () {


            chkTopMarketClick('chkTop50', 'divDmaTop50');
        });

        $('#chkTop60').unbind("click").click(function () {
            chkTopMarketClick('chkTop60', 'divDmaTop60');
        });

        $('#chkTop80').unbind("click").click(function () {
            chkTopMarketClick('chkTop80', 'divDmaTop80');
        });



        $('#chkTop100').unbind("click").click(function () {
            chkTopMarketClick('chkTop100', 'divDmaTop100');
        });

        $('#chkTop150').unbind("click").click(function () {
            chkTopMarketClick('chkTop150', 'divDmaTop150');
        });

        $('#chkTop210').unbind("click").click(function () {
            chkTopMarketClick('chkTop210', 'divDmaTop210');
        });

        /*===============================*/
        /*   Station Image Click */
        /*==================================*/



        /* ==================================================================== */
        /* For Checking/Unchecking CheckAll Checkbox Based on CheckboxList Selection */
        /* ==================================================================== */
        $('#divDmaTop10 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop10', 'divDmaTop10');
        });

        $('#divDmaTop20 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop20', 'divDmaTop20');
        });

        $('#divDmaTop30 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop30', 'divDmaTop30');
        });

        $('#divDmaTop40 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop40', 'divDmaTop40');
        });

        $('#divDmaTop50 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop50', 'divDmaTop50');
        });

        $('#divDmaTop60 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop60', 'divDmaTop60');
        });

        $('#divDmaTop80 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop80', 'divDmaTop80');
        });

        $('#divDmaTop100 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop100', 'divDmaTop100');
        });

        $('#divDmaTop150 input[type=checkbox]').unbind("click").click(function () {
            checkUncheckTopMarket('chkTop150', 'divDmaTop150');
        });

        $('#divDmaTop210 input[type=checkbox]').unbind("click").click(function () {

            checkUncheckTopMarket('chkTop210', 'divDmaTop210');
        });

        $('#divNewsCategoryInner input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divNewsCategoryInner', 'ctl00_Content_Data_ucIQpremium_chkNewsCategorySelectAll');
        });

        $('#divNewsMainPublicationCategory input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divNewsMainPublicationCategory', 'ctl00_Content_Data_ucIQpremium_chkNewsPublicationCategory');
        });

        $('#divNewsGenre input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divNewsGenre', 'ctl00_Content_Data_ucIQpremium_chkNewsGenreSelectAll');
        });

        $('#divNewsRegion input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divNewsRegion', 'ctl00_Content_Data_ucIQpremium_chkNewsRegionAll');
        });

        $('#divSMTypeInner input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divSMTypeInner', 'ctl00_Content_Data_ucIQpremium_chkSMTypeSelectAll');
        });

        $('#divSMCategoryInner input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divSMCategoryInner', 'ctl00_Content_Data_ucIQpremium_chkSMCategorySelectAll');
        });

        $('#divSMRankInner input[type=checkbox]').unbind("click").click(function () {
            CheckUncheckMasterCheckBoxOnChild(this.id, 'divSMRankInner', 'ctl00_Content_Data_ucIQpremium_chkSMRankSelectAll');
        });


        $('#ctl00_Content_Data_ucIQpremium_chkRegionNational').unbind("click").click(function () {
            if (this.checked) {
                var chkRegionSelectAll = document.getElementById('chlkRegionSelectAll');
                MasterSelectallCheckUncheck(true, chkRegionSelectAll, divRegionFilter);
            }
            else {
                var chkRegionSelectAll = document.getElementById('chlkRegionSelectAll');
                MasterSelectallCheckUncheck(false, chkRegionSelectAll, divRegionFilter);
            }
        });

        $('#divRegionFilter img').unbind("click").click(function () {
            var divCheckBoxID = $(this).nextUntil('div.sublist').last().next()[0].id;
            OnImageExpandCollapseClick(this.id, divCheckBoxID);
        });


        $('#divRegionFilter input[id$="_chkRegion"]').unbind("click").click(function () {
            //var divCheckBox = $(this).nextUntil('div.sublist').last().next()[0];
            var divCheckBox = $(this).last().next()[0];
            var chkRegionSelectAll = document.getElementById('chlkRegionSelectAll');
            RegionMainCheckBoxClick(this, divCheckBox, chkRegionSelectAll, divRegionFilter);
        });


        $('#divRegionFilter input[id$="_chkDma"]').unbind("click").click(function () {
            var divCheckBox = $(this).closest('div[id^="divCheckBox"]')[0];
            //var ChkRegion = $(this).closest('div[id^="divCheckBox"]').prevUntil('input[id$="_chkRegion"]').first().prev()[0];
            var ChkRegion = $(this).closest('div[id^="divCheckBox"]').first().prev()[0]
            var chkRegionSelectAll = document.getElementById('chlkRegionSelectAll');
            chkMainRegion(this, divCheckBox, ChkRegion, chkRegionSelectAll, divRegionFilter);
        });


        $('#divAffil img').unbind("click").unbind("click").click(function () {
            var divCheckBoxID = $(this).nextUntil('div.sublist').last().next()[0].id;
            OnImageExpandCollapseClick(this.id, divCheckBoxID);
        });


        $('#divAffil input[id$="_chkStationSubMaster"]').unbind("click").click(function () {
            var divCheckBox = $(this).nextUntil('div.sublist').last().next()[0];
            var chklAffilAll = document.getElementById("chkAffilAll");
            RegionMainCheckBoxClick(this, divCheckBox, chklAffilAll, divAffil);
        });


        $('#divAffil input[id$="_chkTVStation"]').unbind("click").click(function () {
            var divCheckBox = $(this).closest('div[id^="SdivCheckBox"]')[0];
            var ChkAffil = $(this).closest('div[id^="SdivCheckBox"]').prevUntil('input[id$="_chkStationSubMaster"]').first().prev().prev()[0];
            var chklAffilAll = document.getElementById("chkAffilAll");
            chkMainRegion(this, divCheckBox, ChkAffil, chklAffilAll, divAffil);
        });


        if ($('#ctl00_Content_Data_ucIQpremium_ddlMarket').val() == "Rank") {
            $('#divMarketRankFilter').show('slow');
            $('#divMainRegionFilter').hide('slow');
        }
        else {
            $('#divMarketRankFilter').hide('slow');
            $('#divMainRegionFilter').show('slow');
        }

        //        $('#ctl00_Content_Data_ucIQpremium_txtStartDate').unbind("change").change(function () {
        //            //$find("CalendarExtenderToDate")._selectedDate = new Date(this.value);
        //            $find("CalendarExtenderFromDate").set_selectedDate(new Date(this.value));
        //            //alert($find("CalendarExtenderToDate")._selectedDate);

        //        });

        //        $('#ctl00_Content_Data_ucIQpremium_txtEndDate').unbind("change").change(function () {
        //            $find("CalendarExtenderToDate").set_selectedDate(new Date(this.value));
        //        });


        $('#ctl00_Content_Data_ucIQpremium_ddlMarket').unbind("change").change(function () {
            if (this.value == "Rank") {
                $('#divMarketRankFilter').show('slow');
                $('#divMainRegionFilter').hide('slow');
            }
            else {
                $('#divMarketRankFilter').hide('slow');
                $('#divMainRegionFilter').show('slow');
            }
        });


    });


    //Region Select All Checkbox's Click Event
    $('#chlkRegionSelectAll').unbind("click").click(function () {

        if (this.checked) {
            $('#divMainRegionFilter input[type=checkbox]').each(function () {
                $(this).attr('checked', true);
            });
        }
        else {
            $('#divMainRegionFilter input[type=checkbox]').each(function () {
                $(this).attr('checked', false);
            });

        }
    });

    //Radio Market Select All Checkbox's Click Event
    $('#ctl00_Content_Data_ucIQpremium_chkRadioMarketAll').unbind("click").click(function () {

        if (this.checked) {
            $('#divRadioMarket input[type=checkbox]').each(function () {
                $(this).attr('checked', true);
            });
        }
        else {
            $('#divRadioMarket input[type=checkbox]').each(function () {
                $(this).attr('checked', false);
            });

        }
    });

    $('#divRadioMarket input[type=checkbox]').unbind("click").click(function () {

        var isChecked = true;
        $('#divRadioMarket input[type=checkbox]').each(function () {
            if (this.checked == false) {
                isChecked = false;
            }
        });

        if (isChecked == true) {
            $('#ctl00_Content_Data_ucIQpremium_chkRadioMarketAll').attr('checked', true);
        }
        else {
            $('#ctl00_Content_Data_ucIQpremium_chkRadioMarketAll').attr('checked', false);
        }
    });


}


function OnImageExpandCollapseClick(imageID, ControlToExpandCollapse) {

    //alert($("#" + ControlToExpandCollapse));
    if ($("#" + imageID).attr("src") == '../images/collapse_icon.png') {
        $("#" + ControlToExpandCollapse).hide('slow');
        $("#" + imageID).attr("src", '../images/expand_icon.png');
    }
    else {
        $("#" + ControlToExpandCollapse).show('slow');
        $("#" + imageID).attr("src", '../images/collapse_icon.png');
    }
}

function RegionMainCheckBoxClick(checkboxID, divCheckBox, MasterCheckBoxID, MasterDivCheckBox) {
    var checked = true;
    //alert("checkboxID :" + checkboxID.id);
    //if ($('#' + checkboxID.id).is(':checked')) {
    if (checkboxID.checked) {
        checked = true;
    }
    else {
        checked = false;
    }

    $('#' + divCheckBox.id + ' input[type=checkbox]').each(function () {
        $(this).attr('checked', checked);
    });

    MasterSelectallCheckUncheck(checked, MasterCheckBoxID, MasterDivCheckBox);
}

function chkMainRegion(checkboxID, divCheckBox, MainCheckBoxID, MasterCheckBoxID, Masterdiv) {
    //if ($('#' + checkboxID.id).is(':checked')) {
    if (checkboxID.checked) {
        var isChecked = true;
        $('#' + divCheckBox.id + ' input[type=checkbox]').each(function () {
            if (!this.checked) {
                isChecked = false;
            }
        });

        if (isChecked) {
            //$('#' + MainCheckBoxID.id).attr('checked', true);
            MainCheckBoxID.checked = true;
            MasterSelectallCheckUncheck(true, MasterCheckBoxID, Masterdiv);
        }
        else {
            //$('#' + MainCheckBoxID.id).attr('checked', false);
            MainCheckBoxID.checked = false;
            MasterSelectallCheckUncheck(false, MasterCheckBoxID, Masterdiv);
        }

    }
    else {
        //$('#' + MainCheckBoxID.id).attr('checked', false);
        MainCheckBoxID.checked = false;
        MasterSelectallCheckUncheck(false, MasterCheckBoxID, Masterdiv);
    }
}

function checkUncheckChildOnMasterCheckBox(checkboxId, checkboxContentID) {
    var checked = true;
    if ($('#' + checkboxId).is(':checked')) {
        checked = true;
    }
    else {
        checked = false;
    }

    $('#' + checkboxContentID + ' input[type=checkbox]').each(function () {
        this.checked = checked;
    });
}
function CheckUncheckMasterCheckBoxOnChild(checkboxId, checkboxContentID, MasterCheckBoxID) {

    var checked = true;
    if ($('#' + checkboxId).is(':checked')) {
        checked = true;
    }
    else {
        checked = false;
    }

    /*$('#' + checkboxContentID.id + ' input[type=checkbox]').each(function () {
        
    $(this).attr('checked', checked);
    });*/

    MasterSelectallCheckUncheckNewsRegion(checked, MasterCheckBoxID, checkboxContentID);
}

function MasterSelectallCheckUncheckNewsRegion(checkedStatus, checkBoxID, divFilterID) {
    if (checkedStatus == false) {
        $('#' + checkBoxID).attr('checked', false);
    }
    else {
        var isChecked = true;
        $('#' + divFilterID + ' input[type=checkbox]').each(function () {

            if (!this.checked) {
                isChecked = false;
            }
        });

        if (isChecked) {
            $('#' + checkBoxID).attr('checked', true);
        }
        else {
            $('#' + checkBoxID).attr('checked', false);
        }
    }
}

function MasterSelectallCheckUncheck(checkedStatus, checkBoxID, divFilterID) {

    //alert('checkBoxID.id' + checkBoxID.id);
    if (checkedStatus == false) {
        $('#' + checkBoxID.id).attr('checked', false);
    }
    else {
        var isChecked = true;
        $('#' + divFilterID.id + ' input[type=checkbox]').each(function () {

            if (!this.checked) {
                isChecked = false;
            }
        });

        if (isChecked) {
            $('#' + checkBoxID.id).attr('checked', true);
        }
        else {
            $('#' + checkBoxID.id).attr('checked', false);
        }
    }
}

function ClosePlayer() {
    $("#ctl00_Content_Data_ucIQpremium_IframeRawMediaH_divRawMedia").html('');
    closeModal('diviframe');
}




function SetTimeFilter() {
    if ($('#divTimeFilter').is(':visible')) {

        $('#divTimeFilter').hide('slow');
        $('#imgShowTimeFilter').attr('src', '../images/show.png');
        //TotalliOpen = TotalliOpen - 1;
    }
    else {

        $('#imgShowTimeFilter').attr('src', '../images/hiden.png');
        $('#divTimeFilter').show('slow');
        // TotalliOpen = TotalliOpen + 1;

        $('#divTimeFilter div[id!=divInterval]').show('slow');

        if ($('#ctl00_Content_Data_ucIQpremium_rbDuration2').attr('checked') != 'checked') {
            HideDiv('divInterval');

            $('#ctl00_Content_Data_ucIQpremium_ddlDuration').attr('disabled', false);

        }
        else {
            ShowDiv('divInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlDuration').attr('disabled', true);
        }

    }
    //ShowHideApplyButton();
}

function SetProgramFilter() {
    if ($('#divProgramTitle').is(':visible')) {
        $('#divProgramTitle').hide('slow');
        $('#imgShowProgramFilter').attr('src', '../images/show.png');

    }
    else {
        $('#divProgramTitle').show('slow');
        $('#imgShowProgramFilter').attr('src', '../images/hiden.png');
    }
    //ShowHideApplyButton();
    $('#divProgramTitle div').show('slow');
}

function SetMarketFilter() {
    if ($('#divMarketFilter').is(':visible')) {
        $('#divMarketFilter').hide('slow');
        //$('#divMarketFilter div').hide('slow');
        $('#imgShowMarketFilter').attr('src', '../images/show.png');
        $('#imgExpandRankFilter').attr('src', '../images/collapse_icon.png');

        $('#imgRegionSelectAll').attr('src', '../images/collapse_icon.png');

    }
    else {
        $('#divMarketFilter').show('slow');
        $('#divMarketFilter').children('div').children('div').show('slow');
        $('#divMarketFilterStatus').show('slow');
        $('#divFilterTitle').show('slow');
        $('#imgShowMarketFilter').attr('src', '../images/hiden.png');

    }

    //ShowHideApplyButton();
    if ($('#ctl00_Content_Data_ucIQpremium_ddlMarket option:selected').text() == "Rank") {
        $('#divMarketRankFilter').show('hide');
        $('#divMainRegionFilter').hide('slow');

    }
    else {
        $('#divMarketRankFilter').hide();
        $('#divMainRegionFilter').show('slow');

    }
}

function SetCategoryFilter() {
    if ($('#divCategoryFilter').is(':visible')) {
        $('#divCategoryFilter').hide('slow');
        $('#divCategoryFilter div').hide('slow');
        $('#imgShowCategoryFilter').attr('src', '../images/show.png');
        $('#CategoryExpandimg').attr('src', '../images/collapse_icon.png');

    }
    else {
        $('#divCategoryFilter').show('slow');
        $('#divCategoryFilter div').show('slow');
        $('#imgShowCategoryFilter').attr('src', '../images/hiden.png');
    }
    //ShowHideApplyButton();
    $('#affilExpandImg').attr("src") == '../images/collapse_icon.png';

}

function SetStationFilter() {
    if ($('#divStationFilter').is(':visible')) {
        $('#divStationFilter').hide('slow');
        //$('#divStationFilter div').hide('slow');
        $('#imgShowStationFilter').attr('src', '../images/show.png');
        $('#affilExpandImg').attr('src', '../images/collapse_icon.png');
    }
    else {
        $('#divStationFilter').show('slow');
        //$('#divStationFilter div').show('slow');
        $('#divStationFilter').children('div').show('slow');
        $('#imgShowStationFilter').attr('src', '../images/hiden.png');
        //HideDiv('ctl00_Content_Data_ucIQpremium_rptTVStationSubMaster_ctl01_divCheckBox');
        /*HideDiv('ctl00_Content_Data_ucIQpremium_cblAI');
        HideDiv('ctl00_Content_Data_ucIQpremium_cblFJ');
        HideDiv('ctl00_Content_Data_ucIQpremium_cblKO');
        HideDiv('ctl00_Content_Data_ucIQpremium_cblPT');
        HideDiv('ctl00_Content_Data_ucIQpremium_cblUZ');*/

    }

    $('#ctl00_Content_Data_ucIQpremium_CategoryExpandimg').attr("src") == '../images/collapse_icon.png';
}

function DisplayInterval(display) {
    if (display) {
        ShowDiv('divInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlDuration').attr('disabled', true);
    }
    else {
        HideDiv('divInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlDuration').attr('disabled', false);
    }
}

function ShowDiv(divID) {
    $("#" + divID).show('slow');
}


function HideDiv(divID) {
    $("#" + divID).hide('slow');
}


//function FC_ExportReady(objRtn) {

//}
function FC_Zoomed(DOMId, startIndex, endIndex, startItemLabel, endItemLabel) {


    if (startItemLabel != endItemLabel) {



        $('html, body').animate({
            scrollTop: $("#divResult").offset().top - 600
        }, 1000);
        if (DOMId == "divLineChart") {
            __doPostBack('ctl00_Content_Data_ucIQpremium_upGrid', "zoomin," + DOMId + "," + startItemLabel + "," + endItemLabel)
        }
        else if (DOMId == "divNewsChart") {
            __doPostBack('ctl00_Content_Data_ucIQpremium_upOnlineNews', "zoomin," + DOMId + "," + startItemLabel + "," + endItemLabel)
        }
        else if (DOMId == "divSocialMediaChart") {
            __doPostBack('ctl00_Content_Data_ucIQpremium_upSocialMedia', "zoomin," + DOMId + "," + startItemLabel + "," + endItemLabel)
        }
        else if (DOMId == "divTwitterChart2") {
            __doPostBack('ctl00_Content_Data_ucIQpremium_upTwitter', "zoomin," + DOMId + "," + startItemLabel + "," + endItemLabel)
        }
    }
}

function FC_ZoomedOut(eventObject, argumentsObject) {

    $('html, body').animate({
        scrollTop: $("#divResult").offset().top - 300
    }, 1000);
    if (eventObject == "divLineChart") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upGrid', "zoomout," + eventObject)
    }
    else if (eventObject == "divNewsChart") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upOnlineNews', "zoomout," + eventObject)
    }
    else if (eventObject == "divSocialMediaChart") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upSocialMedia', "zoomout," + eventObject)
    }
    else if (eventObject == "divTwitterChart2") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upTwitter', "zoomout," + eventObject)
    }
}

function FC_ResetZoomChart(eventObject, argumentsObject) {
    $('html, body').animate({
        scrollTop: $("#divResult").offset().top - 300
    }, 1000);
    if (eventObject == "divLineChart") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upGrid', "reset," + eventObject)
    }
    else if (eventObject == "divNewsChart") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upOnlineNews', "reset," + eventObject)
    }
    else if (eventObject == "divSocialMediaChart") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upSocialMedia', "reset," + eventObject)
    }
    else if (eventObject == "divTwitterChart2") {
        __doPostBack('ctl00_Content_Data_ucIQpremium_upTwitter', "reset," + eventObject)
    }
}

function FC_Rendered(DOMId) {
    if (DOMId == "divLineChart") {
        //BeginTVChartExportProcess();
        $('#ctl00_Content_Data_ucIQpremium_btncsv').show();
    }
    else if (DOMId == "divNewsChart") {
        //BeginOnlineNewsChartExportProcess();
        $('#ctl00_Content_Data_ucIQpremium_btnCSVNews').show();
    }
    else if (DOMId == "divSocialMediaChart") {
        $('#ctl00_Content_Data_ucIQpremium_btnCSVSM').show();
    }
    else if (DOMId == "divTwitterChart2") {
        $('#ctl00_Content_Data_ucIQpremium_btnCSVTwitter').show();
    }

}

function BeginTVChartExportProcess() {

    /*var exp1 = document.getElementById('fcBatchExporter');
    if (exp1 != null) {
    exp1.parentNode.removeChild(exp1);}*/

    var expTVDiv = document.getElementById('fcexpDiv');
    while (expTVDiv.hasChildNodes()) {
        expTVDiv.removeChild(expTVDiv.lastChild);
    }


    //Initialize Batch Exporter with DOM Id as fcBatchExporter
    var myExportComponent = new FusionChartsExportObject("fcBatchExporter", "../fusionchart/swf/FCExporter.swf");
    //Add the charts to queue. The charts are referred to by their DOM Id.
    myExportComponent.sourceCharts = ['divLineChart'];
    //------ Export Component Attributes ------//
    myExportComponent.componentAttributes.bgColor = 'FFFFFF';
    myExportComponent.componentAttributes.btnsavetitle = 'Save the chart'
    myExportComponent.componentAttributes.btndisabledtitle = 'Waiting for export';

    /**/
    myExportComponent.componentAttributes.width = "200";
    myExportComponent.componentAttributes.height = "50";

    //myExportComponent.componentAttributes.fontFace = 'avenir_65medium'; 
    myExportComponent.componentAttributes.fontColor = '#3290B4';
    myExportComponent.componentAttributes.fontSize = '14';

    myExportComponent.componentAttributes.btnWidth = '150';
    myExportComponent.componentAttributes.btnHeight = '30';
    myExportComponent.componentAttributes.btnColor = 'FFFFFF';
    myExportComponent.componentAttributes.btnBorderColor = 'D9D9D9';

    //Render the exporter SWF in our DIV fcexpDiv
    myExportComponent.Render("fcexpDiv");
    myExportComponent.BeginExport();

    $('#fcexpDiv').delay(2000).show('slow');
    //$('#fcexpDiv').show();
}

function BeginOnlineNewsChartExportProcess() {

    /* var exp1 = document.getElementById('fcBatchExporterNews');
    if (exp1 != null) {       
    exp1.parentNode.removeChild(exp1);
    }*/
    /* var expNewsDiv = document.getElementById('ctl00_Content_Data_ucIQpremium_divfcexNews');
    while (expNewsDiv.hasChildNodes()) {
    expNewsDiv.removeChild(expNewsDiv.lastChild);
    }*/



    var myExportComponentNews = new FusionChartsExportObject("fcBatchExporterNews", "../fusionchart/swf/FCExporter.swf");




    //Add the charts to queue. The charts are referred to by their DOM Id.
    myExportComponentNews.sourceCharts = ['divNewsChart'];
    myExportComponentNews.componentAttributes.bgColor = 'FFFFFF';
    myExportComponentNews.componentAttributes.btnBorderColor = 'D9D9D9';
    myExportComponentNews.componentAttributes.btnsavetitle = 'Save the chart'
    myExportComponentNews.componentAttributes.btndisabledtitle = 'Waiting for export';

    myExportComponentNews.componentAttributes.width = "200";
    myExportComponentNews.componentAttributes.height = "50";

    //myExportComponent.componentAttributes.fontFace = 'avenir_65medium'; 
    myExportComponentNews.componentAttributes.fontColor = '#3290B4';
    myExportComponentNews.componentAttributes.fontSize = '14';

    myExportComponentNews.componentAttributes.btnWidth = '150';
    myExportComponentNews.componentAttributes.btnHeight = '30';
    myExportComponentNews.componentAttributes.btnColor = 'FFFFFF';
    myExportComponentNews.componentAttributes.btnBorderColor = 'D9D9D9';
    myExportComponentNews.Render("divfcexNews");

    myExportComponentNews.BeginExport();
    $('#divfcexNews').show('slow');



}
function FC_Exported(objRtn) {
    if (objRtn.statusCode == "1") { alert('hi'); }
}


/*====================================================================================*/
/*===========================Online News Javascript Starts============================*/
/*====================================================================================*/

function SetNewsTimeFilter() {

    if ($('#divNewsTimeFilter').is(':visible')) {

        $('#divNewsTimeFilter').hide('slow');
        $('#imgShowNewsTime').attr('src', '/images/show.png');

        //TotalliOpen = TotalliOpen - 1;
    }
    else {
        $('#imgShowNewsTime').attr('src', '/images/hiden.png');
        $('#divNewsTimeFilter').show('slow');
        // TotalliOpen = TotalliOpen + 1;

        $('#divNewsTimeFilter div[id!=divNewsInterval]').show('slow');

        if ($('#ctl00_Content_Data_ucIQpremium_rbNewsInterval').attr('checked') != 'checked') {
            HideDiv('divNewsInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlNewsDuration').attr('disabled', false);

        }
        else {
            ShowDiv('divNewsInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlNewsDuration').attr('disabled', true);
        }

    }
    //ShowHideApplyButton();
}

function DisplayNewsInterval(display) {
    if (display) {
        ShowDiv('divNewsInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlNewsDuration').attr('disabled', true);
    }
    else {
        HideDiv('divNewsInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlNewsDuration').attr('disabled', false);
    }
}

function DisplayRadioInterval(display) {
    if (display) {
        ShowDiv('divRadioInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlRadioDuration').attr('disabled', true);
    }
    else {
        HideDiv('divRadioInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlRadioDuration').attr('disabled', false);
    }
}

function SetPublicationFilter() {
    if ($('#divNewsPublication').is(':visible')) {
        $('#divNewsPublication').hide('slow');
        $('#imgShowNewsPublication').attr('src', '/images/show.png');

    }
    else {
        $('#divNewsPublication').show('slow');
        //$('#divNewsPublication div').show('slow');

        $('#imgShowNewsPublication').attr('src', '/images/hiden.png');
    }
    //  ShowHideApplyButton();

}


function SetNewsCategoryFilter() {
    if ($('#divNewsCategory').is(':visible')) {
        $('#divNewsCategory').hide('slow');
        $('#imgShowNewsCategory').attr('src', '/images/show.png');

    }
    else {
        $('#divNewsCategory').show('slow');
        $('#divNewsCategory div').show('slow');
        $('#imgExpandNewsCategory').attr('src', '../images/collapse_icon.png');
        $('#imgShowNewsCategory').attr('src', '/images/hiden.png');
    }
}


function SetNewsPublicationCategoryFilter() {
    if ($('#divShowNewsPublicationCategory').is(':visible')) {
        $('#divShowNewsPublicationCategory').hide('slow');
        $('#imgShowNewsPublicationCategory').attr('src', '/images/show.png');

    }
    else {
        $('#divShowNewsPublicationCategory').show('slow');
        $('#divShowNewsPublicationCategory div').show('slow');
        $('#imgExpandNewsPublicationCategory').attr('src', '../images/collapse_icon.png');
        $('#imgShowNewsPublicationCategory').attr('src', '/images/hiden.png');
    }
}


function SetNewsGenreFilter() {
    if ($('#divNewsGenreFilter').is(':visible')) {
        $('#divNewsGenreFilter').hide('slow');
        $('#imgShowNewsGenreFilter').attr('src', '../images/show.png');

    }
    else {
        $('#divNewsGenreFilter').show('slow');
        $('#divNewsGenreFilter div').show('slow');
        $('#imgNewsGenreExpandimg').attr('src', '../images/collapse_icon.png');
        $('#imgShowNewsGenreFilter').attr('src', '../images/hiden.png');
    }
}

function SetNewsRegionFilter() {
    if ($('#divNewsRegionFilter').is(':visible')) {
        $('#divNewsRegionFilter').hide('slow');
        $('#imgShowNewsRegion').attr('src', '/images/show.png');

    }
    else {
        $('#divNewsRegionFilter').show('slow');
        $('#divNewsRegionFilter div').show('slow');
        $('#NewsRegionExpandImg').attr('src', '../images/collapse_icon.png');
        $('#imgShowNewsRegion').attr('src', '/images/hiden.png');
    }
}

function SetSMTimeFilter() {
    if ($('#divSMTimeFilter').is(':visible')) {

        $('#divSMTimeFilter').hide('slow');
        $('#imgShowSMTime').attr('src', '/images/show.png');

        //TotalliOpen = TotalliOpen - 1;
    }
    else {
        $('#imgShowSMTime').attr('src', '/images/hiden.png');
        $('#divSMTimeFilter').show('slow');
        // TotalliOpen = TotalliOpen + 1;

        $('#divSMTimeFilter div[id!=divSMInterval]').show('slow');

        if ($('#ctl00_Content_Data_ucIQpremium_rbSMInterval').attr('checked') != 'checked') {
            HideDiv('divSMInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlSMDuration').attr('disabled', false);

        }
        else {
            ShowDiv('divSMInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlSMDuration').attr('disabled', true);
        }

    }
}

function SetRadioTimeFilter() {

    if ($('#divRadioTimeFilter').is(':visible')) {

        $('#divRadioTimeFilter').hide('slow');
        $('#imgShowRadioTime').attr('src', '/images/show.png');

        //TotalliOpen = TotalliOpen - 1;
    }
    else {
        $('#imgShowRadioTime').attr('src', '/images/hiden.png');
        $('#divRadioTimeFilter').show('slow');
        // TotalliOpen = TotalliOpen + 1;

        $('#divRadioTimeFilter div[id!=divRadioInterval]').show('slow');

        if ($('#ctl00_Content_Data_ucIQpremium_rbRadioInterval').attr('checked') != 'checked') {
            HideDiv('divRadioInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlRadioDuration').attr('disabled', false);

        }
        else {
            ShowDiv('divRadioInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlRadioDuration').attr('disabled', true);
        }

    }
    //ShowHideApplyButton();
}


function SetRadioMarketFilter() {
    if ($('#divRadioMarketFilter').is(':visible')) {
        $('#divRadioMarketFilter').hide('slow');
        $('#divRadioMarketFilter div').hide('slow');
        $('#imgShowRadioDMA').attr('src', '../images/show.png');
        $('#imgRadioMarketAll').attr('src', '../images/collapse_icon.png');

    }
    else {
        $('#divRadioMarketFilter').show('slow');
        $('#divRadioMarketFilter div').show('slow');
        $('#imgShowRadioDMA').attr('src', '../images/hiden.png');
    }

    //$('#affilExpandImg').attr("src") == '../images/collapse_icon.png';

}

function DisplaySMInterval(display) {
    if (display) {
        ShowDiv('divSMInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlSMDuration').attr('disabled', true);
    }
    else {
        HideDiv('divSMInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlSMDuration').attr('disabled', false);
    }
}


function SetSMSourceFilter() {
    if ($('#divSMSource').is(':visible')) {
        $('#divSMSource').hide('slow');
        $('#imgShowSMSource').attr('src', '/images/show.png');

    }
    else {
        $('#divSMSource').show('slow');
        $('#imgShowSMSource').attr('src', '/images/hiden.png');
    }
}


function SetSMCategoryFilter() {
    if ($('#divSMCategory').is(':visible')) {
        $('#divSMCategory').hide('slow');
        $('#imgShowSMCategory').attr('src', '../images/show.png');

    }
    else {
        $('#divSMCategory').show('slow');
        $('#divSMCategory div').show('slow');
        $('#imgExpandSMCategory').attr('src', '../images/collapse_icon.png');
        $('#imgShowSMCategory').attr('src', '../images/hiden.png');
    }
}


function SetSMSourceTypeFilter() {
    if ($('#divSMType').is(':visible')) {
        $('#divSMType').hide('slow');
        $('#imgShowSMType').attr('src', '../images/show.png');

    }
    else {
        $('#divSMType').show('slow');
        $('#divSMType div').show('slow');
        $('#imgExpandSMType').attr('src', '../images/collapse_icon.png');
        $('#imgShowSMType').attr('src', '../images/hiden.png');
    }
}

function SetSMSourceRankFilter() {
    if ($('#divSMRank').is(':visible')) {
        $('#divSMRank').hide('slow');
        $('#imgShowSMRank').attr('src', '/images/show.png');
    }
    else {
        $('#divSMRank').show('slow');
        $('#divSMRank div').show('slow');
        $('#imgExpandSMRank').attr('src', '../images/collapse_icon.png');
        $('#imgShowSMRank').attr('src', '/images/hiden.png');
    }
}

function DisplayTwitterInterval(display) {
    if (display) {
        ShowDiv('divTwitterInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlTwitterDuration').attr('disabled', true);
    }
    else {
        HideDiv('divTwitterInterval');
        $('#ctl00_Content_Data_ucIQpremium_ddlTwitterDuration').attr('disabled', false);
    }
}

function SetTwitterTimeFilter() {
    if ($('#divTwitterTimeFilter').is(':visible')) {

        $('#divTwitterTimeFilter').hide('slow');
        $('#imgShowTwitterTime').attr('src', '/images/show.png');

        //TotalliOpen = TotalliOpen - 1;
    }
    else {
        $('#imgShowTwitterTime').attr('src', '/images/hiden.png');
        $('#divTwitterTimeFilter').show('slow');
        // TotalliOpen = TotalliOpen + 1;

        $('#divTwitterTimeFilter div[id!=divTwitterInterval]').show('slow');

        if ($('#ctl00_Content_Data_ucIQpremium_rbTwitterInterval').attr('checked') != 'checked') {
            HideDiv('divTwitterInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlTwitterDuration').attr('disabled', false);

        }
        else {
            ShowDiv('divTwitterInterval');
            $('#ctl00_Content_Data_ucIQpremium_ddlTwitterDuration').attr('disabled', true);
        }

    }
}

function SetTwitterSourceFilter() {
    if ($('#divTwitterSource').is(':visible')) {
        $('#divTwitterSource').hide('slow');
        $('#imgShowTwitterSource').attr('src', '/images/show.png');

    }
    else {
        $('#divTwitterSource').show('slow');
        $('#imgShowTwitterSource').attr('src', '/images/hiden.png');
    }
}

function SetTwitterCountFilter() {
    if ($('#divTwitterCount').is(':visible')) {
        $('#divTwitterCount').hide('slow');
        $('#imgShowTwitterCount').attr('src', '/images/show.png');

    }
    else {
        $('#divTwitterCount').show('slow');
        $('#imgShowTwitterCount').attr('src', '/images/hiden.png');
    }
}

function SetTwitterScoreFilter() {
    if ($('#divTwitterScore').is(':visible')) {
        $('#divTwitterScore').hide('slow');
        $('#imgShowTwitterScore').attr('src', '/images/show.png');

    }
    else {
        $('#divTwitterScore').show('slow');
        $('#imgShowTwitterScore').attr('src', '/images/hiden.png');
    }
}

function ShowHideFilterOnCheckbox(FilterName, checkbox) {
    /*var checkboxchecked = 5;
    $('#divHeaderFilter input[type=checkbox]').each(function () {

    if (this.checked) {
    checkboxchecked = checkboxchecked + 1;
    }
    else {
    checkboxchecked = checkboxchecked - 1;
    }
    });


    if (checkboxchecked <= 0) {
    checkbox.checked = true;
    alert('Atleast one Filter must be selected');

    return;
    }*/

    if (FilterName == "TV") {
        if (checkbox.checked == true) {
            if (document.getElementById('ctl00_Content_Data_ucIQpremium_hdnTVData').value != 1) {
                __doPostBack('ctl00_Content_Data_ucIQpremium_upMainSearch', "Filter,upTVFilter");
            }
            ShowDiv('ctl00_Content_Data_ucIQpremium_divMainTVFilter');
        }
        else {
            HideDiv('ctl00_Content_Data_ucIQpremium_divMainTVFilter');
        }
    }

    if (FilterName == "RADIO") {
        if (checkbox.checked == true) {
            if (document.getElementById('ctl00_Content_Data_ucIQpremium_hdnRadioData').value != 1) {
                __doPostBack('ctl00_Content_Data_ucIQpremium_upMainSearch', "Filter,upRadioFilter");
            }
            ShowDiv('ctl00_Content_Data_ucIQpremium_divMainRadioFilter');
        }
        else {
            HideDiv('ctl00_Content_Data_ucIQpremium_divMainRadioFilter');
        }
    }

    if (FilterName == "ONLINENEWS") {
        if (checkbox.checked == true) {
            if (document.getElementById('ctl00_Content_Data_ucIQpremium_hdnONLINENEWSData').value != 1) {
                __doPostBack('ctl00_Content_Data_ucIQpremium_upMainSearch', "Filter,upNewsFilter");
            }
            ShowDiv('ctl00_Content_Data_ucIQpremium_divMainOnlineNewsFilter');
        }
        else {
            HideDiv('ctl00_Content_Data_ucIQpremium_divMainOnlineNewsFilter');
        }
    }

    if (FilterName == "SOCIALMEDIA") {
        if (checkbox.checked == true) {
            if (document.getElementById('ctl00_Content_Data_ucIQpremium_hdnSocialMediaData').value != 1) {
                __doPostBack('ctl00_Content_Data_ucIQpremium_upMainSearch', "Filter,upSMFilter");
            }
            ShowDiv('ctl00_Content_Data_ucIQpremium_divMainSMFilter');
        }
        else {
            HideDiv('ctl00_Content_Data_ucIQpremium_divMainSMFilter');
        }
    }

    if (FilterName == "TWITTER") {
        if (checkbox.checked == true) {
            ShowDiv('ctl00_Content_Data_ucIQpremium_divMainTwitterFilter');
        }
        else {
            HideDiv('ctl00_Content_Data_ucIQpremium_divMainTwitterFilter');
        }
    }
}

//ChartID
// 0 ==> TV
// 1 ==> OnlineNews
function GetCsvData(chartID) {
    if (chartID == 0) {
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfsvcData').value = FusionCharts('divLineChart').getDataAsCSV();
        //alert(FusionCharts('divLineChart').getDataAsCSV());
    }
    else if (chartID == 1) {
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfcsvNewsData').value = FusionCharts('divNewsChart').getDataAsCSV();
        //alert(FusionCharts('divLineChart').getDataAsCSV());
    }
    else if (chartID == 2) {
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfcsvSMData').value = FusionCharts('divSocialMediaChart').getDataAsCSV();
        //alert(FusionCharts('divLineChart').getDataAsCSV());
    }
    else if (chartID == 3) {
        document.getElementById('ctl00_Content_Data_ucIQpremium_hfcsvTwitterData').value = FusionCharts('divTwitterChart2').getDataAsCSV();
        //alert(FusionCharts('divLineChart').getDataAsCSV());
    }
}



function showHidefilterul(ulid) {
    if ($('#' + ulid).is(':visible')) {
        $('#' + ulid).hide('slow');
    }
    else {
        $('#' + ulid).show('slow');
    }
}

function RemoveExportChartComponent() {

    var expTVDiv = document.getElementById('fcexpDiv');
    while (expTVDiv.hasChildNodes()) {
        expTVDiv.removeChild(expTVDiv.lastChild);
    }

    var expNewsDiv = document.getElementById('divfcexNews');

    while (expNewsDiv.hasChildNodes()) {
        expNewsDiv.removeChild(expNewsDiv.lastChild);
    }

    if ($('#ctl00_Content_Data_ucIQpremium_chkNews').attr('checked') != 'checked' && $('#ctl00_Content_Data_ucIQpremium_chkTV').attr('checked') != 'checked' && $('#ctl00_Content_Data_ucIQpremium_chkSocialMedia').attr('checked') != 'checked' && $('#ctl00_Content_Data_ucIQpremium_chkTwitter').attr('checked') != 'checked' && $('#ctl00_Content_Data_ucIQpremium_chkRadio').attr('checked') != 'checked') {

        alert('Atleast one Filter must be selected');
        return false;
    }

    return true;

}

function ExportChart(ChartType) {
    if (ChartType == 'TV') {
        BeginTVChartExportProcess();
    }
    else if (ChartType == 'News') {
        BeginOnlineNewsChartExportProcess();
    }
}



function UpdateSubCategory1(ddl_id,type) {


    if (type == 0) {
        var PCatId = $('[id$="_ddlPCategory"]')[0].id;
        var SubCat1Id = $('[id$="_ddlSubCategory1"]')[0].id;
        var SubCat2Id = $('[id$="_ddlSubCategory2"]')[0].id;
        var SubCat3Id = $('[id$="_ddlSubCategory3"]')[0].id;
        $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "";
    }
    else {
        var PCatId = $('[id$="_ddlArticlePCategory"]')[0].id;
        var SubCat1Id = $('[id$="_ddlArticleSubCategory1"]')[0].id;
        var SubCat2Id = $('[id$="_ddlArticleSubCategory2"]')[0].id;
        var SubCat3Id = $('[id$="_ddlArticleSubCategory3"]')[0].id;
        $('[id$="_lblSaveArticleErrMsg"]')[0].innerHTML = "";
    }


    var PCatSelectedValue = $("#" + PCatId + "").val();
    var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
    var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
    var Cat3SelectedValue = $("#" + SubCat3Id + "").val();
    

    if (ddl_id == PCatId) {

        $("#" + SubCat1Id + " option").removeAttr("disabled");
        $("#" + SubCat2Id + " option").removeAttr("disabled");
        $("#" + SubCat3Id + " option").removeAttr("disabled");

        if (PCatSelectedValue != 0) {
            $("#" + SubCat1Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat2Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");


            if (PCatSelectedValue == Cat1SelectedValue) {
                $("#" + SubCat1Id + "").val(0);
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }
            else if (PCatSelectedValue == Cat2SelectedValue) {
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }
            else if (PCatSelectedValue == Cat3SelectedValue) {
                $("#" + SubCat3Id + "").val(0);
            }


            if ($("#" + SubCat1Id + "").val() != 0) {
                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
            }

            if ($("#" + SubCat2Id + "").val() != 0) {
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
            }

        }
        else {
            $("#" + SubCat1Id + "").val(0);
            $("#" + SubCat2Id + "").val(0);
            $("#" + SubCat3Id + "").val(0);
        }
    }
    else if (ddl_id == SubCat1Id) {
        if (PCatSelectedValue != 0) {
            $("#" + SubCat2Id + " option").removeAttr("disabled");
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            $("#" + SubCat2Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");

            if (Cat1SelectedValue != 0) {

                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat1SelectedValue == Cat2SelectedValue) {
                    $("#" + SubCat2Id + "").val(0);
                    $("#" + SubCat3Id + "").val(0);
                }
                else if (Cat1SelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val(0);
                }

                if ($("#" + SubCat1Id + "").val() != 0) {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != 0) {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }
            }
            else {
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }

        }
        else {
            $("#" + SubCat1Id + "").val(0);
            $("#" + SubCat2Id + "").val(0);
            $("#" + SubCat3Id + "").val(0);
            if (type == 0) {
                $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }else
                $('[id$="_lblSaveArticleErrMsg"]')[0].innerHTML = "Please first select all preceding categories.";
        }
    }
    else if (ddl_id == SubCat2Id) {
        if (PCatSelectedValue != 0 && Cat1SelectedValue != 0) {
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

            if (Cat2SelectedValue != 0) {

                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat2SelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val(0);
                }

                if ($("#" + SubCat1Id + "").val() != 0) {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != 0) {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }
            }
            else {
                $("#" + SubCat3Id + "").val(0);
            }
        }
        else {
            $("#" + SubCat2Id + "").val(0);
            $("#" + SubCat3Id + "").val(0);
            if (type == 0) {
                $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            } else
                $('[id$="_lblSaveArticleErrMsg"]')[0].innerHTML = "Please first select all preceding categories.";

        }
    }
    else if (ddl_id == SubCat3Id) {
        if (PCatSelectedValue == 0 || Cat1SelectedValue == 0 || Cat2SelectedValue == 0) {
            $("#" + SubCat3Id + "").val(0);
            if (type == 0) {
                $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            } else
                $('[id$="_lblSaveArticleErrMsg"]')[0].innerHTML = "Please first select all preceding categories.";

        }
    }
}


function CheckAllCheckBox(maindivID) {
    $('#' + maindivID.id + ' input[type=checkbox]').each(function () {
        $(this).attr('checked', true);
    });
}

function ShowSearchSaveAs() {
    //$find('mpSaveSearch').show();
    ShowModal('pnlSaveSearch');
    $('#spnSaveSearchHeader').text('Save Search');
    $('#ctl00_Content_Data_ucIQpremium_btnSubmit').show();
    $('#ctl00_Content_Data_ucIQpremium_btnUpdate').hide();
    $('#ctl00_Content_Data_ucIQpremium_lblNote').hide();
    if ($('[id$="_chkIsIQAgent"]')[0] != undefined && $('[id$="_chkIsIQAgent"]')[0] != null) {
        $('[id$="_chkIsIQAgent"]')[0].checked = false;
    }
    $('[id$="_chkIsDefaultSearch"]')[0].checked = false;
}


function SetFilterStatus(FilterID, FilterImageID, Status) {
    $('#' + FilterID + '').html(Status);
    if (Status == 'ON') {
        if (FilterImageID != null) {
            $('#' + FilterImageID + '').attr('src', '../images/filter-Selected.png');
            $('#' + FilterID + '').css('color', 'red');
        }
        else
            $('#' + FilterID + '').css('color', '#C1D62E')
    }
    else {
        $('#' + FilterID + '').css('color', 'black');
        if (FilterImageID != null)
            $('#' + FilterImageID + '').attr('src', '../images/filter.png');
    }
}

function last_child() {
    if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
        $('*:last-child').addClass('last-of-type');
    }
}

function sliderChange(textbox, checkbox) {
    if (document.getElementById('ctl00_Content_Data_ucIQpremium_' + textbox).value == 6) {
        $('[id$="_' + checkbox + '"]').attr('checked', true);
    } else {
        $('[id$="_' + checkbox + '"]').attr('checked', false);
    }
}

function PrefferredChecked(checkbox, slider) {
    var slider = $find(slider);
    if ($('#ctl00_Content_Data_ucIQpremium_' + checkbox).attr('checked') == 'checked') {
        slider.set_Value(6);
    }
    else {
        slider.set_Value(1);
    }
}

function ShowModal(pnlid) {
    $('[id$="_' + pnlid + '"]').modal({
        backdrop: 'static',
        keyboard: true
    });
}
function closeModal(pnlid) {
    $('[id$="_' + pnlid + '"]').modal('hide');
}

function SetDivIFrameDiv() {
    $('[id$="_diviframe"]').css("width", "900px");
    //$('[id$="_diviframe"]').css('width', '');

}
function RegisterCCCallback(type) {

    /*var divmarleft2 = parseInt((screen.width / 2) - (920 / 2)).toString() + 'px';
    $('[id$="_divClipPlayer"]').css({ "left": "" + divmarleft2 + "" });*/
    //var divmarleft2 = parseInt((screen.width / 2) - (920 / 2)).toString() + 'px';

    var divmarleft2 = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';

    $('[id$="_diviframe"]').css({ "left": divmarleft2, "position": "fixed" });
}