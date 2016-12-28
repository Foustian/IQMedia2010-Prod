<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAgentQueryEditor.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.IQAgentQueryEditor.IQAgentQueryEditor" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<script type="text/javascript">
    function CheckUnCheckAll(isCheck, rptChecklist) {
        if (isCheck) {

            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                //$(this).attr("disabled", "disabled");
                $(this).attr("checked", "checked");
            });
        }
        else {
            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                //$(this).removeAttr("disabled");
                $(this).removeAttr("checked");
            });
        }
    }

    function SetChkClickEvent(rptChecklist) {
        $("#" + rptChecklist + " input:checkbox").each(function (index) {
            $(this).click(function () {
                SetListManualOrAllSelected(rptChecklist);
            });
        });
    }

    function SetListManualOrAllSelected(rptChecklist) {
        var IsAllChecked = true;
        $("#" + rptChecklist + " input:checkbox").each(function (index) {
            if ($(this).is(':checked') == false) {
                IsAllChecked = false;
            }
        });
        var radioList = rptChecklist.replace("rpt", "rdo");
        if (IsAllChecked) {
            $("#" + radioList + "").find("input[value='1']").attr("checked", "checked");
        }
        else {
            $("#" + radioList + "").find("input[value='2']").attr("checked", "checked");
        }
    }

    function SetCheckedFromList(StrList, rptChecklist) {
        var ListOfCheckItems = StrList.split(',');
        //$("#" + rptChecklist + " input:checkbox").removeAttr("disabled");
        $("#" + rptChecklist + " input:checkbox").removeAttr("checked");
        for (var i = 0; i < ListOfCheckItems.length; i++) 
        {
            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                if ($(this).val() == ListOfCheckItems[i]) {
                    $(this).attr("checked", "checked");
                }
            });
        }
      
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
        <td valign="top" bgcolor="#FFFFFF" class="pad-bt">
            <div>
                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="padding-bottom: 10px">
                            <div class="AdminTitle">
                                IQ Agent Setup</div>
                        </td>
                    </tr>
                </table>
            </div>
            <br />
            <asp:UpdatePanel ID="updIQAgentQueryEditor" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <div style="padding-left: 20px;">
                        <asp:ValidationSummary ID="ValidationSummary3" EnableClientScript="true" runat="server"
                            DisplayMode="BulletList" ValidationGroup="IQAgentQueryEditor" ForeColor="#bd0000"
                            Font-Size="Smaller" />
                    </div>
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblMsg" ForeColor="#FF0000" Font-Size="Smaller" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                <table style="padding-left: 5px;">
                                    <tr>
                                        <td>
                                            Choose Client
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlClient" runat="server" Width="195" AutoPostBack="True" OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvClient" Text="*" ValidationGroup="IQAgentQueryEditor"
                                                ErrorMessage="Please Select Client" ControlToValidate="ddlClient" InitialValue="-1"
                                                runat="server">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            IQ Agent UserID
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtIQAgentUserID" CssClass="textbox03" Width="185" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Query Name
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <AjaxToolkit:ComboBox ID="ddlQueryName" runat="server" OnItemInserting="ddlQueryName_ItemInserting"
                                                AutoPostBack="true" DropDownStyle="DropDown" AutoCompleteMode="Suggest" CaseSensitive="False"
                                                CssClass="ajaxcomboboxbtyle" Enabled="true" ItemInsertLocation="Append" MaxLength="100" Width="165" OnSelectedIndexChanged="ddlQueryName_SelectedIndexChanged">
                                            </AjaxToolkit:ComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-top:5px;">
                                            Current Query Version
                                        </td>
                                        <td style="padding-top:5px;">
                                            :
                                        </td>
                                        <td style="padding-top:5px;">
                                            <asp:TextBox ID="txtCurrentQueryVersion" Width="185" CssClass="textbox03" runat="server" Enabled="false"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSaveQuery" ValidationGroup="IQAgentQueryEditor" Text="Store Search Query"
                                                runat="server" CssClass="btn-grey" OnClick="btnSaveQuery_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="width: 20px;">
                                &nbsp;
                            </td>
                            <td valign="top">
                                <table>
                                    <tr>
                                        <td>
                                            Search Term
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearchTerm" CssClass="textbox03" Width="185" runat="server"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvSearchTerm" ControlToValidate="txtSearchTerm"
                                                Text="*" ValidationGroup="IQAgentQueryEditor" ErrorMessage="Please Enter Search Term"
                                                runat="server">
                                            </asp:RequiredFieldValidator>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Program Title
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProgramTitle" CssClass="textbox03" Width="185" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>
                                     <tr>
                                        <td>
                                            Appearing
                                        </td>
                                        <td>
                                            :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAppearing" CssClass="textbox03" Width="185" runat="server"></asp:TextBox>
                                        </td>
                                    </tr>

                                </table>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSaveQuery" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>
            <div>
                <asp:UpdatePanel UpdateMode="Conditional" runat="server" ID="upStatSkedProg" >
                <ContentTemplate>
                <asp:Panel ID="pnlStatSkedData" runat="server" >
                <table>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="AdminTitle"> DMA Rank and Name</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="grey-grad2">
                                        
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr height="29px">
                                                        <td class="content-textRd" align="center" valign="top">
                                                            <asp:RadioButtonList ID="rdoMarket" Width="30%" runat="server" CellSpacing="1" CellPadding="1"
                                                                RepeatColumns="2" 
                                                                BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right" Style="vertical-align: middle">
                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Select All</span></asp:ListItem>
                                                                <asp:ListItem Value="2"><span style="height:50px; vertical-align:middle;padding-left:5px">Manual Select</span></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                            
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="content-text">
                                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" style="max-height:300px" Visible="true">
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
                                                            </asp:Panel>
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="AdminTitle"> Affiliate Network</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="grey-grad2">
                                        
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr height="29px">
                                                        <td class="content-textRd" align="center" valign="top">
                                                            <asp:RadioButtonList ID="rdoAffil" Width="30%" runat="server" CellSpacing="1" CellPadding="1"
                                                                RepeatColumns="2" 
                                                                BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right" Style="vertical-align: middle">
                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Select All</span></asp:ListItem>
                                                                <asp:ListItem Value="2"><span style="height:50px; vertical-align:middle;padding-left:5px">Manual Select</span></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="content-text">
                                                            <asp:Panel ID="pnlAffil" runat="server" ScrollBars="Auto" style="max-height:150px" Visible="true">
                                                                <asp:DataList ID="rptAffil" runat="server" RepeatColumns="6" Width="100%" CellPadding="1" CellSpacing="0">
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
                                                            </asp:Panel>
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
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <div class="AdminTitle"> Program Category</div>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="grey-grad2">
                                        
                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                    <tr height="29px">
                                                        <td class="content-textRd" align="center" valign="top">
                                                            <asp:RadioButtonList ID="rdoProgramType" Width="30%" runat="server" CellSpacing="1" CellPadding="1" 
                                                                BorderWidth="0" RepeatDirection="Horizontal" Style="padding-left: -5px;">
                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; vertical-align:middle;padding-left:5px">Select All</span></asp:ListItem>
                                                                <asp:ListItem Value="2"><span style="height:50px; vertical-align:middle;padding-left:5px">Manual Select</span></asp:ListItem>
                                                            </asp:RadioButtonList>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top" class="content-text">
                                                            <asp:Panel ID="pnlProgramType" runat="server" ScrollBars="Auto" style="max-height:150px" Visible="true">
                                                                <asp:DataList ID="rptProgramType" runat="server" RepeatColumns="6" CellPadding="1" CellSpacing="0">
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
                                                            </asp:Panel>
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
                            &nbsp;
                        </td>
                    </tr>
                </table>
                </asp:Panel>
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
<asp:UpdateProgress ID="updProgressGrid" runat="server" AssociatedUpdatePanelID="updIQAgentQueryEditor">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
    ControlToOverlayID="updIQAgentQueryEditor" TargetControlID="updProgressGrid"
    CssClass="updateProgress" />
<%--<asp:UpdateProgress ID="updDMANameProgress" runat="server" AssociatedUpdatePanelID="updDMAName"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender2"
    ControlToOverlayID="updDMAName" TargetControlID="updDMANameProgress" CssClass="updateProgress" />
<asp:UpdateProgress ID="updAffiliateProgrss" runat="server" AssociatedUpdatePanelID="updAffiliate"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender3"
    ControlToOverlayID="updAffiliate" TargetControlID="updAffiliateProgrss" CssClass="updateProgress" />
<asp:UpdateProgress ID="updProgramTypeProgress" runat="server" AssociatedUpdatePanelID="updProgramType"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender4"
    ControlToOverlayID="updProgramType" TargetControlID="updProgramTypeProgress"
    CssClass="updateProgress" />
<asp:UpdateProgress ID="updProgramCategoryProgress" runat="server" AssociatedUpdatePanelID="updProgramCategory"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender5"
    ControlToOverlayID="updProgramCategory" TargetControlID="updProgramCategoryProgress"
    CssClass="updateProgress" />--%>