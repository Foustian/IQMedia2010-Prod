<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IFrameMicrosite.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IFrameMicrosite.IFrameMicrosite" %>
<%@ Register Src="../ClipPlayer/ClipPlayer.ascx" TagName="ClipPlayer" TagPrefix="UCClipPlayer" %>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<style type="text/css">

@font-face {
    font-family: 'avenir_65medium';
    src: url('/Css/avenir-medium-webfont.eot');
    src: url('/Css/avenir-medium-webfont.eot?#iefix') format('embedded-opentype'),
         url('/Css/avenir-medium-webfont.woff') format('woff'),
         url('/Css/avenir-medium-webfont.ttf') format('truetype'),
         url('/Css/avenir-medium-webfont.svg#avenir_65medium') format('svg');
    font-weight: normal;
    font-style: normal;

}

body
{
    font: normal 12px "avenir_65medium";
}
    
a
{
    color: #000;
    text-decoration: none;
    outline:medium none;
}
a:hover
{
    text-decoration: underline;
}

.float-left {
	float: left;
}
.float-right {
	float: right;
} 
    
.show-hide{
    background-color:#f5f5f5;	
    /*padding:3px 10px 3px 10px;*/
    padding:3px;
    display:block;
    color:#2f2f2f;
    font-weight:bold;
    border:solid 1px #dfdfdf;
    border-radius: 5px;
    background:#dfdfdf url(../images/show-hide-bg.png) repeat-y center top;
    clear:both;
    overflow:hidden;
}
.show-hide a{
	display:block;
	color:#5e5e5e;
	text-decoration:none;
	margin-top:4px;
	font: normal 14px "avenir_65medium";
	text-transform: uppercase;
}
.show-hide a:hover{
	display:block;
	color:#32b0cf;
	text-decoration:none;
}

.heading-blue2 {
	/*font-family: Arial, Helvetica, sans-serif;*/
	font: 14px/24px "avenir_65medium";
	line-height: 19px;
	font-weight: bold;
	color: #4FB8E6;
	text-decoration: none;
}

.caption {
margin-bottom:5px;
width:100%;
cursor:pointer;
font: 14px/24px "avenir_65medium";
}
    
   
.updateProgress
{
    position: absolute;
    background-color: #989898;
    filter: Alpha(Opacity=70);
    opacity: .7;
    -moz-opacity: .7;
    padding: 0;
    margin: 0;
}
.updateProgress div
{
    padding: 4px;
    position: absolute;
    top: 40%;
    left: 44%;
}
    
.CssClipDetail
{
    background: #f5f5f5;
    padding: 15px;
    border: 2px solid #ddd;
    border-radius: 5px; 
    min-width: 225px;
    min-height: 130px;
}
    
.thumbwrap
{
    height: 100px;
    width: 100px;
    position: relative;
    background: #ddd;
}
.thumbwrap a
{
    position: absolute;
    left: 0px;
    top: 0px;
}
.download
{
    height: 24px;
    width: 22px;
    position: absolute;
    z-index: 10;
    right: 0px;
    bottom: 0px;
}
.download a
{
    text-decoration: none;
}
    
</style>
<script language="javascript" type="text/javascript">
    function showdivCaption() {

        document.getElementById('IFrameMicrosite1_ClipPlayerControl_divClosedCaption').style.display = "block";
        $("#IFrameMicrosite1_ClipPlayerControl_divClosedCaption").show('slow');
    }
    function hidedivCaption() {
        $("#IFrameMicrosite1_ClipPlayerControl_divClosedCaption").hide('slow');
        (document.getElementById('IFrameMicrosite1_ClipPlayerControl_divClosedCaption')).setAttribute('enable', 'true');
    }

    function ClearMsg() {
        document.getElementById("<%=lblMsg.ClientID %>").innerHTML = "";
    }

    function HideShowPopUp(ImageID, PnlPopupID, IsDisplay) {
        if (IsDisplay == 1) {
            var left = 0, top = 0;
            var winWidth = $(window).width();
            var offset = $("#" + ImageID).offset();
            var elewidth = $("#" + PnlPopupID).width();

            if ((offset.left + elewidth + 34) > winWidth) {
                left = (winWidth - elewidth - 35) + "px";
            }
            else {
                left = (offset.left) + "px";
            }
            top = (offset.top - $("#" + PnlPopupID).height() - 33) + "px";
            $("#" + PnlPopupID).css({
                position: 'absolute',
                zIndex: 100,
                left: left,
                top: top
            });
            $("#" + PnlPopupID).show();
        }
        else {
            $("#" + PnlPopupID).hide();

        }
    }

