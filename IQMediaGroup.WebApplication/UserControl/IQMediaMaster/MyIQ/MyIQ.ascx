<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyIQ.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.MyIQ.MyIQ" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="../CustomPager/CustomPager.ascx" TagName="CustomPager" TagPrefix="uc" %>
<%--<style type="text/css">
    .lidivText
    {
        float: left; /*padding-top: 5px;*/
        width: 75%;
    }
    
    .lidivFilter
    {
        float: left; /*padding-top: 6px;*/
        width: 18%;
    }
    .lidivShow
    {
        float: left;
        padding-top: 3px;
        width: 7%;
    }
    .btn-edit
    {
        height: 22px;
        width: 22px;
        border-style: none;
        cursor: pointer;
        background: url(../images/gridicon-edit.png) no-repeat;
        display: inline-block;
        text-decoration: none;
    }
    
    .btn-update
    {
        background: url("../images/gridicon-update.png") no-repeat scroll 0 0 transparent;
        border-style: none;
        cursor: pointer;
        display: inline-block;
        height: 22px;
        text-decoration: none;
        width: 22px;
    }
    
    .btn-delete
    {
        height: 22px;
        width: 22px;
        border-style: none;
        cursor: pointer;
        background: url(../images/gridicon-delete.png) no-repeat;
        display: inline-block;
        text-decoration: none;
    }
    
    .content-text2
    {
        color: #535353;
        font-family: "avenir_65medium";
        font-size: 11px;
        line-height: 15px;
        padding: 4px;
        text-decoration: none;
    }
    
    .heading-blue, .heading-blue a
    {
        color: #335F97;
        font-family: "avenir_65medium";
        font-size: 12px;
        font-weight: bold;
        line-height: 12px;
        padding-left: 4px;
        text-align: left;
        text-decoration: none;
    }
    .popUpInner
    {
        background: url("../images/bluebox-hd-bg.jpg") repeat-x scroll 0 0 white;
        border: 4px solid #DEDEDE;
        border-collapse: separate;
        border-radius: 5px 5px 5px 5px;
    }
</style>--%>
<asp:UpdatePanel ID="upMainSearch" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel ID="divMainSearch" runat="server" CssClass="search-box-bg">
            <div class="form-search margintop8">
                <div class="input-append width100p">
                    <asp:TextBox CssClass="span2 search-query" ID="txtSearch" BackColor="White" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearch2" runat="server" Text="Search" CssClass="btn height30"
                        OnClick="btnSearch_Click" />
                </div>
            </div>
            <br />
            <div class="search-nav" id="divHeaderFilter">
                <asp:CheckBox ID="chkTV" runat="server" onclick="ShowHideFilterOnCheckbox(this)"
                    Checked="true" />
                <a href="javascript:;" class="search-link">TV</a>
                <asp:CheckBox ID="chkNews" runat="server" onclick="ShowHideFilterOnCheckbox(this)"
                    Checked="true" />
                <a href="javascript:;" runat="server" id="aOnlineNews" class="search-link">Online News</a>
                <asp:CheckBox ID="chkSocialMedia" runat="server" onclick="ShowHideFilterOnCheckbox(this)"
                    Checked="true" />
                <a href="javascript:;" runat="server" id="aSocialMedia" class="search-link">Social Media</a>
                <asp:CheckBox ID="chkPrintMedia" runat="server" onclick="ShowHideFilterOnCheckbox(this)"
                    Checked="true" />
                <a href="javascript:;" runat="server" id="aPrintMedia" class="search-link">Print Media</a>
                <asp:CheckBox ID="chkTwitter" runat="server" onclick="ShowHideFilterOnCheckbox(this)"
                    Checked="true" />
                <a href="javascript:;" runat="server" id="aTweet" class="search-link">Twitter</a>
            </div>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<br />
