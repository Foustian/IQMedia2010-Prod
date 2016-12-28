function ValidateGrid(gridend) {
    grid = $('[id$="' + gridend + '"]')[0];
    
    if (grid) {
        var elements = grid.getElementsByTagName('input');
        var checkcount = 0;

        for (var i = 0; i < elements.length; i++) {
            if (elements[i].type == 'checkbox' && elements[i].id.toString().match('ChkDelete') != null && elements[i].checked == true) {
                checkcount = checkcount + 1;
            }
        }

        if (checkcount > 0) {
            return confirm('Are you sure to delete record(s) ? ');
        }

        else {

            alert('Please Select atleast one records to delete');
            return false;
        }
    }

    return false;
}

function ValidateNotificationGrid() {
    var grid = document.getElementById('ctl00_Content_Data_UCIQAgentControl_gvIQNotification');
    if (grid) {
        var elements = grid.getElementsByTagName('input');
        var checkcount = 0;
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].type == 'checkbox' && elements[i].id.toString().match('chkDelete') != null && elements[i].checked == true) {
                checkcount = checkcount + 1;
            }
        }
        if (checkcount > 0) {
            return confirm('Are you sure to delete record(s) ? ');
        }
        else {
            alert('please select atleast one record to delete.');
            return false;
        }
    }
    return false;
}

function expandcollapse(panelid, expcolimg) {

    var panel = document.getElementById(panelid);
    if (panel) {

        if (panel.style.display == 'block') {
            $(panel).hide('slow');
            expcolimg.src = '../images/expand_icon.png';
        }
        else {
            $(panel).show('slow');
            panel.style.display = 'block';
            expcolimg.src = '../images/collapse_icon.png';
        }
    }
}

function ChangeTab(tabID, tabMainID, Filter, isGrid) {
    $('#' + tabMainID + ' div').removeClass('active');
    $('#' + tabID).addClass('active');
    GetDataOnTab(Filter, isGrid);
}

//Filter
//0 == TV
//1 == News
//2 == Social Media
//By IsGrid we can change tab accordingly
function GetDataOnTab(Filter, isGrid) {
    document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfcurrentTab').value = Filter;
    if (Filter == 0) {
        if (document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfTVStatus').value != 1) {
            $("#divTVResultInner").html('');
            __doPostBack('ctl00_Content_Data_UCIQAgentControl_upTVGrid', "Tab");
        }

        displayTVTab();
    }
    else if (Filter == 1) {
        if (document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfOnlineNewsStatus').value != 1) {
            $("#divOnlineNewsResultInner").html('');
            __doPostBack('ctl00_Content_Data_UCIQAgentControl_upOnlineNews', "Tab");
        }
        displayOnlineNewsTab();

    }
    else if (Filter == 2) {
        if (document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfSocialMediaStatus').value != 1) {
            $("#divSocialMediaResultInner").html('');
            __doPostBack('ctl00_Content_Data_UCIQAgentControl_upSocialMedia', "Tab");
        }
        displaySocialMediaTab();

    }
    else if (Filter == 3) {
        if (document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfTwitterMediaStatus').value != 1) {
            $("#divTwitterResultInner").html('');
            __doPostBack('ctl00_Content_Data_UCIQAgentControl_upTwitter', "Tab");
        }
        displayTwitterTab();

    }
}

function displayTVTab() {
    $('#divOnlineNewsResultInner').hide();
    $('#divSocialMediaResultInner').hide();
    $('#divTwitterResultInner').hide();
    
    $('#divTVResultInner').removeClass('display-none');
    $('#divTVResultInner').show('slow');
    
    //$('#divTVResultInner').addClass('display-block');
    
    $('#divGridTab div').removeClass('active');
    $('#tabTV').addClass('active');
}

function displayOnlineNewsTab() {
    $('#divTVResultInner').hide(); $('#divSocialMediaResultInner').hide(); $('#divTwitterResultInner').hide();

    $('#divOnlineNewsResultInner').removeClass('display-none');
    $('#divOnlineNewsResultInner').show('slow'); 

    $('#divGridTab div').removeClass('active');
    $('#tabOnlineNews').addClass('active');
}


function displaySocialMediaTab() {
    $('#divTVResultInner').hide(); $('#divOnlineNewsResultInner').hide(); $('#divTwitterResultInner').hide();

    $('#divSocialMediaResultInner').removeClass('display-none');
    $('#divSocialMediaResultInner').show('slow'); 
    
    $('#divGridTab div').removeClass('active');
    $('#tabSocialMedia').addClass('active');
}

function displayTwitterTab() {
    $('#divTVResultInner').hide(); $('#divOnlineNewsResultInner').hide(); $('#divSocialMediaResultInner').hide();

    $('#divTwitterResultInner').removeClass('display-none');
    $('#divTwitterResultInner').show('slow');

    $('#divGridTab div').removeClass('active');
    $('#tabTwitter').addClass('active');
}

function SetNewsFilterStatus(status) {
    document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfOnlineNewsStatus').value = status;
}

function SetTVFilterStatus(status) {
    document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfTVStatus').value = status;
}

function SetSMFilterStatus(status) {
    document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfSocialMediaStatus').value = status;
}

function SetTwitterFilterStatus(status) {
    document.getElementById('ctl00_Content_Data_UCIQAgentControl_hfTwitterMediaStatus').value = status;
}

function PrintIframe() {   
    var IframeUrl = document.getElementById("ctl00_Content_Data_UCIQAgentControl_iFrameOnlineNewsArticle").src;
    var docprint = window.open(IframeUrl, "_blank");
}

function CloseNewsIframe() {
    $("#ctl00_Content_Data_UCIQAgentControl_iFrameOnlineNewsArticle").html('');
    $("#ctl00_Content_Data_UCIQAgentControl_iFrameOnlineNewsArticle").attr('src', '');
    closeModal('diviFrameOnlineNewsArticle');
    
}

function pageLoad() {
    $(document).ready(function () {
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
        });
    });
}


function ClosePlayer() {
    $("#ctl00_Content_Data_UCIQAgentControl_IframeRawMediaH_divRawMedia").html('');
    closeModal('diviframe');
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
        //CloseNewsIframe();
        $('#spnSaveArticleTitle').html('Article Details');
    }
    else {
        $('#spnSaveArticleTitle').html('Tweet Details');
    }
    ShowModal('pnlSaveArticle');
    //$find('mdlpopupSaveArticle').show();
    return false;

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

function ValidateDataList(datalist) {
    var maindatalist = $('#' + datalist);
    if (maindatalist[0]) {
        var elements = maindatalist[0].getElementsByTagName('a');
        for (var i = 0; i < elements.length; i++) {

            if ($('#' + elements[i].id).attr("href") == undefined) {
                $('#' + elements[i].id).removeAttr("href");
            }
        }
 
    }
}