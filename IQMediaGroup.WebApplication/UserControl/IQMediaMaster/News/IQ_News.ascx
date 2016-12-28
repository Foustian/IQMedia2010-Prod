<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQ_News.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.News.IQ_News" %>
<div class="blue-title">
    News
</div>
<div class="clear">
    <asp:DataList ID="dlnews" runat="server" Width="100%">
        <ItemTemplate>
            <div style="width: 50%;">
                <div style="float: left;">
                    <a href='<%# Eval("Url") %>'>
                        <%# Eval("Headline")%></a>
                </div>
                <div style="float: left; clear: both;">
                    <%# Eval("Headline")%></div>
                <div style="float: left; clear: both;">
                    <%# Eval("ReleaseDate")%></div>
                <div style="float: left; clear: both;">
                    <%# Eval("SubHead")%></div>
                <div style="float: left; clear: both;">
                    <%# Eval("Detail")%></div>
                <div style="float: right; clear: both;">
                    <a href='<%# Eval("Url") %>'>Read More....</a></div>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>
