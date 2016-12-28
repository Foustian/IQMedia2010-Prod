<%@ Assembly Name="$SharePoint.Project.AssemblyFullName$" %>
<%@ Assembly Name="Microsoft.Web.CommandUI, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="SharePoint" Namespace="Microsoft.SharePoint.WebControls"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="Utilities" Namespace="Microsoft.SharePoint.Utilities" Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Register TagPrefix="asp" Namespace="System.Web.UI" Assembly="System.Web.Extensions, Version=3.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" %>
<%@ Import Namespace="Microsoft.SharePoint" %>
<%@ Register TagPrefix="WebPartPages" Namespace="Microsoft.SharePoint.WebPartPages"
    Assembly="Microsoft.SharePoint, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AmexCashWebPartUserControl.ascx.cs"
    Inherits="AmexCash.AmexCashWebPart.AmexCashWebPartUserControl" %>
<%@ Register TagPrefix="cms" Namespace="Microsoft.SharePoint.Publishing.WebControls"
    Assembly="Microsoft.SharePoint.Publishing, Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c" %>
<link href="../../../_layouts/AmexCash/Style.css" rel="stylesheet" type="text/css" />
<script src="../../../_layouts/AmexCash/jquery-1.4.1.min.js" type="text/javascript"></script>
<script type="text/javascript" language="javascript">

    function CheckMonthYearSelection() {
        var drpmonthvalue = document.getElementById('<%=drpmonth.ClientID %>').value;
        var drpyearvalue = document.getElementById('<%= drpyear.ClientID %>').selectedIndex;
        if (drpmonthvalue > 0 && drpyearvalue <= 0) {
            alert('Select Year');
            return false;
        }
    }


    //sharepoint postback to work after clicking on telerik export to pdf
    if (typeof (_spBodyOnLoadFunctionNames) != 'undefined' && _spBodyOnLoadFunctionNames != null) {
        _spBodyOnLoadFunctionNames.push("supressSubmitWraper");
    }

    function supressSubmitWraper() {
        _spSuppressFormOnSubmitWrapper = true;
    }

    $(document).ready(function () {


        /*$("#butsearch").click(function () {
        alert('hi');
        if ($("#drpmonth").val() > 0 && $("#drpyear").val() <= 0) {
        alert('Select Year');
        return false;
        }
        });*/

        $('input[type=text]').addClass('txtbox');
        $('input[type=button]').addClass('btn');
        $('select').addClass('dropdown');

        //        $(".btn").hover(function () {
        //            var p = $(this).css("background-color", "#999");

        //        }, function () {
        //            var p = $(this).css("background-color", "#fff");

        //        });
    });
</script>
<table id="tblfilter" runat="server" cellpadding="7">
    <tr>
        <td>
            <span class="txtlabel">Select Month:</span>
        </td>
        <td>
            <asp:DropDownList ID="drpmonth" runat="server" class="dropdown">
                <asp:ListItem Selected="true" Value="0">--Select Month--</asp:ListItem>
                <asp:ListItem Value="1">January</asp:ListItem>
                <asp:ListItem Value="2">February</asp:ListItem>
                <asp:ListItem Value="3">March</asp:ListItem>
                <asp:ListItem Value="4">April</asp:ListItem>
                <asp:ListItem Value="5">May</asp:ListItem>
                <asp:ListItem Value="6">June</asp:ListItem>
                <asp:ListItem Value="7">July</asp:ListItem>
                <asp:ListItem Value="8">August</asp:ListItem>
                <asp:ListItem Value="9">September</asp:ListItem>
                <asp:ListItem Value="10">October</asp:ListItem>
                <asp:ListItem Value="11">November</asp:ListItem>
                <asp:ListItem Value="12">December</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <span class="txtlabel">Select Year:</span>
        </td>
        <td>
            <asp:DropDownList ID="drpyear" runat="server" class="dropdown">
                <asp:ListItem Selected="true" Value="0">--Select Year--</asp:ListItem>
            </asp:DropDownList>
        </td>
        <td>
            <asp:Button ID="butsearch" OnClick="btnSearch_Click" OnClientClick="return CheckMonthYearSelection();"
                runat="server" Text="Search" CssClass="btn" />
        </td>
    </tr>
