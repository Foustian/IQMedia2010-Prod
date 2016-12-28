<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RightTopLogin.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.RightTopLogin.RightTopLogin" %>
<table border="0" align="right" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <img src="~/Images/topnav-left.jpg" id="imgTopOne" runat="server" visible="false"
                width="39" height="31" border="0" alt="iQMedia" />
        </td>
        <td class="topnav">
            <asp:LinkButton ID="lnkLogin" runat="server" Text="Logout" Visible="false" OnClick="lnkLogin_Click"></asp:LinkButton>
            <%--<asp:LinkButton ID="lnkbtnGlobalAdmin" runat="server" Enabled="false" Visible="false"
                Text="| iQ Global Setting" OnClick="lnkbtnGlobalAdmin_Click"></asp:LinkButton>--%>
        </td>
        <td>
            <img src="~/Images/topnav-right.jpg" id="imgTopTwo" runat="server" visible="false"
                width="5" height="31" border="0" alt="iQMedia" />
        </td>
        <td width="10" runat="server" id="tdImage" visible="false">
            &nbsp;
        </td>
    </tr>
</table>
