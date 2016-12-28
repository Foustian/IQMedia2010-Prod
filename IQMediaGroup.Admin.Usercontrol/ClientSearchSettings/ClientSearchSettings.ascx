<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClientSearchSettings.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.ClientSearchSettings.ClientSearchSettings" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<script src="../Script/jquery.tabify-1.51/jquery.tabify.js" type="text/javascript"
    charset="utf-8"></script>
<style type="text/css" media="screen">
    .menu
    {
        padding: 0;
        clear: both;
    }
    .menu li
    {
        display: inline;
    }
    .menu li a
    {
        background: #E5E5E4;
        padding: 10px;
        float: left;
        border-bottom: none;
        text-decoration: none;
        color: #000;
        font-weight: bold;
    }
    .menu li.active a
    {
        background: #cdcdcd;
    }
    
    .menu li.active input
    {
        background: #cdcdcd;
        padding:15px 4px;
    }
    .menu li input
    {
        background: #E5E5E4;
        padding:15px 4px;
    }
    
    .content
    {
        float: left;
        clear: both;
        border: 1px solid #E5E5E4;
        border-top: none;
        border-left: none;
        background: #cdcdcd;
        padding: 10px 20px 20px;
    }
</style>
<script type="text/javascript">
    function CheckUnCheckAll(isCheck, rptChecklist) {
        if (isCheck) {

            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                //$(this).attr("disabled", "disabled");
                $(this).attr("checked", "checked");
            });
            var radioList = rptChecklist.replace("rpt", "rdo");
            $("#" + radioList + "").find("input[value='1']").attr("checked", "checked");
        }
        else {
            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                //$(this).removeAttr("disabled");
                $(this).removeAttr("checked");
            });
        }
    }

    function SetCheckedList(rptChecklist) {
        var IsAllChecked = true;
        $("#" + rptChecklist + " input:checkbox").each(function (index) {
            if ($(this).is(':checked') == false) {
                IsAllChecked = false;
            }
        });
        var radioList = rptChecklist.replace("rpt", "rdo");
        if (IsAllChecked) {
            $("#" + radioList + "").find("input[value='1']").attr("checked", "checked");
            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                //$(this).attr("disabled", "disabled");
                $(this).attr("checked", "checked");
            });
        }
        else {
            $("#" + radioList + "").find("input[value='2']").attr("checked", "checked");
        }
    }


    function SetChkClickEvent(rptChecklist) {
        $("#" + rptChecklist + " input:checkbox").each(function (index) {
            $(this).click(function () {
                SetCheckedList(rptChecklist);
            });
        });
    }


    function ValidateLists() {

        if ($("#<%=ddlClient.ClientID %>").val() == "-1") {
            document.getElementById("<%=lblErrorMessage.ClientID %>").innerHTML = "Please Select Client";
            return false;
        }

        var rptList = document.getElementById("<%= rptMarket.ClientID %>");
        var rptinps = rptList.getElementsByTagName("input");
        var IsRptListSel = false;
        for (var i = 0; i < rptinps.length; i++) {
            if (rptinps[i].type === "checkbox" && rptinps[i].checked) {

                IsRptListSel = true;
                break;
            }
        }
        if (IsRptListSel == false) {
            document.getElementById("<%=lblErrorMessage.ClientID %>").innerHTML = "Please Select Altest One Item for DMA Rank and Name";
            return false;
        }

        rptList = document.getElementById("<%= rptAffil.ClientID %>");
        rptinps = rptList.getElementsByTagName("input");
        IsRptListSel = false;
        for (var i = 0; i < rptinps.length; i++) {
            if (rptinps[i].type === "checkbox" && rptinps[i].checked) {

                IsRptListSel = true;
                break;
            }
        }
        if (IsRptListSel == false) {
            document.getElementById("<%=lblErrorMessage.ClientID %>").innerHTML = "Please Select Altest One Item for Affiliate Network";
            return false;
        }

        rptList = document.getElementById("<%= rptProgramType.ClientID %>");
        rptinps = rptList.getElementsByTagName("input");
        IsRptListSel = false;
        for (var i = 0; i < rptinps.length; i++) {
            if (rptinps[i].type === "checkbox" && rptinps[i].checked) {

                IsRptListSel = true;
                break;
            }
        }
        if (IsRptListSel == false) {
            document.getElementById("<%=lblErrorMessage.ClientID %>").innerHTML = "Please Select Altest One Item for Program Category";
            return false;
        }


        return true;
    }


