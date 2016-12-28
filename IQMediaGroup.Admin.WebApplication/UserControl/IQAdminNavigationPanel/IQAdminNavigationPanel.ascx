<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAdminNavigationPanel.ascx.cs" Inherits="IQMediaGroup.Admin.Usercontrol.IQAdminNavigationPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
 <style type="text/css">
     .style1
     {
         height: 19px;
     }
 </style>
 <table width="100%" border="0" cellspacing="0" cellpadding="0" >
              <tr>
                <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                  <tr>
                    <td width="8"><img runat="server" src="~/images/contentbox-lt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                    <td class="contentbox-t"><img runat="server" src="~/images/contentbox-t.jpg" width="1" height="8" border="0" alt="iQMedia" /></td>
                    <td width="8"><img runat="server" src="~/images/contentbox-rt.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                  </tr>
                  <tr>
                    <td class="contentbox-l"><img runat="server" src="~/images/contentbox-l.jpg" width="8" height="1" border="0" alt="iQMedia" /></td>
                    <td valign="top" bgcolor="#FFFFFF"><table width="100%" border="0" cellspacing="0" cellpadding="0">
                      <tr>
                        <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                         <%-- <tr>
                            <td><table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr>
                                  <td width="3"><img runat="server" src="~/images/bluebox-hd-left.jpg" width="3" height="24" border="0" alt="iQMedia" /></td>
                                  <td class="bluebox-hd">&nbsp;Admin<br /></td>
                                  <td width="3"><img runat="server" src="~/images/bluebox-hd-right.jpg" width="3" height="24" border="0" alt="iQMedia" /></td>
                                </tr>
                            </table></td> 
                          </tr>--%>
                          <tr>
                            <td><img runat="server" src="~/images/spacer.gif" width="1" height="1" border="0" alt="iQMedia" /></td>
                          </tr>
                          <tr>
                            <td>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0">                                  
                                  <tr>
                                    <td valign="bottom"><table width="100%" class="AdminMenu" cellspacing="0" cellpadding="0">
                                      <tr>
                                        <td>Content Management</td>
                                      </tr>
                                       <tr>
                                        <td><a id="btnCareer" runat="server">Career</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnContactUs" runat="server">Contact Us</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnAboutUs" runat="server">About Us</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnProduct" runat="server">Product</a></td>
                                      </tr>
                                      <tr>
                                        <td class="style1"><a id="btnClientRegistration" runat="server">Client Registration</a></td>
                                      </tr>
                                       <tr>
                                        <td><a id="btnCustomerRegistration"  runat="server">Customer Registration</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnIQRole" runat="server" >IQ Role</a></td>
                                      </tr>
                                       <tr>
                                        <td><a id="btnCustomerRole" runat="server">Customer Role Mapping</a></td>
                                      </tr>
                                       <tr>
                                        <td><a id="btnClientRole" runat="server">Client Role Mapping</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnClipExport" runat="server">Clip Export</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnIQAgentConsole" runat="server">IQ Agent Console</a></td>
                                      </tr>
                                      <tr>
                                        <td><a id="btnIQAgentSetupPage" runat="server">IQ Agent Setup Page</a></td>
                                      </tr>
                                       <tr>
                                        <td><a id="btnPMGSearchDemo" runat="server">PMG Search Demo</a></td>
                                      </tr>
                                    </table></td>
                                  </tr>                                  
                                </table>
                            </td>
                          </tr>
                        </table></td>
                      </tr>
                      
                    </table></td>
                    <td class="contentbox-r"><img runat="server" src="~/images/contentbox-r.jpg" width="8" height="1" border="0" alt="iQMedia" /></td>
                  </tr>
                  <tr>
                    <td><img runat="server" src="~/images/contentbox-lb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                    <td class="contentbox-b"><img runat="server" src="~/images/contentbox-b.jpg" width="1" height="8" border="0" alt="iQMedia" /></td>
                    <td><img runat="server" src="~/images/contentbox-rb.jpg" width="8" height="8" border="0" alt="iQMedia" /></td>
                  </tr>
                </table></td>
              </tr>
              <tr>
                <td><img runat="server" src="~/images/spacer.gif" width="1" height="10" border="0" alt="iQMedia" /></td>
              </tr>
            </table>