<%@ Page Title="Learn about cliQ's inline media workspace" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.InlineMediaWorkspace.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/InlineMediaWorkspace/InlineMediaWorkspace.ascx"
    TagName="InlineMediaWorkspace" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="An inline media workspace empowers users of a media intelligence platform to monitor media by finding coverage more quickly and repurposing it more easily than with a traditional media monitoring service" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, inline media workspace, cliQ, iQ media, video hosting, media monitors, social media monitoring, online new monitoring, twitter monitoring, video clipping service, fair use video clips" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:InlineMediaWorkspace ID="ucInlineMediaWorkspace" runat="server" />
</asp:Content>
