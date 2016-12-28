<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAgent.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQAgent.IQAgent" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Src="../CustomPager/CustomPager.ascx" TagName="CustomPager" TagPrefix="uc" %>
<%@ Register Src="../IframeRawMediaH/IframeRawMediaH.ascx" TagName="IframeRawMediaH"
    TagPrefix="uc1" %>
<div class="clear">
    <div class="span3">
        <div id="divFilterSide" class="well sidebar-nav">
            <asp:UpdatePanel ID="upRawMediaClip" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div id="divMainSearch">
                        <ul id="search-list" class="nav nav-list listBorder filterul marginbottom5">
                            <li class="ulhead">
                                <div class="float-left">
                                    Search Media</div>
                                <%--<asp:TextBox ID="TextBox1" runat="server" Style="display: none;" onchange="sliderChange('txtArticleRate','chkArticlePreferred')">
                                </asp:TextBox>
                                <Ajax:SliderExtender ID="SliderExtender1" runat="server" TargetControlID="TextBox1"
                                    Minimum="1" Maximum="6" Steps="6">
                                </Ajax:SliderExtender>--%>
                            </li>
                            <li>
                                <div class="bs-docs-example display-block">
                                    <asp:ValidationSummary ID="vsIQMediaSearch" EnableClientScript="true" runat="server"
                                        ValidationGroup="IQMediaSearch" ForeColor="#bd0000" Font-Size="Smaller" />
                                    <asp:Label ID="lblRawMediaMsg" runat="server" ForeColor="#bd0000"></asp:Label>
                                    Query Name :
                                    <asp:DropDownList runat="server" ID="drpQueryName" CssClass="MarketDropdown" AutoPostBack="true"
                                        OnSelectedIndexChanged="drpQueryName_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    Search Term :
                                    <asp:TextBox ID="txtSearchMediaText" runat="server" CssClass="programTextbox" Enabled="false"></asp:TextBox>
                                    <cc1:PropertyProxyValidator ID="pplSearchMyclip" runat="server" ControlToValidate="txtSearchMediaText"
                                        Text="*" PropertyName="SearchText" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                        RulesetName="RawMediaDate" ValidationGroup="IQMediaSearch" Font-Size="Smaller"
                                        Display="Dynamic" DisplayMode="List"></cc1:PropertyProxyValidator>
                                    <div class="clear paddingtop5">
                                        <asp:LinkButton ID="btnRefresh" CssClass="float-right right RateArticleNumberLeft"
                                            ToolTip="Click to reload dropdown values" Text="Refresh" ForeColor="#2f4f7f"
                                            Style="text-decoration: none;" runat="server" OnClick="btnRefresh_Click" />
                                    </div>
                                </div>
                            </li>
                        </ul>
                        <asp:Panel ID="pnlNotification" runat="server" Visible="false">
                            <ul id="ulTwitter" class="nav nav-list listBorder filterul">
                                <li class="ulhead">
                                    <div class="float-left">
                                        User Notification Settings</div>
                                </li>
                                <li>
                                    <div class="bs-docs-example display-block">
                                        <div class="clear">
                                            <asp:ValidationSummary ID="ValidationSummary1" EnableClientScript="true" runat="server"
                                                ValidationGroup="validate" ForeColor="#bd0000" Font-Size="Smaller" />
                                            <asp:Label ID="lblNotificationError" runat="server" ForeColor="red"></asp:Label>
                                            <asp:ValidationSummary ID="ValidationSummary2" EnableClientScript="true" runat="server"
                                                ValidationGroup="validate1" ForeColor="#bd0000" Font-Size="Medium" />
                                            <asp:Label ID="lblSuccessDelete" runat="server" ForeColor="Green"></asp:Label>
                                        </div>
                                        <div class="float-right">
                                            <asp:Button ID="btnRemoveNotification" runat="server" Text="Remove Notification"
                                                OnClientClick="return ValidateNotificationGrid();" CssClass="btn-blue2" Style="text-align: right"
                                                OnClick="btnRemoveNotification_Click" />
                                        </div>
                                        <div class="clear">
                                            <br />
                                        </div>
                                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
                                        <asp:GridView ID="gvIQNotification" runat="server" Width="100%" border="0" AllowPaging="True"
                                            PageSize="15" CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                            BorderColor="#E4E4E4" Style="border-collapse: collapse;" PagerSettings-Mode="NextPrevious"
                                            AutoGenerateColumns="False" EmptyDataText="No Notifications" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="gvIQNotification_PageIndexChanging"
                                            OnRowCancelingEdit="gvIQNotification_RowCancelingEdit" OnRowEditing="gvIQNotification_RowEditing"
                                            OnRowUpdating="gvIQNotification_RowUpdating" CssClass="grid" BackColor="#FFFFFF">
                                            <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                            CommandName="Edit" Text=" "></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbtnUpdate" runat="server" CssClass="btn-update" CausesValidation="True"
                                                            CommandName="Update" Text=" " ValidationGroup="validate1"></asp:LinkButton>
                                                        &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" CssClass="btn-cancel" CausesValidation="False"
                                                            CommandName="Cancel" Text=" "></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="10%" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Notification_Address") %>'
                                                            ItemStyle-CssClass="content-text"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="52%" />
                                                    <ItemStyle CssClass="content-text" Width="35%"></ItemStyle>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtEmail" runat="server" Text='<%# Bind("Notification_Address") %>'
                                                            Width="91%" CssClass="grayinput"></asp:TextBox>
                                                        <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtEmailGrid" runat="server" TargetControlID="txtEmail"
                                                            FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                            InvalidChars="<>">
                                                        </AjaxToolkit:FilteredTextBoxExtender>
                                                        <asp:RegularExpressionValidator ID="regEmailGrid" runat="server" Display="None" ValidationGroup="validate1"
                                                            ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ErrorMessage="Please Enter Valid Email Address" Text=""></asp:RegularExpressionValidator>
                                                        <asp:RequiredFieldValidator ID="rfvEmail" Text="" ValidationGroup="validate1" Display="None"
                                                            runat="server" ControlToValidate="txtEmail" ErrorMessage="Please Enter Email Address."></asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Frequency" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFrequency" runat="server" Text='<%# Bind("Frequency") %>' ItemStyle-CssClass="content-text"></asp:Label>
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" Width="24%" />
                                                    <ItemStyle CssClass="content-text" Width="35%"></ItemStyle>
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="hdnIQMotificationKey" runat="server" Value='<%# Bind("IQNotificationKey") %>' />
                                                        <asp:DropDownList ID="drpOption" runat="server" OnPreRender="drpOption_PreRender"
                                                            CssClass="grayselect" Width="120%">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="false" HeaderText="Delete" HeaderStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <input type="checkbox" id="chkDelete" runat="server" value='<%# Eval("IQNotificationKey") %>' />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle Width="14%" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="grid-th" />
                                        </asp:GridView>
                                        <div class="clear">
                                            <br />
                                            Email :
                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="programTextbox"></asp:TextBox>
                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtEmail" runat="server" TargetControlID="txtEmail"
                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                InvalidChars="<>">
                                            </AjaxToolkit:FilteredTextBoxExtender>
                                            <asp:RegularExpressionValidator ID="regEmail" Text="*" runat="server" Display="Dynamic"
                                                ValidationGroup="validate" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ErrorMessage="Please Enter Valid Email Address"></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvEmail" Text="*" ValidationGroup="validate" Display="Dynamic"
                                                runat="server" ControlToValidate="txtEmail" ErrorMessage="Please Enter Email Address."></asp:RequiredFieldValidator>
                                            <div class="clear paddingtop15">
                                                Frequency :</div>
                                            <asp:DropDownList ID="drpOption" runat="server" CssClass="MarketDropdown">
                                            </asp:DropDownList>
                                        </div>
                                        <div class="clear center">
                                            <asp:Button ID="btnSave" runat="server" ValidationGroup="validate" CssClass="btn-blue2"
                                                Text="Save" OnClick="btnSave_Click" /></div>
                                    </div>
                                </li>
                            </ul>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="drpQueryName" EventName="SelectedIndexChanged" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="span9">
        <div class="navbar">
            <div onclick="ShowHideDivResult('divResult',true);" class="show-hide cursor text-align-center">
                <div class="float-left">
                    <a href="javascript:;">RESULTS</a></div>
                <div class="float-right">
                    <a class="right-dropdown" href="javascript:;">
                        <div id="divResultShowHideTitle" class="float-left">
                            SHOW</div>
                        <img alt="" src="../images/show.png" class="imgshowHide" id="imgShowHideResult" /></a></div>
            </div>
            <div class="display-none" id="divResult">
                <asp:HiddenField ID="hfcurrentTab" runat="server" />
                <div id="tabsTV" class="tabMain">
                    <div id="divGridTab" class="clear tabdiv">
                        <div id="tabTV" class="active float-left display-none" onclick="ChangeTab('tabTV','divGridTab',0,1);">
                            TV
                        </div>
                        <div id="tabOnlineNews" class="float-left display-none" onclick="ChangeTab('tabOnlineNews','divGridTab',1,1);">
                            Online News
                        </div>
                        <div id="tabSocialMedia" class="float-left display-none" onclick="ChangeTab('tabSocialMedia','divGridTab',2,1);">
                            Social Media
                        </div>
                        <div id="tabTwitter" class="float-left display-none" onclick="ChangeTab('tabTwitter','divGridTab',3,1);">
                            Twitter
                        </div>
                    </div>
                    <div class="tabContentDiv">
                        <div id="divTVResult" class="display-none">
                            <asp:UpdatePanel ID="upTVGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <asp:HiddenField ID="hfTVStatus" runat="server" Value="0" />
                                    <div id="divTVResultInner" class="display-none">
                                        <div class="clear paddingtop5">
                                            <div class="float-left">
                                                <span id="spnTotalProgramHeader" runat="server" visible="false" class="float-left">Total
                                                    Programs Found :&nbsp; </span>
                                                <asp:Label ID="lblNoOfRadioRawMedia" runat="server" Visible="false" CssClass="float-right"></asp:Label>
                                            </div>
                                            <div class="float-right">
                                                <asp:Button ID="btnRemove" Text="Remove Selected Results" CssClass="btn-blue2" runat="server"
                                                    OnClick="btnRemove_Click" OnClientClick="return ValidateGrid('divTVRpt');" Visible="false" />
                                            </div>
                                        </div>
                                        <div class="clear paddingtop5">
                                        </div>
                                        <div id="divTVRpt" class="grid-iq subdivheight">
                                            <asp:Label ID="lblSuccessMessage" runat="server" Font-Size="Medium" ForeColor="Green"></asp:Label>
                                            <asp:Repeater ID="rptTV" runat="server" OnItemCommand="rptTV_ItemCommand" OnItemDataBound="rptTV_ItemDataBound">
                                                <HeaderTemplate>
                                                    <div class="clear hederdiv grid-th-left">
                                                        <div style="width: 3%" class="center">
                                                            &nbsp;</div>
                                                        <div style="width: 5%" class="center">
                                                            <span>Play<span>
                                                        </div>
                                                        <div style="width: 33%" class="left">
                                                            <span>Program</span></div>
                                                        <div style="width: 18%" class="right">
                                                            <asp:LinkButton ID="DateTime" runat="server" Style="text-align: right" Text="Date Time"
                                                                CommandName="sort" CommandArgument="IQ_Local_Air_DateTime"></asp:LinkButton>
                                                        </div>
                                                        <div style="width: 7%;" class="center">
                                                            &nbsp;</div>
                                                        <div style="width: 20%;">
                                                            <asp:LinkButton ID="lnkMarket" runat="server" Text="Market" CommandName="sort" CommandArgument="StationMarket">
                                                            </asp:LinkButton>
                                                        </div>
                                                        <div style="width: 9%" class="right">
                                                            <span>Mentions</span></div>
                                                        <div style="width: 5%">
                                                            <span></span>
                                                        </div>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div class="clear grid">
                                                        <div style="width: 3%" class="center">
                                                            <img runat="server" id="ExpandImg" style="cursor: pointer; width: 16px;" src="~/images/expand_icon.png"
                                                                alt="" />
                                                        </div>
                                                        <div style="width: 5%" class="center">
                                                            <asp:ImageButton ID="imgPlayButton" runat="server" ImageUrl="~/Images/btn-play.jpg"
                                                                CommandArgument='<%# Eval("RL_VideoGuid") %>' OnCommand="LbtnRawMediaPlay_Command" />
                                                        </div>
                                                        <div style="width: 33%" class="left">
                                                            <span>
                                                                <%# Eval("Title120")%></span>
                                                        </div>
                                                        <div style="width: 18%" class="right">
                                                            <span>
                                                                <%# Convert.ToDateTime(Eval("IQ_Local_Air_DateTime")).ToString("MM/dd/yyyy")%>
                                                                <%# Convert.ToDateTime(Eval("IQ_Local_Air_DateTime")).ToString("hh:mm tt")%></span>
                                                        </div>
                                                        <div style="width: 7%;" class="center">
                                                            <span>
                                                                <asp:Image ID="imgStationLogo" AlternateText=" " ImageUrl='<%# Eval("StationLogo") %>'
                                                                    runat="server" /></span>
                                                        </div>
                                                        <div style="width: 20%;" class="left">
                                                            <span>
                                                                <%# Eval("StationMarket")%></span>
                                                        </div>
                                                        <div style="width: 9%" class="right">
                                                            <span>
                                                                <%# Eval("Number_Hits")%></span>
                                                        </div>
                                                        <div style="width: 5%" class="center">
                                                            <input id="ChkDelete" type="checkbox" value='<%# Eval("ID") %>' runat="server" />
                                                        </div>
                                                    </div>
                                                    <div class="clear">
                                                        <asp:Panel ID="pnlGridChildIQAgentResults" runat="server" Style="display: none">
                                                            <asp:Repeater ID="rptChildTV" runat="server">
                                                                <ItemTemplate>
                                                                    <div class="clear grid">
                                                                        <div style="width: 3%" class="center">
                                                                            &nbsp;</div>
                                                                        <div style="width: 5%" class="center">
                                                                            <asp:ImageButton ID="imgPlayButton" runat="server" ImageUrl="~/Images/btn-play.jpg"
                                                                                CommandArgument='<%# Eval("RL_VideoGuid") %>' OnCommand="LbtnRawMediaPlay_Command" />
                                                                        </div>
                                                                        <div style="width: 33%" class="left">
                                                                            &nbsp;
                                                                        </div>
                                                                        <div style="width: 18%" class="right">
                                                                            <span>
                                                                                <%# Convert.ToDateTime(Eval("IQ_Local_Air_DateTime")).ToString("MM/dd/yyyy")%>
                                                                                <%# Convert.ToDateTime(Eval("IQ_Local_Air_DateTime")).ToString("hh:mm tt")%></span>
                                                                        </div>
                                                                        <div style="width: 7%;" class="center">
                                                                            <span>
                                                                                <asp:Image ID="imgStationLogo" AlternateText=" " ImageUrl='<%# Eval("StationLogo") %>'
                                                                                    runat="server" /></span>
                                                                        </div>
                                                                        <div style="width: 20%;" class="left">
                                                                            <span>
                                                                                <%# Eval("StationMarket")%></span>
                                                                        </div>
                                                                        <div style="width: 9%" class="right">
                                                                            <span>
                                                                                <%# Eval("Number_Hits")%></span>
                                                                        </div>
                                                                        <div style="width: 5%" class="center">
                                                                            <input id="ChkDelete" type="checkbox" value='<%# Eval("ID") %>' runat="server" />
                                                                        </div>
                                                                    </div>
                                                                </ItemTemplate>
                                                            </asp:Repeater>
                                                        </asp:Panel>
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
                                        <div class="clear paddingtop5">
                                            <div class="float-left">
                                                <span id="spnNewsChartProgramFound" runat="server" visible="false" class="float-left">
                                                    Total Articles Found :&nbsp; </span>
                                                <asp:Label ID="lblNewsChart" runat="server" Visible="false" CssClass="float-right"></asp:Label></div>
                                            <div class="float-right">
                                                <asp:Button ID="btnRemoveNM" Text="Remove Selected Results" CssClass="btn-blue2"
                                                    runat="server" OnClick="btnRemoveNM_Click" OnClientClick="return ValidateGrid('_gvOnlineNews');"
                                                    Visible="false" />
                                            </div>
                                        </div>
                                        <div class="clear paddingtop5">
                                        </div>
                                        <asp:Label ID="lblSuccessMessageNM" runat="server" Font-Size="Medium" ForeColor="Green"></asp:Label>
                                        <asp:GridView ID="gvOnlineNews" runat="server" AutoGenerateColumns="false" CellPadding="5"
                                            CellSpacing="0" AllowSorting="true" AllowPaging="false" PageSize="10" EmptyDataText="No Results Found"
                                            CssClass="grid grid-iq" OnSorting="GvOnlineNews_Sorting" OnRowCommand="gvOnlineNews_RowCommand">
                                            <Columns>
                                                <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnArticleID" runat="server" Value='<%# Eval("ArticleID") %>' />
                                                        <asp:ImageButton ID="imgArticleButton" runat="server" CommandArgument='<%# Eval("Url") %>'
                                                            ImageUrl="~/Images/NewsRead.png" CommandName="ShowArticle" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid-th" Width="7%" />
                                                    <ItemStyle CssClass="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="false" HeaderText="Publication">
                                                    <ItemTemplate>
                                                        <a id="aPublication" target="_blank" href='<%# Eval("publication") %>'>
                                                            <%# Eval("publication") %></a>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid-th-left" Width="17%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Title" HeaderText="Title">
                                                    <HeaderStyle CssClass="grid-th-left" Width="31%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="harvest_time" SortExpression="harvest_time" HeaderText="Date Time">
                                                    <HeaderStyle CssClass="grid-th-right" Width="17%" />
                                                    <ItemStyle CssClass="right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Category" HeaderText="Category">
                                                    <HeaderStyle CssClass="grid-th-left" Width="11%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="Genre" HeaderText="Genre">
                                                    <HeaderStyle CssClass="grid-th-left" Width="11%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:BoundField>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <input id="ChkDelete" type="checkbox" value='<%# Eval("ID") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid-th" Width="5%" />
                                                    <ItemStyle CssClass="center" />
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
                                        <div class="clear paddingtop5">
                                            <div class="float-left">
                                                <span id="spnSMChartProgramFound" runat="server" visible="false" class="float-left">
                                                    Total Articles Found :&nbsp; </span>
                                                <asp:Label ID="lblSMChart" runat="server" Visible="false" CssClass="float-right"></asp:Label></div>
                                            <div class="float-right">
                                                <asp:Button ID="btnRemoveSM" Text="Remove Selected Results" CssClass="btn-blue2"
                                                    runat="server" OnClick="btnRemoveSM_Click" OnClientClick="return ValidateGrid('_gvSocialMedia');"
                                                    Visible="false" />
                                            </div>
                                        </div>
                                        <div class="clear paddingtop5">
                                        </div>
                                        <asp:Label ID="lblSuccessMessageSM" runat="server" Font-Size="Medium" ForeColor="Green"></asp:Label>
                                        <asp:GridView ID="gvSocialMedia" runat="server" AutoGenerateColumns="false" CellPadding="5"
                                            CellSpacing="0" AllowSorting="true" AllowPaging="false" PageSize="10" EmptyDataText="No Results Found"
                                            CssClass="grid grid-iq" OnRowCommand="gvSocialMedia_RowCommand" OnSorting="GvSocialMedia_Sorting">
                                            <%--OnSorting="GvOnlineNews_Sorting" OnRowCommand="gvOnlineNews_RowCommand"--%>
                                            <Columns>
                                                <asp:TemplateField ShowHeader="true" HeaderText="Article">
                                                    <ItemTemplate>
                                                        <asp:HiddenField ID="hdnSMArticleID" runat="server" Value='<%# Eval("SeqID") %>' />
                                                        <asp:ImageButton ID="imgSMArticleButton" runat="server" CommandArgument='<%# Eval("link") %>'
                                                            ImageUrl="~/Images/NewsRead.png" AlternateText="" CommandName="ShowArticle" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid-th" Width="7%" />
                                                    <ItemStyle CssClass="center" />
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="false" HeaderText="Publication">
                                                    <ItemTemplate>
                                                        <a id="aPublication" target="_blank" href='<%# Eval("homeLink") %>'>
                                                            <%# Eval("homeLink")%></a>
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid-th-left" Width="11%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="description" HeaderText="Title">
                                                    <HeaderStyle CssClass="grid-th-left" Width="29%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="itemHarvestDate_DT" SortExpression="itemHarvestDate_DT"
                                                    HeaderText="Date Time">
                                                    <HeaderStyle CssClass="grid-th-right" Width="18%" />
                                                    <ItemStyle CssClass="right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="feedCategories" HeaderText="Category">
                                                    <HeaderStyle CssClass="grid-th-left" Width="12%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="feedClass" HeaderText="Type">
                                                    <HeaderStyle CssClass="grid-th-left" Width="12%" />
                                                    <ItemStyle CssClass="left" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="feedRank" HeaderText="Rank">
                                                    <HeaderStyle CssClass="grid-th-right" Width="6%" />
                                                    <ItemStyle CssClass="right" />
                                                </asp:BoundField>
                                                <asp:TemplateField ShowHeader="false">
                                                    <ItemTemplate>
                                                        <input id="ChkDelete" type="checkbox" value='<%# Eval("ID") %>' runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="grid-th" Width="5%" />
                                                    <ItemStyle CssClass="center" />
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
                                            <div class="float-right">
                                                <span id="spnTwitterChartProgramFound" runat="server" visible="false" class="float-left">
                                                    Total Tweets Found :&nbsp; </span>
                                                <asp:Label ID="lblTwitterChart" runat="server" Visible="false" CssClass="float-right"></asp:Label>
                                            </div>
                                        </div>
                                        <div class="clear">
                                        </div>
                                        <asp:DataList ID="dlTweets" Width="100%" runat="server" CellPadding="0" CellSpacing="0"
                                            GridLines="None" OnItemCommand="dlTweets_ItemCommand">
                                            <ItemTemplate>
                                                <div id="datalistInner" class="clear TweetInnerDiv">
                                                    <div class="float-left TweetBodyDivIQP borderBoxSizing">
                                                        <div class="clear">
                                                            <div class="float-left TweetActorDisplayName">
                                                                <a id="aActorLink" runat="server" href='<%#  Eval("actor_link") %>' target="_blank">
                                                                    <asp:Label ID="lblDisplayName" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "actor_displayName")%>'></asp:Label>
                                                                </a><span class="TweetSubdivFont">@</span><asp:Label ID="lblPrefferedUserName" runat="server"
                                                                    CssClass="TweetSubdivFont" Text='<%# Eval("actor_preferredName") %>'></asp:Label>
                                                                <br />
                                                            </div>
                                                            <div class="float-right">
                                                                <div class="float-left TweetSubdivFont">
                                                                    Klout Score:&nbsp;
                                                                    <asp:Label ID="lblKloutScore" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "gnip_Klout_score")%>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </div>
                                                                <div class="float-left TweetSubdivFont">
                                                                    Followers:&nbsp;
                                                                    <asp:Label ID="lblActorFollowers" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "actor_followerscount")%>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                                </div>
                                                                <div class="float-left TweetSubdivFont">
                                                                    Friends:&nbsp;
                                                                    <asp:Label ID="lblActorFriends" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "actor_friendscount")%>'></asp:Label></div>
                                                            </div>
                                                        </div>
                                                        <div class="clear PaddingTopBottom1p TweetBodyText">
                                                            <div class="div75pleft">
                                                                <asp:Label ID="lblTweetBody" runat="server" Text='<%# Convert.ToString(Eval("Summary")) %>'></asp:Label>
                                                            </div>
                                                            <div class="TweetSubdivFont float-right">
                                                                <asp:Label ID="lblPostedDateTime" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "tweet_postedDateTime")%>'></asp:Label></div>
                                                        </div>
                                                    </div>
                                                    <div class="float-right IQPremiumTweetImageDiv center">
                                                        <a id="aActorLinkimage" runat="server" href='<%#  Eval("actor_link") %>' target="_blank">
                                                            <%--<asp:Image ID="imgActor" runat="server" ImageUrl='<%# DataBinder.Eval(Container.DataItem, "actor_image")%>' />--%>
                                                        </a>
                                                        <br />
                                                        <asp:LinkButton ID="lnlSaveTweet" runat="server" CommandName="SaveTweet" Text="Save Tweet"
                                                            CommandArgument='<%# Eval("tweetid") %>'></asp:LinkButton>
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
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
<div id="diviframe" runat="server" class="modal hide fade resizable playerPopUp in">
    <div id="divClosePlayer">
        <input type="image" id="imgCancelPlayer" onclick="ClosePlayer();return false;" class="popup-top-close"
            src="../Images/close-icon.png" />
    </div>
    <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:IframeRawMediaH ID="IframeRawMediaH" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="diviFrameOnlineNewsArticle" runat="server" class="modal hide fade resizable modalPopupDiv iframePopup modalPopupDivNews"
    style="display: none;">
    <div id="divCloseNews">
        <div>
            <input type="image" id="Image1" class="popup-top-close" onclick="CloseNewsIframe();return false;"
                src="../Images/close-icon.png" />
        </div>
    </div>
    <asp:UpdatePanel ID="upOnlineNewsArticle" runat="server" UpdateMode="Conditional"
        style="width: 100%; height: 100%;">
        <ContentTemplate>
            <iframe id="iFrameOnlineNewsArticle" width="100%" height="100%" scrolling="auto"
                visible="false" frameborder="0" runat="server" src=""></iframe>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="DivNews-btn">
        <input type="button" class="btn-green" value="Save Article" onclick="CloseNewsIframe();return OpenSaveArticlePopup(0);" />
        <input type="button" class="btn-green" value="Print Article" onclick="PrintIframe();return false;" />
    </div>
