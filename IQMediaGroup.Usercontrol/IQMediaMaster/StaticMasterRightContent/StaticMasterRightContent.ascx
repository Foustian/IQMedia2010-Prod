<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StaticMasterRightContent.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.StaticMasterRightContent.StaticMasterRightContent" %>
<script type="text/javascript">
    var TakeTourContent = '<a target="_blank" href="http://www.iqmediacorp.com/ClipPlayer/default.aspx?ClipID=ce641edb-44c4-4e73-aabb-32c3f0f61f4e&TE=nb%2fdH9YSimVlgeHlRzMuFAYD3wKa6p4GCFMFEirCGeQ%3d&PN=bt9sZFac%2bKA%3d" ><img src="../images/about/about-img03.png"></a>';
    var GetWhiltePaper = '<a href="../pdf/WP01-Overcoming-TV Media-Monitoring-Limits.pdf" target="_blank"><img src="../images/about/about-img01.png"></a>';
    var LearnMore = '<a href="../pdf/PL1-cliQ-Overview.pdf" target="_blank"><img src="../images/about/about-img02.png"></a>';
    var SeeProPrep = '<a target="_blank" href="http://www.iqmediacorp.com/ClipPlayer/default.aspx?ClipID=6a11df8c-89ca-4906-b610-aaa98f5824c5&TE=iZGAy3ctoirsk62csKd07OZPUyLM19sj&PN=bt9sZFac%2bKA%3d"><img src="../images/about/ProPrep.png"></a>';
    var OptimizedCloud = '<a href="../pdf/PL2-Media-Optimized-for-the-Cloud.pdf" target="_blank"><img src="../images/about/about-img04.png"></a>';
    var QuickFacts = '<a href="../pdf/PL1-cliQ-Fast-Facts.pdf" target="_blank"><img src="../images/about/about-img07.png"></a>';
    var LearnInlineWorkShop = '<a href="../pdf/PL2-Inline-Media-Workspace.pdf" target="_blank"><img src="../images/about/about-img05.png"></a>';
    var LearnMyiQ = '<a href="../pdf/PL2-My-iQ-Enterprise-Media-Center.pdf" target="_blank"><img src="../images/about/about-img06.png"></a>';
    var SuccessButtonTemple = '<a href="../pdf/Success Stories_Temple_01.pdf" target="_blank"><img src="../images/home/IQ_SuccessButton_Temple202.jpg"></a>';
    var MiamiSuccessStories = '<a href="../pdf/Success Stories_Miami Hurricanes.pdf" target="_blank"><img src="../images/home/IQ_SuccessButton_Miami202.jpg"></a>';

    function GetContent(contentid) {
        if (contentid == "TakeTourContent") {
            $('#' + contentid).html(TakeTourContent);
        }
        else if (contentid == "GetWhiltePaper") {
            $('#' + contentid).html(GetWhiltePaper);
        }
        else if (contentid == "LearnMore") {
            $('#' + contentid).html(LearnMore);
        }
        else if (contentid == "SeeProPrep") {
            $('#' + contentid).html(SeeProPrep);
        }
        else if (contentid == "OptimizedCloud") {
            $('#' + contentid).html(OptimizedCloud);
        }
        else if (contentid == "QuickFacts") {
            $('#' + contentid).html(QuickFacts);
        }
        else if (contentid == "LearnInlineWorkShop") {
            $('#' + contentid).html(LearnInlineWorkShop);
        }
        else if (contentid == "LearnMyiQ") {
            $('#' + contentid).html(LearnMyiQ);
        }
        else if (contentid == "SuccessButtonTemple") {
            $('#' + contentid).html(SuccessButtonTemple);
        }
        else if (contentid == "MiamiSuccessStories") {
            $('#' + contentid).html(MiamiSuccessStories);
        }
    }
</script>
<div id="divProducts" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <div class="about-right-bg" id="LearnMore">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("GetWhiltePaper");
        GetContent("LearnMore");
        //GetContent("SeeProPrep");
    </script>