<div class="clear">
    <asp:UpdatePanel ID="upBtnSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="divleftSearch" runat="server" CssClass="span3">
                <div class="well sidebar-nav" id="divFilterSide">
                    <asp:Label ID="lblSearchErr" runat="server" ForeColor="Red"></asp:Label>
                    <asp:ValidationSummary ID="vlsSearch" runat="server" ValidationGroup="vgSearch" />
                    <ul class="nav nav-list marginleft-28">
                        <li class="ulheader">
                            <div class="lidivLeft">
                                &nbsp; SEARCH</div>
                            <div class="marginleft72p">
                                <asp:Button ID="btnReset" OnClick="btnReset_Click" runat="server" Text="Reset All"
                                    CssClass="btn-blue2" />
                            </div>
                        </li>
                    </ul>
                    <div id="divSearch" runat="server">
                        <ul class="nav nav-list listBorder filterul" id="ulSearchFilter">
                            <li class="ulhead">Filter</li>
                            <li id="liFilterInner">
                                <ul class="nav">
                                    <li id="liCategoryFilter1">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="ShowHideFilter('','CategoryFilter1');">
                                                <div class="lidivLeft">
                                                    <img src="~/Images/filter.png" runat="server" id="imgCategoryFilter1" alt="" />&nbsp;&nbsp;Category
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divCategoryFilterStatus1" runat="server">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowCategoryFilter1" class="filterDirectionImg" src="../Images/show.png"
                                                        alt="" />
                                                </div>
                                            </div>
                                            <div id="divCategoryFilter1" class="bs-docs-example display-none center">
                                                <div class="width100p">
                                                    <div>
                                                        <asp:DropDownList ID="ddlCategory1" runat="server" CssClass="width90">
                                                        </asp:DropDownList>
                                                        <asp:RadioButtonList CellPadding="3" ID="rdoCategoryOperator1" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                                            <asp:ListItem Text="Or" Value="Or" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div>
                                                        <asp:DropDownList ID="ddlCategory2" runat="server" CssClass="width90">
                                                        </asp:DropDownList>
                                                        <asp:RadioButtonList CellPadding="3" ID="rdoCategoryOperator2" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                                            <asp:ListItem Text="Or" Value="Or" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div>
                                                        <asp:DropDownList ID="ddlCategory3" runat="server" CssClass="width90">
                                                        </asp:DropDownList>
                                                        <asp:RadioButtonList CellPadding="3" ID="rdoCategoryOperator3" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="And" Value="And"></asp:ListItem>
                                                            <asp:ListItem Text="Or" Value="Or" Selected="True"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </div>
                                                    <div>
                                                        <asp:DropDownList ID="ddlCategory4" runat="server" CssClass="width90">
                                                        </asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liUserFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="ShowHideFilter('UserExpandimg','UserFilter');">
                                                <div class="lidivLeft">
                                                    <img src="~/images/filter.png" runat="server" alt="" id="imgUserFilter" />&nbsp;&nbsp;User
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divUserFilterStatus" runat="server">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowUserFilter" class="filterDirectionImg" src="../images/show.png" alt="" />
                                                </div>
                                            </div>
                                            <div id="divUserFilter" class="bs-docs-example display-none">
                                                <img id="UserExpandimg" src="../images/collapse_icon.png" onclick="OnImageExpandCollapseClick('UserExpandimg','divUser');"
                                                    alt="" /><asp:CheckBox ID="chkUserAll" runat="server" Text="Select All" Checked="true" />
                                                <div id="divUser" class="div-subchecklist">
                                                    <asp:CheckBoxList ValidationGroup="vgSearch" CellPadding="1" CellSpacing="1" ID="chkOwnerList"
                                                        Style="vertical-align: middle;" runat="server">
                                                    </asp:CheckBoxList>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liTimeFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="ShowHideFilter('','TimeFilter');">
                                                <div class="lidivLeft">
                                                    <img src="~/images/filter.png" runat="server" id="imgTimeFilter" alt="" />&nbsp;&nbsp;Time
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divTimeFilterStatus" runat="server">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowTimeFilter" class="filterDirectionImg" src="../images/show.png" alt="" />
                                                </div>
                                            </div>
                                            <div id="divTimeFilter" class="bs-docs-example display-none">
                                                From Date:
                                                <asp:TextBox ValidationGroup="vgSearch" ID="txtFromDate" runat="server" AutoCompleteType="None"
                                                    CssClass="programTextbox"></asp:TextBox>
                                                <AjaxToolkit:CalendarExtender ID="CalEtxtFromDate" runat="server" CssClass="MyCalendar"
                                                    TargetControlID="txtFromDate">
                                                </AjaxToolkit:CalendarExtender>
                                                To Date:
                                                <asp:TextBox ValidationGroup="vgSearch" ID="txtToDate" runat="server" AutoCompleteType="None"
                                                    CssClass="programTextbox"></asp:TextBox>
                                                <AjaxToolkit:CalendarExtender ID="valEtxtToDate" runat="server" CssClass="MyCalendar"
                                                    TargetControlID="txtToDate">
                                                </AjaxToolkit:CalendarExtender>
                                                <asp:CompareValidator ValidationGroup="vgSearch" ID="cmpDateValidator" runat="server"
                                                    ControlToCompare="txtToDate" ControlToValidate="txtFromDate" Text="*" ErrorMessage="To Date Must be greater than From Date"
                                                    Operator="LessThanEqual" Type="Date" Display="Dynamic">
                                                </asp:CompareValidator>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liSearchTermFilter">
                                        <div class="divflt">
                                            <div class="lidivTop" onclick="ShowHideFilter('SearchTermExpandimg','SearchTermFilter');">
                                                <div class="lidivLeft">
                                                    <img src="~/images/filter.png" runat="server" alt="" id="imgSearchTermFilter" />&nbsp;&nbsp;Search
                                                    Term
                                                </div>
                                                <div class="lidivRight">
                                                    <div id="divSearchTermFilterStatus" runat="server">
                                                        OFF
                                                    </div>
                                                    <img id="imgShowSearchTermFilter" class="filterDirectionImg" src="../images/show.png"
                                                        alt="" />
                                                </div>
                                            </div>
                                            <div id="divSearchTermFilter" class="bs-docs-example display-none">
                                                <img id="SearchTermExpandimg" src="../images/collapse_icon.png" onclick="OnImageExpandCollapseClick('SearchTermExpandimg','divSearchTerm');"
                                                    alt="" /><asp:CheckBox ID="chkSearchTermSelectAll" runat="server" Text="Select All"
                                                        Checked="true" />
                                                <div id="divSearchTerm" class="div-subchecklist">
                                                    <%--style="vertical-align: bottom;"--%>
                                                    <input type="checkbox" id="cbTitle" runat="server" value="title" checked="checked"
                                                        onclick="setCheckbox('divSearchTerm','ctl00_Content_Data_UCMyIQControl_chkSearchTermSelectAll')" /><span>Title</span><br />
                                                    <input type="checkbox" id="cbDescription" runat="server" value="description" checked="checked"
                                                        onclick="setCheckbox('divSearchTerm','ctl00_Content_Data_UCMyIQControl_chkSearchTermSelectAll')" /><span>Description</span><br />
                                                    <input type="checkbox" id="cbKeywords" runat="server" value="keywords" checked="checked"
                                                        onclick="setCheckbox('divSearchTerm','ctl00_Content_Data_UCMyIQControl_chkSearchTermSelectAll')" /><span>Keywords</span><br />
                                                    <input type="checkbox" id="cbCC" runat="server" value="cc" checked="checked" onclick="setCheckbox('divSearchTerm','ctl00_Content_Data_UCMyIQControl_chkSearchTermSelectAll')" /><span>CC
                                                        / Content</span>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                        <div class="divApply" id="divApply" style="display: block;">
                            <asp:Button ID="btnSearch" ValidationGroup="vgSearch" OnClick="btnSearch_Click" runat="server"
                                Text="Find Clips" CssClass="btn-blue2 width90" /><br />
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upMainGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel ID="divMinGrid" runat="server" CssClass="span9 mraginleft2p">
                <div class="navbar">
                    <div id="divSearchResult" runat="server">
                        <div class="show-hide cursor center" onclick="ShowHideDivResult('divResult',true);">
                            <div class="float-left">
                                <a href="javascript:;">RESULTS</a></div>
                            <div class="float-right">
                                <a href="javascript:;" class="right-dropdown">
                                    <div class="float-left" id="divResultShowHideTitle">
                                        HIDE
                                    </div>
                                    <img id="imgdivResultShowHide" class="imgshowHide" src="../images/hiden.png" alt="">
                                </a>
                            </div>
                        </div>
                        <div id="divResult" class="tabMain">
                            <asp:HiddenField ID="hfCurrentTabIndex" runat="server" />
                            <div id="tabsMain" style="background-color: White;">
                                <div id="divGridTab" class="clear tabdiv">
                                    <div id="tabTV" runat="server" visible="false" class="active float-left" onclick="ChangeTab('ctl00_Content_Data_UCMyIQControl_tabTV','divGridTab','TV');">
                                        TV
                                    </div>
                                    <div id="tabNews" class="float-left" runat="server" visible="false" onclick="ChangeTab('ctl00_Content_Data_UCMyIQControl_tabNews','divGridTab','News');">
                                        Online News
                                    </div>
                                    <div id="tabSocialMedia" class="float-left" runat="server" visible="false" onclick="ChangeTab('ctl00_Content_Data_UCMyIQControl_tabSocialMedia','divGridTab','SocialMedia');">
                                        Social Media
                                    </div>
                                    <div id="tabPrintMedia" class="float-left" runat="server" visible="false" onclick="ChangeTab('ctl00_Content_Data_UCMyIQControl_tabPrintMedia','divGridTab','PrintMedia');">
                                        Print Media
                                    </div>
                                    <div id="tabTweet" class="float-left" runat="server" visible="false" onclick="ChangeTab('ctl00_Content_Data_UCMyIQControl_tabTweet','divGridTab','Tweet');">
                                        Twitter
                                    </div>
                                </div>
                                <div class="tabContentDiv">
                                    <div id="divTVResult" runat="server" visible="true">
                                        <asp:UpdatePanel ID="upTVGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="divTVResultInner" class="display-none">
                                                    <asp:HiddenField ID="hfTVStatus" runat="server" Value="1" />
                                                    <div class="float-left">
                                                        <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorClips" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="float-right padding5">
                                                        <asp:ImageButton ID="btnRefreshLibrary" CssClass="imgicon" runat="server" OnClick="btnRefreshLibrary_Click"
                                                            ImageUrl="~/Images/refresh.png" AlternateText="Refresh Library" ToolTip="Refresh Library" />
                                                        <asp:ImageButton ID="btnManageCategories" CssClass="imgicon" runat="server" OnClick="btnManageCategories_Click"
                                                            ImageUrl="~/Images/setting.png" AlternateText="Manage Categories" ToolTip="Manage Categories" />
                                                        <asp:ImageButton ID="lnkEmail" CssClass="imgicon" runat="server" OnClientClick="return ValidateEmailGrid('grvClip');"
                                                            ImageUrl="~/Images/email.png" AlternateText="Email Selected Clip(s)" ToolTip="Email Selected Clip(s)" />
                                                        <asp:ImageButton ID="btnClipDownload" CssClass="imgicon" runat="server" OnClick="btnClipDownload_Click"
                                                            ImageUrl="~/Images/download.png" AlternateText="Download Clips" ToolTip="Download Clips" />
                                                        <asp:ImageButton ID="btnRemoveClips" CssClass="imgicon" runat="server" OnClientClick="return ValidateGrid('grvClip');"
                                                            OnClick="btnRemoveClips_Click" ImageUrl="~/Images/close.png" AlternateText="Remove Selected Clip(s)"
                                                            ToolTip="Remove Selected Clip(s)" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:GridView ID="grvClip" runat="server" Width="100%" BorderColor="#e4e4e4" border="0"
                                                        Style="border-collapse: collapse;" ShowHeader="true" CellPadding="5" CellSpacing="0"
                                                        AutoGenerateColumns="false" EmptyDataText="No Results Found" AllowPaging="false"
                                                        OnSorting="grvClip_Sorting" AllowSorting="true" CssClass="grid grid-iq" BackColor="#FFFFFF">
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="false" HeaderText="Play">
                                                                <ItemTemplate>
                                                                    <asp:Button ID="lbtnLink" runat="server" CommandArgument='<%# Bind("ClipID") %>'
                                                                        OnCommand="lbtnPlay_OnCommand" CssClass="btn-play"></asp:Button>
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" Height="20px" />
                                                                <HeaderStyle CssClass="grid-th center" Width="5%" VerticalAlign="Top" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Station">
                                                                <ItemTemplate>
                                                                    <img id="Img2" alt="" src='<%# Eval("ClipLogo") %>' runat="Server" width="23" height="24"
                                                                        border="0" />
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" Width="7%" CssClass="grid-th center" />
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Title" SortExpression="ClipTitle">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClipTitle" Text='<%# Eval("ClipTitle") %>' runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" Width="18%" CssClass="grid-th-left" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Clip Date" SortExpression="ClipCreationDate">
                                                                <ItemTemplate>
                                                                    <%# string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Eval("ClipCreationDate"))) + "<br/>" + string.Format("{0:hh:mm tt}", Convert.ToDateTime(Eval("ClipCreationDate")))%>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-right" Height="20px" Width="11%" />
                                                                <ItemStyle CssClass="right" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Air Date" SortExpression="ClipDate">
                                                                <ItemTemplate>
                                                                    <%# string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Eval("ClipDate"))) + "<br/>" + string.Format("{0:hh:mm tt}", Convert.ToDateTime(Eval("ClipDate")))%>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-right" Width="11%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="right" Height="20px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <HeaderTemplate>
                                                                    Nielsen<br />
                                                                    Audience
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("Audience"))) ? "NA" : Convert.ToInt64(Eval("Audience")).ToString("#,#")%>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-right" Width="9%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="right" Height="20px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="iQ media <br /> Value">
                                                                <ItemTemplate>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("Sqad_ShareValue"))) ? "NA" : Convert.ToInt64(Eval("Sqad_ShareValue").ToString().Split('(')[0]).ToString("#,#") + "(" + Eval("Sqad_ShareValue").ToString().Split('(')[1]%>
                                                                </ItemTemplate>
                                                                <%--<HeaderStyle CssClass="grid-th-right" Width="14%" VerticalAlign="Top" />--%>
                                                                <HeaderStyle CssClass="grid-th-right" Width="9%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="right" Height="20px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Category">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblCategory" runat="server" Text="Categories"></asp:Label>
                                                                    <br />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("CategoryName"))) ? string.Empty : Eval("CategoryName").ToString().Replace(",","<br/>")%>
                                                                </ItemTemplate>
                                                                <%--<HeaderStyle Height="20px" CssClass="grid-th-left" Width="10%" VerticalAlign="Top" />--%>
                                                                <HeaderStyle Height="20px" CssClass="grid-th-left" Width="14%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Owner">
                                                                <HeaderTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text="Owner"></asp:Label>
                                                                    <br />
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-left" Height="20px" Width="7%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                        Text=" " OnClick="lbtnEdit_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" Width="3%" CssClass="center" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="false" HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <input type="checkbox" id="chkDelete" runat="server" value='<%# Eval("ArchiveClipKey") %>' />
                                                                    <asp:HiddenField ID="hfClipID" runat="server" Value='<%# Eval("ClipID") %>' />
                                                                    <asp:HiddenField ID="hfArchiveClipKey" runat="server" Value='<%# Eval("ArchiveClipKey") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                                <HeaderStyle Width="6%" Height="20px" CssClass="grid-th center" VerticalAlign="Top" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle CssClass="grid-th" Height="10px" HorizontalAlign="Center" VerticalAlign="Top">
                                                        </HeaderStyle>
                                                    </asp:GridView>
                                                    <uc:CustomPager ID="ucCustomPager" runat="server" On_PageIndexChange="ucCustomPager_PageIndexChange"
                                                        PageSize="10" />
                                                    <asp:Label ID="lblNoResults" runat="server" Visible="false"></asp:Label>
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="divNewsResult" runat="server" class="tabInnerDiv">
                                        <asp:UpdatePanel ID="upNewsGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="divNewsResultInner" class="display-none">
                                                    <asp:HiddenField ID="hfNewsStatus" runat="server" Value="0" />
                                                    <div class="float-left">
                                                        <asp:Label ID="lblSuccessMessageArticle" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorMsgArticle" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="float-right padding5">
                                                        <asp:ImageButton ID="imgbtnRefreshNews" CssClass="imgicon" runat="server" OnClick="imgbtnRefreshNews_Click"
                                                            ImageUrl="~/Images/refresh.png" AlternateText="Refresh Library" ToolTip="Refresh Library"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnManageCategories1" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClick="btnManageCategories_Click" ImageUrl="~/Images/setting.png" AlternateText="Manage Categories"
                                                            ToolTip="Manage Categories" />
                                                        <asp:ImageButton ID="lnkEmailNews" CssClass="imgicon" runat="server" OnClientClick="return ValidateEmailGrid('grvArticle');"
                                                            ImageUrl="~/Images/email.png" AlternateText="Email Selected Article(s)" ToolTip="Email Selected Article(s)"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnArticleNMDownload" CssClass="imgicon" runat="server" OnClick="btnArticleNMDownload_Click"
                                                            ImageUrl="~/Images/download.png" AlternateText="Download Articles" ToolTip="Download Articles"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnRemoveArticle" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClientClick="return ValidateGrid('grvArticle');" OnClick="btnRemoveArticle_Click"
                                                            ImageUrl="~/Images/close.png" AlternateText="Remove Selected Article(s)" ToolTip="Remove Selected Article(s)" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:GridView ID="grvArticle" runat="server" Width="100%" BorderColor="#e4e4e4" border="0"
                                                        Style="border-collapse: collapse;" ShowHeader="true" CellPadding="5" CellSpacing="0"
                                                        AutoGenerateColumns="false" EmptyDataText="No Results Found" AllowPaging="false"
                                                        OnSorting="grvArticle_Sorting" AllowSorting="true" CssClass="grid grid-iq" BackColor="#FFFFFF">
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="imgArticleButton" ImageUrl="~/Images/NewsRead.png" NavigateUrl='<%# Eval("Url") %>' Target="_blank" runat="server"></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th center" Width="7%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="center" Width="7%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title">
                                                                <HeaderStyle Height="20px" Width="30%" CssClass="grid-th-left" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CreatedDate" SortExpression="CreatedDate" HtmlEncode="False"
                                                                HeaderText="Created<br/>Date" DataFormatString="{0:MM/dd/yyyy<br/>hh:mm tt}">
                                                                <HeaderStyle CssClass="grid-th-right" Height="20px" Width="12%" />
                                                                <ItemStyle CssClass="right" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Categories">
                                                                <ItemTemplate>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("CategoryNames"))) ? string.Empty : Eval("CategoryNames").ToString().Replace(",", "<br/>")%>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-left" Width="20%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Owner">
                                                                <ItemTemplate>
                                                                    <%# Eval("FirstName") %>
                                                                    <%# Eval("LastName") %>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-left" Height="20px" Width="20%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField ShowHeader="false" HeaderText="Download">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnDownload" runat="server" ImageUrl="~/Images/Down.png" CommandArgument='<%# Convert.ToString(Eval("ArticleID")) + "," + Convert.ToString(Eval("Title"))  %>'
                                                                        OnCommand="ibtnDownload_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                                <HeaderStyle Height="20px" Width="9%" CssClass="grid-th center" VerticalAlign="Top" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnArticleEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                        Text=" " OnClick="lbtnArticleEdit_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" Width="5%" CssClass="grid-th center" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="false" HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <input type="checkbox" id="chkArticleDelete" runat="server" value='<%# Eval("ArchiveNMKey") %>' />
                                                                    <asp:HiddenField ID="hfArchiveNMKey" runat="server" Value='<%# Eval("ArchiveNMKey") %>' />
                                                                    <asp:HiddenField ID="hfArticleNMID" runat="server" Value='<%# Eval("ArticleID") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                                <HeaderStyle Width="6%" Height="20px" CssClass="grid-th center" VerticalAlign="Top" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <uc:CustomPager ID="ucCustomPagerArticle" runat="server" On_PageIndexChange="ucCustomPagerArticle_PageIndexChange"
                                                        PageSize="10" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="divSocialMediaResult" runat="server" class="tabInnerDiv">
                                        <asp:UpdatePanel ID="upSocialMediaGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="divSocialMediaResultInner" class="display-none">
                                                    <asp:HiddenField ID="hfSocialMediaStatus" runat="server" Value="0" />
                                                    <div class="float-left">
                                                        <asp:Label ID="lblSuccessMessageArticleSM" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorMsgArticleSM" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="float-right padding5">
                                                        <asp:ImageButton ID="imgbtnRefreshSocialMedia" CssClass="imgicon" runat="server"
                                                            OnClick="imgbtnRefreshSocialMedia_Click" ImageUrl="~/Images/refresh.png" AlternateText="Refresh Library"
                                                            ToolTip="Refresh Library" Visible="false" />
                                                        <asp:ImageButton ID="btnManageCategories2" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClick="btnManageCategories_Click" ImageUrl="~/Images/setting.png" AlternateText="Manage Categories"
                                                            ToolTip="Manage Categories" />
                                                        <asp:ImageButton ID="lnkEmailSocialMedia" CssClass="imgicon" runat="server" OnClientClick="return ValidateEmailGrid('grvSocialArticle');"
                                                            ImageUrl="~/Images/email.png" AlternateText="Email Selected Article(s)" ToolTip="Email Selected Article(s)"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnArticleSMDownload" CssClass="imgicon" runat="server" OnClick="btnArticleSMDownload_Click"
                                                            ImageUrl="~/Images/download.png" AlternateText="Download Articles" ToolTip="Download Articles"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnRemoveSocialArticle" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClientClick="return ValidateGrid('grvSocialArticle');" OnClick="btnRemoveSocialArticle_Click"
                                                            ImageUrl="~/Images/close.png" AlternateText="Remove Selected Article(s)" ToolTip="Remove Selected Article(s)" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:GridView ID="grvSocialArticle" runat="server" Width="100%" BorderColor="#e4e4e4"
                                                        border="0" Style="border-collapse: collapse;" ShowHeader="true" CellPadding="5"
                                                        CellSpacing="0" AutoGenerateColumns="false" EmptyDataText="No Results Found"
                                                        AllowPaging="false" OnSorting="grvSocialArticle_Sorting" AllowSorting="true"
                                                        CssClass="grid grid-iq" BackColor="#FFFFFF">
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="imgSocialArticleButton" ImageUrl="~/Images/NewsRead.png" NavigateUrl='<%# Eval("Url") %>' Target="_blank" runat="server"></asp:HyperLink>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th center" Width="7%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="center" Width="7%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Title" HeaderText="Title" SortExpression="Title">
                                                                <HeaderStyle Height="20px" Width="30%" CssClass="grid-th-left" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="CreatedDate" SortExpression="CreatedDate" HtmlEncode="False"
                                                                HeaderText="Created<br/>Date" DataFormatString="{0:MM/dd/yyyy<br/>hh:mm tt}">
                                                                <HeaderStyle CssClass="grid-th-right" Height="20px" Width="12%" />
                                                                <ItemStyle CssClass="right" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Categories">
                                                                <ItemTemplate>
                                                                    <%# string.IsNullOrEmpty(Convert.ToString(Eval("CategoryNames"))) ? string.Empty : Eval("CategoryNames").ToString().Replace(",", "<br/>")%>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-left" Width="20%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="Owner">
                                                                <ItemTemplate>
                                                                    <%# Eval("FirstName") %>
                                                                    <%# Eval("LastName") %>
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th-left" Height="20px" Width="20%" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:TemplateField>
                                                            <%--<asp:TemplateField ShowHeader="false" HeaderText="Download">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="ibtnSocialDownload" runat="server" ImageUrl="~/Images/Down.png"
                                                                        CommandArgument='<%# Convert.ToString(Eval("ArticleID")) + "," + Convert.ToString(Eval("Title"))  %>'
                                                                        OnCommand="ibtnSocialDownload_Command" />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                                <HeaderStyle Height="20px" Width="9%" CssClass="grid-th center" VerticalAlign="Top" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnSocialArticleEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                        Text=" " OnClick="lbtnSocialArticleEdit_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" Width="5%" CssClass="grid-th center" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="false" HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <input type="checkbox" id="chkSocialArticleDelete" runat="server" value='<%# Eval("ArchiveSMKey") %>' />
                                                                    <asp:HiddenField ID="hfArticleSMID" runat="server" Value='<%# Eval("ArticleID") %>' />
                                                                    <asp:HiddenField ID="hfArchiveSMKey" runat="server" Value='<%# Eval("ArchiveSMKey") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                                <HeaderStyle Width="6%" Height="20px" CssClass="grid-th center" VerticalAlign="Top" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <uc:CustomPager ID="ucCustomPagerSocialArticle" runat="server" On_PageIndexChange="ucCustomPagerSocialArticle_PageIndexChange"
                                                        PageSize="10" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="divPrintMediaResult" runat="server" class="tabInnerDiv">
                                        <asp:UpdatePanel ID="upPrintMediaGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="divPrintMediaResultInner" class="display-none">
                                                    <asp:HiddenField ID="hfPrintMediaStatus" runat="server" Value="0" />
                                                    <div class="float-left">
                                                        <asp:Label ID="lblSuccessMessagePM" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorMsgPM" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div class="float-right padding5">
                                                        <asp:ImageButton ID="imgbtnRefreshPrintMedia" CssClass="imgicon" runat="server" OnClick="imgbtnRefreshPrintMedia_Click"
                                                            ImageUrl="~/Images/refresh.png" AlternateText="Refresh Library" ToolTip="Refresh Library"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnManageCategories3" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClick="btnManageCategories_Click" ImageUrl="~/Images/setting.png" AlternateText="Manage Categories"
                                                            ToolTip="Manage Categories" />
                                                        <asp:ImageButton ID="lnkEmailPrintMedia" CssClass="imgicon" runat="server" OnClientClick="return ValidateEmailGrid('grvPrintMedia');"
                                                            ImageUrl="~/Images/email.png" AlternateText="Email Selected Article(s)" ToolTip="Email Selected Article(s)"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnPrintMediaDownload" CssClass="imgicon" runat="server" OnClick="btnPrintMediaDownload_Click"
                                                            ImageUrl="~/Images/download.png" AlternateText="Download Articles" ToolTip="Download Articles"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnRemovePrintMedia" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClientClick="return ValidateGrid('grvPrintMedia');" OnClick="btnRemovePrintMedia_Click"
                                                            ImageUrl="~/Images/close.png" AlternateText="Remove Selected Article(s)" ToolTip="Remove Selected Article(s)" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <asp:GridView ID="grvPrintMedia" runat="server" Width="100%" BorderColor="#e4e4e4"
                                                        border="0" Style="border-collapse: collapse;" ShowHeader="true" CellPadding="5"
                                                        CellSpacing="0" AutoGenerateColumns="false" EmptyDataText="No Results Found"
                                                        AllowPaging="false" OnSorting="grvPrintMedia_Sorting" AllowSorting="true" CssClass="grid grid-iq"
                                                        BackColor="#FFFFFF">
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                                <ItemTemplate>
                                                                    <asp:ImageButton OnCommand="imgPrintMediaButton_Command" ID="imgPrintMediaButton"
                                                                        runat="server" CommandArgument='<%# Eval("Url") %>' ImageUrl="~/Images/NewsRead.png"
                                                                        CommandName="ShowArticle" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th center" Width="7%" VerticalAlign="Middle" />
                                                                <ItemStyle CssClass="center" Width="7%" />
                                                            </asp:TemplateField>
                                                            <asp:BoundField DataField="Pub_Name" HeaderText="Publication Name" SortExpression="Pub_Name">
                                                                <HeaderStyle Height="20px" Width="26%" CssClass="grid-th-left" VerticalAlign="Middle" />
                                                                <ItemStyle CssClass="left"></ItemStyle>
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Headline" SortExpression="Headline" HeaderText="Headline">
                                                                <HeaderStyle CssClass="grid-th-left" Height="20px" Width="20%" />
                                                                <ItemStyle CssClass="left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="PubDate" SortExpression="PubDate" HtmlEncode="False" HeaderText="Publication<br/>Date"
                                                                DataFormatString="{0:MM/dd/yyyy<br/>hh:mm tt}">
                                                                <HeaderStyle CssClass="grid-th-right" Height="20px" Width="12%" />
                                                                <ItemStyle CssClass="right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Keywords" SortExpression="Keywords" HeaderText="Keywords">
                                                                <HeaderStyle CssClass="grid-th-left" Height="20px" Width="17%" />
                                                                <ItemStyle CssClass="left" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Rating" SortExpression="Rating" HeaderText="Rating">
                                                                <HeaderStyle CssClass="grid-th-right" Height="20px" Width="7%" />
                                                                <ItemStyle CssClass="right" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnPrintMediaEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                        Text=" " OnClick="lbtnPrintMediaEdit_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <HeaderStyle Height="20px" Width="5%" CssClass="grid-th center" VerticalAlign="Top" />
                                                                <ItemStyle CssClass="center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField ShowHeader="false" HeaderText="Select">
                                                                <ItemTemplate>
                                                                    <input type="checkbox" id="chkPMDelete" runat="server" value='<%# Eval("ArchiveBLPMKey") %>' />
                                                                    <asp:HiddenField ID="hfPMFileLocation" runat="server" Value='<%# Eval("FileLocation") %>' />
                                                                    <asp:HiddenField ID="hfArchivePMKey" runat="server" Value='<%# Eval("ArchiveBLPMKey") %>' />
                                                                </ItemTemplate>
                                                                <ItemStyle CssClass="center" />
                                                                <HeaderStyle Width="6%" Height="20px" CssClass="grid-th center" VerticalAlign="Middle" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                    <uc:CustomPager ID="ucCustomPagerPM" runat="server" On_PageIndexChange="ucCustomPagerPM_PageIndexChange"
                                                        PageSize="10" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                    <div id="divTweetResult" runat="server" class="tabInnerDiv">
                                        <asp:UpdatePanel ID="upTweetGrid" runat="server" UpdateMode="Conditional">
                                            <ContentTemplate>
                                                <div id="divTweetResultInner" class="display-none">
                                                    <asp:HiddenField ID="hfTweetStatus" runat="server" Value="0" />
                                                    <div>
                                                        <asp:Label ID="lblSuccessMessageTweet" runat="server" ForeColor="Green"></asp:Label>
                                                        <asp:Label ID="lblErrorMsgTweet" runat="server" ForeColor="Red"></asp:Label>
                                                    </div>
                                                    <div id="divTweetSort" runat="server" visible="false" class="padding5">
                                                        <div class="float-left">
                                                            <span id="spnTweetSort" runat="server">Sort</span>
                                                            <asp:DropDownList ID="ddlTweetSortColumns" runat="server">
                                                                <%--<asp:ListItem Text="User" Value="Actor_DisplayName"></asp:ListItem>
                                                                <asp:ListItem Text="Tweet Body" Value="Tweet_Body"></asp:ListItem>
                                                                <asp:ListItem Text="Tweet Posted Date Time" Value="Tweet_PostedDateTime" Selected="True"></asp:ListItem>
                                                                <asp:ListItem Text="Klout Score" Value="gnip_Klout_Score"></asp:ListItem>--%>
                                                            </asp:DropDownList>
                                                        </div>
                                                        <div class="float-left paddingleft17">
                                                            <asp:RadioButtonList ID="rdoTweetSort" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="0" Selected="True"><span style="vertical-align:middle;padding-left:5px;">Asc</span></asp:ListItem>
                                                                <asp:ListItem Value="1"><span style="vertical-align:middle;padding-left:5px;">Desc</span></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </div>
                                                        <div class="float-left paddingleft17">
                                                            <asp:Button ID="btnSortTweet" runat="server" Text="Sort Data" OnClick="btnSortTweet_Click"
                                                                CssClass="btn-blue2" />
                                                        </div>
                                                    </div>
                                                    <div class="float-right">
                                                        <asp:ImageButton ID="imgbtnRefreshTweet" CssClass="imgicon" runat="server" OnClick="imgbtnRefreshTweet_Click"
                                                            ImageUrl="~/Images/refresh.png" AlternateText="Refresh Library" ToolTip="Refresh Library"
                                                            Visible="false" />
                                                        <asp:ImageButton ID="btnManageCategoriesTweet" Visible="false" CssClass="imgicon"
                                                            runat="server" OnClick="btnManageCategories_Click" ImageUrl="~/Images/setting.png"
                                                            AlternateText="Manage Categories" ToolTip="Manage Categories" />
                                                        <asp:ImageButton ID="lnkEmailTweet" CssClass="imgicon" runat="server" OnClientClick="return ValidateEmailGrid('dlTweet');"
                                                            ImageUrl="~/Images/email.png" AlternateText="Email Selected Article(s)" ToolTip="Email Selected Article(s)"
                                                            Visible="false" />
                                                        <%--  <asp:ImageButton ID="btnTweeterDownload" CssClass="imgicon" runat="server" OnClick="btnPrintMediaDownload_Click"
                                                            ImageUrl="~/Images/download.png" AlternateText="Download Articles" ToolTip="Download Articles"
                                                            Visible="false" />--%>
                                                        <asp:ImageButton ID="btnRemoveTweet" Visible="false" CssClass="imgicon" runat="server"
                                                            OnClientClick="return ValidateGrid('dlTweet');" OnClick="btnRemoveTweet_Click"
                                                            ImageUrl="~/Images/close.png" AlternateText="Remove Selected Tweet(s)" ToolTip="Remove Selected Tweet(s)" />
                                                    </div>
                                                    <div class="clear">
                                                    </div>
                                                    <span id="spnTweetNoData" runat="server" visible="false">No Results Found</span>
                                                    <div class="paddingTop2p">
                                                        <asp:DataList ID="dlTweet" runat="server" Width="100%">
                                                            <ItemTemplate>
                                                                <%--<table style="border-bottom: 1px solid black;" width="100%">
                                                                <tr>
                                                                    <td style="width: 83%; vertical-align: top;">
                                                                        <table>
                                                                            <tr style="font-size: 12px;">
                                                                                <td>
                                                                                    <asp:HiddenField ID="hfTitle" runat="server" Value='<%# Eval("Title") %>' />
                                                                                    <asp:Label ID="lblDisplayName" Font-Bold="true" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actor_DisplayName")%>'></asp:Label><br />
                                                                                </td>
                                                                                <td style="text-align: right; color: #999999;">
                                                                                    Klout Score:
                                                                                    <%# DataBinder.Eval(Container.DataItem, "gnip_Klout_Score")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    Followers:&nbsp;&nbsp;
                                                                                    <%# DataBinder.Eval(Container.DataItem, "Actor_FollowersCount")%>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                    Friends:&nbsp;&nbsp;
                                                                                    <%# DataBinder.Eval(Container.DataItem, "Actor_FriendsCount")%>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="font-size: 15px;">
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="lblTweetBody" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tweet_Body")%>'></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                            <tr style="font-size: 12px; color: #999999; text-align: right;">
                                                                                <td colspan="2">
                                                                                    <asp:Label ID="lblPostedDateTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tweet_PostedDateTime")%>'></asp:Label>&nbsp;&nbsp;
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td style="width: 12%;">
                                                                        <table style="text-align: right; width: 100%;">
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Image ID="imgActor" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Actor_Image")%>' /><br />
                                                                                    <br />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                        <br />
                                                                    </td>
                                                                    <td style="width: 5%; text-align: center;">
                                                                        <input id="chkTweeterSelect" type="checkbox" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "ArchiveTweets_Key")%>' />
                                                                    </td>
                                                                </tr>
                                                            </table>--%>
                                                                <div id="datalistInner" class="clear TweetInnerDiv">
                                                                    <div class="float-left TweetBodyDiv borderBoxSizing">
                                                                        <div class="clear">
                                                                            <div class="float-left TweetActorDisplayName">
                                                                                <asp:HiddenField ID="hfArchiveTweetsKey" runat="server" Value='<%# Eval("ArchiveTweets_Key") %>' />
                                                                                <asp:HiddenField ID="hfTitle" runat="server" Value='<%# Eval("Title") %>' />
                                                                                <a id="aActorLink" runat="server" href='<%#  Eval("Actor_link") %>' target="_blank"
                                                                                    class="float-left">
                                                                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Actor_DisplayName")%>'></asp:Label></a>
                                                                                <div class="float-left TweetSubdivFont">
                                                                                    &nbsp;&nbsp;<%# Eval("Actor_PreferredUserName")%></div>
                                                                                <br />
                                                                            </div>
                                                                            <div class="float-right">
                                                                                <div class="float-left TweetSubdivFont">
                                                                                    Klout Score:&nbsp;
                                                                                    <%# DataBinder.Eval(Container.DataItem, "gnip_Klout_Score")%>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                </div>
                                                                                <div class="float-left TweetSubdivFont">
                                                                                    Followers:&nbsp;
                                                                                    <%# string.Format("{0:n0}", DataBinder.Eval(Container.DataItem, "Actor_FollowersCount"))%>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                                </div>
                                                                                <div class="float-left TweetSubdivFont">
                                                                                    Friends:&nbsp;
                                                                                    <%# string.Format("{0:n0}",DataBinder.Eval(Container.DataItem, "Actor_FriendsCount")) %></div>
                                                                            </div>
                                                                        </div>
                                                                        <div class="clear PaddingTopBottom1p TweetBodyText">
                                                                            <div class="div75pleft">
                                                                                <asp:Label ID="lblTweetBody" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tweet_Body")%>'></asp:Label></div>
                                                                            <div class="TweetSubdivFont float-right">
                                                                                <asp:Label ID="lblPostedDateTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Tweet_PostedDateTime")%>'></asp:Label></div>
                                                                        </div>
                                                                    </div>
                                                                    <div class="float-left TweetImageDiv center">
                                                                        <a id="aActorLinkimage" runat="server" href='<%#  Eval("Actor_link") %>' target="_blank">
                                                                            <asp:Image ID="imgActor" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "Actor_Image")%>' />
                                                                        </a>
                                                                        <div class="center borderBoxSizing">
                                                                            <asp:LinkButton ID="lbtnTweetEdit" runat="server" CausesValidation="False" Text="Edit"
                                                                                OnClick="lbtnTweetEdit_Click"></asp:LinkButton>
                                                                        </div>
                                                                    </div>
                                                                    <div class="float-right Width3p center borderBoxSizing">
                                                                        <input id="chkTweetSelect" type="checkbox" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "ArchiveTweets_Key")%>' />
                                                                    </div>
                                                                </div>
                                                            </ItemTemplate>
                                                        </asp:DataList>
                                                    </div>
                                                    <uc:CustomPager ID="ucCustomePagerTweet" runat="server" On_PageIndexChange="ucCustomePagerTweet_PageIndexChange"
                                                        PageSize="6" />
                                                </div>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<input type="button" id="tgtbtn" runat="server" style="display: none" />
