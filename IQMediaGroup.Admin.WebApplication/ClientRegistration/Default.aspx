<%@ Page Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.ClientRegistration.Default"
    Title="iQMedia :: Client Registration"%>

<%@ Register Src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx"
    TagName="IQAdminNavigationPanel" TagPrefix="uc2" %>
<%@ Register Src="../UserControl/ClientRegistration/ClientRegistration.ascx" TagName="ClientRegistration"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <%--<script type="text/javascript" src="../Script/jquery-1.2.6.js"></script>
    <script type="text/javascript" src="../Script/ui.core.js"></script>
    <script type="text/javascript" src="../Script/ui.combobox.js"></script>
    <link rel = "stylesheet" type = "text/css" href = "../Css/ui.combobox.css" />
--%>
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td>
                <uc1:ClientRegistration ID="ClientRegistration1" runat="server" />
            </td>
        </tr>
    </table>
  
</asp:Content>
