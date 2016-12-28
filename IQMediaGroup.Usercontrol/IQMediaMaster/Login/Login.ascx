<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.Login.Login" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%--<asp:UpdatePanel ID="upIQMediaLogin" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
<asp:Panel ID="pnlbtnSubmit" runat="server" DefaultButton="btnSubmit" CssClass="header-right">
    <span style="width: 100px; float: left;">Client Login </span>
    <asp:Label ID="lblError" runat="server" Visible="False" Font-Size="Smaller" ForeColor="Red"></asp:Label>
    <AjaxToolkit:FilteredTextBoxExtender ID="ftbUserName" runat="server" TargetControlID="txtUserName"
        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" FilterMode="InvalidChars"
        InvalidChars="<>">
    </AjaxToolkit:FilteredTextBoxExtender>
    <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtUserName" TargetControlID="txtUserName"
        WatermarkText="username" runat="server">
    </AjaxToolkit:TextBoxWatermarkExtender>
    <cc1:PropertyProxyValidator ID="pplEmail" runat="server" ControlToValidate="txtUserName"
        Text="*" PropertyName="Email" SourceTypeName="IQMediaGroup.Core.HelperClasses.Customer"
        RulesetName="IQMediaLogin" ValidationGroup="IQMediaLogin" Font-Size="Smaller" SetFocusOnError="true"
        Display="None"></cc1:PropertyProxyValidator>
    <cc1:PropertyProxyValidator ID="ppvForgotPassword" runat="server" ControlToValidate="txtUserName"
        Text="*" PropertyName="Email" SourceTypeName="IQMediaGroup.Core.HelperClasses.Customer"
        Display="None" RulesetName="IQMediaForgotPassword" ValidationGroup="IQMediaForgotPassword" SetFocusOnError="true"
        Font-Size="Smaller"></cc1:PropertyProxyValidator>
    <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtPassword" TargetControlID="txtPassword"
        WatermarkText="password" runat="server">
    </AjaxToolkit:TextBoxWatermarkExtender>
    <AjaxToolkit:FilteredTextBoxExtender ID="ftbPassword" runat="server" TargetControlID="txtPassword"
        FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" FilterMode="InvalidChars"
        InvalidChars="<>">
    </AjaxToolkit:FilteredTextBoxExtender>
    <cc1:PropertyProxyValidator ID="ppvPassword" runat="server" ControlToValidate="txtPassword" SetFocusOnError="true"
        Text="*" Display="None" PropertyName="Password" SourceTypeName="IQMediaGroup.Core.HelperClasses.Customer"
        RulesetName="IQMediaLogin" ValidationGroup="IQMediaLogin" Font-Size="Smaller"></cc1:PropertyProxyValidator>
    <AjaxToolkit:ValidatorCalloutExtender ID="vcepplEmail" runat="server" TargetControlID="pplEmail">
    </AjaxToolkit:ValidatorCalloutExtender>
    <AjaxToolkit:ValidatorCalloutExtender ID="vceppvPassword" runat="server" TargetControlID="ppvPassword">
    </AjaxToolkit:ValidatorCalloutExtender>
    <div style="clear: both;">
        <asp:TextBox ID="txtUserName" runat="server" TabIndex="1"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ControlToValidate="txtUserName" ValidationGroup="IQMediaLogin" SetFocusOnError="true"
            Display="None" ErrorMessage="UserName"></asp:RequiredFieldValidator>
            <AjaxToolkit:ValidatorCalloutExtender ID="vcerfvtxtUserName" runat="server" TargetControlID="rfvtxtUserName">
    </AjaxToolkit:ValidatorCalloutExtender>
        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="second"  style="font-family:Arial"
            TabIndex="2"></asp:TextBox>
        <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ControlToValidate="txtPassword" ValidationGroup="IQMediaLogin" ErrorMessage="Password" SetFocusOnError="true"
            Display="None"></asp:RequiredFieldValidator>
             <AjaxToolkit:ValidatorCalloutExtender ID="vcerfvtxtPassword" runat="server" TargetControlID="rfvtxtPassword">
    </AjaxToolkit:ValidatorCalloutExtender>
        <asp:Button ID="btnSubmit" runat="server" CssClass="submit" Text="Login" ValidationGroup="IQMediaLogin"
            OnClick="btnSubmit_Click" TabIndex="3" CausesValidation="true" />
    </div>
</asp:Panel>
<%--</ContentTemplate>
</asp:UpdatePanel>--%>
<%--<asp:UpdateProgress ID="updProgressLogin" runat="server" AssociatedUpdatePanelID="upIQMediaLogin">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<%--<cc2:UpdateProgressOverlayExtender runat="server" ID="upoeHome" ControlToOverlayID="upIQMediaLogin"
    TargetControlID="updProgressLogin" CssClass="updateProgressLogin" />--%>
