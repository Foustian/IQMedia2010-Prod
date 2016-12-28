<%@ Page Title="How Media Monitoring Companies Can Access a Media Intelligence Platform"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.OEMPartnerships.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/OEMPartnerships/OEMPartnerships.ascx"
    TagName="OEMPartnerships" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Companies that serve the media monitoring industry can now embed media intelligence platform technology into their product offerings which enables them to access iQ media's optimized media cloud" />
    <meta name="keywords" content="media intelligence platform, oprimized media cloud, broadcast TV monitoring, media monitoring, broadcast TV capture, TV video clips, media monitors, media monitoring services, video hosting, iq media oem program, media platform technology, fair use video content, video streaming, cliq" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:OEMPartnerships ID="ucOEMPartnerships" runat="server" />
</asp:Content>
