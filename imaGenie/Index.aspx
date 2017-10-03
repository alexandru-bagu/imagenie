<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="imaGenie.Index" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Repeater ID="imagesRepeater" runat="server" DataSourceID="SqlDataSource1">
        <ItemTemplate>
            <genie:Image runat="server" ImageId='<%# Eval("id") %>' Thumbnail="true" />
        </ItemTemplate>
    </asp:Repeater>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:DefaultConnection %>" SelectCommand="SELECT top 20 id FROM [IMAGE] where IsPublic=1 order by date desc"></asp:SqlDataSource>
</asp:Content>
