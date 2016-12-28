<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQPremium.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQPremium.IQPremium" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="../CustomPager/CustomPager.ascx" TagName="CustomPager" TagPrefix="uc" %>
<%@ Register Src="../IframeRawMediaH/IframeRawMediaH.ascx" TagName="IframeRawMediaH"
    TagPrefix="uc1" %>
<div class="search-box-bg">
    <asp:UpdatePanel ID="upMainSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="form-search margintop8">
                <div class="input-append width100p">
                    <asp:TextBox ID="txtSearch" runat="server" CssClass="span2 search-query"></asp:TextBox>
                    <asp:Button ID="btnSearch" CausesValidation="false" runat="server" Text="Search"
                        CssClass="btn height30" OnClick="btnSearch_click" OnClientClick="return RemoveExportChartComponent();" />
                    <a id="aSearchTips" target="_blank" title="Search Tips" runat="server" class="InformationTag">
                        <img src="../images/info.png" alt="Search Tips"></a>
                </div>
            </div>
            <br />
            <div class="search-nav" id="divHeaderFilter">
                <asp:CheckBox ID="chkTV" runat="server" onclick="ShowHideFilterOnCheckbox('TV',this)"
                    Checked="true" />
                <a href="javascript:;" class="search-link">TV</a>
                <asp:CheckBox ID="chkRadio" runat="server" onclick="ShowHideFilterOnCheckbox('RADIO',this)" />
                <a href="javascript:;" id="aRadio" runat="server" class="search-link">Radio</a>
                <asp:CheckBox ID="chkNews" runat="server" onclick="ShowHideFilterOnCheckbox('ONLINENEWS',this)" />
                <a href="javascript:;" id="aOnlineNews" runat="server" class="search-link">Online News</a>
                <asp:CheckBox ID="chkSocialMedia" runat="server" onclick="ShowHideFilterOnCheckbox('SOCIALMEDIA',this)" />
                <a href="javascript:;" id="aSocialMedia" runat="server" class="search-link">Social Media</a>
                <asp:CheckBox ID="chkTwitter" runat="server" onclick="ShowHideFilterOnCheckbox('TWITTER',this)" />
                <a href="javascript:;" id="aTwitter" runat="server" class="search-link">Twitter</a>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<br />
