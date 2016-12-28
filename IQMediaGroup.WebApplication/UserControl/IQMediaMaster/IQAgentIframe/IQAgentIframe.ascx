<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQAgentIframe.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQAgentIframe.IQAgentIframe" %>
<%@ Register Assembly="FlashControl" Namespace="Bewise.Web.UI.WebControls" TagPrefix="Bewise" %>
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
                        <table id="tblClosedCaption" align="center" style="width: 560px; padding-bottom: 5px;"
                            runat="server" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td height="25" style="width: 560px;">
                                    <div class="show-bg">
                                        <div class="show-hide">
                                            <div class="float-left" onclick="showCaption();">
                                                <a href="javascript:;">
                                                    <img src="../images/show.png" alt="">
                                                    SHOW CAPTION</a></div>
                                            <div class="float-right" onclick="hideCaption();">
                                                <a href="javascript:;">HIDE
                                                    <img src="../images/hiden.png" alt=""></a></div>
                                        </div>
                                    </div>                                    
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="ClosedCaption" align="center" runat="server" style="display: none; width: 560px;">
                                        <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td height="63" class="grey-grad">
                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" class="heading-blue2">
                                                                Closed Caption:<br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" class="content-text-grey">
                                                                <div id="DivCaption" runat="server" class="panel" style="overflow-y: auto;">
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
