<%@ Page Title="iQMedia :: IQAgent Queries" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.IQAgentConsole.Default" %>
<%@ Register src="../UserControl/IQAgentConsole/IQAgentConsole.ascx" tagname="IQAgentConsole" tagprefix="uc1" %>
<%@ Register src="../UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx" tagname="IQAdminNavigationPanel" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td valign="top">
                <uc1:IQAgentConsole ID="IQAgentConsole1" runat="server" />
            </td>
        </tr>
    </table>
    
</asp:Content>
