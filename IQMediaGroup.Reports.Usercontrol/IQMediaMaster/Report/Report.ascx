<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Report.ascx.cs" Inherits="IQMediaGroup.Reports.Usercontrol.IQMediaMaster.Report.Report" %>
<%@ Register Src="../IframeRawMediaReportH/IframeRawMediaReportH.ascx" TagName="IframeRawMediaH"
    TagPrefix="uc1" %>
<asp:UpdatePanel ID="upReport" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:HiddenField ID="hfReportID" runat="server" />
        <div class="result-in-bg">
            <asp:Label ID="lblReportErrorMsg" runat="server" ForeColor="Red" CssClass="center padding5"></asp:Label>
            <asp:Panel ID="divReportResult" runat="server" CssClass="clear">
                <div id="divReport" class="clear">
                    <div class="float-left">
                        <asp:Label ID="lblReportMsg" runat="server" ForeColor="Red"></asp:Label>
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
                    <asp:Panel ID="divReportHeader" runat="server" CssClass="float-left margintop8" Style="font-size: 14px;
                        line-height: 20px">
                        <asp:Label ID="lblReportHeader" runat="server" Text="Report"></asp:Label>
                    </asp:Panel>
                    <div class="clear">
                    </div>
                    <asp:Panel ID="divTVReport" runat="server">
                    </asp:Panel>
                    <asp:Panel ID="divNewsReport" runat="server">
                    </asp:Panel>
                    <asp:Panel ID="divSocialReport" runat="server">
                    </asp:Panel>
                    <asp:Panel ID="divTwitterReport" runat="server">
                    </asp:Panel>
                </div>
            </asp:Panel>
        </div>
    </ContentTemplate>
    <Triggers>
        <asp:PostBackTrigger ControlID="btnReportPdfDownload" />
        <asp:PostBackTrigger ControlID="btnReportCsvDownload" />
    </Triggers>
</asp:UpdatePanel>
<asp:Panel ID="pnlMailPanel" runat="server" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;">
    <asp:UpdatePanel ID="upMail" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain" id="pnlEmailInner1">
                <div class="popup-hd">
                    <span id="emailTitleSpan">Share This Report</span>
                    <div style="width: 27px; float: right;" id="div4" runat="server">
                        <img id="lbtnCancelPopUp" alt="" onclick="closeModal('pnlMailPanel');" src="../Images/close-icon.png" />
                    </div>
                </div>
                <div id="pnlEmailInner" style="background-color: #F8FCFF;" class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="ValidationSummary1" EnableClientScript="true" runat="server"
                            ValidationGroup="validate1" ForeColor="#bd0000" Font-Size="Smaller" />
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
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="" ValidationGroup="validate1"
                                Display="None" runat="server" ControlToValidate="txtFriendsEmail" ErrorMessage="Please Enter Friend's Email Address."></asp:RequiredFieldValidator>
                            <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="None"
                                ValidationGroup="validate1" ControlToValidate="txtFriendsEmail" ValidationExpression="\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*([;]\s*\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*)*"
                                ErrorMessage="Please Enter Valid Friend's Email Address" Text=""></asp:RegularExpressionValidator>
                        </li>
                        <li>Subject :</li>
                        <li>
                            <asp:TextBox ID="txtSubject" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Text="" ValidationGroup="validate1"
                                Display="None" runat="server" ControlToValidate="txtSubject" ErrorMessage="Please Enter Subject."></asp:RequiredFieldValidator>
                        </li>
                        <li>Message :</li>
                        <li>
                            <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="10"></asp:TextBox>
                        </li>
                    </ul>
                    <div class="center">
                        <input type="button" id="btnCancel" value="Cancel" class="btn-blue2" onclick="closeModal('pnlMailPanel');" />
                        <asp:Button ID="btnOK" CssClass="btn-blue2" runat="server" Width="51px" Text="OK"
                            ValidationGroup="validate1" OnClick="btnOK_Click"></asp:Button>
                    </div>
                </div>
            </div>
        </ContentTemplate>
        <%--  <Triggers>
            <asp:AsyncPostBackTrigger ControlID="btnOK" EventName="Click" />
        </Triggers>--%>
    </asp:UpdatePanel>
</asp:Panel>
<div id="diviFrameArticle" runat="server" class="modal hide fade resizable playerPopUp in"
    style="display: none;">
    <div style="width: 27px; float: right;" id="divCloseiFrameArticle">
        <div>
            <img id="img3" class="popup-top-close" src="../Images/close-icon.png" onclick="CloseNewsIframeReport();" />
        </div>
    </div>
    <%--<asp:UpdatePanel ID="upReportArticle" runat="server" UpdateMode="Conditional" style="width: 100%;
        height: 100%;">
        <ContentTemplate>--%>
    <iframe id="iFrameReportArticle" style="width: 100%; height: 100%;" scrolling="auto"
        frameborder="0" src=""></iframe>
    <%--</ContentTemplate>
    </asp:UpdatePanel>--%>
    <div style="width: 100%; text-align: right; position: absolute; margin: -50px 0 0 -25px;">
        <asp:Button ID="btnSave" Text="Save Article" CssClass="btn-green" runat="server"
            OnClientClick="CloseNewsIframeReport();return OpenSaveArticlePopup(0);" />
        <asp:Button ID="btnPrint" Text="Print Article" CssClass="btn-green" runat="server"
            OnClientClick="javascript:PrintIframeReport();return false;" />
        <asp:HiddenField ID="hfarticleID" runat="server" />
        <asp:HiddenField ID="hdnArticleType" runat="server" />
    </div>
