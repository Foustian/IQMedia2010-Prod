<%@ Page Language="C#" AutoEventWireup="true" Title="IQMedia : SMS SubScription"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Subscription.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/FooterPanel/FooterPanel.ascx" TagName="UCFooterPanel"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta name="viewport" content="width=device-width,initial-scale=1.0" />
    <title>IQMedia : SMS SubScription</title>      
    <link href="/Css/fonts/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="/Css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/Css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="/Css/my-style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        body
        {
            padding-top: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="container-fluid">
        <table border="0" align="center" cellpadding="0" cellspacing="0" style="width:100%;">
            <tr>
                <td>
                    <%--<img id="img" runat="server" src="http://qa.iqmediacorp.com/StationLogoImages/WPGH.gif" />--%>
                </td>
            </tr>
            <tr>
                <td class="mainbox" style="height: 99px;">
                    <a runat="server" id="hlogo">
                        <img id="imgLogo" src="~/images/logo_n.png" runat="server" alt="iQMedia" hspace="9"
                            vspace="11" border="0" /></a>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td valign="top" align="center" class="row-fluid content-borader" style="margin-top: 10px;">
                    <div style="display:table-cell;vertical-align:middle;padding:10px;" >
                        <asp:Label ID="lblSuccessMessge" CssClass="subscription-success" runat="server"></asp:Label>
                        <asp:Label ID="lblErrorMessage" CssClass="subscription-fail" runat="server"></asp:Label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td>
                    <uc2:UCFooterPanel ID="UCFooterPanelControl" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
