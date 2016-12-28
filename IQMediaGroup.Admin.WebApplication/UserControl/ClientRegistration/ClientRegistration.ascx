<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientRegistration.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.ClientRegistration.ClientRegistration" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script src="../Script/lightbox/jquery.lightbox-0.5.pack.js" type="text/javascript"></script>
    <script src="../Script/lightbox/jquery.lightbox-0.5.js" type="text/javascript"></script>
<script type="text/javascript">
    function focustxt() {
        document.getElementById('ctl00_Content_Data_ClientRegistration1_txtClientName').value = "";
        document.getElementById('ctl00_Content_Data_ClientRegistration1_txtClientName').focus();
        return false;
    }
</script>
<script type="text/javascript">
    //Button will be clicked
    function PopUpButtonSubmit(evt, element) {
        if (evt.keyCode == 13)
            document.getElementById('ctl00_Content_Data_ClientRegistration1_btnSave').click();
    }
</script>
<script type="text/javascript">
    $(function () {
        $('#divHeaderImage a').lightBox();
    });
    $(function () {
        $('#divPlayerLogo a').lightBox();
    });
</script>
<style type="text/css">
    /* jQuery lightBox plugin - Gallery style */
    #gallery
    {
        background-color: #444;
        padding: 10px;
        width: 520px;
    }
    #gallery ul
    {
        list-style: none;
    }
    #gallery ul li
    {
        display: inline;
    }
    #gallery ul img
    {
        border: 5px solid #3e3e3e;
        border-width: 5px 5px 20px;
    }
    #gallery ul a:hover img
    {
        border: 5px solid #fff;
        border-width: 5px 5px 20px;
        color: #fff;
    }
    #gallery ul a:hover
    {
        color: #fff;
    }
    #jquery-overlay
    {
        position: absolute;
        top: 0;
        left: 0;
        z-index: 90;
        width: 100%;
        height: 500px;
        z-index: 99999;
    }
    #jquery-lightbox
    {
        position: absolute;
        top: 0;
        left: 0;
        width: 100%;
        z-index: 100;
        text-align: center;
        line-height: 0;
        z-index: 999999;
    }
    #jquery-lightbox a img
    {
        border: none;
    }
    #lightbox-container-image-box
    {
        position: relative;
        background-color: #fff;
        width: 250px;
        height: 250px;
        margin: 0 auto;
    }
    #lightbox-container-image
    {
        padding: 10px;
    }
    #lightbox-loading
    {
        position: absolute;
        top: 40%;
        left: 0%;
        height: 25%;
        width: 100%;
        text-align: center;
        line-height: 0;
    }
    #lightbox-nav
    {
        position: absolute;
        top: 0;
        left: 0;
        height: 100%;
        width: 100%;
        z-index: 10;
    }
    #lightbox-container-image-box > #lightbox-nav
    {
        left: 0;
    }
    #lightbox-nav a
    {
        outline: none;
    }
    #lightbox-nav-btnPrev, #lightbox-nav-btnNext
    {
        width: 49%;
        height: 100%;
        zoom: 1;
        display: block;
    }
    #lightbox-nav-btnPrev
    {
        left: 0;
        float: left;
    }
    #lightbox-nav-btnNext
    {
        right: 0;
        float: right;
    }
    #lightbox-container-image-data-box
    {
        font: 10px Verdana, Helvetica, sans-serif;
        background-color: #fff;
        margin: 0 auto;
        line-height: 1.4em;
        overflow: auto;
        width: 100%;
        padding: 0 10px 0;
    }
    #lightbox-container-image-data
    {
        padding: 0 10px;
        color: #666;
    }
    #lightbox-container-image-data #lightbox-image-details
    {
        width: 70%;
        float: left;
        text-align: left;
    }
    #lightbox-image-details-caption
    {
        font-weight: bold;
    }
    #lightbox-image-details-currentNumber
    {
        display: block;
        clear: left;
        padding-bottom: 1.0em;
    }
    #lightbox-secNav-btnClose
    {
        width: 66px;
        float: right;
        padding-bottom: 0.7em;
    }
