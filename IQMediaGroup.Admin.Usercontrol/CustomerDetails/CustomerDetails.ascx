<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerDetails.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.CustomerDetails.CustomerDetails" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<style type="text/css">
    #Panel1
    {
        width: 100%;
        overflow: auto;
        overflow-x: hidden;
        margin-right: 17px;
    }
    * + html #Panel1
    {
        width: auto;
        padding-right: 17px;
    }
</style>
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
            <img id="Img1" runat="server" src="~/images/contentbox-l.jpg" width="8" height="1"
                border="0" alt="iQMedia" />
        </td>
        <td valign="top" bgcolor="#FFFFFF">
            <%--<asp:UpdatePanel ID="upCustomer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>--%>
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td height="3px">
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Panel ID="Panel1" runat="server" Height="90%" Style="vertical-align: middle;
                            position: relative; background-position: center;">
                            <table cellpadding="5" width="100%" cellspacing="0" style="border: solid 1px #e4e4e4;
                                border-collapse: collapse;">
                                <tr>
                                    <td style="padding-bottom: 10px">
                                        <div class="AdminTitle">
                                            Client :
                                            <asp:Label ID="lblClient" runat="server"></asp:Label></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:ValidationSummary ID="ValidationSummary2" EnableClientScript="true" runat="server"
                                            ValidationGroup="validate1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                        <%--<asp:Panel Width="100%" ID="pnlCustomer" runat="server">--%>
                                        <asp:GridView ID="gvCustomer" runat="server" border="1" Width="100%" AllowPaging="True"
                                            PageSize="15" CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                            BorderColor="#E4E4E4" Style="border-collapse: collapse;" PagerSettings-Mode="NextPrevious"
                                            AutoGenerateColumns="False" EmptyDataText="No Data Found" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="gvCustomer_PageIndexChanging"
                                            OnRowDataBound="gvCustomer_RowDataBound" DataKeyNames="CustomerKey" OnRowEditing="gvCustomer_RowEditing"
                                            OnRowCancelingEdit="gvCustomer_RowCancelingEdit" OnRowUpdating="gvCustomer_RowUpdating">
                                            <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                            Text=" " CommandName="Edit"></asp:LinkButton>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:LinkButton ID="lbtnUpdate" runat="server" CausesValidation="true" ValidationGroup="validate1"
                                                            CommandName="Update" CssClass="btn-update" Text=" "></asp:LinkButton>
                                                        &nbsp;<asp:LinkButton ID="lbtnCancel" runat="server" CausesValidation="False" CommandName="Cancel"
                                                            CssClass="btn-cancel" Text=" "></asp:LinkButton>
                                                    </EditItemTemplate>
                                                    <ItemStyle Width="30" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="First Name" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblFirstName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox Width="80px" ID="txtFirstName" runat="server" Text='<%# Bind("FirstName") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ValidationGroup="validate1" ID="reqFirstName" runat="server"
                                                            Display="Dynamic" ErrorMessage="Please Enter First Name" Text="*" ControlToValidate="txtFirstName">
                                                        </asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Last Name" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox Width="80px" ID="txtLastName" runat="server" Text='<%# Bind("LastName") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ValidationGroup="validate1" ID="reqLastName" runat="server"
                                                            Display="Dynamic" ErrorMessage="Please Enter Last Name" Text="*" ControlToValidate="txtLastName">
                                                        </asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Email Address" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblEmail" runat="server" Text='<%# Bind("Email") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox Width="150px" ID="txtEmail" runat="server" Text='<%# Bind("Email") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ID="reqEmail" runat="server" ValidationGroup="validate1"
                                                            Display="Dynamic" ErrorMessage="Please Enter Email" Text="*" ControlToValidate="txtEmail">
                                                        </asp:RequiredFieldValidator>
                                                        <asp:RegularExpressionValidator ID="regEmailGrid" runat="server" Display="None" ValidationGroup="validate1"
                                                            ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                            ErrorMessage="Please Enter Valid Email Address" Text=""></asp:RegularExpressionValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Password" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPassword" runat="server" Text='<%# Bind("CustomerPassword") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox Width="80px" ID="txtPassword" runat="server" Text='<%# Bind("CustomerPassword") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ValidationGroup="validate1" ID="reqPassword" runat="server"
                                                            Display="Dynamic" ErrorMessage="Please Enter Password" Text="*" ControlToValidate="txtPassword">
                                                        </asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Contact No." HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblContactNo" runat="server" Text='<%# Bind("ContactNo") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox Width="80px" ID="txtContactNo" runat="server" Text='<%# Bind("ContactNo") %>'></asp:TextBox>
                                                        <asp:RequiredFieldValidator ValidationGroup="validate1" ID="reqContactNo" runat="server"
                                                            Display="Dynamic" ErrorMessage="Please Enter Contact No." Text="*" ControlToValidate="txtContactNo">
                                                        </asp:RequiredFieldValidator>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Customer Comment" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCustomerComment" runat="server" Text='<%# Bind("CustomerComment") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox Width="130px" Rows="3" TextMode="MultiLine" ID="txtCustomerComment"
                                                            runat="server" Text='<%# Bind("CustomerComment") %>'></asp:TextBox>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Default Page" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblDefaultPage" runat="server" Text='<%# Bind("DefaultPage") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:HiddenField ID="hdnDefaultPage" runat="server" Value='<%# Bind("DefaultPage") %>' />
                                                        <asp:DropDownList ID="ddlDefaultPage" CssClass="content-text" runat="server">
                                                        </asp:DropDownList>
                                                    </EditItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                                <asp:TemplateField ShowHeader="false" Visible="false" HeaderText="IQBasic" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/IQBasic.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIQBasic" Enabled="false" runat="server" Checked='<%# Eval("IQBasic").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEIQBasic" runat="server" Checked='<%# Eval("IQBasic").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="grid-th" VerticalAlign="Top" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="AdvancedSearchAccess" Visible="false" ShowHeader="false"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/AdvancedSearch.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAdvancedSearchAccess" Enabled="false" runat="server" Checked='<%# Eval("AdvancedSearchAccess").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEAdvancedSearchAccess" runat="server" Checked='<%# Eval("AdvancedSearchAccess").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle CssClass="grid-th" VerticalAlign="Top" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="GlobalAdminAccess" Visible="false" ShowHeader="false"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/GlobalAdmin.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkGlobalAdminAccess" Enabled="false" runat="server" Checked='<%# Eval("GlobalAdminAccess").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEGlobalAdminAccess" runat="server" Checked='<%# Eval("GlobalAdminAccess").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="myIQAccess" Visible="false" ShowHeader="false" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/myIQ.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkmyIQAccess" runat="server" Enabled="false" Checked='<%# Eval("myIQAccess").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEmyIQAccess" runat="server" Checked='<%# Eval("myIQAccess").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IQAgentWebsiteAccess" Visible="false" ShowHeader="false"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/IQAgentWebSite.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIQAgentWebsiteAccess" Enabled="false" runat="server" Checked='<%# Eval("IQAgentWebsiteAccess").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEIQAgentWebsiteAccess" runat="server" Checked='<%# Eval("IQAgentWebsiteAccess").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="DownloadClips" Visible="false" ShowHeader="false"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/DownloadClips.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkDownloadClips" runat="server" Enabled="false" Checked='<%# Eval("DownloadClips").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEDownloadClips" runat="server" Checked='<%# Eval("DownloadClips").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="IQCustomAccess" Visible="false" ShowHeader="false"
                                                    HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgIQAgentAccess" runat="server" ImageUrl="~/Images/IQCustom.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIQCustomAccess" runat="server" Enabled="false" Checked='<%# Eval("IQCustomAccess").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEIQCustomAccess" runat="server" Checked='<%# Eval("IQCustomAccess").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UGCDownload" Visible="false" ShowHeader="false" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgUGCDownload" runat="server" ImageUrl="~/Images/ugc_down.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkUGCDownload" runat="server" Enabled="false" Checked='<%# Eval("UGCDownload").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkEUGCDownload" runat="server" Checked='<%# Eval("UGCDownload").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="UGCUploadEdit" Visible="false" ShowHeader="false" HeaderStyle-HorizontalAlign="Center">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="imgUGCUploadEdit" AlternateText="UGCUploadEdit" runat="server" ImageUrl="~/Images/UGCUploadEdit.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkUGCUploadEdit" runat="server" Enabled="false" Checked='<%# Eval("UGCUploadEdit").ToString() == "1" ? true : false %>' />
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkUGCUploadEdit" runat="server" Checked='<%# Eval("UGCUploadEdit").ToString() == "1" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                    <HeaderStyle VerticalAlign="Top" CssClass="grid-th" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <asp:Image ID="ingIQBasic" runat="server" ImageUrl="~/Images/IsActive.jpg" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("IsActive") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkStatus" runat="server" Checked='<%#DataBinder.Eval(Container.DataItem,"IsActive").ToString() == "True" ? true : false %>' />
                                                    </EditItemTemplate>
                                                    <HeaderStyle VerticalAlign="Top" HorizontalAlign="Left" />
                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="grid-th" />
                                        </asp:GridView>
                                        <asp:Label ID="lblNoResults" runat="server" Visible="false"></asp:Label>
                                        <%--</asp:Panel>--%>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <%-- </ContentTemplate>
            </asp:UpdatePanel>--%>
        </td>
        <td class="contentbox-r">
            <img id="Img9" runat="server" src="~/images/contentbox-r.jpg" width="8" height="1"
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
