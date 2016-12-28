<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQRedlassoStations.ascx.cs"
    Inherits="IQMediaGroup.Admin.Usercontrol.IQRedlassoStations.IQRedlassoStations" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
            <img id="Img14" runat="server" src="~/images/contentbox-l.jpg" width="8" height="1"
                border="0" alt="iQMedia" />
        </td>
        <td valign="top" bgcolor="#FFFFFF" class="pad-bt">
            <table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <%--<table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td class="pagetitle">
                                    iQ Redlasso Station
                                </td>
                            </tr>
                        </table>--%>
                        <table width="100%" border="0" cellspacing="3" cellpadding="0">
                            <tr>
                                <td width="2%">
                                </td>
                                <td width="98%" align="left" valign="top">
                                    <asp:ValidationSummary ID="vsIQMediaSearch" EnableClientScript="true" runat="server"
                                        ValidationGroup="Sumbit" ForeColor="#bd0000" Font-Size="Smaller" />
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Size="Smaller"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                            <tr>
                                <td style="padding-bottom: 10px">
                                    <div class="AdminTitle">
                                        iQ Redlasso Station</div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table border="0" cellspacing="5" cellpadding="0">
                                        <tr>
                                            <td class="content-text">
                                                Redlasso Station Code:
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:TextBox ID="txtRedlassoStationCode" runat="server" CssClass="textbox03"></asp:TextBox>
                                                    <asp:RequiredFieldValidator ID="rfvClusterName" Text="*" Display="Dynamic" runat="server"
                                                        ValidationGroup="Sumbit" ControlToValidate="txtRedlassoStationCode" ErrorMessage="Please Enter Redlasso Station Code."></asp:RequiredFieldValidator>
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text">
                                                Redlasso Station Market:
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:DropDownList ID="ddlRedlassoStationMarket" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" Text="*" Display="Dynamic"
                                                        runat="server" ValidationGroup="Sumbit" InitialValue="-1" ControlToValidate="ddlRedlassoStationMarket"
                                                        ErrorMessage="Please Select Redlasso Station Market."></asp:RequiredFieldValidator>
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text">
                                                Redlasso Station Type:
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:DropDownList ID="ddlRedlassoStationType" runat="server" ValidationGroup="Sumbit">
                                                        <asp:ListItem Text="Select Station Type" Value="-1"></asp:ListItem>
                                                        <asp:ListItem Text="TV" Value="1"></asp:ListItem>
                                                        <asp:ListItem Text="Radio" Value="2"></asp:ListItem>
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" Text="*" Display="Dynamic"
                                                        runat="server" ValidationGroup="Sumbit" InitialValue="-1" ControlToValidate="ddlRedlassoStationType"
                                                        ErrorMessage="Please Select Redlasso Station Type."></asp:RequiredFieldValidator>
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text">
                                                Redlasso Station Name:
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:TextBox ID="txtCountry" runat="server" CssClass="textbox03" ValidationGroup="Sumbit"></asp:TextBox>
                                                    <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoComplete3" TargetControlID="txtCountry"
                                                        ServiceMethod="GetPortfolioList" MinimumPrefixLength="1" EnableCaching="true" />
                                                    <asp:RequiredFieldValidator ID="rfvRedlassoStationMarket" Text="*" Display="Dynamic"
                                                        runat="server" ValidationGroup="Sumbit" ControlToValidate="txtCountry" ErrorMessage="Please Enter Station Name."></asp:RequiredFieldValidator>
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="content-text">
                                                IQCluster:
                                            </td>
                                            <td>
                                                <label>
                                                    <asp:DropDownList ID="ddlIQCluster" runat="server" ValidationGroup="Sumbit">
                                                    </asp:DropDownList>
                                                    <asp:RequiredFieldValidator ID="rfvIQCluster" runat="server" Text="*" Display="Dynamic"
                                                        ValidationGroup="Sumbit" ControlToValidate="ddlIQCluster" InitialValue="-1" ErrorMessage="Please Select Cluster."></asp:RequiredFieldValidator>
                                                </label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td align="left">
                                                <label>
                                                    <asp:Button ID="btnSubmit" runat="server" Text="Add" CssClass="btn-blue" ValidationGroup="Sumbit"
                                                        OnClick="btnSubmit_Click" />
                                                </label>
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
                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="10">
                                </td>
                            </tr>
                            <tr>
                                <td height="10">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <%--<tr>
                                            <td width="3">
                                                <img id="Img15" runat="server" src="~/images/bluebox-hd-left.jpg" width="3" height="24"
                                                    border="0" alt="iQMedia" />
                                            </td>
                                            <td class="bluebox-hd">
                                                &nbsp;<br />
                                            </td>
                                            <td width="3">
                                                <img id="Img16" runat="server" src="~/images/bluebox-hd-right.jpg" width="3" height="24"
                                                    border="0" alt="iQMedia" />
                                            </td>
                                        </tr>--%>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="3">
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%" cellpadding="5" cellspacing="0" bordercolor="#e4e4e4"
                                                style="border:solid 1px #e4e4e4;border-collapse: collapse;">
                                        <tr>
                                            <td style="padding-bottom: 10px">
                                                <div class="AdminTitle">
                                                    Redlasso Stations</div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:UpdatePanel ID="upRedlassoStation" runat="server" UpdateMode="Conditional">
                                                    <ContentTemplate>
                                                        <asp:GridView ID="gvRedlassoStation" runat="server" Width="100%" border="1" AllowPaging="true"
                                                            PageSize="15" PageIndex="0" PagerSettings-Mode="NextPrevious" CellPadding="5"
                                                            AutoGenerateEditButton="true" HeaderStyle-CssClass="grid-th" PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg"
                                                            PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg" CellSpacing="0"
                                                            BorderColor="#e4e4e4" Style="border-collapse: collapse;" AutoGenerateColumns="false"
                                                            EmptyDataText="No Data Found" OnRowCancelingEdit="gvRedlassoStation_RowCancelingEdit"
                                                            OnRowEditing="gvRedlassoStation_RowEditing" OnRowUpdating="gvRedlassoStation_RowUpdating"
                                                            OnPageIndexChanging="gvRedlassoStation_PageIndexChanging">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="RedlassoStationCode" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblStationCode" runat="server" Text='<%# Bind("RedlassoStationCode") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                                    <EditItemTemplate>
                                                                        <asp:HiddenField ID="hdnRedlassoStationKey" runat="server" Value='<%# Bind("RedlassoStationKey") %>' />
                                                                        <asp:TextBox ID="txtStationCode" runat="server" Text='<%# Bind("RedlassoStationCode") %>'></asp:TextBox>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RedlassoStationMarket" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRedlassoStationMarket" runat="server" Text='<%# Bind("StationMarketName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlRedlassoStationMarketGrid" runat="server" OnPreRender="ddlRedlassoStationMarketGrid_PreRender">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RedlassoCluster" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRedlassoClusterID" runat="server" Text='<%# Bind("ClusterName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlIQClusterGrid" runat="server" OnPreRender="ddlIQClusterGrid_PreRender">
                                                                        </asp:DropDownList>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RedlassoStation" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRedlassoStation" runat="server" Text='<%# Bind("station_name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                                    <EditItemTemplate>
                                                                        <asp:TextBox ID="txtStationGrid" runat="server" Width="80%"></asp:TextBox>
                                                                        <asp:RequiredFieldValidator ID="rfvStation" runat="server" Display="Dynamic" ControlToValidate="txtStationGrid"
                                                                            Text="*"></asp:RequiredFieldValidator>
                                                                        <ajaxToolkit:AutoCompleteExtender runat="server" ID="autoComplete3" TargetControlID="txtStationGrid"
                                                                            ServiceMethod="GetPortfolioList" MinimumPrefixLength="1" EnableCaching="true" />
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="RedlassoStationType" HeaderStyle-HorizontalAlign="Left">
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblRedlassoTypeID" runat="server" Text='<%# Bind("StationTypeName") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle CssClass="content-text"></ItemStyle>
                                                                    <EditItemTemplate>
                                                                        <asp:DropDownList ID="ddlRedlassoStationTypeGrid" runat="server">
                                                                            <asp:ListItem Text="Select Type" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="TV" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="Radio" Value="2"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                        <asp:RequiredFieldValidator ID="rfvStationTypeGrid" runat="server" Display="Dynamic"
                                                                            InitialValue="0" ControlToValidate="ddlRedlassoStationTypeGrid" ErrorMessage="*"></asp:RequiredFieldValidator>
                                                                    </EditItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </ContentTemplate>
                                                </asp:UpdatePanel>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td height="35">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="right" valign="top" class="contenttext-small">
                                                &nbsp;
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
        <td class="contentbox-r">
            <img id="Img17" runat="server" src="~/images/contentbox-r.jpg" width="8" height="1"
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
<asp:UpdateProgress ID="updProgressRedlassoStation" runat="server" AssociatedUpdatePanelID="upRedlassoStation"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtenderRedlassoStation"
    ControlToOverlayID="upRedlassoStation" TargetControlID="updProgressRedlassoStation"
    CssClass="updateProgress" />
