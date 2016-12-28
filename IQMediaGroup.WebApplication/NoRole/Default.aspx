<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.NoRole.Default" %>

<%@ Register src="../UserControl/IQMediaMaster/NoRoles/NoRole.ascx" tagname="NoRole" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iqMedia :: NoRole</title>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/AC_RunActiveContent.js" type="text/javascript"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:NoRole ID="NoRoleControl" runat="server" />
    </div>
    </form>
</body>
</html>