</table>
<br />
<br />
<table>
    <tr>
        <td>
            <asp:Button ID="btnAdd" runat="server" Text="ADD" OnClick="btnAdd_Click" CssClass="btn" />&nbsp;&nbsp;
            <asp:Button ID="butexp" runat="server" Text="Export to Excel" OnClick="butexp_Click"
                CssClass="btn" />
        </td>
    </tr>
    <tr style="height: 10px">
        <td>
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;
        </td>
    </tr>
</table>
<table id="fsIns" runat="server" visible="false">
    <tr>
        <td align="left">
            <table>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td class="txtlabel" colspan="3">
                        Fields marked with ( <span style="color: Red">*</span>) are required
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="txtlabel"><span style="color: Red">*</span>Bill date :</span>
                    </td>
                    <td>
                        <sharepoint:datetimecontrol runat="server" id="txtbilldate" dateonly="true" isrequiredfield="true" />
                        <asp:CompareValidator ID="cvtxtbilldate" runat="server" ForeColor="Red" ControlToValidate="txtbilldate$txtbilldateDate"
                            Type="Date" Operator="DataTypeCheck" ErrorMessage="*" Display="Dynamic" />
                        <asp:RangeValidator ID="rvtxtbilldate" runat="server" ControlToValidate="txtbilldate$txtbilldateDate"
                            MinimumValue="1/1/1900" MaximumValue="12/31/8900" Display="Dynamic" Type="Date"
                            ErrorMessage="You must specify a valid date within the range of 1/1/1900 and 12/31/8900"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="txtlabel"><span style="color: Red">*</span>Vendor :</span>
                    </td>
                    <td>
                        <asp:DropDownList ID="drpVendor" runat="server" Width="150px" class="dropdown">
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="reqVendor" runat="server" ControlToValidate="drpVendor"
                            ErrorMessage="Select Vendor" InitialValue="--Select--"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="txtlabel"><span style="color: Red">*</span>Purchase Date :</span>
                    </td>
                    <td>
                        <sharepoint:datetimecontrol runat="server" id="txtPurchaseDate" dateonly="true" isrequiredfield="true" />
                        <asp:CompareValidator ID="cvtxtPurchaseDate" runat="server" ForeColor="Red" ControlToValidate="txtPurchaseDate$txtPurchaseDateDate"
                            Type="Date" Operator="DataTypeCheck" ErrorMessage="*" Display="Dynamic" />
                        <asp:RangeValidator ID="rvtxtPurchaseDate" runat="server" ControlToValidate="txtPurchaseDate$txtPurchaseDateDate"
                            MinimumValue="1/1/1900" MaximumValue="12/31/8900" Display="Dynamic" Type="Date"
                            ErrorMessage="You must specify a valid date within the range of 1/1/1900 and 12/31/8900"></asp:RangeValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="txtlabel"><span style="color: Red">*</span>Amount :</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtamount" runat="server" class="txtbox"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqInvoicenum" runat="server" ControlToValidate="txtamount"
                            Display="Dynamic" ErrorMessage="Enter Invoice Amount"></asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvtxtamount" runat="server" ControlToValidate="txtamount"
                            Display="Dynamic" Type="Currency" ErrorMessage="Enter Proper Invoice Amount"
                            Operator="DataTypeCheck"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="txtlabel">Link to Bill :</span>
                    </td>
                    <td>
                        <%--<asp:TextBox ID="txtlink2invoice" runat="server"></asp:TextBox>--%>
                        <cms:asseturlselector defaulttolastusedlocation="false" displaylookinsection="false"
                            overridedialogfeatures="resizable: no; status: yes; scroll: yes; help: no; dialogWidth:500px; dialogHeight:500px;"
                            overridedialogtitle="Custom Picker Title" overridedialogdescription="Custom Picker Description"
                            assettextclientid="testAssetTextClientIDCust" id="assetSelectedImageCustomLauncher"
                            useimageassetpicker="false" runat="server" validateurl="true" isurlrequired="true" />                            
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <span class="txtlabel">GL Code :</span>
                    </td>
                    <td>
                        <asp:TextBox ID="txtGlcode" runat="server" class="txtbox"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator ID="reqInvoiceamt" runat="server" ControlToValidate="txtGlcode"
                    ErrorMessage=""></asp:RequiredFieldValidator>--%>
                        <asp:CompareValidator ID="cvtxtGLCode" runat="server" ControlToValidate="txtGlcode"
                            Type="Integer" ErrorMessage="Enter Proper GL Code" Operator="DataTypeCheck"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td>
                        &nbsp;
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnSave" runat="server" Width="70" Text="Save" OnClick="btnSave_Click"
                            CssClass="btn" />&nbsp;&nbsp;&nbsp;
                        <asp:Button ID="btnCancel" Width="70" CausesValidation="false" runat="server" Text="Cancel"
                            CssClass="btn" OnClick="btnCancel_Click" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div style="overflow:auto;width:1100px; height:450px">
