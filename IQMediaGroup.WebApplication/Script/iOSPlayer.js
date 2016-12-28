function CheckForIOS(ClipID, BaseUrl, IOSAppUrl) {
    var child = window.open('iqmedia://clipid=' + ClipID + '&baseurl=' + BaseUrl + '', 'IQMedia IOS Player');
    /*alert(child);
    alert('iqmedia://clipid=' + ClipID + '&baseurl=' + BaseUrl + '');*/
    setTimeout(function () {
        try {
            var foo = child.location;
            //alert(child.location);
        } catch (err) {
            //alert('return');
            return;
        }
        // Still loading. Code here. 
        window.open(IOSAppUrl, 'IQMedia IOS Player');
    }, 10000);
}
