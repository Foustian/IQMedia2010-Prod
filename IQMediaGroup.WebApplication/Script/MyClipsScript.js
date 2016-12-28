function ShowHideFilter(imgForExpand, Filter) {

    if ($('#div' + Filter + '').is(':visible')) {
        $('#div' + Filter + '').hide('slow');
        $('#imgShow' + Filter + '').attr('src', '../images/show.png');
        $('#' + imgForExpand).attr('src', '../images/collapse_icon.png');

    }
    else {
        $('#div' + Filter + '').show('slow');
        $('#imgShow' + Filter + '').attr('src', '../images/hiden.png');
        $('#div' + Filter + ' div').show('slow');
    }
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



function ValidateEmailGrid(gridID) {
    var grid = $('[id$="_' + gridID + '"]');

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
            alert('Please select atleast one record to Email.');
            return false;
        }
        else {
            if (gridID == 'grvClip')
                SetEmailPopupTitle('0');
            else if (gridID == 'dlTweet')
                SetEmailPopupTitle('3');
            else
                SetEmailPopupTitle('1');

            $('[id$="_hdnEmailType"]').val('Search')

            $('[id$="_txtFriendsEmail"]').val('');
            $('[id$="_txtSubject"]').val('');
            $('[id$="_txtMessage"]').val('');

            ShowModal('pnlMailPanel');
        }
    }
    return false;
}

function ValidateGrid(grid) {
    var grid = $('[id$="_' + grid + '"]');
    if (grid[0]) {
        var elements = grid[0].getElementsByTagName('input');
        var checkcount = 0;
        for (var i = 0; i < elements.length; i++) {
            if (elements[i].type == 'checkbox' && elements[i].checked == true) {
                checkcount = checkcount + 1;
            }
        }
        if (checkcount > 0) {
            return confirm('Are you sure you want to delete these record(s) ? ');
        }
        else {
            alert('Please select atleast one record to delete.');
            return false;
        }
    }
    return false;
}

function SetSearchParam() {
    var isAll = false;
    $("#divSearch input:checkbox").each(function () {

        if ($(this).is(":checked")) {

            if ($(this).val() == "all") {
                isAll = true;
            }
        }
    });
    if (isAll) {
        $("#divSearch input:checkbox").each(function () {
            if ($(this).val() != "all") {

                $(this).attr("checked", "checked");
                $(this).attr("disabled", "disabled");
            }
        });
    }
    else {
        $("#divSearch input:checkbox").each(function () {
            if ($(this).val() != "all") {
                $(this).removeAttr("disabled");
            }
        });
    }
}




function ClosePlayer() {
    //    $('#divIFrameMain').delay(800).queue(function (hideplayer) {
    $('[id$="_divRawMedia"]').html('');
    //$find('mpeClipPlayer').hide();
    closeModal('divClipPlayer');
    //hideplayer(); 
    //});
}

function removeSourceFromiFrame(iframeid) {
    $('[id$="_' + iframeid + '"]').html('');
    $('[id$="_' + iframeid + '"]').attr('src', '');
}


function ShowHideFilterOnCheckbox(checkbox) {
    var checkboxchecked = 0;
    $('#divHeaderFilter input[type=checkbox]').each(function () {
        if (this.checked)
            checkboxchecked = checkboxchecked + 1;
    });


    if (checkboxchecked <= 0) {
        checkbox.checked = true;
        alert('Atleast one filter must be selected');

        return;
    }
}

/*blah = false;

$("#tabs").tabs({
select: function (event, ui) {
if (blah) {
console.log("recursive");
return;
}
blah = true;
event.preventDefault();
$('#tabs').tabs('select', ui.index);
document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = ui.index;
blah = false;
}
}); */


function ChangeTab(tabID, tabMainID, Filter) {
    $('#' + tabMainID + ' div').removeClass('active');
    $('#' + tabID).addClass('active');
    GetDataOntabChange(Filter, tabMainID);
}