</div>
<div id="divOptimizedCloud" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="OptimizedCloud">
    </div>
    <div class="about-right-bg" id="QuickFacts">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("OptimizedCloud");
        GetContent("QuickFacts");
        //GetContent("SeeProPrep");
    </script>
</div>
<div id="divInlineWorkShop" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="LearnInlineWorkShop">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("LearnInlineWorkShop");
        GetContent("GetWhiltePaper");
        //GetContent("SeeProPrep");
    </script>
</div>
<div id="divMyiQ" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="LearnMyiQ">
    </div>
    <div class="about-right-bg" id="QuickFacts">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("LearnMyiQ");
        GetContent("QuickFacts");
        //GetContent("SeeProPrep");
    </script>
</div>
<div id="divIndustries" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="SuccessButtonTemple">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <%--<div class="about-right-bg" id="LearnMore" ></div>
    <div class="about-right-bg" id="SeeProPrep" ></div>--%>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("SuccessButtonTemple");
        GetContent("GetWhiltePaper");
        //         GetContent("LearnMore");
        //         GetContent("SeeProPrep");
    </script>
</div>
<div id="divIndustriesPoliticalParties" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <div class="about-right-bg" id="LearnMore">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("GetWhiltePaper");
        GetContent("LearnMore");
        //GetContent("SeeProPrep");        
    </script>
</div>
<div id="divIndustriesUniverties" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <div class="about-right-bg" id="LearnMore">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("GetWhiltePaper");
        GetContent("LearnMore");
        //GetContent("SeeProPrep");        
    </script>
</div>
<div id="divIndustriesProfessional" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <div class="about-right-bg" id="LearnMore">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("GetWhiltePaper");
        GetContent("LearnMore");
        //GetContent("SeeProPrep");        
    </script>
</div>
<div id="divCollegiateAthleticPrograms" runat="server" visible="false">
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="MiamiSuccessStories">
    </div>
    <div class="about-right-bg" id="SuccessButtonTemple">
    </div>
    <div class="about-right-bg" id="GetWhiltePaper">
    </div>
    <script type="text/javascript">
        GetContent("TakeTourContent");
        GetContent("MiamiSuccessStories");
        GetContent("SuccessButtonTemple");
        GetContent("GetWhiltePaper");        
    </script>
</div>
<div id="divAboutUs" runat="server" visible="false">
    <div class="about-right-bg" id="QuickFacts">
    </div>
    <div class="about-right-bg" id="TakeTourContent">
    </div>
    <div class="about-right-bg" id="LearnMore">
    </div>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("QuickFacts");
        GetContent("TakeTourContent");
        GetContent("LearnMore");
        //GetContent("SeeProPrep");
    </script>
</div>
<div id="divProPrep" runat="server" visible="false">
    <%--<div class="about-right-bg">
        <a target="_blank" href="http://www.iqmediacorp.com/ClipPlayer/default.aspx?ClipID=ce641edb-44c4-4e73-aabb-32c3f0f61f4e&TE=nb%2fdH9YSimVlgeHlRzMuFAYD3wKa6p4GCFMFEirCGeQ%3d&PN=bt9sZFac%2bKA%3d">
            <img src="../images/about/about-img03.png"></a>
    </div>--%>
    <div class="about-right-bg" id="SeeProPrep">
    </div>
    <script type="text/javascript">
        GetContent("SeeProPrep");
    </script>
    <%-- <div class="about-right-bg">
        <a href="../pdf/PL1-cliQ-2-pages.pdf" target="_blank">
            <img src="../images/about/about-img07.png"></a>
    </div>--%>
</div>
<%--<div class="about-right-bg">
    <a href="../pdf/PL1-cliQ-4-pages.pdf" target="_blank">
        <img src="../images/about/about-img02.png"></a>
</div>
<div class="about-right-bg">
    <a href="javascript:;">
        <img src="../images/about/about-img03.png"></a>
</div>
<div class="about-right-bg">
    <a href="../pdf/WP01-Overcoming-TMS-Limits.pdf" target="_blank">
        <img src="../images/about/about-img01.png"></a>
</div>
--%>
