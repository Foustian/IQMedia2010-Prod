<%@ Page Title="Media Intelligence Platform for Political Organizations - Beyond Media Monitoring"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.PoliticalParties.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/PoliticalParties/PoliticalParties.ascx"
    TagName="PoliticalParties" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn how a media intelligence platform is helping political parties to improve on media monitoring to better project their brand and control their message" />
    <meta name="keywords" content="media intelligence platform for polictcal parties, broadcast TV monitoring, media monitoring, media monitoring in politics, social media monitoring, twitter monitoring, tweet monitoring, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, fair use video content, video streaming" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc1:PoliticalParties ID="ucPoliticalParties" runat="server" />
</asp:Content>
