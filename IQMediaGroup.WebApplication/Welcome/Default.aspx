<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Welcome.Default" MasterPageFile="~/IQMaster.Master" %>

<%@ Register Src="~/UserControl/IQMediaMaster/Welcome/Welcome.ascx" TagName="UCWelcome"
    TagPrefix="UC" %>
    
    <asp:Content ID="Content_Admin" ContentPlaceHolderID="Content_Data" runat="Server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <UC:UCWelcome id="UCWelcomeControl" runat="server" />
            </td>
        </tr>
    </table>
    </asp:Content>