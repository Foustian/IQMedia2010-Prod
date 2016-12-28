function SetPlayerObject(id, clipid, width, height) {
    var control = document.getElementById(id);
    var btn = document.getElementById('play-btn-' + id);
    btn.style.display = "none";
    p = navigator.platform;
    //alert(navigator.platform);
    //var IsiOSorAndroid = /(iPad|iPhone|iPod|Android)/g.test(navigator.userAgent);
    if (p === 'iPad' || p === 'iPhone' || p === 'iPod' || p == 'Linux armv7l') {
        GetPlayerUrl(id, clipid);
    } else {
        if (control != null) {
            var objecttag = "<object data=\"http://l3cdn.iqmediacorp.com.c.footprint.net/SWFs/iqmedia_player_resize_cdn_v1.1.swf\" type=\"application/x-shockwave-flash\" name=\"HYETA\" id=\"HUY\" style=\"width:" + width + ";height:" + height + ";\">";
            objecttag += "<param value=\"http://l3cdn.iqmediacorp.com.c.footprint.net/SWFs/iqmedia_player_resize_cdn_v1.1.swf\" name=\"movie\">";
            objecttag += "<param value=\"true\" name=\"allowfullscreen\">";
            objecttag += "<param value=\"high\" name=\"quality\">";
            objecttag += "<param value=\"transparent\" name=\"wmode\">";
            objecttag += "<param value=\"embedId=" + clipid + "&autoPlayback=true\" name=\"flashvars\">";
            objecttag += "<param value=\"all\" name=\"AllowNetworking\">";
            objecttag += "<param value=\"always\" name=\"allowscriptaccess\">";
            objecttag += "<param name=\"AllowNetworking\" value=\"all\">";
            objecttag += "</object>";
            control.innerHTML = objecttag;
            control.onclick = "return false;"
        }
    }
}

function GetPlayerUrl(id, clipid) {
    var control = document.getElementById(id);
    $.ajax({
        type: 'GET',
        dataType: "jsonp",
        url: encodeURI("http://qaservices.iqmediacorp.com/iossvc/GetVars?ClipID=" + clipid + "&IsAndroid=true"),
        success: function (result) {
            if (result != null && result[0] != null && result[0].Media != "" && result[0].HasException == false) {
                window.open(result[0].Media + ".m3u8", "_blank");
            }
            else {
                control.innerHTML = result[0].ExceptionMessage;
            }
        },
        error: function (result) {
            control.innerHTML = result[0].ExceptionMessage;
        }
    });
}