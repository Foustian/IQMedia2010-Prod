<%@ Page Title="iQMedia :: Client Role" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.ClientRole.Default" %>

<%@ Register Src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx"
    TagName="IQAdminNavigationPanel" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/ClientRole/ClientRole.ascx" TagName="ClientRole"
    TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td>
                <uc2:ClientRole ID="ClientRoleControl" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
