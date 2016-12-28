<%@ Page Language="C#" MasterPageFile="~/IQMediaGroupResponsive.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IQPremium.Default"
    Title="iQMedia :: iQ Premium" %>

<%@ Register Src="~/UserControl/IQMediaMaster/IQPremium/IQPremium.ascx" TagName="IQPremium"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Css/Paging.css" />
  <link href="../Css/tab.css" rel="stylesheet" type="text/css" />
    <%--<link href="../Css/tab.css" rel="stylesheet" type="text/css" />--%>
    <%--<script src="../Script/jquery-ui.min.js" type="text/javascript"></script>--%>
<%--    <script src="../Script/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>--%>
  
    <%--<script src="js/jquery.min.js" type="text/javascript"></script>--%>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    
    
    <uc:IQPremium ID="ucIQpremium" runat="server" />
    <%--<script src="../FusionChart/js/FusionCharts.js" type="text/javascript"></script>--%>
    <script src="../FusionChart/js/FusionChartsExportComponent.js" type="text/javascript"></script>
    <script src="../Script/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
    <script src="../Script/IQPremiumScript.js?ver=1.1" type="text/javascript"></script>
    
</asp:Content>
