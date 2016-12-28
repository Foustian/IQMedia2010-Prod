<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NavigationPanel.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.NavigationPanel.NavigationPanel" %>
<%--<div class="top-menu">
    <ul>
        <li><a runat="server" href="~/">Home</a></li>
        <li><a runat="server" href="~/About/">About</a></li>
        <li><a runat="server" href="~/News/">News</a></li>
        <li><a runat="server" href="~/Services/">Services</a></li>
        <li><a runat="server" href="~/Corporate/">Corporate</a></li>
        <li><a runat="server" href="~/Industries/">Industries Served</a></li>
        <li><a runat="server" href="http://info.iqmediacorp.com/ContactUs">Contact US</a></li>
     
    </ul>
</div>--%>
<nav>
<div> 
        <ul class="sf-menu">
          <li class="current first"><a id="spnHome" class="selected" runat="server" href="~/">HOME</a></li>          
          <li><a runat="server" id="spnProducts" href="~/Products">PRODUCTS</a>
          <ul>
          <li>
          <a id="A16" runat="server" href="~/Products">OVERVIEW</a>
          <ul>
            <li><a id="A8" runat="server" href="~/BroadcastTV">BROADCAST TV</a></li>
            <li><a id="A9" runat="server" href="~/OnlineNews">ONLINE NEWS</a></li>
            <li><a id="A10" runat="server" href="~/SocialMedia">SOCIAL MEDIA</a></li>
            <li><a id="A11" runat="server" href="~/Twitter">TWITTER</a></li>
            <li><a id="A12" runat="server" href="~/UserGeneratedContent">USER GENERATED CONTENT</a></li>
          </ul>
          </li>

          <li>
          <a id="A14" href="~/cliQ" runat="server">cliQ</a>
          <ul>
          <li><a id="A1" runat="server" href="~/OptimizedMediaCloud">OPTIMIZED MEDIA CLOUD</a></li>
                <li><a id="A2" runat="server" href="~/InlineMediaWorkspace">INLINE MEDIA WORKSPACE</a>                	
                </li>
                <li><a id="A3" runat="server" href="~/myiQStatic">MY iQ</a></li>                
          </ul>
          
          </li>
            	<li><a id="A15" runat="server" href="~/ProPrep">PROPREP</a></li>
            </ul>    
          
          </li>
          <li class="industry"><a id="spnIndustries" runat="server" href="~/Industries">INDUSTRIES</a>
          <ul>
                <li><a id="A17" runat="server" href="~/Industries">OVERVIEW</a></li>
                <li><a id="A4" runat="server" href="~/PoliticalParties">POLITICAL PARTIES</a></li>
                <li><a id="A5" runat="server" href="~/University">UNIVERSITIES</a></li>
                <li><a id="A6" runat="server" href="~/ProfessionalSports">PROFESSIONAL SPORTS TEAMS</a></li>
                <li><a id="A7" runat="server" href="~/CollegiateAthleticPrograms">COLLEGIATE ATHLETIC DEPARTMENTS</a></li>
          </ul>
          </li>
          <li class="resource"><a id="spnResources" runat="server" href="~/Resources">RESOURCES</a>
          <%--<ul style="width:100%;">
          <li><a id="A8" runat="server" target="_blank" href="~/pdf/WP01-Overcoming-TMS-Limits.pdf"><span runat="server" id="Span1">WHITE PAPERS</span></a></li>
          <li><a id="A9" runat="server" target="_blank" href="~/pdf/PL1-cliQ-2-pages.pdf"><span runat="server" id="Span2">QUICK FACTS</span></a></li>
          <li><a id="A10" runat="server" target="_blank" href="~/pdf/PL1-cliQ-4-pages.pdf"><span runat="server" id="Span3">LEARN MORE ABOUT cliQ</span></a></li>
          <li><a id="A11" runat="server" target="_blank" href="~/pdf/PL2-Media-Cloud.pdf"><span runat="server" id="Span4">WHAT IS OPTIMIZED MEDIA CLOUD?</span></a></li>
          <li style="width:100%;"><a id="A12" runat="server" target="_blank" href="~/pdf/PL2-Inline-Workspace.pdf"><span runat="server" id="Span5">LEARN ABOUT THE INLINE MEDIA WORKSPACE</span></a></li>
          <li><a id="A13" runat="server" target="_blank" href="~/pdf/PL2-My-iQ.pdf"><span runat="server" id="Span6">LEARN ABOUT MY iQ</span></a></li>
          </ul>--%>
          </li>
          <li><a id="spnAboutUs" runat="server" href="~/About/">ABOUT US</a>
          <ul>
          <li><a id="A20" runat="server" href="~/About">OVERVIEW</a></li>
          <li><a id="A13" runat="server" href="~/ManagementTeam">MANAGEMENT TEAM</a></li>
          <li><a id="A18" runat="server" href="~/HowAreWeDifferent">HOW ARE WE DIFFERENT?</a></li>
          <li><a id="A19" runat="server" href="~/OEMPartnerships">OEM PARTNERSHIPS</a></li>
          <li><a id="A21" runat="server" href="~/CopyrightPolicy">COPYRIGHT POLICY</a></li>
          </ul>
          </li>
          <li class="last"><a runat="server"  id="spnContactUs" href="http://info.iqmediacorp.com/ContactUs">CONTACT US</a></li>
        </ul>
      </div>
      </nav>
