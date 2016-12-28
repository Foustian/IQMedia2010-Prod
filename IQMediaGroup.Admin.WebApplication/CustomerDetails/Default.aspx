<%@ Page Title="iQMedia :: Customer Details" Language="C#" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.CustomerDetails.Default" %>

<%@ Register Src="~/UserControl/CustomerDetails/CustomerDetails.ascx" TagName="CustomerDetails"
    TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/RightTopLogin/RightTopLogin.ascx" TagName="UCRightTopLogin"
    TagPrefix="UC" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <!-- Use IE7 mode -->
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE7" />
    <title>IQMedia</title>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="formCM" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server" AsyncPostBackTimeout="3600">
    </asp:ScriptManager>
    <div>
        <table width="963" border="0" align="center" cellpadding="0" cellspacing="0">
            <tr>
                <td class="mainbox">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td valign="top">
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="8" valign="top">
                                            <img id="Img1" runat="Server" src="~/images/mainbox-lt.jpg" width="8" height="8"
                                                border="0" alt="iQMedia" />
                                        </td>
                                        <td valign="top">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td width="317" height="99" valign="bottom">
                                                        <a runat="server" id="hlogo">
                                                            <img id="Img2" runat="Server" src="~/images/logo-IQMediaGroup.jpg" alt="iQMedia"
                                                                width="245" height="71" hspace="9" vspace="11" border="0" /></a>
                                                    </td>
                                                    <td valign="bottom">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                            <tr>
                                                                <td valign="top" colspan="2">
                                                                    <UC:UCRightTopLogin ID="UCRightTopLoginControl" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td width="45">
                                                                    &nbsp;
                                                                </td>
                                                                <td height="64" valign="bottom" align="left">
                                                                    <%--<UC:UCHeaderTabPanel ID="UCHeaderTabPanelControl" runat="server"/>--%>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td width="8" valign="top">
                                            <img id="Img3" runat="Server" src="~/images/mainbox-rt.jpg" width="8" height="8"
                                                border="0" alt="iQMedia" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="contentarea">
                                <table cellpadding="3" width="100%" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <div style="width: 100%; height: 100%; background-color: #C0C0C0">
                                                <asp:Menu ID="Menu1" runat="server" Width="20%" Orientation="Horizontal" BackColor="#F7F6F3"
                                                    DynamicHorizontalOffset="2" Font-Names="Tahoma" Font-Size="12px" ForeColor="#333333"
                                                    StaticSubMenuIndent="5px">
                                                    <StaticSelectedStyle BackColor="#B0B0B0" />
                                                    <StaticMenuItemStyle HorizontalPadding="5px" BackColor="#C0C0C0" VerticalPadding="5px" />
                                                    <DynamicHoverStyle BackColor="#C0C0C0" ForeColor="Black" />
                                                    <DynamicMenuStyle BackColor="#B0B0B0" />
                                                    <DynamicSelectedStyle BackColor="#5D7B9D" />
                                                    <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="5px" />
                                                    <StaticHoverStyle BackColor="#C0C0C0" ForeColor="White" />
                                                </asp:Menu>
                                            </div>
                                            <div style="padding: 10px 2px 5px 5px;">
                                                <asp:Label ID="lblBreadCrumb" runat="server"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" width="100%" align="right">
                                                <tr>
                                                    <td>
                                                        <uc2:CustomerDetails ID="CustomerDetails1" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td width="8" valign="bottom">
                                            <img id="Img4" runat="Server" src="~/images/mainbox-lb.jpg" width="8" height="8"
                                                border="0" alt="iQMedia" />
                                        </td>
                                        <td>
                                            <img id="Img5" runat="Server" src="~/images/spacer.gif" width="1" height="1" border="0"
                                                alt="iQMedia" />
                                        </td>
                                        <td width="8" valign="bottom">
                                            <img id="Img6" runat="Server" src="~/images/mainbox-rb.jpg" width="8" height="8"
                                                border="0" alt="iQMedia" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="footer-text">
                    <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="30">
                                <%--<asp:LinkButton ID="btnHome" runat="server" Text="Home" OnClick="btnHome_Click"></asp:LinkButton>|<asp:LinkButton
                                    ID="lnkProducts" runat="server" Text="Products" OnClick="lnkProducts_Click"></asp:LinkButton>|<asp:LinkButton
                                        ID="lnkAboutUs" runat="server" Text="About Us" OnClick="lnkAboutUs_Click"></asp:LinkButton>|<asp:LinkButton
                                            ID="lnkCareer" runat="server" Text="Careers" OnClick="lnkCareer_Click"></asp:LinkButton>|<asp:LinkButton
                                                ID="lnkContactUs" runat="server" Text="Contact Us" OnClick="lnkContactUs_Click"></asp:LinkButton>--%>
                            </td>
                            <td align="right">
                                &copy; 2009. <strong>iQ Media Group</strong>. All rights reserved.
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
<script type="text/javascript">

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
