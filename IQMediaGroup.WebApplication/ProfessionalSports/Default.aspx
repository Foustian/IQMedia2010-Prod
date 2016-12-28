<%@ Page Title="Media Intelligence Platform for Professional Sports Teams - Beyond Media Monitoring"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.ProfessionalSports.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/ProfessionalSports/ProfessionalSports.ascx"
    TagName="ProfessionalSports" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn how a media intelligence platform is helping professional sports teams to improve on media monitoring to better project their brand and market to corporate sponsors" />
    <meta name="keywords" content="media intelligence platform for professional sports teams, broadcast TV monitoring, media monitoring, media monitoring in politics, social media monitoring, twitter monitoring, tweet monitoring, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, fair use video content, video streaming, media PR for professional sports" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:ProfessionalSports ID="ucProfessionalSports" runat="server" />
</asp:Content>
