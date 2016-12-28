<%@ Page Title="Learn about Proprep - the media monitor for broadcast professionals"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.PropPrep.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/ProPrep/ProPrep.ascx" TagName="ProPrep"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="ProPrep is a media monitoring and clipping service used by broadcast professionals to prepare for their programs; it monitors TV media and enables the quick creation of audio soundbites for on-air radio usage" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, proprep, iQ media, media monitors, radio program monitoring, radio program preparation, social media monitoring, online new monitoring, twitter monitoring, audio clip library, fair use audio clips" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:ProPrep ID="ucProPrep" runat="server" />
</asp:Content>
