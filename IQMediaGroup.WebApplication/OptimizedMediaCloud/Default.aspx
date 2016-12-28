<%@ Page Title="Learn about cliQs optimized media cloud" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.OptimizedMediaCloud.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/OptimizedMediaCloud/OptimizedMediaCloud.ascx"
    TagName="OptimizedMediaCloud" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="An optimized media cloud is the essential foundation for a media intelligence platform, which delivers new capabilities beyond traditional TV media monitoring" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, optimized media cloud, lakshmi algorithm, cliQ, iQ media, video hosting, media monitors, social media monitoring, online new monitoring, twitter monitoring" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:OptimizedMediaCloud ID="ucOptimizedMediaCloud" runat="server" />
</asp:Content>
