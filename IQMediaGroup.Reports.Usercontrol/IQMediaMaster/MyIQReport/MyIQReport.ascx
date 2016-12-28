<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MyIQReport.ascx.cs"
    Inherits="IQMediaGroup.Reports.Usercontrol.IQMediaMaster.MyIQReport.MyIQReport" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<div class="clear">
    <asp:UpdatePanel ID="upBtnReport" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="span3">
                <div class="well sidebar-nav" id="divFilterSide">
                    <asp:Label ID="lblReportErr" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                    <asp:ValidationSummary ID="vlsReport" runat="server" ValidationGroup="vgReport" />
                    <ul class="nav nav-list marginleft-28">
                        <li class="ulheader">
                            <div class="lidivLeft">
                                &nbsp; REPORT</div>
                        </li>
                    </ul>
                    <div id="divReportFilter" class="clear">
                        <ul class="nav nav-list listBorder filterul" id="ulSearchFilter">
                            <li class="ulhead">Filter</li>
                            <li id="liReportFilterInner">
                                <ul class="nav">
                                    <li id="liReportTypeFilter">
                                        <div id="divReportTypeFilter">
                                            <div class="width100p">
                                                <div class="marginbottom5">
                                                    Report Type :</div>
                                                <asp:DropDownList ID="ddlReportType" ValidationGroup="vgReport" runat="server" CssClass="width90">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ValidationGroup="vgReport"
                                                    InitialValue="0" ControlToValidate="ddlReportType" Display="None" ErrorMessage="Please Select Report Type">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                            <div id="ReportFilters" class="clear paddingtop15">
                                                <div id="divDailyReport" runat="server">
                                                    From Date :
                                                    <asp:TextBox ValidationGroup="vgReport" ID="txtFromDate" runat="server" AutoCompleteType="None"
                                                        CssClass="programTextbox"></asp:TextBox>
                                                    <Ajax:CalendarExtender ID="CalendarExtender1" runat="server" CssClass="MyCalendar"
                                                        TargetControlID="txtFromDate">
                                                    </Ajax:CalendarExtender>
                                                    <asp:RequiredFieldValidator ValidationGroup="vgReport" ID="rfvtxtFromDate" runat="server"
                                                        ControlToValidate="txtFromDate" ErrorMessage="Please Enter Report Date" Display="None">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="CompareValidator1" ControlToValidate="txtFromDate" Display="None"
                                                        runat="server" Operator="DataTypeCheck" Type="Date" ErrorMessage="Please Enter Valid From Date"
                                                        ValidationGroup="vgReport"></asp:CompareValidator>
                                                    <br />
                                                    To Date :
                                                    <asp:TextBox ValidationGroup="vgReport" ID="txtToDate" runat="server" AutoCompleteType="None"
                                                        CssClass="programTextbox"></asp:TextBox>
                                                    <Ajax:CalendarExtender ID="CalendarExtender2" runat="server" CssClass="MyCalendar"
                                                        TargetControlID="txtToDate">
                                                    </Ajax:CalendarExtender>
                                                    <asp:RequiredFieldValidator ValidationGroup="vgReport" ID="RequiredFieldValidator4"
                                                        runat="server" ControlToValidate="txtToDate" ErrorMessage="Please Enter To Date"
                                                        Display="None">
                                                    </asp:RequiredFieldValidator>
                                                    <asp:CompareValidator ID="CompareValidator2" ControlToValidate="txtToDate" Display="None"
                                                        runat="server" Operator="DataTypeCheck" Type="Date" ErrorMessage="Please Enter Valid To Date"
                                                        ValidationGroup="vgReport"></asp:CompareValidator>
                                                    <asp:CompareValidator ValidationGroup="vgReport" ID="cmpDateValidator" runat="server"
                                                        ControlToCompare="txtToDate" ControlToValidate="txtFromDate" Text="*" ErrorMessage="To Date Must be greater than From Date"
                                                        Operator="LessThanEqual" Type="Date" Display="Dynamic">
                                                    </asp:CompareValidator>
                                                    <br />
                                                </div>
                                                <div>
                                                    <div class="marginbottom5">
                                                        Category :</div>
                                                    <asp:DropDownList ID="ddlCategory" runat="server" CssClass="width90">
                                                    </asp:DropDownList>
                                                    <br />
                                                    <br />
                                                </div>
                                                <div>
                                                    <img id="ReportSelectionExpandimg" src="../images/collapse_icon.png" onclick="OnImageExpandCollapseClick('ReportSelectionExpandimg','divReportSelection');"
                                                        alt="" /><asp:CheckBox ID="chkReportSelectAll" runat="server" Text="Select All" Checked="true" />
                                                    <div id="divReportSelection" class="div-subchecklist" style="display: block;">
                                                        <input type="checkbox" id="chkReportTV" runat="server" value="title" checked="checked"
                                                            onclick="setCheckbox('divReportSelection','UCMyIQReport1_chkReportSelectAll')" /><span>TV</span><br />
                                                        <input type="checkbox" id="chkReportNews" runat="server" value="description" checked="checked"
                                                            onclick="setCheckbox('divReportSelection','UCMyIQReport1_chkReportSelectAll')" /><span
                                                                id="spnOnlineNews" runat="server">Online News</span><br />
                                                        <input type="checkbox" id="chkReportSM" runat="server" value="keywords" checked="checked"
                                                            onclick="setCheckbox('divReportSelection','UCMyIQReport1_chkReportSelectAll')" /><span
                                                                id="spnSocialMedia" runat="server">Social Media</span><br />
                                                        <input type="checkbox" id="chkReportTwitter" runat="server" value="keywords" checked="checked"
                                                            onclick="setCheckbox('divReportSelection','UCMyIQReport1_chkReportSelectAll')" /><span
                                                                id="spnTwitter" runat="server">Twitter</span><br />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                        <div class="divApply">
                            <asp:Button ID="btnReport" ValidationGroup="vgReport" OnClick="btnReport_Click" runat="server"
                                Text="Show Report" CssClass="btn-blue2 width90" /><br />
                        </div>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upMainGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="span9 mraginleft2p">
                <div class="navbar">
                    <asp:Panel ID="divReportResult" runat="server">
                        <div class="show-hide cursor center" onclick="ShowHideDivResult('divReport',true);">
                            <div class="float-left">
                                <a href="javascript:;">REPORT RESULT </a>
                            </div>
                            <div class="float-right">
                                <a href="javascript:;" class="right-dropdown">
                                    <div class="float-left" id="divReportShowHideTitle">
                                        HIDE
                                    </div>
                                    <img id="imgdivReportShowHide" class="imgshowHide" src="../images/hiden.png" alt="">
                                </a>
                            </div>
                        </div>
                        <div id="divReport" class="clear">
                            <div class="float-left">
                                <asp:Label ID="lblReportSuccessMsg" runat="server" ForeColor="Green"></asp:Label>
                                <asp:Label ID="lblReportErrorMsg" runat="server" ForeColor="Red"></asp:Label>
                            </div>
                            <div class="clear">
                            </div>
                            <div class="float-left margintop8" style="font-size: 14px; line-height: 20px"
                                runat="server" id="divClientLogo">
                                <img id="imgReportClientLogo" runat="server" alt="" />
                            </div>
                            <div class="float-left show-hide margintop8" style="font-size: 14px; line-height: 20px"
                                runat="server" id="divReportHeader">
                                <asp:Label ID="lblReportHeader" runat="server"></asp:Label>
                            </div>
                            <div class="float-right padding5">
                                <input type="button" class="btn-blue2" runat="server" id="btnReportEmail" title="Email Report"
                                    value="Email Report" onclick="return OpenReportEmailPopupReport();" />
                                <asp:Button ID="btnReportPdfDownload" runat="server" ToolTip="Download PDF" CssClass="btn-blue2"
                                    Text="Download PDF" OnClick="btnReportPdfDownload_Click" />
                                <asp:Button ID="btnReportCsvDownload" runat="server" ToolTip="Download CSV" CssClass="btn-blue2"
                                    Text="Download CSV" OnClick="btnReportCsvDownload_Click" />
                            </div>
                            <div class="clear">
                            </div>
                            <div id="divTVReport" runat="server">
                            </div>
                            <div id="divNewsReport" runat="server" class="marginbottom8">
                            </div>
                            <div id="divSocialMediaReport" runat="server" class="marginbottom8">
                            </div>
                            <div id="divTwitterReport" runat="server" class="marginbottom8">
                            </div>
                            <div id="divSummary" runat="server" class="marginbottom8">
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnReportPdfDownload" />
            <asp:PostBackTrigger ControlID="btnReportCsvDownload" />
        </Triggers>
    </asp:UpdatePanel>
