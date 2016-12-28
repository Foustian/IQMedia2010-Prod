<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MyIQ.aspx.cs" Inherits="IQMediaGroup.WebApplication.Report.MyIQ" %>
<%@ Register Src="~/UserControl/IQMediaMaster/MyIQReport/MyIQReport.ascx" TagName="UCMyIQReport"
    TagPrefix="uc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width" />
    <script src="/Script/jquery-1.8.3.min.js" type="text/javascript"></script>
    <%--<link href="/Css/style.css" rel="stylesheet" type="text/css" />--%>
    <link href="/Css/bootstrap.css" rel="stylesheet" type="text/css" />
    <link href="/Css/fonts/stylesheet.css" rel="stylesheet" type="text/css" />
    <link href="/Css/bootstrap-responsive.css" rel="stylesheet" type="text/css" />
    <link href="/Css/my-style.css" rel="stylesheet" type="text/css" />
    <script src="/Script/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
    <script src="/Script/BootStrapjs/bootstrap-modal.js" type="text/javascript"></script>
    <script type="text/javascript" src="/Script/jquery.blockUI.js"></script>
    <script type="text/javascript" src="/Script/Report.js"></script>
    <style type="text/css">
        body
        {
            padding-bottom: 40px;
            background:none;
            padding-top:0px\9;
        }
        .sidebar-nav
        {
            padding: 9px 0;
        }
    </style>
    <!-- Le HTML5 shim, for IE6-8 support of HTML5 elements -->
    <!--[if lt IE 9]>
      <script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
    <![endif]-->
    <!--[if IE 8]>
	<script src="http://html5shim.googlecode.com/svn/trunk/html5.js"></script>
<style>
.ui-resizable {height:70%;}

#main-nav {
width:100%;
text-align:center;
}
#main-nav li {
max-width:16.66% !important;
min-width:14.25% !important;
}
#main-nav li.firstli {
width:18% !important;
}
#main-nav li.lastli {
width:18% !important;
}
/* Custom */
.row-fluid .left 
{
width: 80%;
}

.lidivTop{
clear:both;
overflow:hidden;
}
.lidivLeft{
float:left

}
.lidivLeft img{
vertical-align:middle;
float:left;
max-width:none;
}
.right-dropdown{
float:right
}
.right-dropdown img{
vertical-align:middle;
float:right;
max-width:none;
padding-top:4px;
padding-left:4px;
}
.row-fluid [class*="span"]{
min-height: 20px;
} 
.imgshowHide{
    margin-top:-1px;
    margin-left:1px;
}
.show-hide img
{
   margin-top:-3px;
    margin-left:1px;
}
</style>

<![endif]-->
    <!--[if IE 7]>
	<style>
     #main-nav {
 width:100%;
 text-align:center;
 float:left;
 clear:both;
 overflow:hidden;
 
}
 #main-nav li {
 width:17%;
}
 #main-nav li.firstli {
 width:16%;
}
 #main-nav li.lastli {
 width:16%;
}
    </style>
<![endif]-->
</head>
<body  >
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM1" runat="server">
    </asp:ScriptManager>
    
    <uc2:UCMyIQReport ID="UCMyIQReport1" runat="server"></uc2:UCMyIQReport>
    </form>
    <script src="/Script/html5.js" type="text/javascript"></script>
</body>
<script type="text/javascript" language="javascript">
    var UpdPanelsIds = null;
    Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);


    function BeginRequestHandler(sender, args) {
        //  alert('hi');
        //        var updpnl = sender._postBackSettings.panelID;
        //        alert(updpnl);

        UpdPanelsIds = args.get_updatePanelsToUpdate();
        UpdPanelsIds = UpdPanelsIds.toString().split('$').join('_');


        //$('#' + UpdPanelsIds + '').block({ message: $('#divBlock') });
        $('#' + UpdPanelsIds + '').block({ message: '<div style="height:70px;width:70px;" id="divBlock"><img alt="Loading.." src="../Images/Loading_Trans.gif" id="image1" /></div>' });


    }

    function EndRequestHandler(sender, args) {
        $('#' + UpdPanelsIds + '').unblock();
    }    
</script>
</html>