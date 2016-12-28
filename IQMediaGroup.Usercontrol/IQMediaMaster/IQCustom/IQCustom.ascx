<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQCustom.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQCustom.IQCustom" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%@ Register Src="../CustomPager/CustomPager.ascx" TagName="CustomPager" TagPrefix="uc" %>
<script type="text/javascript" language="javascript">

    function searchString(arrayToSearch, stringToSearch) {
        arrayToSearch.sort();

        for (var i = 0; i < arrayToSearch.length; i++) {

            if (arrayToSearch[i] == stringToSearch)
                return true;
        }
        return false;
    }

    function UploadMediaVisible() {

        var SubCat1Id = "<%= ddlSubCategory1.ClientID %>";
        var SubCat2Id = "<%= ddlSubCategory2.ClientID %>";
        var SubCat3Id = "<%= ddlSubCategory3.ClientID %>";

        $("#" + SubCat1Id + " option").removeAttr("disabled");
        $("#" + SubCat2Id + " option").removeAttr("disabled");
        $("#" + SubCat3Id + " option").removeAttr("disabled");

        $find('mpeUploadMedia').show();
        return false;
    }

    function Uploading() {
        Page_ClientValidate('vgUploadMedia');
        if (Page_IsValid != null && Page_IsValid == true) {

            $get("<%= tblUpload.ClientID %>").style.display = "none";
            document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "none";
            document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "none";
            $get("<%= tblUploading.ClientID %>").style.display = "block";




            var filename = document.getElementById('ctl00_Content_Data_IQCustom1_hdnName').value;
            //alert(filename);
            var extentionIndex = filename.lastIndexOf('.');
            var extention = filename.substring(extentionIndex + 1);

            /*var patt1 = /.mp3/gi;
            var patt2 = /.mp4/gi;
            var patt3 = /.flv/gi;
            var filemp3ext = filename.match(patt1);
            var filemp4exr = filename.match(patt2);
            var fileflvexr = filename.match(patt3);*
            if (filemp3ext != null || filemp4exr != null || fileflvexr != null) {

            }*/
            if (searchString(Extention, extention)) {

            }
            else {
                //alert("MAULIK");
                $get("ctl00_Content_Data_IQCustom1_tblUpload").style.display = "block";
                $get("ctl00_Content_Data_IQCustom1_tblUploading").style.display = "none";
                document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "none";
                document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "none";
                document.getElementById("<%= lblMsg.ClientID %>").innerHTML = "Invalid file, file has to be mp3, mp4 or flv.";
                return false;
            }
        }
        else {
            return false;
        }

    }   

