<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IQAgentIframe.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/IQAgentIframe/IQAgentIframe.ascx" TagName="IQAgentIframe"
    TagPrefix="uc" %>
<%@ Register Src="../UserControl/IQMediaMaster/FooterPanel/FooterPanel.ascx" TagName="UCFooterPanel"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Script/jquery-1.8.3.min.js" type="text/javascript"></script>
    <link href="../Css/my-style.css" rel="stylesheet" type="text/css" />
    <script src="../Script/IQMediaraw.js?v=4624" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
        function showCaption() {

            document.getElementById('ucIQAgentIframe_tblClosedCaption').style.display = "block";
            $("#ucIQAgentIframe_ClosedCaption").show('slow');

           // parent.window.ResizeIframe(730);
        }
        function hideCaption() {
            $("#ucIQAgentIframe_ClosedCaption").hide('slow');
            (document.getElementById('ucIQAgentIframe_ClosedCaption')).setAttribute('enable', 'true');

          //  parent.window.ResizeIframe(475);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
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
                            <img id="Img2" src="~/images/logo_n.png" runat="server" alt="iQMedia" hspace="9"
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
                                        <uc:IQAgentIframe ID="ucIQAgentIframe" runat="server" />
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
    </form>
</body>
</html>
