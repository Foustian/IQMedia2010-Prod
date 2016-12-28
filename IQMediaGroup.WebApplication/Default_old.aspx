<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default_old.aspx.cs" Inherits="IQMediaGroup.WebApplication.Default_old" %>

<%@ Register Src="UserControl/IQMediaMaster/HomeNew/HomeNew.ascx" TagName="HomeNew"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>iQ Media | Monitoring Services for Broadcast Television and Radio Media Content
    </title>
    <meta name="description" content="iQ Media offers you the most intelligent way to monitor broadcast television and radio media content. Our innovative and east-to-use platform empowers you to quickly find the media you are looking for without costly delays." />
    <meta name="keywords" content="iq media, iq media service, tv clips, tv clipping service, tv monitoring, clipping alerts, clipping service, clipping services, broadcast monitoring, media monitor, media monitoring, media tracking, news monitoring, pr software, public relations software, IQMEDIA, IQmedia, IQ Media" />
    <link href="Css/master.css" rel="stylesheet" type="text/css" />
    <link href="Css/flexcrollstyles.css" rel="stylesheet" type="text/css" />
    <link href="Css/tutorsty.css" rel="stylesheet" type="text/css" />

    <!--[if lt IE 8]>
        <link rel="stylesheet" href="Css/master-ie.css" type="text/css" media="screen" />
    <![endif]-->

    <script src="Script/jquery-1.3.2.min.js" type="text/javascript"></script>
    <script src="Script/flexcroll.js" type="text/javascript"></script>

    <!--[if lte IE 6]>
        <script type="text/javascript" src="Script/jquery.pngfix.js"></script>
    <![endif]-->
    
    <script type="text/javascript" src="Script/script.js"></script>

    <script type="text/javascript">
<!--
        function MM_swapImgRestore() { //v3.0
            var i, x, a = document.MM_sr; for (i = 0; a && i < a.length && (x = a[i]) && x.oSrc; i++) x.src = x.oSrc;
        }
        function MM_preloadImages() { //v3.0
            var d = document; if (d.images) {
                if (!d.MM_p) d.MM_p = new Array();
                var i, j = d.MM_p.length, a = MM_preloadImages.arguments; for (i = 0; i < a.length; i++)
                    if (a[i].indexOf("#") != 0) { d.MM_p[j] = new Image; d.MM_p[j++].src = a[i]; }
            }
        }

        function MM_findObj(n, d) { //v4.01
            var p, i, x; if (!d) d = document; if ((p = n.indexOf("?")) > 0 && parent.frames.length) {
                d = parent.frames[n.substring(p + 1)].document; n = n.substring(0, p);
            }
            if (!(x = d[n]) && d.all) x = d.all[n]; for (i = 0; !x && i < d.forms.length; i++) x = d.forms[i][n];
            for (i = 0; !x && d.layers && i < d.layers.length; i++) x = MM_findObj(n, d.layers[i].document);
            if (!x && d.getElementById) x = d.getElementById(n); return x;
        }

        function MM_swapImage() { //v3.0
            var i, j = 0, x, a = MM_swapImage.arguments; document.MM_sr = new Array; for (i = 0; i < (a.length - 2); i += 3)
                if ((x = MM_findObj(a[i])) != null) { document.MM_sr[j++] = x; if (!x.oSrc) x.oSrc = x.src; x.src = a[i + 2]; }
        }
//-->
    </script>
    
    <script language="javascript" type="text/javascript">
        function LogOut() {           
            lnkLogout_Click();
        }
        </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc1:HomeNew ID="HomeNew1" runat="server" />
    </form>

    <!-- Start of Async HubSpot Analytics Code -->

    <script type="text/javascript">

        (function (d, s, i, r) {

            if (d.getElementById(i)) { return; }

            var n = d.createElement(s), e = d.getElementsByTagName(s)[0];

            n.id = i; n.src = '//js.hubspot.com/analytics/' + (Math.ceil(new Date() / r) * r) + '/182448.js';

            e.parentNode.insertBefore(n, e);

        })(document, "script", "hs-analytics", 300000);

    </script>

<!-- End of Async HubSpot Analytics Code -->

</body>
 <script type="text/javascript">

     var _gaq = _gaq || [];
     _gaq.push(['_setAccount', 'UA-21028943-1']);
     _gaq.push(['_trackPageview']);

     (function () {
         var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.sync = true;
         ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
         var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
     })();

</script>
</html>
