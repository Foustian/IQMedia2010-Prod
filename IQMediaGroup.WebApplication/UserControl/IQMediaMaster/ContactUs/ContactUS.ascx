<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ContactUS.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.ContactUS.ContactUS" %>
<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>
<div class="contact">
    <div class="block left-top">
        <div class="block-inner">
            <div class="block-top">
                <!-- -->
            </div>
            <div class="block-top-end">
                <!-- -->
            </div>
            <div class="block-left-end">
                <!-- -->
            </div>
            <div class="block-lt-corner">
                <!-- -->
            </div>
            <div class="block-content columns-wrapper">
                <div class="left-column">
                    <h2>
                        Contact Us</h2>
                    <div class="text-block">
                        <h3>
                            iQ media Group LLC<br />
                            370 West Sandy Ridge Road<br />
                            Building 2<br />
                            Doylestown, PA 18901<br />
                            (267) 898-0651<br />
                            <br />
                            <a href="mailto:info@iqmediacorp.com">info@iQMediaCorp.com</a><br />
                            <br />
                            Call us Toll Free at
                            <br />
                            (855-IQMEDIA) 855-476-3342</h3>
                    </div>
                </div>
                <div class="right-column">
                    <p>
                        Please contact us with any questions or comments.</p>
                    <table cellpadding="5" cellspacing="0" border="0" class="contacttable">
                        <tr>                            
                            <td colspan="3">
                                <asp:ValidationSummary ID="vsErrors" EnableClientScript="true" DisplayMode="List"
                                    runat="server" ValidationGroup="IQMediaContactUs" ForeColor="#bd0000" Font-Size="Smaller" />
                                <asp:Label ID="lblError" runat="server" Visible="False" Font-Size="Small" ForeColor="Red"
                                    Font-Bold="True"></asp:Label>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                First Name:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtFirstName" runat="server" CssClass="styled-input-200" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <cc1:PropertyProxyValidator ID="pplFirstName" runat="server" ControlToValidate="txtFirstName"
                                    Display="Static" Text="*" PropertyName="FirstName" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                    RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" Font-Size="Smaller"></cc1:PropertyProxyValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Last Name:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtLastName" runat="server" CssClass="styled-input-200" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <cc1:PropertyProxyValidator ID="ppvLastName" runat="server" Display="Static" Text="*"
                                    ControlToValidate="txtLastName" PropertyName="LastName" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                    RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" Font-Size="Smaller"></cc1:PropertyProxyValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Company:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtCompanyName" runat="server" CssClass="styled-input-200" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Title:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtTitle" runat="server" CssClass="styled-input-200" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Phone:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtTelephone" runat="server" CssClass="styled-input-200" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <cc1:PropertyProxyValidator ID="ppvContactNo" runat="server" ControlToValidate="txtTelephone"
                                    Display="Static" Text="*" PropertyName="TelephoneNo" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                    RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" Font-Size="Smaller"></cc1:PropertyProxyValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Email:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="styled-input-200" MaxLength="50"></asp:TextBox>
                            </td>
                            <td>
                                <cc1:PropertyProxyValidator ID="ppvEmail" runat="server" ControlToValidate="txtEmailAddress"
                                    Display="Static" Text="*" PropertyName="Email" SourceTypeName="IQMediaGroup.Core.HelperClasses.IQMediaContactUs"
                                    RulesetName="IQMediaContactUs" ValidationGroup="IQMediaContactUs" Font-Size="Smaller"></cc1:PropertyProxyValidator>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                How Did You Hear About Us?
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtHearAboutUs" runat="server" CssClass="styled-input-200"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Best Time to Contact You:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtTimeToContact" runat="server" CssClass="styled-input-200"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="leftcolumn">
                                Comments:
                            </td>
                            <td class="rightcolumn">
                                <asp:TextBox ID="txtComments" runat="server" CssClass="styled-input-200" MaxLength="500"></asp:TextBox>
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="right">
                                <asp:Button ID="btnSubmit" runat="server" CssClass="styled-button" Text="Submit"
                                    OnClick="btnSubmit_Click" ValidationGroup="IQMediaContactUs" />
                            </td>
                            <td>
                            
                            </td>
                        </tr>
                    </table>                   
                </div>
            </div>
        </div>
    </div>
</div>