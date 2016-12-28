<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RawMediaPlayer.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.RawMediaPlayer.RawMediaPlayer" %>
<table cellpadding="0" cellspacing="0" border="0">
    <tr>
        <td>
            <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table>
                        <tr>
                            <td>
                                <table border="0" align="center" cellpadding="0" cellspacing="6" class="border01">
                                    <tr>
                                        <td>
                                            <div id="RL_Player" style="width: 545px; height: 340px">
                                                <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server">
                                                </div>
                                            </div>
                                            <div id="time" runat="server" visible="false">
                                                <div id="timeBar" style="display: block; padding: 0;">
                                                    <img id="time0" class="hourSeek" runat="server" style="" src="~/images/time00.GIF"
                                                        onclick="setSeekPoint(0, this);" />
                                                    <img id="time10" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time10.GIF"
                                                        onclick="setSeekPoint(600, this);" />
                                                    <img id="time20" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time20.GIF"
                                                        onclick="setSeekPoint(1200, this);" />
                                                    <img id="time30" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time30.GIF"
                                                        onclick="setSeekPoint(1800, this);" />
                                                    <img id="time40" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time40.GIF"
                                                        onclick="setSeekPoint(2400, this);" />
                                                    <img id="time50" class="hourSeek" runat="server" style="opacity: 0.4;" src="~/images/time50.GIF"
                                                        onclick="setSeekPoint(3000, this);" />
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table width="559" border="0" align="center" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td height="63" class="grey-grad">
                                            <table width="96%" border="0" align="center" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="heading-blue2">
                                                        Closed Caption:<br />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="content-text-grey">
                                                        <div id="DivCaption" runat="server" class="panel" style="overflow-y: auto;">
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
                </ContentTemplate>
            </asp:UpdatePanel>
        </td>
    </tr>
</table>
