<%@ Page Title="" Language="C#" MasterPageFile="~/Default.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="imaGenie.Profile" %>
<%@ Register src="~/GenieImage.ascx" tagName="Image" tagPrefix="genie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="display:block;width:100%;min-height:300px;">
        <asp:PlaceHolder runat="server" ID="newAlbum" Visible="false">
            <div class="category" style="min-height: 30px; display: block;">
                <div class="highlight" style="display:inline-block;">
                    <asp:TextBox ID="newAlbumName" runat="server"></asp:TextBox><asp:Button ID="albumAdd" OnClick="albumAdd_Click" runat="server" Text="Add"/>
                    <asp:Calendar ID="albumDate" runat="server"></asp:Calendar>
                    <asp:RequiredFieldValidator ID="rfv1" runat="server" ControlToValidate="newAlbumName" ErrorMessage="Name is reqiured."></asp:RequiredFieldValidator>
                </div>
            </div>
        </asp:PlaceHolder>
        <asp:Repeater ID="albumRepeater" runat="server" OnItemDataBound="albumRepeater_ItemDataBound">
            <ItemTemplate>
                <asp:HiddenField runat="server" Value='<%# Eval("id") %>' ID="albumIdHolder" />
                <div class="category">
                    <div class="highlight">
                        <asp:PlaceHolder runat="server" ID="albumNameLink" Visible="true">
                            <a href='<%# "Album.aspx?id=" + Eval("id") %>'><asp:Label CssClass="categorytitle" runat="server" ID="albumNameText" Text='<%# Eval("name") %>'/></a>
                            <asp:PlaceHolder runat="server" ID="editNamePlaceHolder" Visible="false">
                                <asp:Button runat="server"  ID="editAlbumBtn"  ValidationGroup="2" Text="Edit" OnClick="editAlbumBtn_Click"/>
                            </asp:PlaceHolder>
                        </asp:PlaceHolder>
                        <asp:PlaceHolder runat="server" ID="tbPlaceHolder" Visible="false">
                            <asp:TextBox runat="server" ID="albumName" ValidationGroup='<%# Eval("id") %>' Text='<%# Eval("name") %>'></asp:TextBox><asp:Button runat="server"  ID="albumSave" ValidationGroup="2" Text="Save" OnClick="albumSave_Click"/>
                        </asp:PlaceHolder>
                    </div>
                    <asp:Repeater ID="imageRepeater" runat="server">
                        <ItemTemplate>
                            <genie:Image ImageId='<%# Eval("imageid") %>' Thumbnail="true" runat="server" />
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>
    <div style="display:block; width:100%;">
        <asp:PlaceHolder runat="server" ID="privateImages" Visible="false">
            <span>Your images: </span><br />
            <asp:Repeater ID="imageRepeater2" runat="server">
                <ItemTemplate>
                    <genie:Image ImageId='<%# Eval("imageid") %>' Thumbnail="true" runat="server" />
                </ItemTemplate>
            </asp:Repeater>
        </asp:PlaceHolder>
    </div>
</asp:Content>
