<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQRole.ascx.cs" Inherits="IQMediaGroup.Admin.Usercontrol.IQRole.IQRole" %>
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
            <asp:UpdatePanel ID="upRole" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                                    <tr>
                                        <td style="padding-bottom: 10px">
                                            <div class="AdminTitle">
                                                iQ Role</div>
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
                                                        Role Name:
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:TextBox ID="txtRoleName" runat="server" CssClass="textbox01"></asp:TextBox>
                                                            <ajaxtoolkit:filteredtextboxextender id="ftbEtxtRoleName" runat="server" targetcontrolid="txtRoleName"
                                                                filtermode="InvalidChars" filtertype="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                invalidchars="<>">
                                                            </ajaxtoolkit:filteredtextboxextender>
                                                            <asp:RequiredFieldValidator ID="rfvRoleName" Display="Dynamic" Text="*" ValidationGroup="validate"
                                                                runat="server" ControlToValidate="txtRoleName" ErrorMessage="Please Enter Role Name."></asp:RequiredFieldValidator>
                                                        </label>
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
                                                <%--<tr>
                                                    <td width="3">
                                                        <img id="Img5" runat="server" src="~/images/bluebox-hd-left.jpg" width="3" height="24"
                                                            border="0" alt="iQMedia" />
                                                    </td>
                                                    <td class="bluebox-hd">
                                                        <asp:Label ID="lblRoles" runat="server" Text="Roles"></asp:Label><br />
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
                                                            Roles</div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="3">
                                                        <asp:Label ID="lblErrorMessage1" runat="server" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                                        <asp:GridView ID="gvRole" runat="server" Width="100%" border="1" AllowPaging="True"
                                                            PageSize="15" CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                                            BorderColor="#E4E4E4" Style="border-collapse: collapse;" PagerSettings-Mode="NextPrevious"
                                                            AutoGenerateColumns="False" EmptyDataText="No Data Found" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="gvRole_PageIndexChanging"
                                                            OnRowCancelingEdit="gvRole_RowCancelingEdit" OnRowEditing="gvRole_RowEditing"
                                                            OnRowUpdating="gvRole_RowUpdating">
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
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Role Name" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:HiddenField ID="hdnRoleKey" runat="server" Value='<%# Bind("RoleID") %>' />
                                                                        <asp:TextBox ID="txtRoleName" runat="server" Text='<%# Bind("RoleName") %>'></asp:TextBox>
                                                                        <ajaxtoolkit:filteredtextboxextender id="ftbEtxtRoleNameGrid" runat="server" targetcontrolid="txtRoleName"
                                                                            filtermode="InvalidChars" filtertype="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                            invalidchars="<>">
                                                                        </ajaxtoolkit:filteredtextboxextender>
                                                                        <asp:RequiredFieldValidator ID="rfvRoleNameGrid" runat="server" ControlToValidate="txtRoleName"
                                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="content-text" Width="55%"></ItemStyle>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="IsActive" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("IsActive") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlStatus" runat="server" OnPreRender="ddlStatus_PreRender">
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvStatus" runat="server" InitialValue="-1" ControlToValidate="ddlStatus"
                                                                            ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                    <HeaderStyle HorizontalAlign="Left" />
                                                                    <ItemStyle CssClass="content-text" Width="35%"></ItemStyle>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <HeaderStyle CssClass="grid-th" />
                                                        </asp:GridView>
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
<asp:UpdateProgress ID="updProgressRole" runat="server" AssociatedUpdatePanelID="upRole"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtenderRole"
    ControlToOverlayID="upRole" TargetControlID="updProgressRole" CssClass="updateProgress" />