</script>
<asp:UpdatePanel ID="upMain" runat="server" UpdateMode="Conditional">
    <ContentTemplate>
        <asp:Panel runat="server" ID="pnlError">
            <table width="90%" cellspacing="6" cellpadding="0" border="0" align="center">
                <tbody>
                    <tr>
                        <td height="50">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" cellspacing="0" cellpadding="0" border="0">
                                <tbody>
                                    <tr>
                                        <td width="100">
                                        </td>
                                        <td height="100" class="content-text">
                                            <asp:Label ID="lblErrorMsg" runat="server"></asp:Label>
                                        </td>
                                    </tr>
                                </tbody>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td height="50">
                            &nbsp;
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="pnlClips" runat="server">
            <table width="99%" cellpadding="0" cellspacing="0" border="0" align="center">
                <tr>
                    <td align="center">
                        <asp:UpdatePanel ID="upClipPlayer" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <UCClipPlayer:ClipPlayer IsDefaultLoad="false" ID="ClipPlayerControl" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upClip" UpdateMode="Conditional" runat="server">
                            <ContentTemplate>
                                <table width="559px" cellpadding="0" cellspacing="0" border="0" align="center">
                                    <tr style="height: 20px;">
                                        <td>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trError">
                                        <td>
                                            <asp:Label ID="lblMsg" runat="server" ForeColor="Red"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="middle" align="left" style="vertical-align: middle;">
                                            Search :
                                            <asp:TextBox ID="txtSearch" runat="server" Width="315px" Height="22px"></asp:TextBox>
                                        </td>
                                        <td align="right">
                                            <asp:ImageButton ID="btnSearch" runat="server" AlternateText="Search" ImageUrl="~/Images/search.png"
                                                OnClick="btnSearch_Click" />&nbsp;
                                            <asp:ImageButton ID="btnReset" runat="server" AlternateText="Clear Search" ImageUrl="~/Images/clear-search.png"
                                                OnClick="btnReset_Click" />
                                        </td>
                                    </tr>
                                    <tr style="height: 10px;">
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                                <asp:DataList runat="server" ID="dlistClip" CellPadding="0" CellSpacing="0" BorderWidth="0"  RepeatDirection="Horizontal" RepeatLayout="Table"
                                    HorizontalAlign="Center" GridLines="None" OnItemDataBound="dlistClip_ItemDataBound"
                                    OnItemCommand="dlistClip_ItemCommand">
                                    <ItemTemplate>
                                        <table width="140px" align="center" cellpadding="0" cellspacing="0" border="0" >
                                            <tr>
                                                <td align="center">
                                                    <asp:Panel ID="pnlClip" runat="server">
                                                        <table cellpadding="0" cellspacing="0" border="0" style="word-break: break-all; word-wrap: break-word;
                                                            overflow: hidden; table-layout: fixed; width: 100px;">
                                                            <tr>
                                                                <td align="center">
                                                                    <div class="thumbwrap">
                                                                        <asp:LinkButton CommandName="play" CommandArgument='<%# Eval("ClipID")%>' runat="server"
                                                                            ID="lnkImage">
                                                                            <%--<asp:Image ID="thumbClip" Width="100px" Height="100px" runat="server" ImageUrl='<%# GetImage((byte[] )Eval("ClipThumbNailImage"),Eval("ClipID").ToString()) %>'
                                                                                AlternateText='<%# Eval("ClipTitle")%>' ImageAlign="Middle" />--%>
                                                                            <%--<asp:Image ID="thumbClip" Width="100px" Height="100px" runat="server" ImageUrl='<%# Eval("ThumbnailImagePath") %>'
                                                                                AlternateText='<%# Eval("ClipTitle")%>' ImageAlign="Middle" />--%>
                                                                                <asp:Image ID="thumbClip" Width="100px" Height="100px" runat="server" ImageUrl='<%# Convert.ToString(ConfigurationManager.AppSettings["ClipGetPreview"]) + "&amp;eid=" + Eval("ClipID") %>'
                                                                                AlternateText='<%# Eval("ClipTitle")%>' ImageAlign="Middle" />
                                                                         </asp:LinkButton>
                                                                        <%--<div class="download">
                                                                            <asp:ImageButton CommandName="Download" OnCommand="lnkDownload_Command" ImageUrl="~/Images/down_ms.png"
                                                                                CommandArgument='<%# Eval("ClipID") +"," + Eval("ClipTitle") %>' ID="lnkDownload"
                                                                                Width="22px" Height="24px" runat="server" OnClientClick="ClearMsg();" />
                                                                        </div>--%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td align="center">
                                                                    <asp:LinkButton CommandName="play" CommandArgument='<%# Eval("ClipID") %>' ID="lnkTitle"
                                                                        Text='<%# Eval("ClipTitle")%>' runat="server"></asp:LinkButton>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                    <%--<AjaxToolkit:HoverMenuExtender runat="server" ID="bpeClipDetail" TargetControlID="thumbClip"
                                                        PopupControlID="pnlClipDetail" PopupPosition="Top">
                                                        <Animations>                                                                                                                   
                                                            <OnShow>
                                                               <Sequence>                                                                                                                                     
                                                                     <HideAction Visible="true" />
                                                                    <Parallel>                                                                        
                                                                       <FadeIn duration=".2"></FadeIn>
                                                                    </Parallel>
                                                                </Sequence>
                                                            </OnShow>
                                                            <OnHide>
                                                                <Sequence>                                                                
                                                                    <Parallel>                                                                                                                                                                                  
                                                                        <FadeOut duration=".1"></FadeOut>
                                                                    </Parallel>
                                                                    <HideAction />
                                                                </Sequence>
                                                            </OnHide>
                                                        </Animations>
                                                    </AjaxToolkit:HoverMenuExtender>--%>
                                                    <asp:Panel CssClass="CssClipDetail" HorizontalAlign="Left" ID="pnlClipDetail" Style="max-width: 500px;
                                                        max-height: 400px; overflow: auto; display: none;"
                                                        runat="server">
                                                        <strong>Keywords:</strong><br />
                                                        <%--<div style="max-height: 100px;max-width:400px;overflow:auto;">--%>
                                                        <asp:Label ID="lblKeywords" Text='<%# Eval("Keywords")%>' runat="server"></asp:Label><br />
                                                        <%--</div>--%>
                                                        <br />
                                                        <strong>Description:</strong><br />
                                                        <%--<div style="clear: both; float: left; max-height: 300px;max-width:400px;overflow:auto;">--%>
                                                        <asp:Label ID="lblDescription" Text='<%# Eval("Description")%>' runat="server"></asp:Label><br />
                                                        <%--</div>--%>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <table width="560px" align="center">
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </FooterTemplate>
                                    <ItemStyle Width="140px" VerticalAlign="Top" />
                                </asp:DataList>
                                <table align="center" id="btmnav">
                                    <tr>
                                        <td valign="middle" align="left">
                                            <asp:ImageButton ID="btnPrevious" Style="padding-left: 15px;" runat="server" AlternateText="Previous"
                                                ImageUrl="~/Images/prev.png" OnClick="btnPrevious_Click" />
                                        </td>
                                        <td valign="middle" align="right">
                                            <asp:ImageButton ID="btnNext" runat="server" Style="padding-right: 15px;" AlternateText="Next"
                                                ImageUrl="~/Images/next.png" OnClick="btnNext_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </ContentTemplate>
</asp:UpdatePanel>
<asp:UpdateProgress ID="updClip" runat="server" AssociatedUpdatePanelID="upClip">
    <ProgressTemplate>
        <div style="width: 70px;">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upoeClip" ControlToOverlayID="upClip"
    TargetControlID="updClip" CssClass="updateProgress" />
<asp:UpdateProgress ID="updClipPlayer" runat="server" AssociatedUpdatePanelID="upClipPlayer">
    <ProgressTemplate>
        <div style="width: 70px;">
            <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                alt="Loading..." id="imgLoading" />
        </div>
    </ProgressTemplate>
</asp:UpdateProgress>
<cc2:UpdateProgressOverlayExtender runat="server" ID="upoeClipPlayer" ControlToOverlayID="upClipPlayer"
    TargetControlID="updClipPlayer" CssClass="updateProgress" />
