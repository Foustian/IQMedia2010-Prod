<%@ Page Title="iQMedia :: iQ Custom" Language="C#" MasterPageFile="~/IQMediaGroupResponsive.Master"
    AutoEventWireup="true" CodeBehind="Default.aspx.cs" ValidateRequest="false" Inherits="IQMediaGroup.WebApplication.IQCustom.Default" %>

<%@ Register Src="../UserControl/IQMediaMaster/IQCustom/IQCustom.ascx" TagName="IQCustom"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Content_Data" runat="server">
    <%--<div class="commands">--%>
    <uc1:IQCustom ID="IQCustom1" runat="server" />
    <%-- </div>--%>
    <script type="text/javascript">
        function ResizeIframe(height) {

            var _JClipframe = document.getElementById('ctl00_Content_Data_IQCustom1_Clipframe');
            if (_JClipframe) {
                if (height > 450) {
                    _JClipframe.style.height = 450 + "px";
                }
                else {
                    _JClipframe.style.height = height + "px";
                }
            }
        }      
    </script>
    <script src="../Script/jquery-ui-1.9.2.custom.min.js" type="text/javascript"></script>
    <script src="../Script/IQCustomScript.js" type="text/javascript"></script>
    <link href="../Css/style.css" rel="stylesheet" type="text/css" />
    <link href="../Css/fileuploadcontrol_default.css" rel="stylesheet" type="text/css" />
    <link href="../Css/fileuploadcontrol_uploadify.css" rel="stylesheet" type="text/css" />
    <%--<script type="text/javascript" src="../Script/jquery-1.4.2.min.js"></script>--%>
    <script type="text/javascript" src="../Script/swfobject.js"></script>
    <script type="text/javascript" src="../Script/jquery.uploadify.v2.1.4.min.js"></script>
    <script src="../Script/CommonIQ.js" type="text/javascript"></script>
    <%--<script src="../Script/CommonIQ.js" type="text/javascript"></script>--%>

    <script type="text/javascript">



        function ClearAll() {
            //$find('mpeUploadMedia').hide();
            CloseiqCustomModal('pnlUploadMedia');
            $get("ctl00_Content_Data_IQCustom1_tblUpload").style.display = "block";
            document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "block";
            document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "block";
            $get("ctl00_Content_Data_IQCustom1_tblUploading").style.display = "none";
            document.getElementById('ctl00_Content_Data_IQCustom1_vsUploadMedia').style.display = 'none';
            document.getElementById('ctl00_Content_Data_IQCustom1_txtDescription').value = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_txtKeywords').value = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_txtMediaTitle').value = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_lblMsg').innerHTML = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = "";
            document.getElementById('ctl00_Content_Data_IQCustom1_ddlCategory').selectedIndex = 0;
            document.getElementById('ctl00_Content_Data_IQCustom1_lbtnCancelPopUp').style.display = "block";
            document.getElementById('ctl00_Content_Data_IQCustom1_lbtnCancelPopUpUpload').style.display = "none";
            $('#ctl00_Content_Data_IQCustom1_divFU').attr('style', 'display:block');
        }

        function ClearAllQueue() {

            try {
                $('#ctl00_Content_Data_IQCustom1_FileUpload1').uploadifyClearQueue();
            }
            catch (e) {
            }

            //$find('mpeUploadMedia').hide();
            CloseiqCustomModal('pnlUploadMedia');
            //$get("ctl00_Content_Data_IQCustom1_tblUpload").style.display = "block";

            document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "block";
            document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "block";
            $get('ctl00_Content_Data_IQCustom1_FileUpload1Queue').style.display = "none";
            $get("ctl00_Content_Data_IQCustom1_tblUploading").style.display = "none";
            document.getElementById('ctl00_Content_Data_IQCustom1_vsUploadMedia').style.display = 'none';
            document.getElementById('ctl00_Content_Data_IQCustom1_txtDescription').value = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_txtKeywords').value = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_txtMediaTitle').value = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_lblMsg').innerHTML = '';
            document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = "";
            document.getElementById('ctl00_Content_Data_IQCustom1_ddlCategory').selectedIndex = 0;
            document.getElementById('ctl00_Content_Data_IQCustom1_ddlSubCategory1').selectedIndex = 0;
            document.getElementById('ctl00_Content_Data_IQCustom1_ddlSubCategory2').selectedIndex = 0;
            document.getElementById('ctl00_Content_Data_IQCustom1_ddlSubCategory3').selectedIndex = 0;
            document.getElementById('ctl00_Content_Data_IQCustom1_lbtnCancelPopUp').style.display = "block";
            document.getElementById('ctl00_Content_Data_IQCustom1_lbtnCancelPopUpUpload').style.display = "none";
            $('#ctl00_Content_Data_IQCustom1_divFU').attr('style', 'display:block;float:left;');
            document.getElementById('ctl00_Content_Data_IQCustom1_hndIsFtpUpload').value = "false";


        }             

    </script>
    <script language="javascript" type="text/javascript">
        function CheckExt() {
            var filename = document.getElementById('ctl00_Content_Data_IQCustom1_hdnName').value;
            var extentionIndex = filename.lastIndexOf('.');
            var extention = filename.substring(extentionIndex + 1);
            var extlower = extention.toLowerCase();
            if (searchString(Extention, extlower)) {
                return true;
            }
            else {
                if (document.getElementById("ctl00_Content_Data_IQCustom1_hndIsFtpUpload").value != "true") {
                    $('#ctl00_Content_Data_IQCustom1_FileUpload1').uploadifyClearQueue();
                }
                $get("ctl00_Content_Data_IQCustom1_tblUpload").style.display = "block";
                $get("ctl00_Content_Data_IQCustom1_tblUploading").style.display = "none";
                document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "block";
                document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "block";
                document.getElementById("ctl00_Content_Data_IQCustom1_lblMsg").innerHTML = document.getElementById('ErrorMessage').value;
                return false;
            }
        }
        function searchString(arrayToSearch, stringToSearch) {
            arrayToSearch.sort();

            for (var i = 0; i < arrayToSearch.length; i++) {
                var arraytoLower = arrayToSearch[i].toLowerCase();
                if (arraytoLower == stringToSearch)
                    return true;
            }
            return false;
        }
    </script>
    <script type="text/javascript">

        function LoadUploadify() {
            $(document).ready(function () {
                $("#ctl00_Content_Data_IQCustom1_FileUpload1").uploadify({
                    'uploader': 'uploadify.swf',
                    'script': 'Upload.ashx',
                    'cancelImg': 'cancel.png',
                    'folder': 'uploads',
                    'sizeLimit': 2000000000000000,
                    'fileExt': '*.mp3;*.mp4;*.flv',
                    'multi': false,
                    'buttonImg': 'browse.jpg',
                    'width': '68px',
                    'height': '22px',
                    'onSelect': function (event, ID, fileObj) {

                        $get('ctl00_Content_Data_IQCustom1_FileUpload1Queue').style.display = "block";
                        document.getElementById('ctl00_Content_Data_IQCustom1_hdnName').value = fileObj.name;
                        document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = fileObj.name;
                        document.getElementById('ctl00_Content_Data_IQCustom1_hndIsFtpUpload').value = "false";
                        var IsAllowFtp = document.getElementById("ctl00_Content_Data_IQCustom1_IsAllowFtp").value;
                        if (IsAllowFtp != undefined && IsAllowFtp == "1")
                            document.getElementById('ctl00_Content_Data_IQCustom1_btnFtpBrowse').style.display = "none";

                    },
                    'onError': function (event, ID, fileObj, errorObj) {

                        alert("An error has occurred. Please try again" + errorobj.info);
                        $('#ctl00_Content_Data_IQCustom1_FileUpload1').uploadifyClearQueue();
                        document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "block";
                        document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "block";
                        document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = "";
                        document.getElementById('ctl00_Content_Data_IQCustom1_vsUploadMedia').style.display = 'none';
                        document.getElementById('ctl00_Content_Data_IQCustom1_txtDescription').value = '';
                        document.getElementById('ctl00_Content_Data_IQCustom1_txtKeywords').value = '';
                        document.getElementById('ctl00_Content_Data_IQCustom1_txtMediaTitle').value = '';
                        document.getElementById('ctl00_Content_Data_IQCustom1_lblMsg').innerHTML = '';
                        document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = "";
                        document.getElementById('ctl00_Content_Data_IQCustom1_ddlCategory').selectedIndex = 0;
                        document.getElementById('ctl00_Content_Data_IQCustom1_ddlSubCategory1').selectedIndex = 0;
                        document.getElementById('ctl00_Content_Data_IQCustom1_ddlSubCategory2').selectedIndex = 0;
                        document.getElementById('ctl00_Content_Data_IQCustom1_ddlSubCategory3').selectedIndex = 0;
                        document.getElementById('ctl00_Content_Data_IQCustom1_ddlTimeZone').selectedIndex = 0;

                    },

                    'onClearQueue': function (event) {

                    },

                    'onCancel': function (event, ID, fileObj, data) {

                        document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "block";
                        document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "block";
                        document.getElementById('ctl00_Content_Data_IQCustom1_fileuploadname').value = "";
                        var IsAllowFtp = document.getElementById("ctl00_Content_Data_IQCustom1_IsAllowFtp").value;
                        if (IsAllowFtp != undefined && IsAllowFtp == "1")
                            document.getElementById('ctl00_Content_Data_IQCustom1_btnFtpBrowse').style.display = "inline";
                    },
                    'onAllComplete': function (event, data) {

                        if (data.errors > 0) {

                        }
                        else {
                            document.getElementById('ctl00_Content_Data_IQCustom1_btnUploadMedia').click();
                            document.getElementById('ctl00_Content_Data_IQCustom1_tblUploading').style.display = "block";
                            document.getElementById('ctl00_Content_Data_IQCustom1_txtFile').value = document.getElementById('ctl00_Content_Data_IQCustom1_hdnName').value;
                            document.getElementById('ctl00_Content_Data_IQCustom1_lblUploadedStatus').innerHTML = "Completed";
                            document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "none";
                            document.getElementById('ctl00_Content_Data_IQCustom1_tblFile').style.display = "none";
                            document.getElementById('ctl00_Content_Data_IQCustom1_lbtnCancelPopUpUpload').style.display = "block";
                            document.getElementById('ctl00_Content_Data_IQCustom1_lbtnCancelPopUp').style.display = "none";

                        }
                    }
                });

                $("#upload").click(function () {

                    //alert('...inside uploadify....');

                    Page_ClientValidate('vgUploadMedia');
                    if (Page_IsValid != null && Page_IsValid == true) {

                        if (document.getElementById('ctl00_Content_Data_IQCustom1_hdnName').value != "") {
                            var IsValidFile = CheckExt();
                            if (IsValidFile) {

                                document.getElementById("ctl00_Content_Data_IQCustom1_lblMsg").innerHTML = "";
                                document.getElementById('ctl00_Content_Data_IQCustom1_tblNewUpload').style.display = "none";

                                if (document.getElementById("ctl00_Content_Data_IQCustom1_hndIsFtpUpload").value != "true") {
                                    $('#ctl00_Content_Data_IQCustom1_FileUpload1').uploadifyUpload();
                                    return false;
                                }
                                else {
                                    document.getElementById('ctl00_Content_Data_IQCustom1_btnUploadMedia').click();
                                }

                            }

                        }
                        else {

                            document.getElementById('ctl00_Content_Data_IQCustom1_lblMsg').innerHTML = "Please Select File.";
                        }
                    }
                });
            });
        }
    </script>
</asp:Content>
