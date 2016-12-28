<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Contact.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.Contact.Contact" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
                    <table width="648px" border="0" align="center" cellpadding="0" cellspacing="0">
                      <tr>
                        <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td width="3"><img img runat="Server" src="~/images/pagetitle-left.jpg" width="3" height="30" border="0" alt="iQMedia" /></td>
                              <td class="pagetitle">Interested in iQ media</td>
                              <td width="3"><img img runat="Server" src="~/images/pagetitle-right.jpg" width="3" height="30" border="0" alt="iQMedia" /></td>
                            </tr>
                        </table></td>
                      </tr>
                      <tr>
                        <td height="10"></td>
                      </tr>
                     
                <tr>
                    <td> <table width="100%" border="0" cellspacing="3" cellpadding="0">
                            <tr>
                                <td width="148px"></td>
                                <td width="500px" align="left" valign="top">
                                    <asp:ValidationSummary ID="vsErrors" EnableClientScript="true" runat="server" 
                                        ValidationGroup="IQMediaContactUs" ForeColor="#bd0000" Font-Size="Smaller"
                                      />
                                    <asp:Label ID="lblError" runat="server" Visible="False" Font-Size="Small" 
                                    ForeColor="Red"></asp:Label>
                                </td>
                                
                            </tr>
                        </table></td>
                </tr>
                      <tr>
                        <td class="content-text"><div class='flexcroll' id="mycustomscroll">
                          <table width="100%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                              <td valign="top"><table width="97%" border="0" align="right" cellpadding="0" cellspacing="0">
                                <tr>
                                  <td>&nbsp;</td>
                                </tr>
                                <tr>
                                  <td><table width="100%" border="0" cellspacing="0" cellpadding="2">
                                    <tr>
                                      <td width="125" align="right">First Name:</td>
                                      <td><asp:TextBox ID="txtFirstName" runat="server" CssClass="textbox02" TabIndex="50" MaxLength="50"/>
                                          <cc1:PropertyProxyValidator ID="pplFirstName" runat="server" 
                                              ControlToValidate="txtFirstName" Display="Dynamic" Text="*"
                                       PropertyName="FirstName" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs" 
                                        RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" 
                                              Font-Size="Smaller"></cc1:PropertyProxyValidator></td>
                                    </tr>
                                    <tr>
                                      <td align="right">Last Name:</td>
                                      <td><asp:TextBox ID="txtLastName" runat="server" CssClass="textbox02" TabIndex="51" MaxLength="50"></asp:TextBox>
                                          <cc1:PropertyProxyValidator ID="ppvLastName" runat="server" 
                                              ControlToValidate="txtLastName" Display="Dynamic" Text="*"
                                       PropertyName="LastName" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                        RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" 
                                              Font-Size="Smaller"></cc1:PropertyProxyValidator></td>
                                    </tr>
                                    <tr>
                                      <td align="right">Company name:</td>
                                      <td><asp:TextBox ID="txtCompanyName" runat="server" CssClass="textbox02" TabIndex="52" MaxLength="50"></asp:TextBox>
                                         </td>
                                    </tr>
                                    <tr>
                                      <td align="right">Title:</td>
                                      <td><asp:TextBox ID="txtTitle" runat="server" CssClass="textbox02" TabIndex="53" MaxLength="50"></asp:TextBox>
                                          </td>
                                    </tr>
                                    <tr>
                                      <td align="right">Email Address:</td>
                                      <td><asp:TextBox ID="txtEmailAddress" runat="server" CssClass="textbox02" TabIndex="54" MaxLength="50"></asp:TextBox>
                                          <cc1:PropertyProxyValidator ID="ppvEmail" runat="server" 
                                              ControlToValidate="txtEmailAddress" Display="Dynamic" Text="*"
                                       PropertyName="Email" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                        RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" 
                                              Font-Size="Smaller"></cc1:PropertyProxyValidator></td>
                                    </tr>
                                    <tr>
                                      <td align="right">Telephone:</td>
                                      <td><asp:TextBox ID="txtTelephone" runat="server" CssClass="textbox02" TabIndex="55" MaxLength="50"></asp:TextBox>
                                          <cc1:PropertyProxyValidator ID="ppvContactNo" runat="server" 
                                              ControlToValidate="txtTelephone" Display="Dynamic" Text="*"
                                       PropertyName="TelephoneNo" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                        RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" 
                                              Font-Size="Smaller"></cc1:PropertyProxyValidator></td>
                                    </tr>
                                    <tr>
                                      <td align="right" valign="top">Best time to contact<br />
                                        and Comments:</td>
                                      <td><asp:TextBox ID="txtComments" runat="server" TextMode="MultiLine" Rows="4" CssClass="textbox02" TabIndex="56" MaxLength="500"></asp:TextBox></td>
                                    </tr>
                                    <tr>
                                      <td></td>
                                      <td><img img runat="Server" src="~/images/spacer.gif" width="243" height="1" border="0" class="hsep3" alt="iQMedia" /></td>
                                    </tr>
                                    <tr>
                                      <td align="right" valign="top">&nbsp;</td>
                                      <td><asp:Button ID="btnSubmit" runat="server" CssClass="submit-btn" Text="Submit" 
                                ValidationGroup="IQMediaContactUs" onclick="btnSubmit_Click" TabIndex="57"/></td>
                                    </tr>
                                     <tr>
                                       <td></td>
                                        <td><asp:Label ID="lblError1" runat="server" Visible="False" ForeColor="Red"></asp:Label></td>
                                    </tr>
                                  </table></td>
                                </tr>
                                
                              </table></td>
                            </tr>
                          </table>
                              </div></td>
                      </tr>
                      
                    </table>
                    
                    