</script>
<script language="javascript" type="text/javascript">
    function onDelete() {
        var grid = document.getElementById('<%=grvUGCRawMedia.ClientID%>');
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
                alert('please select atleast one record to delete.');
                return false;
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
<script language="javascript" type="text/javascript">
    function UpdateSubCategory1(ddl_id, IsEdit) {

        var PCatId;
        var SubCat1Id;
        var SubCat2Id;
        var SubCat3Id;
        var LblMessageID;
        if (!IsEdit) {
            PCatId = "<%= ddlCategory.ClientID %>";
            SubCat1Id = "<%= ddlSubCategory1.ClientID %>";
            SubCat2Id = "<%= ddlSubCategory2.ClientID %>";
            SubCat3Id = "<%= ddlSubCategory3.ClientID %>";
            LblMessageID = "<%=lblMsg.ClientID %>";
        }
        else {
            PCatId = "<%= ddlPCategory.ClientID %>";
            SubCat1Id = "<%= ddlEditSubCategory1.ClientID %>";
            SubCat2Id = "<%= ddlEditSubCategory2.ClientID %>";
            SubCat3Id = "<%= ddlEditSubCategory3.ClientID %>";
            LblMessageID = "<%=lblUGCMsg.ClientID %>";
        }

        var PCatSelectedValue = $("#" + PCatId + "").val();
        var Cat1SelectedValue = $("#" + SubCat1Id + "").val();
        var Cat2SelectedValue = $("#" + SubCat2Id + "").val();
        var Cat3SelectedValue = $("#" + SubCat3Id + "").val();
        document.getElementById(LblMessageID).innerHTML = "";

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
                document.getElementById(LblMessageID).innerHTML = "<ul><li>Please first select all preceding categories.</li></ul>";
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
                document.getElementById(LblMessageID).innerHTML = "<ul><li>Please first select all preceding categories.</li></ul>";
            }
        }
        else if (ddl_id == SubCat3Id) {
            if (PCatSelectedValue == 0 || Cat1SelectedValue == 0 || Cat2SelectedValue == 0) {
                $("#" + SubCat3Id + "").val(0);
                document.getElementById(LblMessageID).innerHTML = "<ul><li>Please first select all preceding categories.</li></ul>";
            }
        }
    }
    function UpdateSubCategory2(ddl_id) {
        var PCatId = "<%= ddlPCategory.ClientID %>";
        var SubCat1Id = "<%= ddlEditSubCategory1.ClientID %>";
        var SubCat2Id = "<%= ddlEditSubCategory2.ClientID %>";
        var SubCat3Id = "<%= ddlEditSubCategory3.ClientID %>"

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
   
</script>
<div id="divClipFrame" class="show-bg">
    <asp:UpdatePanel ID="upClipFrame" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <iframe id="Clipframe" runat="server" style="width: 100%; height: 27px;" scrolling="no"
                marginwidth="0" marginheight="0" hspace="0" vspace="0" border="0" frameborder="0">
            </iframe>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
<div id="divMainSearch ">
    <asp:UpdatePanel ID="upUGCSearch" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="show-bg">
                <div class="show-hide" style="text-align: center;">
                    <div class="float-left" onclick="showdivSearch();">
                        <a href="javascript:;">
                            <img src="../images/show.png" alt="">
                            SHOW SEARCH</a></div>
                    <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
                    <div class="float-right" onclick="hidedivSearch();">
                        <a href="javascript:;">HIDE
                            <img src="../images/hiden.png" alt=""></a></div>
                </div>
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
                                                                        runat="server" Text="Find Media" CssClass="btn-blue2" /><br />
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
                                                                            <td align="left" style="width: 122px">
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
                                                                                        </tr>
                                                                                    </table>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td align="left" valign="top">
                                                                                Media Uploaded Date:
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
                                                                                Category:
                                                                            </td>
                                                                            <td align="left">
                                                                                <div style="height: 130px; overflow: auto; width: 291px; border: solid #D8D5D5 1px;">
                                                                                    <asp:CheckBoxList ValidationGroup="vgSearch" ID="chkCategories1" runat="server">
                                                                                    </asp:CheckBoxList>
                                                                                </div>
                                                                            </td>
                                                                            <td align="left" valign="top" class="content-text1">
                                                                                Currently Selected Categories:<br />
                                                                                <asp:TextBox CssClass="textbox03" ID="txtCat1Selection" ReadOnly="true" runat="server"
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
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div class="grid-bg">
    <div class="result-bg">
        <div class="float-left">
            User Uploaded Media</div>
    </div>
    <div class="result-in-bg">
        <%--ChildrenAsTriggers="false"--%>
        <asp:UpdatePanel runat="server" ID="upUploadMedia" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="Click" />
                <asp:AsyncPostBackTrigger ControlID="grvUGCRawMedia" />
                <asp:AsyncPostBackTrigger ControlID="ucCustomPager" />
                <asp:AsyncPostBackTrigger ControlID="btnDelete" />
                <asp:PostBackTrigger ControlID="tgtUpload" />
            </Triggers>
            <ContentTemplate>
                <div style="display: block;">
                    <div style="padding-bottom: 5px; padding-left: 5px;">
                        <asp:Button ID="btnRefresh" runat="server" CssClass="btn-blue2" Text="Refresh Library"
                            OnClick="btnRefresh_Click" />
                        <input type="button" class="btn-blue2" id="tgtUpload" onclick="UploadMediaVisible();"
                            runat="server" value="Upload Media" />
                        <asp:Button ID="btnDelete" runat="server" CssClass="btn-blue2" OnClick="btnDelete_Click"
                            OnClientClick="return onDelete();" Text="Remove Selected Media" />
                    </div>
                    <div>
                        <asp:GridView ID="grvUGCRawMedia" runat="server" AllowPaging="false" PageSize="10"
                            Width="100%" BorderColor="#e4e4e4" border="0" GridLines="None" Style="border-collapse: collapse;"
                            ShowHeader="true" CellPadding="5" CellSpacing="0" AutoGenerateColumns="false"
                            EmptyDataText="No Results Found" PagerSettings-Mode="NextPrevious" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" HeaderStyle-CssClass="grid-th"
                            DataKeyNames="UGCGUID" OnSorting="grvUGCRawMedia_Sorting" OnDataBound="grvUGCRawMedia_DataBound"
                            AllowSorting="true" CssClass="grid" BackColor="#FFFFFF" OnRowDataBound="grvUGCRawMedia_RowDataBound">
                            <Columns>
                                <asp:TemplateField ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnEdit" runat="server" CssClass="btn-edit" CausesValidation="False"
                                            OnClick="lbtnEdit_Click" Text=" "></asp:LinkButton>
                                        <asp:HiddenField ID="hfUGCGUID" runat="server" Value='<%# Eval("UGCGUID") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle Width="5%" HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Title" SortExpression="Title">
                                    <ItemTemplate>
                                        <%# Eval("Title") %>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid-th" Width="28%" />
                                    <ItemStyle CssClass="heading-blue" HorizontalAlign="Left"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="AirDate" ReadOnly="true" HeaderText="Uploaded Date" SortExpression="AirDate">
                                    <HeaderStyle CssClass="grid-th" Width="15%" />
                                    <ItemStyle CssClass="content-text2" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:BoundField DataField="CreatedDT" ReadOnly="true" HeaderText="Air/Created Date"
                                    SortExpression="CreateDT">
                                    <HeaderStyle CssClass="grid-th" Width="15%" />
                                    <ItemStyle CssClass="content-text2" HorizontalAlign="Left" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Category">
                                    <ItemTemplate>
                                        <%# Eval("CategoryName") %>
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" Width="12%" CssClass="grid-th" />
                                    <ItemStyle CssClass="content-text2"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Owner">
                                    <ItemTemplate>
                                        <%# Eval("FirstName") %>
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="grid-th" Width="8%" />
                                    <ItemStyle CssClass="content-text2"></ItemStyle>
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:Button ID="lbtnLink" runat="server" CommandArgument='<%# Eval("UGCGUID") %>'
                                            OnCommand="lbtnPlay_OnCommand" CssClass="btn-play"></asp:Button>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="4%" CssClass="grid-th" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false" HeaderText="Download" HeaderStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDownload" runat="server" ImageUrl="~/Images/Down.png" CommandArgument='<%# Eval("UGCGUID") %>'
                                            OnCommand="ibtnDownload_Command" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="8%" CssClass="grid-th" />
                                </asp:TemplateField>
                                <asp:TemplateField ShowHeader="false" HeaderText="Select" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <input type="checkbox" id="chkDelete" runat="server" value='<%# Eval("UGCGUID") %>' />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" />
                                    <HeaderStyle Width="5%" CssClass="grid-th" />
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="grid-th" Height="10px" HorizontalAlign="Center"></HeaderStyle>
                            <PagerStyle Height="30px" VerticalAlign="Middle" CssClass="pagecontent" HorizontalAlign="Left" />
                        </asp:GridView>
                        <uc:CustomPager ID="ucCustomPager" runat="server" On_PageIndexChange="ucCustomPager_PageIndexChange"
                            PageSize="10" />
                        <asp:Label ID="lblNoResults" runat="server" Visible="false"></asp:Label>
                        <asp:Label ID="lblTxtMsg" runat="server" Visible="false" ForeColor="Green"></asp:Label>
                    </div>
                    <%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td height="10">
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table width="100%">
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            
                        </td>
                    </tr>
                    <tr>
                        <td style="padding: 0px 0px 10px 0px">
                            
                        </td>
                    </tr>
                </table>--%>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</div>
<asp:Button ID="btnFtpBrowse2" runat="server" Style="display: none" />
<Ajax:ModalPopupExtender ID="modalpopupFtpBrowse" runat="server" BackgroundCssClass="ModalBackgroundFtpBrowse"
    BehaviorID="modalpopupFtpBrowse" CancelControlID="lbtnCancelFtpBrowse" TargetControlID="btnFtpBrowse2"
    PopupControlID="pnlFtpBrowse">
</Ajax:ModalPopupExtender>
<asp:Panel ID="pnlFtpBrowse" runat="server" CssClass="mdlpopupFtpBrowse" Style="display: none;">
    <table border="0" align="center" cellpadding="0" cellspacing="0" style="margin: 0px 0px 0px 0px;
        width: 620px;" class="popUpInner">
        <tr class="bluebox-hd">
            <td style="padding-left: 10px;">
                FTP UGC Upload
            </td>
            <td align="right" style="padding-right: 10px;">
                <input type="button" id="lbtnCancelFtpBrowse" runat="server" class="btn-cancel" value="" />
            </td>
        </tr>
        <tr style="padding-left: 17px; background-color: #F8FCFF;" class="blue-content-bg">
            <td colspan="3">
                <table width="100%" style="padding: 10px;">
                    <tr>
                        <td class="content-text-blue" style="padding: 5px 10px 5px 5px;">
                            <div>
                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td colspan="2">
                                            <asp:UpdatePanel ID="upFtpBrowse" runat="server" UpdateMode="Conditional">
                                                <ContentTemplate>
                                                    <div style="max-height: 450px; overflow: auto;">
                                                        <table width="100%" border="0" cellspacing="0" cellpadding="5">
                                                            <tr>
                                                                <td align="left" width="40%" valign="top">
                                                                    <asp:TreeView ID="trVDir" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="trVDir_SelectedNodeChanged">
                                                                        <ParentNodeStyle Font-Bold="False" />
                                                                        <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                                                                        <SelectedNodeStyle Font-Underline="True" ForeColor="DarkGray" HorizontalPadding="10px"
                                                                            VerticalPadding="0px" />
                                                                        <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                                                            NodeSpacing="0px" VerticalPadding="0px" />
                                                                    </asp:TreeView>
                                                                </td>
                                                                <td align="left" width="60%" valign="top">
                                                                    <asp:Label ID="lblerr" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                                                                    <asp:ValidationSummary ID="vgSelectFile" DisplayMode="List" runat="server" ValidationGroup="vgSelectOk"
                                                                        runat="server" />
                                                                    <asp:GridView ID="grdFiles" DataKeyNames="Name" BorderWidth="0" GridLines="None"
                                                                        runat="server" AllowPaging="false" Width="100%" CellPadding="5" CellSpacing="0"
                                                                        AutoGenerateColumns="false" ShowHeader="false" EmptyDataText="No Files Found">
                                                                        <Columns>
                                                                            <asp:TemplateField ShowHeader="false">
                                                                                <ItemTemplate>
                                                                                    <asp:LinkButton ID="lnkFilename" OnClick="lnkFilename_Click" runat="server" Text='<%# Eval("Name") %>'></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="5">
                                                        <tr style="height: 5px">
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="left">
                                                                <strong style="float: left;">File name : &nbsp;</strong>
                                                                <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator4" runat="server"
                                                                    ValidationGroup="vgSelectOk" ControlToValidate="txtFileName" ErrorMessage="Please select a file."
                                                                    Text="*"></asp:RequiredFieldValidator>
                                                                <asp:TextBox ID="txtFileName" ReadOnly="true" ValidationGroup="vgSelectOk" runat="server"
                                                                    CssClass="grayinput" Width="420px"></asp:TextBox>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" align="center">
                                                                <asp:Button ID="Button1" OnClick="btnCancel_Click" runat="server" Text="Cancel" CssClass="btn-blue2" />
                                                                <asp:Button ID="btnOk" runat="server" ValidationGroup="vgSelectOk" Text="Ok" CssClass="btn-blue2"
                                                                    OnClick="btnOk_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                            <%--<asp:UpdateProgress ID="upProgressFtpBrowse" runat="server" AssociatedUpdatePanelID="upFtpBrowse"
                                                DisplayAfter="0">
                                                <ProgressTemplate>
                                                    <div>
                                                        <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                                                            alt="Loading..." id="imgLoading" />
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>--%>
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
</asp:Panel>
<%--<cc2:UpdateProgressOverlayExtender runat="server" ID="upProgressOverlayExtender"
    ControlToOverlayID="pnlFtpBrowse" TargetControlID="upProgressFtpBrowse" CssClass="updateProgress" />--%>
<input type="button" runat="server" id="tgtUpload2" style="display: none" value="Upload Media" />
<asp:Panel ID="pnlUploadMedia" runat="server" Style="z-index: 999; display: none;
    width: 574px;">
    <asp:UpdatePanel ID="upUploadMediaPopop" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUploadMedia" />
            <asp:AsyncPostBackTrigger ControlID="btnFtpBrowse" />
        </Triggers>
        <ContentTemplate>
            <table id="tblUpload" runat="server" border="0" align="center" cellpadding="0" cellspacing="0"
                style="width: 574px; background-color: white;" class="popUpInner">
                <tr class="bluebox-hd">
                    <td align="left" style="padding-left: 10px;">
                        UGC Upload
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input type="button" id="lbtnCancelPopUpUpload" runat="server" class="btn-cancel"
                            value="" onclick="ClearAllQueue();" />
                        <input type="button" runat="server" id="lbtnCancelPopUp" style="display: none" class="btn-cancel"
                            value=" " onclick="ClearAllQueue();" />
                    </td>
                </tr>
                <tr style="background-color: #F8FCFF;">
                    <td colspan="2" style="background-color: #F8FCFF;">
                        &nbsp;
                    </td>
                </tr>
                <tr style="background-color: #F8FCFF;">
                    <td width="30%">
                        &nbsp;
                    </td>
                    <td style="padding-left: 10px;" align="left">
                        <asp:ValidationSummary ID="vsUploadMedia" EnableClientScript="true" runat="server"
                            ValidationGroup="vgUploadMedia" DisplayMode="BulletList" ForeColor="#bd0000"
                            Font-Size="12px" />
                        <asp:Label ID="lblMsg" runat="server" Visible="true" Font-Size="Smaller" ForeColor="#bd0000"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background-color: #F8FCFF;">
                        <table id="tblFile" runat="server" cellpadding="3" cellspacing="0" border="0" style="background-color: #F8FCFF;
                            padding: 10px;">
                            <tr>
                                <td width="120px" align="left" valign="top" class="content-text-new">
                                    File Name :
                                </td>
                                <td align="left">
                                    <%--<asp:FileUpload ID="fuMedia" runat="server" CssClass="textbox03" Style="width: 450px;"
                                                                                                            size="45" />--%>
                                    <input type="text" id="fileuploadname" runat="server" readonly="readonly" class="grayinput"
                                        style="width: 200px; height: 15px; vertical-align: top;" size="35" />
                                    <asp:RequiredFieldValidator ID="rfvfuMedia" runat="server" ControlToValidate="fileuploadname"
                                        ErrorMessage="Media file required" Text="*" Display="None" ValidationGroup="vgUploadMedia"></asp:RequiredFieldValidator>
                                    <%--<iframe id="uploadFrame" scrolling="no" frameborder="0" height="28px" width="400px"
                                                                                                            runat="server"></iframe>--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    <div>
                                        <div runat="server" id="divFU" style="float: left;">
                                            <asp:FileUpload ID="FileUpload1" runat="server" />
                                        </div>
                                        <div style="float: left;">
                                            <asp:ImageButton ID="btnFtpBrowse" runat="server" Style="vertical-align: top;" OnClick="btnFtpBrowse_Click"
                                                ImageUrl="~/images/browse-ftp.png" />
                                            <%--<asp:Button ID="btnFtpBrowse" CssClass="browseButton" runat="server" Text="Browse Ftp"
                                                Style="vertical-align: top;" OnClick="btnFtpBrowse_Click" />--%>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td width="120px" align="left" valign="top" class="content-text-new">
                                    Date Uploaded :
                                </td>
                                <td align="left">
                                    <input type="text" value='<%= System.DateTime.Now %>' class="grayinput" readonly="readonly"
                                        style="width: 175px;" />
                                </td>
                            </tr>
                            <tr id="ugcAutoClip" runat="server">
                                <td width="120px" align="left" valign="top" class="content-text-new">
                                    Auto Clip :
                                </td>
                                <td align="left">
                                    <input type="checkbox" id="chkAutoClip" runat="server" value="1" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="background-color: #F8FCFF;">
                        <table id="tblNewUpload" runat="server" border="0" cellpadding="2" cellspacing="0"
                            style="background-color: #F8FCFF; padding: 10px;">
                            <tr>
                                <td colspan="2" align="left" style="padding-left: 5px; font: bold 14px/24px avenir_65medium;">
                                    Media Information :
                                </td>
                            </tr>
                            <tr>
                                <td width="120px" align="left" valign="top" class="content-text-new">
                                    Title :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtMediaTitle" runat="server" CssClass="grayinput" Width="350px"
                                        MaxLength="2048"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtMediaTitle" Text="*" runat="server" ControlToValidate="txtMediaTitle"
                                        ErrorMessage="Title is required." Display="None" ValidationGroup="vgUploadMedia"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Keywords :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtKeywords" runat="server" CssClass="grayinput" Width="350px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtKeywords" Text="*" runat="server" ControlToValidate="txtKeywords"
                                        ErrorMessage="Keywords is required." Display="None" ValidationGroup="vgUploadMedia"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Category :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlCategory" runat="server" Width="185px" onchange="UpdateSubCategory1(this.id,false);"
                                        CssClass="grayselect">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqCategory" runat="server" ControlToValidate="ddlCategory"
                                        ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgUploadMedia"
                                        Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 1 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSubCategory1" runat="server" Width="185px" onchange="UpdateSubCategory1(this.id,false);"
                                        CssClass="grayselect">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 2 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSubCategory2" runat="server" Width="185px" onchange="UpdateSubCategory1(this.id,false);"
                                        CssClass="grayselect">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 3 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlSubCategory3" runat="server" Width="185px" onchange="UpdateSubCategory1(this.id,false);"
                                        CssClass="grayselect">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Air/Created Date :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlMonth" runat="server" CssClass="grayselect">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span>/</span>
                                    <asp:DropDownList ID="ddlDay" runat="server" CssClass="grayselect">
                                        <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="2" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="4" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="6" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="7" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="9" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span>/</span>
                                    <asp:DropDownList ID="ddlYear" runat="server" CssClass="grayselect">
                                        <asp:ListItem Text="2000" Value="2000"></asp:ListItem>
                                        <asp:ListItem Text="2001" Value="2001"></asp:ListItem>
                                        <asp:ListItem Text="2002" Value="2002"></asp:ListItem>
                                        <asp:ListItem Text="2003" Value="2003"></asp:ListItem>
                                        <asp:ListItem Text="2004" Value="2004"></asp:ListItem>
                                        <asp:ListItem Text="2005" Value="2005"></asp:ListItem>
                                        <asp:ListItem Text="2006" Value="2006"></asp:ListItem>
                                        <asp:ListItem Text="2007" Value="2007"></asp:ListItem>
                                        <asp:ListItem Text="2008" Value="2008"></asp:ListItem>
                                        <asp:ListItem Text="2009" Value="2009"></asp:ListItem>
                                        <asp:ListItem Text="2010" Value="2010"></asp:ListItem>
                                        <asp:ListItem Text="2011" Value="2011"></asp:ListItem>
                                        <asp:ListItem Text="2012" Value="2012"></asp:ListItem>
                                        <asp:ListItem Text="2013" Value="2013"></asp:ListItem>
                                        <asp:ListItem Text="2014" Value="2014"></asp:ListItem>
                                        <asp:ListItem Text="2015" Value="2015"></asp:ListItem>
                                        <asp:ListItem Text="2016" Value="2016"></asp:ListItem>
                                        <asp:ListItem Text="2017" Value="2017"></asp:ListItem>
                                        <asp:ListItem Text="2018" Value="2018"></asp:ListItem>
                                        <asp:ListItem Text="2019" Value="2019"></asp:ListItem>
                                        <asp:ListItem Text="2020" Value="2020"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span>&nbsp;</span>
                                    <asp:DropDownList ID="ddlHour" runat="server" CssClass="grayselect">
                                        <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                    </asp:DropDownList>
                                    <span>:</span>
                                    <asp:DropDownList ID="ddlMinute" runat="server" CssClass="grayselect">
                                        <asp:ListItem Text="00" Value="0"></asp:ListItem>
                                        <asp:ListItem Text="01" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="02" Value="2"></asp:ListItem>
                                        <asp:ListItem Text="03" Value="3"></asp:ListItem>
                                        <asp:ListItem Text="04" Value="4"></asp:ListItem>
                                        <asp:ListItem Text="05" Value="5"></asp:ListItem>
                                        <asp:ListItem Text="06" Value="6"></asp:ListItem>
                                        <asp:ListItem Text="07" Value="7"></asp:ListItem>
                                        <asp:ListItem Text="08" Value="8"></asp:ListItem>
                                        <asp:ListItem Text="09" Value="9"></asp:ListItem>
                                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                                        <asp:ListItem Text="11" Value="11"></asp:ListItem>
                                        <asp:ListItem Text="12" Value="12"></asp:ListItem>
                                        <asp:ListItem Text="13" Value="13"></asp:ListItem>
                                        <asp:ListItem Text="14" Value="14"></asp:ListItem>
                                        <asp:ListItem Text="15" Value="15"></asp:ListItem>
                                        <asp:ListItem Text="16" Value="16"></asp:ListItem>
                                        <asp:ListItem Text="17" Value="17"></asp:ListItem>
                                        <asp:ListItem Text="18" Value="18"></asp:ListItem>
                                        <asp:ListItem Text="19" Value="19"></asp:ListItem>
                                        <asp:ListItem Text="20" Value="20"></asp:ListItem>
                                        <asp:ListItem Text="21" Value="21"></asp:ListItem>
                                        <asp:ListItem Text="22" Value="22"></asp:ListItem>
                                        <asp:ListItem Text="23" Value="23"></asp:ListItem>
                                        <asp:ListItem Text="24" Value="24"></asp:ListItem>
                                        <asp:ListItem Text="25" Value="25"></asp:ListItem>
                                        <asp:ListItem Text="26" Value="26"></asp:ListItem>
                                        <asp:ListItem Text="27" Value="27"></asp:ListItem>
                                        <asp:ListItem Text="28" Value="28"></asp:ListItem>
                                        <asp:ListItem Text="29" Value="29"></asp:ListItem>
                                        <asp:ListItem Text="30" Value="30"></asp:ListItem>
                                        <asp:ListItem Text="31" Value="31"></asp:ListItem>
                                        <asp:ListItem Text="32" Value="32"></asp:ListItem>
                                        <asp:ListItem Text="33" Value="33"></asp:ListItem>
                                        <asp:ListItem Text="34" Value="34"></asp:ListItem>
                                        <asp:ListItem Text="35" Value="35"></asp:ListItem>
                                        <asp:ListItem Text="36" Value="36"></asp:ListItem>
                                        <asp:ListItem Text="37" Value="37"></asp:ListItem>
                                        <asp:ListItem Text="38" Value="38"></asp:ListItem>
                                        <asp:ListItem Text="39" Value="39"></asp:ListItem>
                                        <asp:ListItem Text="40" Value="40"></asp:ListItem>
                                        <asp:ListItem Text="41" Value="41"></asp:ListItem>
                                        <asp:ListItem Text="42" Value="42"></asp:ListItem>
                                        <asp:ListItem Text="43" Value="43"></asp:ListItem>
                                        <asp:ListItem Text="44" Value="44"></asp:ListItem>
                                        <asp:ListItem Text="45" Value="45"></asp:ListItem>
                                        <asp:ListItem Text="46" Value="46"></asp:ListItem>
                                        <asp:ListItem Text="47" Value="47"></asp:ListItem>
                                        <asp:ListItem Text="48" Value="48"></asp:ListItem>
                                        <asp:ListItem Text="49" Value="49"></asp:ListItem>
                                        <asp:ListItem Text="50" Value="50"></asp:ListItem>
                                        <asp:ListItem Text="51" Value="51"></asp:ListItem>
                                        <asp:ListItem Text="52" Value="52"></asp:ListItem>
                                        <asp:ListItem Text="53" Value="53"></asp:ListItem>
                                        <asp:ListItem Text="54" Value="54"></asp:ListItem>
                                        <asp:ListItem Text="55" Value="55"></asp:ListItem>
                                        <asp:ListItem Text="56" Value="56"></asp:ListItem>
                                        <asp:ListItem Text="57" Value="57"></asp:ListItem>
                                        <asp:ListItem Text="58" Value="58"></asp:ListItem>
                                        <asp:ListItem Text="59" Value="59"></asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="rfvCreatedDate" runat="server" ControlToValidate="ddlMinute"
                                        Text="*" ErrorMessage="Invalid CreatedDate." ValidationGroup="vgUploadMedia"></asp:RequiredFieldValidator>
                                    <%-- <asp:TextBox ID="txtCreatedDate" CssClass="textbox03" runat="server" ValidationGroup="MKE"
                                                                                                            Width="175px" />--%>
                                    <%-- <AjaxToolkit:MaskedEditExtender ID="maskCreatedDate" runat="server" TargetControlID="txtCreatedDate"
                                                                                                            Mask="99/99/9999 99:99" MessageValidatorTip="false" OnFocusCssClass="MaskedEditFocus"
                                                                                                            OnInvalidCssClass="MaskedEditError" MaskType="DateTime" AcceptAMPM="True" ErrorTooltipEnabled="True" />
                                                                                                        <AjaxToolkit:MaskedEditValidator ID="maskValCreatedDate" runat="server" ControlExtender="maskCreatedDate"
                                                                                                            ControlToValidate="txtCreatedDate" IsValidEmpty="False" EmptyValueMessage="Date and time are required"
                                                                                                            InvalidValueMessage="Date and/or time is invalid" Display="None" ValidationGroup="vgUploadMedia" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Time Zone :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlTimeZone" runat="server" Width="185px" CssClass="grayselect">
                                        <asp:ListItem Text="CST" Value="CST"></asp:ListItem>
                                        <asp:ListItem Text="EST" Value="EST" Selected="True"></asp:ListItem>
                                        <asp:ListItem Text="MST" Value="MST"></asp:ListItem>
                                        <asp:ListItem Text="PST" Value="PST"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Description :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtDescription" CssClass="grayinput" runat="server" TextMode="MultiLine"
                                        Width="175px" Height="75px">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtDescription" Text="*" runat="server" ControlToValidate="txtDescription"
                                        ErrorMessage="Description is required." Display="None" ValidationGroup="vgUploadMedia"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 0px 0px 10px 0px" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="padding: 0px 20px 0px 150px" colspan="2">
                                    <input id="upload" type="button" class="btn-blue2" value="Upload" />
                                    <asp:Button ID="btnUploadMedia" Style="display: none" runat="server" Text="Upload Media"
                                        OnClick="btnUploadMedia_Click" ValidationGroup="vgUploadMedia" CssClass="btn-blue2"
                                        CausesValidation="true" />
                                    <%--<input type="button" id="btnCancel" class="btn-blue2" value="Cancel" onclick="javascript:$find('mpeUploadMedia').hide();$find('vsUploadMedia').style.display = 'none'" />--%>
                                    <input type="button" id="btnCancel" class="btn-blue2" value="Cancel" onclick="ClearAllQueue();" />
                                    <%--<input type="button" value="Click Me.." onclick="alert(document.getElementById('ctl00_Content_Data_IQCustom1_FileUpload1Queue').firstChild.attributes['id'].value.replace('ctl00_Content_Data_IQCustom1_FileUpload1',''));" />
                                                                                                                    <input type="button" value="Click Me.." onclick="$('#ctl00_Content_Data_IQCustom1_FileUpload1').uploadifyCancel(document.getElementById('ctl00_Content_Data_IQCustom1_FileUpload1Queue').firstChild.attributes['id'].value.replace('ctl00_Content_Data_IQCustom1_FileUpload1',''))" />
                                                                                                                     <a href="javascript:$('#ctl00_Content_Data_IQCustom1_FileUpload1').uploadifyCancel($('#ctl00_Content_Data_IQCustom1_FileUpload1Queue').first().attr('id').replace('ctl00_Content_Data_IQCustom1_FileUpload1',''))">Cancel First File</a>.--%>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table runat="server" id="tblUploading" style="display: none;" cellpadding="0" cellspacing="0"
                            border="0">
                            <tr>
                                <td colspan="2" style="padding: 2px;">
                                    <input type="text" runat="server" style="display: none" id="fileName" />
                                    <asp:HiddenField ID="hdnName" runat="server" />
                                    <asp:HiddenField ID="hdnfullName" runat="server" />
                                    <asp:HiddenField ID="hndIsFtpUpload" runat="server" />
                                    <asp:HiddenField ID="IsAllowFtp" runat="server" />
                                    <%--<asp:Button ID="Button1" Style="display: none" runat="server" OnClick="Button1_Click"
                                                                                                                Text="Button" />--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" width="120px" class="content-text-new" style="padding: 2px;">
                                    File Name :
                                </td>
                                <td align="left" style="padding: 2px;">
                                    <asp:TextBox ID="txtFile" CssClass="grayinput" runat="server" Width="350px"></asp:TextBox>
                                    <%--<asp:Label ID="lblFNAme" CssClass="content-text-new" Style="display: block"
                                                                                                                runat="server"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="content-text-new" style="padding: 2px;">
                                    Date Uploaded :
                                </td>
                                <td align="left" style="padding: 2px;">
                                    <input type="text" class="grayinput" value='<%= System.DateTime.Now %>' readonly="readonly"
                                        style="width: 250px;" />
                                    <%--<asp:Label ID="lblDate" Text="<%= System.DateTime.Now %>" CssClass="content-text-new" Style="display: block"
                                                                                                                runat="server"></asp:Label>--%>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" class="content-text-new" style="padding: 2px;">
                                    Uploaded Status :
                                </td>
                                <td align="left" style="padding: 2px;">
                                    <asp:Label ID="lblUploadedStatus" CssClass="content-text-new" Style="display: block;
                                        width: 240px; text-align: left" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<asp:UpdateProgress ID="UpdateProgressUploadMediaPopop" runat="server" AssociatedUpdatePanelID="upUploadMediaPopop">
        <ProgressTemplate>
            <div style="position: relative">
                <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                    alt="Loading..." id="imgLoading" />
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
</asp:Panel>
<AjaxToolkit:ModalPopupExtender runat="server" ID="mpeUploadMedia" TargetControlID="tgtUpload2"
    PopupControlID="pnlUploadMedia" BehaviorID="mpeUploadMedia" BackgroundCssClass="ModalBackground">
</AjaxToolkit:ModalPopupExtender>
<%--<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="upUploadMediaPopop" TargetControlID="UpdateProgressUploadMediaPopop"
    CssClass="updateProgress" />--%>
<input type="button" id="tgtbtn" runat="server" style="display: none" />
<Ajax:ModalPopupExtender ID="mdlpopupUGC" BackgroundCssClass="ModalBackground" BehaviorID="mdlpopupUGC"
    TargetControlID="tgtbtn" PopupControlID="pnlEditUGC" CancelControlID="btnCancelEdit"
    runat="server">
</Ajax:ModalPopupExtender>
<asp:Panel ID="pnlEditUGC" runat="server" Style="z-index: 999; width: 720px; display: none;">
    <asp:UpdatePanel ID="upEditUGC" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <table width="720" border="0" align="center" cellpadding="0" cellspacing="0" class="popUpInner">
                <tr class="bluebox-hd">
                    <td align="left" style="padding-left: 10px;">
                        Edit Media Details
                    </td>
                    <td align="right" style="padding-right: 10px;">
                        <input type="button" runat="server" id="btnClipClose" class="btn-cancel" value=" "
                            onclick="$find('mdlpopupUGC').hide();" />
                    </td>
                </tr>
                <tr style="padding-left: 17px; background-color: #F8FCFF;" class="blue-content-bg">
                    <td colspan="2">
                        <table id="Table1" border="0" cellpadding="2" cellspacing="0" width="100%" style="padding: 10px;"
                            class="popUpInnerTable">
                            <tr>
                                <td>
                                    <asp:ValidationSummary ID="vlSummeryEditMedia" runat="server" ValidationGroup="vgEditMedia"
                                        ForeColor="#bd0000" Font-Size="Smaller" Style="padding-left: 15px;" />
                                    <asp:Label ID="lblUGCMsg" runat="server" Visible="true" Font-Size="Smaller" ForeColor="#bd0000"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" width="100px" class="content-text-new" style="width: 50%;">
                                    Media Title :
                                </td>
                                <td align="left">
                                    <asp:HiddenField ID="hdnEditUDCID" runat="server" />
                                    <asp:TextBox ID="txtEditUGCTitle" runat="server" CssClass="grayinput" Width="525px"
                                        MaxLength="255"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="rfvtxtClipTitle" Text="*" runat="server" ControlToValidate="txtEditUGCTitle"
                                        ErrorMessage="Title is required." Display="None" ValidationGroup="vgEditMedia"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Owner :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlOwner" runat="server" CssClass="grayselect" Width="245px">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="reqOwner" runat="server" ControlToValidate="ddlOwner"
                                        ErrorMessage="Please Select Owner" Text="*" ValidationGroup="vgEditMedia" Display="None"
                                        InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Category :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlPCategory" CssClass="grayselect" runat="server" Width="245px"
                                        onchange="UpdateSubCategory1(this.id,true);">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlPCategory"
                                        ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgEditMedia"
                                        Display="Dynamic" InitialValue="0"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 1 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlEditSubCategory1" runat="server" CssClass="grayselect" Width="245px"
                                        onchange="UpdateSubCategory1(this.id,true);">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 2 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlEditSubCategory2" runat="server" CssClass="grayselect" Width="245px"
                                        onchange="UpdateSubCategory1(this.id,true);">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Sub Category 3 :
                                </td>
                                <td align="left">
                                    <asp:DropDownList ID="ddlEditSubCategory3" runat="server" CssClass="grayselect" Width="245px"
                                        onchange="UpdateSubCategory1(this.id,true);">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Description :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEditUGCDesc" runat="server" TextMode="MultiLine" CssClass="grayinput"
                                        Style="width: 525px; height: 145px; overflow: auto;">
                                    </asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Text="*" runat="server"
                                        ControlToValidate="txtEditUGCDesc" ErrorMessage="Description is required." Display="None"
                                        ValidationGroup="vgEditMedia"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr style="height: 3px;">
                                <td>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" class="content-text-new">
                                    Keywords :
                                </td>
                                <td align="left">
                                    <asp:TextBox ID="txtEditUGCKeyword" runat="server" TextMode="MultiLine" CssClass="grayinput"
                                        Style="width: 525px; height: 145px; overflow: auto;"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Text="*" runat="server"
                                        ControlToValidate="txtEditUGCKeyword" ErrorMessage="Keywords is required." Display="None"
                                        ValidationGroup="vgEditMedia"></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding: 0px 0px 10px 0px" colspan="2">
                                </td>
                            </tr>
                            <tr>
                                <td align="center" colspan="2">
                                    <input type="button" id="btnCancelEdit" class="btn-blue2" value="Cancel" onclick="$find('mdlpopupUGC').hide();" />
                                    <asp:Button ID="btnUpdateUGC" runat="server" Text="Save" ValidationGroup="vgEditMedia"
                                        CssClass="btn-blue2" OnClick="btnUpdateUGC_Click" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnUpdateUGC" EventName="Click" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Panel>
<%--<asp:UpdateProgress ID="updProgressEditUGC" runat="server" AssociatedUpdatePanelID="upEditUGC">
    <ProgressTemplate>
        <div style="position: relative">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender2"
    ControlToOverlayID="upEditUGC" TargetControlID="updProgressEditUGC" CssClass="updateProgress" />
<asp:UpdateProgress ID="uprogSearchUGCRawMedia" runat="server" AssociatedUpdatePanelID="upUGCSearch"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upoverlaySearchUGCRawMedia"
    ControlToOverlayID="upUGCSearch" TargetControlID="uprogSearchUGCRawMedia" CssClass="updateProgress" />
<asp:UpdateProgress ID="uprogUploadMedia" runat="server" AssociatedUpdatePanelID="upUploadMedia"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upoverlayUploadMedia" ControlToOverlayID="upUploadMedia"
    TargetControlID="uprogUploadMedia" CssClass="updateProgress" />
--%>