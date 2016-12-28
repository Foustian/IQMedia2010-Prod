<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.RawMediaPlayer.Default" %>

<%@ Register src="../UserControl/IQMediaMaster/RawMediaPlayer/RawMediaPlayer.ascx" tagname="UCRawMediaPlayer" tagprefix="UC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
   
<script type="text/javascript" src="../Script/flexcroll.js"></script>
 <link href="../Css/tutorsty.css" rel="stylesheet" type="text/css" />
 
<link href="../Css/flexcrollstyles.css" rel="stylesheet" type="text/css" />

<link href="../Css/style.css" rel="stylesheet" type="text/css" />

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="RawMedia" runat="server">
    </asp:ScriptManager>
    <table cellpadding="0" cellspacing="0" border="0">
            <tr>
                <td>
                    
                    <UC:UCRawMediaPlayer ID="UCRawMediaPlayerControl" runat="server" />
                    
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
