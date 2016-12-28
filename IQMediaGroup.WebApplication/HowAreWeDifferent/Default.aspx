<%@ Page Title="How iQ media differs from media monitoring services" Language="C#"
    MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="IQMediaGroup.WebApplication.HowAreWeDifferent.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/HowAreWeDifferent/HowAreWeDifferent.ascx"
    TagName="HowAreWeDifferent" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="iQ media has delivered new technology, a media intelligence platform with an optimized media cloud and an inline media workspace, that revolutionizes the market for media monitoring of broadcast TV, online news, and social media" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, media monitoring services, video hosting, iq media, media platform technology, fair use video content, video streaming, cliq" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:HowAreWeDifferent ID="ucHowAreWeDifferent" runat="server" />
</asp:Content>
