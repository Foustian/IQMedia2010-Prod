<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CDNUploadClient.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.CDNUploadClient.CDNUploadClient" %>
<style type="text/css">
    .list input
    {
        margin: 1px 5px 2px 1px;
    }
</style>
<script language="javascript" type="text/javascript">
    function CheckUncheckAll(CheckListID) {
        var listBox = document.getElementById(CheckListID);
        var inputItems = listBox.getElementsByTagName("input");
        var Check = inputItems[0].checked;

        for (index = 0; index < inputItems.length; index++) {
            if (inputItems[index].type == 'checkbox') {
                inputItems[index].checked = Check;
            }
        }
    }

    function CheckUncheckGrid() {
        var grid = document.getElementById('<%= gvEnableClient.ClientID %>');
        if (grid) {
            var inputcheckbox = grid.getElementsByTagName('input');
            var Check = inputcheckbox[0].checked;
            for (var i = 0; i < inputcheckbox.length; i++) {
                if (inputcheckbox[i].type == 'checkbox') {
                    inputcheckbox[i].checked = Check;

                }

            }
        }
    }
    function ValidateCheckboxList() {

        var listBox = document.getElementById('<%=chkClientDisable.ClientID %>');
        var inputItems = listBox.getElementsByTagName("input");
        var chkcount = 0;
        for (var i = 0; i < inputItems.length; i++) {
            if (inputItems[i].type == 'checkbox' && inputItems[i].checked == true) {
                chkcount = chkcount + 1;
            }
        }
        if (chkcount <= 0) {
            alert('Please select atleast one client to Enable.');
            return false;
        }
        else {
            return true;
        }
        return false;
    }

    function ValidateGrid() {

        var grid = document.getElementById('<%=gvEnableClient.ClientID%>');
        if (grid) {
            var elements = grid.getElementsByTagName('input');
            var checkcount = 0;
            for (var i = 0; i < elements.length; i++) {
                if (elements[i].type == 'checkbox' && elements[i].id.toString().match('chkSelect') != null && elements[i].checked == true) {
                    checkcount = checkcount + 1;
                }
            }
            if (checkcount <= 0) {
                alert('Please select atleast one Client to Disable.');
                return false;
            }
            else {
                return true;
            }
        }
        return false;
    }

    function SetHeaderCheckBox(gridid) {
        var grid = document.getElementById(gridid);
        if (grid) {
            alert('grid loop starts');
            var elements = grid.getElementsByTagName('input');
            var allchecked = 1;
            var chkheader;

            for (var i = 0; i <= elements.length; i++) {
                if (elements[i].type == 'checkbox' && elements[i].id.toString().match('chkheader') != null) {
                    chkheader = elements[i];

                }
                if (elements[i].type == 'checkbox' && elements[i].checked == false && elements[i].id.toString().match('chkheader') == null) {
                    chkheader.checked = false;
                    allchecked = 0;
                }
                if (elements[i].type == 'checkbox') {
                    alert('allchecked_' + allchecked);
                }

            }
            alert('grid loop end');
            //            if (allchecked == 1) {
            //                chkheader.checked = true;
            //            }

        }
        alert('end');
    }

