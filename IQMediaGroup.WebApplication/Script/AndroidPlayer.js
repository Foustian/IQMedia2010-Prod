


function LoadHTML5Player(_clipID) {
    alert('handler called');
    $.ajax(
                {
                    type: 'GET',
                    dataType: 'jsonp',
                    url: 'http://qaservices.iqmediacorp.com/iossvc/GetVars?ClipID=' + _clipID + '&IsAndroid=true',
                    success: OnComplete,
                    error: OnFail
                });
}



function OnComplete(result, textStatus, jqXHR) {
    alert('Complete');
    $.each(result, function () {
        if (this.IsValidMedia.toString() == "true") {
            document.getElementById('ClipPlayerControl_divRawMedia').innerHTML = '<video width="545px" height="340px" tabindex="0" controls="controls" autoplay="autoplay" id="vidClip"><source src="' + this.Media + '.m3u8" type="application/x-mpegURL"></source></video> <div style="position: absolute; top: 43%; left: 43%;" onclick="playVideo();" id="divplaybtn"><img id="Img2" runat="server" images="~/images/play1.png"  /></div>';
        }
    });
}

function OnFail(jqXHR, textStatus, errorThrown) {
    alert('An error occured,please try again');
}

function playVideo() {
    document.getElementById('divplaybtn').style.display = 'none';
    var myVideo = document.getElementById("vidClip"); myVideo.play();
} 