function GetDataOntabChange(Filter) {

    if ($('[id$="_hf' + Filter + 'Status"]')[0].value != 1) {

        __doPostBack($('[id$="_up' + Filter + 'Grid"]')[0].id, "Tab");

    }
    else {
        //var index = $('#tabs a[href="#ctl00_Content_Data_UCMyIQControl_div' + Filter + 'Result"]').parent().index();

        if (Filter == 'TV') {
            document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 0;
        }
        else if (Filter == 'News') {
            document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 1;
        }
        else if (Filter == 'SocialMedia') {
            document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 2;
        }
        else if (Filter == 'PrintMedia') {
            document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 3;
        }
        else if (Filter == 'Tweet') {
            document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 4;
        }
    }

    if (Filter == 'TV') {
        showTVTab();
    }
    else if (Filter == 'News') {
        showOnlineNewsTab();
    }
    else if (Filter == 'SocialMedia') {
        showSocialMediaTab();
    }
    else if (Filter == 'PrintMedia') {
        showPrintMediaTab();
    }
    else if (Filter == 'Tweet') {
        showTweetTab();
    }

}

function showTVTab() {

    document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 0;
    $('#divTweetResultInner').hide();
    $('#divNewsResultInner').hide(); $('#divSocialMediaResultInner').hide(); $('#divPrintMediaResultInner').hide(); $('#divTVResultInner').show();

    $('#divGridTab div').removeClass('active');
    $('#ctl00_Content_Data_UCMyIQControl_tabTV').addClass('active');
}

function showOnlineNewsTab() {

    document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 1;
    $('#divTweetResultInner').hide(); $('#divTVResultInner').hide(); $('#divNewsResultInner').show(); $('#divPrintMediaResultInner').hide(); $('#divSocialMediaResultInner').hide();
    $('#divGridTab div').removeClass('active');
    $('#ctl00_Content_Data_UCMyIQControl_tabNews').addClass('active');
}

function showSocialMediaTab() {

    document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 2;
    $('#divTweetResultInner').hide(); $('#divTVResultInner').hide(); $('#divNewsResultInner').hide(); $('#divPrintMediaResultInner').hide(); $('#divSocialMediaResultInner').show();
    $('#divGridTab div').removeClass('active');
    $('#ctl00_Content_Data_UCMyIQControl_tabSocialMedia').addClass('active');
}

function showPrintMediaTab() {

    document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 3;
    $('#divTweetResultInner').hide(); $('#divTVResultInner').hide(); $('#divNewsResultInner').hide(); $('#divSocialMediaResultInner').hide(); $('#divPrintMediaResultInner').show();
    $('#divGridTab div').removeClass('active');
    $('#ctl00_Content_Data_UCMyIQControl_tabPrintMedia').addClass('active');
}

function showTweetTab() {

    document.getElementById("ctl00_Content_Data_UCMyIQControl_hfCurrentTabIndex").value = 4;
    $('#divTVResultInner').hide(); $('#divNewsResultInner').hide(); $('#divSocialMediaResultInner').hide(); $('#divPrintMediaResultInner').hide(); $('#divTweetResultInner').show();
    $('#divGridTab div').removeClass('active');
    $('#ctl00_Content_Data_UCMyIQControl_tabTweet').addClass('active');
    RemoveBorderBottom();
}

function CloseNewsIframe() {
    $('[id$="_iFrameOnlineNewsArticle"]')[0].innerHTML = '';
    $('[id$="_iFrameOnlineNewsArticle"]')[0].src = '';
}

function PrintIframe() {
    var IframeUrl = $('[id$="_iFrameOnlineNewsArticle"]')[0].src;
    var docprint = window.open(IframeUrl, "_blank");
}

