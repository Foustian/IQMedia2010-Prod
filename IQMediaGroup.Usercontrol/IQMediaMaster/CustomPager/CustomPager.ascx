<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CustomPager.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.CustomPager.CustomPager" %>
<%--     <div class="pagination">
    <ul>
    <li><a href="#">Prev</a></li>
    <li><a href="#">1</a></li>
    <li><a href="#">2</a></li>
    <li><a href="#">3</a></li>
    <li><a href="#">4</a></li>
    <li><a href="#">5</a></li>
    <li><a href="#">6</a></li>
    <li><a href="#">7</a></li>
    <li><a href="#">8</a></li>
    <li><a href="#">9</a></li>
    <li><a href="#">10</a></li>
    <li><a href="#">11</a></li>
    <li><a href="#">12</a></li>
    <li><a href="#">13</a></li>
    <li><a href="#">Next</a></li>
    </ul>
    </div>--%>
<div class="pagination">
    <ul>
        <li>
            <%-- <li>
            <asp:ImageButton ID="lbFirst" runat="server" Text="First" CommandArgument="0" CommandName="First"
                ImageUrl="~/images/paging/first_arrow.png" OnCommand="ListPager_Command" /></li>
        <li>
            <asp:ImageButton ID="lbPrevious" runat="server" CommandArgument="0" CommandName="Previous"
                ImageUrl="~/images/paging/previous_arrow.png" OnCommand="lbPrevious_Click" /></li>--%>
            <%-- <asp:LinkButton ID="lbFirst" runat="server" Text="First" CommandArgument="0" CommandName="First"
                OnCommand="ListPager_Command"></asp:LinkButton>
            <asp:LinkButton ID="lbPrevious" runat="server" Text="..." CommandName="Previous"
                OnClick="lbPrevious_Click"></asp:LinkButton>--%>
            <asp:DataList ID="ListPager" runat="server" OnItemDataBound="ListPager_OnItemDataBound"
                RepeatLayout="Flow" CssClass="paginationspan" RepeatDirection="Horizontal">
                <HeaderTemplate>
                    <asp:LinkButton ID="lbtnFirst" runat="server" Text="|<" OnCommand="ListPager_Command"
                        CommandName="Previous" CommandArgument="0">   
                        <image id="imgFirst" src="../images/paging/First.png"></image>
                        
                    </asp:LinkButton>
                    <asp:LinkButton ID="lbtnPrevious" runat="server" Text="<" OnCommand="ListPager_Command">    
                    <image id="imgFirst" src="../images/paging/Previous.png"></image>                   
                    </asp:LinkButton>
                </HeaderTemplate>
                <ItemTemplate>
                    <%--<asp:LinkButton ID="lbPageNumber" runat="server" CommandArgument='<%# Convert.ToInt16(Eval("PageNumber")) - 1 %>'
                        CommandName="PageNumberChange" Text='<%# Eval("PageNumber")%>' OnCommand="ListPager_Command"></asp:LinkButton>--%>
                    <asp:LinkButton ID="lbPageNumber" runat="server" CommandArgument='<%# Convert.ToInt32(Container.DataItem) - 1 %>'
                        CommandName="PageNumberChange" Text='<%# Container.DataItem %>' OnCommand="ListPager_Command"></asp:LinkButton>
                </ItemTemplate>
                <FooterTemplate>
                    <asp:LinkButton ID="lbtnNext" runat="server" Text=">" CommandName="Next" OnCommand="ListPager_Command">                       
                    <image id="imgFirst" src="../images/paging/Next.png"></image>
                    </asp:LinkButton>
                    <asp:LinkButton ID="lbtnLast" runat="server" Text=">|" CommandName="Last" OnCommand="ListPager_Command">                       
                    <image id="imgFirst" src="../images/paging/Last.png"></image>
                    </asp:LinkButton>
                </FooterTemplate>
                <HeaderStyle Wrap="true" />
                <FooterStyle Wrap="true" />
            </asp:DataList>
        </li>
        <%--<li>
            <asp:ImageButton ID="lbNext" runat="server" Text="First" CommandArgument="0" CommandName="Next"
                ImageUrl="~/images/paging/next_arrow.png" OnCommand="lbNext_Click" /></li>
        <li>
            <asp:ImageButton ID="lbLast" runat="server" CommandArgument="0" CommandName="Last"
                ImageUrl="~/images/paging/last_arrow.png" OnCommand="ListPager_Command" /></li>--%>
    </ul>
</div>
