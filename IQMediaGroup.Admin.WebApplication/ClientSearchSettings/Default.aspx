<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/IQMediaAdminContent.Master" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.ClientSearchSettings.Default" %>
<%@ Register src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx" tagname="IQAdminNavigationPanel" tagprefix="uc1" %>
<%@ Register src="../UserControl/ClientSearchSettings/ClientSearchSettings.ascx" tagname="ClientSearchSettings" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td>
                <uc2:ClientSearchSettings ID="ClientSearchSettings1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
