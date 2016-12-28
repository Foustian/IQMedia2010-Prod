<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Report.ascx.cs" Inherits="IQMediaGroup.Reports.Usercontrol.IQMediaMaster.Report.Report" %>
<asp:UpdatePanel ID="upReport" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <div class="result-in-bg">
            <asp:Label ID="lblReportErrorMsg" runat="server" ForeColor="Red" CssClass="center padding5"></asp:Label>
            <asp:Panel ID="divReportResult" runat="server" CssClass="clear">
                <div id="divReport" class="clear">
                    <div class="float-left">
                        <asp:Label ID="lblReportMsg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="float-right padding5">                        
                        <input type="button" class="btn-blue2" runat="server" id="btnReportEmail" title="Email Report"
                            value="Email Report" onclick="return OpenReportEmailPopup();" />
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
<div id="diviFrameArticle" runat="server" class="modal hide fade resizable modalPopupDiv iframePopup"
    style="display: none;">
    <div style="width: 27px; float: right;" id="divCloseiFrameArticle">
        <div>
            <img id="img3" class="popup-top-close" src="../Images/close-icon.png" onclick="closeModal('diviFrameArticle');removeSourceFromiFrame('iFrameReportArticle');" />
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
        <asp:Button ID="btnPrint" Text="Print Article" CssClass="btn-green" runat="server"
            OnClientClick="javascript:PrintIframe();return false;" />
    </div>
</div>
