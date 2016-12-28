<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TopPanel.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.TopPanel.TopPanel" %>
<%@ Register Src="../Login/Login.ascx" TagName="Login" TagPrefix="uc" %>
<%@ Register Src="../NavigationPanel/NavigationPanel.ascx" TagName="NavigationPanel"
    TagPrefix="uc1" %>
<%@ Register Src="../Logout/Logout.ascx" TagName="Logout" TagPrefix="uc2" %>
<header>
<div class="logo">
    <a runat="server" id="anchorHome" href="~/">
        <img id="imgLogo" runat="server" src="~/images/logo_N.png" alt="" /></a></div>
<uc:Login ID="ucLogin" runat="server" />

<uc2:Logout ID="ucLogout" runat="server" />
<uc1:NavigationPanel ID="ucNavigationPanel" runat="server"></uc1:NavigationPanel>
<input type="button" id="tgtBtn" runat="server" style="display: none;" />
<AjaxToolkit:ModalPopupExtender ID="mpESessionOut" DropShadow="false" BackgroundCssClass="ModalBackgroundLightBox"
    OkControlID="btnSessionTimeOut" Drag="true" TargetControlID="tgtBtn" PopupControlID="pnlMsg"
    runat="server">
</AjaxToolkit:ModalPopupExtender>
<asp:Panel CssClass="ModalPopup" ID="pnlMsg" Style="display: none;" runat="server"
    Width="300px">
    <div style="padding: 5px;">
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
                                <img id="Img1" src="~/Images/bluebox-hd-left.jpg" runat="server" width="3" height="24" border="0"
                                    alt="iQMedia" />
                            </td>
                            <td align="left" class="bluebox-hd">
                                Message
                                <br />
                            </td>
                            <td width="3">
                                <img id="Img2" src="~/Images/bluebox-hd-right.jpg" runat="server" width="3" height="24" border="0"
                                    alt="iQMedia" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <img id="Img3" src="~/Images/spacer.gif" runat="server" width="1" height="10" border="0" alt="iQMedia" />
                </td>
            </tr>
            <tr>
                <td class="content-text-blue" style="padding: 0px 10px;">
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td align="center">
                    <%--<asp:Button ID="btnSessionTimeOut" runat="server" CssClass="login-btn" Text="Ok"
                        OnClick="btnSessionTimeOut_Click" />--%>
                    <input type="button" id="btnSessionTimeOut" value="Ok" class="btn-blue2" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
</header>
