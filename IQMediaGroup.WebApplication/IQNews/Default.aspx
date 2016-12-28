<%@ Page Title="" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IQNews.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/IQ_NewsControl/IQ_NewsControl.ascx"
    TagName="IQ_News" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <uc:IQ_News ID="ucIQNews" runat="server" />
</asp:Content>
