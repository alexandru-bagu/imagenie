<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Album.aspx.cs" Inherits="imaGenie.Album" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="category" style="width:99%">
        <div class="highlight" style="width:100%">
            <asp:Label runat="server" ID="albumName"/>
            <div style="float:right">
                Date: <asp:Label runat="server" ID="albumDate" />
            </div>
        </div>
        <asp:Repeater ID="imagesRepeater" runat="server">
            <ItemTemplate>
                <genie:Image runat="server" ImageId='<%# Eval("id") %>' Thumbnail="true" />
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div style="display:block; width:100%;">
        <asp:PlaceHolder runat="server" ID="privateImages" Visible="false">
            <span>Select images for album: </span><br />
            <asp:Repeater ID="imageRepeater2" runat="server" OnItemDataBound="imageRepeater2_ItemDataBound">
                <ItemTemplate>
                    <div style="display:inline-block">
                        <div style="display:block"><genie:Image ImageId='<%# Eval("imageid") %>' Thumbnail="true" runat="server" /></div>
                        <div style="display:block"><asp:CheckBox ID="imageIsSelected" runat="server" AutoPostBack="true"  ValidationGroup='<%# Eval("imageid") %>' Checked="false" Text="Included in album" OnCheckedChanged="imageIsSelected_CheckedChanged" /></div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>
    </div>
</asp:Content>
