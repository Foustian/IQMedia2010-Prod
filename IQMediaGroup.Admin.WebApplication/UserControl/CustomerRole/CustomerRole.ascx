<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerRole.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.CustomerRole.CustomerRole" %>
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
            <asp:UpdatePanel ID="upClient" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                                    <tr>
                                        <td style="padding-bottom: 10px">
                                            <div class="AdminTitle">
                                                Customer Role Mapping</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="3" cellpadding="0">
                                                <tr>
                                                    <td width="2%">
                                                    </td>
                                                    <td width="98%" align="left" valign="top">
                                                        <asp:ValidationSummary ID="vsIQMediaSearch" EnableClientScript="true" runat="server"
                                                            ValidationGroup="validate" ForeColor="#bd0000" Font-Size="Smaller" />
                                                        <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Size="Smaller"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table border="0" cellspacing="5" cellpadding="0">
                                                <tr>
                                                    <td class="content-text">
                                                        Customer Name:
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="dropdown01">
                                                            </asp:DropDownList>
                                                            <asp:RequiredFieldValidator ID="rfvCustomer" Text="*" ValidationGroup="validate"
                                                                Display="Dynamic" runat="server" InitialValue="0" ControlToValidate="ddlCustomer"
                                                                ErrorMessage="Please Select Customer."></asp:RequiredFieldValidator>
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="content-text">
                                                        Role Name:
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRole" runat="server" CssClass="dropdown01">
                                                        </asp:DropDownList>
                                                        <asp:RequiredFieldValidator ID="rfvRole" Text="*" ValidationGroup="validate" Display="Dynamic"
                                                            runat="server" InitialValue="0" ControlToValidate="ddlRole" ErrorMessage="Please Select Role."></asp:RequiredFieldValidator>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="content-text">
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn-blue" ValidationGroup="validate"
                                                                OnClick="btnAdd_Click" />
                                                        </label>
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
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <%-- <tr>
                                                    <td width="3">
                                                        <img id="Img5" runat="server" src="~/images/bluebox-hd-left.jpg" width="3" height="24"
                                                            border="0" alt="iQMedia" />
                                                    </td>
                                                    <td class="bluebox-hd">
                                                        <asp:Label ID="lblCustomers" runat="server" Text="Customer Roles"></asp:Label><br />
                                                    </td>
                                                    <td width="3">
                                                        <img id="Img6" runat="server" src="~/images/bluebox-hd-right.jpg" width="3" height="24"
                                                            border="0" alt="iQMedia" />
                                                    </td>
                                                </tr>--%>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="3">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" cellpadding="5" cellspacing="0" bordercolor="#e4e4e4" style="border: solid 1px #e4e4e4;
                                                border-collapse: collapse;">
                                                <tr>
                                                    <td style="padding-bottom: 10px">
                                                        <div class="AdminTitle">
                                                            Customer Roles</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:GridView ID="gvCustomerRole" runat="server" Width="100%" border="1" AllowPaging="True"
                                                            PageSize="15" CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                                            BorderColor="#E4E4E4" Style="border-collapse: collapse;" PagerSettings-Mode="NextPrevious"
                                                            AutoGenerateColumns="False" EmptyDataText="No Data Found" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="gvCustomerRole_PageIndexChanging"
                                                            OnRowCancelingEdit="gvCustomerRole_RowCancelingEdit" OnRowEditing="gvCustomerRole_RowEditing"
                                                            OnRowUpdating="gvCustomerRole_RowUpdating" OnDataBound="gvCustomerRole_DataBound">
                                                            <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                                            <Columns>
                                                                <asp:TemplateField ShowHeader="False">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                            CommandName="Edit" Text=" "></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:LinkButton ID="lbtnUpdate" runat="server" CssClass="btn-update" CausesValidation="True"
                                                                            CommandName="Update" Text=" "></asp:LinkButton>
                                                                        &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" CssClass="btn-cancel" CausesValidation="False"
                                                                            CommandName="Cancel" Text=" "></asp:LinkButton>
                                                                    </EditItemTemplate>
                                                                    <ItemStyle HorizontalAlign="Center" />
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Customer Name" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblCustomerName" runat="server" Text="Customer Name"></asp:Label><br />
                                                                        <asp:DropDownList ID="ddlCustomerName" runat="server" AutoPostBack="true" CssClass="dropdown01"
                                                                            OnSelectedIndexChanged="ddlCustomerName_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblCustomerName" runat="server" Text='<%# Bind("FullName") %>' ItemStyle-CssClass="content-text"></asp:Label>
                                                                        <asp:HiddenField ID="hdnCustomerRoleKey" runat="server" Value='<%# Bind("CustomerRoleKey") %>' />
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="content-text" Width="35%"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Role Name" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblRoleName" runat="server" Text="Role Name"></asp:Label><br />
                                                                        <asp:DropDownList ID="ddlRoleName" runat="server" AutoPostBack="true" CssClass="dropdown01"
                                                                            OnSelectedIndexChanged="ddlRoleName_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRoleID" runat="server" Text='<%# Bind("RoleName") %>' ItemStyle-CssClass="content-text"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="content-text" Width="35%"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Client Name" HeaderStyle-HorizontalAlign="Left">
                                                                    <HeaderTemplate>
                                                                        <asp:Label ID="lblClientName" runat="server" Text="Client Name"></asp:Label><br />
                                                                        <asp:DropDownList ID="ddlClientName" runat="server" AutoPostBack="true" CssClass="dropdown01"
                                                                            OnSelectedIndexChanged="ddlClientName_SelectedIndexChanged">
                                                                        </asp:DropDownList>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblClientName" runat="server" Text='<%# Bind("ClientName") %>' ItemStyle-CssClass="content-text"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="content-text" Width="35%"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IsAccess" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("IsAccess") %>' ItemStyle-CssClass="content-text"></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlStatus" runat="server" OnPreRender="ddlStatus_PreRender">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvStatus" Display="Dynamic" runat="server" InitialValue="-1"
                                                                            ControlToValidate="ddlStatus" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="content-text" Width="20%"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="grid-th" />
                                                        </asp:GridView>
                                                        <asp:Label ID="lblNoResults" runat="server" Visible="false"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="35">
                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td align="right" valign="top" class="contenttext-small">
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
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
<asp:UpdateProgress ID="updProgressClient" runat="server" AssociatedUpdatePanelID="upClient"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="upClient" TargetControlID="updProgressClient" CssClass="updateProgress" />
