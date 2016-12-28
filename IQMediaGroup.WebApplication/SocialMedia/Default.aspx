<%@ Page Title="Learn About Monitoring Social Media Platforms" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.SocialMedia.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/SocialMedia/SocialMedia.ascx" TagName="SocialMedia"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Discover how to monitor social media platforms to stay on top of the conversation relating to your brand and your message" />
    <meta name="keywords" content="media intelligence platform, social media monitoring, media monitoring, cliQ, monitor social media, media monitors, iQ Media" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:SocialMedia ID="ucSocialMedia" runat="server" />
</asp:Content>
