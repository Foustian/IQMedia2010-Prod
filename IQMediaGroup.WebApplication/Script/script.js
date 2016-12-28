$(document).ready(function() {
    //Detecting IE6 or less
    var badBrowser = (/MSIE ((5\.5)|6)/.test(navigator.userAgent) && navigator.platform == "Win32");
    if (badBrowser) {
        $(document).pngFix(); 
    }
});