<%--<Ajax:ModalPopupExtender ID="mdlpopupEmail" BackgroundCssClass="ModalBackgroundLightBox"
    BehaviorID="mdlpopupEmail" TargetControlID="tgtbtn" PopupControlID="pnlMailPanel"
    runat="server" CancelControlID="btnCancel">
</Ajax:ModalPopupExtender>--%>
<%--<AjaxToolkit:ResizableControlExtender ID="rceMailPanel" runat="server" TargetControlID="pnlMailPanel"
    HandleCssClass="handleImage" ResizableCssClass="resizing" MinimumWidth="200"
    MinimumHeight="500" MaximumWidth="500" MaximumHeight="1000" HandleOffsetX="1"
    HandleOffsetY="1" />--%>
<asp:Panel ID="pnlMailPanel" runat="server" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;">
    <asp:UpdatePanel ID="upMail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnEmailType" runat="server" Value="Search" />
            <div class="popContainMain" id="pnlEmailInner1">
                <div class="popup-hd">
                    <span id="emailTitleSpan">Share This Video</span>
                    <div style="width: 27px; float: right;" id="div4" runat="server">
                        <img id="lbtnCancelPopUp" onclick="closeModal('pnlMailPanel');" src="../Images/close-icon.png" />
                        <%--<input type="image" id="lbtnCancelPopUp" class="popup-top-close" runat="server" onclick="closeModal('pnlMailPanel');"
                            src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div class="float-right">
                        <input type="button" runat="server" id="lbtnCancelPopUp" class="btn-cancel" value=" "
                            onclick="closeModal('pnlMailPanel');" />
                    </div>--%>
                </div>
                <div id="pnlEmailInner" style="background-color: #F8FCFF;" class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="ValidationSummary1" EnableClientScript="true" runat="server"
                            ValidationGroup="validate1" ForeColor="#bd0000" Font-Size="Smaller" />
                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>Your Email Address :</li>
                        <li>
                            <asp:TextBox ID="txtYourEmail" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regEmailGrid" runat="server" Display="None" ValidationGroup="validate1"
                                ControlToValidate="txtYourEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ErrorMessage="Please Enter Valid Email Address" Text=""></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvEmail" Text="" ValidationGroup="validate1" Display="None"
                                runat="server" ControlToValidate="txtYourEmail" ErrorMessage="Please Enter Your Email Address."></asp:RequiredFieldValidator>
                        </li>
                        <li>Friends's Email Address (separate multiple addresses with semicolon) :</li>
                        <li>
                            <asp:TextBox ID="txtFriendsEmail" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="" ValidationGroup="validate1"
                                Display="None" runat="server" ControlToValidate="txtFriendsEmail" ErrorMessage="Please Enter Friend's Email Address."></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                                ValidationGroup="validate1" ControlToValidate="txtFriendsEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                                ErrorMessage="Please Enter Valid Friend's Email Address" Text=""></asp:RegularExpressionValidator>
                        </li>
                        <li>Subject :</li>
                        <li>
                            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Text="" ValidationGroup="validate1"
                                Display="None" runat="server" ControlToValidate="txtSubject" ErrorMessage="Please Enter Subject."></asp:RequiredFieldValidator>
                        </li>
                        <li>Message :</li>
                        <li>
                            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
                        </li>
                    </ul>
                    <div class="center">
                        <%--<input type="button" id="btnCancel" value="Cancel" class="btn-blue2" onclick="$find('mdlpopupEmail').hide();" />--%>
                        <input type="button" id="btnCancel" value="Cancel" class="btn-blue2" onclick="closeModal('pnlMailPanel');" />
                        <asp:Button ID="btnOK" CssClass="btn-blue2" runat="server" Width="51px" Text="OK"
                            ValidationGroup="validate1" OnClick="btnOK_Click"></asp:Button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lnkEmail" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>
