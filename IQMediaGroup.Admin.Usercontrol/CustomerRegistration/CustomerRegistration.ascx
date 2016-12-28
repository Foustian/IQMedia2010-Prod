<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomerRegistration.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.CustomerRegistration.CustomerRegistration" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<script type="text/javascript">

    function selected_Client(source, eventArgs) {

        document.getElementById('ctl00_Content_Data_CustomerRegistration1_txtSearchCustomerByName').value = "";
        document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnAdd').disabled = false;

    }

</script>
<script type="text/javascript">
    function focustxt() {
        document.getElementById('ctl00_Content_Data_CustomerRegistration1_txtClientName').value = "";
        document.getElementById('ctl00_Content_Data_CustomerRegistration1_txtClientName').focus();
        return false;
    }
</script>
<script type="text/javascript">
    function PopUpButtonSubmit(evt, element) {
        if (evt.keyCode == 13)
            document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnSave').click();
    }
</script>
<script type="text/javascript">
    function PopUpButtonSubmit(evt, element) {
        if (evt.keyCode == 13)
            document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnSave').click();
    }
</script>
<script type="text/javascript">
    function GetChar(event) {
        if (event.keyCode == 13) {
            document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnSearch').click();
        }
    }
</script>
<script type="text/javascript">
    function GetCustomer(event) {
        if (event.keyCode == 13) {
            document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnSearch').click();
        }
    }   

