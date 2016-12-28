<%@ Page Title="" Language="C#" MasterPageFile="~/IQMediaGroupStaticContent.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Politico.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/Political/Political.ascx" TagName="Political"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:Political ID="Political1" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
