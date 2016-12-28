<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IframeRawMediaReportH.ascx.cs" Inherits="IQMediaGroup.Reports.Usercontrol.IQMediaMaster.IframeRawMediaReportH.IframeRawMediaReportH" %>
<div id="divIFrameMain" class="modalPopupMediaDiv">
    <div id="divCapMain" class="Caption">
        <div id="DivCaption" runat="server">
        </div>
    </div>
    <div id="divShowCaption" class="divShowCaption" onclick="RegisterCCCallback();">
        <img src="../../../Images/right_arrow_cc.gif" id="imgCCDirection" alt="Show CC" />
    </div>
    <div class="modalPopupPlayer" id="divRawMedia" runat="server">
    </div>
    <div id="time" class="clear" style="margin-left: 38.5%;">
        <div id="timeBar" style="display: block; padding: 0;">
            <img id="time0" class="hourSeek" runat="server" style="cursor: pointer" src="~/images/time00.GIF"
                onclick="setSeekPoint(0, this);" />
            <img id="time10" class="hourSeek" runat="server" style="opacity: 0.4; cursor: pointer"
                src="~/images/time10.GIF" onclick="setSeekPoint(600, this);" />
            <img id="time20" class="hourSeek" runat="server" style="opacity: 0.4; cursor: pointer"
                src="~/images/time20.GIF" onclick="setSeekPoint(1200, this);" />
            <img id="time30" class="hourSeek" runat="server" style="opacity: 0.4; cursor: pointer"
                src="~/images/time30.GIF" onclick="setSeekPoint(1800, this);" />
            <img id="time40" class="hourSeek" runat="server" style="opacity: 0.4; cursor: pointer"
                src="~/images/time40.GIF" onclick="setSeekPoint(2400, this);" />
            <img id="time50" class="hourSeek" runat="server" style="opacity: 0.4; cursor: pointer"
                src="~/images/time50.GIF" onclick="setSeekPoint(3000, this);" />
        </div>
    </div>
</div>

