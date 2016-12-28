<%@ Page Title="Learn about a media intelligence platform named cliQ"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.cliQ.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/cliQ/cliQ.ascx" TagName="cliQ" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="A media intelligence platform is the next generation beyond simple media monitoring services, and cliQ delivers new capabilities for working with broadcast TV, online news, and social media" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, search TV media content, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, media platform technology, fair use video content, video streaming, optimized media cloud, inline media workspace, my iQ, enterprise media center" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:cliQ ID="uccliQ" runat="server" />
</asp:Content>
