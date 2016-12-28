<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="IQMediaGroup.WebApplication.FusionChart.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>IQ Media :: Fusion Chart</title>
    <script type="text/javascript" src="js/FusionCharts.js"></script>
    <script src="js/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        var test = "";
        alert('test :: ' + test);
    
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="sm" runat="server">
    </asp:ScriptManager>
    <div id="div2">
    </div>
    <input id="test" type="button" onclick="IterateXML();" value="test" />
    <%--<asp:Button ID="test" runat="server" OnClientClick="test()" />--%>
    <script type="text/javascript" language="javascript">

        function LoadMSLinechart(data1) {
            var myZoomChart = new FusionCharts("/fusionchart/MSLine.swf",
                            "MSLine", "800", "200");
            myZoomChart.setJSONData('{ "chart": {   "caption": "Business Results 2005 v 2006",   "xaxisname": "Month",   "yaxisname": "Revenue",   "showvalues": "0",   "numberprefix": "$" }, "categories": [   {     "category": [       {         "label": "Jan"       },       {         "label": "Feb"       },       {         "label": "Mar"       },       {         "label": "Apr"       },       {         "label": "May"       },       {         "label": "Jun"       },       {         "label": "Jul"       },       {         "label": "Aug"       },       {         "label": "Sep"       },       {         "label": "Oct"       },       {         "label": "Nov"       },       {         "label": "Dec"       }     ]   } ], "dataset": [   {     "seriesname": "2006",     "data": [       {         "value": "27400"       },       {         "value": "29800"       },       {         "value": "25800"       },       {         "value": "26800"       },       {         "value": "29600"       },       {         "value": "32600"       },       {         "value": "31800"       },       {         "value": "36700"       },       {         "value": "29700"       },       {         "value": "31900"       },       {         "value": "34800"       },       {         "value": "24800"       }     ]   }	]}');            
            myZoomChart.render("divMsLineChart");
            //            alert('<%= rptAffil.ClientID %>');
            //            for (var i = 0; i <= document.getElementById('rptAffil').elements.length; i++) {
            //                alert(i);
            //            }
            //            $('#<%=rptAffil.ClientID%>').children('input').each(function () {
            //                alert(this);
            //            });

        }
        LoadMSLinechart('tst');

        function IterateXML() {
            var xmlData = '<?xml version="1.0" encoding="utf-8"?><chart caption="2012-06-10" xAxisName="" yAxisName="Hits" showValues="0" showBorder="1" labelDisplay="Rotate" showLegend="0">  <categories>    <category name="5:30AM" />    <category name="6:30AM" />    <category name="7:30AM" />    <category name="8:30AM" />    <category name="9:30AM" />    <category name="10:30AM" />    <category name="11:30AM" />    <category name="12:30AM" />    <category name="1:30PM" />    <category name="2:30PM" />    <category name="3:30PM" />    <category name="4:30PM" />    <category name="5:30PM" />    <category name="6:30PM" />    <category name="7:30PM" />    <category name="8:30PM" />    <category name="9:30PM" />    <category name="10:30PM" />    <category name="11:30PM" />    <category name="0:30AM" />    <category name="1:30AM" />    <category name="2:30AM" />    <category name="3:30AM" />    <category name="4:30AM" />  </categories>  <dataset seriesName="Entertainment">    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />  </dataset>  <dataset seriesName="ABC">    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />  </dataset>    <dataset seriesName="CW">    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />    <set value="0" />  </dataset></chart>';
            

            if (window.DOMParser) {

                parser = new DOMParser();
                xmlDoc = parser.parseFromString(xmlData); //'<Root><Media>dsd</Media><IsValidMedia>true</IsValidMedia><IsOldVersion>false</IsOldVersion><HasException>false</HasException></Root>', "text/xml");
            }
            else // Internet Explorer
            {
                xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
                xmlDoc.async = false;
                xmlDoc.loadXML(txt);
            }

            var root = xmlDoc.getElementsByTagName('chart');

            for (var i = 0; i < root[0].childNodes.length; i++) {

                alert(root[0].childNodes[i].attribute);
                if (root[0].childNodes[i].nodeValue.toString() == 'true') {

                    var Media = xmlDoc.getElementsByTagName('Media');
                    //alert(Media[0].nodeValue);

                    for (var i = 0; i < Media[0].childNodes.length; i++) {
                        alert(Media[0].childNodes[i].nodeValue);
                    }
                }
            }
        }

        function Loadchart(data1) {

            test = data1;
            var myZoomChart = new FusionCharts("/fusionchart/ZoomLine.swf",
                            "ZoomChart", "800", "200");
            myZoomChart.setXMLData(data1);
            myZoomChart.render("div2");
            //            alert('<%= rptAffil.ClientID %>');
            //            for (var i = 0; i <= document.getElementById('rptAffil').elements.length; i++) {
            //                alert(i);
            //            }
            //            $('#<%=rptAffil.ClientID%>').children('input').each(function () {
            //                alert(this);
            //            });

        }
        function FC_Zoomed(DOMId, startIndex, endIndex, startItemLabel, endItemLabel) {
            var dtlstValue = document.getElementById('<%= hfAffilValue.ClientID %>');
            $.ajax({
                type: 'GET',
                //dataType: 'application/json; charset=utf-8',
                dataType: 'text',
                url: "ZoomChartHandler.ashx?endDate=" + endItemLabel.toString() + "&affil=" + dtlstValue.innerText,
                success: OnComplete,
                error: OnFail
            });

            //alert(DOMId + " chart has been zoomed " + startItemLabel + " to " + endItemLabel);
        }

        function OnComplete(data, textStatus, jqXHR) {
            LoadMSLinechart(data);
        }

        function OnFail(jqXHR, textStatus, errorThrown) {
            alert('Request Failed' + errorThrown + textStatus + jqXHR.responseText);
            //lbl.innerHTML = "Error";
        }

    </script>
    <table width="100%">
        <tr>
            <td width="15%">
            </td>
            <td width="70%">
                <br />
                <img src="../Images/mycliqmedia.png" runat="server" id="imgcliq" /><br />
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>Search Term:</strong>
                            <asp:TextBox ID="txtSearchTerm" runat="server" Width="560px" Height="22px" Font-Size="15px"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvSearchTerm" runat="server" ControlToValidate="txtSearchTerm"
                                ValidationGroup="Search" Display="Dynamic" Text="*" ErrorMessage="Enter Search Term"></asp:RequiredFieldValidator>
                            <AjaxToolkit:ValidatorCalloutExtender ID="revDigit1Ext" runat="server" TargetControlID="rfvSearchTerm">
                            </AjaxToolkit:ValidatorCalloutExtender>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <strong>From Date:</strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtFromDate" runat="server" Height="22px" Font-Size="15px"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtenderFromDate" Format="yyyy-MM-dd" runat="server"
                                CssClass="MyCalendar" TargetControlID="txtFromDate">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvtxtFromDate" runat="server" ControlToValidate="txtFromDate"
                                ValidationGroup="Search" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <strong>To Date:</strong>&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:TextBox ID="txtToDate" runat="server" Height="22px" Font-Size="15px"></asp:TextBox>
                            <AjaxToolkit:CalendarExtender ID="CalendarExtenderToDate" Format="yyyy-MM-dd" runat="server"
                                CssClass="MyCalendar" TargetControlID="txtToDate">
                            </AjaxToolkit:CalendarExtender>
                            <asp:RequiredFieldValidator ID="rfvtxtToDate" runat="server" ControlToValidate="txtToDate"
                                ValidationGroup="Search" Display="Dynamic" Text="*"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cmpDateValidator" runat="server" ControlToCompare="txtToDate"
                                ControlToValidate="txtFromDate" Text="*" ErrorMessage="To Date Must be greater than From Date"
                                Operator="LessThanEqual" Type="Date" Display="Dynamic" ValidationGroup="Search">
                            </asp:CompareValidator>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <strong>Affiliate Network:</strong><br />
                            <br />
                            <asp:Panel ID="pnlAffil" runat="server" ScrollBars="Auto" Height="150px" Visible="true">
                                <asp:DataList ID="rptAffil" runat="server" RepeatColumns="3" Width="100%" CellPadding="0"
                                    CellSpacing="0">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" width="100%" class="content-textRd">
                                            <tr height="29px;">
                                                <td width="15" height="100%" valign="top">
                                                    <%# Container.ItemIndex+1 %>
                                                </td>
                                                <td width="107" height="100%" valign="top">
                                                    <asp:Label ID="lblAffiliateName" runat="server" Text='<%# Eval("Station_Affil") %>'></asp:Label>
                                                </td>
                                                <td height="100%" valign="top">
                                                    <input type="checkbox" runat="server" value='<%# Eval("Station_Affil") %>' id='chkAffil' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:DataList>
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn-blue2" OnClick="btnSearch_Click"
                                ValidationGroup="Search" />
                            <asp:HiddenField ID="hfAffilValue" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <br />
                            <asp:Literal ID="divChart" runat="server"></asp:Literal><br />
                            <asp:Literal ID="divMsLineChart" runat="server"></asp:Literal>
                        </td>
                    </tr>
                </table>
            </td>
            <td width="15%">
            </td>
        </tr>
    </table>
    </form>
</body>
<script type="text/javascript">
    FusionCharts("ZoomChart").addEventListener("LegendItemClicked", myChartListener);
    //FusionCharts("ZoomChart").addEventListener("ondrawcomplete", myChartListener);
    function myChartListener(eventObject, argumentsObject) {

        alert(argumentsObject.datasetName);
    }

</script>
</html>
