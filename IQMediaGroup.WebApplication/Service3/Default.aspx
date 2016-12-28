<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Service3.Default" MasterPageFile="~/IQMediaGroupStaticContent.Master"%>

<%@ Register src="../UserControl/IQMediaMaster/Service3/Service3.ascx" tagname="Service3" tagprefix="uc1" %>

<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:Service3 ID="Service3" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>

