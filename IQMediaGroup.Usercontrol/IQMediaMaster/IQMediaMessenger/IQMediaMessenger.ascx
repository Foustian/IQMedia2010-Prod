<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQMediaMessenger.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQMediaMessenger.IQMediaMessenger" %>
<h2>
    iQ Media Messenger</h2>
<p class="subtitle">
    <a href="http://iqmediamessenger.blogspot.com/" target="_blank">Visit our blog</a>
    for the latest updates, industry news, tips and tricks and more!</p>
<div class="messenger msgscroll flexcroll flexcrollactive">
<%-- <p>
        <span>8/30/10</span> iQ Media Group launches service update. <a href="#">Read post...</a>
    </p>
    <p>
        <span>8/20/10</span> iQ Media Group Tips and Tricks: How to locate targeret broadcast
        content quickly. <a href="#">Read post...</a>
    </p>
    <p>
        <span>8/10/10</span> Industry trends in clipping services reveal interesting items.
        <a href="#">Read post...</a>
    </p>
    <p>
        <span>7/30/10</span> iQ Media Group launches service update. <a href="#">Read post...</a>
    </p>--%>
<!-- AddThis Button BEGIN -->
<%--<div class="addthis_toolbox addthis_default_style">
<a href="http://www.addthis.com/bookmark.php?v=250&amp;username=xa-4ccab4a5282ea452" class="addthis_button_compact">Share</a>
<span class="addthis_separator">|</span>

</div>
<script type="text/javascript" src="http://s7.addthis.com/js/250/addthis_widget.js#username=xa-4ccab4a5282ea452"></script>--%>
<!-- AddThis Button END -->
<div>
        <asp:Repeater runat="server" ID="rptRSS">
            <ItemTemplate>
                <p>
                    <span>
                        <%# DataBinder.Eval(Container.DataItem,"PublishDate") %></span>
                    <%# DataBinder.Eval(Container.DataItem,"Title") %>
                    <a href='<%# DataBinder.Eval(Container.DataItem,"Link") %>' target="_blank">Read post...</a>
                </p>
            </ItemTemplate>
        </asp:Repeater>
        <asp:Label ID="lblErrorMessage" ForeColor="#28517C" Font-Bold="true" runat="server" Visible="false"></asp:Label>
    </div>
</div>

