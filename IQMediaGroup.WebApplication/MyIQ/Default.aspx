<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.MyIQ.Default"
    MasterPageFile="~/IQMediaGroupResponsive.Master" Title="iQMedia :: My iQ" %>

<%@ Register Src="~/UserControl/IQMediaMaster/MyIQ/MyIQ.ascx" TagName="UCMyIQ" TagPrefix="UC" %>
<%@ Register Src="~/UserControl/IQMediaMaster/MyIQReport/MyIQReport.ascx" TagName="UCMyIQReport" TagPrefix="UC2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/tab.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content_Data" runat="Server">
    <script src="../Script/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
    <script src="../Script/jquery-extra-selectors.js" type="text/javascript"></script>
    <asp:UpdatePanel ID="upType" runat="server">
        <ContentTemplate>
            <div class="ulheader">
                <asp:RadioButton ID="rdoSearch" AutoPostBack="true" runat="server" GroupName="FilterType"
                    OnCheckedChanged="FilterType_CheckedChanged" Text="Search" />
                <asp:RadioButton ID="rdoReport" AutoPostBack="true" runat="server" GroupName="FilterType"
                    OnCheckedChanged="FilterType_CheckedChanged" Text="Report" />
            </div>
            <asp:Panel ID="pnlSearch" CssClass="clear" runat="server" Style="width: 100%; height: 100%;">
                <UC:UCMyIQ ID="UCMyIQControl" runat="server"  />
            </asp:Panel>
            <asp:Panel ID="pnlReport" CssClass="clear" runat="server" Style="width: 100%; height: 100%;">
                <UC2:UCMyIQReport ID="UCMyIQReportControl" runat="server" />
            </asp:Panel>
        </ContentTemplate>        
    </asp:UpdatePanel>
    <script src="../Script/CommonIQ.js" type="text/javascript" ></script>
    <script src="../Script/MyClipsScript.js" type="text/javascript" ></script>
    <script src="../Script/Report.js" type="text/javascript" ></script>
    <script type="text/javascript" language="javascript">
        var instance = Sys.WebForms.PageRequestManager.getInstance();
        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                $addHandler(document, "keydown", onKeyDown);
            }
        }
        function onKeyDown(e) {
            if (e && e.keyCode == Sys.UI.Key.esc) {
                if (instance.get_isInAsyncPostBack()) {
                    instance.abortPostBack();
                }
            }
        }
    </script>
</asp:Content>
