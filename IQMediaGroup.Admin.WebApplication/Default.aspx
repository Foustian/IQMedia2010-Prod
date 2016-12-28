<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.Admin.WebApplication._Default" %>
<%@ Register src="~/UserControl/Login/Login.ascx" tagname="Login" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>
        iQMedia :: iQ AdminLogin
    </title>
    <link href="~/css/Adminstyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <table id="Table1" width="100%" runat="server" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <uc1:Login ID="Login1" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
