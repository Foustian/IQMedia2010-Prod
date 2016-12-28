<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DownloadClip.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.DownloadClip.DownloadClip" %>
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
                <%--<input type="button" id="btnPolicyCancel" value="Cancel" />--%>
                <asp:Button ID="BtnPolicyContinue" runat="server" CssClass="btn-blue2" Text="Continue"
                    OnClick="BtnPolicyContinue_Click" />
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Panel ID="pnlDownload" runat="server" Height="620px" Width="480px" Visible="false" Style="background-color: #F8FCFF;">
    <table style="width: 100%;" cellpadding="5">
        <tr>
            <td style="width: 100%;">
                <table class="ClipDownloadTbl" width="100%">
                    <tr class="DwnLoadbluebox-tr">
                        <th align="center" valign="middle" class="bluebox-hd">
                            <span>Choose the Format for download</span>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grvFormat" runat="server" Width="100%" DataKeyNames="IQ_ClipDownload_Key,ClipID"
                                AutoGenerateColumns="false" EmptyDataText="No Results Found" BorderColor="#e4e4e4"
                                border="1" GridLines="None" Style="border-collapse: collapse;" ShowHeader="true"
                                CellPadding="5" CellSpacing="0" HeaderStyle-CssClass="grid-th" CssClass="grid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteClip" CommandArgument='<%# Eval("ClipID") %>'
                                                ImageUrl="~/Images/gridicon-delete.png" OnClientClick="javascript:return confirm('Are you sure to delete this record?');"
                                                OnCommand="ImgBtnDelete_Command" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Center" Width="10%" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ClipTitle" HeaderText="Clip Title" ItemStyle-Width="69%" HeaderStyle-Width="69%" ItemStyle-CssClass="content-text2" />
                                    <asp:TemplateField HeaderText="Select Format">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlFormat" runat="server" CssClass="grayinput" style="width:100%;">
                                                <asp:ListItem Text="MP3" Value="mp3"></asp:ListItem>
                                                <asp:ListItem Text="MP4" Value="mp4"></asp:ListItem>
                                                <asp:ListItem Text="WMA" Value="wma"></asp:ListItem>
                                                <asp:ListItem Text="WMV" Value="wmv" Selected="True"></asp:ListItem>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Center" Width="21%" />
                                        <HeaderStyle Width="21%" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr id="trNote" runat="server" style="height: 25px;">
                        <td valign="middle">
                            <span style="color: Red;" class="content-text">Note: Please select WMA or MP3 for audio
                                clips.For video select WMV or MP4.</span>
                        </td>
                    </tr>
                    <tr style="height: 30px;">
                        <td valign="middle" align="center">
                            <asp:Button ID="BtnFormat" runat="server" CssClass="btn-blue2" Text="Submit" Visible="false"
                                OnClick="BtnFormat_Click" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100%">
                <table class="ClipDownloadTbl" width="100%">
                    <tr class="DwnLoadbluebox-tr">
                        <th valign="middle" align="center" class="bluebox-hd">
                            <span>Pending Clip Download Requests</span>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grvPendingRequests" runat="server" Width="100%" DataKeyNames="ClipID,IQ_ClipDownload_Key"
                                AutoGenerateColumns="false" EmptyDataText="No Results Found" BorderColor="#e4e4e4"
                                border="1" GridLines="None" Style="border-collapse: collapse;" ShowHeader="true"
                                CellPadding="5" CellSpacing="0" HeaderStyle-CssClass="grid-th" CssClass="grid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Delete">
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgBtnDelete" runat="server" CommandName="DeleteClip" CommandArgument='<%# Eval("ClipID") %>'
                                                ImageUrl="~/Images/gridicon-delete.png" OnClientClick="javascript:return confirm('Are you sure to delete this record?');"
                                                OnCommand="ImgBtnDelete_Command" />
                                        </ItemTemplate>
                                        <ItemStyle CssClass="content-text2" HorizontalAlign="Center" Width="10%" />
                                        <HeaderStyle Width="10%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ClipTitle" HeaderText="Clip Title" HeaderStyle-Width="48%" ItemStyle-Width="48%" ItemStyle-CssClass="content-text2" />
                                    <asp:BoundField DataField="ClipDLFormat" HeaderText="Format" ItemStyle-Width="10%"
                                        ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="content-text2" HeaderStyle-Width="10%" />
                                    <asp:BoundField DataField="ClipDLRequestDateTime" HeaderText="Request Date" ItemStyle-Width="32%" HeaderStyle-Width="32%" ItemStyle-CssClass="content-text2" />
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100%;">
                <table class="ClipDownloadTbl" width="100%">
                    <tr class="DwnLoadbluebox-tr">
                        <th valign="middle" align="center" class="bluebox-hd">
                            <span>Clip Ready for Download</span>
                        </th>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView ID="grvDownload" runat="server" Width="100%" DataKeyNames="IQ_ClipDownload_Key"
                                AutoGenerateColumns="false" EmptyDataText="No Results Found" BorderColor="#e4e4e4"
                                border="1" GridLines="None" Style="border-collapse: collapse;" ShowHeader="true"
                                CellPadding="5" CellSpacing="0" HeaderStyle-CssClass="grid-th" CssClass="grid">
                                <Columns>
                                    <asp:TemplateField HeaderText="Clip Title">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="LbtnClipTitle" runat="server" CommandArgument='<%# Eval("ClipFileLocation").ToString()+Eval("ClipID").ToString()+"."+Eval("ClipDLFormat").ToString() %>'
                                                Text='<%# Eval("ClipTitle") %>' OnCommand="LbtnClipTitle_Command"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle CssClass="content-text2" Width="68%" />
                                        <HeaderStyle Width="68%" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ClipDLFormat" HeaderText="Format" ItemStyle-Width="12%" HeaderStyle-Width="12%"
                                        ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="content-text2"/>
                                    <asp:BoundField DataField="ClipDLRequestDateTime" HeaderText="Request Date" ItemStyle-Width="20%" ItemStyle-CssClass="content-text2" HeaderStyle-Width="20%"/>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr style="height: 35px;">
                        <td align="center" valign="middle">
                            <asp:Button ID="BtnRefresh" runat="server" CssClass="btn-blue2" Visible="false" Text="Refresh"
                                OnClick="BtnRefresh_Click" />
                            <input type="button" id="hBtnClose" value="Cancel" class="btn-blue2" onclick="window.close();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
<asp:Label ID="lblMsg" runat="server" Visible="false" ForeColor="Red"></asp:Label>
