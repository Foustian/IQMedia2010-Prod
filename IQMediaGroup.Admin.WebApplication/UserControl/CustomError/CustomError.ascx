<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomError.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.CustomError.CustomError" %>
<div>
    <table width="963" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="mainbox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="8" valign="top"><img src="../Images/mainbox-lt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                    <td valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="317" height="99" valign="bottom">
                                                    <a>
                                                        <img src="../Images/logo-IQMediaGroup.jpg" alt="iQMedia" width="245" height="71" hspace="9" vspace="11" border="0" /></a>
                                                </td>
                                                <td valign="top">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="8" valign="top"><img src="../Images/mainbox-rt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="contentarea">
                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" class="contentarea-right">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td width="8"><img src="../Images/contentbox-blue-lt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                                            <td class="contentbox-blue-t"><img src="../Images/contentbox-blue-t.jpg" width="1" height="8" border="0" alt="iQMedia" /></td>
                                                            <td width="8"><img src="../Images/contentbox-blue-rt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                                        </tr>
                                                        <tr>
                                                            <td class="contentbox-l"><img src="../Images/contentbox-l.jpg" width="8" height="1" border="0" alt="iQMedia" /></td>
                                                            <td valign="top" bgcolor="#FFFFFF" class="pad-bt">
                                                                <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="3">
                                                                                        <img src="../Images/bluebox-hd-left.jpg" width="3" height="24" border="0" alt="iQMedia" />
                                                                                    </td>
                                                                                    <td align="left" class="bluebox-hd">
                                                                                        <asp:Label ID="lblHeader" runat="server"></asp:Label>
                                                                                        <br />
                                                                                    </td>
                                                                                    <td width="3">
                                                                                        <img src="../Images/bluebox-hd-right.jpg" width="3" height="24" border="0" alt="iQMedia" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                            <%-- <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td class="pagetitle">
                                                                                        <asp:Label ID="lblHeader" runat="server"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>--%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="border01">
                                                                            <table width="90%" border="0" align="center" cellpadding="0" cellspacing="6">
                                                                                <tr>
                                                                                    <td height="50">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                            <tr>
                                                                                                <td width="100">
                                                                                                    <img id="imgError" src="../Images/error.png" width="64" height="64" hspace="0" border="0" />
                                                                                                </td>
                                                                                                <td height="100" class="content-text">
                                                                                                    <span class="heading-blue">
                                                                                                        <asp:Label ID="lblErrorMsgHeading" runat="server"></asp:Label>
                                                                                                    </span>
                                                                                                    <br />
                                                                                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="50">
                                                                                        <table border="0" align="center" cellpadding="0" cellspacing="5">
                                                                                            <tr>
                                                                                                <td class="content-text">
                                                                                                    Go to:
                                                                                                </td>
                                                                                                <td class="heading-blue">
                                                                                                    <%--<a href="~/Default.aspx">Login Page</a>--%>
                                                                                                    <asp:HyperLink ID="hlHome" NavigateUrl="~/" runat="server">Login Page</asp:HyperLink>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                        <a href="#"></a>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td height="50">
                                                                                        &nbsp;
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td class="contentbox-r">
                                                                <img src="../Images/contentbox-r.jpg" width="8" height="1" border="0" alt="iQMedia" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td><img src="../Images/contentbox-lb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                                            <td class="contentbox-b"><img src="../Images/contentbox-b.jpg" width="1" height="8" border="0" alt="iQMedia" /></td>
                                                            <td><img src="../Images/contentbox-rb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><img src="../Images/spacer.gif" width="1" height="10" border="0" alt="iQMedia" /></td>
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
                                    <td width="8" valign="bottom"><img src="../Images/mainbox-lb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                    <td><img src="../Images/spacer.gif" width="1" height="1" border="0" alt="iQMedia" /></td>
                                    <td width="8" valign="bottom"><img src="../Images/mainbox-rb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="footer-text">
                &nbsp;
            </td>
        </tr>
    </table>
</div>
