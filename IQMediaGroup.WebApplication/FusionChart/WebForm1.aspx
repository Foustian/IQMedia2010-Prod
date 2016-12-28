<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="IQMediaGroup.WebApplication.FusionChart.WebForm1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" src="js/FusionCharts.js"></script>
    <link href="../Css/my-style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" language="javascript">
        function LoadPlayer() {
            /*var B = 'img#IFrameRawMedia1_time0.hourSeek';
            $(B).addClass("selected"); $("#timeBar").children(":not(.selected)").fadeTo(400, 0.4); $(B).removeClass("selected"); $(B).fadeTo(400, 1);
            showRawMediaPlayer(false);*/
        }
       
        $(document).ready(function () {          

            $('#tgtbutton').click(function () {
                $('#ifrmae').attr('src', 'http://localhost:2281/IFrameRawMediaH/Default.aspx?RawMediaID=32fbe744-9f61-439c-b896-ef69e21341b1&SearchTerm=&IsUGC=false');

            });

        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SC1" runat="server">
    </asp:ScriptManager>
    <%-- <div id="divMsLineChart"></div>
     <script type="text/javascript">


         function LoadMSLinechart(data1) {
             alert('hi');
             var myZoomChart = new FusionCharts("/FusionChart/Line.swf",
                            "Line", "800", "200");
             myZoomChart.setJSONData('{"chart":{ "caption":"Business Results 2005 v 2006", "xAxisName":"Month", "yAxisName":"Revenue", "showvalues":"0", "numberprefix":"$" }, "data":[ { "label":"January","value":"17400" }, { "label":"February","value":"19800" }, { "label":"March","value":"21800" }, { "label":"April","value":"23800" }, { "label":"May","value":"29600"  }, { "label":"June","value":"27600" }, { "vline":"true","color":"FF5904","thickness":"2"}, { "label":"July","value":"31800" }, { "label":"August","value":"39700"  }, { "label":"September","value":"37800"}, { "label":"October","value":"21900" }, { "label":"November","value":"32900" }, { "label":"December","value":"39800" } ]}');
             //myZoomChart.setJSONUrl("/FusionChart/Data.json");
             myZoomChart.render("divMsLineChart");


         }
         LoadMSLinechart('tst');
    </script>--%>
    <input type="button" id="tgtbutton" runat="server" value="Show Player" />
    <div id="diviframe" runat="server" style="height: 350px;width:100%;">
        <iframe id="ifrmae" style="width: 100%; height: 100%;margin-left:331px;"  frameborder="0" runat="server" src="http://localhost:2281/IFrameRawMediaH/Default.aspx?RawMediaID=32fbe744-9f61-439c-b896-ef69e21341b1&SearchTerm=&IsUGC=false">
        </iframe>
    </div>
    <AjaxToolkit:ModalPopupExtender BackgroundCssClass="ModalBackgroundLightBox" ID="mpeVideo"
        runat="server" PopupControlID="diviframe" TargetControlID="tgtbutton">
    </AjaxToolkit:ModalPopupExtender>
    </form>
</body>
</html>
