<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAgentReport.ascx.cs"
    Inherits="IQMediaGroup.Reports.Usercontrol.IQMediaMaster.IQAgentReport.IQAgentReport" %>
<%@ Register Src="../Report/Report.ascx" TagName="Report" TagPrefix="uc" %>
<div class="clear">
    <asp:UpdatePanel ID="upReportType" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="span3">
                <div class="well sidebar-nav" id="divFilterSide">
                    <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Font-Size="Small"></asp:Label>
                    <asp:ValidationSummary ID="vsReport" runat="server" ValidationGroup="vgReport" ForeColor="#bd0000"
                        Font-Size="Smaller" />
                    <ul class="nav nav-list marginleft-28">
                        <li class="ulheader">
                            <div class="lidivLeft">
                                &nbsp; REPORT</div>
                        </li>
                    </ul>
                    <div id="divReportFilter" class="clear">
                        <ul class="nav nav-list listBorder filterul" id="ulSearchFilter">
                            <li id="liReportFilterInner">
                                <ul class="nav">
                                    <li id="liReportTypeFilter">
                                        <div id="divReportTypeFilter">
                                            <div class="width100p">
                                                <div class="marginbottom5">
                                                    Report Type :</div>
                                                <asp:DropDownList ID="drpReportTypes" ValidationGroup="vgReport" runat="server" CssClass="width90">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:RequiredFieldValidator ID="reqdrpReportTypes" runat="server" ValidationGroup="vgReport"
                                                    InitialValue="0" ControlToValidate="drpReportTypes" Display="None" ErrorMessage="Please Select Report Type">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </li>
                                    <li id="liReportDateFilter">
                                        <div id="divReportDateFilter">
                                            <div class="width100p">
                                                <div class="marginbottom5">
                                                    Report Date :</div>
                                                <asp:TextBox ValidationGroup="vgReport" ID="txtReportDate" runat="server" AutoCompleteType="None"
                                                    CssClass="programTextbox"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="reqtxtReportDate" runat="server" ValidationGroup="vgReport"
                                                    ControlToValidate="txtReportDate" Display="None" ErrorMessage="Please Select Report Date"></asp:RequiredFieldValidator>
                                            </div>
                                            <div class="right paddingright4p" >
                                                <asp:Button class="btn-blue2 width90" ValidationGroup="vgReport" ID="btnReport" runat="server" Text="Get Report"
                                                    OnClick="btnReport_Click" />
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                    <asp:Panel ID="pnlReports" runat="server" CssClass="listBorder margintop8" Visible="false">
                        <div class="marginleft8 margintop12 ulhead">
                            <div>
                                Client Reports</div>
                        </div>
                        <div class="clear">
                            <div class="paddingClientReport">
                                <asp:GridView ID="gvReports" runat="server" Width="100%" border="0" PageSize="15"
                                    CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                    BorderColor="#E4E4E4" Style="border-collapse: collapse;" AutoGenerateColumns="False"
                                    EmptyDataText="No Reports" CssClass="grid grid-iq" BackColor="#FFFFFF">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Title">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnTitle" CausesValidation="false" runat="server" CommandArgument='<%# Eval("ReportGUID") %>'
                                                    Text='<%# Eval("Title") %>' CommandName="LoadReport" OnCommand="lnkbtnTitle_Command"></asp:LinkButton>
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="grid-th-left" Width="100%" />
                                            <ItemStyle CssClass="content-text left" Width="100%"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <HeaderStyle CssClass="grid-th left" />
                                </asp:GridView>
                            </div>
                        </div>
                    </asp:Panel>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upReportGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="span9 mraginleft2p">
                <uc:Report ID="Report1" runat="server" Visible="false" isEmailActive="true" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--</div>--%>
    <%--  <div class="iq-agent-right" style="width: 64%">
        <div class="ag-gray-main">
            <div style="margin: 6px;">
            </div>
        </div>
    </div>--%>
</div>
