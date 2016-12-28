<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyCliqMediaLogin.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.MyCliqMediaLogin.MyCliqMediaLogin" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<asp:Panel ID="pnlbtnSubmit" runat="server" DefaultButton="btnLogIn">
    <table width="100%" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <h2 class="loginhd center">
                    Login</h2>
            </td>
        </tr>
        <tr style="height:15px;">
            <td>
                <asp:Label ID="lblError" runat="server" ForeColor="Red" Visible="false" Font-Size="12px"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtUserName" runat="server" CssClass="styled-input-135 username-input"
                    TabIndex="1"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtUserName" runat="server" ErrorMessage="User name can not be blank"
                    Display="None" ForeColor="Red" ControlToValidate="txtUserName" ValidationGroup="Login"></asp:RequiredFieldValidator>
                <AjaxToolkit:ValidatorCalloutExtender runat="Server" ID="vcerfvtxtUserName" TargetControlID="rfvtxtUserName"
                    Width="350px" PopupPosition="Right" />
                <Ajax:FilteredTextBoxExtender ID="ftbUserName" runat="server" TargetControlID="txtUserName"
                    FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" FilterMode="InvalidChars"
                    InvalidChars="<>">
                </Ajax:FilteredTextBoxExtender>
                <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtUserName" TargetControlID="txtUserName"
                    WatermarkText="username" runat="server">
                </AjaxToolkit:TextBoxWatermarkExtender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="styled-input-135 password-input"
                    TabIndex="2"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvtxtPassword" runat="server" ErrorMessage="Password can not be blank"
                    Display="None" ForeColor="Red" ControlToValidate="txtPassword" ValidationGroup="Login"></asp:RequiredFieldValidator>
                <AjaxToolkit:ValidatorCalloutExtender runat="Server" ID="vcerfvtxtPassword" TargetControlID="rfvtxtPassword"
                    Width="350px" PopupPosition="Right" />
                <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtPassword" TargetControlID="txtPassword"
                    WatermarkCssClass="styled-input-135 password-input" WatermarkText="password"
                    runat="server">
                </AjaxToolkit:TextBoxWatermarkExtender>
                <Ajax:FilteredTextBoxExtender ID="ftbPassword" runat="server" TargetControlID="txtPassword"
                    FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom" FilterMode="InvalidChars"
                    InvalidChars="<>">
                </Ajax:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr>
            <td>
                <asp:ImageButton ID="btnLogIn" runat="server" CssClass="loginbtn" Text="Login" ValidationGroup="Login"
                    Width="22px" Height="22px" OnClick="btnLogIn_Click" TabIndex="3" ImageUrl="/MyCliqMedia/Images/btn-go.png" />
            </td>
        </tr>
    </table>
</asp:Panel>
