<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="LoginIframe.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.LoginIframe" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<asp:UpdatePanel ID="upIQMediaLogin" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <table width="100%">
            <tr>
                <td>
                    <div>
                        <table width="100%" border="0" cellspacing="3" cellpadding="0">
                            <tr>
                                <td align="left" valign="top">
                                    <asp:ValidationSummary ForeColor="#bd0000" DisplayMode="BulletList" ID="vsErrors"
                                        EnableClientScript="true" runat="server" ValidationGroup="IQMediaLogin" Font-Size="Smaller" />
                                    <asp:ValidationSummary ForeColor="#bd0000" ID="ValidationSummary1" EnableClientScript="true"
                                        runat="server" ValidationGroup="IQMediaForgotPassword" Font-Size="Smaller" />
                                    <asp:Label ID="lblError" runat="server" Visible="False" Font-Size="Smaller" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <asp:Panel ID="pnlbtnSubmit" runat="server" DefaultButton="btnSubmit">
                        <div>
                            <asp:TextBox ID="txtUserName" runat="server" CssClass="styled-input-135 username-input"
                                TabIndex="1"></asp:TextBox>
                            <Ajax:FilteredTextBoxExtender ID="ftbUserName" runat="server" TargetControlID="txtUserName"
                                FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" FilterMode="InvalidChars"
                                InvalidChars="<>">
                            </Ajax:FilteredTextBoxExtender>
                            <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtUserName" TargetControlID="txtUserName"
                                WatermarkCssClass="styled-input-135 username-input" WatermarkText="username"
                                runat="server">
                            </AjaxToolkit:TextBoxWatermarkExtender>
                            <cc1:PropertyProxyValidator ID="pplEmail" runat="server" ControlToValidate="txtUserName"
                                Text="*" PropertyName="Email" SourceTypeName="IQMediaGroup.Core.HelperClasses.Customer"
                                RulesetName="IQMediaLogin" ValidationGroup="IQMediaLogin" Font-Size="Smaller"
                                Display="Dynamic"></cc1:PropertyProxyValidator>
                            <cc1:PropertyProxyValidator ID="ppvForgotPassword" runat="server" ControlToValidate="txtUserName"
                                Text="*" PropertyName="Email" SourceTypeName="IQMediaGroup.Core.HelperClasses.Customer"
                                Display="Dynamic" RulesetName="IQMediaForgotPassword" ValidationGroup="IQMediaForgotPassword"
                                Font-Size="Smaller"></cc1:PropertyProxyValidator>                          
                        </div>
                        <div>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"
                                CssClass="styled-input-135 password-input" TabIndex="2"></asp:TextBox>
                            <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtPassword" TargetControlID="txtPassword"
                                WatermarkCssClass="styled-input-135 password-input" WatermarkText="password"
                                runat="server">
                            </AjaxToolkit:TextBoxWatermarkExtender>
                            <Ajax:FilteredTextBoxExtender ID="ftbPassword" runat="server" TargetControlID="txtPassword"
                                FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" FilterMode="InvalidChars"
                                InvalidChars="<>">
                            </Ajax:FilteredTextBoxExtender>
                            <cc1:PropertyProxyValidator ID="ppvPassword" runat="server" ControlToValidate="txtPassword"
                                Text="*" Display="Dynamic" PropertyName="Password" SourceTypeName="IQMediaGroup.Core.HelperClasses.Customer"
                                RulesetName="IQMediaLogin" ValidationGroup="IQMediaLogin" Font-Size="Smaller"></cc1:PropertyProxyValidator>
                            <asp:Button ID="btnSubmit" runat="server" CssClass="login-button" Text="Login" ValidationGroup="IQMediaLogin"
                                OnClick="btnSubmit_Click" TabIndex="3" />
                            <%-- <input type="checkbox" id="remember-me-checkbox" value="1" />--%>                           
                        </div>
                    </asp:Panel>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="updProgressLogin" runat="server" AssociatedUpdatePanelID="upIQMediaLogin">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upoeHome" ControlToOverlayID="upIQMediaLogin"
    TargetControlID="updProgressLogin" CssClass="updateProgressLogin" />
