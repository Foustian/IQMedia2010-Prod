<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IframeRawMedia.Default" %>

<%@ Register src="../UserControl/IQMediaMaster/IFrameRawMedia/IFrameRawMedia.ascx" tagname="IFrameRawMedia" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
   <!-- <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Css/tutorsty.css" rel="stylesheet" type="text/css" />-->
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">        

        function LoadPlayer() {           
            var B = 'img#IFrameRawMedia1_time0.hourSeek';
            $(B).addClass("selected"); $("#timeBar").children(":not(.selected)").fadeTo(400, 0.4); $(B).removeClass("selected"); $(B).fadeTo(400, 1);
            showRawMediaPlayer(false);
        }       

        function showRawMediaPlayer(isReload) {

            //document.getElementById('ctl00_Content_Data_UCClipandRawMediaControl_tblPlayer').style.display = "block";           

            var _JPlayer = 'div#IFrameRawMedia1_Player'
            var _JDivRawMedia = document.getElementById("IFrameRawMedia1_divRawMedia");
            var _JHF = document.getElementById("IFrameRawMedia1_hfDivRawMedia");

            if (_JHF != null && isReload==true && _JDivRawMedia!=null) {
                _JDivRawMedia.innerHTML = _JHF.value;
            }

            $(_JPlayer).show('slow');

            parent.window.ResizeIframe(730);       

        }
        function hideRawMediaPlayer() {
        
            var _JPlayer = 'div#IFrameRawMedia1_Player'
            $(_JPlayer).hide('slow');
            var _JDivRawMedia = 'div#IFrameRawMedia1_divRawMedia';
            $(_JDivRawMedia).html(null);

            parent.window.ResizeIframe(35);       
        }

        function showCaption() {

             document.getElementById('IFrameRawMedia1_tblClosedCaption').style.display = "block";
             $("#IFrameRawMedia1_ClosedCaption").show('slow');

             parent.window.ResizeIframe(730);
        }
        function hideCaption() {
            $("#IFrameRawMedia1_ClosedCaption").hide('slow');
            (document.getElementById('IFrameRawMedia1_ClosedCaption')).setAttribute('enable', 'true');

            parent.window.ResizeIframe(475);
        }
    
    </script>
    
</head>
<body class="popup">
    <form id="form1" runat="server">
    <div>    
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <uc1:IFrameRawMedia ID="IFrameRawMedia1" runat="server" />    
    </div>
    </form>
</body>
</html>
