<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAgentConsole.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.IQAgentConsole.IQAgentConsole" %>
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
                                IQ Agent Console</div>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <div id="divResultGrid" style="width: 100%">
                <asp:UpdatePanel ID="updSearchResults" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divConfig" style="width: 100%">
                            <table cellpadding="3" cellspacing="3">
                                <tr>
                                    <td>
                                        Choose Client
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlClient" runat="server">
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="btnGetStoredSearches" class="dropdown01" runat="server" Text="Get All IQ Agent Stored Searches"
                                            CssClass="btn-grey" OnClick="btnGetStoredSearches_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <br />
                        <asp:GridView ID="grvSearch" runat="server" AllowPaging="true" PageSize="30" border="1"
                            DataKeyNames="SearchRequestKey" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4"
                            Style="border-collapse: collapse;" AutoGenerateColumns="false" EmptyDataText="No Results Found"
                            PagerSettings-Mode="NextPrevious" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" HeaderStyle-CssClass="grid-th"
                            OnPageIndexChanging="grvSearch_PageIndexChanging" OnRowEditing="grvSearch_RowEditing"
                            OnRowCancelingEdit="grvSearch_RowCancelingEdit" OnRowUpdating="grvSearch_RowUpdating"
                            OnRowDataBound="grvSearch_RowDataBound">
                            <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="False" CommandName="Edit"
                                            CssClass="btn-edit" Text="&nbsp;"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lbtnUpdate" runat="server" CausesValidation="true" CommandName="Update"
                                            CssClass="btn-update" Text=" "></asp:LinkButton>
                                        &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                            CssClass="btn-cancel" Text=" "></asp:LinkButton>
                                    </EditItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Query Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQueryName" Text='<%# Eval("Query_Name") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:Label ID="lblQueryName" Text='<%# Eval("Query_Name") %>' runat="server"></asp:Label>
                                    </EditItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Query Version">
                                    <ItemTemplate>
                                        <asp:Label ID="lblQueryVersion" Text='<%# Eval("Query_Version") %>' runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Search Term">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSearchTerm" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program Name">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProgramName" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Program Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProgramDescription" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Active">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" runat="server" Checked='<%# Convert.ToBoolean(Eval("IsActive")) %>'
                                            Enabled="false" />
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:CheckBox ID="cbIsActive" Checked='<%# Bind("IsActive") %>' runat="server" />
                                    </EditItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                </asp:UpdatePanel>
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
<asp:UpdateProgress ID="updSearchResultsProgress" runat="server" AssociatedUpdatePanelID="updSearchResults"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="updSearchResultsExtender" ControlToOverlayID="updSearchResults"
    TargetControlID="updSearchResultsProgress" CssClass="updateProgress" />
