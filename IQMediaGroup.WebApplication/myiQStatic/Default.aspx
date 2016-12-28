<%@ Page Title="Learn about My iQ - the enterprise media center" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.myiQStatic.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/myiQStatic/myiQStatic.ascx" TagName="myiQStatic"
    TagPrefix="uc" %>

    <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="An enterprise media center like My iQ is a characterisic of a media intelligence platform, enabling users to store and reuse fair use video clips and other media, and is something that is not available with a traditional media monitoring service" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, my iq, enterprise media center, cliQ, iQ media, video hosting, media monitors, social media monitoring, online new monitoring, twitter monitoring, video clip library, fair use video clips, user generated video hosting, user generated video library" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:myiQStatic ID="ucmyiQStatic" runat="server" />
</asp:Content>
