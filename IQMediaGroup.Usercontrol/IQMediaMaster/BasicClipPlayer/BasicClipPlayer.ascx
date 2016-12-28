<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BasicClipPlayer.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.Clip.BasicClipPlayer" %>
<script type="text/javascript">
    function showClipPlayer() {

        //document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_tblPlayer').style.display = "block";

        var _JPlayer = document.getElementById('<%= Player.ClientID%>');

        var _JDivRawMedia = document.getElementById('<%= divRawMedia.ClientID%>');
        var _JHF = document.getElementById('<%= hfDivRawMedia.ClientID %>');

        if (_JHF != null && _JDivRawMedia != null) {
            _JDivRawMedia.innerHTML = _JHF.value;
        }

        if (_JPlayer != null) {
            $(_JPlayer).show('slow');
        }

    }
    function hideClipPlayer() {

        var _JPlayer = document.getElementById('<%= Player.ClientID%>');

        if (_JPlayer != null) {
            $(_JPlayer).hide('slow');
        }

        var _JDivRawMedia = document.getElementById('<%= divRawMedia.ClientID%>');

        if (_JDivRawMedia != null) {
            _JDivRawMedia.innerHTML = null;
        }

    }

    function showClipCaption() {

        /* document.getElementById('<%=tblClosedCaption.ClientID %>').style.display = "block";*/

        var _JCaption = document.getElementById('<%= ClosedCaption.ClientID%>');

        if (_JCaption != null) {
            $(_JCaption).show('slow');
        }
    }
    function hideClipCaption() {
        var _JCaption = document.getElementById('<%= ClosedCaption.ClientID%>');

        if (_JCaption != null) {
            $(_JCaption).hide('slow');
        }
    }
    
</script>
<asp:HiddenField ID="hfDivRawMedia" runat="server" />
<table width="100%" border="0" cellspacing="0" cellpadding="0">
    <tr>
        <td>
            <table id="Table1" width="100%" runat="server" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <div class="show-hide">
                            <div class="float-left" onclick="showClipPlayer();">
                                <a href="javascript:;">
                                    <img src="../images/show.png" alt="">
                                    SHOW PLAYER</a></div>
                            <asp:Label ID="lblActiveSearch" runat="server"></asp:Label>
                            <div class="float-right" onclick="hideClipPlayer();">
                                <a href="javascript:;">HIDE
                                    <img src="../images/hiden.png" alt=""></a></div>
                        </div>
                    </td>
                    <%--<td height="25" class="show-hide">
                        <table width="580px" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td align="left" style="padding-left: 10px;">
                                    <div class="show" style="cursor: pointer;" onclick="showClipPlayer();">
                                        SHOW PLAYER</div>
                                </td>
                                <td align="right" style="padding-right: 10px">
                                    <div class="hide" style="cursor: pointer;" onclick="hideClipPlayer();">
                                        HIDE</div>
                                </td>
                            </tr>
                        </table>
                    </td>--%>
                </tr>
                <tr>
                    <td>
                        <div id="Player" class="border01" runat="server" style="width: 580px; overflow: hidden;">
                            <table border="0" width="100%" align="center" cellpadding="0" cellspacing="6">
                                <tr>
                                    <td>
                                        <table cellpadding="0" cellspacing="0" width="100%" class="border01" border="0" align="center">
                                            <tr>
                                                <td align="center">
                                                    <table cellpadding="0" cellspacing="6" border="0" id="tblPlayer" runat="server">
                                                        <tr>
                                                            <td valign="top">
                                                                <%--       <div id="wrapper" runat="server" jquery1261727998253="53" sizcache="69" sizset="1">
                                                                        <div id="videoplayer" jquery1261727998253="87" sizcache="56" sizset="0">
                                                                            <div jquery1261727998253="86" sizcache="56" sizset="0">
                                                                                <div id="videoframe" sizcache="56" sizset="4">
                                                                                    <div id="RL_Player_Wrapper" style="margin-top: 10px; background: #000000; position: relative">
                                                                                        <div id="RL_Player" style="width: 545px; height: 340px">--%>
                                                                <div style="width: 545px; height: 340px;" id="divRawMedia" runat="server">
                                                                </div>
                                                                <%-- </div>
                                                                                    </div>
                                                                                </div>
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
                                                    <table id="tblClosedCaption" width="100%" runat="server" border="0" cellpadding="0"
                                                        cellspacing="0">
                                                        <tr>
                                                            <td height="25">
                                                                <div class="show-hide">
                                                                    <div class="float-left" onclick="showClipCaption();">
                                                                        <a href="javascript:;">
                                                                            <img src="../images/show.png" alt="">
                                                                            SHOW CAPTION</a></div>
                                                                    <asp:Label ID="Label1" runat="server"></asp:Label>
                                                                    <div class="float-right" onclick="hideClipCaption();">
                                                                        <a href="javascript:;">HIDE
                                                                            <img src="../images/hiden.png" alt=""></a></div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <div id="ClosedCaption" runat="server" style="display: none">
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
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblMsg" runat="server"></asp:Label>
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