<div class="clear">
    <div class="span3">
        <div class="well sidebar-nav" id="divFilterSide">
            <asp:UpdatePanel ID="upSavedSearh" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <ul class="nav nav-list marginleft-28">
                        <li class="ulheader">
                            <div class="lidivLeft">
                                &nbsp;&nbsp;SEARCH</div>
                            <div class="marginleft72p">
                                <asp:Button ID="btnResetAll" runat="server" CssClass="btn-blue2" Text="Reset All"
                                    OnClick="btnResetAll_click" /></div>
                        </li>
                    </ul>
                    <div id="divMainSearch" class="marginbottom5">
                        <ul class="nav nav-list" id="search-list">
                            <li class="ulhead" onclick="showHidefilterul('liSavedSearch');">
                                <div class="float-left">
                                    Saved Search</div>
                            </li>
                            <li id="liSavedSearch" class="clear display-none">
                                <asp:Label ID="lblSavedSearchmsg" runat="server" Visible="false" Font-Size="13px"></asp:Label><br />
                                <asp:Label ID="lblIQAgentmsg" runat="server" Visible="false" Font-Size="13px"></asp:Label>
                                <div class="clear">
                                    <asp:GridView ShowFooter="true" Width="100%" ID="gvSavedSearch" runat="server" AutoGenerateColumns="false"
                                        CellPadding="0" CellSpacing="0" AllowSorting="true" AllowPaging="true" PageSize="5"
                                        SelectedRowStyle-BackColor="#C1D72E" EmptyDataText="No Results Found" CssClass="grid grid-iq"
                                        OnRowCommand="gvSavedSearch_Command" OnDataBound="gvSavedSearch_DataBound" OnRowDataBound="gvSavedSearch_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <b>Title</b> - Click on Title to Load Search Settings
                                                </HeaderTemplate>
                                                <FooterTemplate>
                                                    <div class="float-left">
                                                        <asp:LinkButton CausesValidation="false" ID="btnFirst" runat="server" Text="<<" CommandName="Pager"
                                                            CommandArgument="First"></asp:LinkButton>
                                                        <asp:LinkButton CausesValidation="false" ID="btnPrev" CssClass="paddingleft4" runat="server"
                                                            Text="<" CommandName="Pager" CommandArgument="Prev"></asp:LinkButton>
                                                    </div>
                                                    <div class="float-right">
                                                        <asp:LinkButton CausesValidation="false" ID="btnNext" CssClass="paddingright4" runat="server"
                                                            Text=">" CommandName="Pager" CommandArgument="Next"></asp:LinkButton>
                                                        <asp:LinkButton CausesValidation="false" ID="btnLast" runat="server" Text=">>" CommandName="Pager"
                                                            CommandArgument="Last"></asp:LinkButton>
                                                    </div>
                                                </FooterTemplate>
                                                <ItemTemplate>
                                                    <div class="savesearch-title">
                                                        <asp:LinkButton ID="lnkbtnTitle" CausesValidation="false" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                            Text='<%# Eval("Title") %>' CommandName="LoadSearch"></asp:LinkButton>
                                                        <asp:HiddenField ID="hfSavedSearchCustomerGUID" runat="server" Value='<%# Eval("CustomerGUID") %>' />
                                                    </div>
                                                    <div class="float-right">
                                                        <img alt="I" src="~/Images/icon_clock2.png" runat="server" id="imgIQAgent" />
                                                        <%--<input id="imgIQAgent" type="image" alt="I" src="~/Images/icon_clock1.png" runat="server" />--%>
                                                        <asp:ImageButton CausesValidation="false" ImageUrl="~/Images/icon-view.png" ID="btnLoadSavedSearch"
                                                            runat="server" CommandName="LoadSearch" CommandArgument='<%# Eval("ID") %>' AlternateText="L" />
                                                        <asp:ImageButton CausesValidation="false" ImageUrl="~/Images/icon-edit.png" ID="btnEditSavedSearch"
                                                            runat="server" CommandName="EditSearch" CommandArgument='<%# Eval("ID") %>' AlternateText="E" />
                                                        <asp:ImageButton CausesValidation="false" ImageUrl="~/Images/icon-delete.png" ID="btnRemoveSavedSearch"
                                                            runat="server" CommandName="DeleteSearch" CommandArgument='<%# Eval("ID") %>'
                                                            AlternateText="X" />
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle CssClass="left" />
                                                <HeaderStyle CssClass="grid-th-left" Font-Bold="false" Width="100%" />
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnResetAll" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upTVFilter" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnTVData" Value="0" runat="server" />
                    <div id="divMainTVFilter" runat="server" visible="false" class="marginbottom5">
                        <ul class="nav nav-list listBorder filterul" id="ulTV">
                            <li class="ulhead" onclick="showHidefilterul('liTVInnerFitler')">
                                <div class="float-left">
                                    TV Filter
                                </div>
                                <div class="lidivRightHeader">
                                    <div id="divTVFilterStatus">
                                        OFF</div>
                                </div>
                            </li>
                            <li id="liTVInnerFitler" class="display-none">
                                <ul class="nav">
                                    <li id="liTimeFilter" class="searchListLiBorder">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetTimeFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgTimeFilter" />&nbsp;&nbsp;Time
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divTimeFilterStatus">
                                                        OFF</div>
                                                    <img id="imgShowTimeFilter" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divTimeFilter" class="bs-docs-example">
                                                <asp:RadioButton ID="rbDuration1" Checked="true" Text="Duration" runat="server" GroupName="Duration"
                                                    onclick="DisplayInterval(false);" /><br />
                                                <span class="help-inline">Previous: </span>
                                                <asp:DropDownList ID="ddlDuration" CssClass="span2 iq-duration" runat="server">
                                                    <asp:ListItem Text="24 Hours" Value="day,1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="One Week" Value="day,7"></asp:ListItem>
                                                    <asp:ListItem Text="One Month" Value="month,1"></asp:ListItem>
                                                    <asp:ListItem Text="3 Months" Value="month,3"></asp:ListItem>
                                                    <asp:ListItem Text="6 Months" Value="month,6"></asp:ListItem>
                                                    <asp:ListItem Text="1 Year" Value="year,1"></asp:ListItem>
                                                    <asp:ListItem Text="All" Value="all"></asp:ListItem>
                                                </asp:DropDownList>
                                                <br />
                                                <div class="paddingtop15">
                                                    <asp:RadioButton ID="rbDuration2" Text="Interval" runat="server" GroupName="Duration"
                                                        onclick="DisplayInterval(true);" /><%--</label>--%>
                                                </div>
                                                <div id="divInterval" class="display-none">
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                Start Time:</label></div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtStartDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="pplFromDate" runat="server" ControlToValidate="txtStartDate"
                                                                Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <asp:CustomValidator ID="cusFromDate" runat="server" Display="Dynamic" ControlToValidate="txtStartDate"
                                                                ValidationGroup="AdvancedSearch" OnServerValidate="CVFromDate_ServerValidate"
                                                                Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                            <AjaxToolkit:CalendarExtender BehaviorID="CalendarExtenderFromDate" Format="MM/dd/yyyy"
                                                                ID="CalendarExtenderFromDate" runat="server" CssClass="MyCalendar" TargetControlID="txtStartDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlStartTime" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rdAmPmFromDate" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                End Time:</label>
                                                        </div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtEndDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="pplEndDate" runat="server" ControlToValidate="txtEndDate"
                                                                Text="*" PropertyName="SearchEndDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <AjaxToolkit:CalendarExtender BehaviorID="CalendarExtenderToDate" Format="MM/dd/yyyy"
                                                                ID="CalendarExtenderToDate" runat="server" CssClass="MyCalendar" TargetControlID="txtEndDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlEndTime" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rdAMPMToDate" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <label>
                                                        Time Zone:</label><br />
                                                    <asp:DropDownList ID="ddlTimeZone" runat="server" CssClass="span2" Width="45%">
                                                        <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                                                        <asp:ListItem Text="EST" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="CST" Value="2"></asp:ListItem>
                                                        <asp:ListItem Text="MST" Value="3"></asp:ListItem>
                                                        <asp:ListItem Text="PST" Value="4"></asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liProgramFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetProgramFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgProgramFilter" />&nbsp;&nbsp;Program Detail
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divProgramFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowProgramFilter" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divProgramTitle" class="bs-docs-example">
                                                Program Title:
                                                <asp:TextBox ID="txProgramTitle" class="programTextbox" runat="server"></asp:TextBox>
                                                Appearing:
                                                <asp:TextBox ID="txtAppearing" class="programTextbox" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liMarketFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetMarketFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgMarketFilter" />&nbsp;&nbsp;Market
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divMarketFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowMarketFilter" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divMarketFilter" class="bs-docs-example">
                                                <asp:DropDownList ID="ddlMarket" CssClass="MarketDropdown" runat="server">
                                                    <asp:ListItem Text="Region" Value="Region" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="Rank" Value="Rank"></asp:ListItem>
                                                </asp:DropDownList>
                                                <div id="divMarketRankFilter">
                                                    <img id="imgExpandRankFilter" src="../images/collapse_icon.png" alt="" onclick="OnImageExpandCollapseClick('imgExpandRankFilter', 'divRankFilterMaster');" /><asp:CheckBox
                                                        ClientIDMode="Static" ID="chkRankFIlterSelectAll" runat="server" Text="Select All"
                                                        Checked="true" />
                                                    <div id="divRankFilterMaster" class="div-subchecklist">
                                                        <input type="checkbox" runat="server" id="chkNational" value="National" clientidmode="Static"
                                                            class="marginleft20" />National
                                                        <br />
                                                        <img id="imbExpandTop10" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop10', 'divDmaTop10');" /><asp:CheckBox
                                                            ID="chkTop10" ClientIDMode="Static" runat="server" />1 to 10
                                                        <div id='divDmaTop10' class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop10" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop20" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop20', 'divDmaTop20');" /><asp:CheckBox
                                                            ID="chkTop20" ClientIDMode="Static" runat="server" />11 to 20
                                                        <div id='divDmaTop20' class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop20" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop30" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop30', 'divDmaTop30');" /><asp:CheckBox
                                                            ID="chkTop30" ClientIDMode="Static" runat="server" />21 to 30
                                                        <div id='divDmaTop30' class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop30" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop40" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop40', 'divDmaTop40');" /><asp:CheckBox
                                                            ID="chkTop40" ClientIDMode="Static" runat="server" />31 to 40
                                                        <div id='divDmaTop40' class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop40" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop50" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop50', 'divDmaTop50');" /><asp:CheckBox
                                                            ID="chkTop50" ClientIDMode="Static" runat="server" />41 to 50
                                                        <div id='divDmaTop50' class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop50" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop60" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop60', 'divDmaTop60');" /><asp:CheckBox
                                                            ID="chkTop60" ClientIDMode="Static" runat="server" />51 to 60
                                                        <div id="divDmaTop60" class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop60" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop80" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop80', 'divDmaTop80');" /><asp:CheckBox
                                                            ID="chkTop80" ClientIDMode="Static" runat="server" />61 to 80
                                                        <div id="divDmaTop80" class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop80" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop100" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop100', 'divDmaTop100');" /><asp:CheckBox
                                                            ID="chkTop100" ClientIDMode="Static" runat="server" />81 to 100
                                                        <div id="divDmaTop100" class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop100" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop150" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop150', 'divDmaTop150');" /><asp:CheckBox
                                                            ID="chkTop150" ClientIDMode="Static" runat="server" />101 to 150
                                                        <div id="divDmaTop150" class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop150" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                        <img id="imbExpandTop210" src="../images/expand_icon.png" alt="" onclick="OnImageExpandCollapseClick('imbExpandTop210', 'divDmaTop210');" /><asp:CheckBox
                                                            ID="chkTop210" ClientIDMode="Static" runat="server" />151 to 210
                                                        <div id="divDmaTop210" class="div-subchecklist">
                                                            <asp:Repeater ID="rptTop210" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear">
                                                                        <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Num").ToString()%>' /><%# Eval("Name_Num")%>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                        <br />
                                                    </div>
                                                </div>
                                                <div id="divMainRegionFilter">
                                                    <img id="imgRegionSelectAll" src="../images/collapse_icon.png" alt="" onclick="OnImageExpandCollapseClick('imgRegionSelectAll', 'divRegionFilter');" /><asp:CheckBox
                                                        ClientIDMode="Static" ID="chlkRegionSelectAll" runat="server" />Select All<br />
                                                    <div id="divRegionFilter" class="div-subchecklist">
                                                        <input type="checkbox" id="chkRegionNational" runat="server" class="marginleft20"
                                                            value="National" />National<br />
                                                        <asp:Repeater ID="rptregion" runat="server">
                                                            <ItemTemplate>
                                                                <img id="imgRegion_<%# Eval("ID")%>" src="../images/expand_icon.png" alt="" /><asp:CheckBox
                                                                    ID="chkRegion" runat="server" /><%# Eval("Name") %>
                                                                <div id="divCheckBox_<%# Eval("ID")%>" class="sublist display-none">
                                                                    <asp:Repeater ID="rptDma" runat="server">
                                                                        <ItemTemplate>
                                                                            <div class="clear">
                                                                                <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("IQ_Dma_Name").ToString() + "#" +  Eval("IQ_Dma_Num").ToString() %>' /><%# Eval("Name_Num")%>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                    </asp:Repeater>
                                                                </div>
                                                                <br />
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liCategoryFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetCategoryFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgCategoryFilter" />&nbsp;&nbsp;Category
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divCategoryFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowCategoryFilter" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divCategoryFilter" class="bs-docs-example">
                                                <img id="CategoryExpandimg" src="../images/collapse_icon.png" onclick="OnImageExpandCollapseClick('CategoryExpandimg','divCategory');"
                                                    alt="" /><asp:CheckBox ID="chkCategoryAll" runat="server" Text="Select All" Checked="true" />
                                                <div id="divCategory" class="div-subchecklist">
                                                    <asp:Repeater ID="rptCategory" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkClass" runat="server" value='<%# Eval("IQ_Class_Num").ToString() %>' /><asp:Label
                                                                    ID="lblClass" runat="server" Text='<%# Eval("IQ_Class")%>'></asp:Label>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liStationFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetStationFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgStatusFilter">&nbsp;&nbsp;Station
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divStationFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowStationFilter" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divStationFilter" class="bs-docs-example">
                                                <div class="clear">
                                                </div>
                                                <img id="affilExpandImg" src="../images/collapse_icon.png" alt="" onclick="OnImageExpandCollapseClick('affilExpandImg','divAffil');" /><asp:CheckBox
                                                    ID="chkAffilAll" ClientIDMode="Static" runat="server" Text="Select All" Checked="true" />
                                                <div id="divAffil" class="div-subchecklist">
                                                    <asp:Repeater ID="rptTVStationSubMaster" runat="server">
                                                        <ItemTemplate>
                                                            <img id="imgTVFilterSecectSubMaster_<%# Container.DataItem %>" src="../images/expand_icon.png"
                                                                alt="" /><asp:CheckBox ID="chkStationSubMaster" runat="server" Text="" /><asp:HiddenField
                                                                    ID="hfAffilNum" runat="server" />
                                                            <div id="SdivCheckBox_<%# Container.DataItem %>" class="sublist display-none">
                                                                <asp:Repeater ID="rptTVStationChild" runat="server">
                                                                    <ItemTemplate>
                                                                        <div class="clear">
                                                                            <input type="checkbox" id="chkTVStation" runat="server" value='<%# Eval("IQ_Station_ID").ToString()%>' /><%# Eval("Station_Call_Sign")%>
                                                                        </div>
                                                                    </ItemTemplate>
                                                                </asp:Repeater>
                                                            </div>
                                                            <br />
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upRadioFilter" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnRadioData" Value="0" runat="server" />
                    <div id="divMainRadioFilter" runat="server" visible="false" class="marginbottom5">
                        <ul class="nav nav-list listBorder filterul" id="ulRadio">
                            <li class="ulhead" onclick="showHidefilterul('liRadioInnerFitler')">
                                <div class="float-left">
                                    Radio Filter
                                </div>
                                <div class="lidivRightHeader">
                                    <div id="divRadioFilterStatus">
                                        OFF</div>
                                </div>
                            </li>
                            <li id="liRadioInnerFitler" class="display-none">
                                <ul class="nav">
                                    <li id="liRadioTimeFilter" class="searchListLiBorder">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetRadioTimeFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowRadioTimeFilter" />&nbsp;&nbsp;Time
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divRadioTimeFilterStatus">
                                                        OFF</div>
                                                    <img id="imgShowRadioTime" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divRadioTimeFilter" class="bs-docs-example clear">
                                                <asp:RadioButton ID="rbRadioDuration" Checked="true" Text="Duration" runat="server"
                                                    GroupName="RadioDuration" onclick="DisplayRadioInterval(false);" /><br />
                                                <span class="help-inline">Previous: </span>
                                                <asp:DropDownList ID="ddlRadioDuration" CssClass="span2 iq-duration" runat="server">
                                                    <asp:ListItem Text="24 Hours" Value="day,1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="One Week" Value="day,7"></asp:ListItem>
                                                    <asp:ListItem Text="One Month" Value="month,1"></asp:ListItem>
                                                    <asp:ListItem Text="3 Months" Value="month,3"></asp:ListItem>
                                                    <asp:ListItem Text="6 Months" Value="month,6"></asp:ListItem>
                                                    <asp:ListItem Text="1 Year" Value="year,1"></asp:ListItem>
                                                    <asp:ListItem Text="All" Value="all"></asp:ListItem>
                                                </asp:DropDownList>
                                                <div class="paddingtop15">
                                                    <asp:RadioButton ID="rbRadioInterval" Text="Interval" runat="server" GroupName="RadioDuration"
                                                        onclick="DisplayRadioInterval(true);" />
                                                </div>
                                                <div class="minheight106 display-none" id="divRadioInterval">
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                Start Time:</label></div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtRadioStartDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator7" runat="server" ControlToValidate="txtRadioStartDate"
                                                                Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <asp:CustomValidator ID="CustomValidator4" runat="server" Display="Dynamic" ControlToValidate="txtRadioStartDate"
                                                                ValidationGroup="AdvancedSearch" OnServerValidate="CVFromDate_ServerValidate"
                                                                Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                            <AjaxToolkit:CalendarExtender BehaviorID="ceRadioFromDate" Format="MM/dd/yyyy" ID="CalendarExtender7"
                                                                runat="server" CssClass="MyCalendar" TargetControlID="txtRadioStartDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlRadioStartHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbRadioStart" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                End Time:</label>
                                                        </div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtRadioEndDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator8" runat="server" ControlToValidate="txtRadioEndDate"
                                                                Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <asp:CustomValidator ID="CustomValidator5" runat="server" Display="Dynamic" ControlToValidate="txtRadioEndDate"
                                                                ValidationGroup="AdvancedSearch" OnServerValidate="CVFromDate_ServerValidate"
                                                                Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                            <AjaxToolkit:CalendarExtender BehaviorID="ceRadioEndDate" Format="MM/dd/yyyy" ID="CalendarExtender8"
                                                                runat="server" CssClass="MyCalendar" TargetControlID="txtRadioEndDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlRadioEndHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbRadioEnd" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li>
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetRadioMarketFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowRadioMarketFilter" />&nbsp;&nbsp;Market
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divRadioMarketFilterStatus">
                                                        OFF</div>
                                                    <img id="imgShowRadioDMA" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divRadioMarketFilter" class="bs-docs-example clear">
                                                <img id="imgRadioMarketAll" src="../images/collapse_icon.png" alt="" onclick="OnImageExpandCollapseClick('imgRadioMarketAll', 'divRadioMarket');" /><asp:CheckBox
                                                    ID="chkRadioMarketAll" runat="server" Text="Select All" Checked="true" />
                                                <div id="divRadioMarket" class="div-subchecklist">
                                                    <asp:Repeater ID="rptRadioMarket" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkDma" runat="server" value='<%# Eval("dma_Name").ToString() + "#" +  Eval("dma_Num").ToString() %>' />
                                                                <%# Eval("dma_Name")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upNewsFilter" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnONLINENEWSData" Value="0" runat="server" />
                    <div id="divMainOnlineNewsFilter" runat="server" visible="false" class="marginbottom5">
                        <ul class="nav nav-list listBorder filterul" id="ulNews">
                            <li class="ulhead" onclick="showHidefilterul('liOnlineNewsInnerFitler')">
                                <div class="float-left">
                                    Online News Filter
                                </div>
                                <div class="lidivRightHeader">
                                    <div id="divOnlineNewsFilterStatus">
                                        OFF</div>
                                </div>
                            </li>
                            <li id="liOnlineNewsInnerFitler" class="display-none">
                                <ul class="nav">
                                    <li id="liNewsTimeFilter" class="searchListLiBorder ">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetNewsTimeFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgNewsTimeFilter" />&nbsp;&nbsp;Time
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divNewsTimeFilterStatus">
                                                        OFF</div>
                                                    <img id="imgShowNewsTime" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divNewsTimeFilter" class="bs-docs-example clear">
                                                <asp:RadioButton ID="rbNewsDuration" Checked="true" Text="Duration" runat="server"
                                                    GroupName="NewsDuration" onclick="DisplayNewsInterval(false);" /><br />
                                                <span class="help-inline">Previous: </span>
                                                <asp:DropDownList ID="ddlNewsDuration" CssClass="span2 iq-duration" runat="server">
                                                    <asp:ListItem Text="24 Hours" Value="day,1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="One Week" Value="day,7"></asp:ListItem>
                                                    <asp:ListItem Text="One Month" Value="month,1"></asp:ListItem>
                                                    <asp:ListItem Text="3 Months" Value="month,3"></asp:ListItem>
                                                    <asp:ListItem Text="6 Months" Value="month,6"></asp:ListItem>
                                                    <asp:ListItem Text="1 Year" Value="year,1"></asp:ListItem>
                                                    <asp:ListItem Text="All" Value="all"></asp:ListItem>
                                                </asp:DropDownList>
                                                <div class="paddingtop15">
                                                    <asp:RadioButton ID="rbNewsInterval" Text="Interval" runat="server" GroupName="NewsDuration"
                                                        onclick="DisplayNewsInterval(true);" />
                                                </div>
                                                <div class="minheight106 display-none" id="divNewsInterval">
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                Start Time:</label></div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtNewsStartDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator1" runat="server" ControlToValidate="txtNewsStartDate"
                                                                Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <asp:CustomValidator ID="CustomValidator1" runat="server" Display="Dynamic" ControlToValidate="txtNewsStartDate"
                                                                ValidationGroup="AdvancedSearch" OnServerValidate="CVFromDate_ServerValidate"
                                                                Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender1" BehaviorID="ceNewsStartDate"
                                                                runat="server" CssClass="MyCalendar" TargetControlID="txtNewsStartDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlNewsStartHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbNewsStart" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                End Time:</label>
                                                        </div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtNewsEndDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator2" runat="server" ControlToValidate="txtNewsEndDate"
                                                                Text="*" PropertyName="SearchEndDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender2" BehaviorID="ceNewsEndDate" runat="server"
                                                                CssClass="MyCalendar" TargetControlID="txtNewsEndDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlNewsEndHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbNewsEnd" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <br />
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liNewsPublication">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetPublicationFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowNewsPublicationFilter" />&nbsp;&nbsp;Source
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divNewsPublicationStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowNewsPublication" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divNewsPublication" class="bs-docs-example">
                                                Publication:
                                                <asp:TextBox ID="txtNewsPublication" class="programTextbox" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liNewsCategoryFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetNewsCategoryFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowNewsCategoryFilter" />&nbsp;&nbsp;News
                                                    Category
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divNewsCategoryStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowNewsCategory" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divNewsCategory" class="bs-docs-example">
                                                <img id="imgExpandNewsCategory" class="clear" src="../images/collapse_icon.png" alt=""
                                                    onclick="OnImageExpandCollapseClick('imgExpandNewsCategory', 'divNewsCategoryInner');" /><asp:CheckBox
                                                        ID="chkNewsCategorySelectAll" runat="server" Text="Select All" Checked="true"
                                                        onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkNewsCategorySelectAll','divNewsCategoryInner');" />
                                                <div id="divNewsCategoryInner" class="div-subchecklist">
                                                    <asp:Repeater ID="rptNewsCategory" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkNewsCategory" runat="server" value='<%# Eval("Name").ToString() %>' /><%# Eval("Name")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liNewsPublicatinCategory">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetNewsPublicationCategoryFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowNewsPublicationCategoryFilter" />&nbsp;&nbsp;Publication
                                                    Category
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divShowNewsPublicationCategoryStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowNewsPublicationCategory" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divShowNewsPublicationCategory" class="bs-docs-example">
                                                <img id="imgExpandNewsPublicationCategory" class="clear" src="../images/collapse_icon.png"
                                                    alt="" onclick="OnImageExpandCollapseClick('imgExpandNewsPublicationCategory', 'divNewsMainPublicationCategory');" /><asp:CheckBox
                                                        ID="chkNewsPublicationCategory" runat="server" Text="Select All" Checked="true"
                                                        onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkNewsPublicationCategory','divNewsMainPublicationCategory');" />
                                                <div id="divNewsMainPublicationCategory" class="div-subchecklist">
                                                    <asp:Repeater ID="rptNewsPublicationCategory" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkNewsPublicationCategory" runat="server" value='<%# Eval("ID").ToString() %>' /><%# Eval("Name")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liNewsGenre">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetNewsGenreFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgNewsGenreFilter" />&nbsp;&nbsp;Genre
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divNewsGenreFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowNewsGenreFilter" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divNewsGenreFilter" class="bs-docs-example">
                                                <img id="imgNewsGenreExpandimg" class="clear" src="../images/collapse_icon.png" onclick="OnImageExpandCollapseClick('imgNewsGenreExpandimg','divNewsGenre');"
                                                    alt="" /><asp:CheckBox ID="chkNewsGenreSelectAll" runat="server" Text="Select All"
                                                        Checked="true" onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkNewsGenreSelectAll','divNewsGenre');" />
                                                <div id="divNewsGenre" class="div-subchecklist">
                                                    <asp:Repeater ID="rptNewsGenre" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkNewsGenre" runat="server" value='<%# Eval("Label").ToString() %>' /><%# Eval("Label")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liNewsRegion">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetNewsRegionFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgNewsRegionStatusFilter">&nbsp;&nbsp;Region
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divNewsRegionFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowNewsRegion" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divNewsRegionFilter" class="bs-docs-example">
                                                <img id="NewsRegionExpandImg" class="clear" src="../images/collapse_icon.png" alt=""
                                                    onclick="OnImageExpandCollapseClick('NewsRegionExpandImg','divNewsRegion')" /><asp:CheckBox
                                                        ID="chkNewsRegionAll" runat="server" Text="Select All" Checked="true" onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkNewsRegionAll','divNewsRegion');" />
                                                <div id="divNewsRegion" class="div-subchecklist">
                                                    <asp:Repeater ID="rptNewsRegion" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkNewsregion" runat="server" value='<%# Eval("Label").ToString() %>' /><%# Eval("Label")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upSMFilter" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:HiddenField ID="hdnSocialMediaData" Value="0" runat="server" />
                    <div id="divMainSMFilter" runat="server" visible="false" class="marginbottom5">
                        <ul class="nav nav-list listBorder filterul" id="ulSocialMedia">
                            <li class="ulhead" onclick="showHidefilterul('liSMInnerFitler')">
                                <div class="float-left">
                                    Social Media Filter
                                </div>
                                <div class="lidivRightHeader">
                                    <div id="divSMFilterStatus">
                                        OFF</div>
                                </div>
                            </li>
                            <li id="liSMInnerFitler" class="display-none">
                                <ul class="nav">
                                    <li id="liSMTimeFilter" class="searchListLiBorder">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetSMTimeFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgSMTimeFilter" />&nbsp;&nbsp;Time
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divSMTimeFilterStatus">
                                                        OFF</div>
                                                    <img id="imgShowSMTime" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divSMTimeFilter" class="bs-docs-example clear">
                                                <asp:RadioButton ID="rbSMDuration" Checked="true" Text="Duration" runat="server"
                                                    GroupName="SMDuration" onclick="DisplaySMInterval(false);" /><br />
                                                <span class="help-inline">Previous: </span>
                                                <asp:DropDownList ID="ddlSMDuration" CssClass="span2 iq-duration" runat="server">
                                                    <asp:ListItem Text="24 Hours" Value="day,1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="One Week" Value="day,7"></asp:ListItem>
                                                    <asp:ListItem Text="One Month" Value="month,1"></asp:ListItem>
                                                    <asp:ListItem Text="3 Months" Value="month,3"></asp:ListItem>
                                                    <asp:ListItem Text="6 Months" Value="month,6"></asp:ListItem>
                                                    <asp:ListItem Text="1 Year" Value="year,1"></asp:ListItem>
                                                    <asp:ListItem Text="All" Value="all"></asp:ListItem>
                                                </asp:DropDownList>
                                                <div class="paddingtop15">
                                                    <asp:RadioButton ID="rbSMInterval" Text="Interval" runat="server" GroupName="SMDuration"
                                                        onclick="DisplaySMInterval(true);" />
                                                </div>
                                                <div class="minheight106 display-none" id="divSMInterval">
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                Start Time:</label></div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtSMStartDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator3" runat="server" ControlToValidate="txtSMStartDate"
                                                                Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <asp:CustomValidator ID="CustomValidator2" runat="server" Display="Dynamic" ControlToValidate="txtSMStartDate"
                                                                ValidationGroup="AdvancedSearch" Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender3" BehaviorID="ceSMStartDate" runat="server"
                                                                CssClass="MyCalendar" TargetControlID="txtSMStartDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlSMStartHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbSMStart" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                End Time:</label>
                                                        </div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtSMEndDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator4" runat="server" ControlToValidate="txtSMEndDate"
                                                                Text="*" PropertyName="SearchEndDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender4" BehaviorID="ceSMEndDate" runat="server"
                                                                CssClass="MyCalendar" TargetControlID="txtSMEndDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlSMEndHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbSMEnd" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liSMSource">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetSMSourceFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowSMSourceFilter" />&nbsp;&nbsp;Source
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divSMSourceStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowSMSource" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divSMSource" class="bs-docs-example">
                                                Social Media Source:
                                                <asp:TextBox ID="txtSMSource" class="programTextbox" runat="server"></asp:TextBox>
                                                Author:
                                                <asp:TextBox ID="txtSMAuthor" class="programTextbox" runat="server"></asp:TextBox>
                                                Title:
                                                <asp:TextBox ID="txtSMTitle" class="programTextbox" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liSMCategory">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetSMCategoryFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowSMCategoryFilter" />&nbsp;&nbsp;Source
                                                    Category
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divSMCategoryStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowSMCategory" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divSMCategory" class="bs-docs-example">
                                                <img id="imgExpandSMCategory" class="clear" src="../images/collapse_icon.png" alt=""
                                                    onclick="OnImageExpandCollapseClick('imgExpandSMCategory', 'divSMCategoryInner');" /><asp:CheckBox
                                                        ID="chkSMCategorySelectAll" runat="server" Text="Select All" Checked="true" onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkSMCategorySelectAll','divSMCategoryInner');" />
                                                <div id="divSMCategoryInner" class="div-subchecklist">
                                                    <asp:Repeater ID="rptSMCategory" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkSMCategory" runat="server" value='<%# Eval("Value").ToString() %>' /><%# Eval("Lable")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liSMType">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetSMSourceTypeFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowSMTypeFilter" />&nbsp;&nbsp;Source Type
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divSMTypeStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowSMType" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divSMType" class="bs-docs-example">
                                                <img id="imgExpandSMType" class="clear" src="../images/collapse_icon.png" alt=""
                                                    onclick="OnImageExpandCollapseClick('imgExpandSMType', 'divSMTypeInner');" /><asp:CheckBox
                                                        ID="chkSMTypeSelectAll" runat="server" Text="Select All" Checked="true" onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkSMTypeSelectAll','divSMTypeInner');" />
                                                <div id="divSMTypeInner" class="div-subchecklist">
                                                    <asp:Repeater ID="rptSMType" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkSMType" runat="server" value='<%# Eval("Value").ToString() %>' /><%# Eval("Lable")%>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liSMRank">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetSMSourceRankFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgShowSMRankFilter" />&nbsp;&nbsp;Source Rank
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divSMRankStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowSMRank" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divSMRank" class="bs-docs-example">
                                                <img id="imgExpandSMRank" class="clear" src="../images/collapse_icon.png" alt=""
                                                    onclick="OnImageExpandCollapseClick('imgExpandSMRank', 'divSMRankInner');" /><asp:CheckBox
                                                        ID="chkSMRankSelectAll" runat="server" Text="Select All" Checked="true" onclick="checkUncheckChildOnMasterCheckBox('ctl00_Content_Data_ucIQpremium_chkSMRankSelectAll','divSMRankInner');" />
                                                <div id="divSMRankInner" class="div-subchecklist">
                                                    <asp:Repeater ID="rptSMRank" runat="server">
                                                        <ItemTemplate>
                                                            <div class="clear">
                                                                <input type="checkbox" id="chkSMRank" runat="server" value='<%# Container.DataItem %>' /><%# Container.DataItem %>
                                                            </div>
                                                        </ItemTemplate>
                                                    </asp:Repeater>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upTwitterFilter" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMainTwitterFilter" runat="server" class="marginbottom5 display-none">
                        <ul class="nav nav-list listBorder filterul" id="ulTwitter">
                            <li class="ulhead" onclick="showHidefilterul('liTwitterInnerFitler')">
                                <div class="float-left">
                                    Twitter Filter
                                </div>
                                <div class="lidivRightHeader">
                                    <div id="divTwitterFilterStatus">
                                        OFF</div>
                                </div>
                            </li>
                            <li id="liTwitterInnerFitler" class="display-none">
                                <ul class="nav">
                                    <li id="liTwitterTimeFilter" class="searchListLiBorder">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetTwitterTimeFilter();">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgTwitterTimeFilter" />&nbsp;&nbsp;Time
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divTwitterTimeFilterStatus">
                                                        OFF</div>
                                                    <img id="imgShowTwitterTime" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divTwitterTimeFilter" class="bs-docs-example clear">
                                                <asp:RadioButton ID="rbTwitterDuration" Checked="true" Text="Duration" runat="server"
                                                    GroupName="TwitterDuration" onclick="DisplayTwitterInterval(false);" /><br />
                                                <span class="help-inline">Previous: </span>
                                                <asp:DropDownList ID="ddlTwitterDuration" CssClass="span2 iq-duration" runat="server">
                                                    <asp:ListItem Text="24 Hours" Value="day,1" Selected="True"></asp:ListItem>
                                                    <asp:ListItem Text="One Week" Value="day,7"></asp:ListItem>
                                                    <asp:ListItem Text="One Month" Value="month,1"></asp:ListItem>
                                                    <asp:ListItem Text="3 Months" Value="month,3"></asp:ListItem>
                                                    <asp:ListItem Text="6 Months" Value="month,6"></asp:ListItem>
                                                    <asp:ListItem Text="1 Year" Value="year,1"></asp:ListItem>
                                                    <asp:ListItem Text="All" Value="all"></asp:ListItem>
                                                </asp:DropDownList>
                                                <div class="paddingtop15">
                                                    <asp:RadioButton ID="rbTwitterInterval" Text="Interval" runat="server" GroupName="TwitterDuration"
                                                        onclick="DisplayTwitterInterval(true);" />
                                                </div>
                                                <div class="minheight106 display-none" id="divTwitterInterval">
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                Start Time:</label></div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtTwitterStartDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator5" runat="server" ControlToValidate="txtTwitterStartDate"
                                                                Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <asp:CustomValidator ID="CustomValidator3" runat="server" Display="Dynamic" ControlToValidate="txtTwitterStartDate"
                                                                ValidationGroup="AdvancedSearch" Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender5" BehaviorID="ceTwitterStartDate"
                                                                runat="server" CssClass="MyCalendar" TargetControlID="txtTwitterStartDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlTwitterStartHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbTwitterStart" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                    <div>
                                                        <div class="clear">
                                                            <label>
                                                                End Time:</label>
                                                        </div>
                                                        <div class="iq-date-div">
                                                            <asp:TextBox ID="txtTwitterEndDate" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator6" runat="server" ControlToValidate="txtTwitterEndDate"
                                                                Text="*" PropertyName="SearchEndDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                RulesetName="RawMediaDate" ValidationGroup="AdvancedSearch" Font-Size="Smaller"
                                                                Display="Dynamic"></cc1:PropertyProxyValidator>
                                                            <AjaxToolkit:CalendarExtender ID="CalendarExtender6" BehaviorID="ceTwitterEndDate"
                                                                runat="server" CssClass="MyCalendar" TargetControlID="txtTwitterEndDate">
                                                            </AjaxToolkit:CalendarExtender>
                                                        </div>
                                                        <div class="iq-time-div">
                                                            <div class="iq-hour-div">
                                                                <asp:DropDownList ID="ddlTwitterEndHour" runat="server" AutoPostBack="false" CssClass="hourDropDown">
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
                                                                    <asp:ListItem Text="12:00" Value="0" Selected="True"></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </div>
                                                            <div class="iq-ampm-div">
                                                                <asp:RadioButtonList ID="rbTwitterEnd" runat="server" CellPadding="0" CellSpacing="0"
                                                                    BorderWidth="0" RepeatDirection="Vertical" CssClass="paddingleft-5">
                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liTwitterSource">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetTwitterSourceFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgTwitterSourceFilter" />&nbsp;&nbsp;Source
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divTwitterSourceFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowTwitterSource" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divTwitterSource" class="bs-docs-example">
                                                Actor:
                                                <asp:TextBox ID="txtTweetActor" class="programTextbox" runat="server"></asp:TextBox>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liTwitterCounts">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetTwitterCountFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgTwitterCountFilter" />&nbsp;&nbsp;Count
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divTwitterCountFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowTwitterCount" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divTwitterCount" class="bs-docs-example">
                                                <div class="clear">
                                                    <div>
                                                        <label>
                                                            Followers Count:</label></div>
                                                    <div class="iq-date-div">
                                                        <asp:TextBox ID="txtFollowerCountFrom" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="ftetxtFollowerCountFrom" runat="server"
                                                            TargetControlID="txtFollowerCountFrom" ValidChars="1234567890" />
                                                    </div>
                                                    <div class="float-left paddingtop5">
                                                        &nbsp;To&nbsp;</div>
                                                    <div class="iq-date-div">
                                                        <asp:TextBox ID="txtFollowerCountTo" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                            TargetControlID="txtFollowerCountTo" ValidChars="1234567890" />
                                                    </div>
                                                </div>
                                                <div class="clear paddingtop15">
                                                    <div>
                                                        <label>
                                                            Friends Count:</label></div>
                                                    <div class="iq-date-div">
                                                        <asp:TextBox ID="txtFriendsCountFrom" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                            TargetControlID="txtFriendsCountFrom" ValidChars="1234567890" />
                                                    </div>
                                                    <div class="float-left paddingtop5">
                                                        &nbsp;To&nbsp;</div>
                                                    <div class="iq-date-div">
                                                        <asp:TextBox ID="txtFriendsCountTo" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                            TargetControlID="txtFriendsCountTo" ValidChars="1234567890" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liTwitterScore">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="SetTwitterScoreFilter()">
                                                <div class="lidivLeft">
                                                    <img src="../images/filter.png" id="imgTwitterScoreFilter" />&nbsp;&nbsp;Klout Score
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divTwitterScoreFilterStatus">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowTwitterScore" src="../images/show.png" class="filterDirectionImg">
                                                </div>
                                            </div>
                                            <div id="divTwitterScore" class="bs-docs-example">
                                                <div class="clear">
                                                    <div>
                                                        <label>
                                                            Klout Score:</label></div>
                                                    <div class="iq-date-div">
                                                        <asp:TextBox ID="txtKloutScoreFrom" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                            TargetControlID="txtKloutScoreFrom" ValidChars="1234567890" />
                                                    </div>
                                                    <div class="float-left paddingtop5">
                                                        &nbsp;To&nbsp;</div>
                                                    <div class="iq-date-div">
                                                        <asp:TextBox ID="txtKloutScoreTo" runat="server" CssClass="intervalTextbox"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender5" runat="server"
                                                            TargetControlID="txtKloutScoreTo" ValidChars="1234567890" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upBottomSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="divApply">
                        <asp:Button ID="btnApply" CausesValidation="false" runat="server" CssClass="btn-blue2"
                            Text="Search" OnClick="btnSearch_click" OnClientClick="return RemoveExportChartComponent();" />
                        <input type="button" onclick="ShowSearchSaveAs();" id="btnSaveSearch" class="btn-blue2"
                            value="Save As" />
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnApply" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
        <!--/.well -->
    </div>
    <!--/span-->
    <div class="span9">
        <div>
            <div class="navbar">
                <div class="show-hide cursor text-align-center" onclick="ShowHideDivResult(true);">
                    <div class="float-left">
                        <a href="javascript:;">RESULTS</a></div>
                    <div class="float-right">
                        <a href="javascript:;" class="right-dropdown">
                            <div class="float-left" id="divResultShowHideTitle">
                                SHOW</div>
                            <img id="imgShowHideResult" class="imgshowHide" src="../images/show.png" alt=""></a></div>
                </div>
                <div id="divResult" class="display-none">
                    <%--<asp:UpdatePanel ID="upTab" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                    <asp:HiddenField ID="hfcurrentTab" runat="server" />
                    <div id="tabsTV" class="tabMain">
                        <div id="divGridTab" class="clear tabdiv">
                            <div id="tabTV" class="active float-left display-none" onclick="ChangeTab('tabTV','divGridTab',0,1);">
                                TV
                            </div>
                            <div id="tabRadio" class="active float-left display-none" onclick="ChangeTab('tabRadio','divGridTab',1,1);">
                                Radio
                            </div>
                            <div id="tabOnlineNews" class="float-left display-none" onclick="ChangeTab('tabOnlineNews','divGridTab',2,1);">
                                Online News
                            </div>
                            <div id="tabSocialMedia" class="float-left display-none" onclick="ChangeTab('tabSocialMedia','divGridTab',3,1);">
                                Social Media
                            </div>
                            <div id="tabTwitter" class="float-left display-none" onclick="ChangeTab('tabTwitter','divGridTab',4,1);">
                                Twitter
                            </div>
                        </div>
                        <div class="tabContentDiv">
                            <div id="divTVResult" class="display-none">
                                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfTVStatus" runat="server" Value="0" />
                                        <div id="divTVResultInner" class="display-none">
                                            <div>
                                                <span id="spnChartActive" runat="server" visible="false" style="color: Red;">CHART ACTIVE!</span>
                                                <asp:Label ID="lblTVMsg" runat="server" CssClass="MsgFail"></asp:Label>
                                                <div class="float-right">
                                                    <span id="spnTotalProgramHeader" runat="server" visible="false" class="float-left">Total
                                                        Programs Found :&nbsp; </span>
                                                    <asp:Label ID="lblNoOfRadioRawMedia" runat="server" Visible="false" CssClass="float-right"></asp:Label>
                                                </div>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <div class="grid-iq subdivheight">
                                                <asp:Repeater ID="rptTV" runat="server" OnItemCommand="rptTV_ItemCommand">
                                                    <HeaderTemplate>
                                                        <div class="clear hederdiv grid-th-left">
                                                            <div style="width: 5%" class="center">
                                                                <span>Play<span>
                                                            </div>
                                                            <div style="width: 7%;">
                                                                &nbsp;
                                                            </div>
                                                            <div runat="server" id="divHeaderMarket" style="width: 12%;">
                                                                <asp:LinkButton ID="lnkMarket" runat="server" Text="Market" CommandName="sort" CommandArgument="market">
                                                                </asp:LinkButton>
                                                            </div>
                                                            <div style="width: 18%" class="right">
                                                                <asp:LinkButton ID="DateTime" runat="server" Style="text-align: right" Text="Date Time"
                                                                    CommandName="sort" CommandArgument="DateTime"></asp:LinkButton>
                                                            </div>
                                                            <div runat="server" id="divHeaderTitle" style="width: 21%" class="left">
                                                                <span>Program</span></div>
                                                            <div style="width: 9%;" class="right">
                                                                <span>Nielsen
                                                                    <br />
                                                                    Audience</span></div>
                                                            <div runat="server" id="divHeaderAdShare" style="width: 9%;" class="right">
                                                                <span>iQ media
                                                                    <br />
                                                                    Value</span></div>
                                                            <div runat="server" id="divHeaderHits" style="width: 9%" class="center">
                                                                <span>Mentions</span></div>
                                                            <div style="width: 10%" runat="server" id="divHeaderSentiment" class="right">
                                                                <span>Sentiment</span></div>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div class="clear grid">
                                                            <div style="width: 5%" class="center">
                                                                <asp:ImageButton ID="imgPlayButton" runat="server" ImageUrl="~/Images/btn-play.jpg"
                                                                    CommandArgument='<%# Eval("RawMediaID") %>' OnCommand="LbtnRawMediaPlay_Command" />
                                                            </div>
                                                            <div style="width: 7%;" class="center">
                                                                <span>
                                                                    <asp:Image ID="imgStationLogo" AlternateText=" " ImageUrl='<%# Eval("StationLogo") %>'
                                                                        runat="server" /></span>
                                                            </div>
                                                            <div runat="server" id="divMarket" style="width: 12%;" class="left">
                                                                <span>
                                                                    <%# Eval("IQ_Dma_Name")%></span>
                                                            </div>
                                                            <div style="width: 18%" class="right">
                                                                <span>
                                                                    <%# Convert.ToDateTime(Eval("DateTime")).ToString("MM/dd/yyyy") %>
                                                                    <%# Convert.ToDateTime(Eval("DateTime")).ToString("hh:mm tt")%></span>
                                                            </div>
                                                            <div runat="server" id="divTitle" style="width: 21%" class="left">
                                                                <span>
                                                                    <%# Eval("Title120")%></span>
                                                            </div>
                                                            <div style="width: 9%;" class="right">
                                                                <span>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("IQNielenseAudience"))) ? "NA" : Convert.ToInt64(Eval("IQNielenseAudience")).ToString("#,#")%></span>
                                                            </div>
                                                            <div runat="server" id="divAdShare" style="width: 9%;" class="right">
                                                                <span>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("IQAddShareValue"))) ? "NA" : Convert.ToInt64(Eval("IQAddShareValue").ToString().Split('(')[0]).ToString("#,#") + '(' + Eval("IQAddShareValue").ToString().Split('(')[1]%></span>
                                                            </div>
                                                            <div runat="server" id="divHits" style="width: 9%" class="center">
                                                                <span>
                                                                    <%# Eval("Hits")%></span>
                                                            </div>
                                                            <div runat="server" id="divSentiment" style="width: 10%" class="right">
                                                                <div class="divSentimentMain">
                                                                    <div class="divSentimentNeg">
                                                                        <%# "<div style='width:" + ((Convert.ToInt32(Eval("NegativeSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("NegativeSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("NegativeSentiment"))) + "</div>"%>
                                                                    </div>
                                                                    <div class="divSentimentPos">
                                                                        <%# "<div style='width:" + ((Convert.ToInt32(Eval("PositiveSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("PositiveSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("PositiveSentiment"))) + "</div>"%>
                                                                    </div>
                                                                </div>
                                                                <%--<span>(<label class="PositiveSentiment"><%# Eval("PositiveSentiment")%></label>,<label class="NegativeSentiment"><%# Eval("NegativeSentiment")%></label>)</span>--%>
                                                            </div>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </div>
                                            <div runat="server" id="divNoResults" class="clear grid" visible="false">
                                                <div>
                                                    <span>No Results Found</span></div>
                                            </div>
                                            <uc:CustomPager ID="ucCustomPager" runat="server" On_PageIndexChange="ucCustomPager_PageIndexChange"
                                                PageSize="10" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divOnlineNewsResult" class="display-none">
                                <asp:UpdatePanel ID="upOnlineNews" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfOnlineNewsStatus" runat="server" Value="0" />
                                        <div id="divOnlineNewsResultInner" class="display-none">
                                            <div>
                                                <asp:Label ID="lblNewsMsg" runat="server" CssClass="MsgFail"></asp:Label>
                                                <div class="float-left">
                                                    <span id="spnNewsChartProgramFound" runat="server" visible="false" class="float-left">
                                                        Total Articles Found :&nbsp; </span>
                                                    <asp:Label ID="lblNewsChart" runat="server" Visible="false" CssClass="float-left"></asp:Label>
                                                    <span id="spnNewsChart" runat="server" visible="false" class="float-left paddingleft17"
                                                        style="color: Red;">CHART ACTIVE!</span>
                                                </div>
                                                <div class="float-right">
                                                    <asp:Button ID="btnSaveNMtoMyIQ" runat="server" CssClass="btn-blue2 marginbottom5"
                                                        Text="Save to myiQ" OnClientClick="return OpenSaveArticlePopup1('NM');" />
                                                </div>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:GridView ID="gvOnlineNews" runat="server" AutoGenerateColumns="false" CellPadding="5"
                                                CellSpacing="0" AllowSorting="true" AllowPaging="false" PageSize="10" EmptyDataText="No Results Found"
                                                CssClass="grid grid-iq" OnSorting="GvOnlineNews_Sorting">
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="imgArticleButton" ImageUrl="~/Images/NewsRead.png" NavigateUrl='<%# Eval("Article") %>'
                                                                Target="_blank" runat="server"></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="6%" />
                                                        <ItemStyle CssClass="center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="false" HeaderText="Publication">
                                                        <ItemTemplate>
                                                            <a id="aPublication" runat="server" target="_blank" href='<%# Eval("publication") %>'>
                                                                <%# Eval("publication") %></a>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-left" Width="12%" />
                                                        <ItemStyle CssClass="left break-words" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Title" HeaderText="Title">
                                                        <HeaderStyle CssClass="grid-th-left" Width="16%" />
                                                        <ItemStyle CssClass="left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField SortExpression="date" HeaderText="Date Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("date")).ToString("MM/dd/yyyy") + "<br />" + Convert.ToDateTime(Eval("date")).ToString("hh:mm tt")%>'></asp:Label>
                                                            <asp:Label ID="hfOnlineNewsHarvestDT" Text='<%# Convert.ToDateTime(Eval("date")).ToString("MM/dd/yyyy hh:mm:ss") %>'
                                                                runat="server" Style="display: none;"></asp:Label>
                                                            <%--<asp:HiddenField ID="hfOnlineNewsHarvestDT" Value='<%# Convert.ToDateTime(Eval("date")).ToString("MM/dd/yyyy hh:mm:ss") %>' runat="server" />--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-right" Width="10%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="Category" HeaderText="Category">
                                                        <HeaderStyle CssClass="grid-th-left" Width="8%" />
                                                        <ItemStyle CssClass="left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Genre" HeaderText="Genre">
                                                        <HeaderStyle CssClass="grid-th-left" Width="8%" />
                                                        <ItemStyle CssClass="left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Audience">
                                                        <ItemTemplate>
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td id="caud" runat="server" style="padding: 0px">
                                                                    </td>
                                                                    <td id="ccmpt" runat="server" style="padding: 0px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-right" Width="10%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="iQ media<br/>Value" HtmlEncode="false">
                                                        <HeaderStyle CssClass="grid-th-right" Width="8%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Mentions" HeaderText="Mentions">
                                                        <HeaderStyle CssClass="grid-th" Width="9%" />
                                                        <ItemStyle CssClass="center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Sentiment">
                                                        <ItemTemplate>
                                                            <div class="divSentimentMain">
                                                                <div class="divSentimentNeg">
                                                                    <%# "<div style='width:" + ((Convert.ToInt32(Eval("Sentiments.NegativeSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("Sentiments.NegativeSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("Sentiments.NegativeSentiment"))) + "</div>"%>
                                                                </div>
                                                                <div class="divSentimentPos">
                                                                    <%# "<div style='width:" + ((Convert.ToInt32(Eval("Sentiments.PositiveSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("Sentiments.PositiveSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("Sentiments.PositiveSentiment"))) + "</div>"%>
                                                                </div>
                                                            </div>
                                                            <%--(<label class="PositiveSentiment"><%#DataBinder.Eval(Container.DataItem, "Sentiments.PositiveSentiment")%></label>,<label class="NegativeSentiment"><%#DataBinder.Eval(Container.DataItem, "Sentiments.NegativeSentiment")%></label>)--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-right" Width="9%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="false">
                                                        <ItemTemplate>
                                                            <input type="checkbox" id="chkSave" runat="server" value='<%# Eval("ID") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="center" />
                                                        <HeaderStyle Width="5%" CssClass="grid-th center" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <uc:CustomPager ID="ucOnlineNewsPager" runat="server" On_PageIndexChange="ucOnlineNewsPager_PageIndexChange"
                                                PageSize="10" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divSocialMediaResult" class="display-none">
                                <asp:UpdatePanel ID="upSocialMedia" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfSocialMediaStatus" runat="server" Value="0" />
                                        <div id="divSocialMediaResultInner" class="display-none">
                                            <div>
                                                <asp:Label ID="lblSMMsg" runat="server" CssClass="MsgFail"></asp:Label>
                                                <div class="float-left">
                                                    <span id="spnSMChartProgramFound" runat="server" visible="false" class="float-left">
                                                        Total Articles Found :&nbsp; </span>
                                                    <asp:Label ID="lblSMChart" runat="server" Visible="false" CssClass="float-left"></asp:Label>
                                                    <span id="spnSMChart" runat="server" visible="false" class="float-left paddingleft17"
                                                        style="color: Red;">CHART ACTIVE!</span>
                                                </div>
                                                <div class="float-right">
                                                    <asp:Button ID="btnSaveSMtoMyIQ" runat="server" CssClass="btn-blue2 marginbottom5"
                                                        Text="Save to myiQ" OnClientClick="return OpenSaveArticlePopup1('SM');" />
                                                </div>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:GridView ID="gvSocialMedia" runat="server" AutoGenerateColumns="false" CellPadding="5"
                                                CellSpacing="0" AllowSorting="true" AllowPaging="false" PageSize="10" EmptyDataText="No Results Found"
                                                CssClass="grid grid-iq" OnSorting="GvSocialMedia_Sorting">
                                                <%--OnSorting="GvOnlineNews_Sorting" OnRowCommand="gvOnlineNews_RowCommand"--%>
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="imgSMArticleButton" ImageUrl="~/Images/NewsRead.png" NavigateUrl='<%# Eval("link") %>'
                                                                runat="server" Target="_blank"></asp:HyperLink>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="6%" />
                                                        <ItemStyle CssClass="center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="false" HeaderText="Publication">
                                                        <ItemTemplate>
                                                            <a id="aPublication" runat="server" target="_blank" href='<%# Eval("homeLink") %>'>
                                                                <%# Eval("homeLink")%></a>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-left" Width="11%" />
                                                        <ItemStyle CssClass="left break-words" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="description" HeaderText="Title">
                                                        <HeaderStyle CssClass="grid-th-left" Width="14%" />
                                                        <ItemStyle CssClass="left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField SortExpression="date" HeaderText="Date Time">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("itemHarvestDate_DT")).ToString("MM/dd/yyyy") + "<br />" + Convert.ToDateTime(Eval("itemHarvestDate_DT")).ToString("hh:mm tt")%>'></asp:Label>
                                                            <asp:Label ID="hfsocialMediaHarvestDT" Text='<%# Convert.ToDateTime(Eval("itemHarvestDate_DT")).ToString("MM/dd/yyyy hh:mm:ss") %>'
                                                                runat="server" Style="display: none;"></asp:Label>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-right" Width="10%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="feedCategories" HeaderText="Category">
                                                        <HeaderStyle CssClass="grid-th-left" Width="8%" />
                                                        <ItemStyle CssClass="left" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Type" HeaderStyle-Width="7%" HeaderStyle-CssClass="grid-th-left">
                                                        <ItemTemplate>
                                                            <asp:Label Text='<%# Eval("feedClass") %>' runat="server" ID="lblfeedClass"></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="left" />
                                                    </asp:TemplateField>
                                                    <%--<asp:BoundField DataField="feedClass" HeaderText="Type">
                                                        <HeaderStyle CssClass="grid-th-left" Width="7%" />
                                                        <ItemStyle CssClass="left" />
                                                    </asp:BoundField>--%>
                                                    <asp:BoundField DataField="feedRank" HeaderText="Rank">
                                                        <HeaderStyle CssClass="grid-th-right" Width="5%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Audience">
                                                        <ItemTemplate>
                                                            <table cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td id="caud" runat="server" style="padding: 0px">
                                                                    </td>
                                                                    <td id="ccmpt" runat="server" style="padding: 0px">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-right" Width="10%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField HeaderText="iQ media<br/>Value" HtmlEncode="false">
                                                        <HeaderStyle CssClass="grid-th-right" Width="8%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="Mentions" HeaderText="Mentions">
                                                        <HeaderStyle CssClass="grid-th" Width="8%" />
                                                        <ItemStyle CssClass="center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Sentiment">
                                                        <ItemTemplate>
                                                            <div class="divSentimentMain">
                                                                <div class="divSentimentNeg">
                                                                    <%# "<div style='width:" + ((Convert.ToInt32(Eval("Sentiments.NegativeSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("Sentiments.NegativeSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("Sentiments.NegativeSentiment"))) + "</div>"%>
                                                                </div>
                                                                <div class="divSentimentPos">
                                                                    <%# "<div style='width:" + ((Convert.ToInt32(Eval("Sentiments.PositiveSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("Sentiments.PositiveSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("Sentiments.PositiveSentiment"))) + "</div>"%>
                                                                </div>
                                                            </div>
                                                            <%--(<label class="PositiveSentiment"><%#DataBinder.Eval(Container.DataItem, "Sentiments.PositiveSentiment")%></label>,<label class="NegativeSentiment"><%#DataBinder.Eval(Container.DataItem, "Sentiments.NegativeSentiment")%></label>)--%>
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th-right" Width="9%" />
                                                        <ItemStyle CssClass="right" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField ShowHeader="false">
                                                        <ItemTemplate>
                                                            <input type="checkbox" id="chkSave" runat="server" value='<%# Eval("id") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="center" />
                                                        <HeaderStyle Width="5%" CssClass="grid-th center" VerticalAlign="Top" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <uc:CustomPager ID="ucSMPager" runat="server" On_PageIndexChange="ucSMPager_PageIndexChange"
                                                PageSize="10" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divTwitterResult" class="display-none">
                                <asp:UpdatePanel ID="upTwitter" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfTwitterMediaStatus" runat="server" Value="0" />
                                        <div id="divTwitterResultInner" class="display-none">
                                            <div>
                                                <asp:Label ID="lblTwitterMsg" runat="server" CssClass="MsgFail"></asp:Label>
                                                <div class="float-left">
                                                    <span id="spnTwitterChart" runat="server" visible="false" style="color: Red" class="padding5">
                                                        CHART ACTIVE!</span>
                                                </div>
                                                <div class="float-right">
                                                    <span id="spnTwitterChartProgramFound" runat="server" visible="false" class="float-left">
                                                        Total Tweets Found :&nbsp; </span>
                                                    <asp:Label ID="lblTwitterChart" runat="server" Visible="false" CssClass="float-right"></asp:Label>
                                                </div>
                                                <div class="clear">
                                                </div>
                                                <asp:Panel ID="pnlTwitterSort" runat="server" CssClass="float-left padding5">
                                                    <asp:DropDownList ID="ddlTwitterSortExp" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RadioButtonList ID="rbTwitterSortDir" CssClass="padding5" runat="server" RepeatLayout="Flow"
                                                        RepeatDirection="Horizontal" BorderWidth="0">
                                                        <asp:ListItem Text="Asc&nbsp;&nbsp;" Value="true"></asp:ListItem>
                                                        <asp:ListItem Selected="True" Text="Desc&nbsp;&nbsp;" Value="false"></asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <asp:Button ID="btnTwiiterSort" runat="server" OnClick="btnTwiiterSort_Click" Text="Sort"
                                                        CssClass="btn-blue2" />
                                                </asp:Panel>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:DataList ID="dlTweets" Width="100%" runat="server" OnItemCommand="dlTweets_ItemCommand"
                                                CellPadding="0" CellSpacing="0" GridLines="None">
                                                <ItemTemplate>
                                                    <%--<div id="datalistInner" class="clear" style="border-bottom: 1px solid #999999; padding: 10px;">
                                                        <div style="width: 85%; vertical-align: top; padding-right: 3%; box-sizing: border-box;
                                                            -moz-box-sizing: border-box; -webkit-box-sizing: border-box;" class="float-left">
                                                            <div class="clear">
                                                                <div class="float-left">
                                                                    <a id="aActorLink" runat="server" href='<%# Eval("actor_link") %>' target="_blank">
                                                                        <asp:Label ID="lblDisplayName" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "actor_displayName")%>'></asp:Label></a><br />
                                                                </div>
                                                                <div class="float-right">
                                                                    <div class="float-left" style="font-size: 12px; color: #999999;">
                                                                        Klout Score:
                                                                        <asp:Label ID="lblKloutScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Klout_score")%>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </div>
                                                                    <div class="float-left" style="font-size: 12px; color: #999999;">
                                                                        Followers:&nbsp;&nbsp;
                                                                        <asp:Label ID="lblActorFollowers" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "followers_count")%>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </div>
                                                                    <div class="float-left" style="font-size: 12px; color: #999999;">
                                                                        Friends:&nbsp;&nbsp;
                                                                        <asp:Label ID="lblActorFriends" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "friends_count")%>'></asp:Label></div>
                                                                </div>
                                                            </div>
                                                            <div class="clear" style="padding: 1% 0%;">
                                                                <asp:Label ID="lblTweetBody" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tweet_body")%>'></asp:Label></div>
                                                            <div class="clear" style="font-size: 12px; text-align: right; color: #999999;">
                                                                <asp:Label ID="lblPostedDateTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tweet_postedDateTime")%>'></asp:Label></div>
                                                        </div>
                                                        <div style="width: 12%; text-align: center; box-sizing: border-box; -moz-box-sizing: border-box;
                                                            -webkit-box-sizing: border-box;" class="float-left">
                                                            <a href='<%# Eval("actor_link") %>' target="_blank" style="border: 0;">
                                                                <asp:Image ID="imgActor" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "actor_image")%>' />
                                                            </a>
                                                            <br />
                                                            <asp:LinkButton ID="lnlSaveTweet" runat="server" CommandName="SaveTweet" Text="Save Tweet"
                                                                CommandArgument='<%# Eval("tweet_id") %>'></asp:LinkButton>
                                                        </div>
                                                    </div>--%>
                                                    <div id="datalistInner" class="clear TweetInnerDiv">
                                                        <div class="float-left TweetBodyDivIQP borderBoxSizing">
                                                            <div class="clear">
                                                                <div class="float-left TweetActorDisplayName">
                                                                    <a id="aActorLink" runat="server" href='<%#  Eval("actor_link") %>' target="_blank">
                                                                        <asp:Label ID="lblDisplayName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "actor_displayName")%>'></asp:Label>
                                                                    </a><span class="TweetSubdivFont">@</span><asp:Label ID="lblPrefferedUserName" runat="server"
                                                                        CssClass="TweetSubdivFont" Text='<%# Eval("actor_prefferedUserName") %>'></asp:Label>
                                                                    <br />
                                                                </div>
                                                                <div class="float-right">
                                                                    <div class="float-left TweetSubdivFont">
                                                                        Klout Score:&nbsp;
                                                                        <asp:Label ID="lblKloutScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Klout_score")%>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </div>
                                                                    <div class="float-left TweetSubdivFont">
                                                                        Followers:&nbsp;
                                                                        <asp:Label ID="lblActorFollowers" runat="server" Text='<%# string.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "followers_count")) %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                    </div>
                                                                    <div class="float-left TweetSubdivFont">
                                                                        Friends:&nbsp;
                                                                        <asp:Label ID="lblActorFriends" runat="server" Text='<%# string.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "friends_count")) %>'></asp:Label></div>
                                                                </div>
                                                            </div>
                                                            <div class="clear PaddingTopBottom1p TweetBodyText">
                                                                <div class="div75pleft">
                                                                    <asp:Label ID="lblTweetBody" runat="server" Text='<%# Convert.ToString(Eval("tweet_body")) %>'></asp:Label>
                                                                </div>
                                                                <div class="TweetSubdivFont float-right">
                                                                    <asp:Label ID="lblPostedDateTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tweet_postedDateTime")%>'></asp:Label>
                                                                    <div class="clear" runat="server" id="divSentiment">
                                                                        <div style="float: left;">
                                                                            Sentiment :</div>
                                                                        <div class="divSentimentMainTW">
                                                                            <div class="divSentimentNeg">
                                                                                <%# "<div style='width:" + ((Convert.ToInt32(Eval("Sentiments.NegativeSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("Sentiments.NegativeSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("Sentiments.NegativeSentiment"))) + "</div>"%>
                                                                            </div>
                                                                            <div class="divSentimentPos">
                                                                                <%# "<div style='width:" + ((Convert.ToInt32(Eval("Sentiments.PositiveSentiment")) * 6) > 25 ? 25 : (Convert.ToInt32(Eval("Sentiments.PositiveSentiment")) * 6)) + "px;'>" + (Convert.ToInt32(Eval("Sentiments.PositiveSentiment"))) + "</div>"%>
                                                                            </div>
                                                                        </div>
                                                                        <%-- (<label class="PositiveSentiment"><%#DataBinder.Eval(Container.DataItem, "Sentiments.PositiveSentiment")%></label>,<label class="NegativeSentiment"><%#DataBinder.Eval(Container.DataItem, "Sentiments.NegativeSentiment")%></label>)--%>
                                                                    </div>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <div class="float-right IQPremiumTweetImageDiv center">
                                                            <a id="aActorLinkimage" runat="server" href='<%#  Eval("actor_link") %>' target="_blank">
                                                                <asp:Image ID="imgActor" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "actor_image")%>' />
                                                            </a>
                                                            <br />
                                                            <asp:LinkButton ID="lnlSaveTweet" runat="server" CommandName="SaveTweet" Text="Save Tweet"
                                                                CommandArgument='<%# Eval("tweet_id") %>'></asp:LinkButton>
                                                        </div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:DataList>
                                            <div runat="server" id="divNoResultTwitter" class="clear grid" visible="false">
                                                <div>
                                                    <span>No Results Found</span></div>
                                            </div>
                                            <uc:CustomPager ID="ucTwitterPager" runat="server" On_PageIndexChange="ucTwitterPager_PageIndexChange"
                                                PageSize="15" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divRadioResult" class="display-none">
                                <asp:UpdatePanel ID="upRadio" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <asp:HiddenField ID="hfRadioStatus" runat="server" Value="0" />
                                        <div id="divRadioResultInner">
                                            <div>
                                                <asp:Label ID="lblRadioMsg" runat="server" CssClass="MsgFail"></asp:Label>
                                                <div class="float-left">
                                                    <span id="spnRadioChart" runat="server" visible="false" style="color: Red" class="padding5">
                                                        CHART ACTIVE!</span>
                                                </div>
                                                <div class="float-right">
                                                    <span id="spnRadioFound" runat="server" visible="false" class="float-left">Total Radio
                                                        Results Found :&nbsp; </span>
                                                    <asp:Label ID="lblRadioChart" runat="server" Visible="false" CssClass="float-right"></asp:Label>
                                                </div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div class="clear">
                                            </div>
                                            <asp:GridView ID="grvRadioStations" runat="server" AutoGenerateColumns="false" AllowPaging="false"
                                                PageSize="10" Width="100%" border="0" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4"
                                                BackColor="#FFFFFF" Style="border-collapse: collapse;" AllowSorting="true" EmptyDataText="No Results Found"
                                                PageIndex="0" CssClass="grid grid-iq" OnSorting="grvRadioStations_Sorting">
                                                <Columns>
                                                    <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandArgument='<%# Eval("RawMediaID") %>'
                                                                OnClientClick="hidediv();" OnCommand="LbtnRawMediaPlay_Command" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="5%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                        <ItemTemplate>
                                                            <asp:Image ID="imgStationLogo" ImageUrl='<%# Eval("StationLogo") %>' runat="server" />
                                                        </ItemTemplate>
                                                        <HeaderStyle CssClass="grid-th" Width="10%" />
                                                        <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="dma_name" SortExpression="dma_name" HeaderText="Market">
                                                        <ItemStyle CssClass="content-text2 paddingleft4p" HorizontalAlign="Left" Width="42%" />
                                                        <HeaderStyle CssClass="grid-th-left paddingleft4p" Width="42%" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Date Time" SortExpression="DateTime">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblRawMediaDatetime" runat="server" Text='<%# Eval("RawMediaDateTime") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle CssClass="content-text2 paddingright4p" HorizontalAlign="right" Width="42%" />
                                                        <HeaderStyle CssClass="grid-th-right paddingright4p" Width="42%" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <HeaderStyle CssClass="grid-th" />
                                            </asp:GridView>
                                            <uc:CustomPager ID="ucRadioPager" runat="server" On_PageIndexChange="ucRadioPager_PageIndexChange"
                                                PageSize="10" Visible="false" />
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
            </div>
            <div class="navbar">
                <div class="show-hide cursor" onclick="ShowHideDivChart(true);">
                    <div class="float-left">
                        <a href="javascript:;">CHART RESULTS</a></div>
                    <div class="float-right">
                        <a href="javascript:;" class="right-dropdown">
                            <div class="float-left" id="divChartShowHideTitle">
                                SHOW</div>
                            <img id="imgShowHideChart" class="imgshowHide" src="../images/show.png" alt=""></a></div>
                </div>
                <div class="display-none tabMain" id="divChartResult">
                    <%--<asp:UpdatePanel ID="upChartTab" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>--%>
                    <div id="chartTab2">
                        <div id="divChartTab" class="clear tabdiv">
                            <div id="tabChartTV" class="active float-left display-none" onclick="ChangeTab('ctl00_Content_Data_ucIQpremium_tabChartTV','divChartTab',0,0);">
                                TV
                            </div>
                            <div id="tabChartOL" class="float-left display-none" onclick="ChangeTab('ctl00_Content_Data_ucIQpremium_tabChartOL','divChartTab',2,0);">
                                Online News
                            </div>
                            <div id="tabChartSM" class="float-left display-none" onclick="ChangeTab('ctl00_Content_Data_ucIQpremium_tabChartSM','divChartTab',3,0);">
                                Social Media
                            </div>
                            <div id="tabChartTwitter" class="float-left display-none" onclick="ChangeTab('tabChartTwitter','divChartTab',4,0);">
                                Twitter
                            </div>
                        </div>
                        <div class="tabContentDiv">
                            <div id="divTVChart" class="display-none">
                                <asp:UpdatePanel ID="upChart" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divTVChartInner" class="display-none">
                                            <div id="divLineChart" runat="server">
                                            </div>
                                            <br />
                                            <div class="clear">
                                                <div class="float-left">
                                                    <asp:Button ID="btncsv" runat="server" OnClientClick="GetCsvData(0);" OnClick="btncsv_click"
                                                        Text="Export Data" class="btn-blue2 float-left display-none" />
                                                </div>
                                                <div id="fcexpDiv" class="fusion-chart">
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hfsvcData" runat="server" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btncsv" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divOLChart" class="display-none">
                                <asp:UpdatePanel ID="upNewsChart" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divOLChartInner" class="display-none">
                                            <div id="divNewsChart" runat="server">
                                            </div>
                                            <br />
                                            <div class="clear">
                                                <div class="float-left">
                                                    <asp:Button ID="btnCSVNews" runat="server" OnClientClick="GetCsvData(1);" OnClick="btncsv_click"
                                                        Text="Export Data" class="btn-blue2 display-none" />
                                                </div>
                                                <div id="divfcexNews" align="center">
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hfcsvNewsData" runat="server" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnCSVNews" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divSMChart" class="display-none">
                                <asp:UpdatePanel ID="upSocialMediaChart" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divSMChartInner" class="display-none">
                                            <div id="divSocialMediaChart" runat="server">
                                            </div>
                                            <br />
                                            <div class="clear">
                                                <div class="float-left">
                                                    <asp:Button ID="btnCSVSM" runat="server" OnClientClick="GetCsvData(2);" OnClick="btncsv_click"
                                                        Text="Export Data" class="btn-blue2 display-none" />
                                                </div>
                                                <div id="div4" align="center">
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hfcsvSMData" runat="server" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnCSVSM" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                            <div id="divTwitterChart" class="display-none">
                                <asp:UpdatePanel ID="upTwitterChart" runat="server" UpdateMode="Conditional">
                                    <ContentTemplate>
                                        <div id="divTwitterChartInner" class="display-none">
                                            <div id="divTwitterChart2" runat="server">
                                            </div>
                                            <br />
                                            <div class="clear">
                                                <div class="float-left">
                                                    <asp:Button ID="btnCSVTwitter" runat="server" OnClientClick="GetCsvData(3);" OnClick="btncsv_click"
                                                        Text="Export Data" CssClass="btn-blue2 display-none" />
                                                </div>
                                                <div id="divfcexTwitter">
                                                </div>
                                            </div>
                                            <asp:HiddenField ID="hfcsvTwitterData" runat="server" />
                                        </div>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:PostBackTrigger ControlID="btnCSVTwitter" />
                                    </Triggers>
                                </asp:UpdatePanel>
                            </div>
                        </div>
                    </div>
                    <%--</ContentTemplate>
                    </asp:UpdatePanel>--%>
                </div>
                <br />
                <br />
            </div>
            <div id="diviframe" runat="server" class="modal hide fade resizable modalPopupDiv"
                style="display: none;">
                <div id="divClosePlayer">
                    <%--<input type="image" id="imgCancelPlayer" class="popup-top-close" src="../Images/close-icon.png"
                        onclick="ClosePlayer();" />--%>
                    <img id="img2" src="../Images/close-icon.png" class="popup-top-close" onclick="ClosePlayer();" />
                </div>
                <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <uc1:IframeRawMediaH ID="IframeRawMediaH" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div id="diviFrameOnlineNewsArticle" runat="server" class="modal hide fade resizable modalPopupDiv iframePopup"
                style="display: none;">
                <%--<div id="divCloseNews">--%>
                <div style="width: 27px; float: right;" id="divCloseNews">
                    <div>
                        <img id="img1" src="../Images/close-icon.png" class="popup-top-close" onclick="CloseNewsIframe();" />
                        <%--<input type="image" id="Image1" class="popup-top-close" onclick="CloseNewsIframe();"
                            src="../Images/close-icon.png" />--%>
                    </div>
                </div>
                <asp:UpdatePanel ID="upOnlineNewsArticle" runat="server" UpdateMode="Conditional"
                    style="width: 100%; height: 100%;">
                    <ContentTemplate>
                        <iframe id="iFrameOnlineNewsArticle" style="width: 100%; height: 100%;" scrolling="auto"
                            frameborder="0" runat="server" src=""></iframe>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <%--<div class="DivNews-btn">--%>
                <div style="width: 100%; text-align: right; position: absolute; margin: -50px 0 0 -25px;">
                    <asp:Button ID="btnSave" Text="Save Article" CssClass="btn-green" runat="server"
                        OnClientClick="return OpenSaveArticlePopup(0);" />
                    <asp:Button ID="btnPrint" Text="Print Article" CssClass="btn-green" runat="server"
                        OnClientClick="javascript:PrintIframe();return false;" />
                </div>
            </div>
        </div>
    </div>
