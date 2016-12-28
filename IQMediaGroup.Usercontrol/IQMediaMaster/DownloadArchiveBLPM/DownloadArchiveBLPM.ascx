<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadArchiveBLPM.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.DownloadArchiveBLPM.DownloadArchiveBLPM" %>
<asp:Panel ID="pnlPolicy" runat="server" Visible="true" Style="background-color: #F8FCFF;">
    <table cellpadding="8">
        <tr>
            <td>
                <div id="divPolicy" runat="server" style="height: 450px; width: 425px; overflow: auto;
                    border: 1px solid #C0CDE0;" class="about-mid2">
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <asp:CheckBox CssClass="content-text-new" ID="chkPolicy" runat="server" Text="please check box to accept the above terms and conditions" />
            </td>
        </tr>
        <tr>
            <td align="center">
                <input type="button" id="BtnPolicyCancel" class="btn-blue2" value="Cancel" onclick="window.close();" />
                <asp:Button ID="BtnPolicyContinue" runat="server" CssClass="btn-blue2" Text="Continue"
                    OnClick="BtnPolicyContinue_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlDownload" runat="server" Height="620px" Width="480px" Visible="false"
    Style="background-color: #F8FCFF;">
    <table style="width: 100%;" cellpadding="5">
        <tr>
            <td style="width: 100%;">
                <table class="ClipDownloadTbl" width="100%">
                    <tr class="DwnLoadbluebox-tr">
                        <th valign="middle" align="center" class="bluebox-hd">
                            <span>Print Media Ready for Download</span>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grvDownload" runat="server" Width="100%" DataKeyNames="ID" AutoGenerateColumns="false"
                                EmptyDataText="No Results Found" BorderColor="#e4e4e4" border="1" GridLines="None"
                                Style="border-collapse: collapse;" ShowHeader="true" CellPadding="5" CellSpacing="0"
                                HeaderStyle-CssClass="grid-th" CssClass="grid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Print Media Title">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LbtnArticleTitle" runat="server" CommandArgument='<%# Eval("DownloadLocation") %>'
                                                Text='<%# Eval("Headline") %>' OnCommand="LbtnArticleTitle_Command"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="content-text2" Width="68%" />
                                        <HeaderStyle Width="68%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="DLRequestDateTime" HeaderText="Request Date" ItemStyle-Width="20%"
                                        ItemStyle-CssClass="content-text2" HeaderStyle-Width="20%" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr style="height: 35px;">
                        <td align="center" valign="middle">
                            <%--<asp:Button ID="BtnRefresh" runat="server" CssClass="btn-blue2" Visible="false" Text="Refresh"
                                OnClick="BtnRefresh_Click" />--%>
                            <input type="button" id="hBtnClose" value="Cancel" class="btn-blue2" onclick="window.close();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
