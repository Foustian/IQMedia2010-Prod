<%@ Page Title="iQ media - Originator of the Media Intelligence Platform" Language="C#"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.About.Default"
    MasterPageFile="~/IQMediaGroupStaticContent.Master" %>

<%@ Register Src="../UserControl/IQMediaMaster/About/About.ascx" TagName="About"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="iQ media delivered cliQ, the first media intelligence platorm, which is revolutionizing the media monitoring industry for broadcast TV, online news, and social media" />
    <meta name="keywords" content="media intelligence platform, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, TV media content searching, cliq, proprep, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, media platform technology, fair use video content, video streaming" />
</asp:Content>
<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:About ID="About1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