</div>
<asp:Panel ID="pnlSaveSearch" CssClass="modal hide fade resizable modalPopupDiv height80p"
    runat="server" Style="display: none;">
    <asp:UpdatePanel ID="upEditSaveSearch" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span id="spnSaveSearchHeader">Save Search</span>
                    <div style="width: 27px; float: right;" id="div1" runat="server">
                        <img id="imgSaveSearchClose" src="../Images/close-icon.png" onclick="CancelSaveSearch();" />
                        <%--<input type="image" id="imgEditArticleClos" class="popup-top-close" runat="server"
                            onclick="closeModal('pnlEditArticle');" src="~/Images/close-icon.png" />--%>
                    </div>
                    <%-- <div class="pnlPopup-right-close">
                        <input type="button" id="btnCancelCategoryPopup" onclick="CancelSaveSearch()" class="btn-cancel"
                            value=" " />
                    </div>--%>
                </div>
                <div class="blue-content-bg height90p">
                    <div>
                        <asp:Label ID="lblNote" runat="server" CssClass="error-msg display-none"></asp:Label>
                        <asp:Label ID="lblmsg" runat="server" CssClass="error-msg"></asp:Label>
                        <asp:ValidationSummary ID="vsSaveSearch" EnableClientScript="true" runat="server"
                            ValidationGroup="SaveSearch" CssClass="error-summery" />
                    </div>
                    <ul class="registration">
                        <li>
                            <label>
                                Title :
                            </label>
                            <asp:TextBox ID="txtTitle" MaxLength="150" Width="74%" runat="server" />
                            <asp:RequiredFieldValidator Display="None" ID="rfvTitle" ControlToValidate="txtTitle"
                                ErrorMessage="Title is Required" ValidationGroup="SaveSearch" Text="*" ToolTip="Title is Required"
                                runat="server" />
                        </li>
                        <li>
                            <label>
                                Description :
                            </label>
                            <asp:TextBox ID="txtDescription" MaxLength="500" TextMode="MultiLine" Rows="3" runat="server"
                                Style="width: 74%; height: 145px; overflow: auto;" />
                        </li>
                        <li>
                            <label>
                                Category :
                            </label>
                            <asp:DropDownList ID="ddlCategory" runat="server" Width="36%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqCategory" runat="server" ControlToValidate="ddlCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="SaveSearch" Display="None"
                                InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Is Default :
                            </label>
                            <asp:CheckBox ID="chkIsDefaultSearch" runat="server" Checked="false" />
                        </li>
                        <li id="liIsIQAgent" runat="server">
                            <label>
                                Is IQAgent :
                            </label>
                            <asp:CheckBox ID="chkIsIQAgent" runat="server" />
                        </li>
                    </ul>
                    <div class="text-align-center">
                        <input type="button" value="Cancel" class="btn-blue2" onclick="CancelSaveSearch();" />
                        <asp:Button ID="btnSubmit" CssClass="btn-blue2" runat="server" Text="Submit" ValidationGroup="SaveSearch"
                            OnClick="btnSubmit_Click"></asp:Button>
                        <asp:Button ID="btnUpdate" CssClass="btn-blue2 display-none" runat="server" Text="Update"
                            ValidationGroup="SaveSearch" OnClick="btnUpdate_Click"></asp:Button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlSaveArticle" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;" runat="server" DefaultButton="btnSaveArticle">
    <asp:UpdatePanel ID="upSaveArticle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span id="spnSaveArticleTitle">Article Details</span>
                    <div style="width: 27px; float: right;" id="div7" runat="server">
                        <img id="btnClose" src="../Images/close-icon.png" onclick="closeModal('pnlSaveArticle');" />
                        <%--<input type="image" id="imgEditArticleClos" class="popup-top-close" runat="server"
                            onclick="closeModal('pnlEditArticle');" src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div clchass="pnlPopup-right-close">
                        <input type="button" id="btnClose" class="btn-cancel" value=" " onclick="$find('mdlpopupSaveArticle').hide();" />
                    </div>--%>
                </div>
                <div class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="vlSummerySaveArticle" runat="server" ValidationGroup="vgSaveArticle"
                            CssClass="error-summery" />
                        <asp:Label ID="lblSaveArticleMsg" runat="server" Visible="true" CssClass="error-msg"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>
                            <label>
                                Title :</label>
                            <asp:HiddenField ID="hdnSaveArticleID" runat="server" />
                            <asp:HiddenField ID="hdnArticleType" runat="server" />
                            <asp:TextBox ID="txtArticleTitle" runat="server" Width="74%" MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtArticleTitle" Text="*" runat="server" ControlToValidate="txtArticleTitle"
                                ErrorMessage="Title is required." Display="None" ValidationGroup="vgSaveArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Primary Category :</label>
                            <asp:DropDownList ID="ddlPCategory" runat="server" onclick="selectList_cache=this.value"
                                onchange="return UpdateSubCategory1(this.id,0);" Width="36%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgSaveArticle"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Sub Category 1 :</label>
                            <asp:DropDownList ID="ddlSubCategory1" runat="server" onchange="UpdateSubCategory1(this.id,0);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 2 :</label>
                            <asp:DropDownList ID="ddlSubCategory2" runat="server" onchange="UpdateSubCategory1(this.id,0);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 3 :</label>
                            <asp:DropDownList ID="ddlSubCategory3" runat="server" onchange="UpdateSubCategory1(this.id,0);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Description :</label>
                            <asp:TextBox ID="txtADescription" MaxLength="1000" runat="server" TextMode="MultiLine"
                                Style="width: 74%; height: 145px; overflow: auto;">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtDescription" Text="*" runat="server" ControlToValidate="txtADescription"
                                ErrorMessage="Description is required." Display="None" ValidationGroup="vgSaveArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Keywords :</label>
                            <asp:TextBox ID="txtKeywords" runat="server" MaxLength="500" TextMode="MultiLine"
                                Style="width: 74%; height: 145px; overflow: auto;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtKeywords" Text="*" runat="server" ControlToValidate="txtKeywords"
                                ErrorMessage="Keywords is required." Display="None" ValidationGroup="vgSaveArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Rate This Article :</label>
                            <div>
                                <div style="width: 74%; overflow: auto;">
                                    <span class="float-left RateArticleNumberLeft" style="">1</span> <span class="float-left">
                                        <asp:TextBox ID="txtArticleRate" runat="server" Style="display: none;" onchange="sliderChange('txtArticleRate','chkArticlePreferred')">
                                        </asp:TextBox>
                                        <Ajax:SliderExtender ID="seArticle" runat="server" TargetControlID="txtArticleRate"
                                            Minimum="1" Maximum="6" Steps="6" RaiseChangeOnlyOnMouseUp="true" BehaviorID="seArticle">
                                        </Ajax:SliderExtender>
                                    </span><span class="float-left RateArticleNumberRight">6</span> <span class="float-left prefferredcheckbox">
                                        <asp:CheckBox ID="chkArticlePreferred" runat="server" Text="Preferred" onclick="PrefferredChecked('chkArticlePreferred','seArticle');" />
                                    </span>
                                </div>
                                <%--<div>
                                    <asp:CheckBox ID="chkArticlePreferred" runat="server" Text="Preferred" Style="margin-left: -110px;
                                        margin-top: 3px;" onclick="PrefferredChecked('chkArticlePreferred','seArticle');" />
                                </div>--%>
                            </div>
                        </li>
                    </ul>
                    <div class="text-align-center">
                        <input type="button" id="Button1" class="btn-blue2" value="Cancel" onclick="closeModal('pnlSaveArticle');" />
                        <asp:Button ID="btnSaveArticle" runat="server" Text="Save" ValidationGroup="vgSaveArticle"
                            CausesValidation="true" CssClass="btn-blue2" OnClick="btnSaveArticle_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlSaveArticle1" CssClass="modal hide fade resizable modalPopupDiv height35p"
    Style="display: none;" runat="server" DefaultButton="btnSaveArticle">
    <asp:UpdatePanel ID="upSaveArticle1" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span id="Span1">Article Details</span>
                    <div style="width: 27px; float: right;" id="div2" runat="server">
                        <img id="Img3" src="../Images/close-icon.png" onclick="closeModal('pnlSaveArticle1');" />
                        <%--<input type="image" id="imgEditArticleClos" class="popup-top-close" runat="server"
                            onclick="closeModal('pnlEditArticle');" src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div clchass="pnlPopup-right-close">
                        <input type="button" id="btnClose" class="btn-cancel" value=" " onclick="$find('mdlpopupSaveArticle').hide();" />
                    </div>--%>
                </div>
                <div class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="vlSummerySaveArticle1" runat="server" ValidationGroup="vgSaveArticle1"
                            CssClass="error-summery" />
                        <asp:Label ID="lblSaveArticleErrMsg" runat="server" Visible="true" CssClass="error-msg"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>
                            <asp:HiddenField ID="hdnSaveArticleType" runat="server" />
                            <label>
                                Primary Category :</label>
                            <asp:DropDownList ID="ddlArticlePCategory" runat="server" onclick="selectList_cache=this.value"
                                onchange="return UpdateSubCategory1(this.id,1);" Width="75%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlArticlePCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgSaveArticle1"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Sub Category 1 :</label>
                            <asp:DropDownList ID="ddlArticleSubCategory1" runat="server" onchange="UpdateSubCategory1(this.id,1);"
                                Width="75%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 2 :</label>
                            <asp:DropDownList ID="ddlArticleSubCategory2" runat="server" onchange="UpdateSubCategory1(this.id,1);"
                                Width="75%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 3 :</label>
                            <asp:DropDownList ID="ddlArticleSubCategory3" runat="server" onchange="UpdateSubCategory1(this.id,1);"
                                Width="75%">
                            </asp:DropDownList>
                        </li>
                    </ul>
                    <div class="text-align-center">
                        <input type="button" id="Button2" class="btn-blue2" value="Cancel" onclick="closeModal('pnlSaveArticle1');" />
                        <asp:Button ID="btnSaveArticle1" runat="server" Text="Save" ValidationGroup="vgSaveArticle1"
                            CausesValidation="true" CssClass="btn-blue2" OnClick="btnSaveArticle1_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<script type="text/javascript">
    $('#divActiveFilter').append('<div id="divDateActiveFilter" class="filter-in">Date<span class="cancel" onclick="CallHandler1(ss,hh,kk);removeActiveFilter(this);"></span></div>'); 

</script>
