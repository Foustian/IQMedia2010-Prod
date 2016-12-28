<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.ContactUs.Default"
    Title="IQMedia::ContactUs" MasterPageFile="~/IQMediaGroupStaticContent.Master" %>

<%@ Register Src="../UserControl/IQMediaMaster/ContactUs/ContactUS.ascx" TagName="UCContactUS"
    TagPrefix="UC" %>
<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <UC:UCContactUS ID="UCContactUSControl" runat="server" />
</asp:Content>
