<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomError.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.CustomError.CustomError" %>
<div>
    <table width="963" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td id="Td1" runat="server" align="center" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td valign="top">
                                                    &nbsp;
                                                </td>
                                                <td align="left">
                                                    <a href="javascript:void(0)">
                                                        <img src="../images/logo_N.png" alt="iQMedia" hspace="9" vspace="11" border="0" /></a>
                                                </td>
                                                <td width="8" valign="top">
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table width="96%" border="0" align="center" valign="top" cellspacing="0">
                                            <tr>
                                                <td align="left" class="bluebox-hd">
                                                    <asp:Label ID="lblHeader" runat="server"></asp:Label>
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="blue-content-bg">
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
                                                                        <td height="100" class="content-text2">
                                                                            <span class="heading-blue">
                                                                                <asp:Label ID="lblErrorMsgHeading" runat="server"></asp:Label>
                                                                            </span>
                                                                            <br />
                                                                            <asp:Label ID="lblMsg" runat="server" Font-Size="14px"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="50" align="left">
                                                                <table border="0" align="left" cellpadding="0" cellspacing="5" style="padding-left: 100px;">
                                                                    <tr>
                                                                        <td class="content-text2">
                                                                            Go to:
                                                                        </td>
                                                                        <td class="heading-blue">
                                                                            <a id="A1" href="~/" runat="server">Home</a>
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
                                            <%--  <tr>
                                                <td width="8">
                                                    <img src="../Images/contentbox-lt.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                </td>
                                                <td class="contentbox-t">
                                                    <img src="../Images/contentbox-t.jpg" width="1" height="8" border="0" alt="iQMedia" />
                                                </td>
                                                <td width="8">
                                                    <img src="../Images/contentbox-rt.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                </td>
                                            </tr>--%>
                                            <%--<tr>--%>
                                            <%--<td class="contentbox-l">
                                                    <img src="../Images/contentbox-l.jpg" width="8" height="1" border="0" alt="iQMedia" />
                                                </td>--%>
                                            <%--<td valign="top" bgcolor="#FFFFFF" class="pad-bt">
                                                    <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        
                                                    </table>
                                                </td>--%>
                                            <%--<td class="contentbox-r">
                                                    <img src="../Images/contentbox-r.jpg" width="8" height="1" border="0" alt="iQMedia" />
                                                </td>--%>
                                            <%-- </tr>--%>
                                            <%--<tr>
                                                <td height="8">
                                                    <img src="../Images/contentbox-lb.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                </td>
                                                <td class="contentbox-b">
                                                    <img src="../Images/contentbox-b.jpg" width="1" height="8" border="0" alt="iQMedia" />
                                                </td>
                                                <td>
                                                    <img src="../Images/contentbox-rb.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                </td>
                                            </tr>--%>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="8" height="8" align="left" valign="bottom">
                                                    <img src="../images/mainbox-lb.png" width="8" height="8" border="0" alt="iQMedia" />
                                                </td>
                                                <td>
                                                    <img src="../Images/spacer.gif" width="1" height="1" border="0" alt="iQMedia" />
                                                </td>
                                                <td width="8" height="8" align="right" valign="bottom">
                                                    <img src="../images/mainbox-rb.png" width="8" height="8" border="0" alt="iQMedia" />
                                                </td>
                                            </tr>
                                        </table>
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
                &nbsp;
            </td>
        </tr>
    </table>
</div>
