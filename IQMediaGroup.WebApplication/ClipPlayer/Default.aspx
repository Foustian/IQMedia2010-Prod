<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.ClipPlayer.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/ClipPlayer/ClipPlayer.ascx" TagName="ClipPlayer"
    TagPrefix="UCClipPlayer" %>
<%@ Register Src="../UserControl/IQMediaMaster/FooterPanel/FooterPanel.ascx" TagName="UCFooterPanel"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>iQMedia :: ClipPlayer</title>
    <%--<link href="~/Css/style_v1.css" rel="stylesheet" type="text/css" />--%>
    <link href="~/Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../Script/jquery-1.8.1.min.js"></script>
    <script language="javascript" type="text/javascript">

      /*  var isRedirected = false;
        $(document).ready(function () {

            $(document).unload(function () {
                alert('on unload');
                isRedirected = true;

            });

        });*/

       

        function showdivCaption() {

            document.getElementById('ClipPlayerControl_tblClosedCaption').style.display = "block";
            $("#ClipPlayerControl_divClosedCaption").show('slow');
        }
        function hidedivCaption() {
            $("#ClipPlayerControl_divClosedCaption").hide('slow');
            (document.getElementById('ClipPlayerControl_divClosedCaption')).setAttribute('enable', 'true');
        }

      /*  function redirectFunction() {
            document.location = 'http://www.google.com';
        }*/
    </script>
    <style type="text/css">
        .style1
        {
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
            font-weight: bold;
            color: #FFFFFF;
            text-decoration: none;
            background-image: url('images/bluebox-hd-bg.jpg');
            background-repeat: repeat-x;
            width: 20px;
        }
    </style>
    <script type="text/javascript">
        function LogOut() {
            lnkLogout_Click();

        }

        function ShowMsg(msg) {
            alert(msg);
        }

    </script>
</head>
<body>
    <form id="formM" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="clipDiv" runat="server">
        <div>            
            <table width="963px" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <%--<img id="img" runat="server" src="http://qa.iqmediacorp.com/StationLogoImages/WPGH.gif" />--%>
                    </td>
                </tr>
                <tr>
                    <td class="mainbox" style="height: 99px;">
                        <a runat="server" id="hlogo">
                            <img id="imgLogo" src="~/images/logo_n.png" runat="server" alt="iQMedia" hspace="9"
                                vspace="11" border="0" /></a>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td valign="top" align="center" class="contentarea" style="margin-top: 10px;">
                        <div class="content">
                            <div class="main-content">
                                <div class="block left-top">
                                    <div class="block-inner">
                                        <div class="block-top">
                                            <!-- -->
                                        </div>
                                        <div class="block-top-end">
                                            <!-- -->
                                        </div>
                                        <div class="block-left-end">
                                            <!-- -->
                                        </div>
                                        <div class="block-lt-corner">
                                            <!-- -->
                                        </div>
                                        <br />
                                        <br />
                                        <asp:Label ForeColor="Red" ID="lblErrorMsg" runat="server"></asp:Label>
                                        <UCClipPlayer:ClipPlayer ID="ClipPlayerControl" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <uc2:UCFooterPanel ID="UCFooterPanelControl" runat="server" />
                    </td>
                </tr>
            </table>
        </div>       
    </div>
    <div id="iosDiv" runat="server">
    </div>
    </form>
</body>
<script type="text/javascript">

    function noError() { return true; }
    window.onerror = noError;

</script>
</html>
