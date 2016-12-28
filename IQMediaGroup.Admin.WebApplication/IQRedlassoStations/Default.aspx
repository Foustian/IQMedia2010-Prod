<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/IQMediaAdminContent.Master" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.IQRedlassoStations.Default" %>
<%@ Register src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx" tagname="IQAdminNavigationPanel" tagprefix="uc2" %>


<%@ Register src="../UserControl/IQRedlassoStations/IQRedlassoStations.ascx" tagname="IQRedlassoStations" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">

    <link href="../Css/flexcrollstyles.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="../Script/flexcroll.js"></script>
        <table cellpadding="0" cellspacing="0" width="100%" align="right">
            <tr>
                <td width="200" valign="top">
                     <uc2:IQAdminNavigationPanel ID="IQAdminNavigationPanel1" runat="server" />
                 </td>
                <td>
                    <uc1:IQRedlassoStations ID="IQRedlassoStations1" runat="server" />
                </td>
            </tr>
        </table>
    
</asp:Content>