<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdvanceSearchService.aspx.cs"
    Inherits="IQMediaGroup.AdvanceSearchServiceSample.AdvanceSearchService" ValidateRequest="false"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IQMedia::AdvanceSearchService</title>
    <style type="text/css">
        .leftWidth
        {
            width: 10%;
        }
        .middleWidth
        {
            text-align: center;
            width: 5%;
        }
        .main
        {
            font-family: Verdana;
            font-size: 11px;
        }
    </style>
    <link href="Css/style.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table width="963" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td class="mainbox">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="8" valign="top">
                                        <img id="Img1" runat="Server" src="~/images/mainbox-lt.jpg" width="8" height="8"
                                            border="0" alt="iQMedia" />
                                    </td>
                                    <td valign="top">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td width="317" height="99" valign="bottom">
                                                    <a runat="server" id="hlogo">
                                                        <img id="Img2" runat="Server" src="~/images/logo-IQMediaGroup.jpg" alt="iQMedia"
                                                            width="245" height="71" hspace="9" vspace="11" border="0" /></a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td width="8" valign="top">
                                        <img id="Img3" runat="Server" src="~/images/mainbox-rt.jpg" width="8" height="8"
                                            border="0" alt="iQMedia" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="contentarea">
                            <%--<table cellpadding="3" width="100%" cellspacing="0" border="0">
                                <tr>
                                    <td>
                                        <div style="width: 100%; height: 100%; background-color: #C0C0C0">
                                            <asp:Menu ID="Menu1" runat="server" Width="65%" Orientation="Horizontal" BackColor="#F7F6F3"
                                                DynamicHorizontalOffset="2" Font-Names="Tahoma" Font-Size="12px" ForeColor="#333333"
                                                StaticSubMenuIndent="5px">
                                                <StaticSelectedStyle BackColor="#B0B0B0" />
                                                <StaticMenuItemStyle HorizontalPadding="5px" BackColor="#C0C0C0" VerticalPadding="5px" />
                                                <DynamicHoverStyle BackColor="#C0C0C0" ForeColor="Black" />
                                                <DynamicMenuStyle BackColor="#B0B0B0" />
                                                <DynamicSelectedStyle BackColor="#5D7B9D" />
                                                <DynamicMenuItemStyle HorizontalPadding="5px" VerticalPadding="5px" />
                                                <StaticHoverStyle BackColor="#C0C0C0" ForeColor="White" />
                                            </asp:Menu>
                                        </div>
                                        <div style="padding: 10px 2px 5px 5px;">
                                            <asp:Label ID="lblBreadCrumb" runat="server"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ContentPlaceHolder ID="Content_Data" runat="server">
                                        </asp:ContentPlaceHolder>
                                    </td>
                                </tr>
                            </table>--%>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%" class="main">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                            <tr>
                                                <td class="leftWidth">
                                                    <asp:Label ID="lblService" runat="server" Text="Service"></asp:Label>
                                                </td>
                                                <td class="middleWidth">
                                                    :
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="main" ID="drpService" AutoPostBack="true"
                                                        OnSelectedIndexChanged="drpService_SelectedIndexChanged">
                                                        <asp:ListItem Value="GetSSPData">GetSSPData</asp:ListItem>
                                                        <asp:ListItem Value="GetRawMedia">GetRawMedia</asp:ListItem>
                                                        <asp:ListItem Value="GetRadioStation">GetRadioStation</asp:ListItem>
                                                        <asp:ListItem Value="GetRadioRawMedia">GetRadioRawMedia</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                            <tr>
                                                <td class="leftWidth">
                                                    <asp:Label ID="lblCase" runat="server" Text="Case"></asp:Label>
                                                </td>
                                                <td class="middleWidth">
                                                    :
                                                </td>
                                                <td>
                                                    <asp:DropDownList runat="server" CssClass="main" ID="drpCase" AutoPostBack="true"
                                                        OnSelectedIndexChanged="drpCase_SelectedIndexChanged">
                                                        <asp:ListItem Value="None">None</asp:ListItem>
                                                        <asp:ListItem Value="Case-1">Case-1</asp:ListItem>
                                                        <asp:ListItem Value="Case-2">Case-2</asp:ListItem>
                                                        <asp:ListItem Value="Case-3">Case-3</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                            <tr>
                                                <td class="leftWidth">
                                                    <asp:Label ID="lblURL" runat="server" Text="URL"></asp:Label>
                                                </td>
                                                <td class="middleWidth">
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtURL" CssClass="main" Width="500px" runat="server" ReadOnly="false"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                            <tr>
                                                <td class="leftWidth">
                                                    <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                                                </td>
                                                <td class="middleWidth">
                                                    :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDescription" CssClass="main" Width="500px" runat="server" TextMode="MultiLine"
                                                        Height="63px"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                            <tr>
                                                <td>
                                                    <a id="Screen" runat="server" cssclass="main">View screen for above selection</a>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" border="0" width="50%">
                                            <tr>
                                                <td align="left"  style="padding-left:90px">
                                                    <asp:Button ID="btnGo" Width="50px" CssClass="main" runat="server" Text="GO" OnClick="btnGo_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td>
                                                    Request:
                                                </td>
                                                <td>
                                                    Response:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtRequest" CssClass="main" Width="600px" Height="400px" TextMode="MultiLine"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtResponse" CssClass="main" Width="600px" Height="400px" TextMode="MultiLine"
                                                        runat="server"></asp:TextBox>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td width="8" valign="bottom">
                                        <img id="Img4" runat="Server" src="~/images/mainbox-lb.jpg" width="8" height="8"
                                            border="0" alt="iQMedia" />
                                    </td>
                                    <td>
                                        <img id="Img5" runat="Server" src="~/images/spacer.gif" width="1" height="1" border="0"
                                            alt="iQMedia" />
                                    </td>
                                    <td width="8" valign="bottom">
                                        <img id="Img6" runat="Server" src="~/images/mainbox-rb.jpg" width="8" height="8"
                                            border="0" alt="iQMedia" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="footer-text">
                <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="30">
                        </td>
                        <td align="right">
                            &copy; 2009. <strong>iQ Media Group</strong>. All rights reserved.
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <%--<table cellpadding="0" cellspacing="0" border="0" width="100%" class="main">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td class="leftWidth">
                            <asp:Label ID="lblService" runat="server" Text="Service"></asp:Label>
                        </td>
                        <td class="middleWidth">
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" CssClass="main" ID="drpService" AutoPostBack="true"
                                OnSelectedIndexChanged="drpService_SelectedIndexChanged">
                                <asp:ListItem Value="GetSSPData">GetSSPData</asp:ListItem>
                                <asp:ListItem Value="GetRawMedia">GetRawMedia</asp:ListItem>
                                <asp:ListItem Value="GetRadioStation">GetRadioStation</asp:ListItem>
                                <asp:ListItem Value="GetRadioRawMedia">GetRadioRawMedia</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td class="leftWidth">
                            <asp:Label ID="lblCase" runat="server" Text="Case"></asp:Label>
                        </td>
                        <td class="middleWidth">
                            :
                        </td>
                        <td>
                            <asp:DropDownList runat="server" CssClass="main" ID="drpCase" AutoPostBack="true"
                                OnSelectedIndexChanged="drpCase_SelectedIndexChanged">
                                <asp:ListItem Value="None">None</asp:ListItem>
                                <asp:ListItem Value="Case-1">Case-1</asp:ListItem>
                                <asp:ListItem Value="Case-2">Case-2</asp:ListItem>
                                <asp:ListItem Value="Case-3" Enabled="false">Case-3</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td class="leftWidth">
                            <asp:Label ID="lblURL" runat="server" Text="URL"></asp:Label>
                        </td>
                        <td class="middleWidth">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtURL" CssClass="main" Width="500px" runat="server" ReadOnly="false"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td class="leftWidth">
                            <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label>
                        </td>
                        <td class="middleWidth">
                            :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDescription" CssClass="main" Width="500px" runat="server" TextMode="MultiLine"
                                Height="63px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td>
                            <a id="Screen" runat="server" cssclass="main">View screen for above selection</a>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0" border="0" width="50%">
                    <tr>
                        <td align="left">
                            <asp:Button ID="btnGo" runat="server" Text="GO" OnClick="btnGo_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td>
                            Request:
                        </td>
                        <td>
                            Response:
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtRequest" CssClass="main" Width="600px" Height="400px" TextMode="MultiLine"
                                runat="server"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="txtResponse" CssClass="main" Width="600px" Height="400px" TextMode="MultiLine"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>--%>
    </form>
</body>
</html>
