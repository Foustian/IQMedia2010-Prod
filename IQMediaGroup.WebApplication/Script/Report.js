function ReportDocReady() {
    $('[id$="_pnlMailPanel"]').on('show', function () {
        $('[id$="_pnlMailPanel"]').css("top", "8%");
        $('[id$="_pnlMailPanel"]').css("left", "21%");
        $('[id$="_pnlMailPanel"]').css("height", "auto");
        $('[id$="_pnlMailPanel"]').css("width", "57%");
        $('[id$="_pnlMailPanel"]').resizable({ resize: function (e, ui) {
            $('[id$="_pnlMailPanel"]').css("position", "fixed");
            $('[id$="_pnlMailPanel"]').css("top", "10%");
        }
        });
        $('.ui-icon').attr("title", 'Drag to resize');
        $('[id$="_pnlMailPanel"]').draggable({ handle: ".popup-hd" });
    })

    $('[id$="_diviFrameArticle"]').on('show', function () {
        $('[id$="_diviFrameArticle"]').css('top', '7%');
        $('[id$="_diviFrameArticle"]').css("left", "10%");
        $('[id$="_diviFrameArticle"]').css("height", "85%");
        $('[id$="_diviFrameArticle"]').css("width", "82%");
        $('[id$="_diviFrameArticle"]').resizable({ resize: function (e, ui) {
            $('[id$="_diviFrameArticle"]').css("position", "fixed");
        }
        });
        $('.ui-icon').attr("title", 'Drag to resize');
        $('[id$="_diviFrameArticle"]').draggable({ handle: "#diviFrameArticle" });
    });

    $('[id$="_ddlReportType"]').unbind("change").change(function () {
        if (this.value == "0") {
            $('#ReportFilters').hide();
        }
        else {
            $('#ReportFilters').show();

        }
    });

    $('[id$="_drpReportTypes"]').unbind("change").change(function () {
        if (this.value == "0") {
            $('#liReportDateFilter').hide();
        }
        else {
            var arrval = this.value.split(',');
            if (arrval[1] == "DailyReport") {
                DailyDatePicker();
                
            }
            else if (arrval[1] == "WeeklyReport") {
                WeeklyDatePicker();
            }
            $('#liReportDateFilter').show();
            $('[id$="_txtReportDate"]').datepicker('setDate', new Date()); 
        }
    });

    if ($('[id$="_drpReportTypes"]').val() != undefined && $('[id$="_drpReportTypes"]').val() != "0") {
        var reportval = $('[id$="_drpReportTypes"]').val().split(',');
        if (reportval[1] == "DailyReport") {
            DailyDatePicker();
        }
        else if (reportval[1] == "WeeklyReport") {
            WeeklyDatePicker();
        }
    }
}

function WeeklyDatePicker() {
    
    $('[id$="_txtReportDate"]').datepicker("destroy");
    $('[id$="_txtReportDate"]').datepicker({
        beforeShowDay: function (date) {
            var day = date.getDay();
            return [(day == 1), ''];
        },
        defaultDate: new Date(),
        changeMonth: true,
        changeYear: true
    })
}

function DailyDatePicker() {
    $('[id$="_txtReportDate"]').datepicker("destroy");
    $('[id$="_txtReportDate"]').datepicker({ defaultDate: new Date(), changeMonth: true, changeYear: true });
}

function OpenReportEmailPopupReport() {

    $('[id$="_txtFriendsEmail"]').val('');
    $('[id$="_txtSubject"]').val('');
    $('[id$="_txtMessage"]').val('');

    ShowModal('pnlMailPanel');
    return false;

}

function ShowArticle(url, ID, Type) {
    $('#iFrameReportArticle').attr('src', url);
    $('[id$="_hfarticleID"]').val(ID);
    $('[id$="_hdnArticleType"]').val(Type);
    ShowModal('diviFrameArticle');
}


function PrintIframeReport() {
    var IframeUrl = document.getElementById("iFrameReportArticle").src;
    var docprint = window.open(IframeUrl, "_blank");
}

function CloseNewsIframeReport() {
    $("#iFrameReportArticle").html('');
    $("#iFrameReportArticle").attr('src', '');
    closeModal('diviFrameArticle');
}



function ClosePlayerReport() {
    $('[id$="_divRawMedia"]')[0].innerHTML = "";
    closeModal('diviframe');
}

function PlayVideo(VideoID) {
    __doPostBack($('[id$="Report1_upReport"]')[0].id, VideoID);
}

function PlayClip(ClipID) {
    __doPostBack($('[id$="UCMyIQReportControl_upMainGrid"]')[0].id, ClipID);
}

function SetPlayerPopupWidth() {
    var divmarleft2 = parseInt(($(window).width() / 2) - (920 / 2)).toString() + 'px';

    $('[id$="_diviframe"]').css({ "left": divmarleft2, "position": "fixed" });
}

function SaveTweet(TweetID, Type) {    
    $('[id$="_hfarticleID"]').val(TweetID);
    $('[id$="_hdnArticleType"]').val(Type);
    OpenSaveArticlePopup(1);   
}