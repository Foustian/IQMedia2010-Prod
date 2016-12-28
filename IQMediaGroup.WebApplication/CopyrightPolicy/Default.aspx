<%@ Page Title="" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.CopyrightPolicy.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/CopyrightPolicy/CopyrightPolicy.ascx" TagName="CopyrightPolicy"
    TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc:CopyrightPolicy ID="ucCopyrightPolicy" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
