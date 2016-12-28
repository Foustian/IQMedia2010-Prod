<%@ Page Title="iQMedia :: Customer Registration" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.CustomerRegistration.Default" %>
<%@ Register src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx" tagname="IQAdminNavigationPanel" tagprefix="uc1" %>
<%@ Register src="../UserControl/CustomerRegistration/CustomerRegistration.ascx" tagname="CustomerRegistration" tagprefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td>
                <uc2:CustomerRegistration ID="CustomerRegistration1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
