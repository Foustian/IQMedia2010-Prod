<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HeaderTabPanel.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.HeaderTabPanel.HeaderTabPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="Ajax" %>
<%-- <nav>
      <div>
            <ul class="sf-submenu">
              <li class="current first"><a id="aMYIQ" runat="server"   href="~/myclips">MY iQ</a></li>
              <!--<li><a href="javascript:;" rel="dropmenu1">Products</a></li>-->
              <li><a id="aIQBasic"  runat="server" href="~/ClipandRawMedia">iQ BASIC</a></li>
              <li><a id="aIQAdvanced" runat="server" href="~/IQAdvance">iQ ADVANCED</a></li>
              <li><a id="aIQAgent" runat="server" href="~/IQAgent">iQ AGENT</a></li>
              <li class="last"><a id="aIQCustom" runat="server" href="~/IQCustom">iQ CUSTOM</a></li>
            </ul>
      </div>
      

    </nav>--%>
<ul class="nav" id="main-nav">
    <li class="" id="aMYIQ" runat="server"><a runat="server" href="~/myclips" class="first">
        My iQ</a></li>
    <li id="aMYIQnew" runat="server"><a runat="server" href="~/MyIQ">my iQ <%--<img style="margin-top:-5px;position:absolute;" alt="" src="../Images/new.png" />--%></a></li>
    <li id="aIQBasic" runat="server"><a runat="server" href="~/ClipandRawMedia">cliQ TV</a></li>
    <li id="aIQAdvanced" runat="server"><a runat="server" href="~/IQAdvance">iQ Advanced</a></li>
    <li id="aIQAgent" runat="server"><a runat="server" href="~/IQAgent">cliQ Agent</a></li>
    <li id="aIQPremium" runat="server" class="active"><a runat="server" href="~/IQPremium">
        omni cliQ</a></li>
    <li runat="server" id="aIQCustom" class=""><a runat="server" href="~/IQCustom" class="last">
        cliQ UGC</a></li>
</ul>
