<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ClipPlayer.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.ClipPlayer.ClipPlayer" %>
<%--<%@ Register Src="../RightSideAdvertise/RightSideAdvertise.ascx" TagName="UCRightSideAdvertise"
    TagPrefix="UC" %>--%>
<%@ Register Assembly="FlashControl" Namespace="Bewise.Web.UI.WebControls" TagPrefix="Bewise" %>
<%@ Register Src="~/UserControl/IQMediaMaster/RightTopLogin/RightTopLogin.ascx" TagName="UCRightTopLogin"
    TagPrefix="UC" %>
<table width="545px" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td align="center" valign="middle">
            <table border="0" align="left" width="545px" cellpadding="6" cellspacing="0" class="border01">
                <tr>
                    <td>
                        <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server" visible="false">
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table id="tblClosedCaption" style="width: 545px; overflow: hidden;" runat="server"
                            border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="25" class="hdbar">
                                    <table style="width: 545px; overflow: hidden" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td>
                                                <div class="show-hide">
                                                    <div class="float-left" onclick="showdivCaption();">
                                                        <a href="javascript:;">
                                                            <img src="../images/show.png" alt="">
                                                            SHOW CAPTION</a></div>
                                                    <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
                                                    <div class="float-right" onclick="hidedivCaption();">
                                                        <a href="javascript:;">HIDE
                                                            <img src="../images/hiden.png" alt=""></a></div>
                                                </div>
                                            </td>
                                            <%--<td align="left" style="padding-left: 10px">
                                                <div class="show" style="cursor: pointer;" onclick="showdivCaption();">
                                                    SHOW CAPTION</div>
                                            </td>
                                            <td align="right" style="padding-right: 10px">
                                                <div class="hide" style="cursor: pointer;" onclick="hidedivCaption();">
                                                    HIDE</div>
                                            </td>--%>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divClosedCaption" style="width: 545px; overflow: hidden; display: none;"
                                        runat="server">
                                        <table style="width: 545px; overflow: hidden" border="0" align="left" cellpadding="0"
                                            cellspacing="0">
                                            <tr>
                                                <td height="63" class="grey-grad">
                                                    <table style="width: 545px; overflow: hidden" border="0" align="left" cellpadding="0"
                                                        cellspacing="0">
                                                        <tr>
                                                            <td align="left" class="heading-blue2">
                                                                Closed Caption:
                                                                <br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="content-text-grey">
                                                                <div id="DivCaption" runat="server" class="panel" style="height: 242px; overflow-y: auto;">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
