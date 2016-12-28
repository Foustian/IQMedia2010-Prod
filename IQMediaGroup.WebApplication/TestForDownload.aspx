<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TestForDownload.aspx.cs" Inherits="IQMediaGroup.WebApplication.TestForDownload" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    <asp:TextBox ID="txtR" runat="server"></asp:TextBox>
    <asp:Button ID="btnSubmit" runat="server" Text="Get File Info" OnClick="btnSubmit_Click" />
    </div>
    </form>
</body>
</html>
