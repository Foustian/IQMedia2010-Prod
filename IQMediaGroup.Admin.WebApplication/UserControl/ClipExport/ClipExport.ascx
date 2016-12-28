<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClipExport.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.ClipExport.ClipExport" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
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
            <asp:UpdatePanel ID="upExport" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <table width="100%" border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                                    <tr>
                                        <td style="padding-bottom: 10px">
                                            <div class="AdminTitle">
                                                Clip Export</div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <table width="100%" border="0" cellspacing="3" cellpadding="0">
                                                            <tr>
                                                                <td width="2%">
                                                                </td>
                                                                <td width="98%" align="left" valign="top">
                                                                    <asp:ValidationSummary ID="vsIQMediaSearch" EnableClientScript="true" runat="server"
                                                                        ValidationGroup="validate" ForeColor="#bd0000" Font-Size="Small" />
                                                                    <asp:Label ID="lblMessage" runat="server" Visible="false" ForeColor="Green" Font-Size="Small"></asp:Label>
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
                                            <table border="0" cellspacing="5" cellpadding="0">
                                                <tr>
                                                    <td class="content-text">
                                                        Client Name:
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:DropDownList ID="ddlClient" runat="server" AutoPostBack="true" CssClass="dropdown01"
                                                                OnSelectedIndexChanged="ddlClient_SelectedIndexChanged">
                                                            </asp:DropDownList>
                                                            <cc1:PropertyProxyValidator ID="PropertyProxyValidator1" runat="server" ControlToValidate="ddlClient"
                                                                Text="*" PropertyName="TodayDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                OnValueConvert="ClusterValidator_ValueConvertClient" RulesetName="RawMediaDate"
                                                                ValidationGroup="validate" Font-Size="Smaller" Display="Dynamic" DisplayMode="List"></cc1:PropertyProxyValidator>
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="content-text">
                                                        Customer Name:
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:DropDownList ID="ddlCustomer" runat="server" Enabled="false" CssClass="dropdown01">
                                                            </asp:DropDownList>
                                                            <cc1:PropertyProxyValidator ID="pplSelectCluster" runat="server" ControlToValidate="ddlCustomer"
                                                                Text="*" PropertyName="StartIndex" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                OnValueConvert="ClusterValidator_ValueConvert" RulesetName="RawMediaDate" ValidationGroup="validate"
                                                                Font-Size="Smaller" Display="Dynamic" DisplayMode="List"></cc1:PropertyProxyValidator>
                                                        </label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td valign="top">
                                                        Date:
                                                    </td>
                                                    <td align="left" valign="top">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtFromDate" runat="server" AutoCompleteType="None" CssClass="textbox03"
                                                                        Width="70px"></asp:TextBox>
                                                                    <cc1:PropertyProxyValidator ID="pplFromDate" runat="server" ControlToValidate="txtFromDate"
                                                                        Text="*" PropertyName="SearchStartDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                        RulesetName="RawMediaDate" ValidationGroup="validate" Font-Size="Smaller" Display="Dynamic"
                                                                        DisplayMode="List"></cc1:PropertyProxyValidator>
                                                                    <%--<asp:RegularExpressionValidator ID="regFromDate" runat="server" ControlToValidate="txtFromDate" Text="*" ErrorMessage="Please enter valid Start Date." ValidationExpression="(([1-9]|1[012])[- /.]([1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d)|((1[012]|0[1-9])(3[01]|2\d|1\d|0[1-9])(19|20)\d\d)|((1[012]|0[1-9])[- /.](3[01]|2\d|1\d|0[1-9])[- /.](19|20)\d\d)"></asp:RegularExpressionValidator>--%>
                                                                    <asp:CustomValidator ID="cusFromDate" runat="server" Display="Dynamic" ControlToValidate="txtFromDate"
                                                                        ValidationGroup="validate" OnServerValidate="cvFromDate_ServerValidate" Text="*"
                                                                        ErrorMessage="FromDate can not be greater than Current Date."></asp:CustomValidator>
                                                                    <ajaxtoolkit:textboxwatermarkextender id="tbwEtxtFromDate" targetcontrolid="txtFromDate"
                                                                        watermarkcssclass="watermarked" watermarktext="From Date" runat="server">
                                                                    </ajaxtoolkit:textboxwatermarkextender>
                                                                    <ajaxtoolkit:calendarextender id="CalEtxtFromDate" runat="server" cssclass="MyCalendar"
                                                                        targetcontrolid="txtFromDate">
                                                                    </ajaxtoolkit:calendarextender>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtToDate" runat="server" AutoCompleteType="None" CssClass="textbox03"
                                                                        Width="70px"></asp:TextBox>
                                                                    <cc1:PropertyProxyValidator ID="pplEndDate" runat="server" ControlToValidate="txtToDate"
                                                                        Text="*" PropertyName="SearchEndDate" SourceTypeName="IQMediaGroup.Core.HelperClasses.Search"
                                                                        RulesetName="RawMediaDate" ValidationGroup="validate" Font-Size="Smaller" Display="Dynamic"
                                                                        DisplayMode="List"></cc1:PropertyProxyValidator>
                                                                    <%--<asp:RegularExpressionValidator ID="regToDate" runat="server" ControlToValidate="txtToDate" Text="*" ErrorMessage="Please enter valid End Date." ValidationExpression="(([1-9]|1[012])[- /.]([1-9]|[12][0-9]|3[01])[- /.](19|20)\d\d)|((1[012]|0[1-9])(3[01]|2\d|1\d|0[1-9])(19|20)\d\d)|((1[012]|0[1-9])[- /.](3[01]|2\d|1\d|0[1-9])[- /.](19|20)\d\d)"></asp:RegularExpressionValidator>--%>
                                                                    <asp:CustomValidator ID="susCurrentDate" Display="Dynamic" runat="server" ControlToValidate="txtToDate"
                                                                        ValidationGroup="validate" OnServerValidate="cvCompareDate_ServerValidate" Text="*"
                                                                        ErrorMessage="ToDate can not be less than From Date."></asp:CustomValidator>
                                                                    <asp:CustomValidator ID="cusToDate" runat="server" Display="Dynamic" ControlToValidate="txtToDate"
                                                                        ValidationGroup="validate" OnServerValidate="cvToDate_ServerValidate" Text="*"
                                                                        ErrorMessage="ToDate can not be greater than Current Date."></asp:CustomValidator>
                                                                    <ajaxtoolkit:textboxwatermarkextender id="tbwEtxtToDate" targetcontrolid="txtToDate"
                                                                        watermarkcssclass="watermarked" watermarktext="To Date" runat="server">
                                                                    </ajaxtoolkit:textboxwatermarkextender>
                                                                    <ajaxtoolkit:calendarextender id="valEtxtToDate" runat="server" cssclass="MyCalendar"
                                                                        targetcontrolid="txtToDate">
                                                                    </ajaxtoolkit:calendarextender>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="content-text">
                                                    </td>
                                                    <td>
                                                        <label>
                                                            <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn-blue" ValidationGroup="validate"
                                                                OnClick="btnExport_Click" />
                                                        </label>
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
            <img id="Img8" runat="server" src="~/images/contentbox-r.jpg" width="8" height="1"
                border="0" alt="iQMedia" />
        </td>
    </tr>
</table>
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
<asp:UpdateProgress ID="updProgressExport" runat="server" AssociatedUpdatePanelID="upExport"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upodExport" ControlToOverlayID="upExport"
    TargetControlID="updProgressExport" CssClass="updateProgress" />