<input type="button" id="Button1" runat="server" style="display: none" />
<%--<Ajax:ModalPopupExtender ID="mdlpopupEmail" BackgroundCssClass="ModalBackgroundLightBox"
    BehaviorID="mdlpopupEmail" TargetControlID="tgtbtn" PopupControlID="pnlMailPanel"
    runat="server" CancelControlID="btnCancel">
</Ajax:ModalPopupExtender>--%>
<%--<AjaxToolkit:ResizableControlExtender ID="rceMailPanel" runat="server" TargetControlID="pnlMailPanel"
    HandleCssClass="handleImage" ResizableCssClass="resizing" MinimumWidth="200"
    MinimumHeight="500" MaximumWidth="500" MaximumHeight="1000" HandleOffsetX="1"
    HandleOffsetY="1" />--%>
<input type="button" id="tgtEditClip" runat="server" style="display: none;" />
<%--<Ajax:ModalPopupExtender ID="mdlpopupClip" BackgroundCssClass="ModalBackgroundLightBox"
    BehaviorID="mdlpopupClip" TargetControlID="tgtEditClip" PopupControlID="pnlClipPanel"
    runat="server">
</Ajax:ModalPopupExtender>--%>
<asp:Panel ID="pnlClipPanel" runat="server" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;" DefaultButton="btnClipUpdate">
    <asp:UpdatePanel ID="upEditClip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span>Edit Clip Details</span>
                    <div style="width: 27px; float: right;" id="div5" runat="server">
                        <img id="Img1" src="../Images/close-icon.png" onclick="closeModal('pnlClipPanel');" />
                        <%--<input type="image" id="Image2" class="popup-top-close" runat="server" onclick="closeModal('pnlClipPanel');"
                            src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div style="float: right; padding-right: 10px;">
                        <input type="button" runat="server" id="btnClipClose" class="btn-cancel" value=" "
                            onclick="closeModal('pnlClipPanel');" />
                    </div>--%>
                </div>
                <div style="background-color: #F8FCFF;" class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="vlSummeryEditClip" runat="server" ValidationGroup="vgEditClip"
                            ForeColor="#bd0000" Font-Size="Smaller" />
                        <asp:Label ID="lblClipMsg" runat="server" Visible="true" ForeColor="Red"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>
                            <label>
                                Clip Title :</label>
                            <asp:HiddenField ID="hdnArchiveClipKey" runat="server" />
                            <asp:HiddenField ID="hdnEditClipID" runat="server" />
                            <asp:TextBox ID="txtEditClipTitle" runat="server" Width="74%" MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtClipTitle" Text="*" runat="server" ControlToValidate="txtEditClipTitle"
                                ErrorMessage="Title is required." Display="None" ValidationGroup="vgEditClip"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Owner :</label>
                            <asp:DropDownList ID="ddlOwner" runat="server" Width="36%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqOwner" runat="server" ControlToValidate="ddlOwner"
                                ErrorMessage="Please Select Owner" Text="*" ValidationGroup="vgEditClip" Display="None"
                                InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Category 1 :</label>
                            <asp:DropDownList ID="ddlPCategory" runat="server" Width="36%" onclick="selectList_cache=this.value"
                                onchange="return UpdateSubCategory(this.id,0);">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="reqCategory" runat="server" ControlToValidate="ddlPCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgEditClip" Display="Dynamic"
                                InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Category 2 :</label>
                            <asp:DropDownList ID="ddlSubCategory1" runat="server" Width="36%" onchange="UpdateSubCategory(this.id,0);">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Category 3 :</label>
                            <asp:DropDownList ID="ddlSubCategory2" runat="server" Width="36%" onchange="UpdateSubCategory(this.id,0);">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Category 4 :</label>
                            <asp:DropDownList ID="ddlSubCategory3" runat="server" Width="36%" onchange="UpdateSubCategory(this.id,0);">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Description :</label>
                            <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" Style="width: 74%;
                                height: 145px; overflow: auto;">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtDescription" Text="*" runat="server" ControlToValidate="txtDescription"
                                ErrorMessage="Description is required." Display="None" ValidationGroup="vgEditClip"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Keywords :</label>
                            <asp:TextBox ID="txtKeywords" runat="server" TextMode="MultiLine" Style="width: 74%;
                                height: 145px; overflow: auto;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtKeywords" Text="*" runat="server" ControlToValidate="txtKeywords"
                                ErrorMessage="Keywords is required." Display="None" ValidationGroup="vgEditClip"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Rate This Clip :</label>
                            <div>
                                <div style="width: 74%; overflow: auto;">
                                    <span class="float-left RateArticleNumberLeft">1</span> <span class="float-left">
                                        <asp:TextBox ID="txtClipRate" runat="server" Style="display: none;" onchange="sliderChange('txtClipRate','chkClipPreferred');">
                                        </asp:TextBox>
                                        <Ajax:SliderExtender ID="seClipRate" runat="server" TargetControlID="txtClipRate"
                                            Minimum="1" Maximum="6" Steps="6" TooltipText="{0}" RaiseChangeOnlyOnMouseUp="true"
                                            BehaviorID="seClipRate">
                                        </Ajax:SliderExtender>
                                    </span><span class="float-left RateArticleNumberRight">6</span> <span class="float-left prefferredcheckbox">
                                        <asp:CheckBox CssClass="float-left" ID="chkClipPreferred" runat="server" Text="Preferred"
                                            onclick="PrefferredChecked('chkClipPreferred','seClipRate');" Style="float: none;" /></span>
                                </div>
                                <%-- <div>
                                    <asp:CheckBox ID="" runat="server" Text="Preferred" Style="margin-left: -110px;
                                        margin-top: 3px;" onclick="PrefferredChecked('chkClipPreferred','seClipRate');" />
                                </div>--%>
                            </div>
                        </li>
                    </ul>
                    <div style="text-align: center;">
                        <input type="button" id="Button4" class="btn-blue2" value="Cancel" onclick="closeModal('pnlClipPanel');" />
                        <asp:Button ID="btnClipUpdate" runat="server" Text="Save" ValidationGroup="vgEditClip"
                            CausesValidation="true" CssClass="btn-blue2" OnClick="btnClipUpdate_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<!-- Custom Category ModapPopUp -->
