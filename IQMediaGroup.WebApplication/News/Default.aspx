<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.News.Default" MasterPageFile="~/IQMediaGroupStaticContent.Master"%>

<%@ Register src="../UserControl/IQMediaMaster/News/News.ascx" tagname="News" tagprefix="uc1" %>

<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:News ID="News1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>

