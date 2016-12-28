<%@ Page Title="" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.CDN.Default" %>

<%@ Register Src="~/UserControl/CDNUploadClient/CDNUploadClient.ascx" TagName="CDN" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table width="100%">
        <tr>
            <td>
                <uc2:CDN ID="ucCDN" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
