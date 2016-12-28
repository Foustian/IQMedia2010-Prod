<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Default" %>

<%@ Register Src="UserControl/IQMediaMaster/Home/Home.ascx" TagName="Home" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Media Intelligence Platform for Monitoring and Leveraging Broadcast TV, Online
        News, and Social Media </title>
    <meta name="description" content="Activate broadcast TV, online news, and social media with a media intelligence platform, search TV media and make video clips for sharing" />
    <meta name="keywords" content="broadcast TV monitoring, television monitoring, media intelligence platform, media monitoring, video clips, video clipping service, TV media search, TV media alerts, online news, media monitoring services" />
    <meta name="title" content="media intelligence platform" />
    <link href="http://fonts.googleapis.com/css?family=Open+Sans:400" rel="stylesheet"
        type="text/css" />
    <script src="Script/jquery-1.8.1.min.js" type="text/javascript"></script>
    <link href="Css/fonts/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="Css/style_v2.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="css/superfish.css" media="screen" />
    <script src="Script/html5.js" type="text/javascript"></script>
    <script src="Script/hoverIntent.js" type="text/javascript"></script>
    <script src="Script/superfish.js" type="text/javascript"></script>
    <script type="text/javascript">

        // initialise plugins
        jQuery(function () {
            jQuery('ul.sf-menu').superfish();
        });
        $(document).ready(function () {

            $(".sf-menu li ul li a").hover(function () {
                // $(this).parent().parent().parent().find("a:first").attr('style', 'background:url(images/mainlink-bg-hover.png) repeat-x left top; color:#fff');
            }, function () {
                // $(this).parent().parent().parent().find("a").attr('style', null);
            });

            if (navigator.userAgent.indexOf('Safari') != -1 && navigator.userAgent.indexOf('Chrome') == -1) {
                $('.sf-menu li a').addClass('safari');
                $('.sf-menu li li a').addClass('safari-sub');
                $('.sf-menu li.first a, .sf-menu li.last a, .sf-menu li li a').removeClass('safari');
            }

        });

    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <uc1:Home ID="Home1" runat="server" />
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
</html>
