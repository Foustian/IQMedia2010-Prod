<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.Upload.Default" %>

<%@ Register Assembly="FileUploadLibrary" Namespace="darrenjohnstone.net.FileUpload"
    TagPrefix="cc1" %>
<%@ Register Src="../UserControl/IQMediaMaster/Upload/Upload.ascx" TagName="Upload"
    TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        BODY{margin:0; padding:0;}
    </style>
</head>
<body>
    <form id="form" runat="server" enctype="multipart/form-data">
    <asp:ScriptManager ID="scriptManager" runat="server" />
    <script type="text/javascript">
        function pageLoad(sender, args) {
            //  register the form and upload elements
            //alert('<%=this.form.ClientID %>----' + '<%=this.upload.UploadKey %>');
            window.parent.register(
                $get('<%=this.form.ClientID %>'),
                $get('<%=this.fileUpload.ClientID %>'),
                '<%=this.upload.UploadKey %>'
            );
        }
    </script>
    <div>
        <cc1:UploadController ID="upload" runat="server" />
        <input id="fileUpload" runat="server" type="file" style="width: 100%" />
    </div>
    </form>
</body>
</html>
