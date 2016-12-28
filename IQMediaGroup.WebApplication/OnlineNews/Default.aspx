<%@ Page Title="Monitoring Access to Online News from 50,000 News Sources" Language="C#"
    MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs"
    Inherits="IQMediaGroup.WebApplication.OnlineNews.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/OnlineNews/OnlineNews.ascx" TagName="OnlineNews"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn how to access over 50,000 online news sources to monitor media mentions and stay on top of coverage of your brand" />
    <meta name="keywords" content="media intelligence platform, online news monitoring, media monitoring, cliQ, monitor online news coverage, media monitors, iQ Media" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:OnlineNews ID="ucOnlineNews" runat="server" />
</asp:Content>
