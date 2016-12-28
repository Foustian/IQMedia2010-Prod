jqeueyLoad();
function jqeueyLoad() {
    var head = document.getElementsByTagName('head')[0];
    var script = document.createElement('script');
    script.type = 'text/javascript';
    script.onreadystatechange = function () {
        if (this.readyState == 'loaded') {
            $(document).ready(function () {
                resize();
            });

            $(window).resize(function () {
                resize();
            });
        }
    }
    script.onload = function () {
        $(document).ready(function () {
            resize();
        });

        $(window).resize(function () {
            resize();
        });
    }
    script.src = 'http://qa.iqmediacorp.com/Script/jquery-1.3.2.min.js';
    head.appendChild(script);
}
function resize() {
    var winWidth = 0, winHeight = 0;
    toolHeight = 0;
    if (typeof (window.innerWidth) == 'number') {
        //Non-IE
        winWidth = window.innerWidth;
        winHeight = window.innerHeight;
        toolHeight = window.outerHeight - winHeight+23;
    } else if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
        //IE 6+ in 'standards compliant mode'
        winWidth = document.documentElement.clientWidth;
        winHeight = document.documentElement.clientHeight;
        toolHeight = 210;
    } else if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
        //IE 4 compatible
        winWidth = document.body.clientWidth;
        winHeight = document.body.clientHeight;
        toolHeight = 210;
    }
    var maxheight = screen.height - toolHeight;
    var maxwidth = screen.width;
    var embed = document.getElementById("HUY");
    var CalHeight = Math.round(eval(winHeight * 340) / eval(maxheight));
    var CalWidth = Math.round(eval(winWidth * 545) / eval(maxwidth));
    
    var Wratio = eval(eval(CalWidth) / eval(CalHeight)).toFixed(1);
    var Hratio = eval(eval(CalHeight) / eval(CalWidth)).toFixed(1);
    document.getElementById("inn").innerHTML = 'winWidth:' + winWidth + 'winHeight:' + winHeight + '';
    document.getElementById("inn").innerHTML += '<br/>CalWidth:' + CalWidth + 'CalHeight:' + CalHeight + '';
    if (winWidth > winHeight) {
        /*if (CalWidth <= winWidth) {*/
          /*  if (Wratio == 1.6) {
                newW = CalWidth;
                newH = CalHeight;
                document.getElementById("inn").innerHTML += '<br/>default: <span>newW:' + newW + 'newH:' + newH + '</span>';
            }   
            else {*/

                newH = CalHeight;
                newW = Math.round(eval(CalHeight) * eval(1.603));
                document.getElementById("inn").innerHTML += '<br/>height wise width:<span>newW:' + newW + 'newH:' + newH + '</span>';
            /*}*/
        /*}
        else {
            newW = 0;
            newH = 0;
        }*/
    }
    else {
       /* if (CalHeight <= winHeight) {*/
          /*  if (Hratio == 0.6) {
                newW = CalWidth;
                newH = CalHeight;
                document.getElementById("inn").innerHTML += '<br/><span>newW:' + newW + 'newH:' + newH + '</span>';
            }
            else {*/

                newW = CalWidth;
                newH = Math.round(eval(CalWidth) * eval(0.623));
                document.getElementById("inn").innerHTML +='<br/>width wise height:<span>newW:' + newW + 'newH:' + newH + '</span>';
            /*}*/
        /*}
        else {
            newW = 0;
            newH = 0;
        }*/
    }

    if (newW == 0 || newH == 0) {
        
    }
    else {
        embed.width = newW;
        embed.height = newH;
    }
};

