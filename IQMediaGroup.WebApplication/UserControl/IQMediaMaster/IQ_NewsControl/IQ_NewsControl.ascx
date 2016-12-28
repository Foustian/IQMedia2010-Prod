<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQ_NewsControl.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQ_NewsControl.IQ_NewsControl" %>
<div class="blue-title">
    News
</div>
<div class="clear">
    <asp:DataList ID="dlnews" runat="server" CssClass="margin10 width100p" CellSpacing="15">
        <ItemTemplate>
            <div class="clear dlNewsInnerDiv width100p">
                <div class="float-left">
                    <a href='<%# Eval("Url") %>' target="_blank" class="color0088CC">
                        <h1>
                            <%# Eval("Headline")%></h1>
                    </a>
                </div>
                <div class="float-left clear paddingbottom1p font12 color999999">
                    <%# Eval("ReleaseDate")%></div>
                <div class="color8CC5DC clear float-left">
                    <h3>
                        <%# Eval("SubHead")%></h3>
                </div>
                <div class="font13 clear float-left">
                    <%# Eval("Detail")%></div>
                <div class="float-right clear" >
                    <a href='<%# Eval("Url") %>' target="_blank" class="color0088CC">Read More....</a></div>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>
