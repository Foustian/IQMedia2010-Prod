<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PMGSearchDemo.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.PMGSearchDemo.PMGSearchDemo" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td class="contentbox-blue-toplt" width="8px" height="8px">
        </td>
        <td class="contentbox-blue-topt" width="98%" height="8px">
        </td>
        <td class="contentbox-blue-toprt" width="8px" height="8px">
        </td>
    </tr>
    <tr>
        <td class="contentbox-l">
            <img id="Img4" runat="server" src="~/images/contentbox-l.jpg" width="8" height="1"
                border="0" alt="iQMedia" />
        </td>
        <td valign="top" bgcolor="#FFFFFF" class="pad-bt">
         <div>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-bottom: 10px">
                            <div class="AdminTitle">
                                PMG Search Demo</div>
                        </td>
                    </tr>
                </table>
            </div>
            <div style="padding-left: 20px;">
                <asp:ValidationSummary ID="vsPMGSearchDemo" EnableClientScript="true" runat="server"
                    ForeColor="#bd0000" Font-Size="Smaller" ValidationGroup="PMGSearch" />
            </div>
            
            <div>
                <div>
                    <asp:Label ID="lblMsg" Visible="false" Text="Only search term is required.No other parameters are required."
                        ForeColor="Maroon" runat="server"></asp:Label>
                </div>
                <asp:UpdatePanel ID="updSearch" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table>
                            <tr>
                                <td>
                                    Search Term
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtSearchTerm" CssClass="textbox03" runat="server" Width="245px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvSearchTerm" Display="None" ControlToValidate="txtSearchTerm"
                                        ErrorMessage="Search term is required." ValidationGroup="PMGSearch" Text="*"
                                        runat="server"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Start Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStartDate" runat="server" AutoCompleteType="None" CssClass="textbox03"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalEtxtStartDate" runat="server" CssClass="MyCalendar"
                                        TargetControlID="txtStartDate">
                                    </AjaxToolkit:CalendarExtender>
                                </td>
                                <td>
                                    Start Hour
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlStartHour" runat="server" CssClass="dropdown01">
                                        <asp:ListItem Value="0">00</asp:ListItem>
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                        <asp:ListItem Value="13">13</asp:ListItem>
                                        <asp:ListItem Value="14">14</asp:ListItem>
                                        <asp:ListItem Value="15">15</asp:ListItem>
                                        <asp:ListItem Value="16">16</asp:ListItem>
                                        <asp:ListItem Value="17">17</asp:ListItem>
                                        <asp:ListItem Value="18">18</asp:ListItem>
                                        <asp:ListItem Value="19">19</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="21">21</asp:ListItem>
                                        <asp:ListItem Value="22">22</asp:ListItem>
                                        <asp:ListItem Value="23">23</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    End Date
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtEndDate" runat="server" AutoCompleteType="None" CssClass="textbox03"></asp:TextBox>
                                    <AjaxToolkit:CalendarExtender ID="CalEtxtEndDate" runat="server" CssClass="MyCalendar"
                                        TargetControlID="txtEndDate">
                                    </AjaxToolkit:CalendarExtender>
                                </td>
                                <td>
                                    End Hour
                                </td>
                                <td>
                                    :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlEndHour" runat="server" CssClass="dropdown01">
                                        <asp:ListItem Value="0">00</asp:ListItem>
                                        <asp:ListItem Value="1">01</asp:ListItem>
                                        <asp:ListItem Value="2">02</asp:ListItem>
                                        <asp:ListItem Value="3">03</asp:ListItem>
                                        <asp:ListItem Value="4">04</asp:ListItem>
                                        <asp:ListItem Value="5">05</asp:ListItem>
                                        <asp:ListItem Value="6">06</asp:ListItem>
                                        <asp:ListItem Value="7">07</asp:ListItem>
                                        <asp:ListItem Value="8">08</asp:ListItem>
                                        <asp:ListItem Value="9">09</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                        <asp:ListItem Value="11">11</asp:ListItem>
                                        <asp:ListItem Value="12">12</asp:ListItem>
                                        <asp:ListItem Value="13">13</asp:ListItem>
                                        <asp:ListItem Value="14">14</asp:ListItem>
                                        <asp:ListItem Value="15">15</asp:ListItem>
                                        <asp:ListItem Value="16">16</asp:ListItem>
                                        <asp:ListItem Value="17">17</asp:ListItem>
                                        <asp:ListItem Value="18">18</asp:ListItem>
                                        <asp:ListItem Value="19">19</asp:ListItem>
                                        <asp:ListItem Value="20">20</asp:ListItem>
                                        <asp:ListItem Value="21">21</asp:ListItem>
                                        <asp:ListItem Value="22">22</asp:ListItem>
                                        <asp:ListItem Value="23">23</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Sort Fields
                                </td>
                                <td>
                                    :
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtSortFields" runat="server" Width="245px" CssClass="textbox03"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                </td>
                                <td colspan="4">
                                    <asp:Button ID="btnSearch" Text="Search" Width="150px" runat="server" OnClick="btnSearch_Click"
                                        ValidationGroup="PMGSearch" />
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="updSearchGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div style="font-weight: bold; padding: 10px 10px 10px 0px; display: none" id="divTotalResultCount"
                            runat="server">
                            <span>Total no of video hours = </span>
                            <asp:Label ID="lblTotalHourOfVideo" runat="server"></asp:Label>
                        </div>
                        <asp:GridView ID="gvSearchResult" runat="server" AutoGenerateColumns="false" CellPadding="5"
                            AllowPaging="true" CellSpacing="0" BorderColor="#e4e4e4" Style="border-collapse: collapse;"
                            EmptyDataText="No Results Found" AllowSorting="true" PagerSettings-Mode="NextPrevious"
                            PageIndex="0" OnRowCommand="gvSearchResult_RowCommand" OnSorting="gvSearchResult_Sorting">
                            <Columns>
                                <asp:BoundField DataField="RawMediaID" HeaderText="GUID" />
                                <asp:TemplateField HeaderText="Station">
                                    <ItemTemplate>
                                        <asp:Image ID="imgStationLogo" ImageUrl='<%# string.Concat("~/StationLogoImages/",Eval("StationID"),".gif") %>'
                                            AlternateText='<%# Eval("StationID") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Market" HeaderText="Market" SortExpression="market" />
                                <asp:BoundField DataField="Affiliate" HeaderText="Affiliate" SortExpression="affiliate" />
                                <asp:BoundField DataField="DateTime" HeaderText="DateTime" SortExpression="date,hour" />
                                <asp:TemplateField HeaderText="Hit Count" SortExpression="Hits">
                                    <ItemTemplate>
                                        <asp:Label ID="lblHitCount" Text='<%# Eval("Hits") %>' runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="btnPlay" CssClass="btn-play" CommandName="PlayVideo" CommandArgument='<%# Eval("RawMediaID") %>'
                                            runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="grid-th" />
                            <PagerStyle CssClass="heading-blue" />
                        </asp:GridView>
                        <div style="padding-top: 10px; display: none" id="divPager" runat="server">
                            <asp:ImageButton ID="imgRawMediaPrevious" runat="server" AlternateText="Previous"
                                OnClick="imgRawMediaPrevious_Click" ImageUrl="~/Images/arrow-previous.jpg" />
                            <asp:ImageButton ID="imgRawMediaNext" runat="server" AlternateText="Next" OnClick="imgRawMediaNext_Click"
                                ImageUrl="~/Images/arrow-next.jpg" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div>
                    <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td>
                                        <table border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                                            <tr>
                                                <td>
                                                    <div id="wrapper" runat="server" jquery1261727998253="53" sizcache="69" sizset="1">
                                                        <div id="videoplayer" jquery1261727998253="87" sizcache="56" sizset="0">
                                                            <div class="content" jquery1261727998253="86" sizcache="56" sizset="0">
                                                                <div id="videoframe" sizcache="56" sizset="4">
                                                                    <div id="RL_Player_Wrapper" style="margin-top: 10px; background: #000000; position: relative">
                                                                        <div id="RL_Player" style="width: 545px; height: 340px">
                                                                            <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server">
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                    <div id="time" runat="server" visible="false">
                                                                        <div id="timeBar" style="display: block; padding: 0;">
                                                                            <img id="time0" class="hourSeek" runat="server" style="" src="~/images/time00.GIF"
                                                                                onclick="setSeekPoint(0, this);" />
                                                                            <img id="time10" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time10.GIF"
                                                                                onclick="setSeekPoint(600, this);" />
                                                                            <img id="time20" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time20.GIF"
                                                                                onclick="setSeekPoint(1200, this);" />
                                                                            <img id="time30" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time30.GIF"
                                                                                onclick="setSeekPoint(1800, this);" />
                                                                            <img id="time40" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time40.GIF"
                                                                                onclick="setSeekPoint(2400, this);" />
                                                                            <img id="time50" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time50.GIF"
                                                                                onclick="setSeekPoint(3000, this);" />
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <table id="tblPlayer" runat="server" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td height="25" class="hdbar">
                                                    <table width="559" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td align="left" style="padding-left: 10px">
                                                                <%--<a href="#divEnd" class="show" onclick="showdivCaption();">SHOW CAPTION</a>--%>
                                                                 <div class="show" style="cursor:pointer;" onclick="showdivCaption();">SHOW CAPTION</div>
                                                            </td>
                                                            <td align="right" style="padding-right: 10px">
                                                                <%--<a href="#divEnd" class="hide" onclick="hidedivCaption();">HIDE</a>--%>
                                                                <div class="hide" style="cursor:pointer;" onclick="hidedivCaption();">HIDE</div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <div id="Player" runat="server" style="display: none">
                                                        <table width="559" border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td height="63" class="grey-grad">
                                                                    <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td class="heading-blue2">
                                                                                Closed Caption:<br />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="content-text-grey">
                                                                                <div id="DivCaption" runat="server" class="panel" style="overflow-y: auto;">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
        </td>
        <td class="contentbox-r">
            <img id="Img7" runat="server" src="~/images/contentbox-r.jpg" width="8" height="1"
                border="0" alt="iQMedia" />
        </td>
    </tr>
    <tr>
        <td class="contentbox-blue-botlt" width="8px" height="8px">
        </td>
        <td class="contentbox-blue-bott" width="98%" height="8px">
        </td>
        <td class="contentbox-blue-botrt" width="8px" height="8px">
        </td>
    </tr>
</table>
<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="updSearch"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="updSearch" TargetControlID="UpdateProgress1" CssClass="updateProgress" />
<asp:UpdateProgress ID="updSearchResult" runat="server" AssociatedUpdatePanelID="updSearchGrid"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="updProgressExtenderRawMediaResults"
    ControlToOverlayID="updSearchGrid" TargetControlID="updSearchResult" CssClass="updateProgress" />
