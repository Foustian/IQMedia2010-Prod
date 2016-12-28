<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="IQMediaGroup.AdvanceSearchServiceSample.Search" %>

<%--<%@ Register Assembly="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    Namespace="Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet"
    TagPrefix="cc1" %>--%>
<%@ Register Assembly="Flan.Controls" Namespace="Flan.Controls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <style type="text/css">
        .bluebox-hd
        {
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
            font-weight: bold;
            color: #FFFFFF;
            text-decoration: none;
        }
        .content-text-blue
        {
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
            line-height: 15px; /*color: #335f97;*/
            text-decoration: none;
            width: auto;
        }
        
        .dropdown01
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            text-decoration: none;
            color: #333333;
        }
        .grey-grad2
        {
            background-color: #f8f8f8;
            background-image: url(../images/grey-grad.jpg);
            background-repeat: repeat-x;
            background-position: bottom;
            padding: 15px;
        }
        
        .content-textRd
        {
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
            line-height: 15px;
            color: #535353;
            text-decoration: none;
            border-bottom-style: solid;
            border-bottom-width: 1px;
            border-bottom-color: #e4e4e4;
        }
        
        .hdbar
        {
            background-color: #F4F4F4;
            border: 1px solid #ddd;
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
            line-height: 25px;
            color: #335f97;
            text-decoration: none;
        }
        .hdbar div.show
        {
            text-decoration: none;
            color: #335f97;
            background-image: url(../images/arrow-down.gif);
            background-repeat: no-repeat;
            background-position: left center;
            padding-left: 15px;
            font-weight: bold;
            width: 100px;
        }
        .hdbar div.hide
        {
            text-decoration: none;
            color: #335f97;
            background-image: url(../images/arrow-up.gif);
            background-repeat: no-repeat;
            background-position: left center;
            padding-left: 15px;
            font-weight: bold;
            width: 25px;
        }
        
        
        .updateProgress
        {
            position: absolute;
            background-color: #989898;
            filter: Alpha(Opacity=70);
            opacity: 0.70;
            -moz-opacity: 0.70;
            padding: 0px;
            margin: 0px;
            left: 250px;
            width: 350px;
        }
        .updateProgress div
        {
            padding: 4px;
            position: absolute;
            top: 45%;
            left: 35%;
        }
        
        .MyCalendar .ajax__calendar_container
        {
            background-color: white;
            z-index: 999999;
        }
        
        .textbox04
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            text-decoration: none;
            border: 1px solid #d8d5d5;
            padding: 2px;
            color: #333333;
        }
        
        .textbox03
        {
            font-family: Verdana, Arial, Helvetica, sans-serif;
            font-size: 11px;
            text-decoration: none;
            border: 1px solid #d8d5d5;
            padding: 4px 3px;
            color: #333333;
        }
        
        .responsediv
        {
            display: inline-block;
            float: right;
            width: 270px;
        }
        
        body
        {
            margin: 13px 0px;
            padding: 0px;
            background-position: top;
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
            color: #535353;
            text-decoration: none; /*background-image: url(../images/form-bg.jpg);
            background-repeat: repeat-x;
            background-color: #dddddd;
            background-position: left top;
            margin: 0px;
            padding: 0px;*/
        }
        
        .show
        {
            text-decoration: none;
            color: #335f97;
            background-image: url(../images/arrow-down.gif);
            background-repeat: no-repeat;
            background-position: left center;
            padding-left: 15px;
            font-weight: bold;
            width: 100px;
        }
        .hide
        {
            text-decoration: none;
            color: #335f97;
            background-image: url(../images/arrow-up.gif);
            background-repeat: no-repeat;
            background-position: left center;
            padding-left: 15px;
            font-weight: bold;
            width: 25px;
        }
        .btn-play
        {
            background-color: #ffffff;
            background-image: url(../images/btn-play.jpg);
            background-repeat: no-repeat;
            height: 22px;
            width: 24px;
            border-style: none;
            font-weight: bold;
            color: #FFFFFF;
            font-family: Tahoma, Verdana, Arial;
            font-size: 11px;
        }
        .btn-blue3
        {
            background-color: #F4F4F4; /*background-image: url(../images/btn-grey-bg.jpg);*/
            background-repeat: repeat-x;
            border: 1px solid #777;
            color: #335f97;
            font-family: Arial,Helvetica,sans-serif;
            font-size: 11px;
            font-weight: bold;
            height: 25px;
            padding: 0 15px;
        }
    </style>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script type="text/javascript">


        function CheckUnCheckAll(isCheck, rptChecklist) {
            if (isCheck) {

                $("#" + rptChecklist + " input:checkbox").each(function (index) {
                    $(this).attr("disabled", "disabled");
                    $(this).attr("checked", "checked");
                });
            }
            else {
                $("#" + rptChecklist + " input:checkbox").each(function (index) {
                    $(this).removeAttr("disabled");
                    $(this).removeAttr("checked");
                });
            }
        }

  
    
    </script>