<input type="button" id="ccButton1" runat="server" style="display: none" />
<input type="button" id="tgtbtn1" runat="server" style="display: none" />
<%--<Ajax:ModalPopupExtender ID="mpCustomCategory" BackgroundCssClass="ModalBackgroundLightBox"
    BehaviorID="mpCustomCategory" TargetControlID="tgtbtn1" PopupControlID="pnlCancelCustomCategory"
    runat="server">
</Ajax:ModalPopupExtender>--%>
<asp:Panel ID="pnlCustomCategory" runat="server" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;">
    <asp:UpdatePanel ID="upCustomCategory" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd" style="padding-left: 10px;">
                    <span>Custom Category Management</span>
                    <div style="width: 27px; float: right;" id="div6" runat="server">
                        <img id="btnCancelCategoryPopup" src="../Images/close-icon.png" onclick="closeModal('pnlCustomCategory');"
                            class="customCategoryImage" />
                        <%--<input type="image" id="btnCancelCategoryPopup" class="popup-top-close" runat="server"
                            onclick="closeModal('pnlCustomCategory');" src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div style="float: right; padding-right: 10px;">
                        
                        <input type="button" runat="server" id="btnCancelCategoryPopup" class="btn-cancel"
                            value=" " onclick="closeModal('pnlCustomCategory');" />
                    </div>--%>
                </div>
                <div style="background-color: #F8FCFF;" class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="ValidationSummary3" EnableClientScript="true" runat="server"
                            ValidationGroup="CustomCategory" ForeColor="#bd0000" />
                        <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>
                            <label>
                                Category Name :</label>
                            <asp:TextBox ID="txtCategoryName" Width="70%" runat="server" />
                            <asp:RequiredFieldValidator ID="rfvCategoryName" ControlToValidate="txtCategoryName"
                                ErrorMessage="Category Name is Required" ValidationGroup="CustomCategory" Text="*"
                                Display="Dynamic" ToolTip="Category Name is Required" runat="server" />
                        </li>
                        <li>
                            <label>
                                Category Description :</label>
                            <asp:TextBox ID="txtCategoryDescription" Width="70%" TextMode="MultiLine" Rows="3"
                                runat="server" />
                        </li>
                        <li>
                            <label>
                                &nbsp;</label>
                            <input type="button" id="btnCancelCustomCategory" value="Cancel" class="btn-blue2"
                                onclick="closeModal('pnlCustomCategory');" />
                            <asp:Button ID="btnSaveCustomCategory" CssClass="btn-blue2" runat="server" Width="51px"
                                Text="Add" ValidationGroup="CustomCategory" OnClick="btnSaveCustomCategory_Click">
                            </asp:Button>
                        </li>
                        <li>
                            <div style="max-height: 420px; overflow: auto; border-width: 1px; border-color: #E4E4E4;
                                border-style: solid;">
                                <asp:GridView ID="gvCustomCategory" runat="server" Width="100%" border="1" AllowPaging="True"
                                    CssClass="grid grid-iq" PageSize="8" CellPadding="5" CellSpacing="0" AutoGenerateEditButton="False"
                                    BorderColor="#E4E4E4" Style="border-collapse: collapse;" PagerSettings-Mode="NextPrevious"
                                    AutoGenerateColumns="False" EmptyDataText="No Data Found" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                    PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="gvCustomCategory_PageIndexChanging"
                                    OnRowCancelingEdit="gvCustomCategory_RowCancelingEdit" OnRowEditing="gvCustomCategory_RowEditing"
                                    OnRowDataBound="gvCustomCategory_RowDataBound" OnRowUpdating="gvCustomCategory_RowUpdating"
                                    OnRowCommand="gvCustomCategory_RowCommand" DataKeyNames="CategoryGUID" PagerStyle-Width="100">
                                    <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="btn-delete" CausesValidation="false"
                                                    CommandName="DeleteRecord" Text=" " CommandArgument='<%# Eval("CategoryKey") %>'
                                                    OnClientClick="javascript:return confirm('Are you sure to delete record ?')" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CssClass="btn-delete" CausesValidation="false"
                                                    CommandName="DeleteRecord" Text=" " CommandArgument='<%# Eval("CategoryKey") %>'
                                                    OnClientClick="javascript:return false;" />
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="grid-th" Height="20px" />
                                            <ItemStyle CssClass="content-text-new" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="False" HeaderStyle-Width="7%">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                    CommandName="Edit" Text=" "></asp:LinkButton>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:LinkButton ID="lbtnUpdate" runat="server" ValidationGroup="CustomCategoryGrid"
                                                    CssClass="btn-update" CausesValidation="True" CommandName="Update" Text=" "></asp:LinkButton>
                                                &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" CssClass="btn-cancel" CausesValidation="False"
                                                    CommandName="Cancel" Text=" "></asp:LinkButton>
                                                <asp:HiddenField ID="hfCustomCategoryKey" Value='<%# Eval("CategoryKey") %>' runat="server" />
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="grid-th" Height="20px" />
                                            <ItemStyle CssClass="content-text-new" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Name" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryName" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCategoryName" Width="80%" runat="server" Text='<%# Bind("CategoryName") %>' />
                                                <asp:RequiredFieldValidator ID="rfvCategoryName" ControlToValidate="txtCategoryName"
                                                    ValidationGroup="CustomCategoryGrid" ErrorMessage="Category Name is required"
                                                    runat="server" />
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="grid-th" Font-Bold="true" HorizontalAlign="Left" Height="20px"
                                                Width="30%" />
                                            <ItemStyle HorizontalAlign="Left" Width="140px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategoryDescription" runat="server" Text='<%# Eval("CategoryDescription") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txtCategoryDescription" Width="80%" runat="server" TextMode="MultiLine"
                                                    Text='<%# Bind("CategoryDescription") %>' Rows="3" />
                                            </EditItemTemplate>
                                            <HeaderStyle CssClass="grid-th" Font-Bold="true" Height="20px" />
                                            <ItemStyle HorizontalAlign="Left" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grid-th" Height="20px" />
                                </asp:GridView>
                            </div>
                        </li>
                    </ul>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<input type="button" id="tgtbtnPlayer" runat="server" value="Show player" style="display: none" />
