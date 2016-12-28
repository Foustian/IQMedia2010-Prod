<%@ Page Title="iQMedia :: PMG Search Demo" Language="C#" MasterPageFile="~/IQMediaAdminContent.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication.PMGSearchDemo.Default" %>
<%@ Register src="~/UserControl/IQAdminNavigationPanel/IQAdminNavigationPanel.ascx" tagname="IQAdminNavigationPanel" tagprefix="uc1" %>
<%@ Register Src="~/UserControl/PMGSearchDemo/PMGSearchDemo.ascx" TagName="PMGSearchDemo" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <link href="../Css/tutorsty.css" rel="stylesheet" type="text/css" />    
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />   

    <script language="javascript" type="text/javascript">
        function showdivCaption() {

            document.getElementById('ctl00_Content_Data_PMGSearchDemo1_tblPlayer').style.display = "block";
            $("#ctl00_Content_Data_PMGSearchDemo1_Player").show('slow');
        }
        function hidedivCaption() {
            $("#ctl00_Content_Data_PMGSearchDemo1_Player").hide('slow');
            (document.getElementById('ctl00_Content_Data_PMGSearchDemo1_Player')).setAttribute('enable', 'true');
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" align="right">
        <tr>
            <td valign="top">
                <uc1:PMGSearchDemo ID="PMGSearchDemo1" runat="server" />
            </td>
            <td>
                <div id="divEnd">
                    
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
