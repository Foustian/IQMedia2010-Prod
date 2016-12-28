<%@ Page Title="Learn How to Monitor and Leverage Broadcast TV" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.BroadcastTV.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/BroadcastTV/BroadcastTV.ascx" TagName="BroadcastTV"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Discover the difference between TV media monitoring and a media intelligence platform for working with fair use TV content" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, cliQ, search TV media content, monitor television coverage, TV video clips, media monitors, iQ Media, hosting video clips, streaming video clips" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:BroadcastTV ID="ucBroadcastTV" runat="server" />
</asp:Content>
