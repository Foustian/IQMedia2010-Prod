<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.CustomError.Default" %>

<%@ Register src="../UserControl/IQMediaMaster/CustomError/CustomError.ascx" tagname="CustomError" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iQMedia :: CustomError</title>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />   

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc1:CustomError ID="CustomError1" runat="server" />
    </div>
    </form>
</body>
</html>
