<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="logtext.aspx.cs" Inherits="IQMediaGroup.WebApplication.logtext" %>

<%@ Register src="UserControl/IQMediaMaster/Login/Login.ascx" tagname="Login" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Script/jquery-1.8.1.min.js" type="text/javascript"></script>
    <script src="Script/jquery.blockUI.js" type="text/javascript"></script>
    <script type="text/javascript">
        var UpdPanelsIds = null;
        Sys.WebForms.PageRequestManager.getInstance().add_beginRequest(BeginRequestHandler);
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);


        function BeginRequestHandler(sender, args) {
            //  alert('hi');
            //        var updpnl = sender._postBackSettings.panelID;
            //        alert(updpnl);

            UpdPanelsIds = args.get_updatePanelsToUpdate();
            UpdPanelsIds = UpdPanelsIds.toString().split('$').join('_');


            $('#' + UpdPanelsIds + '').block({ message: $('#divBlock') });

        }

        function EndRequestHandler(sender, args) {
            $('#' + UpdPanelsIds + '').unblock();
        }


    
</script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:ScriptManager ID="spt1" runat="server"></asp:ScriptManager>
        <uc1:Login ID="Login1" runat="server" />
    
    </div>
    </form>
    
</body>

</html>
