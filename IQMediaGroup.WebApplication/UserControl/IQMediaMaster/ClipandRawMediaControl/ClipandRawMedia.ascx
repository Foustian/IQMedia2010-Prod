<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClipandRawMedia.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.ClipandRawMediaControl.ClipandRawMedia" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%@ Register Src="../BasicClipPlayer/BasicClipPlayer.ascx" TagName="BasicClipPlayer"
    TagPrefix="uc2" %>
<select id="ddlCategories" visible="false" disabled="disabled" style="display: none;">
    <option></option>
</select>
<div class="iq-agent-main">
    <div class="iq-agent-left">
        <div class="ag-gray-main">
            <div class="ag-blue-title">
                iQ media Clipper</div>
            <asp:UpdatePanel ID="upRawMediaClip" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="ag-lin-main">
                        <div id="media" class="searchtab-active" onclick="viewtab(1);">
                            Search Media</div>
                        <div id="clips" class="searchtab" onclick="viewtab(2);">
                            Search Clips</div>
                        <div id="searchmedia" style="clear: both; border: 1px solid #CCDDED; padding: 9px;">
                            <asp:Panel ID="pnlSearchMedia" runat="server" Width="100%" DefaultButton="btnSearchRawMedia">
                                <table border="0" align="center" cellpadding="0" width="100%" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="vsIQMediaSearch" EnableClientScript="true" runat="server"
                                                ValidationGroup="IQMediaSearch" ForeColor="#bd0000" Font-Size="Smaller" Style="text-align: center;" />
                                            <asp:Label ID="lblRawMediaMsg" runat="server" ForeColor="#bd0000"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSearchMediaText" runat="server" MaxLength="255" CssClass="grayinput" style="width:200px;"></asp:TextBox>
                                            <cc1:PropertyProxyValidator ID="pplSearchMyclip" runat="server" ControlToValidate="txtSearchMediaText"
                                                Text="*" PropertyName="SearchText" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                RulesetName="RawMediaDate" ValidationGroup="IQMediaSearch" Font-Size="Smaller"
                                                Display="Dynamic" DisplayMode="List"></cc1:PropertyProxyValidator>
                                            <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtSearchMediaText" TargetControlID="txtSearchMediaText"
                                                WatermarkCssClass="grayinput" WatermarkText="Search Text" runat="server">
                                            </AjaxToolkit:TextBoxWatermarkExtender>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSearchRawMedia" runat="server" CssClass="btn-blue2" Text="Search"
                                                ValidationGroup="IQMediaSearch" OnClick="btnSearchRawMedia_Click" />
                                            <br />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                        <div id="searchclips" style="display: none; clear: both; border: 1px solid #CCDDED;
                            padding: 9px;">
                            <asp:Panel ID="pnlSearchClip" runat="server" DefaultButton="btnSearchClip" Width="100%">
                                <table border="0" align="center" cellpadding="0" cellspacing="0" width="100%">
                                    <tr>
                                        <td>
                                            <asp:ValidationSummary ID="ValidationSummary3" EnableClientScript="true" runat="server"
                                                ValidationGroup="IQMediaSearchRawMedia" ForeColor="#bd0000" Font-Size="Smaller"
                                                Style="text-align: center;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSearchText" runat="server" MaxLength="255" CssClass="grayinput"  style="width:200px;"></asp:TextBox>
                                            <AjaxToolkit:TextBoxWatermarkExtender ID="txtSearchTextClip" TargetControlID="txtSearchText"
                                                WatermarkCssClass="grayinput" WatermarkText="Search Text" runat="server">
                                            </AjaxToolkit:TextBoxWatermarkExtender>
                                            <cc1:PropertyProxyValidator ID="pplSearcClip" runat="server" ControlToValidate="txtSearchText"
                                                Text="*" PropertyName="SearchText" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                RulesetName="SearchClips" ValidationGroup="IQMediaSearchRawMedia" Font-Size="Smaller"
                                                Display="Dynamic" DisplayMode="List"></cc1:PropertyProxyValidator>
                                        </td>
                                        <td align="right">
                                            <asp:Button ID="btnSearchClip" runat="server" CssClass="btn-blue2" Text="Search"
                                                ValidationGroup="IQMediaSearchRawMedia" OnClick="btnSearchClip_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </div>
                    </div>
                    <div class="result-bg">
                        <asp:Label ID="lblSearchResult" runat="server" Text="Search Results"></asp:Label>
                    </div>
                    <div class="ag-lin-main">
                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td>
                                    <%--height="24"--%>
                                    <asp:HiddenField ID="hfIsTimeOut" runat="server" />
                                    <asp:HiddenField ID="RawMediaID" runat="server" />
                                    <asp:Label ID="lblTimeOutMsg" runat="server" Style="display: none;"></asp:Label>
                                    <div id="rawMedia">
                                        <asp:Panel ID="pnlClip" runat="server" ScrollBars="Auto" Visible="false">
                                            <asp:GridView ID="grvClip" runat="server" Width="100%" border="0" CellPadding="5"
                                                CellSpacing="0" BorderColor="#e4e4e4" PagerSettings-Mode="NextPrevious" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" PageSize="10"
                                                Style="border-collapse: collapse;" AutoGenerateColumns="false" EmptyDataText="No Results Found"
                                                AllowPaging="true" OnPageIndexChanging="grvClip_PageIndexChanging" AllowSorting="true"
                                                OnSorting="grvClip_Sorting" CssClass="grid" HeaderStyle-CssClass="grid-th">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <img id="Img2" runat="Server" src='<%# Bind("ClipLogo") %>' width="23" border="0" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="35px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="35px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="ClipTitle" HeaderText="Title" ItemStyle-CssClass="heading-blue"
                                                        HeaderStyle-HorizontalAlign="Left" SortExpression="ClipTitle" ItemStyle-Width="40%">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="55px" />
                                                        <HeaderStyle CssClass="grid-th" Width="55px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ClipCreationDate" HeaderText="Clip Creation Date" ItemStyle-CssClass="content-text-new"
                                                        HeaderStyle-HorizontalAlign="Left" SortExpression="ClipCreationDate" ItemStyle-Width="25%">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="50px" />
                                                        <HeaderStyle CssClass="grid-th" Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="ClipDate" HeaderText="Clip Air Date" ItemStyle-CssClass="content-text-new"
                                                        HeaderStyle-HorizontalAlign="Left" SortExpression="ClipDate" ItemStyle-Width="25%">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="50px" />
                                                        <HeaderStyle CssClass="grid-th" Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandArgument='<%# Bind("ClipID") %>'
                                                                OnCommand="lbtnPlay_Command" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="25px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="25" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <PagerStyle CssClass="heading-blue" />
                                            </asp:GridView>
                                        </asp:Panel>
                                        <br />
                                    </div>
                                    <div id="divTimeOut">
                                        <asp:Panel ID="pnl" runat="server" ScrollBars="None" Visible="false">
                                            <div style="font-weight: bold; padding: 5px 5px 5px 0px;">
                                                <span>Total Programs Found = </span>
                                                <asp:Label ID="lblNoOfRawMedia" runat="server"></asp:Label>
                                            </div>
                                            <br />
                                            <asp:HiddenField ID="hfOffsetValue" runat="server" />
                                            <asp:GridView ID="grvRawMedia" runat="server" AutoGenerateColumns="false" Width="100%"
                                                border="0" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4" Style="border-collapse: collapse;"
                                                AllowSorting="true" OnSorting="grvRawMedia_Sorting" EmptyDataText="No Results Found"
                                                OnRowCommand="grvRawMedia_RowCommand" CssClass="grid" HeaderStyle-CssClass="grid-th">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgStationLogo" ImageUrl='<%# Eval("StationLogo") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="38px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="38px" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="Program" DataField="Title120">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="45px" />
                                                        <HeaderStyle CssClass="grid-th" Width="45px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Market" HeaderText="Market" SortExpression="market">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="50px" />
                                                        <HeaderStyle CssClass="grid-th" Width="50px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="DateTime" HeaderText="DateTime" SortExpression="datetime">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="70px" />
                                                        <HeaderStyle CssClass="grid-th" Width="70px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Hits" HeaderText="Hits">
                                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Center" Width="28px" />
                                                        <HeaderStyle CssClass="grid-th" Width="28px" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandName="PlayVideo"
                                                                CommandArgument='<%# Eval("RawMediaID") %>' OnClientClick="showdivPlayer();" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="33px" />
                                                        <ItemStyle HorizontalAlign="Center" Width="33px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <div style="padding-top: 10px; padding-bottom: 10px;">
                                                <table width="100%">
                                                    <tr>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <asp:ImageButton ID="imgRawMediaPrevious" runat="server" AlternateText="Previous"
                                                                            OnClick="imgRawMediaPrevious_Click" ImageUrl="~/Images/arrow-previous.jpg" />
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblCurrentPageNo" Font-Bold="true" runat="server"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="imgRawMediaNext" runat="server" AlternateText="Next" OnClick="imgRawMediaNext_Click"
                                                                            ImageUrl="~/Images/arrow-next.jpg" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                            <%--<asp:ImageButton ID="imgRawMediaPrevious" runat="server" AlternateText="Previous"
                                                                OnClick="imgRawMediaPrevious_Click" ImageUrl="~/Images/arrow-previous.jpg" />--%>
                                                            <span style="vertical-align: top;">
                                                                <%-- <asp:Label ID="lblCurrentPageNo" Font-Bold="true" runat="server"></asp:Label>--%>
                                                            </span>
                                                            <%-- <asp:ImageButton ID="imgRawMediaNext" runat="server" AlternateText="Next" OnClick="imgRawMediaNext_Click"
                                                                ImageUrl="~/Images/arrow-next.jpg" />--%>
                                                        </td>
                                                        <%--<td align="right" runat="server" id="divtxtplay" valign="top" class="contenttext-small">
                                                            Click on play button to play video
                                                        </td>--%>
                                                    </tr>
                                                </table>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="iq-agent-right">
        <div class="ag-gray-main">
            <div class="div-box" id="divVideo" style="display: none;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <iframe id="Clipframe" runat="server" style="width: 581px; height: 730px; display: none;"
                                        scrolling="no" marginwidth="0" marginheight="0" hspace="0" vspace="0" border="0"
                                        frameborder="0"></iframe>
                                    <div id="divClip" runat="server" style="display: none;">
                                        <uc2:BasicClipPlayer ID="ClipPlayer2" runat="server" />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
    </div>
</div>
<%--<asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="upRawMediaClip"
    DisplayAfter="0" >
    <ProgressTemplate>
        <div style="background-color:gray;filter:alpha(opacity=70);opacity:0.7;width:100%;height:100%;">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<%--<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="upRawMediaClip" TargetControlID="UpdateProgress1" CssClass="updateProgress" />--%>
