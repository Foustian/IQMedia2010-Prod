<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyClips.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.MyClips.MyClips" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="../CustomPager/CustomPager.ascx" TagName="CustomPager" TagPrefix="uc" %>
<script language="javascript" type="text/javascript">
    function UpdateSubCategory1(ddl_id) {


        var PCatId = "<%= ddlPCategory.ClientID %>";
        var SubCat1Id = "<%= ddlSubCategory1.ClientID %>";
        var SubCat2Id = "<%= ddlSubCategory2.ClientID %>";
        var SubCat3Id = "<%= ddlSubCategory3.ClientID %>";


        var PCatSelectedValue = $("#" + PCatId + "").val();
        var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
        var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
        var Cat3SelectedValue = $("#" + SubCat3Id + "").val();
        document.getElementById("<%=lblClipMsg.ClientID %>").innerHTML = "";

        if (ddl_id == PCatId) {

            $("#" + SubCat1Id + " option").removeAttr("disabled");
            $("#" + SubCat2Id + " option").removeAttr("disabled");
            $("#" + SubCat3Id + " option").removeAttr("disabled");

            if (PCatSelectedValue != 0) {
                $("#" + SubCat1Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
                $("#" + SubCat2Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");


                if (PCatSelectedValue == Cat1SelectedValue) {
                    $("#" + SubCat1Id + "").val(0);
                    $("#" + SubCat2Id + "").val(0);
                    $("#" + SubCat3Id + "").val(0);
                }
                else if (PCatSelectedValue == Cat2SelectedValue) {
                    $("#" + SubCat2Id + "").val(0);
                    $("#" + SubCat3Id + "").val(0);
                }
                else if (PCatSelectedValue == Cat3SelectedValue) {
                    $("#" + SubCat3Id + "").val(0);
                }


                if ($("#" + SubCat1Id + "").val() != 0) {
                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                }

                if ($("#" + SubCat2Id + "").val() != 0) {
                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                }

            }
            else {
                $("#" + SubCat1Id + "").val(0);
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
            }
        }
        else if (ddl_id == SubCat1Id) {
            if (PCatSelectedValue != 0) {
                $("#" + SubCat2Id + " option").removeAttr("disabled");
                $("#" + SubCat3Id + " option").removeAttr("disabled");

                $("#" + SubCat2Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");

                if (Cat1SelectedValue != 0) {

                    $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                    if (Cat1SelectedValue == Cat2SelectedValue) {
                        $("#" + SubCat2Id + "").val(0);
                        $("#" + SubCat3Id + "").val(0);
                    }
                    else if (Cat1SelectedValue == Cat3SelectedValue) {
                        $("#" + SubCat3Id + "").val(0);
                    }

                    if ($("#" + SubCat1Id + "").val() != 0) {
                        $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                        $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    }

                    if ($("#" + SubCat2Id + "").val() != 0) {
                        $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                    }
                }
                else {
                    $("#" + SubCat2Id + "").val(0);
                    $("#" + SubCat3Id + "").val(0);
                }

            }
            else {
                $("#" + SubCat1Id + "").val(0);
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
                document.getElementById("<%=lblClipMsg.ClientID %>").innerHTML = "Please first select all preceding categories.";
            }
        }
        else if (ddl_id == SubCat2Id) {
            if (PCatSelectedValue != 0 && Cat1SelectedValue != 0) {
                $("#" + SubCat3Id + " option").removeAttr("disabled");

                $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");

                if (Cat2SelectedValue != 0) {

                    $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");

                    if (Cat2SelectedValue == Cat3SelectedValue) {
                        $("#" + SubCat3Id + "").val(0);
                    }

                    if ($("#" + SubCat1Id + "").val() != 0) {
                        $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                        $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                    }

                    if ($("#" + SubCat2Id + "").val() != 0) {
                        $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
                    }
                }
                else {
                    $("#" + SubCat3Id + "").val(0);
                }
            }
            else {
                $("#" + SubCat2Id + "").val(0);
                $("#" + SubCat3Id + "").val(0);
                document.getElementById("<%=lblClipMsg.ClientID %>").innerHTML = "Please first select all preceding categories.";
            }
        }
        else if (ddl_id == SubCat3Id) {
            if (PCatSelectedValue == 0 || Cat1SelectedValue == 0 || Cat2SelectedValue == 0) {
                $("#" + SubCat3Id + "").val(0);
                document.getElementById("<%=lblClipMsg.ClientID %>").innerHTML = "Please first select all preceding categories.";
            }
        }
    }
    function UpdateSubCategory2(ddl_id) {
        var PCatId = "<%= ddlPCategory.ClientID %>";
        var SubCat1Id = "<%= ddlSubCategory1.ClientID %>";
        var SubCat2Id = "<%= ddlSubCategory2.ClientID %>";
        var SubCat3Id = "<%= ddlSubCategory3.ClientID %>"

        var PCatSelectedValue = $("#" + PCatId + "").val();
        var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
        var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
        var Cat3SelectedValue = $("#" + SubCat3Id + "").val();

        if (ddl_id == PCatId) {
            if (PCatSelectedValue != 0) {
                $("#" + SubCat1Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
                $("#" + SubCat2Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + PCatSelectedValue + "']").attr("disabled", "disabled");
            }
        }
        else if (ddl_id == SubCat1Id) {
            if (Cat1SelectedValue != 0) {
                $("#" + SubCat2Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat2Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
            }
        }
        else if (ddl_id == SubCat2Id) {
            if (Cat2SelectedValue != 0) {
                $("#" + SubCat3Id + " option[value='" + $("#" + PCatId + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat1Id + "").val() + "']").attr("disabled", "disabled");
                $("#" + SubCat3Id + " option[value='" + $("#" + SubCat2Id + "").val() + "']").attr("disabled", "disabled");
            }
        }
    }
    function ValidateGrid() {
        var grid = document.getElementById('<%=grvClip.ClientID%>');
        if (grid) {
            var elements = grid.getElementsByTagName('input');
            var checkcount = 0;
            for (var i = 0; i < elements.length; i++) {
                if (elements[i].type == 'checkbox' && elements[i].id.toString().match('chkDelete') != null && elements[i].checked == true) {
                    checkcount = checkcount + 1;
                }
            }
            if (checkcount > 0) {
                return confirm('Are you sure to delete record(s) ? ');
            }
            else {
                alert('Please select atleast one record to delete.');
                return false;
            }
        }
        return false;
    }
</script>
<script language="javascript" type="text/javascript">
    function ValidateEmailGrid() {
        var grid = document.getElementById('<%=grvClip.ClientID%>');
        if (grid) {
            var elements = grid.getElementsByTagName('input');
            var checkcount = 0;
            for (var i = 0; i < elements.length; i++) {
                if (elements[i].type == 'checkbox' && elements[i].id.toString().match('chkDelete') != null && elements[i].checked == true) {
                    checkcount = checkcount + 1;
                }
            }
            if (checkcount <= 0) {
                alert('Please select atleast one record to Email.');
                return false;
            }
            else {
                return true;
            }
        }
        return false;
    }
</script>
<script language="javascript" type="text/javascript">
    function showdivSearch() {
        $("#<%=DivSearch.ClientID %>").show('slow');
    }
    function hidedivSearch() {
        $("#<%=DivSearch.ClientID %>").hide('slow');
        (document.getElementById('<%=DivSearch.ClientID %>')).setAttribute('enable', 'true');
    }
    function SetSearchParam() {
        var isAll = false;
        $("#divSearch input:checkbox").each(function () {

            if ($(this).is(":checked")) {

                if ($(this).val() == "all") {
                    isAll = true;
                }
            }
        });
        if (isAll) {
            $("#divSearch input:checkbox").each(function () {
                if ($(this).val() != "all") {

                    $(this).attr("checked", "checked");
                    $(this).attr("disabled", "disabled");
                }
            });
        }
        else {
            $("#divSearch input:checkbox").each(function () {
                if ($(this).val() != "all") {
                    $(this).removeAttr("disabled");
                }
            });
        }
    }
    function CheckUncheckAll(CheckListID) {
        //alert('CheckUncheckAll')
        var listBox = document.getElementById(CheckListID);
        var inputItems = listBox.getElementsByTagName("input");
        var Check = inputItems[0].checked;

        for (index = 0; index < inputItems.length; index++) {
            if (inputItems[index].type == 'checkbox') {
                inputItems[index].checked = Check;
            }
        }
    }
    function setCheckbox(chkid, inputid) {
        //alert('setCheckbox')
        var chk = document.getElementById(chkid);
        var options = chk.getElementsByTagName("input");
        var labels = chk.getElementsByTagName("label");
        document.getElementById(inputid).value = "";
        var allChecked = 1;


        for (var i = 0; i < options.length; i++) {
            if (options[i].checked) {
                document.getElementById(inputid).value += labels[i].innerHTML + " + ";
            }
            else {
                if (i != 0) {
                    allChecked = 0;
                    options[0].checked = false;
                    document.getElementById(inputid).value = document.getElementById(inputid).value.replace("All + ", "");
                }
            }
        }
        if (allChecked) {
            options[0].checked = true;
            document.getElementById(inputid).value = "All";
        }

        if (document.getElementById(inputid).value.indexOf("+ ") > 0) {
            document.getElementById(inputid).value = document.getElementById(inputid).value.substr(0, document.getElementById(inputid).value.length - 2);
        }
    }

</script>
<div class="show-bg">
    <div class="show-hide">
        <div class="float-left" onclick="showdivPlayer();">
            <a href="javascript:;">
                <img src="../images/show.png" alt="">
                SHOW PLAYER</a></div>
        <div class="float-right" onclick="hidedivPlayer();">
            <a href="javascript:;">HIDE
                <img src="../images/hiden.png" alt=""></a></div>
    </div>
    <div id="divVideo" class="divVideo">
        <asp:UpdatePanel ID="upClipPlayer" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div style="text-align: center;" align="center">
                    <div align="center">
                        <%--  <asp:UpdatePanel ID="upClipPlayer" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                        <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server" visible="false"
                            align="center">
                        </div>
                        <asp:Label ID="lblTimeOutPlayer" runat="server" Style="display: none; padding-top: 10px"></asp:Label>
                        <div class="show-hide" id="divCaptionShowHide" style="margin-top: 6px; display: none;">
                            <div class="float-left" onclick="showdivCaption();">
                                <a href="javascript:;">
                                    <img src="../images/show.png" alt="">
                                    SHOW CAPTION</a></div>
                            <div class="float-right" onclick="hidedivCaption();">
                                <a href="javascript:;">HIDE <a href="../InlineMediaWorkspace/">../InlineMediaWorkspace/</a>
                                    <img src="../images/hiden.png" alt=""></a></div>
                        </div>
                        <%-- </ContentTemplate>
                </asp:UpdatePanel>--%>
                    </div>
                </div>
                <div id="divClosedCaption" runat="server" style="display: none">
                    <table style="width: 100%;" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="63" class="grey-grad">
                                <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td align="left" class="heading-blue2">
                                            Closed Caption:
                                            <br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="content-text-grey">
                                            <div id="DivCaption" runat="server" class="panel" style="height: 242px; overflow-y: auto;">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<div class="show-bg">
    <%--<div class="show-hide">
            <div class="float-left" onclick="showdivSearch();">
                <a href="javascript:;" >
                    <img src="../images/show.png" alt="">
                    SHOW SEARCH</a></div>
            <div>
                <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
            </div>
            <div class="float-right" onclick="hidedivSearch();">
                <a href="javascript:;">HIDE
                    <img src="../images/hiden.png" alt=""></a></div>
        </div>--%>
    <div id="divMainSearch">
        <div style="text-align: center;">
            <asp:UpdatePanel ID="upBtnSearch" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div class="show-hide">
                        <div class="float-left" onclick="showdivSearch();">
                            <a href="javascript:;">
                                <img src="../images/show.png" alt="">
                                SHOW SEARCH</a></div>
                        <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
                        <div class="float-right" onclick="hidedivSearch();">
                            <a href="javascript:;">HIDE
                                <img src="../images/hiden.png" alt=""></a></div>
                    </div>
                    <table id="tblSearch" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%">
                        <%--<tr>
                                <td height="25" class="hdbar">
                                    <table style="padding-left: 10px; width: 100%;" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td align="left" style="width: 420px;">
                                                <div class="show" style="cursor: pointer;" onclick="showdivSearch();">
                                                    SHOW SEARCH</div>
                                            </td>
                                            <td align="left">
                                                <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
                                            </td>
                                            <td align="right" style="padding-right: 10px">
                                                <div class="hide" style="cursor: pointer;" onclick="hidedivSearch();">
                                                    HIDE</div>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>--%>
                        <tr>
                            <td>
                                <div id="DivSearch" runat="server" style="width: 100%; overflow: hidden; display: none;">
                                    <table border="0" width="100%" align="center" cellpadding="0" cellspacing="6" class="border01">
                                        <tr>
                                            <td>
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <table border="0" align="left" cellpadding="0" width="100%" cellspacing="6" class="border01">
                                                                <tr>
                                                                    <td>
                                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                                            <tr>
                                                                                <td style="width: 180px" align="center" valign="top">
                                                                                    <br />
                                                                                    <br />
                                                                                    <br />
                                                                                    <asp:Button ID="btnSearch" ValidationGroup="vgSearch" OnClick="btnSearch_Click" Width="120px"
                                                                                        runat="server" Text="Find Clips" CssClass="btn-blue2" /><br />
                                                                                    <br />
                                                                                    <asp:Button ID="btnReset" OnClick="btnReset_Click" Width="120px" runat="server" Text="Reset"
                                                                                        CssClass="btn-blue2" />
                                                                                </td>
                                                                                <td>
                                                                                    <table cellpadding="5" cellspacing="0" width="100%" align="left">
                                                                                        <tr>
                                                                                            <td colspan="3">
                                                                                                <asp:Label ID="lblSearchErr" runat="server" ForeColor="Red"></asp:Label>
                                                                                                <asp:ValidationSummary ID="vlsSearch" runat="server" ValidationGroup="vgSearch" />
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" style="width: 116px">
                                                                                                Search Term:
                                                                                            </td>
                                                                                            <td colspan="2" align="left">
                                                                                                <asp:TextBox CssClass="grayinput" ValidationGroup="vgSearch" ID="txtSearch" runat="server"
                                                                                                    Width="578px"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td colspan="2" align="left">
                                                                                                <div id="divSearch">
                                                                                                    <table>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <input type="checkbox" id="cbAll" runat="server" value="all" checked="checked" onclick="SetSearchParam();" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                All
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                &nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <input type="checkbox" id="cbTitle" runat="server" value="title" checked="checked" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                Title
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                &nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <input type="checkbox" id="cbDescription" runat="server" value="description" checked="checked" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                Description
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                &nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <input type="checkbox" id="cbKeywords" runat="server" value="keywords" checked="checked" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                Keywords
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                &nbsp;&nbsp;
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                <input type="checkbox" id="cbCC" runat="server" value="cc" checked="checked" />
                                                                                                            </td>
                                                                                                            <td>
                                                                                                                Closed Caption
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top">
                                                                                                Clip Creation Date:
                                                                                            </td>
                                                                                            <td colspan="2" align="left" class="content-text1">
                                                                                                <div style="width: 190px; float: left;">
                                                                                                    From Date:<br />
                                                                                                    <asp:TextBox ValidationGroup="vgSearch" ID="txtFromDate" runat="server" AutoCompleteType="None"
                                                                                                        CssClass="grayinput"></asp:TextBox>
                                                                                                    <AjaxToolkit:CalendarExtender ID="CalEtxtFromDate" runat="server" CssClass="MyCalendar"
                                                                                                        TargetControlID="txtFromDate">
                                                                                                    </AjaxToolkit:CalendarExtender>
                                                                                                </div>
                                                                                                <div style="width: 190px; float: left;">
                                                                                                    To Date:<br />
                                                                                                    <asp:TextBox ValidationGroup="vgSearch" ID="txtToDate" runat="server" AutoCompleteType="None"
                                                                                                        CssClass="grayinput"></asp:TextBox>
                                                                                                    <AjaxToolkit:CalendarExtender ID="valEtxtToDate" runat="server" CssClass="MyCalendar"
                                                                                                        TargetControlID="txtToDate">
                                                                                                    </AjaxToolkit:CalendarExtender>
                                                                                                    <asp:CompareValidator ValidationGroup="vgSearch" ID="cmpDateValidator" runat="server"
                                                                                                        ControlToCompare="txtToDate" ControlToValidate="txtFromDate" Text="*" ErrorMessage="To Date Must be greater than From Date"
                                                                                                        Operator="LessThanEqual" Type="Date" Display="Dynamic">
                                                                                                    </asp:CompareValidator>
                                                                                                </div>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top">
                                                                                                Owner:
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                                    <asp:CheckBoxList ValidationGroup="vgSearch" CellPadding="1" CellSpacing="1" ID="chkOwnerList"
                                                                                                        Style="vertical-align: middle;" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </div>
                                                                                            </td>
                                                                                            <td align="left" valign="top" class="content-text1">
                                                                                                Currently Selected Owners:<br />
                                                                                                <asp:TextBox CssClass="textbox03" ID="txtOwnerSelection" ReadOnly="true" runat="server"
                                                                                                    TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top">
                                                                                                Category Selection 1:
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                                    <asp:CheckBoxList ValidationGroup="vgSearch" ID="chkCategories1" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </div>
                                                                                            </td>
                                                                                            <td align="left" valign="top" class="content-text1">
                                                                                                Currently Selected Category Selection 1:<br />
                                                                                                <asp:TextBox CssClass="textbox03" ID="txtCat1Selection" ReadOnly="true" runat="server"
                                                                                                    TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td align="left" valign="top">
                                                                                                Category Selection 2:
                                                                                            </td>
                                                                                            <td align="left">
                                                                                                <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                                    <asp:CheckBoxList ValidationGroup="vgSearch" ID="chkCategories2" runat="server">
                                                                                                    </asp:CheckBoxList>
                                                                                                </div>
                                                                                            </td>
                                                                                            <td align="left" valign="top" class="content-text1">
                                                                                                Currently Selected Category Selection 2:<br />
                                                                                                <asp:TextBox CssClass="textbox03" ID="txtCat2Selection" ReadOnly="true" runat="server"
                                                                                                    TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
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
                                </div>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                    <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
        </div>
    </div>
</div>
<div class="grid-bg">
    <div class="result-bg">
        <div class="float-left">
            Clip Results</div>
    </div>
    <div class="result-in-bg">
        <asp:UpdatePanel ID="upMainGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                    <tr>
                        <td>
                            <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td align="left">
                                        <asp:Button ID="btnRefreshLibrary" runat="server" Text="Refresh Library" CssClass="btn-blue2"
                                            OnClick="btnRefreshLibrary_Click" />
                                        <asp:Button ID="btnClipDownload" runat="server" Visible="false" CssClass="btn-blue2"
                                            Text="Download Clips" OnClick="btnClipDownload_Click" />
                                        <asp:Button ID="lnkEmail" runat="server" Text="Email Selected Clip(s)" OnClick="lnkEmail_Click"
                                            CssClass="btn-blue2" OnClientClick="return ValidateEmailGrid();" />
                                        <asp:Button ID="btnManageCategories" CssClass="btn-blue2" runat="server" Text="Manage Categories"
                                            OnClick="btnManageCategories_Click" />
                                        <asp:Button ID="btnRemoveClips" runat="server" Text="Remove Selected Clip(s)" OnClientClick="return ValidateGrid();"
                                            CssClass="btn-blue2" Style="text-align: right" OnClick="btnRemoveClips_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green"></asp:Label>
                            <asp:Label ID="lblErrorClips" runat="server" ForeColor="Red"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HiddenField ID="hfdDdlUser" runat="server" />
                            <div style="padding-top: 10px;">
                                <asp:GridView ID="grvClip" runat="server" Width="100%" BorderColor="#e4e4e4" border="0"
                                    Style="border-collapse: collapse;" ShowHeader="true" CellPadding="5" CellSpacing="0"
                                    AutoGenerateColumns="false" EmptyDataText="No Results Found" PagerSettings-Mode="NextPrevious"
                                    PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg" PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg"
                                    HeaderStyle-CssClass="grid-th" OnSorting="grvClip_Sorting" AllowSorting="true"
                                    CssClass="grid" BackColor="#FFFFFF">
                                    <Columns>
                                        <asp:TemplateField ShowHeader="False">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                    Text=" " OnClick="lbtnEdit_Click"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle Height="20px" Width="5%" HorizontalAlign="Center" VerticalAlign="Top" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Station">
                                            <ItemTemplate>
                                                <img id="Img2" runat="Server" src='<%# Eval("ClipLogo") %>' width="23" height="24"
                                                    border="0" />
                                            </ItemTemplate>
                                            <HeaderStyle Height="20px" Width="7%" HorizontalAlign="Center" CssClass="grid-th" />
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Title" SortExpression="ClipTitle">
                                            <ItemTemplate>
                                                <asp:Label ID="lblClipTitle" Text='<%# Eval("ClipTitle") %>' runat="server"></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle Height="20px" Width="30%" CssClass="grid-th" VerticalAlign="Top" />
                                            <ItemStyle CssClass="heading-blue" HorizontalAlign="Left"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ClipCreationDate" ReadOnly="true" HeaderText="Clip Creation Date"
                                            SortExpression="ClipCreationDate">
                                            <HeaderStyle CssClass="grid-th" Height="20px" Width="14%" />
                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ClipDate" ReadOnly="true" HeaderText="Air Date Time" SortExpression="ClipDate">
                                            <HeaderStyle CssClass="grid-th" Width="14%" VerticalAlign="Top" />
                                            <ItemStyle CssClass="content-text2" Height="20px" HorizontalAlign="Left" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="Category">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text="Category"></asp:Label>
                                                <%--  <asp:DropDownList ID="ddlCategory" runat="server" AutoPostBack="true" CssClass="dropdown01"
                                                                                                                        OnSelectedIndexChanged="ddlCategory_SelectedIndexChanged" Width="45px">
                                                                                                                    </asp:DropDownList>--%>
                                                <br />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle HorizontalAlign="Center" Height="20px" CssClass="grid-th" Width="10%"
                                                VerticalAlign="Top" />
                                            <ItemStyle CssClass="content-text2"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="Owner">
                                            <HeaderTemplate>
                                                <asp:Label ID="lblName" runat="server" Text="Owner"></asp:Label>
                                                <%--<asp:DropDownList ID="ddlName" runat="server" AutoPostBack="true" CssClass="dropdown01"
                                                                                                                        OnSelectedIndexChanged="ddlName_SelectedIndexChanged" Width="45px">
                                                                                                                    </asp:DropDownList>--%>
                                                <br />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:Label ID="lblName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid-th" Height="20px" Width="10%" VerticalAlign="Top" />
                                            <ItemStyle CssClass="content-text2"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <asp:Button ID="lbtnLink" runat="server" CommandArgument='<%# Bind("ClipID") %>'
                                                    OnCommand="lbtnPlay_OnCommand" CssClass="btn-play" OnClientClick="btnstart();">
                                                </asp:Button>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Height="20px" />
                                            <HeaderStyle CssClass="grid-th" Width="5%" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false" HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <input type="checkbox" id="chkDelete" runat="server" value='<%# Eval("ArchiveClipKey") %>' />
                                                <asp:HiddenField ID="hfClipID" runat="server" Value='<%# Eval("ClipID") %>' />
                                                <asp:HiddenField ID="hfArchiveClipKey" runat="server" Value='<%# Eval("ArchiveClipKey") %>' />
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                            <HeaderStyle Width="5%" Height="20px" CssClass="grid-th" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grid-th" Height="10px" HorizontalAlign="Center" VerticalAlign="Top">
                                    </HeaderStyle>
                                    <PagerStyle Height="30px" VerticalAlign="Middle" CssClass="pagecontent" HorizontalAlign="Left" />
                                </asp:GridView>
                                <uc:CustomPager ID="ucCustomPager" runat="server" On_PageIndexChange="ucCustomPager_PageIndexChange"
                                    PageSize="10" />
                            </div>
                            <asp:Label ID="lblNoResults" runat="server" Visible="false"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<%--<table border="0" align="center" cellpadding="0" cellspacing="0" style="width: 100%;">
    <tr>
        <td valign="top">
            <div class="div-box" style="display: block">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td align="center">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                    <td valign="top" bgcolor="#FFFFFF" align="center">
                                        <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td>
                                                    <table id="tblPlayer" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td height="25" class="hdbar">
                                                                <table style="padding-left: 10px; width: 100%;" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td align="left">
                                                                            <div class="show" style="cursor: pointer;" onclick="showdivPlayer();">
                                                                                SHOW PLAYER</div>
                                                                        </td>
                                                                        <td align="right" style="padding-right: 10px">
                                                                            <div class="hide" style="cursor: pointer;" onclick="hidedivPlayer();">
                                                                                HIDE</div>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="Player" runat="server" style="width: 100%; overflow: hidden; display: none">
                                                                    <table border="0" width="100%" align="center" cellpadding="0" cellspacing="6" class="border01">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:UpdatePanel ID="upClipPlayer" runat="server" UpdateMode="Conditional">
                                                                                    <ContentTemplate>
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table border="0" align="center" cellpadding="0" width="545px" cellspacing="6" class="border01">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="wrapper" runat="server" jquery1261727998253="53" sizcache="69" sizset="1">
                                                                                                                    <div id="videoplayer" jquery1261727998253="87" sizcache="56" sizset="0">
                                                                                                                        <div class="content123" jquery1261727998253="86" sizcache="56" sizset="0">
                                                                                                                            <div id="videoframe" sizcache="56" sizset="4">
                                                                                                                                <div id="RL_Player_Wrapper" style="margin-top: 10px; background: #000000; position: relative">
                                                                                                                                    <div id="RL_Player" style="width: 545px; height: 340px">
                                                                                                                                        <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server" visible="false">
                                                                                                                                        </div>
                                                                                                                                    </div>
                                                                                                                                </div>
                                                                                                                            </div>
                                                                                                                        </div>
                                                                                                                    </div>
                                                                                                                </div>
                                                                                                                <asp:Label ID="lblTimeOutPlayer" runat="server" Style="display: none; padding-top: 10px"></asp:Label>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table id="tblClosedCaption" width="944px" runat="server" border="0" cellpadding="0"
                                                                                                        cellspacing="0">
                                                                                                        <tr>
                                                                                                            <td height="25" class="hdbar">
                                                                                                                <table style="width: 944px;" border="0" cellspacing="0" cellpadding="0">
                                                                                                                    <tr>
                                                                                                                        <td align="left" style="padding-left: 10px">
                                                                                                                            <div class="show" style="cursor: pointer;" onclick="showdivCaption();">
                                                                                                                                SHOW CAPTION</div>
                                                                                                                        </td>
                                                                                                                        <td align="right" style="padding-right: 10px">
                                                                                                                            <div class="hide" style="cursor: pointer;" onclick="hidedivCaption();">
                                                                                                                                HIDE</div>
                                                                                                                        </td>
                                                                                                                    </tr>
                                                                                                                </table>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <div id="divClosedCaption" runat="server" style="display: none">
                                                                                                                    <table style="width: 944px;" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td height="63" class="grey-grad">
                                                                                                                                <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                                                                                    <tr>
                                                                                                                                        <td align="left" class="heading-blue2">
                                                                                                                                            Closed Caption:
                                                                                                                                            <br />
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                    <tr>
                                                                                                                                        <td class="content-text-grey">
                                                                                                                                            <div id="DivCaption" runat="server" class="panel" style="height: 242px; overflow-y: auto;">
                                                                                                                                            </div>
                                                                                                                                        </td>
                                                                                                                                    </tr>
                                                                                                                                </table>
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </div>
                                                                                                            </td>
                                                                                                        </tr>
                                                                                                    </table>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ContentTemplate>
                                                                                </asp:UpdatePanel>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:UpdatePanel ID="upBtnSearch" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table id="tblSearch" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td height="25" class="hdbar">
                                                                        <table style="padding-left: 10px; width: 100%;" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td align="left" style="width: 420px;">
                                                                                    <div class="show" style="cursor: pointer;" onclick="showdivSearch();">
                                                                                        SHOW SEARCH</div>
                                                                                </td>
                                                                                <td align="left">
                                                                                    <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
                                                                                </td>
                                                                                <td align="right" style="padding-right: 10px">
                                                                                    <div class="hide" style="cursor: pointer;" onclick="hidedivSearch();">
                                                                                        HIDE</div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <div id="DivSearch" runat="server" style="width: 100%; overflow: hidden; display: none;">
                                                                            <table border="0" width="100%" align="center" cellpadding="0" cellspacing="6" class="border01">
                                                                                <tr>
                                                                                    <td>
                                                                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                                            <tr>
                                                                                                <td>
                                                                                                    <table border="0" align="left" cellpadding="0" width="100%" cellspacing="6" class="border01">
                                                                                                        <tr>
                                                                                                            <td>
                                                                                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                                                                                    <tr>
                                                                                                                        <td style="width: 180px" align="center" valign="top">
                                                                                                                            <br />
                                                                                                                            <br />
                                                                                                                            <br />
                                                                                                                            <asp:Button ID="btnSearch" ValidationGroup="vgSearch" OnClick="btnSearch_Click" Width="120px"
                                                                                                                                runat="server" Text="Find Clips" CssClass="btn-blue2" /><br />
                                                                                                                            <br />
                                                                                                                            <asp:Button ID="btnReset" OnClick="btnReset_Click" Width="120px" runat="server" Text="Reset"
                                                                                                                                CssClass="btn-blue2" />
                                                                                                                        </td>
                                                                                                                        <td>
                                                                                                                            <table cellpadding="5" cellspacing="0" width="100%" align="left">
                                                                                                                                <tr>
                                                                                                                                    <td colspan="3">
                                                                                                                                        <asp:Label ID="lblSearchErr" runat="server" ForeColor="Red"></asp:Label>
                                                                                                                                        <asp:ValidationSummary ID="vlsSearch" runat="server" ValidationGroup="vgSearch" />
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td align="left" style="width: 116px">
                                                                                                                                        Search Term:
                                                                                                                                    </td>
                                                                                                                                    <td colspan="2" align="left">
                                                                                                                                        <asp:TextBox CssClass="textbox03" ValidationGroup="vgSearch" ID="txtSearch" runat="server"
                                                                                                                                            Width="578px"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                    </td>
                                                                                                                                    <td colspan="2" align="left">
                                                                                                                                        <div id="divSearch">
                                                                                                                                            <table>
                                                                                                                                                <tr>
                                                                                                                                                    <td>
                                                                                                                                                        <input type="checkbox" id="cbAll" runat="server" value="all" checked="checked" onclick="SetSearchParam();" />
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        All
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        &nbsp;&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        <input type="checkbox" id="cbTitle" runat="server" value="title" checked="checked" />
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        Title
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        &nbsp;&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        <input type="checkbox" id="cbDescription" runat="server" value="description" checked="checked" />
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        Description
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        &nbsp;&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        <input type="checkbox" id="cbKeywords" runat="server" value="keywords" checked="checked" />
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        Keywords
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        &nbsp;&nbsp;
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        <input type="checkbox" id="cbCC" runat="server" value="cc" checked="checked" />
                                                                                                                                                    </td>
                                                                                                                                                    <td>
                                                                                                                                                        Closed Caption
                                                                                                                                                    </td>
                                                                                                                                                </tr>
                                                                                                                                            </table>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td align="left" valign="top">
                                                                                                                                        Clip Creation Date:
                                                                                                                                    </td>
                                                                                                                                    <td colspan="2" align="left" class="content-text1">
                                                                                                                                        <div style="width: 160px; float: left;">
                                                                                                                                            From Date:<br />
                                                                                                                                            <asp:TextBox ValidationGroup="vgSearch" ID="txtFromDate" runat="server" AutoCompleteType="None"
                                                                                                                                                CssClass="textbox03"></asp:TextBox>
                                                                                                                                            <AjaxToolkit:CalendarExtender ID="CalEtxtFromDate" runat="server" CssClass="MyCalendar"
                                                                                                                                                TargetControlID="txtFromDate">
                                                                                                                                            </AjaxToolkit:CalendarExtender>
                                                                                                                                        </div>
                                                                                                                                        <div style="width: 160px; float: left;">
                                                                                                                                            To Date:<br />
                                                                                                                                            <asp:TextBox ValidationGroup="vgSearch" ID="txtToDate" runat="server" AutoCompleteType="None"
                                                                                                                                                CssClass="textbox03"></asp:TextBox>
                                                                                                                                            <AjaxToolkit:CalendarExtender ID="valEtxtToDate" runat="server" CssClass="MyCalendar"
                                                                                                                                                TargetControlID="txtToDate">
                                                                                                                                            </AjaxToolkit:CalendarExtender>
                                                                                                                                            <asp:CompareValidator ValidationGroup="vgSearch" ID="cmpDateValidator" runat="server"
                                                                                                                                                ControlToCompare="txtToDate" ControlToValidate="txtFromDate" Text="*" ErrorMessage="To Date Must be greater than From Date"
                                                                                                                                                Operator="LessThanEqual" Type="Date" Display="Dynamic">
                                                                                                                                            </asp:CompareValidator>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td align="left" valign="top">
                                                                                                                                        Owner:
                                                                                                                                    </td>
                                                                                                                                    <td align="left">
                                                                                                                                        <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                                                                            <asp:CheckBoxList ValidationGroup="vgSearch" CellPadding="1" CellSpacing="1" ID="chkOwnerList"
                                                                                                                                                Style="vertical-align: middle;" runat="server">
                                                                                                                                            </asp:CheckBoxList>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td align="left" valign="top" class="content-text1">
                                                                                                                                        Currently Selected Owners:<br />
                                                                                                                                        <asp:TextBox CssClass="textbox03" ID="txtOwnerSelection" ReadOnly="true" runat="server"
                                                                                                                                            TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td align="left" valign="top">
                                                                                                                                        Category Selection 1:
                                                                                                                                    </td>
                                                                                                                                    <td align="left">
                                                                                                                                        <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                                                                            <asp:CheckBoxList ValidationGroup="vgSearch" ID="chkCategories1" runat="server">
                                                                                                                                            </asp:CheckBoxList>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td align="left" valign="top" class="content-text1">
                                                                                                                                        Currently Selected Category Selection 1:<br />
                                                                                                                                        <asp:TextBox CssClass="textbox03" ID="txtCat1Selection" ReadOnly="true" runat="server"
                                                                                                                                            TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td>
                                                                                                                                    </td>
                                                                                                                                </tr>
                                                                                                                                <tr>
                                                                                                                                    <td align="left" valign="top">
                                                                                                                                        Category Selection 2:
                                                                                                                                    </td>
                                                                                                                                    <td align="left">
                                                                                                                                        <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                                                                            <asp:CheckBoxList ValidationGroup="vgSearch" ID="chkCategories2" runat="server">
                                                                                                                                            </asp:CheckBoxList>
                                                                                                                                        </div>
                                                                                                                                    </td>
                                                                                                                                    <td align="left" valign="top" class="content-text1">
                                                                                                                                        Currently Selected Category Selection 2:<br />
                                                                                                                                        <asp:TextBox CssClass="textbox03" ID="txtCat2Selection" ReadOnly="true" runat="server"
                                                                                                                                            TextMode="MultiLine" Rows="5" Columns="40"></asp:TextBox>
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
                                                                        </div>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                        <Triggers>
                                                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
                                                            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
                                                        </Triggers>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" align="left" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td height="10">
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td width="3">
                                                                                        <img id="Img10" runat="Server" src="~/images/bluebox-hd-left.jpg" width="3" height="24"
                                                                                            border="0" alt="iQMedia" />
                                                                                    </td>
                                                                                    <td class="bluebox-hd" align="left">
                                                                                        &nbsp;Clip Results
                                                                                    </td>
                                                                                    <td width="3">
                                                                                        <img id="Img11" runat="Server" src="~/images/bluebox-hd-right.jpg" width="3" height="24"
                                                                                            border="0" alt="iQMedia" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="blue-content-bg">
                                                                            <asp:UpdatePanel ID="upMainGrid" runat="server" UpdateMode="Conditional">
                                                                                <ContentTemplate>
                                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table border="0" width="100%" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td align="left">
                                                                                                            <asp:Button ID="btnRefreshLibrary" runat="server" Text="Refresh Library" CssClass="btn-blue2"
                                                                                                                OnClick="btnRefreshLibrary_Click" />
                                                                                                            <asp:Button ID="btnClipDownload" runat="server" Visible="false" CssClass="btn-blue2"
                                                                                                                Text="Download Clips" OnClick="btnClipDownload_Click" />
                                                                                                            <asp:Button ID="lnkEmail" runat="server" Text="Email Selected Clip(s)" OnClick="lnkEmail_Click"
                                                                                                                CssClass="btn-blue2" OnClientClick="return ValidateEmailGrid();" />
                                                                                                            <asp:Button ID="btnManageCategories" CssClass="btn-blue2" runat="server" Text="Manage Categories"
                                                                                                                OnClick="btnManageCategories_Click" />
                                                                                                            <asp:Button ID="btnRemoveClips" runat="server" Text="Remove Selected Clip(s)" OnClientClick="return ValidateGrid();"
                                                                                                                CssClass="btn-blue2" Style="text-align: right" OnClick="btnRemoveClips_Click" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green"></asp:Label>
                                                                                                <asp:Label ID="lblErrorClips" runat="server" ForeColor="Red"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                        <tr>
                                                                                            <td>
                                                                                                <asp:HiddenField ID="hfdDdlUser" runat="server" />
                                                                                                <div style="padding-top: 10px;">
                                                                                                    <asp:GridView ID="grvClip" runat="server" AllowPaging="true" PageSize="10" Width="100%"
                                                                                                        BorderColor="#e4e4e4" border="0" Style="border-collapse: collapse;" ShowHeader="true"
                                                                                                        CellPadding="5" CellSpacing="0" AutoGenerateColumns="false" EmptyDataText="No Results Found"
                                                                                                        PagerSettings-Mode="NextPrevious" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                                                                        PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" OnPageIndexChanging="grvClip_PageIndexChanging"
                                                                                                        HeaderStyle-CssClass="grid-th" OnSorting="grvClip_Sorting" AllowSorting="true"
                                                                                                        CssClass="grid">
                                                                                                        <Columns>
                                                                                                            <asp:TemplateField ShowHeader="False">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                                                                                                        Text=" " OnClick="lbtnEdit_Click"></asp:LinkButton>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle Height="20px" Width="5%" HorizontalAlign="Center" VerticalAlign="Top" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Station">
                                                                                                                <ItemTemplate>
                                                                                                                    <img id="Img2" runat="Server" src='<%# Eval("ClipLogo") %>' width="23" height="24"
                                                                                                                        border="0" />
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle Height="20px" Width="7%" HorizontalAlign="Center" CssClass="grid-th" />
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Title" SortExpression="ClipTitle">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblClipTitle" Text='<%# Eval("ClipTitle") %>' runat="server"></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle Height="20px" Width="30%" CssClass="grid-th" VerticalAlign="Top" />
                                                                                                                <ItemStyle CssClass="heading-blue" HorizontalAlign="Left"></ItemStyle>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:BoundField DataField="ClipCreationDate" ReadOnly="true" HeaderText="Clip Creation Date"
                                                                                                                SortExpression="ClipCreationDate">
                                                                                                                <HeaderStyle CssClass="grid-th" Height="20px" Width="14%" />
                                                                                                                <ItemStyle CssClass="content-text2" HorizontalAlign="Left" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:BoundField DataField="ClipDate" ReadOnly="true" HeaderText="Air Date Time" SortExpression="ClipDate">
                                                                                                                <HeaderStyle CssClass="grid-th" Width="14%" VerticalAlign="Top" />
                                                                                                                <ItemStyle CssClass="content-text2" Height="20px" HorizontalAlign="Left" />
                                                                                                            </asp:BoundField>
                                                                                                            <asp:TemplateField HeaderText="Category">
                                                                                                                <HeaderTemplate>
                                                                                                                    <asp:Label ID="lblCategory" runat="server" Text="Category"></asp:Label>
                                                                                                                    
                                                                                                                    <br />
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblCategory" runat="server" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle HorizontalAlign="Center" Height="20px" CssClass="grid-th" Width="10%"
                                                                                                                    VerticalAlign="Top" />
                                                                                                                <ItemStyle CssClass="content-text2"></ItemStyle>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField HeaderText="Owner">
                                                                                                                <HeaderTemplate>
                                                                                                                    <asp:Label ID="lblName" runat="server" Text="Owner"></asp:Label>
                                                                                                                    
                                                                                                                    <br />
                                                                                                                </HeaderTemplate>
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Label ID="lblName" runat="server" Text='<%# Bind("FirstName") %>'></asp:Label>
                                                                                                                </ItemTemplate>
                                                                                                                <HeaderStyle CssClass="grid-th" Height="20px" Width="10%" VerticalAlign="Top" />
                                                                                                                <ItemStyle CssClass="content-text2"></ItemStyle>
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Left">
                                                                                                                <ItemTemplate>
                                                                                                                    <asp:Button ID="lbtnLink" runat="server" CommandArgument='<%# Bind("ClipID") %>'
                                                                                                                        OnCommand="lbtnPlay_OnCommand" CssClass="btn-play" OnClientClick="btnstart();">
                                                                                                                    </asp:Button>
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" Height="20px" />
                                                                                                                <HeaderStyle CssClass="grid-th" Width="5%" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                            <asp:TemplateField ShowHeader="false" HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                                                                                                <ItemTemplate>
                                                                                                                    <input type="checkbox" id="chkDelete" runat="server" value='<%# Eval("ArchiveClipKey") %>' />
                                                                                                                    <asp:HiddenField ID="hfClipID" runat="server" Value='<%# Eval("ClipID") %>' />
                                                                                                                    <asp:HiddenField ID="hfArchiveClipKey" runat="server" Value='<%# Eval("ArchiveClipKey") %>' />
                                                                                                                </ItemTemplate>
                                                                                                                <ItemStyle HorizontalAlign="Center" />
                                                                                                                <HeaderStyle Width="5%" Height="20px" CssClass="grid-th" VerticalAlign="Top" />
                                                                                                            </asp:TemplateField>
                                                                                                        </Columns>
                                                                                                        <HeaderStyle CssClass="grid-th" Height="10px" HorizontalAlign="Center" VerticalAlign="Top">
                                                                                                        </HeaderStyle>
                                                                                                        <PagerStyle Height="30px" VerticalAlign="Middle" CssClass="pagecontent" HorizontalAlign="Left" />
                                                                                                    </asp:GridView>
                                                                                                </div>
                                                                                                <asp:Label ID="lblNoResults" runat="server" Visible="false"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </ContentTemplate>
                                                                            </asp:UpdatePanel>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td height="35">
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td align="right" valign="top" class="contenttext-small">
                                                                                        Click on play button to play video.
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <img id="Img12" runat="Server" src="~/images/spacer.gif" width="1" height="10" border="0"
                                                                                alt="iQMedia" />
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
                    <tr>
                        <td>
                            <img id="Img33" runat="server" src="~/images/spacer.gif" width="1" height="10" border="0"
                                alt="iQMedia" />
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>--%>
<input type="button" id="tgtbtn" runat="server" style="display: none" />
<Ajax:ModalPopupExtender ID="mdlpopupEmail" BackgroundCssClass="ModalBackground"
    BehaviorID="mdlpopupEmail" TargetControlID="tgtbtn" PopupControlID="pnlMailPanel"
    runat="server" CancelControlID="btnCancel">
</Ajax:ModalPopupExtender>
<asp:Panel ID="pnlMailPanel" runat="server" Style="display: none;">
    <asp:UpdatePanel ID="upMail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="500" cellspacing="0" cellpadding="0" border="0" align="center" style="background-color: white;"
                class="popUpInner">
                <tbody>
                    <tr class="bluebox-hd">
                        <td style="padding-left: 10px;">
                            Share This Video :
                        </td>
                        <td align="right" style="padding-right: 10px;">
                            <input type="button" runat="server" id="lbtnCancelPopUp" class="btn-cancel" value=" "
                                onclick="$find('mdlpopupEmail').hide();" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 10px; background-color: #F8FCFF;" class="blue-content-bg"
                            colspan="2">
                            <table width="100%">
                                <tbody>
                                    <tr>
                                        <td style="padding-left: 10px;">
                                            <asp:ValidationSummary ID="ValidationSummary1" EnableClientScript="true" runat="server"
                                                ValidationGroup="validate1" ForeColor="#bd0000" Font-Size="Smaller" />
                                            <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
                                            <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="content-text-new">
                                            Your Email Address :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtYourEmail" runat="server" class="grayinput" size="27" Style="width: 450px;"
                                                Width="170px"></asp:TextBox>
                                            <asp:RegularExpressionValidator ID="regEmailGrid" runat="server" Display="None" ValidationGroup="validate1"
                                                ControlToValidate="txtYourEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                                ErrorMessage="Please Enter Valid Email Address" Text=""></asp:RegularExpressionValidator>
                                            <asp:RequiredFieldValidator ID="rfvEmail" Text="" ValidationGroup="validate1" Display="None"
                                                runat="server" ControlToValidate="txtYourEmail" ErrorMessage="Please Enter Your Email Address."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="content-text-new">
                                            Friends's Email Address (separate multiple addresses with semicolon) :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFriendsEmail" runat="server" class="grayinput" size="27" Style="width: 450px;"
                                                Width="170px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="" ValidationGroup="validate1"
                                                Display="None" runat="server" ControlToValidate="txtFriendsEmail" ErrorMessage="Please Enter Friend's Email Address."></asp:RequiredFieldValidator>
                                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                                                ValidationGroup="validate1" ControlToValidate="txtFriendsEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                                                ErrorMessage="Please Enter Valid Friend's Email Address" Text=""></asp:RegularExpressionValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="content-text-new">
                                            Subject :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSubject" runat="server" class="grayinput" size="27" Style="width: 450px;"
                                                Width="170px"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Text="" ValidationGroup="validate1"
                                                Display="None" runat="server" ControlToValidate="txtSubject" ErrorMessage="Please Enter Subject."></asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="content-text-new">
                                            Message :
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMessage" runat="server" class="grayinput" size="27" Style="width: 450px"
                                                TextMode="MultiLine" Rows="10"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="padding-top: 10px">
                                            <input type="button" id="btnCancel" value="Cancel" class="btn-blue2" onclick="$find('mdlpopupEmail').hide();" />
                                            <asp:Button ID="btnOK" CssClass="btn-blue2" runat="server" Width="51px" Text="OK"
                                                ValidationGroup="validate1" OnClick="btnOK_Click"></asp:Button>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                </tbody>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<input type="button" id="tgtEditClip" runat="server" style="display: none;" />
<Ajax:ModalPopupExtender ID="mdlpopupClip" BackgroundCssClass="ModalBackground" BehaviorID="mdlpopupClip"
    TargetControlID="tgtEditClip" PopupControlID="pnlClipPanel" runat="server">
</Ajax:ModalPopupExtender>
<asp:Panel ID="pnlClipPanel" runat="server" Style="display: none; z-index: 999; width: 720px;"
    DefaultButton="btnClipUpdate">
    <asp:UpdatePanel ID="upEditClip" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="720" border="0" align="center" cellpadding="0" cellspacing="0" style="background-color: white;"
                class="popUpInner">
                <tr class="bluebox-hd">
                    <td align="left" style="padding-left: 10px;">
                        Edit Clip Details
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input type="button" runat="server" id="btnClipClose" class="btn-cancel" value=" "
                            onclick="$find('mdlpopupClip').hide();" />
                    </td>
                </tr>
                <tr style="padding-left: 17px; background-color: #F8FCFF;" class="blue-content-bg">
                    <td colspan="2">
                        <table style="padding: 10px;" width="100%">
                            <tr class="content-text-new">
                                <td style="padding-left: 10px;" colspan="2">
                                    <asp:ValidationSummary ID="vlSummeryEditClip" runat="server" ValidationGroup="vgEditClip"
                                        ForeColor="#bd0000" Font-Size="Smaller" />
                                    <asp:Label ID="lblClipMsg" runat="server" Visible="true" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" width="50%" class="content-text-new">
                                    Clip Title :
                                </td>
                                <td align="left">
                                    <asp:HiddenField ID="hdnArchiveClipKey" runat="server" />
                                    <asp:HiddenField ID="hdnEditClipID" runat="server" />
                                    <asp:TextBox ID="txtEditClipTitle" runat="server" CssClass="grayinput" Width="525px"
                                        MaxLength="255"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtClipTitle" Text="*" runat="server" ControlToValidate="txtEditClipTitle"
                                        ErrorMessage="Title is required." Display="None" ValidationGroup="vgEditClip"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Owner :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlOwner" runat="server" CssClass="grayinput" Width="245px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqOwner" runat="server" ControlToValidate="ddlOwner"
                                        ErrorMessage="Please Select Owner" Text="*" ValidationGroup="vgEditClip" Display="None"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Primary Category :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlPCategory" CssClass="grayinput" runat="server" Width="245px"
                                        onclick="selectList_cache=this.value" onchange="return UpdateSubCategory1(this.id);">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqCategory" runat="server" ControlToValidate="ddlPCategory"
                                        ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgEditClip" Display="Dynamic"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 1 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSubCategory1" runat="server" CssClass="grayinput" Width="245px"
                                        onchange="UpdateSubCategory1(this.id);">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 2 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSubCategory2" runat="server" CssClass="grayinput" Width="245px"
                                        onchange="UpdateSubCategory1(this.id);">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 3 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSubCategory3" runat="server" CssClass="grayinput" Width="245px"
                                        onchange="UpdateSubCategory1(this.id);">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Description :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDescription" runat="server" TextMode="MultiLine" CssClass="grayinput"
                                        Style="width: 525px; height: 145px; overflow: auto;">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtDescription" Text="*" runat="server" ControlToValidate="txtDescription"
                                        ErrorMessage="Description is required." Display="None" ValidationGroup="vgEditClip"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 3px;" class="content-text-new">
                                <td>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="left" valign="top" class="content-text-new">
                                    Keywords :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtKeywords" runat="server" TextMode="MultiLine" CssClass="grayinput"
                                        Style="width: 525px; height: 145px; overflow: auto;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtKeywords" Text="*" runat="server" ControlToValidate="txtKeywords"
                                        ErrorMessage="Keywords is required." Display="None" ValidationGroup="vgEditClip"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td style="padding: 0px 0px 10px 0px" colspan="2">
                                </td>
                            </tr>
                            <tr class="content-text-new">
                                <td align="center" colspan="2">
                                    <input type="button" id="Button1" class="btn-blue2" value="Cancel" onclick="$find('mdlpopupClip').hide();" />
                                    <asp:Button ID="btnClipUpdate" runat="server" Text="Save" ValidationGroup="vgEditClip"
                                        CausesValidation="true" CssClass="btn-blue2" OnClick="btnClipUpdate_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<%--<asp:UpdateProgress ID="updProgressEditClip" runat="server" AssociatedUpdatePanelID="upEditClip">
    <ProgressTemplate>
        <div style="position: relative">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender2"
    ControlToOverlayID="upEditClip" TargetControlID="updProgressEditClip" CssClass="updateProgress" />
<asp:UpdateProgress ID="updProgressMyClips" runat="server" AssociatedUpdatePanelID="upBtnSearch">
    <ProgressTemplate>
        <div sytle="left: 450px;">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>--%>
<!-- Custom Category ModapPopUp -->
<input type="button" id="ccButton1" runat="server" style="display: none" />
<input type="button" id="tgtbtn1" runat="server" style="display: none" />
<Ajax:ModalPopupExtender ID="mpCustomCategory" BackgroundCssClass="ModalBackground"
    BehaviorID="mpCustomCategory" TargetControlID="tgtbtn1" PopupControlID="pnlCancelCustomCategory"
    runat="server">
</Ajax:ModalPopupExtender>
<asp:Panel ID="pnlCancelCustomCategory" runat="server" Style="display: none; z-index: 999;">
    <asp:UpdatePanel ID="upCustomCategory" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <table width="650" border="0" align="center" cellpadding="0" cellspacing="0" class="popUpInner">
                <tr class="bluebox-hd">
                    <td style="padding-left: 10px;">
                        Custom Category Management
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input type="button" runat="server" id="btnCancelCategoryPopup" class="btn-cancel"
                            value=" " onclick="$find('mpCustomCategory').hide();" />
                    </td>
                </tr>
                <tr style="padding-left: 17px; background-color: #F8FCFF;" class="blue-content-bg">
                    <td colspan="3">
                        <table width="100%" style="padding: 5px; border-collapse: separate;">
                            <tr>
                                <td style="padding-left: 10px; height: 25px;" colspan="3">
                                    <asp:ValidationSummary ID="ValidationSummary2" EnableClientScript="true" runat="server"
                                        ValidationGroup="CustomCategory" ForeColor="#bd0000" />
                                    <asp:Label ID="lblMsg" runat="server" ForeColor="Green"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="content-text-new" style="width: 24%;">
                                    Category Name :
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtCategoryName" CssClass="grayinput" Width="450" runat="server" />
                                    <asp:RequiredFieldValidator ID="rfvCategoryName" ControlToValidate="txtCategoryName"
                                        ErrorMessage="Category Name is Required" ValidationGroup="CustomCategory" Text="*"
                                        Display="Dynamic" ToolTip="Category Name is Required" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="content-text-new">
                                    Category Description :
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtCategoryDescription" CssClass="grayinput" Width="450" TextMode="MultiLine"
                                        Rows="3" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td style="padding-top: 10px">
                                    <input type="button" id="btnCancelCustomCategory" value="Cancel" class="btn-blue2"
                                        onclick="$find('mpCustomCategory').hide();" />
                                    <asp:Button ID="btnSaveCustomCategory" CssClass="btn-blue2" runat="server" Width="51px"
                                        Text="Add" ValidationGroup="CustomCategory" OnClick="btnSaveCustomCategory_Click">
                                    </asp:Button>
                                </td>
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" align="center" class="content-text-blue" style="padding: 0px 10px;">
                                    <div style="max-height: 420px; overflow: auto; border-width: 1px; border-color: #E4E4E4;
                                        border-style: solid;">
                                        <asp:GridView ID="gvCustomCategory" runat="server" Width="100%" border="1" AllowPaging="True"
                                            CssClass="grid" PageSize="8" CellPadding="5" CellSpacing="0" AutoGenerateEditButton="False"
                                            HeaderStyle-CssClass="grid-th" BorderColor="#E4E4E4" Style="border-collapse: collapse;"
                                            PagerSettings-Mode="NextPrevious" AutoGenerateColumns="False" EmptyDataText="No Data Found"
                                            PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg" PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg"
                                            OnPageIndexChanging="gvCustomCategory_PageIndexChanging" OnRowCancelingEdit="gvCustomCategory_RowCancelingEdit"
                                            OnRowEditing="gvCustomCategory_RowEditing" OnRowDataBound="gvCustomCategory_RowDataBound"
                                            OnRowUpdating="gvCustomCategory_RowUpdating" OnRowCommand="gvCustomCategory_RowCommand"
                                            DataKeyNames="CategoryGUID" PagerStyle-Width="100">
                                            <PagerSettings Mode="NextPrevious" NextPageImageUrl="~/Images/arrow-next.jpg" PreviousPageImageUrl="~/Images/arrow-previous.jpg" />
                                            <Columns>
                                                <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30">
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
                                                <asp:TemplateField ShowHeader="False" HeaderStyle-Width="30">
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
                                                        <asp:TextBox ID="txtCategoryName" Width="135" CssClass="grayinput" runat="server"
                                                            Text='<%# Bind("CategoryName") %>' />
                                                        <asp:RequiredFieldValidator ID="rfvCategoryName" ControlToValidate="txtCategoryName"
                                                            ValidationGroup="CustomCategoryGrid" ErrorMessage="Category Name is required"
                                                            runat="server" />
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="grid-th" Font-Bold="true" HorizontalAlign="Left" Height="20px"
                                                        Width="140px" />
                                                    <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="140px" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblCategoryDescription" runat="server" Text='<%# Eval("CategoryDescription") %>'></asp:Label>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:TextBox ID="txtCategoryDescription" Width="200" CssClass="grayinput" runat="server"
                                                            TextMode="MultiLine" Text='<%# Bind("CategoryDescription") %>' Rows="3" />
                                                    </EditItemTemplate>
                                                    <HeaderStyle CssClass="grid-th" Font-Bold="true" Height="20px" />
                                                    <ItemStyle CssClass="content-text2" HorizontalAlign="Left" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <HeaderStyle CssClass="grid-th" Height="20px" />
                                        </asp:GridView>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<%--<asp:UpdateProgress ID="updCustomCategory" runat="server" AssociatedUpdatePanelID="upCustomCategory">
    <ProgressTemplate>
        <div style="position: relative">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upoeCustomCategory" ControlToOverlayID="upCustomCategory"
    TargetControlID="updCustomCategory" CssClass="updateProgress" />
<cc2:UpdateProgressOverlayExtender runat="server" ID="updoGrid" ControlToOverlayID="upBtnSearch"
    TargetControlID="updProgressMyClips" CssClass="updateProgress" />
<asp:UpdateProgress ID="updProgressGrid" runat="server" AssociatedUpdatePanelID="upMainGrid">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="upMainGrid" TargetControlID="updProgressGrid" CssClass="updateProgress" />
<asp:UpdateProgress ID="updEmailClip" runat="server" AssociatedUpdatePanelID="upMail">
    <ProgressTemplate>
        <div sytle="left: 450px;">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="updoEmailClip" ControlToOverlayID="upMail"
    TargetControlID="updEmailClip" CssClass="updateProgress" />--%>
