<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAdvance.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQAdvance.IQAdvance" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<div class="iq-agent-main">
    <div class="iq-agent-left">
        <div class="ag-gray-main">
            <div class="ag-blue-title">
                Search Terms</div>
            <div class="ag-lin-main">
                <asp:ValidationSummary DisplayMode="BulletList" ID="vsIQMediaSearch" EnableClientScript="true"
                    runat="server" ValidationGroup="IQMediaSearch" ForeColor="#bd0000" Font-Size="Smaller" />
            </div>
            <div class="ag-lin-main">
                <asp:RadioButtonList ID="rblTvOrRadio" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                    Style="vertical-align: middle; width: 100%;" CellPadding="5" OnSelectedIndexChanged="RblTVOrRadio_SelectedIndexChanged">
                    <asp:ListItem Value="0" Selected="True"><span style="vertical-align:middle;padding-left:5px;">TV Stations</span></asp:ListItem>
                    <asp:ListItem Value="1"><span style="vertical-align:middle;padding-left:5px;">Radio Stations</span></asp:ListItem>
                </asp:RadioButtonList>
            </div>
            <div class="ag-lin-main">
                <asp:UpdatePanel ID="updSearchTerms" runat="server">
                    <ContentTemplate>
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr id="trInitialSearch" runat="server">
                                <td class="mid">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                        <tr>
                                            <td colspan="2">
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text1">
                                                Program Title:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProgram" runat="server" CssClass="grayinput" Width="135px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text1">
                                                Appearing:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAppearing" runat="server" CssClass="grayinput" Width="135px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text1">
                                                Search Term:
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtSearch" runat="server" Width="135px" CssClass="grayinput"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 10px 0px 0px 0px">
                                    <asp:Button ID="btnSubmit" runat="server" ValidationGroup="AdvancedSearch" CssClass="btn-blue2"
                                        Text="Search" OnClick="BtnSubmit_Click" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="#bd0000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="ag-gray-main">
            <div class="ag-lin-main">
                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td class="result-bg">
                                                &nbsp;Raw Media Results<br />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="3">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:HiddenField ID="hfOffsetValue" runat="server" />
                                    <asp:Label ID="lblTimeOutMsg" runat="server" Style="display: none;"></asp:Label>
                                    <div id="divTimeOut">
                                        <asp:Panel ID="pnl" Height="100%" runat="server" Visible="false">
                                            <asp:Panel ID="pnlRadioStations" runat="server" Visible="false">
                                                <div style="font-weight: bold; padding: 5px 5px 5px 0px;">
                                                    <span>Total Programs Found = </span>
                                                    <asp:Label ID="lblNoOfRadioRawMedia" runat="server"></asp:Label>
                                                </div>
                                                <br />
                                                <asp:GridView ID="grvRadioStations" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                                    PageSize="10" Width="100%" border="0" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4"
                                                    BackColor="#FFFFFF" Style="border-collapse: collapse;" AllowSorting="true" EmptyDataText="No Results Found"
                                                    PageIndex="0" OnPageIndexChanging="GrvRadioStations_PageIndexChanging" OnSorting="GrvRadioStations_Sorting"
                                                    CssClass="grid">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgStationLogo" ImageUrl='<%# Eval("StationLogo") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="grid-th" Width="38px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="38px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="dma_name" SortExpression="dma_name" HeaderText="Market">
                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="45px" />
                                                            <HeaderStyle CssClass="grid-th" Width="45px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Date Time" SortExpression="DateTime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRawMediaDatetime" runat="server" Text='<%# Eval("RawMediaDateTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="50px" />
                                                            <HeaderStyle CssClass="grid-th" Width="50px" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandArgument='<%# Eval("RawMediaID") %>'
                                                                    OnClientClick="hidediv();" OnCommand="LbtnRawMediaPlay_Command" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="grid-th" Width="33px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="33px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="grid-th" />
                                                    <PagerStyle Height="30px" VerticalAlign="Middle" HorizontalAlign="Left" CssClass="pagecontent1" />
                                                    <PagerTemplate>
                                                        <div style="padding-top: 10px; padding-bottom: 10px;">
                                                            <table>
                                                                <tr>
                                                                    <td valign="middle">
                                                                        <asp:ImageButton ID="btnPrevious" runat="server" AlternateText="Previous" CommandName="Page"
                                                                            CommandArgument="Prev" ImageUrl="~/Images/arrow-previous.jpg" />
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <asp:Label ID="lblRadoCurrentPageNo" Font-Bold="true" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td valign="middle">
                                                                        <asp:ImageButton ID="btnNext" runat="server" AlternateText="Next" CommandName="Page"
                                                                            CommandArgument="Next" ImageUrl="~/Images/arrow-next.jpg" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                    </PagerTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlPMGSearchBasic" runat="server" Visible="false">
                                                <div style="font-weight: bold; padding: 5px 5px 5px 0px;">
                                                    <span>Total Programs Found = </span>
                                                    <asp:Label ID="lblNoOfRawMedia" runat="server"></asp:Label>
                                                </div>
                                                <br />
                                                <asp:GridView ID="grvRawMediaPMGBasic" runat="server" AutoGenerateColumns="false"
                                                    Width="100%" border="0" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4"
                                                    BackColor="#FFFFFF" Style="border-collapse: collapse;" AllowSorting="true" AllowPaging="true"
                                                    PageSize="2" OnSorting="GrvRawMediaPmgBasic_Sorting" EmptyDataText="No Results Found"
                                                    CssClass="grid">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Image ID="imgStationLogo" ImageUrl='<%# Eval("StationLogo") %>' runat="server" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="grid-th" Width="38px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="38px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Program" DataField="Title120" ItemStyle-CssClass="heading-blue"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="40%">
                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="45px" />
                                                            <HeaderStyle CssClass="grid-th" Width="45px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="IQ_Dma_Name" SortExpression="market" HeaderText="Market">
                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="50px" />
                                                            <HeaderStyle CssClass="grid-th" Width="50px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Date Time" SortExpression="datetime">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblRawMediaDatetime" runat="server" Text='<%# Eval("DateTime") %>'></asp:Label>
                                                            </ItemTemplate>
                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="70px" />
                                                            <HeaderStyle CssClass="grid-th" Width="70px" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField HeaderText="Hits" DataField="Hits" ItemStyle-CssClass="content-text-new"
                                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="25%">
                                                            <ItemStyle HorizontalAlign="Center" Width="28px" />
                                                            <HeaderStyle CssClass="grid-th" Width="28px" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandArgument='<%# Eval("RawMediaID") %>'
                                                                    OnClientClick="hidediv();" OnCommand="LbtnRawMediaPlay_Command" />
                                                            </ItemTemplate>
                                                            <HeaderStyle CssClass="grid-th" Width="33px" />
                                                            <ItemStyle HorizontalAlign="Center" Width="33px" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="grid-th" />
                                                    <PagerStyle Height="30px" VerticalAlign="Middle" HorizontalAlign="Left" CssClass="pagecontent1" />
                                                    <PagerSettings Visible="false" />
                                                </asp:GridView>
                                                <div style="padding-top: 10px; padding-bottom: 10px;">
                                                    <table width="100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:ImageButton ID="imgRawMediaPrevious" runat="server" AlternateText="Previous"
                                                                    OnClick="ImgRawMediaPrevious_Click" ImageUrl="~/Images/arrow-previous.jpg" />
                                                                <span style="vertical-align: top;">
                                                                    <asp:Label ID="lblCurrentPageNo" Font-Bold="true" runat="server"></asp:Label></span>
                                                                <asp:ImageButton ID="imgRawMediaNext" runat="server" AlternateText="Next" OnClick="ImgRawMediaNext_Click"
                                                                    ImageUrl="~/Images/arrow-next.jpg" />
                                                            </td>
                                                            <%--<td align="right" runat="server" id="divtxtplay" valign="top" class="contenttext-small">
                                                                Click on play button to play video
                                                            </td>--%>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </asp:Panel>
                                        </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
    </div>
    <div class="iq-agent-right">
        <div class="ag-gray-main">
            <div>
                <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <iframe id="ClipFrame" runat="server" visible="false" style="width: 581px; height: 730px;"
                            scrolling="no" marginwidth="0" marginheight="0" hspace="0" vspace="0" border="0"
                            frameborder="0"></iframe>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <div class="ag-gray-main">
            <div style="width: 100%;">
                <div class="show-bg">
                    <div class="show-hide">
                        <div class="float-left" onclick="showdiv();">
                            <a href="javascript:;">
                                <img src="../images/show.png" alt="">
                                SHOW SEARCH</a></div>
                        <div class="float-right" onclick="hidediv();">
                            <a href="javascript:;">HIDE
                                <img src="../images/hiden.png" alt=""></a></div>
                    </div>
                </div>
                <div style="width: 570px;" class="ag-lin-main">
                    <table id="tblSearch" runat="server" cellpadding="0" cellspacing="0" style="display: none;">
                        <tr>
                            <td>
                                <div id="jason" runat="server" style="width: 100%; overflow: hidden;" class="hdcontent">
                                    <asp:UpdatePanel ID="validate" runat="server" UpdateMode="Conditional">
                                        <ContentTemplate>
                                            <table style="width: 100%; overflow: hidden;" border="0" cellspacing="3" cellpadding="0">
                                                <tr>
                                                    <td width="2%">
                                                    </td>
                                                    <td width="98%" align="left" valign="top">
                                                        <asp:ValidationSummary ID="ValidationSummary1" EnableClientScript="true" runat="server"
                                                            ValidationGroup="AdvancedSearch" DisplayMode="List" ForeColor="#bd0000" Font-Size="Smaller" />
                                                        <asp:Label ID="Label1" runat="server" ForeColor="#bd0000"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                    <table style="width: 570px; overflow: hidden;" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="bluebox-hd">
                                                                        &nbsp;Date &amp; Time<br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td height="150" valign="top" class="blue-content-bg">
                                                            <asp:UpdatePanel ID="date" runat="server" UpdateMode="Conditional">
                                                                <ContentTemplate>
                                                                    <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                                                        <tr id="trTimeZone" runat="server">
                                                                            <td width="100" class="content-text1">
                                                                                Time Zone:&nbsp;&nbsp;
                                                                            </td>
                                                                            <td>
                                                                                <asp:DropDownList ID="ddlTimeZone" runat="server" CssClass="grayselect">
                                                                                    <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                                                                                    <asp:ListItem Text="EST" Value="1"></asp:ListItem>
                                                                                    <asp:ListItem Text="CST" Value="2"></asp:ListItem>
                                                                                    <asp:ListItem Text="MST" Value="3"></asp:ListItem>
                                                                                    <asp:ListItem Text="PST" Value="4"></asp:ListItem>
                                                                                </asp:DropDownList>
                                                                            </td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td valign="top" width="100" class="content-text1">
                                                                                Date:
                                                                            </td>
                                                                            <td width="225" class="content-text1">
                                                                                From Date:<br />
                                                                                <asp:TextBox ID="txtFromDate" runat="server" AutoCompleteType="None" CssClass="grayinput"
                                                                                    Style="cursor: pointer;"></asp:TextBox>
                                                                                <cc1:PropertyProxyValidator ID="pplFromDate" runat="server" ControlToValidate="txtFromDate"
                                                                                    Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                                    RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                                    Display="Dynamic"></cc1:PropertyProxyValidator>
                                                                                <asp:CustomValidator ID="cusFromDate" runat="server" Display="Dynamic" ControlToValidate="txtFromDate"
                                                                                    ValidationGroup="AdvancedSearch" OnServerValidate="CVFromDate_ServerValidate"
                                                                                    Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                                                <AjaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkFromDate" TargetControlID="txtFromDate"
                                                                                    WatermarkCssClass="watermarked" WatermarkText="From Date" runat="server">
                                                                                </AjaxToolkit:TextBoxWatermarkExtender>
                                                                                <AjaxToolkit:CalendarExtender ID="CalendarExtenderFromDate" runat="server" CssClass="MyCalendar"
                                                                                    TargetControlID="txtFromDate">
                                                                                </AjaxToolkit:CalendarExtender>
                                                                            </td>
                                                                            <td class="content-text1">
                                                                                To Date:<br />
                                                                                <asp:TextBox ID="txtToDate" runat="server" AutoCompleteType="None" CssClass="grayinput"
                                                                                    Style="cursor: pointer;"></asp:TextBox>
                                                                                <cc1:PropertyProxyValidator ID="pplEndDate" runat="server" ControlToValidate="txtToDate"
                                                                                    Text="*" PropertyName="SearchEndDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                                    RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                                    Display="Dynamic"></cc1:PropertyProxyValidator>
                                                                                <AjaxToolkit:TextBoxWatermarkExtender ID="TextBoxWatermarkToDate" TargetControlID="txtToDate"
                                                                                    WatermarkCssClass="watermarked" WatermarkText="To Date" runat="server">
                                                                                </AjaxToolkit:TextBoxWatermarkExtender>
                                                                                <AjaxToolkit:CalendarExtender ID="CalendarExtenderToDate" runat="server" CssClass="MyCalendar"
                                                                                    TargetControlID="txtToDate">
                                                                                </AjaxToolkit:CalendarExtender>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td class="content-text1">
                                                                                Time:&nbsp;
                                                                            </td>
                                                                            <td class="content-text1">
                                                                                From Time:
                                                                                <br />
                                                                                <table cellpadding="0" cellspacing="0" border="0">
                                                                                    <tr>
                                                                                        <td style="padding-right: 5px">
                                                                                            <asp:DropDownList ID="ddlStartTime" runat="server" AutoPostBack="false" CssClass="grayselect">
                                                                                                <asp:ListItem Text="12:00" Value="0"></asp:ListItem>
                                                                                                <asp:ListItem Text="01:00" Value="1"></asp:ListItem>
                                                                                                <asp:ListItem Text="02:00" Value="2"></asp:ListItem>
                                                                                                <asp:ListItem Text="03:00" Value="3"></asp:ListItem>
                                                                                                <asp:ListItem Text="04:00" Value="4"></asp:ListItem>
                                                                                                <asp:ListItem Text="05:00" Value="5"></asp:ListItem>
                                                                                                <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                                                                                                <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                                                                                                <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                                                                                                <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                                                                                                <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                                                                                                <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td class="content-text-blue" align="left">
                                                                                            <asp:RadioButtonList ID="rdAmPmFromDate" runat="server" CellPadding="0" CellSpacing="0"
                                                                                                BorderWidth="0" RepeatDirection="Vertical" Style="padding-left: -5px;">
                                                                                                <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                                                <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td class="content-text1">
                                                                                To Time:
                                                                                <br />
                                                                                <table border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td style="padding-right: 5px">
                                                                                            <asp:DropDownList ID="ddlEndTime" runat="server" AutoPostBack="false" CssClass="grayselect">
                                                                                                <asp:ListItem Text="12:00" Value="0"></asp:ListItem>
                                                                                                <asp:ListItem Text="01:00" Value="1"></asp:ListItem>
                                                                                                <asp:ListItem Text="02:00" Value="2"></asp:ListItem>
                                                                                                <asp:ListItem Text="03:00" Value="3"></asp:ListItem>
                                                                                                <asp:ListItem Text="04:00" Value="4"></asp:ListItem>
                                                                                                <asp:ListItem Text="05:00" Value="5"></asp:ListItem>
                                                                                                <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                                                                                                <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                                                                                                <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                                                                                                <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                                                                                                <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                                                                                                <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                                                                                            </asp:DropDownList>
                                                                                        </td>
                                                                                        <td class="content-text-blue" align="left">
                                                                                            <asp:RadioButtonList ID="rdAMPMToDate" runat="server" CellPadding="0" CellSpacing="0"
                                                                                                BorderWidth="0" RepeatDirection="Vertical" Style="padding-left: -5px;">
                                                                                                <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                                                <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                                            </asp:RadioButtonList>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ContentTemplate>
                                                            </asp:UpdatePanel>
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
                                                <table width="570px" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table width="570px" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="bluebox-hd">
                                                                        &nbsp;DMA Rank and Name<br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="blue-content-bg">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr style="height: 29px;">
                                                                    <td class="content-textRd" align="center" valign="top">
                                                                        <asp:RadioButtonList ID="rdoMarket" runat="server" CellSpacing="1" CellPadding="1"
                                                                            Width="50%" RepeatColumns="2" BorderWidth="0" AutoPostBack="false" RepeatDirection="Horizontal"
                                                                            TextAlign="Right" Style="vertical-align: middle">
                                                                            <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:middle">Select All</span></asp:ListItem>
                                                                            <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:middle">Manual Select</span></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                        <asp:RadioButtonList ID="rdoRadioStations" runat="server" CellSpacing="1" CellPadding="1"
                                                                            RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right"
                                                                            Style="vertical-align: middle">
                                                                            <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:middle">Select All</span></asp:ListItem>
                                                                            <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:middle">Manual Select</span></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" class="content-text">
                                                                        <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Style="max-height: 300px"
                                                                            Visible="true">
                                                                            <asp:DataList ID="rptMarket" runat="server" RepeatColumns="3" Width="100%" CellPadding="0"
                                                                                CellSpacing="0">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                        <tr>
                                                                                            <td width="15" height="100%" valign="top" style="height: 29px;">
                                                                                                <%# Container.ItemIndex+1 %>
                                                                                            </td>
                                                                                            <td width="107" height="100%" valign="top">
                                                                                                <%# Eval("IQ_Dma_Name") %>
                                                                                            </td>
                                                                                            <td valign="top" height="100%">
                                                                                                <input type="checkbox" id="chkMarket" runat="server" value='<%# Eval("IQ_Dma_Name") %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:DataList>
                                                                            <asp:DataList ID="rptRadioStations" runat="server" RepeatColumns="3" Width="100%"
                                                                                CellPadding="0" CellSpacing="0">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                        <tr>
                                                                                            <td width="15" height="100%" valign="top" style="height: 29px;">
                                                                                                <%# Container.ItemIndex+1 %>
                                                                                            </td>
                                                                                            <td width="107" height="100%" valign="top">
                                                                                                <asp:Label ID="lblRadioStation" runat="server" Text="<%# Container.DataItem %>" CssClass="content-text"></asp:Label>
                                                                                            </td>
                                                                                            <td valign="top" height="100%">
                                                                                                <input type="checkbox" id="chkRadioStation" runat="server" value='<%# Container.DataItem %>' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:DataList>
                                                                        </asp:Panel>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trAffiliateNetwork" runat="server">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="bluebox-hd">
                                                                        &nbsp;Affiliate Network<br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="blue-content-bg">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr height="29px">
                                                                    <td class="content-textRd" align="center" valign="top">
                                                                        <asp:RadioButtonList ID="rdoAffil" runat="server" CellSpacing="3" CellPadding="3"
                                                                            Width="50%" RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right"
                                                                            Style="vertical-align: middle">
                                                                            <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:middle">Select All</span></asp:ListItem>
                                                                            <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:middle">Manual Select</span></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" class="content-text1">
                                                                        <asp:Panel ID="pnlAffil" runat="server" ScrollBars="Auto" Style="max-height: 150px"
                                                                            Visible="true">
                                                                            <asp:DataList ID="rptAffil" runat="server" RepeatColumns="3" Width="100%" CellPadding="0"
                                                                                CellSpacing="0">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                        <tr>
                                                                                            <td width="15" height="100%" valign="top" style="height: 29px;">
                                                                                                <%# Container.ItemIndex+1 %>
                                                                                            </td>
                                                                                            <td width="107" height="100%" valign="top">
                                                                                                <%# Eval("Station_Affil") %>
                                                                                            </td>
                                                                                            <td height="100%" valign="top">
                                                                                                <%--<input type="checkbox" runat="server" value='<%# Eval("Station_Affil_Num") %>' id='chkAffil' />--%>
                                                                                                <input type="checkbox" runat="server" value='<%# Eval("Station_Affil") %>' id='chkAffil' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:DataList>
                                                                        </asp:Panel>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr id="trProgramSubCategory" runat="server">
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                <tr>
                                                                    <td class="bluebox-hd">
                                                                        &nbsp;Program Category<br />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="blue-content-bg">
                                                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                <tr height="29px">
                                                                    <td class="content-textRd" align="center" valign="top">
                                                                        <asp:RadioButtonList ID="rdoProgramType" runat="server" CellSpacing="1" CellPadding="1"
                                                                            Width="50%" BorderWidth="0" RepeatDirection="Horizontal" Style="padding-left: -5px;">
                                                                            <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:middle">Select All</span></asp:ListItem>
                                                                            <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:middle">Manual Select</span></asp:ListItem>
                                                                        </asp:RadioButtonList>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td valign="top" class="content-text1">
                                                                        <asp:Panel ID="pnlProgramType" runat="server" ScrollBars="Auto" Style="max-height: 150px;"
                                                                            Visible="true">
                                                                            <asp:DataList ID="rptProgramType" runat="server" RepeatColumns="3" Width="100%" CellPadding="0"
                                                                                CellSpacing="0">
                                                                                <ItemTemplate>
                                                                                    <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                        <tr>
                                                                                            <td width="15" height="100%" valign="top" style="height: 29px;">
                                                                                                <%# Container.ItemIndex+1 %>
                                                                                            </td>
                                                                                            <td width="107" height="100%" valign="top">
                                                                                                <%# Eval("IQ_Class") %>
                                                                                            </td>
                                                                                            <td height="100%" valign="top">
                                                                                                <input type="checkbox" runat="server" value='<%# Eval("IQ_Class_Num") %>' id='chkProgramType' />
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ItemTemplate>
                                                                            </asp:DataList>
                                                                        </asp:Panel>
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
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                                                    <ContentTemplate>
                                                        <asp:Button ID="btnSubmitSecond" runat="server" ValidationGroup="AdvancedSearch"
                                                            CssClass="btn-blue2" Text="Search" OnClick="BtnSubmit_Click" />
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                                <%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
                                                    <ProgressTemplate>
                                                        <div>
                                                            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                                                                alt="Loading..." id="imgLoading" />
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                                <cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender6"
                                                    ControlToOverlayID="UpdatePanel1" TargetControlID="UpdateProgress1" CssClass="updateProgressbtn" />--%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </div>
</div>
<%--<asp:UpdateProgress ID="updProgressRawMediaResults" runat="server" AssociatedUpdatePanelID="upGrid"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<asp:UpdateProgress ID="updSearchTermsProgress" runat="server" AssociatedUpdatePanelID="updSearchTerms"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="updProgressExtenderRawMediaResults"
    ControlToOverlayID="upGrid" TargetControlID="updProgressRawMediaResults" CssClass="updateProgress" />
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="updSearchTerms" TargetControlID="updSearchTermsProgress"
    CssClass="updateProgress" />
--%>