</div>
<asp:Panel ID="pnlSaveArticle" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;" runat="server" DefaultButton="btnSaveArticle">
    <asp:UpdatePanel ID="upSaveArticle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span id="spnSaveArticleTitle" runat="server">Article Details</span>
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
                                onchange="return UpdateSubCategory1(this.id);" Width="36%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgSaveArticle"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Sub Category 1 :</label>
                            <asp:DropDownList ID="ddlSubCategory1" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 2 :</label>
                            <asp:DropDownList ID="ddlSubCategory2" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 3 :</label>
                            <asp:DropDownList ID="ddlSubCategory3" runat="server" onchange="UpdateSubCategory1(this.id);"
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
                                        <AjaxToolkit:SliderExtender ID="seArticle" runat="server" TargetControlID="txtArticleRate"
                                            Minimum="1" Maximum="6" Steps="6" RaiseChangeOnlyOnMouseUp="true" BehaviorID="seArticle">
                                        </AjaxToolkit:SliderExtender>
                                    </span><span class="float-left RateArticleNumberRight">6</span> <span class="float-left prefferredcheckbox">
                                        <asp:CheckBox ID="chkArticlePreferred" runat="server" Text="Preferred" onclick="PrefferredChecked('chkArticlePreferred','seArticle');" />
                                    </span>
                                </div>
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
<%--<asp:Panel ID="pnlSaveArticle" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;" runat="server" DefaultButton="btnSaveArticle">
    <asp:UpdatePanel ID="upSaveArticle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span id="spnSaveArticleTitle" runat="server">Article Details</span>
                    <div style="width: 27px; float: right;" id="div7" runat="server">
                        <img id="btnClose" src="../Images/close-icon.png" onclick="closeModal('pnlSaveArticle');" />
                    </div>
                </div>
                <div class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="vlSummerySaveArticle" runat="server" ValidationGroup="vgSaveArticle"
                            CssClass="error-summery" />
                        <asp:Label ID="lblSaveArticleMsg" runat="server" Visible="true" CssClass="MsgFail"></asp:Label>
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
                                onchange="return UpdateSubCategory1(this.id);" Width="36%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgSaveArticle"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Sub Category 1 :</label>
                            <asp:DropDownList ID="ddlSubCategory1" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 2 :</label>
                            <asp:DropDownList ID="ddlSubCategory2" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 3 :</label>
                            <asp:DropDownList ID="ddlSubCategory3" runat="server" onchange="UpdateSubCategory1(this.id);"
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
                                        <AjaxToolkit:SliderExtender ID="seArticle" runat="server" TargetControlID="txtArticleRate"
                                            Minimum="1" Maximum="6" Steps="6" RaiseChangeOnlyOnMouseUp="true" BehaviorID="seArticle">
                                        </AjaxToolkit:SliderExtender>
                                    </span><span class="float-left RateArticleNumberRight">6</span> <span class="float-left prefferredcheckbox">
                                        <asp:CheckBox ID="chkArticlePreferred" runat="server" Text="Preferred" onclick="PrefferredChecked('chkArticlePreferred','seArticle');" />
                                    </span>
                                </div>
                            </div>
                        </li>
                    </ul>
                    <div class="text-align-center">
                        <input type="button" id="Button2" class="btn-blue2" value="Cancel" onclick="closeModal('pnlSaveArticle');" />
                        <asp:Button ID="btnSaveArticle" runat="server" Text="Save" ValidationGroup="vgSaveArticle"
                            CausesValidation="true" CssClass="btn-blue2" OnClick="btnSaveArticle_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>--%>
