<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Category.aspx.cs" Inherits="imaGenie.Category" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Repeater ID="imagesRepeater" runat="server">
        <ItemTemplate>
            <genie:Image runat="server" ImageId='<%# Eval("id") %>' Thumbnail="true" />
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