</script>
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
            <img id="Img1" runat="server" src="~/images/contentbox-l.jpg" width="8" height="1"
                border="0" alt="iQMedia" />
        </td>
        <td valign="top" bgcolor="#FFFFFF">
            <table class="border01" style="padding-left:5px" border="0" align="center" cellpadding="0" cellspacing="0"
                width="100%">
                <tr>
                    <td style="padding-bottom: 5px">
                        <div class="AdminTitle">
                            CDN Upload Client
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Green"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" align="left" border="0" cellpadding="0" cellspacing="3">
                            <tr>
                                <td valign="top" style="width:180px;padding-top:2px;">
                                    Client having CDNUpload Disable:
                                </td>
                                <td>
                                    <div style="height: 130px; overflow: auto; width: 330px; border: solid #D8D5D5 1px;">
                                        <asp:CheckBoxList ValidationGroup="vgSearch" CellPadding="1" CssClass="list" CellSpacing="2"
                                            ID="chkClientDisable" Style="vertical-align: middle;" runat="server">
                                        </asp:CheckBoxList>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnEnable" runat="server" Text="Enable" OnClick="btnEnable_click"
                                        OnClientClick="return ValidateCheckboxList();" CssClass="btn-grey" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="height: 10px">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" align="left" border="0" cellpadding="0" cellspacing="3" >
                            <tr>
                                 <td style="width:180px" >
                                    Client having CDN Upload Enabled :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClientName" runat="server" CssClass="textbox03" AutoCompleteType="Disabled"></asp:TextBox>
                                    <%-- <asp:Image ID="imgArrow" runat="server" Height="15px" o ImageUrl="~/Images/arrow-down-grey.gif" />--%>
                                    <asp:RequiredFieldValidator ID="rf111" Text="*" Display="Dynamic" ValidationGroup="validate"
                                        runat="server" ControlToValidate="txtClientName" ErrorMessage="Please Select Client Name."></asp:RequiredFieldValidator>
                                    <%--<asp:ImageButton runat="server" ID="imgArrow" Width="18px" Height="22px" ImageUrl="~/Images/dropbtn.jpg"
                            OnClientClick="return focustxt();" />--%>
                                    <%--  <AjaxToolkit:AutoCompleteExtender runat="server" ID="AutoCompleteExtender1" ServicePath="~/SearchTerm.asmx"
                            Enabled="true" ServiceMethod="GetCompletionList" MinimumPrefixLength="0" TargetControlID="txtClientName">
                        </AjaxToolkit:AutoCompleteExtender>--%>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" OnClick="btnSearch_click"
                                        CssClass="btn-grey" ValidationGroup="validate" />
                                    <asp:Button ID="btnReseSearch" runat="server" Text="Clear Search" OnClick="btnResetSearch_click"
                                        CssClass="btn-grey" CausesValidation="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">&nbsp;</td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Button ID="btnDisable" runat="server" Text="Disable" OnClick="btnDisable_Click"
                                        OnClientClick="return ValidateGrid();" CssClass="btn-grey" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:GridView ID="gvEnableClient" runat="server" AutoGenerateColumns="false" AutoGenerateEditButton="False"
                                        border="1" BorderColor="#E4E4E4" CellPadding="2" EmptyDataText="No Data Found"
                                        HeaderStyle-CssClass="grid-th" PagerSettings-Mode="NextPrevious" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                        PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" PageSize="50"
                                        OnSorting="gvEnableClient_Sorting" Style="border-collapse: collapse;" Width="515px"
                                        AllowSorting="true">
                                        <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                        <Columns>
                                            <asp:TemplateField ShowHeader="False">
                                                <HeaderTemplate>
                                                    <input type="checkbox" id="chkheader" onclick="CheckUncheckGrid();" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="checkbox" id="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="25%" />
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderText="Client Name" SortExpression="ClientName"
                                                ShowHeader="true">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblClientName" runat="server" ItemStyle-CssClass="content-text" Text='<%# Bind("ClientName") %>'></asp:Label>
                                                    <asp:HiddenField ID="hdnClientKey" runat="server" Value='<%# Bind("ClientKey") %>' />
                                                </ItemTemplate>
                                                <HeaderStyle HorizontalAlign="Left" CssClass="grid-th" />
                                                <ItemStyle CssClass="content-text" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle CssClass="grid-th" />
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <div style="padding-top: 10px; padding-bottom: 10px;" id="divrawmediapaging" runat="server"
                                        visible="false">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    <div>
                                                        <span style="vertical-align: bottom">
                                                            <asp:ImageButton ID="imgPrevious" runat="server" AlternateText="Previous" OnClick="imgPrevious_Click"
                                                                ImageUrl="~/Images/arrow-previous.jpg" />
                                                        </span><span style="vertical-align: top">
                                                            <asp:Label ID="lblCurrentPageNo" Font-Bold="true" runat="server"></asp:Label>
                                                        </span><span style="vertical-align: bottom">
                                                            <asp:ImageButton ID="imgNext" runat="server" AlternateText="Next" OnClick="imgNext_Click"
                                                                ImageUrl="~/Images/arrow-next.jpg" />
                                                        </span>
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    &nbsp;
                                </td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td class="contentbox-r">
            <img id="Img2" runat="server" src="~/images/contentbox-r.jpg" width="8" height="1"
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
