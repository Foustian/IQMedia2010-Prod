<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RedlassoStationMarket.ascx.cs" Inherits="IQMediaGroup.Admin.Usercontrol.RedlassoStationMarket.RedlassoStationMarket" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
                                               <table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="8"><img id="Img1" runat="server" src="~/images/contentbox-blue-lt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                    <td class="contentbox-blue-t"><img id="Img2" runat="server" src="~/images/contentbox-blue-t.jpg" width="1" height="8" border="0" alt="iQMedia" /></td>
                    <td width="8"><img id="Img3" runat="server" src="~/images/contentbox-blue-rt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                  </tr>
                  <tr>
                    <td class="contentbox-l"><img id="Img4" runat="server" src="~/images/contentbox-l.jpg" width="8" height="1" border="0" alt="iQMedia" /></td>
                    <td valign="top" bgcolor="#FFFFFF" class="pad-bt"><table width="94%" border="0" align="center" cellpadding="0" cellspacing="0">
                      <tr>
                        <td><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                          <tr>
                            <td class="pagetitle">Redlasso Station Market</td>
                          </tr>
                        </table>
                        <table width="100%" border="0" cellspacing="3" cellpadding="0">
                            <tr>
                               <td width="2%">
                                </td>
                                <td width="98%" align="left" valign="top">
                               <asp:ValidationSummary ID="vsIQMediaSearch" EnableClientScript="true" runat="server"
                                      ValidationGroup="validate" ForeColor="#bd0000" Font-Size="Smaller" />
                                    <asp:Label ID="lblMessage" runat="server" ForeColor="Green" Font-Size="Smaller"></asp:Label>
                                  </td>
                             </tr>
                        </table>
                        </td>
                      </tr>
                      <tr>
                        <td><table width="100%" border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                          <tr>
                            <td><table border="0" cellspacing="5" cellpadding="0">
                                <tr>
                                  <td class="content-text">StationMarketName:</td>
                                  <td><label>
                                        <asp:TextBox ID="txtStationMarketName" runat="server" CssClass="textbox03"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvStationMarketName" runat="server" Text="*" Display="Dynamic" ValidationGroup="validate" ControlToValidate="txtStationMarketName" ErrorMessage="Please Enter Station Market Name."></asp:RequiredFieldValidator>
                                  </label></td>
                                  <td><label>
                                        <asp:Button ID="btnAdd" runat="server" Text="Add" CssClass="btn-blue" ValidationGroup="validate" 
                                         onclick="btnAdd_Click" />
                                  </label></td>
                                </tr>
                              </table></td>
                          </tr>
                        </table></td>
                        </tr>
                      
                      <tr>
                        <td><table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                        <tr>
                            <td height="10"></td>
                          </tr>
                       <tr>
                            <td height="10"></td>
                          </tr>
                          <tr>
                            <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td width="3"><img id="Img5" runat="server" src="~/images/bluebox-hd-left.jpg" width="3" height="24" border="0" alt="iQMedia" /></td>
                                <td class="bluebox-hd">&nbsp;<br /></td>
                                <td width="3"><img id="Img6" runat="server" src="~/images/bluebox-hd-right.jpg" width="3" height="24" border="0" alt="iQMedia" /></td>
                              </tr>
                            </table></td>
                          </tr>
                          <tr>
                            <td height="3"></td>
                          </tr>
                          <tr>
                            <td><table width="100%" border="1" cellpadding="5" cellspacing="0" bordercolor="#e4e4e4" style="border-collapse:collapse;">
                            <tr>
                            <td colspan="3">
                             <asp:UpdatePanel ID="upRedlassoStationMarket" runat="server" UpdateMode="Conditional">
                               <ContentTemplate>
                                <asp:GridView ID="gvStationMarket" runat="server" Width="100%" border="1"  AllowPaging="true"
                                    CellPadding="5" AutoGenerateEditButton="true" pagesize="15" PageIndex="0" PagerSettings-Mode="NextPrevious"
                                CellSpacing="0" BorderColor="#e4e4e4" Style="border-collapse: collapse;" HeaderStyle-CssClass="grid-th"
                                    AutoGenerateColumns="false" EmptyDataText="No Data Found" 
                                    onrowcancelingedit="gvStationMarket_RowCancelingEdit"
                                    onrowediting="gvStationMarket_RowEditing" 
                                    onrowupdating="gvStationMarket_RowUpdating" 
                                    onpageindexchanging="gvStationMarket_PageIndexChanging" 
                                    PagerSettings-NextPageImageUrl="~/Images/arrow-next.jpg" PagerSettings-PreviousPageImageUrl="~/Images/arrow-previous.jpg"
                                    >
                                <Columns>
                                    <asp:BoundField HeaderText="RedlassoStationMarketID" DataField="RedlassoStationMarketKey" HeaderStyle-HorizontalAlign="Left" >
                                         <ItemStyle CssClass="content-text"></ItemStyle>
                                     </asp:BoundField>
                                    <asp:TemplateField HeaderText="StationMarketName" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStationMarketName" runat="server" Text='<%# Bind("StationMarketName") %>'></asp:Label>
                                        </ItemTemplate>
                                         <ItemStyle CssClass="content-text"></ItemStyle>
                                        <EditItemTemplate>
                                            <asp:HiddenField ID="hdnRedlassoStationMarketKey" runat="server" Value='<%# Bind("RedlassoStationMarketKey") %>' />
                                            <asp:TextBox ID="txtMarketName" runat="server" Text='<%# Bind("StationMarketName") %>'></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvMarketNameGrid" runat="server" ErrorMessage="*" ControlToValidate="txtMarketName"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="IsActive" HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("IsActive") %>'></asp:Label>
                                        </ItemTemplate>
                                         <ItemStyle CssClass="content-text"></ItemStyle>
                                        <EditItemTemplate>
                                            <asp:DropDownList ID="ddlStatus" runat="server">
                                                <asp:ListItem Text="Select Status" Value="-1"></asp:ListItem>
                                                <asp:ListItem Text="True" Value="0"></asp:ListItem>
                                                <asp:ListItem Text="False" Value="1"></asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:RequiredFieldValidator ID="rfvStatus" runat="server" InitialValue="-1" ControlToValidate="ddlStatus" ErrorMessage="*"></asp:RequiredFieldValidator>
                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                </asp:GridView>
                               </ContentTemplate>
                               </asp:UpdatePanel>
                            </td>
                        </tr>
                            </table></td>
                          </tr>
                          <tr>
                            <td height="35"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td>&nbsp;</td>
                                <td align="right" valign="top" class="contenttext-small">&nbsp;</td>
                              </tr>
                            </table></td>
                          </tr>
                        </table></td>
                      </tr>
                    </table></td>
                    <td class="contentbox-r"><img id="Img7" runat="server" src="~/images/contentbox-r.jpg" width="8" height="1" border="0" alt="iQMedia" /></td>
                  </tr>
                  <tr>
                    <td><img id="Img8" runat="server" src="~/images/contentbox-lb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                    <td class="contentbox-b"><img id="Img9" runat="server" src="~/images/contentbox-b.jpg" width="1" height="8" border="0" alt="iQMedia" /></td>
                    <td><img id="Img10" runat="server" src="~/images/contentbox-rb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                  </tr>
                </table>
<asp:UpdateProgress ID="updProgressRedlassoStationMarket" runat="server" AssociatedUpdatePanelID="upRedlassoStationMarket" DisplayAfter="0">
  <ProgressTemplate>
    <div>
     <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index:99999999;" alt="Loading..." id="imgLoading" />
    </div>
 </ProgressTemplate>
 </asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtenderRedlassoStationMarket" ControlToOverlayID="upRedlassoStationMarket" TargetControlID="updProgressRedlassoStationMarket" CssClass="updateProgress" />