</div>
<div id="diviFrameArticle" runat="server" class="modal hide fade resizable modalPopupDiv iframePopup"
    style="display: none;">
    <div style="width: 27px; float: right;" id="divCloseiFrameArticle">
        <div>
            <img id="img3" class="popup-top-close" src="../Images/close-icon.png" onclick="CloseNewsIframeReport();" />
        </div>
    </div>
    <iframe id="iFrameReportArticle" style="width: 100%; height: 100%;" scrolling="auto"
        frameborder="0" src=""></iframe>
    <div style="width: 100%; text-align: right; position: absolute; margin: -50px 0 0 -25px;">
        <asp:Button ID="btnPrint" Text="Print Article" CssClass="btn-green" runat="server"
            OnClientClick="javascript:PrintIframeReport();return false;" />
        <asp:HiddenField ID="hfarticleID" runat="server" />
        <asp:HiddenField ID="hdnArticleType" runat="server" />
    </div>
</div>
<asp:Panel ID="divClipPlayer" runat="server" Style="display: none;" CssClass="modal hide fade resizable playerPopUp">
    <asp:UpdatePanel ID="upClipPlayer" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div style="width: 27px; float: right;" id="divClose" runat="server">
                <img id="lbtnCancelPlayer" class="popup-top-close" src="../Images/close-icon.png"
                    onclick="ClosePlayer();" />
                <%--onclick="ClosePlayer();" />--%>
                <%--<input type="image" id="lbtnCancelPlayer" class="popup-top-close" runat="server"
                    onclick="ClosePlayer();" src="~/Images/close-icon.png" />--%>
            </div>
            <div id="divIFrameMain" style="width: 885px; padding: 15px 15px 15px 0; height: 340px;">
                <div id="DivCaption" runat="server" class="Caption" style="margin-left: 15px;">
                </div>
                <div id="divShowCaption" style="float: left;">
                    <img src="../../../Images/right_arrow_cc.gif" id="imgCCDirection" alt="Show CC" />
                </div>
                <div style="width: 545px; height: 340px; float: right; background-color: Black;"
                    id="divRawMedia" runat="server">
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<asp:Panel ID="pnlMailPanel" runat="server" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;">
    <asp:UpdatePanel ID="upMail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:HiddenField ID="hdnEmailType" runat="server" Value="Search" />
            <div class="popContainMain" id="pnlEmailInner1">
                <div class="popup-hd">
                    <span id="emailTitleSpan">Share This Report</span>
                    <div style="width: 27px; float: right;" id="div4" runat="server">
                        <img id="lbtnCancelPopUp" onclick="closeModal('pnlMailPanel');" src="../Images/close-icon.png" />
                        <%--<input type="image" id="lbtnCancelPopUp" class="popup-top-close" runat="server" onclick="closeModal('pnlMailPanel');"
                            src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div class="float-right">
                        <input type="button" runat="server" id="lbtnCancelPopUp" class="btn-cancel" value=" "
                            onclick="closeModal('pnlMailPanel');" />
                    </div>--%>
                </div>
                <div id="pnlEmailInner" style="background-color: #F8FCFF;" class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="ValidationSummary1" EnableClientScript="true" runat="server"
                            ValidationGroup="validate1" ForeColor="#bd0000" Font-Size="Smaller" />
                        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="red" Font-Size="Medium"></asp:Label>
                        <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>Your Email Address :</li>
                        <li>
                            <asp:TextBox ID="txtYourEmail" runat="server"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="regEmailGrid" runat="server" Display="None" ValidationGroup="validate1"
                                ControlToValidate="txtYourEmail" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                                ErrorMessage="Please Enter Valid Email Address" Text=""></asp:RegularExpressionValidator>
                            <asp:RequiredFieldValidator ID="rfvEmail" Text="" ValidationGroup="validate1" Display="None"
                                runat="server" ControlToValidate="txtYourEmail" ErrorMessage="Please Enter Your Email Address."></asp:RequiredFieldValidator>
                        </li>
                        <li>Friends's Email Address (separate multiple addresses with semicolon) :</li>
                        <li>
                            <asp:TextBox ID="txtFriendsEmail" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Text="" ValidationGroup="validate1"
                                Display="None" runat="server" ControlToValidate="txtFriendsEmail" ErrorMessage="Please Enter Friend's Email Address."></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                                ValidationGroup="validate1" ControlToValidate="txtFriendsEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                                ErrorMessage="Please Enter Valid Friend's Email Address" Text=""></asp:RegularExpressionValidator>
                        </li>
                        <li>Subject :</li>
                        <li>
                            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" Text="" ValidationGroup="validate1"
                                Display="None" runat="server" ControlToValidate="txtSubject" ErrorMessage="Please Enter Subject."></asp:RequiredFieldValidator>
                        </li>
                        <li>Message :</li>
                        <li>
                            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
                        </li>
                    </ul>
                    <div class="center">
                        <%--<input type="button" id="btnCancel" value="Cancel" class="btn-blue2" onclick="$find('mdlpopupEmail').hide();" />--%>
                        <input type="button" id="btnCancel" value="Cancel" class="btn-blue2" onclick="closeModal('pnlMailPanel');" />
                        <asp:Button ID="btnOK" CssClass="btn-blue2" runat="server" Width="51px" Text="OK"
                            ValidationGroup="validate1" OnClick="btnOK_Click"></asp:Button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
