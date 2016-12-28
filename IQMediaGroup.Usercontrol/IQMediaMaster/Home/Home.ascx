<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Home.ascx.cs" Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.Home.Home" %>
<%@ Register Src="../TopPanel/TopPanel.ascx" TagName="TopPanel" TagPrefix="uc1" %>
<%@ Register Src="../FooterPanel/FooterPanel.ascx" TagName="Footer" TagPrefix="uc2" %>
<%@ Register Src="~/UserControl/IQMediaMaster/SocialNetworkingWebsitesPanel/SocialNetworkingWebsitesPanel.ascx"
    TagName="SocialNetworkingWebsitesPanel" TagPrefix="uc3" %>
<div id="wrapper">
    <uc1:TopPanel ID="ucTopPanel" runat="server"></uc1:TopPanel>
    <section>
    <div class="banner">
    
     <div id="banner"><img  id="imgBanner" runat="server" src=''>
     <a href="/products/" class="btn-more" >Learn more</a>
     </div>
    <%-- <script type="text/javascript">
         var html = document.getElementById("banner");
         var r = Math.random();
         if (r > 0.5) {
             html.innerHTML = "<a href='pdf/PL1-cliQ-Overview.pdf' target='_blank' class='leadinbox'><img src='images/home/banner01.png'></a> ";

         } else {
             html.innerHTML = "<a href='pdf/WP01-Overcoming-TMS-Limits.pdf' target='_blank' class='leadinbox'><img src='images/home/banner02.png'></a>";
         }
</script>--%>
      
    </div>
    <div class="content-main">
    	<div class="small-text"><div class="banner-bottom-box">
        <a href='http://info.iqmediacorp.com/free-sms-text-alerts-from-website' target='_blank' class='leadinbox'><img id="imgSubBanner" runat="server" src="" alt=""></a>
</div>
    	<div id="pText"  style="min-height:75px;" runat="server"></div><uc3:SocialNetworkingWebsitesPanel ID="SocialNetworkingWebsitesPanel" runat="server" /></div>
       <div id="show">
               <a target="_blank" href='http://www.iqmediacorp.com/ClipPlayer/default.aspx?ClipID=ce641edb-44c4-4e73-aabb-32c3f0f61f4e&TE=nb%2fdH9YSimVlgeHlRzMuFAYD3wKa6p4GCFMFEirCGeQ%3d&PN=bt9sZFac%2bKA%3d' class='leadinbox'><img src='images/home/home-img03.png'></a> 
               <a href='pdf/Success Stories_Miami Hurricanes.pdf' target='_blank' class="leadinbox"><img src="images/home/IQ_SuccessButton_Miami.jpg" alt=""></a>
               <a href='pdf/WP01-Overcoming-TV Media-Monitoring-Limits.pdf' target='_blank' class='leadinbox'><img src='images/home/home-img01.png'></a>
       </div>
      <%-- <script type="text/javascript">
           var html = document.getElementById("show");
                      var r = Math.random();
                      if (r > 0.5) {
           html.innerHTML = "<a href='pdf/PL1-cliQ-Overview.pdf' target='_blank' class='leadinbox'><img src='images/home/home-img02.png'></a> <a href='javascript:;' class='leadinbox'><img src='images/home/home-img03.png'></a> <a href='pdf/WP01-Overcoming-TMS-Limits.pdf' target='_blank' class='leadinbox'><img src='images/home/home-img01.png'></a> ";

                      } else {
                          html.innerHTML = "<a href='pdf/WP01-Overcoming-TMS-Limits.pdf' target='_blank' class='leadinbox'><img src='images/home/home-img04.png'></a> <a href='pdf/PL2-Inline-Workspace.pdf' target='_blank' class='leadinbox'><img src='images/home/home-img05.png'></a> <a href='pdf/PL2-My-iQ.pdf' target='_blank' class='leadinbox'><img src='images/home/home-img06.png'></a>";
                      }
</script>--%>
     
       
    </div>
   
  </section>
    <uc2:Footer ID="ucFooter" runat="server"></uc2:Footer>
</div>
