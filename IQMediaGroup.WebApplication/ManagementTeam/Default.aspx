<%@ Page Title="iQ media executives originated the media intelligence platform" Language="C#"
    MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="IQMediaGroup.WebApplication.ManagementTeam.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/ManagementTeam/ManagementTeam.ascx"
    TagName="ManagementTeam" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="iQ media executives combine broad industry expertise to deliver the industry's first media intellingence platform, the next generation beyond media monitoring for broadcast TV, online news, and social media" />
    <meta name="keywords" content="media intelligence platform, iq media executives, iq media managment team, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, TV video clips, media monitors, media monitoring services, video hosting, iq media, media platform technology, fair use video content, video streaming, cliq" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:ManagementTeam ID="ucManagementTeam" runat="server" />
</asp:Content>
