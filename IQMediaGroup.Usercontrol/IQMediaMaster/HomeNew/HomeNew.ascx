<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HomeNew.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.HomeNew.HomeNew" %>
<%@ Register Src="../SocialNetworkingWebsitesPanel/SocialNetworkingWebsitesPanel.ascx"
    TagName="SocialNetworkingWebsitesPanel" TagPrefix="uc4" %>
<%@ Register Src="../SearchSite/SearchSite.ascx" TagName="SearchSite" TagPrefix="uc5" %>
<%@ Register Src="../NavigationPanel/NavigationPanel.ascx" TagName="NavigationPanel"
    TagPrefix="uc3" %>
<%@ Register Src="../Login/Login.ascx" TagName="Login" TagPrefix="uc1" %>
<%@ Register Src="../HeaderRightPanel/HeaderRightPanel.ascx" TagName="HeaderRightPanel"
    TagPrefix="uc2" %>
<%@ Register Src="../FooterPanel/FooterPanel.ascx" TagName="FooterPanel" TagPrefix="uc6" %>
<%@ Register Src="../Logout/Logout.ascx" TagName="Logout" TagPrefix="uc7" %>
<%@ Register Src="../IQMediaMessenger/IQMediaMessenger.ascx" TagName="IQMediaMessenger"
    TagPrefix="uc8" %>
<%@ Register Src="../OurProduct/OurProduct.ascx" TagName="OurProduct" TagPrefix="uc9" %>
<div class="header">
    <div class="header-inner">
        <div class="top">
            <div class="login-form">
                <uc1:Login ID="ucLogin" runat="server" />
                <uc7:Logout ID="ucLogout" runat="server" />
            </div>
            <div class="top-contacts">
                <uc2:HeaderRightPanel ID="HeaderRightPanel1" runat="server" />
            </div>
        </div>
        <div class="banner">
            <img src="images/banner.jpg" height="223" width="801" alt="" />
            <%--<div class="left-glowing">
                <!-- -->
            </div>
            <div class="bottom-glowing">
                <!-- -->
            </div>
            <div class="right-glowing">
                <!-- -->
            </div>--%>
        </div>
        <uc3:NavigationPanel ID="NavigationPanel1" runat="server" />
    </div>
</div>
<div class="content">
    <div class="top-line">
        <div class="fleft">
            <uc5:SearchSite ID="SearchSite1" runat="server" />

        </div>
        <div class="fright" style="margin-right: 28px;">
            <uc4:SocialNetworkingWebsitesPanel ID="SocialNetworkingWebitesPanel1" runat="server" />
        </div>
    </div>
    <div class="columns-wrapper">
        <div class="block right-bottom left-column">
            <div class="block-inner">
                <div class="block-bottom">
                    <!-- -->
                </div>
                <div class="block-bottom-end">
                    <!-- -->
                </div>
                <div class="block-right-end">
                    <!-- -->
                </div>
                <div class="block-rb-corner">
                    <!-- -->
                </div>
                <div class="block-content msgpad">
                    <uc9:OurProduct ID="ourProduct1" runat="server" />
                </div>
            </div>
        </div>
        <div class="block right-bottom right-column">
            <div class="block-inner">
                <div class="block-bottom">
                    <!-- -->
                </div>
                <div class="block-bottom-end">
                    <!-- -->
                </div>
                <div class="block-right-end">
                    <!-- -->
                </div>
                <div class="block-rb-corner">
                    <!-- -->
                </div>
                <div class="block-content">
                    <h2>
                        Our Services</h2>
                    <div class="services-wrapper">
                        <div class="service">
                            <p>
                                iQ Media</p>
                            <a href="~/About/" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image10','','images/service1_rollover.jpg',1)">
                                <img src="images/service1.jpg" alt="iQ Media" width="167" height="114" border="0"
                                    class="image-border" id="Image10" /></a>
                        </div>
                        <div class="service">
                            <p>
                                My iQ</p>
                            <a href="~/Service2/" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image9','','images/service2_rollover.jpg',1)">
                                <img src="images/service2.jpg" alt="My iQ" width="167" height="114" border="0" class="image-border"
                                    id="Image9" /></a></div>
                        <div class="service">
                            <a href="~/Services/" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image8','','images/service3_rollover.jpg',1)">
                                <img src="images/service3.jpg" alt="iQ Media Services" width="167" height="114" border="0"
                                    class="image-border" id="Image8" /></a>
                            <p>
                                iQ Media Services</p>
                        </div>
                        <div class="service">
                            <a href="~/Service3/" runat="server" onmouseout="MM_swapImgRestore()" onmouseover="MM_swapImage('Image7','','images/service4_rollover.jpg',1)">
                                <img src="images/service4.jpg" alt="IQ Media Innovation" width="167" height="114"
                                    class="image-border" border="0" id="Image7" /></a>
                            <p>
                                iQ Media Innovation</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="content-text">
        <p>
            iQ Media&#0153; offers you the most intelligent way to monitor broadcast television
            and radio media content. Our innovative and easy-to-use platform empowers you to
            quickly find the media you are looking for without costly delays. We have leveraged
            some of the best technology and delivery systems available to bring you the finest
            product in the market.</p>
    </div>
</div>
<uc6:FooterPanel ID="FooterPanel1" runat="server" />
<input type="button" id="tgtBtn" runat="server" style="display: none;" />
<AjaxToolkit:ModalPopupExtender ID="mpESessionOut" DropShadow="false" BackgroundCssClass="ModalBackground"
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
                                <img src="~/Images/bluebox-hd-left.jpg" runat="server" width="3" height="24" border="0"
                                    alt="iQMedia" />
                            </td>
                            <td align="left" class="bluebox-hd">
                                Message
                                <br />
                            </td>
                            <td width="3">
                                <img src="~/Images/bluebox-hd-right.jpg" runat="server" width="3" height="24" border="0"
                                    alt="iQMedia" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <img src="~/Images/spacer.gif" runat="server" width="1" height="10" border="0" alt="iQMedia" />
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
                    <input type="button" id="btnSessionTimeOut" value="Ok" class="login-btn" runat="server" />
                </td>
            </tr>
        </table>
    </div>
</asp:Panel>
