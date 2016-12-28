﻿var Tynt = Tynt || [];
if (typeof _wau !== "undefined") {
    var WAU_ren = WAU_ren || [];
    clearTimeout(window.WAU_f_init);
    window.WAU_f_init = setTimeout(WAU_pl, 300)
}

function WAU_classic(b, d) {
    alert('WAU_classic : b:' + b + ' d :' + d);
    if (typeof d === "undefined") {
        var d = -1
    }
    var a = "";
    if (document.title) {
        a = encodeURIComponent(document.title.substr(0, 80).replace(/(\?=)|(\/)/g, ""))
    }
    alert('WAU_classic : A:' + a);
    var c = document.getElementsByTagName("script")[0];

    alert('WAU_classic : c:' + c);
    (function () {
        var f = encodeURIComponent(document.referrer);
        alert('WAU_classic : f:' + f);
        var e = document.createElement("script");
        e.async = "async";
        e.type = "text/javascript";
        e.src = "http://whos.amung.us/pingjs/?k=" + b + "&t=" + a + "&c=c&y=" + f + "&a=" + d + "&r=" + Math.ceil(Math.random() * 999999);
        alert('WAU_classic : e.src:' + "http://whos.amung.us/pingjs/?k=" + b + "&t=" + a + "&c=c&y=" + f + "&a=" + d + "&r=" + Math.ceil(Math.random() * 999999));
        c.parentNode.insertBefore(e, c)
    })();
    if (document.location.protocol == "http:") {
        Tynt.push("w!" + b);
        (function () {
            var e = document.createElement("script");
            e.async = "async";
            e.type = "text/javascript";
            e.src = "http://cdn.tynt.com/tc.js";
            alert('WAU_classic http e.src:' + e.src);
            c.parentNode.insertBefore(e, c)
        })() 
     }
}

function WAU_r_c(c, key, async_index) {
    alert('WAU_r_c : c:' + c + ' key :' + key + ' async_index :' + async_index);
    if (typeof async_index === "undefined") {
        var async_index = -1
    }
    var raw_im_data = "data:image/gif;base64,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";
    var raw_im_meta_l = eval("({'0':[-69,-29,9,17], '1':[0,-29,6,17], '2':[-6,-29,9,17], '3':[-15,-29,9,17], '4':[-24,-29,10,17], '5':[-34,-29,8,17],'6':[-42,-29,9,17], '7':[-51,-29,9,17], '8':[-60,-29,9,17], '9':[-70,-46,9,14], ',':[-78,-29,4,17]})");
    var raw_im_meta_s = eval("({'0':[-59,-46,7,14], '1':[0,-46,4,14], '2':[-4,-46,7,14], '3':[-11,-46,7,14], '4':[-18,-46,7,14], '5':[-25,-46,6,14],'6':[-31,-46,7,14], '7':[-38,-46,7,14], '8':[-45,-46,7,14], '9':[-52,-46,7,14], ',':[-66,-46,4,14]})");
    if (WAU_legacy_b()) {
        raw_im_data = "http://widgets.amung.us/widtemplates/classicoutline.gif"
    }
    c = c.split("");
    var w_large = 0;
    var w_small = 0;
    for (var i = 0; i < c.length; i++) {
        w_large += raw_im_meta_l[c[i]][2] + 2;
        w_small += raw_im_meta_s[c[i]][2] + 2
    }
    if ((w_large - 2) > 54) {
        var y_pos = 9;
        var meta = raw_im_meta_s;
        var left_offset = 24
    } 
    else {
        var y_pos = 7;
        var meta = raw_im_meta_l;
        var left_offset = 19;
        if (w_large > 25) {
            left_offset = 23
        } 
        if (w_large > 50) {
            left_offset = 21
        }
    }
    var img = document.createElement("img");
    img.onload = function () {
        var wid = document.createElement("div");
        wid.style.position = "relative";
        wid.style.display = "inline-block";
        wid.style.backgroundImage = "url(" + raw_im_data + ")";
        wid.style.width = "81px";
        wid.style.height = "29px";
        wid.style.padding = "0";
        wid.style.overflow = "hidden";
        wid.style.cursor = "pointer";
        wid.style.direction = "ltr";
        wid.title = "Click to see stats for this site by whos.amung.us (" + key + ")";
        var txt = document.createElement("div");
        txt.style.position = "absolute";
        txt.style.top = y_pos + "px";
        txt.style.padding = "0";
        txt.style.margin = "0";
        var x_pos = 0;
        var txt_w = 0;
        for (var i = 0; i < c.length; i++) {
            var char_meta = meta[c[i]];
            var character = document.createElement("div");
            character.style.backgroundImage = "url(" + raw_im_data + ")";
            character.style.backgroundRepeat = "no-repeat";
            character.style.backgroundAttachment = "scroll";
            character.style.backgroundPosition = char_meta[0] + "px " + char_meta[1] + "px";
            character.style.position = "absolute"; character.style.width = char_meta[2] + "px";
            character.style.height = char_meta[3] + "px";
            character.style.left = x_pos + "px";
            character.style.lineHeight = char_meta[3] + "px";
            character.style.overflow = "hidden";
            character.style.padding = "0";
            character.style.margin = "0";
            txt.appendChild(character);
            x_pos += char_meta[2] + 2;
            txt_w += char_meta[2] + 2
        }
        txt.style.left = (left_offset + Math.floor((54 - (txt_w - 2)) / 2)) + "px";
        wid.appendChild(txt);
        wid.onclick = function () {
            window.location = "http://whos.amung.us/stats/" + key + "/"
        };
        if (async_index >= 0) {
            var scr = document.getElementById("_wau" + _wau[async_index][2]);
            scr.parentNode.insertBefore(wid, scr.nextSibling)
        }
        else {
            WAU_insert(wid, "amung.us/classic.js")
        }
    };
    img.src = raw_im_data
}

function WAU_insert(c, d) {
    var a = document.getElementsByTagName("script");
    for (var b = 0; b < a.length; b++) {
        if (a[b].src.indexOf(d) > 0) {
            a[b].parentNode.insertBefore(c, a[b].nextSibling)
        }
    }
}

function WAU_legacy_b() {
    if (navigator.appVersion.indexOf("MSIE") != -1 && parseFloat(navigator.appVersion.split("MSIE")[1]) < 8) {
        return true
    }
    return false
}

function WAU_pl() {
    document.body ? WAU_la() : setTimeout(WAU_pl, 500)
}

function WAU_la() {
    for (var a = 0; a < _wau.length; a++) {
        if (typeof WAU_ren[a] === "undefined" || WAU_ren[a] == false) {
            WAU_ren[a] = true;
            if (typeof window["WAU_" + _wau[a][0]] === "function") {
                if (_wau[a][0] == "map") {
                    window.WAU_map(_wau[a][1], _wau[a][3], _wau[a][4], _wau[a][5], _wau[a][6], a)
                }
                else {
                    if (typeof _wau[a][3] !== "undefined") {
                        window["WAU_" + _wau[a][0]](_wau[a][1], _wau[a][3], a)
                    }
                    else {
                        window["WAU_" + _wau[a][0]](_wau[a][1], a)
                    }
                }
            }
        }
    }
};