</style>
<%--<style type="text/css">
    .EditCombobox
    {
      position: relative !important; 
      float:left;
      clear:right;
    }
    .EditCombobox ul.ajax__combobox_itemlist
    {
        position: absolute !important;
        top: 0px !important;
        left: 0px !important;
        margin-top:22px;
        clear:left
    }
</style>--%>
<style type="text/css">
    .EditCombobox
    {
        position: relative !important;
    }
    .EditCombobox ul.ajax__combobox_itemlist
    {
        position: absolute !important;
        top: 22px !important;
        left: 0px !important;
    }
    @media screen and (-webkit-min-device-pixel-ratio:0)
    {
        .EditCombobox ul.ajax__combobox_itemlist
        {
            top: 0px !important;
        }
    }
    
    /*#pnlMailPanel {
	width: 100%;
	overflow: auto;
	overflow-y: hidden;
	margin-right: 17px;
    }
    *+html #pnlMailPanel {
	width: auto;
	padding-right: 17px;*/
</style>
<table width="100%" border="0" cellpadding="0" cellspacing="0">
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
            <img runat="server" src="~/images/contentbox-l.jpg" width="8" height="1" border="0"
                alt="iQMedia" />
        </td>
        <td valign="top" bgcolor="#FFFFFF">
            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <table align="center" border="0" cellpadding="0" cellspacing="6" class="border01"
                            width="100%">
                            <tr>
                                <td style="padding-bottom: 5px">
                                    <div class="AdminTitle">
                                        Client Registration</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="3" width="100%">
                                        <tr>
                                            <td width="2%">
                                            </td>
                                            <td align="left" valign="top" width="98%">
                                                <asp:ValidationSummary ID="vsIQMediaSearch" runat="server" EnableClientScript="true"
                                                    ForeColor="#bd0000" ValidationGroup="validate" />
                                                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Visible="false"></asp:Label>
                                                <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Panel ID="mainPanel" runat="server" DefaultButton="btnSearch">
                                        <table border="0" cellpadding="0" cellspacing="5">
                                            <tr>
                                                <td class="content-text">
                                                    Search Client:
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtClientName" runat="server" CssClass="textbox03" AutoCompleteType="Disabled"></asp:TextBox>
                                                    <%-- <asp:Image ID="imgArrow" runat="server" Height="15px" o ImageUrl="~/Images/arrow-down-grey.gif" />--%>
                                                    <asp:RequiredFieldValidator ID="rf111" Text="*" Display="Dynamic" ValidationGroup="validate"
                                                        runat="server" ControlToValidate="txtClientName" ErrorMessage="Please Select Client Name."></asp:RequiredFieldValidator>
                                                    <asp:ImageButton runat="server" ID="imgArrow" Width="18px" Height="22px" ImageUrl="~/Images/dropbtn.jpg"
                                                        OnClientClick="return focustxt();" />
                                                    <AjaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" ServicePath="~/SearchTerm.asmx"
                                                        Enabled="true" ServiceMethod="GetCompletionList" MinimumPrefixLength="0" TargetControlID="txtClientName">
                                                    </AjaxToolkit:AutoCompleteExtender>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Button ID="btnSearch" runat="server" CssClass="btn-blue" OnClick="btnSearch_Click"
                                                            Text="Search" ValidationGroup="validate" />
                                                    </label>
                                                </td>
                                                <td>
                                                    <label>
                                                        <asp:Button ID="btnAdd" runat="server" CssClass="btn-blue" OnClick="btnAdd_Click"
                                                            Text="Add" />
                                                    </label>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table align="center" border="0" cellpadding="0" cellspacing="0" width="100%">
                            <%--<tr>
                                <td height="10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    </table>
                                </td>
                            </tr>--%>
                            <tr>
                                <td height="3">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div style="padding: 5px 5px 5px 1px;">
                                        <asp:Label ID="lblErrorMsg" ForeColor="Red" Font-Size="Small" runat="server"></asp:Label>
                                    </div>
                                    <table bordercolor="#e4e4e4" cellpadding="5" cellspacing="0" style="border: solid 1px #e4e4e4;
                                        border-collapse: collapse;" width="100%">
                                        <tr>
                                            <td style="padding-bottom: 10px">
                                                <div class="AdminTitle">
                                                    Clients</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <%-- <asp:Panel ID="Panel1" runat="server"></asp:Panel>--%>
                                                <asp:GridView ID="gvClient" runat="server" AllowPaging="True" AutoGenerateColumns="true"
                                                    AutoGenerateEditButton="False" border="1" BorderColor="#E4E4E4" CellPadding="5"
                                                    EmptyDataText="No Data Found" HeaderStyle-CssClass="grid-th" OnPageIndexChanging="gvClient_PageIndexChanging"
                                                    PagerSettings-Mode="NextPrevious" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                    PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" PageSize="15"
                                                    Style="border-collapse: collapse;" Width="100%" OnRowDataBound="gvClient_OnRowDataBound" >
                                                    <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                                    <Columns>
                                                        <asp:TemplateField ShowHeader="False">
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="lbtnEdit" runat="server" CausesValidation="False" OnClick="lbtnEdit_Click"
                                                                    CssClass="btn-edit" Text="&nbsp;" CommandArgument='<%# Bind("ClientKey") %>'></asp:LinkButton>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Client Name">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblClientName" runat="server" ItemStyle-CssClass="content-text" Text='<%# Bind("ClientName") %>'></asp:Label>
                                                                <asp:HiddenField ID="hdnClientKey" runat="server" Value='<%# Bind("ClientKey") %>' />
                                                            </ItemTemplate>
                                                            <HeaderStyle HorizontalAlign="Left" />
                                                            <ItemStyle CssClass="content-text" Width="25%" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <HeaderStyle CssClass="grid-th" />
                                                </asp:GridView>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <%--<tr>
                                <td height="35">
                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="right" class="contenttext-small" valign="top">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div onkeydown="PopUpButtonSubmit(event, this);">
                            <asp:Panel ID="pnlMailPanel" Style="vertical-align: middle; min-height: 500px; max-height: 660px;"
                                DefaultButton="btnSave" runat="server" ScrollBars="Auto">
                                <table border="0" style="vertical-align: middle;" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td valign="top" bgcolor="#FFFFFF">
                                            <table border="0" align="center" style="vertical-align: middle;" cellpadding="0"
                                                cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table width="100%" style="vertical-align: middle" border="0" align="center" cellpadding="0"
                                                            cellspacing="3">
                                                            <tr>
                                                                <td style="padding-bottom: 2px">
                                                                    <div class="AdminTitle">
                                                                        Client Setup Page</div>
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
                                                                                    ValidationGroup="validateClient" ForeColor="#bd0000" />
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
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Client Name:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtClientNameAdd" MaxLength="50" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="ftbEtxtFirstName" runat="server" TargetControlID="txtClientNameAdd"
                                                                                                    FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtClientNameAdd" Text="*" Display="Dynamic" ValidationGroup="validateClient"
                                                                                                    runat="server" ControlToValidate="txtClientNameAdd" ErrorMessage="Please Enter Client Name."></asp:RequiredFieldValidator>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Master Client:
                                                                                        </td>
                                                                                        <td align="left" valign="top" style="vertical-align: top;">
                                                                                            <AjaxToolkit:ComboBox ID="ddlMasterClient" runat="server" CssClass="EditCombobox"
                                                                                                RenderMode="Inline" AutoPostBack="false" DropDownStyle="DropDown" AutoCompleteMode="Suggest"
                                                                                                CaseSensitive="false">
                                                                                            </AjaxToolkit:ComboBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td style="padding-right: 50px" valign="top">
                                                                                <table border="0" cellspacing="5" cellpadding="0">
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Setup Date:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtSetupDate" ReadOnly="true" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            IsActive
                                                                                        </td>
                                                                                        <td align="left">
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
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Address1:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtAddress1" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server"
                                                                                                    TargetControlID="txtAddress1" FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtAddress1" Text="*" Display="Dynamic" ValidationGroup="validateClient"
                                                                                                    runat="server" ControlToValidate="txtAddress1" ErrorMessage="Please Enter Address1."></asp:RequiredFieldValidator>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Address2:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <asp:TextBox ID="txtAddress2" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            City:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtCity" runat="server" MaxLength="50" CssClass="textbox03"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server"
                                                                                                    TargetControlID="txtCity" FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtCity" Text="*" Display="Dynamic" ValidationGroup="validateClient"
                                                                                                    runat="server" ControlToValidate="txtCity" ErrorMessage="Please Enter City."></asp:RequiredFieldValidator>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            State:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <asp:DropDownList ID="drpState" Width="140px" runat="server" CssClass="dropdown01">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="reqState" runat="server" ControlToValidate="drpState"
                                                                                                ErrorMessage="Please Select State" Text="*" ValidationGroup="validateClient"
                                                                                                Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Zip:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtZip" MaxLength="5" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server"
                                                                                                    TargetControlID="txtZip" FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtZip" Text="*" Display="Dynamic" ValidationGroup="validateClient"
                                                                                                    runat="server" ControlToValidate="txtZip" ErrorMessage="Please Enter Zip."></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic"
                                                                                                    ValidationGroup="validateClient" Text="*" ErrorMessage="Please enter valid Zip."
                                                                                                    ValidationExpression="\d{5}(-\d{4})?" ControlToValidate="txtZip"></asp:RegularExpressionValidator>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td style="padding-right: 50px" valign="top">
                                                                                <table border="0" cellspacing="5" cellpadding="0">
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Attention:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtAttention" MaxLength="50" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender4" runat="server"
                                                                                                    TargetControlID="txtAttention" FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" Display="Dynamic"
                                                                                                    ValidationGroup="validateClient" runat="server" ControlToValidate="txtAttention"
                                                                                                    ErrorMessage="Please Enter Attention."></asp:RequiredFieldValidator>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Phone:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtPhone" MaxLength="15" runat="server" CssClass="textbox03"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender6" runat="server"
                                                                                                    TargetControlID="txtPhone" FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtPhone" Text="*" Display="Dynamic" ValidationGroup="validateClient"
                                                                                                    runat="server" ControlToValidate="txtPhone" ErrorMessage="Please Enter Phone."></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="regContactNo" Text="*" runat="server" Display="Dynamic"
                                                                                                    ValidationGroup="validateClient" ControlToValidate="txtPhone" ValidationExpression="^[01]?[- .]?(\([2-9]\d{2}\)|[2-9]\d{2})[- .]?\d{3}[- .]?\d{4}$"
                                                                                                    ErrorMessage="Please Enter Valid Phone"></asp:RegularExpressionValidator>
                                                                                            </label>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Industry:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <asp:DropDownList ID="drpIndustry" Width="140px" runat="server" CssClass="dropdown01">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="drpIndustry"
                                                                                                ErrorMessage="Please Select Industry." Text="*" ValidationGroup="validateClient"
                                                                                                Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
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
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Bill Type:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <asp:DropDownList ID="drpBillType" Width="140px" runat="server" CssClass="dropdown01">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="drpBillType"
                                                                                                ErrorMessage="Please Select Bill Type." Text="*" ValidationGroup="validateClient"
                                                                                                Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Bill Frequency:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <asp:DropDownList ID="drpBillFrequency" Width="140px" runat="server" CssClass="dropdown01">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="drpBillFrequency"
                                                                                                ErrorMessage="Please Select Bill Frequency." Text="*" ValidationGroup="validateClient"
                                                                                                Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                </table>
                                                                            </td>
                                                                            <td style="padding-right: 50px" valign="top">
                                                                                <table border="0" cellspacing="5" cellpadding="0">
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            Pricing Code:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <asp:DropDownList ID="drpPricingCode" Width="140px" runat="server" CssClass="dropdown01">
                                                                                            </asp:DropDownList>
                                                                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="drpPricingCode"
                                                                                                ErrorMessage="Please Select Pricing Code." Text="*" ValidationGroup="validateClient"
                                                                                                Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                                                                        </td>
                                                                                    </tr>
                                                                                    <tr>
                                                                                        <td align="left" class="content-text" width="150px">
                                                                                            No of Users:
                                                                                        </td>
                                                                                        <td align="left" width="250px">
                                                                                            <label>
                                                                                                <asp:TextBox ID="txtNoofUsers" runat="server" CssClass="textbox03" Text="1"></asp:TextBox>
                                                                                                <AjaxToolkit:FilteredTextBoxExtender ID="FilteredTextBoxExtender8" runat="server"
                                                                                                    TargetControlID="txtNoofUsers" FilterMode="InvalidChars" FilterType="Numbers,UppercaseLetters,LowercaseLetters,Custom"
                                                                                                    InvalidChars="<>">
                                                                                                </AjaxToolkit:FilteredTextBoxExtender>
                                                                                                <asp:RequiredFieldValidator ID="rfvtxtNoofUsers" Text="*" Display="Dynamic" ValidationGroup="validateClient"
                                                                                                    runat="server" ControlToValidate="txtNoofUsers" ErrorMessage="Please Enter No of Users."></asp:RequiredFieldValidator>
                                                                                                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtNoofUsers"
                                                                                                    ErrorMessage="Please Enter Valid No of Users." Text="*" ValidationExpression="^\d+$"
                                                                                                    ValidationGroup="validateClient"></asp:RegularExpressionValidator>
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
                                                            <tr align="center">
                                                                <td>
                                                                    <table border="0" cellspacing="10" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                Custom Header:
                                                                            </td>
                                                                            <td>
                                                                                <div id="divHeaderImage">
                                                                                    <a href="" title="View" runat="server" id="aHeaderImage" visible="false">View</a>
                                                                                </div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CheckBox ID="chkIsCustomHeader" runat="server" Text="&nbsp;&nbsp;Show Image On Header" />
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:FileUpload ID="fuCustomHeaderImage" runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                Player Logo:
                                                                            </td>
                                                                            <td>
                                                                                <div id="divPlayerLogo">
                                                                                    <a href="" title="View" runat="server" id="aPlayerLogo" visible="false">View</a>
                                                                                </div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <asp:CheckBox ID="chkIsPlayerLogo" runat="server" Text="&nbsp;&nbsp;Show Logo On Player" />
                                                                            </td>
                                                                            <td align="right">
                                                                                <asp:FileUpload ID="fuPlayerLogo" runat="server" />
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
                                                                                             <asp:HiddenField ID="hfHeaderImage" runat="server" />
                                                                                            <asp:HiddenField ID="hfPlayerLogo" runat="server" />
                                                                                            <asp:HiddenField ID="hfCreatedHeaderImage" runat="server" />
                                                                                            <asp:HiddenField ID="hfCreatePlayerLogo" runat="server" />
                                                                                            <asp:DataList ID="rptRoles" runat="server" RepeatColumns="2" CellPadding="0" CellSpacing="0">
                                                                                                <ItemTemplate>
                                                                                                    <table cellpadding="5" cellspacing="0" width="100%">
                                                                                                        <tr height="29px;">
                                                                                                            <%--  <td width="15" height="100%" valign="top">
                                                                                                                    <%# Container.ItemIndex+1 %>
                                                                                                                </td>--%>
                                                                                                            <td width="200" height="100%" valign="middle">
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
                                                                                <table border="0" cellspacing="10" cellpadding="0">
                                                                                    <tr>
                                                                                        <td>
                                                                                            <asp:Button ID="btnSave" ValidationGroup="validateClient" Width="150px" CssClass="btn-blue2"
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
            </table>
        </td>
        <td class="contentbox-r">
            <img runat="server" src="~/images/contentbox-r.jpg" width="8" height="1" border="0"
                alt="iQMedia" />
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
<%--<asp:UpdateProgress ID="updProgressClient" runat="server" AssociatedUpdatePanelID="upClient"
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
--%>