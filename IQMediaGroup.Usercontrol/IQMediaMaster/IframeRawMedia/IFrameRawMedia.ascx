<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IFrameRawMedia.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IframeRawMedia.IFrameRawMedia" %>
<style type="text/css">
    body
    {
        min-width: 581px !important;
    }
</style>
<asp:UpdatePanel ID="upIframe" runat="server">
    <ContentTemplate>
        <table width="100%" border="0" cellspacing="0" cellpadding="0">
            <tr id="trPlayer" runat="server">
                <td>
                    <asp:HiddenField ID="hfDivRawMedia" runat="server" />
                    <table width="100%" id="Table1" runat="server" border="0" cellpadding="0" cellspacing="0">
                        <tr id="trShowHide" runat="server">
                            <td>
                                <div class="show-bg">
                                    <div class="show-hide">
                                        <div class="float-left" onclick="showRawMediaPlayer(true);">
                                            <a href="javascript:;">
                                                <img src="../images/show.png" alt="">
                                                SHOW PLAYER</a></div>
                                        <div class="float-right" onclick="hideRawMediaPlayer();">
                                            <a href="javascript:;">HIDE
                                                <img src="../images/hiden.png" alt=""></a></div>
                                    </div>
                                </div>
                                <%--"hdbaZXr"--%>
                                <%--  <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td align="left" style="padding-left: 10px">
                                            <div class="show" style="cursor: pointer;" onclick="showRawMediaPlayer(true);">
                                                <img alt="" src="../images/show.png">SHOW PLAYER</div>
                                        </td>
                                        <td align="right" style="padding-right: 10px">
                                            <div class="hide" style="cursor: pointer;" onclick="hideRawMediaPlayer();">
                                                HIDE<img alt="" src="../images/hiden.png"></div>
                                        </td>
                                    </tr>
                                </table>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <div id="Player" runat="server" class="border01" style="width: 100%; overflow: hidden;
                                    display: none">
                                    <table border="0" width="100%" align="center" cellpadding="0" cellspacing="6">
                                        <tr>
                                            <td>
                                                <div style="text-align: right; padding: 5px 5px 5px 0px;">
                                                    <a href="javascript:void(0);" onclick="window.open('../Help/Help.htm',null,'height=450,width=500,scrollbars=yes,toolbar=no,menubar=no,location=no');">
                                                        facing problem while making clips ?</a>
                                                </div>
                                                <table cellpadding="0" cellspacing="0" width="100%" class="border01" border="0" align="center">
                                                    <tr>
                                                        <td>
                                                            <table align="center" cellpadding="0" cellspacing="6" border="0">
                                                                <tr>
                                                                    <td valign="top">
                                                                        <%-- <div id="wrapper" runat="server" jquery1261727998253="53" sizcache="69" sizset="1">
                                                                    <div id="videoplayer" jquery1261727998253="87" sizcache="56" sizset="0">
                                                                        <div jquery1261727998253="86" sizcache="56" sizset="0">
                                                                            <div id="videoframe" sizcache="56" sizset="4">
                                                                                <div id="RL_Player_Wrapper" style="margin-top: 10px; background: #000000; position: relative">
                                                                                    <div id="RL_Player" style="width: 545px; height: 340px">--%>
                                                                        <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server">
                                                                        </div>
                                                                        <%-- </div>
                                                                                </div>--%>
                                                                        <div id="time" runat="server">
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
                                                                        <%-- </div>
                                                                        </div>
                                                                    </div>
                                                                </div>--%>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td>
                                                            <table id="tblClosedCaption" align="center" style="width: 560px; padding-bottom: 5px;"
                                                                runat="server" border="0" cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td height="25" style="width:560px;">
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
                                                                        <%-- <table style="width: 560px;" border="0" cellspacing="0" cellpadding="0">
                                                                            <tr>
                                                                                <td align="left" style="padding-left: 10px">
                                                                                    <div class="show" style="cursor: pointer;" onclick="showCaption();">
                                                                                        SHOW CAPTION</div>
                                                                                </td>
                                                                                <td align="right" style="padding-right: 10px">
                                                                                    <div class="hide" style="cursor: pointer;" onclick="hideCaption();">
                                                                                        HIDE</div>
                                                                                </td>
                                                                            </tr>
                                                                        </table>--%>
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
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
                </td>
            </tr>
        </table>
    </ContentTemplate>
</asp:UpdatePanel>