</head>
<body>
    <form id="form1" runat="server" defaultbutton="btnlogin">
    <AjaxToolkit:ToolkitScriptManager ID="ScriptManager1" EnablePageMethods="true" AsyncPostBackTimeout="3600"
        runat="server">
    </AjaxToolkit:ToolkitScriptManager>
    <table width="100%">
        <tr>
            <td width="15%">
            </td>
            <td width="70%">
                <a id="hlogo" runat="server">
                    <img id="imgMainLogo" runat="server" src="~/images/logo.png" alt="" /></a>
            </td>
            <td width="15%">
                <%-- <asp:UpdatePanel ID="uplogout" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>--%>
                <%--<asp:LinkButton ID="lnkblogout" runat="server" Text="LogOut" OnClick="lnkblogout_click"></asp:LinkButton>--%>
                <%--</ContentTemplate>
                </asp:UpdatePanel>--%>
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
        </tr>
    </table>
    <table id="tbllogin" runat="server" width="100%">
        <tr>
            <td width="15%">
            </td>
            <td width="70%">
                <table>
                    <tr>
                        <asp:Label ID="lblMessage" runat="server" Visible="false" Text="Message" ForeColor="Red"></asp:Label>
                    </tr>
                    <tr>
                        <td>
                            UserID:
                        </td>
                        <td>
                            <asp:TextBox ID="txtUserID" runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvuserid" runat="server" ControlToValidate="txtUserID"
                                ErrorMessage="*" ForeColor="Red" ValidationGroup="Authenticate"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Password:
                        </td>
                        <td>
                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvpassword" runat="server" ControlToValidate="txtPassword"
                                ErrorMessage="*" ForeColor="Red" ValidationGroup="Authenticate"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <asp:Button ID="btnlogin" runat="server" Text="LOGIN" OnClick="btnlogin_click" ValidationGroup="Authenticate"
                                CssClass="btn-blue3" />
                        </td>
                    </tr>
                </table>
            </td>
            <td width="15%">
            </td>
        </tr>
    </table>
    <table id="tbldata" cellpadding="0" cellspacing="0" width="100%" runat="server">
        <tr>
            <td width="15%">
            </td>
            <td width="70%">
                <table width="100%" style="border-style: solid; border-width: thin; padding: 5px 5px 5px 5px">
                    <tr>
                        <td width="100%">
                            <asp:UpdatePanel ID="upVideo" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table border="0" cellpadding="1" cellspacing="0" width="100%" id="tblvideo" runat="server">
                                        <tr>
                                            <td class="hdbar">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="10%" style="padding-left: 10px">
                                                            <div class="show" style="cursor: pointer;" onclick="showdivVideo()">
                                                                Show Video</div>
                                                        </td>
                                                        <td width="85%">
                                                        </td>
                                                        <td width="5%" align="right" style="padding-right: 10px">
                                                            <div class="hide" style="cursor: pointer;" onclick="hidedivVideo()">
                                                                Hide</div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divvideo" runat="server" width="100%">
                                                    <iframe id="Clipframe" runat="server" visible="false" style="width: 585px; height: 730px;"
                                                        scrolling="no" marginwidth="0" marginheight="0" hspace="0" vspace="0" border="0"
                                                        frameborder="0"></iframe>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblGrid" runat="server" width="100%">
                                        <tr>
                                            <td class="hdbar" height="25" width="100%">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="10%" style="padding-left: 10px">
                                                            <div class="show" style="cursor: pointer;" onclick="showdivResult()">
                                                                Show Result</div>
                                                        </td>
                                                        <td width="85%">
                                                        </td>
                                                        <td width="5%" align="right" style="padding-right: 10px">
                                                            <div class="hide" style="cursor: pointer;" onclick="hidedivResult()">
                                                                Hide</div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divresult" runat="server">
                                                    <table cellpadding="1" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td width="100%">
                                                                <%-- <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                                                            <ContentTemplate>--%>
                                                                <asp:GridView ID="grvRawMediaPMGBasic" runat="server" AutoGenerateColumns="false"
                                                                    Width="100%" border="1" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4"
                                                                    Style="border-collapse: collapse;" AllowSorting="true" AllowPaging="true" PageSize="10"
                                                                    OnSorting="grvRawMediaPMGBasic_Sorting" EmptyDataText="No Results Found">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Image ID="imgStationLogo" ImageUrl='<%# Eval("StationLogo") %>' runat="server" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="grid-th" Width="10%" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="10%" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Program" DataField="Title120" ItemStyle-CssClass="heading-blue"
                                                                            HeaderStyle-HorizontalAlign="Left">
                                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="35%" />
                                                                            <HeaderStyle Width="35%" />
                                                                        </asp:BoundField>
                                                                        <asp:BoundField DataField="IQ_Dma_Name" HeaderText="Market">
                                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="20%" />
                                                                            <HeaderStyle CssClass="grid-th" Width="20%" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Date Time">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRawMediaDatetime" runat="server" Text='<%# Eval("DateTime") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="20%" />
                                                                            <HeaderStyle CssClass="grid-th" Width="20%" />
                                                                        </asp:TemplateField>
                                                                        <asp:BoundField HeaderText="Hits" DataField="Hits" ItemStyle-CssClass="content-text-new"
                                                                            HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemStyle HorizontalAlign="Right" Width="5%" />
                                                                            <HeaderStyle CssClass="grid-th" Width="5%" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandArgument='<%# Eval("RawMediaID") %>'
                                                                                    OnClientClick="hidediv();" OnCommand="lbtnRawMediaPlay_Command" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="grid-th" Width="5%" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="grid-th" />
                                                                    <PagerStyle Height="30px" VerticalAlign="Middle" HorizontalAlign="Left" CssClass="pagecontent1" />
                                                                    <PagerSettings Visible="false" />
                                                                </asp:GridView>
                                                                <div style="padding-top: 10px; padding-bottom: 10px;" id="divrawmediapaging" runat="server"
                                                                    visible="false">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <span style="vertical-align: bottom">
                                                                                        <asp:ImageButton ID="imgRawMediaPrevious" runat="server" AlternateText="Previous"
                                                                                            OnClick="imgRawMediaPrevious_Click" ImageUrl="~/Images/arrow-previous.jpg" />
                                                                                    </span><span style="vertical-align: top">
                                                                                        <asp:Label ID="lblCurrentPageNo" Font-Bold="true" runat="server"></asp:Label>
                                                                                    </span><span style="vertical-align: bottom">
                                                                                        <asp:ImageButton ID="imgRawMediaNext" runat="server" AlternateText="Next" OnClick="imgRawMediaNext_Click"
                                                                                            ImageUrl="~/Images/arrow-next.jpg" />
                                                                                    </span>
                                                                                </div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <asp:GridView ID="grvRadioStations" runat="server" AutoGenerateColumns="false" AllowPaging="true"
                                                                    PageSize="10" Width="100%" border="1" CellPadding="5" CellSpacing="0" BorderColor="#e4e4e4"
                                                                    Style="border-collapse: collapse;" AllowSorting="true" EmptyDataText="No Results Found"
                                                                    PageIndex="0" OnPageIndexChanging="grvRadioStations_PageIndexChanging" OnSorting="grvRadioStations_Sorting">
                                                                    <Columns>
                                                                        <%--<asp:TemplateField HeaderText="Station" HeaderStyle-HorizontalAlign="Center">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgStationLogo" ImageUrl='<%# Eval("StationID") %>' runat="server" />
                                                                </ItemTemplate>
                                                                <HeaderStyle CssClass="grid-th" Width="38px" />
                                                                <ItemStyle HorizontalAlign="Center" Width="38px" />
                                                            </asp:TemplateField>--%>
                                                                        <asp:BoundField DataField="dma_name" HeaderText="Market">
                                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="50%" />
                                                                            <HeaderStyle CssClass="grid-th" Width="50%" />
                                                                        </asp:BoundField>
                                                                        <asp:TemplateField HeaderText="Date Time">
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblRawMediaDatetime" runat="server" Text='<%# Eval("DateTime") %>'></asp:Label>
                                                                            </ItemTemplate>
                                                                            <ItemStyle CssClass="content-text2" HorizontalAlign="Left" Width="40%" />
                                                                            <HeaderStyle CssClass="grid-th" Width="40%" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField ShowHeader="false" HeaderText="Play" HeaderStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <asp:Button ID="lbtnPlay" runat="server" CssClass="btn-play" CommandArgument='<%# Eval("RawMediaID") %>'
                                                                                    OnClientClick="hidediv();" OnCommand="lbtnRawMediaPlay_Command" />
                                                                            </ItemTemplate>
                                                                            <HeaderStyle CssClass="grid-th" Width="5%" />
                                                                            <ItemStyle HorizontalAlign="Center" Width="5%" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <HeaderStyle CssClass="grid-th" />
                                                                    <PagerStyle Height="30px" VerticalAlign="Middle" HorizontalAlign="Left" CssClass="pagecontent1" />
                                                                    <PagerTemplate>
                                                                        <div style="padding-top: 10px; padding-bottom: 10px;">
                                                                            <table>
                                                                                <tr>
                                                                                    <td valign="middle">
                                                                                        <span style="vertical-align: bottom">
                                                                                            <asp:ImageButton ID="btnPrevious" runat="server" AlternateText="Previous" CommandName="Page"
                                                                                                CommandArgument="Prev" ImageUrl="~/Images/arrow-previous.jpg" />
                                                                                        </span><span style="vertical-align: top">
                                                                                            <asp:Label ID="lblRadoCurrentPageNo" Font-Bold="true" runat="server"></asp:Label>
                                                                                        </span><span style="vertical-align: bottom">
                                                                                            <asp:ImageButton ID="btnNext" runat="server" AlternateText="Next" CommandName="Page"
                                                                                                CommandArgument="Next" ImageUrl="~/Images/arrow-next.jpg" />
                                                                                        </span>
                                                                                </tr>
                                                                            </table>
                                                                        </div>
                                                                    </PagerTemplate>
                                                                </asp:GridView>
                                                                <div style="padding-top: 10px; padding-bottom: 10px; vertical-align: top" id="divradiorawmediapaging"
                                                                    runat="server" visible="false">
                                                                    <table width="100%">
                                                                        <tr>
                                                                            <td valign="top">
                                                                                <asp:ImageButton ID="imgbtnradioprv" runat="server" AlternateText="Previous" OnClick="imgbtnradioprv_Click"
                                                                                    ImageUrl="~/Images/arrow-previous.jpg" />
                                                                                <asp:Label ID="lblradiopage" Font-Bold="true" runat="server"></asp:Label>
                                                                                <asp:ImageButton ID="imgbtnradionxt" runat="server" AlternateText="Next" OnClick="imgbtnradionxt_Click"
                                                                                    ImageUrl="~/Images/arrow-next.jpg" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </div>
                                                                <%--   </ContentTemplate>
                                                        </asp:UpdatePanel>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="upprggrid" runat="server" AssociatedUpdatePanelID="upGrid"
                                DisplayAfter="0">
                                <ProgressTemplate>
                                    <div>
                                        <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                                            alt="Loading..." id="imgLoading" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender2"
                                ControlToOverlayID="upGrid" TargetControlID="upprggrid" CssClass="updateProgress" />
                        </td>
                    </tr>
                    <tr>
                        <td class="hdbar" height="25" width="100%">
                            <table width="100%">
                                <tr>
                                    <td width="10%" style="padding-left: 10px">
                                        <div class="show" style="cursor: pointer;" onclick="showdivSearch()">
                                            Show Search</div>
                                    </td>
                                    <td width="85%">
                                    </td>
                                    <td width="5%" align="right" style="padding-right: 10px">
                                        <div class="hide" style="cursor: pointer;" onclick="hidedivSearch()">
                                            Hide</div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td width="100%">
                            <asp:UpdatePanel ID="updSearchTerms" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <div id="divSearch" runat="server">
                                        <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img id="Img7" runat="server" src="~/images/spacer.gif" width="1" height="1" border="0"
                                                        alt="iQMedia" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="bluebox-content">
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td style="background-color: #EFEFEF; color: #000000; font-weight: bold; font-family: Arial,Helvetica,sans-serif;
                                                                height: 25px" width="100%">
                                                                &nbsp;Search Terms<br />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background: #f8f8f8" align="left">
                                                                <asp:RadioButtonList ID="rblTvOrRadio" AutoPostBack="true" runat="server" RepeatDirection="Horizontal"
                                                                    Style="vertical-align: middle;" CellPadding="5" OnSelectedIndexChanged="rblTvOrRadio_SelectedIndexChanged">
                                                                    <asp:ListItem Value="0" Selected="True"><span style="vertical-align:top;padding-left:5px;">TV Stations</span></asp:ListItem>
                                                                    <asp:ListItem Value="1"><span style="vertical-align:top;padding-left:5px;">Radio Stations</span></asp:ListItem>
                                                                </asp:RadioButtonList>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background: #f8f8f8">
                                                                <%--<asp:UpdatePanel ID="updSearchTerms" runat="server">
                                                            <ContentTemplate>--%>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr id="trInitialSearch" runat="server">
                                                                        <td>
                                                                            <table width="100%" border="0" cellspacing="0" cellpadding="3">
                                                                                <tr>
                                                                                    <td colspan="2">
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Program Title:
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtProgram" CssClass="textbox04" runat="server" Height="22px" Width="400px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Appearing:
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtAppearing" CssClass="textbox04" runat="server" Height="22px"
                                                                                            Width="400px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                        Search Term:
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtSearch" CssClass="textbox04" runat="server" Height="22px" Width="400px"></asp:TextBox>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="padding: 10px 0px 0px 0px">
                                                                            <asp:Button ID="btnSubmit" runat="server" ValidationGroup="AdvancedSearch" CssClass="btn-blue3"
                                                                                Text="Search" OnClick="btnSubmit_Click" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblAdvMsg" runat="server" ForeColor="#bd0000"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                                <%-- </ContentTemplate>
                                                        </asp:UpdatePanel>--%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                        <tr>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background-color: #EFEFEF; color: #000000; font-weight: bold; font-family: Arial,Helvetica,sans-serif;
                                                                height: 25px">
                                                                &nbsp;Date &amp; Time &amp; Sorting<br />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="150" valign="top" style="background: #f8f8f8">
                                                    <asp:UpdatePanel ID="date" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table width="100%" border="0" cellpadding="3" cellspacing="0">
                                                                <tr id="trTimeZone" runat="server">
                                                                    <td width="125" class="content-text1">
                                                                        Time Zone:&nbsp;&nbsp;
                                                                    </td>
                                                                    <td>
                                                                        <asp:DropDownList ID="ddlTimeZone" runat="server" CssClass="dropdown01">
                                                                            <asp:ListItem Text="ALL" Value="0"></asp:ListItem>
                                                                            <asp:ListItem Text="EST" Value="1"></asp:ListItem>
                                                                            <asp:ListItem Text="CST" Value="2"></asp:ListItem>
                                                                            <asp:ListItem Text="MST" Value="3"></asp:ListItem>
                                                                            <asp:ListItem Text="PST" Value="4"></asp:ListItem>
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td>
                                                                    </td>
                                                                </tr>
                                                                <tr width="210px">
                                                                    <td valign="top" width="125" class="content-text1">
                                                                        Date:
                                                                    </td>
                                                                    <td class="content-text1">
                                                                        <table>
                                                                            <tr>
                                                                                <td>
                                                                                    From DateTime:
                                                                                    <asp:TextBox ID="txtFromDate" runat="server" AutoCompleteType="None" CssClass="textbox03"></asp:TextBox>
                                                                                    <asp:DropDownList ID="ddlStartTime" runat="server" AutoPostBack="false" CssClass="dropdown01">
                                                                                        <asp:ListItem Text="12:00" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="01:00" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="02:00" Value="2"></asp:ListItem>
                                                                                        <asp:ListItem Text="03:00" Value="3"></asp:ListItem>
                                                                                        <asp:ListItem Text="04:00" Value="4"></asp:ListItem>
                                                                                        <asp:ListItem Text="05:00" Value="5"></asp:ListItem>
                                                                                        <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                                                                                        <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                                                                                        <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                                                                                        <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                                                                                        <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                                                                                        <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtFromDate" TargetControlID="txtFromDate"
                                                                                        WatermarkCssClass="watermarked" WatermarkText="From Date" runat="server">
                                                                                    </AjaxToolkit:TextBoxWatermarkExtender>
                                                                                    <AjaxToolkit:CalendarExtender ID="CalEtxtFromDate" runat="server" CssClass="MyCalendar"
                                                                                        TargetControlID="txtFromDate">
                                                                                    </AjaxToolkit:CalendarExtender>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:RadioButtonList ID="rdAmPmFromDate" runat="server" CellPadding="0" CellSpacing="0"
                                                                                        BorderWidth="0" RepeatDirection="Vertical" Style="padding-left: -5px;" Height="40px"
                                                                                        Width="36px">
                                                                                        <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                                        <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                                    </asp:RadioButtonList>
                                                                                    <asp:CustomValidator ID="cusFromDate" runat="server" Display="Dynamic" ControlToValidate="txtFromDate"
                                                                                        ValidationGroup="AdvancedSearch" OnServerValidate="cvFromDate_ServerValidate"
                                                                                        Text="*" ErrorMessage="From Date should not be greater than Current Date"></asp:CustomValidator>
                                                                                </td>
                                                                                <td>
                                                                                    To DateTime:<br />
                                                                                    <asp:TextBox ID="txtToDate" runat="server" AutoCompleteType="None" CssClass="textbox03"></asp:TextBox>
                                                                                    <asp:DropDownList ID="ddlEndTime" runat="server" AutoPostBack="false" CssClass="dropdown01">
                                                                                        <asp:ListItem Text="12:00" Value="0"></asp:ListItem>
                                                                                        <asp:ListItem Text="01:00" Value="1"></asp:ListItem>
                                                                                        <asp:ListItem Text="02:00" Value="2"></asp:ListItem>
                                                                                        <asp:ListItem Text="03:00" Value="3"></asp:ListItem>
                                                                                        <asp:ListItem Text="04:00" Value="4"></asp:ListItem>
                                                                                        <asp:ListItem Text="05:00" Value="5"></asp:ListItem>
                                                                                        <asp:ListItem Text="06:00" Value="6"></asp:ListItem>
                                                                                        <asp:ListItem Text="07:00" Value="7"></asp:ListItem>
                                                                                        <asp:ListItem Text="08:00" Value="8"></asp:ListItem>
                                                                                        <asp:ListItem Text="09:00" Value="9"></asp:ListItem>
                                                                                        <asp:ListItem Text="10:00" Value="10"></asp:ListItem>
                                                                                        <asp:ListItem Text="11:00" Value="11"></asp:ListItem>
                                                                                    </asp:DropDownList>
                                                                                    <AjaxToolkit:TextBoxWatermarkExtender ID="tbwEtxtToDate" TargetControlID="txtToDate"
                                                                                        WatermarkCssClass="watermarked" WatermarkText="To Date" runat="server">
                                                                                    </AjaxToolkit:TextBoxWatermarkExtender>
                                                                                    <AjaxToolkit:CalendarExtender ID="valEtxtToDate" runat="server" CssClass="MyCalendar"
                                                                                        TargetControlID="txtToDate">
                                                                                    </AjaxToolkit:CalendarExtender>
                                                                                </td>
                                                                                <td class="content-text1" width="200px">
                                                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                                                        <tr>
                                                                                            <td>
                                                                                            </td>
                                                                                            <td class="content-text-blue" align="left">
                                                                                                <asp:RadioButtonList ID="rdAMPMToDate" runat="server" CellPadding="0" CellSpacing="0"
                                                                                                    BorderWidth="0" RepeatDirection="Vertical" Style="padding-left: -5px;">
                                                                                                    <asp:ListItem Text="AM" Value="12" Selected="true"></asp:ListItem>
                                                                                                    <asp:ListItem Text="PM" Value="24"></asp:ListItem>
                                                                                                </asp:RadioButtonList>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td colspan="2">
                                                                        Sorting available For: datetime,guid,station,market<br />
                                                                        For Ex. datetime,market-<br />
                                                                        This will sort data first on datetime ascending and than market descending
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        Sort Fields
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtSortField" CssClass="textbox04" runat="server" Height="22px"
                                                                            Width="400px"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="background-color: #EFEFEF; color: #000000; font-weight: bold; height: 25px;
                                                                            font-family: Arial,Helvetica,sans-serif">
                                                                            &nbsp;DMA Rank and Name<br />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="150" valign="top" class="grey-grad2">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr height="29px">
                                                                        <td class="content-textRd" align="center" valign="top">
                                                                            <asp:RadioButtonList ID="rdoMarket" runat="server" CellSpacing="1" CellPadding="1"
                                                                                Width="50%" RepeatColumns="2" BorderWidth="0" AutoPostBack="false" RepeatDirection="Horizontal"
                                                                                TextAlign="Right" Style="vertical-align: middle">
                                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:top">Select All</span></asp:ListItem>
                                                                                <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:top">Manual Select</span></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                            <asp:RadioButtonList ID="rdoRadioStations" runat="server" CellSpacing="1" CellPadding="1"
                                                                                RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right"
                                                                                Style="vertical-align: middle" Width="50%">
                                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:top">Select All</span></asp:ListItem>
                                                                                <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:top">Manual Select</span></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" class="content-text">
                                                                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="300px" Visible="true">
                                                                                <asp:DataList ID="rptMarket" runat="server" RepeatColumns="4" Width="100%" CellPadding="0"
                                                                                    CellSpacing="0" Style="background: #f8f8f8">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                            <tr height="29px;">
                                                                                                <td width="15" height="100%" valign="top">
                                                                                                    <%# Container.ItemIndex+1 %>
                                                                                                </td>
                                                                                                <td width="107" height="100%" valign="top">
                                                                                                    <%# Eval("IQ_Dma_Name") %>
                                                                                                </td>
                                                                                                <td valign="top" height="100%">
                                                                                                    <input type="checkbox" id="chkMarket" runat="server" value='<%# Eval("IQ_Dma_Num") %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:DataList>
                                                                                <asp:DataList ID="rptRadioStations" runat="server" RepeatColumns="4" Width="100%"
                                                                                    CellPadding="0" CellSpacing="0">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                            <tr height="29px;">
                                                                                                <td width="15" height="100%" valign="top">
                                                                                                    <%# Container.ItemIndex+1 %>
                                                                                                </td>
                                                                                                <td width="107" height="100%" valign="top">
                                                                                                    <%# Eval("Dma_Name")%>
                                                                                                    <%--<asp:Label ID="lblRadioStation" runat="server" Text="<%# Container.DataItem %>" CssClass="content-text"></asp:Label>--%>
                                                                                                </td>
                                                                                                <td valign="top" height="100%">
                                                                                                    <input type="checkbox" id="chkRadioStation" runat="server" value='<%# Eval("StationID") %>' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:DataList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trAffiliateNetwork" runat="server">
                                                <td>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="background-color: #EFEFEF; height: 25px; color: #000000; font-weight: bold;
                                                                            font-family: Arial,Helvetica,sans-serif">
                                                                            &nbsp;Affiliate Network<br />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="150" valign="top" class="grey-grad2">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr height="29px">
                                                                        <td class="content-textRd" align="center" valign="top">
                                                                            <asp:RadioButtonList ID="rdoAffil" runat="server" CellSpacing="3" CellPadding="3"
                                                                                Width="50%" RepeatColumns="2" BorderWidth="0" RepeatDirection="Horizontal" TextAlign="Right"
                                                                                Style="vertical-align: middle">
                                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:top">Select All</span></asp:ListItem>
                                                                                <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:top">Manual Select</span></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" class="content-text1">
                                                                            <asp:Panel ID="pnlAffil" runat="server" ScrollBars="Auto" Height="150px" Visible="true">
                                                                                <asp:DataList ID="rptAffil" runat="server" RepeatColumns="4" Width="100%" CellPadding="0"
                                                                                    CellSpacing="0" Style="background: #f8f8f8">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                            <tr height="29px;">
                                                                                                <td width="15" height="100%" valign="top">
                                                                                                    <%# Container.ItemIndex+1 %>
                                                                                                </td>
                                                                                                <td width="107" height="100%" valign="top">
                                                                                                    <%# Eval("Station_Affil") %>
                                                                                                </td>
                                                                                                <td height="100%" valign="top">
                                                                                                    <%--<input type="checkbox" runat="server" value='<%# Eval("Station_Affil_Num") %>' id='chkAffil' />--%>
                                                                                                    <input type="checkbox" runat="server" value='<%# Eval("Station_Affil") %>' id='chkAffil' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:DataList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr id="trProgramSubCategory" runat="server">
                                                <td>
                                                    <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                                                    <tr>
                                                                        <td style="background-color: #EFEFEF; color: #000000; font-weight: bold; font-family: Arial,Helvetica,sans-serif;
                                                                            height: 25px">
                                                                            &nbsp;Program Category<br />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td height="150" valign="top" class="grey-grad2">
                                                                <table width="100%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr height="29px">
                                                                        <td class="content-textRd" align="center" valign="top">
                                                                            <asp:RadioButtonList ID="rdoProgramType" runat="server" CellSpacing="1" CellPadding="1"
                                                                                Width="50%" BorderWidth="0" RepeatDirection="Horizontal" Style="padding-left: -5px;">
                                                                                <asp:ListItem Selected="true" Value="1"><span style="height:50px; padding-left:5px; vertical-align:top">Select All</span></asp:ListItem>
                                                                                <asp:ListItem Value="2"><span style="height:50px; padding-left:5px; vertical-align:top">Manual Select</span></asp:ListItem>
                                                                            </asp:RadioButtonList>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top" class="content-text1">
                                                                            <asp:Panel ID="pnlProgramType" runat="server" ScrollBars="Auto" Height="150px" Visible="true">
                                                                                <asp:DataList ID="rptProgramType" runat="server" RepeatColumns="4" Width="100%" CellPadding="0"
                                                                                    CellSpacing="0" Style="background: #f8f8f8">
                                                                                    <ItemTemplate>
                                                                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                                                                            <tr height="29px;">
                                                                                                <td width="15" height="100%" valign="top">
                                                                                                    <%# Container.ItemIndex+1 %>
                                                                                                </td>
                                                                                                <td width="107" height="100%" valign="top">
                                                                                                    <%# Eval("IQ_Class") %>
                                                                                                </td>
                                                                                                <td height="100%" valign="top">
                                                                                                    <input type="checkbox" runat="server" value='<%# Eval("IQ_Class_Num") %>' id='chkProgramType' />
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </ItemTemplate>
                                                                                </asp:DataList>
                                                                            </asp:Panel>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                            <asp:UpdateProgress ID="updSearchTermsProgress" runat="server" AssociatedUpdatePanelID="updSearchTerms"
                                DisplayAfter="0">
                                <ProgressTemplate>
                                    <div>
                                        <img src="../Images/27-1.gif" style="width: 70px; height: 70px; z-index: 99999999;"
                                            alt="Loading..." id="imgLoading" />
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <cc2:UpdateProgressOverlayExtender runat="server" ID="UpdateProgressOverlayExtender1"
                                ControlToOverlayID="updSearchTerms" TargetControlID="updSearchTermsProgress"
                                CssClass="updateProgress" />
                        </td>
                    </tr>
                    <tr>
                        <td abbr="100%">
                            <asp:UpdatePanel ID="upResponse" runat="server" UpdateMode="Conditional">
                                <ContentTemplate>
                                    <table id="tblResponse" runat="server" width="100%">
                                        <tr>
                                            <td class="hdbar" height="25">
                                                <table width="100%">
                                                    <tr>
                                                        <td width="10%" style="padding-left: 10px">
                                                            <div class="show" style="cursor: pointer;" onclick="showdivResponse()">
                                                                Show Reponse</div>
                                                        </td>
                                                        <td width="85%">
                                                        </td>
                                                        <td width="5%" align="right" style="padding-right: 10px">
                                                            <div class="hide" style="cursor: pointer;" onclick="hidedivResponse()">
                                                                Hide</div>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <div id="divResponse" runat="server">
                                                    <asp:UpdatePanel ID="upjsonreponse" runat="server" UpdateMode="Conditional">
                                                        <ContentTemplate>
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        Request:
                                                                    </td>
                                                                    <td>
                                                                        Response:
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:TextBox ID="txtRequest" CssClass="main" Width="450px" Height="400px" TextMode="MultiLine"
                                                                            ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox ID="txtResponse" CssClass="main" Width="450px" Height="400px" TextMode="MultiLine"
                                                                            ReadOnly="true" runat="server"></asp:TextBox>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </ContentTemplate>
                                                    </asp:UpdatePanel>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="15%">
            </td>
        </tr>
    </table>
    </form>
    <script type="text/javascript">
        function showdivResult() {
            $("#<%=divresult.ClientID %>").show('slow');
        }
        function hidedivResult() {
            $("#<%=divresult.ClientID %>").hide('slow');
            (document.getElementById('<%=divresult.ClientID %>')).setAttribute('enable', 'true');
        }
        function showdivVideo() {
            $("#<%=divvideo.ClientID %>").show('slow');
        }
        function hidedivVideo() {
            $("#<%=divvideo.ClientID %>").hide('slow');
            (document.getElementById('<%=divvideo.ClientID %>')).setAttribute('enable', 'true');
        }
        function showdivSearch() {
            $("#<%=divSearch.ClientID %>").show('slow');
        }
        function hidedivSearch() {
            $("#<%= divSearch.ClientID %>").hide('slow');
            (document.getElementById('<%=divSearch.ClientID %>')).setAttribute('enable', 'true');
        }
        function showdivResponse() {
            $("#<%=divResponse.ClientID %>").show('slow');
        }
        function hidedivResponse() {
            $("#<%= divResponse.ClientID %>").hide('slow');
            (document.getElementById('<%=divResponse.ClientID %>')).setAttribute('enable', 'true');
        }
    </script>
</body>
</html>
