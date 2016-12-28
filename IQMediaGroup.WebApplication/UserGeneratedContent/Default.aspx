<%@ Page Title="Learn How to Combine User Generated Video Content with Broadcast TV"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.UserGeneratedContent.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/UserGeneratedContent/UserGeneratedContent.ascx"
    TagName="UserGeneratedContent" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Integrate your user generated content videos into a media intelligence platform, where they can be combined with fair use broadcast TV clips to power your outbound communications, which is more than a media monitoring service can do" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, user generated video content, media monitoring, cliQ, search TV media content, monitor television coverage, TV video clips, media monitors, iQ Media, video playout, video clip hosting, video streaming" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:UserGeneratedContent ID="ucUserGeneratedContent" runat="server" />
</asp:Content>