function UpdateSubCategory(ddl_id, Type) {
    var PCatId, SubCat1Id, SubCat2Id, SubCat3Id;

    if (Type == 0) {
        PCatId = $('[id$="_ddlPCategory"]')[0].id;
        SubCat1Id = $('[id$="_ddlSubCategory1"]')[0].id;
        SubCat2Id = $('[id$="_ddlSubCategory2"]')[0].id;
        SubCat3Id = $('[id$="_ddlSubCategory3"]')[0].id;
    }
    else {
        var PCatId = $('[id$="_ddlPArticleCategory"]')[0].id;
        var SubCat1Id = $('[id$="_ddlArticleSubCategory1"]')[0].id;
        var SubCat2Id = $('[id$="_ddlArticleSubCategory2"]')[0].id;
        var SubCat3Id = $('[id$="_ddlArticleSubCategory3"]')[0].id;
    }

    var PCatSelectedValue = $("#" + PCatId + "").val();
    var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
    var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
    var Cat3SelectedValue = $("#" + SubCat3Id + "").val();
    if (Type == 0) {
        $('[id$="_lblClipMsg"]')[0].innerHTML = "";
    }
    else {
        $('[id$="_lblArticlepMsg"]')[0].innerHTML = "";
    }

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
            if (Type == 0) {
                $('[id$="_lblClipMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }
            else {
                $('[id$="_lblArticlepMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }
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
            if (Type == 0) {
                $('[id$="_lblClipMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }
            else {
                $('[id$="_lblArticlepMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }

        }
    }
    else if (ddl_id == SubCat3Id) {
        if (PCatSelectedValue == 0 || Cat1SelectedValue == 0 || Cat2SelectedValue == 0) {
            $("#" + SubCat3Id + "").val(0);
            if (Type == 0) {
                $('[id$="_lblClipMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }
            else {
                $('[id$="_lblArticlepMsg"]')[0].innerHTML = "Please first select all preceding categories.";
            }

        }
    }
}

function UpdateSubCategory2(ddl_id, Type) {

    var PCatId, SubCat1Id, SubCat2Id, SubCat3Id;
    if (Type == 0) {
        PCatId = $('[id$="_ddlPCategory"]')[0].id;
        SubCat1Id = $('[id$="_ddlSubCategory1"]')[0].id;
        SubCat2Id = $('[id$="_ddlSubCategory2"]')[0].id;
        SubCat3Id = $('[id$="_ddlSubCategory3"]')[0].id;
    }
    else {
        PCatId = $('[id$="_ddlPArticleCategory"]')[0].id;
        SubCat1Id = $('[id$="_ddlArticleSubCategory1"]')[0].id;
        SubCat2Id = $('[id$="_ddlArticleSubCategory2"]')[0].id;
        SubCat3Id = $('[id$="_ddlArticleSubCategory3"]')[0].id;
    }

    var PCatSelectedValue = $("#" + PCatId + "").val();
    var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
    var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
    var Cat3SelectedValue = $("#" + SubCat3Id + "").val();

    if (ddl_id == PCatId) {
        if (PCatSelectedValue != 0) {
            $("#" + SubCat1Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat2Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
        }
    }
    else if (ddl_id == SubCat1Id) {
        if (Cat1SelectedValue != 0) {
            $("#" + SubCat2Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
        }
    }
    else if (ddl_id == SubCat2Id) {
        if (Cat2SelectedValue != 0) {
            $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
            $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
        }
    }
}

function MyIQDocReady() {
    $(document).ready(function () {
        last_child();

        $('[id$="_pnlMailPanel"]').on('show', function () {
            $('[id$="_pnlMailPanel"]').css("top", "10%");
            $('[id$="_pnlMailPanel"]').css("left", "21%");
            $('[id$="_pnlMailPanel"]').css("height", "74%");
            $('[id$="_pnlMailPanel"]').css("width", "57%");
            $('[id$="_pnlMailPanel"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlMailPanel"]').css("position", "fixed");
                $('[id$="_pnlMailPanel"]').css("top", "10%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlMailPanel"]').draggable({ handle: ".popup-hd" });
        })

        $('[id$="_pnlEditArticle"]').on('show', function () {
            $('[id$="_pnlEditArticle"]').css("top", "10%");
            $('[id$="_pnlEditArticle"]').css("left", "21%");
            $('[id$="_pnlEditArticle"]').css("height", "74%");
            $('[id$="_pnlEditArticle"]').css("width", "57%");
            $('[id$="_pnlEditArticle"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlEditArticle"]').css("position", "fixed");
                $('[id$="_pnlEditArticle"]').css("top", "10%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlEditArticle"]').draggable({ handle: ".popup-hd" });
        })

        $('[id$="_pnlCustomCategory"]').on('show', function () {
            $('[id$="_pnlCustomCategory"]').css("top", "10%");
            $('[id$="_pnlCustomCategory"]').css("left", "21%");
            $('[id$="_pnlCustomCategory"]').css("height", "74%");
            $('[id$="_pnlCustomCategory"]').css("width", "57%");
            $('[id$="_pnlCustomCategory"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlCustomCategory"]').css("position", "fixed");
                $('[id$="_pnlCustomCategory"]').css("top", "10%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlCustomCategory"]').draggable({ handle: ".popup-hd" });
        })

        $('[id$="_pnlClipPanel"]').on('show', function () {
            $('[id$="_pnlClipPanel"]').css("top", "10%");
            $('[id$="_pnlClipPanel"]').css("left", "21%");

            $('[id$="_pnlClipPanel"]').css("height", "74%");
            $('[id$="_pnlClipPanel"]').css("width", "57%");

            $('[id$="_pnlClipPanel"]').resizable({ resize: function (e, ui) {
                $('[id$="_pnlClipPanel"]').css("position", "fixed");
                $('[id$="_pnlClipPanel"]').css("top", "10%");
            }
            });
            $('.ui-icon').attr("title", 'Drag to resize');
            $('[id$="_pnlClipPanel"]').draggable({ handle: ".popup-hd" });
        })


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



        $('[id$="_ddlReportType"]').unbind("change").change(function () {
            if (this.value == "Daily Report") {
                $('#ReportFilters').show();
            }
            else {
                $('#ReportFilters').hide();
            }
        });

        /*var mpe = $find('mdlpopupEmail');
        mpe.add_shown(mpe_Shown);
        mpe.add_hidden(mpe_Hidden);*/
    });

}

function last_child() {
    if ($.browser.msie && parseInt($.browser.version, 10) <= 8) {
        $('*:last-of-type').addClass('last-of-type');
    }
}

function SetEmailPopupTitle(tabvalue) {
    if (tabvalue == '0') {
        $('#emailTitleSpan').html('Share This Video');
    }
    else if (tabvalue == '1') {
        $('#emailTitleSpan').html('Share This Article');
    }
    else if (tabvalue == '2') {
        $('#emailTitleSpan').html('Share This Report');
    }
    else if (tabvalue == '3') {
        $('#emailTitleSpan').html('Share This Tweet(s)');
    }

}

//child = null;
//function CheckForIOS(ClipID,BaseUrl,IOSAppUrl) {
//    child = window.open('iqmedia://clipid=' + ClipID + '&baseurl=' + BaseUrl + '', 'IQMedia IOS Player');
//    alert(child);
//    child.opener.focus();
//    setTimeout(function () {
//        try {
//            var foo = child.location;
//            alert(child.location)
//        } catch (err) {
//            alert('return');
//            return;
//        }
//        // Still loading. Code here. 
//        var ConfirmResult = confirm('If your device does not have the IQMedia app installed, then click on OK to install. If it is installed Click on Cancel and launch the iQMedia app and try again.')
//        if (ConfirmResult) {
//            child.focus();
//            child.close();
//            window.location = 'itms-services://?action=download-manifest&amp;url=http://qa.iqmediacorp.com/IOSApp/IQMediaCorp.plist';
//        }

//        //child.opener.blur();
//        //child.focus();

//    }, 300);
//} 


childObj = null;
function CheckForIOS(ClipUrl, IOSAppUrl) {

    alert(ClipUrl);
    var newWin = window.open(ClipUrl);
    // see if we get a window object
    alert(newWin);
    childObj = window.open(ClipUrl, 'IQMedia IOS Player');
    alert(childObj);
    childObj.opener.focus();
    setTimeout(function () {
        try {
            var foo = childObj.location;
            alert(childObj.location);
        } catch (err) {
            alert('return');
            return;
        }
        // Still loading. Code here. 
        var ConfirmResult = confirm('If your device does not have the IQMedia app installed, then click on OK to install. If it is installed Click on Cancel and launch the iQMedia app and try again.')
        if (ConfirmResult) {
            childObj.focus();
            childObj.close();
            window.open('itms-services://?action=download-manifest&amp;url=http://qa.iqmediacorp.com/IOSApp/IQMediaCorp.plist', 'IQMedia IOS Player');
        }

        //childObj.opener.blur();
        //childObj.focus();

    }, 300);
}



function OpenReportEmailPopup() {
    SetEmailPopupTitle('2');

    $('[id$="_txtFriendsEmail"]').val('');
    $('[id$="_txtSubject"]').val('');
    $('[id$="_txtMessage"]').val('');
    $('[id$="_hdnEmailType"]').val('Report')

    ShowModal('pnlMailPanel');
    return false;
}

/*function mpe_Shown(sender, e) {

}*/

function ValidateDataList(datalist) {
    var maindatalist = $('#' + datalist);
    if (maindatalist[0]) {
        var elements = maindatalist[0].getElementsByTagName('a');
        for (var i = 0; i < elements.length; i++) {

            if ($('#' + elements[i].id).attr("href") == undefined) {
                $('#' + elements[i].id).removeAttr("href");
            }
        }

        RemoveBorderBottom();
    }
}

function RemoveBorderBottom() {
    $("div[id=datalistInner]:last").css("border-bottom", "0px solid black");
}

function RegisterCCCallback(type) {

    /*var divmarleft2 = parseInt((screen.width / 2) - (920 / 2)).toString() + 'px';
    $('[id$="_divClipPlayer"]').css({ "left": "" + divmarleft2 + "" });*/
    //var divmarleft2 = parseInt((screen.width / 2) - (920 / 2)).toString() + 'px';

    var divmarleft2 = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';
    
    $('[id$="_divClipPlayer"]').css({ "left":  divmarleft2  ,"position": "fixed"});

    //alert("screen.width :" + screen.width);
    //alert("window.outerWidth :" + window.outerWidth);
    //alert("$(window).width() :" + $(window).width());   // returns height of browser viewpor
    //alert("$(document).width() :" + $(document).width()); // returns height of HTML document


    $('#divShowCaption').click(function () {
        if ($('[id$="_DivCaption"]').is(':visible')) {
            $('[id$="_DivCaption"]').hide(500);

            $('[id$="_divClipPlayer"]').animate({
                width: '585px'
            }, 1000, function () {
            });

            $('#divIFrameMain').animate({
                width: '565px'

            }, 1000, function () {

                //$('[id$="_divClipPlayer"]').css('width', '43%');
                $('#imgCCDirection').attr('src', '../../images/left_arrow_cc.gif');

                var divmarleft = parseInt(($(window).width() / 2) - (600 / 2)).toString() + 'px';
                
                // alert(width);
                $('[id$="_divClipPlayer"]').animate({
                    left: divmarleft,
                    position : "fixed"                    
                                        
                });
                //document.getElementById("ctl00_Content_Data_UCMyIQControl_divClipPlayer").style.left = divmarleft;

            });


        }
        else {
            // $('#IFrameRawMedia1_DivCaption').show(1000, ResizeIframe('700px'));
            $('[id$="_divClipPlayer"]').animate({
                width: '900px'
            }, 1000, function () {

            });
            $('#divIFrameMain').animate({
                width: '885px'
            }, 1000, function () {
                $('#imgCCDirection').attr('src', '../../images/right_arrow_cc.gif');

                divmarleft = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';

                // alert(width);
                $('[id$="_divClipPlayer"]').animate({
                    left: divmarleft,
                    position: "fixed" 
                });
                //document.getElementById("ctl00_Content_Data_UCMyIQControl_divClipPlayer").style.left = divmarleft;
                $('[id$="_DivCaption"]').show();
            });
            //$('#ctl00_Content_Data_UCMyIQControl_DivCaption').show(2000);
        }
    });
}