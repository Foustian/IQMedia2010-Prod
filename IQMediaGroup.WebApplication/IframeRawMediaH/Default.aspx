<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IframeRawMediaH.Default" %>

<%@ Register Src="~/UserControl/IQMediaMaster/IframeRawMediaH/IframeRawMediaH.ascx"
    TagName="IframeRawMediaH" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <!-- <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Css/tutorsty.css" rel="stylesheet" type="text/css" />-->
    <script src="../Script/jquery-1.8.1.min.js" type="text/javascript"></script>
    <%--<link href="../Css/style.css" rel="stylesheet" type="text/css" />--%>
    <link href="../Css/my-style.css" rel="stylesheet" type="text/css" />    
    <script type="text/javascript" language="javascript">
        $(document).ready(function () {                      
            $('#divShowCaption').click(function () {
                if ($('#IFrameRawMedia1_DivCaption').is(':visible')) {
                    // $('#IFrameRawMedia1_DivCaption').hide(1000, ResizeIframe('600px'));
                    $('#IFrameRawMedia1_DivCaption').hide(500);

                    $('#divIFrameMain').animate({

                        width: '570px',
                        marginLeft: '30%'


                    }, 1000, function () {
                        $('#imgCCDirection').attr('src', '../../images/left_arrow_cc.gif');
                    });

                }
                else {
                    // $('#IFrameRawMedia1_DivCaption').show(1000, ResizeIframe('700px'));
                    $('#divIFrameMain').animate({
                        width: '870px',
                        marginLeft: '20%'

                    }, 500, function () {
                        $('#imgCCDirection').attr('src', '../../images/right_arrow_cc.gif');
                    });
                    $('#IFrameRawMedia1_DivCaption').show(800);



                }
            });
        });
        function ResizeIframe(fixWidth) {

            // alert(document.getElementById("diviframe").contentWindow.document.body.scrollWidth);
            $('#divIFrameMain').css('width', fixWidth); // document.getElementById("ifrmae").contentWindow.document.body.scrollWidth + "px");            
        }
    </script>
</head>
<body class="popup">
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <uc:IframeRawMediaH ID="IFrameRawMedia1" runat="server" />
    </div>
    </form>
</body>
<script type="text/javascript" language="javascript">
    //  ResizeIframe('600px');
</script>
</html>
