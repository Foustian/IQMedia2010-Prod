<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Upload.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.Upload.Upload" %>
<%@ Register assembly="FileUploadLibrary" namespace="darrenjohnstone.net.FileUpload" tagprefix="cc1" %>
<div>
    <cc1:uploadcontroller id="upload" runat="server" />
    <input id="fileUpload" runat="server" type="file" class="textbox03" style="width: 400px;display:none" />
</div>