</script>
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
        <td valign="top" bgcolor="#FFFFFF">
            <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="upStatSkedProg">
                <ContentTemplate>
                    <table width="100%" cellspacing="0" cellpadding="0" border="0" align="center">
                        <tr>
                            <td>
                                <table width="100%" class="border01" style="vertical-align: middle" border="0" align="center"
                                    cellpadding="0" cellspacing="3">
                                    <tr>
                                        <td style="padding-bottom: 10px">
                                            <div class="AdminTitle">
                                                Client Search Settings Setup Page</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="3" cellpadding="0">
                                                <tr>
                                                    <td width="2%">
                                                    </td>
                                                    <td width="98%" align="left" valign="top">
                                                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red"></asp:Label>
                                                        <asp:Label ID="lblSuccessMessage" runat="server" ForeColor="Green"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" cellspacing="3" cellpadding="0">
                                                <tr style="height: 20px">
                                                    <td style="width: 100px;">
                                                        Choose Client :
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlClient" AutoPostBack="true" runat="server" Width="195" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td height="10x">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td height="3">
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div style="padding: 5px 5px 5px 1px;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="SearchSettings" runat="server" visible="false" width="100%" class="border01"
                                    style="vertical-align: middle" border="0" align="center">
                                    <tr>
                                        <td style="padding-bottom: 10px">
                                            <div class="AdminTitle">
                                                Search Settings</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div id="tabs">
                                                <ul id="statskedmenu" class="menu">
                                                    <li class="active">
                                                            <a href="#dma">DMA Rank and Name</a>
                                                        <div style="float: left; background-color: red; height: 33px">
                                                            <asp:ImageButton AlternateText="" style="border-left: 1px solid #cdcdcd;border-right: 1px solid #cdcdcd;" ID="imgsortdma" runat="server" ImageUrl="" OnClick="SortSelection" />
                                                        </div>
                                                    </li>
                                                    <li>
                                                        <a href="#stationaffil">Affiliate Network</a>
                                                        <div style="float: left; background-color: red; height: 33px">
                                                            <asp:ImageButton AlternateText="" style="border-left: 1px solid #cdcdcd;border-right: 1px solid #cdcdcd;" ID="imgsortaffil" runat="server" ImageUrl="" OnClick="SortSelection" /></div>
                                                    </li>
                                                    <li><a href="#programcategory">Program Category</a></li>
                                                </ul>
                                                <div id="dma" class="content">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="grey-grad2">
                                                        <tr height="29px">
                                                            <td class="content-textRd" align="left" valign="top">
                                                                <asp:RadioButtonList ID="rdoSortMarket" AutoPostBack="true" Width="30%" runat="server"
                                                                    CellSpacing="1" CellPadding="1" RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal"
                                                                    TextAlign="Right" Style="vertical-align: middle" OnSelectedIndexChanged="SortSelection">
                                                                    <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">DMA Rank</span></asp:ListItem>
                                                                    <asp:ListItem Value="0"><span style="height:50px; vertical-align:middle;padding-left:5px">DMA Name</span></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr height="29px">
                                                            <td class="content-textRd" align="center" valign="top">
                                                                <asp:RadioButtonList ID="rdoMarket" Width="30%" runat="server" CellSpacing="1" CellPadding="1"
                                                                    RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right"
                                                                    Style="vertical-align: middle">
                                                                    <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Select All</span></asp:ListItem>
                                                                    <asp:ListItem Value="2"><span style="height:50px; vertical-align:middle;padding-left:5px">Manual Select</span></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" class="content-text">
                                                                <asp:DataList ID="rptMarket" runat="server" RepeatColumns="6" Width="100%" CellPadding="1"
                                                                    CellSpacing="0">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                            <tr height="29px;">
                                                                                <td width="15" height="100%" valign="top">
                                                                                    <%# Container.ItemIndex+1 %>
                                                                                </td>
                                                                                <td width="107" height="100%" valign="top">
                                                                                    <asp:Label ID="lblMarket" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IQ_Dma_Name") %>'
                                                                                        CssClass="content-text"></asp:Label>
                                                                                </td>
                                                                                <td valign="top" height="100%">
                                                                                    <input type="checkbox" id="chkSelectMarket" runat="server" value='<%# Eval("IQ_Dma_Num") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="stationaffil" class="content">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="grey-grad2">
                                                        <tr height="29px">
                                                            <td class="content-textRd" align="left" valign="top">
                                                                <asp:RadioButtonList ID="rdoSortAffil" AutoPostBack="true" Width="30%" runat="server"
                                                                    CellSpacing="1" CellPadding="1" RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal"
                                                                    TextAlign="Right" Style="vertical-align: middle" OnSelectedIndexChanged="SortSelection">
                                                                    <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Affiliate Number</span></asp:ListItem>
                                                                    <asp:ListItem Value="0"><span style="height:50px; vertical-align:middle;padding-left:5px">Affiliate Name</span></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr height="29px">
                                                            <td class="content-textRd" align="center" valign="top">
                                                                <asp:RadioButtonList ID="rdoAffil" Width="30%" runat="server" CellSpacing="1" CellPadding="1"
                                                                    RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right"
                                                                    Style="vertical-align: middle">
                                                                    <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Select All</span></asp:ListItem>
                                                                    <asp:ListItem Value="2"><span style="height:50px; vertical-align:middle;padding-left:5px">Manual Select</span></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" class="content-text">
                                                                <asp:DataList ID="rptAffil" runat="server" RepeatColumns="6" Width="100%" CellPadding="1"
                                                                    CellSpacing="0">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                            <tr height="29px;">
                                                                                <td width="15" height="100%" valign="top">
                                                                                    <%# Container.ItemIndex+1 %>
                                                                                </td>
                                                                                <td width="107" height="100%" valign="top">
                                                                                    <asp:Label ID="lblAffil" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.Station_Affil") %>'
                                                                                        CssClass="content-text"></asp:Label>
                                                                                </td>
                                                                                <td height="100%" valign="top">
                                                                                    <input type="checkbox" id="chkSelectAffil" runat="server" value='<%# Eval("Station_Affil_Num") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <div id="programcategory" class="content">
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0" class="grey-grad2">
                                                        <tr height="29px">
                                                            <td class="content-textRd" align="center" valign="top">
                                                                <asp:RadioButtonList ID="rdoProgramType" Width="30%" runat="server" CellSpacing="1"
                                                                    CellPadding="1" BorderWidth="0" RepeatDirection="Horizontal" Style="padding-left: -5px;">
                                                                    <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Select All</span></asp:ListItem>
                                                                    <asp:ListItem Value="2"><span style="height:50px; vertical-align:middle;padding-left:5px">Manual Select</span></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" class="content-text">
                                                                <asp:DataList ID="rptProgramType" RepeatDirection="Horizontal" runat="server" RepeatColumns="6"
                                                                    CellPadding="1" CellSpacing="0">
                                                                    <ItemTemplate>
                                                                        <table cellpadding="0" cellspacing="0" class="content-textRd">
                                                                            <tr height="29px;">
                                                                                <td width="15" height="100%" valign="top">
                                                                                    <%# Container.ItemIndex+1 %>
                                                                                </td>
                                                                                <td width="107" height="100%" valign="top">
                                                                                    <asp:Label ID="lblProgramType" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.IQ_Class") %>'
                                                                                        CssClass="content-text"></asp:Label>
                                                                                </td>
                                                                                <td height="100%" valign="top">
                                                                                    <input type="checkbox" id="chkSelectProgramType" runat="server" value='<%# Eval("IQ_Class_Num") %>' />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </ItemTemplate>
                                                                </asp:DataList>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" style="padding-right:10px" >
                                            <asp:Button ID="btnSave" Width="150px" CssClass="btn-blue2" runat="server" Text="Save"
                                                OnClick="btnSave_Click" OnClientClick="return ValidateLists();" />
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
<asp:UpdateProgress ID="updProgressGrid" runat="server" AssociatedUpdatePanelID="upStatSkedProg">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="upStatSkedProg" TargetControlID="updProgressGrid" CssClass="updateProgress" />
<script type="text/javascript">
    $(document).ready(function () {
        // bind your jQuery events here initially
        $('#statskedmenu').tabify();
        //$("#tabs").tabs();
    });

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_endRequest(function () {
        // re-bind your jQuery events here
        $('#statskedmenu').tabify();
        //$("#tabs").tabs();
    });
</script>
