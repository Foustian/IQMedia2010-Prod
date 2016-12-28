<%@ Page Title="" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.IQRedlassoStationMarket.Default" %>

<%@ Register Src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx"
    TagName="IQAdminNavigationPanel" TagPrefix="uc1" %>
<%@ Register Src="../UserControl/RedlassoStationMarket/RedlassoStationMarket.ascx"
    TagName="RedlassoStationMarket" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <link href="../Css/flexcrollstyles.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../Script/flexcroll.js"></script>

    <table cellpadding="0" cellspacing="0" width="100%" align="center">
        <tr>
            <%--<td width="200" valign="top">
                <uc1:IQAdminNavigationPanel ID="IQAdminNavigationPanel1" runat="server" />
            </td>--%>
            <td align="center">
                <uc2:RedlassoStationMarket ID="RedlassoStationMarket1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
