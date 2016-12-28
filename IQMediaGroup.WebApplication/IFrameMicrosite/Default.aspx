<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.IFrameMicrosite.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/IFrameMicrosite/IFrameMicrosite.ascx"
    TagName="IFrameMicrosite" TagPrefix="UCIFrameMicrosite" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>iQMedia :: MicroSite</title>
    <script src="../Script/IQMediaraw.js?v=4624" type="text/javascript"></script>
    <script src="../Script/jquery-1.3.2.min.js?v=4272" type="text/javascript"></script>
      
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table width="100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <UCIFrameMicrosite:IFrameMicrosite ID="IFrameMicrosite1" runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
