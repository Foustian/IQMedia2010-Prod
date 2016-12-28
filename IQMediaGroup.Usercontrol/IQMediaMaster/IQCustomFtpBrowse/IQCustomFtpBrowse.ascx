<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQCustomFtpBrowse.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQCustomFtpBrowse.IQCustomFtpBrowse" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%--<asp:UpdatePanel ID="upFtpBrowse" runat="server" UpdateMode="Conditional">
    <ContentTemplate>--%>
        <div style="max-height: 450px; overflow: auto;">
            <table width="100%" border="0" cellspacing="0" cellpadding="5">
                <tr>
                    <td align="left" width="40%" valign="top">
                        <asp:TreeView ID="trVDir" runat="server" ImageSet="Arrows" OnSelectedNodeChanged="trVDir_SelectedNodeChanged">
                            <ParentNodeStyle Font-Bold="False" />
                            <HoverNodeStyle Font-Underline="True" ForeColor="#5555DD" />
                            <SelectedNodeStyle Font-Underline="True" ForeColor="DarkGray" HorizontalPadding="10px"
                                VerticalPadding="0px" />
                            <NodeStyle Font-Names="Verdana" Font-Size="8pt" ForeColor="Black" HorizontalPadding="5px"
                                NodeSpacing="0px" VerticalPadding="0px" />
                        </asp:TreeView>
                    </td>
                    <td align="left" width="60%" valign="top">
                        <asp:Label ID="lblerr" runat="server" Visible="false" ForeColor="Red"></asp:Label>
                        <asp:ValidationSummary ID="vgSelectFile" DisplayMode="List" runat="server" ValidationGroup="vgSelectOk"
                            runat="server" />
                        <asp:GridView ID="grdFiles" DataKeyNames="Name" BorderWidth="0" GridLines="None"
                            runat="server" AllowPaging="false" Width="100%" CellPadding="5" CellSpacing="0"
                            AutoGenerateColumns="false" ShowHeader="false" EmptyDataText="No Files Found">
                            <Columns>
                                <asp:TemplateField ShowHeader="false">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkFilename" OnClick="lnkFilename_Click" runat="server" Text='<%# Eval("Name") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
            </table>
        </div>
        <table width="100%" border="0" cellspacing="0" cellpadding="5">
            <tr style="height: 5px">
                <td>
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td colspan="2" align="left">
                    <strong>File name : </strong>
                    <asp:RequiredFieldValidator Display="Dynamic" ID="RequiredFieldValidator1" runat="server"
                        ValidationGroup="vgSelectOk" ControlToValidate="txtFileName" ErrorMessage="Please select a file."
                        Text="*"></asp:RequiredFieldValidator><asp:TextBox ID="txtFileName" ReadOnly="true"
                            ValidationGroup="vgSelectOk" runat="server" CssClass="textbox03" Width="342px"></asp:TextBox>
                    <asp:Button ID="btnOk" runat="server" ValidationGroup="vgSelectOk" Text="Ok" CssClass="btn-blue2"
                        OnClick="btnOk_Click" />
                    <asp:Button ID="btnCancel" OnClick="btnCancel_Click" runat="server" Text="Cancel"
                        CssClass="btn-blue2" />
                </td>
            </tr>
        </table>
    <%--</ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="upProgressFtpBrowse" runat="server" AssociatedUpdatePanelID="upFtpBrowse"
    DisplayAfter="0">
    <ProgressTemplate>
        <div>
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upProgressOverlayExtender"
    ControlToOverlayID="upFtpBrowse" TargetControlID="upProgressFtpBrowse" CssClass="updateProgress" />--%>
