<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" MasterPageFile="~/IQMaster.Master" Title="IQMedia::Interested in IQMedia" Inherits="IQMediaGroup.WebApplication.Contact.Default" %>

<%@ Register src="../UserControl/IQMediaMaster/Contact/Contact.ascx" tagname="UCContact" tagprefix="UC" %>

<asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
 <table cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td>
                <UC:UCContact id="UCContactControl" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
