<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Categories.aspx.cs" Inherits="imaGenie.Categories" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display: inline-block; vertical-align: top;">
        <asp:LoginView ID="newCategoryView" runat="server">
             <RoleGroups>
                <asp:RoleGroup Roles="Administrator">
                    <ContentTemplate>
                        <div class="category" style="min-height: 30px;">
                            <div class="highlight" style="display:inline-block;">
                                <asp:TextBox ID="newCategoryName" runat="server"></asp:TextBox><asp:Button ID="categoryInsert" OnClick="categoryInsert_Click" runat="server" Text="Add"/>
                            </div>
                        </div>
                        <br /> <br /> <br />
                    </ContentTemplate>
                </asp:RoleGroup>
            </RoleGroups>
        </asp:LoginView>
        <asp:Repeater ID="categoryRepeater" runat="server" DataSourceID="SqlDataSource1" OnItemDataBound="categoryRepeater_ItemDataBound">
            <ItemTemplate>
                <div class="category">
                    <div class="highlight">
                        <asp:LoginView runat="server">
                            <RoleGroups>
                                <asp:RoleGroup Roles="Administrator,Moderator">
                                    <ContentTemplate>
                                        <asp:TextBox ID="categoryName" runat="server" OnTextChanged="categoryName_TextChanged" Text='<%# Eval("name") %>' ValidationGroup='<%# Eval("categoryid") %>' AutoPostBack="true"></asp:TextBox><asp:Button ID="categoryDelete" ValidationGroup='<%# Eval("categoryid") %>' OnClick="categoryDelete_Click" runat="server" Text="Remove"/>
                                    </ContentTemplate>
                                </asp:RoleGroup>
                                <asp:RoleGroup Roles="Normal">
                                    <ContentTemplate>
                                        <a href='<%# "Category.aspx?id=" + Eval("categoryid") %>'><h2 class="categorytitle"><%# Eval("name") %></h2></a>
                                    </ContentTemplate>
                                </asp:RoleGroup>
                            </RoleGroups>
                            <AnonymousTemplate>
                                <ContentTemplate>
                                    <a href='<%# "Category.aspx?id=" + Eval("categoryid") %>'><h2 class="categorytitle"><%# Eval("name") %></h2></a>
                                </ContentTemplate>
                            </AnonymousTemplate>
                        </asp:LoginView>
                    </div>
                    <asp:Repeater ID="imageRepeater" runat="server">
                        <ItemTemplate>
                            <genie:Image ImageId='<%# Eval("imageid") %>' Thumbnail="true" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="select id as categoryid, name from category"></asp:SqlDataSource>
    </div>
</asp:Content>