<table>
    <tr>
        <td>
            <%--  <asp:Button ID="butexp" runat="server" Text="Export to Excel" OnClick="butexp_Click" />--%>
        </td>
    </tr>
    <tr id="trGrd" runat="server">
        <td>
            <asp:GridView ID="grdContact" runat="server" AutoGenerateColumns="False" OnRowCancelingEdit="grdContact_RowCancelingEdit"
                OnRowDataBound="grdContact_RowDataBound" border="1" CellPadding="7" Style="border-collapse: collapse;"
                BorderColor="#CCCCCC" OnRowEditing="grdContact_RowEditing" OnRowUpdating="grdContact_RowUpdating"
                AutoGenerateEditButton="True" AllowPaging="True" OnPageIndexChanging="grdContact_PageIndexChanging"
                BackColor="White" BorderStyle="None" BorderWidth="1px" EnableModelValidation="True"
                ForeColor="Black" GridLines="Horizontal">
                <%--<AlternatingRowStyle BackColor="White" ForeColor="#284775" />--%>
                <Columns>
                    <asp:TemplateField HeaderText="ID" ControlStyle-BackColor="White" HeaderStyle-HorizontalAlign="Left"
                        Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblId" BackColor="White" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblId" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                        </EditItemTemplate>
                        <ControlStyle BackColor="White"></ControlStyle>
                        <HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Seq #" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblSeqnum" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:Label ID="lblSeqnum" runat="server" Text='<%# Bind("ID") %>'></asp:Label>
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#33608a" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Bill Date" ItemStyle-BackColor="White" ControlStyle-BackColor="White"
                        HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblBillDate" runat="server" Text='<%# Bind("Billdate","{0:M/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <sharepoint:datetimecontrol runat="server" id="txtBillDate" dateonly="true" isrequiredfield="true" />
                        </EditItemTemplate>
                        <ControlStyle BackColor="White"></ControlStyle>
                        <HeaderStyle BackColor="#33608a" />
                        <ItemStyle BackColor="White"></ItemStyle>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Vendor" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblVendor" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:DropDownList ID="drpVen" runat="server" Width="120px">
                            </asp:DropDownList>
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#33608a" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purchase Date" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblPurcahseDate" runat="server" Text='<%# Bind("Purchasedate","{0:M/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <sharepoint:datetimecontrol runat="server" id="txtPurcahseDate" dateonly="true" isrequiredfield="true" />
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#33608a" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Amount" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtAmount" runat="server" Text='<%# Bind("Amount") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator ID="reqInvoicenum" runat="server" ControlToValidate="txtAmount"
                                Display="Dynamic" ErrorMessage="Enter Invoice Amount"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvtxtamount" runat="server" ControlToValidate="txtAmount"
                                Display="Dynamic" Type="Currency" ErrorMessage="Enter Proper Invoice Amount"
                                Operator="DataTypeCheck"></asp:CompareValidator>
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#33608a" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Link to Bill" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:HyperLink ID="hypLinktoinvoice" runat="server" Target="_blank"></asp:HyperLink>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtlinktoinvoice" runat="server" Text='<%# Bind("Link_x0020_to_x0020_invoice") %>'></asp:TextBox>--%>
                            <cms:asseturlselector defaulttolastusedlocation="false" displaylookinsection="false"
                                overridedialogfeatures="resizable: no; status: yes; scroll: yes; help: no; dialogWidth:500px; dialogHeight:500px;"
                                overridedialogtitle="Custom Picker Title" overridedialogdescription="Custom Picker Description"
                                assettextclientid="testAssetTextClientIDCust" id="assetSelectedImageCustomLauncher"
                                useimageassetpicker="false" runat="server" />
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#33608a" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="GL Code" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblGlCode" runat="server" Text='<%# Bind("GLCode") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtGlCode" runat="server" Text='<%# Bind("GLCode") %>'></asp:TextBox>
                            <asp:CompareValidator ID="cvtxtGLCode" runat="server" ControlToValidate="txtGlCode"
                                Display="Dynamic" Type="Integer" ErrorMessage="Enter Proper GL Code" Operator="DataTypeCheck"></asp:CompareValidator>
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#33608a" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Approval" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkapproval" runat="server" Enabled="false" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="chkapproval" runat="server" AutoPostBack="true" OnCheckedChanged="chkapproval_CheckedChanged" />
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#4683bb" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purchase Approval" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblPurchaseAppr" runat="server" Text='<%# Bind("Purchase_x0020_Approval") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtPurchaseAppr" runat="server" Text='<%# Bind("Purchase_x0020_approval") %>'></asp:TextBox>--%>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <contenttemplate>
                                    <asp:Label ID="lblPurchaseAppr" runat="server" Text='<%# Bind("Purchase_x0020_Approval") %>'></asp:Label>
                                </contenttemplate>
                                <triggers>
                                    <asp:AsyncPostBackTrigger  ControlID="chkapproval" EventName="CheckedChanged"/>
                                </triggers>
                            </asp:UpdatePanel>
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#4683bb" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Purchase Approval date" HeaderStyle-HorizontalAlign="Left">
                        <ItemTemplate>
                            <asp:Label ID="lblPurchaseDate" runat="server" Text='<%# Bind("PurchaseApprovalDate","{0:M/dd/yyyy}") %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                            <%--<spuc:datetimecontrol runat="server" id="txtPurchaseDate" dateonly="true" />--%>
                            <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                <contenttemplate>
                                    <asp:Label ID="lblPurchaseDate" runat="server"></asp:Label>
                                </contenttemplate>
                                <triggers>
                                    <asp:AsyncPostBackTrigger  ControlID="chkapproval" EventName="CheckedChanged"/>
                                </triggers>
                            </asp:UpdatePanel>
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#4683bb" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Is Active">
                        <ItemTemplate>
                            <asp:CheckBox ID="checkactive1" Enabled="false" Checked="true" runat="server" />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:CheckBox ID="checkactive" Checked="true" runat="server" />
                        </EditItemTemplate>
                        <HeaderStyle BackColor="#4683bb" />
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
                <HeaderStyle BackColor="Gray" Font-Bold="True" ForeColor="White"  Wrap="false"/>
                <%--<PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />--%>
                <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
                <PagerSettings FirstPageText="First" LastPageText="Last" Mode="NumericFirstLast" PageButtonCount="5" position="Top" />
                    <PagerStyle CssClass="pagination" HorizontalAlign="Left" VerticalAlign="Middle"/>
            </asp:GridView>
            <%--<HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />--%>
        </td>
    </tr>
</table>
</div>
