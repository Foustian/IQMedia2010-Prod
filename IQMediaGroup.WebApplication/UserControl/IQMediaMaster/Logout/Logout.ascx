<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Logout.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.Logout.Logout" %>
<div class="header-rightLogOut">
    <asp:LinkButton ID="lbtnLogout" runat="server" CssClass="logout" Text="Logout" OnClick="lbtnLogout_Click"></asp:LinkButton>&nbsp;
    <asp:LinkButton ID="lbtnRMyiq" runat="server" CssClass="return-iq" Text="Return to myiQ"
        OnClick="lbtnRMyiq_Click"></asp:LinkButton>
</div>
