<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Resources.Default"
    Title="Information about Media Intelligence Platforms - Next Generation Beyond Media Monitoring Services" %>

<%@ Register Src="~/UserControl/IQMediaMaster/TopPanel/TopPanel.ascx" TagName="TopPanel"
    TagPrefix="uc1" %>
<%@ Register Src="~/UserControl/IQMediaMaster/StaticMasterRightContent/StaticMasterRightContent.ascx"
    TagName="StaticMasterRightContent" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/IQMediaMaster/FooterPanel/FooterPanel.ascx" TagName="FooterPanel"
    TagPrefix="uc3" %>
<%@ Register Src="~/UserControl/IQMediaMaster/Resources/Resources.ascx" TagName="Resource"
    TagPrefix="uc4" %>
<%@ Register Src="~/UserControl/IQMediaMaster/SocialNetworkingWebsitesPanel/SocialNetworkingWebsitesPanel.ascx"
    TagName="SocialNetworkingWebsitesPanel" TagPrefix="uc5" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Information about Media Intelligence Platforms - Next Generation Beyond Media
        Monitoring Services </title>
    <meta name="description" content="Resources you can use to learn about media intelligence platforms and how they are transforming the media monitoring industry" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, TV media content searching, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, media platform technology, fair use video content, video streaming" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400" rel="stylesheet"
        type="text/css" />
    <link href="http://fonts.googleapis.com/css?family=Merriweather:400" rel="stylesheet"
        type="text/css" />
    <script src="../Script/jquery-1.2.6.min.js" type="text/javascript"></script>
    <link href="../Css/fonts/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style_v2.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="../css/superfish.css" media="screen" />
    <script src="../Script/hoverIntent.js" type="text/javascript"></script>
    <%--<link href="../Css/chromemenu/chrometheme/chromestyle.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="../Script/chromejs/chrome.js" type="text/javascript"></script>--%>
    <script src="../Script/html5.js" type="text/javascript"></script>
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

        function viewtab(n) {
            if (n == 1) {

                if (document.getElementById('searchmedia'))
                    document.getElementById('searchmedia').style.display = "block";
                if (document.getElementById('media'))
                    document.getElementById('media').className = "searchtab-active";
                if (document.getElementById('searchclips'))
                    document.getElementById('searchclips').style.display = "none";
                if (document.getElementById('clips'))
                    document.getElementById('clips').className = "searchtab";

            } else if (n == 2) {

                if (document.getElementById('searchmedia'))
                    document.getElementById('searchmedia').style.display = "none";
                if (document.getElementById('media'))
                    document.getElementById('media').className = "searchtab";
                if (document.getElementById('searchclips'))
                    document.getElementById('searchclips').style.display = "block";
                if (document.getElementById('clips'))
                    document.getElementById('clips').className = "searchtab-active";

            }

        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <AjaxToolkit:ToolkitScriptManager ID="ScriptManager1" AsyncPostBackTimeout="3600"
        runat="server">
    </AjaxToolkit:ToolkitScriptManager>
    <div id="wrapper">
        <uc1:TopPanel ID="TopPanel" runat="server" />
        <section>
    
    <div class="content-main">
    	<div class="about-left2">
        	<div class="top"><img src="../images/about-top-bg2.png" alt=""></div>
            <div class="about-mid2">
            	<uc5:SocialNetworkingWebsitesPanel ID="SocialNetworkingWebsitesPanel" runat="server" />
                
        <uc4:Resource runat="server" ID="ucResource" />       
          
        </div>
        <div class="bottom"><img src="../images/about-bottom-bg2.png" alt=""></div>
   	  
    </div>
    </div>
   
  </section>
        <uc3:FooterPanel ID="ucFooterPanel" runat="server" />
    </div>
    </form>
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
    <!-- Start pdf link in google analytics -->
    <script type="text/javascript">
        $("a[href$='pdf']").each(function (index) {
            pdfLabel = $(this).attr('href');
            pdfOnClick = "_gaq.push(['_trackEvent', 'PDF', 'Download', '" + pdfLabel + "']);";
            $(this).attr("onClick", pdfOnClick);
        });
    </script>
    <!-- End pdf link in google analytics -->
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
    <!-- Google Code for Remarketing tag -->
    <!-- Remarketing tags may not be associated with personally identifiable information or placed on pages related to sensitive categories. For instructions on adding this tag and more information on the above requirements, read the setup guide: google.com/ads/remarketingsetup -->
    <script type="text/javascript">

// <![CDATA[
        var google_conversion_id = 985937458;

        var google_conversion_label = "7KwVCKba0AcQsuyQ1gM";

        var google_custom_params = window.google_tag_params;

        var google_remarketing_only = true;

// ]]> 

    </script>
    <script type="text/javascript" src="http://www.googleadservices.com/pagead/conversion.js">

    </script>
    <noscript>
        <div style="display: inline;">
            <img height="1" width="1" style="border-style: none;" alt="" src="http://googleads.g.doubleclick.net/pagead/viewthroughconversion/985937458/?value=0&amp;label=7KwVCKba0AcQsuyQ1gM&amp;guid=ON&amp;script=0" />
        </div>
    </noscript>
</body>
<script type="text/javascript">

    function __flash__addCallback(instance, name) {
        if (instance) {
            instance[name] = null;
        }
    }
    function __flash__removeCallback(instance, name) {
        if (instance) {
            instance[name] = null;
        }
    }
</script>
<script type="text/javascript">

    function noError() { return true; }
    window.onerror = noError;

</script>
</html>
