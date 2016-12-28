<%@ Page Title="Media Intelligence Platform for Collegiate Athletic Programs - Beyond Media Monitoring"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.CollegiateAthleticPrograms.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/CollegiateAthleticPrograms/CollegiateAthleticPrograms.ascx"
    TagName="CollegiateAthleticPrograms" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn how a media intelligence platform is helping collegiate athletic programs to improve on media monitoring to better market to corporate sponsors, control their PR, and reach out to donors" />
    <meta name="keywords" content="media intelligence platform for collegiate athletic programs, broadcast TV monitoring, media monitoring, media monitoring in politics, social media monitoring, twitter monitoring, tweet monitoring, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, fair use video content, video streaming, media PR for collegiate athletics" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:CollegiateAthleticPrograms ID="ucCollegiateAthleticPrograms" runat="server" />
</asp:Content>
