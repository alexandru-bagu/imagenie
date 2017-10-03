<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Search.aspx.cs" Inherits="imaGenie.Search" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    Search results for "<%= Server.UrlDecode(Request.Params["q"]) %>": <br />
    <asp:Repeater ID="imagesRepeater" runat="server">
        <ItemTemplate>
            <genie:Image runat="server" ImageId='<%# Eval("id") %>' Thumbnail="true" />
        </ItemTemplate>
    </asp:Repeater>
</asp:Content>
