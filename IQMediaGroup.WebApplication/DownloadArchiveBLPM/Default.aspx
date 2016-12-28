<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.DownloadArchiveBLPM.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/DownloadArchiveBLPM/DownloadArchiveBLPM.ascx"
    TagName="DownloadArchiveBLPM" TagPrefix="uc" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .ClipDownloadTbl
        {
            border: 1px solid #000000;
        }
    </style>
    <title>Print Media Download</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <uc:DownloadArchiveBLPM ID="ucDownloadArchiveBLPM" runat="server" />
    </div>
    </form>
</body>
</html>
