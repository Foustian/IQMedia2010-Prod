<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IQAgent.Default"
    MasterPageFile="~/IQMediaGroupResponsive.Master" Title="iQMedia :: iQ Agent" %>

<%@ Register Src="../UserControl/IQMediaMaster/IQAgent/IQAgent.ascx" TagName="UCIQAgent"
    TagPrefix="UC" %>
<%@ Register Src="../UserControl/IQMediaMaster/IQAgentReport/IQAgentReport.ascx"
    TagName="UCIQAgentReport" TagPrefix="UC2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link rel="stylesheet" type="text/css" href="../Css/Paging.css" />
    <link rel="stylesheet" type="text/css" href="../Css/tab.css" />
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content_Data" runat="Server">
    <script src="../Script/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
    <%--<asp:UpdatePanel ID="upType" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <div class="ulheader">
        <asp:RadioButtonList RepeatDirection="Horizontal" AutoPostBack="true" ID="rdoIQAgentType"
            runat="server" OnSelectedIndexChanged="rdoIQAgentType_SelectedIndexChanged">
            <asp:ListItem Text="Search" Value="0"></asp:ListItem>
            <asp:ListItem Text="Report" Value="1"></asp:ListItem>
        </asp:RadioButtonList>
    </div>
    <%--<asp:Panel ID="pnlSearch" CssClass="clear" runat="server">
                <UC:UCIQAgent ID="UCIQAgentControl" runat="server" />
            </asp:Panel>--%>
    <%-- <asp:Panel ID="pnlReport" CssClass="clear" runat="server">
                <UC2:UCIQAgentReport ID="UCIQAgentControl1" runat="server" />
            </asp:Panel>--%>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <%--<asp:UpdatePanel ID="upIQAgentSearch" runat="server" UpdateMode="Always" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
    <asp:Panel ID="pnlSearch" CssClass="clear" runat="server">
        <UC:UCIQAgent ID="UCIQAgentControl" runat="server" />
    </asp:Panel>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <%--<asp:UpdatePanel ID="upIQAgentReport" runat="server" UpdateMode="Conditional" ChildrenAsTriggers="true">
        <ContentTemplate>--%>
    <asp:Panel ID="pnlReport" CssClass="clear" runat="server">
        <UC2:UCIQAgentReport ID="UCIQAgentControl1" runat="server" />
    </asp:Panel>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <script src="../Script/CommonIQ.js" type="text/javascript"></script>
    <script type="text/javascript" src="../Script/IQAgent.js"></script>
    <script type="text/javascript" src="../Script/Report.js"></script>
</asp:Content>
