<%@ Page Language="C#" Title="Industries that Use a Media Intelligence Platform to Push Beyond Simple Media Monitoring"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Industries.Default"
    MasterPageFile="~/IQMediaGroupStaticContent.Master" %>

<%@ Register Src="../UserControl/IQMediaMaster/Industries/Industries.ascx" TagName="Industries"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <meta name="description" content="New media intelligence platforms are being adopted by a wide range of industries in which companies need to monitor media to protect their brand, control their message, and promote their ideas" />
    <meta name="keywords" content="media intelligence platform for industries, broadcast TV monitoring, media monitoring, social media monitoring, twitter monitoring, tweet monitoring, search TV media content, cliq, monitor television coverage, TV video clips, media monitors, media monitoring services, video hosting, iq media, media platform technology, fair use video content, video streaming, optimized media cloud, inline media workspace, my iQ, enterprise media center" />
</asp:Content>
<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:Industries ID="Industries1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