<%--<Ajax:ModalPopupExtender ID="mpeClipPlayer" BackgroundCssClass="ModalBackgroundLightBox"
    PopupControlID="divClipPlayer" TargetControlID="tgtbtnPlayer" BehaviorID="mpeClipPlayer"
    PopupDragHandleControlID="divClose" runat="server">
</Ajax:ModalPopupExtender>--%>
<asp:Panel ID="divClipPlayer" BorderColor="#6f6f6f" BackColor="White" BorderWidth="10px"
    runat="server" Style="display: none;" CssClass="modal hide fade resizable playerPopUp">
    <asp:UpdatePanel ID="upClipPlayer" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 27px; float: right;" id="divClose" runat="server">
                <img id="lbtnCancelPlayer" class="popup-top-close" src="../Images/close-icon.png"
                    onclick="ClosePlayer();" />
                <%--onclick="ClosePlayer();" />--%>
                <%--<input type="image" id="lbtnCancelPlayer" class="popup-top-close" runat="server"
                    onclick="ClosePlayer();" src="~/Images/close-icon.png" />--%>
            </div>
            <div id="divIFrameMain" style="width: 885px; padding: 15px 15px 15px 0; height: 340px;">
                <div id="DivCaption" runat="server" class="Caption" style="margin-left: 15px;">
                </div>
                <div id="divShowCaption" style="float: left;">
                    <img src="../../../Images/right_arrow_cc.gif" id="imgCCDirection" alt="Show CC" />
                </div>
                <div style="width: 545px; height: 340px; float: right; background-color: Black;"
                    id="divRawMedia" runat="server">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<input type="button" id="tgtEditArticle" runat="server" style="display: none;" />
