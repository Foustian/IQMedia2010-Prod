<%@ Page Title="iQMedia :: IQRole" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.IQRole.Default" %>

<%@ Register Src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx"
    TagName="IQAdminNavigationPanel" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/IQRole/IQRole.ascx" TagName="IQRole" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td>
                <uc1:IQRole ID="IQRole1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
