<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IRawMedia.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IRawMedia.IRawMedia" %>
<%@ Register src="~/UserControl/IQMediaMaster/LoginIframe/LoginIframe.ascx" tagname="LoginIframe" tagprefix="uc1" %>
<asp:UpdatePanel ID="upIframe" runat="server">
    <ContentTemplate>
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr id="trPlayer" runat="server">
        <td>
            <asp:HiddenField ID="hfDivRawMedia" runat="server" />
            <table id="Table1" runat="server" border="0" cellpadding="0" cellspacing="0">
                <tr id="trShowHide" runat="server">
                    <td height="25" class="hdbar">
                        <table width="580px" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" style="padding-left: 10px">
                                    <div class="show" style="cursor: pointer;" onclick="showRawMediaPlayer(true);">
                                        SHOW PLAYER</div>
                                </td>
                                <td align="right" style="padding-right: 10px">
                                    <div class="hide" style="cursor: pointer;" onclick="hideRawMediaPlayer();">
                                        HIDE</div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="Player" runat="server" class="border01" style="width: 580px; overflow: hidden;">
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
                                                    <table cellpadding="0" cellspacing="6" border="0">
                                                        <tr>
                                                            <td valign="top">
                                                                <div id="wrapper" runat="server" jquery1261727998253="53" sizcache="69" sizset="1">
                                                                    <div id="videoplayer" jquery1261727998253="87" sizcache="56" sizset="0">
                                                                        <div jquery1261727998253="86" sizcache="56" sizset="0">
                                                                            <div id="videoframe" sizcache="56" sizset="4">
                                                                                <div id="RL_Player_Wrapper" style="margin-top: 10px; background: #000000; position: relative">
                                                                                    <div id="RL_Player" style="width: 545px; height: 340px">
                                                                                        <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server">
                                                                                        </div>
                                                                                    </div>
                                                                                </div>
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
                                                                            </div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table id="tblClosedCaption" width="100%" runat="server" border="0" cellpadding="0"
                                                        cellspacing="0">
                                                        <tr>
                                                            <td height="25" class="hdbar">
                                                                <table style="width: 565px;" border="0" cellspacing="0" cellpadding="0">
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
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="ClosedCaption" runat="server" style="display: none;">
                                                                    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
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
    <tr id="trLogin" runat="server">
        <td style="font-size:90%;">           
        <div  class="login-form">
            <uc1:LoginIframe ID="ucLoginIframe" runat="server" OnLoggedIn="ucLoginIframe_LoggedIn" />
            </div>
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
