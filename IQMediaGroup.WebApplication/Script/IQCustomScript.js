$(document).ready(function () {

    $('[id$="_pnlFtpBrowse"]').on('show', function () {
        
        $('[id$="_pnlFtpBrowse"]').css("top", "10%");
        $('[id$="_pnlFtpBrowse"]').css("left", "27%");
        $('[id$="_pnlFtpBrowse"]').css("z-index", "1051");
        $('[id$="_pnlFtpBrowse"]').css("width", "620px");
        $('[id$="_pnlFtpBrowse"]').resizable({ resize: function (e, ui) {
            $('[id$="_pnlFtpBrowse"]').css("position", "fixed");
            $('[id$="_pnlFtpBrowse"]').css("top", "10%");
        }
        });
        $('.ui-icon').attr("title", 'Drag to resize');
        $('[id$="_pnlFtpBrowse"]').draggable({ handle: ".popup-hd" });
    });


    $('[id$="_pnlUploadMedia"]').on('show', function () {
        
        $('[id$="_pnlUploadMedia"]').css("top", "10%");
        $('[id$="_pnlUploadMedia"]').css("left", "27%");        
        $('[id$="_pnlUploadMedia"]').resizable({ resize: function (e, ui) {
            $('[id$="_pnlUploadMedia"]').css("position", "fixed");
            $('[id$="_pnlUploadMedia"]').css("top", "10%");
        }
        });
        $('.ui-icon').attr("title", 'Drag to resize');
        $('[id$="_pnlUploadMedia"]').draggable({ handle: ".popup-hd" });
    });
});


function showUploadMediaHideFtpBrowse() {
    
    HideUploadModal('pnlFtpBrowse');

    //ShowUploadModal('pnlUploadMedia');
    //$('[id$="_pnlUploadMedia"]').css("z-index", "19999");
}

function ShowiqCustomModal(pnlid) {
    
    //$('[id$="_' + pnlid + '"]').css({ "background-color": "White", "border-color": "rgb(111, 111, 111)", "border-width": "10px", "border-style": "solid", "display": "block", "opacity": "100" });
    $('[id$="_' + pnlid + '"]').modal({
        backdrop: 'static',
        keyboard: true
    });
    
}

function CloseiqCustomModal(pnlid) {
    
    $('[id$="_' + pnlid + '"]').modal('hide');
}

function closeFtpBrowseModal() {
    CloseiqCustomModal('pnlFtpBrowse');
    document.getElementById('ctl00_Content_Data_IQCustom1_hndIsFtpUpload').value = 'false';
    document.getElementById('ctl00_Content_Data_IQCustom1_hdnName').value = '';
    document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = '';
    


}