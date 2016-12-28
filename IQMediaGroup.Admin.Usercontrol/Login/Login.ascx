<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Login.ascx.cs" Inherits="IQMediaGroup.Admin.Usercontrol.Login.Login" %>
<asp:Panel ID="pnlLogin" DefaultButton="btnLogin" runat="server">
    <table width="100%" border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td height="610">
                <table border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="center" valign="top"><img runat="server"  src="~/images/logo.png" width="239" height="67" vspace="20" border="0" /></td>
                    </tr>
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td><img runat="server"  src="~/images/login-bg-top.jpg" width="324" height="10" border="0" /></td>
                                </tr>
                                <tr>
                                    <td valign="top" class="bg-login">
                                        <table width="92%" border="0" align="center" cellpadding="2" cellspacing="6">
                                            <tr>
                                                <td align="center" class="text02">
                                                    Enter User Name and Password for Login
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" class="errormsg">
                                                    <asp:ValidationSummary ID="vsErrors" EnableClientScript="true" CssClass="errormsg"
                                                        runat="server" ValidationGroup="validate" />
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:Label ID="lblError" runat="server" Visible="False" CssClass="errormsg"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="bg-login-btm">
                                        <table width="92%" border="0" align="center" cellpadding="2" cellspacing="6">
                                            <tr>
                                                <td width="90px" align="right" class="text01">
                                                    Username :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="textbox01" Width="125px"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvUserName" Text="*" Display="Dynamic" ValidationGroup="validate"
                                                        runat="server" ControlToValidate="txtUserName" ErrorMessage="Please Enter User Name."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" class="text01">
                                                    Password :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="textbox01" Width="125px" TextMode="Password"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvPassword" Text="*" Display="Dynamic" ValidationGroup="validate"
                                                        runat="server" ControlToValidate="txtPassword" ErrorMessage="Please Enter Password."></asp:RequiredFieldValidator>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnLogin" runat="server" Text="Login Now!" ValidationGroup="validate"
                                                        CssClass="btn01" OnClick="btnLogin_Click" />
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
</asp:Panel>
