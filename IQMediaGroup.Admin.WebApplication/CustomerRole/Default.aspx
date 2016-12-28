<%@ Page Title="iQMedia :: Customer Role" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.CustomerRole.Default" %>
<%@ Register src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx" tagname="IQAdminNavigationPanel" tagprefix="uc1" %>
<%@ Register src="../UserControl/CustomerRole/CustomerRole.ascx" tagname="CustomerRole" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
<table cellpadding="0" cellspacing="0" width="100%" align="right">
            <tr>
                <td>
                    <uc2:CustomerRole ID="CustomerRole1" runat="server" />
                </td>
            </tr>
        </table>
</asp:Content>