</script>
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
                    <td>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                            <tr>
                                <td style="padding-bottom: 5px">
                                    <div class="AdminTitle">
                                        Customer Registration</div>
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
                                                    ValidationGroup="validate" ForeColor="#bd0000" />
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Green"></asp:Label>
                                                <asp:Label ID="lblErrorMessageGUID" runat="server" ForeColor="Red"></asp:Label>
                                                <asp:Label ID="lblInvalidCustomer" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table style="vertical-align: middle" border="0" cellspacing="0" cellpadding="5">
                                        <tr>
                                            <td class="content-text">
                                                Client Name:
                                            </td>
                                            <td>
                                                <%--<table border="0" cellspacing="5" cellpadding="0">--%>
                                                <asp:TextBox ID="txtClientName" runat="server" CssClass="textbox03" Width="143px"
                                                    onkeypress="GetChar(event);"></asp:TextBox>
                                                <asp:ImageButton runat="server" ID="imgArrow" Width="18px" Height="22px" ImageUrl="~/Images/dropbtn.jpg"
                                                    OnClientClick="return focustxt();" />
                                                <script type="text/javascript">
                                                    function ChangeButton(ClientName) {
                                                        var content = document.getElementById('ctl00_Content_Data_CustomerRegistration1_txtClientName').value;
                                                        var str = ClientName;
                                                        var ClientArray = str.split(",");

                                                        for (var i = 0; i < ClientArray.length; i++) {
                                                            if (content.toLowerCase() == ClientArray[i].toLowerCase()) {
                                                                //alert("if" + content);
                                                                document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnAdd').disabled = false;
                                                                break;
                                                            }
                                                            else {
                                                                //alert("else" + content);
                                                                document.getElementById('ctl00_Content_Data_CustomerRegistration1_btnAdd').disabled = true;
                                                            }
                                                        }
                                                    }   
                                                </script>
                                                <AjaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" ServicePath="~/SearchTerm.asmx"
                                                    Enabled="true" ServiceMethod="GetCompletionList" MinimumPrefixLength="0" TargetControlID="txtClientName"
                                                    OnClientItemSelected="selected_Client">
                                                </AjaxToolkit:AutoCompleteExtender>
                                                <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" Display="Dynamic"
                                                    ValidationGroup="validate" runat="server" ControlToValidate="txtClientName" ErrorMessage="Please Select Client Name."></asp:RequiredFieldValidator>--%>
                                                <%-- </table>--%>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:Button ID="btnSearch" runat="server" CssClass="btn-blue" OnClick="btnSearch_Click"
                                                        Text="Search" />
                                                </label>
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:Button ID="btnAdd" Enabled="false" runat="server" CssClass="btn-blue" OnClick="btnAdd_Click"
                                                        Text="Add" ValidationGroup="validate" />
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text">
                                                Customer Email:
                                            </td>
                                            <td>
                                                <%--<asp:TextBox ID="txtSearchCustomerByName" runat="server" CssClass="textbox03" Width="143px"></asp:TextBox>--%>
                                                <asp:TextBox ID="txtSearchCustomerByName" runat="server" CssClass="textbox03" Width="143px"
                                                    onkeyup="GetCustomer(event);"></asp:TextBox>
                                                <AjaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender2" ServicePath="~/SearchTerm.asmx"
                                                    Enabled="true" ServiceMethod="GetCustomerList" MinimumPrefixLength="1" TargetControlID="txtSearchCustomerByName">
                                                </AjaxToolkit:AutoCompleteExtender>
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
                            <%--<tr>
                                <td height="10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                            <tr>
                                <td height="3px">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="Panel1" runat="server" Height="90%" Style="vertical-align: middle;
                                        position: relative; background-position: center; display: none;">
                                        <table cellpadding="5" width="100%" cellspacing="0" style="border: solid 1px #e4e4e4;
                                            border-collapse: collapse;">
                                            <tr>
                                                <td style="padding-bottom: 10px">
                                                    <div class="AdminTitle">
                                                        Customer</div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red" Font-Size="Smaller"></asp:Label>
                                                    <%--<asp:Panel Width="100%" ID="pnlCustomer" runat="server">--%>
                                                    <asp:GridView ID="gvCustomer" runat="server" border="1" Width="100%" AllowPaging="True"
                                                        PageSize="15" CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                                        BorderColor="#E4E4E4" Style="border-collapse: collapse;" PagerSettings-Mode="NextPrevious"
                                                        AutoGenerateColumns="true" EmptyDataText="No Data Found" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                        PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="gvCustomer_PageIndexChanging" OnRowDataBound="gvCustomer_OnRowDataBound"  >
                                                        <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                                        <Columns>
                                                            <asp:TemplateField ShowHeader="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                        Text=" " CommandArgument='<%#Bind("CustomerKey") %>' OnClick="lbtnEdit_Click"></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="30" />
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
                                    <div onkeydown="PopUpButtonSubmit(event, this);">
                                        <asp:Panel ID="pnlMailPanel" DefaultButton="btnSave" runat="server" ScrollBars="Auto"
                                            Style="vertical-align: middle; min-height: 500px; display: none;" GroupingText="">
                                            <table border="0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td valign="top" bgcolor="#FFFFFF">
                                                        <table border="0" align="center" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="3">
                                                                        <tr>
                                                                            <td style="padding-bottom: 2px">
                                                                                <div class="AdminTitle">
                                                                                    Customer Setup Page</div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <table width="100%" border="0" cellspacing="3" cellpadding="0">
                                                                                    <tr>
                                                                                        <td width="2%">
                                                                                        </td>
                                                                                        <td width="98%" align="left" valign="top">
                                                                                            <asp:ValidationSummary ID="ValidationSummary1" ShowSummary="true" runat="server"
                                                                                                ValidationGroup="validateCustomer" ForeColor="#bd0000" />
                                                                                            <%--<asp:Label ID="Label1" runat="server" ForeColor="Green"></asp:Label>--%>
                                                                                            <asp:Label ID="lblErrorMessageRole" runat="server" ForeColor="Red"></asp:Label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td style="padding-right: 50px">
                                                                                            <table border="0" cellspacing="5" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Client Name:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtClientNameAdd" ReadOnly="true" MaxLength="50" runat="server"
                                                                                                                CssClass="textbox03"></asp:TextBox>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Master Client:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtMasterClient" ReadOnly="true" MaxLength="50" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td style="padding-right: 50px" valign="top">
                                                                                            <table border="0" cellspacing="5" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Setup Date:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtSetupDate" ReadOnly="true" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        IsActive
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:CheckBox ID="ChkActive" runat="server" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-bottom: 2px">
                                                                                <div class="AdminTitle1">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td style="padding-right: 50px">
                                                                                            <table border="0" cellspacing="5" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Customer First Name:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtFirstName" runat="server" TargetControlID="txtFirstName"
                                                                                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                                InvalidChars="<>">
                                                                                                            </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                            <asp:RequiredFieldValidator ID="rfvFirstName" Text="*" Display="Dynamic" ValidationGroup="validateCustomer"
                                                                                                                runat="server" ControlToValidate="txtFirstName" ErrorMessage="Please Enter First Name."></asp:RequiredFieldValidator>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Customer Last Name:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtLastName" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtLastName" runat="server" TargetControlID="txtLastName"
                                                                                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                                InvalidChars="<>">
                                                                                                            </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                            <asp:RequiredFieldValidator ID="rfvLastName" Text="*" Display="Dynamic" ValidationGroup="validateCustomer"
                                                                                                                runat="server" ControlToValidate="txtLastName" ErrorMessage="Please Enter Last Name."></asp:RequiredFieldValidator>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Customer Email:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtEmail" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtEmail" runat="server" TargetControlID="txtEmail"
                                                                                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                                InvalidChars="<>">
                                                                                                            </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                            <asp:RegularExpressionValidator ID="regEmail" Text="*" runat="server" Display="Dynamic"
                                                                                                                ValidationGroup="validateCustomer" ControlToValidate="txtEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                                                                                ErrorMessage="Please Enter Valid Email Address"></asp:RegularExpressionValidator>
                                                                                                            <asp:RequiredFieldValidator ID="rfvEmail" Text="*" ValidationGroup="validateCustomer"
                                                                                                                Display="Dynamic" runat="server" ControlToValidate="txtEmail" ErrorMessage="Please Enter Email Address."></asp:RequiredFieldValidator>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                        <td style="padding-right: 50px" valign="top">
                                                                                            <table border="0" cellspacing="5" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        iQMedia Password:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="textbox03"></asp:TextBox>
                                                                                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtPassword" runat="server" TargetControlID="txtPassword"
                                                                                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                                InvalidChars="<>">
                                                                                                            </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                            <asp:RequiredFieldValidator ID="rfvPassword" Text="*" ValidationGroup="validateCustomer"
                                                                                                                Display="Dynamic" runat="server" ControlToValidate="txtPassword" ErrorMessage="Please Enter Password."></asp:RequiredFieldValidator>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Phone:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtContactNo" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtContactNo" runat="server" TargetControlID="txtContactNo"
                                                                                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                                InvalidChars="<>">
                                                                                                            </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                            <asp:RegularExpressionValidator ID="regContactNo" Text="*" runat="server" Display="Dynamic"
                                                                                                                ValidationGroup="validateCustomer" ControlToValidate="txtContactNo" ValidationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
                                                                                                                ErrorMessage="Please Enter Valid ContactNo"></asp:RegularExpressionValidator>
                                                                                                            <asp:RequiredFieldValidator ID="rfvContactNo" Text="*" ValidationGroup="validateCustomer"
                                                                                                                Display="Dynamic" runat="server" ControlToValidate="txtContactNo" ErrorMessage="Please Enter ContactNo."></asp:RequiredFieldValidator>
                                                                                                        </label>
                                                                                                    </td>
                                                                                                </tr>
                                                                                                <tr>
                                                                                                    <td class="content-text" width="150px">
                                                                                                        Comments:
                                                                                                    </td>
                                                                                                    <td width="250px">
                                                                                                        <label>
                                                                                                            <asp:TextBox ID="txtComments" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                            <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtComments" runat="server" TargetControlID="txtComments"
                                                                                                                FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                                InvalidChars="<>">
                                                                                                            </AjaxToolkit:FilteredTextBoxExtender>
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
                                                                            <td style="padding-bottom: 2px">
                                                                                <div class="AdminTitle1">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" style="padding-left: 6px">
                                                                                <table border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td class="content-text" width="153px">
                                                                                           <b>Is Multi Login:</b>
                                                                                        </td>
                                                                                        <td>
                                                                                            <asp:CheckBox ID="cbIsMultiLogin" runat="server" />
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="padding-bottom: 2px">
                                                                                <div class="AdminTitle1">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td style="padding-right: 50px">
                                                                                            <table border="0" cellspacing="0" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:DataList ID="rptRoles" runat="server" RepeatColumns="2" CellPadding="0" CellSpacing="0">
                                                                                                            <ItemTemplate>
                                                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                                    <tr height="29px;">
                                                                                                                        <%--  <td width="15" height="100%" valign="top">
                                                                                                                    <%# Container.ItemIndex+1 %>
                                                                                                                </td>--%>
                                                                                                                        <td width="200" valign="middle" height="100%">
                                                                                                                            <asp:Label ID="lblRole" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.RoleName") %>'
                                                                                                                                CssClass="content-text"></asp:Label>
                                                                                                                            <asp:HiddenField ID="hdnValue" runat="server" Value='<%# Bind("RoleID") %>' />
                                                                                                                        </td>
                                                                                                                        <td valign="middle" width="75" height="100%">
                                                                                                                            <asp:CheckBox ID="chkSelectRole" runat="server" />
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </ItemTemplate>
                                                                                                        </asp:DataList>
                                                                                                    </td>
                                                                                                    <td valign="top" class="content-text">
                                                                                                        Default Page:
                                                                                                        <asp:DropDownList ID="ddlDefaultPage" CssClass="content-text" runat="server">
                                                                                                        </asp:DropDownList>
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
                                                                                <div class="AdminTitle1">
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="center">
                                                                                <table border="0" cellspacing="0" cellpadding="0">
                                                                                    <tr>
                                                                                        <td style="padding-right: 50px">
                                                                                            <table border="0" cellspacing="10" cellpadding="0">
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnSave" ValidationGroup="validateCustomer" Width="150px" CssClass="btn-blue2"
                                                                                                            runat="server" Text="Save" OnClick="btnSave_Click" />
                                                                                                    </td>
                                                                                                    <td>
                                                                                                        <asp:Button ID="btnCancel" Width="150px" CssClass="btn-blue2" runat="server" Text="Cancel"
                                                                                                            OnClick="btnCancel_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </div>
                                    <input type="button" id="tgtbtn" runat="server" style="display: none" />
                                    <AjaxToolkit:ModalPopupExtender ID="mdlpopupScreen" BackgroundCssClass="ModalBackground"
                                        BehaviorID="mdlpopupScreen" TargetControlID="tgtbtn" PopupControlID="pnlMailPanel"
                                        runat="server" CancelControlID="btnCancel">
                                    </AjaxToolkit:ModalPopupExtender>
                                </td>
                            </tr>
                            <%--<tr>
                                <td height="10">
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
                            </tr>--%>
                        </table>
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
<%--<asp:UpdateProgress ID="updProgressCustomer" runat="server" AssociatedUpdatePanelID="upCustomer"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtenderCustomer"
    ControlToOverlayID="upCustomer" TargetControlID="updProgressCustomer" CssClass="updateProgress" />
--%>