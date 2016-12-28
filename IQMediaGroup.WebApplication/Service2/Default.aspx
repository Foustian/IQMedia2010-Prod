<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Services.Default" MasterPageFile="~/IQMediaGroupStaticContent.Master"%>

<%@ Register src="../UserControl/IQMediaMaster/Service2/Service2.ascx" tagname="Service2" tagprefix="uc1" %>

<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:Service2 ID="Service2" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>

