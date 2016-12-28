<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.DownloadArchiveSM.Default" %>

<%@ Register src="../UserControl/IQMediaMaster/DownloadArchiveSM/DownloadArchiveSM.ascx" tagname="DownloadArchiveSM" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
 <style type="text/css">
        .ClipDownloadTbl{border:1px solid #000000;}
    </style>
<title>Social Media Article Download</title>    
</head>
<body style ="overflow:hidden" >
    <form id="form1" runat="server">
    <div>
        <uc1:DownloadArchiveSM ID="DownloadArchiveSM1" runat="server" />
    </div>
    </form>
</body>
</html>