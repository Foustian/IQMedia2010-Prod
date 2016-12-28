<%@ Page Title="Learn About Media Intelligence Platforms for Media Monitoring and TV Video Clip Sharing" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Products.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/Products/Products.ascx" TagName="Products"
    TagPrefix="uc1" %>
     <asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
         <meta name="description" content="Learn how a Media Intelligence Platform goes beyond traditional media monitoring, enabling you to search, clip, and repurpose TV, onine news, and social media coverage in just seconds" />
         <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, search TV media content, monitor television coverage, TV video clips, media monitors, media monitoring services, optmized media cloud" />
     </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc1:Products ID="ucProducts" runat="server" />
</asp:Content>