<%--<nav>
<div>
        <ul class="sf-menu">
          <li class="current first"><a id="spnHome" class="selected" runat="server" href="~/">HOME</a></li>          
          <li><a runat="server" id="spnProducts" href="~/Products">PRODUCTS</a>
          <ul>
            	<li><a runat="server" href="~/OptimizedMediaCloud">OPTIMIZED MEDIA CLOUD</a></li>
                <li><a runat="server" href="~/InlineMediaWorkspace">INLINE MEDIA WORKSPACE</a>                	
                </li>
                <li><a runat="server" href="~/myiQStatic">MY iQ</a></li>                
            </ul>    
          
          </li>
          <li class="industry"><a id="spnIndustries" runat="server" href="~/Industries">INDUSTRIES</a>
          <ul>
                <li><a id="A4" runat="server" href="~/PoliticalParties">POLITICAL PARTIES</a></li>
                <li><a id="A5" runat="server" href="~/University">UNIVERSITIES</a></li>
                <li><a id="A6" runat="server" href="~/ProfessionalSports">PROFESSIONAL SPORTS TEAMS</a></li>
                <li><a id="A7" runat="server" href="~/CollegiateAthleticPrograms">COLLEGIATE ATHLETIC DEPARTMENTS</a></li>
          </ul>
          </li>
          <li class="resource"><a id="spnResources" runat="server" href="~/Resources">RESOURCES</a>
          <ul style="width:100%;">
          <li><a id="A8" runat="server" target="_blank" href="~/pdf/WP01-Overcoming-TMS-Limits.pdf"><span runat="server" id="Span1">WHITE PAPERS</span></a></li>
          <li><a id="A9" runat="server" target="_blank" href="~/pdf/PL1-cliQ-2-pages.pdf"><span runat="server" id="Span2">QUICK FACTS</span></a></li>
          <li><a id="A10" runat="server" target="_blank" href="~/pdf/PL1-cliQ-4-pages.pdf"><span runat="server" id="Span3">LEARN MORE ABOUT cliQ</span></a></li>
          <li><a id="A11" runat="server" target="_blank" href="~/pdf/PL2-Media-Cloud.pdf"><span runat="server" id="Span4">WHAT IS OPTIMIZED MEDIA CLOUD?</span></a></li>
          <li style="width:100%;"><a id="A12" runat="server" target="_blank" href="~/pdf/PL2-Inline-Workspace.pdf"><span runat="server" id="Span5">LEARN ABOUT THE INLINE MEDIA WORKSPACE</span></a></li>
          <li><a id="A13" runat="server" target="_blank" href="~/pdf/PL2-My-iQ.pdf"><span runat="server" id="Span6">LEARN ABOUT MY iQ</span></a></li>
          </ul>
          </li>
          <li><a id="spnAboutUs" runat="server" href="~/About/">ABOUT US</a></li>
          <li class="last"><a runat="server"  id="spnContactUs" href="http://info.iqmediacorp.com/ContactUs">CONTACT US</a></li>
        </ul>
      </div>
      </nav>--%>
<%--<nav>
<div>
<asp:Menu ID="Menu1" runat="server" Orientation="Horizontal">
    <Items>
        <asp:MenuItem NavigateUrl="~/default.aspx" Selected="True" Text="Home" 
            Value="Home"></asp:MenuItem>
        <asp:MenuItem NavigateUrl="~/Products" Text="Products" Value="Products">
            <asp:MenuItem NavigateUrl="~/OptimizedMediaCloud" Text="Optimized Media Cloud" 
                Value="Optimized Media Cloud"></asp:MenuItem>
            <asp:MenuItem NavigateUrl="~/InlineMediaWorkspace" 
                Text="INLINE MEDIA WORKSPACEAAAAAAAAAAAAAAAAAAA" 
                Value="INLINE MEDIA WORKSPACEAAAAAAAAAAAAAAAAAAA"></asp:MenuItem>
        </asp:MenuItem>
        <asp:MenuItem NavigateUrl="~/Industries" Text="Industries" Value="Industries">
        </asp:MenuItem>
        <asp:MenuItem NavigateUrl="~/Resources" Text="Resources" Value="Resources">
        </asp:MenuItem>
        <asp:MenuItem NavigateUrl="~/Resources" Text="About US" Value="About US">
        </asp:MenuItem>
        <asp:MenuItem NavigateUrl="~/info.iqmediacorp.com/ContactUs" Text="CONTACT US" 
            Value="CONTACT US"></asp:MenuItem>
    </Items>
</asp:Menu>
</div></nav>--%>
<%--<script type="text/javascript">

    cssdropdown.startchrome("chromemenu")

</script>--%>
