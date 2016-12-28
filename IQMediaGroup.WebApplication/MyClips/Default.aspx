<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.MyClips.Default"
    MasterPageFile="~/IQMediaGroupResponsive.Master" Title="iQMedia :: my iQ" %>
<%@ Register Src="~/UserControl/IQMediaMaster/MyClips/MyClips.ascx" TagName="UCMyClips"
    TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $("#divVideo").hide();
            document.getElementById("divCaptionShowHide").style.display = "none";
        });

        function showdivPlayer() {
            //document.getElementById('ctl00_Content_Data_UCMyClipsControl_tblPlayer').style.display = "block";
            //$("#ctl00_Content_Data_UCMyClipsControl_Player").show('slow');
            $("#divVideo").show('slow');

        }
        function hidedivPlayer() {

            $("#divVideo").hide('slow');
            (document.getElementById('divVideo')).setAttribute('enable', 'true');
            /*$("#ctl00_Content_Data_UCMyClipsControl_Player").hide('slow');
            (document.getElementById('ctl00_Content_Data_UCMyClipsControl_Player')).setAttribute('enable', 'true');*/
        }
    </script>

    <script language="javascript" type="text/javascript">
        function showdivCaption() {

            //            document.getElementById('ctl00_Content_Data_UCMyClipsControl_tblClosedCaption').style.display = "block";
            //            $("#ctl00_Content_Data_UCMyClipsControl_divClosedCaption").show('slow');

            document.getElementById('ctl00_Content_Data_UCMyClipsControl_divClosedCaption').style.display = "block";
            $("#ctl00_Content_Data_UCMyClipsControl_divClosedCaption").show('slow');
        }
        function hidedivCaption() {
            //            $("#ctl00_Content_Data_UCMyClipsControl_divClosedCaption").hide('slow');
            //            (document.getElementById('ctl00_Content_Data_UCMyClipsControl_divClosedCaption')).setAttribute('enable', 'true');
            $("#ctl00_Content_Data_UCMyClipsControl_divClosedCaption").hide('slow');
            (document.getElementById('ctl00_Content_Data_UCMyClipsControl_divClosedCaption')).setAttribute('enable', 'true');
        }

        function Resize(ddlCategoryGrid) {

            ddlCategoryGrid.style.width = "80px";
            ddlCategoryGrid.style.zIndex = "99999";
        }
    </script>

</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content_Data" runat="Server">

    <script type="text/javascript" language="javascript">

        var instance = Sys.WebForms.PageRequestManager.getInstance();
        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                $addHandler(document, "keydown", onKeyDown);
            }
        }
        function onKeyDown(e) {
            if (e && e.keyCode == Sys.UI.Key.esc) {
                if (instance.get_isInAsyncPostBack()) {
                    instance.abortPostBack();
                }
            }
        }
    </script>

    <script type="text/javascript" language="javascript">

        var temp = 0;
        var IsTimeOut = 0;
        function btnstart() {
            temp = 1;

        }

        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

        function BeginRequestHandler(sender, args) {
            //alert('begin request from inner page');
            //var elem = $get('ctl00_Content_Data_UCMyClipsControl_Player');
            var elem = $get('divVideo');
            if (temp == 2) {
                elem.innerHTML = "Process Started..";

            }

        }
        function EndRequestHandler(sender, args) {
            // alert('end request from inner page');
            if (temp == 1) {

                //                var elem = $get('ctl00_Content_Data_UCMyClipsControl_Player');
                var elem = $get('divVideo');
                elem.style.display = "block";
                document.getElementById("divCaptionShowHide").style.display = "block";
                temp = 0;

            }
        }

        function ShowMsg(msg) {
            alert(msg);
        }

    </script>   
  
    <table width="100%" cellpadding="0" cellspacing="0" align="center">       
        <tr>
            <td>
                <UC:UCMyClips ID="UCMyClipsControl" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
