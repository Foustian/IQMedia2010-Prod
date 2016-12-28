<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.NeedHelp.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../Css/style.css" rel="stylesheet" type="text/css">
</head>
<body>
    <form id="form1" runat="server" height="100" width="100">
    <div>
        <table width="50%" border="0" cellpadding="0" cellspacing="0">
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
                                                    <td width="8" height="8" valign="top">
                                                        <img src="../images/mainbox-lt.png" width="8" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                    <td align="left" bgcolor="#f5f5f5">
                                                        <img src="../images/spacer.gif" width="1" height="1" border="0" />
                                                    </td>
                                                    <td width="8" height="8" valign="top">
                                                        <img src="../images/mainbox-rt.png" width="8" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top" bgcolor="#f5f5f5">
                                                        &nbsp;
                                                    </td>
                                                    <td align="left" bgcolor="#f5f5f5">
                                                        <a href="javascript:void(0)">
                                                            <img src="../images/logo-IQMediaGroup.jpg" alt="iQMedia" width="245" height="71"
                                                                hspace="9" vspace="11" border="0" /></a>
                                                    </td>
                                                    <td width="8" valign="top" bgcolor="#f5f5f5">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td bgcolor="#f5f5f5">
                                            <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="8">
                                                        <img src="../Images/contentbox-lt.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                    <td class="contentbox-t">
                                                        <img src="../Images/contentbox-t.jpg" width="1" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                    <td width="8">
                                                        <img src="../Images/contentbox-rt.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="contentbox-l">
                                                        <img src="../Images/contentbox-l.jpg" width="8" height="1" border="0" alt="iQMedia" />
                                                    </td>
                                                    <td valign="top" bgcolor="#FFFFFF">
                                                        <table width="99%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td height="2">
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td width="3">
                                                                                <img id="Img8" runat="server" src="~/Images/bluebox-hd-left.jpg" width="3" height="24"
                                                                                    border="0" alt="iQMedia" />
                                                                            </td>
                                                                            <td align="center" class="bluebox-hd">
                                                                                Need help logging on?
                                                                                <br />
                                                                            </td>
                                                                            <td width="3">
                                                                                <img id="Img9" runat="server" src="~/Images/bluebox-hd-right.jpg" width="3" height="24"
                                                                                    border="0" alt="iQMedia" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <img id="Img10" runat="server" src="~/Images/spacer.gif" width="1" height="10" border="0"
                                                                        alt="iQMedia" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td class="content-text-blue" style="padding: 0px 10px;">
                                                                    <p>
                                                                        Your password must be at least six characters and at most fifteen characters, with
                                                                        any combination of letters and numbers.</p>
                                                                    <p>
                                                                        User Name and Password are case sensitive.</p>
                                                                    <p>
                                                                        By pressing Forgot Password link you will get an Email which contains your password.<br />
                                                                    </p>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <a href="javascript:window.close();">
                                                                        <asp:Button ID="btnOK" CssClass="login-btn" runat="server" Width="51px" Text="OK">
                                                                        </asp:Button></a>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td class="contentbox-r">
                                                        <img src="../Images/contentbox-r.jpg" width="8" height="1" border="0" alt="iQMedia" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="8">
                                                        <img src="../Images/contentbox-lb.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                    <td class="contentbox-b">
                                                        <img src="../Images/contentbox-b.jpg" width="1" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                    <td>
                                                        <img src="../Images/contentbox-rb.jpg" width="8" height="8" border="0" alt="iQMedia" />
                                                    </td>
                                                </tr>
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
                                                    <td bgcolor="#f5f5f5">
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
        </table>
    </div>
    </form>
</body>
</html>