<%--<Ajax:ModalPopupExtender ID="mdlpopupArticle" BackgroundCssClass="ModalBackgroundLightBox"
    BehaviorID="mdlpopupArticle" TargetControlID="tgtEditArticle" PopupControlID="pnlEditArticle"
    runat="server">
</Ajax:ModalPopupExtender>--%>
<asp:Panel ID="pnlEditArticle" runat="server" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;" DefaultButton="btnArticleUpdate">
    <asp:UpdatePanel ID="upEditArticle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd" style="padding-left: 10px;">
                    <asp:Label ID="spnEditArtile"  runat="server" Text="Edit Article Details" ></asp:Label>
                    <div style="width: 27px; float: right;" id="div7" runat="server">
                        <img id="imgEditArticleClos" src="../Images/close-icon.png" onclick="closeModal('pnlEditArticle');" />
                        <%--<input type="image" id="imgEditArticleClos" class="popup-top-close" runat="server"
                            onclick="closeModal('pnlEditArticle');" src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div style="float: right; padding-right: 10px;">
                        <input type="button" id="Button2" class="btn-cancel" value=" " onclick="closeModal('pnlEditArticle');" />
                    </div>--%>
                </div>
                <div style="background-color: #F8FCFF;" class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="vlSummeryEditArticle" runat="server" ValidationGroup="vgEditArticle"
                            ForeColor="#bd0000" Font-Size="Smaller" />
                        <asp:Label ID="lblArticlepMsg" runat="server" Visible="true" ForeColor="Red"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li id="liTitle">
                            <label>
                                Title :</label>
                            <asp:HiddenField ID="hdnEditArchiveKey" runat="server" />
                            <asp:HiddenField ID="hdnArticleType" runat="server" />
                            <asp:TextBox ID="txtArticleTitle" runat="server" Width="74%" MaxLength="255"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtArticleTitle" Text="*" runat="server" ControlToValidate="txtArticleTitle"
                                ErrorMessage="Title is required." Display="None" ValidationGroup="vgEditArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Category 1 :</label>
                            <asp:DropDownList ID="ddlPArticleCategory" runat="server" Width="36%" onclick="selectList_cache=this.value"
                                onchange="return UpdateSubCategory(this.id,1);">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="rfvddlPArticleCategory" runat="server" ControlToValidate="ddlPArticleCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgEditArticle"
                                Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Category 2 :</label>
                            <asp:DropDownList ID="ddlArticleSubCategory1" runat="server" Width="36%" onchange="UpdateSubCategory(this.id,1);">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Category 3 :</label>
                            <asp:DropDownList ID="ddlArticleSubCategory2" runat="server" Width="36%" onchange="UpdateSubCategory(this.id,1);">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Category 4 :</label>
                            <asp:DropDownList ID="ddlArticleSubCategory3" runat="server" Width="36%" onchange="UpdateSubCategory(this.id,1);">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Description :</label>
                            <asp:TextBox ID="txtArticleDescription" runat="server" TextMode="MultiLine" Style="width: 74%;
                                height: 145px; overflow: auto;">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtArticleDescription" Text="*" runat="server"
                                ControlToValidate="txtArticleDescription" ErrorMessage="Description is required."
                                Display="None" ValidationGroup="vgEditArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Keywords :</label>
                            <asp:TextBox ID="txtArticleKeywords" runat="server" TextMode="MultiLine" Style="width: 74%;
                                height: 145px; overflow: auto;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtArticleKeywords" Text="*" runat="server" ControlToValidate="txtArticleKeywords"
                                ErrorMessage="Keywords is required." Display="None" ValidationGroup="vgEditArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label id="lblrateartile" runat="server">
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
                    <div style="text-align: center;">
                        <input type="button" id="Button5" class="btn-blue2" value="Cancel" onclick="closeModal('pnlEditArticle');" />
                        <asp:Button ID="btnArticleUpdate" runat="server" Text="Save" ValidationGroup="vgEditArticle"
                            CausesValidation="true" CssClass="btn-blue2" OnClick="btnArticleUpdate_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<div id="diviFrameOnlineNewsArticle" runat="server" class="modal hide fade resizable modalPopupDiv iframePopup"
    style="display: none;">
    <div style="width: 27px; float: right;" id="divCloseNews">
        <div>
            <img id="img3" class="popup-top-close" src="../Images/close-icon.png" onclick="closeModal('diviFrameOnlineNewsArticle');removeSourceFromiFrame('iFrameOnlineNewsArticle');" />
            <%--<input type="image" id="Image1" class="popup-top-close" runat="server" onclick="closeModal('diviFrameOnlineNewsArticle');"
                src="~/Images/close-icon.png" />--%>
        </div>
    </div>
    <asp:UpdatePanel ID="upOnlineNewsArticle" runat="server" UpdateMode="Conditional"
        style="width: 100%; height: 100%;">
        <ContentTemplate>
            <iframe id="iFrameOnlineNewsArticle" style="width: 100%; height: 100%;" scrolling="auto"
                frameborder="0" runat="server" src=""></iframe>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="width: 100%; text-align: right; position: absolute; margin: -50px 0 0 -25px;">
        <asp:Button ID="btnPrint" Text="Print Article" CssClass="btn-green" runat="server"
            OnClientClick="javascript:PrintIframe();return false;" />
    </div>
</div>
<input type="button" id="tgtbtnArticle" runat="server" style="display: none;" />
<%--<AjaxToolkit:ModalPopupExtender ID="mpeArticlePopup" runat="server" CancelControlID="divCloseNews"
    BackgroundCssClass="ModalBackgroundLightBox" Enabled="True" PopupControlID="diviFrameOnlineNewsArticle"
    OnCancelScript="CloseNewsIframe();" TargetControlID="tgtbtnArticle" BehaviorID="mpeArticlePopup">
</AjaxToolkit:ModalPopupExtender>--%>