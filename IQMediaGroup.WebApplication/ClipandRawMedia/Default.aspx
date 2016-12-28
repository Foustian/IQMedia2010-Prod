<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" ValidateRequest="false"
    Inherits="IQMediaGroup.WebApplication.ClipandRawMedia.Default" Title="iQMedia :: iQ Basic"
    MasterPageFile="~/IQMediaGroupResponsive.Master" %>

<%@ Register Src="../UserControl/IQMediaMaster/ClipandRawMediaControl/ClipandRawMedia.ascx"
    TagName="UCClipandRawMedia" TagPrefix="UC" %>
<asp:Content ContentPlaceHolderID="head" runat="server">

    <link href="../Css/tutorsty.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function showdivPlayer() {

            //document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_tblPlayer').style.display = "block";
            $("#divShowHide").show('slow');
            //alert(location.hostname);           

        }
        function hidedivPlayer() {
            $("#divShowHide").hide('slow');
            //(document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_Player')).setAttribute('enable', 'true');                       

        }

        function showCaption() {

            document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_tblClosedCaption').style.display = "block";
            $("#ctl00_Content_Data_UCClipandRawMediaControl_ClosedCaption").show('slow');
        }
        function hideCaption() {
            $("#ctl00_Content_Data_UCClipandRawMediaControl_ClosedCaption").hide('slow');
            (document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_ClosedCaption')).setAttribute('enable', 'true');
        }
    </script>
    <script type="text/javascript">

        function ResizeIframe(height) {

            var _JClipframe = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_Clipframe');

            if (_JClipframe) {
                _JClipframe.style.height = height + "px";
            }
        }      
    
    </script>
    <script type="text/javascript">

        function showDivBox() {
            document.getElementById("divVideo").style.display = "block";
        }

        function showtrace(Msg) {
            alert("hello");
        }
 
    </script>
</asp:Content>
<asp:Content ID="Content" ContentPlaceHolderID="Content_Data" runat="Server">
    <script type="text/javascript">


        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest);

        var IsTimeOut = 0;
        var PostBackElement;
        var IsSearch;

        var instance = Sys.WebForms.PageRequestManager.getInstance();
        function pageLoad(sender, args) {
            if (!args.get_isPartialLoad()) {
                $addHandler(document, "keydown", onKeyDown);
            }
        }
        function onKeyDown(e) {
            if (e && e.keyCode == Sys.UI.Key.esc) {
                if (instance.get_isInAsyncPostBack()) {
                    instance.abortPostBack();
                }
            }
        }

        function endRequest(sender, args) {
           
            if (args.get_error() != undefined) {

                if (PostBackElement && (PostBackElement.id.toString().match('btnSearchRawMedia') != null || PostBackElement.id.toString().match('btnSearchClip') != null)) {
                    IsSearch = true;

                    vLblTimeOut = $get('ctl00_Content_Data_UCClipandRawMediaControl_lblTimeOutMsg');

                    var vDivRawMedia = $get('divTimeOut');
                    if (vLblTimeOut != null && vDivRawMedia != null) {
                        vLblTimeOut.innerHTML = "The server timed out before receiving a response. Please retry your search.";

                        vLblTimeOut.style.display = "block";
                        vDivRawMedia.style.display = "none";

                        IsTimeOut = 1;
                    }
                }
                else if (PostBackElement && PostBackElement.id.toString().match('lbtnPlay') != null) {
                    IsSearch = false;
                    vLblTimeOut = $get('ctl00_Content_Data_UCClipandRawMediaControl_lblTimeOutPlayer');
                    if (vLblTimeOut != null) {
                        vLblTimeOut.innerHTML = "The server timed out before receiving a response. Please retry to play RawMedia.";
                        vLblTimeOut.style.display = "block";
                    }
                }

                args.set_errorHandled(true);
            }

        }

        function setOffset() {

            var offset = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_hfOffsetValue');
            if (offset) {

                //alert(offset.value);
                var count = Math.floor(offset.value / 60);
                //alert(count);
                if (count >= 0 && count <= 10) {

                    var img0 = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_time0');
                    if (img0) {
                        //alert('img0');
                        setSeekPoint(offset.value, img0);
                    }

                }
                if (count >= 11 && count <= 20) {

                    var img1 = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_time10');
                    if (img1) {
                        //alert('img1');
                        setSeekPoint(offset.value, img1);
                    }
                }
                if (count >= 21 && count <= 30) {

                    var img2 = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_time20');
                    if (img2) {
                        //alert('img2');
                        setSeekPoint(offset.value, img2);
                    }
                }
                if (count >= 31 && count <= 40) {

                    var img3 = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_time30');
                    if (img3) {
                        //alert('img3');
                        setSeekPoint(offset.value, img3);
                    }
                }
                if (count >= 41 && count <= 50) {

                    var img4 = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_time40');
                    if (img4) {
                        //alert('img4');
                        setSeekPoint(offset.value, img4);
                    }
                }
                if (count >= 51) {

                    var img5 = document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_time50');
                    if (img5) {
                        //alert('img5');
                        setSeekPoint(offset.value, img5);
                    }
                }
                offset.value = '';
            }
        }

        function beginRequest(sender, args) {
            //alert($('#upProgressMaster'));
            
            vLblTimeOut = $get('ctl00_Content_Data_UCClipandRawMediaControl_lblTimeOutMsg');
            vLblTimeOutPlayer = $get('ctl00_Content_Data_UCClipandRawMediaControl_lblTimeOutPlayer');

            if (vLblTimeOutPlayer != null) {
                vLblTimeOutPlayer.style.display = "none";
                vLblTimeOutPlayer.innerHTML = "";
            }

            var vDivRawMedia = $get('divTimeOut');

            if (vDivRawMedia != null && vLblTimeOut != null) {

                vLblTimeOut.style.display = "none";

                if (IsTimeOut == 1) {
                    vDivRawMedia.innerHTML = "";
                }

                vDivRawMedia.style.display = "block";
            }

            var elem = args.get_postBackElement();

            if (elem) {

                PostBackElement = elem;
            }
            else {
                PostBackElement = null;
            }
        }

    </script>
    <script language="javascript" type="text/javascript">
        function showtrace(tracemessage) {
            document.getElementById('<%=lblTrace.ClientID%>').innerHTML += tracemessage + "<br />";

            alert(tracemessage);
        }
    </script>
    <table cellpadding="0" cellspacing="0" width="100%" align="center">
        <tr>
            <td>
                <asp:Label ID="lblTrace" runat="server"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <UC:UCClipandRawMedia ID="UCClipandRawMediaControl" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
