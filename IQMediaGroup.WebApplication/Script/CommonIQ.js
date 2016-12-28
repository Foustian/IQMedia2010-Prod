function ShowHideDiv(Div) {
    if ($('#' + Div + '').is(':visible')) {
        $('#' + Div + '').hide('slow');
    }
    else {
        $('#' + Div + '').show('slow');
    }
}


function ShowModal(pnlid) {
    $('div.modal-backdrop').remove();
//    $('[id$="_' + pnlid + '"]').modal({
//        backdrop: 'static',
//        keyboard: true,
//    });
    $('[id$="_' + pnlid + '"]').css({ "background-color": "White", "border-color": "rgb(111, 111, 111)", "border-width": "10px", "border-style": "solid", "display": "block", "opacity": "100" });
    $('[id$="_' + pnlid + '"]').modal({
        backdrop: 'static',
        keyboard: true
    });
    //$('[id$="_' + pnlid + '"]').modal('show');
    
}
function closeModal(pnlid) {
    $('[id$="_' + pnlid + '"]').css({ "display": "none" });
    $('[id$="_' + pnlid + '"]').modal('hide');
    $('div.modal-backdrop').remove();
}

function CheckUncheckAll(CheckListID, CheckAllID) {
    //alert('CheckUncheckAll')    
    var listBox = document.getElementById(CheckListID);
    var inputItems = listBox.getElementsByTagName("input");
    var Check = document.getElementById(CheckAllID).checked;
    for (index = 0; index < inputItems.length; index++) {
        if (inputItems[index].type == 'checkbox') {
            inputItems[index].checked = Check;
        }
    }
}

function setCheckbox(chklistid, CheckAllID) {
    //alert('setCheckbox')
    var chklist = document.getElementById(chklistid);
    var options = chklist.getElementsByTagName("input");
    var labels = chklist.getElementsByTagName("label");
    var allChecked = 1;

    for (var i = 0; i < options.length; i++) {
        if (options[i].checked == false) {
            allChecked = 0;
            break;
        }

    }

    if (allChecked)
        document.getElementById(CheckAllID).checked = true;
    else
        document.getElementById(CheckAllID).checked = false;

}

function ShowHideDivResult(DivID, needtoClose) {

    if ($('#' + DivID + '').is(':visible')) {

        $('#' + DivID + 'ShowHideTitle').text('SHOW');
        $('#img' + DivID + 'ShowHide').attr('src', '../images/SHOW.png');

        if (needtoClose) {

            $('#' + DivID + '').hide('slow');
        }
        else {

            $('#' + DivID + 'ShowHideTitle').text('HIDE');
            $('#img' + DivID + 'ShowHide').attr('src', '../images/hiden.png');
        }
    }
    else {
        $('#' + DivID + '').show('slow');


        $('#' + DivID + 'ShowHideTitle').text('HIDE');
        $('#img' + DivID + 'ShowHide').attr('src', '../images/hiden.png');
    }
}

function PrefferredChecked(checkbox, slider) {
    var slider = $find(slider);
    if ($('[id$="_' + checkbox + '"]').attr('checked') == 'checked') {
        slider.set_Value(6);
    }
    else {
        slider.set_Value(1);
    }
}

function sliderChange(textbox, checkbox) {
    if ($('[id$="_' + textbox + '"]')[0].value == 6) {
        $('[id$="_' + checkbox + '"]').attr('checked', true);
    } else {
        $('[id$="_' + checkbox + '"]').attr('checked', false);
    }
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

function UpdateSubCategory1(ddl_id) {

    var PCatId = $('[id$="_ddlPCategory"]')[0].id;
    var SubCat1Id = $('[id$="_ddlSubCategory1"]')[0].id;
    var SubCat2Id = $('[id$="_ddlSubCategory2"]')[0].id;
    var SubCat3Id = $('[id$="_ddlSubCategory3"]')[0].id;

    var PCatSelectedValue = $("#" + PCatId + "").val();
    var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
    var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
    var Cat3SelectedValue = $("#" + SubCat3Id + "").val();
    $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "";

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
            $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "Please first select all preceding categories.";
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
            $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "Please first select all preceding categories.";

        }
    }
    else if (ddl_id == SubCat3Id) {
        if (PCatSelectedValue == 0 || Cat1SelectedValue == 0 || Cat2SelectedValue == 0) {
            $("#" + SubCat3Id + "").val(0);
            $('[id$="_lblSaveArticleMsg"]')[0].innerHTML = "Please first select all preceding categories.";

        }
    }
}