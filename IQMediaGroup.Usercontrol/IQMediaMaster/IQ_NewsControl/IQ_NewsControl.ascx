<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IQ_NewsControl.ascx.cs"
    Inherits="IQMediaGroup.Usercontrol.IQMediaMaster.IQ_NewsControl.IQ_NewsControl" %>
<div class="blue-title">
    News
</div>
<div class="clear">
    <asp:DataList ID="dlnews" runat="server" CssClass="width100p">
        <ItemTemplate>
            <div class="clear dlNewsInnerDiv width100p">
                <div class="float-left">
                    <a href='<%# Eval("Url") %>' target="_blank">
                        <h1>
                            <%# Eval("Headline")%></h1>
                    </a>
                </div>
                <div class="clear paddingbottom1p font12 color999999">
                    <%# DataBinder.Eval(Container.DataItem, "ReleaseDate", "{0:MMMM dd, yyyy}")%></div>
                <div class="newsSubHead clear">                   
                        <%# Eval("SubHead")%>
                </div>
                <div class="clear">
                    <%# Eval("Detail")%></div>
                <div class="float-right clear" >
                    <a href='<%# Eval("Url") %>' target="_blank" class="color0088CC">Read More....</a></div>
            </div>
        </ItemTemplate>
    </asp:DataList>
</div>
