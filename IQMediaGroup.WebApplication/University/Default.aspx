<%@ Page Title="Media Intelligence Platform for Universities - Beyond Media Monitoring"
    Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.University.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/University/University.ascx" TagName="University"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="Learn how a media intelligence platform is helping universities to improve on media monitoring to better project their brand, market to donors, and communicate with alumni" />
    <meta name="keywords" content="media intelligence platform for universities, broadcast TV monitoring, media monitoring, media monitoring in politics, social media monitoring, twitter monitoring, tweet monitoring, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, fair use video content, video streaming" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc:University ID="ucUniversity" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
