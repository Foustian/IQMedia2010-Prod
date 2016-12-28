<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAgentReport.ascx.cs"
    Inherits="IQMediaGroup.Reports.Usercontrol.IQMediaMaster.IQAgentReport.IQAgentReport" %>
<%@ Register Src="../Report/Report.ascx" TagName="Report" TagPrefix="uc" %>
<div class="row-fluid" style="border: none; width: 100%; padding: 0px;">
    <%--<div class="iq-agent-left" style="width: 35%;">--%>
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
                                                <asp:DropDownList ID="drpReportTypes" ValidationGroup="vgReport" runat="server" CssClass="width90"
                                                    OnSelectedIndexChanged="drpReportTypes_SelectedIndexChanged" AutoPostBack="true">
                                                </asp:DropDownList>
                                                <br />
                                                <asp:RequiredFieldValidator ID="reqdrpReportTypes" runat="server" ValidationGroup="vgReport"
                                                    InitialValue="0" ControlToValidate="drpReportTypes" Display="None" ErrorMessage="Please Select Report Type">
                                                </asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </li>
                                </ul>
                            </li>
                        </ul>
                    </div>
                    <asp:Panel ID="pnlReports" runat="server" CssClass="listBorder" Visible="false">
                        <div class="marginleft8 margintop12 ulhead">
                            <div>
                                Client Reports</div>
                        </div>
                        <div class="clear">
                            <div class="paddingClientReport">
                                <asp:GridView ID="gvReports" runat="server" Width="100%" border="0" AllowPaging="True"
                                    PageSize="15" CellPadding="5" AutoGenerateEditButton="False" HeaderStyle-CssClass="grid-th"
                                    BorderColor="#E4E4E4" Style="border-collapse: collapse;" AutoGenerateColumns="False"
                                    EmptyDataText="No Reports" CssClass="grid grid-iq" BackColor="#FFFFFF" OnRowCommand="gvReports_Command">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Title">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lnkbtnTitle" CausesValidation="false" runat="server" CommandArgument='<%# Eval("ID") %>'
                                                    Text='<%# Eval("Title") %>' CommandName="LoadReport"></asp:LinkButton>
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
    <div class="span9">
        <asp:UpdatePanel ID="upReportGrid" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <uc:Report ID="Report1" runat="server" Visible="false" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <%--</div>--%>
    <div class="iq-agent-right" style="width: 64%">
        <div class="ag-gray-main">
            <div style="margin: 6px;">
            </div>
        </div>
    </div>
</div>