</div>
<asp:Panel ID="pnlSaveArticle" CssClass="modal hide fade resizable modalPopupDiv height80p"
    Style="display: none;" runat="server" DefaultButton="btnSaveArticle">
    <asp:UpdatePanel ID="upSaveArticle" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="popContainMain">
                <div class="popup-hd">
                    <span id="spnSaveArticleTitle">Article Details</span>
                    <div style="width: 27px; float: right;" id="div7" runat="server">
                        <img id="btnClose" src="../Images/close-icon.png" onclick="closeModal('pnlSaveArticle');" />
                        <%--<input type="image" id="imgEditArticleClos" class="popup-top-close" runat="server"
                            onclick="closeModal('pnlEditArticle');" src="~/Images/close-icon.png" />--%>
                    </div>
                    <%--<div clchass="pnlPopup-right-close">
                        <input type="button" id="btnClose" class="btn-cancel" value=" " onclick="$find('mdlpopupSaveArticle').hide();" />
                    </div>--%>
                </div>
                <div class="blue-content-bg height90p">
                    <div>
                        <asp:ValidationSummary ID="vlSummerySaveArticle" runat="server" ValidationGroup="vgSaveArticle"
                            CssClass="error-summery" />
                        <asp:Label ID="lblSaveArticleMsg" runat="server" Visible="true" CssClass="MsgFail"></asp:Label>
                    </div>
                    <ul class="registration">
                        <li>
                            <label>
                                Title :</label>
                            <asp:HiddenField ID="hdnSaveArticleID" runat="server" />
                            <asp:TextBox ID="txtArticleTitle" runat="server" Width="74%" MaxLength="250"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtArticleTitle" Text="*" runat="server" ControlToValidate="txtArticleTitle"
                                ErrorMessage="Title is required." Display="None" ValidationGroup="vgSaveArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Primary Category :</label>
                            <asp:DropDownList ID="ddlPCategory" runat="server" onclick="selectList_cache=this.value"
                                onchange="return UpdateSubCategory1(this.id);" Width="36%">
                            </asp:DropDownList>
                            <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="ddlPCategory"
                                ErrorMessage="Please Select Category" Text="*" ValidationGroup="vgSaveArticle"
                                Display="None" InitialValue="0"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Sub Category 1 :</label>
                            <asp:DropDownList ID="ddlSubCategory1" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 2 :</label>
                            <asp:DropDownList ID="ddlSubCategory2" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Sub Category 3 :</label>
                            <asp:DropDownList ID="ddlSubCategory3" runat="server" onchange="UpdateSubCategory1(this.id);"
                                Width="36%">
                            </asp:DropDownList>
                        </li>
                        <li>
                            <label>
                                Description :</label>
                            <asp:TextBox ID="txtADescription" MaxLength="1000" runat="server" TextMode="MultiLine"
                                Style="width: 74%; height: 145px; overflow: auto;">
                            </asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtDescription" Text="*" runat="server" ControlToValidate="txtADescription"
                                ErrorMessage="Description is required." Display="None" ValidationGroup="vgSaveArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Keywords :</label>
                            <asp:TextBox ID="txtKeywords" runat="server" MaxLength="500" TextMode="MultiLine"
                                Style="width: 74%; height: 145px; overflow: auto;"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvtxtKeywords" Text="*" runat="server" ControlToValidate="txtKeywords"
                                ErrorMessage="Keywords is required." Display="None" ValidationGroup="vgSaveArticle"></asp:RequiredFieldValidator>
                        </li>
                        <li>
                            <label>
                                Rate This Article :</label>
                            <div>
                                <div>
                                    <span class="float-left RateArticleNumberLeft" style="">1</span> <span class="float-left">
                                        <asp:TextBox ID="txtArticleRate" runat="server" Style="display: none;" onchange="sliderChange('txtArticleRate','chkArticlePreferred')">
                                        </asp:TextBox>
                                        <AjaxToolkit:SliderExtender ID="seArticle" runat="server" TargetControlID="txtArticleRate"
                                            Minimum="1" Maximum="6" Steps="6" RaiseChangeOnlyOnMouseUp="true" BehaviorID="seArticle">
                                        </AjaxToolkit:SliderExtender> </span><span class="float-left RateArticleNumberRight">6</span>
                                    <span class="float-left prefferredcheckbox">
                                        <asp:CheckBox ID="chkArticlePreferred" runat="server" Text="Preferred" onclick="PrefferredChecked('chkArticlePreferred','seArticle');" />
                                    </span>
                                </div>
                                <%--<div>
                                    <asp:CheckBox ID="chkArticlePreferred" runat="server" Text="Preferred" Style="margin-left: -110px;
                                        margin-top: 3px;" onclick="PrefferredChecked('chkArticlePreferred','seArticle');" />
                                </div>--%>
                            </div>
                        </li>
                    </ul>
                    <div class="text-align-center">
                        <input type="button" id="Button1" class="btn-blue2" value="Cancel" onclick="closeModal('pnlSaveArticle');" />
                        <asp:Button ID="btnSaveArticle" runat="server" Text="Save" ValidationGroup="vgSaveArticle"
                            CausesValidation="true" CssClass="btn-blue2" OnClick="btnSaveArticle_Click" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Panel>
<div id="diviframe" runat="server" class="modal fade resizable display-none playerPopUp">
    <div id="divClosePlayer">
        <input type="image" id="imgCancelPlayer" onclick="ClosePlayerReport();return false;"
            class="popup-top-close" src="../Images/close-icon.png" />
    </div>
    <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <uc1:IframeRawMediaH runat="server" ID="IframeRawMediaH"></uc1:IframeRawMediaH>
        </ContentTemplate>
    </asp:UpdatePanel>
</div>
