<%@ Page Title="iQMedia :: iQ Advanced" Language="C#" MasterPageFile="~/IQMediaGroupResponsive.Master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IQAdvance.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/IQAdvance/IQAdvance.ascx" TagName="IQAdvance"
    TagPrefix="UC" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
<script type="text/javascript">
    function CheckUnCheckAll(isCheck, rptChecklist) {
        if(isCheck)
        {

            $("#" + rptChecklist + " input:checkbox").each(function (index) {
                $(this).attr("disabled", "disabled");
                $(this).attr("checked", "checked");
            });
         }
         else
         {
             $("#" + rptChecklist + " input:checkbox").each(function (index) {
                 $(this).removeAttr("disabled");
                 $(this).removeAttr("checked");
             });
         }
    }
    
    
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">    

    <script language="javascript" type="text/javascript">

        function hidediv() {
            document.getElementById('ctl00_Content_Data_IQAdvancePanel_tblSearch').opacity = '';
            $("#ctl00_Content_Data_IQAdvancePanel_tblSearch").removeClass("show-bg");
            $("#ctl00_Content_Data_IQAdvancePanel_jason").hide('slow');
        }

        function showdiv() {
            document.getElementById('ctl00_Content_Data_IQAdvancePanel_tblSearch').style.display = "block";
            $("#ctl00_Content_Data_IQAdvancePanel_tblSearch").addClass("show-bg");
            $("#ctl00_Content_Data_IQAdvancePanel_jason").show('slow');
        }      

        function showCaption() {

            document.getElementById('ctl00_Content_Data_IQAdvancePanel_tblClosedCaption').style.display = "block";
            $("#ctl00_Content_Data_IQAdvancePanel_ClosedCaption").show('slow');
        }
        function hideCaption() {
            $("#ctl00_Content_Data_IQAdvancePanel_ClosedCaption").hide('slow');
            (document.getElementById('ctl00_Content_Data_IQAdvancePanel_ClosedCaption')).setAttribute('enable', 'true');
        }

        function SearchButtonClick() {
            document.getElementById('btnShowSearch').style.display = "none";
            showdiv();

        }
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(beginRequest_Handler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(endRequest_Handler);

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
        
        function endRequest_Handler(sender, args) {
            
            if (args.get_error() != undefined) {
                if (PostBackElement && PostBackElement.id.toString().match('btnSubmit') != null) 
                {
                    IsSearch = true;
                
                    vLblTimeOut = $get('ctl00_Content_Data_IQAdvancePanel_lblTimeOutMsg');

                    var vDivRawMedia = $get('divTimeOut');
                    if (vLblTimeOut != null && vDivRawMedia != null) 
                    {
                        vLblTimeOut.innerHTML = "The server timed out before receiving a response. Please retry your search.";
                        vLblTimeOut.style.display = "block";
                        vDivRawMedia.style.display = "none";
                        IsTimeOut = 1;
                    }
                }
            else if (PostBackElement && PostBackElement.id.toString().match('lbtnPlay') != null)
            {
                IsSearch = false;
                    vLblTimeOut = $get('ctl00_Content_Data_IQAdvancePanel_lblTimeOutPlayer');
                    if (vLblTimeOut != null) {
                        vLblTimeOut.innerHTML = "The server timed out before receiving a response. Please retry to play RawMedia.";
                        vLblTimeOut.style.display = "block";
                    }
            }       

                args.set_errorHandled(true);
            }
            
            if(PostBackElement)
            {
                if (PostBackElement.id.toString().match('lbtnPlay') != null) {                    
                    
                    var B = 'img#ctl00_Content_Data_IQAdvancePanel_time0.hourSeek';
                    $(B).addClass("selected"); $("#timeBar").children(":not(.selected)").fadeTo(400, 0.4); $(B).removeClass("selected");$(B).fadeTo(400, 1);
                    //setTimeout("setOffset()", 0);
                    //setOffset();
                }
            }

            //scrollTo(0, 0);

        }

        function setOffset() {

            var offset = document.getElementById('ctl00_Content_Data_IQAdvancePanel_hfOffsetValue');
            if (offset) {

                //alert(offset.value);
                var count = Math.floor(offset.value / 60);
                //alert(count);
                if (count >= 0 && count <= 10) {

                    var img0 = document.getElementById('ctl00_Content_Data_IQAdvancePanel_time0');
                    if (img0) {
                        //alert('img0');
                        setSeekPoint(offset.value, img0);
                    }

                }
                if (count >= 11 && count <= 20) {

                    var img1 = document.getElementById('ctl00_Content_Data_IQAdvancePanel_time10');
                    if (img1) {
                        //alert('img1');
                        setSeekPoint(offset.value, img1);
                    }
                }
                if (count >= 21 && count <= 30) {

                    var img2 = document.getElementById('ctl00_Content_Data_IQAdvancePanel_time20');
                    if (img2) {
                        //alert('img2');
                        setSeekPoint(offset.value, img2);
                    }
                }
                if (count >= 31 && count <= 40) {

                    var img3 = document.getElementById('ctl00_Content_Data_IQAdvancePanel_time30');
                    if (img3) {
                        //alert('img3');
                        setSeekPoint(offset.value, img3);
                    }
                }
                if (count >= 41 && count <= 50) {

                    var img4 = document.getElementById('ctl00_Content_Data_IQAdvancePanel_time40');
                    if (img4) {
                        //alert('img4');
                        setSeekPoint(offset.value, img4);
                    }
                }
                if (count >= 51) {

                    var img5 = document.getElementById('ctl00_Content_Data_IQAdvancePanel_time50');
                    if (img5) {
                        //alert('img5');
                        setSeekPoint(offset.value, img5);
                    }
                }
                offset.value = '';
            }
        }

        function beginRequest_Handler(sender, args) {
            
            vLblTimeOut = $get('ctl00_Content_Data_IQAdvancePanel_lblTimeOutMsg');
            vLblTimeOutPlayer = $get('ctl00_Content_Data_IQAdvancePanel_lblTimeOutPlayer');
            
            if(vLblTimeOutPlayer!=null)
            {
                vLblTimeOutPlayer.style.display = "none";
                vLblTimeOutPlayer.innerHTML = "";
            }
            var vDivRawMedia = $get('divTimeOut');

            if (vDivRawMedia != null && vLblTimeOut != null) 
            {
                vLblTimeOut.style.display = "none";

                if (IsTimeOut == 1 && IsSearch == true) 
                {
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
    
    <script type="text/javascript">

        function ResizeIframe(height) {

            var _JClipframe = document.getElementById('ctl00_Content_Data_IQAdvancePanel_ClipFrame');

            if (_JClipframe) {
                _JClipframe.style.height = height+"px";
            }
        }       
    
    </script>

    <link href="../Css/tutorsty.css" rel="stylesheet" type="text/css" />
    <link href="../Css/flexcrollstyles.css" rel="stylesheet" type="text/css" />
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />

    <table cellpadding="0" cellspacing="0" align="center">
        <tr>
            <td>
                <UC:IQAdvance ID="IQAdvancePanel" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
