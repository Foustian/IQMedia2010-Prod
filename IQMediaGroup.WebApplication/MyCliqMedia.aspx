<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyCliqMedia.aspx.cs" Inherits="IQMediaGroup.WebApplication.MyCliqMedia" %>

<%@ Register Src="~/UserControl/IQMediaMaster/MyCliqMediaLogin/MyCliqMediaLogin.ascx"
    TagName="UCMyCliqMediaLogin" TagPrefix="uc" %>
<%@ Register Src="~/UserControl/IQMediaMaster/Logout/Logout.ascx" TagName="Logout"
    TagPrefix="uc2" %>
<%--<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">--%>
<!DOCTYPE HTML>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cliq - The easiest, fastest, most powerful way to monitor broadcast media
    </title>
    <link rel="Stylesheet" href="MyCliqMedia/css/style.css" />
    <script src="MyCliqMedia/js/html5.js" type="text/javascript"></script>
    <script src="MyCliqMedia/js/jquery.min.js" type="text/javascript"></script>
    <script src="MyCliqMedia/js/functions.js" type="text/javascript"></script>
    <style type="text/css">
        html, body
        {
            width: 100%;
            height: 100%;
            overflow: hidden;
            background: #16130c;
        }
        #bg
        {
            position: absolute;
            width: 100%;
            z-index: 0;
        }
        #bg img
        {
            width: 100%;
        }
        #wrapper
        {
            position: relative;
            z-index: 10;
            height: 100%;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <img src="MyCliqMedia/images/concept-bg.jpg" alt="Cliq - The easiest, fastest, most powerful way to monitor broadcast media"
        id="bg">
    <div id="wrapper">
        <div id="topstrip">
            <div class="float-right">
                <a href="#" title="Facebook">
                    <img src="MyCliqMedia/images/social-icon-facebook.png" alt="Facebook"></a> <a href="#"
                        title="Twitter">
                        <img src="MyCliqMedia/images/social-icon-twitter.png" alt="Twitter"></a>
                <a href="#" title="LinkedIn">
                    <img src="MyCliqMedia/images/social-icon-linkedin.png" alt="LinkedIn"></a>
            </div>
        </div>
        <header>     
          <div id="punchline">You can monitor round the clock, or you can just</div>
          <div id="logo"><img src="MyCliqMedia/images/logo-cliq.png" alt="Cliq"></div>
     </header>
        <div class="centerbox">
            <div id="infobubble">
                The easiest,<br>
                fastest, most<br>
                powerful way<br>
                to monitor<br>
                broadcast media
            </div>
            <div id="login">
                <%--<h2 class="loginhd center">Login</h2>--%>
                <div id="loginform">
                    <uc:UCMyCliqMediaLogin ID="UCMyCliqMediaLogin" runat="server" />
                </div>
                <div id="logoutform" style="margin-top:50px;">
                    <uc2:Logout ID="ucLogout" runat="server" />
                </div>
            </div>
        </div>
        <footer>
    		<div class="poweredby"><img src="MyCliqMedia/images/powered-by.png" alt="Cliq"></div>
          <div class="copyright">&copy; 2012, iQ media Corporation<a href="http://www.iqmediacorpt.com">iqmediacorp.com</a></div>
     </footer>
    </div>
    <%-- <img src="MyCliqMedia/images/concept-bg1.jpg" alt="Cliq - The easiest, fastest, most powerful way to monitor broadcast media"
        id="bg">
    <div id="wrapper">
        <div id="topstrip">
            <div class="float-right">
                <a href="#" title="Facebook">
                    <img src="MyCliqMedia/images/social-icon-facebook.png" alt="Facebook"></a> <a href="#"
                        title="Twitter">
                        <img src="MyCliqMedia/images/social-icon-twitter.png" alt="Twitter"></a>
                <a href="#" title="LinkedIn">
                    <img src="MyCliqMedia/images/social-icon-linkedin.png" alt="LinkedIn"></a>
            </div>
        </div>
        <div id="punchline">
            You can monitor round the clock, or you can just</div>
        <div id="logo">
            <img src="MyCliqMedia/images/logo-cliq.png" alt="Cliq"></div>
        <div class="centerbox">
            <div id="infobubble">
                The easiest,<br />
                fastest, most<br />
                powerful way<br />
                to monitor<br />
                broadcast media
            </div>
            <div id="login">
                <h2 class="loginhd center">
                    Login</h2>
                <div id="loginform" width="100%">
                    <uc:UCMyCliqMediaLogin ID="UCMyCliqMediaLogin" runat="server" />
                </div>
            </div>
        </div>
        <div class="poweredby">
            <img src="MyCliqMedia/images/powered-by.png" alt="Cliq"></div>
        <div class="copyright">
            &copy; 2012, iQ media Corporation<a href="http://www.iqmediacorpt.com">iqmediacorp.com</a></div>
    </div>--%>
    </form>
</body>
</html>
